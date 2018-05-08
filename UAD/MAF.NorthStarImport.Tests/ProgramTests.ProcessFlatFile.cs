using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Fakes;
using MAF.NorthStarImport.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;

namespace MAF.NorthStarImport.Tests
{
    public partial class ProgramTests
    {
        private const string MethodProcessFlatFile = "ProcessFlatFile";
        private const string DummyString = "DummyString";
        private const string CustomerLog = "customerLog";
        private const string ColumnName = "ColumnName";
        private const string ColumnValue = "ColumnValue";
        private const string SubscriberList = "SubscriberList";
        private const string UndeliverableEmailsList = "UndeliverableEmailsList";
        private const string SpamComplaintList = "SpamComplaintList";
        private const string UnsubscribeList = "UnsubscribeList";

        [Test]
        public void ProcessFlatFile_OnEmptyFile_ReachEnd()
        {
            // Arrange
            SetupShimsForFile();
            var memoryStream = new MemoryStream();
            ShimProgram.AllInstances.LoadFileFileInfoProcess = (_, __, ___) => new DataTable();
            ReflectionHelper.SetValue(_target, CustomerLog, new StreamWriter(memoryStream));
            var fileInfo = new FileInfo(DummyString);
            var process = new Process
            {
                FileFolder = DummyString,
                ProcessType = ProcessType.Subscribe.ToString()
            };

            // Act
            ReflectionHelper.CallMethod(_target, MethodProcessFlatFile, fileInfo, process);
            var subscriberList = _target.GetFieldValue(SubscriberList) as List<Subscribe>;
            var undeliverableEmailsList = _target.GetFieldValue(UndeliverableEmailsList) as List<Subscribe>;
            var spamComplaintList = _target.GetFieldValue(SpamComplaintList) as List<Subscribe>;
            var unsubscribeList = _target.GetFieldValue(UnsubscribeList) as List<Subscribe>;

            // Assert
            subscriberList.ShouldSatisfyAllConditions(
                () => subscriberList.ShouldNotBeNull(),
                () => subscriberList.Count.ShouldBe(0)
            );
            undeliverableEmailsList.ShouldBeNull();
            spamComplaintList.ShouldBeNull();
            unsubscribeList.ShouldBeNull();
        }

        [Test]
        [TestCase(ProcessType.Subscribe, MFFields.Email)]
        [TestCase(ProcessType.Subscribe, MFFields.PubCode)]
        [TestCase(ProcessType.Subscribe, MFFields.FName)]
        [TestCase(ProcessType.Subscribe, MFFields.LName)]

        public void ProcessFlatFile_OnSubscriberList_FillSubscriberList(ProcessType processType, MFFields mfField)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessFlatFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType, mfField);
            
            // Act
            ReflectionHelper.CallMethod(_target, MethodProcessFlatFile, fileInfo, process);
            var subscriberList = _target.GetFieldValue(SubscriberList) as List<Subscribe>;
            var undeliverableEmailsList = _target.GetFieldValue(UndeliverableEmailsList) as List<UpdateEmailStatus>;
            var spamComplaintList = _target.GetFieldValue(SpamComplaintList) as List<UpdateEmailStatus>;
            var unsubscribeList = _target.GetFieldValue(UnsubscribeList) as List<Unsubscribe>;

            // Assert
            subscriberList.ShouldSatisfyAllConditions(
                () => subscriberList.ShouldNotBeNull(),
                () => subscriberList.Count.ShouldBe(1)
            );
            undeliverableEmailsList.ShouldBeNull();
            spamComplaintList.ShouldBeNull();
            unsubscribeList.ShouldBeNull();
        }

        [Test]
        public void ProcessFlatFile_OnUndeliverableEmailsList_FillSubscriberList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessFlatFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.UndeliverableEmails, MFFields.Email);

            // Act
            ReflectionHelper.CallMethod(_target, MethodProcessFlatFile, fileInfo, process);
            var subscriberList = _target.GetFieldValue(SubscriberList) as List<Subscribe>;
            var undeliverableEmailsList = _target.GetFieldValue(UndeliverableEmailsList) as List<UpdateEmailStatus>;
            var spamComplaintList = _target.GetFieldValue(SpamComplaintList) as List<UpdateEmailStatus>;
            var unsubscribeList = _target.GetFieldValue(UnsubscribeList) as List<Unsubscribe>;

            // Assert
            undeliverableEmailsList.ShouldSatisfyAllConditions(
                () => undeliverableEmailsList.ShouldNotBeNull(),
                () => undeliverableEmailsList.Count.ShouldBe(1)
            );
            subscriberList.ShouldBeNull();
            spamComplaintList.ShouldBeNull();
            unsubscribeList.ShouldBeNull();
        }

        [Test]
        [TestCase(ProcessType.SpamComplaints, MFFields.Email)]
        [TestCase(ProcessType.SpamComplaints, MFFields.StatusUpdatedDate)]

        public void ProcessFlatFile_OnSpamComplaintList_FillSubscriberList(ProcessType processType, MFFields mfField)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessFlatFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType, mfField);

            // Act
            ReflectionHelper.CallMethod(_target, MethodProcessFlatFile, fileInfo, process);
            var subscriberList = _target.GetFieldValue(SubscriberList) as List<Subscribe>;
            var undeliverableEmailsList = _target.GetFieldValue(UndeliverableEmailsList) as List<UpdateEmailStatus>;
            var spamComplaintList = _target.GetFieldValue(SpamComplaintList) as List<UpdateEmailStatus>;
            var unsubscribeList = _target.GetFieldValue(UnsubscribeList) as List<Unsubscribe>;

            // Assert
            spamComplaintList.ShouldSatisfyAllConditions(
                () => spamComplaintList.ShouldNotBeNull(),
                () => spamComplaintList.Count.ShouldBe(1)
            );
            undeliverableEmailsList.ShouldBeNull();
            subscriberList.ShouldBeNull();
            unsubscribeList.ShouldBeNull();
        }

        [Test]
        [TestCase(ProcessType.Unsubscribe, MFFields.Email)]
        [TestCase(ProcessType.Unsubscribe, MFFields.PubCode)]
        [TestCase(ProcessType.Unsubscribe, MFFields.StatusUpdatedDate)]
        [TestCase(ProcessType.Unsubscribe, MFFields.StatusUpdatedReason)]

        public void ProcessFlatFile_OnUnsubscribeList_FillSubscriberList(ProcessType processType, MFFields mfField)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessFlatFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType, mfField);

            // Act
            ReflectionHelper.CallMethod(_target, MethodProcessFlatFile, fileInfo, process);
            var subscriberList = _target.GetFieldValue(SubscriberList) as List<Subscribe>;
            var undeliverableEmailsList = _target.GetFieldValue(UndeliverableEmailsList) as List<UpdateEmailStatus>;
            var spamComplaintList = _target.GetFieldValue(SpamComplaintList) as List<UpdateEmailStatus>;
            var unsubscribeList = _target.GetFieldValue(UnsubscribeList) as List<Unsubscribe>;

            // Assert
            unsubscribeList.ShouldSatisfyAllConditions(
                () => unsubscribeList.ShouldNotBeNull(),
                () => unsubscribeList.Count.ShouldBe(1)
            );
            undeliverableEmailsList.ShouldBeNull();
            spamComplaintList.ShouldBeNull();
            subscriberList.ShouldBeNull();
        }

        private Process GetProcess(ProcessType processType, MFFields mfField)
        {
            return new Process
            {
                FileFolder = DummyString,
                ProcessType = processType.ToString(),
                HeaderMap =  new List<Header>
                {
                    new Header
                    {
                        FileField = ColumnName,
                        Ignore = false,
                        MFField = mfField.ToString()
                    }
                }
            };
        }

        private void SetupShimsForFile()
        {
            ShimFileInfo.ConstructorString = (obj, str) => { };
            ShimFileSystemInfo.AllInstances.FullNameGet = _ => DummyString;
            ShimDirectory.ExistsString = _ => false;
            ShimDirectory.CreateDirectoryString = _ => null;
            ShimFile.MoveStringString = (_, __) => { };
            ShimFile.OpenStringFileModeFileAccess = (_, __, ___) => new ShimFileStream();
        }

        private void SetupShimsForProcessFlatFile()
        {
            var memoryStream = new MemoryStream();
            ReflectionHelper.SetValue(_target, CustomerLog, new StreamWriter(memoryStream));
            ShimProgram.AllInstances.LoadFileFileInfoProcess = (_, __, ___) =>
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add(ColumnName, typeof(string));
                var row = dataTable.NewRow();
                row[0] = ColumnValue;
                dataTable.Rows.Add(row);
                return dataTable;
            };
            _target.SetField(SubscriberList, null);
            _target.SetField(UndeliverableEmailsList, null);
            _target.SetField(SpamComplaintList, null);
            _target.SetField(UnsubscribeList, null);
        }
    }
}
