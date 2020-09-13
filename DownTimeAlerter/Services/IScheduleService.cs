using System.Threading;
using System.Threading.Tasks;
using ServiceWorkerCronJobDemo.Controllers;

namespace ServiceWorkerCronJobDemo.Services
{
    public interface IScheduleService
    {
        Task Start(DownTimeAppDto downTimeAppDto, CancellationToken cancellationToken);
    }
}