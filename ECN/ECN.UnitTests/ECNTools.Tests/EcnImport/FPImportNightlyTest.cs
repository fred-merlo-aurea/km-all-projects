using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using ECNTools.ECN_Import;
using ECNTools.ECN_Import.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECNTools.Tests.ECN_Import
{
    /// <summary>
    /// Unit test for <see cref="FPImportNightly"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FPImportNightlyTest
    {
        private const string ConnectionString = "data source=127.0.0.1;Integrated Security=SSPI;";
        private const string ExtractDataFromFileAndImport = "ExtractDataFromFileAndImport";
        private const string MethodInitializeComponent = "InitializeComponent";
        private const string MethodUpdateEmailProfile = "UpdateEmailProfile";
        private const string EmailAddress = "EmailAddress";
        private const string Canada = "canada";
        private const string Country = "usa";
        private const string SubscriberId = "SUBSCRIBERID";
        private const string PublicationCode = "PUBLICATIONCODE";
        private const string AlternateEmail = "ALTERNATE_EMAILADDRESS";
        private const string Code = "PUBCODE";
        private const string Ignore = "DEMO39";
        private const int Id = 10;
        private FPImportNightly _fpImportNightlyObject;
        private PrivateObject _privateObject;
        private DataTable _dataTable;
        protected IDisposable _shimObject;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
            _dataTable?.Dispose();
        }

        [Test]
        public void InitializeComponent_LoadDefaultValue_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            CreateClassObject();

            // Act
            _privateObject.Invoke(MethodInitializeComponent, null);

            // Assert
            AssertMethodResult(_fpImportNightlyObject);
        }

        [Test]
        public void InitializeComponent_LoadDefaultValue_ValidateFileDialogIsAssignedSuccessfully()
        {
            // Arrange
            CreateClassObject();

            // Act
            _privateObject.Invoke(MethodInitializeComponent, null);

            // Assert
            var fPImportNightlyName = (string)_privateObject.GetFieldOrProperty("Name");
            var fPImportNightlyText = (string)_privateObject.GetFieldOrProperty("Text");
            var fileDialog = (OpenFileDialog)_privateObject.GetFieldOrProperty("openFileDialog1");
            var fileName = fileDialog.FileName;
            fPImportNightlyName.ShouldSatisfyAllConditions(
                () => fPImportNightlyName.ShouldNotBeNullOrWhiteSpace(),
                () => fPImportNightlyText.ShouldNotBeNullOrWhiteSpace(),
                () => fileName.ShouldNotBeNullOrWhiteSpace(),
                () => fPImportNightlyName.ShouldBe(FpImportHelper.FPIName),
                () => fPImportNightlyName.ShouldBe(FpImportHelper.FPIName),
                () => fileName.ShouldBe(FpImportHelper.DialogFileName));
        }

        [TestCase(10, 10, true)]
        [TestCase(13, 13, true)]
        [TestCase(14, 14, true)]
        [TestCase(10, 10, false)]
        [TestCase(13, 13, false)]
        [TestCase(14, 14, false)]
        [TestCase(16, 16, true)]
        [TestCase(17, 17, true)]
        [TestCase(18, 18, true)]
        [TestCase(16, 16, false)]
        [TestCase(17, 17, false)]
        [TestCase(18, 18, false)]
        public void ExtractDataFromFileAndImport_BaseDataTableIsNotNull_NotifyAdminWithMessage(int xact, int email, bool ecnSubIDMatch)
        {
            // Arrange
            var isEmailGroupExist = false;
            var messageAddedToLog = string.Empty;
            var isNonAccessCheck = false;
            var isEmailAndSubscriberIdExist = false;
            CreateClassObject();
            var dBaseDataTable = CreateBaseDataTableObject(xact, email);
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) =>
            {
                isNonAccessCheck = true;
                return CreateGroupObject();
            };
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (emailId, groupId) =>
            {
                isEmailGroupExist = true;
                return CreateEmailObject(emailId);
            };
            ShimFPImportNightly.AllInstances.CheckEmailAndSubscriberIDStringStringString = (x, y, z, m) => { isEmailAndSubscriberIdExist = true; };
            ShimFPImportNightly.AllInstances.getDummyEmailAddressString = (x, y) => { return y; };
            ShimFPImportNightly.AllInstances.ReportNonExistEmailProfileStringGroupInt32StringDataRow = (sender, rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows) => { };
            ShimFPImportNightly.AllInstances.NotifyAdminStringString = (sender, subject, body) => { };
            ShimFPImportNightly.AllInstances.WriteToLogString = (sender, message) => { messageAddedToLog = message; };
            ShimFPImportNightly.AllInstances.UpdateEmailProfileStringGroupInt32StringDataRow = (sender, rowEmailAddress, pubGroup, custID, rowAltEMailAddress, selectDBIVDataTableRows) => { };
            FPImportNightly.ECNEmailID_SubIDMatch = ecnSubIDMatch;

            var groupID = "121";
            var lastDownloadDate = DateTime.Now.ToString();
            var parameters = new object[] { dBaseDataTable, groupID, lastDownloadDate };

            // Act
            _privateObject.Invoke(ExtractDataFromFileAndImport, parameters);

            // Assert
            isEmailGroupExist.ShouldBeTrue();
            messageAddedToLog.ShouldNotBeNullOrEmpty();
            isNonAccessCheck.ShouldBeTrue();
            isEmailAndSubscriberIdExist.ShouldBeTrue();
        }

        [Test]
        [TestCase(Canada)]
        [TestCase(Country)]
        [TestCase("")]
        public void UpdateEmailProfile_ForDifferentCountriesAndGroupUdf_UpdateEmail(string country)
        {
            // Arrange
            CreateClassObject();
            InitializeUpdateEmail();
            var pubGroup = new Group() { CustomerID = Id, GroupID = Id };
            var messageAddedToLog = string.Empty;
            ShimFPImportNightly.AllInstances.WriteToLogString = (sender, message) => { messageAddedToLog = message; };

            // Act
            _privateObject.Invoke(MethodUpdateEmailProfile, new object[] { EmailAddress, pubGroup, Id, EmailAddress, CreateDataRow(country)});

            // Assert
            messageAddedToLog.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void UpdateEmailProfile_ForNullEmailId_UpdateEmail()
        {
            // Arrange
            CreateClassObject();
            ShimFPImportNightly.newEmailObjGet = () => new Email() { EmailID = 0 };
            var pubGroup = new Group() { CustomerID = Id, GroupID = Id };
            var messageAddedToLog = string.Empty;
            ShimFPImportNightly.AllInstances.WriteToLogString = (sender, message) => { messageAddedToLog = message; };

            // Act
            _privateObject.Invoke(MethodUpdateEmailProfile, new object[] { EmailAddress, pubGroup, Id, EmailAddress, CreateDataRow(Canada) });

            // Assert
            messageAddedToLog.ShouldNotBeNullOrEmpty();
        }

        private void InitializeUpdateEmail()
        {
            ShimFPImportNightly.newEmailObjGet = () => new Email() { EmailID = Id };
            var groupData = CreateGroupDataFields(1, SubscriberId);
            var groupData1 = CreateGroupDataFields(2, PublicationCode);
            var groupData2 = CreateGroupDataFields(3, AlternateEmail);
            var groupData3 = CreateGroupDataFields(4, Code);
            var groupData4 = CreateGroupDataFields(5, Ignore);
            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 =
                (x) => new List<GroupDataFields> { groupData, groupData1, groupData2, groupData3, groupData4 };
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString = 
                (x, y, q, w, e, r, t, u, i, o) => new DataTable();
        }

        private static GroupDataFields CreateGroupDataFields(int id, string shortName)
        {
            return new GroupDataFields()
            {
                GroupDataFieldsID = id,
                ShortName = shortName
            };
        }

        private DataRow CreateDataRow(string country)
        {
            _dataTable = new DataTable()
            {
                Columns = { "ZIP", "PLUS4", "COUNTRY", "FNAME", "LNAME", "COMPANY", "TITLE", "ADDRESS",
                    "MAILSTOP", "CITY", "STATE", "PHONE", "FAX", "WEBSITE", "SEQUENCE", "PUBCODE" , "EMAIL", "EMAILID" },
                Rows = { { "25698", "1", country, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty } }
            };
            return _dataTable.Rows[_dataTable.Rows.Count - 1];
        }

        private static Group CreateGroupObject()
        {
            return new Group
            {
                CustomerID = 1,
                GroupID = 1,
                GroupName = "Unit Test",
            };
        }

        private static DataTable CreateBaseDataTableObject(int xact, int email)
        {
            var dBaseDataTable = new DataTable();
            dBaseDataTable.Columns.Add("PUBCODE", typeof(string));
            dBaseDataTable.Columns.Add("EMAIL", typeof(string));
            dBaseDataTable.Columns.Add("EMAILID", typeof(int));
            dBaseDataTable.Columns.Add("XACT", typeof(int));
            dBaseDataTable.Columns.Add("SEQUENCE", typeof(int));
            dBaseDataTable.Rows.Add("121", "t@.c", email, xact, 1);
            dBaseDataTable.Rows.Add("121", "t@.c", email, xact, 1);
            dBaseDataTable.Rows.Add("124", "test@adminUnitTest2.com", 1, 1, 1);
            dBaseDataTable.Rows.Add("122", "test@adminUnitTest.com", 17, 17, 1);
            dBaseDataTable.Rows.Add("123", "test@adminUnitTest1.com", 100, 100, 1);
            return dBaseDataTable;
        }

        private static Email CreateEmailObject(int emailId)
        {
            if (emailId == 10 || emailId == 16)
            {
                return null;
            }
            else if (emailId == 13)
            {
                return new Email
                {
                    EmailID = emailId
                };
            }
            else if (emailId == 14)
            {
                return new Email
                {
                    EmailID = emailId + 1
                };
            }
            else if (emailId == 18)
            {
                return new Email
                {
                    EmailID = -1
                };
            }
            else
            {
                return new Email
                {
                    EmailID = emailId,
                };

            }
        }

        private object GetTextbox(string tbName)
        {
            var txt = string.Empty;
            var textBox = _privateObject.GetFieldOrProperty(tbName) as TextBox;
            if (textBox != null)
            {
                txt = textBox.Text;
            }
            return txt;
        }

        private object GetLabel(string tbName)
        {
            var txt = string.Empty;
            var label = _privateObject.GetFieldOrProperty(tbName) as Label;
            if (label != null)
            {
                txt = label.Text;
            }
            return txt;
        }

        private object GetButton(string tbName)
        {
            var txt = string.Empty;
            var btn = _privateObject.GetFieldOrProperty(tbName) as Button;
            if (btn != null)
            {
                txt = btn.Text;
            }
            return txt;
        }

        private object GetRadioButton(string tbName)
        {
            var txt = string.Empty;
            var radioBtn = _privateObject.GetFieldOrProperty(tbName) as RadioButton;
            if (radioBtn != null)
            {
                txt = radioBtn.Text;
            }
            return txt;
        }

        private object GetCheckbox(string tbName)
        {
            var txt = string.Empty;
            var ckBox = _privateObject.GetFieldOrProperty(tbName) as CheckBox;
            if (ckBox != null)
            {
                txt = ckBox.Text;
            }
            return txt;
        }

        private T Get<T>(string propName)
        {
            var val = (T)_privateObject.GetFieldOrProperty(propName);
            return val;
        }

        private void CreateClassObject()
        {
            ShimConfigurationManager.ConnectionStringsGet = () =>
            {
                var sampleConfigSettingCollection = new ConnectionStringSettingsCollection();
                var dummyConnectionString = new ConnectionStringSettings("ecnAccessKey", ConnectionString);
                sampleConfigSettingCollection.Add(dummyConnectionString);
                return sampleConfigSettingCollection;
            };
            var appSettings = new NameValueCollection
            {
                { "ecnAccessKey", Guid.NewGuid().ToString() },
                { "LinenumberToStart","1" },
                { "ECNCommunicator",ConnectionString },
            };
            ShimConfigurationManager.AppSettingsGet = () => { return appSettings; };

            var connectionStringSettingsCollection = new ConnectionStringSettingsCollection();
            connectionStringSettingsCollection.Add(new ConnectionStringSettings { ConnectionString = ConnectionString, Name = "ECNCommunicator" });

            ShimConfigurationManager.ConnectionStringsGet = () => { return connectionStringSettingsCollection; };
            ShimFPImportNightly.AllInstances.LoadListBox = (x) => { };
            _fpImportNightlyObject = new FPImportNightly();
            _privateObject = new PrivateObject(_fpImportNightlyObject);
        }

        private void AssertMethodResult(FPImportNightly result)
        {
            result.ShouldSatisfyAllConditions(
                () => GetLabel(FpImportHelper.Label1).ShouldBe(FpImportHelper.Label1Text),
                () => GetLabel(FpImportHelper.Label2).ShouldBe(FpImportHelper.Label2Text),
                () => GetButton(FpImportHelper.ButtonChooseDBF).ShouldBe(FpImportHelper.ButtonChooseDBFText),
                () => GetButton(FpImportHelper.ButtonImport).ShouldBe(FpImportHelper.ButtonImportText),
                () => GetButton(FpImportHelper.ButtonClose).ShouldBe(FpImportHelper.ButtonCloseText),
                () => GetButton(FpImportHelper.ButtonCancel).ShouldBe(FpImportHelper.ButtonCancelText)
            );
        }
    }
}
