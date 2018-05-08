using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;
using Telerik.Reporting.Processing;
using Telerik.Reporting.Processing.Fakes;
using UAS.ReportLibrary.Reports;

namespace UAS.ReportLibrary.Tests.Reports
{
    //// <summary>
    /// Unit test for <see cref="SubscriberPaidDetails"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class SubscriberPaidDetailsTest
    {
        private const string GetParameters = "GetParameters";
        private const string Filters = "Filters";
        private IDisposable _shimObject;
        private SubscriberPaidDetails _subscriberPaidDetails;
        private PrivateObject _privateObject;
        private bool _logCriticalError;

        [SetUp]
        public void Setup()
        {
            _subscriberPaidDetails = new SubscriberPaidDetails();
            _privateObject = new PrivateObject(_subscriberPaidDetails);
            _shimObject = ShimsContext.Create();
            ShimAppData.AllInstances.AuthorizedUserGet = (x) =>
              new UserAuthorization
              {
                  User = new User
                  {
                      CurrentClient = new Client
                      {
                          ClientConnections = new KMPlatform.Object.ClientConnections()
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
                    { Filters, new Parameter { Name = Filters, Value = Filters } }
                };

            ShimProcessingElement.AllInstances.ReportGet = (sender) => new ShimReport();
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void CategorySummary_LoadPageControlThrowException_LogCriticalErrorForApplication()
        {
            // Arrange
            var mock = new Mock<DataItem>();
            DataItem dataItem = new ShimDataItem(mock.Object);
            var parameters = new object[] { dataItem, EventArgs.Empty };

            // Act
            _privateObject.Invoke(GetParameters, parameters);

            // Assert
            _logCriticalError.ShouldBeTrue();
        }
    }
}
