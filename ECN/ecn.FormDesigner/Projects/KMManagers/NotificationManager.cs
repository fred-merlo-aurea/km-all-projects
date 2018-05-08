using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using KMDbManagers;
using KMEnums;
using KMEntities;
using KMModels;
using KMModels.PostModels;

namespace KMManagers
{
    public class NotificationManager : ManagerBase
    {
        private NotificationDbManager NM
        {
            get
            {
                return DB.NotificationDbManager;
            }
        }

        public IEnumerable<SubscriberNotificationModel> GetAllSubscriberNotificationsByFormID(int id)
        {
            return GetAllByFormIDAndTargetType<SubscriberNotificationModel>(id, NotificationType.Subscriber); 
        }

        public IEnumerable<InternalUserNotificationModel> GetAllUserNotificationsByFormID(int id)
        {
            return GetAllByFormIDAndTargetType<InternalUserNotificationModel>(id, NotificationType.User); 
        }

        public DOINotificationModel GetDOINotificationByFormID(int id)
        {
            return NM.GetDoubleOptInByFormID(id).ConvertToModel<DOINotificationModel>();
        }

        public IEnumerable<TModel> GetAllByFormIDAndTargetType<TModel>(int id, NotificationType type) where TModel : ModelBase, new()
        {
            return NM.GetAllByFormIDAndTargetType(id, type).ConvertToModels<TModel>();
        }

        public void Save(KMPlatform.Entity.User User,int ChannelID, FormNotificationsPostModel model)
        {
            //fill entities
            Dictionary<Notification, object[]> list = new Dictionary<Notification, object[]>();
            if (model.SubscriberNotifications != null)
            {
                foreach (var subscriber in model.SubscriberNotifications)
                {
                    InitializeNotification(subscriber, model.Id, list);
                }
            }
            if (model.InternalUserNotifications != null)
            {
                foreach (var user in model.InternalUserNotifications)
                {
                    InitializeNotification(user, model.Id, list);
                }
            }

            //save
            Form form = new FormManager().GetByID(ChannelID, model.Id);
            form.UpdatedBy = User.UserName;
            form.LastUpdated = DateTime.Now;
            using (TransactionScope t = new TransactionScope())
            {
                //delete
                List<Notification> toRemove = form.Notifications.Where(x => !x.IsDoubleOptIn && !list.Keys.Select(y => y.Notification_Seq_ID).Contains(x.Notification_Seq_ID)).ToList();
                foreach(var n in toRemove)
                {
                    NM.Remove(n);
                    if (n.ConditionGroup_Seq_ID.HasValue)
                    {
                        DB.ConditionGroupDbManager.RemoveMain(n.ConditionGroup);
                    }
                }
                NM.SaveChanges();

                //edit
                List<Notification> existing = form.Notifications.Where(x => !x.IsDoubleOptIn && list.Keys.Select(y => y.Notification_Seq_ID).Contains(x.Notification_Seq_ID)).ToList();
                foreach(var n in existing)
                {
                    Notification edited = list.Keys.Single(x => x.Notification_Seq_ID == n.Notification_Seq_ID);
                    if (list[edited] == null)
                    {
                        edited.ConditionGroup_Seq_ID = null;
                        if (n.ConditionGroup_Seq_ID.HasValue)
                        {
                            DB.ConditionGroupDbManager.RemoveMain(n.ConditionGroup_Seq_ID.Value);
                        }
                    }
                    else
                    {
                        int groupID = DB.ConditionGroupDbManager.ResetLogicMethodByGroupID(n.ConditionGroup_Seq_ID, (bool)list[edited][0]);
                        edited.ConditionGroup_Seq_ID = groupID;
                        DB.ConditionDbManager.RewriteAll(groupID, (IEnumerable<Condition>)list[edited][1], false);
                    }
                    list.Remove(edited);
                }

                //add
                foreach (var item in list)
                {
                    if (item.Value != null)
                    {
                        int groupId = DB.ConditionGroupDbManager.AddNew((bool)item.Value[0]);
                        DB.ConditionDbManager.RewriteAll(groupId, (IEnumerable<Condition>)item.Value[1], true);
                        item.Key.ConditionGroup_Seq_ID = groupId;
                    }
                    NM.Add(item.Key);
                }

                //commit
                NM.SaveChanges();
                DB.ConditionDbManager.SaveChanges();
                DB.ConditionGroupDbManager.SaveChanges();
                t.Complete();
            }
        }

        public void Save(FormDoubleOptInPostModel model)
        {
            Notification doubleOptIn = NM.GetDoubleOptInByFormID(model.Id);
            doubleOptIn.FromName = model.Notification.FromName;
            doubleOptIn.Subject = model.Notification.SubjectLine;
            doubleOptIn.Message = model.Notification.Message;
            doubleOptIn.LandingPage = model.Page;
            
            NM.SaveChanges();
        }

        private void InitializeNotification(ConditionalNotificationModel model, int formId, Dictionary<Notification, object[]> list)
        {
            Notification n = null;
            if (model.Id == 0)
            {
                n = new Notification();
                n.Form_Seq_ID = formId;
            }
            else
            {
                n = NM.GetByID(model.Id);
            }
            FillNotification(n, model);
            if (model.Conditions == null || model.Conditions.Count() == 0)
            {
                list.Add(n, null);
            }
            else
            {
                list.Add(n, new object[] { model.ConditionType == ConditionType.And, GetDbConditions(model.Conditions) });
            }
        }

        private IEnumerable<Condition> GetDbConditions(IEnumerable<ConditionModel> conditions)
        {
            foreach (var c in conditions)
            {
                Condition db = new Condition();
                FillCondition(db, c);

                yield return db;
            }
        }

        private void FillNotification(Notification n, NotificationModel model)
        {
            n.FromName = model.FromName;
            n.Message = model.Message ?? string.Empty;
            n.Subject = model.SubjectLine;
            n.IsInternalUser = model is InternalUserNotificationModel;
            n.ToEmail = null;
            if (n.IsInternalUser)
            {
                n.ToEmail = ((InternalUserNotificationModel)model).ToEmail;
            }
        }
    }
}