using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.Entity;
using FrameworkServices.Fakes;
using FrameworkUASService = FrameworkUAS.Service;
using FrameworkUAS.Object.Fakes;
using FrameworkUAS.Object;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ReportLibrary.Reports;
using ReportLibrary.Reports.Fakes;
using Shouldly;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;

namespace UAS.UnitTests.ReportLibrary.Reports
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class SingleResponseTest
    {
        private IDisposable _shimContex;
        private PrivateObject _privateTestedObject;
        private SingleResponse _singleResponse;
        private bool _logCriticalError;
        private const int ClientID = 1;
        private const string TestedMethod_GetParameters = "GetParameters";

        [SetUp]
        public void TestSetup()
        {
            _shimContex = ShimsContext.Create();          

            ShimReportUtilities.GetResponsesInt32 = (productId) =>
            {
                return new List<ResponseGroup>();
            };

             _singleResponse = new SingleResponse();
            _privateTestedObject = new PrivateObject(_singleResponse);
        }

        [TearDown]
        public void TestTearDown()
        {
            _shimContex.Dispose();
        }
        
        [Test]
        public void SingleResponse_GetParameters_WithLoggedException()
        {
            // Arrange            
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { _singleResponse, eventArgs };
            LogCriticalErrorForApplication();
            CreateAuthorizedUser();

            // Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethod_GetParameters, methodArguments));

            // Assert
            _logCriticalError.ShouldBeTrue();
        }
        
        private void CreateAuthorizedUser()
        {
            ShimAppData.AllInstances.AuthorizedUserGet = (x) =>
            {
                return new UserAuthorization
                {
                    User = new User()
                    {
                        CurrentClient = new Client()
                        {
                            ClientConnections = new KMPlatform.Object.ClientConnections(),
                            ClientID = ClientID
                        }
                    }
                };
            };
        }
        private void LogCriticalErrorForApplication()
        {
            ShimServiceClient.UAS_ApplicationLogClient = () =>
            {
                return new ShimServiceClient<IApplicationLog>
                {
                    ProxyGet = () =>
                    {
                        return new StubIApplicationLog()
                        {
                            LogCriticalErrorGuidStringStringEnumsApplicationsStringInt32 =
                            (accessKey, ex, sourceMethod, application, note, clientId) =>
                            {
                                _logCriticalError = true;
                                return new FrameworkUASService.Response<int>
                                {
                                    Message = string.Empty,
                                    ProcessCode = string.Empty,
                                    Result = 1,
                                    Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error
                                };
                            }
                        };
                    }
                };
            };
        }
    }
}
