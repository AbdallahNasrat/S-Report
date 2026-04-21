using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.NotficationServices
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null);
        Task<bool> SendTopicNotificationAsync(string topic, string title, string body);
    }
}
