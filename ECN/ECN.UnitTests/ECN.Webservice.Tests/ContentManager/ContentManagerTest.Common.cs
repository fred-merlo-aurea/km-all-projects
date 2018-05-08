using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using ecn.webservice;
using ecn.webservice.Facades;
using ecn.webservice.Facades.Params;
using ECN.MarketinAutomation.Tests.Models.PostModels.Controls;

namespace ECN.Webservice.Tests.ContentManager
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ContentManagerTest : ManagerTestBase
    {
        private ecn.webservice.ContentManager _manager;
        private Mock<IContentFacade> _contentFacadeMock;

        [SetUp]
        public void Setup()
        {
            _executionWrapper = new WebMethodExecutionWrapper();
            _manager = new ecn.webservice.ContentManager(_executionWrapper);

            InitExecutionWrapperMocks();

            _contentFacadeMock = GetContentFacadeMock();
            _manager.ContentFacade = GetContentFacade(false);
        }

        [TearDown]
        public void TearDown()
        {
        }

        private Mock<IContentFacade> GetContentFacadeMock()
        {
            var contentFacadeMock = new Mock<IContentFacade>();

            return contentFacadeMock;
        }

        private IContentFacade GetContentFacade(bool useMock)
        {
            var result = useMock ? _contentFacadeMock.Object : new ContentFacade();

            return result;
        }

        private void MockSearchForContent(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.SearchForContent(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<string>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.SearchForContent(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<string>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockSearchForMessages(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.SearchForMessages(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<string>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.SearchForMessages(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<string>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetContentListByFolderId(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.GetContentListByFolderId(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.GetContentListByFolderId(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetMessageListByFolderId(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.GetMessageListByFolderId(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.GetMessageListByFolderId(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetContent(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.GetContent(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.GetContent(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetMessage(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.GetMessage(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.GetMessage(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockPreviewMessage(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.PreviewMessage(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.PreviewMessage(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockPreviewContent(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.PreviewContent(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.PreviewContent(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetFolders(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.GetFolders(
                    It.IsAny<WebMethodExecutionContext>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.GetFolders(
                        It.IsAny<WebMethodExecutionContext>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockDeleteFolder(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.DeleteFolder(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.DeleteFolder(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockDeleteContent(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.DeleteContent(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.DeleteContent(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockDeleteMessage(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.DeleteMessage(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.DeleteMessage(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockUpdateContent(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.UpdateContent(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<ContentParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.UpdateContent(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<ContentParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockUpdateMessage(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.UpdateMessage(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<MessageParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.UpdateMessage(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<MessageParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockAddContent(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.AddContent(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<ContentParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.AddContent(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<ContentParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockAddMessage(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.AddMessage(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<MessageParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.AddMessage(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<MessageParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockAddFolder(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.AddFolder(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<FolderParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.AddFolder(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<FolderParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetTemplates(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.GetTemplates(
                    It.IsAny<WebMethodExecutionContext>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.GetTemplates(
                        It.IsAny<WebMethodExecutionContext>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetMessageTypes(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.GetMessageTypes(
                    It.IsAny<WebMethodExecutionContext>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.GetMessageTypes(
                        It.IsAny<WebMethodExecutionContext>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetCustomerDepts(
            Mock<IContentFacade> contentFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            contentFacadeMock
                .Setup(mock => mock.GetCustomerDepts(
                    It.IsAny<WebMethodExecutionContext>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                contentFacadeMock
                    .Setup(mock => mock.GetCustomerDepts(
                        It.IsAny<WebMethodExecutionContext>()))
                    .Throws(exceptionToRaise);
            }
        }
    }
}