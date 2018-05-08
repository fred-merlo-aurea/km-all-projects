using ECN_Framework_Common.Objects.Communicator;
using BlastAbstractEntity = ECN_Framework_Entities.Communicator.BlastAbstract;
using BlastChampionEntity = ECN_Framework_Entities.Communicator.BlastChampion;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using UserEntity = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator.BlastCreator
{
    public class BlastFromLayoutCampaign : BlastFromCampaignAbstract
    {
        public override void CreateBlastsFromCampaignItem(CampaignItemEntity item, UserEntity user, bool keepPaused)
        {
            BlastLayout layout = null;
            BlastAbstractEntity blast = null;

            if (item.BlastList[0].BlastID != null)
            {
                blast = GetBlast(item.BlastList[0].BlastID.Value, user);
                blast.UpdatedUserID = user.UserID;
            }
            else
            {
                blast = new BlastChampionEntity
                {
                    CreatedUserID = user.UserID,
                    CustomerID = user.CustomerID
                };
            }

            blast.TestBlast = "N";
            blast.StatusCode = Enums.BlastStatusCode.System.ToString();
            blast.BlastType = Enums.BlastType.Layout.ToString();
            blast.EmailSubject = item.BlastList[0].EmailSubject;
            blast.LayoutID = item.BlastList[0].LayoutID;
            blast.EmailFrom = item.FromEmail;
            blast.EmailFromName = item.FromName;
            blast.ReplyTo = item.ReplyTo;
            blast.SendTime = item.SendTime;
            blast.GroupID = item.BlastList[0].GroupID;
            blast.IgnoreSuppression = item.IgnoreSuppression;

            layout = new BlastLayout();
            var newBlastID = Save(layout, blast, user);
            CampaignItemBlast.UpdateBlastID(item.BlastList[0].CampaignItemBlastID, newBlastID, user.UserID);
        }
    }
}