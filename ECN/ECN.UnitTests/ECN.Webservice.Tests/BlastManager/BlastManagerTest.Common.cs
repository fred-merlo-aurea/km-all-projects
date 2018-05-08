using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ecn.webservice;
using ecn.webservice.Facades;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using KMPlatform.Object;
using Moq;
using NUnit.Framework;
using BlastManager = ecn.webservice.BlastManager;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BlastManagerTest: ManagerTestBase
    {
        private const int SampleBlastId = 111;
        private const int SampleListId = 8765;
        private const int SampleMessageId = 9876;
        private const int SampleDeptId = 7654;
        private const int SampleFilterId = 6543;
        private const string SampleSubject = "SampleSubject";
        private const string SampleFromEmail = "SampleFromEmail";
        private const string SampleFromName = "SampleFromName";
        private const string SampleReplyMail = "SampleReplyEmail";
        private const string SampleXmlSchedule = "SampleXmlSchedule";
        private const string SampleRefBlasts = "SampleRefBlasts";
        private const int SampleGetBlastEmailListCount = 9001;
        private const int SampleCampaignItemId = 112233;
        private const string SampleFilterType = "SampleFilterType";

        private BlastManager _manager;
        private Mock<IBlastManager> _blastManagerMock;
        private Mock<ICampaignItemBlastManager> _campaignItemBlastManagerMock;
        private Mock<ICampaignItemTestBlastManager> _campaignItemTestBlastManagerMock;
        private Mock<ICampaignItemBlastRefBlastManager> _campaignItemBlastRefBlastManagerMock;
        private Mock<IBlastFacade> _blastFacadeMock;

        [SetUp]
        public void Setup()
        {
            _executionWrapper = new WebMethodExecutionWrapper();
            _manager = new BlastManager(_executionWrapper);

            InitExecutionWrapperMocks();

            _blastManagerMock = GetBlastManagerMock();
            _campaignItemBlastManagerMock = GetCampaignItemBlastManagerMock();
            _campaignItemTestBlastManagerMock = GetCampaignItemTestBlastManagerMock();
            _campaignItemBlastRefBlastManagerMock = GetCampaignItemBlastRefBlastManagerMock();
            _blastFacadeMock = GetBlastFacadeMock();
            _manager.BlastFacade = GetBlastFacade(false);
        }

        [TearDown]
        public void TearDown()
        {
        }
        
        private CommunicatorEntities.BlastAbstract GetBlast()
        {
            var blast = new CommunicatorEntities.BlastRegular();

            return blast;
        }

        private CommunicatorEntities.CampaignItemBlast GetCampaignItemBlast()
        {
            var campaignItemBlast = new CommunicatorEntities.CampaignItemBlast()
            {
                CampaignItemID = SampleCampaignItemId
            };

            return campaignItemBlast;
        }

        private CommunicatorEntities.CampaignItemTestBlast GetCampaignItemTestBlast()
        {
            var campaignItemTestBlast = new CommunicatorEntities.CampaignItemTestBlast();

            return campaignItemTestBlast;
        }

        private Mock<ICampaignItemBlastManager> GetCampaignItemBlastManagerMock()
        {
            var campaignItemBlastManagerMock = new Mock<ICampaignItemBlastManager>();

            MockGetByBlastId(campaignItemBlastManagerMock, GetCampaignItemBlast());
            
            return campaignItemBlastManagerMock;
        }

        private Mock<ICampaignItemTestBlastManager> GetCampaignItemTestBlastManagerMock()
        {
            var campaignItemTestBlastManagerMock = new Mock<ICampaignItemTestBlastManager>();

            MockGetByBlastId(campaignItemTestBlastManagerMock, GetCampaignItemTestBlast());

            return campaignItemTestBlastManagerMock;
        }

        private Mock<ICampaignItemBlastRefBlastManager> GetCampaignItemBlastRefBlastManagerMock()
        {
            var campaignItemBlastRefBlastManagerMock = new Mock<ICampaignItemBlastRefBlastManager>();

            return campaignItemBlastRefBlastManagerMock;
        }

        private Mock<IBlastManager> GetBlastManagerMock()
        {
            var blastManagerMock = new Mock<IBlastManager>();

            MockGetByBlastId(blastManagerMock, GetBlast());
            MockGetBySearch(blastManagerMock, new List<CommunicatorEntities.BlastAbstract> { GetBlast() });
            MockGetBlastEmailListForDynamicContent(blastManagerMock, GetBlastEmailListDataTable());

            return blastManagerMock;
        }

        private Mock<IBlastFacade> GetBlastFacadeMock()
        {
            var blastFacadeMock = new Mock<IBlastFacade>();
            
            return blastFacadeMock;
        }

        private IBlastFacade GetBlastFacade(bool useMock)
        {
            var result = useMock ? _blastFacadeMock.Object : new BlastFacade();

            result.BlastsManager = _blastManagerMock.Object;
            result.CampaignItemBlastsManager = _campaignItemBlastManagerMock.Object;
            result.CampaignItemTestBlastManager = _campaignItemTestBlastManagerMock.Object;
            result.CampaignItemBlastRefBlastManager = _campaignItemBlastRefBlastManagerMock.Object;

            return result;
        }

        private DataTable GetBlastEmailListDataTable()
        {
            var result = new DataTable();
            result.Columns.Add(string.Empty, typeof(int));
            result.Rows.Add(SampleGetBlastEmailListCount);

            return result;
        }

        private void MockGetByBlastId(
            Mock<IBlastManager> blastManagerMock,
            CommunicatorEntities.BlastAbstract result,
            Exception exceptionToRaise = null)
        {
            blastManagerMock
                .Setup(mock => mock.GetByBlastId(
                    It.IsAny<int>(),
                    It.IsAny<User>(),
                    It.IsAny<bool>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastManagerMock
                    .Setup(mock => mock.GetByBlastId(
                        It.IsAny<int>(),
                        It.IsAny<User>(),
                        It.IsAny<bool>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetByBlastId(
            Mock<ICampaignItemBlastManager> campaignItemTestBlastManagerMock,
            CommunicatorEntities.CampaignItemBlast result,
            Exception exceptionToRaise = null)
        {
            campaignItemTestBlastManagerMock
                .Setup(mock => mock.GetByBlastId(
                    It.IsAny<int>(),
                    It.IsAny<User>(),
                    It.IsAny<bool>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                campaignItemTestBlastManagerMock
                    .Setup(mock => mock.GetByBlastId(
                        It.IsAny<int>(),
                        It.IsAny<User>(),
                        It.IsAny<bool>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetByBlastId(
            Mock<ICampaignItemTestBlastManager> campaignItemTestBlastManagerMock,
            CommunicatorEntities.CampaignItemTestBlast result,
            Exception exceptionToRaise = null)
        {
            campaignItemTestBlastManagerMock
                .Setup(mock => mock.GetByBlastId(
                    It.IsAny<int>(),
                    It.IsAny<User>(),
                    It.IsAny<bool>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                campaignItemTestBlastManagerMock
                    .Setup(mock => mock.GetByBlastId(
                        It.IsAny<int>(),
                        It.IsAny<User>(),
                        It.IsAny<bool>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetBySearch(
            Mock<IBlastManager> blastManagerMock,
            IList<CommunicatorEntities.BlastAbstract> result,
            Exception exceptionToRaise = null)
        {
            blastManagerMock
                .Setup(mock => mock.GetBySearch(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<bool?>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<int?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<User>(),
                    It.IsAny<bool>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastManagerMock
                    .Setup(mock => mock.GetBySearch(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<int?>(),
                        It.IsAny<int?>(),
                        It.IsAny<bool?>(),
                        It.IsAny<string>(),
                        It.IsAny<DateTime?>(),
                        It.IsAny<DateTime?>(),
                        It.IsAny<int?>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<User>(),
                        It.IsAny<bool>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockAddBlast(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.AddBlast(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<AddBlastParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.AddBlast(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<AddBlastParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockAddScheduledBlast(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.AddScheduledBlast(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.AddScheduledBlast(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<Dictionary<string, object>>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockUpdateBlast(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.UpdateBlast(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<UpdateBlastParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.UpdateBlast(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<UpdateBlastParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetBlastReport(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.GetBlastReport(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<int>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.GetBlastReport(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<int>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetBlastReportByISP(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.GetBlastReportByISP(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<GetBlastReportByISPParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.GetBlastReportByISP(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<GetBlastReportByISPParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetBlastOpensReport(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.GetBlastOpensReport(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<GetBlastReportParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.GetBlastOpensReport(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<GetBlastReportParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetBlastClicksReport(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.GetBlastClicksReport(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<GetBlastReportParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.GetBlastClicksReport(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<GetBlastReportParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetBlastBounceReport(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.GetBlastBounceReport(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<GetBlastReportParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.GetBlastBounceReport(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<GetBlastReportParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetBlastUnsubscribeReport(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.GetBlastUnsubscribeReport(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<GetBlastReportParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.GetBlastUnsubscribeReport(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<GetBlastReportParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetBlastDeliveryReport(
            Mock<IBlastFacade> blastFacadeMock,
            string result,
            Exception exceptionToRaise = null)
        {
            blastFacadeMock
                .Setup(mock => mock.GetBlastDeliveryReport(
                    It.IsAny<WebMethodExecutionContext>(),
                    It.IsAny<GetBlastDeliveryReportParams>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastFacadeMock
                    .Setup(mock => mock.GetBlastDeliveryReport(
                        It.IsAny<WebMethodExecutionContext>(),
                        It.IsAny<GetBlastDeliveryReportParams>()))
                    .Throws(exceptionToRaise);
            }
        }

        private void MockGetBlastEmailListForDynamicContent(
            Mock<IBlastManager> blastManagerMock,
            DataTable result,
            Exception exceptionToRaise = null)
        {
            blastManagerMock.Setup(mock => mock.GetBlastEmailListForDynamicContent(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<List<CommunicatorEntities.CampaignItemBlastFilter>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .Returns(result);

            if (exceptionToRaise != null)
            {
                blastManagerMock.Setup(mock => mock.GetBlastEmailListForDynamicContent(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<List<CommunicatorEntities.CampaignItemBlastFilter>>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<bool>()))
                    .Throws(exceptionToRaise);
            }
        }
    }
}
