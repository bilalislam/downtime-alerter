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

        private readonly IScheduler _scheduleService;

        public DownTimeAlerterController(IScheduler scheduleService){
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public IEnumerable<DownTimeAlerterViewModel> Get(){
            return _timers.Select(x => new DownTimeAlerterViewModel()
            {
                Name = x.Key,
                Url = x.Value.Url,
                Interval = x.Value.Timer.Interval,
                Email = x.Value.Email,
                NotificationType = x.Value.NotificationType
            }).ToList();
        }

        /// <summary>
        /// TODO : Validation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Create([FromBody] DownTimeAlerterViewModel model, CancellationToken cancellationToken){
            if (model == null) return;
            _timers.TryGetValue(model.Name, out var app);

            app ??= new DownTimeAppDto()
            {
                Name = model.Name,
                Url = model.Url,
                Email = model.Email,
                NotificationType = model.NotificationType,
                Timer = new Timer(model.Interval)
            };

            _timers.TryAdd(app.Name, app);
            await _scheduleService.StartAsync(app, cancellationToken);
        }

        /// <summary>
        /// TODO: Validation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="model"></param>
        [HttpPut("{name}")]
        public void Update([FromRoute] string name, [FromBody] DownTimeAlerterViewModel model){
            if (string.IsNullOrEmpty(name)) return;
            _timers.TryGetValue(name, out var app);
            if (app != null){
                app.Name = model.Name;
                app.Url = model.Url;
                app.Email = model.Email;
                app.NotificationType = model.NotificationType;
                app.Timer.Interval = model.Interval;

                _timers.AddOrUpdate(app.Name, app, (key, old) => app);
            }
        }


        [HttpDelete("{name}")]
        public void Delete([FromRoute] string name){
            if (string.IsNullOrEmpty(name)) return;
            _timers.Remove(name, out var app);
            app.Timer.Dispose();
        }
    }
}