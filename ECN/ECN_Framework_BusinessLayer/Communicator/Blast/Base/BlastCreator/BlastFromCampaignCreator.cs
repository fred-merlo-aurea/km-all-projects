using ECN_Framework_Common.Objects.Communicator;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using UserEntity = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator.BlastCreator
{
    public class BlastFromCampaignCreator
    {
        public static void CreateBlastsFromCampaignItem(int campaignItemId, UserEntity user)
        {
            var item = CampaignItem.GetByCampaignItemID_NoAccessCheck(campaignItemId, true);

            var blastCreator = CreateBlastCreator(item);
            if (blastCreator != null)
            {
                blastCreator.BlastType = CreateBlastTypeEnum.None;
                var keepPaused = false;
                blastCreator.CreateBlastsFromCampaignItem(item, user, keepPaused);
            }
        }

        public static void CreateBlastsFromCampaignItem_NoAccessCheck(int campaignItemId, UserEntity user)
        {
            var item = CampaignItem.GetByCampaignItemID_NoAccessCheck(campaignItemId, true);

            var blastCreator = CreateBlastCreator(item);
            if (blastCreator != null)
            {
                if (item.CampaignItemType == Enums.CampaignItemType.Personalization.ToString())
                {
                    return;
                }

                blastCreator.BlastType = CreateBlastTypeEnum.NoAccessCheck;
                var keepPaused = false;
                blastCreator.CreateBlastsFromCampaignItem(item, user, keepPaused);
            }
        }

        public static void CreateBlastsFromCampaignItem_UseAmbientTransaction(int campaignItemId, UserEntity user, bool keepPaused)
        {
            var item = CampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(campaignItemId, true);

            var blastCreator = CreateBlastCreator(item);

            if (blastCreator != null)
            {
                if (item.CampaignItemType == Enums.CampaignItemType.Personalization.ToString())
                {
                    return;
                }

                blastCreator.BlastType = CreateBlastTypeEnum.AmbientTransaction;
                blastCreator.CreateBlastsFromCampaignItem(item, user, keepPaused);
            }
        }

        private static BlastFromCampaignAbstract CreateBlastCreator(CampaignItemEntity item)
        {
            BlastFromCampaignAbstract blastCreator = null;
            if (item != null)
            {
                if (item.CampaignItemType == Enums.CampaignItemType.Regular.ToString() ||
                    item.CampaignItemType == Enums.CampaignItemType.Salesforce.ToString())
                {
                    blastCreator = new BlastFromRegularSaleforceCampaign();
                }
                else if (item.CampaignItemType == Enums.CampaignItemType.Champion.ToString())
                {
                    blastCreator = new BlastFromChampionCampaign();
                }
                else if (item.CampaignItemType == Enums.CampaignItemType.AB.ToString())
                {
                    blastCreator = new BlastFromABCampaign();
                }
                else if (item.CampaignItemType == Enums.CampaignItemType.SMS.ToString())
                {
                    blastCreator = new BlastFromSMSCampaign();
                }
                else if (item.CampaignItemType == Enums.CampaignItemType.Layout.ToString())
                {
                    blastCreator = new BlastFromLayoutCampaign();
                }
                else if (item.CampaignItemType == Enums.CampaignItemType.NoOpen.ToString())
                {
                    blastCreator = new BlastFromNoOpenCampaign();
                }
                else if (item.CampaignItemType == Enums.CampaignItemType.Personalization.ToString())
                {
                    blastCreator = new BlastFromPersonalizationCampaign();
                }
            }

            return blastCreator;
        }
    }
}