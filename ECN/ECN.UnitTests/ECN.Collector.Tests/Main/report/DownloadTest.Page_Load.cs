using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.Fakes;
using ECN_Framework_BusinessLayer.Collector.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using NUnit.Framework;
using Shouldly;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Collector.Tests.main.report
{
    public partial class DownloadTest
    {
        private const string SampleGroupName = "SampleGroupName";
        private const string SampleCustomerName = "SampleCustomerName";
        private const string DownloadLinkControl = "DownloadLink";
        private const string ImagesVirtualPath = "Images_VirtualPath";
        private StringBuilder _responseText;

        [Test]
        [TestCase("like","*")]
        [TestCase("equals", "*")]
        [TestCase("ends", "*")]
        [TestCase("starts", "*")]
        [TestCase("contains","*")]
        [TestCase("like","?")]
        public void Page_Load_WhenDownLoadTypeIsXLS_CreatesLinkForFileDownload(string searchType,string subscriberType)
        {
            // Arrange
            QueryString.Add(SearchTypeKey, searchType);
            QueryString.Add(SubTypeKey, subscriberType);
            SetFakesForPageLoadMethod();
            var custId = _testEntity.Request.QueryString[CustIDKey].ToString();
            var groupId = _testEntity.Request.QueryString[GroupIDKey].ToString();
            var filePath = $"{TestBasePath}\\customers\\{custId}\\downloads\\";
            var fileName = $"{custId}_{groupId}_emails{FileTypes[0]}";
            
            // Act
            _privateObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);
            var downloadLink = Get<HyperLink>(_privateObject, DownloadLinkControl);
            
            // Assert
            downloadLink.ShouldNotBeNull();
            downloadLink.ShouldSatisfyAllConditions(
                () => downloadLink.Visible.ShouldBeTrue(),
                () => _responseText.ShouldNotBeNull(),
                () => _responseText.ToString().ShouldContain("<script language='javascript' type='text/javascript'>"),
                () => _responseText.ToString().ShouldContain("initDownLoader();"),
                () => _responseText.ToString().ShouldContain("completed();"),
                () => downloadLink.NavigateUrl.ShouldNotBeNullOrWhiteSpace(),
                () => downloadLink.NavigateUrl.Replace("/","\\").ShouldContain($"{filePath}{fileName}"),
                () => Directory.GetFiles(filePath).ShouldNotBeEmpty(),
                () => Directory.GetFiles(filePath).ShouldContain(x => x.Contains(fileName)));
        }

        [Test]
        [TestCase("like", "*")]
        public void Page_Load_WhenDownLoadTypeIsXML_CreatesLinkForFileDownload(string searchType, string subscriberType)
        {
            // Arrange
            QueryString.Add(SearchTypeKey, searchType);
            QueryString.Add(SubTypeKey, subscriberType);
            QueryString.Remove(FileTypeKey);
            QueryString.Add(FileTypeKey, FileTypes[1]);
            SetFakesForPageLoadMethod();
            var custId = _testEntity.Request.QueryString[CustIDKey].ToString();
            var groupId = _testEntity.Request.QueryString[GroupIDKey].ToString();
            var filePath = $"{TestBasePath}\\customers\\{custId}\\downloads\\";
            var fileName = $"{custId}_{groupId}_emails{FileTypes[1]}";

            // Act
            _privateObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);
            var downloadLink = Get<HyperLink>(_privateObject, DownloadLinkControl);

            // Assert
            downloadLink.ShouldNotBeNull();
            downloadLink.ShouldSatisfyAllConditions(
                () => downloadLink.Visible.ShouldBeTrue(),
                () => _responseText.ShouldNotBeNull(),
                () => _responseText.ToString().ShouldContain("<script language='javascript' type='text/javascript'>"),
                () => _responseText.ToString().ShouldContain("initDownLoader();"),
                () => _responseText.ToString().ShouldContain("completed();"),
                () => downloadLink.NavigateUrl.ShouldNotBeNullOrWhiteSpace(),
                () => downloadLink.NavigateUrl.Replace("/", "\\").ShouldContain($"{filePath}{fileName}"),
                () => Directory.GetFiles(filePath).ShouldNotBeEmpty(),
                () => Directory.GetFiles(filePath).ShouldContain(x => x.Contains(fileName)));
        }

        private void SetFakesForPageLoadMethod()
        {
            _responseText = new StringBuilder();
            ShimHttpResponse.AllInstances.WriteString = (r, text) =>
            {
                _responseText.AppendLine(text);
            };

            var settings = new NameValueCollection();
            settings.Add(ImagesVirtualPath, TestContext.CurrentContext.TestDirectory);
            ShimConfigurationManager.AppSettingsGet = () => settings;

            ShimHttpServerUtility.AllInstances.MapPathString = (s, path) => path.Replace("/", "\\");

            ShimFilter.GetByFilterIDInt32User = (fid, user) => new CommunicatorEntities.Filter { WhereClause = "1 = 1" };

            ShimParticipant.ExportParticipantsInt32Int32 = (fid, gid) => GetDataTable();
        }

        private DataTable GetDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("FilterID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            dt.Columns.Add("GroupName", typeof(string));
            dt.Columns.Add("CustomerID", typeof(int));
            dt.Columns.Add("CustomerName", typeof(string));
            var row = dt.NewRow();
            row[0] = OneString;
            row[1] = OneString;
            row[2] = SampleGroupName;
            row[3] = OneString;
            row[4] = SampleCustomerName;
            dt.Rows.Add(row);
            return dt;
        }
    }
}
