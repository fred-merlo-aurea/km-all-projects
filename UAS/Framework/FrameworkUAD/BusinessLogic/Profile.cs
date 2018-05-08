using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
     public class Profile
    {
         public List<Entity.Profile> Search(string searchValue, string searchFields, string orderBy, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Profile> retList = null;
            retList = DataAccess.Profile.Search(searchValue, searchFields, orderBy, client);
            return retList;
        }
         public Entity.Profile Select(int profileID, KMPlatform.Object.ClientConnections client)
        {
            Entity.Profile retItem = null;
            retItem = DataAccess.Profile.Select(profileID, client);
            return retItem;
        }
         public List<Entity.Profile> SelectPublication(int publicationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublication(publicationID, client).ToList();
            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
                FormatPublicationToolTip(s);
                s.IsNewProfile = false;
            }
            return x;
        }
         public List<Entity.Profile> SelectPublisher(int publisherID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublisher(publisherID, client).ToList();
            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
                FormatPublicationToolTip(s);
                s.IsNewProfile = false;
            }
            return x;
        }
         public List<Entity.Profile> SelectPublicationSubscribed(int publicationID, bool isSubscribed, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublicationSubscribed(publicationID, isSubscribed, client).ToList();
            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
                FormatPublicationToolTip(s);
                s.IsNewProfile = false;
            }
            return x;
        }
         public List<Entity.Profile> SelectPublicationProspect(int publicationID, bool isProspect, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublicationProspect(publicationID, isProspect, client).ToList();
            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
                FormatPublicationToolTip(s);
                s.IsNewProfile = false;
            }
            return x;
        }
         public List<Entity.Profile> SelectPublication(int publicationID, bool isSubscribed, bool isProspect, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublication(publicationID, isSubscribed, isProspect, client).ToList();
            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
                FormatPublicationToolTip(s);
                s.IsNewProfile = false;
            }
            return x;
        }
        public Entity.Profile FormatPublicationToolTip(Entity.Profile x)
        {
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("Publications");
            //sb.AppendLine(string.Empty);

            //string[] pubs = x.PublicationToolTip.Split(',');
            //foreach (string s in pubs)
            //    sb.AppendLine(s);

            //x.PublicationToolTip = sb.ToString();
            return x;
        }
        public Entity.Profile BindPublicationList(Entity.Profile x)
        {
            //x.PublicationList = Publication.SelectSubscriber(x.SubscriberID).ToList();
            return x;
        }
        public Entity.Profile BindCustomProperties(Entity.Profile x)
        {
            //x.MarketingMapList = MarketingMap.SelectSubscriber(x.SubscriberID).ToList();//SubscriberMarketingMap.Select(x.SubscriberID).ToList();
            //x.ProspectList = Prospect.Select(x.SubscriberID).ToList();
            //x.SubscriptionList = Subscription.SelectSubscriber(x.SubscriberID).ToList();
            //x.ResponseMapList = SubscriptionResponseMap.SelectSubscriber(x.SubscriberID).ToList();
            //x.PublicationList = Publication.SelectSubscriber(x.SubscriberID).ToList();
            return x;
        }
        public List<Entity.Profile> Search(string searchValue, List<Entity.Profile> searchList)
        {
            if (searchList == null)
            {
                throw new ArgumentNullException(nameof(searchList));
            }

            var worker = new KMPlatform.BusinessLogic.Profile();
            return worker.Search(searchValue, searchList);
        }


        public int Save(Entity.Profile x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.ProfileID = DataAccess.Profile.Save(x,client);
                scope.Complete();
            }

            return x.ProfileID;
        }
    }
}
