using System;
using System.Collections.Generic;
using System.Fakes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Fakes;
using ControlCenter.Modules;
using ControlCenter.Modules.Fakes;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using FrameworkUAS.Service;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.DataForm;
using Telerik.Windows.Controls.Fakes;
using Telerik.Windows.Controls.GridView;
using UAS.UnitTests.Helpers;
using UAS_WS.Service.Fakes;
using RadDataForm = Telerik.Windows.Controls.RadDataForm;

namespace UAS.UnitTests.ControlCenter.Modules
{
    [Apartment(ApartmentState.STA)]
    [TestFixture]
    public class ClientManagementTest
    {
        private const string ClientChangeCheckMethod = "ClientChangeCheck";
        private const string OriginalClientProperty = "originalClient";
        private const string GridClientsField = "grdClients";
        private const string MyClientProperty = "myClient";
        private const string ClientListProperty = "clientList";
        private const string ClientName = "ClientName";
        private const string ClientCode = "ClientCode";
        private const string ClientDisplayName = "DisplayName";
        private const string ClientTestDbConnectionString = "ClientTestDBConnectionString";
        private const string ClientLiveDbConnectionString = "ClientLiveDBConnectionString";
        private const string ClientAccountManagerEmails = "AccountManagerEmails";
        private const string ClientEmails = "ClientEmails";
        private const string ClientIsActive = "IsActive";
        private const string ClientIgnoreUnknownFiles = "IgnoreUnknownFiles";
        private const string ClientHasPaid = "HasPaid";
        private const string ClientIskmClient = "IsKMClient";
        private const int UserId = 12345;
        private const int ClientId = 54321;
        private const string CurrentClientNameCannotBeBlank = "Current client name cannot be blank.";
        private const string CurrentDisplayNameCannotBeBlank = "Current display name cannot be blank.";
        private const string CurrentClientCodeCannotBeBlank = "Current client code cannot be blank.";
        private const string CurrentClientTestDbConnectionStringCannotBeBlank = "Current client test db connection string cannot be blank.";
        private const string CurrentClientLiveDbConnectionStringCannotBeBlank = "Current client live db connection string cannot be blank.";
        private const string CurrentAccountManagerEmailsCannotBeBlank = "Current account manager emails cannot be blank.";
        private const string CurrentClientNameHasBeenUsedPleaseProvideUniqueClientName = "Current client name has been used. Please provide a unique client name.";
        private const string CurrentDisplayNameHasBeenUsedPleaseProvideUniqueDisplayName = "Current display name has been used. Please provide a unique display name.";
        private const string CurrentClientCodeHasBeenUsedPleaseProvideUniqueClientCode = "Current client code has been used. Please provide a unique client code.";

        private static readonly string[] ClientPropertiesToCompare =
        {
            ClientName,
            ClientCode,
            ClientDisplayName,
            ClientTestDbConnectionString,
            ClientLiveDbConnectionString,
            ClientIsActive,
            ClientIgnoreUnknownFiles,
            ClientAccountManagerEmails,
            ClientEmails,
            ClientHasPaid,
            ClientIskmClient
        };

        private static readonly DateTime DateTimeNow = new DateTime(2018, 2, 08, 10, 10, 10);

        private static readonly Guid AuthAccessKey = new Guid("9325A0D0-E27E-47DE-9B27-C442071D0CD3");

        private IDisposable _context;

        private static IEnumerable<Tuple<string, string>> ClientPropertiesCheckedForEmpty
        { 
            get
            {
                yield return new Tuple<string, string>(ClientName, CurrentClientNameCannotBeBlank);
                yield return new Tuple<string, string>(ClientDisplayName, CurrentDisplayNameCannotBeBlank);
                yield return new Tuple<string, string>(ClientCode, CurrentClientCodeCannotBeBlank);
                yield return new Tuple<string, string>(ClientTestDbConnectionString, CurrentClientTestDbConnectionStringCannotBeBlank);
                yield return new Tuple<string, string>(ClientLiveDbConnectionString, CurrentClientLiveDbConnectionStringCannotBeBlank);
                yield return new Tuple<string, string>(ClientAccountManagerEmails, CurrentAccountManagerEmailsCannotBeBlank);
            }
        }

        private static IEnumerable<Tuple<string, string>> ClientPropertiesToCheckForChangesAgainstClientList
        { 
            get
            {
                yield return new Tuple<string, string>(ClientName, CurrentClientNameHasBeenUsedPleaseProvideUniqueClientName);
                yield return new Tuple<string, string>(ClientDisplayName, CurrentDisplayNameHasBeenUsedPleaseProvideUniqueDisplayName);
                yield return new Tuple<string, string>(ClientCode, CurrentClientCodeHasBeenUsedPleaseProvideUniqueClientCode);
            }
        }

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            ShimClientManagement.Constructor = (_) => { };
            ShimDateTime.NowGet = () => DateTimeNow;
            ShimAppData.myAppDataGet = () => new AppData();
            ShimAppData.AllInstances.AuthorizedUserGet = appData =>
            {
                    var userAuthorization = new UserAuthorization { AuthAccessKey = AuthAccessKey };

                    var user = new User { UserID = UserId };
                    userAuthorization.User = user;

                    return userAuthorization;
            };

            ShimClient.AllInstances.SaveGuidClient = (client, guid, entityClient) => new Response<int>(ClientId);
            ShimClientManagement.AllInstances.GetClientId = management => ClientId;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void ClientChangeCheck_InvalidSender_NoException()
        {
            // Arrange
            var clientManagement = new ClientManagement();
            var invalidSender = new StringBuilder();

            // Act, Assert
            CallClientChangeCheck(clientManagement, invalidSender);
        }

        [Test]
        public void ClientChangeCheck_ClientEqualsOriginalClient_HasNoChanges()
        {
            // Arrange
            var clientManagement = CreateClientManagement();
            var radDataFormMock = GetRadDataFormMock();
            radDataFormMock.Object.CurrentItem = CreateClient();

            var counterRebindCalled = 0;
            ShimDataControl.AllInstances.Rebind = (_) =>
            {
                counterRebindCalled++;
            };

            // Act
            CallClientChangeCheck(clientManagement, radDataFormMock.Object);

            // Assert
            clientManagement.ShouldSatisfyAllConditions(
                () => counterRebindCalled.ShouldBe(1),
                () => clientManagement.GetPropertyValueAs<Client>(MyClientProperty).DateUpdated.HasValue.ShouldBeFalse());
        }

        [Test]
        [TestCaseSource(nameof(ClientPropertiesToCompare))]
        public void ClientChangeCheck_ClientHasOneChangedProperty_HasChanges(string propertyToCompare)
        {
            // Arrange
            var clientManagement = CreateClientManagement();
            var radDataFormMock = GetRadDataFormMock();
            var currentClient = CreateClient();

            var currentValue = currentClient.GetPropertyValue(propertyToCompare);
            object changedValue = null;
            var actualSelectedItem = new object();

            // there are only bool and string properties
            if (currentValue is bool)
            {
                changedValue = !(bool)currentValue;
            }
            else if (currentValue is string)
            {
                changedValue = new string(((string)currentValue).Reverse().ToArray());
            }

            currentClient.SetProperty(propertyToCompare, changedValue);
            radDataFormMock.Object.CurrentItem = currentClient;

            var counterRebindCalled = 0;
            ShimDataControl.AllInstances.Rebind = (_) =>
            {
                counterRebindCalled++;
            };

            ShimDataControl.AllInstances.SelectedItemSetObject = (control, item) =>
            {
                actualSelectedItem = item;
            };

            // Act
            CallClientChangeCheck(clientManagement, radDataFormMock.Object);
            var myClient = clientManagement.GetPropertyValueAs<Client>(MyClientProperty);
            var gridClients = clientManagement.GetFieldValueAs<RadGridView>(GridClientsField);

            // Assert
            clientManagement.ShouldSatisfyAllConditions(
                () => counterRebindCalled.ShouldBe(0),
                () => myClient.UpdatedByUserID.ShouldBe(UserId),
                () => myClient.DateUpdated.ShouldBe(DateTimeNow),
                () => myClient.ClientID.ShouldBe(ClientId),
                () => actualSelectedItem.ShouldBeNull(),
                () => gridClients.RowDetailsVisibilityMode.ShouldBe(GridViewRowDetailsVisibilityMode.Collapsed));
        }

        [Test]
        [TestCaseSource(nameof(ClientPropertiesToCheckForChangesAgainstClientList))]
        public void ClientChangeCheck_ClientIsInClientList_MessageBoxShown(Tuple<string, string> property)
        {
            // Arrange
            var clientManagement = CreateClientManagement();
            var radDataFormMock = GetRadDataFormMock();
            var firstClient = CreateClient();
            string actualMessage = null;

            radDataFormMock.Object.CurrentItem = CreateClient();

            firstClient.SetProperty(property.Item1, string.Empty);
            var secondClient = CreateClient();
            secondClient.ClientID = firstClient.ClientID * -1;

            clientManagement.SetProperty(ClientListProperty, new List<Client> { firstClient, secondClient });

            var counterRebindCalled = 0;
            ShimDataControl.AllInstances.Rebind = (_) =>
            {
                counterRebindCalled++;
            };

            ShimMessageBox.ShowStringStringMessageBoxButtonMessageBoxImage = (messageBoxText, caption, button, icon) =>
            {
                actualMessage = messageBoxText;
                return MessageBoxResult.OK;
            };

            // Act
            CallClientChangeCheck(clientManagement, radDataFormMock.Object);

            // Assert
            clientManagement.ShouldSatisfyAllConditions(
                () => counterRebindCalled.ShouldBe(1),
                () => actualMessage.ShouldBe(property.Item2));
        }

        [Test]
        [TestCaseSource(nameof(ClientPropertiesCheckedForEmpty))]
        public void ClientChangeCheck_ClientHasEmptyProperty_MessageBoxShown(Tuple<string, string> property)
        {
            // Arrange
            var clientManagement = CreateClientManagement();
            var radDataFormMock = GetRadDataFormMock();
            var currentClient = CreateClient();
            string actualMessage = null;

            currentClient.SetProperty(property.Item1, string.Empty);
            radDataFormMock.Object.CurrentItem = currentClient;

            var counterRebindCalled = 0;
            ShimDataControl.AllInstances.Rebind = (_) =>
            {
                counterRebindCalled++;
            };

            ShimMessageBox.ShowStringStringMessageBoxButtonMessageBoxImage = (messageBoxText, caption, button, icon) =>
            {
                actualMessage = messageBoxText;
                return MessageBoxResult.OK;
            };

            // Act
            CallClientChangeCheck(clientManagement, radDataFormMock.Object);

            // Assert
            clientManagement.ShouldSatisfyAllConditions(
                () => counterRebindCalled.ShouldBe(1),
                () => actualMessage.ShouldBe(property.Item2));
        }

        private static ClientManagement CreateClientManagement()
        {
            var clientManagement = new ClientManagement();

            var dataControl = new Mock<RadGridView>();
            dataControl.SetupAllProperties();
            dataControl.Object.RowDetailsVisibilityMode = GridViewRowDetailsVisibilityMode.Visible;

            clientManagement.SetField(GridClientsField, dataControl.Object);

            var originalClient = CreateClient();
            clientManagement.SetProperty(OriginalClientProperty, originalClient);

            return clientManagement;
        }

        private static Client CreateClient()
        {
            var client = new Client
            {
                ClientName = ClientName,
                ClientCode = ClientCode,
                DisplayName = ClientDisplayName,
                ClientTestDBConnectionString = ClientTestDbConnectionString,
                ClientLiveDBConnectionString = ClientLiveDbConnectionString,
                IsActive = false,
                IgnoreUnknownFiles = true,
                AccountManagerEmails = ClientAccountManagerEmails,
                ClientEmails = ClientEmails,
                HasPaid = true,
                IsKMClient = true,
                ClientID = ClientId
            };

            return client;
        }

        private static Mock<RadDataForm> GetRadDataFormMock()
        {
            return typeof(RadDataForm).GetMock();
        }

        private static void CallClientChangeCheck(object instance, object sender)
        {
            typeof(ClientManagement).CallMethod(
                    ClientChangeCheckMethod, 
                    new[] { sender, new EditEndedEventArgs(EditAction.Cancel) },
                    instance);
        }
    }
}
