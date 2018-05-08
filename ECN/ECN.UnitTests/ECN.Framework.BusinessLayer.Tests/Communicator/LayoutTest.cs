using System;
using System.Collections.Generic;
using System.Data;
using ECN_Framework_BusinessLayer.Accounts.Interfaces;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using ECN_Framework_Entities.Accounts;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public class LayoutTest
    {
        private const int CustomerId = 1;
        private const int BaseChannelId = 1;
        private const int LayoutId = 1;
        private const int Slot = 2;
        private const int TemplateId = 1;
        private const int FolderId = 1;
        private const int MessageTypeId = 1;

        private Mock<ILayoutManager> _layoutManager;
        private Mock<IUserManager> _user;
        private Mock<IAccessCheckManager> _accessCheck;
        private Mock<IContentManager> _content;
        private Mock<IFolderManager> _folder;
        private Mock<ITemplateManager> _template;
        private Mock<IMessageTypeManager> _messageType;
        private Mock<IConversionLinksManager> _conversionLinks;

        [SetUp]
        public void SetUp()
        {
            _layoutManager = new Mock<ILayoutManager>();
            _accessCheck = new Mock<IAccessCheckManager>();

            _user = new Mock<IUserManager>();
            _user
                .Setup(x => x.HasAccess(It.IsAny<User>(), It.IsAny<Enums.Services>(), It.IsAny<Enums.ServiceFeatures>(), It.IsAny<Enums.Access>()))
                .Returns(true);

            _content = new Mock<IContentManager>();
            _content
                .Setup(x => x.GetByContentID(It.IsAny<int>(), It.IsAny<User>(), true))
                .Returns(new EntitiesCommunicator.Content());

            _folder = new Mock<IFolderManager>();
            _folder
                .Setup(x => x.GetByFolderID(It.IsAny<int>(), It.IsAny<User>()))
                .Returns(new EntitiesCommunicator.Folder());

            _template = new Mock<ITemplateManager>();
            _template
                .Setup(x => x.GetByTemplateID(It.IsAny<int>(), It.IsAny<User>()))
                .Returns(new EntitiesCommunicator.Template());

            _messageType = new Mock<IMessageTypeManager>();
            _messageType.Setup(x => x.GetByMessageTypeID(It.IsAny<int>(), It.IsAny<User>()));

            _conversionLinks = new Mock<IConversionLinksManager>();
            _conversionLinks
                .Setup(x => x.GetByLayoutID(It.IsAny<int>(), It.IsAny<User>()))
                .Returns(new List<EntitiesCommunicator.ConversionLinks>());
        }

        [Test]
        public void GetPreview_NoLayouts_ReturnsEmptyString()
        {
            // Arrange
            var customerManager = CreateCustomerManager();
            var layoutsTable = new DataTable();
            InitializeLayoutManager(layoutsTable);
            Layout.Initialize(_layoutManager.Object, customerManager.Object);
            var user = new User()
            {
                CustomerID = CustomerId
            };

            // Act
            var preview = Layout.GetPreview(LayoutId, ContentTypeCode.FILE, false, user);

            // Assert
            preview.ShouldBeEmpty();
        }

        [Test]
        public void GetPreview_ValidLayout_ReturnsBodyString()
        {
            // Arrange
            var customerManager = CreateCustomerManager();
            var layoutsTable = PrepareLayoutsTable();
            InitializeLayoutManager(layoutsTable);
            Layout.Initialize(_layoutManager.Object, customerManager.Object);
            var user = new User()
            {
                CustomerID = CustomerId
            };

            // Act
            var preview = Layout.GetPreview(LayoutId, ContentTypeCode.HTML, false, user);

            // Assert
            preview.ShouldBe($"<table  cellpadding=0 cellspacing=0  width='100%'><tr><td>{ Slot }</td></tr></table>");
        }

        [Test]
        public void GetPreviewNoAccessCheck_NoLayouts_ReturnsEmptyString()
        {
            // Arrange
            var customerManager = CreateCustomerManager();
            var layoutsTable = new DataTable();
            InitializeLayoutManager(layoutsTable);
            Layout.Initialize(_layoutManager.Object, customerManager.Object);

            // Act
            var preview = Layout.GetPreviewNoAccessCheck(LayoutId, ContentTypeCode.FILE, false, CustomerId);

            // Assert
            preview.ShouldBeEmpty();
        }

        [Test]
        public void GetPreviewNoAccessCheck_ValidLayout_ReturnsBodyString()
        {
            // Arrange
            var customerManager = CreateCustomerManager();
            var layoutsTable = PrepareLayoutsTable();
            InitializeLayoutManager(layoutsTable);
            Layout.Initialize(_layoutManager.Object, customerManager.Object);

            // Act
            var preview = Layout.GetPreviewNoAccessCheck(LayoutId, ContentTypeCode.HTML, false, CustomerId);

            // Assert
            preview.ShouldBe($"<table  cellpadding=0 cellspacing=0  width='100%'><tr><td>{ Slot }</td></tr></table>");
        }

        [Test]
        public void GetByCustomerID_ValidLayouts_ReturnsLayoutsList()
        {
            // Arrange
            _accessCheck
                .Setup(x => x.CanAccessByCustomer(It.IsAny<IList<EntitiesCommunicator.Layout>>(), It.IsAny<User>()))
                .Returns(true);

            _layoutManager
                .Setup(x => x.GetByCustomerID(CustomerId))
                .Returns(LayoutsList);

            Layout.Initialize(
                _layoutManager.Object,
                _user.Object,
                _accessCheck.Object,
                _content.Object,
                _template.Object,
                _folder.Object,
                _messageType.Object,
                _conversionLinks.Object);

            // Act
            var layoutList = Layout.GetByCustomerID(CustomerId, null, true);

            // Assert
            AssertLayoutList(layoutList);
            VerifyAllMocks();
        }

        [Test]
        public void GetByLayoutSearch8_ValidLayouts_ReturnsLayoutsList()
        {
            // Arrange
            var user = new User()
            {
                CustomerID = CustomerId
            };

            _accessCheck
                .Setup(x => x.CanAccessByCustomer(It.IsAny<IList<EntitiesCommunicator.Layout>>(), It.IsAny<User>()))
                .Returns(true);

            _layoutManager
                .Setup(x => x.GetByLayoutSearch(
                    string.Empty,
                    It.IsAny<int?>(),
                    CustomerId,
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<bool?>()))
                .Returns(LayoutsList);

            Layout.Initialize(
                _layoutManager.Object,
                _user.Object,
                _accessCheck.Object,
                _content.Object,
                _template.Object,
                _folder.Object,
                _messageType.Object,
                _conversionLinks.Object);

            // Act
            var layoutList = Layout.GetByLayoutSearch(string.Empty, null, CustomerId, null, null, user, true, null);

            // Assert
            AssertLayoutList(layoutList);
            VerifyAllMocks();
        }

        [Test]
        public void GetByLayoutSearch9_ValidLayouts_ReturnsLayoutsList()
        {
            // Arrange
            var user = new User()
            {
                CustomerID = CustomerId
            };

            _accessCheck
                .Setup(x => x.CanAccessByCustomer(It.IsAny<IList<EntitiesCommunicator.Layout>>(), It.IsAny<User>()))
                .Returns(true);

            _layoutManager
                .Setup(x => x.GetByLayoutSearch(
                    string.Empty, 
                    It.IsAny<int?>(),
                    CustomerId,
                    It.IsAny<int?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<bool?>()))
                .Returns(LayoutsList);

            Layout.Initialize(
                _layoutManager.Object,
                _user.Object,
                _accessCheck.Object,
                _content.Object,
                _template.Object,
                _folder.Object,
                _messageType.Object,
                _conversionLinks.Object);

            // Act
            var layoutList = Layout.GetByLayoutSearch(string.Empty, null, null, CustomerId, null, null, user, true, null);

            // Assert
            AssertLayoutList(layoutList);
            VerifyAllMocks();
        }

        [Test]
        public void GetByLayoutID_ValidLayouts_ReturnsLayoutsList()
        {
            // Arrange
            var user = new User()
            {
                CustomerID = CustomerId
            };

            _accessCheck
                .Setup(x => x.CanAccessByCustomer(It.IsAny<EntitiesCommunicator.Layout>(), It.IsAny<User>()))
                .Returns(true);

            _layoutManager
                .Setup(x => x.GetByLayoutID(LayoutId))
                .Returns(LayoutsList[0]);

            Layout.Initialize(
                _layoutManager.Object,
                _user.Object,
                _accessCheck.Object,
                _content.Object,
                _template.Object,
                _folder.Object,
                _messageType.Object,
                _conversionLinks.Object);

            // Act
            var layout = Layout.GetByLayoutID(LayoutId, user, true);

            // Assert
            layout.ShouldSatisfyAllConditions(
                () => layout.ShouldNotBeNull(),
                () => layout.ContentSlot1 = Slot);

            VerifyAllMocks();
        }

        [Test]
        public void GetByFolderIDCustomerID_ValidLayouts_ReturnsLayoutsList()
        {
            // Arrange
            var user = new User()
            {
                CustomerID = CustomerId
            };

            _accessCheck
                .Setup(x => x.CanAccessByCustomer(It.IsAny<IList<EntitiesCommunicator.Layout>>(), It.IsAny<User>()))
                .Returns(true);

            _layoutManager
                .Setup(x => x.GetByFolderIDCustomerID(FolderId, CustomerId))
                .Returns(LayoutsList);

            Layout.Initialize(
                _layoutManager.Object,
                _user.Object,
                _accessCheck.Object,
                _content.Object,
                _template.Object,
                _folder.Object,
                _messageType.Object,
                _conversionLinks.Object);

            // Act
            var layoutList = Layout.GetByFolderIDCustomerID(FolderId, user, true);

            // Assert
            AssertLayoutList(layoutList);
            VerifyAllMocks();
        }

        private void AssertLayoutList(List<EntitiesCommunicator.Layout> layoutList)
        {
            layoutList.ShouldSatisfyAllConditions(
                () => layoutList.ShouldNotBeNull(),
                () => layoutList.Count.ShouldBe(1));
        }

        private void VerifyAllMocks()
        {
            _layoutManager.VerifyAll();
            _content.Verify(x => x.GetByContentID(Slot, It.IsAny<User>(), true), Times.Exactly(9));
            _user.VerifyAll();
            _accessCheck.VerifyAll();
            _folder.VerifyAll();
            _template.VerifyAll();
            _messageType.VerifyAll();
            _conversionLinks.VerifyAll();
        }

        private List<EntitiesCommunicator.Layout> LayoutsList => new List<EntitiesCommunicator.Layout>()
        {
            new EntitiesCommunicator.Layout()
            {
                ContentSlot1 = Slot,
                ContentSlot2 = Slot,
                ContentSlot3 = Slot,
                ContentSlot4 = Slot,
                ContentSlot5 = Slot,
                ContentSlot6 = Slot,
                ContentSlot7 = Slot,
                ContentSlot8 = Slot,
                ContentSlot9 = Slot,
                TemplateID = TemplateId,
                FolderID = FolderId,
                MessageTypeID = MessageTypeId
            }
        };

        private void InitializeLayoutManager(DataTable layoutsTable)
        {
            _layoutManager
                .Setup(x => x.GetByLayoutID(LayoutId, CustomerId, BaseChannelId))
                .Returns(layoutsTable);
        }

        private Mock<ICustomerManager> CreateCustomerManager()
        {
            var customer = new Customer()
            {
                BaseChannelID = BaseChannelId
            };

            var customerManager = new Mock<ICustomerManager>();
            customerManager
                .Setup(x => x.GetByCustomerId(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(customer);

            return customerManager;
        }

        private DataTable PrepareLayoutsTable()
        {
            var layouts = new DataTable();
            layouts.Columns.Add("TableOptions");
            layouts.Columns.Add("ContentSlot1");
            layouts.Columns.Add("ContentSlot2");
            layouts.Columns.Add("ContentSlot3");
            layouts.Columns.Add("ContentSlot4");
            layouts.Columns.Add("ContentSlot5");
            layouts.Columns.Add("ContentSlot6");
            layouts.Columns.Add("ContentSlot7");
            layouts.Columns.Add("ContentSlot8");
            layouts.Columns.Add("ContentSlot9");
            layouts.Columns.Add("TemplateSource");
            layouts.Columns.Add("TemplateText");

            var dataRow = layouts.NewRow();
            for (var i = 1; i < layouts.Columns.Count; i++)
            {
                dataRow[i] = Slot;
            }
            layouts.Rows.Add(dataRow);

            return layouts;
        }
    }
}
