using Probotbor.Core.Services.Plc;

namespace Probotbor.Core.ViewModels
{
    public partial class PlcVm
    {
        public PlcVm(PlcMainService plcMainService) 
        {
            PlcMainService = plcMainService;
        }

        public PlcMainService PlcMainService { get; }
    }
}
