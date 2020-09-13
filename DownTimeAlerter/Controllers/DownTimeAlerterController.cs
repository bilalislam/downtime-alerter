using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ServiceWorkerCronJobDemo.Services;
using Timer = System.Timers.Timer;

namespace ServiceWorkerCronJobDemo.Controllers
{
    [ApiController]
    [Route("health-check")]
    public class DownTimeAlerterController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public DownTimeAlerterController(IWorkerService workerService){
            _workerService = workerService;
        }

        /// <summary>
        /// List workers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<DownTimeAlerterViewModel> Get(){
            return _workerService.List().Select(x => new DownTimeAlerterViewModel()
            {
                Name = x.Name,
                Email = x.Email,
                Url = x.Url,
                Interval = x.Timer.Interval,
                NotificationType = x.NotificationType
            });
        }

        /// <summary>
        /// Create Worker
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public void Create([FromBody] DownTimeAlerterViewModel model){
            if (!ModelState.IsValid) return;

            var worker = new WorkerDto()
            {
                Name = model.Name,
                Url = model.Url,
                Email = model.Email,
                NotificationType = model.NotificationType,
                Timer = new Timer(model.Interval)
            };

            _workerService.Add(model.Name, worker);
        }

        /// <summary>
        /// Update Worker
        /// </summary>
        /// <param name="name"></param>
        /// <param name="model"></param>
        [HttpPut("{name}")]
        public void Update([FromRoute] string name, [FromBody] DownTimeAlerterViewModel model){
            if (string.IsNullOrEmpty(name) || !ModelState.IsValid) return;
            _workerService.Update(name, new WorkerDto()
            {
                Name = model.Name,
                Url = model.Url,
                Email = model.Email,
                NotificationType = model.NotificationType,
                Timer = new Timer(model.Interval)
            });
        }


        /// <summary>
        /// Delete worker
        /// </summary>
        /// <param name="name"></param>
        [HttpDelete("{name}")]
        public void Delete([FromRoute] string name){
            if (string.IsNullOrEmpty(name)) return;
            _workerService.Remove(name);
        }
    }
}