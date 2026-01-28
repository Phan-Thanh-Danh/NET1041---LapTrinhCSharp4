using Bai2.Services.Interfaces;

namespace Bai2.Services
{
    public class LoggingService : ILoggingService
    {
        public void Log(string message)
        {
            Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
        }
    }
}
