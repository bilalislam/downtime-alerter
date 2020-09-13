using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceWorkerCronJobDemo.Controllers
{
    public class DownTimeAlerterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int NotificationType { get; set; }

        [Required]
        [Range(1, Double.MaxValue)]
        public double Interval { get; set; }
    }
}