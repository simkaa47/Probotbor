namespace Probotbor.Core.Models.Communication.Modbus
{
    public class ModbusReadMemory
    {
        public int Offset { get; set; }
        public ushort[] Buffer { get; set; } = new ushort[0];   
    }
}
