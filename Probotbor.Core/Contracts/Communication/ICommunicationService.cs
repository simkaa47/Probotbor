namespace Probotbor.Core.Contracts.Communication
{
    public interface ICommunicationService
    {
        public void ReadAllData();
        public void WriteParameter(object parameter);

        public event Action ScanCompletedEvent;
        public bool Connected { get; }
    }
}
