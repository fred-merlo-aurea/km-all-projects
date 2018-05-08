using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using AMS_Operations;
using AMS_Operations.Fakes;
using FrameworkSubGen.BusinessLogic.API.Fakes;
using FrameworkSubGen.Entity;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.AMS_Operations
{
    /// <summary>
    ///     Unit Tests for <see cref="Operations.SubGenToUAD"/>
    /// </summary>
    [TestFixture]
    public partial class OperationTest
    {
        private const string AccountField = "accounts";
        private const string SgClient = "Knowledge_Marketing";
        private const int AccountId = 10;
        private const string Message = "Throw Exception to break Infinite Loop";

        private string _subGenSyncClients;
        private ChangeDataCapture _changeDataCapture;
        private ChangeDataCapture _actualDataCapture;

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SubGenToUAD_WithNullEntities_SetEntitiesVerifyAllDataChanges(bool isSubGenClient)
        {
            // Arrange
            SetupForSubGenUad();
            _subGenSyncClients = isSubGenClient ?
                SgClient :
                null;

            _changeDataCapture = typeof(ChangeDataCapture).CreateInstance(true);
            _changeDataCapture.account_id = AccountId;

            ChangeDataCapture expectedDataCapture = typeof(ChangeDataCapture).CreateInstance(true);

            if (isSubGenClient)
            {
                expectedDataCapture.addresses = new List<Address>();
                expectedDataCapture.bundles = new List<Bundle>();
                expectedDataCapture.custom_fields = new List<CustomField>();
                expectedDataCapture.purchases = new List<Purchase>();
                expectedDataCapture.subscribers = new List<Subscriber>();
                expectedDataCapture.subscriptions = new List<Subscription>();
            }

            // Act
            try
            {
                _operations.SubGenToUAD();
            }
            catch (Exception exp)
            {
                // Assert
                exp.Message.ShouldBe(Message);
                VerifyDataCapture(expectedDataCapture);
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SubGenToUAD_WithEntities_VerifyAllDataChanges(bool isSubGenClient)
        {
            // Arrange
            SetupForSubGenUad();
            _subGenSyncClients = isSubGenClient ?
                SgClient :
                null;

            _changeDataCapture = typeof(ChangeDataCapture).CreateInstance();
            _changeDataCapture.account_id = AccountId;

            ChangeDataCapture expectedDataCapture = typeof(ChangeDataCapture).CreateInstance();

            // Act
            try
            {
                _operations.SubGenToUAD();
            }
            catch (Exception exp)
            {
                // Assert
                exp.Message.ShouldBe(Message);
                VerifyDataCapture(expectedDataCapture);
            }
        }

        private void VerifyDataCapture(ChangeDataCapture expectedDataCapture)
        {
            _actualDataCapture.ShouldSatisfyAllConditions(
                () => _actualDataCapture.addresses?
                    .IsListContentMatched(expectedDataCapture.addresses)
                    .ShouldBeTrue(),
                () => _actualDataCapture.bundles?
                    .IsListContentMatched(expectedDataCapture.bundles)
                    .ShouldBeTrue(),
                () => _actualDataCapture.custom_fields?
                    .IsListContentMatched(expectedDataCapture.custom_fields)
                    .ShouldBeTrue(),
                () => _actualDataCapture.purchases?
                    .IsListContentMatched(expectedDataCapture.purchases)
                    .ShouldBeTrue(),
                () => _actualDataCapture.subscribers?
                    .IsListContentMatched(expectedDataCapture.subscribers)
                    .ShouldBeTrue(),
                () => _actualDataCapture.subscriptions?
                    .IsListContentMatched(expectedDataCapture.subscriptions)
                    .ShouldBeTrue(),
                () => _actualDataCapture.account_id = AccountId
            );
        }

        private void SetupForSubGenUad()
        {
            int counter = 0;
            Account account = new Account
            {
                company_name = SgClient.Replace("_", " ")
            };

            _operations.SetField(AccountField, new List<Account>
            {
                account
            });

            ShimOperations.AllInstances.LogErrorExceptionString = (_, __, ___) => throw new Exception(Message);

            ShimOperations.AllInstances.CDCtoImportSubscriberChangeDataCaptureEnumsClientAccount =
                (operations, capture, sgClient, acc) =>
                {
                    if (counter == 1)
                    {
                        throw new Exception();
                    }

                    counter++;
                    sgClient.ToString().ShouldBe(SgClient);
                    acc.ShouldBe(account);

                    _actualDataCapture = capture;
                };

            ShimChangeDataCapture.AllInstances.GetEnumsClientDateTimeListOfEnumsEntities = (_, sgClient, __, ____) =>
            {
                sgClient.ToString().ShouldBe(SgClient);

                return _changeDataCapture;
            };

            ShimClient.AllInstances.AMS_SelectPaidBoolean = (client, includeCustomObjects) =>
            {
                includeCustomObjects.ShouldBeFalse();
                return new List<Client>
                {
                    new Client
                    {
                        DisplayName = SgClient.Replace("_", " ")
                    }
                };
            };

            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                {"SubGenSyncInterval", "0"},
                {"SubGenSyncClients", _subGenSyncClients}
            };
        }
    }
}
