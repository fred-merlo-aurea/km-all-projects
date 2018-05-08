using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ECN_Framework_BusinessLayer.Communicator;
using Entities = ECN_Framework_Entities.Communicator;
using EntitiesFakes = ECN_Framework_Entities.Communicator.Fakes;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using KMPlatform.Entity;
using ECN.TestHelpers;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class BlastTest
    {
        private const string MethodCreateBlastsFromCampaignItem_NoAccessCheck =
            "CreateBlastsFromCampaignItem_NoAccessCheck";

        [Test]
        public void CreateBlastsFromCampaignItem_NoAccessCheck_ItemIsNull_ReachEnd()
        {
            // Arrange
            var campaignItemID = 1;
            var user = new User();
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, child) => null;

            // Act, Assert
            CallCreateBlastsFromCampaignItem_NoAccessCheck(new object[]
            {
                campaignItemID,
                user
            });
        }

        [Test]
        [TestCase(CampaignItemType.Regular, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.Salesforce, null, CampaignItemFormatType.TEXT)]
        [TestCase(CampaignItemType.Champion, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.Champion, null, CampaignItemFormatType.TEXT)]
        [TestCase(CampaignItemType.AB, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.AB, null, CampaignItemFormatType.TEXT)]
        [TestCase(CampaignItemType.SMS, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.SMS, null, CampaignItemFormatType.TEXT)]
        [TestCase(CampaignItemType.Layout, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.Layout, null, CampaignItemFormatType.TEXT)]
        [TestCase(CampaignItemType.NoOpen, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.NoOpen, null, CampaignItemFormatType.TEXT)]
        public void CreateBlastsFromCampaignItem_NoAccessCheck_MultipleTypes_UpdateBlastID(
            CampaignItemType type,
            int? blastId,
            CampaignItemFormatType formatType)
        {
            // Arrange
            var campaignItemID = 1;
            var user = new User
            {
                UserID = 1
            };
            var itemUpdated = false;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean =
                (id, child) => GetCampaignItemForNoAccessCheck(type, blastId, formatType);
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) => new Entities::BlastRegular();
            ShimBlast.GetByBlastID_UseAmbientTransactionInt32UserBoolean = 
                (id, usr, child) => new Entities::BlastRegular();
            ShimBlastFields.GetByBlastID_NoAccessCheckInt32 = (id) => null;
            ShimBlastExtendedAbstract.AllInstances.Save_NoAccessCheckBlastAbstractUser = (obj, blast, usr) => 1;
            ShimSample.GetBySampleID_NoAccessCheckInt32User = (id, usr) => new Entities::Sample
            {
                SampleID = 1
            };
            ShimBlastStandardAbstract.AllInstances.Save_NoAccessCheckBlastAbstractUser = (obj, blast, usr) => 1;
            ShimCampaignItemBlast.UpdateBlastIDInt32Int32Int32 = (id, newId, userId) =>
            {
                itemUpdated = true;
            };
            ShimBlastABMaster.Save_NoAccessCheckBlastABMasterUser = (blast, usr) => { };
            ShimBlastSMS.AllInstances.Save_NoAccessCheckBlastAbstractUser = (obj, blast, usr) => 1;
            ShimBlastStandardAbstract.AllInstances.Save_NoAccessCheckBlastAbstractUser = (obj, blast, usr) => 1;

            // Act
            CallCreateBlastsFromCampaignItem_NoAccessCheck(new object[] {
                campaignItemID,
                user});

            // Assert 
            itemUpdated.ShouldBeTrue();
        }

        private Entities::CampaignItem GetCampaignItemForNoAccessCheck(CampaignItemType type, int? blastId,
            CampaignItemFormatType formatType)
        {
            return new Entities::CampaignItem
            {
                CampaignItemType = type.ToString(),
                BlastList = GetBlastList(blastId),
                SuppressionList = new List<Entities::CampaignItemSuppression>
                {
                    new Entities::CampaignItemSuppression
                    {
                        GroupID = 1
                    }
                },
                BlastField1 = "TestString",
                BlastField2 = "TestString",
                BlastField3 = "TestString",
                BlastField4 = "TestString",
                BlastField5 = "TestString",
                CampaignItemFormatType = formatType.ToString(),
                CustomerID = 1,
                SampleID = 1
            };
        }

        private void CallCreateBlastsFromCampaignItem_NoAccessCheck(object[] parametersValues)
        {
            MethodInfo methodInfo = typeof(Blast).GetAllMethods()
                .FirstOrDefault(x => x.Name == MethodCreateBlastsFromCampaignItem_NoAccessCheck
                                     && x.IsPrivate);
            if (methodInfo != null)
            {
                methodInfo.Invoke(null, parametersValues);
            }
        }
    }
}
