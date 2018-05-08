using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using KM.Integration.Marketo;
using KM.Integration.Marketo.Process.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using MAF.FilterScheduleExport.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using KMPSEnum = KMPS.MD.Objects.Enums;

namespace MAF.FilterScheduleExport.Tests
{
    /// <summary>
    /// Unit test for <see cref="Program"/> class.
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class ProgramTest
    {
        private const string DefaultConnectionString = "Data Source=Unit Test;Initial Catalog=TextProject.DBContext;Integrated Security=True;MultipleActiveResultSets=True";
        private const string NoRecordFoundMessage = "No new records available for scheduled export.";
        private Program _program;
        private IDisposable _shimObject;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
            CreatePageFakeObject();
            _program = new Program();
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void RunEngine_MasterSupressionIsFalse_SaveResultToFile()
        {
            // Arrange
            var isGroupCreated = false;
            var isImportresultExist = false;
            ShimGroups.GetGroupByIDInt32 = (x) =>
            {
                isGroupCreated = true;
                return CreateGroupsObject(false);
            };
            ShimUtilities.getImportedResultHashtableDateTime = (x, y) =>
            {
                isImportresultExist = true;
                return CreateImportedResultDataTable();
            };

            // Act
            _program.RunEngine();

            // Assert
            isGroupCreated.ShouldBeTrue();
            isImportresultExist.ShouldBeTrue();
        }

        [Test]
        public void RunEngine_ExportTypeIsECAndMasterSupressionIsTrue_ThrowNullRefrenceException()
        {
            // Arrange
            ShimGroups.GetGroupByIDInt32 = (x) =>
            {
                return CreateGroupsObject(true);
            };
            ShimUtilities.getImportedResultHashtableDateTime = (x, y) =>
            {
                return CreateImportedResultDataTable();
            };

            // Assert
            Should.Throw<NullReferenceException>(() =>
            {
                // Act
                _program.RunEngine();
            });
        }

        [Test]
        public void RunEngine_GroupIdIsZero_ThrowException()
        {
            // Arrange
            ShimGroups.GetGroupByIDInt32 = (x) =>
            {
                return CreateGroupsObject(true, 0);
            };
            ShimUtilities.getImportedResultHashtableDateTime = (x, y) =>
            {
                return CreateImportedResultDataTable();
            };

            // Assert
            Should.Throw<Exception>(() =>
            {
                // Act
                _program.RunEngine();
            });
        }

        [Test]
        public void RunEngine_ImportingResultIsNull_SaveEmptyResult()
        {
            // Arrange
            var resultMessage = string.Empty;
            var isGroupCreated = false;
            ShimGroups.GetGroupByIDInt32 = (x) =>
            {
                isGroupCreated = true;
                return CreateGroupsObject(false);
            };
            ShimUtilities.getImportedResultHashtableDateTime = (x, y) =>
            {
                return CreateImportedResultDataTable(false);
            };
            ShimProgram.AllInstances.NotifyClientStringStringString = (sender, mailTo, message, filtername) =>
            {
                resultMessage += message;
            };

            // Act
            _program.RunEngine();

            // Assert
            resultMessage.ShouldContain(NoRecordFoundMessage);
            isGroupCreated.ShouldBeTrue();
        }

        private List<Result> CreateResultObject()
        {
            var resultList = new List<Result>();
            resultList.Add(new Result { type = "a", status = "Success" });
            resultList.Add(new Result { type = "a", status = "Success" });
            resultList.Add(new Result { type = "b", status = "Success" });
            resultList.Add(new Result { type = "b", status = "Success" });
            resultList.Add(new Result { type = "b", status = "Success" });
            return resultList;
        }

        private List<FilterScheduleIntegration> CreateFilterScheduleIntegrationObject()
        {
            var filterScheduleIntegrationList = new List<FilterScheduleIntegration>();
            var integrationParamName = new string[] { "BASEURL", "CLIENTID", "CLIENTSECRET", "PARTITION" };
            foreach (var item in integrationParamName)
            {
                filterScheduleIntegrationList.Add(new FilterScheduleIntegration
                {
                    IntegrationParamName = item,
                    IntegrationParamValue = item
                });
            }
            return filterScheduleIntegrationList;
        }

        private DataTable CreateImportedResultDataTable(bool includeRow = true)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Action", typeof(string));
            dataTable.Columns.Add("Totals", typeof(int));
            dataTable.Columns.Add("sortorder", typeof(string));
            if (includeRow)
            {
                dataTable.Rows.Add("NEW", 100, "a");
                dataTable.Rows.Add("Changed", 100, "b");
            }
            return dataTable;
        }

        private List<FilterExportField> CreateFilterExportFieldObject()
        {
            var filterExportedList = new List<FilterExportField>();
            var columnList = new string[] { "ADDRESS1", "REGIONCODE", "ZIPCODE", "PUBTRANSACTIONDATE", "QUALIFICATIONDATE", "FNAME", "LNAME", "ISLATLONVALID", "Demo1" };
            foreach (var item in columnList)
            {
                filterExportedList.Add(new FilterExportField
                {
                    MappingField = item,
                    ExportColumn = item,
                    IsCustomValue = true,
                    CustomValue = "Unit Test"
                });
            }
            return filterExportedList;
        }

        private List<FilterSchedule> CreateFilterScheduleListObject()
        {
            var filterScheduleList = new List<FilterSchedule>();
            var filterSchedule = CreateFilterScheduleObject();
            filterSchedule.FileNameFormat = "FileName_DateTime";
            filterSchedule.ExportTypeID = KMPSEnum.ExportType.FTP;
            filterSchedule.FilterScheduleID = 1;
            filterSchedule.ExportFormat = "CSV";
            filterScheduleList.Add(filterSchedule);

            filterSchedule = CreateFilterScheduleObject();
            filterSchedule.FileNameFormat = "FileName_Date";
            filterSchedule.ExportFormat = string.Empty;
            filterSchedule.FileName = "Unit Test";
            filterSchedule.FilterScheduleID = 2;
            filterSchedule.ExportTypeID = KMPSEnum.ExportType.FTP;
            filterScheduleList.Add(filterSchedule);

            filterSchedule = CreateFilterScheduleObject();
            filterSchedule.FileNameFormat = "FileName_DateTime";
            filterSchedule.ExportFormat = string.Empty;
            filterSchedule.FilterScheduleID = 21;
            filterSchedule.FileName = "Unit Test";
            filterSchedule.ExportTypeID = KMPSEnum.ExportType.FTP;
            filterScheduleList.Add(filterSchedule);

            filterSchedule = CreateFilterScheduleObject();
            filterSchedule.FileNameFormat = string.Empty;
            filterSchedule.ExportFormat = "CSV";
            filterSchedule.FileName = "Unit Test";
            filterSchedule.ExportTypeID = KMPSEnum.ExportType.FTP;
            filterSchedule.FilterScheduleID = 3;
            filterScheduleList.Add(filterSchedule);

            filterSchedule = CreateFilterScheduleObject();
            filterSchedule.GroupID = 1;
            filterSchedule.ExportTypeID = KMPSEnum.ExportType.ECN;
            filterSchedule.FilterScheduleID = 4;
            filterScheduleList.Add(filterSchedule);

            filterSchedule = CreateFilterScheduleObject();
            filterSchedule.FilterScheduleID = 5;
            filterSchedule.ExportTypeID = KMPSEnum.ExportType.Marketo;
            filterScheduleList.Add(filterSchedule);
            return filterScheduleList;
        }

        private static Client CreateClientObject()
        {
            var client = new Client();
            client.IsActive = true;
            client.IsAMS = true;
            client.ClientTestDBConnectionString = DefaultConnectionString;
            client.ClientLiveDBConnectionString = DefaultConnectionString;
            return client;
        }

        private static FilterSchedule CreateFilterScheduleObject()
        {
            var filterSchedule = new FilterSchedule();
            filterSchedule.FilterScheduleID = 1;
            filterSchedule.FilterID = 1;
            filterSchedule.FilterName = "Unit Test Filter";
            filterSchedule.EmailNotification = "Yes";
            filterSchedule.FileNameFormat = "FileName_DateTime";
            return filterSchedule;
        }

        private DataTable CreateDataTableObject()
        {
            DataTable dataTable = new DataTable();
            var columnList = new object[] { "SUBSCRIPTIONID", "EMAIL", "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS", "ADDRESS2", "ADDRESS3", "CITY", "STATE", "ZIP", "COUNTRY", "PHONE", "FAX", "MOBILE" };
            foreach (var column in columnList)
            {
                dataTable.Columns.Add(column.ToString(), typeof(string));
            }
            dataTable.Rows.Add("1", "test@unittest.com", "Test", "Test", "KMALL", "KMAll", "Test", "Test", "Test2", "Test", "Test", "175034", "US", "9872267361", "00-1234", "9872267361");
            return dataTable;
        }

        private NameValueCollection CreateAppSettingsKey()
        {
            var appSettingsKey = new NameValueCollection();
            appSettingsKey.Add("NotifyClient", bool.TrueString.ToLower());
            appSettingsKey.Add("ScheduledDate", DateTime.Now.ToString());
            appSettingsKey.Add("ScheduledTime", DateTime.Now.TimeOfDay.ToString());
            appSettingsKey.Add("MasterDBs", "Unit Test, Admin");
            appSettingsKey.Add("MasterDBs_Exclude", "Sample Test, AdminTest");
            return appSettingsKey;
        }

        private Groups CreateGroupsObject(bool masterSupression, int groupId = 1)
        {
            return new Groups
            {
                GroupID = groupId,
                MasterSupression = masterSupression,
                GroupName = "Unit Test"
            };
        }

        private void CreatePageFakeObject()
        {
            ShimClient.AllInstances.SelectBoolean = (x, y) =>
            {
                return Enumerable.Repeat(CreateClientObject(), 10).ToList();
            };
            ShimDataFunctions.GetDBNameClientConnections = (x) => { return "Unit Test"; };
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                return CreateAppSettingsKey();
            };
            ShimFilterSchedule.GetScheduleByDateTimeClientConnectionsStringString = (x, y, z) =>
            {
                return CreateFilterScheduleListObject();
            };
            ShimFilterSchedule.ExportClientConnectionsInt32 = (x, y) =>
            {
                var tupleData = new Tuple<DataTable, string, DataTable, bool>(CreateDataTableObject(), string.Empty, CreateDataTableObject(), true);
                return tupleData;
            };

            ShimProgram.AllInstances.WriteStatusString = (sender, message) => { };
            ShimProgram.AllInstances.ToCSVDataTableStringBoolean = (sender, table, headerText, showHeader) => { return string.Empty; };
            ShimProgram.AllInstances.ToTXTDataTableStringBoolean = (sender, table, headerText, showHeader) => { return string.Empty; };
            ShimProgram.AllInstances.FTPReportStringStringStringStringStringString = (sender, reportName, reportLocation, ftpServer, ftpUserName, ftpPassword, ftpFolder) => { };
            ShimProgram.AllInstances.NotifyClientStringStringString = (sender, mailTo, message, filtername) => { };
            ShimFilterExportField.getByFilterScheduleIDClientConnectionsInt32 = (clientconnection, filterScheduleID) =>
            {
                return CreateFilterExportFieldObject();
            };
            ShimProgram.AllInstances.SaveFilterScheduleLogClientConnectionsFilterScheduleInt32String = (sender, clientconnections, filterSchedule, count, fileName) => { };
            ShimProgram.AllInstances.WriteResultstoFileInt32StringListOfResult = (sender, filterScheduleID, clientdbname, results) => { };
            ShimFilterSchedule.ExportClientConnectionsInt32 = (clientconnections, filterScheduleID) =>
            {
                return new Tuple<DataTable, string, DataTable, bool>
                (
                    CreateDataTableObject(),
                    "Unit Test",
                    CreateDataTableObject(),
                    filterScheduleID == 1
                );
            };
            ShimUtilities.ExportToECNInt32StringInt32Int32StringStringListOfExportFieldsDataTableInt32EnumsGroupExportSource =
                (groupID, groupName, customerID, folderID, promoCode, jobCode, exportFields, dtSubscribers, userID, source) =>
                {
                    return new Hashtable();
                };

            ShimFilterExportField.getDisplayNameClientConnectionsInt32 = (x, y) => { return new List<FilterExportField>(); };
            ShimFilterScheduleIntegration.getByFilterScheduleIDClientConnectionsInt32 = (clientconnections, filterScheduleID) =>
            {
                return CreateFilterScheduleIntegrationObject();
            };
            ShimMarketoRestAPIProcess.AllInstances.CreateUpdateLeadsListOfDictionaryOfStringStringStringStringNullableOfInt32 =
                (sender, leads, lookupField, Partition, groupId) =>
                {
                    return CreateResultObject();
                };
        }

    }
}
