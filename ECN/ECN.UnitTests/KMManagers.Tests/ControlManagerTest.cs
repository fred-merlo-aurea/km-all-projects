using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Transactions.Fakes;
using System.Web.Configuration.Fakes;
using aspNetMX.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMDbManagers.Fakes;
using KMEntities;
using KMEnums;
using KMManagers.Fakes;
using KMModels;
using KMModels.Controls;
using KMModels.Controls.Standard.Common;
using KMModels.Controls.Standard.Uncommon;
using KMModels.PostModels;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ControlPostModel = KMModels.Controls.Control;
using KMModelsControl = KMModels.Controls;

namespace KMManagers.Tests
{
    /// <summary>
    /// Unit test for <see cref="ControlManager"/> class.
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class ControlManagerTest
    {
        private const string Language = "EN";
        private const string UrlToContentKey = "UrlToContent";
        private const string SubmitFormHandlerUrlKey = "SubmitFormHandlerUrl";
        private const string PrepopulateFromDbHandlerUrlKey = "PrepopulateFromDbHandlerUrl";
        private const string UrlToContent = "UrlToContent";
        private const string SubmitFormHandlerUrl = "SubmitFormHandlerUrl";
        private const string PrepopulateFromDbHandlerUrl = "PrepopulateFromDbHandlerUrl";
        private const string KMCommonapplication = "12345";
        private const int ConfirmPasswordSeqId = 100;
        private const int NumericPropertySeqId = 1000;
        private ControlManager _controlManager;
        private IDisposable _shimsContext;
        private NameValueCollection _appSettings;
        private readonly Random _random = new Random();

        [SetUp]
        public void SetUp()
        {
            _shimsContext = ShimsContext.Create();
            _appSettings = new NameValueCollection();
            CommonShims();
            _controlManager = new ControlManager();
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void Save_FormControlValueToDatabase_ReturnsTrueStatus()
        {
            // Arrange
            var user = new User { UserID = 1, CustomerID = 1, DefaultClientID = 1 };
            var channelID = 1;
            var apiKey = Guid.NewGuid().ToString();
            var model = new FormControlsPostModel { Id = 1, CustomerID = 1 };
            var controls = CreateControlPostModelObject();
            var oldGrid = new List<string>();

            // Act
            var result = _controlManager.Save(user, channelID, apiKey, model, controls, ref oldGrid);

            // Assert
            result.ShouldBeTrue();
            oldGrid.ShouldSatisfyAllConditions
            (
                () => oldGrid.Any().ShouldBeTrue(),
                () => oldGrid.Count.ShouldBe(2),
                () => oldGrid.First().ShouldBe("200"),
                () => oldGrid.Last().ShouldBe("country")
            );
        }

        private List<ControlPostModel> CreateControlPostModelObject()
        {
            var controls = new List<ControlPostModel>();
            // Default Control
            CreateDefaultControlObject(controls);
            // Standard Control
            CreateStandardControlObject(controls);
            return controls;
        }

        private void CreateDefaultControlObject(List<ControlPostModel> controls)
        {
            controls.Add(new TextBox { Required = true, });
            controls.Add(new TextArea { });
            controls.Add(new DropDown
            {
                Categories = CreateControlCategoryObject()
            });
            controls.Add(new RadioButton
            {
                Categories = CreateControlCategoryObject()
            });
            controls.Add(new CheckBox
            {
                Categories = CreateControlCategoryObject()
            });
            controls.Add(new ListBox
            {
                Categories = CreateControlCategoryObject()
            });
            controls.Add(new Grid
            {
                Controls = GridControl.Checkboxes,
                Validation = GridValidation.AtLeastOne,
                Columns = new List<string> { "a", "b", "c" },
                Rows = new List<string> { "1", "2", "3", "4" }
            });
            controls.Add(new PageBreak { Previous = "a", Next = "b" });
            controls.Add(new Hidden
            {
                PopulationType = PopulationType.Querystring,
                Value = string.Empty,
                Parameter = string.Empty
            });
            controls.Add(new Literal { Text = "Unit Test" });
            controls.Add(new NewsLetter
            {
                Categories = CreateControlCategoryObject(),
                Groups = CreateGroupModelObject()
            });
        }

        private void CreateStandardControlObject(List<ControlPostModel> controls)
        {
            controls.Add(new Gender
            {
                Required = true,
                PopulationType = PopulationType.Querystring,
                Parameter = string.Empty
            });
            controls.Add(new Country
            {
                Required = true,
                PopulationType = PopulationType.Querystring,
                Parameter = string.Empty,
                Items = CreateListItemObject()
            });
            controls.Add(new State
            {
                Required = true,
                PopulationType = PopulationType.Querystring,
                Parameter = string.Empty
            });
            controls.Add(new Password
            {
                ConfirmPassword = true,
                ConfirmPasswordLabelHTML = "Unit Test"
            });
            controls.Add(new KMModelsControl.Standard.Common.Email
            {
                AllowChanges = bool.TrueString.ToLower()
            });
            controls.Add(new Fax
            {
                Id = 1,
                Parameter = string.Empty
            });
        }

        private static List<ListItem> CreateListItemObject()
        {
            return new List<ListItem>()
                {
                     new ListItem
                     {
                         CategoryID = 1,
                         CategoryName = "Unit Test",
                         Default = true,
                         Id = 1,
                         Order = 1,
                         Text = "Unit Test",
                         Value = "Unit Test"
                     }
                };
        }

        private List<GroupModel> CreateGroupModelObject()
        {
            return new List<GroupModel>
                {
                    new GroupModel
                    {
                        GroupID = 1,
                        Category = new KMModelsControl.ControlCategory(),
                        CustomerID = 1,
                        Default = true,
                        GroupName = "A",
                        LabelHTML = "A",
                        Order = 1,
                        UDFs = new List<Udf> { new Udf { ShortName = "Unit Test", } }
                    },
                    new GroupModel
                    {
                        GroupID = 2,
                        Category = new KMModelsControl.ControlCategory(),
                        CustomerID = 2,
                        Default = true,
                        GroupName = "A",
                        LabelHTML = "A",
                        Order = 1,
                        UDFs = new List<Udf> { new Udf { ShortName = "Test Test", } }
                    }
                };
        }

        private List<KMModelsControl.ControlCategory> CreateControlCategoryObject()
        {
            return new List<KMModelsControl.ControlCategory>
            {
                 new KMModelsControl.ControlCategory {  CategoryID=1, CategoryName="A"},
                 new KMModelsControl.ControlCategory {  CategoryID=2, CategoryName="B"},
                 new KMModelsControl.ControlCategory {  CategoryID=3, CategoryName="C"},
                 new KMModelsControl.ControlCategory {  CategoryID=-1, CategoryName="A"},
                 new KMModelsControl.ControlCategory {  CategoryID=4, CategoryName="B"},
                 new KMModelsControl.ControlCategory {  CategoryID=5, CategoryName="C"},
            };
        }

        private void CommonShims()
        {
            ShimMXValidate.Constructor = (a) => { };
            ShimMXValidate.StaticConstructor = () => { };
            ShimAPIRunnerBase.StaticConstructor = () => { };
            ShimAPIRunnerBase.Constructor = (x) => { };
            ShimFormManager.AllInstances.GetByIDInt32Int32 = (sender, channelId, userId) =>
            {
                return CreateKMEntitiesFormObject();
            };
            ShimGroupDataFields.SaveGroupDataFieldsUser = (x, y) => { return 1; };
            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 = (groupId) =>
            {
                return CreateGroupDataFieldsObject();
            };
            ShimDbManagerBase.AllInstances.SaveChanges = (x) => { };
            ShimControlDbManager.AllInstances.AddControl = (x, y) => { };
            ShimFormManager.AllInstances.SaveChanges = (x) => { };
            ShimControlManager.AllInstances.CMGet = (x) =>
            {
                return new KMDbManagers.ControlDbManager();
            };
            CreateAppSettingsObject();
            var user = new User();
            ShimHTMLGenerator.StaticConstructor = () => { };
            ShimUser.GetByAccessKeyStringBoolean = (accessKey, getChildren) => { return user; };
            ShimUser.AllInstances.SelectUserInt32Boolean = (instance, userId, includeObjects) => { return user; };
            ShimConfigurationManager.AppSettingsGet = () => { return _appSettings; };
            ShimWebConfigurationManager.AppSettingsGet = () => { return _appSettings; };
            ShimFormManager.AllInstances.SaveChanges = (sender) => { };
            ShimControlDbManager.AllInstances.RemoveControl = (sender, item) => { };
            ShimControlManager.AllInstances.UpdateTextBoxControlControlControlPropertyManager = (sender, edited, m, cpm) => { };
            ShimControlManager.AllInstances.RewriteFormPropertyByNameControlStringIEnumerableOfStringControlPropertyManager = (x, y, z, m, n) => { };
            ShimControlManager.AllInstances.RewriteFormPropertyByNameControlStringStringControlPropertyManager = (x, y, z, n, m) => { };
            ShimControlManager.AllInstances.RewriteRequiredFormPropertyControlStringControlPropertyManager = (x, y, z, m) => { };
            ShimControlManager.AllInstances.RewriteValueFormPropertyControlIEnumerableOfListItemControlPropertyManager = (x, y, z, m) => { };
            ShimControlManager.AllInstances.GetPresetValuesByTypeIDInt32 = (x, y) => { return CreateListItemObject(); };
            ShimControlCategoryDbManager.AllInstances.RemoveAllExceptInt32IEnumerableOfInt32 = (x, y, z) => { };
            ShimControlCategoryDbManager.AllInstances.AddControlCategory = (x, y) => { };
            ShimControlCategoryDbManager.AllInstances.GetByIDInt32 = (x, y) => { return new KMEntities.ControlCategory { }; };
            ShimControlCategoryDbManager.AllInstances.GetByNameInt32String = (x, y, z) => { return new KMEntities.ControlCategory { }; };
            ShimControlCategoryDbManager.AllInstances.RemoveAllInt32 = (x, y) => { };
            ShimControlDbManager.AllInstances.GetByIDInt32 = (x, y) => { return new KMEntities.Control { }; };
            ShimNewsletterGroupUDFDbManager.AllInstances.RemoveAllExceptInt32IEnumerableOfInt32 = (x, y, z) => { };
            ShimNewsletterGroupUDFDbManager.AllInstances.RemoveAllInt32 = (x, y) => { };
            ShimNewsletterGroupDbManager.AllInstances.RemoveAllExceptInt32IEnumerableOfInt32 = (x, y, z) => { };
            ShimNewsletterGroupDbManager.AllInstances.RemoveAllInt32 = (x, y) => { };
            ShimNewsletterGroupDbManager.AllInstances.AddNewsletterGroup = (x, y) => { };
            ShimFormControlPropertyDbManager.AllInstances.AddFormControlProperty = (x, y) => { };
            ShimTransactionScope.AllInstances.Complete = (sender) => { };
            ShimThirdPartyQueryValueDbManager.AllInstances.RemoveThirdPartyQueryValue = (x, y) => { };
            ShimConditionGroupDbManager.AllInstances.RemoveIEnumerableOfConditionGroup = (x, y) => { return new List<int> { 101, 102, 103 }; };
            ShimConditionGroupDbManager.AllInstances.RemoveIEnumerableOfInt32 = (x, y) => { };
            ShimConditionGroupDbManager.AllInstances.GetConditionGroupIDsByControlControl = (x, y) => { return new List<int> { 101 }; };
            ShimRuleDbManager.AllInstances.GetByCondGroupIDsIEnumerableOfInt32 = (x, y) => { return new List<KMEntities.Rule> { new KMEntities.Rule { Control_ID = 1 } }; };
            ShimRuleDbManager.AllInstances.RemoveRule = (x, y) => { };
            ShimRuleDbManager.AllInstances.RemoveIEnumerableOfRule = (x, y) => { };
            ShimNotificationDbManager.AllInstances.RemoveNotification = (x, y) => { };
            ShimNotificationDbManager.AllInstances.RemoveIEnumerableOfInt32 = (x, y) => { };
            ShimConditionGroupDbManager.AllInstances.RemoveIEnumerableOfInt32 = (x, y) => { };
            ShimNotificationDbManager.AllInstances.GetIDsByCondGroupIDsIEnumerableOfInt32 = (x, y) => { return new List<int> { 101, 102, 103 }; };
        }

        private void CreateAppSettingsObject()
        {
            _appSettings.Add("MasterAccessKey", GetString());
            _appSettings.Add("GoogleCapthcaSiteKey", GetString());
            _appSettings.Add(UrlToContentKey, UrlToContent);
            _appSettings.Add(SubmitFormHandlerUrlKey, SubmitFormHandlerUrl);
            _appSettings.Add(PrepopulateFromDbHandlerUrlKey, PrepopulateFromDbHandlerUrl);
            _appSettings.Add("MasterAccessKey", GetString());
            _appSettings.Add("KMCommon_Application", KMCommonapplication);
            _appSettings.Add("CssDir", AppDomain.CurrentDomain.BaseDirectory);
            _appSettings.Add("DefaultCSSPath", AppDomain.CurrentDomain.BaseDirectory);
            _appSettings.Add("ApiDomain", Guid.NewGuid().ToString());
            _appSettings.Add("ApiTimeout", "30");
            _appSettings.Add("aspNetMXLevel", "360");
            _appSettings.Add("aspNetMX.LicenseKey",
                "QENSR-6F3UP-Q2QKF-SNTGA-VVWE1-AEY7X-DJBCM-HYF1A-RCRU7-XDXK6-JY6JT-5NCYC-WLD5Z-6KC3L-SX");
            _appSettings.Add("BlockSubmitIfTimeout", "2");
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }

        private int GetNumber()
        {
            return _random.Next(10, 1000);
        }

        private List<GroupDataFields> CreateGroupDataFieldsObject()
        {
            return new List<GroupDataFields>
                {
                     new GroupDataFields
                     {
                      ShortName="Unit Test"
                     },
                     new GroupDataFields
                     {
                      ShortName="Admin Test"
                     }
                };
        }

        private Form CreateKMEntitiesFormObject()
        {
            var controls = new List<KMEntities.Control>();
            var control = new KMEntities.Control
            {
                Control_ID = 101,
                ThirdPartyQueryValues = new List<KMEntities.ThirdPartyQueryValue>(),
                Rules = new List<KMEntities.Rule> { new KMEntities.Rule { ConditionGroup = new ConditionGroup() } }
            };
            controls.Add(control);
            return new Form
            {
                Active = 1,
                Form_Seq_ID = 1,
                Controls = controls
            };
        }
    }
}
