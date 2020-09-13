using System.Threading;
using System.Threading.Tasks;
using DownTimeAlerter.Notification;
using ServiceWorkerCronJobDemo.Controllers;

namespace ServiceWorkerCronJobDemo.Services
{
    public interface IScheduler
    {
        Task StartAsync(DownTimeAppDto downTimeAppDto, CancellationToken cancellationToken);
    }

    public class Scheduler : IScheduler
    {
        private readonly IUrlStatusChecker _urlStatusChecker;
        private readonly NotificationFactory _notificationFactory;

        public Scheduler(IUrlStatusChecker urlStatusChecker, NotificationFactory notificationFactory){
            _urlStatusChecker = urlStatusChecker;
            _notificationFactory = notificationFactory;
        }

        public async Task StartAsync(DownTimeAppDto downTimeAppDto, CancellationToken cancellationToken){
            downTimeAppDto.Timer.Elapsed += async (sender, args) =>
            {
                if (!cancellationToken.IsCancellationRequested){
                    var status = await _urlStatusChecker.CheckUrlAsync(downTimeAppDto.Url);
                    if (!status){
                        await _notificationFactory
                            .Create((NotificationType) downTimeAppDto.NotificationType, downTimeAppDto.Email)
                            .SendAsync();
                    }
                }
            };
            downTimeAppDto.Timer.Start();
            await Task.CompletedTask;
        }
    }
}