using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using ServiceWorkerCronJobDemo.Controllers;

namespace ServiceWorkerCronJobDemo.Services
{
    public interface IWorkerService
    {
        IEnumerable<WorkerDto> List();

        WorkerDto Get(string key);
        void Add(string key, WorkerDto worker);

        void Update(string key, WorkerDto worker);
        void Remove(string key);
    }

    public class WorkerService : IWorkerService
    {
        private static readonly ConcurrentDictionary<string, WorkerDto> _workers =
            new ConcurrentDictionary<string, WorkerDto>();

        private readonly IScheduler _scheduler;

        public WorkerService(IScheduler scheduleService){
            _scheduler = scheduleService;
        }

        public IEnumerable<WorkerDto> List(){
            return _workers.Values;
        }

        public WorkerDto Get(string key){
            _workers.TryGetValue(key, out var worker);
            return worker;
        }

        public void Add(string key, WorkerDto worker){
            _workers.TryGetValue(key, out var app);
            if (app == null){
                _workers.TryAdd(key, worker);
                _scheduler.StartAsync(worker, CancellationToken.None);
            }
        }

        public void Update(string key, WorkerDto worker){
            _workers.TryGetValue(key, out var app);
            if (app != null){
                Remove(key);
                Add(worker.Name, worker);
            }
        }

        public void Remove(string key){
            _workers.TryRemove(key, out var worker);
            worker?.Timer.Dispose();
        }
    }
}