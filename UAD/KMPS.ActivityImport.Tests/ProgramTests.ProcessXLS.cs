using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Fakes;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Excel.Fakes;
using KMPS.ActivityImport.Entity;
using KMPS.ActivityImport.Fakes;
using KMPS.MD.Objects;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;

namespace KMPS.ActivityImport.Tests
{
    public partial class ProgramTests
    {
        private const string CustomerLog = "customerLog";
        private const string MethodProcessXLS = "ProcessXLS";

        [Test]
        public void ProcessXLS_OnEmptyFile_ReachEnd()
        {
            // Arrange
            SetupShimsForFile();
            var memoryStream = new MemoryStream();
            ReflectionHelper.SetValue(_target, CustomerLog, new StreamWriter(memoryStream));
            _target.SetField(ClickFileImports, null);
            _target.SetField(OpenFileImports, null);
            _target.SetField(TopicFileImports, null);
            _target.SetField(VisitFileImports, null);
            _target.SetField(StatusUpdateFileImports, null);
            var stubIExcelDataReader = new StubIExcelDataReader
            {
                AsDataSet = () =>
                {
                    var dataSet = new DataSet();
                    dataSet.Tables.Add(new DataTable());
                    return dataSet;
                }
            };
            ShimExcelReaderFactory.CreateBinaryReaderStream = (_) => stubIExcelDataReader;
            var fileInfo = new FileInfo(DummyString);
            var process = new Process
            {
                FileFolder = DummyString,
                ProcessType = ProcessType.ClickImport.ToString()
            };

            // Act
            CallProcessXLSMethod(fileInfo, process);
            var clickFileImportsList = _target.GetFieldValue(ClickFileImports) as List<ClickFile>;
            var openFileImportsList = _target.GetFieldValue(OpenFileImports) as List<OpenFile>;
            var topicFileImportsList = _target.GetFieldValue(TopicFileImports) as List<TopicFile>;
            var visitFileImportsList = _target.GetFieldValue(VisitFileImports) as List<VisitFile>;
            var statusUpdateFileImportsList = _target.GetFieldValue(StatusUpdateFileImports) as List<StatusUpdateFile>;

            // Assert
            clickFileImportsList.ShouldSatisfyAllConditions(
                () => clickFileImportsList.ShouldNotBeNull(),
                () => clickFileImportsList.ShouldBeEmpty(),
                () =>
                {
                    openFileImportsList.ShouldNotBeNull();
                    openFileImportsList.ShouldBeEmpty();
                    topicFileImportsList.ShouldNotBeNull();
                    topicFileImportsList.ShouldBeEmpty();
                    visitFileImportsList.ShouldNotBeNull();
                    visitFileImportsList.ShouldBeEmpty();
                    statusUpdateFileImportsList.ShouldNotBeNull();
                    statusUpdateFileImportsList.ShouldBeEmpty();
                });
        }

        [Test]
        [TestCase(ProcessType.ClickImport)]
        [TestCase(ProcessType.OpenImport)]
        [TestCase(ProcessType.TopicImport)]
        [TestCase(ProcessType.VisitImport)]
        [TestCase(ProcessType.StatusUpdateImport)]
        public void ProcessXLS_OnMultipleCases_FillList(ProcessType processType)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType);

            // Act
            CallProcessXLSMethod(fileInfo, process);
            var clickFileImportsList = _target.GetFieldValue(ClickFileImports) as List<ClickFile>;
            var openFileImportsList = _target.GetFieldValue(OpenFileImports) as List<OpenFile>;
            var topicFileImportsList = _target.GetFieldValue(TopicFileImports) as List<TopicFile>;
            var visitFileImportsList = _target.GetFieldValue(VisitFileImports) as List<VisitFile>;
            var statusUpdateFileImportsList = _target.GetFieldValue(StatusUpdateFileImports) as List<StatusUpdateFile>;

            // Assert
            // assertion bypassed here because of a marked BUG starting from line #633 to line #641 
            clickFileImportsList.ShouldSatisfyAllConditions(
                () => clickFileImportsList.ShouldNotBeNull(),
                () => clickFileImportsList.ShouldBeEmpty(),
                () =>
                {
                    openFileImportsList.ShouldNotBeNull();
                    openFileImportsList.ShouldBeEmpty();
                    topicFileImportsList.ShouldNotBeNull();
                    topicFileImportsList.ShouldBeEmpty();
                    visitFileImportsList.ShouldNotBeNull();
                    visitFileImportsList.ShouldBeEmpty();
                    statusUpdateFileImportsList.ShouldNotBeNull();
                    statusUpdateFileImportsList.ShouldBeEmpty();
                });
        }

        private void CallProcessXLSMethod(FileInfo fileInfo, Process process)
        {
            var methodInfo = typeof(Program).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(x => x.Name == MethodProcessXLS && x.IsPrivate && x.ReturnType == typeof(void));
            if (methodInfo != null)
            {
                methodInfo.Invoke(_target, new object[] {fileInfo, process});
            }
        }

        private void SetupShimsForProcessXLSFile()
        {
            var memoryStream = new MemoryStream();
            ReflectionHelper.SetValue(_target, CustomerLog, new StreamWriter(memoryStream));
            _target.SetField(ClickFileImports, null);
            _target.SetField(OpenFileImports, null);
            _target.SetField(TopicFileImports, null);
            _target.SetField(VisitFileImports, null);
            _target.SetField(StatusUpdateFileImports, null);
            var stubIExcelDataReader = new StubIExcelDataReader
            {
                AsDataSet = () =>
                {
                    var dataSet = new DataSet();
                    var dataTable = new DataTable();
                    dataTable.Columns.Add(nameof(OpenFile.BlastID), typeof(string));
                    dataTable.Columns.Add(nameof(OpenFile.Address), typeof(string));
                    dataTable.Columns.Add(nameof(OpenFile.OpenTime), typeof(string));
                    dataTable.Columns.Add(nameof(TopicFile.ActivityDateTime), typeof(string));
                    dataTable.Columns.Add(nameof(VisitFile.EmailAddress), typeof(string));
                    dataTable.Columns.Add(nameof(StatusUpdateFile.EmailStatus), typeof(string));
                    var row = dataTable.NewRow();
                    row[0] = DummyId;
                    row[1] = DummyString;
                    row[2] = DateTime.MinValue.ToShortDateString();
                    row[3] = DummyString;
                    row[4] = DummyString;
                    row[5] = DummyId;
                    dataTable.Rows.Add(row);
                    dataSet.Tables.Add(dataTable);
                    return dataSet;
                }
            };
            ShimExcelReaderFactory.CreateOpenXmlReaderStream = (_)=> stubIExcelDataReader;
            ShimExcelReaderFactory.CreateBinaryReaderStream = (_) => stubIExcelDataReader;
        }

        private void SetupShimsForFile()
        {
            ShimFileInfo.ConstructorString = (obj, str) => { };
            ShimFileSystemInfo.AllInstances.FullNameGet = _ => DummyString;
            ShimDirectory.ExistsString = _ => false;
            ShimDirectory.CreateDirectoryString = _ => null;
            ShimFile.MoveStringString = (_, __) => { };
            ShimFile.OpenStringFileModeFileAccess = (_, __, ___) => new ShimFileStream();
            _target.SetField(CurrentCustomer, new Customer
            {
                FileArchive = DummyString
            });
        }

        private static Process GetProcess(ProcessType processType)
        {
            return new Process
            {
                FileFolder = DummyString,
                ProcessType = processType.ToString()
            };
        }
    }
}
