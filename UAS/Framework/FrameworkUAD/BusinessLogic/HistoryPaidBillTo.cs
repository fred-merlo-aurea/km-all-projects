using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class HistoryPaidBillTo
    {
        public List<Entity.HistoryPaidBillTo> SelectSubscription(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistoryPaidBillTo> retList = null;
            retList = DataAccess.HistoryPaidBillTo.SelectSubscription(subscriptionID, client);
            return retList;
        }
        public Entity.HistoryPaidBillTo Select(int subscriptionPaidID, KMPlatform.Object.ClientConnections client)
        {
            Entity.HistoryPaidBillTo item = null;
            item = DataAccess.HistoryPaidBillTo.Select(subscriptionPaidID,client);
            return item;
        }

        public int Save(FrameworkUAD.Entity.PaidBillTo myPaidBillTo, int userID, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Entity.HistoryPaidBillTo hp = new Entity.HistoryPaidBillTo();
            hp.PaidBillToID = myPaidBillTo.PaidBillToID;
            hp.SubscriptionPaidID = myPaidBillTo.SubscriptionPaidID;
            hp.PubSubscriptionID = myPaidBillTo.PubSubscriptionID;
            hp.FirstName = myPaidBillTo.FirstName;
            hp.LastName = myPaidBillTo.LastName;
            hp.Company = myPaidBillTo.Company;
            hp.Title = myPaidBillTo.Title;
            hp.AddressTypeID = myPaidBillTo.AddressTypeId;
            hp.Address1 = myPaidBillTo.Address1;
            hp.Address2 = myPaidBillTo.Address2;
            hp.City = myPaidBillTo.City;
            hp.RegionCode = myPaidBillTo.RegionCode;
            hp.RegionID = myPaidBillTo.RegionID;
            hp.ZipCode = myPaidBillTo.ZipCode;
            hp.Plus4 = myPaidBillTo.Plus4;
            hp.CarrierRoute = myPaidBillTo.CarrierRoute;
            hp.County = myPaidBillTo.County;
            hp.Country = myPaidBillTo.Country;
            hp.CountryID = myPaidBillTo.CountryID;
            hp.Latitude = myPaidBillTo.Latitude;
            hp.Longitude = myPaidBillTo.Longitude;
            hp.IsAddressValidated = myPaidBillTo.IsAddressValidated;
            hp.AddressValidationDate = myPaidBillTo.AddressValidationDate;
            hp.AddressValidationSource = myPaidBillTo.AddressValidationSource;
            hp.AddressValidationMessage = myPaidBillTo.AddressValidationMessage;
            hp.Email = myPaidBillTo.Email;
            hp.Phone = myPaidBillTo.Phone;
            hp.Fax = myPaidBillTo.Fax;
            hp.Mobile = myPaidBillTo.Mobile;
            hp.Website = myPaidBillTo.Website;
            hp.DateCreated = DateTime.Now;
            hp.CreatedByUserID = userID;

            return Save(hp,client);
        }
        public int Save(Entity.HistoryPaidBillTo x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.HistoryPaidBillToID = DataAccess.HistoryPaidBillTo.Save(x,client);
                scope.Complete();
            }

            return x.HistoryPaidBillToID;
        }
    }
}
