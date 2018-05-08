using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Text;
using System.Data;
using System.Diagnostics;
using Core_AMS.Utilities;
using EntitySubscription = FrameworkUAD.Entity.Subscription;

namespace FrameworkUAD.BusinessLogic
{
    public class Subscription
    {
        private const int ValidUsaZipLength = 5;
        private const int ValidCanadaZipLength = 7;
        private const string InvalidUsaZipPrefix = "000";
        private const string ZipSeparatorUsa = "-";
        private const string ZipSeparatorCanada = " ";
        private const string ZipCodePadding = "0";
        private const int ZipPlus4MaxLength = 4;
        private const int CountryIdUsa = 1;
        private const string CountryUsa = "UNITED STATES";
        private const int CountryIdCanada = 2;
        private const string CountryCanada = "Canada";

        public bool ExistsStandardFieldName(string name, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.Subscription.ExistsStandardFieldName(name, client);
            return exists;
        }
        public List<Entity.Subscription> Select(KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties = false)
        {
            List<Entity.Subscription> retList = null;
            retList = DataAccess.Subscription.Select(client);
            if (includeCustomProperties == true)
            {
                foreach (var c in retList)
                {
                    GetCustomProperties(c, client, clientDisplayName, true);
                }
            }

            if (retList != null)
                return FormatZipCode(retList);
            else
                return retList;
        }
        public List<Entity.Subscription> Select(string email, KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties = false, bool isClientService = false)
        {
            List<Entity.Subscription> retList = null;
            retList = DataAccess.Subscription.Select(email, client, isClientService);
            if (includeCustomProperties == true)
            {
                foreach (var c in retList)
                {
                    GetCustomProperties(c, client, clientDisplayName, true);
                }
            }

            if (retList != null)
                return FormatZipCode(retList);
            else
                return retList;
        }
        public List<Entity.Subscription> SelectInValidAddresses(KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties = false)
        {
            List<Entity.Subscription> retList = null;
            retList = DataAccess.Subscription.SelectInValidAddresses(client);
            if (includeCustomProperties == true)
            {
                foreach (var c in retList)
                {
                    GetCustomProperties(c, client, clientDisplayName, true);
                }
            }
            if (retList != null)
                return FormatZipCode(retList);
            else
                return retList;
        }
        public List<int> SelectIDs(KMPlatform.Object.ClientConnections client)
        {
            List<int> retList = new List<int>();
            retList = DataAccess.Subscription.SelectIDs(client);
            return retList;
        }
        public List<Entity.Subscription> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Subscription> retList = null;
            retList = DataAccess.Subscription.SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client);
            if (retList != null)
                return FormatZipCode(retList);
            else
                return retList;
        }
        public Entity.Subscription Select(int subscriptionID, KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties = false)
        {
            Entity.Subscription retItem = null;
            retItem = DataAccess.Subscription.Select(subscriptionID, client);
            if (includeCustomProperties == true)
                GetCustomProperties(retItem, client, clientDisplayName, true);
            if (retItem != null)
                return FormatZipCode(retItem);
            else
                return retItem;
        }
        public List<Entity.Subscription> SearchAddressZip(string address1, string zipCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Subscription> retList = null;
            retList = DataAccess.Subscription.SearchAddressZip(address1, zipCode, client).ToList();
            //foreach (Entity.Subscription s in x)
            //{
            //    //BindCustomProperties(s);
            //    // s.IsNewSubscriber = false;
            //    FormatPublicationToolTip(s);
            //}
            if (retList != null)
                return FormatZipCode(retList);
            else
                return retList;
        }

        public EntitySubscription FormatZipCode(EntitySubscription entity)
        {
            if (entity?.Country != null || entity?.CountryID >= 0)
            {
                FormatZipCode(entity, true);
            }

            return entity;
        }

        public List<EntitySubscription> FormatZipCode(List<EntitySubscription> list)
        {
            if (list == null)
            {
                return null;
            }

            foreach (var entity in list)
            {
                FormatZipCode(entity, false);
            }

            return list;
        }

        private void FormatZipCode(EntitySubscription entity, bool bypassValidUsaZipLength)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Country == null && entity.CountryID < 0)
            {
                return;
            }

            try
            {
                // Canada
                if (CountryCanada.Equals(entity.Country, StringComparison.CurrentCultureIgnoreCase) || entity.CountryID == CountryIdCanada)
                {
                    FormatCanadaZipCode(entity);
                }
                // USA
                else if (CountryUsa.Equals(entity.Country, StringComparison.CurrentCultureIgnoreCase) || entity.CountryID == CountryIdUsa)
                {
                    // Task-47706: AMS Web - Adding a new subscriber in AMS Entry and subscriber has a possible name match existing in a UAD product, the State value isn’t pre-populating on entry screen.
                    if (!bypassValidUsaZipLength || entity.Zip.Length != ValidUsaZipLength || entity.Plus4.Length != ZipPlus4MaxLength)
                    {
                        FormatUsaZipCode(entity);
                    }
                }
                // Mexico or Foreign
                else //Mexico or Foreign  (ps.Country.Equals("MEXICO", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 429)
                {
                    //do nothing with ZipCode - just keep whatever is there
                    entity.Plus4 = string.Empty;
                }
            }
            catch (Exception ex)
            {
                //suppress any null errors
                Trace.TraceError(ex.ToString());
            }
        }

        private void FormatUsaZipCode(EntitySubscription entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            const int maxTotalLength = ValidUsaZipLength + ZipPlus4MaxLength;
            const int zipMinLength = ValidUsaZipLength - 1;

            if (entity.Zip.Length == ValidUsaZipLength && entity.Zip.StartsWith(InvalidUsaZipPrefix))
            {
                entity.Zip = string.Empty;
            }

            if (entity.Zip.Length < zipMinLength)
            {
                entity.Zip = string.Empty;
            }
            else if (entity.Zip.Length == zipMinLength)
            {
                entity.Zip = $"{ZipCodePadding}{entity.Zip}";
            }
            else if (entity.Zip.Length >= ValidUsaZipLength)
            {
                var zipOriginal = entity.Zip.Replace(ZipSeparatorUsa, string.Empty);
                if (entity.Zip.Length >= ValidUsaZipLength && entity.Zip.Length <= maxTotalLength)
                {
                    entity.Zip = zipOriginal.Substring(0, ValidUsaZipLength);
                    entity.Plus4 = string.Empty;
                }
                else
                {
                    entity.Zip = entity.Zip;
                }

                if (zipOriginal.Length == maxTotalLength)
                {
                    entity.Plus4 = zipOriginal.Substring(ValidUsaZipLength, ZipPlus4MaxLength);
                }
            }

            if (entity.Plus4.Length > ZipPlus4MaxLength)
            {
                entity.Plus4 = string.Empty;
            }

            var zipCheck = StringFunctions.GetNumbersOnly(entity.Zip);
            if (zipCheck.Length < ValidUsaZipLength)
            {
                entity.Zip = string.Empty;
            }
        }

        private void FormatCanadaZipCode(EntitySubscription entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            const int minZipLength = ValidCanadaZipLength - 1;
            const int sliceLength = 3;

            if (entity.Zip.Length == minZipLength)
            {
                entity.Zip = $"{entity.Zip.Substring(0, sliceLength)}{ZipSeparatorCanada}{entity.Zip.Substring(sliceLength, sliceLength)}";
                entity.Plus4 = string.Empty;
            }
            else if (entity.Zip.Length == ValidCanadaZipLength && entity.Zip.Contains(ZipSeparatorCanada))
            {
                entity.Plus4 = string.Empty;
            }
            else if (entity.Zip.Length == sliceLength && entity.Plus4.Length == sliceLength)
            {
                entity.Zip = $"{entity.Zip}{ZipSeparatorCanada}{entity.Plus4}";
                entity.Plus4 = string.Empty;
            }
            else if (entity.Zip.Length > ValidCanadaZipLength)
            {
                entity.Zip = entity.Zip.Contains(ZipSeparatorCanada)
                    ? entity.Zip.Substring(0, ValidCanadaZipLength)
                    : $"{entity.Zip.Substring(0, sliceLength)}{ZipSeparatorCanada}{entity.Zip.Substring(sliceLength, sliceLength)}";

                entity.Plus4 = string.Empty;
            }
        }

        public bool AddressUpdate(List<Entity.Subscription> list, KMPlatform.Object.ClientConnections client)
        {
            bool done = true;
            try
            {
                StringBuilder sbXML = new StringBuilder();
                sbXML.AppendLine("<XML>");
                foreach (Entity.Subscription x in list)
                {
                    sbXML.AppendLine("<Subscription>");

                    sbXML.AppendLine("<SubscriptionID>" + x.SubscriptionID.ToString() + "</SubscriptionID>");
                    sbXML.AppendLine("<Address>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.Address.Trim()) + "</Address>");
                    sbXML.AppendLine("<MailStop>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.MailStop.Trim()) + "</MailStop>");
                    sbXML.AppendLine("<City>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.City.Trim()) + "</City>");
                    sbXML.AppendLine("<State>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.State.Trim()) + "</State>");
                    sbXML.AppendLine("<Zip>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.Zip.Trim()) + "</Zip>");
                    sbXML.AppendLine("<Plus4>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.Plus4.Trim()) + "</Plus4>");
                    sbXML.AppendLine("<Latitude>" + x.Latitude.ToString() + "</Latitude>");
                    sbXML.AppendLine("<Longitude>" + x.Longitude.ToString() + "</Longitude>");
                    sbXML.AppendLine("<IsLatLonValid>" + x.IsLatLonValid.ToString() + "</IsLatLonValid>");
                    sbXML.AppendLine("<LatLongMsg>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.LatLonMsg.Trim()) + "</LatLongMsg>");
                    sbXML.AppendLine("<Country>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.Country.Trim()) + "</Country>");
                    sbXML.AppendLine("<County>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.County.Trim()) + "</County>");

                    sbXML.AppendLine("</Subscription>");
                }
                sbXML.AppendLine("</XML>");

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        DataAccess.Subscription.AddressUpdate(sbXML.ToString(), client);

                        scope.Complete();
                        done = true;
                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        done = false;
                        string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                        fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "Subscription.AddressUpdate"));
                    }
                }
            }
            catch (Exception ex)
            {
                done = false;
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "SubscriptionTransformed.AddressUpdateBulkSql"));
                throw ex;
            }
            return done;
        }
        public bool AddressUpdate(string xml, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            done = DataAccess.Subscription.AddressUpdate(xml, client);
            return done;
        }
        public bool NcoaUpdateAddress(string xml, KMPlatform.Object.ClientConnections client, int SourceFileID)
        {
            bool success = false;
            int userId = 0;
            KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
            userId = uWorker.SearchEmail("NcoaImport@TeamKM.com").UserID;

            using (TransactionScope scope = new TransactionScope())
            {
                success = DataAccess.Subscription.NcoaUpdateAddress(xml, client, userId, SourceFileID);
                scope.Complete();
            }
            return success;
        }

        public DataTable FindMatches(int productID, string fname, string lname, string company, string address, string state, string zip, string phone, string email, string title, KMPlatform.Object.ClientConnections client)
        {
            DataTable retItem = new DataTable();
            retItem = DataAccess.Subscription.FindMatches(productID, fname, lname, company, address, state, zip, phone, email, title, client);
            return retItem;
        }
        private void GetCustomProperties(Entity.Subscription sub, KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties)
        {
            SubscriberConsensusDemographic scdData = new SubscriberConsensusDemographic();
            sub.SubscriptionConsensusDemographics = scdData.Select(sub.SubscriptionID, client);
            ProductSubscription psData = new ProductSubscription();
            sub.ProductList = psData.Select(sub.SubscriptionID, client, clientDisplayName, includeCustomProperties).ToList();
        }

        public int Save(Entity.Subscription x, KMPlatform.Object.ClientConnections client)
        {
            x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);
            x.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Mobile);
            x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Fax);

            using (TransactionScope scope = new TransactionScope())
            {
                x.SubscriptionID = DataAccess.Subscription.Save(x, client);
                scope.Complete();
            }

            return x.SubscriptionID;
        }
    }
}
