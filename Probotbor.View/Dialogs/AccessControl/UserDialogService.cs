using Probotbor.Core.Contracts.AccessControl;
using Probotbor.Core.Models.AccessControl;
using Probotbor.View.Dialogs.AccessControl;
using System.Threading.Tasks;

namespace Probotbor.View.Dialogs
{
    internal class UserDialogService : IAccessDialogService
    {
        public  Task<bool> ShowDialog(User user)
        {
            UserDialogWindow userWindow = new UserDialogWindow(user);
            var result =  userWindow.ShowDialog();
            if (result == true) { return Task.FromResult(true); }
            return Task.FromResult(false);
        }
    }
}
