using KMEntities;
using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.PostModels
{
    public class FormNotificationsPostModel : PostModelBase
    {
        public int Id { get; set; }

        public IEnumerable<SubscriberNotificationModel> SubscriberNotifications { get; set; }

        public IEnumerable<InternalUserNotificationModel> InternalUserNotifications { get; set; }

        public int CustomerID { get; set; }
    }
}
