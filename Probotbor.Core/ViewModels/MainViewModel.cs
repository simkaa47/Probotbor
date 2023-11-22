using CommunityToolkit.Mvvm.ComponentModel;

namespace Probotbor.Core.ViewModels
{
    public partial class MainViewModel:ObservableObject
    {
        public MainViewModel(ParametersVm parametersVm, PlcVm plcVm, AccessViewModel accessViewModel)
        {
            ParametersVm = parametersVm;
            PlcVm = plcVm;
            AccessViewModel = accessViewModel;
        }

        public ParametersVm ParametersVm { get; }
        public PlcVm PlcVm { get; }
        public AccessViewModel AccessViewModel { get; }
    }
}
