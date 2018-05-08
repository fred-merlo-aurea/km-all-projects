using System;
using System.Data;
using System.Data.Common.Fakes;
using System.Configuration.Fakes;
using System.IO.Fakes;
using System.Data.Fakes;
using System.Data.OleDb.Fakes;
using System.Net.Mail.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;


namespace ECNExternalSrcDataImport.Tests
{
    public partial class DataImporterTest
    {
        private const string MethodMain = "Main";
        private const string ColumnImporterID = "ImporterID";
        private const string DummyText = "Dummy";
        private const string DummyIntText = "1";
        private const string DummyMail = "dummy@dummy.com";
        private const string AppSettingKeyECNUserAccessKey = "ECNUserAccessKey";
        private const string AppSettingKeyConnString = "connString";
        private const string AppSettingKeyCommDB = "commDB";
        private const string AppSettingKeyCustDownloadPath = "custDownloadPath";
        private const string AppSettingKeyNOTIFICATIONEMAIL = "NOTIFICATION_EMAIL";
        private const string AppSettingKeyNOTIFICATIONEMAILFROM = "NOTIFICATION_EMAIL_FROM";
        private const string AppSettingKeySMTPSERVER = "SMTP_SERVER";
        private const string DBColumnChannelID = "ChannelID";
        private const string DBColumnCustomerID = "CustomerID";
        private const string DBColumnImportGroupID = "ImportGroupID";
        private const string DBColumnAppendData = "AppendData";
        private const string DBColumnImportName = "ImportName";
        private const string DBColumnImportType = "ImportType";
        private const string DBColumnSiteAddress = "SiteAddress";
        private const string DBColumnUserName = "UserName";
        private const string DBColumnPassword = "Password";
        private const string DBColumnDirectoryQuery = "Directory_Query";        
        private const string DBColumnDatabaseFileName = "Database_FileName";
        private const string DBColumnTableSheet = "Table_Sheet";
        private const string DBColumnImportFrequency = "ImportFrequency";
        private const string DBColumnImportSetting = "ImportSetting";
        private const string DBColumnImportDateTime = "ImportDateTime";
        private const string DBColumnActive = "Active";
        private const string DBColumnDateAdded = "DateAdded";
        private const string DBColumnDateUpdated = "DateUpdated";
        private const string DBColumnAdminEmail = "AdminEmail";
        private const string DBColumnSecureConnectionData = "SecureConnectionData";
        private const string DBValueSFTP = "SFTP";
        private const string DBValueFTP = "FTP";
        private const string DBValueDummyTxtFile = "dummy.txt";
        private const string DBValueDummyXlsFile = "dummy.xls";
        private const string DBValueDummyXmlFile = "dummy.xml";
        private const int ValueGroupId = 1;
        private const string ReturnValueSucceded = "succeded";

        private const int SampleImporterID1 = 1;

        [TestMethod]
        public void MainTest_WithoutArg_NoImporter_NoException()
        {
            Main_TestInitialize(true);

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithoutArg_NoImporter_ExceptionOnImporter()
        {
            Main_TestInitialize(false);

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableSqlCommandString = (command, connectionString) =>
            {
                throw new Exception(Exception);
            };

            ShimSmtpClient.AllInstances.SendMailMessage = (s, m) =>
            {
                _anyException = true;
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { } });
            _anyException.ShouldBeTrue();
        }

        [TestMethod]
        public void MainTest_WithoutArg_NoFTP_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable();
                return sampleUDFImportScheduleTable;
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithoutArg_NoFTP_ExceptionOnUDFImport()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                throw new Exception(Exception);
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { } });
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBeSameAs(Exception);
        }

        [TestMethod]
        public void MainTest_WithArg_SFTPFail_TXTFile_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable();
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueSFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyTxtFile;
                sampleUDFImportScheduleTable.Rows[0][DBColumnAdminEmail] = DummyMail;

                return sampleUDFImportScheduleTable;
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithArg_SFTPSuccess_TXTFile_ColumnNamesNotMatch_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable();
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueSFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyTxtFile;

                return sampleUDFImportScheduleTable;
            };

            ShimFtpFunctions.AllInstances.Download_SFTPStringStringBoolean = (x1, x2, x3, x4) =>
            {
                return ReturnValueSucceded;
            };

            ShimStreamReader.ConstructorString = (x1, x2) => { };
            ShimStreamReader.AllInstances.ReadLine = (x) => { return DummyText; };
            ShimStreamReader.AllInstances.ReadToEnd = (x) => { return DummyText; };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => {
                return new ECN_Framework_Entities.Communicator.Fakes.ShimGroup
                {
                    GroupIDGet = () => { return ValueGroupId; }
                };
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithArg_SFTPSuccess_TXTFile_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable(true);
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueSFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyTxtFile;
                sampleUDFImportScheduleTable.Rows[0][DBColumnAdminEmail] = DummyMail;

                return sampleUDFImportScheduleTable;
            };
            
            ShimFtpFunctions.AllInstances.Download_SFTPStringStringBoolean = (x1, x2, x3, x4) =>
            {
                return ReturnValueSucceded;
            };

            ShimStreamReader.ConstructorString = (x1, x2) => { };
            ShimStreamReader.AllInstances.ReadLine = (x) => { return DummyText; };
            ShimStreamReader.AllInstances.ReadToEnd = (x) => { return DummyText; };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => {
                return new ECN_Framework_Entities.Communicator.Fakes.ShimGroup
                {
                    GroupIDGet = () => { return ValueGroupId; }
                };
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithArg_SFTPSuccess_XLSFile_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable(true, true);
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueSFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyXlsFile;
                sampleUDFImportScheduleTable.Rows[0][DBColumnAdminEmail] = DummyMail;
                return sampleUDFImportScheduleTable;
            };

            ShimFtpFunctions.AllInstances.Download_SFTPStringStringBoolean = (x1, x2, x3, x4) =>
            {
                return ReturnValueSucceded;
            };

            ShimStreamReader.ConstructorString = (x1, x2) => { };
            ShimStreamReader.AllInstances.ReadLine = (x) => { return DummyText; };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => {
                return new ECN_Framework_Entities.Communicator.Fakes.ShimGroup
                {
                    GroupIDGet = () => { return ValueGroupId; }
                };
            };

            ShimDbDataAdapter.AllInstances.FillDataSetString =
                (x1, dataSet, tableName) =>
                {
                    var excelTable = _sampleDataTable.Clone();
                    excelTable.TableName = tableName;
                    dataSet.Tables.Add(excelTable);
                    return 0;
                };

            ShimOleDbDataAdapter.ConstructorStringString = (x1, x2, x3) =>
            {
                x1 = new ShimOleDbDataAdapter();
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithArg_SFTPSuccess_XMLFile_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable(true, true);
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueSFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyXmlFile;
                sampleUDFImportScheduleTable.Rows[0][DBColumnAdminEmail] = DummyMail;
                return sampleUDFImportScheduleTable;
            };

            ShimFtpFunctions.AllInstances.Download_SFTPStringStringBoolean = (x1, x2, x3, x4) =>
            {
                return ReturnValueSucceded;
            };

            ShimStreamReader.ConstructorString = (x1, x2) => { };
            ShimStreamReader.AllInstances.ReadLine = (x) => { return DummyText; };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => {
                return new ECN_Framework_Entities.Communicator.Fakes.ShimGroup
                {
                    GroupIDGet = () => { return ValueGroupId; }
                };
            };

            ShimDataSet.AllInstances.ReadXmlString = (x, xmlString) =>
            {
                x.Tables.Add(_sampleDataTable.Clone());
                return XmlReadMode.Auto;
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithArg_FTPFail_TXTFile_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable(false, false);
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyTxtFile;
                sampleUDFImportScheduleTable.Rows[0][DBColumnAdminEmail] = DummyMail;
                return sampleUDFImportScheduleTable;
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithArg_FTPSuccess_TXTFile_ColumnNamesNotMatch_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable(false, false);
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyTxtFile;
                sampleUDFImportScheduleTable.Rows[0][DBColumnAdminEmail] = DummyMail;
                return sampleUDFImportScheduleTable;
            };

            KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.DirectoryListSimpleString = (x, mask) =>
            {
                return new string[] { DBValueDummyTxtFile };
            };

            ShimStreamReader.ConstructorString = (x1, x2) => { };
            ShimStreamReader.AllInstances.ReadLine = (x) => { return DummyText; };
            ShimStreamReader.AllInstances.ReadToEnd = (x) => { return DummyText; };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => {
                return new ECN_Framework_Entities.Communicator.Fakes.ShimGroup
                {
                    GroupIDGet = () => { return ValueGroupId; }
                };
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithArg_FTPSuccess_TXTFile_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable(true, false);
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyTxtFile;
                sampleUDFImportScheduleTable.Rows[0][DBColumnAdminEmail] = DummyMail;
                return sampleUDFImportScheduleTable;
            };

            KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.DirectoryListSimpleString = (x, mask) =>
            {
                return new string[] { DBValueDummyTxtFile};
            };

            ShimStreamReader.ConstructorString = (x1, x2) => { };
            ShimStreamReader.AllInstances.ReadLine = (x) => { return DummyText; };
            ShimStreamReader.AllInstances.ReadToEnd = (x) => { return DummyText; };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => {
                return new ECN_Framework_Entities.Communicator.Fakes.ShimGroup
                {
                    GroupIDGet = () => { return ValueGroupId; }
                };
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithArg_FTPSuccess_XLSFile_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable(true, true);
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyXlsFile;
                sampleUDFImportScheduleTable.Rows[0][DBColumnAdminEmail] = DummyMail;
                return sampleUDFImportScheduleTable;
            };

            KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.DirectoryListSimpleString = (x, mask) =>
            {
                return new string[] { DBValueDummyXlsFile };
            };

            ShimStreamReader.ConstructorString = (x1, x2) => { };
            ShimStreamReader.AllInstances.ReadLine = (x) => { return DummyText; };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => {
                return new ECN_Framework_Entities.Communicator.Fakes.ShimGroup
                {
                    GroupIDGet = () => { return ValueGroupId; }
                };
            };

            ShimDbDataAdapter.AllInstances.FillDataSetString =
                (x1, dataSet, tableName) =>
                {
                    var excelTable = _sampleDataTable.Clone();
                    excelTable.TableName = tableName;
                    dataSet.Tables.Add(excelTable);
                    return 0;
                };

            ShimOleDbDataAdapter.ConstructorStringString = (x1, x2, x3) =>
            {
                x1 = new ShimOleDbDataAdapter();
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        [TestMethod]
        public void MainTest_WithArg_FTPSuccess_XMLFile_NoException()
        {
            Main_TestInitialize();

            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableStringString = (command, connectionString) =>
            {
                var sampleUDFImportScheduleTable = CreateSampleTable(true, true);
                sampleUDFImportScheduleTable.Rows[0][DBColumnImportType] = DBValueFTP;
                sampleUDFImportScheduleTable.Rows[0][DBColumnDatabaseFileName] = DBValueDummyXmlFile;
                sampleUDFImportScheduleTable.Rows[0][DBColumnAdminEmail] = DummyMail;
                return sampleUDFImportScheduleTable;
            };

            KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.DirectoryListSimpleString = (x, mask) =>
            {
                return new string[] { DBValueDummyXmlFile };
            };

            ShimStreamReader.ConstructorString = (x1, x2) => { };
            ShimStreamReader.AllInstances.ReadLine = (x) => { return DummyText; };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => {
                return new ECN_Framework_Entities.Communicator.Fakes.ShimGroup
                {
                    GroupIDGet = () => { return ValueGroupId; }
                };
            };

            ShimDataSet.AllInstances.ReadXmlString = (x, xmlString) =>
            {
                x.Tables.Add(_sampleDataTable.Clone());
                return XmlReadMode.Auto;
            };

            _dataImporter.InvokeStatic(MethodMain, new object[] { new string[] { DummyIntText } });
            _anyException.ShouldBeFalse();
        }

        private DataTable CreateSampleTable(bool withDummyColumn = false, bool withEmail = false)
        {
            var sampleUDFImportScheduleTable = new DataTable();
            sampleUDFImportScheduleTable.Clear();
            sampleUDFImportScheduleTable.Columns.Add(DBColumnChannelID);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnCustomerID);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnImportGroupID);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnAppendData);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnImportName);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnImportType);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnSiteAddress);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnUserName);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnPassword);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnDirectoryQuery);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnDatabaseFileName);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnTableSheet);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnImportFrequency);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnImportSetting);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnImportDateTime);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnActive);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnDateAdded);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnDateUpdated);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnAdminEmail);
            sampleUDFImportScheduleTable.Columns.Add(DBColumnSecureConnectionData);
            if (withDummyColumn)
            {
                sampleUDFImportScheduleTable.Columns.Add(DummyText);
            }
            if (withEmail)
            {
                sampleUDFImportScheduleTable.Columns.Add(DBColumnEmailAddress);
            }
            var sampleDataRow = sampleUDFImportScheduleTable.NewRow();
            sampleDataRow[DBColumnChannelID] = DummyText;
            sampleDataRow[DBColumnCustomerID] = DummyIntText;
            sampleDataRow[DBColumnImportGroupID] = DummyIntText;
            sampleDataRow[DBColumnAppendData] = DummyText;
            sampleDataRow[DBColumnImportName] = DummyText;
            sampleDataRow[DBColumnImportType] = DummyText;
            sampleDataRow[DBColumnSiteAddress] = DummyText;
            sampleDataRow[DBColumnUserName] = DummyText;
            sampleDataRow[DBColumnPassword] = DummyText;
            sampleDataRow[DBColumnDirectoryQuery] = DummyText;
            sampleDataRow[DBColumnDatabaseFileName] = DBValueDummyTxtFile;
            sampleDataRow[DBColumnTableSheet] = DummyText;
            sampleDataRow[DBColumnImportFrequency] = DummyText;
            sampleDataRow[DBColumnImportSetting] = DummyText;
            sampleDataRow[DBColumnImportDateTime] = DummyText;
            sampleDataRow[DBColumnActive] = DummyText;
            sampleDataRow[DBColumnDateAdded] = DummyText;
            sampleDataRow[DBColumnDateUpdated] = DummyText;
            sampleDataRow[DBColumnAdminEmail] = DummyText;
            sampleDataRow[DBColumnSecureConnectionData] = DummyText;
            if (withDummyColumn)
            {
                sampleDataRow[DummyText] = DummyText;
            }
            if (withEmail)
            {
                sampleDataRow[DBColumnEmailAddress] = DummyText;
            }
            sampleUDFImportScheduleTable.Rows.Add(sampleDataRow);

            return sampleUDFImportScheduleTable;
        }

        private void Main_TestInitialize(bool noImporter = false)
        {
            ECN_Framework_DataLayer.Fakes.ShimDataFunctions.GetDataTableSqlCommandString = (command, connectionString) =>
            {
                var sampleImporterDataTable = new DataTable();
                sampleImporterDataTable.Clear();
                sampleImporterDataTable.Columns.Add(ColumnImporterID);
                if(!noImporter)
                {
                    var sampleDataRow = sampleImporterDataTable.NewRow();
                    sampleDataRow[ColumnImporterID] = SampleImporterID1;
                    sampleImporterDataTable.Rows.Add(sampleDataRow);
                }

                return sampleImporterDataTable;
            };

            KMPlatform.BusinessLogic.Fakes.ShimUser.GetByAccessKeyStringBoolean = (appSetting, getChild) =>
            {
                return null;
            };

            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) =>
                {
                    var sampleTable = new DataTable();
                    sampleTable.Clear();
                    sampleTable.Columns.Add(DBColumnAction);
                    sampleTable.Columns.Add(DBColumnCounts);
                    var sampleRow = sampleTable.NewRow();
                    sampleRow[DBColumnAction] = DBValueT;
                    sampleRow[DBColumnCounts] = DBValue2;
                    sampleTable.Rows.Add(sampleRow);

                    return sampleTable;
                };

            ShimConfigurationManager.AppSettingsGet =
                () => new System.Collections.Specialized.NameValueCollection
                {
                    { AppSettingKeyECNUserAccessKey, DummyText },
                    { AppSettingKeyConnString, DummyText },
                    { AppSettingKeyCommDB, DummyText },
                    { AppSettingKeyCustDownloadPath, DummyText },
                    { AppSettingKeyKMCommon_Application, AppSettingValue1 },
                    { AppSettingKeyNOTIFICATIONEMAIL, DummyMail },
                    { AppSettingKeyNOTIFICATIONEMAILFROM, DummyMail },
                    { AppSettingKeySMTPSERVER, DummyText }
                };

            ShimSmtpClient.AllInstances.SendMailMessage = (s, m) => { };
        }
    }
}