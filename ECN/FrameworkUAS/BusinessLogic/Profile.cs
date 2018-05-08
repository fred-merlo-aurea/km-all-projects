using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class Profile
    {
        public  List<Entity.Profile> Search(string searchValue, string searchFields, string orderBy)
        {
            List<Entity.Profile> retList = null;
            retList = DataAccess.Profile.Search(searchValue, searchFields, orderBy);

            return retList;
        }
        public  Entity.Profile Select(int profileID)
        {
            Entity.Profile retItem = null;
            retItem = DataAccess.Profile.Select(profileID);

            return retItem;
        }
        public  List<Entity.Profile> SelectPublication(int publicationID)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublication(publicationID).ToList();

            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
            }
            return x;
        }
        public  List<Entity.Profile> SelectPublisher(int publisherID)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublisher(publisherID).ToList();

            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
            }
            return x;
        }
        public  List<Entity.Profile> SelectPublicationSubscribed(int publicationID, bool isSubscribed)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublicationSubscribed(publicationID, isSubscribed).ToList();

            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
            }
            return x;
        }
        public  List<Entity.Profile> SelectPublicationProspect(int publicationID, bool isProspect)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublicationProspect(publicationID, isProspect).ToList();

            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
            }
            return x;
        }
        public  List<Entity.Profile> SelectPublication(int publicationID, bool isSubscribed, bool isProspect)
        {
            List<Entity.Profile> x = null;
            x = DataAccess.Profile.SelectPublication(publicationID, isSubscribed, isProspect).ToList();

            foreach (Entity.Profile s in x)
            {
                //BindCustomProperties(s);
            }
            return x;
        }

        public  Entity.Profile BindPublicationList(Entity.Profile x)
        {
            //x.PublicationList = Publication.SelectSubscriber(x.SubscriberID).ToList();
            return x;
        }
        public  Entity.Profile BindCustomProperties(Entity.Profile x)
        {
            //x.MarketingMapList = MarketingMap.SelectSubscriber(x.SubscriberID).ToList();//SubscriberMarketingMap.Select(x.SubscriberID).ToList();
            //x.ProspectList = Prospect.Select(x.SubscriberID).ToList();
            //x.SubscriptionList = Subscription.SelectSubscriber(x.SubscriberID).ToList();
            //x.ResponseMapList = SubscriptionResponseMap.SelectSubscriber(x.SubscriberID).ToList();
            //x.PublicationList = Publication.SelectSubscriber(x.SubscriberID).ToList();
            return x;
        }

        public List<T> Search<T>(string searchValue, List<T> searchList) where T: Entity.IProfile
        {
            if (searchValue == null)
            {
                throw new ArgumentNullException(nameof(searchValue));
            }

            if (searchList == null)
            {
                throw new ArgumentNullException(nameof(searchList));
            }

            searchValue = searchValue.ToLower();
            var matchList = new List<T>();

            matchList.AddRange(searchList.Where(x => IsMatch(x.FirstName, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.LastName, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Company, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Title, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Address1, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Address2, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.City, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.RegionCode, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.ZipCode, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Plus4, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.CarrierRoute, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.County, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Country, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.IsAddressValidated.ToString(), searchValue)));
            matchList.AddRange(searchList.Where(x => x.AddressValidationDate != null && IsMatch(x.AddressValidationDate.ToString(), searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.AddressValidationSource, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.AddressValidationMessage, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Email, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Phone, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Fax, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Mobile, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.Website, searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.DateCreated.ToString(), searchValue)));
            matchList.AddRange(searchList.Where(x => x.DateUpdated != null && IsMatch(x.DateUpdated.ToString(), searchValue)));
            matchList.AddRange(searchList.Where(x => IsMatch(x.CreatedByUserID.ToString(), searchValue)));
            matchList.AddRange(searchList.Where(x => x.UpdatedByUserID != null && IsMatch(x.UpdatedByUserID.ToString(), searchValue)));

            return matchList.Distinct().ToList();
        }

        private bool IsMatch(string source, string searchValue)
        {
            var match = source != null 
                        && source.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) > -1;
            return match;
        }

        public int Save(Entity.Profile x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.ProfileID = DataAccess.Profile.Save(x);
                scope.Complete();
            }

            return x.ProfileID;
        }
    }
}
