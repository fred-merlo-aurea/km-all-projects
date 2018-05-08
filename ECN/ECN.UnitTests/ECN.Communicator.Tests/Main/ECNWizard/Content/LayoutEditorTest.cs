﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Content;
using ecn.controls;
using ECN.Communicator.Tests.Main.Salesforce.SF_Pages;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Content
{

    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.ECNWizard.Content.layoutEditor"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    class LayoutEditorTest : PageHelper
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
            // Arrange, Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
        }

        [Test]
        public void Page_Load_WithLayout_Success()
        {
            // Arrange
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (p) => new Template { SlotsTotal = 0 };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (p1, p2) => new Layout { CustomerID = 1, LayoutID = 1, FolderID = 0, TemplateID = 1, Slot1 = new ECN_Framework_Entities.Communicator.Content { } };
            QueryString.Add("LayoutID", "1");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Page_Load", new object[] { null, null }));
        }

        [Test]
        public void Initialize_Success()
        {
            // Arrange
            ShimTemplate.GetByBaseChannelIDInt32User = (p1, p2) => new List<Template> { new Template { Category = "1" } };

            //Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Initialize", new object[] { }));
        }

        [Test]
        public void Initialize_WithLayout_Success()
        {
            // Arrange
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (p) => new Template { SlotsTotal = 0 };
            ShimTemplate.GetByBaseChannelIDInt32User = (p1, p2) => new List<Template> { new Template { Category = "1" } };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (p1, p2) => new Layout { CustomerID = 1, LayoutID = 1, FolderID = 0, TemplateID = 1, Slot1 = new ECN_Framework_Entities.Communicator.Content { } };
            QueryString.Add("LayoutID", "1");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Initialize", new object[] { }));
        }

        [Test]
        public void ValidateSlotContents_Success()
        {
            // Arrange
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (p) => new Template { SlotsTotal = 9 };
            for (int i = 1; i <= 9; i++)
            {
                var hdnSlot = _testObject.GetFieldOrProperty("hdnSlot" + i) as HiddenField;
                hdnSlot.Value = "1";
            }

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("ValidateSlotContents", new object[] { }));
        }

        [Test]
        public void ValidateSlotContents_Exception()
        {
            // Arrange
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (p) => new Template { SlotsTotal = 9 };
            for (int i = 1; i <= 9; i++)
            {
                var hdnSlot = _testObject.GetFieldOrProperty("hdnSlot" + i) as HiddenField;
                hdnSlot.Value = "0";
            }

            // Act, Assert
            var result = Should.Throw<Exception>(() => _testObject.Invoke("ValidateSlotContents", new object[] { }));
            result.InnerException.ShouldNotBeNull();
            var ecnException = result.InnerException as ECNException;
            ecnException.ErrorList.Count.ShouldBe(1);
            ecnException.ErrorList[0].ErrorMessage.ShouldBe("Please enter content for slot(s) 1,2,3,4,5,6,7,8,9");
        }

        [Test]
        public void UpdateContentDetail_Success()
        {
            // Arrange
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (p1, p2) => 
                new ECN_Framework_Entities.Communicator.Content { ContentTitle = "Test Title", ContentID = p1 };
            for (int i = 1; i <= 9; i++)
            {
                var selectedSlot = _testObject.GetFieldOrProperty("SelectedSlot") as HiddenField;
                selectedSlot.Value = i.ToString();

            // Act, Assert
                Should.NotThrow(() => _testObject.Invoke("updateContentDetail", new object[] { i }));
                var lblSlot = _testObject.GetFieldOrProperty("lblSlot" + i) as Label;
                var hdnSlot = _testObject.GetFieldOrProperty("hdnSlot" + i) as HiddenField;
                lblSlot.Text.ShouldBe("Test Title");
                hdnSlot.Value.ShouldBe(i.ToString());
            }
        }

        [Test]
        public void SaveLayout_Success()
        {
            Layout resultLayout = null;
            ShimLayout.SaveLayoutUser = (p1, p2) => { resultLayout = p1; };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (p) => new Template { SlotsTotal = 9 };
            for (int i = 1; i <= 9; i++)
            {
                var hdnSlot = _testObject.GetFieldOrProperty("hdnSlot" + i) as HiddenField;
                hdnSlot.Value = i.ToString();
            }
            QueryString.Add("LayoutID", "1");

            // Act, Assert
            var layoutID = Should.NotThrow(() => _testObject.Invoke("SaveLayout", new object[] { }));
            layoutID.ShouldBe(1);
            resultLayout.ShouldNotBeNull();
            for (int i = 1; i <= 9; i++)
            {
                ReflectionHelper.GetPropertyValue<int>(resultLayout, "ContentSlot" + i).ShouldBe(i);
            }
        }

        [Test]
        public void SaveLayout_CreateNew_Success()
        {
            Layout resultLayout = null;
            ShimLayout.SaveLayoutUser = (p1, p2) => { resultLayout = p1; };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (p) => new Template { SlotsTotal = 9 };
            for (int i = 1; i <= 9; i++)
            {
                var hdnSlot = _testObject.GetFieldOrProperty("hdnSlot" + i) as HiddenField;
                hdnSlot.Value = i.ToString();
            }

            // Act, Assert
            var layoutID = Should.NotThrow(() => _testObject.Invoke("SaveLayout", new object[] { }));
            layoutID.ShouldBe(-1);
            resultLayout.ShouldNotBeNull();
            for (int i = 1; i <= 9; i++)
            {
                ReflectionHelper.GetPropertyValue<int>(resultLayout, "ContentSlot" + i).ShouldBe(i);
            }
        }

        [Test]
        public void UpdateSlotDetail_Success()
        {
            // Arrange, Act, Assert
            for (int i = 1; i <= 9; i++)
            {
                Should.NotThrow(() => _testObject.Invoke("updateSlotDetail", new object[] { "lnkbtnCreateContent" + i }));
                var selecedSlot = _testObject.GetFieldOrProperty("SelectedSlot") as HiddenField;
                selecedSlot.Value.ShouldBe(i.ToString());
                Should.NotThrow(() => _testObject.Invoke("updateSlotDetail", new object[] { "lnkbtnExistingContent" + i }));
                selecedSlot = _testObject.GetFieldOrProperty("SelectedSlot") as HiddenField;
                selecedSlot.Value.ShouldBe(i.ToString());
            }            
        }

        [Test]
        public void PopulatePreview_Success()
        {
            // Arrange
            _testObject.SetFieldOrProperty("TemplateID", "1");
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (p) => new Template {};
            for (int i = 1; i <= 9; i++)
            {
                var hdnSlot = _testObject.GetFieldOrProperty("hdnSlot" + i) as HiddenField;
                hdnSlot.Value = i.ToString();
            }
            ShimContent.GetContent_NoAccessCheckNullableOfInt32EnumsContentTypeCodeBooleanNullableOfInt32NullableOfInt32NullableOfInt32 =
                (p1, p2, p3, p4, p5, p6) => p1.ToString();

            // Act, Assert
             var body = Should.NotThrow(() => _testObject.Invoke("PopulatePreview", new object[] {  }));
            body.ShouldBe("<table  cellpadding=0 cellspacing=0  width='100%'><tr><td></td></tr></table>");            
        }

        [Test]
        public void LoadFormData_Success()
        {
            // Arrange 
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (p) => new Template { SlotsTotal = 9 };
            var layout = new Layout { LayoutID = 1, CustomerID = 1, FolderID = 0, MessageTypeID = 0, DisplayAddress = "address" };
            for (int i = 1; i <= 9; i++)
            {
                ReflectionHelper.SetProperty(layout, "ContentSlot" + i, i);
                ReflectionHelper.SetProperty(layout, "Slot" + i, new ECN_Framework_Entities.Communicator.Content { ContentTitle = "title" });
            }
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (p1, p2) => layout;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("LoadFormData", new object[] { 1 }));
            for (int i = 1; i <= 9; i++)
            {
                var hdnSlot = _testObject.GetFieldOrProperty("hdnSlot" + i) as HiddenField;
                hdnSlot.Value.ShouldBe(i == 1 ? "5" : "0");

                var hyperLink = _testObject.GetFieldOrProperty("HyperLink" + i) as HyperLink;
                hyperLink.NavigateUrl.ShouldBe("../../content/contentfilters.aspx?SlotNumber=" + i + "&LayoutID=1");
            }            
        }

        [Test]
        public void LoadFormData_Default_Success()
        {
            // Arrange 
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (p) => new Template { SlotsTotal = 1 };
            var layout = new Layout { LayoutID = 1, CustomerID = 1, FolderID = 0, MessageTypeID = 1, DisplayAddress = string.Empty };
            layout.ContentSlot1 = 1;
            layout.Slot1 = new ECN_Framework_Entities.Communicator.Content { ContentTitle = "title" };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (p1, p2) => layout;
            _testObject.SetFieldOrProperty("CompanyAddress", "CompanyAddress");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("LoadFormData", new object[] { 1 }));
            var DisplayAddress = _testObject.GetFieldOrProperty("DisplayAddress") as TextBox;
            DisplayAddress.Text.ShouldBe("CompanyAddress");
        }

        [Test]
        public void LoadFormData_Exception()
        {
            // Arrange 
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (p1, p2) => new Layout { CustomerID = 2 };

            // Act, Assert
            var result = Should.Throw<Exception>(() => _testObject.Invoke("LoadFormData", new object[] { 1 }));
            result.InnerException.ShouldNotBeNull();
            result.InnerException.ShouldBeOfType(typeof(SecurityException));
        }

        [Test]
        public void checkDynamicContent_NoProductFeature_Success()
        {
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => false;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("checkDynamicContent", new object[] { }));
            for (int i = 1; i <= 9; i++)
            {
                var hyperLink = _testObject.GetFieldOrProperty("HyperLink" + i) as HyperLink;
                hyperLink.NavigateUrl.ShouldBe("javascript:upgrade();");
            }
        }

        [Test]
        public void checkDynamicContent_NoLayout_Success()
        {
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (p1, p2) => null;

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("checkDynamicContent", new object[] { }));
            for (int i = 1; i <= 9; i++)
            {
                var hyperLink = _testObject.GetFieldOrProperty("HyperLink" + i) as HyperLink;
                hyperLink.Visible.ShouldBeFalse();
            }
        }

         private void CreateTestObjects()
        {
            _testPage = new layoutEditor();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage);
            QueryString = new NameValueCollection();
            var contentExplorer1 = _testObject.GetFieldOrProperty("contentExplorer1") as contentExplorer;
            InitializeAllControls(contentExplorer1);
            var templateBorder = _testObject.GetFieldOrProperty("TemplateBorder") as RadioButtonList;
            templateBorder.Items.Add("N");
            var folderID = _testObject.GetFieldOrProperty("folderID") as DropDownList;
            folderID.Items.Add("0");
            var ddlMessageType = _testObject.GetFieldOrProperty("ddlMessageType") as DropDownList;
            ddlMessageType.Items.Add("0");
            var contentExplorerObject = new PrivateObject(contentExplorer1);      
            var gridView = contentExplorerObject.GetFieldOrProperty("ContentGrid") as ecnGridView;
            InitilizeContentGrid(gridView);
            var templaterepeater = _testObject.GetFieldOrProperty("templaterepeater") as DataList;
            templaterepeater.ItemTemplate = new DLTemplateItem { controls =new List<Control> {
                new ImageButton { ID = "ImageButton1" },
                new TextBox { ID = "TemplateID", Text = "1" } } };
            templaterepeater.SelectedItemTemplate = new DLTemplateItem
            {
                controls = new List<Control> {
                new TextBox { ID = "TemplateID", Text = "1"  },
                new TextBox { ID = "SlotsTotal", Text = "1" } }
            };
            ShimTemplate.GetByStyleCode_NoAccessCheckInt32String = (p1, p2) => new List<Template> { new Template { } };
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
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (p1, p2) => new Layout { LayoutID = p1 };
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimFolder.GetByTypeInt32StringUser = (p1, p2, p3) => new List<Folder> { new Folder { } };
            ShimMessageType.GetByBaseChannelID_NoAccessCheckInt32 = (p) => new List<MessageType> { new MessageType { } };
            ShimCustomer.GetByCustomerIDInt32Boolean = (p1, p2) => new Customer { };
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
