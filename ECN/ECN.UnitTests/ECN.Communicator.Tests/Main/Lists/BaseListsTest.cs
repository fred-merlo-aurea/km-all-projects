using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.IO.Fakes;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.communicator.main.lists.reports;
using ecn.communicator.main.lists.reports.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts.Fakes;
using ECN_Framework_Entities.Communicator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Lists
{
	/// <summary>
	/// Base class for ecn.communicator.main.Lists Test
	/// </summary>
	/// <typeparam name="T">Type of class under test</typeparam>
	public abstract class BaseListsTest<T> : PageHelper where T : IDisposable, new()
	{
		protected const string SampleGroupId1 = "1";
		protected const string SampleCustomerId1 = "1";
		protected const string SampleChannelId1 = "1";
		protected const string SampleFilterId1 = "1";
		protected const string SubscribeTypeAll = "*";
		protected const string SearchTypeLike = "like";
		protected const string SearchTypeEquals = "equals";
		protected const string SearchTypeStarts = "starts";
		protected const string SearchTypeEnds = "ends";

		protected const string DownloadTypeXls = ".xls";
		protected const string DownloadTypeCsv = ".csv";
		protected const string DownloadTypeXml = ".xml";
		protected const string DownloadTypeTxt = ".txt";
		protected const string SampleEmail = "sample@email.com";
		protected const string Emails = "emails";
		protected const string XlsContentType = "application/vnd.ms-excel";
		protected const string XlsFileText = "DataColumn1\tDataColumn2\tData11\tData12\tData21\tData22\t";
		protected const string TxtContentType = "text/csv";
		protected const string TxtFileText = "\"DataColumn1\",\"DataColumn2\",\"Data11\",\"Data12\",\"Data21\",\"Data22\",";
		protected const string XmlContentType = "text/xml";
		protected const string CsvContentType = "text/csv";

		protected string contentType = string.Empty;
		protected string responseHeader = string.Empty;
		protected string responseText = string.Empty;
		protected string fileText = string.Empty;

		protected T testObject;
		protected PrivateObject privateObject;

		[SetUp]
		public void TestInitialize()
		{
			QueryString = new NameValueCollection();
			testObject = new T();
			privateObject = new PrivateObject(testObject);

			contentType = string.Empty;
			responseHeader = string.Empty;
			responseText = string.Empty;
			fileText = string.Empty;
		}

		[TearDown]
		public void TestCleanup()
		{
			QueryString = null;
			testObject.Dispose();
			privateObject = null;
		}

		protected virtual void InitializeFields(string chId, string custId, string grpId, string fileType, string subType,
			string srchType, string srchEm, string filterId, string profFilter)
		{
			ShimGroupExportUdfSetting.AllInstances.selectedGet = obj => profFilter;
			var subscribeFilterType = new DropDownList();
			subscribeFilterType.Items.Add(subType);
			subscribeFilterType.Items[0].Selected = true;
			var emailFilter = new TextBox {Text = srchEm};
			var searchType = new DropDownList();
			searchType.Items.Add(srchType);
			searchType.Text = srchType;
			var chID_Hidden = new HtmlInputHidden {Value = chId};
			var custID_Hidden = new HtmlInputHidden {Value = custId};
			var grpID_Hidden = new HtmlInputHidden {Value = grpId};
			var filteredDownloadType = new DropDownList();
			filteredDownloadType.Items.Add(fileType);
			filteredDownloadType.Items[0].Selected = true;
			var ddlFilteredDownloadOnly = new GroupExportUdfSetting();

			privateObject.SetField("SubscribeTypeFilter", subscribeFilterType);
			privateObject.SetField("EmailFilter", emailFilter);
			privateObject.SetField("SearchEmailLike", searchType);
			privateObject.SetField("chID_Hidden", chID_Hidden);
			privateObject.SetField("custID_Hidden", custID_Hidden);
			privateObject.SetField("grpID_Hidden", grpID_Hidden);
			privateObject.SetField("FilteredDownloadType", filteredDownloadType);
			privateObject.SetField("ddlFilteredDownloadOnly", ddlFilteredDownloadOnly);
		}

		protected void InitializeTests(string chId, string custId, string grpId, string fileType, string subType,
			string srchType, string srchEm, string filterId, string profFilter)
		{
			InitializeFields(chId, custId, grpId, fileType, subType, srchType, srchEm, filterId, profFilter);
			ShimFilter.GetByFilterIDInt32User = (fileterId, user) => new Filter();
			ShimECNSession.CurrentSession = () =>
			{
				var session = (ECNSession) new ShimECNSession();
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

			ECN_Framework_Entities.Communicator.Fakes.ShimFilter.AllInstances.WhereClauseGet =
				filter => "where dummy = 'dummy'";

			var tempDataTable = new DataTable
			{
				Columns = {"DataColumn1", "DataColumn2"},
				Rows = {{"Data11", "Data12"}, {"Data21", "Data22"}}
			};

			ShimEmailGroup.GetGroupEmailProfilesWithUDFInt32Int32StringStringString =
				(p1, p2, p3, p4, p5) => tempDataTable;

			ShimEmailGroup.GetGroupEmailProfilesWithUDFInt32Int32StringDateTimeDateTimeBooleanStringString =
				(p1, p2, p3, p4, p5, p6, p7, p8) => tempDataTable;

			ShimConfigurationManager.AppSettingsGet =
				() => new NameValueCollection
				{
					{"Images_VirtualPath", string.Empty}
				};

			ShimHttpResponse.AllInstances.ContentTypeSetString = (p1, p2) => contentType += p2;
			ShimHttpResponse.AllInstances.AddHeaderStringString = (p1, p2, p3) => responseHeader += p2 + " " + p3;
			ShimHttpResponse.AllInstances.WriteFileString = (p1, p2) => responseText += p2;
            ShimTextWriter.AllInstances.WriteLineString = (p1, p2) => fileText += p2;
			ShimPage.AllInstances.ResponseGet = p => new ShimHttpResponse();
			ShimHttpServerUtilityBase.AllInstances.MapPathString = (p1, p2) => string.Empty;
			ShimDirectory.ExistsString = p => false;
			ShimDirectory.CreateDirectoryString = p => null;
			ShimFile.ExistsString = p => true;
			ShimFile.DeleteString = p => { };
			ShimFile.AppendTextString = p => new ShimStreamWriter();
		}
	}
}