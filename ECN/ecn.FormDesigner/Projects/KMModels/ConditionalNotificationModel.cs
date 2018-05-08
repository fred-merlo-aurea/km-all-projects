using KMEntities;
using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels
{
    public abstract class ConditionalNotificationModel : NotificationModel
    {
        public ConditionType ConditionType { get; set; }

        public IEnumerable<ConditionModel> Conditions { get; set; }

        public int CustomerID { get; set; }

        public override void FillData(object entity)
        {
            base.FillData(entity);

            var notification = (Notification)entity;
            if (notification.ConditionGroup_Seq_ID.HasValue)
            {
                ConditionType = notification.ConditionGroup.LogicGroup ? ConditionType.And : ConditionType.Or;
                Conditions = notification.ConditionGroup.Conditions.ConvertToModels<ConditionModel>().ToList();
                ((List<ConditionModel>)Conditions).ForEach(x => x.Type = ConditionType);
            }
        }
    }
}
