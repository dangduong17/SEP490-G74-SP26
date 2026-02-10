using vn.edu.fpt.entity;

namespace vn.edu.fpt.service
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification?> GetNotificationByIdAsync(int id);
        Task<Notification?> CreateNotificationAsync(Notification notification);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteNotificationAsync(int id);
        Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId);
    }
}
