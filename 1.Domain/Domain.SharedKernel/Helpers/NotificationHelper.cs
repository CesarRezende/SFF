using Flunt.Notifications;

namespace SFF.SharedKernel.Helpers
{
    public static class NotificationHelper
    {
        public static string CreateLogMsg(this IEnumerable<Notification> notifications) 
        {
            return notifications.Select(x => x.Message).Aggregate((x, y) => $"{x}\r\n{y}");
        }
    }
}
