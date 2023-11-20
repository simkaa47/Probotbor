using Microsoft.Extensions.Options;
using Probotbor.Core.Models.Plc;
using Probotbor.Core.Models.Probotbor;

namespace Probotbor.Core.Services.Plc
{
    public class PlcMainService
    {

        public PlcModel PlcModel { get; set; }
        public bool Initialized { get; }
        public ProbotborSettings ProbotborSettings { get; set; }
        public PlcMainService(IOptions<ProbotborSettings> options)
        {
            ProbotborSettings = options.Value;
            PlcModel = new PlcModel(ProbotborSettings);
            Initialized = true;
        }
    }
}
