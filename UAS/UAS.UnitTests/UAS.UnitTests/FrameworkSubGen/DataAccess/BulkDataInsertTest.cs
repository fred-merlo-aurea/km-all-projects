using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using FrameworkSubGen.BusinessLogic.API.Fakes;
using FrameworkSubGen.DataAccess;
using FrameworkSubGen.DataAccess.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;

using ShimKMCommonDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkSubGen.DataAccess
{
    using global::FrameworkSubGen.Entity;

    [TestFixture]
    public class BulkDataInsertTest : Fakes
    {
        private const int FieldId = 20;
        private const string DestinationTableName = "DestinationTableName";
        private const string BatchSize = "BatchSize";
        private const string BulkCopyTimeout = "TimeOut";
        private const string BulkDataInsertTestExceptionMessage = "BulkDataInsertTest_Exception";
        public const int AccountId = 10;
        public const string BulkDataInsertClassName = "FrameworkSubGen.DataAccess.BulkDataInsert";

        private static readonly string[] UserColumnNames = 
            {
                "user_id",
                "first_name",
                "last_name",
                "password",
                "password_md5",
                "email",
                "is_admin",
                "account_id"
            };

        private static readonly string[] SubscriberColumnNames = 
            {
                "subscriber_id",
                "renewal_code",
                "email",
                "password",
                "password_md5",
                "first_name",
                "last_name",
                "source",
                "create_date",
                "delete_date",
                "account_id"
            };

        private static readonly string[] PublicationColumnNames = 
            {
                "publication_id",
                "name",
                "issues_per_year",
                "account_id"
            };

        private static readonly string[] PaymentColumnNames = 
            {
                "amount",
                "notes",
                "transaction_id",
                "type",
                "account_id"
            };

        private static readonly string[] IssueColumnNames = 
            {
                "issue_id",
                "name",
                "date",
                "account_id"
            };

        private static readonly string[] HistoryColumnNames = 
            {
                "history_id",
                "user_id",
                "notes",
                "date",
                "subscriber_id",
                "account_id"
            };

        private static readonly string[] PurchaseColumnNames = 
            {
                "billing_address_id",
                "bundle_id",
                "invoice_id",
                "name",
                "subscriber_id",
                "account_id"
            };

        private static readonly string[] SubscriptionColumnNames = 
            {
                "subscription_id",
                "publication_id",
                "mailing_address_id",
                "billing_address_id",
                "issues",
                "copies",
                "paid_issues_left",
                "unearned_revenue",
                "type",
                "date_created",
                "date_expired",
                "date_last_renewed",
                "last_purchase_bundle_id",
                "renew_campaign_active",
                "audit_classification",
                "audit_request_type",
                "account_id"
            };

        private static readonly string[] MailingListColumnNames =
            {
                "count",
                "date_created",
                "grace_from_date",
                "grace_issues",
                "is_gap",
                "mailing_list_id",
                "name",
                "parent_mailing_list_id",
                "status",
                "account_id"
            };

        private static readonly string[] OrderTotalLineItemColumnNames =
            {
                "product_id",
                "bundle_id",
                "price",
                "sub_total",
                "tax_total",
                "grand_total",
                "account_id"
            };

        private static readonly string[] SubscriptionListItemColumnNames =
            {
                "subscription_id",
                "copies",
                "is_grace",
                "is_paid",
                "type",
                "account_id",
            };

        private static readonly string[] SubscriberDemographicColumnNames =
            {
                "subscriber_id",
                "account_id",
                "field_id",
                "text_value",
                "DateCreated",
                "IsProcessed",
            };

        private static readonly string[] SubscriberDemographicDetailColumnNames =
            {
                "subscriber_id",
                "option_id",
                "field_id",
                "account_id",
                "value",
                "DateCreated",
                "IsProcessed"
            };

        private static readonly string[] OrderColumnNames =
            {
              "order_id",
              "mailing_address_id",
              "billing_address_id",
              "subscriber_id",
              "import_name",
              "user_id",
              "channel_id",
              "order_date",
              "is_gift",
              "sub_total",
              "tax_total", 
              "grand_total",
              "account_id"
            };

        private static readonly string[] OrderItemColumnNames =
            {
                "order_item_id",
                "bundle_id",
                "refund_date",
                "fulfilled_date", 
                "sub_total",
                "tax_total",
                "grand_total",
                "account_id"
            };

        private static readonly string[] AccountColumnNames =
            {
                "account_id",
                "company_name",
                "email",
                "website",
                "active",
                "api_active",
                "api_key",
                "api_login",
                "plan",
                "audit_type",
                "created",
                "master_checkout"
            };

        private static readonly string[] AddressColumnNames =
            {
               "address_id",
               "first_name",
               "last_name",
               "address",
               "address_line_2",
               "company",
               "city",
               "state",
               "subscriber_id",
               "country",
               "country_name",
               "country_abbreviation",
               "latitude",
               "longitude",
               "verified",
               "zip_code",
               "account_id"
            };

        private static readonly string[] BundleColumnNames =
            {
               "bundle_id",
               "name",
               "price",
               "active",
               "promo_code",
               "description",
               "type",
               "account_id"
            };

        private readonly Mocks mocks = new Mocks();
        private IDisposable context;

        private static IEnumerable<IBulkDataInsertTestData<IEntity>> BulkDataInsertTestData
        {
            get
            {
                yield return new BulkDataInsertTestData<Account>(
                    "Account",
                    AccountColumnNames,
                    (entities, accountId) => { new BulkDataInsert().Account(entities.Cast<Account>()); });
                yield return new BulkDataInsertTestData<Address>(
                    "Address",
                    AddressColumnNames,
                    (entities, accountId) => { new BulkDataInsert().Address(entities.Cast<Address>(), accountId); });
                yield return new BulkDataInsertTestData<Bundle>(
                    "Bundle",
                    BundleColumnNames,
                    (entities, accountId) => { new BulkDataInsert().Bundle(entities.Cast<Bundle>(), accountId); });
                yield return new BulkDataInsertTestData<Order>(
                    "Order",
                    OrderColumnNames,
                    (entities, accountId) => { new BulkDataInsert().Order(entities.Cast<Order>(), accountId); });
                yield return new BulkDataInsertTestData<OrderItem>(
                    "OrderItem",
                    OrderItemColumnNames,
                    (entities, accountId) => { new BulkDataInsert().OrderItem(entities.Cast<OrderItem>(), accountId); });
                yield return new BulkDataInsertTestData<User>(
                    "User", 
                    UserColumnNames,
                    (entities, accountId) => { new BulkDataInsert().User(entities.Cast<User>(), accountId); });
                yield return new BulkDataInsertTestData<Subscriber>(
                    "Subscriber",
                    SubscriberColumnNames,
                    (entities, accountId) => { new BulkDataInsert().Subscriber(entities.Cast<Subscriber>(), accountId); });
                yield return new BulkDataInsertTestData<Publication>(
                    "Publication",
                    PublicationColumnNames,
                    (entities, accountId) => { new BulkDataInsert().Publication(entities.Cast<Publication>(), accountId); });
                yield return new BulkDataInsertTestData<Payment>(
                    "Payment",
                    PaymentColumnNames,
                    (entities, accountId) => { new BulkDataInsert().Payment(entities.Cast<Payment>(), accountId); });
                yield return new BulkDataInsertTestData<Issue>(
                    "Issue",
                    IssueColumnNames,
                    (entities, accountId) => { new BulkDataInsert().Issue(entities.Cast<Issue>(), accountId); });
                yield return new BulkDataInsertTestData<History>(
                    "History",
                    HistoryColumnNames,
                    (entities, accountId) => { new BulkDataInsert().History(entities.Cast<History>(), accountId); });
                yield return new BulkDataInsertTestData<Purchase>(
                    "Purchase",
                    PurchaseColumnNames,
                    (entities, accountId) => { new BulkDataInsert().Purchase(entities.Cast<Purchase>(), accountId); });
                yield return new BulkDataInsertTestData<Subscription>(
                    "Subscription",
                    SubscriptionColumnNames, 
                    (entities, accountId) => { new BulkDataInsert().Subscription(entities.Cast<Subscription>(), accountId); });
                yield return new BulkDataInsertTestData<MailingList>(
                    "MailingList",
                    MailingListColumnNames,
                    (entities, accountId) => { new BulkDataInsert().MailingList(entities.Cast<MailingList>(), accountId); });
                yield return new BulkDataInsertTestData<OrderTotalLineItem>(
                    "OrderTotalLineItem", 
                    OrderTotalLineItemColumnNames,
                    (entities, accountId) => { new BulkDataInsert().OrderTotalLineItem(entities.Cast<OrderTotalLineItem>(), accountId); });
                yield return new BulkDataInsertTestData<SubscriptionList>(
                    "SubscriptionList",
                    SubscriptionListItemColumnNames, 
                    (entities, accountId) => { new BulkDataInsert().SubscriptionList(entities.Cast<SubscriptionList>(), accountId); });
                yield return new BulkDataInsertTestData<SubscriberDemographic>(
                    "SubscriberDemographic", 
                    SubscriberDemographicColumnNames, 
                    (entities, accountId) => { global::FrameworkSubGen.DataAccess.SubscriberDemographic.Save(entities.Cast<SubscriberDemographic>()); },
                    "FrameworkSubGen.DataAccess.SubscriberDemographic",
                    "Save");
                yield return new BulkDataInsertTestData<SubscriberDemographicDetail>(
                    "SubscriberDemographicDetail", 
                    SubscriberDemographicDetailColumnNames,
                    (entities, accountId) => { global::FrameworkSubGen.DataAccess.SubscriberDemographicDetail.Save(entities.Cast<SubscriberDemographicDetail>()); },
                    "FrameworkSubGen.DataAccess.SubscriberDemographicDetail",
                    "Save");
            }
        }

        [SetUp]
        public void SetUp()
        {
            context = ShimsContext.Create();
            SetupFakes(mocks);
            
            CreateStubs();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        [TestCaseSource(nameof(BulkDataInsertTestData))]
        public void BulkDataInsert_DataCorrect_DataWritten(IBulkDataInsertTestData<IEntity> testData)
        {
            const int numberOfEntities = 5;
            var tableName = $"[SubGenData].[dbo].[{testData.TableName}]";

            // Arrange
            var entities = Enumerable
                .Range(0, numberOfEntities)
                .Select(x => testData.CreateEntity())
                .ToList();

            var actualTableColumnMappings = new Dictionary<string, Dictionary<string, string>>();
            var actualBulkCopySettings = new Dictionary<string, Dictionary<string, object>>();
            var actualNumberOfRows = 0;

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (sqlBulkCopy, dataTable) =>
                {
                    actualNumberOfRows = dataTable.Rows.Count;
                    var bcSettings = new Dictionary<string, object>
                                         {
                                             [DestinationTableName] =
                                                 sqlBulkCopy.DestinationTableName,
                                             [BatchSize] = sqlBulkCopy.BatchSize,
                                             [BulkCopyTimeout] = sqlBulkCopy.BulkCopyTimeout
                                         };

                    actualBulkCopySettings[sqlBulkCopy.DestinationTableName] = bcSettings;

                    var bcTableColumnMappings = new Dictionary<string, string>();
                    foreach (SqlBulkCopyColumnMapping columnMapping in sqlBulkCopy.ColumnMappings)
                    {
                        bcTableColumnMappings.Add(columnMapping.SourceColumn, columnMapping.DestinationColumn);
                    }

                    actualTableColumnMappings[sqlBulkCopy.DestinationTableName] = bcTableColumnMappings;
                };

            // Act
            testData.TestAction(entities, AccountId);

            entities.ShouldAllBe(entity => entity.account_id == AccountId);
            actualNumberOfRows.ShouldBe(numberOfEntities);

            actualBulkCopySettings.ShouldSatisfyAllConditions(
                () => actualBulkCopySettings.Values.Count.ShouldBe(1),
                () => actualBulkCopySettings.ShouldContainKey(tableName),
                () => actualBulkCopySettings[tableName][BatchSize].ShouldBe(0),
                () => actualBulkCopySettings[tableName][BulkCopyTimeout].ShouldBe(0),
                () => actualBulkCopySettings[tableName][DestinationTableName].ShouldBe(tableName, $"table {testData.TableName} missing."));

            actualTableColumnMappings.ShouldSatisfyAllConditions(
                () => actualTableColumnMappings.Values.Count.ShouldBe(1),
                () => actualTableColumnMappings[tableName].Count.ShouldBe(testData.ColumnNames.Count()),
                () => actualTableColumnMappings[tableName].ShouldAllBe(keyValue => keyValue.Key == keyValue.Value),
                () => testData.ColumnNames
                    .ToList()
                    .ForEach(columnName => actualTableColumnMappings[tableName]
                    .ShouldContainKey(columnName, $"column: {columnName} in table {testData.TableName} is missing.")));
        }

        [Test]
        [TestCaseSource(nameof(BulkDataInsertTestData))]
        public void BulkDataInsert_ExceptionThrown_ApiLogWritten(IBulkDataInsertTestData<IEntity> testData)
        {
            const int numberOfEntities = 5;
            var className = testData.TestClassName;
            var methodName = testData.TestMethodName;

            // Arrange
            var apiLogs = new List<ApiLog>();
            var entities = Enumerable
                            .Range(0, numberOfEntities)
                            .Select(x => testData.CreateEntity())
                            .ToList();

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (sqlBulkCopy, dataTable) =>
                throw new InvalidOperationException(BulkDataInsertTestExceptionMessage);

            ShimAuthentication.SaveApiLogExceptionStringString = (ex, myClass, myMethod) =>
            {
                apiLogs.Add(new ApiLog
                {
                    Entity = myClass,
                    Method = myMethod
                });
            };

            // Act
            testData.TestAction(entities, AccountId);

            // Assert
            entities.ShouldAllBe(entity => entity.account_id == AccountId);
            apiLogs.ShouldSatisfyAllConditions(
                () => apiLogs.ShouldContain(apiLog => apiLog.Entity.Equals(className) && apiLog.Method.Equals(methodName)),
                () => apiLogs.Count.ShouldBe(1));
        }

        // this is a special case ..as CustomFields contains ValueOptions and upload them too
        [Test]
        public void CustomField_DataCorrect_DataWritten()
        {
            const int numberOfEntities = 5;
            const string tableNameCustomField = "[SubGenData].[dbo].[CustomField]";
            const string tableNameValueOption = "[SubGenData].[dbo].[ValueOption]";
            var columnNamesCustomField = new[]
                                             {
                                                "field_id",
                                                "name",
                                                "display_as",
                                                "type",
                                                "allow_other",
                                                "text_value",
                                                "account_id"
                                            };

            var columnNamesValueOption = new[]
                                             {
                                                 "option_id",
                                                 "value",
                                                 "display_as",
                                                 "disqualifier",
                                                 "active",
                                                 "order",
                                                 "account_id"
                                             };

            // Arrange
            var entities = CreateEntities<CustomField>(numberOfEntities);
            foreach (var customField in entities)
            {
                customField.value_options = new List<ValueOption> { new ValueOption(), new ValueOption() };
                customField.field_id = FieldId;
            }

            var actualTableColumnMappings = new Dictionary<string, Dictionary<string, string>>();
            var actualBulkCopySettings = new Dictionary<string, Dictionary<string, object>>();
            var actualNumberOfRows = new Dictionary<string, int>()
                                         {
                                             [tableNameCustomField] = 0,
                                             [tableNameValueOption] = 0
                                         };

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (sqlBulkCopy, dataTable) =>
            {
                actualNumberOfRows[sqlBulkCopy.DestinationTableName] += dataTable.Rows.Count;
                var bcSettings = new Dictionary<string, object>
                {
                    [DestinationTableName] = sqlBulkCopy.DestinationTableName,
                    [BatchSize] = sqlBulkCopy.BatchSize,
                    [BulkCopyTimeout] = sqlBulkCopy.BulkCopyTimeout
                };

                actualBulkCopySettings[sqlBulkCopy.DestinationTableName] = bcSettings;

                var bcTableColumnMappings = new Dictionary<string, string>();
                foreach (SqlBulkCopyColumnMapping columnMapping in sqlBulkCopy.ColumnMappings)
                {
                    bcTableColumnMappings.Add(columnMapping.SourceColumn, columnMapping.DestinationColumn);
                }

                actualTableColumnMappings[sqlBulkCopy.DestinationTableName] = bcTableColumnMappings;
            };

            // Act
            var bulkDataInsert = new BulkDataInsert();
            bulkDataInsert.CustomField(entities, AccountId);

            // Assert
            entities.ShouldAllBe(entity => entity.account_id == AccountId);

            foreach (var customField in entities)
            {
                customField.value_options.ShouldAllBe(valueOption => valueOption.field_id == FieldId
                                                                     && valueOption.account_id == AccountId);
            }

            actualNumberOfRows[tableNameCustomField].ShouldBe(5);
            actualNumberOfRows[tableNameValueOption].ShouldBe(10);

            actualBulkCopySettings.ShouldSatisfyAllConditions(
                () => actualBulkCopySettings.Values.Count.ShouldBe(2),
                () => actualBulkCopySettings.ShouldContainKey(tableNameCustomField),
                () => actualBulkCopySettings.ShouldContainKey(tableNameValueOption),
                () => actualBulkCopySettings[tableNameCustomField][BatchSize].ShouldBe(0),
                () => actualBulkCopySettings[tableNameCustomField][BulkCopyTimeout].ShouldBe(0),
                () => actualBulkCopySettings[tableNameCustomField][DestinationTableName].ShouldBe(tableNameCustomField, "table CustomField missing."),
                () => actualBulkCopySettings[tableNameValueOption][BatchSize].ShouldBe(0),
                () => actualBulkCopySettings[tableNameValueOption][BulkCopyTimeout].ShouldBe(0),
                () => actualBulkCopySettings[tableNameValueOption][DestinationTableName].ShouldBe(tableNameValueOption, "table ValueOption missing."));

            actualTableColumnMappings.ShouldSatisfyAllConditions(
                () => actualTableColumnMappings.Values.Count.ShouldBe(2),
                () => actualTableColumnMappings[tableNameCustomField].Count.ShouldBe(columnNamesCustomField.Length),
                () => actualTableColumnMappings[tableNameValueOption].ShouldAllBe(keyValue => keyValue.Key == keyValue.Value),
                () => actualTableColumnMappings[tableNameCustomField].Count.ShouldBe(columnNamesCustomField.Length),
                () => actualTableColumnMappings[tableNameValueOption].ShouldAllBe(keyValue => keyValue.Key == keyValue.Value),
                () => columnNamesCustomField
                      .ToList()
                      .ForEach(columnName => actualTableColumnMappings[tableNameCustomField]
                      .ShouldContainKey(columnName, $"column: {columnName} in table: CustomField is missing.")),
                () => columnNamesValueOption
                      .ToList()
                      .ForEach(columnName => actualTableColumnMappings[tableNameValueOption]
                      .ShouldContainKey(columnName, $"column: {columnName} in table: ValueOption is missing.")));
        }

        // this is a special case ..as CustomFields contains ValueOptions and upload them too
        [Test]
        public void CustomField_ExceptionThrown_ApiLogWritten()
        {
            const int numberOfEntities = 5;
            const string methodName = "CustomField";

            // Arrange
            var apiLogs = new List<ApiLog>();
            var entities = CreateEntities<CustomField>(numberOfEntities);

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (sqlBulkCopy, dataTable) =>
                throw new InvalidOperationException(BulkDataInsertTestExceptionMessage);

            ShimAuthentication.SaveApiLogExceptionStringString = (ex, myClass, myMethod) =>
            {
                apiLogs.Add(new ApiLog
                {
                    Entity = myClass,
                    Method = myMethod
                });
            };

            // Act
            var bulkDataInsert = new BulkDataInsert();
            bulkDataInsert.CustomField(entities, AccountId);

            // Assert
            entities.ShouldAllBe(entity => entity.account_id == AccountId);
            apiLogs.ShouldSatisfyAllConditions(
                () => apiLogs.ShouldContain(apiLog => apiLog.Entity.Equals(BulkDataInsertClassName) && apiLog.Method.Equals(methodName)),
                () => apiLogs.Count.ShouldBe(numberOfEntities + 1));
        }

        // this is a special case ..as ValueOptions also have a fieldId parameter
        [Test]
        public void ValueOption_DataCorrect_DataWritten()
        {
            const int numberOfEntities = 5;
            const string tableName = "[SubGenData].[dbo].[ValueOption]";
            var columnNames = new[]
                                  {
                                      "option_id", "value", "display_as", "disqualifier", "active", "order",
                                      "account_id"
                                  };

            // Arrange
            var entities = CreateEntities<ValueOption>(numberOfEntities);

            var actualTableColumnMappings = new Dictionary<string, Dictionary<string, string>>();
            var actualBulkCopySettings = new Dictionary<string, Dictionary<string, object>>();
            var actualNumberOfRows = 0;

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (sqlBulkCopy, dataTable) =>
            {
                actualNumberOfRows = dataTable.Rows.Count;
                var bcSettings = new Dictionary<string, object>
                {
                    [DestinationTableName] =
                                             sqlBulkCopy.DestinationTableName,
                    [BatchSize] = sqlBulkCopy.BatchSize,
                    [BulkCopyTimeout] = sqlBulkCopy.BulkCopyTimeout
                };

                actualBulkCopySettings[sqlBulkCopy.DestinationTableName] = bcSettings;

                var bcTableColumnMappings = new Dictionary<string, string>();
                foreach (SqlBulkCopyColumnMapping columnMapping in sqlBulkCopy.ColumnMappings)
                {
                    bcTableColumnMappings.Add(columnMapping.SourceColumn, columnMapping.DestinationColumn);
                }

                actualTableColumnMappings[sqlBulkCopy.DestinationTableName] = bcTableColumnMappings;
            };

            // Act
            var bulkDataInsert = new BulkDataInsert();
            bulkDataInsert.ValueOptions(entities, FieldId, AccountId);

            // Assert
            entities.ShouldAllBe(entity => entity.account_id == AccountId && entity.field_id == FieldId);
            actualNumberOfRows.ShouldBe(numberOfEntities);

            actualBulkCopySettings.ShouldSatisfyAllConditions(
                () => actualBulkCopySettings.Values.Count.ShouldBe(1),
                () => actualBulkCopySettings.ShouldContainKey(tableName),
                () => actualBulkCopySettings[tableName][BatchSize].ShouldBe(0),
                () => actualBulkCopySettings[tableName][BulkCopyTimeout].ShouldBe(0),
                () => actualBulkCopySettings[tableName][DestinationTableName].ShouldBe(tableName, "table ValueOption missing."));

            actualTableColumnMappings.ShouldSatisfyAllConditions(
                () => actualTableColumnMappings.Values.Count.ShouldBe(1),
                () => actualTableColumnMappings[tableName].Count.ShouldBe(columnNames.Length),
                () => actualTableColumnMappings[tableName].ShouldAllBe(keyValue => keyValue.Key == keyValue.Value));

            foreach (var columnName in columnNames)
            {
                actualTableColumnMappings[tableName].ShouldContainKey(
                    columnName,
                    $"column: {columnName} in table: ValueOption is missing.");
            }
        }

        // this is a special case ..as ValueOptions also have a fieldId parameter
        [Test]
        public void ValueOption_ExceptionThrown_ApiLogWritten()
        {
            const int numberOfEntities = 5;
            const string methodName = "ValueOption";

            // Arrange
            var apiLogs = new List<ApiLog>();
            var entities = CreateEntities<ValueOption>(numberOfEntities);

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (sqlBulkCopy, dataTable) =>
                throw new InvalidOperationException(BulkDataInsertTestExceptionMessage);

            ShimAuthentication.SaveApiLogExceptionStringString = (ex, myClass, myMethod) =>
            {
                apiLogs.Add(new ApiLog
                {
                    Entity = myClass,
                    Method = myMethod
                });
            };

            // Act
            var bulkDataInsert = new BulkDataInsert();
            bulkDataInsert.ValueOptions(entities, FieldId, AccountId);

            // Assert
            entities.ShouldAllBe(entity => entity.account_id == AccountId && entity.field_id == FieldId);
            apiLogs.ShouldSatisfyAllConditions(
                () => apiLogs.ShouldContain(apiLog => apiLog.Entity.Equals(BulkDataInsertClassName) && apiLog.Method.Equals(methodName)),
                () => apiLogs.Count.ShouldBe(1));
        }

        private static void CreateStubs()
        {
            ShimKMCommonDataFunctions.ExecuteScalarSqlCommandString = (command, connectionString) => 0;
            ShimConfigurationManager.ConnectionStringsGet = () =>
                {
                    var connectionStringSettingsCollection =
                        new ConnectionStringSettingsCollection
                            {
                                new ConnectionStringSettings(
                                    "SubGenData",
                                    "Database=SubGenData")
                            };

                    return connectionStringSettingsCollection;
                };
        }

        private static List<T> CreateEntities<T>(int numberOfEntities) where T : class, IEntity, new()
        {
            var entities = Enumerable.Range(0, numberOfEntities).Select(x => new T { account_id = AccountId }).ToList();
            return entities;
        }
    }
}