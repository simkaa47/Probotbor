using Probotbor.Core.Contracts.Communication;
using Probotbor.Core.Models.Communication;
using System.Threading.Tasks;

namespace Probotbor.View.Dialogs.Errors
{
    public class AddErrorDialog : IErrorDialog
    {
        public Task<bool> ShowDialog(Parameter<bool> par)
        {
            AddErrorDialogWindow errWindow = new AddErrorDialogWindow(par);
            var result = errWindow.ShowDialog();
            if (result == true) { return Task.FromResult(true); }
            return Task.FromResult(false);
        }
    }
}
