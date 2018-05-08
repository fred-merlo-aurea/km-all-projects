using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class Objects
    {
        public List<FrameworkUAD.Object.SubscriberProduct> GetProductDemographics(KMPlatform.Object.ClientConnections client, string emailAddress, string productCode, List<FrameworkUAD.Object.SubscriberProductDemographic> dimensions = null)
        {
            List<Object.SubscriberProduct> retList = null;
            retList = DataAccess.Objects.GetProductDemographics(client, emailAddress, productCode);

            //now get the dimensions requested if null get all
            foreach (Object.SubscriberProduct sp in retList)
            {
                SubscriberProductDemographic spdData = new SubscriberProductDemographic();
                List<Object.SubscriberProductDemographic> spdList = spdData.Select(sp.SubscriptionID, productCode, client).ToList();
                if (dimensions == null)
                    sp.Demographics = spdList;
                else
                {
                    sp.Demographics = (from a in spdList
                                       join u in dimensions on a.Name.ToLower() equals u.Name.ToLower()
                                       select a).ToList();
                }
            }
            return retList;
        }
        public List<FrameworkUAD.Object.SubscriberConsensus> GetDemographics(KMPlatform.Object.ClientConnections client, string emailAddress, List<FrameworkUAD.Object.SubscriberConsensusDemographic> dimensions = null)
        {
            List<Object.SubscriberConsensus> retList = null;
            retList = DataAccess.Objects.GetDemographics(client, emailAddress);

            //now get the dimensions requested if null get all
            foreach (Object.SubscriberConsensus sc in retList)
            {
                SubscriberConsensusDemographic scdData = new SubscriberConsensusDemographic();
                List<Object.SubscriberConsensusDemographic> scdList = scdData.Select(sc.SubscriptionID, client);
                if (dimensions == null)
                    sc.Demographics = scdList;
                else
                {
                    sc.Demographics = (from a in scdList
                                       join u in dimensions on a.Name.ToLower() equals u.Name.ToLower()
                                       select a).ToList();
                }
            }

            return retList;
        }
        public List<FrameworkUAD.Object.Dimension> GetDimensions(KMPlatform.Object.ClientConnections client)
        {
            List<Object.Dimension> retList = null;
            retList = DataAccess.Objects.GetDimensions(client);
            return retList;
        }
        public List<FrameworkUAD.Object.CustomField> GetCustomFields(KMPlatform.Object.ClientConnections client, string productCode = "", bool includeAdHocs = true, bool includeConsensus = true)
        {
            List<Object.CustomField> products = DataAccess.Objects.GetCustomFields_Product(client);
            if (!string.IsNullOrEmpty(productCode))
                products = products.Where(x => x.ProductCode.Equals(productCode, StringComparison.CurrentCultureIgnoreCase)).ToList();
            List<Object.CustomField> productAdhocs = new List<Object.CustomField>();
            if (includeAdHocs == true)
            {
                productAdhocs = DataAccess.Objects.GetCustomFields_AdHoc(client);
                if (!string.IsNullOrEmpty(productCode))
                    productAdhocs = productAdhocs.Where(x => x.ProductCode.Equals(productCode, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            List<Object.CustomField> all = new List<Object.CustomField>();
            if (includeConsensus == true)
            {
                List<Object.CustomField> consensus = DataAccess.Objects.GetCustomFields_Consensus(client);
                List<Object.CustomField> consensusAdhocs = new List<Object.CustomField>();
                consensusAdhocs = DataAccess.Objects.GetCustomFields_Consensus(client);
                if (includeAdHocs == true)
                    consensusAdhocs = DataAccess.Objects.GetCustomFields_ConsensusAdHoc(client);

                all.AddRange(consensus);
                all.AddRange(consensusAdhocs);
            }
            
            all.AddRange(products);
            all.AddRange(productAdhocs);
            
            return all;
        }
        public List<FrameworkUAD.Object.CustomField> GetCustomFields(KMPlatform.Object.ClientConnections client, string productCode = "")
        {
            List<Object.CustomField> products = DataAccess.Objects.GetCustomFields_Product(client);
            if (!string.IsNullOrEmpty(productCode))
                products = products.Where(x => x.ProductCode.Equals(productCode, StringComparison.CurrentCultureIgnoreCase)).ToList();
            List<Object.CustomField> productAdhocs = new List<Object.CustomField>();
            productAdhocs = DataAccess.Objects.GetCustomFields_AdHoc(client);
            if (!string.IsNullOrEmpty(productCode))
                productAdhocs = productAdhocs.Where(x => x.ProductCode.Equals(productCode, StringComparison.CurrentCultureIgnoreCase)).ToList();


            List<Object.CustomField> all = new List<Object.CustomField>();
            all.AddRange(products);
            all.AddRange(productAdhocs);
            return all;
        }
        public List<FrameworkUAD.Object.CustomField> GetConsensusCustomFields(KMPlatform.Object.ClientConnections client)
        {
            List<Object.CustomField> consensus = new List<Object.CustomField>();
            List<Object.CustomField> consensusAdhocs = new List<Object.CustomField>();
            consensus = DataAccess.Objects.GetCustomFields_Consensus(client);
            consensusAdhocs = DataAccess.Objects.GetCustomFields_ConsensusAdHoc(client);
            List<Object.CustomField> all = new List<Object.CustomField>();
            all.AddRange(consensus);
            all.AddRange(consensusAdhocs);
            return all;
        }
        public List<FrameworkUAD.Object.CustomFieldBrand> GetCustomFieldsBrand(KMPlatform.Object.ClientConnections client, string brandName = "")
        {
            List<Object.CustomFieldBrand> products = DataAccess.Objects.GetCustomFieldsBrand(client);
            if (!string.IsNullOrEmpty(brandName))
                products = products.Where(x => x.BrandName.Equals(brandName, StringComparison.CurrentCultureIgnoreCase)).ToList();

            return products;
        }
        public List<FrameworkUAD.Object.CustomFieldValue> GetCustomFieldValues(KMPlatform.Object.ClientConnections client, string productCode = "", string fieldName = "")
        {
            List<FrameworkUAD.Object.CustomFieldValue> list = DataAccess.Objects.GetCustomFieldValues(client);
            if (!string.IsNullOrEmpty(productCode))
                list = list.Where(x => x.ProductCode.Equals(productCode, StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (!string.IsNullOrEmpty(fieldName))
                list = list.Where(x => x.Name.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)).ToList();

            return list;
        }
        public List<FrameworkUAD.Object.CustomFieldValue> GetConsensusCustomFieldValues(KMPlatform.Object.ClientConnections client, string fieldName = "")
        {
            List<FrameworkUAD.Object.CustomFieldValue> list = DataAccess.Objects.GetConsensusCustomFieldValues(client);
            if (!string.IsNullOrEmpty(fieldName))
                list = list.Where(x => x.Name.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return list;
        }
    }
}
