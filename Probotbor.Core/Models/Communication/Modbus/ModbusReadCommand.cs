namespace Probotbor.Core.Models.Communication.Modbus
{
    public class ModbusReadCommand
    {
        public int Start { get; set; }
        public int Count { get; set; }
        public ushort[] Buffer { get; set; }

        public ModbusRegType RegType { get; set; }

        public ModbusReadCommand(int start, int count, ushort[] buffer, ModbusRegType regType)
        {
            Start = start;
            Count = count;
            Buffer = buffer;
            RegType = regType;
        }
    }
}
