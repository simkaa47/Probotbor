using Probotbor.Core.Models.Communication;

namespace Probotbor.Core.Contracts.Communication
{
    public interface IErrorDialog
    {
        Task<bool> ShowDialog(Parameter<bool> par);
    }
}
