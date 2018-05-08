using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const string ActionTypeCodeColumn = "ActionTypeCode";
        private const string DistinctCountColumn = "DistinctCount";
        private const string Send = "send";
        private const string Bounce = "bounce";
        private const string Clickthrough = "clickthrough";
        private const string NoClick = "noclick";
        private const string NoOpen = "noopen";
        private const string NotSent = "notsent";
        private const string Open = "open";
        private const string OpenNoClick = "open_noclick";
        private const string Suppressed = "suppressed";
        private const int ExpectedHeatMapStats = 8;
        private int? _layoutPlansBlastId;
        private int _campaiginItemId;
        private int _sendDiscountCount;
        private int _bounceDiscountCount;
        private int _clickthroughDiscountCount;
        private int _notSentDiscountCount;
        private int _noClickDiscountCount;
        private int _noOpenDiscountCount;
        private int _openDiscountCount;
        private int _openNoClickDiscountCount;
        private int _suppressedOpenDiscountCount;
        private int? _campaiginItemBlastId;
        private int? _campaignItemBlastCustomerId;
        private bool _blastStatusFirstTime;
        private bool _blastStatusSecondTime;

        [Test]
        public void GetHeatMapStats_NotValidControls_ShouldBeAddedToResult()
        {
            //Arrange
            var control = new End();
            var controls = new List<ControlBase>
            {
                control
            };
            var connectors = new List<Connector>();

            //Act
            var result = CallGetHeatMapStats(controls, ref connectors);

            //Assert
            result.ShouldContain(control);
        }

        [Test]
        public void GetHeatMapStats_GroupControl_AddUpdatedGroupToResult()
        {
            //Arrange
            const string SubscribedColumn = "subscribed";
            const string UnsubscribedColumn = "unsubscribed";
            const string SuppressedColumn = "suppressed";
            var subscribed = GetAnyNumber();
            var unsubscribed = GetAnyNumber();
            var suppressed = GetAnyNumber();
            var control = new Group();
            var controls = new List<ControlBase>
            {
                control
            };
            var connectors = new List<Connector>();
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = id =>
            {
                return new CommunicatorEntities.Group();
            };
            ShimEmailGroup.GetGroupStatsInt32Int32 = (groupId, customerId) =>
            {
                var table = new DataTable();
                table.Columns.Add(SubscribedColumn);
                table.Columns.Add(UnsubscribedColumn);
                table.Columns.Add(SuppressedColumn);
                var row = table.NewRow();
                row[SubscribedColumn] = subscribed;
                row[UnsubscribedColumn] = unsubscribed;
                row[SuppressedColumn] = suppressed;
                table.Rows.Add(row);
                return table;
            };

            //Act
            var result = CallGetHeatMapStats(controls, ref connectors);

            //Assert
            result.ShouldContain(control);
            control.Subscribed.ShouldBe(subscribed);
            control.Unsubscribed.ShouldBe(unsubscribed);
            control.Suppressed.ShouldBe(suppressed);
        }

        [Test]
        public void GetHeatMapStats_CampaignItem_WillSetHeatMapStats()
        {
            //Arrange
            CommonShimsForGetJeatMapStats();

            var control = new CampaignItem();
            var controls = new List<ControlBase>
            {
                control
            };
            var connectors = new List<Connector>();
            _sendDiscountCount = GetAnyNumber();
            _bounceDiscountCount = GetAnyNumber();
            var expectedHeatMapStats = _sendDiscountCount - _bounceDiscountCount;

            //Act
            var result = CallGetHeatMapStats(controls, ref connectors);

            //Assert
            result.ShouldContain(control);
            control.HeatMapStats.ShouldBe(expectedHeatMapStats);
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Click)]
        [TestCase(MarketingAutomationControlType.Direct_Click)]
        [TestCase(MarketingAutomationControlType.FormAbandon)]
        [TestCase(MarketingAutomationControlType.FormSubmit)]
        [TestCase(MarketingAutomationControlType.Direct_Open)]
        [TestCase(MarketingAutomationControlType.Direct_NoOpen)]
        [TestCase(MarketingAutomationControlType.NoClick)]
        [TestCase(MarketingAutomationControlType.NoOpen)]
        [TestCase(MarketingAutomationControlType.NotSent)]
        [TestCase(MarketingAutomationControlType.Open)]
        [TestCase(MarketingAutomationControlType.Open_NoClick)]
        [TestCase(MarketingAutomationControlType.Sent)]
        [TestCase(MarketingAutomationControlType.Subscribe)]
        [TestCase(MarketingAutomationControlType.Suppressed)]
        [TestCase(MarketingAutomationControlType.Unsubscribe)]
        public void GetHeatMapStats_OtherThanFormControls_WillSetHeatMapStats(
            MarketingAutomationControlType controlType)
        {
            //Arrange
            CommonShimsForGetJeatMapStats();
            _layoutPlansBlastId = GetAnyNumber();
            var control = GetControl(controlType);
            var controls = new List<ControlBase>
            {
                control
            };
            var connectors = new List<Connector>();
            _sendDiscountCount = GetAnyNumber();
            _bounceDiscountCount = GetAnyNumber();

            //Act
            var result = CallGetHeatMapStats(controls, ref connectors);

            //Assert
            result.ShouldContain(control);
            GetHeatMapStats(control).ShouldBe(_sendDiscountCount);
        }

        [Test]
        [TestCase(MarketingAutomationControlType.CampaignItem)]
        [TestCase(MarketingAutomationControlType.NoClick)]
        [TestCase(MarketingAutomationControlType.Click)]
        [TestCase(MarketingAutomationControlType.NoOpen)]
        [TestCase(MarketingAutomationControlType.Open_NoClick)]
        [TestCase(MarketingAutomationControlType.Open)]
        [TestCase(MarketingAutomationControlType.NotSent)]
        [TestCase(MarketingAutomationControlType.Sent)]
        [TestCase(MarketingAutomationControlType.Suppressed)]
        [TestCase(MarketingAutomationControlType.Direct_Click)]
        [TestCase(MarketingAutomationControlType.Direct_Open)]
        [TestCase(MarketingAutomationControlType.FormSubmit)]
        [TestCase(MarketingAutomationControlType.FormAbandon)]
        [TestCase(MarketingAutomationControlType.Subscribe)]
        [TestCase(MarketingAutomationControlType.Unsubscribe)]
        [TestCase(MarketingAutomationControlType.Direct_NoOpen)]
        public void GetHeatMapStats_FormWithDifferentParent_WillSetHeatMapStats(
            MarketingAutomationControlType parentType)
        {
            //Arrange
            CommonShimsForGetJeatMapStats();
            _layoutPlansBlastId = GetAnyNumber();
            var form = GetControl(MarketingAutomationControlType.Form);
            var parentControl = GetControl(parentType);
            var controls = new List<ControlBase>
            {
                form
            };
            var connectors = new List<Connector>();
            AddConnectorFor(controls, connectors, parentControl, form);
            _sendDiscountCount = GetAnyNumber();
            _bounceDiscountCount = GetAnyNumber();
            _campaiginItemId = GetAnyNumber();

            //Act
            var result = CallGetHeatMapStats(controls, ref connectors);

            //Assert
            result.ShouldContain(form);
            GetHeatMapStats(form).ShouldBe(_sendDiscountCount);
        }

        [Test]
        [TestCase(MarketingAutomationControlType.CampaignItem, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.CampaignItem, true, true, Zero)]
        [TestCase(MarketingAutomationControlType.Click, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.Click, true, true, Zero)]
        [TestCase(MarketingAutomationControlType.NoClick, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.NoClick, true, true, Zero)]
        [TestCase(MarketingAutomationControlType.NoOpen, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.NoOpen, true, true, Zero)]
        [TestCase(MarketingAutomationControlType.NotSent, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.NotSent, true, true, Zero)]
        [TestCase(MarketingAutomationControlType.Open, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.Open, true, true, Zero)]
        [TestCase(MarketingAutomationControlType.Open_NoClick, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.Open_NoClick, true, true, Zero)]
        [TestCase(MarketingAutomationControlType.Sent, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.Sent, true, true, Zero)]
        [TestCase(MarketingAutomationControlType.Suppressed, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.Suppressed, true, true, Zero)]
        [TestCase(MarketingAutomationControlType.Direct_Click, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.Direct_Open, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.Subscribe, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.Unsubscribe, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.Direct_NoOpen, false, true, ExpectedHeatMapStats)]
        public void GetHeatMapStats_WaitAndSingleChild_WillSetHeatMapStats(
            MarketingAutomationControlType childType,
            bool activeOrSentParent,
            bool activeOrSentChild,
            int expectedHeatMapStats)
        {
            //Arrange
            var waitParent = GetControl(MarketingAutomationControlType.End);
            var wait = GetControl(MarketingAutomationControlType.Wait);
            var child = GetControl(childType);
            _blastStatusFirstTime = activeOrSentParent;
            _blastStatusSecondTime = activeOrSentChild;
            var controls = new List<ControlBase>
            {
                wait
            };
            var connectors = new List<Connector>();
            AddConnectorFor(controls, connectors, wait, child);
            AddConnectorFor(controls, connectors, waitParent, wait);
            _campaiginItemBlastId = GetAnyNumber();
            _campaignItemBlastCustomerId = GetAnyNumber();
            _layoutPlansBlastId = GetAnyNumber();
            _sendDiscountCount = expectedHeatMapStats;
            _sendDiscountCount = expectedHeatMapStats;
            _bounceDiscountCount = expectedHeatMapStats;
            _clickthroughDiscountCount = expectedHeatMapStats;
            _notSentDiscountCount = expectedHeatMapStats;
            _noClickDiscountCount = expectedHeatMapStats;
            _noOpenDiscountCount = expectedHeatMapStats;
            _openDiscountCount = expectedHeatMapStats;
            _openNoClickDiscountCount = expectedHeatMapStats;
            _suppressedOpenDiscountCount = expectedHeatMapStats;
            CommonShimsForGetJeatMapStats();

            //Act
            CallGetHeatMapStats(controls, ref connectors);

            //Assert
            GetHeatMapStats(wait).ShouldBe(expectedHeatMapStats);
        }

        [TestCase(MarketingAutomationControlType.FormAbandon, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.FormSubmit, false, true, ExpectedHeatMapStats)]
        public void GetHeatMapStats_WaitAndSingleChildOfTypeFormAbandonOrFormSubmit_WillSetHeatMapStats(
            MarketingAutomationControlType childType,
            bool activeOrSentParent,
            bool activeOrSentChild,
            int expectedHeatMapStats)
        {
            //Arrange
            var parentOfParentTypes = new[]
            {
                MarketingAutomationControlType.CampaignItem,
                MarketingAutomationControlType.NoClick,
                MarketingAutomationControlType.Click,
                MarketingAutomationControlType.NoOpen,
                MarketingAutomationControlType.Open_NoClick,
                MarketingAutomationControlType.Open,
                MarketingAutomationControlType.NotSent,
                MarketingAutomationControlType.Sent,
                MarketingAutomationControlType.Suppressed,
                MarketingAutomationControlType.Direct_Click,
                MarketingAutomationControlType.Direct_Open,
                MarketingAutomationControlType.FormSubmit,
                MarketingAutomationControlType.FormAbandon,
                MarketingAutomationControlType.Subscribe,
                MarketingAutomationControlType.Unsubscribe,
                MarketingAutomationControlType.Direct_NoOpen
            };

            var waitParent = GetControl(MarketingAutomationControlType.Form);
            var wait = GetControl(MarketingAutomationControlType.Wait);
            var child = GetControl(childType);
            _blastStatusFirstTime = activeOrSentParent;
            _blastStatusSecondTime = activeOrSentChild;

            _campaiginItemBlastId = GetAnyNumber();
            _campaignItemBlastCustomerId = GetAnyNumber();
            _layoutPlansBlastId = GetAnyNumber();
            _sendDiscountCount = expectedHeatMapStats;
            _sendDiscountCount = expectedHeatMapStats;
            _bounceDiscountCount = expectedHeatMapStats;
            _clickthroughDiscountCount = expectedHeatMapStats;
            _notSentDiscountCount = expectedHeatMapStats;
            _noClickDiscountCount = expectedHeatMapStats;
            _noOpenDiscountCount = expectedHeatMapStats;
            _openDiscountCount = expectedHeatMapStats;
            _openNoClickDiscountCount = expectedHeatMapStats;
            _suppressedOpenDiscountCount = expectedHeatMapStats;
            CommonShimsForGetJeatMapStats();
            foreach (var type in parentOfParentTypes)
            {
                var waitParentParent = GetControl(type);
                var controls = new List<ControlBase>
                {
                    wait
                };
                var connectors = new List<Connector>();
                AddConnectorFor(controls, connectors, wait, child);
                AddConnectorFor(controls, connectors, waitParent, wait);
                AddConnectorFor(controls, connectors, waitParentParent, waitParent);

                //Act
                CallGetHeatMapStats(controls, ref connectors);

                //Assert
                GetHeatMapStats(wait).ShouldBe(expectedHeatMapStats);
            }
        }

        [TestCase(MarketingAutomationControlType.FormAbandon, false, true, ExpectedHeatMapStats)]
        [TestCase(MarketingAutomationControlType.FormSubmit, false, true, ExpectedHeatMapStats)]
        public void GetHeatMapStats_WaitAndMultipleChildrenOfTypeFormAbandonOrFormSubmit_WillSetHeatMapStats(
            MarketingAutomationControlType childType,
            bool activeOrSentParent,
            bool activeOrSentChild,
            int expectedHeatMapStats)
        {
            //Arrange
            var parentOfParentTypes = new[]
            {
                MarketingAutomationControlType.CampaignItem,
                MarketingAutomationControlType.NoClick,
                MarketingAutomationControlType.Click,
                MarketingAutomationControlType.NoOpen,
                MarketingAutomationControlType.Open_NoClick,
                MarketingAutomationControlType.Open,
                MarketingAutomationControlType.NotSent,
                MarketingAutomationControlType.Sent,
                MarketingAutomationControlType.Suppressed,
                MarketingAutomationControlType.Direct_Click,
                MarketingAutomationControlType.Direct_Open,
                MarketingAutomationControlType.FormSubmit,
                MarketingAutomationControlType.FormAbandon,
                MarketingAutomationControlType.Subscribe,
                MarketingAutomationControlType.Unsubscribe,
                MarketingAutomationControlType.Direct_NoOpen
            };

            var waitParent = GetControl(MarketingAutomationControlType.Form);
            var wait = GetControl(MarketingAutomationControlType.Wait);
            _blastStatusFirstTime = activeOrSentParent;
            _blastStatusSecondTime = activeOrSentChild;

            _campaiginItemBlastId = GetAnyNumber();
            _campaignItemBlastCustomerId = GetAnyNumber();
            _layoutPlansBlastId = GetAnyNumber();
            _sendDiscountCount = expectedHeatMapStats;
            _sendDiscountCount = expectedHeatMapStats;
            _bounceDiscountCount = expectedHeatMapStats;
            _clickthroughDiscountCount = expectedHeatMapStats;
            _notSentDiscountCount = expectedHeatMapStats;
            _noClickDiscountCount = expectedHeatMapStats;
            _noOpenDiscountCount = expectedHeatMapStats;
            _openDiscountCount = expectedHeatMapStats;
            _openNoClickDiscountCount = expectedHeatMapStats;
            _suppressedOpenDiscountCount = expectedHeatMapStats;
            CommonShimsForGetJeatMapStats();
            foreach (var type in parentOfParentTypes)
            {
                var waitParentParent = GetControl(type);
                var controls = new List<ControlBase>
                {
                    wait
                };
                var connectors = new List<Connector>();
                AddConnectorFor(controls, connectors, wait, GetControl(childType));
                AddConnectorFor(controls, connectors, wait, GetControl(childType));
                AddConnectorFor(controls, connectors, waitParent, wait);
                AddConnectorFor(controls, connectors, waitParentParent, waitParent);

                //Act
                CallGetHeatMapStats(controls, ref connectors);

                //Assert
                connectors.ShouldContain(connector => connector.HeatMapStats == expectedHeatMapStats);
            }
        }

        private void CommonShimsForGetJeatMapStats()
        {
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (layoutPlanId, user) =>
            {
                return new CommunicatorEntities.LayoutPlans
                {
                    BlastID = _layoutPlansBlastId
                };
            };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (blatId, getChildren) =>
            {
                return new CommunicatorEntities.CampaignItem
                {
                    CampaignItemID = _campaiginItemId
                };
            };
            ShimBlastActivity.GetBlastMAReportDataByCampaignItemIDInt32String = (campaignItemId, reportType) =>
            {
                var discountCount = 0;
                switch (reportType)
                {
                    case Send:
                        discountCount = _sendDiscountCount;
                        break;
                    case Bounce:
                        discountCount = _bounceDiscountCount;
                        break;
                    case Clickthrough:
                        discountCount = _clickthroughDiscountCount;
                        break;
                    case NoClick:
                        discountCount = _noClickDiscountCount;
                        break;
                    case NoOpen:
                        discountCount = _noOpenDiscountCount;
                        break;
                    case NotSent:
                        discountCount = _notSentDiscountCount;
                        break;
                    case Open:
                        discountCount = _openDiscountCount;
                        break;
                    case OpenNoClick:
                        discountCount = _openNoClickDiscountCount;
                        break;
                    case Suppressed:
                        discountCount = _suppressedOpenDiscountCount;
                        reportType = "Global";
                        break;
                }
                return CreateBlastMAReportData(reportType, discountCount);
            };
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (triggerPlanId, user) =>
            {
                return new CommunicatorEntities.TriggerPlans
                {
                    BlastID = _layoutPlansBlastId
                };
            };
            ShimAutomationBaseController.AllInstances.CurrentUserGet = instance =>
            {
                return new User
                {
                    UserID = GetAnyNumber()
                };
            };
            ShimBlastActivityClicks.GetUniqueByURLStringInt32 = (url, campaignItemId) =>
            {
                return _sendDiscountCount;
            };
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (campaignItemId, getChildren) =>
            {
                return new CommunicatorEntities.CampaignItem
                {
                    BlastList = new List<CommunicatorEntities.CampaignItemBlast>
                    {
                        new CommunicatorEntities.CampaignItemBlast
                        {
                            BlastID = _campaiginItemBlastId,
                            CustomerID = _campaignItemBlastCustomerId
                        }
                    }
                };
            };
            var blastActiveOrSentCallCount = 0;
            ShimBlast.ActiveOrSentInt32Int32 = (blastId, customerId) =>
            {
                ++blastActiveOrSentCallCount;
                if (blastActiveOrSentCallCount > 1)
                {
                    return _blastStatusFirstTime;
                }
                else
                {
                    return _blastStatusSecondTime;
                }
            };
            ShimControlBase.blastLicenseCount_UpdateInt32 = campaignItemId =>
            {
                return _sendDiscountCount;
            };
            ShimBlastSingle.DownloadEmailLayoutPlanID_ProcessedInt32StringUser = (layoutPlanId, processed, user) =>
            {
                var result = new DataTable();
                for (int i = 0; i < _sendDiscountCount; i++)
                {
                    result.Rows.Add(result.NewRow());
                }
                return result;
            };
        }

        private DataTable CreateBlastMAReportData(string activationTypeCode, int distinctCount)
        {
            var table = new DataTable();
            table.Columns.Add(ActionTypeCodeColumn);
            table.Columns.Add(DistinctCountColumn);
            var row = table.NewRow();
            row[ActionTypeCodeColumn] = activationTypeCode;
            row[DistinctCountColumn] = distinctCount;
            table.Rows.Add(row);
            return table;
        }

        private ControlBase GetControl(MarketingAutomationControlType controlType)
        {
            ControlBase result = null;
            switch (controlType)
            {
                case MarketingAutomationControlType.CampaignItem:
                    result = new CampaignItem();
                    break;
                case MarketingAutomationControlType.Click:
                    result = new Click();
                    break;
                case MarketingAutomationControlType.Direct_Click:
                    result = new Direct_Click();
                    break;
                case MarketingAutomationControlType.Direct_NoOpen:
                    result = new Direct_NoOpen();
                    break;
                case MarketingAutomationControlType.Direct_Open:
                    result = new Direct_Open();
                    break;
                case MarketingAutomationControlType.End:
                    result = new End();
                    break;
                case MarketingAutomationControlType.Form:
                    result = new Form();
                    break;
                case MarketingAutomationControlType.FormSubmit:
                    result = new FormSubmit();
                    break;
                case MarketingAutomationControlType.FormAbandon:
                    result = new FormAbandon();
                    break;
                case MarketingAutomationControlType.Group:
                    result = new Group();
                    break;
                case MarketingAutomationControlType.NoClick:
                    result = new NoClick();
                    break;
                case MarketingAutomationControlType.NoOpen:
                    result = new NoOpen();
                    break;
                case MarketingAutomationControlType.NotSent:
                    result = new NotSent();
                    break;
                case MarketingAutomationControlType.Open:
                    result = new Open();
                    break;
                case MarketingAutomationControlType.Open_NoClick:
                    result = new Open_NoClick();
                    break;
                case MarketingAutomationControlType.Sent:
                    result = new Sent();
                    break;
                case MarketingAutomationControlType.Start:
                    result = new Start();
                    break;
                case MarketingAutomationControlType.Subscribe:
                    result = new Subscribe();
                    break;
                case MarketingAutomationControlType.Suppressed:
                    result = new Suppressed();
                    break;
                case MarketingAutomationControlType.Unsubscribe:
                    result = new Unsubscribe();
                    break;
                case MarketingAutomationControlType.Wait:
                    result = new Wait();
                    break;
                default:
                    throw new NotSupportedException("This control type is not supported.");
            }
            result.ControlID = GetUniqueString();
            return result;
        }

        private int? GetHeatMapStats(object control)
        {
            return control.GetType()
                .GetProperty("HeatMapStats")
                ?.GetValue(control) as int?;
        }

        delegate List<ControlBase> GetHeatMapStatsDelegate(List<ControlBase> controls, ref List<Connector> connectors);

        private List<ControlBase> CallGetHeatMapStats(List<ControlBase> controls, ref List<Connector> connectors)
        {
            const string MethodName = "GetHeatMapStats";
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var methodInfo = _diagramsController.GetType()
                .GetMethod(MethodName, flags);
            var methodDelegate = (GetHeatMapStatsDelegate)Delegate.CreateDelegate(typeof(GetHeatMapStatsDelegate),
                _diagramsController, methodInfo);
            methodDelegate.ShouldNotBeNull();
            return methodDelegate(controls, ref connectors);
        }
    }
}
