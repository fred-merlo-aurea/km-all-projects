using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using Microsoft.QualityTools.Testing.Fakes;
using ecn.webservice;
using ecn.webservice.Fakes;
using ecn.common.classes.Fakes;
using KMPlatform.Entity;
using BusinessUser = KMPlatform.BusinessLogic.Fakes.ShimUser;
using NUnit.Framework;
using Shouldly;

namespace ECN.Webservice.Tests
{
    [TestFixture]
    public class ListImport_XMLTest
    {
        private IDisposable _shimObject;
        private const string _ECNAccesskey = "2B4E4F20-B642-457D-8407-DB82F1BDC401";
        private readonly NameValueCollection _appSettings = new NameValueCollection();
        private DataTable _dtFile;
        private string _oldEmailAddres;
        private string _newEmailAddress;
        private string _accessKey;
        private string _dummyData;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _appSettings.Add("KMCommon", "KMCommon");
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings.Clear();
            _shimObject.Dispose();
        }

        [Test]
        public void importDataWithUpdate_whenDataProvided_DataIsSavedAndEmailAddressIsUpdated()
        {
            Initialize();
            CreateShims();
            var emailUpdated = false;
            var dataSaved = false;
            var listImport_XMLObject = new ListImport_XML();
            ShimDataFunctions.ExecuteStringSqlCommand = (x, y) =>
            {
                emailUpdated = true;
                return 0;
            };
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, xmlProfile, e, f, g, h, i, j) =>
            {
                if (xmlProfile.Contains(_dummyData))
                {
                    dataSaved = true;
                }
                return ImportEmailDataTable();
            };

            // Act
            listImport_XMLObject.importDataWithUpdate(_dtFile, _oldEmailAddres, _newEmailAddress, _accessKey);

            // Assert
            dataSaved.ShouldSatisfyAllConditions(
                () => emailUpdated.ShouldBeTrue(),
                () => dataSaved.ShouldBeTrue());
        }

        [Test]
        public void importDataWithUpdate_whenDataProvided_ResultsAreImported()
        {
            Initialize();
            CreateShims();
            var listImport_XMLObject = new ListImport_XML();

            // Act
            var result = listImport_XMLObject.importDataWithUpdate(_dtFile, _oldEmailAddres, _newEmailAddress, _accessKey);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => listImport_XMLObject.importResults.ShouldNotBeNullOrWhiteSpace(),
                () => listImport_XMLObject.importResults.ShouldContain("<ImportTime>"),
                () => result.ShouldBeTrue());
        }

        private void Initialize()
        {
            _dummyData = "dummyDataForTable";
            _dtFile = new DataTable();
            _dtFile.Columns.Add("dummyColumnName");
            _dtFile.Columns.Add("user_duymmyCOlumnName");
            _dtFile.Columns.Add("emailaddress");
            _dtFile.Rows.Add(_dummyData, _dummyData, _dummyData);
            _oldEmailAddres = "old@email.com";
            _newEmailAddress = "new@email.com";
            _accessKey = "dummyAccessKey";
        }

        private void CreateShims()
        {
            BusinessUser.ECN_GetByAccessKeyStringBoolean = (x, y) => new User();
            var udfsHashtable = new Hashtable();
            udfsHashtable["user_duymmycolumnname"] = "One";
            ShimListImport_XML.AllInstances.getUDFsForListInt32 = (x, y) => udfsHashtable;
            ShimDataFunctions.ExecuteStringSqlCommand = (x, y) => 0;
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, xmlProfile, e, f, g, h, i, j) => ImportEmailDataTable();
        }

        private DataTable ImportEmailDataTable()
        {
            var importTable = new DataTable();
            importTable.Columns.Add("dummycolumn");
            importTable.Columns.Add("Action");
            importTable.Columns.Add("counts");
            importTable.Rows.Add("dummyRow", "T", "1");
            importTable.Rows.Add("dummyRow", "I", "1");
            importTable.Rows.Add("dummyRow", "U", "1");
            importTable.Rows.Add("dummyRow", "D", "1");
            importTable.Rows.Add("dummyRow", "S", "1");
            importTable.Rows.Add("dummyRow", "M", "1");
            return importTable;
        }
    }
}