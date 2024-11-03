namespace EpamWeb.Loggers
{
    public interface ILoggerManager
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message, Exception ex = null);
        void CloseAndFlush();
    }
}
