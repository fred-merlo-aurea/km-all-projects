using System;
using System.Collections;
using System.Configuration.Fakes;
using System.Data;
using System.Fakes;
using System.IO;
using System.IO.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KM.Common.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace ECNExternalSrcDataImport.Tests
{
    public partial class DataImporterTest
    {
        private const string MethodImportData = "ImportData";
        private const string Exception = "Exception from SqlCommand";
        private const string AlreadyAddedException = "Item has already been added. Key in dictionary: 'T'  Key being added: 'T'";
        private const string DataColumnAction = "Action";
        private const string DataColumnCounts = "Counts";

        private DataTable _sampleDataTable;

        [TestMethod]
        public void ImportDataTest_UppercaseActionData_VerifyNoAnyExceptionThrown()
        {
            ImportData_TestInitialize();
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) =>
                {
                    var sampleDataTable = new DataTable();
                    sampleDataTable.Clear();
                    sampleDataTable.Columns.Add(DataColumnAction);
                    sampleDataTable.Columns.Add(DataColumnCounts);
                    AddRowToActionTable(sampleDataTable, "T", "2");
                    AddRowToActionTable(sampleDataTable, "T", "3");
                    AddRowToActionTable(sampleDataTable, "I", "3");
                    AddRowToActionTable(sampleDataTable, "U", "3");
                    AddRowToActionTable(sampleDataTable, "D", "3");
                    AddRowToActionTable(sampleDataTable, "S", "3");
                    AddRowToActionTable(sampleDataTable, "E", "3");
                    AddRowToActionTable(sampleDataTable, "M", "3");

                    return sampleDataTable;
                };

            _dataImporter.InvokeStatic(MethodImportData, _sampleDataTable);
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void ImportDataTest_LowercaseActionData_ThrowException()
        {
            ImportData_TestInitialize();
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) =>
                {
                    DataTable sampleDataTable = new DataTable();
                    sampleDataTable.Clear();
                    sampleDataTable.Columns.Add(DataColumnAction);
                    sampleDataTable.Columns.Add(DataColumnCounts);
                    AddRowToActionTable(sampleDataTable, "t", "2");
                    AddRowToActionTable(sampleDataTable, "t", "3");

                    return sampleDataTable;
                };

            _dataImporter.InvokeStatic(MethodImportData, _sampleDataTable);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(AlreadyAddedException);
        }

        [TestMethod]
        public void ImportDataTest_OnException()
        {
            ImportData_TestInitialize();
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => throw new Exception(Exception);

            _dataImporter.InvokeStatic(MethodImportData, _sampleDataTable);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBeSameAs(Exception);
        }

        private void AddRowToActionTable(DataTable table, string action, string counts)
        {
            var dataRow = table.NewRow();
            dataRow[DataColumnAction] = action;
            dataRow[DataColumnCounts] = counts;
            table.Rows.Add(dataRow);
        }

        private void ImportData_TestInitialize()
        {
            _dataImporter = new PrivateType(AssemblyName, ClassName);

            ShimDateTime.NowGet = () => new DateTime(2018, 1, 1);

            _sampleDataTable = new DataTable();
            _sampleDataTable.Clear();
            _sampleDataTable.Columns.Add("user_name");
            _sampleDataTable.Columns.Add("subscribetype");
            _sampleDataTable.Columns.Add("emailaddress");
            DataRow dr = _sampleDataTable.NewRow();
            dr["user_name"] = "User1";
            dr["subscribetype"] = 1;
            dr["emailaddress"] = "User1@unittest.com";
            _sampleDataTable.Rows.Add(dr);
            dr = _sampleDataTable.NewRow();
            dr["user_name"] = "User2";
            dr["subscribetype"] = 2;
            dr["emailaddress"] = "User2@unittest.com";
            _sampleDataTable.Rows.Add(dr);

            ShimGroupDataFields.GetGroupUDFHashtable_NoAccessCheckInt32 = groupId => new Hashtable
            {
                {1, "name"}
            };

            _dataImporter.SetStaticFieldOrProperty(FieldLogFile, (StreamWriter) new ShimStreamWriter());
            _dataImporter.SetStaticFieldOrProperty(FieldGroupId, "1");
            _dataImporter.SetStaticFieldOrProperty(FieldCustomerId, "1");

            ShimConfigurationManager.AppSettingsGet =
                () => new System.Collections.Specialized.NameValueCollection {{"KMCommon_Application", "1"}};

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, name, appId, note, gd, ec) =>
                {
                    _exceptionMessage = ex.Message;
                    _anyException = true;
                    return 0;
                };
        }
    }
}