using CommunityToolkit.Mvvm.ComponentModel;
using Probotbor.Core.Models.Communication;

namespace Probotbor.Core.Models.Events
{
    public partial class Error : Parameter<bool>
    {
        public Error(string name, string description, bool minValue, bool maxValue) : base(name, description, minValue, maxValue)
        {
            
        }

        [ObservableProperty]
        private DateTime _lastTimeExecute;


    }
}
