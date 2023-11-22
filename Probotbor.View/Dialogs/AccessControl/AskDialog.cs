using Probotbor.Core.Contracts.Dialog;
using System.Threading.Tasks;

namespace Probotbor.View.Dialogs.AccessControl
{
    public class AskDialog : IQuestionDialog
    {
        public Task<bool> Ask(string title, string message)
        {
            AskDialogWindow window = new AskDialogWindow(); 
            window.Title = title;
            window.Content.Text = message;
            if (window.ShowDialog() == true)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
