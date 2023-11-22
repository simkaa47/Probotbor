using CommunityToolkit.Mvvm.ComponentModel;
using Probotbor.Core.Infrastructure.DataAccess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Probotbor.Core.Models.AccessControl
{
    public enum UserAccessLevel
    {
        [Description("Не авторизован")]
        None,
        [Description("Сервис")]
        Service,
        [Description("Администратор")]
        Admin
    }
    public partial class User : EntityCommon
    {        
        [Required]
        [NotifyDataErrorInfo]
        [ObservableProperty]
        [MinLength(3, ErrorMessage = "Login must contain at least 3 symbols")]
        private string? _login;

        [ObservableProperty]
        [Required]
        [NotifyDataErrorInfo]
        [MinLength(3, ErrorMessage = "Password name must contain at least 3 symbols")]
        private string? _password;

        [ObservableProperty]
        [Required]
        [NotifyDataErrorInfo]
        [MinLength(3, ErrorMessage = "First name must contain at least 3 symbols")]
        private string? _firstName;

        [ObservableProperty]
        [Required]
        [NotifyDataErrorInfo]
        [MinLength(3, ErrorMessage = "Last name name must contain at least 3 symbols")]
        private string? _lastName;

        [ObservableProperty]
        private UserAccessLevel _accessLevel;
        [NotMapped]
        public string FullName => $"{LastName} {FirstName}";


         
    }
}
