using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.Configuration.Fakes;
using System.Configuration.Fakes;
using KMEntities;
using KMManagers.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using aspNetMX.Fakes;
using KMDbManagers.Fakes;

namespace KMManagers.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class KMManagerTestsHelper
    {
        private static Form _form;
        private static Guid _dummyGuidValue = Guid.NewGuid();
        private static bool _formManagerChangesAreSaved = false;
        public const int CustomerID = 111;
        public const int GroupID = 11;        
        public const string CustomerAcessKey = "XYZABCD";
        public const string SourceKey = "Source";
        public const string ConfirmationTemplatePathKey = "ConfirmationTemplatePath";
        public const string DoubleOptInEmailTemplateKey = "DoubleOptInEmailTemplate";
        public const string GCSecret = "GoogleCaptchaSecret";
        public const string KMRootPathKey = "KMRoot_Path";
        public const string AspNetLicenseKey = "aspNetMX.LicenseKey";
        public const string AppSettingKmCommon = "KMCommon";
        public const string ApiDomainKey = "ApiDomain";
        public const string ApiTimeoutKey = "ApiTimeout";
        public const string AspNetMxLevelKey = "aspNetMXLevel";
        public const string BlockSubmitIfTimeoutKey = "BlockSubmitIfTimeout";
        public const string MasterAccessKey = "MasterAccessKey";
        public const string KMCommonApplicationKey = "KMCommon_Application";
        public const string DummyAppConfigValue = "appCofnigvalue";
        public const string DummyFilePathValue = "/";
        public const string DummyStringValue = "dummyStringValue";
        public const string DummyLicenseValue = "aaaa-bbbb-cccc-dddd";
        public const string DummyEmailAddress = "dummy@dummy.com";
        public const string DummyIntValue = "360";
        public const string GroupIDColumnName = "GroupID";
        public const string SubscribeTypeCodeColumnName = "SubscribeTypeCode";
        public const string MailingProfilePswdColumnValue = "PasssowrdDummyValue";
        public const string MailingProfilePswdColumnName = "Password";
        public static bool FormManagerChangesAreSaved { get => _formManagerChangesAreSaved; set => _formManagerChangesAreSaved = value; }

        public static Form GetForm()
        {
            _form = new Form()
            {
                GroupID = GroupID,
                CustomerID = CustomerID,
                CustomerAccessKey = CustomerAcessKey,
                TokenUID = _dummyGuidValue
            };
            return _form;
        }

        public static void CreateAppSettingsShim()
        {
            ShimConfigurationManager.AppSettingsGet =
               () => new NameValueCollection
               {
                    {MasterAccessKey,  CustomerAcessKey },
                    {KMCommonApplicationKey,  DummyIntValue },
                    {AppSettingKmCommon, AppSettingKmCommon }
               };
        }

        public static void CreateWebAppSettingsShim()
        {
            ShimWebConfigurationManager.AppSettingsGet =
                () => new NameValueCollection
                {
                    { GCSecret, DummyAppConfigValue },
                    { SourceKey, DummyAppConfigValue },
                    { ConfirmationTemplatePathKey, DummyFilePathValue },
                    { DoubleOptInEmailTemplateKey, DummyAppConfigValue },
                    { KMRootPathKey, DummyAppConfigValue },
                    { AspNetLicenseKey, DummyLicenseValue },
                    { ApiDomainKey, DummyAppConfigValue },
                    { ApiTimeoutKey, DummyIntValue },
                    { AspNetMxLevelKey, DummyIntValue },
                    { BlockSubmitIfTimeoutKey, DummyStringValue},
                };
        }

        public static void CreateFormManagerShim(List<GroupDataFields> groupDataFields, Form form = null)
        {
            ShimFormManager.Constructor =
               (formManager) =>
               {
                   var shim = new ShimFormManager(formManager)
                   {
                       GetFieldsByFormForm = (_form) => { return groupDataFields; },
                       GetByIDInt32Int32 = (chanelId, id) => { return form; },
                       SaveChanges = () => { FormManagerChangesAreSaved = true; }
                   };
               };
        }

        public static void CreateControlPropertyManagerShim(int controlPropertySeqID)
        {
            ShimControlPropertyManager.Constructor =
                (controlPropertyManager) =>
                {
                    var shim = new ShimControlPropertyManager(controlPropertyManager)
                    {
                        GetPropertyByNameAndControlStringControl =
                        (name, control) =>
                        {
                            return new ControlProperty() { ControlProperty_Seq_ID = controlPropertySeqID };
                        },
                        GetRequiredPropertyByControlControl = (control) => new ControlProperty
                        {
                            ControlProperty_Seq_ID = controlPropertySeqID
                        }
                    };
                    
                };
        }

        public static void CreateEmailGroupShim()
        {
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString =
                (user, customerID, groupID, profile, uds, type, isSubscribe, boolParam, stringParam, source) =>
                {
                    return new DataTable();
                };

            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 =
                (emailAddres, groupID) =>
                {
                    return new EmailGroup();
                };
            ShimEmailGroup.ExistsStringInt32Int32 =
                (emailAddres, groupID, customerID) =>
                {
                    return true;
                };
            ShimEmailGroup.GetBestProfileForEmailAddressInt32Int32String =
                (groupId, customerID, email) =>
                {
                    var dataTable = new DataTable();
                    dataTable.Columns.Add(GroupIDColumnName);
                    dataTable.Columns.Add(SubscribeTypeCodeColumnName);
                    dataTable.Columns.Add(MailingProfilePswdColumnName, typeof(string));
                    dataTable.Rows.Add(GroupID, DummyStringValue, MailingProfilePswdColumnValue);
                    return dataTable;
                };
        }

        public static void CreateMXValidateShim()
        {
            ShimMXValidate.Constructor = (validate) => { };
        }

        public static IList<Control> CreateControls(int controlID, string fieldLabelValue = "", int? mainTypeId = 0, int typeSeqID = 0, int? fieldID = null,
                                                    string htmlIDGuid = "")
        {
            var controls = new List<Control>();
            var control = CreateControl(controlID, fieldLabelValue, mainTypeId, typeSeqID, fieldID, htmlIDGuid);
            controls.Add(control);

            return controls;
        }

        public static Control CreateControl(int controlID, string fieldLabelValue = "", int? mainTypeId = 0, int typeSeqID = 0, int? fieldID = null, string htmlIDGuid="")
        {
            return new Control()
            {
                Control_ID = controlID,
                ControlType = new ControlType()
                {
                    MainType_ID = mainTypeId
                },
                FieldLabel = fieldLabelValue,
                Type_Seq_ID = typeSeqID,
                FieldID = fieldID,
                HTMLID = String.IsNullOrEmpty(htmlIDGuid) ? Guid.Empty : new Guid(htmlIDGuid)
            };
        }

        public static GroupDataFields CreateGroupDataField(int groupFieldID, string groupFieldShortName)
        {
            var groupDataFieldValue = new GroupDataFields()
            {
                GroupDataFieldsID = groupFieldID,
                ShortName = groupFieldShortName
            };
            return groupDataFieldValue;
        }

        public static void ChangeControlToAddNewsletterGroup(Control control, string labelHtml = "", int? groupID= null)
        {
            int groupIDValue = groupID.HasValue ? groupID.Value : GroupID;

            control.NewsletterGroups = new List<NewsletterGroup>
            {
                new NewsletterGroup()
                {
                    GroupID = groupIDValue,
                    LabelHTML = labelHtml,
                    Control_ID = control.Control_ID
                }
            };
        }

        public static void CreateDbResolverShim()
        {
            ShimDbResolver.Constructor = (db) => { };
        }
    }
}
