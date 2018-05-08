using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECN_Framework_DataLayer;

using Should;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Text.RegularExpressions;

namespace EmailMarketing.API.Specs.EmailGroup
{
    public class SubscriberTestDataProvider : ISubscriberTestDataProvider
    {
        private static SubscriberTestDataProvider singleton;
        public static SubscriberTestDataProvider Factory()
        {
            if (null == singleton)
            {
                singleton = new SubscriberTestDataProvider();
            }

            return singleton;
        }

        public string TransformEmailAddress(TestHelper helper, string emailAddress)
        {
            var emailReplacementTokenMatch = Regex.Match(emailAddress, @"#(\w+)#");
            if (false == emailReplacementTokenMatch.Success)
            {
                return emailAddress;
            }

            string testName = emailReplacementTokenMatch.Value; // emailReplacementTokenMatch.Captures[1].Value;
            switch (testName)
            {
                case "#rand#":
                    string randomizedEmailAddress = emailAddress.Replace("#rand#", helper.RandomString);
                    TestNameExecuteStringCache.Add(emailAddress, randomizedEmailAddress);
                    return randomizedEmailAddress;
                case "#SubscribedInTestGroup#":
                    return GetUniqueEmailAddressInTestGroup(emailAddress, "S");
                case "#UnsubscribedInTestGroup#":
                    return GetUniqueEmailAddressInTestGroup(emailAddress, "U");
                case "#NotInTestGroup#":
                    return GetUniqueEmailAddressNotInTestGroup(emailAddress);
                case "#GlobalMasterSupressionList#":
                    return GetEmailAddressInGlobalMasterSupressionList(emailAddress);
                case "#ChannelMasterSupressionList#":
                    return GetEmailAddressInChannelMasterSupressionList(emailAddress);
                case "#MasterSupressionGroup#":
                    return GetEmailAddressInMasterSupressionGroup(emailAddress);
                default:
                    throw new ApplicationException(String.Format("unknown replacement token {0} in test data table", testName));
            }
        }

        public int TransformGroupId(int groupId)
        {
            return -1 == groupId ? GroupID : groupId;
        }

        public IEnumerable<Models.EmailGroup.Email> TransformEmailList(Table table, TestHelper helper)
        {
            List<Models.EmailGroup.Email> results = new List<Models.EmailGroup.Email>();
            IEnumerable<Models.EmailGroup.Email> emailList = table.CreateSet<Models.EmailGroup.Email>();
            // replace placeholder with randomized values
            foreach (var row in emailList)
            {
                row.GroupId = TransformGroupId(row.GroupId);
                row.EmailAddress = TransformEmailAddress(helper, row.EmailAddress);

                //yield return row;
                results.Add(row);
            }
            return results;
        }

        public IEnumerable<Models.EmailGroup.SubscriptionResult> TransformResultList(IEnumerable<Models.EmailGroup.SubscriptionResult> results)
        {

            // replace placeholder with randomized values
            foreach (var row in results)
            {
                // restore groupID to default value if we populated it, above
                // populate groupId if it has the default value
                if (row.GroupID == GroupID)
                {
                    row.GroupID = -1;
                }

                // if email-address is from the database, replace it with the test name
                string testName = GetTestNameFromCachedValue(row.EmailAddress);
                if (false == String.IsNullOrEmpty(testName))
                {
                    row.EmailAddress = testName; // "#" + testName + "#";
                }

                yield return row;
            }
        }

        public static int? ExecuteInt32(string sql, string connectionStringName = "communicator")
        {
            string dbValue = ExecuteString(sql, connectionStringName);

            int returnValue;
            if (null != dbValue && Int32.TryParse(dbValue, out returnValue))
            {
                return returnValue;
            }

            return null;
        }

        public static string ExecuteString(string sql, string connectionStringName = "communicator")
        {
            object dbValue = DataFunctions.ExecuteScalar(sql, connectionStringName);

            return null == dbValue ? null : dbValue.ToString();
        }

        protected SubscriberTestDataProvider() { }

        protected int? baseChannelID = null;
        protected int? customerID = null;
        protected int? groupID = null;

        public string ApiAccessKey { 
            get; 
            set; 
        }
        
        public Dictionary<String, String> TestNameExecuteStringCache = new Dictionary<string, string>();

        public string GetTestNameFromCachedValue(string emailAddress)
        {
            return TestNameExecuteStringCache.FirstOrDefault((s) => s.Value == emailAddress).Key;
        }
        public string GetCachedTestValueOrExecuteString(string testName, Func<string> makeSql)
        {
            if (false == TestNameExecuteStringCache.ContainsKey(testName))
            {
                string emailAddress = ExecuteString(makeSql());

                emailAddress.ShouldNotBeEmpty();
                
                TestNameExecuteStringCache.Add(testName, emailAddress);
            }
            return TestNameExecuteStringCache[testName];
        }

        virtual public int CustomerID
        {
            set
            {
                customerID = value;
            }
            get
            {
                if (false == customerID.HasValue)
                {
                    string sql = String.Format(@"select CustomerID from Users (NOLOCK) where AccessKey = '{0}'", ApiAccessKey);
                    int? id = ExecuteInt32(sql, "accounts");

                    id.HasValue.ShouldBeTrue();
                    
                    this.customerID = id;
                }

                return customerID.Value;
            }
        }

        virtual public int BaseChannelID
        {
            set
            {
                baseChannelID = value;
            }
            get
            {
                if (false == baseChannelID.HasValue)
                {
                    string sql = String.Format(@"SELECT BaseChannelID from Customer (NOLOCK) where customerID = {0} and IsDeleted = 0", CustomerID);
                    int? id = ExecuteInt32(sql, "accounts");
                    
                    id.HasValue.ShouldBeTrue();

                    baseChannelID = id;
                }

                return baseChannelID.Value;
            }
        }

        virtual public int GroupID
        {
            set
            {
                groupID = value;
            }

            get
            {
                if (false == groupID.HasValue)
                {
                    string sql = String.Format(@"
                        -- select ID of test group with the most members that has at least two subscribers and two unsubscribers
                         with SubscriberCount   As (select GroupID, COUNT(*) [Count] FROM EmailGroups (NOLOCK) WHERE SubscribeTypeCode = 'S' Group By GroupID),
                              UnsubscriberCount As (select GroupID, COUNT(*) [Count] FROM EmailGroups (NOLOCK) WHERE SubscribeTypeCode = 'U' Group By GroupID)
                       select TOP 1 G.GroupID
                         from Groups (NOLOCK) G,EmailGroups (NOLOCK) EG, SubscriberCount SC, UnSubscriberCount UC
                        where CustomerID = {0} 
                          AND LOWER(GroupName) like '%test%' 
                          and G.GroupID = EG.GroupID 
                          and EG.SubscribeTypeCode IN ('S','U')
                          and EG.GroupID = SC.GroupID
                          and EG.GroupID = UC.GroupID
                        group by G.GroupID, UC.Count, SC.Count
                       having SC.Count > 2 AND UC.Count > 2
                        order by COUNT(SubscribeTypeCode) DESC
                    ", CustomerID);
                    int? id = ExecuteInt32(sql);

                    id.HasValue.ShouldBeTrue();
                    
                    this.groupID = id;
                }

                return groupID.Value;
            }
        }

        public string GetUniqueEmailAddressInTestGroup(string testName, string subscribeTypeCode = "S")
        {
            return GetCachedTestValueOrExecuteString(testName, () => MakeQueryEmailAddressInGroup(subscribeTypeCode));
        }

        string MakeQueryEmailAddressInGroup(string subscribeTypeCode)
        {
            string uniquenessPredicate = 0 == TestNameExecuteStringCache.Count
                                       ? ""
                                       : String.Format(@"
                            AND EmailAddress NOT IN('{0}')", String.Join("','", TestNameExecuteStringCache.Values));
            return String.Format(@"
                    SELECT TOP 1 EmailAddress 
                    FROM   emails (NOLOCK) 
                    WHERE  CustomerID = {0}{1} 
                            AND EmailID IN(SELECT DISTINCT EmailID 
                                            FROM   EmailGroups (NOLOCK)
                                            WHERE  GroupID = {2} 
                                                    AND SubscribeTypeCode = '{3}') 
                            AND EmailAddress NOT IN (SELECT EmailAddress 
                                                    FROM   GlobalMasterSuppressionList (NOLOCK)
                                                    WHERE  IsDeleted <> 1) 
                            AND EmailAddress NOT IN (SELECT EmailAddress 
                                                    FROM   ChannelMasterSuppressionList (NOLOCK)
                                                    WHERE  IsDeleted <> 1 
                                                            AND BaseChannelID = {4}) 
                    ", CustomerID, uniquenessPredicate, GroupID, subscribeTypeCode, BaseChannelID);
        }

        public string GetUniqueEmailAddressNotInTestGroup(string testName)
        {
            return GetCachedTestValueOrExecuteString(testName, () => MakeQueryEmailAddressNotInGroup());
        }

        string MakeQueryEmailAddressNotInGroup()
        {
            string uniquenessPredicate = 0 == TestNameExecuteStringCache.Count
                                       ? ""
                                       : String.Format(@"
                                   AND EmailAddress NOT IN('{0}')", String.Join("','", TestNameExecuteStringCache.Values));
            return String.Format(@"
				WITH GroupMasterSupression AS (
						SELECT Emails.EmailID 
						  FROM EmailGroups (NOLOCK) , Groups (NOLOCK) , Emails (NOLOCK) 
						 WHERE Emails.EmailID = EmailGroups.EmailID 
						   AND Groups.GroupID = EmailGroups.GroupID 
						   AND Groups.MasterSupression = 1 
						   AND Groups.CustomerID = {0}
					)
                SELECT TOP 1 Emails.EmailAddress 
                FROM   Emails (NOLOCK) 
                LEFT OUTER JOIN GroupMasterSupression ON (Emails.EmailID = GroupMasterSupression.EmailID)
                WHERE  CustomerID = {0} {1}
                        AND GroupMasterSupression.EmailID IS NULL
                        AND Emails.EmailID NOT IN(SELECT DISTINCT EmailID 
                                                    FROM   EmailGroups (NOLOCK)
                                                   WHERE  GroupID =  {2})
                        AND EmailAddress NOT IN (SELECT EmailAddress 
                                                FROM   GlobalMasterSuppressionList (NOLOCK)
                                                WHERE  IsDeleted <> 1) 
                        AND EmailAddress NOT IN (SELECT EmailAddress 
                                                FROM   ChannelMasterSuppressionList (NOLOCK)
                                                WHERE  IsDeleted <> 1 
                                                        AND BaseChannelID = {3})
                 ", CustomerID, uniquenessPredicate, GroupID, BaseChannelID);
        }

        public string GetEmailAddressInGlobalMasterSupressionList(string testName)
        {
            return GetCachedTestValueOrExecuteString(testName, () =>
            {
                return @"
                             SELECT TOP 1 EmailAddress 
                             FROM   GlobalMasterSuppressionList (NOLOCK) 
                             WHERE  IsDeleted <> 1 
                                    AND dbo.Fn_ValidateEmailAddress(EmailAddress) = 1 
                            ";
            });
        }

        public string GetEmailAddressInChannelMasterSupressionList(string testName)
        {
            return GetCachedTestValueOrExecuteString(testName, () =>
            {
                return String.Format(@"
                                            SELECT TOP 1 EmailAddress 
                                            FROM   ChannelMasterSuppressionList (NOLOCK) 
                                            WHERE  IsDeleted <> 1 
                                                   AND BaseChannelID = {0} 
                                          ", BaseChannelID);
            });
        }

        public string GetEmailAddressInMasterSupressionGroup(string testName)
        {
            return GetCachedTestValueOrExecuteString(testName, () =>
            {
                return String.Format(@"
                                            SELECT TOP 1 EmailAddress 
                                            FROM   EmailGroups (NOLOCK), 
                                                   Groups      (NOLOCK), 
                                                   Emails      (NOLOCK) 
                                            WHERE  Emails.EmailID = EmailGroups.EmailID 
                                                   AND Groups.GroupID = EmailGroups.GroupID 
                                                   AND Groups.MasterSupression = 1 
                                                   AND Groups.CustomerID = {0}
                                          ", CustomerID);
            });
        }
    }
}
