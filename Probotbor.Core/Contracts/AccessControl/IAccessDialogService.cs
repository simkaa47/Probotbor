using Probotbor.Core.Models.AccessControl;

namespace Probotbor.Core.Contracts.AccessControl
{
    public interface IAccessDialogService
    {
        Task<bool> ShowDialog(User user);
    }
}
