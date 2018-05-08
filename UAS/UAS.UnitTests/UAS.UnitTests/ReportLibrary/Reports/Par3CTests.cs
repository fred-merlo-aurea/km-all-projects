using System;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD_Lookup;
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
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class Par3CTests
    {
        private const string GetParameters = "GetParameters";
        private Par3C _par3C;
        private PrivateObject _privateObject;
        private IDisposable _shimObject;
        private bool _logCriticalError;

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
            ReportUtilities.Debug = true;

            CreateClassObject();
            LogCriticalErrorForApplication();
            CreateAuthorizedUser();

            var parameters = new object[] { _par3C, EventArgs.Empty };

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
            _par3C = new Par3C();
            _privateObject = new PrivateObject(_par3C);
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
                                        Status = Enums.ServiceResponseStatusTypes.Error
                                    };
                                }
                        };
                    }
                };
            };
        }
    }
}