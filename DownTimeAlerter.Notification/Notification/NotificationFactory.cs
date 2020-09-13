using System;

namespace DownTimeAlerter.Notification
{
    public class NotificationFactory
    {
        /// <summary>
        /// TODO : This could be move into DI because of EmailService shouldn't create here
        /// </summary>
        /// <param name="type"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public INotificationService Create(NotificationType type, string email){
            return type switch
            {
                NotificationType.Email => new EmailService(email),
                _ => throw new NotImplementedException()
            };
        }
    }
}