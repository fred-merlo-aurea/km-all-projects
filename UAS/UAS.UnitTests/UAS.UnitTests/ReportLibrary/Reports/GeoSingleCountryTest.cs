using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Core_AMS.Utilities.Fakes;
using FrameworkServices.Fakes;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Reporting.Processing.Fakes;
using UAS.ReportLibrary.Fakes;
using UAS.ReportLibrary.Reports;
using UAS.UnitTests.Helpers;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;
using FrameworkUASService = FrameworkUAS.Service;
using Moq;
using NUnit.Framework;
using Shouldly;
using static FrameworkUAD_Lookup.Enums;

namespace UAS.UnitTests.ReportLibrary.Reports
{
    /// <summary>
    /// Unit test for <see cref="GeoSingleCountry"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class GeoSingleCountryTest
    {
        private GeoSingleCountry _geoSingleCountry;
        private PrivateObject _privateObject;
        protected IDisposable _shimObject;
        private const string GetParameters = "GetParameters";
        private bool _logCriticalError = false;
        private bool _errorSaved = false;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
            ShimReportUtilities.GetCountries = () => new DataTable();
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void GetParameters_ReportParametersIsNull_ExceptionIsThrownAndIsSavedToDatabase()
        {
            // Arrange
            CreateClassObject();
            CreateAuthorizedUser();
            var mock = new Mock<Telerik.Reporting.Processing.DataItem>();
            Telerik.Reporting.Processing.Report report = new ShimReport();
            ShimProcessingElement.AllInstances.ReportGet = (x) => report;
            ShimStringFunctions.FormatExceptionException = (x) => string.Empty;
            ShimApplicationLog.AllInstances.SaveApplicationLogEnumsApplicationsEnumsSeverityTypesString = (a, b, c, d, e) =>
            {
                _errorSaved = true;
                return 0;
            };
            var parameters = new object[] { mock.Object, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(GeoSingleCountry), GetParameters, parameters, _geoSingleCountry);

            // Assert
            _errorSaved.ShouldBeTrue();
        }

        [Test]
        public void GetParameters_ReportParametersIsNull_LogCriticalErrorForApplication()
        {
            // Arrange
            CreateClassObject();
            LogCriticalErrorForApplication();
            CreateAuthorizedUser();
            var mock = new Mock<Telerik.Reporting.Processing.DataItem>();
            Telerik.Reporting.Processing.Report report = new ShimReport();
            ShimProcessingElement.AllInstances.ReportGet = (x) => report;
            var parameters = new object[] { mock.Object, EventArgs.Empty };

            // Act
            ReflectionHelper.CallMethod(typeof(GeoSingleCountry), GetParameters, parameters, _geoSingleCountry);

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

        private void LogCriticalErrorForApplication()
        {
            ShimStringFunctions.FormatExceptionException = (x) => string.Empty;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String = (a, b, c, d, e, f, g) =>
            {
                _logCriticalError = true;
                return 0;
            };
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
                                        Status = ServiceResponseStatusTypes.Error
                                    };
                                }
                        };
                    }
                };
            };
        }

        private void CreateClassObject()
        {
            _geoSingleCountry = new GeoSingleCountry();
            _privateObject = new PrivateObject(_geoSingleCountry);
        }
    }
}