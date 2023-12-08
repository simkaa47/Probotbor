using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Probotbor.Core.Contracts.Communication;
using Probotbor.Core.Models.Communication;
using Probotbor.Core.Models.Communication.Siemens;
using Probotbor.Core.Models.Plc;
using S7;

namespace Probotbor.Core.Services.Plc
{
    public class SiemensCommunicationService : ICommunicationService
    {
        public bool Connected { get; private set; }

        public event Action? ScanCompletedEvent;
        public CommSettings CommSettings { get; }
        private readonly ILogger<SiemensCommunicationService> _logger;
        private S7Client _s7Client { get; set; } = new S7Client() { RecvTimeout = 500, SendTimeout = 500, ConnTimeout = 500 };
        private S7MultiVar _reader;


        public SiemensCommunicationService(IOptions<CommSettings> options, ILogger<SiemensCommunicationService> logger)
        {
            CommSettings = options.Value;
            _logger = logger;
            _reader = new S7MultiVar(_s7Client);
            _s7Client.SendTimeout = 500;            
        }

        private void Init()
        {

        }

        public void ReadAllData()
        {
            try
            {
                if (!_s7Client.Connected)
                    Connect();
                else
                {
                    var checkedParams = PlcModel.Parameters
                        .Where(p => p.IsRequired).ToList();


                    var sequences = checkedParams
                        .GroupBy(p => p.DbNum)
                        .Select(g => new SiemensReadArea(g.ToList(), g.Key, g.Select(p => p.ByteNum).Min(), g.Select(p => p.ByteNum + GetParamSize(p) - 1).Max()));



                    foreach (var sec in sequences)
                    {
                        var result = _s7Client.DBRead(sec.DbNum, sec.Start, sec.Data.Length, sec.Data);
                        if (result == 0)
                        {
                            GetValueFromParameter(sec);
                        }
                        else
                        {
                            throw new Exception($"Не удалось прочитать  данные с ПЛК Siemens (DbNum = {sec.DbNum}, ByteNum = {sec.Start}, Length = {sec.Data.Length})");
                        }
                    }
                }
                ScanCompletedEvent?.Invoke();
            }
            catch (Exception ex)
            {
                Disconnect();
                throw new Exception(ex.Message);

            }
        }

        private void GetValueFromParameter(SiemensReadArea area)
        {
            foreach (var par in area.Parameters)
            {
                if (par is Parameter<ushort> parUshort)
                {
                    parUshort.Value = S7.S7.GetUIntAt(area.Data, parUshort.ByteNum - area.Start);
                }
                else if (par is Parameter<short> parShort)
                {
                    parShort.Value = S7.S7.GetIntAt(area.Data, parShort.ByteNum - area.Start);
                }
                else if (par is Parameter<bool> parBool)
                {
                    parBool.Value = S7.S7.GetBitAt(area.Data, parBool.ByteNum - area.Start, parBool.BitNum);
                }
                else if (par is Parameter<int> parInt)
                {
                    parInt.Value = S7.S7.GetDIntAt(area.Data, parInt.ByteNum - area.Start);
                }
                else if (par is Parameter<uint> parUint)
                {
                    parUint.Value = S7.S7.GetUDIntAt(area.Data, parUint.ByteNum - area.Start);
                }
                else if (par is Parameter<float> parFloat)
                {
                    parFloat.Value = S7.S7.GetRealAt(area.Data, parFloat.ByteNum - area.Start);
                }
                else if (par is Parameter<string> parstring && parstring.IsRequired)
                {
                    parstring.Value = S7.S7.GetStringAt(area.Data, parstring.ByteNum - area.Start);
                }
            }

        }

        public void WriteParameter(object parameter)
        {
            if (!Connected) return;
            if (parameter is Parameter<ushort> parUshort)
            {
                var arr = new byte[2];
                S7.S7.SetUIntAt(arr, 0, parUshort.WriteValue);
                Write(parUshort, arr);
            }
            else if (parameter is Parameter<short> parShort)
            {
                var arr = new byte[2];
                S7.S7.SetIntAt(arr, 0, parShort.WriteValue);
                Write(parShort, arr);
            }
            else if (parameter is Parameter<bool> parBool)
            {
                var arr = new byte[1];
                _s7Client.DBRead(parBool.DbNum, parBool.ByteNum, 1, arr);
                S7.S7.SetBitAt(ref arr, 0, parBool.BitNum, parBool.WriteValue);
                Write(parBool, arr);
            }
            else if (parameter is Parameter<int> parInt)
            {
                var arr = new byte[4];
                S7.S7.SetUDintAt(arr, 0, parInt.WriteValue);
                Write(parInt, arr);
            }
            else if (parameter is Parameter<uint> parUint)
            {
                var arr = new byte[4];
                S7.S7.SetUDintAt(arr, 0, (int)parUint.WriteValue);
                Write(parUint, arr);
            }
            else if (parameter is Parameter<float> parFloat)
            {
                var arr = new byte[4];
                S7.S7.SetRealAt(arr, 0, parFloat.WriteValue);
                Write(parFloat, arr);
            }
            else if (parameter is Parameter<string> parstring)
            {
                var arr = new byte[parstring.Length];
                S7.S7.SetStringAt(arr, 0, parstring.Length, parstring.WriteValue);
                Write(parstring, arr);
            }

            void Write<T>(Parameter<T> parameter, byte[] bytes) where T : IComparable
            {
                try
                {
                    _logger.LogInformation($"Производится запись параметра \"{parameter.Description}\" со значением \"{parameter.WriteValue}\"");
                    var result = _s7Client.DBWrite(parameter.DbNum, parameter.ByteNum, bytes.Length, bytes);
                    if(result!=0)
                    {

                    }
                }
                catch (Exception ex)
                {
                    Disconnect();
                    throw new Exception($"Ошибка записи параметра \"{parameter.Description}\" (№ ДБ={parameter.DbNum}, № байта={parameter.ByteNum}) - {ex.Message}");
                    
                }
            }
        }


        private void Connect()
        {
            try
            {
                _logger.LogInformation($"Выполняется покдлючение к ПЛК Siemens");
                _s7Client.ConnectTo(CommSettings.Ip, 0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception($"Подключение к ПЛК Siemens - {ex.Message}");
            }
            finally { Connected = _s7Client.Connected; }

        }

        private void Disconnect()
        {
            try
            {
                _logger.LogInformation($"Выполняется отключение от ПЛК Siemens");
                if (_s7Client.Connected)
                    _s7Client.Disconnect();
            }
            catch (Exception ex)
            {
                throw new Exception($"Отключение от ПЛК Siemens - {ex.Message}");
            }
            finally { Connected = _s7Client.Connected; }
        }

        private int GetParamSize(ParameterBase par)
        {
            if (par is Parameter<ushort> || par is Parameter<short>)
                return 2;
            else if (par is Parameter<bool>)
                return 1;
            else if (par is Parameter<int> || par is Parameter<uint> || par is Parameter<float>)
                return 4;
            else if (par is Parameter<string> parString)
                return parString.Length;
            return 1;
        }
    }
}
