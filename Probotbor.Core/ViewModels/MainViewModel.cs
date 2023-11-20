using CommunityToolkit.Mvvm.ComponentModel;

namespace Probotbor.Core.ViewModels
{
    public partial class MainViewModel:ObservableObject
    {
        public MainViewModel(ParametersVm parametersVm, PlcVm plcVm)
        {
            ParametersVm = parametersVm;
            PlcVm = plcVm;
        }

        public ParametersVm ParametersVm { get; }
        public PlcVm PlcVm { get; }
    }
}
