using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkSubGen.BusinessLogic
{
    public class ImportSubscriber
    {
        public List<Entity.ImportSubscriber> Select(int accountId, bool isMergedToUAD)
        {
            List<Entity.ImportSubscriber> retList = null;
            retList = DataAccess.ImportSubscriber.Select(accountId, isMergedToUAD).ToList();

            BusinessLogic.ImportDimension idWorker = new ImportDimension();
            List<Entity.ImportDimension> idList = idWorker.Select(accountId, isMergedToUAD);
            foreach (Entity.ImportSubscriber iSub in retList)
                iSub.Dimensions = idList.FirstOrDefault(x => x.SystemSubscriberID == iSub.SystemSubscriberID && x.SubscriptionID == iSub.SubscriptionID && x.PublicationID == iSub.PublicationID);

            //doing like this will be several calls - get bulk then join up
            //foreach (Entity.ImportSubscriber iSub in retList)
            //{
            //    iSub.Dimensions = idWorker.Select(accountId, iSub.SystemSubscriberID, iSub.SubscriptionID, iSub.PublicationID);
            //}
            return retList;
        }
        public bool Save(List<Entity.ImportSubscriber> list)
        {
            bool done = false;
            foreach (Entity.ImportSubscriber x in list)
            {

            }
            return done;
        }
        public bool UpdateMergedToUAD(List<Entity.ImportSubscriber> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<XML>");
            foreach (Entity.ImportSubscriber x in list)
            {
                sb.AppendLine("<ImportSubscriber>");
                sb.AppendLine("<SystemSubscriberID>" + x.SystemSubscriberID.ToString() + "</SystemSubscriberID>");
                sb.AppendLine("<SubscriptionID>" + x.SubscriptionID.ToString() + "</SubscriptionID>");
                sb.AppendLine("<PublicationID>" + x.PublicationID.ToString() + "</PublicationID>");
                sb.AppendLine("<account_id>" + x.account_id.ToString() + "</account_id>");
                sb.AppendLine("</ImportSubscriber>");
            }
            sb.AppendLine("</XML>");

            return DataAccess.ImportSubscriber.UpdateMergedToUAD(sb.ToString());
        }
        public bool JobImportSubscriberFile(List<Entity.ImportSubscriber> list)
        {
            list = CleanForXml(list);
            bool done = false;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            int checkCount = 1;
            //batch this in 250 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.ImportSubscriber x in list)
            {
                checkCount++;
                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.ImportSubscriber>(x));

                sbXML.AppendLine(xmlObject);
                counter++;
                processedCount++;
                done = false;
                if (processedCount == total || counter == BatchSize)
                {
                    try
                    {
                        //check this xml - need ImportDimensions and lists of ImportDimensionDetail
                        //ImportDimensionDetail still need the PubCode_ stripped from DimensionField
                        DataAccess.ImportSubscriber.JobImportSubscriberFile("<XML>" + sbXML.ToString() + "</XML>");
                        done = true;
                    }
                    catch (Exception ex)
                    {
                        done = false;
                        API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                    }
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
            return done;
        }
        public List<Entity.ImportSubscriber> CleanForXml(List<Entity.ImportSubscriber> list)
        {
            foreach (var x in list)
            {
                if (!string.IsNullOrEmpty(x.AuditCategoryCode))
                    x.AuditCategoryCode = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.AuditCategoryCode);
                if (!string.IsNullOrEmpty(x.AuditCategoryName))
                    x.AuditCategoryName = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.AuditCategoryName);
                if (!string.IsNullOrEmpty(x.AuditRequestTypeCode))
                    x.AuditRequestTypeCode = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.AuditRequestTypeCode);
                if (!string.IsNullOrEmpty(x.AuditRequestTypeName))
                    x.AuditRequestTypeName = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.AuditRequestTypeName);
                if (!string.IsNullOrEmpty(x.BillingAddressCity))
                    x.BillingAddressCity = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.BillingAddressCity);
                if (!string.IsNullOrEmpty(x.BillingAddressCompany))
                    x.BillingAddressCompany = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.BillingAddressCompany);
                if (!string.IsNullOrEmpty(x.BillingAddressFirstName))
                    x.BillingAddressFirstName = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.BillingAddressFirstName);
                if (!string.IsNullOrEmpty(x.BillingAddressLastName))
                    x.BillingAddressLastName = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.BillingAddressLastName);
                if (!string.IsNullOrEmpty(x.BillingAddressLine1))
                    x.BillingAddressLine1 = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.BillingAddressLine1);
                if (!string.IsNullOrEmpty(x.BillingAddressState))
                    x.BillingAddressState = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.BillingAddressState);
                if (!string.IsNullOrEmpty(x.BillingAddressZip))
                    x.BillingAddressZip = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.BillingAddressZip);
                if (!string.IsNullOrEmpty(x.BillingAddressCountry))
                    x.BillingAddressCountry = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.BillingAddressCountry);
                if (!string.IsNullOrEmpty(x.MailingAddressCity))
                    x.MailingAddressCity = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.MailingAddressCity);
                if (!string.IsNullOrEmpty(x.MailingAddressCompany))
                    x.MailingAddressCompany = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.MailingAddressCompany);
                if (!string.IsNullOrEmpty(x.MailingAddressFirstName))
                    x.MailingAddressFirstName = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.MailingAddressFirstName);
                if (!string.IsNullOrEmpty(x.MailingAddressLastName))
                    x.MailingAddressLastName = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.MailingAddressLastName);
                if (!string.IsNullOrEmpty(x.MailingAddressLine1))
                    x.MailingAddressLine1 = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.MailingAddressLine1);
                if (!string.IsNullOrEmpty(x.MailingAddressState))
                    x.MailingAddressState = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.MailingAddressState);
                if (!string.IsNullOrEmpty(x.MailingAddressTitle))
                    x.MailingAddressTitle = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.MailingAddressTitle);
                if (!string.IsNullOrEmpty(x.MailingAddressZip))
                    x.MailingAddressZip = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.MailingAddressZip);
                if (!string.IsNullOrEmpty(x.MailingAddressCountry))
                    x.MailingAddressCountry = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.MailingAddressCountry);
                if (!string.IsNullOrEmpty(x.PublicationName))
                    x.PublicationName = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.PublicationName);
                if (!string.IsNullOrEmpty(x.RenewalCode_CustomerID))
                    x.RenewalCode_CustomerID = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.RenewalCode_CustomerID);
                if (!string.IsNullOrEmpty(x.SubscriberAccountFirstName))
                    x.SubscriberAccountFirstName = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.SubscriberAccountFirstName);
                if (!string.IsNullOrEmpty(x.SubscriberAccountLastName))
                    x.SubscriberAccountLastName = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.SubscriberAccountLastName);
                if (!string.IsNullOrEmpty(x.SubscriberEmail))
                    x.SubscriberEmail = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.SubscriberEmail);
                if (!string.IsNullOrEmpty(x.SubscriberPhone))
                    x.SubscriberPhone = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.SubscriberPhone);
                if (!string.IsNullOrEmpty(x.SubscriberSource))
                    x.SubscriberSource = Core_AMS.Utilities.XmlFunctions.CleanAllXml(x.SubscriberSource);
            }
            return list;
        }
        public List<Entity.ImportSubscriber> RemoveXmlFormatting(List<Entity.ImportSubscriber> list)
        {
            foreach (var x in list)
            {
                if (!string.IsNullOrEmpty(x.AuditCategoryCode))
                    x.AuditCategoryCode = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.AuditCategoryCode);
                if (!string.IsNullOrEmpty(x.AuditCategoryName))
                    x.AuditCategoryName = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.AuditCategoryName);
                if (!string.IsNullOrEmpty(x.AuditRequestTypeCode))
                    x.AuditRequestTypeCode = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.AuditRequestTypeCode);
                if (!string.IsNullOrEmpty(x.AuditRequestTypeName))
                    x.AuditRequestTypeName = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.AuditRequestTypeName);
                if (!string.IsNullOrEmpty(x.BillingAddressCity))
                    x.BillingAddressCity = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.BillingAddressCity);
                if (!string.IsNullOrEmpty(x.BillingAddressCompany))
                    x.BillingAddressCompany = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.BillingAddressCompany);
                if (!string.IsNullOrEmpty(x.BillingAddressFirstName))
                    x.BillingAddressFirstName = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.BillingAddressFirstName);
                if (!string.IsNullOrEmpty(x.BillingAddressLastName))
                    x.BillingAddressLastName = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.BillingAddressLastName);
                if (!string.IsNullOrEmpty(x.BillingAddressLine1))
                    x.BillingAddressLine1 = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.BillingAddressLine1);
                if (!string.IsNullOrEmpty(x.BillingAddressState))
                    x.BillingAddressState = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.BillingAddressState);
                if (!string.IsNullOrEmpty(x.BillingAddressZip))
                    x.BillingAddressZip = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.BillingAddressZip);
                if (!string.IsNullOrEmpty(x.BillingAddressCountry))
                    x.BillingAddressCountry = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.BillingAddressCountry);
                if (!string.IsNullOrEmpty(x.MailingAddressCity))
                    x.MailingAddressCity = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.MailingAddressCity);
                if (!string.IsNullOrEmpty(x.MailingAddressCompany))
                    x.MailingAddressCompany = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.MailingAddressCompany);
                if (!string.IsNullOrEmpty(x.MailingAddressFirstName))
                    x.MailingAddressFirstName = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.MailingAddressFirstName);
                if (!string.IsNullOrEmpty(x.MailingAddressLastName))
                    x.MailingAddressLastName = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.MailingAddressLastName);
                if (!string.IsNullOrEmpty(x.MailingAddressLine1))
                    x.MailingAddressLine1 = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.MailingAddressLine1);
                if (!string.IsNullOrEmpty(x.MailingAddressState))
                    x.MailingAddressState = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.MailingAddressState);
                if (!string.IsNullOrEmpty(x.MailingAddressTitle))
                    x.MailingAddressTitle = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.MailingAddressTitle);
                if (!string.IsNullOrEmpty(x.MailingAddressZip))
                    x.MailingAddressZip = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.MailingAddressZip);
                if (!string.IsNullOrEmpty(x.MailingAddressCountry))
                    x.MailingAddressCountry = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.MailingAddressCountry);
                if (!string.IsNullOrEmpty(x.PublicationName))
                    x.PublicationName = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.PublicationName);
                if (!string.IsNullOrEmpty(x.RenewalCode_CustomerID))
                    x.RenewalCode_CustomerID = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.RenewalCode_CustomerID);
                if (!string.IsNullOrEmpty(x.SubscriberAccountFirstName))
                    x.SubscriberAccountFirstName = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.SubscriberAccountFirstName);
                if (!string.IsNullOrEmpty(x.SubscriberAccountLastName))
                    x.SubscriberAccountLastName = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.SubscriberAccountLastName);
                if (!string.IsNullOrEmpty(x.SubscriberEmail))
                    x.SubscriberEmail = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.SubscriberEmail);
                if (!string.IsNullOrEmpty(x.SubscriberPhone))
                    x.SubscriberPhone = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.SubscriberPhone);
                if (!string.IsNullOrEmpty(x.SubscriberSource))
                    x.SubscriberSource = Core_AMS.Utilities.XmlFunctions.RemoveFormatXMLSpecialCharacters(x.SubscriberSource);
            }
            return list;
        }
    }
}
