using ECN_Framework_Common.Objects.Communicator;
using BlastAbstractEntity = ECN_Framework_Entities.Communicator.BlastAbstract;
using BlastRegularEntity = ECN_Framework_Entities.Communicator.BlastRegular;
using BlastFieldsEntity = ECN_Framework_Entities.Communicator.BlastFields;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using UserEntity = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator.BlastCreator
{
    public class BlastFromPersonalizationCampaign : BlastFromCampaignAbstract
    {
        public BlastFromPersonalizationCampaign()
        {
            BlastType = CreateBlastTypeEnum.None;
        }

        public override void CreateBlastsFromCampaignItem(CampaignItemEntity item, UserEntity user, bool keepPaused)
        {
            BlastFieldsEntity fields = null;

            foreach (var ciBlast in item.BlastList)
            {
                BlastPersonalization reg = null;
                BlastAbstractEntity blast = null;

                if (ciBlast.BlastID != null)
                {
                    blast = GetBlast(ciBlast.BlastID.Value, user);
                    blast.UpdatedUserID = user.UserID;
                    fields = BlastFields.GetByBlastID_NoAccessCheck(ciBlast.BlastID.Value);
                }
                else
                {
                    blast = new BlastRegularEntity
                    {
                        CreatedUserID = user.UserID,
                        CustomerID = user.CustomerID
                    };
                }

                blast.StatusCode = Enums.BlastStatusCode.PendingContent.ToString();

                blast.EmailSubject = ciBlast.EmailSubject;
                blast.EmailFrom = item.FromEmail;
                blast.EmailFromName = item.FromName;
                blast.ReplyTo = item.ReplyTo;
                blast.LayoutID = ciBlast.LayoutID;
                blast.TestBlast = "N";
                blast.SendTime = item.SendTime;
                blast.BlastType = Enums.BlastType.Personalization.ToString();
                blast.BlastScheduleID = item.BlastScheduleID;
                blast.OverrideAmount = item.OverrideAmount;
                blast.OverrideIsAmount = item.OverrideIsAmount;
                blast.GroupID = ciBlast.GroupID;
                blast.AddOptOuts_to_MS = ciBlast.AddOptOuts_to_MS;
                blast.DynamicFromEmail = ciBlast.DynamicFromEmail;
                blast.DynamicFromName = ciBlast.DynamicFromName;
                blast.DynamicReplyToEmail = ciBlast.DynamicReplyTo;
                blast.EnableCacheBuster = item.EnableCacheBuster;
                blast.IgnoreSuppression = item.IgnoreSuppression;

                SetSuppressionGroups(blast, item);
                SetBlastField(item, blast, fields);

                reg = new BlastPersonalization();
                var newBlastID = reg.Save(blast, user);
                CampaignItemBlast.UpdateBlastID(ciBlast.CampaignItemBlastID, newBlastID, user.UserID);
            }
        }
    }
}