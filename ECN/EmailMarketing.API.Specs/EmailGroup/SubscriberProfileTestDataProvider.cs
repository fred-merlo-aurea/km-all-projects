using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Should;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using EmailMarketing.API.Models.EmailGroup;   


namespace EmailMarketing.API.Specs.EmailGroup
{
    public class SubscriberProfileTestDataProvider : SubscriberTestDataProvider, ISubscriberTestDataProvider
    {
        /*
        new virtual private static SubscriberProfileTestDataProvider singleton;
        new virtual public static SubscriberProfileTestDataProvider Factory()
        {
            if (null == singleton)
            {
                singleton = new SubscriberProfileTestDataProvider();
            }

            return singleton;
        }
         */

        public IEnumerable<Profile> TransformProfileList(Table table, TestHelper helper)
        {
            List<Profile> results = new List<Profile>();

            foreach(var row in table.Rows)
            {
                Profile profile = new Profile
                {
                    EmailAddress = TransformEmailAddress(helper, row["Email Address"]),
                    Title = "title" + helper.RandomString,
                    FirstName = "first" + helper.RandomString,
                    LastName = "last" + helper.RandomString,
                    FullName = "full" + helper.RandomString
                };
               
                #region standard custom fields

                string cf1_name = row["UDF 1 Name"];
                string cf2_name = row["UDF 2 Name"];
                if(false == String.IsNullOrEmpty(cf1_name))
                {
                    Dictionary<string,string> cf = new Dictionary<string, string>();
                    
                    // special case: test sending an empty list
                    if (cf1_name != "EMPTY LIST")
                    {
                        cf.Add(cf1_name, row["UDF 1 Value"].Replace("#RAND#", helper.RandomString));
                        if (false == String.IsNullOrEmpty(cf2_name))
                        {
                            cf.Add(cf2_name, row["UDF 2 Value"].Replace("#RAND#", helper.RandomString));
                        }
                    }

                }
                #endregion standard custom fields
                #region transactional custom fields

                string tf11_name = row["TF 1.1 N"];
                string tf12_name = row["TF 1.2 N"];
                if (false == String.IsNullOrWhiteSpace(tf11_name))
                {
                    List<Dictionary<string, string>> transactionList = new List<Dictionary<string, string>>();

                    if (tf11_name == "EMPTY LIST")
                    {
                        // special case #1: empty list of transactions
                    }
                    else if (tf11_name == "EMPTY SET")
                    {  // special case #2: list contains an empty set
                        transactionList.Add(new Dictionary<string, string>());
                    }
                    else 
                    {
                        Dictionary<string, string> firstTransaction = new Dictionary<string, string>();
                        firstTransaction.Add(tf11_name, row["TF 1.1 V"].Replace("#RAND#", helper.RandomString));
                        if (false == String.IsNullOrWhiteSpace(tf12_name))
                        {
                            firstTransaction.Add(tf12_name, row["TF 1.2 V"].Replace("#RAND#", helper.RandomString));
                        }
                        transactionList.Add(firstTransaction);

                        string tf21_name = row["TF 2.1 N"];
                        string tf22_name = row["TF 2.2 N"];
                        if(String.IsNullOrWhiteSpace(tf21_name))
                        {
                            // only one transaction
                        }
                        else if (tf21_name == "empty list")
                        {
                            // special case #1: empty list of transactions
                        }
                        else if (tf21_name == "empty set")
                        {  // special case #2: list contains an empty set
                            transactionList.Add(new Dictionary<string, string>());
                        }
                        else
                        {
                            Dictionary<string, string> secondTransaction = new Dictionary<string, string>();
                            secondTransaction.Add(tf21_name, row["TF 2.1 V"].Replace("#RAND#", helper.RandomString));
                            if (false == String.IsNullOrWhiteSpace(tf22_name))
                            {
                                secondTransaction.Add(tf22_name, row["TF 2.2 V"].Replace("#RAND#",helper.RandomString));
                            }
                            transactionList.Add(secondTransaction);
                        }
                    }

                }

                #endregion transactional custom fields

                
                results.Add(profile);
            }

            return results;
        }

        override public int GroupID
        {
            get
            {
                if (false == groupID.HasValue)
                {
                    // select ID of test group with the most members that has at least two subscribers, two unsubscribers, two standalone UDFs and two transaction UDFs
                    string sql = String.Format(@"
                        select TOP 1 A.GroupID
                           --, A.COUNT StandaloneCount, B.Count TransactionalCount, C.Count SubscriberCount, D.Count UnsubscriberCount
                        FROM (
	                        select GroupID, COUNT(*) [Count] FROM GroupDatafields (NOLOCK) 
	                        WHERE IsDeleted = 0 AND DatafieldSetID IS NULL 
	                        AND GroupID IN (select GroupID from Groups where CustomerID =1)
	                        Group By GroupID) A
                        JOIN (
	                        select GroupID, COUNT(*) [Count] FROM GroupDatafields (NOLOCK) 
	                        WHERE IsDeleted = 0 AND DatafieldSetID IS NOT NULL 
	                        AND GroupID IN (select GroupID from Groups where CustomerID =1)
	                        Group By GroupID) B
	                        ON A.GroupID = B.GroupID
                        JOIN (
	                        select GroupID, COUNT(*) [Count] FROM EmailGroups (NOLOCK) 
	                        WHERE SubscribeTypeCode = 'S' 
	                        AND GroupID IN (select GroupID from Groups where CustomerID =1)
	                        Group By GroupID) C
	                        ON A.GroupID = C.GroupID
                        JOIN (
	                        select GroupID, COUNT(*) [Count] FROM EmailGroups (NOLOCK) 
	                        WHERE SubscribeTypeCode = 'U' 
	                        AND GroupID IN (select GroupID from Groups where CustomerID =1)
	                        Group By GroupID) D
	                        ON A.GroupID = D.GroupID
                        WHERE A.Count > 1 and B.Count > 1 and C.Count > 1 and D.Count > 1	
                        ORDER BY GroupID, A.Count, B.Count, C.Count
                    ", CustomerID);
                    int? id = ExecuteInt32(sql);

                    id.HasValue.ShouldBeTrue();

                    this.groupID = id;
                }

                return groupID.Value;
            }
        }
    }
}
