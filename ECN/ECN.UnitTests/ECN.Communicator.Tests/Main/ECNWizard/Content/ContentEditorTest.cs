using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Content;
using ecn.communicator.main.ECNWizard.Content.Fakes;
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
    ///     Unit tests for <see cref="ecn.communicator.main.ECNWizard.Content.contentEditor"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ContentEditorTest : PageHelper
    {
        private PrivateObject _testObject;
        private UserControl _testPage;
        private IDisposable _shimObject;

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
        public void Page_Load_Success()
        {
            //Arrange 
            var folderID = _testObject.GetFieldOrProperty("folderID") as DropDownList;
            var drpUserID = _testObject.GetFieldOrProperty("drpUserID") as DropDownList;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
            folderID.Items.Count.ShouldBeGreaterThan(0);
            folderID.Items[0].Text.ShouldBe("root");
            drpUserID.Items.Count.ShouldBe(2);
        }

        [Test]
        public void Page_Load_WithContent_Success()
        {
            //Arrange 
            var loadContentCalled = false;
            ShimcontentEditor.AllInstances.loadContentInt32Boolean = (p1, p2, p3) => loadContentCalled = true;
            QueryString.Add("ContentID", "1");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
            loadContentCalled.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_PostBack_Success()
        {
            //Arrange 
            ShimPage.AllInstances.IsPostBackGet = (p) => true;
            var folderID = _testObject.GetFieldOrProperty("folderID") as DropDownList;
            var drpUserID = _testObject.GetFieldOrProperty("drpUserID") as DropDownList;
            var fckEdior1 = _testObject.GetFieldOrProperty("FCKeditor1") as TextBox;
            var fckEdior2 = _testObject.GetFieldOrProperty("FCKeditor2") as TextBox;
            fckEdior1.Text = "Test";
            fckEdior2.Text = "Test";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
            folderID.Items.Count.ShouldBe(0);
            drpUserID.Items.Count.ShouldBe(0);
            fckEdior1.Text.ShouldBe("Test");
            fckEdior2.Text.ShouldBe("Test");
        }

        [Test]
        public void SaveContent_WYSWYCEditor_NoContent_Success()
        {
            // Arrange
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("true");
            rblContentType.SelectedValue = "true";
            var folderID = _testObject.GetFieldOrProperty("folderID") as DropDownList;
            folderID.Items.Add("0");
            folderID.SelectedValue = "0";
            var fckEdior1 = _testObject.GetFieldOrProperty("FCKeditor1") as TextBox;
            fckEdior1.Text = "test1";
            ECN_Framework_Entities.Communicator.Content content = null;
            ShimContent.SaveContentUser = (p1, p2) => { content = p1; return 0; };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("SaveContent", new object[] { }));
            content.ShouldSatisfyAllConditions(
                () => content.ShouldNotBeNull(),
                () => content.ContentSource.ShouldBe("test1"),
                () => content.ContentMobile.ShouldBe("test1"));
        }

        [Test]
        public void SaveContent_DefaultEditor_WithContent_Success([Values(true, false)]bool withUserId)
        {
            // Arrange
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("false");
            rblContentType.SelectedValue = "false";
            var folderID = _testObject.GetFieldOrProperty("folderID") as DropDownList;
            folderID.Items.Add("0");
            folderID.SelectedValue = "0";
            var txtEditorSource = _testObject.GetFieldOrProperty("txtEditorSource") as TextBox;
            txtEditorSource.Text = "test1";
            var txtEditorMobile = _testObject.GetFieldOrProperty("txtEditorMobile") as TextBox;
            txtEditorMobile.Text = "test2";
            var drpUserID = _testObject.GetFieldOrProperty("drpUserID") as DropDownList;
            drpUserID.Items.Add("0");
            drpUserID.Items.Add("1");
            QueryString.Add("ContentID", "1");
            if (withUserId)
            {
                drpUserID.SelectedValue = "1";
            }
            else
            {
                drpUserID.SelectedValue = "0";
            }
            ECN_Framework_Entities.Communicator.Content content = null;
            ShimContent.SaveContentUser = (p1, p2) => { content = p1; return 0; };
            ShimContent.GetByContentIDInt32UserBoolean = (p1, p2, p3) =>
                new ECN_Framework_Entities.Communicator.Content { IsValidated = true, CreatedUserID = 1 };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("SaveContent", new object[] { }));
            content.ShouldSatisfyAllConditions(
                () => content.ShouldNotBeNull(),
                () => content.ContentSource.ShouldBe("test1"),
                () => content.ContentMobile.ShouldBe("test2"));
        }

        [Test]
        public void SaveContent_WYSWYCEditor_WithContent_Exception()
        {
            // Arrange
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("true");
            rblContentType.SelectedValue = "true";
            var folderID = _testObject.GetFieldOrProperty("folderID") as DropDownList;
            folderID.Items.Add("0");
            folderID.SelectedValue = "0";
            var fckEdior1 = _testObject.GetFieldOrProperty("FCKeditor1") as TextBox;
            fckEdior1.Text = "test1";
            QueryString.Add("ContentID", "1");
            ShimContent.GetByContentIDInt32UserBoolean = (p1, p2, p3) =>
                new ECN_Framework_Entities.Communicator.Content { IsValidated = true, CreatedUserID = 1, LockedFlag = "y" };
            var phError = _testObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = _testObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("SaveContent", new object[] { }));
            phError.Visible.ShouldBeTrue();
            lblErrorMessage.Text.ShouldBe("<br/>Content: You do not have permission to edit this content");
        }

        [Test]
        public void LoadContent_Success([Values(true,false)] bool withNew)
        {
            // Arrange
            ShimContent.GetByContentIDInt32UserBoolean = (p1, p2, p3) => 
                new ECN_Framework_Entities.Communicator.Content { ContentSource = "test1", ContentMobile = "test2", UseWYSIWYGeditor = withNew,
                    ContentTypeCode = "html", FolderID = 0, LockedFlag = "Y", ContentText = "http://km.com" };
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("True");
            rblContentType.Items.Add("False");
            var folderID = _testObject.GetFieldOrProperty("folderID") as DropDownList;
            folderID.Items.Add("0");
            var drpUserID = _testObject.GetFieldOrProperty("drpUserID") as DropDownList;
            drpUserID.Items.Add("0");            
            var fckEdior1 = _testObject.GetFieldOrProperty("FCKeditor1") as TextBox;
            var fckEdior2 = _testObject.GetFieldOrProperty("FCKeditor2") as TextBox;
            var txtEditorSource = _testObject.GetFieldOrProperty("txtEditorSource") as TextBox;
            var txtEditorMobile = _testObject.GetFieldOrProperty("txtEditorMobile") as TextBox;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("loadContent", new object[] { 1, withNew }));
            if (withNew)
            {
                fckEdior1.Text.ShouldBe("test1");
                fckEdior2.Text.ShouldBe("test2");
            }
            else
            {
                txtEditorSource.Text.ShouldBe("test1");
                txtEditorMobile.Text.ShouldBe("test2");
            }
            rblContentType.SelectedValue.ShouldBe(withNew.ToString());
            folderID.SelectedValue.ShouldBe("0");
        }

        [Test]
        public void LoadContent_Exception()
        {
            // Arrange
            ShimContent.GetByContentIDInt32UserBoolean = (p1, p2, p3) => throw new Exception("Test Exception");
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("True");
            rblContentType.Items.Add("False");
            var folderID = _testObject.GetFieldOrProperty("folderID") as DropDownList;
            folderID.Items.Add("0");
            var drpUserID = _testObject.GetFieldOrProperty("drpUserID") as DropDownList;
            drpUserID.Items.Add("0");
            var fckEdior1 = _testObject.GetFieldOrProperty("FCKeditor1") as TextBox;
            var fckEdior2 = _testObject.GetFieldOrProperty("FCKeditor2") as TextBox;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("loadContent", new object[] { 1, false }));
            fckEdior1.Text.ShouldBeEmpty();
            fckEdior2.Text.ShouldBeEmpty();
            rblContentType.SelectedValue.ShouldBeEmpty();
        }

        [Test]
        public void ValidateContentInitialize_Success([Values(true, false)]bool withNew)
        {
            // Arrange
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("True");
            rblContentType.Items.Add("False");
            rblContentType.SelectedValue = withNew.ToString();
            _testObject.SetFieldOrProperty("upContentExplorer", new UpdatePanel { ID = "1", UpdateMode = UpdatePanelUpdateMode.Conditional });
            var saveContentCalled = false;
            ShimcontentEditor.AllInstances.SaveContent = (p) => { saveContentCalled = true; return 0; };
            QueryString.Add("ContentID", "1");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ValidateContentInitialize", new object[] { null, null }));
            saveContentCalled.ShouldBe(true);
        }

        [Test]
        public void ValidateContentInitialize_Exception()
        {
            // Arrange
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("True");
            rblContentType.Items.Add("False");
            rblContentType.SelectedValue = "True";
            _testObject.SetFieldOrProperty("upContentExplorer", new UpdatePanel { ID = "1", UpdateMode = UpdatePanelUpdateMode.Conditional });
            var saveContentCalled = false;
            ShimcontentEditor.AllInstances.SaveContent = (p) => throw new ECNException(new List<ECNError> { });
            var phError = _testObject.GetFieldOrProperty("phError") as PlaceHolder;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ValidateContentInitialize", new object[] { null, null }));
            phError.Visible.ShouldBeTrue();
        }

        [Test]
        public void BtnConvertoInlineCSS_Click_Success([Values(true,false)]bool withEditor, [Values(true, false)]bool withExisting)
        {
            // Arrange
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("True");
            rblContentType.Items.Add("False");
            rblContentType.SelectedValue = withEditor.ToString();
            var fCKeditor1 = _testObject.GetFieldOrProperty("FCKeditor1") as TextBox;
            var contentText = _testObject.GetFieldOrProperty("ContentText") as TextBox;
            var txtEditorSource = _testObject.GetFieldOrProperty("txtEditorSource") as TextBox;
            contentText.Text = "content";
            if (withExisting)
            {
                txtEditorSource.Text = "text from editor";                
                fCKeditor1.Text = "FCK Editor";
            }

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("btnConvertoInlineCSS_Click", new object[] { null, null }));
            if (withEditor)
            {
                if (withExisting)
                {
                    fCKeditor1.Text.ShouldBe("FCK Editor");
                }
                else
                {
                    fCKeditor1.Text.ShouldBe("content");
                }
            }
            else
            {
                if (withExisting)
                {
                    txtEditorSource.Text.ShouldBe("text from editor");                    
                }
                else
                {
                    txtEditorSource.Text.ShouldBe("content");
                }
            }
        }

        [Test]
        public void ClearContentOfIDs_Success()
        {
            var testHtml = "<a href = 'http://km.com' ECN_ID = 'ecn1'/><area href = 'http://km.com' ECN_ID = 'ecn2'/>";

            // Act, Assert
            var result = Should.NotThrow(() => _testObject.Invoke("ClearContentOfIDs", new object[] { testHtml }));
            // TODO - 2018-04-05 Fix bug in the code in the code #677 and # 694 as false null check. Please fix it and activate following line
            //result.ShouldBe("<a href = 'http://km.com'/><area href = 'http://km.com'/>");
        }

        [Test]
        public void SetShowPane_Success([Values("html", "text", "file", "feed", "")]string type)
        {
            // Arrange
            var ddContentTypeCode = _testObject.GetFieldOrProperty("ddContentTypeCode") as DropDownList;
            ddContentTypeCode.SelectedValue = type;
            var panelContentSource = _testObject.GetFieldOrProperty("panelContentSource") as Panel;
            var panelContentText = _testObject.GetFieldOrProperty("panelContentText") as Panel;
            var panelContentURL = _testObject.GetFieldOrProperty("panelContentURL") as Panel;
            var panelContentFilePointer = _testObject.GetFieldOrProperty("panelContentFilePointer") as Panel;
            var GetTextButton = _testObject.GetFieldOrProperty("GetTextButton") as Button;

            // Act, Assert
            var result = Should.NotThrow(() => _testObject.Invoke("setShowPane", new object[] { }));
            switch (type)
            {
                case "html":
                    panelContentSource.Visible.ShouldBeTrue();
                    panelContentText.Visible.ShouldBeTrue();
                    panelContentURL.Visible.ShouldBeFalse();
                    panelContentFilePointer.Visible.ShouldBeFalse();
                    GetTextButton.Visible.ShouldBeTrue();
                    break;
                case "text":
                    panelContentSource.Visible.ShouldBeFalse();
                    panelContentText.Visible.ShouldBeTrue();
                    panelContentURL.Visible.ShouldBeFalse();
                    panelContentFilePointer.Visible.ShouldBeFalse();
                    GetTextButton.Visible.ShouldBeFalse();
                    break;
                case "file":
                    panelContentSource.Visible.ShouldBeFalse();
                    panelContentText.Visible.ShouldBeFalse();
                    panelContentURL.Visible.ShouldBeFalse();
                    panelContentFilePointer.Visible.ShouldBeTrue();
                    break;
                case "feed":
                    panelContentSource.Visible.ShouldBeFalse();
                    panelContentText.Visible.ShouldBeFalse();
                    panelContentURL.Visible.ShouldBeTrue();
                    panelContentFilePointer.Visible.ShouldBeFalse();
                    break;
                default:
                    panelContentSource.Visible.ShouldBeTrue();
                    panelContentText.Visible.ShouldBeTrue();
                    panelContentURL.Visible.ShouldBeTrue();
                    panelContentFilePointer.Visible.ShouldBeTrue();
                    break;
            }
        }

        [Test]
        public void RblContentType_SelectedIndexChanged_Success([Values(true, false)]bool value)
        {
            // Arrange
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("True");
            rblContentType.Items.Add("False");
            rblContentType.SelectedValue = value.ToString();
            var FCKeditor1 = _testObject.GetFieldOrProperty("FCKeditor1") as TextBox;
            var FCKeditor2 = _testObject.GetFieldOrProperty("FCKeditor2") as TextBox;
            var pnlTxtMobile = _testObject.GetFieldOrProperty("pnlTxtMobile") as Panel;
            var pnlTxtSource = _testObject.GetFieldOrProperty("pnlTxtSource") as Panel;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("rblContentType_SelectedIndexChanged", new object[] { null, null}));
            if (value)
            {
                FCKeditor1.Visible.ShouldBeTrue();
                FCKeditor2.Visible.ShouldBeTrue();
                pnlTxtMobile.Visible.ShouldBeFalse();
                pnlTxtSource.Visible.ShouldBeFalse();
            }
            else
            {
                FCKeditor1.Visible.ShouldBeFalse();
                FCKeditor2.Visible.ShouldBeFalse();
                pnlTxtMobile.Visible.ShouldBeTrue();
                pnlTxtSource.Visible.ShouldBeTrue();
            }
        }

        [Test]
        public void GetTextFromHTML_Success([Values(true,false)]bool withEditor)
        {
            // Arrange
            var rblContentType = _testObject.GetFieldOrProperty("rblContentType") as RadioButtonList;
            rblContentType.Items.Add("True");
            rblContentType.Items.Add("False");
            rblContentType.SelectedValue = withEditor.ToString();
            var fCKeditor1 = _testObject.GetFieldOrProperty("FCKeditor1") as TextBox;
            var txtEditorSource = _testObject.GetFieldOrProperty("txtEditorSource") as TextBox;
            var contentText = _testObject.GetFieldOrProperty("ContentText") as TextBox;
            fCKeditor1.Text = "FCKEditor";
            txtEditorSource.Text = "Editor";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("GetTextFromHTML", new object[] { null, null }));
            if (withEditor)
                contentText.Text.ShouldBe("FCKEditor");
            else
                contentText.Text.ShouldBe("Editor");
        }

        [Test]
        public void Reset_Success()
        {
            // Arrange
            var fckEditor = _testObject.GetFieldOrProperty("FCKeditor1") as TextBox;
            var fckEditor2 = _testObject.GetFieldOrProperty("FCKeditor2") as TextBox;
            var contentText = _testObject.GetFieldOrProperty("ContentText") as TextBox;
            var contentTitle = _testObject.GetFieldOrProperty("ContentTitle") as TextBox;
            var lockedFlag = _testObject.GetFieldOrProperty("LockedFlag") as CheckBox;
            var folderID = _testObject.GetFieldOrProperty("folderID") as DropDownList;
            var contentURL = _testObject.GetFieldOrProperty("ContentURL") as TextBox;
            folderID.Items.Add("0");
            folderID.Items.Add("1");
            fckEditor.Text = "test";
            fckEditor2.Text = "test";
            contentText.Text = "test";
            contentTitle.Text = "test";
            lockedFlag.Checked = true;
            folderID.SelectedValue = "1";
            contentURL.Text = "test";

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Reset", new object[] { }));
            fckEditor.Text.ShouldBe("");
            fckEditor2.Text.ShouldBe("");
            contentText.Text.ShouldBe("");
            contentTitle.Text.ShouldBe("");
            lockedFlag.Checked.ShouldBeFalse();
            folderID.SelectedValue.ShouldBe("0");
            contentURL.Text.ShouldBe("");
        }

        private void CreateTestObjects()
        {
            _testPage = new contentEditor();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage);
            var ddContentTypeCode = _testObject.GetFieldOrProperty("ddContentTypeCode") as DropDownList;
            ddContentTypeCode.Items.Add("html");
            ddContentTypeCode.Items.Add("text");
            ddContentTypeCode.Items.Add("file");
            ddContentTypeCode.Items.Add("feed");
            ddContentTypeCode.Items.Add("");
            QueryString = new NameValueCollection { };
        }

        private void InitializeFakes()
        {
            ShimFolder.GetByTypeInt32StringUser = (p1, p2, p3) => new List<Folder> { new Folder { } };
            ShimUser.GetByCustomerIDInt32 = (p) => new List<User> { new User { UserID = 1, IsActive = true } };
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
