using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using KMManagers.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMEntities;
using KMEnums;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMManagers.Tests.Helpers;

namespace KMManagers.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class FormSubmitterTest
    {
        private IDisposable _shimObject;
        private PrivateObject _formSubmitter;
        private Dictionary<int, string> _values;
        private List<string> _statesValues;
        private Form _form;
        private Guid _dummyGuidValue = Guid.NewGuid();
        private List<GroupDataFields> _groupDataFields;                         
        private const string DummyStringValue = "dummyStringValue";
        private const string DummyIntValue = "360";
        private const string DummyEmailAddress = "dummy@dummy.com";
        private const string EmptyValue = "";
        private const string PrivateFieldValues = "values";
        private const string PrivateFieldForm = "form";
        private const string PopulationTypeDatabase = "2";
        private const int ControlID = 1;
        private const int ControlID_2 = 2;
        private const int ControlID_3 = 3;
        private const int GroupID = 11;
        private const int CustomerID = 111;
        private int ControlPropertySeqID = 71;
        private const int TypeSeqIDForStates = 110;
        private const int TypeSeqIDForCountries = 112;
        private const int TypeSeqIDForMailing = 217;
        private const int TypeSeqIDForMailingPassword = 103;
        private const int TypeSeqIDForLiteral = 11;
        private const int TypeSeqIDForGrid = 7;        
        private const int TypeSeqIDForNewsletter = 10;
        private const int GroupFieldID = 70;        
        private const string GroupFieldShortName = "GroupFieldShortName";
        private const string CustomerAcessKey = "XYZABCD";
        private const string CountryID = "BR";
        private const string CountryName = "Brazil";
        private const int CountryNumber = 55;
        private const string RegionName = "Rio de Janeiro";
        private const string RegionID = "RJ";
        private const string DifferentRegionID = "SP";
        private const string RegionCode = "RJA";
        private const string StateItem = CountryID + "|" + RegionID + "|" + RegionName;
        private const int HtmlControltypeAsNewsLetter = (int)KMEnums.ControlType.NewsLetter;
        private const int HtmlControltypeAsCheckbox = (int)KMEnums.ControlType.CheckBox;
        private const int HtmlControltypeAsRadioButton = (int)KMEnums.ControlType.RadioButton;
        private const int HtmlControltypeAsListbox = (int)KMEnums.ControlType.ListBox;
        private const int HtmlControltypeAsDropDown = (int)KMEnums.ControlType.DropDown;
        private const string MailingProfilePswdColumnValue = "PasssowrdDummyValue";
        private const string MailingProfilePswdColumnName = "Password";
        private const string SnippetForLiteral = "Literal";
        private const string SnippetForGrid = "Grid";

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();

            KMManagerTestsHelper.CreateWebAppSettingsShim();

            KMManagerTestsHelper.CreateAppSettingsShim();

            ShimDirectory.ExistsString =
                (path) =>
                {
                    return true;
                };

            ShimStreamReader.ConstructorString =
                (stream, path) =>
                {
                    var shim = new ShimStreamReader(stream);
                    shim.ReadToEnd = () => DummyStringValue;
                    shim.Close = () => { };
                };

            KMManagerTestsHelper.CreateMXValidateShim();

            ShimControlManager.Constructor =
                (controlManger) =>
                {
                    var shim = new ShimControlManager(controlManger);
                    shim.GetStatesAll = () => _statesValues;
                    shim.GetStateCodeInt32 = (code) => RegionCode;
                    shim.GetCountryNameInt32 = (code) => CountryName;
                    shim.GetStateNameString = (code) => RegionName;
                };

            ShimLogger.WriteLogString = (value) => { };

            _groupDataFields = new List<GroupDataFields>();
            KMManagerTestsHelper.CreateFormManagerShim(_groupDataFields);

            ShimUser.GetByAccessKeyStringBoolean =
                (accesKey, getChildren) =>
                {
                    return new KMPlatform.Entity.User();
                };
            
            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 =
                (groupId) =>
                {
                    return new List<GroupDataFields>();
                };

            KMManagerTestsHelper.CreateEmailGroupShim();

            ShimLayoutPlans.GetByGroupID_NoAccessCheckInt32Int32 = 
                (groupID, customerID) => 
                {
                    return new List<LayoutPlans>();
                };
            
            ShimLayoutPlans.GetByFormTokenUID_NoAccessCheckGuid =
                (uid) =>
                {
                    return new List<LayoutPlans>();
                };

            KM.Common.Entity.Fakes.ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethhod, applicationID, arg1, arg2, arg3) =>
                {
                    return 1;
                };

            KMManagerTestsHelper.CreateControlPropertyManagerShim(ControlPropertySeqID);

            _values = new Dictionary<int, string>();
            _statesValues = new List<string>();
            _form = KMManagerTestsHelper.GetForm();

            var formSubmitter = new FormSubmitter();
            _formSubmitter = new PrivateObject(formSubmitter);
            _formSubmitter.SetFieldOrProperty(PrivateFieldValues, _values);
            _formSubmitter.SetFieldOrProperty(PrivateFieldForm, _form);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private static List<Control> CreateControls(int controlID, string fieldLabelValue="", int? mainTypeId = 0, int typeSeqID = 0, int? fieldID = null, string htmlIDGuid = "")
        {
           return (List<Control>)KMManagerTestsHelper.CreateControls(controlID, fieldLabelValue, mainTypeId, typeSeqID, fieldID, htmlIDGuid);
        }

        private static Control CreateControl(int controlID, string fieldLabelValue = "", int? mainTypeId = 0, int typeSeqID = 0,int? fieldID = null, string htmlIDGuid = "")
        {
            return KMManagerTestsHelper.CreateControl(controlID, fieldLabelValue, mainTypeId, typeSeqID, fieldID, htmlIDGuid); 
        }

        private Condition CreateTestCondition(string controlValueToMatch, ComparisonType comparisonType, int htmlControltype = 0, int typeSeqID = 0)
        {
            var control = CreateControl(controlID: ControlID,
                                        mainTypeId: htmlControltype,
                                        typeSeqID: typeSeqID);

            var condition = new Condition()
            {
                Control = control,
                Value = controlValueToMatch,
                Operation_ID = (int)comparisonType
            };
            return condition;
        }

        private Condition CreateTestConditionForHTMLControl(string controlValueToMatch, ComparisonType comparisonType, int htmlControltype, int typeSeqID = 0)
        {
            var condition = CreateTestCondition(controlValueToMatch, comparisonType, htmlControltype, typeSeqID);

            return condition;
        }

        private static void ChangeControlToAddNewsletterGroup(Control control, string labelHtml = "", int? groupID = null)
        {
            KMManagerTestsHelper.ChangeControlToAddNewsletterGroup(control, labelHtml);
        }
        private GroupDataFields CreateGroupDataField()
        {  
            return KMManagerTestsHelper.CreateGroupDataField(GroupID,GroupFieldShortName);
        }
    }
}
