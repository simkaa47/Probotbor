using Probotbor.Core.Models.Communication;
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
        public static List<ParameterBase> Parameters { get; } = new List<ParameterBase>();
        public ProbotborSettings ProbotborSettings { get; }
        public PlcIndicationModel IndicationModel { get; }
    }
}
