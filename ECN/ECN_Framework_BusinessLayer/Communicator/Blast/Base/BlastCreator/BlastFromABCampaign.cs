using System;
using ECN_Framework_Common.Objects.Communicator;
using BlastABEntity = ECN_Framework_Entities.Communicator.BlastAB;
using BlastABMasterEntity = ECN_Framework_Entities.Communicator.BlastABMaster;
using BlastAbstractEntity = ECN_Framework_Entities.Communicator.BlastAbstract;
using BlastFieldsEntity = ECN_Framework_Entities.Communicator.BlastFields;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using UserEntity = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator.BlastCreator
{
    public class BlastFromABCampaign : BlastFromCampaignAbstract
    {
        private const int BlastAPosition = 0;
        private const int BlastBPosition = 1;
        private const string TestBlastType = "N";

        public override void CreateBlastsFromCampaignItem(CampaignItemEntity item, UserEntity user, bool keepPaused)
        {
            BlastAbstractEntity blastA = null;
            BlastAbstractEntity blastB = null;
            BlastFieldsEntity fieldsA = null;
            BlastFieldsEntity fieldsB = null;

            if (item.BlastList.Count > 0 && item.BlastList[0].BlastID != null)
            {
                blastA = GetBlast(item.BlastList[0].BlastID.Value, user);
                blastA.UpdatedUserID = user.UserID;
                fieldsA = GetFields(item.BlastList[0].BlastID.Value, user);
            }
            else
            {
                blastA = new BlastABEntity
                {
                    CreatedUserID = user.UserID,
                    CustomerID = user.CustomerID
                };
            }

            var suppressionGroups = string.Empty;
            MapBlastObjects(item, blastA, suppressionGroups, BlastAPosition);
            suppressionGroups = SetSuppressionGroups(blastA, item);
            SetRefBlast(blastA, item.BlastList[0].RefBlastList);
            var refBlasts = string.Empty;
            SetBlastField(item, blastA, fieldsA);


            if (item.BlastList.Count > 1 && item.BlastList[1].BlastID != null)
            {
                blastB = GetBlast(item.BlastList[1].BlastID.Value, user);
                blastB.UpdatedUserID = user.UserID;
                fieldsB = GetFields(item.BlastList[1].BlastID.Value, user);
            }
            else
            {
                blastB = new BlastABEntity
                {
                    CreatedUserID = user.UserID,
                    CustomerID = user.CustomerID
                };
            }
            MapBlastObjects(item, blastB, suppressionGroups, BlastBPosition);
            SetRefBlast(blastB, item.BlastList[1].RefBlastList);
            SetBlastField(item, blastB, fieldsB);

            var abMaster = new BlastABMasterEntity
            {
                BlastA = blastA,
                BlastB = blastB
            };

            ABMasterSave(abMaster, user);

            CampaignItemBlast.UpdateBlastID(item.BlastList[0].CampaignItemBlastID, blastA.BlastID, user.UserID);
            CampaignItemBlast.UpdateBlastID(item.BlastList[1].CampaignItemBlastID, blastB.BlastID, user.UserID);
        }

        private static void MapBlastObjects(CampaignItemEntity campaign, BlastAbstractEntity blast, string suppressionGroups, int position)
        {
            if (campaign == null)
            {
                throw new ArgumentNullException(nameof(campaign));
            }

            if (blast == null)
            {
                throw new ArgumentNullException(nameof(blast));
            }

            blast.StatusCode = Enums.BlastStatusCode.Pending.ToString();
            blast.BlastType = Enums.BlastType.Sample.ToString();
            blast.TestBlast = TestBlastType;
            blast.SampleID = campaign.SampleID;
            blast.SendTime = campaign.SendTime;
            blast.BlastScheduleID = campaign.BlastScheduleID;
            blast.OverrideAmount = campaign.OverrideAmount / 2;
            blast.OverrideIsAmount = campaign.OverrideIsAmount;
            blast.IgnoreSuppression = campaign.IgnoreSuppression;
            blast.EmailSubject = campaign.BlastList[position].EmailSubject;
            blast.EmailFrom = campaign.BlastList[position].EmailFrom;
            blast.EmailFromName = campaign.BlastList[position].FromName;
            blast.ReplyTo = campaign.BlastList[position].ReplyTo;
            blast.LayoutID = campaign.BlastList[position].LayoutID;
            blast.GroupID = campaign.BlastList[position].GroupID;
            blast.AddOptOuts_to_MS = campaign.BlastList[position].AddOptOuts_to_MS;
            blast.DynamicFromEmail = campaign.BlastList[position].DynamicFromEmail;
            blast.DynamicFromName = campaign.BlastList[position].DynamicFromName;
            blast.DynamicReplyToEmail = campaign.BlastList[position].DynamicReplyTo;

            if (!string.IsNullOrWhiteSpace(suppressionGroups))
            {
                blast.BlastSuppression = suppressionGroups;
            }
        }
    }
}