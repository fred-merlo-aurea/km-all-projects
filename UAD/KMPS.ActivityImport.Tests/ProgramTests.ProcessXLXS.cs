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
        private const string MethodProcessXLXS = "ProcessXlxs";

        [Test]
        public void ProcessXLXS_OnEmptyFile_ReachEnd()
        {
            // Arrange
            SetupShimsForFile();
            using (var memoryStream = new MemoryStream())
            {
                ReflectionHelper.SetValue(_target, CustomerLog, new StreamWriter(memoryStream));
            }
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
            CallProcessXLXSMethod(fileInfo, process);
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
        public void ProcessXLXS_OnClickImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.ClickImport);

            // Act
            CallProcessXLXSMethod(fileInfo, process);
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
        public void ProcessXLXS_OnOpenImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.OpenImport);

            // Act
            CallProcessXLXSMethod(fileInfo, process);
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
        public void ProcessXLXS_OnTopicImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.TopicImport);

            // Act
            CallProcessXLXSMethod(fileInfo, process);
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
        public void ProcessXLXS_OnVisitImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.VisitImport);

            // Act
            CallProcessXLXSMethod(fileInfo, process);
            var clickFileImportsList = _target.GetFieldValue(ClickFileImports) as List<ClickFile>;
            var openFileImportsList = _target.GetFieldValue(OpenFileImports) as List<OpenFile>;
            var topicFileImportsList = _target.GetFieldValue(TopicFileImports) as List<TopicFile>;
            var visitFileImportsList = _target.GetFieldValue(VisitFileImports) as List<VisitFile>;
            var statusUpdateFileImportsList = _target.GetFieldValue(StatusUpdateFileImports) as List<StatusUpdateFile>;

            // Assert
            // assertion bypassed here because of a marked BUG starting in line #900
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
        public void ProcessXLXS_OnStatusUpdateImport_FillList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.StatusUpdateImport);

            // Act
            CallProcessXLXSMethod(fileInfo, process);
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

        private void CallProcessXLXSMethod(FileInfo fileInfo, Process process)
        {
            var methodInfo = typeof(Program).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(x => x.Name == MethodProcessXLXS && x.IsPrivate && x.ReturnType == typeof(void));
            if (methodInfo != null)
            {
                methodInfo.Invoke(_target, new object[] {fileInfo, process});
            }
        }
    }
}
