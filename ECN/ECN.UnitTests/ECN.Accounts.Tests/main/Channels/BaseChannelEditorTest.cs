using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.accounts.channelsmanager;
using ecn.accounts.channelsmanager.Fakes;
using ecn.accounts.includes;
using ecn.accounts.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;
using Telerik.Web.UI.Fakes;
using static ECN_Framework_Common.Objects.Accounts.Enums;
using static KMPlatform.Entity.ServiceFeature;
using MasterPageAccount = ecn.accounts.MasterPages.Accounts;
using PrivateObject = Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject;
using ShimContactEntity = ECN_Framework_Entities.Accounts.Fakes.ShimContact;

namespace ECN.Accounts.Tests.main.Channels
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class BaseChannelEditorTest
    {
        private const int UserId = 10;
        private const int Zero = 0;
        private const string Yes = "yes";
        private const string No = "no";
        private const string ValidateSeparator = "<BR>";
        private IDisposable _shimsContext;
        private basechanneleditor _baseChannelEditor;
        private PrivateObject _baseChannelEditorPrivate;
        private Random _random = new Random();
        private HiddenField _hfBaseChannelPlatformClientGroupID;
        private HiddenField _hfBaseChannelGuid;
        private TextBox _tbChannelName;
        private TextBox _tbChannelURL;
        private TextBox _tbWebAddress;
        private RadioButtonList _rblActive;
        private DropDownList _ddlChannelType;
        private DropDownList _ddlChannelPartnerType;
        private DropDownList _ddlMSCustomer;
        private ContactEditor _contactEditor;
        private Label _lblErrorMessage;
        private RadTreeList _tlClientGroupServiceFeatures;
        private User _user;
        private int _baseChannelId;
        private ClientGroup _clientGroupSelectResult;
        private int _clientGroupSaveResult;
        private ClientGroup _clientGroupPassedToSave;
        private ClientGroupServiceMap _clientGroupServicePassedToSave;
        private int _clientGroupServiceMapSaveResult;
        private int _clientGroupServiceFeatureMapSaveResult;
        private ClientGroupServiceFeatureMap _clientGroupServiceFeatureMapPassedToSave;
        private string _securityGroupTemplateNamePassedToCreateFromTemplate;
        private int _clientGroupIdPassedToCreateFromTemplate;
        private string _administrativeLevelPassedToCreateFromTemplate;
        private User _userPassedToCreateFromTemplate;
        private bool _ecnSessionRemoveFromSessionCalled;
        private Contact _contact;
        private MemoryStream _responseStream;
        private StreamWriter _responseStreamWriter;
        private BaseChannel _baseChannelPassedToSave;
        private string _validateErrorToThrow;
        private Exception _baseChannelSaveException;
        private Exception _clientGroupSaveException;

        [SetUp]
        public void Setup()
        {
            _shimsContext = ShimsContext.Create();
            _baseChannelEditor = new basechanneleditor();
            _baseChannelEditorPrivate = new PrivateObject(_baseChannelEditor);
            _user = new User
            {
                UserID = UserId
            };
            _baseChannelId = default(int);
            _clientGroupSelectResult = null;
            _clientGroupSaveResult = default(int);
            _clientGroupPassedToSave = null;
            _clientGroupServicePassedToSave = null;
            _clientGroupServiceMapSaveResult = default(int);
            _clientGroupServiceFeatureMapSaveResult = default(int);
            _clientGroupServiceFeatureMapPassedToSave = null;
            _securityGroupTemplateNamePassedToCreateFromTemplate = null;
            _clientGroupIdPassedToCreateFromTemplate = default(int);
            _administrativeLevelPassedToCreateFromTemplate = null;
            _userPassedToCreateFromTemplate = null;
            _ecnSessionRemoveFromSessionCalled = default(bool);
            _contact = new Contact();
            _responseStream = new MemoryStream();
            _responseStreamWriter = new StreamWriter(_responseStream);
            _baseChannelPassedToSave = null;
            _validateErrorToThrow = null;
            _baseChannelSaveException = null;
            _clientGroupSaveException = null;
            InitializeFields();
            CommonShims();
        }

        [TearDown]
        public void TearDown()
        {
            _responseStreamWriter.Dispose();
            _responseStream.Dispose();
            _baseChannelEditor.Dispose();
            _shimsContext.Dispose();
        }

        [Test]
        [TestCase(Yes, true)]
        [TestCase(No, false)]
        public void btnSave_Click_IsActive_IsSaved(string selectedValue, bool result)
        {
            //Arrange
            _rblActive.SelectedValue = Yes;

            //Act
            CallbtnSave_Click();

            //Assert
            _clientGroupPassedToSave.IsActive.ShouldBeTrue();
        }

        [Test]
        [TestCase(1, 0, UserId, null, "6AB8B85C-10AF-4F4F-9492-7F5A6021C8C0")]
        [TestCase(1, 0, UserId, null, null)]
        [TestCase(0, 0, null, null, null)]
        [TestCase(0, 1, null, 1, null)]
        public void btnSave_Click_WillUpdateIfNeeded(
            int baseChannelId,
            int customerId,
            int? expectedUserId,
            int? expectedCustomer,
            string baseChannelGuid)
        {
            //Arrange
            _baseChannelId = baseChannelId;
            var item = customerId.ToString();
            _ddlMSCustomer.Items.Add(item);
            _ddlMSCustomer.SelectedValue = item;
            _hfBaseChannelGuid.Value = baseChannelGuid;

            //Act
            CallbtnSave_Click();

            //Assert
            _clientGroupPassedToSave.UpdatedByUserID.ShouldBe(expectedUserId);
            _baseChannelPassedToSave.MSCustomerID.ShouldBe(expectedCustomer);
        }

        [Test]
        public void btnSave_Click_WhenValidateThrowException_WillShowErrorMessage()
        {
            //Arrange
            _validateErrorToThrow = $"{GetAnyString()}{ValidateSeparator}{GetAnyString()}{ValidateSeparator}";

            //Act
            CallbtnSave_Click();

            //Assert
            _lblErrorMessage.Text.ShouldBe(_validateErrorToThrow);
        }

        [Test]
        public void btnSave_Click_SaveException_ShouldThrowExceptin()
        {
            //Arrange
            _hfBaseChannelPlatformClientGroupID.Value = GetAnyNumber().ToString();
            _clientGroupSelectResult = new ClientGroup();
            _baseChannelSaveException = new Exception();

            //Act, Assert
            Assert.That(() => CallbtnSave_Click(), Throws.Exception.InnerException.EqualTo(_baseChannelSaveException));
        }

        [Test]
        public void btnSave_Click_SaveExceptionAndRollback_ShouldThrowAggregateException()
        {
            //Arrange
            _hfBaseChannelPlatformClientGroupID.Value = GetAnyNumber().ToString();
            _clientGroupSelectResult = new ClientGroup();
            _baseChannelSaveException = new Exception();
            _clientGroupSaveException = new Exception();

            //Act, Assert
            Assert.That(() => CallbtnSave_Click(), Throws.Exception.InnerException.TypeOf<AggregateException>());
        }

        [Test]
        public void btnSave_Click_FeatureServiceIdAndFeatureIdAboveZero_SaveFeatureMap()
        {
            //Arrange
            const string ServiceIdColumn = "ServiceID";
            const string FeatureIdCollumn = "ServiceFeatureID";
            const string MapIdColumn = "MAPID";
            const string IsAdditionalCostColumn = "IsAdditionalCost";
            const string IdColumn = "ID";
            var item = new TreeListDataItem(_tlClientGroupServiceFeatures, 0, false);
            var serviceId = GetAnyNumber();
            var featureId = GetAnyNumber();
            var mapId = GetAnyNumber();
            var row = new Dictionary<string, string>
            {
                {
                    ServiceIdColumn ,
                    serviceId.ToString()
                },
                {
                    FeatureIdCollumn,
                    featureId.ToString()
                },
                {
                    MapIdColumn,
                    mapId.ToString()
                },
                {
                    IsAdditionalCostColumn,
                    GetAnyNumber().ToString()
                },
                {
                    IdColumn,
                    $"S{serviceId}"
                }
            };
            item.ExtractValues(row);
            _tlClientGroupServiceFeatures.Items.Add(item);
            item.Selected = true;
            ShimTreeListDataItem.AllInstances.ItemGetString = (instance, key) =>
            {
                return new TableCell { Text = row[key] };
            };

            //Act
            CallbtnSave_Click();

            //Assert
            _clientGroupServiceFeatureMapPassedToSave.ShouldSatisfyAllConditions(
                () => _clientGroupServiceFeatureMapPassedToSave.ShouldNotBeNull(),
                () => _clientGroupServiceFeatureMapPassedToSave.ClientGroupServiceFeatureMapID.ShouldBe(mapId),
                () => _clientGroupServiceFeatureMapPassedToSave.ServiceID.ShouldBe(serviceId),
                () => _clientGroupServiceFeatureMapPassedToSave.ServiceFeatureID.ShouldBe(featureId));
        }

        [Test]
        public void btnSave_Click_FeatureServiceIdAboveZeroAndFeatureIdZero_SaveServiceMap()
        {
            //Arrange
            const string ServiceIdColumn = "ServiceID";
            const string FeatureIdCollumn = "ServiceFeatureID";
            const string MapIdColumn = "MAPID";
            const string IsAdditionalCostColumn = "IsAdditionalCost";
            const string IdColumn = "ID";
            var item = new TreeListDataItem(_tlClientGroupServiceFeatures, 0, false);
            var serviceId = GetAnyNumber();
            var featureId = Zero;
            var mapId = GetAnyNumber();
            var row = new Dictionary<string, string>
            {
                {
                    ServiceIdColumn ,
                    serviceId.ToString()
                },
                {
                    FeatureIdCollumn,
                    featureId.ToString()
                },
                {
                    MapIdColumn,
                    mapId.ToString()
                },
                {
                    IsAdditionalCostColumn,
                    GetAnyNumber().ToString()
                },
                {
                    IdColumn,
                    $"S{serviceId}"
                }
            };
            item.ExtractValues(row);
            _tlClientGroupServiceFeatures.Items.Add(item);
            item.Selected = true;
            ShimTreeListDataItem.AllInstances.ItemGetString = (instance, key) =>
            {
                return new TableCell { Text = row[key] };
            };

            //Act
            CallbtnSave_Click();

            //Assert
            _clientGroupServicePassedToSave.ShouldSatisfyAllConditions(
                () => _clientGroupServicePassedToSave.ShouldNotBeNull(),
                () => _clientGroupServicePassedToSave.ClientGroupServiceMapID.ShouldBe(mapId),
                () => _clientGroupServicePassedToSave.ServiceID.ShouldBe(serviceId));
        }
        
        private void CallbtnSave_Click()
        {
            const string MethodName = "btnSave_Click";
            _baseChannelEditorPrivate.Invoke(MethodName, new object[] { null, null });
        }

        private void CallbtnSave2_Click()
        {
            const string MethodName = "btnSave2_Click";
            _baseChannelEditorPrivate.Invoke(MethodName, new object[] { null, null });
        }

        private void CommonShims()
        {
            ShimUserSession();
            Shimbasechanneleditor.AllInstances.getBaseChannelID = instance => _baseChannelId;
            ShimBaseChannel.ValidateBaseChannelUser = (baseCannel, user) =>
            {
                if (_validateErrorToThrow != null)
                {
                    var errors = _validateErrorToThrow.Split(new[] { ValidateSeparator },
                        StringSplitOptions.RemoveEmptyEntries)
                        .Select(message => new ECNError { ErrorMessage = message })
                        .ToList();
                    throw new ECNException(errors);
                }
            };
            ShimClientGroup.AllInstances.SelectInt32Boolean = (instance, clientGroupId, includeObjects) =>
            {
                return _clientGroupSelectResult;
            };
            var clientGroupSaveCallCount = 0;
            ShimClientGroup.AllInstances.SaveClientGroup = (instance, clientGroup) =>
            {
                ++clientGroupSaveCallCount;
                _clientGroupPassedToSave = clientGroup;
                if (_clientGroupSaveException != null && clientGroupSaveCallCount > 1)
                {
                    throw _clientGroupSaveException;
                }
                return _clientGroupSaveResult;
            };
            ShimClientGroupServiceMap.AllInstances.SaveClientGroupServiceMap = (instance, groupServiceMap) =>
            {
                _clientGroupServicePassedToSave = groupServiceMap;
                return _clientGroupServiceMapSaveResult;
            };
            ShimClientGroupServiceFeatureMap.AllInstances.SaveClientGroupServiceFeatureMap = (instance, map) =>
            {
                _clientGroupServiceFeatureMapPassedToSave = _clientGroupServiceFeatureMapPassedToSave ?? map;
                return _clientGroupServiceFeatureMapSaveResult;
            };
            ShimSecurityGroup.AllInstances.CreateFromTemplateForClientGroupStringInt32StringUser =
                (instance, securityGroupTemplateName, clientGroupId, administrativeLevel, user) =>
                {
                    _securityGroupTemplateNamePassedToCreateFromTemplate = securityGroupTemplateName;
                    _clientGroupIdPassedToCreateFromTemplate = clientGroupId;
                    _administrativeLevelPassedToCreateFromTemplate = administrativeLevel;
                    _userPassedToCreateFromTemplate = user;
                    return GetAnyNumber();
                };
            ShimContactEntity.ConstructorStringStringStringStringStringStringStringStringStringStringStringString =
                (instace, salutation, firstName, lastName, title, phone, fax, email, address, city, state, country,
                    zip) =>
                {
                };
            ShimBaseChannel.SaveBaseChannelUser = (baseChannel, user) =>
            {
                if (_baseChannelSaveException != null)
                {
                    throw _baseChannelSaveException;
                }
                _baseChannelPassedToSave = baseChannel;
            };
            ShimServiceFeature.AllInstances.GetClientGroupTreeListNullableOfInt32Boolean =
                (instance, clientGroupId, isAdditionalCost) =>
                {
                    return new List<ClientGroupTreeListRow>
                    {
                        new ClientGroupTreeListRow
                        {
                            ServiceFeatureID = GetAnyNumber()
                        }
                    };
                };

            ShimPage.AllInstances.ResponseGet = instance =>
            {
                return new System.Web.HttpResponse(_responseStreamWriter);
            };
            ShimHttpResponse.AllInstances.RedirectString = (instance, url) =>
            {
            };
            ShimEmailDirect.SaveEmailDirect = email =>
            {
                return GetAnyNumber();
            };
        }

        private void ShimUserSession()
        {
            ShimPage.AllInstances.MasterGet = instance =>
            {
                return new MasterPageAccount();
            };
            ShimECNSession.Constructor = instance => { };
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructor = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null);
            var session = constructor?.Invoke(new object[0]) as ECNSession;
            if(session!=null)
            {
                session.CurrentUser = _user;
            }
            ShimECNSession.CurrentSession = () => session;
            ShimECNSession.AllInstances.RemoveSession = instance =>
            {
                _ecnSessionRemoveFromSessionCalled = true;
            };
            ShimECNSession.AllInstances.ClearSession = instance => { _ecnSessionRemoveFromSessionCalled = true; };
            Shimbasechanneleditor.AllInstances.MasterGet = (b) => new MasterPageAccount { };
            ShimAccounts.AllInstances.UserSessionGet = (a) => session; 
        }

        private T GetPrivateField<T>(string fieldName) where T : class
        {
            var field = _baseChannelEditorPrivate.GetField(fieldName) as T;
            field.ShouldNotBeNull($"Private field {fieldName} of type {typeof(T).Name} should not be null");
            return field;
        }

        private void InitializeFields()
        {
            InitializeInternalFields(_baseChannelEditor);
            const string HiddenFieldName = "hfBaseChannelPlatformClientGroupID";
            _hfBaseChannelPlatformClientGroupID = GetPrivateField<HiddenField>(HiddenFieldName);
            _hfBaseChannelGuid = GetPrivateField<HiddenField>("hfBaseChannelGuid");
            _tbChannelName = GetPrivateField<TextBox>("tbChannelName");
            _tbChannelURL = GetPrivateField<TextBox>("tbChannelURL");
            _tbWebAddress = GetPrivateField<TextBox>("tbWebAddress");
            _rblActive = GetPrivateField<RadioButtonList>("rblActive");
            _rblActive.Items.Add(Yes);
            _rblActive.Items.Add(No);
            _ddlChannelType = GetPrivateField<DropDownList>("ddlChannelType");
            _ddlChannelType.Items.Add(ChannelType.Unknown.ToString());
            _ddlChannelPartnerType = GetPrivateField<DropDownList>("ddlChannelPartnerType");
            _ddlChannelPartnerType.Items.Add(GetAnyNumber().ToString());
            _ddlMSCustomer = GetPrivateField<DropDownList>("ddlMSCustomer");
            _ddlMSCustomer.Items.Add(GetAnyNumber().ToString());
            _contactEditor = GetPrivateField<ContactEditor>("ContactEditor");
            _lblErrorMessage = GetPrivateField<Label>("lblErrorMessage");
            _tlClientGroupServiceFeatures = GetPrivateField<RadTreeList>("tlClientGroupServiceFeatures");
            InitializeInternalFields(_contactEditor);
            _contactEditor.Contact = _contact;
        }

        private void InitializeInternalFields(object toInitialize)
        {
            Func<string> GetId = () => $"ID__{GetAnyNumber()}";
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var fields = toInitialize.GetType()
                .GetFields(flags)
                .Where(field => field.GetValue(toInitialize) == null);
            foreach (var field in fields)
            {
                var value = field.FieldType
                    .GetConstructor(new Type[0])
                    ?.Invoke(new object[0]);
                if (value != null)
                {
                    var idProperty = field.FieldType.GetProperty("ID");
                    idProperty?.SetValue(value, GetId());
                }
                field.SetValue(toInitialize, value);
            }
        }

        private int GetAnyNumber()
        {
            const int randomRangeMin = 10;
            const int randomRangeMax = randomRangeMin * 100;
            return _random.Next(randomRangeMin, randomRangeMax);
        }

        private string GetAnyString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
