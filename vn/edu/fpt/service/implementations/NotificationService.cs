using vn.edu.fpt.entity;
using vn.edu.fpt.repository;

namespace vn.edu.fpt.service
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _unitOfWork.Notifications.GetAllAsync();
        }

        public async Task<Notification?> GetNotificationByIdAsync(int id)
        {
            return await _unitOfWork.Notifications.GetByIdAsync(id);
        }

        public async Task<Notification?> CreateNotificationAsync(Notification notification)
        {
            notification.CreatedAt = DateTime.Now;
            notification.IsRead = false;

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            return notification;
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (notification == null)
                return false;

            notification.IsRead = true;
            notification.UpdatedAt = DateTime.Now;

            _unitOfWork.Notifications.Update(notification);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (notification == null)
                return false;

            notification.IsDeleted = true;
            notification.UpdatedAt = DateTime.Now;

            _unitOfWork.Notifications.Update(notification);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId)
        {
            var notifications = await _unitOfWork.Notifications.GetAllAsync();
            return notifications.Where(n => n.UserId == userId && !n.IsDeleted);
        }
    }
}
