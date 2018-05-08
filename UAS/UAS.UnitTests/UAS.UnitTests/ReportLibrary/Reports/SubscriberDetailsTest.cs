using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using FrameworkServices.Fakes;
using FrameworkUAS.Service;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using ReportLibrary.Reports;
using Shouldly;
using Telerik.Reporting.Processing;
using Telerik.Reporting.Processing.Fakes;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes; 
using PlatformClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.ReportLibrary.Reports
{
    /// <summary>
    /// Unit test for <see cref="SubscriberDetails"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public partial class SubscriberDetailsTest
    {
        private const string Filters = "Filters";
        private const string AdHocFilters = "AdHocFilters";
        private const string IssueID = "IssueID";
        private const string GetParametersMethodName = "GetParameters";
        private SubscriberDetails _subscriberDetails;
        private PrivateObject _privateObject;
        private IDisposable _shimObject;
        private bool _logCriticalError;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
            _subscriberDetails = new SubscriberDetails();
            _privateObject = new PrivateObject(_subscriberDetails);
            ShimAppData.AllInstances.AuthorizedUserGet = (x) =>
              new UserAuthorization
              {
                  User = new User
                  {
                      CurrentClient = new Client
                      {
                          ClientConnections = new PlatformClientConnections()
                      }
                  }
              };
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (sender, exceptionMessage, sourceMethod, application, note, clientId, subject) =>
                {
                    _logCriticalError = true;
                    return 1;
                };
            ShimReport.Constructor = (sender) => { };
            ShimReport.AllInstances.ParametersGet = (sender) =>
                new Dictionary<string, Parameter>
                {
                    { Filters, new Parameter { Name = Filters, Value = Filters } },
                    { AdHocFilters, new Parameter { Name = AdHocFilters, Value = AdHocFilters} },
                    { IssueID, new Parameter { Name = IssueID, Value = "1"} }
                };

            ShimProcessingElement.AllInstances.ReportGet = (sender) => new ShimReport();

            ShimServiceClient.UAS_ApplicationLogClient = () => new ShimServiceClient<IApplicationLog>
            {
                ProxyGet = () => new StubIApplicationLog
                {
                    LogCriticalErrorGuidStringStringEnumsApplicationsStringInt32 =
                    (sender, exceptionMessage, sourceMethod, application, note, clientId) => 
                    {
                        _logCriticalError = true;
                        return new Response<int> { Result = 1 };
                    }
                }
            };
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void GetParameters_LoadPageControlThrowException_LogCriticalErrorForApplication()
        {
            // Arrange
            var mock = new Mock<DataItem>();
            DataItem dataItem = new ShimDataItem(mock.Object);
            var parameters = new object[] { dataItem, EventArgs.Empty };

            // Act
            _privateObject.Invoke(GetParametersMethodName, parameters);

            // Assert
            _logCriticalError.ShouldBeTrue();
        }
    }
}
