using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DownTimeAlerter.Notification
{
    public class EmailService : INotificationService
    {
        public string Email { get; private set; }

        public EmailService(string email){
            Email = email;
        }

        /// <summary>
        /// TODO : Add smtp server etc.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SendAsync(){
            Console.WriteLine($"Email sent to {Email}");
            await Task.CompletedTask;
        }
    }
}