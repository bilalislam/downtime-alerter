using DownTimeAlerter.Notification;

namespace ServiceWorkerCronJobDemo.Controllers
{
    public class DownTimeAlerterViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public int NotificationType { get; set; }
        public double Interval { get; set; }
    }
}