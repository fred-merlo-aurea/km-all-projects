using System;
using ControlCenter.com.ecn5.webservices.ListManager.Fakes;
using ControlCenter.Interfaces;
using FrameworkServices;
using FrameworkServices.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Telerik.Windows.Controls;
using UAS.UnitTests.ControlCenter.Common;
using UAS_WS.Interface;
using FUasObject = FrameworkUAS.Object;
using ProductCreation = ControlCenter.Controls.ProductCreation;
using ShimAccountManager = ControlCenter.com.ecn5.webservices.AccountManager.Fakes.ShimAccountManager;
using ShimConfigurationManager = System.Configuration.Fakes.ShimConfigurationManager;
using ShimWorker = ControlCenter.Controls.Fakes.ShimProductCreation;

namespace UAS.UnitTests.ControlCenter.Controls
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class ProductCreationTest : Fakes
    {
        private const string BaseChannelValuesXml = "<root><BaseChannel><ID>1</ID><Name>TestCustomer</Name></BaseChannel></root>";
        private const string CustomersXml = "<root><Customer><ID>1</ID><Name>TestCustomer</Name></Customer></root>";
        private const string GroupsXml = "<root><Group><ID>1</ID><Name>TestCustomer</Name></Group></root>";
        private const string CustomersComboBoxName = "cbCustomers";
        private const string BaseComboBoxName = "cbBase";
        private const string UasMasterAccessKeyName = "UASMasterAccessKey";
        private const string UasMasterAccessKeyValue = "";

        private Mock<ServiceClient<IApplicationLog>> _serviceClientMock;

        [SetUp]
        public void Setup()
        {
            _serviceClientMock = new Mock<ServiceClient<IApplicationLog>>
            {
                CallBase = true
            };
        }

        [Test]
        public void CbCustomers_SelectionChanged_DataXmlNotNull_SetsValuesInComboBox()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
                {
                    { UasMasterAccessKeyName, UasMasterAccessKeyValue }
                };
                SetupShimForProductCreationClass(GroupsXml);
                var customersComboBox = InitializeCustomerRadComboBoxWithOneRecord(CustomersComboBoxName);

                // Act
                customersComboBox.SelectedIndex = 0;

                // Assert
                var newCustomerCount = GetCustomersCountFromComboBox(ref customersComboBox);
                newCustomerCount.ShouldNotBeNull();
                newCustomerCount.ShouldBeGreaterThan(0);
                ((KeyValuePair<int, string>)customersComboBox.SelectedItem).Key.ShouldBe(100);
                ((KeyValuePair<int, string>)customersComboBox.SelectedItem).Value.ShouldBe("Default Customer");
            }
        }

        [Test]
        public void CbBase_SelectionChanged_DataXmlNotNull_SetsValuesInComboBox()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
                {
                    { UasMasterAccessKeyName, UasMasterAccessKeyValue }
                };
                SetupShimForProductCreationClass(CustomersXml);
                var comboBox = InitializeCustomerRadComboBoxWithOneRecord(BaseComboBoxName);

                // Act
                comboBox.SelectedIndex = 0;

                // Assert
                var newCustomerCount = GetCustomersCountFromComboBox(ref comboBox);
                newCustomerCount.ShouldNotBeNull();
                newCustomerCount.ShouldBeGreaterThan(0);
                ((KeyValuePair<int, string>)comboBox.SelectedItem).Key.ShouldBe(100);
                ((KeyValuePair<int, string>)comboBox.SelectedItem).Value.ShouldBe("Default Customer");
            }
        }

        [Test]
        public void LoadBaseChannels_SelectionChanged_DataXmlNotNull_SetsValuesInComboBox()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
                {
                    { UasMasterAccessKeyName, UasMasterAccessKeyValue }
                };
                SetupShimForProductCreationClass(BaseChannelValuesXml);
                var comboBox = InitializeCustomerRadComboBoxWithOneRecord(BaseComboBoxName);

                // Act
                comboBox.SelectedIndex = 0;

                // Assert
                var newCustomerCount = GetCustomersCountFromComboBox(ref comboBox);
                newCustomerCount.ShouldNotBeNull();
                newCustomerCount.ShouldBeGreaterThan(0);
                ((KeyValuePair<int, string>)comboBox.SelectedItem).Key.ShouldBe(100);
                ((KeyValuePair<int, string>)comboBox.SelectedItem).Value.ShouldBe("Default Customer");
            }
        }

        private static void SetupShimForProductCreationClass(string customersXml)
        {
            ShimWorker.AllInstances.LoadData = creation => { };

            ShimServiceClient.UAD_ProductClient = () => null;
            ShimServiceClient.UAD_ProductGroupClient = () => null;
            ShimServiceClient.UAD_ProductTypesClient = () => null;
            ShimServiceClient.UAD_FrequencyClient = () => null;

            var appData = new FUasObject.AppData { AuthorizedUser = new FUasObject.UserAuthorization { User = new User { CurrentClient = new Client { ClientID = 1 } } } };
            FUasObject.AppData.myAppData = appData;

            ShimAccountManager.AllInstances.GetCustomers_InternalStringInt32 = (manager, s, arg3) => customersXml;
            ShimListManager.AllInstances.GetLists_InternalStringInt32 = (manager, s, arg3) => customersXml;
        }

        private static PrivateObject GetProductCreationObject()
        {
            var mock = new Mock<IAppSettingsProvider>();

            mock.Setup(provider
                    => provider.GetAppSettingsValue(It.IsAny<string>()))
                .Returns(String.Empty);
            return new PrivateObject(new ProductCreation(mock.Object));

        }

        private static RadComboBox InitializeCustomerRadComboBoxWithOneRecord(string comboBoxName)
        {
            GetCustomersRadComboBox(out var customersComboBox, comboBoxName);
            var customers = new Dictionary<int, string> { { 100, "Default Customer" } };
            customersComboBox.ItemsSource = customers;
            return customersComboBox;
        }

        private static void GetCustomersRadComboBox(out RadComboBox customersComboBox, string comboBoxName)
        {
            var productCreationObj = GetProductCreationObject();
            customersComboBox = (RadComboBox)productCreationObj.GetField(comboBoxName);
        }

        private static int GetCustomersCountFromComboBox(ref RadComboBox customersComboBox)
        {
            return customersComboBox.Items?.Count ?? 0;
        }
    }
}