namespace Probotbor.Core.Models.Communication
{
    public class ParameterSequence
    {
        public ParameterSequence(int start, int end)
        {
            Start = start;
            End = end;
        }
        public int Start { get; }
        public int End { get; }
    }
}
