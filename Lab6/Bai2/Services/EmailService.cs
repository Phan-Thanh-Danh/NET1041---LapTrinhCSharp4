using Bai2.Services.Interfaces;

namespace Bai2.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            Console.WriteLine($"Send mail to {to} - {subject}");
        }
    }
}
