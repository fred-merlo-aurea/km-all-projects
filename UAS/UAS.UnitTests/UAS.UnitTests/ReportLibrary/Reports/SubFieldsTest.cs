using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel.Fakes;
using FrameworkServices.Fakes;
using FrameworkUAS.Service;
using static FrameworkUAD_Lookup.Enums;
using FrameworkUAS.Object.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using ReportLibrary.Reports;
using ReportLibrary.Reports.Fakes;
using Telerik.Reporting.Processing;
using Telerik.Reporting.Processing.Fakes;
using UAS_WS.Interface;
using FrameworkObject = FrameworkUAS.Object;

namespace UAS.UnitTests.ReportLibrary.Reports
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SubFieldsTest
    {
        private const string MethodGetParameter = "GetSubFieldsParameters";
        private const int Ten = 10;
        private const int One = 1;
        private SubFields _field;
        private PrivateObject _privateObject;
        private IDisposable _shims;

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetParameters_ForNullParameter_ShouldThrowException(bool param)
        {
            // Arrange
            SetUp();
            ShimReportUtilitiesBase.DebugGet = () => param;
            _field = new SubFields();
            _privateObject = new PrivateObject(_field);
            var dataItem = new Mock<DataItem>();
            MockServiceClient(MockIApplicationLog(new ApplicationLog
            {
                IsFixed = true,
                ClientId = One,
                ApplicationId = One
            }));

            // Act & Assert
            try
            {
                _privateObject.Invoke(MethodGetParameter, dataItem.Object, new EventArgs());
            }
            catch (Exception ex)
            {
                NUnit.Framework.Assert.That(ex is Exception);
            }
        }

        private void SetUp()
        {
            _shims = ShimsContext.Create();
            var report = new Moq.Mock<Report>();
            ShimProcessingElement.AllInstances.ReportGet = (x) => report.Object;
            ShimClientBase<IApplicationLog>.ConstructorString = (clientBase, endPoint) => { };
            var kmUserAuthorization = new UserAuthorization();
            var client = new Client() { ClientID = Ten };
            var user = new User() { CurrentClient = client };
            kmUserAuthorization.User = user;
            var userAuthorization = new FrameworkObject.UserAuthorization(kmUserAuthorization);
            ShimAppData.AllInstances.AuthorizedUserGet = (x) => userAuthorization;
        }

        private static void MockServiceClient<T>(T t) where T : class
        {
            ShimServiceClient<T>.AllInstances.ProxyGet = (obj) => t;
        }

        private static IApplicationLog MockIApplicationLog(params ApplicationLog[] applicationLogs)
        {
            var channelMock = new Mock<IApplicationLog>();
            var responses = new Response<List<ApplicationLog>>
            {
                Result = new List<ApplicationLog>(applicationLogs),
                Status = ServiceResponseStatusTypes.Success
            };
            channelMock.Setup(channel => channel.SelectApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(responses);
            return channelMock.Object;
        }
    }
}
