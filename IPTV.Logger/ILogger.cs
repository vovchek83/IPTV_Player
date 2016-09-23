namespace IPTV.Logger
{
    public interface ILogger
    {
        void Error(string message);
        void Debug(string message);
        void Warning(string message);
        void Info(string message);
    }
}