using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Probotbor.Core.Models.AccessControl
{
    public partial class Login : ObservableValidator
    {
        [ObservableProperty]
        [Required]
        [MinLength(3, ErrorMessage = "Login must contain at least 3 symbols")]
        private string? _loginName;

        [ObservableProperty]
        [Required]
        [MinLength(3, ErrorMessage = "Password name must contain at least 3 symbols")]
        private string? _password;

        [ObservableProperty]
        private bool _faliledLogin;

        [ObservableProperty]
        private bool _isSuccessLogin;


        public Login()
        {
            this.PropertyChanged += (o, s) =>
            {
                if (s.PropertyName != nameof(FaliledLogin))
                {
                    FaliledLogin = false;
                }
            };
        }
    }
}
