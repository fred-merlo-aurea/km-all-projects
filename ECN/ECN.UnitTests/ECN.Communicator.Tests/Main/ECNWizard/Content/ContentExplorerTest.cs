using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Content;
using ecn.communicator.main.ECNWizard.Content.Fakes;
using ecn.controls;
using ECN.Communicator.Tests.Main.Salesforce.SF_Pages;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Content
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.ECNWizard.Content.contentExplorer"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    class ContentExplorerTest : PageHelper
    {
        private const string ErrorMsg = "Test Error Message";
        private const string PlaceHolderError = "phError";
        private const string LabelErrorMessage = "lblErrorMessage";
        private const string SetEcnError = "setECNError";
        private PrivateObject _testObject;
        private UserControl _testPage;
        private IDisposable _shimObject;
        private StateBag _stateBag;

    [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            InitializeFakes();
            FakeSession();
            CreateTestObjects();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void SetEcnError_SetEcnExcpetion_SetsLblErrorMessageText()
        {
            // Arrange
            var testObject = new contentExplorer();
            var privateObject = new PrivateObject(testObject);
            using (var placeHolder = new PlaceHolder { Visible = false })
            {
                using (var label = new Label())
                {
                    var ecnError = new ECNError(Enums.Entity.APILogging, Enums.Method.Validate, ErrorMsg);
                    var ecnExcpetion = new ECNException(new List<ECNError>
                    {
                        ecnError
                    });
                    privateObject.SetFieldOrProperty(PlaceHolderError, placeHolder);
                    privateObject.SetFieldOrProperty(LabelErrorMessage, label);

                    // Act
                    privateObject.Invoke(SetEcnError, ecnExcpetion);

                    // Assert
                    testObject.ShouldSatisfyAllConditions(
                        () => ((PlaceHolder)privateObject.GetFieldOrProperty(PlaceHolderError)).Visible.ShouldBeTrue(),
                        () => ((Label)privateObject.GetFieldOrProperty(LabelErrorMessage)).Text.
                            ShouldBe($"<br/>{ecnError.Entity}: {ecnError.ErrorMessage}"));
                }
            }
        }

        [Test]
        public void Page_Load_Success()
        {
            //Arrange 
            var contentUserID = _testObject.GetFieldOrProperty("ContentUserID") as DropDownList;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
            contentUserID.Items.Count.ShouldBe(2);
        }

        [Test]
        public void Page_Load_PostBack_NoUserError()
        {
            //Arrange 
            ShimPage.AllInstances.IsPostBackGet = (p) => true;
            var contentUserID = _testObject.GetFieldOrProperty("ContentUserID") as DropDownList;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
            contentUserID.Items.Count.ShouldBe(2);
        }

        [Test]
        public void LoadContentGrid_Success([Values(true, false)]bool isAllFolders)
        {
            // Arrange
            var ContentUserID = _testObject.GetFieldOrProperty("ContentUserID") as DropDownList;
            ContentUserID.Items.Add("1");
            ContentUserID.SelectedValue = "1";
            var cbxAllFoldersContent = _testObject.GetFieldOrProperty("cbxAllFoldersContent") as CheckBox;
            cbxAllFoldersContent.Checked = isAllFolders;
            var ds = new DataSet();
            ds.Tables.Add(new DataTable { Columns = { "ContentTitle", "CreatedDate", "UpdatedDate", "ContentTypeCode", "DESC" }, Rows = { { 1, 1 } } });
            ShimContent.GetByContentTitleStringNullableOfInt32NullableOfInt32NullableOfInt32NullableOfDateTimeNullableOfDateTimeUserInt32Int32StringStringString =
                (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => ds;
            var lblTotalNumberOfPagesGroup = _testObject.GetFieldOrProperty("lblTotalNumberOfPagesGroup") as Label;
            var lblTotalRecords = _testObject.GetFieldOrProperty("lblTotalRecords") as Label;
            lblTotalRecords.Text = "1";
            _stateBag.Add("contentGridPageIndex", "1");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("loadContentGrid", new object[] { 1, 1 }));
            lblTotalNumberOfPagesGroup.Text.ShouldBe("1");
        }

        [Test]
        public void Page_PreRender_Success([Values(true, false)]bool isAllFolders)
        {
            // Arrange
            _testObject.SetFieldOrProperty("ShowArchiveFilter", isAllFolders);
            _testObject.SetFieldOrProperty("IsSelect", !isAllFolders);
            var ddlArchiveFilter = _testObject.GetFieldOrProperty("ddlArchiveFilter") as DropDownList;
            ddlArchiveFilter.Items.Add("active");
            var cbxAllFoldersContent = _testObject.GetFieldOrProperty("cbxAllFoldersContent") as CheckBox;
            cbxAllFoldersContent.Checked = isAllFolders;
            KM.Platform.Fakes.ShimUser.IsAdministratorUser = (p) => isAllFolders;
            ShimcontentExplorer.AllInstances.loadContentGridInt32Int32 = (p1, p2, p3) => { };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_PreRender", new object[] { null, null }));
            if (isAllFolders)
            {
                ddlArchiveFilter.Visible.ShouldBeTrue();
            }
            else
            {
                ddlArchiveFilter.Visible.ShouldBeFalse();
            }
        }

        [Test]
        public void ContentGrid_Command_DeleteLayout_Success()
        {
            // Arrange
            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("DeleteContent", "1"));
            var isGridLoaded = false;
            ShimContent.DeleteInt32User = (p1, p2) =>
            {
                isGridLoaded = true;
                throw new ECNException(new List<ECNError> { });
            };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ContentGrid_Command", new object[] { null, eventArgs }));
            isGridLoaded.ShouldBeTrue();
        }

        [Test]
        public void ContentGrid_Command_SelectLayout_Success()
        {
            // Arrange
            var eventArgs = new GridViewCommandEventArgs(null, new CommandEventArgs("SelecContent", "1"));
            ShimContent.GetByLayoutID_NoAccessCheckInt32Boolean = (p1, p2) => new List<ECN_Framework_Entities.Communicator.Content> { new ECN_Framework_Entities.Communicator.Content { } };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ContentGrid_Command", new object[] { null, eventArgs }));
        }

        private void CreateTestObjects()
        {
            _testPage = new contentExplorer();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage);
            _stateBag = new StateBag();
            var contentFolderID = _testObject.GetFieldOrProperty("ContentFolderID");
            InitializeAllControls(contentFolderID);
            var gridView = _testObject.GetFieldOrProperty("ContentGrid") as ecnGridView;
            InitilizeContentGrid(gridView);
        }

        private void InitilizeContentGrid(ecnGridView gridView)
        {
            gridView.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new Label { ID = "lblFolderName" } }
            });
            gridView.Columns.Add(new BoundField { DataField = "ContentTitle" });
            gridView.Columns.Add(new BoundField { DataField = "CreatedDate" });
            gridView.Columns.Add(new BoundField { DataField = "UpdatedDate" });
            gridView.Columns.Add(new BoundField { DataField = "ContentTypeCode" });
            gridView.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new CheckBox { ID = "chkIsValidated" } }
            });
            gridView.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new System.Web.UI.WebControls.Image { } }
            });
            gridView.Columns.Add(new HyperLinkField { });
            gridView.Columns.Add(new HyperLinkField { });
            gridView.Columns.Add(new HyperLinkField { });
            gridView.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new System.Web.UI.WebControls.Image { } }
            });
            gridView.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new LinkButton { ID = "DeleteContentBtn" } }
            });
            gridView.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new LinkButton { ID = "SelectContentBtn" } }
            });
            gridView.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new CheckBox { ID = "chkIsArchived" } }
            });
        }

        private void InitializeFakes()
        {
            ShimControl.AllInstances.ViewStateGet = (p) => _stateBag;
            ShimUser.GetByCustomerIDInt32 = (p) => new List<User> { new User { UserID = 0, IsActive = true} };
            ShimUserGroup.GetInt32 = (p) => new List<UserGroup> { };
            ShimFolder.GetByTypeInt32StringUser = (p1, p2, p3) => new List<Folder> { new Folder { } };
        }

        private void FakeSession()
        {
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User { CustomerID = 1};
                session.CurrentBaseChannel = new BaseChannel { };
                session.CurrentCustomer = new Customer { CommunicatorLevel = "1" };
                return session;
            };
        }
    }
}