using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceWorkerCronJobDemo.Services;
using Timer = System.Timers.Timer;

namespace ServiceWorkerCronJobDemo.Controllers
{
    [ApiController]
    [Route("health-check")]
    public class DownTimeAlerterController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, DownTimeAppDto> _timers =
            new ConcurrentDictionary<string, DownTimeAppDto>();

        private readonly IScheduleService _scheduleService;

        public DownTimeAlerterController(IScheduleService scheduleService){
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public IEnumerable<DownTimeAlerterViewModel> Get(){
            return _timers.Select(x => new DownTimeAlerterViewModel()
            {
                Name = x.Key,
                Url = x.Value.Url,
                Interval = x.Value.Timer.Interval
            }).ToList();
        }


        [HttpPost]
        public async Task Create([FromBody] DownTimeAlerterViewModel model, CancellationToken cancellationToken){
            if (model == null) return;
            _timers.TryGetValue(model.Name, out var app);

            app ??= new DownTimeAppDto()
            {
                Name = model.Name,
                Url = model.Url,
                Timer = new Timer(model.Interval)
            };

            _timers.TryAdd(app.Name, app);
            await _scheduleService.Start(app, cancellationToken);
        }

        [HttpPut]
        public void Update([FromBody] DownTimeAlerterViewModel model){
            if (string.IsNullOrEmpty(model.Name)) return;
            _timers.TryGetValue(model.Name, out var app);
            if (app != null){
                app.Url = model.Url;
                app.Timer.Interval = model.Interval;
            }

            _timers.AddOrUpdate(model.Name, app, (key, old) => app);
        }

        /// <summary>
        /// timer dispose oldugunda elapsed olan delegation silinmesi lazım test et.
        /// </summary>
        /// <param name="model"></param>
        [HttpDelete]
        public void Delete([FromBody] DownTimeAlerterViewModel model){
            if (string.IsNullOrEmpty(model.Name)) return;
            _timers.Remove(model.Name, out var app);
            app.Timer.Dispose();
        }
    }
}