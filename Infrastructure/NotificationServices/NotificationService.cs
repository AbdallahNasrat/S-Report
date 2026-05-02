using Application.DTOs.NotficationDTOs;
using Application.DTOs.NotificationDTOs;
using Application.Services.NotificationServices;
using Domain.Entites;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Notification = Domain.Entites.Notification;

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

        public async Task<bool> SendNotificationToMultipleCities(int employeeId, SubmaitCityNotificationDto dto)
        {
            if (dto.CityIDs == null || !dto.CityIDs.Any()) {
                return await SendTopicNotificationAsync("all_users", dto.Title, dto.Body);
            }
            var cities = await _uow.CitiesRepo.GetWhereAsync(c => dto.CityIDs.Contains(c.Id));
            var notification = new Notification()
            {
                Title = dto.Title,
                Body = dto.Body,
                Date = DateTime.Now,
                EmployeeId = employeeId,
                Cities = cities.ToList()
            };
            _uow.NotificationRepo.Add(notification);
            var result = await _uow.SaveChangesAsync() > 0;
            if (result) {
                foreach (var id in cities) {
                    _ = SendTopicNotificationAsync($"city_{id}", notification.Title, notification.Body);
                }
            }
            return result;
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

        public async Task<MyNotificationDto> GetNotificationById(int id)
        {
            var notification = await _uow.NotificationRepo.GetByIdAsync(id,false);
            if (notification == null) return null;
            var result = new MyNotificationDto()
            {
                Title = notification.Title,
                Body = notification.Body,
                date = notification.Date
            };
            return result;
        }

        public async Task<IEnumerable<MyNotificationDto>> GetMyNotifications(int userId, int cityId)
        {

            var notifications = await _uow.NotificationRepo.GetAllAsync(n =>
                n.UserId == userId ||
                n.Cities.Any(c => c.Id == cityId) ||
                n.UserId == null // لو نوتفكيشن عامة للكل
            , "Cities"
            );

            
            return notifications
                .OrderByDescending(n => n.Date) 
                .Select(n => new MyNotificationDto()
                {
                    Title = n.Title,
                    Body = n.Body,
                    date = n.Date
                });
        }

        
        public async Task<IEnumerable<CityNotificationDto>> GetCityNotifications(int cityId)
        {
            var city = await _uow.CitiesRepo.GetByIdAsync(cityId, false);
            if (city == null) throw new NotFoundException("the cityid does not exist");
            var notitfications = await _uow.NotificationRepo.GetAllAsync(n => n.Cities.Any(c => c.Id == cityId) || n.UserId==null, "Cities");
            if (notitfications == null || !notitfications.Any()) return [];

            var result = notitfications.Select(n => new CityNotificationDto()
            {
                Title = n.Title,
                Body = n.Body,
                date = n.Date,
                userId=n.UserId
                
            });
            result = result.OrderByDescending(n => n.date);

            return result;
        }
        public async Task<IEnumerable<AllNotificationsInSystemDto>> GetAllNotifications(int page, int size)
        {
            if (page <= 0) page = 1;
            if (size <= 0 || size > 50) size = 10; 


            int skip = (page - 1) * size;


            var notifications = await _uow.NotificationRepo.GetAllAsync(
                filter: null,
                properties: "Cities"
            );

            var result = notifications
                .OrderByDescending(n => n.Date) // الأحدث أولاً
                .Skip(skip)                    // فوت اللي فات
                .Take(size)                    // خد المطلوب بس
                .Select(n => new AllNotificationsInSystemDto()
                {
                    Title = n.Title,
                    Body = n.Body,
                    date = n.Date,
                    userId=n.UserId,
                    cityName = n.Cities?
                    .Where(c => !string.IsNullOrWhiteSpace(c.Name)) // بيتجاهل الـ null والمسافات الفاضية
                    .Select(c => c.Name)                            // بياخد الأسماء الحقيقية بس
                    .ToList() ?? new List<string>()

                }).ToList();

            return result;
        }



        public async Task SendSilentMissionNotification(Report report)
        {
            // بنبعت لـ Topic باسم المدينة، عشان كل متطوعين المدينة دي يوصلهم الإشعار في الخلفية
            string topicName = $"City_{report.CityId}_Volunteers";

            // 🚀 لاحظ: إحنا مش بنحط "Notification" object عشان ميعملش صوت، بنحط "Data" بس
            var message = new Message()
            {
                Topic = topicName,
                Data = new Dictionary<string, string>()
            {
                { "type", "NEW_NEARBY_MISSION" }, // عشان فلاتر يعرف نوع الإشعار
                { "reportId", report.Id.ToString() },
                { "latitude", report.Latitude.ToString() },
                { "longitude", report.Longitude.ToString() },
                { "description", report.Description ?? "بلاغ جديد يحتاج لتدخل" }
            }
            };

            try
            {
                // إرسال الإشعار لفايربيز
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Successfully sent message: {response}");
            }
            catch (Exception ex)
            {
                // يفضل تسجل الإيرور هنا لو فايربيز زعل
                Console.WriteLine($"Error sending Firebase message: {ex.Message}");
            }
        }
}
}