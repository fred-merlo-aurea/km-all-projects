using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.listsmanager;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    /// UT for <see cref="emaildataeditor"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class EmailDataEditorTest : BaseListsTest<emaildataeditor>
    {
        [Test]
        public void Page_Load_Success([Values(true, false)]bool hasAccess)
        {
            // Arrange
            InitilizeTestObjects();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, p1, p2, p3) => hasAccess;
            QueryString.Add("GroupID", "1");
            QueryString.Add("EmailID", "1");
            QueryString.Add("DFSID", "1");
            ShimEmail.GetByEmailIDInt32User = (p1, p2) => new Email { };
            var EmailsGrid = privateObject.GetFieldOrProperty("EmailsGrid") as DataGrid;
            EmailsGrid.Columns.Add(new BoundColumn());
            EmailsGrid.Columns.Add(new BoundColumn());
            EmailsGrid.Columns.Add(new BoundColumn());
            ShimEmailDataValues.GetStandaloneUDFDataValuesInt32Int32User = (p1, p2, p3) => new DataTable { Columns = { "1", "2", "3" }, Rows = {{ "1", "2", "3" } } };
            ShimDataFieldSets.GetByGroupIDInt32 = (p) => new List<DataFieldSets> { new DataFieldSets { } };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            ShimEmailDataValues.GetTransUDFDataValuesInt32Int32StringInt32User = (p1, p2, p3, p4, p5) => new DataTable { Columns = { "EmailID", "LastModifiedDate" }, Rows = { { "1", "2018-1-1" } } };

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            EmailsGrid.Columns[2].Visible.ShouldBe(hasAccess);
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer { CommunicatorLevel = "1"};
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