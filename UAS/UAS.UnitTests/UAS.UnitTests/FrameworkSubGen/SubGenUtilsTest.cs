using System;
using System.Collections.Generic;
using FrameworkSubGen;
using FrameworkSubGen.Entity;
using FrameworkSubGen.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using BusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;
using BusinessLogicAPIFakes = FrameworkSubGen.BusinessLogic.API.Fakes;
using KMP = KMPlatform.Entity;
using SuG = FrameworkSubGen.Entity;

namespace UAS.UnitTests.FrameworkSubGen
{
	/// <summary>
	///     Unit Tests for <see cref="SubGenUtils.GetLoginToken"/>
	/// </summary>
	[TestFixture]
	public class SubGenUtilsTest
	{
		private IDisposable _context;
		private List<Account> _accounts;
		private Enums.Client _client;
		private KMP.User _user;
		private SubGenUtils _sgu;
		private SubGenUserMap _sgum;

		[SetUp]
		public void SetUp()
		{
			_context = ShimsContext.Create();
			_accounts = new List<Account> { new Account() };
			_client = new Enums.Client();
			_user = typeof(KMP.User).CreateInstance();
			_user.CurrentClient = typeof(Client).CreateInstance();
			_sgum = typeof(SubGenUserMap).CreateInstance();
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

		[Test]
		public void GetLoginToken_WhenSubGenUtilsAccountsListIsNull_CallSetAccountsAndFillTheList()
		{
			// Arrange
			Initialize();
			_user.CurrentClient.ClientID = 1;
			_sgu = new SubGenUtils();
			_sgu.SetField("accounts", null);

			// Act
			_sgu.GetLoginToken(_client, _user, true);
			var result = _sgu.GetFieldValue("accounts");

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBe(_accounts);
		}

		[Test]
		public void GetLoginToken_WhenSubGenUserIdAndNameExists_ReturnApiToken()
		{
			// Arrange
			Initialize();
			_user.CurrentClient.ClientID = 1;
			_user.EmailAddress = "master@test.com";
			_sgu = new SubGenUtils();
			_sgu.SetField("accounts", null);

			// Act
			string result = _sgu.GetLoginToken(_client, _user, true);

			// Assert
			result.ShouldNotBeNullOrWhiteSpace();
		}

		[Test]
		public void GetLoginToken_WhenSubGenUserIdAndNameNotExists_GetSubGenUserUpdateAndSaveKmUserReturnApiToken()
		{
			// Arrange
			Initialize();

			_accounts.ForEach(x =>
			{
				x.company_name = "Master";
				x.account_id = 123;
			});
			var kmUserUpdated = false;
			BusinessLogicFakes.ShimSubGenUserMap.AllInstances.SaveSubGenUserMap = (item, x) =>
			{
				kmUserUpdated = true;
				return true;
			};
			_user.CurrentClient.ClientID = 454;
			_user.EmailAddress = "master@test.com";
			_sgu = new SubGenUtils();
			_sgu.SetField("accounts", null);

			// Act
			string result = _sgu.GetLoginToken(_client, _user, true);

			// Assert
			kmUserUpdated.ShouldBeTrue();
			result.ShouldNotBeNullOrWhiteSpace();
		}

		[Test]
		public void GetLoginToken_WhenUserNotExists_GetSubGenUserUpdateAndSaveKmUserReturnApiToken()
		{
			// Arrange
			Initialize();
			_accounts.ForEach(x =>
			{
				x.company_name = "Master";
				x.account_id = 123;
			});
			var newUserCreated = false;
			var newUserSaved = false;
			BusinessLogicAPIFakes.ShimUser.AllInstances.GetUsersEnumsClientBooleanInt32StringStringStringString = (item, a, b, c, d, e, f, g) =>
			new List<SuG.User>
			{
				new SuG.User
				{
					email = "new_user@test.com",
					user_id = 22
				}
			};
			BusinessLogicFakes.ShimSubGenUserMap.AllInstances.SaveSubGenUserMap = (item, x) =>
			{
				newUserSaved = true;
				return true;
			};
			BusinessLogicAPIFakes.ShimUser.AllInstances.CreateEnumsClientUser = (item, a, b) =>
			{
				newUserCreated = true;
				return 1;
			};
			_user.CurrentClient.ClientID = 454;
			_user.EmailAddress = "master@test.com";
			_sgu = new SubGenUtils();

			// Act
			string result = _sgu.GetLoginToken(_client, _user, true);

			// Assert
			newUserCreated.ShouldBeTrue();
			newUserSaved.ShouldBeTrue();
			result.ShouldNotBeNullOrWhiteSpace();
		}

		[Test]
		public void GetLoginToken_WhenInvalidDataGiven_ThrowsException()
		{
			// Arrange
			ShimSubGenUtils.AllInstances.SetAccounts = item => { item.SetField("accounts", _accounts); };
			ShimSubGenUtils.AllInstances.GetAccounts = item => _accounts;
			SubGenUserMap sgum = typeof(SubGenUserMap).CreateInstance();
			sgum.ClientID = 1;
			sgum.UserID = 1;
			sgum.SubGenUserId = 1;
			var sgumList = new List<SubGenUserMap> { sgum };
			sgum.SubGenAccountName = "Master";
			BusinessLogicFakes.ShimSubGenUserMap.AllInstances.SelectInt32 = (item, x) => sgumList;
			var client = new Enums.Client();
			KMP.User user = typeof(KMP.User).CreateInstance();
			user.CurrentClient = typeof(Client).CreateInstance();
			user.CurrentClient.ClientID = 1;
			user.EmailAddress = "master@test.com";
			var sgu = new SubGenUtils();
			sgu.SetField("accounts", null);

			//Act , Assert
			Assert.That(() =>
					sgu.GetLoginToken(client, user, true),
				Throws.TypeOf<ArgumentException>());
		}

		[Test]
		public void GetTestingLoginToken_WhenSubGenUtilsAccountsListIsNull_CallSetAccountsAndFillTheList()
		{
			// Arrange
			InitializeForGetTestingLogin();
			var setAccountsCalled = false;
			_user.CurrentClient.ClientID = 678;
			_sgu = typeof(SubGenUtils).CreateInstance();
			_sgu.SetField("accounts", null);
			ShimSubGenUtils.AllInstances.SetAccounts = item =>
			{
				setAccountsCalled = true;
				item.SetField("accounts", _accounts);
			};
			_accounts.Clear();
			_accounts = new List<Account> {
				new Account()
				{
					company_name = "KM API Testing"
				}
			};

			// Act
			string token = _sgu.GetTestingLoginToken(_user, false);
			var accountsList = _sgu.GetFieldValue("accounts");

			// Assert
			setAccountsCalled.ShouldBeTrue();
			accountsList.ShouldNotBeNull();
			accountsList.ShouldBe(_accounts);
			token.ShouldNotBeNullOrWhiteSpace();
		}

		[Test]
		public void GetTestingLoginToken_WhenSubGenUserIdAndNameExists_ReturnApiToken()
		{
			// Arrange
			InitializeForGetTestingLogin();
			_sgu = typeof(SubGenUtils).CreateInstance();
			_user.CurrentClient.ClientID = _sgum.ClientID;
			_user.UserName = _sgum.SubGenAccountName;
			_sgu = new SubGenUtils();
			_sgu.SetField("accounts", _accounts);

			// Act
			string result = _sgu.GetTestingLoginToken(_user, false);

			// Assert
			result.ShouldNotBeNullOrWhiteSpace();
		}

		[Test]
		public void GetTestingLoginToken_WhenSubGenUserIdAndNameNotExists_GetSubGenUserUpdateAndSaveKmUserReturnApiToken()
		{
			// Arrange
			var kmUserUpdatedAndSaved = false;
			_user.EmailAddress = "km_api_testing@test.com";
			InitializeForGetTestingLogin();
			_sgu = typeof(SubGenUtils).CreateInstance();
			_sgu = new SubGenUtils();
			_sgu.SetField("accounts", _accounts);
			BusinessLogicFakes.ShimSubGenUserMap.AllInstances.SelectInt32 = (item, x) => new List<SubGenUserMap>();
			BusinessLogicFakes.ShimSubGenUserMap.AllInstances.SaveSubGenUserMap = (item, x) =>
			{
				kmUserUpdatedAndSaved = true;
				return true;
			};

			// Act
			string result = _sgu.GetTestingLoginToken(_user, true);

			// Assert
			kmUserUpdatedAndSaved.ShouldBeTrue();
			result.ShouldNotBeNullOrWhiteSpace();
		}

		[Test]
		public void GetTestingLoginToken_WhenUserNotExists_GetSubGenUserUpdateAndSaveKmUserReturnApiToken()
		{
			// Arrange
			var newUserCreated = false;
			var newUserSaved = false;
			_user.EmailAddress = "other@test.com";
			InitializeForGetTestingLogin();
			_sgu = typeof(SubGenUtils).CreateInstance();
			_sgu = new SubGenUtils();
			_sgu.SetField("accounts", _accounts);
			BusinessLogicFakes.ShimSubGenUserMap.AllInstances.SelectInt32 = (item, x) => new List<SubGenUserMap>();
			BusinessLogicFakes.ShimSubGenUserMap.AllInstances.SaveSubGenUserMap = (item, x) =>
			{
				newUserSaved = true;
				return true;
			};
			BusinessLogicAPIFakes.ShimUser.AllInstances.CreateEnumsClientUser = (item, a, b) =>
			{
				newUserCreated = true;
				return 1;
			};

			// Act
			string result = _sgu.GetTestingLoginToken(_user, true);

			// Assert
			newUserCreated.ShouldBeTrue();
			newUserSaved.ShouldBeTrue();
			result.ShouldNotBeNullOrWhiteSpace();
		}

		[Test]
		public void GetTestingLoginToken_WhenInvalidDataGiven_ThrowsException()
		{
			// Arrange
			InitializeForGetTestingLogin();
			_sgu = typeof(SubGenUtils).CreateInstance();
			_user.CurrentClient.ClientID = _sgum.ClientID;
			_user.UserName = _sgum.SubGenAccountName;
			_sgu = new SubGenUtils();
			_sgu.SetField("accounts", _accounts);
			_user.CurrentClient = null;

			//Act , Assert
			Assert.That(() =>
					_sgu.GetTestingLoginToken(_user, true),
				Throws.TypeOf<ArgumentException>());
		}

		private void Initialize()
		{
			ShimSubGenUtils.AllInstances.SetAccounts = item => { item.SetField("accounts", _accounts); };
			ShimSubGenUtils.AllInstances.GetAccounts = item => _accounts;
			BusinessLogicAPIFakes.ShimUser.AllInstances.GetLoginTokenEnumsClientInt32Boolean = (item, x, userId, z) =>
			{
				if (userId > 0)
					return new global::FrameworkSubGen.Object.ApiLoginToken
					{
						api_login_token = "test_token"
					};

				return null;
			};
			BusinessLogicAPIFakes.ShimUser.AllInstances.GetUsersEnumsClientBooleanInt32StringStringStringString =
				(item, a, b, c, d, e, f, g) => new List<SuG.User>
				{
					new SuG.User
					{
						email = "master@test.com",
						user_id = 22
					}
				};
			SubGenUserMap sgum = typeof(SubGenUserMap).CreateInstance();
			sgum.ClientID = 1;
			sgum.UserID = 1;
			sgum.SubGenUserId = 1;
			var sgumList = new List<SubGenUserMap> { sgum };
			sgum.SubGenAccountName = "Master";
			BusinessLogicFakes.ShimSubGenUserMap.AllInstances.SelectInt32 = (item, x) => sgumList;
		}

		private void InitializeForGetTestingLogin()
		{
			ShimSubGenUtils.AllInstances.SetAccounts = item => { item.SetField("accounts", _accounts); };
			ShimSubGenUtils.AllInstances.GetAccounts = item => _accounts;
			BusinessLogicAPIFakes.ShimUser.AllInstances.GetLoginTokenEnumsClientInt32Boolean = (item, x, userId, z) =>
			{
				if (userId > 0)
					return new global::FrameworkSubGen.Object.ApiLoginToken
					{
						api_login_token = "test_token"
					};

				return null;
			};
			BusinessLogicAPIFakes.ShimUser.AllInstances.GetUsersEnumsClientBooleanInt32StringStringStringString =
				(item, a, b, c, d, e, f, g) => new List<SuG.User>
				{
					new SuG.User
					{
						email = "km_api_testing@test.com",
						user_id = 678
					}
				};
			BusinessLogicAPIFakes.ShimUser.AllInstances.CreateEnumsClientUser = (item, a, b) =>
			{
				return 1;
			};
			_sgum.ClientID = 678;
			_sgum.UserID = 789;
			_sgum.SubGenUserId = 123;
			_sgum.SubGenAccountName = "KM API Testing";
			var sgumList = new List<SubGenUserMap> { _sgum };
			BusinessLogicFakes.ShimSubGenUserMap.AllInstances.SelectInt32 = (item, x) => sgumList;
			_accounts.Clear();
			_accounts = new List<Account> {
				new Account()
				{
					company_name = "KM API Testing"
				}
			};
		}
	}
}