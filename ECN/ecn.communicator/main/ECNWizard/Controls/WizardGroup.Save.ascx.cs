using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using KM.Common;
using KMPlatform.Entity;
using CampaignItemBlast = ECN_Framework_Entities.Communicator.CampaignItemBlast;
using CampaignItemSuppression = ECN_Framework_Entities.Communicator.CampaignItemSuppression;
using BusinessCampaignItemSuppression = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression;
using BusinessCampaignItemBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast;
using EntityCampaignItem = ECN_Framework_Entities.Communicator.CampaignItem;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardGroup
    {
        private const int Step2 = 2;
        private const int MaxBlastsRefNumber = 5;
        private const char Comma = ',';
        private const string ABTestToken = "ab";
        private static readonly TimeSpan TenMinutes = new TimeSpan(0, 10, 0);

        public bool Save()
        {
            var user = ECNSession.CurrentSession().CurrentUser;
            var campaignItem = CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            var isAbCampaignItem = campaignItem.CampaignItemType.Equals(ABTestToken, StringComparison.InvariantCultureIgnoreCase);

            var listGroups = groupExplorer1.getSelectedGroups();
            if (!CheckGroupsSelected(listGroups) ||
                !CheckOneGroupForAbBlast(listGroups, isAbCampaignItem))
            {
                return false;
            }

            var suppressionGroupsDt = groupExplorer1.getSuppressionGroups();
            var ciBlastList = new List<CampaignItemBlast>();
            if (!BuildBlastList(listGroups, campaignItem, user, ciBlastList, isAbCampaignItem))
            {
                return false;
            }

            var ciSuppressionList = BuildSuppressionList(suppressionGroupsDt, user);
            campaignItem.UpdatedUserID = user.UserID;
            campaignItem.CompletedStep = campaignItem.CompletedStep > Step2 ? campaignItem.CompletedStep : Step2;
            ProcessSave(campaignItem, user, ciBlastList, ciSuppressionList);
            return true;
        }

        private void ProcessSave(
            EntityCampaignItem campaignItem,
            User user,
            List<CampaignItemBlast> campaignItemBlasts,
            List<CampaignItemSuppression> campaignItemSuppressions)
        {
            Guard.NotNull(campaignItem, nameof(campaignItem));
            Guard.NotNull(user, nameof(user));
            Guard.NotNull(campaignItemBlasts, nameof(campaignItemBlasts));
            Guard.NotNull(campaignItemSuppressions, nameof(campaignItemSuppressions));

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TenMinutes))
            {
                foreach (var ciBlast in campaignItem.BlastList)
                {
                    CampaignItemBlastRefBlast.Delete(ciBlast.CampaignItemBlastID, user);
                }

                CampaignItem.Save(campaignItem, user);
                BusinessCampaignItemBlast.Save(CampaignItemID, campaignItemBlasts, user);
                BusinessCampaignItemSuppression.Delete_NoAccessCheck(CampaignItemID, user);

                foreach (var ciSuppression in campaignItemSuppressions)
                {
                    BusinessCampaignItemSuppression.Save(ciSuppression, user);
                }

                Blast.CreateBlastsFromCampaignItem(campaignItem.CampaignItemID, user, true);
                scope.Complete();
            }
        }

        private List<CampaignItemSuppression> BuildSuppressionList(IEnumerable<Group.GroupObject> suppressionGroups, User user)
        {
            return suppressionGroups.Select(groupObject => new CampaignItemSuppression
                {
                    GroupID = groupObject.GroupID,
                    CampaignItemID = CampaignItemID,
                    CreatedUserID = user.UserID,
                    UpdatedUserID = user.UserID,
                    CustomerID = user.CustomerID,
                    Filters = groupObject.filters
                })
                .ToList();
        }

        private bool BuildBlastList(
            IEnumerable<Group.GroupObject> listGroups,
            EntityCampaignItem campaignItem,
            User user,
            ICollection<CampaignItemBlast> ciBlastList,
            bool isAbCampaignItem)
        {
            foreach (var groupObject in listGroups)
            {
                if (!BuildBlastsForGroup(campaignItem, user, ciBlastList, isAbCampaignItem, groupObject))
                {
                    return false;
                }
            }

            return true;
        }

        private bool BuildBlastsForGroup(
            EntityCampaignItem campaignItem,
            User user,
            ICollection<CampaignItemBlast> campaignItemBlasts,
            bool isAbCampaignItem,
            Group.GroupObject groupObject)
        {
            var blast = new CampaignItemBlast
            {
                CampaignItemID = CampaignItemID,
                GroupID = groupObject.GroupID
            };

            if (!FillMainBlast(campaignItem, user, groupObject, blast))
            {
                return false;
            }
            campaignItemBlasts.Add(blast);

            if (!isAbCampaignItem)
            {
                return true;
            }

            var additionalAbBlast = new CampaignItemBlast
            {
                CampaignItemID = CampaignItemID,
                GroupID = groupObject.GroupID,
                Filters = groupObject.filters
            };

            FillAdditionalAbBlast(campaignItem, user, additionalAbBlast);
            campaignItemBlasts.Add(additionalAbBlast);
            return true;
        }

        private static void FillAdditionalAbBlast(EntityCampaignItem campaignItem, User user, CampaignItemBlast additionalBlast)
        {
            if (campaignItem.BlastList.Count == Step2)
            {
                var blastOld = campaignItem.BlastList[1];
                additionalBlast.LayoutID = blastOld.LayoutID;
                additionalBlast.EmailSubject = blastOld.EmailSubject;
                additionalBlast.DynamicFromName = blastOld.DynamicFromName;
                additionalBlast.DynamicFromEmail = blastOld.DynamicFromEmail;
                additionalBlast.DynamicReplyTo = blastOld.DynamicReplyTo;
                additionalBlast.AddOptOuts_to_MS = blastOld.AddOptOuts_to_MS;
                additionalBlast.UpdatedUserID = user.UserID;
                additionalBlast.CreatedUserID = blastOld.CreatedUserID;
                additionalBlast.EmailFrom = blastOld.EmailFrom;
                additionalBlast.ReplyTo = blastOld.ReplyTo;
                additionalBlast.FromName = blastOld.FromName;
            }
            else
            {
                additionalBlast.CreatedUserID = user.UserID;
            }

            additionalBlast.CustomerID = user.CustomerID;
        }

        private bool FillMainBlast(EntityCampaignItem campaignItem, User user, Group.GroupObject groupObject, CampaignItemBlast blast)
        {
            var prePopLayoutId = getPrePopLayoutID();
            if (prePopLayoutId > 0)
            {
                blast.LayoutID = prePopLayoutId;
            }

            blast.Filters = groupObject.filters;
            if (!CheckSmartSegmentFilters(groupObject))
            {
                return false;
            }

            if (campaignItem.BlastList.Count > 0)
            {
                var ciBlastOld = campaignItem.BlastList[0];
                blast.LayoutID = ciBlastOld.LayoutID;
                blast.EmailSubject = ciBlastOld.EmailSubject;
                blast.DynamicFromName = ciBlastOld.DynamicFromName;
                blast.DynamicFromEmail = ciBlastOld.DynamicFromEmail;
                blast.DynamicReplyTo = ciBlastOld.DynamicReplyTo;
                blast.AddOptOuts_to_MS = ciBlastOld.AddOptOuts_to_MS;
                blast.UpdatedUserID = user.UserID;
                blast.CreatedUserID = ciBlastOld.CreatedUserID;
                blast.EmailFrom = ciBlastOld.EmailFrom;
                blast.ReplyTo = ciBlastOld.ReplyTo;
                blast.FromName = ciBlastOld.FromName;
            }
            else
            {
                blast.CreatedUserID = user.UserID;
            }

            blast.CustomerID = user.CustomerID;
            return true;
        }

        private bool CheckSmartSegmentFilters(Group.GroupObject groupObject)
        {
            foreach (var refBlasts in groupObject.filters.Where(f => f.SmartSegmentID != null).Select(cibf => cibf.RefBlastIDs.Split(Comma)))
            {
                if (refBlasts.Length > MaxBlastsRefNumber)
                {
                    throwECNException("You cannot have more than 5 Ref Blasts for Smart Segment Filter");
                    return false;
                }

                if (refBlasts.Length == 0)
                {
                    throwECNException("Ref Blast not selected for SmartSegment");
                    return false;
                }
            }

            return true;
        }

        private bool CheckOneGroupForAbBlast(List<Group.GroupObject> listGroups, bool isAbCampaignItem)
        {
            if (listGroups.Count > 1 && isAbCampaignItem)
            {
                throwECNException("Multiple Group Selections are not supported for A/B Blast");
                return false;
            }

            return true;
        }

        private bool CheckGroupsSelected(List<Group.GroupObject> listGroups)
        {
            if (listGroups.Count == 0)
            {
                throwECNException("Group not selected");
                return false;
            }

            return true;
        }
    }
}