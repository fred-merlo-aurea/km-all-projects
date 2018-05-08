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
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_Entities.Activity.Report;
using TestBlastDeliveryReport = ecn.automatedreporting.Reports.BlastDeliveryReport;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetBlastDeliveryReport = "GetBlastDeliveryReport";
        private const string SuccessfulResultStringGetBlastDeliveryReport =
            "Your scheduled report of Blast Delivery has been attached";
        private const string NoRecordStringGetBlastDeliveryReport =
            "Your scheduled report of Blast Delivery didn't return any data";
        private const string FailureMessageForGetBlastDeliveryReport =
            "Your scheduled report of Blast Delivery has either failed or didn't return any data.";

        [Test]
        public void GetBlastDeliveryReport_FileTypePdf_RecurrenceDefault_NoException()
        {
            // Arrange
            InitilizeGetBlastDeliveryReportTests(FileTypePdf, String.Empty);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestBlastDeliveryReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetBlastDeliveryReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetBlastDeliveryReport_FileTypeXls_RecurrenceMonthly_NoException()
        {
            // Arrange
            InitilizeGetBlastDeliveryReportTests(FileTypeXls, ValueRecurrenceTypeMonthly);
            SettingsForFileTypePdfXls();
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestBlastDeliveryReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetBlastDeliveryReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetBlastDeliveryReport_FileTypeXml_RecurrenceWeekly_NoException()
        {
            // Arrange
            InitilizeGetBlastDeliveryReportTests(FileTypeXml, ValueRecurrenceTypeWeekly);
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestBlastDeliveryReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetBlastDeliveryReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetBlastDeliveryReport_FileTypeCsv_RecurrenceDaily_NoException()
        {
            // Arrange
            InitilizeGetBlastDeliveryReportTests(FileTypeCsv, ValueRecurrenceTypeDaily);
            ShimEmailPerformanceByDomainReport.AddDelimiterListOfEmailPerformanceByDomain =
                (p) => String.Empty;
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestBlastDeliveryReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetBlastDeliveryReport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetBlastDeliveryReport_FileTypeDefault_RecurrenceTypeMonthly_NoData()
        {
            // Arrange
            InitilizeGetBlastDeliveryReportTests(String.Empty, ValueRecurrenceTypeMonthly);
            ShimBlastDelivery.GetStringDateTimeDateTimeBoolean =
                (x1, x2, x3, x4) => new List<BlastDelivery>();
            _testedClass.SetStaticFieldOrProperty(FieldMasterStartDate, new DateTime(2018, 2, 2));
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestBlastDeliveryReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(NoRecordStringGetBlastDeliveryReport);
        }

        [Test]
        public void GetBlastDeliveryReport_FileTypeDefault_RecurrenceDefault_NoReportParameterException()
        {
            // Arrange
            InitilizeGetBlastDeliveryReportTests(String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                ReportParametersGet = () => CreateXmlSample(),
                RecurrenceTypeGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };
            var message = new EmailDirect();
            var constructedBody = String.Empty;
            var report = new TestBlastDeliveryReport(message, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureMessageForGetBlastDeliveryReport);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void InitilizeGetBlastDeliveryReportTests(string fileFormat, string recurrenceType)
        {
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.AppendAllTextStringString = (x1, x2) => { };

            ShimCustomer.GetByCustomerIDInt32Boolean = (x1, x2) => null;

            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                CustomerIDGet = () => ValueInt1,
                ReportParametersGet = () => CreateXmlSample(),
                ExportFormatGet = () => fileFormat,
                RecurrenceTypeGet = () => recurrenceType
            };

            ShimBlastDelivery.GetStringDateTimeDateTimeBoolean = 
                (x1, x2, x3, x4) => new List<BlastDelivery>() { new BlastDelivery { EmailSubject = DummyText, Delivered = ValueInt1} };
            ShimAttachment.ConstructorString = (x1,x2) => { };
            ShimFileInfo.AllInstances.AppendText = (p) => new ShimStreamWriter();
            ShimXmlSerializer.AllInstances.SerializeTextWriterObject = (x1, x2, x3) => { };
        }

        private string CreateXmlSample()
        {
            var sampleXMLDoc = new XmlDocument();
            var reportScheduleElement = sampleXMLDoc.CreateElement(XMLKeyReportSchedule);
            sampleXMLDoc.AppendChild(reportScheduleElement);
            var sampleElement = sampleXMLDoc.CreateElement(XMLKeyCustomerID);
            sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueText1));
            reportScheduleElement.AppendChild(sampleElement);

            return sampleXMLDoc.InnerXml;
        }
    }
}
