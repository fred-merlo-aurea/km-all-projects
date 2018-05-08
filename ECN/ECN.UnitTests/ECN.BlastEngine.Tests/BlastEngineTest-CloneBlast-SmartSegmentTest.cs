using System;
using System.Collections.Generic;
using System.Data;
using ecn.blastengine;
using ecn.common.classes.Fakes;
using ecn.communicator.classes;
using ECN.TestHelpers;
using KM.Common.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ecn.blastengine.Fakes;
using CommonObjects = ECN_Framework_Common.Objects;
using CommonFakes = KM.Common.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using EntitiesCommunicatorFakes = ECN_Framework_Entities.Communicator.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ECN_Framework.Common.Fakes;
using ecn.communicator.classes.Fakes;
using static ECN_Framework_Common.Objects.Enums;
using BusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;
using BusinessLayerAccountsFakes = ECN_Framework_BusinessLayer.Accounts.Fakes;
using KMPlatform.Entity;
using KMPlatform.Entity.Fakes;
using NUnit.Framework;
using AccountsFakes = ECN_Framework_BusinessLayer.Accounts.Fakes;
using AccountsEntity = ECN_Framework_Entities.Accounts;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ECN.BlastEngine.Tests
{
    public partial class BlastEngineTest
    {
        [Test]
        public void CloneBlast_CampaignItemSmartSegmentBlastExist_ReachEnd()
        {
            // Arrange
            var exceptionLogged = false;
            var reachedEnd = false;
            var blastEngine = new ECNBlastEngine();
            var blastId = 1;
            var blastGroupId = 1;
            appSettings.Add("LogStatistics", bool.TrueString);
            var blast = new EntitiesCommunicator::BlastRegular();
            blast.BlastID = blastId;
            blast.GroupID = blastGroupId;
            blast.BlastType = BlastType.HTML.ToString();
            blast.CustomerID = 1;
            blast.CreatedUserID = 1;
            var setupInfo = new EntitiesCommunicator::BlastSetupInfo();
            CommonFakes::ShimFileFunctions.LogConsoleActivityStringString = (str, output) => { };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                throw new Exception("Exception from SqlCommand");
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return blast;
            };
            BusinessLogicFakes::ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User
                {
                    UserID = 1
                };
            };
            AccountsFakes::ShimCustomer.GetByCustomerIDInt32Boolean = (id, child) =>
            {
                return new AccountsEntity::Customer
                {
                    BaseChannelID = 1,
                    PlatformClientID = 1
                };
            };
            BusinessLayerAccountsFakes::ShimBaseChannel.GetByBaseChannelIDInt32 = (id) =>
            {
                return new AccountsEntity::BaseChannel
                {
                    PlatformClientGroupID = 1
                };
            };
            ShimECNBlastEngine.AllInstances.SendAccountManagersEmailNotificationStringString = (eng, msg, exc) =>
            {
                exceptionLogged = true;
            };
            BusinessLogicFakes::ShimClient.AllInstances.SelectInt32Boolean = (obj, id, include) =>
            {
                return new Client
                {
                    ClientID = 1
                };
            };
            BusinessLogicFakes::ShimClientGroup.AllInstances.SelectInt32Boolean = (obj, id, include) =>
            {
                return new ClientGroup();
            };
            BusinessLogicFakes::ShimSecurityGroup.AllInstances.SelectInt32Int32BooleanBoolean =
                (obj, uid, cid, isKm, include) =>
                {
                    return new SecurityGroup();
                };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return new EntitiesCommunicator::CampaignItem
                {
                    CampaignID = 1,
                    CampaignItemID = 1,
                    CampaignItemType = CampaignItemType.Regular.ToString(),
                    SendTime = new DateTime(2018, 1, 1),
                    CampaignItemName = "TestCampaignItemName",
                    CampaignItemIDOriginal = 1,
                    CampaignItemNameOriginal = "TestCampaignItemNameOriginal",
                    BlastList = new List<EntitiesCommunicator::CampaignItemBlast>
                    {
                        new EntitiesCommunicator::CampaignItemBlast
                        {
                            BlastID = blastId
                        }
                    },
                    SuppressionList = new List<EntitiesCommunicator::CampaignItemSuppression>()
                };
            };
            ShimCampaignItem.Save_NoAccessCheckCampaignItemUser = (item, user) => 1;
            ShimCampaignItemBlast.Save_NoAccessCheckCampaignItemBlastUser = (item, user) => 1;
            ShimBlast.CreateBlastsFromCampaignItem_NoAccessCheckInt32UserBoolean = (id, user, child) => { };
            ShimBlast.GetByCampaignItemBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                reachedEnd = true;
                return null;
            };
            ShimECNBlastEngine.AllInstances.GetSmartSegmentBlastInt32 = (obj, id) => 1;

            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_CloneBlast,
                new object[]
                {
                    blastId,
                    setupInfo
                }, blastEngine);

            // Assert
            Assert.IsFalse(exceptionLogged);
            Assert.IsTrue(reachedEnd);
        }

        [Test]
        public void CloneBlast_CampaignItemSmartSegmentBlastSuppressionListExist_ReachEnd()
        {
            // Arrange
            var exceptionLogged = false;
            var reachedEnd = false;
            var blastEngine = new ECNBlastEngine();
            var blastId = 1;
            var blastGroupId = 1;
            appSettings.Add("LogStatistics", bool.TrueString);
            var blast = new EntitiesCommunicator::BlastRegular();
            blast.BlastID = blastId;
            blast.GroupID = blastGroupId;
            blast.BlastType = BlastType.HTML.ToString();
            blast.CustomerID = 1;
            blast.CreatedUserID = 1;
            var setupInfo = new EntitiesCommunicator::BlastSetupInfo();
            CommonFakes::ShimFileFunctions.LogConsoleActivityStringString = (str, output) => { };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                throw new Exception("Exception from SqlCommand");
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return blast;
            };
            BusinessLogicFakes::ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User
                {
                    UserID = 1
                };
            };
            AccountsFakes::ShimCustomer.GetByCustomerIDInt32Boolean = (id, child) =>
            {
                return new AccountsEntity::Customer
                {
                    BaseChannelID = 1,
                    PlatformClientID = 1
                };
            };
            BusinessLayerAccountsFakes::ShimBaseChannel.GetByBaseChannelIDInt32 = (id) =>
            {
                return new AccountsEntity::BaseChannel
                {
                    PlatformClientGroupID = 1
                };
            };
            ShimECNBlastEngine.AllInstances.SendAccountManagersEmailNotificationStringString = (eng, msg, exc) =>
            {
                exceptionLogged = true;
            };
            BusinessLogicFakes::ShimClient.AllInstances.SelectInt32Boolean = (obj, id, include) =>
            {
                return new Client
                {
                    ClientID = 1
                };
            };
            BusinessLogicFakes::ShimClientGroup.AllInstances.SelectInt32Boolean = (obj, id, include) =>
            {
                return new ClientGroup();
            };
            BusinessLogicFakes::ShimSecurityGroup.AllInstances.SelectInt32Int32BooleanBoolean =
                (obj, uid, cid, isKm, include) =>
                {
                    return new SecurityGroup();
                };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return new EntitiesCommunicator::CampaignItem
                {
                    CampaignID = 1,
                    CampaignItemID = 1,
                    CampaignItemType = CampaignItemType.Regular.ToString(),
                    SendTime = new DateTime(2018, 1, 1),
                    CampaignItemName = "TestCampaignItemName",
                    CampaignItemIDOriginal = 1,
                    CampaignItemNameOriginal = "TestCampaignItemNameOriginal",
                    BlastList = new List<EntitiesCommunicator::CampaignItemBlast>
                    {
                        new EntitiesCommunicator::CampaignItemBlast
                        {
                            BlastID = blastId
                        }
                    },
                    SuppressionList = new List<EntitiesCommunicator::CampaignItemSuppression>
                    {
                        new EntitiesCommunicator::CampaignItemSuppression
                        {
                            CampaignItemID = 1,
                            Filters = new List<EntitiesCommunicator::CampaignItemBlastFilter>
                            {
                                new EntitiesCommunicator::CampaignItemBlastFilter
                                {
                                    FilterID = 1,
                                    IsDeleted = false
                                }
                            }
                        }
                    }
                };
            };
            ShimCampaignItem.Save_NoAccessCheckCampaignItemUser = (item, user) => 1;
            ShimCampaignItemBlast.Save_NoAccessCheckCampaignItemBlastUser = (item, user) => 1;
            ShimBlast.CreateBlastsFromCampaignItem_NoAccessCheckInt32UserBoolean = (id, user, child) => { };
            ShimBlast.GetByCampaignItemBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                reachedEnd = true;
                return null;
            };
            ShimECNBlastEngine.AllInstances.GetSmartSegmentBlastInt32 = (obj, id) => 1;
            ShimCampaignItemSuppression.Save_NoAccessCheckCampaignItemSuppressionUser = (item, user) => 1;
            ShimCampaignItemBlastFilter.SaveCampaignItemBlastFilter = (filter) => 1;

            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_CloneBlast,
                new object[]
                {
                    blastId,
                    setupInfo
                }, blastEngine);

            // Assert
            Assert.IsFalse(exceptionLogged);
            Assert.IsTrue(reachedEnd);
        }

        [Test]
        public void CloneBlast_CampaignItemSmartSegmentBlastFiltersExist_ReachEnd()
        {
            // Arrange
            var exceptionLogged = false;
            var reachedEnd = false;
            var blastEngine = new ECNBlastEngine();
            var blastId = 1;
            var blastGroupId = 1;
            appSettings.Add("LogStatistics", bool.TrueString);
            var blast = new EntitiesCommunicator::BlastRegular();
            blast.BlastID = blastId;
            blast.GroupID = blastGroupId;
            blast.BlastType = BlastType.HTML.ToString();
            blast.CustomerID = 1;
            blast.CreatedUserID = 1;
            var setupInfo = new EntitiesCommunicator::BlastSetupInfo();
            CommonFakes::ShimFileFunctions.LogConsoleActivityStringString = (str, output) => { };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                throw new Exception("Exception from SqlCommand");
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return blast;
            };
            BusinessLogicFakes::ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User
                {
                    UserID = 1
                };
            };
            AccountsFakes::ShimCustomer.GetByCustomerIDInt32Boolean = (id, child) =>
            {
                return new AccountsEntity::Customer
                {
                    BaseChannelID = 1,
                    PlatformClientID = 1
                };
            };
            BusinessLayerAccountsFakes::ShimBaseChannel.GetByBaseChannelIDInt32 = (id) =>
            {
                return new AccountsEntity::BaseChannel
                {
                    PlatformClientGroupID = 1
                };
            };
            ShimECNBlastEngine.AllInstances.SendAccountManagersEmailNotificationStringString = (eng, msg, exc) =>
            {
                exceptionLogged = true;
            };
            BusinessLogicFakes::ShimClient.AllInstances.SelectInt32Boolean = (obj, id, include) =>
            {
                return new Client
                {
                    ClientID = 1
                };
            };
            BusinessLogicFakes::ShimClientGroup.AllInstances.SelectInt32Boolean = (obj, id, include) =>
            {
                return new ClientGroup();
            };
            BusinessLogicFakes::ShimSecurityGroup.AllInstances.SelectInt32Int32BooleanBoolean =
                (obj, uid, cid, isKm, include) =>
                {
                    return new SecurityGroup();
                };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return new EntitiesCommunicator::CampaignItem
                {
                    CampaignID = 1,
                    CampaignItemID = 1,
                    CampaignItemType = CampaignItemType.Regular.ToString(),
                    SendTime = new DateTime(2018, 1, 1),
                    CampaignItemName = "TestCampaignItemName",
                    CampaignItemIDOriginal = 1,
                    CampaignItemNameOriginal = "TestCampaignItemNameOriginal",
                    BlastList = new List<EntitiesCommunicator::CampaignItemBlast>
                    {
                        new EntitiesCommunicator::CampaignItemBlast
                        {
                            BlastID = blastId,
                            Filters = new List<EntitiesCommunicator::CampaignItemBlastFilter>
                            {
                                new EntitiesCommunicator::CampaignItemBlastFilter
                                {
                                    FilterID = 1
                                }
                            }
                        }
                    },
                    SuppressionList = new List<EntitiesCommunicator::CampaignItemSuppression>()
                };
            };
            ShimCampaignItem.Save_NoAccessCheckCampaignItemUser = (item, user) => 1;
            ShimCampaignItemBlast.Save_NoAccessCheckCampaignItemBlastUser = (item, user) => 1;
            ShimBlast.CreateBlastsFromCampaignItem_NoAccessCheckInt32UserBoolean = (id, user, child) => { };
            ShimBlast.GetByCampaignItemBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                reachedEnd = true;
                return null;
            };
            ShimECNBlastEngine.AllInstances.GetSmartSegmentBlastInt32 = (obj, id) => 1;
            ShimCampaignItemBlastFilter.SaveCampaignItemBlastFilter = (filter) => 1;

            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_CloneBlast,
                new object[]
                {
                    blastId,
                    setupInfo
                }, blastEngine);

            // Assert
            Assert.IsFalse(exceptionLogged);
            Assert.IsTrue(reachedEnd);
        }

        [Test]
        public void CloneBlast_CampaignItemSmartSegmentBlastSmartSegmentFiltersExist_ReachEnd()
        {
            // Arrange
            var exceptionLogged = false;
            var reachedEnd = false;
            var blastEngine = new ECNBlastEngine();
            var blastId = 1;
            var blastGroupId = 1;
            appSettings.Add("LogStatistics", bool.TrueString);
            var blast = new EntitiesCommunicator::BlastRegular();
            blast.BlastID = blastId;
            blast.GroupID = blastGroupId;
            blast.BlastType = BlastType.HTML.ToString();
            blast.CustomerID = 1;
            blast.CreatedUserID = 1;
            var setupInfo = new EntitiesCommunicator::BlastSetupInfo();
            CommonFakes::ShimFileFunctions.LogConsoleActivityStringString = (str, output) => { };
            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                throw new Exception("Exception from SqlCommand");
            };
            ShimECNBlastEngine.AllInstances.SetBlastSingleToErrorInt32 = (eng, id) => { };
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return blast;
            };
            BusinessLogicFakes::ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User
                {
                    UserID = 1
                };
            };
            AccountsFakes::ShimCustomer.GetByCustomerIDInt32Boolean = (id, child) =>
            {
                return new AccountsEntity::Customer
                {
                    BaseChannelID = 1,
                    PlatformClientID = 1
                };
            };
            BusinessLayerAccountsFakes::ShimBaseChannel.GetByBaseChannelIDInt32 = (id) =>
            {
                return new AccountsEntity::BaseChannel
                {
                    PlatformClientGroupID = 1
                };
            };
            ShimECNBlastEngine.AllInstances.SendAccountManagersEmailNotificationStringString = (eng, msg, exc) =>
            {
                exceptionLogged = true;
            };
            BusinessLogicFakes::ShimClient.AllInstances.SelectInt32Boolean = (obj, id, include) =>
            {
                return new Client
                {
                    ClientID = 1
                };
            };
            BusinessLogicFakes::ShimClientGroup.AllInstances.SelectInt32Boolean = (obj, id, include) =>
            {
                return new ClientGroup();
            };
            BusinessLogicFakes::ShimSecurityGroup.AllInstances.SelectInt32Int32BooleanBoolean =
                (obj, uid, cid, isKm, include) =>
                {
                    return new SecurityGroup();
                };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return new EntitiesCommunicator::CampaignItem
                {
                    CampaignID = 1,
                    CampaignItemID = 1,
                    CampaignItemType = CampaignItemType.Regular.ToString(),
                    SendTime = new DateTime(2018, 1, 1),
                    CampaignItemName = "TestCampaignItemName",
                    CampaignItemIDOriginal = 1,
                    CampaignItemNameOriginal = "TestCampaignItemNameOriginal",
                    BlastList = new List<EntitiesCommunicator::CampaignItemBlast>
                    {
                        new EntitiesCommunicator::CampaignItemBlast
                        {
                            BlastID = blastId,
                            Filters = new List<EntitiesCommunicator::CampaignItemBlastFilter>
                            {
                                new EntitiesCommunicator::CampaignItemBlastFilter
                                {
                                    FilterID = 1,
                                    SmartSegmentID = 1
                                }
                            }
                        }
                    },
                    SuppressionList = new List<EntitiesCommunicator::CampaignItemSuppression>()
                };
            };
            ShimCampaignItem.Save_NoAccessCheckCampaignItemUser = (item, user) => 1;
            ShimCampaignItemBlast.Save_NoAccessCheckCampaignItemBlastUser = (item, user) => 1;
            ShimBlast.CreateBlastsFromCampaignItem_NoAccessCheckInt32UserBoolean = (id, user, child) => { };
            ShimBlast.GetByCampaignItemBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                reachedEnd = true;
                return null;
            };
            ShimECNBlastEngine.AllInstances.GetSmartSegmentBlastInt32 = (obj, id) => 1;
            ShimCampaignItemBlastFilter.SaveCampaignItemBlastFilter = (filter) => 1;

            // Act
            typeof(ECNBlastEngine).CallMethod(METHOD_CloneBlast,
                new object[]
                {
                    blastId,
                    setupInfo
                }, blastEngine);

            // Assert
            Assert.IsFalse(exceptionLogged);
            Assert.IsTrue(reachedEnd);
        }
    }
}