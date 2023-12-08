namespace Probotbor.Core.Models.Communication
{
    public class CommSettings
    {
        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; }
        public CommProtocol Protocol { get; set; }

    }
}
