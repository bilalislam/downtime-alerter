using System.Threading.Tasks;

namespace DownTimeAlerter.Notification
{
    public interface INotificationService
    {
        Task SendAsync();
    }
}