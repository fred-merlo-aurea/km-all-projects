using System;
using System.Diagnostics.CodeAnalysis;
using KMPlatform.Object;
using KMPS.MD.Main.Widgets;
using KMPS.MD.Main.Widgets.Fakes;
using KMPS.MD.Objects.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Main.Widgets
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TotalGrossSubscribersTests : BaseWidgetsTests
    {
        private const string MethodGetData = "GetData";

        private TotalGrossSubscribers _testEntity;
        private PrivateObject _testEntityPrivate;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new TotalGrossSubscribers();
            _testEntityPrivate = new PrivateObject(_testEntity);
            InitializeUserControl(_testEntity);
        }

        [Test]
        public void GetData_WithStartAndEndTime_AddsLineSeries()
        {
            // Arrange
            var mock = MockRadHtmlChart(RadHtmlChartName);
            ShimForCumulativeGrowth();
            ShimWidgetBase.AllInstances.ClientConnectionsGet = _ => null;

            // Act
            PrivateControl.Invoke(MethodGetData, 1, 2017, 2, 2018);

            // Assert
            mock.Object.PlotArea.Series.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void UserSessionGetter_IfUserSessionFieldIsNull_ReturnsEcnCurrentSession()
        {
            // Arrange
            _testEntityPrivate.SetField(UserSessionFieldName, null);

            // Act
            var returnedValue = _testEntity.UserSession;

            // Assert
            returnedValue.ShouldBeSameAs(ECNSessionCurrentFake);
        }

        [Test]
        public void UserSessionGetter_IfUserSessionFieldIsNotNull_ReturnsFieldValue()
        {
            // Arrange
            var shimAnotherEcnSession = new ShimECNSession();
            var anotherEcnSession = shimAnotherEcnSession.Instance;
            _testEntityPrivate.SetField(UserSessionFieldName, anotherEcnSession);

            // Act
            var returnedValue = _testEntity.UserSession;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeSameAs(anotherEcnSession),
                () => returnedValue.ShouldNotBeSameAs(ECNSessionCurrentFake));
        }

        [Test]
        public void ClientConnectionsGetter_IfClientConnectionsFieldIsNull_ReturnsNewClientConnectionsAndSetsItToField()
        {
            // Arrange
            _testEntityPrivate.SetField(ClientConnectionsFieldName, null);

            // Act
            var returnedValue = _testEntity.ClientConnections;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeSameAs(ClientConnectionsFake),
                () => _testEntityPrivate.GetField(ClientConnectionsFieldName).ShouldBeSameAs(ClientConnectionsFake));
        }

        [Test]
        public void ClientConnectionsGetter_IfClientConnectionsFieldIsNotNull_ReturnsFieldValue()
        {
            // Arrange
            var clientConnections = new ClientConnections();
            _testEntityPrivate.SetField(ClientConnectionsFieldName, clientConnections);

            // Act
            var returnedValue = _testEntity.ClientConnections;

            // Assert
            returnedValue.ShouldBeSameAs(clientConnections);
        }
    }
}