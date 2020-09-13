using System;
using System.Threading;
using System.Threading.Tasks;
using ServiceWorkerCronJobDemo.Controllers;

namespace ServiceWorkerCronJobDemo.Services
{
    public class ScheduleService : IScheduleService
    {
        /// <summary>
        /// timer async olmalı
        /// </summary>
        /// <param name="downTimeAppDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Start(DownTimeAppDto downTimeAppDto, CancellationToken cancellationToken){
            downTimeAppDto.Timer.Elapsed += (sender, args) =>
            {
                if (!cancellationToken.IsCancellationRequested){
                    Console.WriteLine("working");
                }
            };
            downTimeAppDto.Timer.Start();

            await Task.CompletedTask;
        }
    }
}