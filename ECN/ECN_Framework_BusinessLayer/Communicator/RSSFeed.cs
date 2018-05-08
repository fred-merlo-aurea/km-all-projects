using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml;
using ECN_Framework_Common.Objects;
using System.Net;

namespace ECN_Framework_BusinessLayer.Communicator
{
     [Serializable]
    public class RSSFeed
    {

         private static bool Exists(int RSSFeedID,string Name, int CustomerID)
         {
             bool exists = false;
             using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
             {
                 exists = ECN_Framework_DataLayer.Communicator.RSSFeed.Exists(RSSFeedID,Name, CustomerID);
                 scope.Complete();
             }

             return exists;
         }

         private static bool UsedInContent(string rssFeedName, int CustomerID)
         {
             bool usedInContent = false;
             using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
             {
                 usedInContent = ECN_Framework_DataLayer.Communicator.RSSFeed.UsedInContent(rssFeedName, CustomerID);
                 scope.Complete();
             }
             return usedInContent;
         }

         public static ECN_Framework_Entities.Communicator.RSSFeed GetByID(int FeedID)
         {
             ECN_Framework_Entities.Communicator.RSSFeed rss = new ECN_Framework_Entities.Communicator.RSSFeed();
             using (TransactionScope scope = new TransactionScope())
             {
                 rss = ECN_Framework_DataLayer.Communicator.RSSFeed.GetByFeedID(FeedID);
                 scope.Complete();
             }
             return rss;
         }

         public static ECN_Framework_Entities.Communicator.RSSFeed GetByFeedName(string FeedName, int CustomerID)
         {
             ECN_Framework_Entities.Communicator.RSSFeed rss = new ECN_Framework_Entities.Communicator.RSSFeed();
             using (TransactionScope scope = new TransactionScope())
             {
                 rss = ECN_Framework_DataLayer.Communicator.RSSFeed.GetByFeedName(FeedName, CustomerID);
                 scope.Complete();
             }
             return rss;
         }

         public static List<ECN_Framework_Entities.Communicator.RSSFeed> GetByCustomerID(int CustomerID)
         {
             List<ECN_Framework_Entities.Communicator.RSSFeed> rssList = new List<ECN_Framework_Entities.Communicator.RSSFeed>();
             using(TransactionScope scope = new TransactionScope())
             {
                 rssList = ECN_Framework_DataLayer.Communicator.RSSFeed.GetByCustomerID(CustomerID);
                 scope.Complete();
             }
             return rssList;
         }

         public static int Save(ECN_Framework_Entities.Communicator.RSSFeed rss, KMPlatform.Entity.User user)
         {
             Validate(rss, user);
             int returnID = -1;
             using (TransactionScope scope = new TransactionScope())
             {
                 returnID = ECN_Framework_DataLayer.Communicator.RSSFeed.Save(rss, user.UserID);
                 scope.Complete();
             }
             return returnID;
         }

         public static void Delete(ECN_Framework_Entities.Communicator.RSSFeed rss, KMPlatform.Entity.User user)
         {
             if (UsedInContent(rss.Name, rss.CustomerID))
             {
                 ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
                 List<ECNError> errorList = new List<ECNError>();

                 errorList.Add(new ECNError(Enums.Entity.RSSFeed, Method, "Cannot delete RSS feed because it is used in content"));
                 throw new ECNException(errorList);
             }
             using (TransactionScope scope = new TransactionScope())
             {
                 if (!UsedInContent(rss.Name, rss.CustomerID))
                 {
                     ECN_Framework_DataLayer.Communicator.RSSFeed.Delete(rss.FeedID, user.UserID);
                }

                 scope.Complete();
             }
         }

        private static void Validate(ECN_Framework_Entities.Communicator.RSSFeed rss, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(rss.FeedID, rss.Name, user.CustomerID))
                errorList.Add(new ECNError(Enums.Entity.RSSFeed, Method, "RSS Feed Name already exists"));

            if (rss.StoriesToShow == null || rss.StoriesToShow <= 0)
                errorList.Add(new ECNError(Enums.Entity.RSSFeed, Method, "Must enter number of stories"));

            Verify(rss.URL.Trim());

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Verify(string rssURL)
        {
            List<ECNError> errorList = new List<ECNError>();
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            System.ServiceModel.Syndication.SyndicationFeed sfTest = new System.ServiceModel.Syndication.SyndicationFeed();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                XmlReader reader = XmlReader.Create(rssURL.Trim());
                sfTest = System.ServiceModel.Syndication.SyndicationFeed.Load(reader);

                if(sfTest == null)
                {
                    errorList.Add(new ECNError(Enums.Entity.RSSFeed, Method, "Unable to validate RSS Feed"));
                }
            }
            catch (Exception ex)
            {
                errorList.Add(new ECNError(Enums.Entity.RSSFeed, Method, "Unable to validate RSS Feed"));
            }

            if(errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}
