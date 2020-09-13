using System.Timers;

namespace ServiceWorkerCronJobDemo.Controllers
{
    public class DownTimeAppDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Timer Timer { get; set; }
    }
}