namespace Probotbor.Core.Models.Communication.Siemens
{
    public class SiemensReadArea
    {
        public byte[] Data { get; }
        public List<ParameterBase> Parameters { get; }
        public int DbNum { get; }
        public int Start { get; }
        public int End { get; }

        public SiemensReadArea(List<ParameterBase> parameters, int dbNum, int start, int end)
        {
            Parameters = parameters;
            DbNum = dbNum;
            Start = start;
            End = end;
            if(end>=start)
                Data = new byte[end - start + 1];
            else
                Data = new byte[0];

        }


    }
}
