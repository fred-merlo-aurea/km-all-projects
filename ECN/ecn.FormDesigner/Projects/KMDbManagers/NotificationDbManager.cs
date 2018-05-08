using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;
using KMEnums;

namespace KMDbManagers
{
    public class NotificationDbManager : DbManagerBase
    {
        public Notification GetByID(int id)
        {
            return KM.Notifications.Single(x => x.Notification_Seq_ID == id);
        }

        public IEnumerable<Notification> GetAllByFormIDAndTargetType(int id, NotificationType type)
        {
            return KM.Notifications.Include("ConditionGroup")
                        .Where(x => x.Form_Seq_ID == id && x.IsInternalUser == (type == NotificationType.User) && !x.IsDoubleOptIn);
        }

        public Notification GetDoubleOptInByFormID(int id)
        {
            return KM.Notifications.SingleOrDefault(x => x.Form_Seq_ID == id && x.IsDoubleOptIn);
        }
        
        public void RemoveByFormID(int formId)
        {
            List<Notification> toRemove = KM.Notifications.Where(x => x.Form_Seq_ID == formId).ToList();
            foreach (var n in toRemove)
            {
                KM.Notifications.Remove(n);
            }
        }

        public void Remove(IEnumerable<int> IDs)
        {
            List<Notification> toRemove = KM.Notifications.Where(x => IDs.Contains(x.Notification_Seq_ID)).ToList();
            foreach (var n in toRemove)
            {
                KM.Notifications.Remove(n);
            }
        }

        public void Remove(Notification n)
        {
            KM.Notifications.Remove(KM.Notifications.Single(x => x.Notification_Seq_ID == n.Notification_Seq_ID));
        }

        public void Add(Notification n)
        {
            KM.Notifications.Add(n);
        }

        public IEnumerable<int> GetIDsByCondGroupIDs(IEnumerable<int> IDs)
        {
            return KM.Notifications.Where(x => x.ConditionGroup_Seq_ID.HasValue && IDs.Contains(x.ConditionGroup_Seq_ID.Value)).Select(x => x.Notification_Seq_ID);
        }
    }
}