using EasyModbus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Probotbor.Core.Contracts.Communication;
using Probotbor.Core.Models.Communication;
using Probotbor.Core.Models.Communication.Modbus;
using Probotbor.Core.Models.Plc;
using System.Text;

namespace Probotbor.Core.Services.Plc
{
    public class ModbusCommunicationService : ICommunicationService
    {
        public bool IsInitialized { get; private set; }
        public CommSettings CommSettings { get; }
        public bool Connected { get; private set; }

        private ModbusClient _client = new ModbusClient();
        public ModbusReadMemory holdingReadMemory = new ModbusReadMemory();
        public ModbusReadMemory inputReadMemory = new ModbusReadMemory();
        public List<ModbusReadCommand> commands = new List<ModbusReadCommand>();
        private readonly ILogger<ModbusCommunicationService> _logger;

        public event Action? ScanCompletedEvent;

        public ModbusCommunicationService(IOptions<CommSettings> options, ILogger<ModbusCommunicationService> logger)
        {
            CommSettings = options.Value;
            _logger = logger;
        }

        private void Init()
        {
            InitByType(ModbusRegType.Holding, holdingReadMemory);
            InitByType(ModbusRegType.Reading, inputReadMemory);
            IsInitialized = true;
        }


        private void InitByType(ModbusRegType regType, ModbusReadMemory readMemory)
        {
            var sequences = GetParameterSequences(regType).OrderBy(s => s.Start).ToList();
            if (sequences.Count == 0) return;
            var min = sequences.First().Start;
            var max = sequences.Select(s => s.End).Max();
            readMemory.Offset = min;
            readMemory.Buffer = new ushort[max - min + 1];
            commands.Clear();
            int i = min;
            do
            {
                var belongeds = GetBelongedSequences(new ParameterSequence(i, i + 99), sequences);
                if (belongeds.Count == 0)
                {
                    i = i + 100;
                    continue;
                }
                else
                {
                    var points = belongeds.SelectMany(b => new int[] { b.Start, b.End }).ToList();
                    var minPoint = Math.Max(points.Min(), i);
                    var maxPoint = Math.Min(points.Max(), i + 99);
                    var count = maxPoint - minPoint + 1;
                    commands.Add(new ModbusReadCommand(minPoint, count, readMemory.Buffer, regType));
                    i = maxPoint + 1;
                }
            } while (i < max);

        }


        private List<ParameterSequence> GetParameterSequences(ModbusRegType regType)
        {
            var list = new List<ParameterSequence>();
            foreach (var par in PlcModel.Parameters)
            {
                if (par is Parameter<ushort> parUshort && parUshort.ModbusRegType == regType && parUshort.IsRequired)
                {
                    list.Add(new ParameterSequence(parUshort.RegNum, parUshort.RegNum));
                }

                else if (par is Parameter<short> parShort && parShort.ModbusRegType == regType && parShort.IsRequired)
                    list.Add(new ParameterSequence(parShort.RegNum, parShort.RegNum));
                else if (par is Parameter<bool> parBool && parBool.ModbusRegType == regType && parBool.IsRequired)
                    list.Add(new ParameterSequence(parBool.RegNum, parBool.RegNum));
                else if (par is Parameter<int> parInt && parInt.ModbusRegType == regType && parInt.IsRequired)
                    list.Add(new ParameterSequence(parInt.RegNum, parInt.RegNum + 1));
                else if (par is Parameter<uint> parUint && parUint.ModbusRegType == regType && parUint.IsRequired)
                    list.Add(new ParameterSequence(parUint.RegNum, parUint.RegNum + 1));
                else if (par is Parameter<float> parFloat && parFloat.ModbusRegType == regType && parFloat.IsRequired)
                    list.Add(new ParameterSequence(parFloat.RegNum, parFloat.RegNum + 1));
                else if (par is Parameter<string> parstring && parstring.ModbusRegType == regType && parstring.IsRequired)
                {
                    var regs = parstring.Length % 2 != 0 ? parstring.Length / 2 + 1 : parstring.Length / 2;
                    list.Add(new ParameterSequence(parstring.RegNum, parstring.RegNum + regs - 1));
                }

            }
            return list;
        }

        private List<ParameterSequence> GetBelongedSequences(ParameterSequence diapasone, IEnumerable<ParameterSequence> sequences)
        {
            var list = new List<ParameterSequence>();
            list = sequences.Where(s => IsBelongToSequence(diapasone, s)).ToList();
            return list;
        }

        private static bool IsBelongToSequence(ParameterSequence diapasone, ParameterSequence sequence)
        {
            return (sequence.Start >= diapasone.Start && sequence.Start <= diapasone.End) ||
                (sequence.End >= diapasone.Start && sequence.End <= diapasone.End) ||
                (sequence.Start < diapasone.Start && sequence.End > diapasone.End);
        }

        private void GetValuesForParameters()
        {
            foreach (var par in PlcModel.Parameters)
            {
                if (par is Parameter<ushort> parUshort && parUshort.IsRequired)
                {
                    var memory = parUshort.ModbusRegType == ModbusRegType.Holding ? holdingReadMemory : inputReadMemory;
                    if (memory is not null && memory.Buffer is not null)
                        parUshort.Value = memory.Buffer[parUshort.RegNum - memory.Offset];
                }
                else if (par is Parameter<short> parShort && parShort.IsRequired)
                {
                    var memory = parShort.ModbusRegType == ModbusRegType.Holding ? holdingReadMemory : inputReadMemory;
                    if (memory is not null && memory.Buffer is not null)
                    {
                        var bytes = BitConverter.GetBytes(memory.Buffer[parShort.RegNum - memory.Offset]);
                        parShort.Value = BitConverter.ToInt16(bytes);
                    }

                }
                else if (par is Parameter<bool> parBool && parBool.IsRequired)
                {
                    var memory = parBool.ModbusRegType == ModbusRegType.Holding ? holdingReadMemory : inputReadMemory;
                    if (memory is not null && memory.Buffer is not null)
                        parBool.Value = (memory.Buffer[parBool.RegNum - memory.Offset] & (ushort)Math.Pow(2, parBool.BitNum)) > 0;
                }
                else if (par is Parameter<int> parInt && parInt.IsRequired)
                {
                    var memory = parInt.ModbusRegType == ModbusRegType.Holding ? holdingReadMemory : inputReadMemory;
                    if (memory is not null && memory.Buffer is not null)
                    {
                        var bytes = BitConverter.GetBytes(memory.Buffer[parInt.RegNum - memory.Offset]);
                        parInt.Value = BitConverter.ToInt32(bytes);
                    }

                }
                else if (par is Parameter<uint> parUint && parUint.IsRequired)
                {
                    var memory = parUint.ModbusRegType == ModbusRegType.Holding ? holdingReadMemory : inputReadMemory;
                    if (memory is not null && memory.Buffer is not null)
                    {
                        var bytes = BitConverter.GetBytes(memory.Buffer[parUint.RegNum - memory.Offset]);
                        parUint.Value = BitConverter.ToUInt32(bytes);
                    }

                }
                else if (par is Parameter<float> parFloat && parFloat.IsRequired)
                {
                    var memory = parFloat.ModbusRegType == ModbusRegType.Holding ? holdingReadMemory : inputReadMemory;
                    var bytes = memory?.Buffer?.Skip(parFloat.RegNum - memory.Offset).Take(2).SelectMany(s => BitConverter.GetBytes(s)).ToArray();
                    parFloat.Value = BitConverter.ToSingle(bytes);
                }
                else if (par is Parameter<string> parstring && parstring.IsRequired)
                {
                    var memory = parstring.ModbusRegType == ModbusRegType.Holding ? holdingReadMemory : inputReadMemory;
                    var regs = parstring.Length % 2 != 0 ? parstring.Length / 2 + 1 : parstring.Length / 2;
                    var bytes = memory?.Buffer?
                        .Skip(parstring.RegNum - memory.Offset)
                        .Take(regs)
                        .SelectMany(s => BitConverter.GetBytes(s))
                        .TakeWhile(b => b > 0)
                        .ToArray();
                    if (bytes != null)
                        parstring.Value = Encoding.ASCII.GetString(bytes).Replace("\0", "");
                }
            }
        }

        public void ReadAllData()
        {
            Init();
            foreach (var command in commands)
            {
                if (command.RegType == ModbusRegType.Holding)
                {
                    var regs = ReadHoldingRegisters(command.Start, command.Count);
                    regs.CopyTo(0, holdingReadMemory.Buffer, command.Start - holdingReadMemory.Offset, regs.Count);
                }
                else
                {
                    var regs = ReadReadingRegisters(command.Start, command.Count);
                    regs.CopyTo(0, inputReadMemory.Buffer, command.Start - inputReadMemory.Offset, regs.Count);
                }

            }
            GetValuesForParameters();
            ScanCompletedEvent?.Invoke();
        }

        private void SetBit(ref ushort aByte, int pos, bool value)
        {
            if (value)
            {
                //left-shift 1, then bitwise OR
                aByte = (ushort)(aByte | (1 << pos));
            }
            else
            {
                //left-shift 1, then take complement, then bitwise AND
                aByte = (ushort)(aByte & ~(1 << pos));
            }
        }

        public void WriteParameter(object parameter)
        {
            if (parameter is Parameter<short> parShort)
            {
                var bytes = BitConverter.GetBytes(parShort.WriteValue);
                WriteRegisters(new ushort[] { BitConverter.ToUInt16(bytes) }, parShort.RegNum);
            }
            else if (parameter is Parameter<ushort> parUShort)
            {
                WriteRegisters(new ushort[] { parUShort.WriteValue }, parUShort.RegNum);
            }
            else if (parameter is Parameter<bool> parBool)
            {
                var reg = ReadHoldingRegisters(parBool.RegNum, 1).First();
                SetBit(ref reg, parBool.BitNum, parBool.WriteValue);
                WriteRegisters(new ushort[] { reg }, parBool.RegNum);
            }
            else if (parameter is Parameter<string> parString)
            {
                var bytes = Encoding.ASCII.GetBytes(parString.WriteValue).
                    Take(Math.Min(parString.WriteValue.Length, parString.Length))
                    .Append((byte)0)
                    .ToArray();
                var regs = new ushort[(bytes.Length + 1) / 2];
                for (int i = 0; i < bytes.Length; i += 2)
                {
                    if (i + 1 == bytes.Length)
                        regs[i / 2] = (ushort)bytes[i];
                    else regs[i / 2] = BitConverter.ToUInt16(bytes, i);
                }
                WriteRegisters(regs, parString.RegNum);
            }
        }

        private List<ushort> ReadHoldingRegisters(int startRegNum, int regCnt)
        {
            try
            {
                if (_client == null) throw new Exception("Modbus client is null");
                if (!_client.Connected)
                    Connect();
                return _client.ReadHoldingRegisters(startRegNum, regCnt).Select(i => (ushort)i).ToList();

            }
            catch (Exception ex)
            {
                Disconnect();
                throw new Exception($"Чтение {regCnt} holding Modbus регистров по адресу {startRegNum} - {ex.Message}");
            }
        }

        private List<ushort> ReadReadingRegisters(int startRegNum, int regCnt)
        {
            try
            {
                if (_client == null) throw new Exception("Modbus client is null");
                if (!_client.Connected)
                    Connect();
                return _client.ReadInputRegisters(startRegNum, regCnt).Select(i => (ushort)i).ToList();

            }
            catch (Exception ex)
            {
                Disconnect();
                throw new Exception($"Чтение {regCnt} input Modbus регистров по адресу {startRegNum} - {ex.Message}");
            }
        }

        private void WriteRegisters(ushort[] buf, int startRegNum)
        {
            try
            {
                if (_client == null) throw new Exception("Modbus client is null");
                if (!_client.Connected)
                    Connect();
                var regs = buf.Select(i => (int)i).ToArray();
                _client.WriteMultipleRegisters(startRegNum, regs);

            }
            catch (Exception ex)
            {
                Disconnect();
                throw new Exception($"Запись {buf.Length} Modbus регистров по адресу {startRegNum} - {ex.Message}");
            }
        }

        public void WriteCoil(int coilNumber, bool value)
        {
            try
            {
                if (_client == null) throw new Exception("Modbus client is null");
                if (!_client.Connected)
                    Connect();
                _client.WriteSingleCoil(coilNumber, value);
            }
            catch (Exception ex)
            {
                Disconnect();
                throw new Exception($"Запись Modbus coil №{coilNumber} - {ex.Message}");
            }
        }


        private void Connect()
        {
            _client = new ModbusClient();
            _client.IPAddress = CommSettings.Ip;
            _client.Port = CommSettings.Port;
            _logger.LogInformation($"Выполняется подключение к Modbus серверу по адресу {CommSettings.Ip}:{CommSettings.Port}");
            _client.Connect();
            Connected = _client.Connected;
            if (Connected)
            {
                _logger.LogInformation($"Подключение к Modbus серверу по адресу {CommSettings.Ip}:{CommSettings.Port} выполнено успешно");
            }
        }

        private void Disconnect()
        {
            if (_client != null && _client.Connected)
            {
                _logger.LogInformation($"Выполняется отключение от {_client.IPAddress}:{_client.Port}");
                _client.Disconnect();
                _logger.LogInformation($"Отключение от {_client.IPAddress}:{_client.Port}  выполнено успешно");
                Connected = _client.Connected;
            }
        }

    }
}
