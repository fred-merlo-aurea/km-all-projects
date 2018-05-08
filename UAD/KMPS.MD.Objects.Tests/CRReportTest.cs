using System;
using System.Collections.Generic;
using System.IO.Fakes;
using System.Web.Fakes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Objects.Tests
{
    [TestFixture]
    public class CRReportTest
    {
        private const string FileName = "c:\\MyFileName";
        private const string AttachmentHeaderValue = "attachment; filename=c:\\MyFileName";
        private const string TempFileName = "c:\\test\\temppath\\tempfile.xx";
        private const string TempFileXls = "c:\\test\\temppath\\tempfile.xls";
        private const string TempFilePdf = "c:\\test\\temppath\\tempfile.pdf";
        private const string TempFileDoc = "c:\\test\\temppath\\tempfile.doc";
        private const string ContentDisposition = "content-disposition";
        private IDisposable _context;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            CreateCommonShims();
        }

        private static void CreateCommonShims()
        {
            var httpContext = new ShimHttpContext().Instance;
            var httpResponse = new ShimHttpResponse().Instance;
            var httpApplication = new ShimHttpApplication().Instance;
           
            ShimCRReport.ExportReportReportDocument = document => { };
            ShimHttpContext.CurrentGet = () => httpContext;
            ShimHttpContext.AllInstances.ResponseGet = _ => httpResponse;
            ShimHttpContext.AllInstances.ApplicationInstanceGet = _ => httpApplication;
            ShimPath.GetTempFileName = () => TempFileName;
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public void Export_FormatHtml_NothingWritten()
        {
            // Arrange
            ShimHttpResponse.AllInstances.WriteFileString = (_, __) => throw new ArgumentException();
            
            // Act, Assert
            CRReport.Export(null, CRExportEnum.HTML, string.Empty);
        }  
        
        [Test]
        [TestCase(CRExportEnum.XLS, ExportFormatType.Excel, TempFileXls, "application/x-msexcel")]
        [TestCase(CRExportEnum.PDF, ExportFormatType.PortableDocFormat, TempFilePdf, "application/pdf")]
        [TestCase(CRExportEnum.DOC, ExportFormatType.RichText, TempFileDoc, "application/msword")]
        public void Export_FormatXls_XlsWritten(
            CRExportEnum exportType,
            ExportFormatType exportFormatType,
            string tempFileName,
            string contentType)
        {
            // Arrange
            ReportDocument actualReportDocument = null;
            var actualFileNameWritten = string.Empty;
            var actualFileNameDeleted = string.Empty;
            var actualContentType = string.Empty;
            var actualFlushCalled = 0;
            var actualCompleteRequestCalled = 0;
            var responseHeader = new Dictionary<string, string>();
            var exportOptions = new ExportOptions();

            ShimCRReport.ReportExportOptionsReportDocument = document => exportOptions;
            ShimHttpResponse.AllInstances.WriteFileString = (_, fileName) => { actualFileNameWritten = fileName; };
            ShimHttpResponse.AllInstances.Flush = response => actualFlushCalled++;
            ShimHttpApplication.AllInstances.CompleteRequest = application => actualCompleteRequestCalled++; 
            ShimFile.DeleteString = fileName => { actualFileNameDeleted = fileName; };
            ShimHttpResponse.AllInstances.ContentTypeSetString = (_, newContentType) => actualContentType = newContentType;
            ShimHttpResponse.AllInstances.AddHeaderStringString = (httpResponse, headerName, headerValue) =>
            {
                responseHeader[headerName] = headerValue;
            };

            // Act
            CRReport.Export(null, exportType, FileName);

            // Assert
            actualReportDocument.ShouldSatisfyAllConditions(
                () => responseHeader[ContentDisposition].ShouldBe(AttachmentHeaderValue),
                () => actualContentType.ShouldBe(contentType),
                () => actualFlushCalled.ShouldBe(1),
                () => actualCompleteRequestCalled.ShouldBe(1),
                () => actualFileNameWritten.ShouldBe(tempFileName),
                () => actualFileNameDeleted.ShouldBe(tempFileName),
                () => exportOptions.ExportDestinationType.ShouldBe(ExportDestinationType.DiskFile),
                () => exportOptions.ExportFormatType.ShouldBe(exportFormatType));
        }
    }
}
