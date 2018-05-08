using System;
using System.IO.Fakes;
using System.Xml;
using System.Net.Fakes;
using System.IO;
using Shouldly;
using ecn.automatedreporting;
using NUnit.Framework;
using System.Data;
using TestGroupExportReport = ecn.automatedreporting.Reports.GroupExportReport;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetGroupExport = "GetGroupExport";
        private const string XMLKeyFilterId = "FilterID";
        private const string SuccessfulResultStringGetGroupExport =
            "Your scheduled export of Group Export has been moved to";
        private const string NoRecordStringGetGroupExport = 
            "Your scheduled export of Group Export didn't return any data";
        private const string FailureMessageFoExportGroup =
            "Your scheduled export of Group Export has failed";
        
        [Test]
        public void GetGroupExport_FileFormatDefault_NoException()
        {
            // Arrange
            InitilizeGetGroupExportTests(String.Empty, String.Empty, String.Empty);
            var constructedBody = String.Empty;
            var report = new TestGroupExportReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetGroupExport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupExport_FileFormatTxt_AllSubscribeTypeCode_NoException()
        {
            // Arrange
            InitilizeGetGroupExportTests(FileFormatTxt, ValueTextCharStar, String.Empty);
            var constructedBody = String.Empty;
            var report = new TestGroupExportReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetGroupExport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupExport_FileFormatCsv_ExportSettingStandAlone_NoException()
        {
            // Arrange
            InitilizeGetGroupExportTests(FileFormatCsv, String.Empty, ExportSettingStandAlone);
            var constructedBody = String.Empty;
            var report = new TestGroupExportReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetGroupExport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupExport_FileFormatXls_ExportSettingAllUdfs_NoException()
        {
            // Arrange
            InitilizeGetGroupExportTests(FileFormatXls, String.Empty, ExportSettingAllUdfs);
            var constructedBody = String.Empty;
            var report = new TestGroupExportReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetGroupExport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupExport_FileFormatXml_NoException()
        {
            // Arrange
            InitilizeGetGroupExportTests(FileFormatXml, String.Empty, String.Empty);

            var constructedBody = String.Empty;
            var report = new TestGroupExportReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetGroupExport);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetGroupExport_FileFormatDefault_FTPException()
        {
            // Arrange
            InitilizeGetGroupExportTests(String.Empty, String.Empty, String.Empty);
            ShimWebRequest.CreateString = (p) =>
            {
                throw new Exception(ExceptionMessageFTP);
            };
            var constructedBody = String.Empty;
            var report = new TestGroupExportReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureMessageFoExportGroup);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionMessageFTP);
        }

        [Test]
        public void GetGroupExport_FileFormatDefault_NoData()
        {
            // Arrange
            InitilizeGetGroupExportTests(String.Empty, String.Empty, String.Empty);
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimEmailGroup.
                GetGroupEmailProfilesWithUDFInt32Int32StringStringString =
                (x1, x2, x3, x4, x5) => new DataTable();
            var constructedBody = String.Empty;
            var report = new TestGroupExportReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(NoRecordStringGetGroupExport);
        }

        [Test]
        public void GetGroupExport_FileFormatDefault_InvalidGroupId()
        {
            // Arrange
            InitilizeGetGroupExportTests(String.Empty, String.Empty, String.Empty, ValueText0);
            var constructedBody = String.Empty;
            var report = new TestGroupExportReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureMessageFoExportGroup);
        }

        [Test]
        public void GetGroupExport_FileFormatDefault_NoReportParameterException()
        {
            // Arrange
            InitilizeGetGroupExportTests(String.Empty, String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                ReportParametersGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };
            var constructedBody = String.Empty;
            var report = new TestGroupExportReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureMessageFoExportGroup);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void InitilizeGetGroupExportTests(string fileFormat, string exportSubscribeTypeCode,
            string exportSettings, string groupId = ValueText1)
        {
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
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(groupId));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFTPURL);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyCustomerID);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueText1));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFTPUsername);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFTPPassword);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyExportSettings);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(exportSettings));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyExportFormat);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(fileFormat));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyExportSubscribeTypeCode);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(exportSubscribeTypeCode));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFilterId);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(ValueText1));
                    reportScheduleElement.AppendChild(sampleElement);

                    return sampleXMLDoc.InnerXml;
                },
                ExportFormatGet = () => fileFormat,
                CreatedUserIDGet = () => ValueInt1
            };

            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.ReadAllBytesString = (p) => { return new byte[0]; };
            ShimFile.WriteAllBytesStringByteArray = (x1,x2) => { };
            ShimFile.WriteAllTextStringString = (x1, x2) => { };
            ShimFile.AppendTextString = (x1) =>
            {
                return new ShimStreamWriter();
            };
            ShimTextWriter.AllInstances.WriteLineString = (instance, p) => { };

            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimFilter.GetByFilterID_NoAccessCheckInt32 =
                (p) =>
                {
                    return new ECN_Framework_Entities.Communicator.Fakes.ShimFilter
                    {
                        WhereClauseGet = () => DummyText
                    };
                };

            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimEmailGroup.
                GetGroupEmailProfilesWithUDFInt32Int32StringStringString = (x1, x2, x3, x4, x5) =>
                {
                    var sampleTable = new DataTable();
                    sampleTable.Clear();
                    sampleTable.Columns.Add(DummyText);
                    var sampleRow = sampleTable.NewRow();
                    sampleRow[DummyText] = DummyText;
                    sampleTable.Rows.Add(sampleRow);

                    return sampleTable;
                };

            ShimWebRequest.CreateString = (p) =>
            {
                return new ShimFtpWebRequest
                {
                    GetRequestStream = () =>
                    {
                        return (Stream)new ShimFileStream
                        {
                            WriteByteArrayInt32Int32 = (x1, x2, x3) => { }
                        };
                    },
                    GetResponse = () =>
                    {
                        return new ShimFtpWebResponse
                        {
                            StatusDescriptionGet = () => DummyText
                        };
                    }
                };
            };
        }
    }
}
