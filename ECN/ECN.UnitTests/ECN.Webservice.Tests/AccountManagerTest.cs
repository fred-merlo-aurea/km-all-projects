using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ecn.common.classes.Fakes;
using ecn.webservice;
using ecn.webservice.classes.Fakes;
using ECN_Framework.Common.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using BusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;
using EcnCommonClasses = ecn.common.classes;
using EcnDataLayerFakes = ECN_Framework_DataLayer.Accounts.Fakes;
using KmCommonFakes = KM.Common.Fakes;
using KmEntity = KM.Common.Entity;
using KmEntityFakes = KM.Common.Entity.Fakes;

namespace ECN.Webservice.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AccountManagerTest
    {
        private AccountManager _accountManager;
        private DataTable _dataTable;
        private IDisposable _shims;
        private const string Name = "name";
        private const string Key = "key";
        private const string Details = "details";
        private const string StringId = "10";
        private const string AccountsOptions = "1";
        private const string Response = "Response";
        private const string Url = "details?details";
        private const string Value = "&add";
        private const string UasMasterAccessKey = "UASMasterAccessKey";
        private const string Application = "KMCommon_Application";
        private const string Act = "act";
        private const string Com = "com";
        private const int Id = 1;
        private const int Zero = 0;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
            _dataTable?.Dispose();
        }

        [Test]
        public void AddCustomer_ForCustomerIdZero_SaveCustomer()
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeCustomer();

            // Act
            var result = _accountManager.AddCustomer(Key, Name, true, Details, Details, Details, Details, Details,
                Details, Details, Details, Details, Details, Details, Details, Details, Details,
                Details, Details, Details, Details, Details, Details, Details);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public void UpdateCustomer_ForCustomerId_UpdateCustomer(bool param1, bool param2)
        {
            // Arrange
            InitializeCustomer();
            InitializeAuthenticationHandler(param1, param2);
            ShimSecurityAccess.hasAccessStringString = (x, y) => true;
            _accountManager = new AccountManager();

            // Act
            var result = _accountManager.UpdateCustomer(Details, Id, Details, false, Details, Details, Details, Details, Details,
                Details, Details, Details, Details, Details, Details, Details, Details, Details, Details, Details, Details,
                Details, Details, Details, Details);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public void GetCustomers_ForSystemAdminAndUser_ReturnResponse(bool param1, bool param2)
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeGetCustomer(param1, param2);

            // Act
            var result = _accountManager.GetCustomers(Details);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public void GetCustomerbyId_ForSystemAdminAndUser_ReturnResponse(bool param1, bool param2)
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeGetCustomer(param1, param2);

            // Act
            var result = _accountManager.GetCustomerbyID(Details, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public void AddUser_ForSystemAdmin_ReturnResponse(bool param1, bool param2)
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeUser(param1, param2);

            // Act
            var result = _accountManager.AddUser(Details, Id, Details, Details, true);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public void UpdateUser_ForUserIdAndSystemAdmin_ReturnResponse(bool param1, bool param2)
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeUser(param1, param2);

            // Act
            var result = _accountManager.UpdateUser(Details, Id, Id, Details, Details, true);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        public void UpdateUser_ForUserIdZero_ReturnResponse()
        {
            // Arrange
            _accountManager = new AccountManager();
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Response;

            // Act
            var result = _accountManager.UpdateUser(Details, Id, Zero, Details, Details, true);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(false, false, true)]
        public void GetUsers_ForSystemAdminAndUser_ReturnResponse(bool param1, bool param2, bool param3)
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeGetCustomer(param1, param2);
            ShimSecurityAccess.hasAccessStringString = (x, y) => param3;

            // Act
            var result = _accountManager.GetUsers(Details, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(false, false, true)]
        public void GetUserbyID_ForSystemAdminAndUser_ReturnResponse(bool param1, bool param2, bool param3)
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeGetCustomer(param1, param2);
            ShimSecurityAccess.hasAccessStringString = (x, y) => param3;

            // Act
            var result = _accountManager.GetUserbyID(Details, Id, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true, Details)]
        [TestCase(true, true, "")]
        [TestCase(true, false, Details)]
        [TestCase(false, false, Details)]
        public void GetLogin_ForSystemAdminAndUser_ReturnUrl(bool param1, bool param2, string param3)
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeGetCustomer(param1, param2);
            InitializeLogin(param3);
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Url;

            // Act
            var result = _accountManager.GetLogin(Details, Details, Id, Value);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Url));
        }

        [Test]
        [TestCase(true, true, Details)]
        [TestCase(true, true, "")]
        [TestCase(true, false, Details)]
        [TestCase(false, false, Details)]
        public void GetBaseChanelsInternalForSystemAdminAndUser_ReturnResponse(bool param1, bool param2, string param3)
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeBaseChannel(param1, param2, param3);

            // Act
            var result = _accountManager.GetBaseChanels_Internal(Details);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true, Details)]
        [TestCase(true, true, "")]
        [TestCase(true, false, Details)]
        [TestCase(false, false, Details)]
        public void GetCustomersInternalForSystemAdminAndUser_ReturnResponse(bool param1, bool param2, string param3)
        {
            // Arrange
            _accountManager = new AccountManager();
            InitializeBaseChannel(param1, param2, param3);

            // Act
            var result = _accountManager.GetCustomers_Internal(Details, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        private void InitializeBaseChannel(bool param1, bool param2, string param3)
        {
            InitializeAuthenticationHandler(param1, param2);
            var collection = new NameValueCollection()
            {
                {UasMasterAccessKey, param3.ToUpper() },
                { Application, StringId}
            };
            ShimConfigurationManager.AppSettingsGet = () => collection;
            EcnDataLayerFakes.ShimBaseChannel.GetAll = () => new List<BaseChannel>
            {
                new BaseChannel()
                {
                    BaseChannelID = Id,
                    BaseChannelName = Details
                }
            };
            EcnDataLayerFakes.ShimCustomer.GetByBaseChannelIDInt32 = (x) => new List<Customer>
            {
                new Customer()
                {
                    CustomerID = Id,
                    CustomerName = Details
                }
            };
        }

        private void InitializeLogin(string param)
        {
            var collection = new NameValueCollection()
            {
                {UasMasterAccessKey, param.ToUpper() },
                { Application, StringId}
            };
            ShimConfigurationManager.AppSettingsGet = () => collection;
            BusinessLogicFakes.ShimUser.GetByUserIDInt32Boolean = (x, y) => new User()
            {
                UserName = Details,
                Password = Details
            };
            KmEntityFakes.ShimEncryption.GetCurrentByApplicationIDInt32 = (x) => new KmEntity.Encryption();
            KmCommonFakes.ShimEncryption.EncryptStringEncryption = (x, y) => Details;
        }

        private void InitializeUser(bool param1, bool param2)
        {
            InitializeAuthenticationHandler(param1, param2);
            ShimSecurityAccess.hasAccessStringString = (x, y) => true;
            ShimCustomer.GetCustomerByIDInt32 = (x) => new EcnCommonClasses.Customer();
            ShimUser.AllInstances.Save = (x) => new EcnCommonClasses.User();
        }

        private void InitializeGetCustomer(bool param1, bool param2)
        {
            InitializeAuthenticationHandler(param1, param2);
            var collection = new NameValueCollection()
            {
                {Com, Act },
                {Act, Act }
            };
            ShimConfigurationManager.AppSettingsGet = () => collection;
            ShimSQLHelper.executeXmlReaderStringStringString = (x, y, z) => Response;
        }

        private void InitializeAuthenticationHandler(bool param1, bool param2)
        {
            ShimAuthenticationHandler.AllInstances.authenticateUser = (x) => param1;
            ShimAuthenticationHandler.AllInstances.isSysAdmin = (x) => param2;
            ShimAuthenticationHandler.AllInstances.isChannelAdmin = (x) => param2;
            ShimAuthenticationHandler.AllInstances.isAdmin = (x) => param2;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Response;
        }

        private void InitializeCustomer()
        {
            BusinessLogicFakes.ShimUser.ECN_GetByAccessKeyStringBoolean = (x, y) => new User() { UserID = Id };
            var collection = new NameValueCollection()
            {
                {Act, Act }
            };
            ShimConfigurationManager.AppSettingsGet = () => collection;
            _dataTable = new DataTable()
            {
                Columns = { "BaseChannelID", "CustomerID", "UserID", "AccountsOptions", "ProductDetailID" },
                Rows = { { StringId, StringId, StringId, AccountsOptions, StringId } }
            };
            ShimDataFunctions.GetDataTableStringString = (x, y) => _dataTable;
            ShimDataFunctions.ExecuteScalarStringString = (x, y) => true;
            ShimDataFunctions.ExecuteStringString = (x, y) => Id;
            ShimCustomer.GetCustomerByIDInt32 = (x) => new EcnCommonClasses.Customer()
            {
                SubscriptionsEmail = Details,
                CreatorLevel = Id
            };
            ShimAuthenticationHandler.AllInstances.baseChannelIDGet = (x) => Id;
            ShimCustomer.AllInstances.SaveInt32 = (x, y) => new EcnCommonClasses.Customer();
            ShimCustomer.AllInstances.CreateDefaultFeaturesInt32 = (x, y) => new EcnCommonClasses.Customer();
            ShimCustomer.AllInstances.CreateDefaulRoleInt32 = (x, y) => new EcnCommonClasses.Customer();
            ShimSQLHelper.executeStringString = (x, y) => Id;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Response;
        }
    }
}