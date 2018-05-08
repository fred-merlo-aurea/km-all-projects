using Core.ADMS.Events;
using FrameworkUAS.Entity;
using Core_AMS.Utilities;
using System.Data;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace ADMS.ClientMethods
{
    class Scranton : ClientSpecialCommon
    {
        private const string AdHocDimensionGroupNameScrantonDomainsSurvey = "Scranton_Domains_Survey";
        private const string AdHocDimensionGroupNameScrantonCompanySurvey = "Scranton_Company_Survey";
        private const string DimensionSurvey = "Survey";
        private const string MatchValueFieldDomainOnly = "Domain Only";
        private const string DimensionValueBdcgG300 = "BDCG300";
        private const string SurveyAdHocDimValue = "PBG300";
        private const string CompanyNameMatchField = "Company Name";
        private const string Email1MatchField = "Email1";
        private const string Email2MatchField = "Email2";

        public void SurveyAdHocImport(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = AdHocDimensionGroupNameScrantonCompanySurvey,
                    CreatedDimension = DimensionSurvey,
                    DimensionOperator = EqualOperation,
                    MatchValueFunc = row =>
                        GetRowMatchToken(row, CompanyNameMatchField, Email1MatchField, Email2MatchField),
                    DimensionValue = SurveyAdHocDimValue,
                    StandardField = StandardFieldCompany
                });
            ArchiveFile(eventMessage);
        }

        public void SurveyDomainsImport(FileMoved eventMessage)
        {
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(
                new FillAgGroupAndTableArgs()
                {
                    EventMessage = eventMessage,
                    AdHocDimensionGroupName = AdHocDimensionGroupNameScrantonDomainsSurvey,
                    CreatedDimension = DimensionSurvey,
                    DimensionOperator = EndsWithOperation,
                    MatchValueField = MatchValueFieldDomainOnly,
                    DimensionValue = DimensionValueBdcgG300,
                    StandardField = StandardFieldEmail
                });

            ArchiveFile(eventMessage);
        }
        public string emailCheck(string email)
        {
            if (email.Contains("@"))
            {
                //email = email.Substring(email.IndexOf('@') + 1);
                //if(email.Contains("."))
                //    email = email.Substring(0, email.IndexOf("."));

                string[] emailSplit = email.Split('@');
                email = emailSplit[1];
            }
            var hosts = new List<string> { "GMAIL", "MSN", "YAHOO", "AOL", "COMCAST", "HOTMAIL" };
            //string email1 = dr["Email1"].ToString();
            if (email.Contains("www."))
                email = email.Replace("www.", "");
            if (email.Contains("http://"))
                email = email.Replace("http://", "");
            //if (email.Contains(".com"))
            //    email = email.Replace(".com", "");
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
            List<AdHocDimension> companyList = new List<AdHocDimension>();
            List<AdHocDimension> domainList = new List<AdHocDimension>();
            int adgCompanyId = 0;
            int adgDomainId = 0;
            
            if (adgList.Any(x => x.AdHocDimensionGroupName.Equals("Scranton_Company_Survey")) == true)
                adgCompanyId = adgList.First(x => x.AdHocDimensionGroupName.Equals("Scranton_Company_Survey")).AdHocDimensionGroupId;
            
            if (adgList.Any(x => x.AdHocDimensionGroupName.Equals(AdHocDimensionGroupNameScrantonDomainsSurvey)) == true)
                adgDomainId = adgList.First(x => x.AdHocDimensionGroupName.Equals(AdHocDimensionGroupNameScrantonDomainsSurvey)).AdHocDimensionGroupId;
            
            if (adgCompanyId > 0)
                companyList = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).AdHocDimensions;
            
            if (adgDomainId > 0)
                domainList = adgList.Single(x => x.AdHocDimensionGroupId == adgDomainId).AdHocDimensions;

            Core_AMS.Utilities.FuzzySearch fs = new FuzzySearch();

            HashSet<string> matchCompanies = new HashSet<string>();
            HashSet<string> matchEmails = new HashSet<string>();
            foreach(AdHocDimension ad in companyList)
            {
                string[] values = ad.MatchValue.Split(':').ToArray();
                if(values != null && values.Count() == 3)
                {
                    if (!string.IsNullOrEmpty(values[0]))
                        matchCompanies.Add(values[0]);

                    if (!string.IsNullOrEmpty(values[1]))
                        matchEmails.Add(values[1]);

                    if (!string.IsNullOrEmpty(values[2]))
                        matchEmails.Add(values[2]);
                }
            }
            //Need the 
            foreach (FrameworkUAD.Entity.SubscriberTransformed st in data)
            {
                int pubId = clientPubCodes.Single(x => x.Value.ToLower().Equals(st.PubCode.ToLower())).Key;
                #region SURVEY = PBG300
                //if st.Company fuzzy matches OR st.Email domain matches then create SURVEY demo
                bool foundCompanyMatch = false;
                if (matchCompanies.Count > 0 && !string.IsNullOrEmpty(st.Company))
                {
                    if (matchCompanies.Any(x => fs.Search(st.Company, x) >= 80) == true)
                    {
                        foundCompanyMatch = true;
                        FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                        sdo.CreatedByUserID = st.CreatedByUserID;
                        sdo.DateCreated = DateTime.Now;
                        sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).CreatedDimension;
                        sdo.NotExists = false;
                        sdo.PubID = pubId;
                        sdo.SORecordIdentifier = st.SORecordIdentifier;
                        sdo.STRecordIdentifier = st.STRecordIdentifier;
                        sdo.Value = "PBG300";
                        sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                        sdo.IsAdhoc = true;

                        st.DemographicTransformedList.Add(sdo);
                    }
                }

                if (foundCompanyMatch == false && matchEmails.Count > 0)
                {
                    //'GMAIL', 'MSN', 'YAHOO', 'AOL', 'COMCAST', and 'HOTMAIL'
                    if(!string.IsNullOrEmpty(st.Email) && (!st.Email.ToLower().EndsWith("gmail.com") || !st.Email.ToLower().EndsWith("msn.com") || !st.Email.ToLower().EndsWith("yahoo.com") || !st.Email.ToLower().EndsWith("aol.com") || !st.Email.ToLower().EndsWith("comcast.com") || !st.Email.ToLower().EndsWith("hotmail.com")))
                    {
                        string[] profileEmailArray = st.Email.Split('@');
                        if (profileEmailArray.Length == 2 && !string.IsNullOrEmpty(profileEmailArray[1]))
                        {
                            if (matchEmails.Any(x => profileEmailArray[1].Equals(x, StringComparison.CurrentCultureIgnoreCase)) == true)
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                                sdo.CreatedByUserID = st.CreatedByUserID;
                                sdo.DateCreated = DateTime.Now;
                                sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == adgCompanyId).CreatedDimension;
                                sdo.NotExists = false;
                                sdo.PubID = pubId;
                                sdo.SORecordIdentifier = st.SORecordIdentifier;
                                sdo.STRecordIdentifier = st.STRecordIdentifier;
                                sdo.Value = "PBG300";
                                sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                                sdo.IsAdhoc = true;

                                st.DemographicTransformedList.Add(sdo);
                            }
                        }
                    }
                }
                #endregion

                if (domainList.Count > 0)
                {
                    if(!string.IsNullOrEmpty(st.Email) && domainList.Any(x => st.Email.ToLower().EndsWith(x.MatchValue.ToLower())) == true)
                    {
                        FrameworkUAD.Entity.SubscriberDemographicTransformed sdo = new FrameworkUAD.Entity.SubscriberDemographicTransformed();
                        sdo.CreatedByUserID = st.CreatedByUserID;
                        sdo.DateCreated = DateTime.Now;
                        sdo.MAFField = adgList.Single(x => x.AdHocDimensionGroupId == adgDomainId).CreatedDimension;
                        sdo.NotExists = false;
                        sdo.PubID = pubId;
                        sdo.SORecordIdentifier = st.SORecordIdentifier;
                        sdo.STRecordIdentifier = st.STRecordIdentifier;
                        sdo.Value = DimensionValueBdcgG300;
                        sdo.DemographicUpdateCodeId = demoUpdate.CodeId;
                        sdo.IsAdhoc = true;

                        st.DemographicTransformedList.Add(sdo);
                    }
                }
            }

            return data;
        }

        private string GetRowMatchToken(DataRow row, string companyNameField, string email1Field, string email2Field)
        {
            return string.Format(
                "{0}:{1}:{2}",
                row[companyNameField],
                emailCheck(row[email1Field].ToString()),
                emailCheck(row[email2Field].ToString()));
        }

        private static void ArchiveFile(FileMoved eventMessage)
        {
            if (File.Exists(eventMessage.ImportFile.FullName))
            {
                File.Move(
                    eventMessage.ImportFile.FullName,
                    string.Format(
                        "{0}\\{1}\\Processed\\{2}",
                        Core.ADMS.BaseDirs.getClientArchiveDir(),
                        eventMessage.Client.FtpFolder,
                        eventMessage.ImportFile.Name));
            }
        }
    }
}
