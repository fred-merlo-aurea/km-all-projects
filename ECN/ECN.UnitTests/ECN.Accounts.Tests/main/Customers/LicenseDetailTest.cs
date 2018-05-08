using System;
using System.Data;
using System.Web.UI.WebControls;
using ecn.accounts.customersmanager;
using Ecn.Accounts.Interfaces;
using ECN.Tests.Helpers;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Shouldly;

namespace ECN.Accounts.Tests.Main.Customers
{
    [TestFixture]
    public class LicenseDetailTest
    {
        private const int LicenseIdValue = 0;
        private const string StringEndsWithCommunicator = "communicator)";
        private const string StringEndsWithCollector = "collector)";
        private const string StringEndsWithCreator = "creator)";
        private licensedetail _license;
        private Mock<IDataFunctions> _dataFunctions;

        [SetUp]
        public void SetUp()
        {
            _dataFunctions = new Mock<IDataFunctions>();
            _dataFunctions.Setup(x => x.GetDataTable(It.IsAny<string>(), It.IsAny<string>())).Returns(new DataTable());

            _license = new licensedetail(_dataFunctions.Object);
        }

        [Test]
        public void LoadCustomersDropdown_EmptyChannelName_DoesNotSetColumnIDDataSource()
        {
            // Arrange
            InitializeDropDowns(string.Empty);

            // Act
            _license.LoadCustomersDropdown();

            // Assert
            AssertCustomerDropDown(Is.Null);
            _dataFunctions.Verify(x => x.GetDataTable(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void LoadCustomersDropdown_ChannelNameEndsWithCommunicator_FiltersCustomerIDWithCommunicatorChannelID()
        {
            // Arrange
            InitializeDropDowns(StringEndsWithCommunicator);

            // Act
            _license.LoadCustomersDropdown();

            // Assert
            var whereStatement = string.Format(" WHERE CommunicatorChannelID= {0}",  StringEndsWithCommunicator);
            var sqlQuery = GetCustomerByChannelIdQuery(whereStatement);
            AssertCustomerDropDown(Is.Not.Null);
            _dataFunctions.Verify(x => x.GetDataTable(sqlQuery, It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void LoadCustomersDropdown_ChannelNameEndsWithCollector_FiltersCustomerIDWithCollectorChannelID()
        {
            // Arrange
            InitializeDropDowns(StringEndsWithCollector);

            // Act
            _license.LoadCustomersDropdown();

            // Assert
            var whereStatement = string.Format(" WHERE CollectorChannelID= {0}", StringEndsWithCollector);
            var sqlQuery = GetCustomerByChannelIdQuery(whereStatement);
            AssertCustomerDropDown(Is.Not.Null);
            _dataFunctions.Verify(x => x.GetDataTable(sqlQuery, It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void LoadCustomersDropdown_ChannelNameEndsWithCreator_FiltersCustomerIDWithCreatorChannelID()
        {
            // Arrange
            InitializeDropDowns(StringEndsWithCreator);

            // Act
            _license.LoadCustomersDropdown();

            // Assert
            var whereStatement = string.Format(" WHERE CreatorChannelID= {0}", StringEndsWithCreator);
            var sqlQuery = GetCustomerByChannelIdQuery(whereStatement);
            AssertCustomerDropDown(Is.Not.Null);
            _dataFunctions.Verify(x => x.GetDataTable(sqlQuery, It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void LoadCustomersDropdownEventCallbackOverload_EmptyChannelName_DoesNotSetColumnIDDataSource()
        {
            // Arrange
            InitializeDropDowns(string.Empty);

            // Act
            Should.Throw<InvalidOperationException>(() => _license.LoadCustomersDropdown(null, null));

            // Assert
            AssertCustomerDropDown(Is.Null);
            var sqlQuery = GetCustomerByLicenseID(LicenseIdValue);
            _dataFunctions.Verify(x => x.GetDataTable(sqlQuery, It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void LoadCustomersDropdownEventCallbackOverload_ChannelNameEndsWithCommunicator_FiltersCustomerIDWithCommunicatorChannelID()
        {
            // Arrange
            InitializeDropDowns(StringEndsWithCommunicator);

            // Act
            Should.Throw<InvalidOperationException>(() => _license.LoadCustomersDropdown(null, null));

            // Assert
            var sqlQuery = GetCustomerByLicenseID(LicenseIdValue);
            AssertCustomerDropDown(Is.Not.Null);
            _dataFunctions.Verify(x => x.GetDataTable(sqlQuery, It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void LoadCustomersDropdownEventCallbackOverload_ChannelNameEndsWithCollector_FiltersCustomerIDWithCollectorChannelID()
        {
            // Arrange
            InitializeDropDowns(StringEndsWithCollector);

            // Act
            Should.Throw<InvalidOperationException>(() => _license.LoadCustomersDropdown(null, null));

            // Assert
            var sqlQuery = GetCustomerByLicenseID(LicenseIdValue);
            AssertCustomerDropDown(Is.Not.Null);
            _dataFunctions.Verify(x => x.GetDataTable(sqlQuery, It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void LoadCustomersDropdownEventCallbackOverload_ChannelNameEndsWithCreator_FiltersCustomerIDWithCreatorChannelID()
        {
            // Arrange
            InitializeDropDowns(StringEndsWithCreator);

            // Act
            Should.Throw<InvalidOperationException>(() => _license.LoadCustomersDropdown(null, null));

            // Assert
            var sqlQuery = GetCustomerByLicenseID(LicenseIdValue);
            AssertCustomerDropDown(Is.Not.Null);
            _dataFunctions.Verify(x => x.GetDataTable(sqlQuery, It.IsAny<string>()), Times.Once());
        }

        private string GetCustomerByLicenseID(int customerLicenseID)
        {
            return string.Format(
                "{0}{1}{2}{3}{4}",
                " SELECT * ",
                " FROM [CustomerLicense] ",
                " WHERE CLID=",
                customerLicenseID,
                " and IsDeleted = 0 ");
        }

        private string GetCustomerByChannelIdQuery(string whereStatement)
        {
            return string.Format(
                "{0}{1}{2}{3}",
                " SELECT CustomerID, CustomerName, CreateDate, ActiveFlag ",
                " FROM [Customer] ",
                whereStatement,
                " AND ActiveFlag = 'Y' and IsDeleted = 0 ORDER BY CustomerName ");
        }

        private void AssertCustomerDropDown(NullConstraint constraint)
        {
            var customerDropdown = ReflectionHelper.GetFieldInfoFromInstanceByName(_license, "CustomerID")
                .GetValue(_license) 
                as DropDownList;

            Assert.That(customerDropdown, Is.Not.Null);
            Assert.That(customerDropdown.DataSource, constraint);
        }

        private void InitializeDropDowns(string firstItem)
        {
            var dropDownList = new DropDownList();
            dropDownList.Items.Add(new ListItem(firstItem, firstItem));
            dropDownList.SelectedIndex = 0;
            ReflectionHelper.SetValue(_license, "ChannelList", dropDownList);
            ReflectionHelper.SetValue(_license, "CustomerID", new DropDownList());
        }
    }
}
