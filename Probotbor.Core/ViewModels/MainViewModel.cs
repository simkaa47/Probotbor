using CommunityToolkit.Mvvm.ComponentModel;

namespace Probotbor.Core.ViewModels
{
    public partial class MainViewModel:ObservableObject
    {
        public MainViewModel(ParametersVm parametersVm)
        {
            ParametersVm = parametersVm;
        }

        public ParametersVm ParametersVm { get; }
    }
}
