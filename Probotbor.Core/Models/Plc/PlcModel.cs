using Probotbor.Core.Models.Probotbor;

namespace Probotbor.Core.Models.Plc
{
    public class PlcModel
    {
        public PlcModel(ProbotborSettings probotborSettings)
        {
            ProbotborSettings = probotborSettings;
            IndicationModel = new PlcIndicationModel(probotborSettings);
        }
        public static List<object> Parameters { get; } = new List<object>();
        public ProbotborSettings ProbotborSettings { get; }
        public PlcIndicationModel IndicationModel { get; }
    }
}
