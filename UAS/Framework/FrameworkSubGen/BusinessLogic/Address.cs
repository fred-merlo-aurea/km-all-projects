using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkSubGen.BusinessLogic.API;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using ServiceStack.Text;
using EntityAddress = FrameworkSubGen.Entity.Address;
using ProductSubscription = FrameworkUAD.Entity.ProductSubscription;

namespace FrameworkSubGen.BusinessLogic
{
    public class Address
    {
        private const string BusinessLogicErrorMessage = "FrameworkSubGen.BusinessLogic.Address";
        private const string AcsBusinessLogicMethodName = "UpdateForACS";
        private const string NcoaBusinessLogicMethodName = "UpdateForNCOA";
        private const string ApiErrorMessage = "FrameworkSubGen.BusinessLogic.API.Address";
        private const string ApiMethodName = "Update";
        private const string BusinessLogicMethodName = "Save";

        public string GetXml(List<Entity.Address> list)
        {
            foreach (Entity.Address x in list)
                FormatData(x);
            list = CleanForXml(list);

            string xml = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<List<Entity.Address>>(list));
            return xml;
        }
        public List<Entity.Address> CleanForXml(List<Entity.Address> list)
        {
            foreach (var x in list)
            {
                if (!string.IsNullOrEmpty(x.first_name))
                    x.first_name = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.first_name);
                if (!string.IsNullOrEmpty(x.last_name))
                    x.last_name = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.last_name);
                if (!string.IsNullOrEmpty(x.address))
                    x.address = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.address);
                if (!string.IsNullOrEmpty(x.address_line_2))
                    x.address_line_2 = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.address_line_2);
                if (!string.IsNullOrEmpty(x.company))
                    x.company = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.company);
                if (!string.IsNullOrEmpty(x.city))
                    x.city = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.city);
                if (!string.IsNullOrEmpty(x.state))
                    x.state = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.state);
                if (!string.IsNullOrEmpty(x.country))
                    x.country = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.country);
                if (!string.IsNullOrEmpty(x.country_abbreviation))
                    x.country_abbreviation = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.country_abbreviation);
                if (!string.IsNullOrEmpty(x.country_name))
                    x.country_name = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.country_name);
                if (!string.IsNullOrEmpty(x.zip_code))
                    x.zip_code = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.zip_code);
            }
            return list;
        }

        public bool SaveBulkXml(IList<Entity.Address> addresses)
        {
            if (addresses == null || !addresses.Any())
            {
                return true;
            }

            foreach (var item in addresses)
            {
                FormatData(item);
            }

            const int batchSize = 500;
            const string errorMessage = "SubGen.BusinessLogic.Address.SaveBulkXml";
            var coreImporter = new CoreImport();
            var result = coreImporter.CoreSaveBulkXml(
                addresses,
                (xml) =>
                {
                    DataAccess.Address.SaveBulkXml(xml);
                },
                batchSize,
                errorMessage,
                false,
                true);

            return result;
        }

        public void FormatData(Entity.Address x)
        {
            try
            {
                #region truncate strings
                if (x.first_name != null && x.first_name.Length > 50)
                    x.first_name = x.first_name.Substring(0, 50);
                if (x.last_name != null && x.last_name.Length > 50)
                    x.last_name = x.last_name.Substring(0, 50);
                if (x.address != null && x.address.Length > 50)
                    x.address = x.address.Substring(0, 50);
                if (x.address_line_2 != null && x.address_line_2.Length > 50)
                    x.address_line_2 = x.address_line_2.Substring(0, 50);
                if (x.company != null && x.company.Length > 50)
                    x.company = x.company.Substring(0, 50);
                if (x.city != null && x.city.Length > 50)
                    x.city = x.city.Substring(0, 50);
                if (x.state != null && x.state.Length > 60)
                    x.state = x.state.Substring(0, 60);
                if (x.country != null && x.country.Length > 50)
                    x.country = x.country.Substring(0, 50);
                if (x.country_abbreviation != null && x.country_abbreviation.Length > 50)
                    x.country_abbreviation = x.country_abbreviation.Substring(0, 50);
                if (x.country_name != null && x.country_name.Length > 50)
                    x.country_name = x.country_name.Substring(0, 50);
                if (x.zip_code != null && x.zip_code.Length > 50)
                    x.zip_code = x.zip_code.Substring(0, 50);
                #endregion
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.Address", "FormatData");
            }
        }
        public List<Entity.Address> Select(int subscriberId)
        {
            List<Entity.Address> x = new List<Entity.Address>();
            try
            {
                x = DataAccess.Address.Select(subscriberId);
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.Address", "Select");
            }
            return x;
        }
        public List<Entity.Address> SelectOnMailingId(int subscriberId, int publicationId)
        {
            List<Entity.Address> x = new List<Entity.Address>();
            try
            {
                x = DataAccess.Address.SelectOnMailingId(subscriberId, publicationId);
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.Address", "SelectOnMailingId");
            }
            return x;
        }
        public List<Entity.Address> SelectOnMailingId(int subscriptionId)
        {
            List<Entity.Address> x = new List<Entity.Address>();
            try
            {
                x = DataAccess.Address.SelectOnMailingId(subscriptionId);
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.Address", "SelectOnMailingId");
            }
            return x;
        }
        public List<Entity.Address> SelectOnMailingId(int accountId, int publicationId, string address, string address2, string city, string state, string zipCode)
        {
            List<Entity.Address> x = new List<Entity.Address>();
            try
            {
                x = DataAccess.Address.SelectOnMailingId(accountId, publicationId, address, address2, city, state, zipCode);
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.Address", "SelectOnMailingId");
            }
            return x;
        }
        public bool Save(Entity.Address address)
        {
            FormatData(address);
            bool done = false;
            try
            {
                DataAccess.Address.Save(address);
                done = true;
            }
            catch (Exception ex)
            {
                done = false;
                API.Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.Address", "Save");
            }
            return done;
        }
        public bool Save(List<Entity.Address> list)
        {
            foreach (Entity.Address x in list)
                FormatData(x);
            bool done = false;
            try
            {
                DataAccess.Address.Save(list);
                done = true;
            }
            catch (Exception ex)
            {
                done = false;
                API.Authentication.SaveApiLog(ex, "FrameworkSubGen.BusinessLogic.Address", "Save");
            }
            return done;
        }
        
        /// <summary>
        /// Will return a list of any addresses that were not able to be sent to SubGen for an update due to not being able to determine addressId.
        /// If the returned list count is 0 then all address were able to be updated
        /// </summary>
        /// <param name="productSubscriptions">List of ProductSubscriptions </param>
        /// <param name="acsFileDetails">List of AcsFileDetails</param>
        /// <param name="kmClientId">KM Client Id</param>
        /// <returns></returns>
        public List<ProductSubscription> UpdateForACS(IList<ProductSubscription> productSubscriptions, IList<AcsFileDetail> acsFileDetails, int kmClientId)
        {
            return Update(productSubscriptions, kmClientId, true, acsFileDetails).ToList();
        }
        public List<ProductSubscription> UpdateForNCOA(IList<ProductSubscription> productSubscriptions, IList<NCOA> ncoaList, int kmClientId)
        {
            return Update(productSubscriptions, kmClientId, false, null).ToList();
        }

        public IList<ProductSubscription> Update(
            IList<ProductSubscription> productSubscriptions, 
            int kmClientId, 
            bool updateOldAddresses, 
            IList<AcsFileDetail> acsListForOldAddresses)
        {
            GuardNotNull(productSubscriptions, nameof(productSubscriptions));

            var haveSubGenMailId = productSubscriptions.Where(x => x.SubGenMailingAddressId > 0).ToList();
            var noSubGenMailId = productSubscriptions.Where(x => x.SubGenMailingAddressId <= 0).ToList();
            var updateList = new List<EntityAddress>();
            var foundSubGenMailId = new List<ProductSubscription>();
            try
            {               
                foreach (var productSubscription in haveSubGenMailId)
                {
                    var address = CreateAddressFrom(productSubscription);
                    updateList.Add(address);
                }
                foreach (var productSubscription in noSubGenMailId)
                {
                    var addressesToUpdate = new List<EntityAddress>();
                    if (productSubscription.SubGenSubscriberID > 0)
                    {
                        addressesToUpdate = GetAddressesForSubscriber(kmClientId, productSubscription).ToList();
                    }
                    else if (productSubscription.SubGenSubscriptionID > 0)
                    {
                        addressesToUpdate = GetAddressesForSubscription(productSubscription).ToList();
                    }
                    else if (updateOldAddresses)
                    {
                        GuardNotNull(acsListForOldAddresses, nameof(acsListForOldAddresses));
                        addressesToUpdate = GetOldAddresses(acsListForOldAddresses, kmClientId, productSubscription).ToList();
                    }

                    UpdateAddressesFrom(addressesToUpdate, productSubscription,
                        (address, subscription) =>
                        {
                            updateList.Add(address);
                            foundSubGenMailId.Add(subscription);
                        });
                }
                foreach (var found in foundSubGenMailId)
                {
                    noSubGenMailId.Remove(found);
                }

                DataAccess.Address.UpdateForACS(GetXml(updateList));
            }
            // All exceptions must be caught in a silent manner in order not to interrupt the process, 
            // thus it is mandatory to catch all Global Exceptions
            catch (Exception ex)
            {                
                var methodName = updateOldAddresses ? AcsBusinessLogicMethodName : NcoaBusinessLogicMethodName;
                Authentication.SaveApiLog(ex, BusinessLogicErrorMessage, methodName);
            }

            SendToSubGen(kmClientId, updateList);

            return noSubGenMailId;
        }

        private static void GuardNotNull(object item, string itemName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(itemName);
            }
        }

        private IList<EntityAddress> GetAddressesForSubscriber(int kmClientId, ProductSubscription productSubscription)
        {
            var publication = new Publication();
            var publicationEntity = publication.SelectKmPubCode(productSubscription.PubCode, kmClientId);
            if (publicationEntity == null || publicationEntity.publication_id <= 0)
            {
                return Enumerable.Empty<EntityAddress>().ToList();
            }

            return SelectOnMailingId(productSubscription.SubGenSubscriberID, publicationEntity.publication_id);
        }

        private IList<EntityAddress> GetAddressesForSubscription(ProductSubscription productSubscription)
        {
            return SelectOnMailingId(productSubscription.SubGenSubscriptionID);
        }

        private IList<EntityAddress> GetOldAddresses(IList<AcsFileDetail> acsListForOldAddresses, 
            int kmClientId, ProductSubscription productSubscription)
        {
            var publication = new Publication();
            var publicationEntity = publication.SelectKmPubCode(productSubscription.PubCode, kmClientId);
            var account = new Account();
            var accountEntity = account.Select(kmClientId);
            var oldAddress = acsListForOldAddresses.FirstOrDefault(x => x.SequenceID == productSubscription.SequenceID);

            if (oldAddress == null)
            {
                return Enumerable.Empty<EntityAddress>().ToList();
            }

            return SelectOnMailingId(
                accountEntity.account_id,
                publicationEntity.publication_id, 
                oldAddress.OldAddress1,
                oldAddress.OldAddress2, 
                oldAddress.OldCity,
                oldAddress.OldStateAbbreviation, 
                oldAddress.OldZipCode);
        }

        private static void UpdateAddressesFrom(
            IList<EntityAddress> addressesToUpdate, 
            ProductSubscription productSubscription, 
            Action<EntityAddress, ProductSubscription> onAddressUpdated)
        {
            if (addressesToUpdate == null)
            {
                return;
            }

            foreach (var address in addressesToUpdate)
            {
                address.address = productSubscription.Address1;
                address.address_line_2 = productSubscription.Address2;
                address.city = productSubscription.City;
                address.country = productSubscription.Country;
                address.state = productSubscription.RegionCode;
                address.zip_code = productSubscription.ZipCode;

                onAddressUpdated(address, productSubscription);
            }
        }

        private static EntityAddress CreateAddressFrom(ProductSubscription productSubscription)
        {
            if (productSubscription == null)
            {
                throw new ArgumentNullException(nameof(productSubscription));
            }

            // Only updating address so leave everything else blank
            return new EntityAddress
            {
                address = productSubscription.Address1,
                address_id = productSubscription.SubGenMailingAddressId,
                address_line_2 = productSubscription.Address2,
                city = productSubscription.City,
                country = productSubscription.Country,
                state = productSubscription.RegionCode,
                zip_code = productSubscription.ZipCode
            };
        }

        private static void SendToSubGen(int kmClientId, IList<EntityAddress> addresses)
        {
            try
            {
                var account = new Account();
                var subGenAccount = account.Select(kmClientId);
                
                var addressApi = new API.Address();
                var sgClient = Entity.Enums.GetClient(subGenAccount.company_name.Trim());
                foreach (var address in addresses)
                {
                    try
                    {
                        addressApi.Update(sgClient, address);
                    }
                    // All exceptions must be caught in a silent manner in order not to interrupt the process, 
                    // thus it is mandatory to catch all Global Exceptions
                    catch (Exception ex)
                    {
                        Authentication.SaveApiLog(ex, ApiErrorMessage, ApiMethodName);
                    }
                }
            }
            // All exceptions must be caught in a silent manner in order not to interrupt the process, 
            // thus it is mandatory to catch all Global Exceptions
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, BusinessLogicErrorMessage, BusinessLogicMethodName);
            }
        }
    }
}
