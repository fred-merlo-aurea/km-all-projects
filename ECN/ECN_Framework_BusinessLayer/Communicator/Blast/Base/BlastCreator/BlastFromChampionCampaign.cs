using ECN_Framework_Common.Objects.Communicator;
using BlastAbstractEntity = ECN_Framework_Entities.Communicator.BlastAbstract;
using BlastFieldsEntity = ECN_Framework_Entities.Communicator.BlastFields;
using BlastChampionEntity = ECN_Framework_Entities.Communicator.BlastChampion;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using UserEntity = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator.BlastCreator
{
    public class BlastFromChampionCampaign : BlastFromCampaignAbstract
    {
        public override void CreateBlastsFromCampaignItem(CampaignItemEntity item, UserEntity user, bool keepPaused)
        {
            BlastChampion champ = null;
            BlastAbstractEntity blast = null;
            BlastFieldsEntity fields = null;

            if (item.BlastList[0].BlastID != null)
            {
                blast = GetBlast(item.BlastList[0].BlastID.Value, user);
                blast.UpdatedUserID = user.UserID;
                fields = GetFields(item.BlastList[0].BlastID.Value, user);
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
            blast.StatusCode = Enums.BlastStatusCode.Pending.ToString();
            blast.BlastType = Enums.BlastType.Champion.ToString();

            blast.EmailFrom = item.FromEmail;
            blast.EmailFromName = item.FromName;
            blast.ReplyTo = item.ReplyTo;
            blast.SendTime = item.SendTime;
            var sample = GetBySampleID(item.SampleID.Value, user);
            blast.SampleID = sample.SampleID;
            blast.BlastScheduleID = item.BlastScheduleID;
            blast.OverrideAmount = item.OverrideAmount;
            blast.OverrideIsAmount = item.OverrideIsAmount;
            blast.GroupID = item.BlastList[0].GroupID;
            blast.AddOptOuts_to_MS = blast.AddOptOuts_to_MS;
            blast.DynamicFromEmail = item.BlastList[0].DynamicFromEmail;
            blast.DynamicFromName = item.BlastList[0].DynamicFromName;
            blast.DynamicReplyToEmail = item.BlastList[0].DynamicReplyTo;
            blast.IgnoreSuppression = item.IgnoreSuppression;

            SetSuppressionGroups(blast, item);
            SetRefBlast(blast, item.BlastList[0].RefBlastList);
            SetBlastField(item, blast, fields);

            champ = new BlastChampion();
            var newBlastID = Save(champ, blast, user);
            CampaignItemBlast.UpdateBlastID(item.BlastList[0].CampaignItemBlastID, newBlastID, user.UserID);
        }
    }
}