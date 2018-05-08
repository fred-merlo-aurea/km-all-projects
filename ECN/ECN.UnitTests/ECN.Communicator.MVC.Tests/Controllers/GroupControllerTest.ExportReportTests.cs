using System;
using System.Data;
using System.Reflection;
using System.Web.Fakes;
using System.Web.Mvc.Fakes;
using ecn.communicator.mvc.Controllers;
using ecn.communicator.mvc.Infrastructure.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts.Fakes;
using KM.Platform.Fakes;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.MVC.Tests
{
    public partial class GroupControllerTest
    {
        private const string FileFormatXls = "xls";
        private const string FileFormatCsv = "csv";
        private const string FileFormatTxt = "txt";
        private const string SampleDate = "2018.1.1";
        private const string SystemAdminChannelCustomerResponse = "\r\n0,0,,";
        private const string ChannelAdminChannelCustomerResponse = "\r\n,0,,";
        private const string GroupMemberChannelCustomerResponse = "\r\n1,0,,";
        private const string DefaultUserChannelCustomerResponse = "\r\n0,1,,";

        [Test]
        public void ExportReport_FileFormatTxt_SystemAdmin()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportReportTests();
            ShimUser.IsSystemAdministratorUser = (user) => true;

            // Act
            controller.ExportReport(String.Empty, String.Empty, FileFormatTxt);

            // Assert
            _contentType.ShouldBe("text/csv");
            _responseHeader.ShouldContain("EmailSearch.txt");
            _fileText.ShouldContain(SystemAdminChannelCustomerResponse + SampleEmail);
        }

        [Test]
        public void ExportReport_FileFormatCsv_ChannelAdmin()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportReportTests();
            ShimUser.IsSystemAdministratorUser = (user) => false;
            ShimUser.IsChannelAdministratorUser = (user) => true;
            ShimHttpContext.CurrentGet = () => new ShimHttpContext();
            ShimHttpContext.AllInstances.ResponseGet = (p) => new ShimHttpResponse();
            ShimHttpResponse.AllInstances.ContentTypeSetString = (p1, p2) => _contentType += p2;
            ShimHttpResponse.AllInstances.AddHeaderStringString = (p1, p2, p3) => _responseHeader += p2 + " " + p3;
            ShimHttpResponse.AllInstances.WriteString = (p1, p2) => _fileText += p2;

            // Act
            controller.ExportReport(String.Empty, String.Empty, FileFormatCsv);

            // Assert
            _contentType.ShouldBe("text/csv");
            _responseHeader.ShouldContain("EmailSearch.csv");
            _fileText.ShouldContain(ChannelAdminChannelCustomerResponse + SampleEmail);
        }

        [Test]
        public void ExportReport_FileFormatXls_GroupMember()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportReportTests();
            ShimUser.IsSystemAdministratorUser = (user) => false;
            ShimUser.IsChannelAdministratorUser = (user) => false;
            KMPlatform.Entity.Fakes.ShimUser.AllInstances.CurrentSecurityGroupGet = (instance) =>
                new KMPlatform.Entity.Fakes.ShimSecurityGroup();
            KMPlatform.Entity.Fakes.ShimSecurityGroup.AllInstances.ClientGroupIDGet = (instance) => 1;
            InitilizeECNFakes();

            // Act
            controller.ExportReport(String.Empty, String.Empty, FileFormatXls);

            // Assert
            _contentType.ShouldBe("application/vnd.ms-excel");
            _responseHeader.ShouldContain("EmailSearch.xls");
            _fileText.ShouldBeEmpty();
        }

        [Test]
        public void ExportReport_FileFormatTxt_GroupMember()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportReportTests();
            ShimUser.IsSystemAdministratorUser = (user) => false;
            ShimUser.IsChannelAdministratorUser = (user) => false;
            KMPlatform.Entity.Fakes.ShimUser.AllInstances.CurrentSecurityGroupGet = (instance) =>
                new KMPlatform.Entity.Fakes.ShimSecurityGroup();
            KMPlatform.Entity.Fakes.ShimSecurityGroup.AllInstances.ClientGroupIDGet = (instance) => 1;
            InitilizeECNFakes();

            // Act
            controller.ExportReport(String.Empty, String.Empty, FileFormatTxt);

            // Assert
            _contentType.ShouldBe("text/csv");
            _responseHeader.ShouldContain("EmailSearch.txt");
            _fileText.ShouldContain(GroupMemberChannelCustomerResponse + SampleEmail);
        }

        [Test]
        public void ExportReport_FileFormatTxt_DefaultUser()
        {
            // Arrange
            var controller = new GroupController();
            InitilizeExportReportTests();
            ShimUser.IsSystemAdministratorUser = (user) => false;
            ShimUser.IsChannelAdministratorUser = (user) => false;
            KMPlatform.Entity.Fakes.ShimUser.AllInstances.CurrentSecurityGroupGet = (instance) =>
                new KMPlatform.Entity.Fakes.ShimSecurityGroup();
            KMPlatform.Entity.Fakes.ShimSecurityGroup.AllInstances.ClientGroupIDGet = (instance) => 0;
            InitilizeECNFakes();

            // Act
            controller.ExportReport(String.Empty, String.Empty, FileFormatTxt);

            // Assert
            _contentType.ShouldBe("text/csv");
            _responseHeader.ShouldContain("EmailSearch.txt");
            _fileText.ShouldContain(DefaultUserChannelCustomerResponse + SampleEmail);
        }

        private void InitilizeExportReportTests()
        {
            _contentType = String.Empty;
            _responseHeader = String.Empty;
            _responseText = String.Empty;
            _fileText = String.Empty;
            ShimEmail.EmailSearchStringStringUserStringStringInt32Int32StringString =
                (filterType, searchTerm, user, currentBaseChannel, customerID, currentPage, pageSize, sortColumn, sortDirection) =>
                new DataTable
                {
                    Columns = { "TotalCount", "BaseChannelName", "CustomerName", "GroupName", "EmailAddress",
                        "SubscribeTypeCode", "DateCreated", "DateModified" },
                    Rows = { { "1", currentBaseChannel, customerID, String.Empty, SampleEmail, String.Empty, SampleDate, SampleDate } }
                };

            ShimReport.AllInstances.LoadReportDefinitionStream = (x1, x2) => { };
            ShimLocalReport.AllInstances.SetParametersIEnumerableOfReportParameter = (x1, x2) => { };
            ShimReport.AllInstances.RenderStringStringStringOutStringOutStringOutStringArrayOutWarningArrayOut =
                (Report instance, string format, string deviceInfo, out string mimeType, out string encoding, 
                    out string fileNameExtension, out string[] streams, out Warning[] warnings) =>
                {
                    mimeType = String.Empty;
                    encoding = String.Empty;
                    fileNameExtension = String.Empty;
                    streams = new string[0];
                    warnings = new Warning[0];

                    return new Byte[0];
                };

            ShimConvenienceMethods.GetCurrentUser = () => new KMPlatform.Entity.User();
            ShimHttpResponseWrapper.AllInstances.ContentTypeSetString = (p1, p2) => _contentType += p2;
            ShimHttpResponseWrapper.AllInstances.AddHeaderStringString = (p1, p2, p3) => _responseHeader += p2 + " " + p3;
            ShimHttpResponseWrapper.AllInstances.WriteString = (p1, p2) => _fileText += p2;
            ShimHttpResponseWrapper.AllInstances.BinaryWriteByteArray = (p1, p2) => _fileText += System.Text.Encoding.UTF8.GetString(p2);
            ShimController.AllInstances.ResponseGet = (p) => new ShimHttpResponseWrapper();
            ShimController.AllInstances.ServerGet = (p) => new ShimHttpServerUtilityWrapper();
            ShimHttpServerUtilityWrapper.AllInstances.MapPathString = (p1, p2) => Assembly.GetExecutingAssembly().Location;
        }

        private void InitilizeECNFakes()
        {
            ShimECNSession.CurrentSession = () =>
            {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentBaseChannel = new ShimBaseChannel
                {
                    BaseChannelIDGet = () => 1
                };
                session.CurrentCustomer = new ShimCustomer
                {
                    CustomerIDGet = () => 1
                };
                return session;
            };
        }
    }
}
