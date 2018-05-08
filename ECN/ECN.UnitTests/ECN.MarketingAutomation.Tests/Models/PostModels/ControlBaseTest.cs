using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using ECN.MarketingAutomation.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using ControlType = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType;
using EntitesForm = ECN_Framework_Entities.FormDesigner.Form;
using Entities = ECN_Framework_Entities.Communicator;

namespace ECN.MarketinAutomation.Tests.Models.PostModels
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ControlBaseTest
    {
        private IDisposable _context;
        private List<MAControl> _controls;
        private List<MAConnector> _connectors;
        private List<ControlBase> _originalControls;
        private User _user;
        private List<ControlBase> _resultControlBaseList;
        private List<ControlBase> _baseControls;
        private List<Connector> _jsonConnectors;
        private const string StartWith = "{\"shapes\"";
        private const string Id = "15";

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [TestCase(ControlType.CampaignItem)]
        [TestCase(ControlType.Click)]
        [TestCase(ControlType.Direct_Click)]
        [TestCase(ControlType.Form)]
        [TestCase(ControlType.FormAbandon)]
        [TestCase(ControlType.FormSubmit)]
        [TestCase(ControlType.Direct_Open)]
        [TestCase(ControlType.Direct_NoOpen)]
        [TestCase(ControlType.End)]
        [TestCase(ControlType.Group)]
        [TestCase(ControlType.NoClick)]
        [TestCase(ControlType.NoOpen)]
        [TestCase(ControlType.NotSent)]
        [TestCase(ControlType.Open)]
        [TestCase(ControlType.Open_NoClick)]
        [TestCase(ControlType.Sent)]
        [TestCase(ControlType.Start)]
        [TestCase(ControlType.Subscribe)]
        [TestCase(ControlType.Suppressed)]
        [TestCase(ControlType.Unsubscribe)]
        [TestCase(ControlType.Wait)]
        public void GetModelsFromObject_WhenControlsAreProvided_ResultContainsModelsForGivenControl(ControlType controlType)
        {
            // Arrange
            Initialize();
            var _mControl = CreateInstanceWithValues(
                typeof(MAControl),
                new
                {
                    ControlType = controlType,
                    ControlID = "1"
                });

            _controls.Add(_mControl);
            var _cBase = CreateInstanceWithValues(
                typeof(Form),
                new
                {
                    ControlType = controlType,
                    ControlID = "1",
                    ControlTypeAsString = controlType
                });
            _originalControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(MAConnector),
                new
                {
                    From = "1",
                    To = "1",
                });
            _connectors.Add(_connector);

            // Act
            _resultControlBaseList = ControlBase.GetModelsFromObject(_controls, _connectors, _originalControls, _user);

            //Assert
            var resultItem = _resultControlBaseList.Where(x => x.ControlType == controlType).First();
            _resultControlBaseList.ShouldSatisfyAllConditions(
                () => _resultControlBaseList.ShouldNotBeNull(),
                () => _resultControlBaseList.ShouldNotBeEmpty(),
                () => _resultControlBaseList.ShouldContain(resultItem));
        }

        [Test]
        public void GetModelsFromObject_WhenControlsAreNotProvided_ResultIsEmpty()
        {
            // Arrange
            Initialize();

            // Act
            _resultControlBaseList = ControlBase.GetModelsFromObject(_controls, _connectors, _originalControls, _user);

            //Assert
            _resultControlBaseList.ShouldSatisfyAllConditions(
                       () => _resultControlBaseList.ShouldNotBeNull(),
                       () => _resultControlBaseList.ShouldBeEmpty());
        }

        [TestCase(ControlType.CampaignItem)]
        [TestCase(ControlType.Click)]
        [TestCase(ControlType.Direct_Click)]
        [TestCase(ControlType.Form)]
        [TestCase(ControlType.FormAbandon)]
        [TestCase(ControlType.FormSubmit)]
        [TestCase(ControlType.Direct_Open)]
        [TestCase(ControlType.Direct_NoOpen)]
        [TestCase(ControlType.End)]
        [TestCase(ControlType.NoClick)]
        [TestCase(ControlType.NoOpen)]
        [TestCase(ControlType.NotSent)]
        [TestCase(ControlType.Open)]
        [TestCase(ControlType.Open_NoClick)]
        [TestCase(ControlType.Sent)]
        [TestCase(ControlType.Start)]
        [TestCase(ControlType.Subscribe)]
        [TestCase(ControlType.Suppressed)]
        [TestCase(ControlType.Unsubscribe)]
        [TestCase(ControlType.Wait)]
        public void GetModelsFromObject_WhenControlIsNotActiveOrSent_ResultModelIsEditableAndRemovable(ControlType controlType)
        {
            // Arrange
            Initialize();
            var _mControl = CreateInstanceWithValues(
                typeof(MAControl),
                new
                {
                    ControlType = controlType,
                    IsCancelled = false,
                    ControlID = "1"
                });

            _controls.Add(_mControl);
            var _cBase = CreateInstanceWithValues(
                typeof(Form),
                new
                {
                    ControlType = controlType,
                    ControlID = "1",
                    ControlTypeAsString = controlType
                });
            _originalControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(MAConnector),
                new
                {
                    From = "1",
                    To = "1",
                });
            _connectors.Add(_connector);
            ShimControlBase.IsActiveOrSentInt32Int32 = (x, y) => { return false; };
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (x, y) =>
            {
                return ReflectionHelper.CreateInstanceWithValues(typeof(TriggerPlans), new { Status = "y" });
            };
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (x, y) =>
            {
                return ReflectionHelper.CreateInstanceWithValues(typeof(LayoutPlans), new { Status = "y" });
            };
            ShimBlastSingle.ExistsByBlastIDInt32Int32 = (x, y) => { return false; };
            var editable = false;

            // Act
            _resultControlBaseList = ControlBase.GetModelsFromObject(_controls, _connectors, _originalControls, _user);

            //Assert
            var resultItem = _resultControlBaseList.Where(x => x.ControlType == controlType).First();
            editable = resultItem.editable.remove;
            _resultControlBaseList.ShouldSatisfyAllConditions(
                () => _resultControlBaseList.ShouldNotBeNull(),
                () => _resultControlBaseList.ShouldNotBeEmpty(),
                () => _resultControlBaseList.ShouldContain(resultItem),
                () => editable.ShouldBeTrue());
        }

        [TestCase(ControlType.CampaignItem)]
        [TestCase(ControlType.Click)]
        [TestCase(ControlType.Direct_Click)]
        [TestCase(ControlType.FormAbandon)]
        [TestCase(ControlType.FormSubmit)]
        [TestCase(ControlType.Direct_Open)]
        [TestCase(ControlType.Direct_NoOpen)]
        [TestCase(ControlType.NoClick)]
        [TestCase(ControlType.NoOpen)]
        [TestCase(ControlType.NotSent)]
        [TestCase(ControlType.Open)]
        [TestCase(ControlType.Open_NoClick)]
        [TestCase(ControlType.Sent)]
        [TestCase(ControlType.Subscribe)]
        [TestCase(ControlType.Suppressed)]
        [TestCase(ControlType.Unsubscribe)]
        public void GetModelsFromObject_WhenControlsCampaignItemTemplateDoesNotExist_ResultModelsCampaignItemTemplateHasNoName(ControlType controlType)
        {
            // Arrange
            Initialize();
            var _mControl = CreateInstanceWithValues(
                typeof(MAControl),
                new
                {
                    ControlType = controlType,
                    ControlID = "1"
                });

            _controls.Add(_mControl);
            var _cBase = CreateInstanceWithValues(
                typeof(Form),
                new
                {
                    ControlType = controlType,
                    ControlID = "1",
                    ControlTypeAsString = controlType
                });
            _originalControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(MAConnector),
                new
                {
                    From = "1",
                    To = "1",
                });
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (cItemId, user, getChildren) =>
            {
                var properties = new
                {
                    CampaignItemID = 1,
                    CustomerID = 1,
                    CampaignID = 1,
                    SendTime = DateTime.Now,
                    ControlTypeAsString = ControlType.CampaignItem,
                    CampaignItemTemplateID = -1
                };
                return CreateInstanceWithValues(typeof(Entities.CampaignItem), properties);
            };
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                var properties = new
                {
                    CampaignItemID = 1,
                    CustomerID = 1,
                    CampaignID = 1,
                    SendTime = DateTime.Now,
                    ControlTypeAsString = ControlType.CampaignItem,
                    CampaignItemTemplateID = -1
                };
                var instance = CreateInstanceWithValues(typeof(Entities.CampaignItem), properties);
                return instance;
            };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                return CreateInstanceWithValues(typeof(Entities.CampaignItem), new
                {
                    CampaignItemTemplateID = -1
                });
            };
            _connectors.Add(_connector);
            var campaignItemTemplateName = "test String";

            // Act
            _resultControlBaseList = ControlBase.GetModelsFromObject(_controls, _connectors, _originalControls, _user);

            //Assert
            var resultItem = _resultControlBaseList.Where(x => x.ControlType == controlType).First();
            campaignItemTemplateName = (string)ReflectionHelper.GetPropertyValue(resultItem, "CampaignItemTemplateName");

            _resultControlBaseList.ShouldSatisfyAllConditions(
                       () => _resultControlBaseList.ShouldNotBeNull(),
                       () => _resultControlBaseList.ShouldNotBeEmpty(),
                       () => _resultControlBaseList.ShouldContain(resultItem),
                       () => campaignItemTemplateName.ShouldBeEmpty());
        }

        [TestCase(ControlType.Direct_Click)]
        [TestCase(ControlType.FormAbandon)]
        [TestCase(ControlType.FormSubmit)]
        public void GetModelsFromObject_WhenControlHasHttpLink_ResultModelsContainsActualLink(ControlType controlType)
        {
            // Arrange
            Initialize();
            var _mControl = CreateInstanceWithValues(
                typeof(MAControl),
                new
                {
                    ControlType = controlType,
                    ControlID = "1"
                });

            _controls.Add(_mControl);
            var _cBase = CreateInstanceWithValues(
                typeof(Form),
                new
                {
                    ControlType = controlType,
                    ControlID = "1",
                    ControlTypeAsString = controlType
                });
            _originalControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(MAConnector),
                new
                {
                    From = "1",
                    To = "1",
                });
            _connectors.Add(_connector);
            var criteria = "http://test.com";
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (x, y) =>
            {
                return CreateInstanceWithValues(typeof(LayoutPlans), new
                {
                    Criteria = criteria
                });
            };
            var actualLink = "";
            // Act
            _resultControlBaseList = ControlBase.GetModelsFromObject(_controls, _connectors, _originalControls, _user);

            //Assert
            var resultItem = _resultControlBaseList.Where(x => x.ControlType == controlType).First();
            actualLink = (string)ReflectionHelper.GetPropertyValue(resultItem, "ActualLink");

            _resultControlBaseList.ShouldSatisfyAllConditions(
                       () => _resultControlBaseList.ShouldNotBeNull(),
                       () => _resultControlBaseList.ShouldNotBeEmpty(),
                       () => _resultControlBaseList.ShouldContain(resultItem),
                       () => actualLink.ShouldNotBeEmpty(),
                       () => actualLink.ShouldBe(criteria));
        }

        [TestCase(ControlType.Wait, ControlType.Click)]
        [TestCase(ControlType.Wait, ControlType.Direct_Click)]
        [TestCase(ControlType.Wait, ControlType.Direct_NoOpen)]
        public void GetModelsFromObject_WhenWaitControlHasChildControls_ResultModelsTimeIsChanged(ControlType controlType, ControlType childType)
        {
            // Arrange
            Initialize();
            var _mControl = CreateInstanceWithValues(
                typeof(MAControl),
                new
                {
                    ControlType = controlType,
                    ControlID = "1"
                });
            _controls.Add(_mControl);
            var _cBase = CreateInstanceWithValues(
                typeof(Form),
                new
                {
                    ControlType = controlType,
                    ControlID = "1",
                    ControlTypeAsString = controlType
                });
            _originalControls.Add(_cBase);
            var _connector = CreateInstanceWithValues(
                typeof(MAConnector),
                new
                {
                    From = "1",
                    To = "1",
                });
            _connectors.Add(_connector);
            var dateParam = DateTime.ParseExact(
                "2009-05-08 14:40:52,531",
                "yyyy-MM-dd HH:mm:ss,fff",
                CultureInfo.InvariantCulture);
            ShimControlBase.GetChildControlBaseListOfMAControlListOfMAConnector = (x, y, z) =>
            {
                return new MAControl() { ControlType = childType, ECNID = 1 };
            };
            ShimControlBase.GetParentControlBaseListOfMAControlListOfMAConnector = (x, y, z) =>
            {
                return new MAControl() { ControlType = childType };
            };
            ShimControlBase.GetParentCIControlBaseListOfMAControlListOfMAConnector = (x, y, z) =>
            {
                return new MAControl() { ControlType = childType };
            };
            var addedDays = 1;
            var addedHours = 0;
            var addedMinutes = 0;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (cItemId, user, getChildren) =>
            {
                if (cItemId < 0)
                {
                    var properties = new
                    {
                        CampaignItemID = 1,
                        CustomerID = 1,
                        CampaignID = 1,
                        SendTime = dateParam,
                        ControlTypeAsString = ControlType.CampaignItem
                    };
                    return CreateInstanceWithValues(typeof(Entities.CampaignItem), properties);
                }
                else
                {
                    var properties = new
                    {
                        CampaignItemID = 1,
                        CustomerID = 1,
                        CampaignID = 1,
                        SendTime = dateParam.
                        AddDays(addedDays).
                        AddHours(addedHours).
                        AddMinutes(addedMinutes),
                        ControlTypeAsString = ControlType.CampaignItem
                    };
                    return CreateInstanceWithValues(typeof(Entities.CampaignItem), properties);
                }
            };
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (x, y) =>
            {
                Decimal? tempPeriod = addedDays;
                var lp = CreateInstanceWithValues(typeof(LayoutPlans), new
                {
                    Period = tempPeriod
                });
                return lp;
            };
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (x, y) =>
            {
                Decimal? tempPeriod = addedDays;
                var tp = CreateInstanceWithValues(typeof(TriggerPlans), new
                {
                    Period = tempPeriod
                });
                return tp;
            };

            // Act
            _resultControlBaseList = ControlBase.GetModelsFromObject(_controls, _connectors, _originalControls, _user);

            //Assert
            var timeChanged = false;
            var resultItem = _resultControlBaseList.Where(x => x.ControlType == controlType).First();
            var daysDiff = (int?)ReflectionHelper.GetPropertyValue(resultItem, "Days") ?? 0;
            var hoursDiff = (int?)ReflectionHelper.GetPropertyValue(resultItem, "Hours") ?? 0;
            var minutesDiff = (int?)ReflectionHelper.GetPropertyValue(resultItem, "Minutes") ?? 0;
            timeChanged = (bool)ReflectionHelper.GetPropertyValue(resultItem, "TimeChanged");
            _resultControlBaseList.ShouldSatisfyAllConditions(
                       () => _resultControlBaseList.ShouldNotBeNull(),
                       () => _resultControlBaseList.ShouldNotBeEmpty(),
                       () => _resultControlBaseList.ShouldContain(resultItem),
                       () => daysDiff.ShouldBe(addedDays),
                       () => hoursDiff.ShouldBe(addedHours),
                       () => minutesDiff.ShouldBe(addedMinutes),
                       () => timeChanged.ShouldBeTrue());
        }

        [Test]
        public void GetJSONDiagramFromControls_ForClick_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Click),
                new
                {
                    ControlType = ControlType.Click,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Click
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForDirectClick_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Direct_Click),
                new
                {
                    ControlType = ControlType.Direct_Click,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Direct_Click
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForForm_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Form),
                new
                {
                    ControlType = ControlType.Form,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Form
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForFormAbandon_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(FormAbandon),
                new
                {
                    ControlType = ControlType.FormAbandon,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.FormAbandon
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForFormSubmit_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(FormSubmit),
                new
                {
                    ControlType = ControlType.FormSubmit,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.FormSubmit
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForDirectOpen_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Direct_Open),
                new
                {
                    ControlType = ControlType.Direct_Open,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Direct_Open
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForDirectNoOpen_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Direct_NoOpen),
                new
                {
                    ControlType = ControlType.Direct_NoOpen,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Direct_NoOpen
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForEnd_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(End),
                new
                {
                    ControlType = ControlType.End,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.End
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForGroup_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(ecn.MarketingAutomation.Models.PostModels.Controls.Group),
                new
                {
                    ControlType = ControlType.Group,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Group
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForNoClick_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(NoClick),
                new
                {
                    ControlType = ControlType.NoClick,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.NoClick
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForNoOpen_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(NoOpen),
                new
                {
                    ControlType = ControlType.NoOpen,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.NoOpen
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForNotSent_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(NotSent),
                new
                {
                    ControlType = ControlType.NotSent,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.NotSent
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForOpen_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Open),
                new
                {
                    ControlType = ControlType.Open,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Open
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForOpenNoClick_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Open_NoClick),
                new
                {
                    ControlType = ControlType.Open_NoClick,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Open_NoClick
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForSent_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Sent),
                new
                {
                    ControlType = ControlType.Sent,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Sent
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForStart_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Start),
                new
                {
                    ControlType = ControlType.Start,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Start
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForSubscribe_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Subscribe),
                new
                {
                    ControlType = ControlType.Subscribe,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Subscribe
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForSuppressed_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Suppressed),
                new
                {
                    ControlType = ControlType.Suppressed,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Suppressed
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForUnSubscribe_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Unsubscribe),
                new
                {
                    ControlType = ControlType.Unsubscribe,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Unsubscribe
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForWait_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(Wait),
                new
                {
                    ControlType = ControlType.Wait,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.Wait
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForNullCampaignItem_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(ecn.MarketingAutomation.Models.PostModels.Controls.CampaignItem),
                new
                {
                    ControlType = ControlType.CampaignItem,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.CampaignItem,
                    CreateCampaignItem = false
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);
            SetUpJSON(false);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        [Test]
        public void GetJSONDiagramFromControls_ForCampaignItem_ShouldReturnJSONString()
        {
            // Arrange
            SetDefaultsForJSON();
            var _cBase = CreateInstanceWithValues(
                typeof(ecn.MarketingAutomation.Models.PostModels.Controls.CampaignItem),
                new
                {
                    ControlType = ControlType.CampaignItem,
                    ControlID = Id,
                    ControlTypeAsString = ControlType.CampaignItem,
                    CreateCampaignItem = false
                });

            _baseControls.Add(_cBase);

            var _connector = CreateInstanceWithValues(
                typeof(Connector),
                new
                {
                    id = Id
                });

            _jsonConnectors.Add(_connector);
            SetUpJSON(true);

            // Act
            var result = ControlBase.GetJSONDiagramFromControls(_baseControls, _jsonConnectors, 10);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldStartWith(StartWith));
        }

        private dynamic CreateInstanceWithValues(Type type, dynamic values)
        {
            return ReflectionHelper.CreateInstanceWithValues(type, values);
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private void CreateCommonShims()
        {

            ShimCampaignItemTemplate.Get_CampaignItemTemplateIDInt32UserBoolean = (a, b, c) =>
            {
                var properties = new
                {
                    TemplateName = "sample template name",
                };
                return CreateInstanceWithValues(typeof(Entities.CampaignItemTemplate), properties);
            };

            ShimCampaign.GetByCampaignIDInt32UserBoolean = (campaignID, user, getChildren) =>
            {
                return new Campaign()
                {
                    CustomerID = 1,
                    CampaignID = 1,
                    CampaignName = "campaign name"
                };
            };
            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (cItemID, user, getChildren) =>
            {
                CampaignItemBlast cibObj = CreateInstance(typeof(CampaignItemBlast));
                cibObj.Blast = CreateInstance(typeof(BlastLayout));
                var cib = new List<CampaignItemBlast>() { cibObj };
                return cib;
            };
            ShimCustomer.GetByCustomerIDInt32Boolean = (customerID, getChildren) =>
            {
                return new Customer();
            };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                return CreateInstance(typeof(Layout));
            };
            ShimControlBase.FindParentCIControlBaseListOfMAControlListOfMAConnector = (ci, controls, connectors) => { return new MAControl(); };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (x, y) => { return CreateInstance(typeof(Layout)); };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) =>
            {
                return CreateInstanceWithValues(typeof(Entities.Group), new
                {
                    ControlType = ControlType.CampaignItem
                });
            };
        }
        private void CreateSpecialShims()
        {
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (cItemId, user, getChildren) =>
            {
                var properties = new
                {
                    CampaignItemID = 1,
                    CustomerID = 1,
                    CampaignID = 1,
                    SendTime = DateTime.Now,
                    ControlTypeAsString = ControlType.CampaignItem
                };
                return CreateInstanceWithValues(typeof(Entities.CampaignItem), properties);
            };
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                var properties = new
                {
                    CampaignItemID = 1,
                    CustomerID = 1,
                    CampaignID = 1,
                    SendTime = DateTime.Now,
                    ControlTypeAsString = ControlType.CampaignItem
                };
                var ci = CreateInstanceWithValues(typeof(Entities.CampaignItem), properties);
                return ci;
            };
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (x, y) =>
            {
                return CreateInstanceWithValues(typeof(LayoutPlans), new { });
            };
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                return CreateInstanceWithValues(typeof(Entities.BlastRegular), new { });
            };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                return CreateInstanceWithValues(typeof(Entities.CampaignItem), new { });
            };
            ShimForm.GetByFormID_NoAccessCheckInt32 = (x) =>
            {
                return CreateInstanceWithValues(
                    typeof(EntitesForm),
                    new { Name = "sample Form name" }
                    );
            };
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (x, y) =>
            {
                return CreateInstanceWithValues(typeof(TriggerPlans), new { });
            };
        }

        private void SetDefaults()
        {
            _controls = new List<MAControl>();
            _connectors = new List<MAConnector>();
            _originalControls = new List<ControlBase>();
            _user = new User();
        }

        private void SetDefaultsForJSON()
        {
            _baseControls = new List<ControlBase>();
            _jsonConnectors = new List<Connector>();
        }

        private void Initialize()
        {
            BasicShims.CreateShims();
            CreateCommonShims();
            CreateSpecialShims();
            SetDefaults();
        }

        private void SetUpJSON(bool param)
        {
            _context = ShimsContext.Create();
            if (param)
            {
                ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) =>
                {
                    return CreateInstanceWithValues(typeof(Entities.CampaignItem), new { });
                };
            }
            else
            {
                ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) =>
                {
                    return null;
                };
            }
        }
    }
}