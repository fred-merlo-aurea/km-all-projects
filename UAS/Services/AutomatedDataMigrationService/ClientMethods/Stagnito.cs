using Core.ADMS.Events;
using FrameworkUAS.Entity;
using Core_AMS.Utilities;
using System.Data;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ADMS.ClientMethods
{
    class Stagnito : ClientSpecialCommon
    {
        private const string EnsambleCompanyIdField = "CompanyID";
        private const string ViolaCompanyIdField = "Company_ID";
        private const string EnsambleDim = "EnsembleIQCompanyID";
        private const string ViolaDim = "ViolaCompanyID";
        private const string ViolaDimensionGroup = "Stagnito_Company_Viola_CompanyID";
        private const string EnsambleDimensionGroup = "Stagnito_Company_EnsambleIQ_CompanyID";
        private const string MatchNameField = "Match Name";
        private const string MatchDomainField = "Match Domain";
        private const string CompanyFieldName = "company_name";
        private const string DomainFieldName = "domain";

        public void EnsambleCompanyIDAdHocImport(KMPlatform.Entity.Client c,FrameworkUAS.Entity.SourceFile sf,FrameworkUAS.Entity.ClientCustomProcedure ccp,FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = EnsambleDimensionGroup,
                    CreatedDimension = EnsambleDim,
                    DimensionOperator = EqualOperation,
                    MatchValueFunc = row => GetRowMatchToken(row, MatchNameField, MatchDomainField),
                    DimensionValueField = EnsambleCompanyIdField,
                    StandardField = CompanyStandardField
                });

            MoveProcessedToArchive(eventMessage);
        }

        public void ViolaCompanyIDAdHocImport(KMPlatform.Entity.Client c, FrameworkUAS.Entity.SourceFile sf, FrameworkUAS.Entity.ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = ViolaDimensionGroup,
                    CreatedDimension = ViolaDim,
                    DimensionOperator = EqualOperation,
                    MatchValueFunc = row => GetRowMatchToken(row, CompanyFieldName, DomainFieldName),
                    DimensionValueField = ViolaCompanyIdField,
                    StandardField = CompanyStandardField
                });

            MoveProcessedToArchive(eventMessage);
        }

        public string emailCheck(string email)
        {
            if (email.Contains("@"))
            {
                string[] emailSplit = email.Split('@');
                email = emailSplit[1];
            }
            var hosts = new List<string> { "GMAIL", "MSN", "YAHOO", "AOL", "COMCAST", "HOTMAIL" };

            if (email.Contains("www."))
                email = email.Replace("www.", "");
            if (email.Contains("http://"))
                email = email.Replace("http://", "");

            if (hosts.Contains(email, StringComparer.CurrentCultureIgnoreCase))
                email = "";
            return email;
        }
        public List<FrameworkUAD.Entity.SubscriberTransformed> CompanySurvey(List<FrameworkUAD.Entity.SubscriberTransformed> data, Dictionary<int, string> clientPubCodes, int clientId)
        {
            List<FrameworkUAD_Lookup.Entity.Code> demoUpdates = new List<FrameworkUAD_Lookup.Entity.Code>();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            demoUpdates = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);
            FrameworkUAD_Lookup.Entity.Code demoUpdate = demoUpdates.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString()));

            //execute in Validator.ValidateData after TransformedDedupe - 
            //at this point the incoing ST list has had pubs validated so we know everything exists and matches
            FrameworkUAS.BusinessLogic.AdHocDimensionGroup adgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            List<AdHocDimensionGroup> adgList = adgWorker.Select(clientId, true);
            List<AdHocDimension> EIQ_CompanyList = new List<AdHocDimension>();
            List<AdHocDimension> ViolaCompanyList = new List<AdHocDimension>();
            int EIQID = 0;
            int ViolaID = 0;

            if (adgList.Any(x => x.AdHocDimensionGroupName.Equals("Stagnito_Company_EnsambleIQ_CompanyID")) == true)
                EIQID = adgList.First(x => x.AdHocDimensionGroupName.Equals("Stagnito_Company_EnsambleIQ_CompanyID")).AdHocDimensionGroupId;

            if (adgList.Any(x => x.AdHocDimensionGroupName.Equals("Stagnito_Company_Viola_CompanyID")) == true)
                ViolaID = adgList.First(x => x.AdHocDimensionGroupName.Equals("Stagnito_Company_Viola_CompanyID")).AdHocDimensionGroupId;

            if (EIQID > 0)
                EIQ_CompanyList = adgList.Single(x => x.AdHocDimensionGroupId == EIQID).AdHocDimensions;

            if (ViolaID > 0)
                ViolaCompanyList = adgList.Single(x => x.AdHocDimensionGroupId == ViolaID).AdHocDimensions;

            Core_AMS.Utilities.FuzzySearch fs = new FuzzySearch();

            Dictionary<string, string> matchCompanies = new Dictionary<string, string>();
            Dictionary<string, string> matchEmails = new Dictionary<string, string>();
            foreach (AdHocDimension ad in EIQ_CompanyList)
            {
                string[] values = ad.MatchValue.Split(':').ToArray();
                if (values != null && values.Count() == 2)
                {
                    if (!string.IsNullOrEmpty(values[0]) && !(matchCompanies.ContainsKey(values[0])))
                        matchCompanies.Add(values[0], ad.DimensionValue);

                    if (!string.IsNullOrEmpty(values[1]) && !(matchEmails.ContainsKey(values[1])))
                        matchEmails.Add(values[1], ad.DimensionValue);
                }
            }
            //Need the 
            foreach (FrameworkUAD.Entity.SubscriberTransformed st in data)
            {
                int pubId = clientPubCodes.Single(x => x.Value.ToLower().Equals(st.PubCode.ToLower())).Key;
                #region Stagnito_Company_EnsambleIQ_CompanyID
                bool foundMatch = false;
                if (foundMatch == false && matchEmails.Count > 0)
                {
                    //'GMAIL', 'MSN', 'YAHOO', 'AOL', 'COMCAST', and 'HOTMAIL'
                    if (!string.IsNullOrEmpty(st.Email) && (!st.Email.ToLower().EndsWith("gmail.com") || !st.Email.ToLower().EndsWith("msn.com") || !st.Email.ToLower().EndsWith("yahoo.com") || !st.Email.ToLower().EndsWith("aol.com") || !st.Email.ToLower().EndsWith("comcast.com") || !st.Email.ToLower().EndsWith("hotmail.com")))
                    {
                        string[] profileEmailArray = st.Email.Split('@');
                        if (profileEmailArray.Length == 2 && !string.IsNullOrEmpty(profileEmailArray[1]))
                        {
                            if (matchEmails.Any(x => profileEmailArray[1].Equals(x.Key, StringComparison.CurrentCultureIgnoreCase)) == true)
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                                sdo.CreatedByUserID = st.CreatedByUserID;
                                sdo.DateCreated = DateTime.Now;
                                sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == EIQID).CreatedDimension;
                                sdo.NotExists = false;
                                sdo.PubID = pubId;
                                sdo.SORecordIdentifier = st.SORecordIdentifier;
                                sdo.STRecordIdentifier = st.STRecordIdentifier;
                                sdo.Value = matchEmails.FirstOrDefault(x => profileEmailArray[1].Equals(x.Key, StringComparison.CurrentCultureIgnoreCase)).Value;
                                sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                                sdo.IsAdhoc = true;

                                st.DemographicTransformedList.Add(sdo);
                                foundMatch = true;
                            }
                        }
                    }
                    //if st.Company fuzzy matches OR st.Email domain matches then create SURVEY demo
                    if (matchCompanies.Count > 0 && !string.IsNullOrEmpty(st.Company) && foundMatch == false)
                    {
                        if (matchCompanies.Any(x => fs.Search(st.Company, x.Key) >= 80) == true)
                        {
                            foundMatch = true;
                            FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                            sdo.CreatedByUserID = st.CreatedByUserID;
                            sdo.DateCreated = DateTime.Now;
                            sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == EIQID).CreatedDimension;
                            sdo.NotExists = false;
                            sdo.PubID = pubId;
                            sdo.SORecordIdentifier = st.SORecordIdentifier;
                            sdo.STRecordIdentifier = st.STRecordIdentifier;
                            sdo.Value = matchCompanies.FirstOrDefault(x => fs.Search(st.Company, x.Key) >= 80).Value;
                            sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                            sdo.IsAdhoc = true;

                            st.DemographicTransformedList.Add(sdo);
                        }
                    }
                }
            }
            matchCompanies.Clear();
            matchEmails.Clear();
            foreach (AdHocDimension ad in ViolaCompanyList)
            {
                string[] values = ad.MatchValue.Split(':').ToArray();
                if (values != null && values.Count() == 2)
                {
                    if (!string.IsNullOrEmpty(values[0]) && !(matchCompanies.ContainsKey(values[0])))
                        matchCompanies.Add(values[0], ad.DimensionValue);

                    if (!string.IsNullOrEmpty(values[1]) && !(matchEmails.ContainsKey(values[1])))
                        matchEmails.Add(values[1], ad.DimensionValue);
                }
            }
            foreach (FrameworkUAD.Entity.SubscriberTransformed st in data)
            {
                #endregion
                #region Stagnito_Company_Viola_Company
                bool foundMatch = false;
                int pubId = clientPubCodes.Single(x => x.Value.ToLower().Equals(st.PubCode.ToLower())).Key;
                
                if (foundMatch == false && matchEmails.Count > 0)
                {
                    //'GMAIL', 'MSN', 'YAHOO', 'AOL', 'COMCAST', and 'HOTMAIL'
                    if (!string.IsNullOrEmpty(st.Email) && (!st.Email.ToLower().EndsWith("gmail.com") || !st.Email.ToLower().EndsWith("msn.com") || !st.Email.ToLower().EndsWith("yahoo.com") || !st.Email.ToLower().EndsWith("aol.com") || !st.Email.ToLower().EndsWith("comcast.com") || !st.Email.ToLower().EndsWith("hotmail.com")))
                    {
                        string[] profileEmailArray = st.Email.Split('@');
                        if (profileEmailArray.Length == 2 && !string.IsNullOrEmpty(profileEmailArray[1]))
                        {
                            if (matchEmails.Any(x => profileEmailArray[1].Equals(x.Key, StringComparison.CurrentCultureIgnoreCase)) == true)
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                                sdo.CreatedByUserID = st.CreatedByUserID;
                                sdo.DateCreated = DateTime.Now;
                                sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == ViolaID).CreatedDimension;
                                sdo.NotExists = false;
                                sdo.PubID = pubId;
                                sdo.SORecordIdentifier = st.SORecordIdentifier;
                                sdo.STRecordIdentifier = st.STRecordIdentifier;
                                sdo.Value = matchEmails.FirstOrDefault(x => profileEmailArray[1].Equals(x.Key, StringComparison.CurrentCultureIgnoreCase)).Value;
                                sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                                sdo.IsAdhoc = true;

                                st.DemographicTransformedList.Add(sdo);
                                foundMatch = true;
                            }
                        }
                    }
                    //if st.Company fuzzy matches OR st.Email domain matches then create SURVEY demo
                    if (matchCompanies.Count > 0 && !string.IsNullOrEmpty(st.Company) && foundMatch == false)
                    {
                        if (matchCompanies.Any(x => fs.Search(st.Company, x.Key) >= 80) == true)
                        {
                            foundMatch = true;
                            FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                            sdo.CreatedByUserID = st.CreatedByUserID;
                            sdo.DateCreated = DateTime.Now;
                            sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == ViolaID).CreatedDimension;
                            sdo.NotExists = false;
                            sdo.PubID = pubId;
                            sdo.SORecordIdentifier = st.SORecordIdentifier;
                            sdo.STRecordIdentifier = st.STRecordIdentifier;
                            sdo.Value = matchCompanies.FirstOrDefault(x => fs.Search(st.Company, x.Key) >= 80).Value;
                            sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                            sdo.IsAdhoc = true;

                            st.DemographicTransformedList.Add(sdo);
                        }
                    }
                }
            #endregion
            }
            return data;
        }

        private string GetRowMatchToken(DataRow row, string companyFieldName, string domainFieldName)
        {
            return string.Format("{0}:{1}", row[companyFieldName], emailCheck(row[domainFieldName].ToString()));
        }

        private static void MoveProcessedToArchive(FileMoved eventMessage)
        {
            if (System.IO.File.Exists(eventMessage.ImportFile.FullName))
            {
                System.IO.File.Move(eventMessage.ImportFile.FullName,
                    string.Format(
                        "{0}\\{1}\\Processed\\{2}",
                        Core.ADMS.BaseDirs.getClientArchiveDir(),
                        eventMessage.Client.FtpFolder,
                        eventMessage.ImportFile.Name));
            }
        }
    }
}
