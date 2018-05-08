using ECN_Framework_Common.Objects.Communicator;
using BlastAbstractEntity = ECN_Framework_Entities.Communicator.BlastAbstract;
using BlastRegularEntity = ECN_Framework_Entities.Communicator.BlastRegular;
using BlastFieldsEntity = ECN_Framework_Entities.Communicator.BlastFields;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using UserEntity = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator.BlastCreator
{
    public class BlastFromRegularSaleforceCampaign : BlastFromCampaignAbstract
    {
        public override void CreateBlastsFromCampaignItem(CampaignItemEntity item, UserEntity user, bool keepPaused)
        {
            BlastFieldsEntity fields = null;

            foreach (var ciBlast in item.BlastList)
            {
                BlastRegular reg = null;
                BlastAbstractEntity blast = null;

                if (ciBlast.BlastID != null)
                {
                    blast = GetBlast(ciBlast.BlastID.Value, user);
                    blast.UpdatedUserID = user.UserID;
                    fields = GetFields(ciBlast.BlastID.Value, user);
                }
                else
                {
                    blast = new BlastRegularEntity();
                    blast.CreatedUserID = user.UserID;
                    blast.CustomerID = GetCustomerId(item, user);
                }

                blast.StatusCode = GetStatus(keepPaused);

                if (item.CampaignItemFormatType == Enums.CampaignItemFormatType.HTML.ToString())
                {
                    blast.BlastType = Enums.BlastType.HTML.ToString();
                }
                else if (item.CampaignItemFormatType == Enums.CampaignItemFormatType.TEXT.ToString())
                {
                    blast.BlastType = Enums.BlastType.TEXT.ToString();
                }

                blast.EmailSubject = ciBlast.EmailSubject;
                blast.EmailFrom = item.FromEmail;
                blast.EmailFromName = item.FromName;
                blast.ReplyTo = item.ReplyTo;
                blast.LayoutID = ciBlast.LayoutID;
                blast.TestBlast = "N";
                blast.SendTime = item.SendTime;
                blast.BlastType = item.CampaignItemFormatType;
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

                reg = new BlastRegular();
                var newBlastID = Save(reg, blast, user);
                CampaignItemBlast.UpdateBlastID(ciBlast.CampaignItemBlastID, newBlastID, user.UserID);
            }
        }

        public override string GetStatus(bool keepPaused)
        {
            if (BlastType == CreateBlastTypeEnum.AmbientTransaction && keepPaused)
            {
                return Enums.BlastStatusCode.Paused.ToString();
            }

            return Enums.BlastStatusCode.Pending.ToString();
        }
    }
}