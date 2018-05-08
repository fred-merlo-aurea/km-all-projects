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
        private const string _MethodCreateBlastsFromCampaignItem_UseAmbientTransaction =
            "CreateBlastsFromCampaignItem_UseAmbientTransaction";

        private void CallCreateBlastsFromCampaignItem_UseAmbientTransactionMethod(object[] parametersValues)
        {
            MethodInfo methodInfo = typeof(Blast).GetAllMethods()
                .FirstOrDefault(x => x.Name == _MethodCreateBlastsFromCampaignItem_UseAmbientTransaction
               && x.IsPrivate);
            if (methodInfo != null)
            {
                methodInfo.Invoke(null, parametersValues);
            }
        }

        [Test]
        public void CreateBlastsFromCampaignItem_UseAmbientTransaction_ItemIsNull_ReachEnd()
        {
            // Arrange
            var campaignItemID = 1;
            var user = new User();
            var keepPaused = true;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransactionInt32Boolean = (id, child) => null;

            // Act, Assert
            CallCreateBlastsFromCampaignItem_UseAmbientTransactionMethod(new object[] {
                campaignItemID,
                user,
                keepPaused });
        }

        [Test]
        [TestCase(CampaignItemType.Regular, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.Salesforce, null, CampaignItemFormatType.TEXT)]
        [TestCase(CampaignItemType.SMS, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.SMS, null, CampaignItemFormatType.TEXT)]
        [TestCase(CampaignItemType.Layout, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.Layout, null, CampaignItemFormatType.TEXT)]
        [TestCase(CampaignItemType.NoOpen, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.NoOpen, null, CampaignItemFormatType.TEXT)]
        public void CreateBlastsFromCampaignItem_UseAmbientTransaction_MultipleTypes_UpdateBlastID(
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
            var keepPaused = true;
            var itemUpdated = false;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransactionInt32Boolean = (id, child) =>
            {
                return new Entities::CampaignItem
                {
                    CampaignItemType = type.ToString(),
                    BlastList = new List<Entities::CampaignItemBlast>
                    {
                        new Entities::CampaignItemBlast
                        {
                            BlastID = blastId,
                            LayoutID = 1,
                            GroupID = 1
                        }
                    },
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
                    CustomerID = 1
                };
            };
            ShimBlast.GetByBlastID_NoAccessCheck_UseAmbientTransactionInt32Boolean = (id, child) =>
            {
                return new Entities::BlastRegular();
            };
            ShimBlast.GetByBlastID_UseAmbientTransactionInt32UserBoolean = (id, usr, child) =>
            {
                return new Entities::BlastRegular();
            };
            ShimBlastFields.GetByBlastID_NoAccessCheck_UseAmbientTransactionInt32 = (id) => null;
            ShimBlastExtendedAbstract.AllInstances.SaveBlastAbstractUser = (obj, blast, usr) => 1;
            ShimBlastSMS.AllInstances.SaveBlastAbstractUser = (obj, blast, usr) => 1;
            ShimBlastStandardAbstract.AllInstances.SaveBlastAbstractUser = (obj, blast, usr) => 1;
            ShimCampaignItemBlast.UpdateBlastIDInt32Int32Int32 = (id, newId, userId) =>
            {
                itemUpdated = true;
            };

            // Act
            CallCreateBlastsFromCampaignItem_UseAmbientTransactionMethod(new object[] {
                campaignItemID,
                user,
                keepPaused });

            // Assert 
            itemUpdated.ShouldBeTrue();
        }

        [Test]
        [TestCase(CampaignItemType.Champion, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.Champion, null, CampaignItemFormatType.TEXT)]
        [TestCase(CampaignItemType.AB, 1, CampaignItemFormatType.HTML)]
        [TestCase(CampaignItemType.AB, null, CampaignItemFormatType.TEXT)]
        public void CreateBlastsFromCampaignItem_UseAmbientTransaction_ChampionOrAB_UpdateBlastID(
            CampaignItemType type, 
            int? blastId,
            CampaignItemFormatType formatType)
        {
            // Arrange
            var campaignItemID = 1;
            var user = new User();
            var keepPaused = true;
            var itemUpdated = false;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransactionInt32Boolean = (id, child) =>
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
                    SampleID = 1,
                    CustomerID = 1
                };
            };
            ShimSample.GetBySampleID_UseAmbientTransactionInt32User = (id, usr) =>
            {
                return new Entities::Sample
                {
                    SampleID = 1
                };
            };
            ShimBlast.GetByBlastID_UseAmbientTransactionInt32UserBoolean = (id, usr, child) =>
            {
                return new Entities::BlastRegular();
            };
            ShimBlastFields.GetByBlastID_UseAmbientTransactionInt32User = (id, usr) => null;
            ShimBlastExtendedAbstract.AllInstances.SaveBlastAbstractUser = (obj, blast, usr) => 1;
            ShimBlastABMaster.SaveBlastABMasterUser = (blast, usr) => { };
            ShimCampaignItemBlast.UpdateBlastIDInt32Int32Int32 = (id, newId, userId) =>
            {
                itemUpdated = true;
            };

            // Act
            CallCreateBlastsFromCampaignItem_UseAmbientTransactionMethod(new object[] {
                campaignItemID,
                user,
                keepPaused });

            // Assert 
            itemUpdated.ShouldBeTrue();
        }
    }
}
