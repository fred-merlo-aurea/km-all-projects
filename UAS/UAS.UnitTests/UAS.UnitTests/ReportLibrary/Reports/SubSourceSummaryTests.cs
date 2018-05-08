using System;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.Object;
using FrameworkServices.Fakes;
using FrameworkUAS.Object.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ReportLibrary.Reports;
using Shouldly;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;
using FrameworkUASService = FrameworkUAS.Service;

namespace UAS.UnitTests.ReportLibrary.Reports
{
    [TestFixture][ExcludeFromCodeCoverage]
    public partial class SubSourceSummaryTests
    {
        private const string GetParameters = "GetParameters";
        private SubSourceSummary _subSourceSummary;
        private PrivateObject _privateObject;
        private IDisposable _shimObject;
        private bool _logCriticalError = false;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void GetParametersTest_LogCriticalErrorForApplication()
        {
            // Arrange
            ReportUtilitiesBase.Debug = true;
            CreateClassObject();
            LogCriticalErrorForApplication();
            CreateAuthorizedUser();                       

            var parameters = new object[] { _subSourceSummary, EventArgs.Empty };

            // Act
            _privateObject.Invoke(GetParameters, parameters);

            // Assert
            _logCriticalError.ShouldBeTrue();
        }

        private void CreateAuthorizedUser()
        {
            ShimAppData.AllInstances.AuthorizedUserGet = (x) =>
            {
                return new UserAuthorization
                {
                    User = new User
                    {
                        CurrentClient = new Client
                        {
                            ClientConnections = new KMPlatform.Object.ClientConnections()
                        }
                    }
                };
            };
        }

        private void CreateClassObject()
        {
            _subSourceSummary = new SubSourceSummary();
            _privateObject = new PrivateObject(_subSourceSummary);
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