using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Configuration.Fakes;
using System.Reflection;
using System.Web.Mvc;
using System.Linq;
using System.Transactions.Fakes;
using ecn.MarketingAutomation.Controllers;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Entities = ECN_Framework_Entities.Communicator;
using Shouldly;
using KMPlatform.Entity;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using ecn.MarketingAutomation.Models.PostModels;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ECN.Tests.Helpers;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class HomeControllerTest
    {
        private const string DummyString = "DummyString";
        private const string StatusOKCode = "200";
        private const string ErrorCode = "500";
        private IDisposable _shimObject;
        private HomeController _controller;
        private PrivateObject _privateObject;
        private const string _MethodSaveCampaignItem = "SaveCampaignItem";
        private const string _PropertyCurrentUser = "CurrentUser";
        private const string MethodDeleteECNObject = "DeleteECNObject";

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();

            var httpContext = MvcMockHelpers.MockHttpContext();
            _controller = new HomeController();
            _controller.SetMockControllerContext(httpContext);
            _privateObject = new PrivateObject(_controller);
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
            _appSettings.Clear();
        }

        [Test]
        public void SaveCampaignItem_CreateCampaignItemIsFalse_ReturnPositiveNumber()
        {
            // Arrange
            var campaignItemID = 1;
            var campaignItem = new CampaignItem
            {
                CreateCampaignItem = false,
                CampaignItemID = campaignItemID
            };

            // Act	
            var actualResult = _privateObject.Invoke(_MethodSaveCampaignItem, new object[] { campaignItem, true });

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.ShouldBe(campaignItemID);
        }

        [Test]
        [TestCase(1, 1, "Group", 1)]
        [TestCase(0, 0, "", null)]
        public void SaveCampaignItem_CreateCampaignItemIsTrue_ReturnPositiveNumber(int campaignID, int campaignItemTemplateID, string subCategory, int? blastID)
        {
            // Arrange
            var campaignItemID = 1;
            const string controlId = "TestId";
            var campaignItem = new CampaignItem
            {
                CreateCampaignItem = true,
                CampaignItemID = campaignItemID,
                CampaignID = campaignID,
                CustomerID = 1,
                CampaignItemTemplateID = campaignItemTemplateID,
                SubCategory = subCategory,
                SendTime = DateTime.Now,
                ControlID = controlId,
                SelectedGroups = new List<GroupSelect>
                {
                    new GroupSelect
                    {
                        CustomerID = 1,
                        FolderID = 1,
                        GroupID=1
                    }
                },
                SuppressedGroups = new List<GroupSelect>
                {
                    new GroupSelect
                    {
                        GroupID = 1
                    }
                }
            };
            ShimAutomationBaseController.AllInstances.CurrentUserGet = (obj) =>
            {
                return new User
                {
                    UserID = 1
                };
            };
            ShimAutomationBaseController.AllInstances.AllConnectorsGet = (obj) =>
            {
                return new List<Connector>
                {
                    new Connector
                    {
                        to = new to
                        {
                            shapeId = controlId
                        },
                        from = new from
                        {
                            shapeId = controlId
                        }
                    }
                };
            };
            ShimAutomationBaseController.AllInstances.AllControlsGet = (obj) =>
            {
                return new List<ControlBase>
                {
                    new Wait
                    {
                        Days = 1,
                        Hours = 1,
                        Minutes = 1,
                        ControlID = controlId,
                    }
                };
            };
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return new Entities::CampaignItem
                {
                    BlastList = new List<Entities::CampaignItemBlast>
                    {
                        new Entities::CampaignItemBlast
                        {
                            BlastID = blastID,
                            CampaignItemBlastID = 1,
                        }
                    },
                    SuppressionList = new List<Entities::CampaignItemSuppression>()
                };
            };
            ShimCampaignItemBlastRefBlast.DeleteInt32UserBoolean = (id, user, child) => { };
            ShimCampaignItem.Save_UseAmbientTransactionCampaignItemUser = (ci, usr) => campaignItemID;
            ShimAutomationBaseController.AllInstances.SaveLinkTrackingParamOptionsCampaignItem = (obj, ci) => { };
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean = (val, usr, child) =>
            {
                return new Entities::CampaignItemTemplate
                {
                    OptoutGroupList = new List<Entities::CampaignItemTemplateOptoutGroup>
                    {
                        new Entities::CampaignItemTemplateOptoutGroup
                        {
                            GroupID = 1
                        }
                    },
                    OptOutSpecificGroup = true,
                    OptOutMasterSuppression = true
                };
            };
            ShimCampaignItemOptOutGroup.SaveCampaignItemOptOutGroupUser = (ci, usr) => { };
            ShimCampaignItemBlast.Save_UseAmbientTransactionInt32ListOfCampaignItemBlastUser = (id, list, usr) => { };
            ShimCampaignItemSuppression.Delete_NoAccessCheckInt32UserBoolean = (id, usr, child) => { };
            ShimCampaignItemSuppression.SaveCampaignItemSuppressionUser = (sub, usr) => 1;
            ShimBlast.CreateBlastsFromCampaignItem_UseAmbientTransactionInt32UserBooleanBoolean =
                (id, usr, child, keep) => { };
            ShimCampaign.Save_UseAmbientTransactionCampaignUser = (c, usr) => 1;
            ShimECNSession shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new User();
            ShimECNSession.CurrentSession = () => shimECNSession.Instance;

            // Act	
            var actualResult = _privateObject.Invoke(_MethodSaveCampaignItem, new object[] { campaignItem, true });

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.ShouldBe(campaignItemID);
        }

        [Test]
        public void Pause_OnException_ReturnErrorResponse()
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
            {
                throw new Exception();
            };

            // Act	
            var actualResult = _controller.Pause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                });
        }

        [Test]
        public void Pause_OnECNException_ReturnErrorResponse()
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
                new Entities::MarketingAutomation();
            SetShimForCurrentUser();
            ShimTransactionScope.Constructor = (obj) =>
            {
                throw new ECNException(
                    new List<ECNError>
                    {
                         new ECNError
                         {
                             ErrorMessage = DummyString
                         }
                    },
                    Enums.ExceptionLayer.WebSite);
            };

            // Act	
            var actualResult = _controller.Pause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(DummyString);
                });
        }

        [Test]
        public void Pause_OnSecurityException_ReturnErrorResponse()
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
                new Entities::MarketingAutomation();
            const string ErrorString = "You do not have permission to resume this automation";
            SetShimForCurrentUser();
            ShimTransactionScope.Constructor = (obj) => throw new SecurityException();

            // Act	
            var actualResult = _controller.Pause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void Pause_OnTransactionScopeException_ReturnErrorResponse()
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
                new Entities::MarketingAutomation();
            const string ErrorString = "An error occurred";
            SetShimForCurrentUser();
            ShimTransactionScope.Constructor = (obj) => throw new Exception();

            // Act	
            var actualResult = _controller.Pause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        [TestCase(MarketingAutomationControlType.CampaignItem)]
        [TestCase(MarketingAutomationControlType.Click)]
        [TestCase(MarketingAutomationControlType.NoClick)]
        [TestCase(MarketingAutomationControlType.NoOpen)]
        [TestCase(MarketingAutomationControlType.NotSent)]
        [TestCase(MarketingAutomationControlType.Open)]
        [TestCase(MarketingAutomationControlType.Open_NoClick)]
        [TestCase(MarketingAutomationControlType.Sent)]
        [TestCase(MarketingAutomationControlType.Suppressed)]
        [TestCase(MarketingAutomationControlType.Direct_Click)]
        [TestCase(MarketingAutomationControlType.Direct_Open)]
        [TestCase(MarketingAutomationControlType.Subscribe)]
        [TestCase(MarketingAutomationControlType.Unsubscribe)]
        [TestCase(MarketingAutomationControlType.Direct_NoOpen)]
        [TestCase(MarketingAutomationControlType.Start)]
        [TestCase(MarketingAutomationControlType.End)]
        [TestCase(MarketingAutomationControlType.Wait)]
        [TestCase(MarketingAutomationControlType.Group)]
        public void Pause_OnValidCall_ReturnSuccess(MarketingAutomationControlType controlType)
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
            {
                return new Entities::MarketingAutomation()
                {
                    Controls = new List<Entities::MAControl>
                    {
                        new Entities::MAControl
                        {
                            ECNID = 1,
                            ControlType = controlType
                        }
                    }
                };
            };
            SetShimForCurrentUser();
            SetShimsForTransactionScope();
            SetShimsForPauseAndUnPauseMethods("Y");

            // Act	
            var actualResult = _controller.Pause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(StatusOKCode);
                });
        }

        [Test]
        public void UnPause_OnException_ReturnErrorResponse()
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
            {
                throw new Exception();
            };

            // Act	
            var actualResult = _controller.UnPause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                });
        }

        [Test]
        public void UnPause_OnECNException_ReturnErrorResponse()
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
                new Entities::MarketingAutomation();
            SetShimForCurrentUser();
            ShimTransactionScope.Constructor = (obj) =>
            {
                throw new ECNException(
                    new List<ECNError>
                    {
                         new ECNError
                         {
                             ErrorMessage = DummyString
                         }
                    },
                    Enums.ExceptionLayer.WebSite);
            };

            // Act	
            var actualResult = _controller.UnPause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(DummyString);
                });
        }

        [Test]
        public void UnPause_OnSecurityException_ReturnErrorResponse()
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
                new Entities::MarketingAutomation();
            const string ErrorString = "You do not have permission to resume this automation";
            SetShimForCurrentUser();
            ShimTransactionScope.Constructor = (obj) => throw new SecurityException();

            // Act	
            var actualResult = _controller.UnPause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        public void UnPause_OnTransactionScopeException_ReturnErrorResponse()
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
                new Entities::MarketingAutomation();
            const string ErrorString = "An error occurred";
            SetShimForCurrentUser();
            ShimTransactionScope.Constructor = (obj) => throw new Exception();

            // Act	
            var actualResult = _controller.UnPause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(ErrorCode);
                    jsonList[1].ShouldContain(ErrorString);
                });
        }

        [Test]
        [TestCase(MarketingAutomationControlType.CampaignItem)]
        [TestCase(MarketingAutomationControlType.Click)]
        [TestCase(MarketingAutomationControlType.NoClick)]
        [TestCase(MarketingAutomationControlType.NoOpen)]
        [TestCase(MarketingAutomationControlType.NotSent)]
        [TestCase(MarketingAutomationControlType.Open)]
        [TestCase(MarketingAutomationControlType.Open_NoClick)]
        [TestCase(MarketingAutomationControlType.Sent)]
        [TestCase(MarketingAutomationControlType.Suppressed)]
        [TestCase(MarketingAutomationControlType.Direct_Click)]
        [TestCase(MarketingAutomationControlType.Direct_Open)]
        [TestCase(MarketingAutomationControlType.Subscribe)]
        [TestCase(MarketingAutomationControlType.Unsubscribe)]
        [TestCase(MarketingAutomationControlType.Direct_NoOpen)]
        [TestCase(MarketingAutomationControlType.Start)]
        [TestCase(MarketingAutomationControlType.End)]
        [TestCase(MarketingAutomationControlType.Wait)]
        [TestCase(MarketingAutomationControlType.Group)]
        public void UnPause_OnValidCall_ReturnSuccess(MarketingAutomationControlType controlType)
        {
            // Arrange
            var mAId = 1;
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (id, child, usr) =>
            {
                return new Entities::MarketingAutomation()
                {
                    Controls = new List<Entities::MAControl>
                    {
                        new Entities::MAControl
                        {
                            ECNID = 1,
                            ControlType = controlType
                        }
                    }
                };
            };
            SetShimForCurrentUser();
            SetShimsForTransactionScope();
            SetShimsForPauseAndUnPauseMethods("P");

            // Act	
            var actualResult = _controller.UnPause(mAId) as JsonResult;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Data.ShouldNotBeNull(),
                () =>
                {
                    var jsonList = actualResult.Data as List<string>;
                    jsonList.ShouldNotBeNull();
                    jsonList[0].ShouldBe(StatusOKCode);
                });
        }

        [Test]
        public void HasAuthorised_AuthorisedClient_true()
        {
            // Arrange
            var authorisedUserId = 10;
            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User()
            {
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
                {
                    new UserClientSecurityGroupMap() { ClientID = authorisedUserId}
                }
            };
            ShimECNSession.CurrentSession = () => session;

            // Act and  Assert
            _controller.HasAuthorized(0, authorisedUserId).ShouldBeTrue();
        }

        [Test]
        public void HasAuthorised_NotAuthorisedClient_false()
        {
            // Arrange
            var authorisedUserId = 10;
            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User()
            {
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
            };
            ShimECNSession.CurrentSession = () => session;

            // Act and  Assert
            _controller.HasAuthorized(0, authorisedUserId).ShouldBeFalse();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.CampaignItem)]
        [TestCase(MarketingAutomationControlType.Click)]
        [TestCase(MarketingAutomationControlType.NoClick)]
        [TestCase(MarketingAutomationControlType.NoOpen)]
        [TestCase(MarketingAutomationControlType.NotSent)]
        [TestCase(MarketingAutomationControlType.Open)]
        [TestCase(MarketingAutomationControlType.Open_NoClick)]
        [TestCase(MarketingAutomationControlType.Sent)]
        [TestCase(MarketingAutomationControlType.Suppressed)]
        [TestCase(MarketingAutomationControlType.Direct_Click)]
        [TestCase(MarketingAutomationControlType.Direct_Open)]
        [TestCase(MarketingAutomationControlType.Subscribe)]
        [TestCase(MarketingAutomationControlType.Unsubscribe)]
        public void DeleteECNObject_MultipleCases_ECNObjectDeleted(MarketingAutomationControlType controlType)
        {
            // Arrange
            var maControl = new Entities::MAControl
            {
                ECNID = DummyId,
                ControlType = controlType
            };
            var itemDeleted = false;
            ShimCampaignItem.DeleteInt32UserBoolean = (_, __, ___) => { itemDeleted = true; };
            ShimCurrentUser();
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (_, __) => new Entities::LayoutPlans
            {
                LayoutPlanID = DummyId,
                BlastID = DummyId
            };
            ShimLayoutPlans.DeleteByLayoutPlanIDInt32User = (_, __) => { };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new Entities::CampaignItem
            {
                CampaignItemID = DummyId
            };

            // Act	
            _privateObject.Invoke(MethodDeleteECNObject, maControl);

            // Assert
            itemDeleted.ShouldBeTrue();
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Direct_NoOpen)]
        [TestCase(MarketingAutomationControlType.Start)]
        [TestCase(MarketingAutomationControlType.End)]
        [TestCase(MarketingAutomationControlType.Wait)]
        [TestCase(MarketingAutomationControlType.Group)]
        public void DeleteECNObject_NoDeleteAction_ECNObjectNotDeleted(MarketingAutomationControlType controlType)
        {
            // Arrange
            var maControl = new Entities::MAControl
            {
                ECNID = DummyId,
                ControlType = controlType
            };
            var itemDeleted = false;
            ShimCampaignItem.DeleteInt32UserBoolean = (_, __, ___) => { itemDeleted = true; };

            // Act	
            _privateObject.Invoke(MethodDeleteECNObject, maControl);

            // Assert
            itemDeleted.ShouldBeFalse();
        }

        private void SetShimForCurrentUser()
        {
            ShimAutomationBaseController.AllInstances.CurrentUserGet = (obj) =>
            {
                return new User
                {
                    UserID = 1
                };
            };
        }

        private void SetShimsForTransactionScope()
        {
            ShimTransactionScope.Constructor = (obj) => { };
            ShimTransactionScope.AllInstances.Dispose = (obj) => { };
            ShimTransactionScope.AllInstances.Complete = (obj) => { };
        }

        private void SetShimsForPauseAndUnPauseMethods(string status)
        {
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return new Entities::CampaignItem
                {
                    BlastList = new List<Entities::CampaignItemBlast>
                    {
                        new Entities::CampaignItemBlast
                        {
                            BlastID = 1,
                            CustomerID = 1,
                            Blast = new Entities::BlastRegular
                            {
                                StatusCode = "paused"
                            }
                        }
                    },
                    CustomerID = 1,
                };
            };
            ShimBlast.ActiveOrSentInt32Int32 = (id, cusId) => false;
            ShimBlast.Pause_UnPauseBlastInt32BooleanUser = (id, isPause, usr) => { };
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (id, usr) =>
            {
                return new Entities::LayoutPlans
                {
                    Status = status,
                    LayoutPlanID = 1
                };
            };
            ShimLayoutPlans.SaveLayoutPlansUser = (lp, usr) => 1;
            ShimBlastSingle.Pause_UnPause_ForLayoutPlanIDInt32BooleanUser = (id, isPause, usr) => { };
            ShimBlastSingle.Pause_Unpause_ForTriggerPlanIDInt32BooleanUser = (id, isPause, usr) => { };
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (id, usr) =>
            {
                return new Entities::TriggerPlans
                {
                    Status = status,
                    TriggerPlanID = 1
                };
            };
            ShimTriggerPlans.SaveTriggerPlansUser = (tp, usr) => 1;
            ShimMarketingAutomation.SaveMarketingAutomationUser = (ma, usr) => 1;
            ShimMarketingAutomationHistory.InsertInt32Int32String = (id, userId, action) => { };
        }
    }
}
