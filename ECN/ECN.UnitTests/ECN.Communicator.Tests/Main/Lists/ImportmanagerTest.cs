using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.includes;
using ecn.communicator.listsmanager;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    /// UT for <see cref="importmanager"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ImportManagerTest : BaseListsTest<importmanager>
    {
        [Test]
        public void Page_Load_DefaultUser_Success()
        {
            // Arrange
            InitilizeTestObjects();

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("../default.aspx");
        }

        [Test]
        public void Page_Load_HasAccess_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (p1, p2, p3, p4) => true;
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimHttpServerUtility.AllInstances.MapPathString = (x, y) => "testFile";
            ShimDirectory.GetFilesStringString = (p1, p2) => new string[] { "test.xml" };
            ShimFileInfo.AllInstances.LengthGet = (p) => 10000;
            ShimGroup.GetByGroupIDInt32User = (p1,p2) => new Group { FolderID = 1  };
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCode.GetAll = () => new List<ECN_Framework_Entities.Accounts.Code>
            {
                new ECN_Framework_Entities.Accounts.Code { CodeType = "FormatType" }
            };
            QueryString.Add("GroupID", "1"); 
            QueryString.Add("deletefile", "deletefile");
            var SubscribeTypeCode = privateObject.GetFieldOrProperty("SubscribeTypeCode") as DropDownList;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            SubscribeTypeCode.ShouldSatisfyAllConditions(
                () => SubscribeTypeCode.Items.Count.ShouldBe(2),
                () => SubscribeTypeCode.Items[0].Value.ShouldBe("S"),
                () => SubscribeTypeCode.Items[1].Value.ShouldBe("U"));
        }

        [Test]
        public void ImportFile_SelectedIndexChanged_Success([Values("txt", "csv", "xls", "xlsx", "none")]string fileExt)
        {
            // Arrange
            InitilizeTestObjects();
            var ImportFile = privateObject.GetFieldOrProperty("ImportFile") as DropDownList;
            var fileName = "test." + fileExt;
            if (fileExt != "none")
            {
                ImportFile.Items.Add(fileName);
                ImportFile.SelectedValue = fileName;
            }
            var phTXTDelimiter = privateObject.GetFieldOrProperty("phTXTDelimiter") as PlaceHolder;
            var CSVTXTPanel = privateObject.GetFieldOrProperty("CSVTXTPanel") as Panel;
            var excelPanel = privateObject.GetFieldOrProperty("ExcelPanel") as Panel;

            // Act
            privateObject.Invoke("ImportFile_SelectedIndexChanged", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => phTXTDelimiter.Visible.ShouldBe(fileExt == "txt"),
                () => CSVTXTPanel.Visible.ShouldBe(fileExt == "csv" || fileExt == "txt"),
                () => excelPanel.Visible.ShouldBe(fileExt == "xls" || fileExt == "xlsx"));
        }

        [Test]
        public void OnBubbleEvent_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var ctrlgroupsLookup1 = privateObject.GetFieldOrProperty("ctrlgroupsLookup1") as ecn.communicator.main.ECNWizard.Group.groupsLookup;
            var hfGroupSelectionMode = privateObject.GetFieldOrProperty("hfGroupSelectionMode") as HiddenField;
            var lblSelectGroupName = privateObject.GetFieldOrProperty("lblSelectGroupName") as Label;
            hfGroupSelectionMode.Value = "SelectGroup"; 
            ShimGroup.GetByGroupIDInt32User = (p1, p2) => new Group { GroupName = "testName"};

            // Act
            var result = (bool)privateObject.Invoke("OnBubbleEvent", new object[] { "GroupSelected", null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => result.ShouldBeTrue(),
                () => ctrlgroupsLookup1.Visible.ShouldBeFalse(),
                () => lblSelectGroupName.Text.ShouldBe("testName"));
        }

        [Test]
        public void ImportIt_Success([Values("txt", "csv", "xls", "xlsx")]string fileExt)
        {
            // Arrange
            InitilizeTestObjects();
            var ImportFile = privateObject.GetFieldOrProperty("ImportFile") as DropDownList;
            var fileName = "test." + fileExt;
            ImportFile.Items.Add(fileName);
            ImportFile.SelectedValue = fileName;
            var format = fileExt == "txt" ? "O" : fileExt == "csv" ? "C" : "X"; 
            var FormatTypeCode = privateObject.GetFieldOrProperty("FormatTypeCode") as DropDownList;
            FormatTypeCode.Items.Add("fmt");
            FormatTypeCode.SelectedValue = "fmt";
            var hfSelectGroupID = privateObject.GetFieldOrProperty("hfSelectGroupID") as HiddenField;
            hfSelectGroupID.Value = "1";
            var SubscribeTypeCode = privateObject.GetFieldOrProperty("SubscribeTypeCode") as DropDownList;
            SubscribeTypeCode.Items.Add("test");
            SubscribeTypeCode.SelectedValue = "test";
            var HandleDuplicates = privateObject.GetFieldOrProperty("HandleDuplicates") as RadioButtonList;
            HandleDuplicates.Items.Add("test");
            HandleDuplicates.SelectedValue = "test";
            var drpDelimiter = privateObject.GetFieldOrProperty("drpDelimiter") as DropDownList;
            drpDelimiter.Items.Add("test");
            drpDelimiter.SelectedValue = "test";

            // Act
            privateObject.Invoke("ImportIt", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("importDatafromFile.aspx?file="+fileName+"&ftc=fmt&stc=test&gid=1&dupes=test&ft="+ format + "&line=&sheet=&dl=test");
        }

        [Test]
        public void ImportIt_ExtensionException()
        {
            // Arrange
            InitilizeTestObjects();
            var ImportFile = privateObject.GetFieldOrProperty("ImportFile") as DropDownList;
            ImportFile.Items.Add(string.Empty);
            ImportFile.SelectedValue = string.Empty;
            var errorlabel = privateObject.GetFieldOrProperty("errorlabel") as Label;

            // Act
            privateObject.Invoke("ImportIt", new object[] { null, null });

            // Assert
            errorlabel.Text.ShouldBe("Error: Please select a File");
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
            var uploadbox = privateObject.GetFieldOrProperty("uploadbox") as uploader;
            InitializeAllControls(uploadbox);
            QueryString = new System.Collections.Specialized.NameValueCollection { };
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer();
                session.CurrentBaseChannel = new BaseChannel();
                return session;
            };
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            var sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            ShimUserControl.AllInstances.SessionGet = (p) => sessionState;
        }        
	}
}