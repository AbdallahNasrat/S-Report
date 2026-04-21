
using Application.Services.NotficationServices;
using FirebaseAdmin.Messaging;

namespace Infrastructure.Notifications
{
    public class FirebaseNotificationService : INotificationService
    {
        public async Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null)
        {
            try
            {
                var message = new Message()
                {
                    Token = deviceToken,
                    Notification = new Notification()
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
                Notification = new Notification() { Title = title, Body = body }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return true;
        }
    }
}