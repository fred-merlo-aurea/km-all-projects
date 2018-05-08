using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Excel.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;

namespace MAF.NorthStarImport.Tests
{
    public partial class ProgramTests
    {
        private const string MethodProcessXLXS = "ProcessXLXS";

        [Test]
        public void ProcessXLXS_OnEmptyFile_ReachEnd()
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
            ShimExcelReaderFactory.CreateOpenXmlReaderStream = (_) => stubIExcelDataReader;
            var fileInfo = new FileInfo(DummyString);
            var process = new Process
            {
                FileFolder = DummyString,
                ProcessType = ProcessType.Subscribe.ToString()
            };

            // Act
            CallProcessXLXSMethod(fileInfo, process);
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

        public void ProcessXLXS_OnSubscriberList_FillSubscriberList(ProcessType processType, MFFields mfField)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType, mfField);

            // Act
            CallProcessXLXSMethod(fileInfo, process);
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
        public void ProcessXLXS_OnUndeliverableEmailsList_FillSubscriberList()
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(ProcessType.UndeliverableEmails, MFFields.Email);

            // Act
            CallProcessXLXSMethod(fileInfo, process);
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

        public void ProcessXLXS_OnSpamComplaintList_FillSubscriberList(ProcessType processType, MFFields mfField)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType, mfField);

            // Act
            CallProcessXLXSMethod(fileInfo, process);
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

        public void ProcessXLXS_OnUnsubscribeList_FillSubscriberList(ProcessType processType, MFFields mfField)
        {
            // Arrange
            SetupShimsForFile();
            SetupShimsForProcessXLSFile();
            var fileInfo = new FileInfo(DummyString);
            var process = GetProcess(processType, mfField);

            // Act
            CallProcessXLXSMethod(fileInfo, process);
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

        private void CallProcessXLXSMethod(FileInfo fileInfo, Process process)
        {
            var methodInfo = typeof(Program).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(x => x.Name == MethodProcessXLXS && x.IsPrivate && x.ReturnType == typeof(void));
            if (methodInfo != null)
            {
                methodInfo.Invoke(_target, new object[] { fileInfo, process });
            }
        }
    }
}
