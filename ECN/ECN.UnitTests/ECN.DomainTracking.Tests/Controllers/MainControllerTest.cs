using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Web.Security;
using System.Web.Security.Fakes;
using System.Web.UI.DataVisualization.Charting;
using ecn.domaintracking.Controllers;
using ecn.domaintracking.Models;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Assert = NUnit.Framework.Assert;

namespace ECN.DomainTracking.Tests.Controllers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MainControllerTest
    {
        private IDisposable _shimObject;
        private MainController _controller;
        private PrivateObject _controllerObject;
        private static string _authenticationUserData;

        private Chart _chart = new Chart();
        private Color _selectedColor = Color.SteelBlue;
        private string _property;
        private string _legend;

        private const int AxisXInterval = 0;
        private const int BorderWidth = 3;
        private const int MarkerSize = 10;
        private const int ShadowOffset = 0;
        private const bool IsVisibleInLegend = true;
        private const bool IsValueShownAsLabel = true;
        private const string DateKey = "Date";
        private const string ViewsKey = "Views";
        private const string PageViewsKey = "Page Views";
        private const string ToolTipKey = "#VALY{G}";

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();

            var httpContext = MvcMockHelpers.MockHttpContext();

            _controller = new MainController();
            _controller.SetMockControllerContext(httpContext);
            _controllerObject = new PrivateObject(_controller, new PrivateType(typeof(MainController)));;
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void _ClientDropDown_AuthorisedUserClientGroupChange_sets_AuthenticationTicketInCookie()
        {
            // Arrange
            const int clientGroupId = 5;
            var clientDropdownModel = new ClientDropDown()
            {
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 3,
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = clientGroupId } }
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(clientGroupId, expectedAuthenticationTicketHash, true, false);
            ShimUser.IsChannelAdministratorUser = (_) => true;

            // Act
            _controller._ClientDropDown(clientDropdownModel);

            // Assert
            _controller.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,3,10,00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void _ClientDropDown_AuthorisedUserClientChange_sets_AuthenticationTicketInCookie()
        {
            // Arrange
            const int clientGroupId = 5;
            var clientDropdownModel = new ClientDropDown()
            {
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = clientGroupId } },
                SelectedClientID = 4,
                CurrentClientID = 6,
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 99
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(clientGroupId, expectedAuthenticationTicketHash, true, false);
            ShimUser.IsChannelAdministratorUser = (_) => false;

            // Act
            _controller._ClientDropDown(clientDropdownModel);

            // Assert
            _controller.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,5,20,00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void _ClientDropDown_UnAuthorisedUser_DoesNotSet_AuthenticationTicketInCookie()
        {
            // Arrange
            const int clientGroupId = 5;
            var clientDropdownModel = new ClientDropDown()
            {
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = clientGroupId } },
                SelectedClientID = 4,
                CurrentClientID = 6,
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 99
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(clientGroupId, expectedAuthenticationTicketHash, false, false);
            ShimUser.IsChannelAdministratorUser = (_) => false;

            // Act
            _controller._ClientDropDown(clientDropdownModel);

            // Assert
            _controller.Response.Cookies[FormsAuthentication.FormsCookieName].ShouldBeNull();
        }

        [Test]
        public void _ClientDropDown_SystemAdministratorClientChange_sets_AuthenticationTicketInCookie()
        {
            // Arrange
            const int clientGroupId = 5;
            var clientDropdownModel = new ClientDropDown()
            {
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = clientGroupId } },
                SelectedClientID = 4,
                CurrentClientID = 6,
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 99
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(clientGroupId, expectedAuthenticationTicketHash, false, true);
            ShimUser.IsChannelAdministratorUser = (_) => false;

            // Act
            _controller._ClientDropDown(clientDropdownModel);

            // Assert
            _controller.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,5,20,00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void HasAuthorised_AuthorisedClient_true()
        {
            // Arrange
            var authorisedUserId = 10;
            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User()
            {
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
                {
                    new UserClientSecurityGroupMap() { ClientID = authorisedUserId}
                }
            };
            ShimECNSession.CurrentSession = () => session;

            // Act and  Assert
            _controller.HasAuthorized(0, authorisedUserId).ShouldBeTrue();
        }

        [Test]
        public void HasAuthorised_NotAuthorisedClient_false()
        {
            // Arrange
            var authorisedUserId = 10;
            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User()
            {
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
            };
            ShimECNSession.CurrentSession = () => session;

            // Act and  Assert
            _controller.HasAuthorized(0, authorisedUserId).ShouldBeFalse();
        }

        [Test]
        public void SetChartSeries_WhenChartIsNull_ThrowsException()
        {
            // Arrange
            _chart = null;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() => 
                _controllerObject.Invoke("SetChartSeries", _chart, _property, _legend, _selectedColor));

            // Assert
            Assert.IsInstanceOf<Exception>(exception.InnerException);
        }

        [Test]
        public void SetChartSeries_WhenPropertyIsNullOrWhiteSpace_ThrowsException()
        {
            // Arrange
            _chart = new Chart();
            _property = string.Empty;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _controllerObject.Invoke("SetChartSeries", _chart, _property, _legend, _selectedColor));

            // Assert
            Assert.IsInstanceOf<Exception>(exception.InnerException);
        }

        [Test]
        [TestCase("Sample1", "")]
        [TestCase("Sample1", "Legend 1")]
        public void SetChartSeries_WhenChartAndPropertyAreNotNull_ShouldFillAllFields(string property, string legend)
        {
            // Arrange
            _chart = new Chart();
            _chart.Series.Clear();
            _selectedColor = Color.Orange;

            CreateLegendForChart(_chart, legend, property);

            // Act
            _controllerObject.Invoke("SetChartSeries", _chart, property, legend, _selectedColor);

            // Assert
            _chart.ShouldSatisfyAllConditions(
                () => _chart.Series[0].Name.ShouldBe(property),
                () => _chart.Series[0].XValueMember.ShouldBe(DateKey),
                () => _chart.Series[0].YValueMembers.ShouldBe(ViewsKey),
                () => _chart.Series[0].ChartType.ShouldBe(SeriesChartType.Line),
                () => _chart.Series[0].IsVisibleInLegend.ShouldBe(IsVisibleInLegend),
                () => _chart.Series[0].Legend.ShouldBe(legend),
                () => _chart.Series[0].ShadowOffset.ShouldBe(ShadowOffset),
                () => _chart.Series[0].ToolTip.ShouldBe(ToolTipKey),
                () => _chart.Series[0].BorderWidth.ShouldBe(BorderWidth),
                () => _chart.Series[0].Color.ShouldBe(_selectedColor),
                () => _chart.Series[0].IsValueShownAsLabel.ShouldBe(IsValueShownAsLabel),
                () => _chart.Series[0].MarkerSize.ShouldBe(MarkerSize),
                () => _chart.Series[0].MarkerStyle.ShouldBe(MarkerStyle.Circle));
        }

        [Test]
        public void SetChartAreas_WhenChartIsNull_ThrowsException()
        {
            // Arrange
            _chart = null;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _controllerObject.Invoke("SetChartAreas", _chart, _property, AxisXInterval));

            // Assert
            Assert.IsInstanceOf<Exception>(exception.InnerException);
        }

        [Test]
        public void SetChartAreas_WhenPropertyIsNullOrWhiteSpace_ThrowsException()
        {
            // Arrange
            _chart = new Chart();
            _property = string.Empty;

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _controllerObject.Invoke("SetChartAreas", _chart, _property, AxisXInterval));

            // Assert
            Assert.IsInstanceOf<Exception>(exception.InnerException);
        }

        [Test]
        public void SetChartAreas_WhenChartIsNotNull_ShouldFillAllFields()
        {
            // Arrange
            _chart = new Chart();
            _chart.ChartAreas.Clear();
            _property = "Sample1";

            // Act
            _controllerObject.Invoke("SetChartAreas", _chart, _property, AxisXInterval);

            // Assert
            _chart.ShouldSatisfyAllConditions(
                () => _chart.ChartAreas[0].Name.ShouldBe(_property),
                () => _chart.ChartAreas[0].AxisX.Title.ShouldBe(DateKey),
                () => _chart.ChartAreas[0].AxisY.Title.ShouldBe(PageViewsKey),
                () => _chart.ChartAreas[0].AxisX.MajorGrid.Enabled.ShouldBe(false),
                () => _chart.ChartAreas[0].AxisY.MajorGrid.Enabled.ShouldBe(false),
                () => _chart.ChartAreas[0].AxisX.Interval.ShouldBe(AxisXInterval),
                () => _chart.ChartAreas[0].BackColor.ShouldBe(Color.Transparent),
                () => _chart.ChartAreas[0].ShadowColor.ShouldBe(Color.Transparent),
                () => _chart.ChartAreas[0].AxisX.MajorGrid.LineColor.ShouldBe(Color.LightGray),
                () => _chart.ChartAreas[0].AxisY.MajorGrid.LineColor.ShouldBe(Color.LightGray));
        }

        private static void CreateLegendForChart(Chart chart, string legendName, string chartName)
        {
            if (string.IsNullOrWhiteSpace(legendName))
            {
                return;
            }

            var knownLegend = new Legend(legendName)
            {
                Position = {Auto = true},
                Docking = Docking.Top,
                Alignment = StringAlignment.Near,
                LegendStyle = LegendStyle.Column,
                BackColor = Color.Transparent,
                DockedToChartArea = chartName,
                LegendItemOrder = LegendItemOrder.SameAsSeriesOrder
            };
            chart.Legends.Add(knownLegend);
        }

        private static void SetupShimsFor_ClientDropdownTest(
            int clientGroupId,
            string expectedHash,
            bool isUserAuthorised,
            bool isSysytemAdministrator)
        {
            const int clientId = 10;
            ShimBaseChannel.GetAll = () => new List<ECN_Framework_Entities.Accounts.BaseChannel>
            {
                new ECN_Framework_Entities.Accounts.BaseChannel()
                {
                    PlatformClientGroupID = clientGroupId
                }
            };

            ShimClient.AllInstances.SelectActiveForClientGroupLiteInt32 =
                (instance, _) => new List<Client> { new Client() { ClientID = clientId } };

            ShimFormsAuthentication.SetAuthCookieStringBoolean = (_, __) => { };
            ShimFormsAuthentication.EncryptFormsAuthenticationTicket = t =>
            {
                _authenticationUserData = t.UserData;
                return expectedHash;
            };

            ShimFormsAuthentication.SignOut = () => { };

            ShimCustomer.GetByClientIDInt32Boolean = (_, __) => { return new ECN_Framework_Entities.Accounts.Customer(); };

            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User();
            session.Instance.CurrentUserClientGroupClients = new List<Client>();
            session.ClientIDGet = () => 20;
            session.ClientGroupIDGet = () => clientGroupId;

            KM.Platform.Fakes.ShimUser.IsSystemAdministratorUser = (_) => isSysytemAdministrator;
            if (isUserAuthorised)
            {
                session.Instance.CurrentUser.UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
                {
                    new UserClientSecurityGroupMap() { ClientID =  clientId},
                    new UserClientSecurityGroupMap() { ClientID =  session.Instance.ClientID},
                };
            }

            ShimECNSession.CurrentSession = () => session;
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection() { { "Communicator_VirtualPath", string.Empty } };
        }
    }
}
