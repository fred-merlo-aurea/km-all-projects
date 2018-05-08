using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using ecn.webservice;
using ecn.webservice.Facades;
using ecn.webservice.Facades.Params;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ListManagerTest: ManagerTestBase
    {
        private ListManager _manager;
        private Mock<IListFacade> _listFacadeMock;

        [SetUp]
        public void Setup()
        {
            _executionWrapper = new WebMethodExecutionWrapper();
            _manager = new ListManager(_executionWrapper);

            InitExecutionWrapperMocks();

            _listFacadeMock = GetListFacadeMock();
            _manager.ListFacade = GetListFacade(false);
        }

        [TearDown]
        public void TearDown()
        {
        }
        
        private Mock<IListFacade> GetListFacadeMock()
        {
            var listFacadeMock = new Mock<IListFacade>();
            
            return listFacadeMock;
        }

        private IListFacade GetListFacade(bool useMock)
        {
            var result = useMock ? _listFacadeMock.Object : new ListFacade();

            return result;
        }

        private void MockGetFolders(
            Mock<IListFacade> listFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            listFacadeMock
                .Setup(mock => mock.GetFolders(
                    It.IsAny<WebMethodExecutionContext>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                listFacadeMock
                    .Setup(mock => mock.GetFolders(
                        It.IsAny<WebMethodExecutionContext>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetListEmailProfilesByEmailAddress(
            Mock<IListFacade> listFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            listFacadeMock
                .Setup(mock => mock.GetListEmailProfilesByEmailAddress(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<GetListEmailProfilesParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                listFacadeMock
                    .Setup(mock => mock.GetListEmailProfilesByEmailAddress(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<GetListEmailProfilesParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetCustomFields(
            Mock<IListFacade> listFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            listFacadeMock
                .Setup(mock => mock.GetCustomFields(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                listFacadeMock
                    .Setup(mock => mock.GetCustomFields(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetFilters(
            Mock<IListFacade> listFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            listFacadeMock
                .Setup(mock => mock.GetFilters(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                listFacadeMock
                    .Setup(mock => mock.GetFilters(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }
    }
}
