using Domain.Entites;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class NotificationRepo : GenericRepo<Notification>, INotificationRepository
    {
        public NotificationRepo(Context context) : base(context)
        {
        }

    }
}
