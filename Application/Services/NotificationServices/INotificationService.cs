using Application.DTOs.NotficationDTOs;

namespace Application.Services.NotificationServices
{
    public interface INotificationService
    {
        public Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null);
        public Task<bool> SendTopicNotificationAsync(string topic, string title, string body);

        public Task<bool> SendNotificationToUser(int employeeId, SubmitNotificationDto dto);
        public Task<bool> SendNotificationToCity(int employeeId, SubmitNotificationDto dto);




    }
}
