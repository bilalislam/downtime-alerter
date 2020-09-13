using System.Timers;

namespace ServiceWorkerCronJobDemo.Controllers
{
    public class WorkerDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public int NotificationType { get; set; }
        public Timer Timer { get; set; }
        public double Interval { get; set; }
    }
}