using Application.DTOs.NotficationDTOs;
using Application.Services.NotificationServices;
using Domain.Entites;
using Domain.Interfaces.Repositories;
using FirebaseAdmin.Messaging;

namespace Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        public NotificationService(IUnitOfWork unitOfWork) {
            _uow = unitOfWork;
        }
                  
        public async Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null)
        {
            try
            {
                var message = new Message()
                {
                    Token = deviceToken,
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data //  ex  ReportId)
                };

                
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                
                return !string.IsNullOrEmpty(response);
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        public async Task<bool> SendTopicNotificationAsync(string topic, string title, string body)
        {
            var message = new Message()
            {
                Topic = topic,
                Notification = new FirebaseAdmin.Messaging.Notification() { Title = title, Body = body }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return true;
        }

        public async Task<bool> SendNotificationToCity(int employeeId, SubmitNotificationDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool>SendNotificationToUser(int employeeId,SubmitNotificationDto dto)
        {
            var userId = dto.UserId ?? 0;
            if (userId == 0) throw new ArgumentNullException("userId");            
            var user = await _uow.UsersRepo.GetByIdAsync(userId);
            if (user == null) return false; 
            var notification = new Domain.Entites.Notification()
            {
                Title = dto.Title,
                Body = dto.Body,
                UserId = dto.UserId,
                Date= DateTime.Now,
                EmployeeId = employeeId,               
            };
            _uow.NotificationRepo.Add(notification);
            var result = await _uow.SaveChangesAsync() > 0;

            if (result && user.IsNotificationEnabled && !string.IsNullOrEmpty(user.FcmToken))
            {               
                _ = SendNotificationAsync(user.FcmToken, dto.Title, dto.Body);
            }
            return result;
        }
    }
}