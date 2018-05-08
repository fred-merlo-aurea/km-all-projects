using System;
using System.IO.Fakes;
using System.Xml;
using System.Data;
using System.Net.Fakes;
using System.IO;
using KMPlatform.BusinessLogic.Fakes;
using Shouldly;
using ecn.automatedreporting;
using NUnit.Framework;
using TestFtpExportsReport = ecn.automatedreporting.Reports.FtpExportsReport;

namespace ECN.AutomatedReporting.Tests
{
    public partial class ProgramTest
    {
        private const string MethodGetFtpExports = "GetFtpExports";
        private const string XMLKeyExports = "Exports";
        private const string XMLKeysends = "sends";
        private const string XMLKeyopens = "opens";
        private const string XMLKeyunopened = "unopened";
        private const string XMLKeyclicks = "clicks";
        private const string XMLKeynoclicks = "no-clicks";
        private const string XMLKeybounces = "bounces";
        private const string XMLKeyunsubscribes = "unsubscribes";
        private const string XMLKeyresends = "resends";
        private const string XMLKeysuppressed = "suppressed";
        private const string XMLKeyforwards = "forwards";
        private const string ValueTexttrue = "true";
        private const string ValueTextCharStar = "*";
        private const string ExportSettingStandAlone = "profileplusstandalone";
        private const string ExportSettingAllUdfs = "profileplusalludfs";
        private const string SuccessfulResultStringGetFtpExports =
            "Your scheduled export of sends has been moved to Dummy";
        private const string ConnectionFailureResultStringGetFtpExports =
            "There was a problem moving your scheduled export";
        private const string NoRecordStringGetFtpExports =
            "had no records.";
        private const string FailureResultStringGetFtpExports = 
            "Your scheduled export of blast details failed";

        [Test]
        public void GetFtpExports_FileFormatDefault_NoException()
        {
            // Arrange
            InitilizeGetFtpExportsTests(String.Empty, String.Empty, String.Empty);
            var constructedBody = String.Empty;
            var report = new TestFtpExportsReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetFtpExports);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetFtpExports_FileFormatTxt_AllSubscribeTypeCode_NoException()
        {
            // Arrange
            InitilizeGetFtpExportsTests(FileFormatTxt, ValueTextCharStar, String.Empty);
            var constructedBody = String.Empty;
            var report = new TestFtpExportsReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetFtpExports);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetFtpExports_FileFormatCsv_ExportSettingStandAlone_NoException()
        {
            // Arrange
            InitilizeGetFtpExportsTests(FileFormatCsv, String.Empty, ExportSettingStandAlone);
            var constructedBody = String.Empty;
            var report = new TestFtpExportsReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetFtpExports);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetFtpExports_FileFormatXls_ExportSettingAllUdfs_NoException()
        {
            // Arrange
            InitilizeGetFtpExportsTests(FileFormatXls, String.Empty, ExportSettingAllUdfs);
            var constructedBody = String.Empty;
            var report = new TestFtpExportsReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetFtpExports);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetFtpExports_FileFormatXml_NoException()
        {
            // Arrange
            InitilizeGetFtpExportsTests(FileFormatXml, String.Empty, String.Empty);
            var constructedBody = String.Empty;
            var report = new TestFtpExportsReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeTrue();
            report.Body.ShouldContain(SuccessfulResultStringGetFtpExports);
            _anyException.ShouldBeFalse();
        }

        [Test]
        public void GetFtpExports_FileFormatDefault_FTPException()
        {
            // Arrange
            InitilizeGetFtpExportsTests(String.Empty, String.Empty, String.Empty);
            ShimWebRequest.CreateString = (p) =>
            {
                throw new Exception(ExceptionMessageFTP);
            };
            var constructedBody = String.Empty;
            var report = new TestFtpExportsReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(ConnectionFailureResultStringGetFtpExports);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionMessageFTP);
        }

        [Test]
        public void GetFtpExports_FileFormatDefault_NoData()
        {
            // Arrange
            InitilizeGetFtpExportsTests(String.Empty, String.Empty, String.Empty);
            ECN_Framework_BusinessLayer.Activity.View.Fakes.ShimBlastActivity.
                DownloadBlastReportDetails_NoAccessCheckInt32Int32BooleanStringStringStringStringStringStringStringBoolean =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11) => new DataTable();
            var constructedBody = String.Empty;
            var report = new TestFtpExportsReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(NoRecordStringGetFtpExports);
        }

        [Test]
        public void GetFtpExports_FileFormatDefault_NoReportParameterException()
        {
            // Arrange
            InitilizeGetFtpExportsTests(String.Empty, String.Empty, String.Empty);
            _reportSchedule = new ECN_Framework_Entities.Communicator.Fakes.ShimReportSchedule
            {
                ReportParametersGet = () => throw new Exception(ExceptionNoReportScheduleParameters)
            };
            var constructedBody = String.Empty;
            var report = new TestFtpExportsReport(null, constructedBody, _reportSchedule, DateTime.Today);

            // Act	
            var returnResult = report.Execute();

            // Assert
            returnResult.ShouldNotBeNull();
            returnResult.success.ShouldBeFalse();
            report.Body.ShouldContain(FailureResultStringGetFtpExports);
            _anyException.ShouldBeTrue();
            _exceptionMessage.ShouldBe(ExceptionNoReportScheduleParameters);
        }

        private void InitilizeGetFtpExportsTests(string fileFormat, string exportSubscribeTypeCode , string exportSettings)
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
                    var sampleElement = sampleXMLDoc.CreateElement(XMLKeyExportSettings);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(exportSettings));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyExportFormat);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(fileFormat));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyExportSubscribeTypeCode);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(exportSubscribeTypeCode));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFtpUrl);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFtpUsername);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyFtpPassword);
                    sampleElement.AppendChild(sampleXMLDoc.CreateTextNode(DummyText));
                    reportScheduleElement.AppendChild(sampleElement);

                    sampleElement = sampleXMLDoc.CreateElement(XMLKeyExports);
                    var sampleElement2 = sampleXMLDoc.CreateElement(XMLKeysends);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(XMLKeyopens);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(XMLKeyunopened);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(XMLKeyclicks);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(XMLKeynoclicks);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(XMLKeybounces);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(XMLKeyunsubscribes);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(XMLKeyresends);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(XMLKeysuppressed);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(XMLKeyforwards);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
                    reportScheduleElement.AppendChild(sampleElement);
                    sampleElement2 = sampleXMLDoc.CreateElement(DummyText);
                    sampleElement2.AppendChild(sampleXMLDoc.CreateTextNode(ValueTexttrue));
                    sampleElement.AppendChild(sampleElement2);
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

            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean =
                (x1, x2) => new ECN_Framework_Entities.Communicator.BlastRegular();
            ECN_Framework_Entities.Communicator.Fakes.ShimBlast.AllInstances.SendTimeGet = (p) => new DateTime(2018, 1, 1);
            ECN_Framework_Entities.Communicator.Fakes.ShimBlast.AllInstances.CustomerIDGet = (p) => ValueInt1;

            ECN_Framework_BusinessLayer.Activity.View.Fakes.ShimBlastActivity.
                DownloadBlastReportDetails_NoAccessCheckInt32Int32BooleanStringStringStringStringStringStringStringBoolean =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11) =>
                {
                    var sampleTable = new DataTable();
                    sampleTable.Clear();
                    sampleTable.Columns.Add(DummyText);
                    var sampleRow = sampleTable.NewRow();
                    sampleRow[DummyText] = DummyText;
                    sampleTable.Rows.Add(sampleRow);

                    return sampleTable;
                };

            ShimUser.GetByUserIDInt32Boolean = (x1, x2) => null;
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
