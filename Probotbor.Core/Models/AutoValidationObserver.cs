using CommunityToolkit.Mvvm.ComponentModel;

namespace Probotbor.Core.Models
{
    public class AutoValidationObserver : ObservableValidator
    {
        public AutoValidationObserver()
        {
            this.PropertyChanged += (o, e) =>
            {
                ValidateAllProperties();
            };
        }
    }
}
