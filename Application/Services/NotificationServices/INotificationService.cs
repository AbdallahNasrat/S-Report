using Application.DTOs.NotficationDTOs;
using Application.DTOs.NotificationDTOs;
using Domain.Entites;

namespace Application.Services.NotificationServices
{
    public interface INotificationService
    {
        public Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null);
        public Task<bool> SendTopicNotificationAsync(string topic, string title, string body);

        public Task<bool> SendNotificationToUser(int employeeId, SubmitNotificationDto dto);
        public Task<bool> SendNotificationToMultipleCities(int employeeId, SubmaitCityNotificationDto dto);
        public Task<MyNotificationDto> GetNotificationById(int id);
        public Task<IEnumerable<MyNotificationDto>>GetMyNotifications(int userId, int cityId);
        public Task<IEnumerable<CityNotificationDto>> GetCityNotifications(int cityId);
        public Task<IEnumerable<AllNotificationsInSystemDto>> GetAllNotifications(int page, int size);
        public Task SendSilentMissionNotification(Report report);





    }
}
