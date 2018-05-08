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
        private const string MethodProcessFlatFile = "ProcessFlatFile";

        [Test]
        public void ProcessFlatFile_OnEmptyFile_ReachEnd()
        {
            // Arrange
            SetupShimsForFile();
            ShimProgram.AllInstances.LoadFileFileInfoProcess = (_, __, ___) => new DataTable();
            ShimProgram.AllInstances.EmailBadFileFileInfo = (_, __) => { };
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
            ShimExcelReaderFactory.CreateOpenXmlReaderStream = (_) => stubIExcelDataReader;
            var fileInfo = new FileInfo(DummyString);
            var process = new Process
            {
                FileFolder = DummyString,
                ProcessType = ProcessType.ClickImport.ToString()
            };

            // Act
            CallProcessFlatFileMethod(fileInfo, process);
            var clickFileImportsList = _target.GetFieldValue(ClickFileImports) as List<ClickFile>;
            var openFileImportsList = _target.GetFieldValue(OpenFileImports) as List<OpenFile>;
            var topicFileImportsList = _target.GetFieldValue(TopicFileImports) as List<TopicFile>;
            var visitFileImportsList = _target.GetFieldValue(VisitFileImports) as List<VisitFile>;
            var statusUpdateFileImportsList = _target.GetFieldValue(StatusUpdateFileImports) as List<StatusUpdateFile>;

            // Assert
            clickFileImportsList.ShouldSatisfyAllConditions(
                () => clickFileImportsList.ShouldBeNull(),
                () =>
                {
                    openFileImportsList.ShouldBeNull();
                    topicFileImportsList.ShouldBeNull();
                    visitFileImportsList.ShouldBeNull();
                    statusUpdateFileImportsList.ShouldBeNull();
                });
        }

        [Test]
        public void ProcessFlatFile_OnClickImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessFlatFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.ClickImport);

            // Act
            CallProcessFlatFileMethod(fileInfo, process);
            var clickFileImportsList = _target.GetFieldValue(ClickFileImports) as List<ClickFile>;
            var openFileImportsList = _target.GetFieldValue(OpenFileImports) as List<OpenFile>;
            var topicFileImportsList = _target.GetFieldValue(TopicFileImports) as List<TopicFile>;
            var visitFileImportsList = _target.GetFieldValue(VisitFileImports) as List<VisitFile>;
            var statusUpdateFileImportsList = _target.GetFieldValue(StatusUpdateFileImports) as List<StatusUpdateFile>;

            // Assert
            clickFileImportsList.ShouldSatisfyAllConditions(
                () => clickFileImportsList.ShouldNotBeNull(),
                () => clickFileImportsList.ShouldNotBeEmpty(),
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
        public void ProcessFlatFile_OnOpenImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessFlatFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.OpenImport);

            // Act
            CallProcessFlatFileMethod(fileInfo, process);
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
                    openFileImportsList.ShouldNotBeEmpty();
                    topicFileImportsList.ShouldNotBeNull();
                    topicFileImportsList.ShouldBeEmpty();
                    visitFileImportsList.ShouldNotBeNull();
                    visitFileImportsList.ShouldBeEmpty();
                    statusUpdateFileImportsList.ShouldNotBeNull();
                    statusUpdateFileImportsList.ShouldBeEmpty();
                });
        }

        [Test]
        public void ProcessFlatFile_OnTopicImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessFlatFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.TopicImport);

            // Act
            CallProcessFlatFileMethod(fileInfo, process);
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
                    topicFileImportsList.ShouldNotBeEmpty();
                    visitFileImportsList.ShouldNotBeNull();
                    visitFileImportsList.ShouldBeEmpty();
                    statusUpdateFileImportsList.ShouldNotBeNull();
                    statusUpdateFileImportsList.ShouldBeEmpty();
                });
        }

        [Test]
        public void ProcessFlatFile_OnVisitImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessFlatFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.VisitImport);

            // Act
            CallProcessFlatFileMethod(fileInfo, process);
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
                    visitFileImportsList.ShouldNotBeEmpty();
                    statusUpdateFileImportsList.ShouldNotBeNull();
                    statusUpdateFileImportsList.ShouldBeEmpty();
                });
        }

        [Test]
        public void ProcessFlatFile_OnStatusUpdateImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessFlatFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.StatusUpdateImport);

            // Act
            CallProcessFlatFileMethod(fileInfo, process);
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
                    statusUpdateFileImportsList.ShouldNotBeEmpty();
                });
        }

        private void CallProcessFlatFileMethod(FileInfo fileInfo, Process process)
        {
            var methodInfo = typeof(Program).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(x => x.Name == MethodProcessFlatFile && x.IsPrivate && x.ReturnType == typeof(void));
            if (methodInfo != null)
            {
                methodInfo.Invoke(_target, new object[] {fileInfo, process});
            }
        }

        private void SetupShimsForProcessFlatFile()
        {
            var memoryStream = new MemoryStream();
            ReflectionHelper.SetValue(_target, CustomerLog, new StreamWriter(memoryStream));
            _target.SetField(ClickFileImports, null);
            _target.SetField(OpenFileImports, null);
            _target.SetField(TopicFileImports, null);
            _target.SetField(VisitFileImports, null);
            _target.SetField(StatusUpdateFileImports, null);
            ShimProgram.AllInstances.LoadFileFileInfoProcess = (_, __, ___) =>
            {
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
                return dataTable;
            };
        }
    }
}
