using System;
using System.Collections.Generic;
using ECN_Framework_Common.Objects.Communicator;
using BlastABMasterEntity = ECN_Framework_Entities.Communicator.BlastABMaster;
using BlastAbstractEntity = ECN_Framework_Entities.Communicator.BlastAbstract;
using BlastFieldsEntity = ECN_Framework_Entities.Communicator.BlastFields;
using CampaignItemBlastRefBlastEntity = ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using SampleEntity = ECN_Framework_Entities.Communicator.Sample;
using UserEntity = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator.BlastCreator
{
    public abstract class BlastFromCampaignAbstract
    {
        public static void SetBlastField(CampaignItemEntity campaign, BlastAbstractEntity blast, BlastFieldsEntity fields)
        {
            if (campaign == null)
            {
                throw new ArgumentNullException(nameof(campaign));
            }
            if (blast == null)
            {
                throw new ArgumentNullException(nameof(blast));
            }

            if (
                campaign.BlastField1.Trim().Length > 0
                || campaign.BlastField2.Trim().Length > 0
                || campaign.BlastField3.Trim().Length > 0
                || campaign.BlastField4.Trim().Length > 0
                || campaign.BlastField5.Trim().Length > 0)
            {
                if (fields == null)
                {
                    fields = new BlastFieldsEntity();
                }

                fields.Field1 = campaign.BlastField1;
                fields.Field2 = campaign.BlastField2;
                fields.Field3 = campaign.BlastField3;
                fields.Field4 = campaign.BlastField4;
                fields.Field5 = campaign.BlastField5;
                blast.Fields = fields;
            }
        }

        public static string SetSuppressionGroups(BlastAbstractEntity blast, CampaignItemEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (blast == null)
            {
                throw new ArgumentNullException(nameof(blast));
            }

            var suppressionGroups = string.Empty;

            foreach (var suppression in item.SuppressionList)
            {
                suppressionGroups = $"{suppressionGroups}{suppression.GroupID},";
            }

            if (suppressionGroups.Length > 0)
            {
                suppressionGroups = suppressionGroups.TrimEnd(',');
            }

            blast.BlastSuppression = suppressionGroups;

            return suppressionGroups;
        }

        public static void SetRefBlast(BlastAbstractEntity blast, List<CampaignItemBlastRefBlastEntity> refBlastList)
        {
            if (refBlastList == null)
            {
                throw new ArgumentNullException(nameof(refBlastList));
            }
            if (blast == null)
            {
                throw new ArgumentNullException(nameof(blast));
            }

            var refBlasts = string.Empty;

            foreach (var ciRefBlast in refBlastList)
            {
                refBlasts = $"{refBlasts}{ciRefBlast.RefBlastID},";
            }
            if (refBlasts.Length > 0)
            {
                refBlasts = refBlasts.TrimEnd(',');
            }

            blast.RefBlastID = refBlasts;
        }

        public CreateBlastTypeEnum BlastType { get; set; }

        public virtual string GetStatus(bool keepPaused)
        {
            return Enums.BlastStatusCode.Pending.ToString();
        }

        public int GetCustomerId(CampaignItemEntity item, UserEntity user)
        {
            if (BlastType == CreateBlastTypeEnum.None)
            {
                if (this is BlastFromRegularSaleforceCampaign)
                {
                    return user.CustomerID;
                }
            }
            else if (BlastType == CreateBlastTypeEnum.NoAccessCheck)
            {
                if (this is BlastFromRegularSaleforceCampaign)
                {
                    return user.CustomerID;
                }
            }
            else if (BlastType == CreateBlastTypeEnum.AmbientTransaction)
            {
                if (this is BlastFromRegularSaleforceCampaign)
                {
                    return item.CustomerID.Value;
                }
            }

            return -1;
        }

        public void ABMasterSave(BlastABMasterEntity abMaster, UserEntity user)
        {
            if (BlastType == CreateBlastTypeEnum.None || BlastType == CreateBlastTypeEnum.AmbientTransaction)
            {
                BlastABMaster.Save(abMaster, user);
            }
            else if (BlastType == CreateBlastTypeEnum.NoAccessCheck)
            {
                BlastABMaster.Save_NoAccessCheck(abMaster, user);
            }
        }

        public int Save(BlastAbstract blastAbstract, BlastAbstractEntity blast, UserEntity user)
        {
            if (BlastType == CreateBlastTypeEnum.None || BlastType == CreateBlastTypeEnum.AmbientTransaction)
            {
                return blastAbstract.Save(blast, user);
            }

            return blastAbstract.Save_NoAccessCheck(blast, user);
        }

        public SampleEntity GetBySampleID(int sampleID, UserEntity user)
        {
            SampleEntity sample = null;
            if (BlastType == CreateBlastTypeEnum.None)
            {
                sample = Sample.GetBySampleID(sampleID, user);
            }
            else if (BlastType == CreateBlastTypeEnum.NoAccessCheck)
            {
                sample = Sample.GetBySampleID_NoAccessCheck(sampleID, user);
            }
            else if (BlastType == CreateBlastTypeEnum.AmbientTransaction)
            {
                sample = Sample.GetBySampleID_UseAmbientTransaction(sampleID, user);
            }

            return sample;
        }

        public BlastAbstractEntity GetBlast(int blastId, UserEntity user)
        {
            BlastAbstractEntity blast = null;
            if (BlastType == CreateBlastTypeEnum.None)
            {
                if (this is BlastFromRegularSaleforceCampaign
                )
                {
                    blast = Blast.GetByBlastID_NoAccessCheck(blastId, true);
                }
                else
                {
                    blast = Blast.GetByBlastID(blastId, user, true);
                }
            }
            else if (BlastType == CreateBlastTypeEnum.NoAccessCheck)
            {
                blast = Blast.GetByBlastID_NoAccessCheck(blastId, true);
            }
            else if (BlastType == CreateBlastTypeEnum.AmbientTransaction)
            {
                if (this is BlastFromRegularSaleforceCampaign)
                {
                    blast = Blast.GetByBlastID_NoAccessCheck_UseAmbientTransaction(blastId, true);
                }
                else
                {
                    blast = Blast.GetByBlastID_UseAmbientTransaction(blastId, user, true);
                }
            }

            return blast;
        }

        public BlastFieldsEntity GetFields(int blastId, UserEntity user)
        {
            BlastFieldsEntity fields = null;
            if (BlastType == CreateBlastTypeEnum.None)
            {
                if (this is BlastFromRegularSaleforceCampaign)
                {
                    fields = BlastFields.GetByBlastID_NoAccessCheck(blastId);
                }
                else
                {
                    fields = BlastFields.GetByBlastID(blastId, user);
                }
            }
            else if (BlastType == CreateBlastTypeEnum.NoAccessCheck)
            {
                fields = BlastFields.GetByBlastID_NoAccessCheck(blastId);
            }
            else if (BlastType == CreateBlastTypeEnum.AmbientTransaction)
            {
                if (this is BlastFromRegularSaleforceCampaign)
                {
                    fields = BlastFields.GetByBlastID_NoAccessCheck_UseAmbientTransaction(blastId);
                }
                else
                {
                    fields = BlastFields.GetByBlastID_UseAmbientTransaction(blastId, user);
                }
            }

            return fields;
        }

        public abstract void CreateBlastsFromCampaignItem(CampaignItemEntity item, UserEntity user, bool keepPaused);
    }
}