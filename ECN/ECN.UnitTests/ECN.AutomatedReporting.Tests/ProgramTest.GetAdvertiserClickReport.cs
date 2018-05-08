using System;
using System.IO.Fakes;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization.Fakes;
using System.Net.Mail.Fakes;
using Shouldly;
using ecn.automatedreporting;
using NUnit.Framework;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Activity.Report;
using ECN_Framework_Entities.Communicator;
using EntityAdvertiserClickReport = ecn.automatedreporting.Reports.AdvertiserClickReport;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetAdvertiserClickReport = "GetAdvertiserClickReport";
        private const string SuccessfulResultStringGetAdvertiserClickReport =
            "Your scheduled report of Advertiser Click has been attached";
        private const string NoRecordStringGetAdvertiserClickReport =
            "Your scheduled report of Advertiser Click didn't return any data";
        private const string FailureMessageForGetAdvertiserClickReport =
            "Your scheduled report of Advertiser Click has failed";
        
        [Test]
        public void GetAdvertiserClickReport_FileTypePdf_RecurrenceDefault_NoException()
        {
            // Arrange
            InitilizeGetAdvertiserClickReportTests(FileTypePdf, String.Empty);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = string.Empty;
            var report = new EntityAdvertiserClickReport(
                message,
                constructedBody,
                _reportSchedule,
                DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetAdvertiserClickReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetAdvertiserClickReport_FileTypeXls_RecurrenceMonthly_NoException()
        {
            // Arrange
            InitilizeGetAdvertiserClickReportTests(FileTypeXls, ValueRecurrenceTypeMonthly);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = string.Empty;
            var report = new EntityAdvertiserClickReport(
                message,
                constructedBody,
                _reportSchedule,
                DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetAdvertiserClickReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetAdvertiserClickReport_FileTypeXml_RecurrenceWeekly_NoException()
        {
            // Arrange
            InitilizeGetAdvertiserClickReportTests(FileTypeXml, ValueRecurrenceTypeWeekly);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new EntityAdvertiserClickReport(
                message,
                constructedBody,
                _reportSchedule,
                DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetAdvertiserClickReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetAdvertiserClickReport_FileTypeCsv_RecurrenceDaily_NoException()
        {
            // Arrange
            InitilizeGetAdvertiserClickReportTests(FileTypeCsv, ValueRecurrenceTypeDaily);
            var message = new EmailDirect();
            var constructedBody = string.Empty;
            var report = new EntityAdvertiserClickReport(
                message,
                constructedBody,
                _reportSchedule,
                DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetAdvertiserClickReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetAdvertiserClickReport_FileTypeDefault_RecurrenceTypeMonthly_NoData()
        {
            // Arrange
            InitilizeGetAdvertiserClickReportTests(String.Empty, ValueRecurrenceTypeMonthly);
            ShimAdvertiserClickReport.GetListInt32DateTimeDateTime =
                (x1, x2, x3) => new List<AdvertiserClickReport>();
            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 2, 2));
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new EntityAdvertiserClickReport(
                message,
                constructedBody,
                _reportSchedule,
                DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(NoRecordStringGetAdvertiserClickReport);
        }

        [Test]
        public void GetAdvertiserClickReport_FileTypeDefault_RecurrenceDefault_NoReportParameterException()
        {
            // Arrange
            InitilizeGetAdvertiserClickReportTests(String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                CreatedUserIDGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };

            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new EntityAdvertiserClickReport(
                message,
                constructedBody,
                _reportSchedule,
                DateTime.Today);

            // Act	
            var returnResult = report.Execute();            

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureMessageForGetAdvertiserClickReport);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void InitilizeGetAdvertiserClickReportTests(string fileFormat, string recurrenceType)
        {
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.AppendAllTextStringString = (x1, x2) => { };

            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                CustomerIDGet = () => ValueInt1,
                BlastIDGet = () => ValueInt1,
                ReportParametersGet = () =>
                {
                    var sampleXMLDoc = new XmlDocument();
                    var reportScheduleElement = sampleXMLDoc.CreateElement(XMLKeyReportSchedule);
                    sampleXMLDoc.AppendChild(reportScheduleElement);
                    var sampleElement = sampleXMLDoc.CreateElement(XMLKeyGroupID);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueText1));
                    reportScheduleElement.AppendChild(sampleElement);

                    return sampleXMLDoc.InnerXml;
                },
                ExportFormatGet = () => fileFormat,
                CreatedUserIDGet = () => ValueInt1,
                RecurrenceTypeGet = () => recurrenceType,
            };

            ShimAdvertiserClickReport.GetListInt32DateTimeDateTime =
                (x1, x2, x3) => new List<AdvertiserClickReport>() { new AdvertiserClickReport { EmailSubject = DummyText } };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (p) => new Group();
            ShimAttachment.ConstructorString = (x1,x2) => { };
            ShimFileInfo.AllInstances.AppendText = (p) => new ShimStreamWriter();
            ShimXmlSerializer.AllInstances.SerializeTextWriterObject = (x1, x2, x3) => { };

            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 1, 1));
        }
    }
}
