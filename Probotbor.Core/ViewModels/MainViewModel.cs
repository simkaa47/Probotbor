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
            _timer = new Timer(UpdateTime);
            _timer.Change(0, 1000);
        }

        Timer _timer;

        [ObservableProperty]
        private DateTime _pcTime;

        public ParametersVm ParametersVm { get; }
        public PlcVm PlcVm { get; }
        public AccessViewModel AccessViewModel { get; }


        private void UpdateTime(object? obj)
        {
            PcTime = DateTime.Now;
        }
    }
}
