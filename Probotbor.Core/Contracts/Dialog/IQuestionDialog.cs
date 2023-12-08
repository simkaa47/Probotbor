namespace Probotbor.Core.Contracts.Dialog
{
    public interface IQuestionDialog
    {
        Task<bool> Ask(string title, string message);
    }
}
