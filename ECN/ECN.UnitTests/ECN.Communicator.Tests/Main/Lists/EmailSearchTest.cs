using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.lists.Fakes;
using ecn.communicator.main.lists;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Activity.Report.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using static KMPlatform.Enums;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PageCommunicator = ecn.communicator.MasterPages.Communicator;
using PlatformShimUser = KM.Platform.Fakes.ShimUser;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class EmailSearchTest : PageHelper
    {
        private const string MethodExportReportClick = "ExportReport_Click";
        private const string MethodGoToPageGroupChanged = "GoToPageGroup_TextChanged";
        private const string DllPath = "\\ECN_Framework_Common\\bin\\Release\\ECN_Framework_Common.dll";
        private const string SearchPath = "\\ECN.UnitTests\\";
        private const string Csv = "CSV";
        private const string Pdf = "pdf";
        private const string Xls = "xls";
        private const string Txt = "txt";
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string DummyDateString = "1/1/2018";
        private const string LayoutValueString = "1";
        private const string Unsubscribe = "U";
        private const string Subscribe = "S";
        private const string One = "1";
        private const int LayoutId = 1;
        private const int GroupId = 1;
        private const int CutomerDDValue = 0;
        private StateBag _viewState;
        private EmailSearch _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _context;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new EmailSearch { };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void GoToPageGroup_TextChanged_Success_EmailGridIsLoaded(bool isSystemAdmin, bool isChannelAdmin)
        {
            // Arrange 
            Initialize();
            CreateShims();
            SetExportType(Csv);
            PlatformShimUser.IsSystemAdministratorUser = (x) => isSystemAdmin;
            PlatformShimUser.IsChannelAdministratorUser = (x) => isChannelAdmin;
            var emailGridLoaded = false;
            ShimEmail.EmailSearchStringStringUserStringStringInt32Int32StringString = (a, b, c, d, e, f, g, h, i) =>
            {
                emailGridLoaded = true;
                return CreateEmailSearchTable();
            };

            // Act
            _privateTestObject.Invoke(MethodGoToPageGroupChanged, new TextBox { Text = One }, EventArgs.Empty);

            // Assert
            emailGridLoaded.ShouldBeTrue();
        }

        [TestCase(Pdf)]
        [TestCase(Xls)]
        [TestCase(Txt)]
        public void ExportReport_Click_WhenExportToOtherFormat_ReportIsExported(string exportType)
        {
            // Arrange 
            Initialize();
            CreateShims();
            SetExportType(exportType);
            var reportExported = false;
            ShimHttpResponse.AllInstances.BinaryWriteByteArray = (x, y) => { };
            ShimHttpResponse.AllInstances.End = (x) =>
            {
                reportExported = true;
            };

            // Act
            _privateTestObject.Invoke(MethodExportReportClick, null, EventArgs.Empty);

            // Assert
            reportExported.ShouldBeTrue();
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ExportReport_Click_WhenExportToCSV_ReportIsExportedToCSVFormat(bool isSystemAdmin, bool isChannelAdmin)
        {
            // Arrange 
            Initialize();
            CreateShims();
            SetExportType(Csv);
            var exportedToCSV = false;
            PlatformShimUser.IsSystemAdministratorUser = (x) => isSystemAdmin;
            PlatformShimUser.IsChannelAdministratorUser = (x) => isChannelAdmin;
            ShimReportViewerExport.ExportToCSVStringString = (x, y) =>
            {
                exportedToCSV = true;
            };

            // Act
            _privateTestObject.Invoke(MethodExportReportClick, null, EventArgs.Empty);

            // Assert
            exportedToCSV.ShouldBeTrue();
        }

        private void Initialize()
        {
            _viewState = new StateBag();
            _viewState.Add("gSortField", DummyString);
            _viewState.Add("gSortDirection", DummyString);
            _viewState.Add("gCurrentPage", One);
            _viewState.Add("gTotalPages", One);
            _viewState.Add("gCurrentPage", One);
            _viewState.Add("gPageSize", One);
            ReflectionHelper.SetField(_testEntity, "Addresses", new TextBox { Text = DummyString });
            ReflectionHelper.SetField(_testEntity, "FormatTypeCode", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = DummyString
                    }
                }
            });
        }

        private void SetSubscribeTypeCode(string code)
        {
            ReflectionHelper.SetField(_testEntity, "SubscribeTypeCode", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = code
                    }
                }
            });
        }

        private void SetExportType(string code)
        {
            ReflectionHelper.SetField(_testEntity, "ddlExportType", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = code
                    }
                }
            });
        }

        private DataTable CreateImportEmailsTable(string actionType)
        {
            var importEmailTable = new DataTable();
            importEmailTable.Columns.Add("Action");
            importEmailTable.Columns.Add("Counts");
            importEmailTable.Rows.Add(actionType, One);
            return importEmailTable;
        }

        private DataTable CreateEmailSearchTable()
        {
            var importEmailTable = new DataTable();
            importEmailTable.Columns.Add("TotalCount");
            importEmailTable.Columns.Add("DateCreated");
            importEmailTable.Columns.Add("DateModified");
            importEmailTable.Columns.Add("EmailAddress");
            importEmailTable.Columns.Add("SubscribeTypeCode");
            importEmailTable.Columns.Add("BaseChannelName");
            importEmailTable.Columns.Add("CustomerName");
            importEmailTable.Columns.Add("GroupName");
            importEmailTable.Columns.Add("Subscribe");
            importEmailTable.Rows.Add(One, DummyDateString, DummyDateString, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString);
            return importEmailTable;
        }

        private void CreateShims()
        {
            ShimEmailSearch.AllInstances.MasterGet = (mt) => new PageCommunicator();
            PlatformShimUser.IsChannelAdministratorUser = (x) => true;
            ShimControl.AllInstances.ViewStateGet = (x) => _viewState;
            ShimEmail.EmailSearchStringStringUserStringStringInt32Int32StringString = (a, b, c, d, e, f, g, h, i) => CreateEmailSearchTable();
            ShimHttpServerUtility.AllInstances.MapPathString = (x, y) => GetDllPath();
            ShimReportViewerExport.ExportToCSVStringString = (x, y) => { };
            var dummySecurityGroup = ReflectionHelper.CreateInstance(typeof(SecurityGroup));
            dummySecurityGroup.AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator;
            ShimSecurityGroup.AllInstances.SelectInt32BooleanBoolean = (a, b, c, d) => dummySecurityGroup;
            ShimBaseChannel.GetByPlatformClientGroupIDInt32 = (x) => ReflectionHelper.CreateInstance(typeof(BaseChannel));
            ShimLocalReport
                .AllInstances
                .RenderStringStringPageCountModeStringOutStringOutStringOutStringArrayOutWarningArrayOut = (
                LocalReport format,
                string renderers,
                string info,
                PageCountMode mode,
                out string type,
                out string encoding,
                out string extension,
                out string[] streams,
                out Warning[] warnings) =>
                {
                    type = DummyString;
                    encoding = string.Empty;
                    extension = DummyString;
                    streams = new string[0];
                    warnings = new Warning[0];

                    return new byte[1];
                };
        }

        private string GetDllPath()
        {
            var ecnPathIndex = Assembly.GetExecutingAssembly().Location.IndexOf(SearchPath);
            var ecnPath = Assembly.GetExecutingAssembly().Location.Substring(0, ecnPathIndex);
            var filePath = string.Format("{0}{1}", ecnPath, DllPath);
            return filePath;
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            var client = ReflectionHelper.CreateInstance(typeof(Client));
            var userSecurityGroup = ReflectionHelper.CreateInstance(typeof(SecurityGroup));
            var userClientSecurityGroupMapList = new List<UserClientSecurityGroupMap>
            {
                ReflectionHelper.CreateInstance(typeof(UserClientSecurityGroupMap))
            };
            var currentUser = new User()
            {
                UserID = 1,
                UserName = TestUser,
                IsActive = true,
                CurrentClient = client,
                UserClientSecurityGroupMaps = userClientSecurityGroupMapList,
                CurrentSecurityGroup = userSecurityGroup
            };
            shimSession.Instance.CurrentUser = currentUser;
            shimSession.Instance.CurrentCustomer = new Customer() { CustomerID = 1 };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}