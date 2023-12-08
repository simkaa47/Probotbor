using Probotbor.Core.Models.Communication;

namespace Probotbor.Core.Models.Plc
{
    public class Kanistra
    {
        public Kanistra(int index)
        {
            Index = index;
            Id.Name += index;            
            ProbeCnt.Name += index;            
            IsExist.Name += index;            
        }
        public int Index { get; }

        public Parameter<string> Id { get; set; } = new Parameter<string>("KanistraId", "Id канистры", string.Empty, "ZZZZZZZZZZZZZZZZ") { Length = 12 };
        public Parameter<short> ProbeCnt { get; } = new Parameter<short>("ProbeCnt", "Кол-во проб в канистре", 0, short.MaxValue);
        public Parameter<bool> IsExist { get; } = new Parameter<bool>("KanistraExist", "Наличие канистры", false, true);
    }
}
