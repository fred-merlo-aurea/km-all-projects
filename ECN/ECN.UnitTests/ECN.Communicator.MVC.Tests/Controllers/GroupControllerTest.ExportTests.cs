using System;
using System.Data;
using System.Web.Mvc.Fakes;
using System.IO.Fakes;
using System.Web.Fakes;
using System.Configuration.Fakes;
using NUnit.Framework;
using ecn.communicator.mvc.Controllers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ecn.communicator.mvc.Infrastructure.Fakes;
using Shouldly;

namespace ECN.Communicator.MVC.Tests
{
    public partial class GroupControllerTest
    {
        private const string SampleGroupId1 = "1";
        private const string SubcribeTypeAll = "*";
        private const string SearchTypeLike = "like";
        private const string SearchTypeEquals = "equals";
        private const string SearchTypeStarts = "starts";
        private const string SearchTypeEnds = "ends";
        private const string DownloadTypeXls = ".xls";
        private const string DownloadTypeCsv = ".csv";
        private const string DownloadTypeXml = ".xml";
        private const string DownloadTypeTxt = ".txt";
        private const string SampleEmail = "sample@email.com";

        private string _contentType = String.Empty;
        private string _responseHeader = String.Empty;
        private string _responseText = String.Empty;
        private string _fileText = String.Empty;

        [Test]
        public void Export_SubcribeTypeDefault_SearchTypeDefault_DownloadTypeDefault_NoEmail()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportTests();

            // Act
            controller.Export(SampleGroupId1, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty);

            // Assert
            _contentType.ShouldBeEmpty();
            _fileText.ShouldBeEmpty();
        }

        [Test]
        public void Export_SubcribeTypeDefault_SearchTypeDefault_DownloadTypeDefault()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportTests();

            // Act
            controller.Export(SampleGroupId1, String.Empty, SampleEmail, String.Empty, String.Empty, String.Empty);

            // Assert
            _contentType.ShouldBeEmpty();
            _fileText.ShouldBeEmpty();
        }

        [Test]
        public void Export_SubcribeTypeAll_SearchTypeLike_DownloadTypeXls()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportTests();

            // Act
            controller.Export(SampleGroupId1, SubcribeTypeAll, SampleEmail, SearchTypeLike, DownloadTypeXls, String.Empty);

            // Assert
            _contentType.ShouldBe("application/vnd.ms-excel");
            _fileText.ShouldBe("DataColumn1\tDataColumn2\tData11\tData12\tData21\tData22\t");
            _responseHeader.ShouldContain("emails.xls");
            _responseText.ShouldContain("emails.xls");
        }

        [Test]
        public void Export_SubcribeTypeDefault_SearchTypeEquals_DownloadTypeCsv()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportTests();

            // Act
            controller.Export(SampleGroupId1, String.Empty, SampleEmail, SearchTypeEquals, DownloadTypeCsv, String.Empty);

            // Assert
            _contentType.ShouldBe("text/csv");
            _fileText.ShouldBe("\"DataColumn1\",\"DataColumn2\",\"Data11\",\"Data12\",\"Data21\",\"Data22\",");
            _responseHeader.ShouldContain("emails.csv");
            _responseText.ShouldContain("emails.csv");
        }

        [Test]
        public void Export_SubcribeTypeDefault_SearchTypeStarts_DownloadTypeXml()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportTests();

            // Act
            controller.Export(SampleGroupId1, String.Empty, SampleEmail, SearchTypeStarts, DownloadTypeXml, String.Empty);

            // Assert
            _contentType.ShouldBe("text/xml");
            _fileText.ShouldBeEmpty();
            _responseHeader.ShouldContain("emails.xml");
            _responseText.ShouldContain("emails.xml");
        }

        [Test]
        public void Export_SubcribeTypeDefault_SearchTypeEnds_DownloadTypeTxt()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportTests();

            // Act
            controller.Export(SampleGroupId1, String.Empty, SampleEmail, SearchTypeEnds, DownloadTypeTxt, String.Empty);

            // Assert
            _contentType.ShouldBe("text/csv");
            _fileText.ShouldBe("DataColumn1\tDataColumn2\tData11\tData12\tData21\tData22\t");
            _responseHeader.ShouldContain("emails.txt");
            _responseText.ShouldContain("emails.txt");
        }

        private void InitilizeExportTests()
        {
            _contentType = String.Empty;
            _responseHeader = String.Empty;
            _responseText = String.Empty;
            _fileText = String.Empty;

            ShimConvenienceMethods.GetCurrentUser = () => new KMPlatform.Entity.User();
            ShimEmailGroup.GetGroupEmailProfilesWithUDFInt32Int32StringStringString =
                (p1, p2, p3, p4, p5) => new DataTable()
                {
                    Columns = { "DataColumn1", "DataColumn2" },
                    Rows = { { "Data11", "Data12" }, { "Data21", "Data22" } }
                };

            ShimConfigurationManager.AppSettingsGet =
                () => new System.Collections.Specialized.NameValueCollection
                {
                    { "Images_VirtualPath", String.Empty }
                };

            ShimHttpResponseWrapper.AllInstances.ContentTypeSetString = (p1, p2) => _contentType += p2;
            ShimHttpResponseWrapper.AllInstances.AddHeaderStringString = (p1, p2, p3) => _responseHeader += p2 + " " + p3;
            ShimHttpResponseWrapper.AllInstances.WriteFileString = (p1, p2) => _responseText += p2;
            ShimTextWriter.AllInstances.WriteLineString = (p1, p2) => _fileText += p2;
            ShimController.AllInstances.ResponseGet = (p) => new ShimHttpResponseWrapper();
            ShimController.AllInstances.ServerGet = (p) => new ShimHttpServerUtilityWrapper();
            ShimHttpServerUtilityBase.AllInstances.MapPathString = (p1, p2) => String.Empty;
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimFile.AppendTextString = (p) => new ShimStreamWriter();
        }
    }
}
