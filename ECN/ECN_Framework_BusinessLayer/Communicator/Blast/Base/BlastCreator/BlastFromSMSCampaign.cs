using ECN_Framework_Common.Objects.Communicator;
using BlastAbstractEntity = ECN_Framework_Entities.Communicator.BlastAbstract;
using BlastRegularEntity = ECN_Framework_Entities.Communicator.BlastRegular;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using UserEntity = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator.BlastCreator
{
    public class BlastFromSMSCampaign : BlastFromCampaignAbstract
    {
        public override void CreateBlastsFromCampaignItem(CampaignItemEntity item, UserEntity user, bool keepPaused)
        {
            foreach (var ciBlast in item.BlastList)
            {
                BlastSMS sms = null;
                BlastAbstractEntity blast = null;

                if (ciBlast.BlastID != null)
                {
                    blast = GetBlast(ciBlast.BlastID.Value, user);
                    blast.UpdatedUserID = user.UserID;
                }
                else
                {
                    blast = new BlastRegularEntity
                    {
                        CreatedUserID = user.UserID,
                        CustomerID = user.CustomerID
                    };
                }

                blast.StatusCode = Enums.BlastStatusCode.Pending.ToString();
                blast.BlastType = Enums.BlastType.SMS.ToString();
                blast.EmailSubject = ciBlast.EmailSubject;
                blast.EmailFrom = item.FromEmail;
                blast.EmailFromName = item.FromName;
                blast.ReplyTo = item.ReplyTo;
                blast.LayoutID = ciBlast.LayoutID;
                blast.TestBlast = "N";
                blast.SendTime = item.SendTime;
                blast.BlastScheduleID = item.BlastScheduleID;
                blast.OverrideAmount = item.OverrideAmount;
                blast.OverrideIsAmount = item.OverrideIsAmount;
                blast.GroupID = ciBlast.GroupID;

                SetSuppressionGroups(blast, item);

                sms = new BlastSMS();
                var newBlastID = Save(sms, blast, user);
                CampaignItemBlast.UpdateBlastID(ciBlast.CampaignItemBlastID, newBlastID, user.UserID);
            }
        }
    }
}