using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Core_AMS.Utilities;
using FrameworkSubGen.Entity;

namespace FrameworkSubGen.DataAccess
{
    /// <summary>
    /// these have issues because only an insert - really need an insert/update method
    /// </summary>
    public class BulkDataInsert
    {
        private const string SubGenDataKey = "SubGenData";
        private const string ClassNameForApiLog = "FrameworkSubGen.DataAccess.BulkDataInsert";
        private const string CustomfieldTableName = "CustomField";
        private const string UserTableName = "User";
        private const string SubscriptionListTableName = "SubscriptionList";
        private const string SubscriptionTableName = "Subscription";
        private const string OrderitemTableName = "OrderItem";
        private const string OrderTotalLineItemTableName = "OrderTotalLineItem";
        private const string PaymentTableName = "Payment";
        private const string PublicationTableName = "Publication";
        private const string PurchaseTableName = "Purchase";
        private const string SubscriberTableName = "Subscriber";
        private const string MailinglistTableName = "MailingList";
        private const string BundleTableName = "Bundle";
        private const string AddressTableName = "Address";
        private const string AccountTableName = "Account";
        private const string ValueOptionTableName = "ValueOption";
        private const string HistoryTableName = "History";
        private const string IssueTableName = "Issue";
        private const string OrderTableName = "Order";

        public static bool BulkInsert<T>(
                                         IEnumerable<T> entities,
                                         string tableName,
                                         IEnumerable<string> columnNames,
                                         int? accountId = null,
                                         string className = null,
                                         string methodName = null) where T : class, IEntity
        {
            var success = false;
            var list = entities.ToList();

            if (accountId.HasValue)
            {
                foreach (var entity in list)
                {
                    entity.account_id = accountId.Value;
                }
            }

            using (var dataTable = BulkDataReader<T>.ToDataTable(list))
            {
                using (var connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings[SubGenDataKey].ToString()))
                {
                    try
                    {
                        connection.Open();
                        using (var bc = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, null)
                        {
                            DestinationTableName = $"[{connection.Database}].[dbo].[{tableName}]",
                            BatchSize = 0,
                            BulkCopyTimeout = 0
                        })
                        {
                            foreach (var columnName in columnNames)
                            {
                                bc.ColumnMappings.Add(columnName, columnName);
                            }

                            bc.WriteToServer(dataTable);
                            bc.Close();
                        }

                        success = true;
                    }
                    catch (Exception ex) when (
                        ex is InvalidCastException
                        || ex is SqlException
                        || ex is InvalidOperationException
                        || ex is IOException)
                    {
                        BusinessLogic.API.Authentication.SaveApiLog(ex, className ?? ClassNameForApiLog, methodName ?? tableName);
                    }
                }
            }

            return success;
        }

        public void Account(IEnumerable<Entity.Account> accounts)
        {
            var columnNames = new[]
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

            BulkInsert(accounts, AccountTableName, columnNames);
        }

        public void Address(IEnumerable<Entity.Address> adresses, int accountId)
        {
            var columnNames = new[]
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

            BulkInsert(adresses, AddressTableName, columnNames, accountId);
        }

        public void Bundle(IEnumerable<Entity.Bundle> bundles, int accountId)
        {
            var columnNames = new[]
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

            BulkInsert(bundles, BundleTableName, columnNames, accountId);
        }

        public void CustomField(IList<Entity.CustomField> customFields, int accountId)
        {
            var columnNames = new[]
                                  {
                                      "field_id",
                                      "name",
                                      "display_as",
                                      "type",
                                      "allow_other",
                                      "text_value",
                                      "account_id"
                                  };

            BulkInsert(customFields, CustomfieldTableName, columnNames, accountId);

            foreach (var customField in customFields)
            {
                if (customField?.value_options != null)
                {
                    ValueOptions(customField.value_options, customField.field_id, accountId);
                }
            }
        }

        public void ValueOptions(List<Entity.ValueOption> valueOptions, int fieldId, int accountId)
        {
            foreach (var valueOption in valueOptions)
            {
                valueOption.field_id = fieldId;
                valueOption.account_id = accountId;
            }

            var columnNames = new[]
                                  {
                                     "option_id",
                                     "value",
                                     "display_as",
                                     "disqualifier",
                                     "active",
                                     "order",
                                     "account_id"
                                };

            BulkInsert(valueOptions, ValueOptionTableName, columnNames, accountId);
        }

        public void History(IEnumerable<Entity.History> histories, int accountId)
        {
            var columnNames = new[]
                                  {
                                     "history_id",
                                     "user_id",
                                     "notes",
                                     "date",
                                     "subscriber_id",
                                     "account_id"
                                };

            BulkInsert(histories, HistoryTableName, columnNames, accountId);
        }

        public void Issue(IEnumerable<Entity.Issue> issues, int accountId)
        {
            var columnNames = new[]
                                  {
                                     "issue_id",
                                     "name",
                                     "date",
                                     "account_id"
                                };

            BulkInsert(issues, IssueTableName, columnNames, accountId);
        }

        public void MailingList(IEnumerable<Entity.MailingList> mailingLists, int accountId)
        {
            var columnNames = new[]
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

            BulkInsert(mailingLists, MailinglistTableName, columnNames, accountId);
        }

        public void Order(IEnumerable<Entity.Order> orders, int accountId)
        {
            var columnNames = new[]
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

            BulkInsert(orders, OrderTableName, columnNames, accountId);
        }

        public void OrderItem(IEnumerable<OrderItem> orderItems, int accountId)
        {
            var columnNames = new[]
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

            BulkInsert(orderItems, OrderitemTableName, columnNames, accountId);
        }

        public void OrderTotalLineItem(IEnumerable<OrderTotalLineItem> orderTotalLineItems, int accountId)
        {
            var columnNames = new[]
                                  {
                                      "product_id",
                                      "bundle_id",
                                      "price",
                                      "sub_total",
                                      "tax_total",
                                      "grand_total",
                                      "account_id"
                                  };

            BulkInsert(orderTotalLineItems, OrderTotalLineItemTableName, columnNames, accountId);
        }

        public void Payment(IEnumerable<Entity.Payment> payments, int accountId)
        {
            var columnNames = new[]
                                  {
                                      "amount",
                                      "notes",
                                      "transaction_id",
                                      "type",
                                      "account_id"
                                  };

            BulkInsert(payments, PaymentTableName, columnNames, accountId);
        }

        public void Publication(IEnumerable<Entity.Publication> publications, int accountId)
        {
            var columnNames = new[]
                                  {
                                      "publication_id",
                                      "name",
                                      "issues_per_year",
                                      "account_id"
                                  };

            BulkInsert(publications, PublicationTableName, columnNames, accountId);
        }

        public void Purchase(IEnumerable<Entity.Purchase> purchases, int accountId)
        {
            var columnNames = new[]
                                  {
                                      "billing_address_id",
                                      "bundle_id",
                                      "invoice_id",
                                      "name",
                                      "subscriber_id",
                                      "account_id"
                                  };

            BulkInsert(purchases, PurchaseTableName, columnNames, accountId);
        }

        public void Subscriber(IEnumerable<Entity.Subscriber> subscribers, int accountId)
        {
            var columnNames = new[]
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

            BulkInsert(subscribers, SubscriberTableName, columnNames, accountId);
        }

        public void Subscription(IEnumerable<Entity.Subscription> subscriptions, int accountId)
        {
            var columnNames = new[]
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

            BulkInsert(subscriptions, SubscriptionTableName, columnNames, accountId);
        }

        public void SubscriptionList(IEnumerable<Entity.SubscriptionList> subscriptionLists, int accountId)
        {
            var columnNames = new[]
                                  {
                                      "subscription_id",
                                      "copies",
                                      "is_grace",
                                      "is_paid",
                                      "type",
                                      "account_id"
                                  };

            BulkInsert(subscriptionLists, SubscriptionListTableName, columnNames, accountId);
        }

        public void User(IEnumerable<Entity.User> users, int accountId)
        {
            var columnNames = new[]
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

            BulkInsert(users, UserTableName, columnNames, accountId);
        }
    }
}
