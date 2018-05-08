using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class Notification
    {
        public static bool ExistsByName(string NotificationName, int NotificationID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Accounts.Notification.ExistsByName(NotificationName, NotificationID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ExistsByTime(int NotificationID,  string startDate, string startTime, string EndDate, string EndTime)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Accounts.Notification.ExistsByTime(NotificationID, startDate, startTime, EndDate, EndTime);
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.Accounts.Notification> GetAll()
        {
            return ECN_Framework_DataLayer.Accounts.Notification.GetAll();
        }

        public static ECN_Framework_Entities.Accounts.Notification GetByCurrentDateTime()
        {
            ECN_Framework_Entities.Accounts.Notification notification = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                notification = ECN_Framework_DataLayer.Accounts.Notification.GetByCurrentDateTime();
                scope.Complete();
            }

            return notification;
        }

        public static ECN_Framework_Entities.Accounts.Notification GetByNotificationID(int NotificationID)
        {
            ECN_Framework_Entities.Accounts.Notification notification = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                notification = ECN_Framework_DataLayer.Accounts.Notification.GetByNotificationID(NotificationID);
                scope.Complete();
            }

            return notification;
        }

        public static void Validate(ECN_Framework_Entities.Accounts.Notification notification, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (ExistsByName(notification.NotificationName, notification.NotificationID))
            {
                errorList.Add(new ECNError() { ErrorMessage = "Notification name already exists", Entity = Enums.Entity.Notification, Method = Method });
                throw new ECNException(errorList);
            }

            if (ExistsByTime(notification.NotificationID, notification.StartDate, notification.StartTime, notification.EndDate, notification.EndTime))
            {
                errorList.Add(new ECNError() { ErrorMessage = "Another notification already exists within this time", Entity = Enums.Entity.Notification, Method = Method });
                throw new ECNException(errorList);
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Delete(int NotificationID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Accounts.Notification.Delete(NotificationID, user.UserID);
                scope.Complete();
            }
        }
        public static void Save(ECN_Framework_Entities.Accounts.Notification notification, KMPlatform.Entity.User user)
        {
            Validate(notification, user);
            ECN_Framework_DataLayer.Accounts.Notification.Save(notification);
        }
    }
}
