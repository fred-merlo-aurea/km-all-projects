using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Excel.Fakes;
using MAF.NorthStarImport.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;

namespace MAF.NorthStarImport.Tests
{
    public partial class ProgramTests
    {
        private const string MethodProcessXLS = "ProcessXLS";

        [Test]
        public void ProcessXLS_OnEmptyFile_ReachEnd()
        {
            // Arrange
            SetupShimsForFile();
            var memoryStream = new MemoryStream();
            ReflectionHelper.SetValue(_target, CustomerLog, new StreamWriter(memoryStream));
            _target.SetField(SubscriberList, null);
            _target.SetField(UndeliverableEmailsList, null);
            _target.SetField(SpamComplaintList, null);
            _target.SetField(UnsubscribeList, null);
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
                ProcessType = ProcessType.Subscribe.ToString()
            };

            // Act
            CallProcessXLSMethod(fileInfo, process);
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

        public void ProcessXLS_OnSubscriberList_FillSubscriberList(ProcessType processType, MFFields mfField)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType, mfField);

            // Act
            CallProcessXLSMethod(fileInfo, process);
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
        public void ProcessXLS_OnUndeliverableEmailsList_FillSubscriberList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.UndeliverableEmails, MFFields.Email);

            // Act
            CallProcessXLSMethod(fileInfo, process);
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

        public void ProcessXLS_OnSpamComplaintList_FillSubscriberList(ProcessType processType, MFFields mfField)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType, mfField);

            // Act
            CallProcessXLSMethod(fileInfo, process);
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

        public void ProcessXLS_OnUnsubscribeList_FillSubscriberList(ProcessType processType, MFFields mfField)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType, mfField);

            // Act
            CallProcessXLSMethod(fileInfo, process);
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
            _target.SetField(SubscriberList, null);
            _target.SetField(UndeliverableEmailsList, null);
            _target.SetField(SpamComplaintList, null);
            _target.SetField(UnsubscribeList, null);
            var stubIExcelDataReader = new StubIExcelDataReader
            {
                AsDataSet = () =>
                {
                    var dataSet = new DataSet();
                    var dataTable = new DataTable();
                    dataTable.Columns.Add(ColumnName, typeof(string));
                    var row = dataTable.NewRow();
                    row[0] = ColumnValue;
                    dataTable.Rows.Add(row);
                    dataSet.Tables.Add(dataTable);
                    return dataSet;
                }
            };
            ShimExcelReaderFactory.CreateOpenXmlReaderStream = (_)=> stubIExcelDataReader;
            ShimExcelReaderFactory.CreateBinaryReaderStream = (_) => stubIExcelDataReader;
        }
    }
}
