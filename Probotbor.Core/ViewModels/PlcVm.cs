using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Probotbor.Core.Models.Communication;
using Probotbor.Core.Services.Plc;

namespace Probotbor.Core.ViewModels
{
    public partial class PlcVm:ObservableObject
    {
        public PlcVm(PlcMainService plcMainService) 
        {
            PlcMainService = plcMainService;
        }

        public PlcMainService PlcMainService { get; }


        [RelayCommand]
        public void WriteParameter(object parameter)
        {
            
            
            
            
            if (parameter is Parameter<bool> parBool)
            {
                parBool.WriteValue = !parBool.Value;
            }
            PlcMainService.WriteParameter(parameter);
        }
    }
}
