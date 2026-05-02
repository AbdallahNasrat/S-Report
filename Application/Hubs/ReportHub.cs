using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace SReport.Hubs
{
    public class ReportHub : Hub
    {
        // الفرونت إند هينادي الميثود دي ويبعت رقم المدينة
        public async Task JoinCityGroup(string cityId)
        {
            // بنضيف الكونكشن بتاع اليوزر ده لجروب باسم المدينة
            await Groups.AddToGroupAsync(Context.ConnectionId, cityId);
        }

        // يفضل دايماً تعمل ميثود للخروج عشان توفر موارد السيرفر
        public async Task LeaveCityGroup(string cityId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, cityId);
        }
    }
}