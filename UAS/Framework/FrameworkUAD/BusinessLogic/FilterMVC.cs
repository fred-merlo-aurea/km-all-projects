using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameworkUAD.Object;
using KM.Common.Extensions;
using KMPlatform.Object;

namespace FrameworkUAD.BusinessLogic
{
    public class FilterMVC
    {
        private const string DateConditionDateRange = "DATERANGE";
        private const string DateConditionXDays = "XDAYS";
        private const string DateConditionYear = "YEAR";
        private const string DateConditionMonth = "MONTH";
        private const string DateConditionExpressionToday = "EXP:Today";

        private const string ProfilePermissionMail = "MAILPERMISSION";
        private const string ProfilePermissionFax = "FAXPERMISSION";
        private const string ProfilePermissionPhone = "PHONEPERMISSION";
        private const string ProfilePermissionOtherProducts = "OTHERPRODUCTSPERMISSION";
        private const string ProfilePermissionThirdParty = "THIRDPARTYPERMISSION";
        private const string ProfilePermissionEmailRenew = "EMAILRENEWPERMISSION";
        private const string ProfilePermissionText = "TEXTPERMISSION";
        private const string ProfileEmail = "EMAIL";
        private const string ProfilePhone = "PHONE";
        private const string ProfileFax = "FAX";

        private const string BlastSendTimeColumnName = "bla.SendTime";
        private const string CategoryCode = "CATEGORY CODE";
        private const string TransactionCode = "TRANSACTION CODE";
        private const string QSourceCode = "QSOURCE CODE";
        private const string FilterResultFormatString = "<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>";
        private const string QueryFilterNoCdataFormatString = "<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>";
        private const char CommaSeparator = ',';
        private const string ClickSearchTypeSearchAll = "Search All";

        private static readonly DateTime d1900 = new DateTime(1900, 1, 1);

        public static Object.FilterMVC ExecuteFilter(KMPlatform.Object.ClientConnections clientconnection,  Object.FilterMVC filter)
        {
            var x = DataAccess.FilterMVC.ExecuteFilter(clientconnection, getFilterQuery(filter, "distinct ps.SubscriptionID", "", clientconnection), filter);
            return x;
        }
        public static Object.FilterMVC GetCounts(KMPlatform.Object.ClientConnections clientconnection, Object.FilterMVC filter)
        {
            var x = DataAccess.FilterMVC.GetCounts(clientconnection, getFilterQuery(filter, " count(distinct ps.SubscriptionID) ", "", clientconnection), filter);
            return x;

        }
        public static Object.FilterMVC GetFilterByID(KMPlatform.Object.ClientConnections clientconnection,int filterID)
        {
            Object.FilterMVC retItem = new Object.FilterMVC();
            retItem.Fields = new List<Object.FilterDetails>();
            retItem = DataAccess.FilterMVC.GetFilterByID(clientconnection, filterID);
            retItem.Fields = DataAccess.FilterDetails.getByFilterID(clientconnection,filterID).ToList();

            return retItem;
        }
        public static Object.FilterMVC Execute(KMPlatform.Object.ClientConnections clientconnection, Object.FilterMVC filter, string AddlFilters)
        {
            var x = DataAccess.FilterMVC.Execute(clientconnection, filter, getFilterQuery(filter, "count(distinct ps.SubscriptionID) ", AddlFilters, clientconnection));
            return x;
            
        }
        public static List<int> getSubscriber(KMPlatform.Object.ClientConnections clientconnection, string AddlFilters,  Object.FilterMVC filter)
        {
            var x = DataAccess.FilterMVC.getSubscriber(clientconnection, filter, getFilterQuery(filter, "distinct ps.SubscriptionID", AddlFilters, clientconnection));
            return x;
        }
        public static List<int> getProductArchiveSubscriber(KMPlatform.Object.ClientConnections clientconnection, string AddlFilters, Object.FilterMVC filter,int IssueID)
        {
            var x = DataAccess.FilterMVC.getSubscriber(clientconnection, filter, getProductArchiveFilterQuery(filter, IssueID, clientconnection));
            return x;
        }
        public static string getFilterQuery(Object.FilterMVC filter, KMPlatform.Object.ClientConnections clientconnection)
        {
            return (getFilterQuery(filter, "distinct ps.SubscriptionID", "", clientconnection));
        }
        public static string getProductFilterQuery(Object.FilterMVC filter, KMPlatform.Object.ClientConnections clientconnection, bool IsAddRemove=false)
        {
            return (getFilterQuery(filter, "distinct ps.PubSubscriptionID", "", clientconnection, IsAddRemove));
        }
        public static string getProductArchiveFilterQuery(Object.FilterMVC filter, int IssueID, KMPlatform.Object.ClientConnections clientconnection)
        {
            return (getProductArchiveFilterQuery(filter, "distinct ps.PubSubscriptionID", "", IssueID, clientconnection));
        }
        public static string getProductArchiveFilterQuery(Object.FilterMVC filter, string select_list, string addlFilters, int IssueID, KMPlatform.Object.ClientConnections clientconnection)
        {
            var pubIDs = string.Empty;
            var query = string.Empty;
            var createTempTablequery = string.Empty;
            var dropTempTablequery = string.Empty;
            
            // Outer Query that will finally run... Get the data needed + any additional tables for the final XLS output
            var whereCondition = " IssueID = " + IssueID;
            var includeCategoryTypeCondition = !FieldNameExists(filter, CategoryCode);
            var includeXactTypeCondition = !FieldNameExists(filter, TransactionCode);
            var includeQSourceTypeCondition = !FieldNameExists(filter, QSourceCode);

            #region profile fields

            var profilefields = filter.Fields
                                    .Where(f => f.FilterType != Enums.FiltersType.Dimension && f.FilterType != Enums.FiltersType.Activity)
                                    .ToList();

            foreach (var field in profilefields)
            {
                whereCondition = GetConditionBeginning(whereCondition, field);

                switch (field.Name.ToUpper())
                {
                    case "PRODUCT":
                        pubIDs = field.Values;
                        whereCondition += "ps.pubid in (" + field.Values + " ) ";
                        break;
                    case "DATACOMPARE":

                        string[] strDataCompare = field.Values.Split('|');

                        string dataCompareType = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeId(Convert.ToInt32(strDataCompare[1])).CodeName;
                        if (string.Equals(dataCompareType, "Match", StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join DataCompareProfile d with(nolock) on s.IGRP_NO = d.IGrp_No ";
                            whereCondition += "d.ProcessCode = '" + strDataCompare[0] + "'";
                        }
                        else
                        {
                            query += " left outer join DataCompareProfile d with(nolock) on s.IGRP_NO = d.IGrp_No and  d.ProcessCode = '" + strDataCompare[0] + "'";
                            whereCondition += "d.SubscriberFinalId is null ";
                        }
                        break;
                    case "STATE":
                        whereCondition += "ps.RegionCode in ('" + field.Values.Replace(",", "','") + "') ";
                        break;

                    case "COUNTRY":

                        string[] strCountryId = field.Values.Split(',');

                        whereCondition += "(";

                        {
                            for (int i = 0; i < strCountryId.Length; i++)
                            {
                                if (strCountryId[i] == "1")
                                {
                                    whereCondition += (i > 0 ? " OR " : "") + "ps.CountryID = 1";
                                }
                                else if (strCountryId[i] == "3")
                                {
                                    whereCondition += (i > 0 ? " OR " : "") + "((ps.CountryID = 1) or (ps.CountryID = 2)) ";
                                }
                                else if (strCountryId[i] == "4")
                                {
                                    whereCondition += (i > 0 ? " OR " : "") + "not ((ps.CountryID = 1) or (ps.CountryID = 2) or ISNULL(ps.CountryID,0) = 0)";
                                }
                                else
                                {
                                    whereCondition += (i > 0 ? " OR " : "") + "ps.CountryID in ( " + strCountryId[i] + " ) ";
                                }
                            }
                        }

                        whereCondition += ")";

                        break;

                    case ProfileEmail:
                        whereCondition += CreateProfileCondition("ps.Email", string.Empty, field.Values);
                        break;
                    case ProfilePhone:
                        whereCondition += CreateProfileCondition("ps.Phone", string.Empty, field.Values);
                        break;
                    case ProfileFax:
                        whereCondition += CreateProfileCondition("ps.fax", string.Empty, field.Values);
                        break;
                    case "GEOLOCATED":
                        string[] strIsLatLonValid = field.Values.Split(',');

                        whereCondition += "(";
                        for (int i = 0; i < strIsLatLonValid.Length; i++)
                        {
                            whereCondition += (i > 0 ? " OR " : "") + "s.IsLatLonValid = " + strIsLatLonValid[i];
                        }

                        whereCondition += ")";

                        break;
                    case ProfilePermissionMail:
                        whereCondition += CreatePermissionCondition("MailPermission", field.Values);
                        break;
                    case ProfilePermissionFax:
                        whereCondition += CreatePermissionCondition("FaxPermission", field.Values);
                        break;
                    case ProfilePermissionPhone:
                        whereCondition += CreatePermissionCondition("PhonePermission", field.Values);
                        break;
                    case ProfilePermissionOtherProducts:
                        whereCondition += CreatePermissionCondition("OtherProductsPermission", field.Values);
                        break;
                    case ProfilePermissionThirdParty:
                        whereCondition += CreatePermissionCondition("ThirdPartyPermission", field.Values);
                        break;
                    case ProfilePermissionEmailRenew:
                        whereCondition += CreatePermissionCondition("EmailRenewPermission", field.Values);
                        break;
                    case ProfilePermissionText:
                        whereCondition += CreatePermissionCondition("TextPermission", field.Values);
                        break;
                    case "MEDIA":
                        whereCondition += "ps.Demo7 in (  '" + field.Values.Replace(",", "','") + "') ";
                        break;
                    case "QFROM":
                        whereCondition += " ps.QualificationDate >= '" + field.Values + "' ";
                        break;
                    case "QTO":
                        whereCondition += " ps.QualificationDate <= '" + field.Values + " 23:59:59' ";
                        break;
                    case "EMAIL STATUS":
                        whereCondition += "ps.EmailStatusID in (" + field.Values + ") ";
                        break;
                    case CategoryCode:
                        whereCondition += "ps.PubCategoryID in ( " + field.Values + " ) ";
                        break;
                    case "CATEGORY TYPE":
                        if (includeCategoryTypeCondition)
                           whereCondition += "ISNULL(sak.PubCategoryID, ps.PubCategoryID) in (select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock) where CategoryCodeTypeID in ( " + field.Values + " ) )";
                        break;
                    case TransactionCode:
                        whereCondition += "ps.PubTransactionID in ( " + field.Values + " )";
                        break;
                    case "XACT":
                        if (includeXactTypeCondition)
                            whereCondition += "ISNULL(sak.PubTransactionID,ps.PubTransactionID) in (select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock) where TransactionCodeTypeID in ( " + field.Values + " ) )";
                        break;
                    case QSourceCode:
                        whereCondition += "ps.PubQSourceID in ( " + field.Values + " ) ";
                        break;
                    case "QSOURCE TYPE":
                        if (includeQSourceTypeCondition)
                            whereCondition += "ps.PubQSourceID in (select CodeID from UAD_Lookup..Code with (nolock) where ParentCodeId in ( " + field.Values + " ) )";
                        break;
                    case "ADHOC":
                        #region Adhoc support
                        //sub_query += "sfilter." + node.Attributes["column"].Value + " like '" + field.Values + "%'";
                        //wgh either add this to subquery or get rid of the last " and "
                        //sub_query = sub_query.Remove(sub_query.Length - 5);

                        if (field.Group.Contains("|"))
                        {
                            string[] strID = field.Group.Split('|');
                            if (strID[0] == "d")
                            {
                                var i = 0;

                                string wherecondition_field = string.Empty;

                                if (strID[1].ToLower().Contains("qdate") || strID[1].ToLower().Contains("qualificationdate"))
                                {
                                    wherecondition_field = "ps.QualificationDate";
                                }
                                else if (strID[1].ToLower().Contains("[transactiondate]") || strID[1].ToLower().Contains("[pubtransactiondate]"))
                                {
                                    wherecondition_field = "convert(date,ps." + strID[1] + ")";
                                }
                                else if (strID[1].ToLower().Contains("[statusupdateddate]"))
                                {
                                    wherecondition_field = "convert(date,ps." + strID[1] + ")";
                                }
                                else if (strID[1].ToLower().Contains("datecreated") || strID[1].ToLower().Contains("dateupdated"))
                                {
                                    wherecondition_field = "ps." + strID[1];
                                }
                                else
                                {
                                    wherecondition_field = "ps." + strID[1];
                                }

                                var builder = new StringBuilder(whereCondition);
                                AppendDateCondition(builder, wherecondition_field, field.SearchCondition, field.Values);
                                whereCondition = builder.ToString();
                            }
                            else if (strID[0] == "i" || strID[0] == "f")
                            {

                                whereCondition += "(";

                                string[] strValue = field.Values.Split('|');

                                int i = 0;

                                switch (field.SearchCondition.ToUpper())
                                {
                                    case "EQUAL":

                                        if (strValue[0] != "")
                                        {
                                            whereCondition += "ps." + strID[1] + " = " + strValue[0];
                                            i = 1;
                                        }
                                        break;
                                    case "RANGE":

                                        if (strValue[0] != "")
                                        {
                                            whereCondition += "ps." + strID[1] + " >= " + strValue[0];
                                            i = 1;
                                        }

                                        if (strValue[1] != "")
                                        {
                                            whereCondition += (i > 0 ? " and " : "") + "ps." + strID[1] + " <= " + strValue[1];
                                        }
                                        break;
                                    case "GREATER":

                                        if (strValue[0] != "")
                                        {
                                            whereCondition += "ps." + strID[1] + " > " + strValue[0];
                                            i = 1;
                                        }
                                        break;
                                    case "LESSER":

                                        if (strValue[0] != "")
                                        {
                                            whereCondition += "ps." + strID[1] + " < " + strValue[0];
                                            i = 1;
                                        }
                                        break;
                                }

                                whereCondition += ")";

                            }
                            else if (strID[0] == "e")
                            {

                                if (field.SearchCondition.ToUpper() != "DOES NOT CONTAIN" && field.SearchCondition.ToUpper() != "IS EMPTY")
                                {
                                    whereCondition += " ps.PubSubscriptionID in (select ips.PubSubscriptionID" +
                                                      " FROM IssueArchivePubSubscriptionsExtension E with (nolock) join IssueArchiveProductSubscription ips on ips.IssueArchiveSubscriptionId = E.IssueArchiveSubscriptionId" +
                                                      " WHERE ips.issueID = " + IssueID + " and ";
                                }
                                else
                                {
                                    whereCondition += " ps.PubSubscriptionID not in (select ips.PubSubscriptionID" +
                                                      " FROM IssueArchivePubSubscriptionsExtension E with (nolock) join IssueArchiveProductSubscription ips on ips.IssueArchiveSubscriptionId = E.IssueArchiveSubscriptionId" +
                                                      " WHERE ips.issueID = " + IssueID + " and ";
                                }

                                string columnName = string.Concat("E.", strID[1]);

                                switch (strID[2].ToLower())
                                {
                                    case "i":
                                    case "f":
                                        {
                                            columnName = string.Compare(strID[2], "i", true) == 0
                                                ? string.Format("CAST({0} AS INT)", columnName)
                                                : string.Format("CAST({0} AS FLOAT)", columnName);

                                            string[] strValue = field.Values.Split('|');
                                            whereCondition += "(";

                                            switch (field.SearchCondition.ToUpper())
                                            {
                                                case "EQUAL":

                                                    if (strValue[0] != "")
                                                    {
                                                        whereCondition += columnName + " = " + strValue[0];
                                                    }
                                                    break;
                                                case "RANGE":

                                                    if (strValue[0] != "")
                                                    {
                                                        whereCondition += columnName + " >= " + strValue[0];
                                                    }

                                                    if (strValue[1] != "")
                                                    {
                                                        whereCondition += (strValue[0] != "" ? " and " : " ") + columnName + " <= " + strValue[1];
                                                    }
                                                    break;
                                                case "GREATER":

                                                    if (strValue[0] != "")
                                                    {
                                                        whereCondition += columnName + " > " + strValue[0];
                                                    }
                                                    break;
                                                case "LESSER":

                                                    if (strValue[0] != "")
                                                    {
                                                        whereCondition += columnName + " < " + strValue[0];
                                                    }
                                                    break;
                                            }

                                            whereCondition += "))";
                                            break;
                                        }
                                    case "b":
                                        {
                                            whereCondition += string.Format("CAST({0} AS BIT) = {1})", columnName, field.Values);
                                            break;
                                        }
                                    case "d":
                                        {
                                            columnName = string.Format("case when IsDate({0}) = 1 then CAST({0} AS DATETIME) else null end", columnName);
                                            var builder = new StringBuilder(whereCondition);
                                            AppendDateCondition(builder, columnName, field.SearchCondition, field.Values);
                                            whereCondition = builder.ToString();
                                            break;
                                        }
                                    default:
                                        {
                                            string[] strAdhoc = field.Values.Split(',');

                                            for (int i = 0; i < strAdhoc.Length; i++)
                                            {
                                                switch (field.SearchCondition.ToUpper())
                                                {
                                                    case "EQUAL":
                                                        whereCondition += (i > 0 ? " OR " : "") + " " + columnName + " = '" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()) + "'";
                                                        break;
                                                    case "CONTAINS":
                                                    case "DOES NOT CONTAIN":
                                                        whereCondition += (i > 0 ? " OR " : "") + " " + "PATINDEX('%" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "%', " + columnName + ") > 0 ";
                                                        break;
                                                    case "START WITH":
                                                        whereCondition += (i > 0 ? " OR " : "") + " " + "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "%', " + columnName + ") > 0 ";
                                                        break;
                                                    case "END WITH":
                                                        whereCondition += (i > 0 ? " OR " : "") + " " + "PATINDEX('%" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "', " + columnName + ") > 0 ";
                                                        break;
                                                    case "IS EMPTY":
                                                    case "IS NOT EMPTY":
                                                        whereCondition += (i > 0 ? " OR " : "") + " (ISNULL(" + columnName + ", '') != '' " + " )";
                                                        break;
                                                }
                                            }

                                            whereCondition += ")";
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        {
                            string wherecondition_field = string.Empty;

                            if (field.Group.Equals("[COUNTRY]", StringComparison.OrdinalIgnoreCase))
                            {

                                if (field.SearchCondition.Equals("IS EMPTY", StringComparison.OrdinalIgnoreCase))
                                {
                                    query += " left outer join UAD_Lookup..Country c on c.CountryID = ps.countryID ";
                                }
                                else
                                {
                                    query += " join UAD_Lookup..Country c on c.CountryID = ps.countryID ";
                                }

                                wherecondition_field = "c.ShortName";
                            }
                            else if (field.Group.Equals("[IGRP_NO]", StringComparison.OrdinalIgnoreCase))
                            {
                                wherecondition_field = "cast(s." + field.Group + " as varchar(100))";
                            }
                            else
                            {
                                wherecondition_field = "ps." + field.Group;
                            }

                            whereCondition = CreateStringCondition(whereCondition, field.Group, field.SearchCondition, field.Values, wherecondition_field);
                        }
                        break;
                    #endregion
                    case "ZIPCODE-RADIUS":
                        whereCondition += CreateZipCodeRadiusCondition(field.SearchCondition);
                        break;
                    case "WAVE MAILING":
                        whereCondition += " ISNULL(ps.IsInActiveWaveMailing,0) = " + field.Values;
                        break;
                    case "QUALIFICATIONYEAR":
                        var pubID = Convert.ToInt32(profilefields.Find(x => x.Name.Equals("PRODUCT", StringComparison.OrdinalIgnoreCase)).Values);
                        whereCondition += CreateQualificationYearCondition(clientconnection, pubID, field.Values);
                        break;
                }

            }

            #endregion

            #region Dimension fields
            // Loop through all of the Dimensions's
            // the PubId's go into the sub_query list as it limits the number of rows we need to get
            // All the other business/function/industry/etc fields


            // Now loop through all of the added master_ids
            // by putting them into a list and looping over the list, creating 
            // something called the bubble_up_query. This is a recursive query that places each
            // master id group in it's own query to properly get the OR behavior between each of these master id groups.

            bool added_master_id = false;
            string dimquery = "";
            string dimquery_start = "";
            // Each one of the queries starts the same way either selecting the igrp_no or the cgrp_no depending on if we want individual or company consensus
            //string bubble_up_query_igrp_start = "select distinct igrp_no from subscriptions sfilter join subscriptiondetails sd on sd.subscriptionid = sfilter.subscriptionid where ";

            dimquery_start = "select distinct ps1.SubscriptionID from IssueArchiveProductSubscription ps1  with (nolock) join IssueArchiveProductSubscriptionDetail psd  with (nolock) on ps1.IssueArchiveSubscriptionId = psd.IssueArchiveSubscriptionId  where ps1.issueID = " + IssueID + " and  ";


            var dimensionfields = (from f in filter.Fields
                                   where f.FilterType == FrameworkUAD.BusinessLogic.Enums.FiltersType.Dimension
                                   //where f.Column.ToUpper() == "MASTERID"
                                   select f
                                       ).ToList();

            foreach (Object.FilterDetails f in dimensionfields)
            {
                if (dimquery != "")
                    dimquery += " intersect ";

                dimquery += dimquery_start;
                dimquery += " psd.codesheetID in (" + f.Values + " ) ";
                added_master_id = true;
            }

            #endregion

            #region Activity Fields

            var Activityfields = (from f in filter.Fields
                                  where f.FilterType == FrameworkUAD.BusinessLogic.Enums.FiltersType.Activity
                                  //where f.Column.ToUpper() == "ACTIVITY"
                                  select f
                            ).ToList();

            var openCondition = new StringBuilder();
            var openBlastCondition = new StringBuilder();

            var clickCondition = new StringBuilder();
            var clickBlastCondition = new StringBuilder();


            var visitCondition = new StringBuilder();
            string openquery = string.Empty;
            string clickquery = string.Empty;
            string visitquery = string.Empty;
            int opencount = -1;
            int clickcount = -1;
            int visitcount = -1;
            string link = string.Empty;
            string domainTrackingID = string.Empty;
            string url = string.Empty;
            string openSearchType = string.Empty;
            string clickSearchType = string.Empty;

            bool bjoinBlastforOpen = false;
            bool bjoinBlastforClick = false;

            foreach (Object.FilterDetails f in Activityfields)
            {
                #region Activity field for loop
                switch (f.Name.ToUpper())
                {
                    // DIRECT INFO CASES
                    case "OPEN CRITERIA":
                        openSearchType = f.SearchCondition;
                        opencount = Convert.ToInt32(f.Values);
                        break;

                    case "OPEN ACTIVITY":
                        AppendDateNumberCondition(openCondition, f.SearchCondition, f.Values);
                        break;

                    case "OPEN BLASTID":
                        bjoinBlastforOpen = true;
                        openBlastCondition.Append(openBlastCondition.Length > 0 ? $" and bla.BlastID in ({f.Values})" : $" where bla.BlastID in ({f.Values})");
                        break;

                    case "OPEN CAMPAIGNS":
                        bjoinBlastforOpen = true;
                        openBlastCondition.Append(openBlastCondition.Length > 0 ? $" and bla.ECNCampaignID in ({f.Values})" : $" where bla.ECNCampaignID in ({f.Values})");
                        break;

                    case "OPEN EMAIL SUBJECT":
                        bjoinBlastforOpen = true;
                        openBlastCondition.Append(openBlastCondition.Length > 0 ? $" and bla.Emailsubject like '%{Object.Helper.ReplaceSingleQuotes(f.Values.Trim()).Replace("_", "[_]")}%'" : $" Where bla.Emailsubject like '%{Object.Helper.ReplaceSingleQuotes(f.Values.Trim()).Replace("_", "[_]")}%'");
                        break;

                    case "OPEN EMAIL SENT DATE":
                        bjoinBlastforOpen = true;
                        AppendDateCondition(openBlastCondition, BlastSendTimeColumnName, f.SearchCondition, f.Values);
                        break;

                    case "CLICK CRITERIA":
                        clickSearchType = f.SearchCondition;
                        clickcount = Convert.ToInt32(f.Values);
                        break;

                    case "LINK":
                        link = f.Values;

                        break;

                    case "CLICK ACTIVITY":
                        AppendDateNumberCondition(clickCondition, f.SearchCondition, f.Values);
                        break;

                    case "CLICK BLASTID":
                        bjoinBlastforClick = true;
                        clickBlastCondition.Append(clickBlastCondition.Length > 0 ? $" and bla.BlastID in ({f.Values})" : $" where bla.BlastID in ({f.Values})");
                        break;

                    case "CLICK CAMPAIGNS":
                        bjoinBlastforClick = true;
                        clickBlastCondition.Append(clickBlastCondition.Length > 0 ? $" and bla.ECNCampaignID in ({f.Values})" : $" where bla.ECNCampaignID in ({f.Values})");
                        break;

                    case "CLICK EMAIL SUBJECT":
                        bjoinBlastforClick = true;
                        clickBlastCondition.Append(clickBlastCondition.Length > 0 ? $" and bla.Emailsubject like '%{Object.Helper.ReplaceSingleQuotes(f.Values.Trim()).Replace("_", "[_]")}%'" : $" Where bla.Emailsubject like '%{Object.Helper.ReplaceSingleQuotes(f.Values.Trim()).Replace("_", "[_]")}%'");
                        break;

                    case "CLICK EMAIL SENT DATE":
                        bjoinBlastforClick = true;
                        AppendDateCondition(clickBlastCondition, BlastSendTimeColumnName, f.SearchCondition, f.Values);
                        break;

                    case "DOMAIN TRACKING":
                        domainTrackingID = f.Values;
                        break;

                    case "URL":
                        url = f.Values;
                        break;

                    case "VISIT CRITERIA":
                        visitcount = Convert.ToInt32(f.Values);
                        break;

                    case "VISIT ACTIVITY":
                        AppendDateNumberCondition(visitCondition, f.SearchCondition, f.Values);
                        break;
                }
                #endregion
            }

            if (pubIDs.Length > 0)
            {

                openCondition.Append(openCondition.Length > 0 ? " and pso.pubID in (" + pubIDs + ")" : " where pso.pubID in (" + pubIDs + ")");

                clickCondition.Append(clickCondition.Length > 0 ? " and psc.pubID in (" + pubIDs + ")" : " where psc.pubID in (" + pubIDs + ")");

            }

            string activityWhereCondition = string.Empty;

            #region Open

            if (opencount >= 0)
            {
                {
                    #region Search Specific Products
                    openquery = " select so.SubscriptionID  from IssueArchiveProductSubscription pso   with (NOLOCK) " +
                                       " join SubscriberOpenActivity so  with (NOLOCK) on pso.issueID = " + IssueID + " and so.PubSubscriptionID = pso.PubSubscriptionID ";


                    if (bjoinBlastforOpen)
                    {
                        openquery += " join #tempOblast tob  with (NOLOCK) on so.blastID = tob.blastID ";
                    }

                    openquery += openCondition;

                    openquery += " group by so.SubscriptionID";

                    openquery += (opencount > 0 ? " HAVING Count(so.openactivityid) >= " + opencount.ToString() : " ");

                    #endregion
                }
            }

            #endregion

            #region Click

            if (link.Length > 0)
            {
                if (clickcount < 0)
                    clickcount = 1;

                string linkquery = string.Empty;

                string[] strlink = link.Split(',');

                for (int i = 0; i < strlink.Length; i++)
                {
                    linkquery += (linkquery.Length > 0 ? " or (Link like '%" + Object.Helper.ReplaceSingleQuotes(strlink[i].Trim()).Replace("_", "[_]") + "%' or  LinkAlias like '%" + Object.Helper.ReplaceSingleQuotes(strlink[i].Trim()).Replace("_", "[_]") + "%')" : " (Link like'%" + Object.Helper.ReplaceSingleQuotes(strlink[i].Trim()).Replace("_", "[_]") + "%' or LinkAlias like '%" + Object.Helper.ReplaceSingleQuotes(strlink[i].Trim()).Replace("_", "[_]") + "%')");
                }

                clickCondition.Append(clickCondition.Length > 0 ? " and (" + linkquery + ")" : " Where (" + linkquery + ")");
            }

            if (clickcount >= 0)
            {
                {
                    clickquery = " select sc.SubscriptionID from IssueArchiveProductSubscription psc  with (NOLOCK) " +
                                                         " join SubscriberClickActivity sc  with (NOLOCK)  on psc.issueID = " + IssueID + " and  sc.PubSubscriptionID = psc.PubSubscriptionID ";



                    if (bjoinBlastforClick)
                    {
                        clickquery += " join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID ";
                    }

                    clickquery += clickCondition;

                    clickquery += " group by sc.SubscriptionID";

                    clickquery += (clickcount > 0 ? " having COUNT(sc.ClickActivityID) >= " + clickcount.ToString() : " ");
                }
            }

            #endregion

            #region Visit Activity

            if (visitcount >= 0)
            {

                visitquery = " select sv.SubscriptionID from " +
                                " SubscriberVisitActivity sv  with (NOLOCK) ";

                if (domainTrackingID.Length > 0)
                {
                    visitCondition.Append(visitCondition.Length > 0 ? " and sv.DomainTrackingID = " + domainTrackingID : " where sv.DomainTrackingID = " + domainTrackingID);
                }

                if (url.Length > 0)
                {
                    string[] strUrl = url.Split(',');

                    string urlquery = string.Empty;

                    for (int i = 0; i < strUrl.Length; i++)
                    {
                        urlquery += (urlquery.Length > 0 ? " or sv.url like '%" + Object.Helper.ReplaceSingleQuotes(strUrl[i].Trim()).Replace("_", "[_]") + "%'" : " sv.url like '%" + Object.Helper.ReplaceSingleQuotes(strUrl[i].Trim()).Replace("_", "[_]") + "%'");
                    }

                    visitCondition.Append(visitCondition.Length > 0 ? " and (" + urlquery + ")" : " Where (" + urlquery + ")");
                }

                if (visitCondition.Length > 0)
                {
                    visitquery += visitCondition;
                }

                visitquery += " group by sv.SubscriptionID";

                visitquery += (visitcount > 0 ? " having COUNT(sv.VisitActivityID) >= " + visitcount.ToString() : " ");

            }
            #endregion


            #endregion
            // If we have added master_ids we need to select based on the cgrp or igrp entities that the bubble_up_query selects for us
            // Otherwise we can just add the sub query and it will select things ignoring their survey responses
            if (added_master_id)
            {
                createTempTablequery += " Create table #tempDimSub (subscriptionid int);";
                createTempTablequery += " CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (subscriptionid); ";

                createTempTablequery += " Insert into #tempDimSub " + dimquery + ";";

                //query += " join #tempDimSub x4 on x4.SubscriptionID = s.SubscriptionID ";

                dropTempTablequery += " drop table #tempDimSub; ";
            }

            if (openquery.Length > 0)
            {
                if (bjoinBlastforOpen)
                {
                    createTempTablequery += "CREATE TABLE #tempOblast (blastid INT); ";
                    createTempTablequery += "CREATE UNIQUE CLUSTERED INDEX #tempOblast_1 ON #tempOblast (blastid);  ";
                    createTempTablequery += " insert into #tempOblast SELECT distinct blastid FROM blast bla WITH(nolock)  ";
                    createTempTablequery += openBlastCondition;
                }

                createTempTablequery += "; Create table #tempSOA (subscriptionid int);";
                createTempTablequery += " CREATE UNIQUE CLUSTERED INDEX tempSOA_1 on #tempSOA  (subscriptionid); ";

                createTempTablequery += " Insert into #tempSOA " + openquery + ";";

                if (opencount == 0)
                {
                    query += " left outer join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ";
                    whereCondition += (whereCondition.Length == 0 ? " " : " where ") + " soa1.SubscriptionID is null";
                }
                else
                {
                    query += " join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ";
                }



                dropTempTablequery += " drop table #tempSOA; ";
            }

            if (clickquery.Length > 0)
            {
                if (bjoinBlastforClick)
                {
                    createTempTablequery += "CREATE TABLE #tempCblast (blastid INT); ";
                    createTempTablequery += "CREATE UNIQUE CLUSTERED INDEX #tempCblast_1 ON #tempCblast (blastid);  ";
                    createTempTablequery += "Insert into #tempCblast SELECT distinct blastid FROM blast bla WITH (nolock)  ";
                    createTempTablequery += clickBlastCondition;
                }

                createTempTablequery += "; Create table #tempSCA (subscriptionid int);";
                createTempTablequery += " CREATE UNIQUE CLUSTERED INDEX tempSCA_1 on #tempSCA  (subscriptionid); ";

                createTempTablequery += " Insert into #tempSCA " + clickquery + ";";

                if (clickcount == 0)
                {
                    query += " left outer join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ";
                    whereCondition += (whereCondition.Length == 0 ? " " : " and ") + " sca1.SubscriptionID is null";
                }
                else
                    query += " join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ";

                dropTempTablequery += " drop table #tempSCA; ";
            }

            if (visitquery.Length > 0)
            {
                createTempTablequery += "; Create table #tempSVA (subscriptionid int);";
                createTempTablequery += " CREATE UNIQUE CLUSTERED INDEX tempSVA_1 on #tempSVA  (subscriptionid); ";

                createTempTablequery += " Insert into #tempSVA " + visitquery + ";";

                if (visitcount == 0)
                {
                    query += " left outer join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ";
                    whereCondition += (whereCondition.Length == 0 ? " " : " and ") + " sva1.SubscriptionID is null";
                }
                else
                {
                    query += " join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ";
                }

                dropTempTablequery += " drop table #tempSVA; ";
            }

            if (addlFilters.Length > 0)
                query += addlFilters;

            //txtShowQuery.Text = txtShowQuery.Text + "\r\n" + "\r\n" + query + (wherecondition.Length == 0 ? "" : " where " + wherecondition);

            if (bjoinBlastforOpen)
            {
                dropTempTablequery += "drop table #tempOblast; ";
            }

            if (bjoinBlastforClick)
            {
                dropTempTablequery += "drop table #tempCblast; ";
            }

            if (added_master_id)
            {
                query = "select " + select_list + " from #tempDimSub x4  with (nolock) join IssueArchiveProductSubscription ps  with (nolock)  on x4.subscriptionID = ps.subscriptionID " + query;
            }
            else
            {
                query = "select " + select_list + " from IssueArchiveProductSubscription ps  with (nolock) " + query;
            }

            return createTempTablequery + query + (whereCondition.Length == 0 ? "" : " where " + whereCondition) + ";" + dropTempTablequery;
        }

        /// <summary>
        /// Builds a condition for string and append it to existing wherecondition string
        /// </summary>
        /// <param name="wherecondition">the starting where condition to append</param>
        /// <param name="group">the group by clause</param>
        /// <param name="searchCondition">the condition to use in search</param>
        /// <param name="searchValues">the values to compare with</param>
        /// <param name="columnName">the column name to use</param>
        /// <returns></returns>
        public static string CreateStringCondition(string wherecondition, string group, string searchCondition, string searchValues, string columnName)
        {
            var values = searchValues.Split(',');
            var builder = new StringBuilder(wherecondition);
            builder.Append("(");

            for (int i = 0; i < values.Length; i++)
            {
                switch (searchCondition.ToUpper())
                {
                    case "EQUAL":
                        builder.Append((i > 0 ? " OR " : "") + "( " + columnName + " = '" + Object.Helper.ReplaceSingleQuotes(values[i].Trim()) + "')");
                        break;
                    case "CONTAINS":
                        builder.Append((i > 0 ? " OR " : "") + "( " + "PATINDEX('%" + Object.Helper.ReplaceSingleQuotes(values[i].Trim()).Replace("_", "[_]") + "%', " + columnName + ") > 0 )");
                        break;
                    case "START WITH":
                        builder.Append((i > 0 ? " OR " : "") + "( " + "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(values[i].Trim()).Replace("_", "[_]") + "%', " + columnName + ") > 0 )");
                        break;
                    case "END WITH":
                        builder.Append((i > 0 ? " OR " : "") + "( " + "PATINDEX('%" + Object.Helper.ReplaceSingleQuotes(values[i].Trim()).Replace("_", "[_]") + "', " + columnName + ") > 0 )");
                        break;
                    case "DOES NOT CONTAIN":
                        builder.Append((i > 0 ? " AND " : "") + "( isnull(" + columnName + ",'') not like '%" + Object.Helper.ReplaceSingleQuotes(values[i].Trim()).Replace("_", "[_]") + "%'  )");
                        break;
                    case "RANGE":
                        var zipRange = values[i].Trim().Split('|');
                        builder.Append((i > 0 ? " OR " : "") + " (substring(" + columnName + ", 1, len('" + zipRange[0].Trim() + "')) between '" + zipRange[0].Trim() + "' and '" + zipRange[1].Trim() + "')");
                        break;
                    case "IS EMPTY":
                        if (group.Equals("[COUNTRY]", StringComparison.OrdinalIgnoreCase))
                        {
                            builder.Append((i > 0 ? " OR " : "") + " (c.CountryID is NULL ) ");
                        }
                        else if (group.Equals("[IGRP_NO]", StringComparison.OrdinalIgnoreCase))
                        {
                            builder.Append((i > 0 ? " OR " : "") + " ( " + columnName + " is NULL) ");
                        }
                        else
                        {
                            builder.Append((i > 0 ? " OR " : "") + "( " + columnName + " is NULL or " + columnName + " = '')");
                        }
                        break;
                    case "IS NOT EMPTY":
                        if (group.Equals("[COUNTRY]", StringComparison.OrdinalIgnoreCase))
                        {
                            builder.Append((i > 0 ? " OR " : "") + " (c.CountryID is NOT NULL ) ");
                        }
                        else if (group.Equals("[IGRP_NO]", StringComparison.OrdinalIgnoreCase))
                        {
                            builder.Append((i > 0 ? " OR " : "") + " ( " + columnName + " is NOT NULL) ");
                        }
                        else
                        {
                            builder.Append((i > 0 ? " OR " : "") + " (ISNULL(" + columnName + ", '') != '' " + " )");
                        }
                        break;
                }
            }

            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Builds a condition for a profile column
        /// </summary>
        /// <param name="columnName">the column name</param>
        /// <param name="existsColumnName">exists column name (in case of non product view)</param>
        /// <param name="values">Values to check for ("0" or "1")</param>
        /// <param name="viewType">view type</param>
        /// <returns>the condition string</returns>
        public static string CreateProfileCondition(string columnName, string existsColumnName, string values, Enums.ViewType viewType = Enums.ViewType.ProductView)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(nameof(columnName));
            }
            
            if (string.IsNullOrWhiteSpace(values))
            {
                throw new ArgumentNullException(nameof(values));
            }

            var searchValues = values.Split(',');
            var whereCondition = new StringBuilder("(");

            if (viewType == Enums.ViewType.ProductView || viewType == Enums.ViewType.CrossProductView)
            {
                for (var i = 0; i < searchValues.Length; i++)
                {
                    if (searchValues[i] == "1")
                    {
                        whereCondition.AppendFormat((i > 0 ? " OR " : "") + "isnull({0}, '') != '' ", columnName);
                    }
                    else
                    {
                        whereCondition.AppendFormat((i > 0 ? " OR " : "") + "isnull({0}, '') = '' ", columnName);
                    }
                }
            }
            else
            {
                for (int i = 0; i < searchValues.Length; i++)
                {
                    whereCondition.AppendFormat((i > 0 ? " OR " : "") + "{0} = " + searchValues[i], existsColumnName);
                }
            }

            whereCondition.Append(")");
            return whereCondition.ToString();
        }

        /// <summary>
        /// Creates a condition on qualification year
        /// </summary>
        /// <param name="clientConnection">client connection</param>
        /// <param name="pubID">product PubID</param>
        /// <param name="values">The years values to compare with</param>
        /// <returns>a string condition query</returns>
        private static string CreateQualificationYearCondition(ClientConnections clientConnection, int pubID, string values)
        {
            if (string.IsNullOrWhiteSpace(values))
            {
                throw new ArgumentNullException(nameof(values));
            }
            
            var product = new Product().Select(pubID, clientConnection);

            if (product == null)
            {
                throw new ArgumentException(nameof(pubID));
            }

            var years = values.Split(',');
            var whereCondition = new StringBuilder("(");
            
            DateTime startDate;
            DateTime endDate;
            
            if (!string.IsNullOrWhiteSpace(product.YearStartDate) && !string.IsNullOrWhiteSpace(product.YearEndDate))
            {
                int currentYear;
                DateTime productStartDate;
                if (!DateTime.TryParse(product.YearStartDate + "/" + DateTime.Now.Year, out productStartDate))
                {
                    throw new InvalidOperationException("CreateQualificationYearCondition: Failed parsing product start date");
                }

                if (DateTime.Now >  productStartDate)
                {
                    currentYear = DateTime.Now.Year;
                }
                else
                {
                    currentYear = DateTime.Now.Year - 1;
                }

                if (!DateTime.TryParse(product.YearStartDate + "/" + currentYear, out startDate))
                {
                    throw new InvalidOperationException("CreateQualificationYearCondition: Failed parsing product start date");
                }

                endDate = startDate.AddYears(1).AddSeconds(-1);
            }
            else
            {
                startDate = DateTime.Now;
                endDate = startDate.AddYears(-1).AddDays(+1);
            }

            for (var i = 0; i < years.Length; i++)
            {
                whereCondition.Append((i > 0 ? " OR " : "") + " ps.QualificationDate between convert(varchar(20), DATEADD(year, -" + (Convert.ToInt32(years[i]) - 1) + ", '" + startDate.ToShortDateString() + "'),111)  and  convert(varchar(20), DATEADD(year, -" + (Convert.ToInt32(years[i]) - 1) + ", '" + endDate.ToShortDateString() + "'),111) + ' 23:59:59'");
            }

            whereCondition.Append(")");
            return whereCondition.ToString();
        }

        /// <summary>
        /// Builds a condition for Zip code radius
        /// </summary>
        /// <param name="condition">separated string: zipcode|radiusMin|radiusMax</param>
        /// <returns>condition string</returns>
        public static string CreateZipCodeRadiusCondition(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var zipValues = condition.Split('|');
            if (zipValues.Length < 3)
            {
                throw new ArgumentException(nameof(condition));
            }

            var radiusMin = 0;
            var radiusMax = 0;
            var zipCode = zipValues[0];
            if (!Int32.TryParse(zipValues[1], out radiusMin))
            {
                throw new InvalidOperationException("CreateZipCodeRadiusCondition: failed parsing RadiusMin");
            }

            if (!Int32.TryParse(zipValues[2], out radiusMax))
            {
                throw new InvalidOperationException("CreateZipCodeRadiusCondition: failed parsing RadiusMax");
            }
            
            var locationLat = 0D;
            var locationLon = 0D;
            var values = CalculateZipCodeRadius(radiusMin, radiusMax, zipCode, (zipCode.Length == 5 ? "UNITED STATES" : "CANADA"), out locationLat, out locationLon);

            var whereCondition = new StringBuilder("(");

            //Max Radius Latitude
            whereCondition.Append("s.Latitude >= " + values[0] + " and " + "s.Latitude <= " + values[1]);

            //Max Radius Longitude
            whereCondition.Append(" and " + "s.Longitude >= " + values[2] + " and " + "s.Longitude <= " + values[3]);

            //Min Radius Lat & Long
            whereCondition.Append(" and (s.Latitude<=" + values[4] + " OR " + values[5] + "<=s.Latitude OR s.Longitude<=" + values[6] + " OR " + values[7] + "<=s.Longitude)");

            whereCondition.Append(" and isnull(s.IsLatLonValid,0) = 1 ");

            if (radiusMin > 0 && radiusMax > 0)
            {
                whereCondition.Append(" and ( master.dbo.fn_CalcDistanceBetweenLocations(" + locationLat + ", " + locationLon + ", s.Latitude, s.Longitude, 0) between  " + radiusMin + " and " + radiusMax + ")");
            }
            else if (radiusMax > 0)
            {
                whereCondition.Append(" and ( master.dbo.fn_CalcDistanceBetweenLocations(" + locationLat + ", " + locationLon + ", s.Latitude, s.Longitude, 0) <= " + radiusMax + ")");
            }

            whereCondition.Append(")");
            return whereCondition.ToString();
        }

        /// <summary>
        /// Calculate zip code radius values for a zip code
        /// </summary>
        /// <param name="radiusMin">minimum radius</param>
        /// <param name="radiusMax">maximum radius</param>
        /// <param name="zipCode">zip code</param>
        /// <param name="country">country name</param>
        /// <param name="locationLat">output latitude of location</param>
        /// <param name="locationLon">output longitude of location</param>
        /// <returns>array of 8 values for this field</returns>
        public static double[] CalculateZipCodeRadius(int radiusMin, int radiusMax, string zipCode, string country, out double locationLat, out double locationLon)
        {
            var values = new double[8];
            var mylocation = new Object.Location();
            mylocation.PostalCode = zipCode;
            mylocation = Object.Location.ValidateBingAddress(mylocation, country);
            if (mylocation.IsValid)
            {
                var PI_180 = Math.PI / 180D;
                locationLat = Convert.ToDouble(mylocation.Latitude);
                locationLon = Convert.ToDouble(mylocation.Longitude);

                var radiusLatTotalRangeMax = Convert.ToDouble(radiusMax) / 69D;
                var minLatRangeMax = locationLat - radiusLatTotalRangeMax;
                var maxLatRangeMax = locationLat + radiusLatTotalRangeMax;
                var minLonRangeMax = locationLon + (radiusLatTotalRangeMax / Math.Cos(minLatRangeMax * PI_180));
                var maxLonRangeMax = locationLon - (radiusLatTotalRangeMax / Math.Cos(maxLatRangeMax * PI_180));

                var radiusLatTotalRangeMin = Convert.ToDouble(radiusMin) / 69D;
                var minLatRangeMin = locationLat - radiusLatTotalRangeMin;
                var maxLatRangeMin = locationLat + radiusLatTotalRangeMin;
                var minLonRangeMin = locationLon + (radiusLatTotalRangeMin / Math.Cos(minLatRangeMin * PI_180));
                var maxLonRangeMin = locationLon - (radiusLatTotalRangeMin / Math.Cos(maxLatRangeMin * PI_180));

                Double temp;

                if (minLatRangeMin > maxLatRangeMin && minLonRangeMin < maxLonRangeMin)
                {
                    temp = maxLatRangeMin;
                    maxLatRangeMin = minLatRangeMin;
                    minLatRangeMin = temp;
                }
                else if (minLatRangeMin < maxLatRangeMin && minLonRangeMin > maxLonRangeMin)
                {
                    temp = maxLonRangeMin;
                    maxLonRangeMin = minLonRangeMin;
                    minLonRangeMin = temp;

                }
                else if (minLatRangeMin > maxLatRangeMin && minLonRangeMin > maxLonRangeMin)
                {
                    temp = maxLatRangeMin;
                    maxLatRangeMin = minLatRangeMin;
                    minLatRangeMin = temp;

                    temp = maxLonRangeMin;
                    maxLonRangeMin = minLonRangeMin;
                    minLonRangeMin = temp;
                }

                if (minLatRangeMax > maxLatRangeMax && minLonRangeMax < maxLonRangeMax)
                {
                    temp = maxLatRangeMax;
                    maxLatRangeMax = minLatRangeMax;
                    minLatRangeMax = temp;
                }
                else if (minLatRangeMax < maxLatRangeMax && minLonRangeMax > maxLonRangeMax)
                {
                    temp = maxLonRangeMax;
                    maxLonRangeMax = minLonRangeMax;
                    minLonRangeMax = temp;

                }
                else if (minLatRangeMax > maxLatRangeMax && minLonRangeMax > maxLonRangeMax)
                {
                    temp = maxLatRangeMax;
                    maxLatRangeMax = minLatRangeMax;
                    minLatRangeMax = temp;

                    temp = maxLonRangeMax;
                    maxLonRangeMax = minLonRangeMax;
                    minLonRangeMax = temp;
                }

                values[0] = minLatRangeMax;
                values[1] = maxLatRangeMax;
                values[2] = minLonRangeMax;
                values[3] = maxLonRangeMax;
                values[4] = minLatRangeMin;
                values[5] = maxLatRangeMin;
                values[6] = minLonRangeMin;
                values[7] = maxLonRangeMin;

                return values;
            }
            else
            {
                throw new Object.InvalidZipCodeException("Invalid zipcode");
            }
        }

        /// <summary>
        /// Builds a condition for getting permission
        /// </summary>
        /// <param name="columnName">permission column name</param>
        /// <param name="values">the values to compare</param>
        /// <param name="viewType">(optional) the view type to use</param>
        /// <returns>the generated condition string</returns>
        public static string CreatePermissionCondition(string columnName, string values, Enums.ViewType viewType = Enums.ViewType.ProductView)
        {
            var permissions = values.Split(',');
            var wherecondition = new StringBuilder("(");

            if (viewType == Enums.ViewType.ProductView || viewType == Enums.ViewType.CrossProductView)
            {
                for (var i = 0; i < permissions.Length; i++)
                {
                    if (permissions[i] == "-1")
                    {
                        wherecondition.Append((i > 0 ? " OR " : "") + "ps." + columnName + " is null");
                    }
                    else
                    {
                        wherecondition.Append((i > 0 ? " OR " : "") + "ps." + columnName + " = " + permissions[i]);
                    }
                }
            }
            else
            {
                for (var i = 0; i < permissions.Length; i++)
                {
                    if (permissions[i] == "-1")
                    {
                        wherecondition.Append((i > 0 ? " OR " : "") + "s." + columnName + " is null");
                    }
                    else
                    {
                        wherecondition.Append((i > 0 ? " OR " : "") + "s." + columnName + " = " + permissions[i]);
                    }
                }
            }

            wherecondition.Append(")");
            return wherecondition.ToString();
        }

        /// <summary>
        /// Append a date condition to an existing select query StringBuilder
        /// </summary>
        /// <param name="condition">the condition StringBuilder to append</param>
        /// <param name="columnName">column name</param>
        /// <param name="searchCondition">the search condition to be used</param>
        /// <param name="values">the value(s) to compare with</param>
        public static void AppendDateCondition(StringBuilder condition, string columnName, string searchCondition, string values)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            if (string.IsNullOrWhiteSpace(searchCondition))
            {
                throw new ArgumentNullException(nameof(searchCondition));
            }

            if (string.IsNullOrWhiteSpace(values))
            {
                throw new ArgumentNullException(nameof(values));
            }

            var value = values.Split('|');
            var conditionsCount = 0;

            condition.Append("(");

            switch (searchCondition.ToUpper())
            {
                case DateConditionDateRange:

                    var date = string.Empty;
                    if (!string.IsNullOrWhiteSpace(value[0]))
                    {
                        if (value[0] == DateConditionExpressionToday)
                        {
                            date = DateTime.Now.ToShortDateString();
                            condition.AppendFormat("{0} >='{1}'", columnName, date);
                        }
                        else if (value[0].Contains(DateConditionExpressionToday + "["))
                        {
                            var days = Convert.ToDouble(value[0].Substring(value[0].IndexOf("[") + 1, value[0].IndexOf("]") - (value[0].IndexOf("[") + 1)));
                            date = DateTime.Now.AddDays(days).ToShortDateString();
                            condition.AppendFormat("{0} >='{1}'", columnName, date);
                        }
                        else
                        {
                            condition.AppendFormat("{0} >= '{1}' ", columnName, value[0]);
                        }

                        conditionsCount = 1;
                    }

                    if (value.Length > 1 && !string.IsNullOrWhiteSpace(value[1]))
                    {
                        if (value[1] == DateConditionExpressionToday)
                        {
                            date = DateTime.Now.ToShortDateString();
                            condition.Append((conditionsCount > 0 ? " and " : "") + columnName + " <='" + date + " 23:59:59'");
                        }
                        else if (value[1].Contains(DateConditionExpressionToday + "["))
                        {
                            var days = Convert.ToDouble(value[1].Substring(value[1].IndexOf("[") + 1, value[1].IndexOf("]") - (value[1].IndexOf("[") + 1)));
                            date = DateTime.Now.AddDays(days).ToShortDateString();
                            condition.Append((conditionsCount > 0 ? " and " : "") + columnName + " <='" + date + " 23:59:59'");
                        }
                        else
                        {
                            condition.Append((conditionsCount > 0 ? " and " : "") + columnName + " <= '" + value[1] + " 23:59:59'");
                        }
                    }

                    break;
                case DateConditionXDays:
                    if (!string.IsNullOrWhiteSpace(value[0]))
                    {
                        if (value[0].Equals("1YR", StringComparison.OrdinalIgnoreCase))
                        {
                            condition.Append(columnName + ">= '" + DateTime.Now.AddYears(-1).ToShortDateString() + "'");
                        }
                        else if (value[0].Equals("6mon", StringComparison.OrdinalIgnoreCase))
                        {
                            condition.Append(columnName + ">= '" + DateTime.Now.AddMonths(-6).ToShortDateString() + "'");
                        }
                        else
                        {
                            condition.Append(columnName + ">= '" + DateTime.Now.AddDays(-Convert.ToInt32(value[0])).ToShortDateString() + "'");
                        }
                    }

                    break;
                case DateConditionYear:
                    if (!string.IsNullOrWhiteSpace(value[0]))
                    {
                        condition.Append("year(" + columnName + ") >= '" + value[0] + "' ");
                        conditionsCount = 1;
                    }

                    if (value.Length > 1 && !string.IsNullOrWhiteSpace(value[1]))
                    {
                        condition.Append((conditionsCount > 0 ? " and " : "") + "year(" + columnName + ") <= '" + value[1] + "' ");
                    }

                    break;
                case DateConditionMonth:
                    if (!string.IsNullOrWhiteSpace(value[0]))
                    {
                        condition.Append("month(" + columnName + ") >= '" + value[0] + "' ");
                        conditionsCount = 1;
                    }

                    if (value.Length > 1 && !string.IsNullOrWhiteSpace(value[1]))
                    {
                        condition.Append((conditionsCount > 0 ? " and " : "") + "month(" + columnName + ") <= '" + value[1] + "' ");
                    }

                    break;
            }

            condition.Append("))");
        }

        /// <summary>
        /// Append date condition (using number type in sql instead of date)
        /// </summary>
        /// <param name="condition">the condition StringBuilder to append to</param>
        /// <param name="searchCondition">the search condition to use</param>
        /// <param name="searchValues">the values to compare with</param>
        public static void AppendDateNumberCondition(StringBuilder condition, string searchCondition, string searchValues)
        {
            var values = searchValues.Split('|');

            if (searchCondition.Equals(DateConditionDateRange, StringComparison.OrdinalIgnoreCase))
            {
                if (values[0] != string.Empty)
                {
                    if (values[0] == "EXP:Today")
                    {
                        condition.Append(condition.Length > 0 ? " and " : " where ");
                        condition.Append(" DateNumber >= " + (DateTime.Now.Date - d1900).TotalDays);
                    }
                    else if (values[0].Contains("EXP:Today["))
                    {
                        var days = values[0].Substring(values[0].IndexOf("[") + 1, values[0].IndexOf("]") - (values[0].IndexOf("[") + 1));

                        condition.Append(condition.Length > 0 ? " and " : " where ");
                        condition.Append(" DateNumber >= " + (DateTime.Now.Date.AddDays(Convert.ToDouble(days)) - d1900).TotalDays);
                    }
                    else
                    {
                        condition.Append(condition.Length > 0 ? " and " : " where ");
                        condition.Append(" DateNumber >= " + (DateTime.Parse(values[0]) - d1900).TotalDays);
                    }
                }

                if (values[1] != string.Empty)
                {
                    if (values[1] == "EXP:Today")
                    {
                        condition.Append(condition.Length > 0 ? " and " : " where ");
                        condition.Append(" DateNumber <= " + (DateTime.Now.Date - d1900).TotalDays);
                    }
                    else if (values[1].Contains("EXP:Today["))
                    {
                        var days = values[1].Substring(values[1].IndexOf("[") + 1, values[1].IndexOf("]") - (values[1].IndexOf("[") + 1));

                        condition.Append(condition.Length > 0 ? " and " : " where ");
                        condition.Append(" DateNumber <= " + (DateTime.Now.Date.AddDays(Convert.ToDouble(days)) - d1900).TotalDays);
                    }
                    else
                    {
                        condition.Append(condition.Length > 0 ? " and " : " where ");
                        condition.Append(" DateNumber <= " + (DateTime.Parse(values[1]) - d1900).TotalDays);
                    }
                }
            }
            else if (searchCondition.Equals(DateConditionXDays, StringComparison.OrdinalIgnoreCase))
            {
                if (searchValues.Equals("1YR", StringComparison.OrdinalIgnoreCase))
                {
                    condition.Append(condition.Length > 0 ? " and " : " where ");
                    condition.Append(" DateNumber >= " + (DateTime.Now.Date.AddYears(-1) - d1900).TotalDays);
                }
                else if (searchValues.Equals("6mon", StringComparison.OrdinalIgnoreCase))
                {
                    condition.Append(condition.Length > 0 ? " and " : " where ");
                    condition.Append(" DateNumber >= " + (DateTime.Now.Date.AddMonths(-6) - d1900).TotalDays);
                }
                else
                {
                    condition.Append(condition.Length > 0 ? " and " : " where ");
                    condition.Append(" DateNumber >= " + (DateTime.Now.Date.AddDays(-Convert.ToInt32(searchValues)) - d1900).TotalDays);
                }
            }
            else if (searchCondition.Equals(DateConditionYear, StringComparison.OrdinalIgnoreCase))
            {
                if (values[0] != string.Empty)
                {
                    condition.Append(condition.Length > 0 ? " and " : " where ");
                    condition.Append(" DateNumber >= " + (Convert.ToDateTime($"1/1/{values[0]}", CultureInfo.InvariantCulture).Date - d1900).TotalDays);
                }

                if (values[1] != string.Empty)
                {
                    condition.Append(condition.Length > 0 ? " and " : " where ");
                    condition.Append(" DateNumber <= " + (Convert.ToDateTime($"12/31/{values[1]}", CultureInfo.InvariantCulture).Date - d1900).TotalDays);
                }
            }
            else if (searchCondition.Equals(DateConditionMonth, StringComparison.OrdinalIgnoreCase))
            {
                if (values[0] != string.Empty)
                {
                    condition.Append(condition.Length > 0 ? " and " : " where ");
                    condition.Append(" DateNumber >= " + (Convert.ToDateTime($"{values[0]}/1/{DateTime.Now.Year}", CultureInfo.InvariantCulture).Date - d1900).TotalDays);
                }

                if (values[1] != string.Empty)
                {
                    condition.Append(condition.Length > 0 ? " and " : " where ");
                    var daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, Convert.ToInt32(values[1]));
                    condition.Append(" DateNumber <= " + (Convert.ToDateTime($"{values[1]}/{daysInMonth}/{DateTime.Now.Year}", CultureInfo.InvariantCulture).Date - d1900).TotalDays);
                }
            }
        }

        /// <summary>
        /// Builds an obnoxious query string based on user selections
        /// </summary>
        /// <param name="doc">The XML document build by the GUI</param>
        /// <param name="FilterID"> If we are talking about a specific filter, put in the id of that xml filter</param>
        /// <param name="individual_consensus">True if individual consensus or xxalse for business consensus</param>
        /// <param name="select_list">The column's needed or count(distinct igrp_no) for summaries</param>
        /// <param name="all_master_codes"> If you need master codes add this to your call</param>
        /// <returns></returns>
        public static string getFilterQuery(Object.FilterMVC filter, string select_list, string addlFilters, KMPlatform.Object.ClientConnections clientconnection, bool IsAddRemove=false)
        {
            var pubIDs = string.Empty;
            var query = string.Empty;
            var createTempTablequery = string.Empty;
            var dropTempTablequery = string.Empty;

            // Outer Query that will finally run... Get the data needed + any additional tables for the final XLS output
            var wherecondition = string.Empty;
            var includeSubscriptionTbl = false;
            var includeCategoryTypeCondition = !FieldNameExists(filter, CategoryCode);
            var includeXactTypeCondition = !FieldNameExists(filter, TransactionCode);
            var includeQSourceTypeCondition = !FieldNameExists(filter, QSourceCode);
            
            #region profile fields

            var profilefields = filter.Fields
                                    .Where(f => f.FilterType != Enums.FiltersType.Dimension && f.FilterType != Enums.FiltersType.Activity)
                                    .ToList();

            foreach (var field in profilefields)
            {
                wherecondition = GetConditionBeginning(wherecondition, field);

                switch (field.Name.ToUpper())
                {
                    // DIRECT INFO CASES
                    case "BRAND":
                        if (filter.ViewType != FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView && filter.ViewType != FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                        {
                            query += " join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  join brand b with (nolock) on b.brandID = bd.brandID ";
                            wherecondition += "b.IsDeleted = 0 and bd.BrandID = " + field.Values;
                        }
                        break;
                    case "PRODUCT":
                        pubIDs = field.Values;
                        wherecondition += "ps.pubid in (" + field.Values + " ) ";
                        break;
                    case "DATACOMPARE":

                        string[] strDataCompare = field.Values.Split('|');

                        string dataCompareType = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeId(Convert.ToInt32(strDataCompare[1])).CodeName;
                        includeSubscriptionTbl = true;
                        if (string.Equals(dataCompareType, "Match", StringComparison.OrdinalIgnoreCase))
                        {

                            query += " join DataCompareProfile d with(nolock) on s.IGRP_NO = d.IGrp_No ";
                            wherecondition += "d.ProcessCode = '" + strDataCompare[0] + "'";
                        }
                        else
                        {
                            query += " left outer join DataCompareProfile d with(nolock) on s.IGRP_NO = d.IGrp_No and  d.ProcessCode = '" + strDataCompare[0] + "'";
                            wherecondition += "d.SubscriberFinalId is null ";
                        }
                        break;
                    case "STATE":
                        if (filter.ViewType != FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView && filter.ViewType != FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                        {
                            includeSubscriptionTbl = true;
                            wherecondition += "s.State in ('" + field.Values.Replace(",", "','") + "') ";
                        }
                        else
                            wherecondition += "ps.RegionCode in ('" + field.Values.Replace(",", "','") + "') ";
                        break;
                    case "COUNTRY":

                        string[] strCountryId = field.Values.Split(',');

                        wherecondition += "(";

                        if (filter.ViewType != FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView && filter.ViewType != FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                        {
                            includeSubscriptionTbl = true;
                            for (int i = 0; i < strCountryId.Length; i++)
                            {
                                if (strCountryId[i] == "1")
                                {
                                    wherecondition += (i > 0 ? " OR " : "") + "s.CountryID = 1";
                                }
                                else if (strCountryId[i] == "3")
                                {
                                    wherecondition += (i > 0 ? " OR " : "") + "((s.CountryID = 1) or (s.CountryID = 2)) ";
                                }
                                else if (strCountryId[i] == "4")
                                {
                                    wherecondition += (i > 0 ? " OR " : "") + "not ((s.CountryID = 1) or (s.CountryID = 2) or ISNULL(s.CountryID,0) = 0)";
                                }
                                else
                                {
                                    wherecondition += (i > 0 ? " OR " : "") + "s.CountryID in ( " + strCountryId[i] + " ) ";
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < strCountryId.Length; i++)
                            {
                                if (strCountryId[i] == "1")
                                {
                                    wherecondition += (i > 0 ? " OR " : "") + "ps.CountryID = 1";
                                }
                                else if (strCountryId[i] == "3")
                                {
                                    wherecondition += (i > 0 ? " OR " : "") + "((ps.CountryID = 1) or (ps.CountryID = 2)) ";
                                }
                                else if (strCountryId[i] == "4")
                                {
                                    wherecondition += (i > 0 ? " OR " : "") + "not ((ps.CountryID = 1) or (ps.CountryID = 2) or ISNULL(ps.CountryID,0) = 0)";
                                }
                                else
                                {
                                    wherecondition += (i > 0 ? " OR " : "") + "ps.CountryID in ( " + strCountryId[i] + " ) ";
                                }
                            }
                        }

                        wherecondition += ")";

                        break;
                    case ProfileEmail:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreateProfileCondition("ps.Email", "s.emailexists", field.Values, filter.ViewType);
                        break;
                    case ProfilePhone:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreateProfileCondition("ps.Phone", "s.phoneexists", field.Values, filter.ViewType);
                        break;
                    case ProfileFax:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreateProfileCondition("ps.fax", "s.faxexists", field.Values, filter.ViewType);
                        break;
                    case "GEOLOCATED":
                        string[] strIsLatLonValid = field.Values.Split(',');

                        wherecondition += "(";

                        includeSubscriptionTbl = true;

                        for (int i = 0; i < strIsLatLonValid.Length; i++)
                        {
                            wherecondition += (i > 0 ? " OR " : "") + "s.IsLatLonValid = " + strIsLatLonValid[i];
                        }

                        wherecondition += ")";

                        break;
                    case ProfilePermissionMail:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreatePermissionCondition("MailPermission", field.Values, filter.ViewType);
                        break;
                    case ProfilePermissionFax:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreatePermissionCondition("FaxPermission", field.Values, filter.ViewType);
                        break;
                    case ProfilePermissionPhone:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreatePermissionCondition("PhonePermission", field.Values, filter.ViewType);
                        break;
                    case ProfilePermissionOtherProducts:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreatePermissionCondition("OtherProductsPermission", field.Values, filter.ViewType);
                        break;
                    case ProfilePermissionThirdParty:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreatePermissionCondition("ThirdPartyPermission", field.Values, filter.ViewType);
                        break;
                    case ProfilePermissionEmailRenew:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreatePermissionCondition("EmailRenewPermission", field.Values, filter.ViewType);
                        break;
                    case ProfilePermissionText:
                        includeSubscriptionTbl = (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView) ? includeSubscriptionTbl : true;
                        wherecondition += CreatePermissionCondition("TextPermission", field.Values, filter.ViewType);
                        break;
                    case "MEDIA":
                        wherecondition += "ps.Demo7 in (  '" + field.Values.Replace(",", "','") + "') ";
                        break;
                    case "QFROM":
                        wherecondition += " ps.QualificationDate >= '" + field.Values + "' ";
                        break;
                    case "QTO":
                        wherecondition += " ps.QualificationDate <= '" + field.Values + " 23:59:59' ";
                        break;
                    case "EMAIL STATUS":
                        wherecondition += "ps.EmailStatusID in (" + field.Values + ") ";
                        break;
                    case CategoryCode:
                        if (IsAddRemove)
                            wherecondition += "ISNULL(sak.PubCategoryID, ps.PubCategoryID) in ( " + field.Values + " ) ";
                        else
                            wherecondition += "ps.PubCategoryID in ( " + field.Values + " ) ";
                        break;
                    case "CATEGORY TYPE":
                        if (IsAddRemove && includeCategoryTypeCondition)
                        {
                            wherecondition += "ISNULL(sak.PubCategoryID, ps.PubCategoryID) in (select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock) where CategoryCodeTypeID in ( " + field.Values + " ) )";
                        }
                        else if (includeCategoryTypeCondition)
                        {
                            wherecondition += "ps.PubCategoryID  in (select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock) where CategoryCodeTypeID in ( " + field.Values + " ) )";
                        }
                        break;
                    case TransactionCode:
                        if (IsAddRemove)
                            wherecondition += "ISNULL(sak.PubTransactionID,ps.PubTransactionID) in ( " + field.Values + " )";
                        else
                            wherecondition += "ps.PubTransactionID in ( " + field.Values + " )";
                        break;
                    case "XACT":
                        if (IsAddRemove && includeXactTypeCondition)
                        {
                            wherecondition += "ISNULL(sak.PubTransactionID,ps.PubTransactionID) in (select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock) where TransactionCodeTypeID in ( " + field.Values + " ) )";
                        }
                        else if (includeXactTypeCondition)
                        {
                            wherecondition += "ps.PubTransactionID in (select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock) where TransactionCodeTypeID in ( " + field.Values + " ) )";
                        }
                        break;
                    case QSourceCode:
                        wherecondition += "ps.PubQSourceID in ( " + field.Values + " ) ";
                        break;
                    case "QSOURCE TYPE":
                        if (includeQSourceTypeCondition)
                            wherecondition += "ps.PubQSourceID in (select CodeID from UAD_Lookup..Code with (nolock) where ParentCodeId in ( " + field.Values + " ) )";
                        break;
                    case "ADHOC":
                        #region Adhoc support
                        //sub_query += "sfilter." + node.Attributes["column"].Value + " like '" + field.Values + "%'";
                        //wgh either add this to subquery or get rid of the last " and "
                        //sub_query = sub_query.Remove(sub_query.Length - 5);

                        if (field.Group.Contains("|"))
                        {
                            string[] strID = field.Group.Split('|');
                            if (strID[0] == "m")
                            {
                                string query_isempty = string.Empty;
                                string query_notnull = string.Empty;
                                string query_isnotempty = string.Empty;

                                if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                {
                                    if (filter.BrandID == 0)
                                    {
                                        query_isempty =
                                            " (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock) " +
                                            " left outer join (select distinct vrc.subscriptionID from vw_RecentConsensus vrc " +
                                            " where vrc.MasterGroupID=" + strID[1] + ") inn1 on sfilter.SubscriptionID = inn1.SubscriptionID " +
                                            " where inn1.subscriptionID is null ";
                                        query_notnull =
                                            " (select distinct vrc.subscriptionID from vw_RecentConsensus vrc " +
                                            " join Mastercodesheet ms with (nolock)  on vrc.MasterID = ms.MasterID " +
                                            " where vrc.MasterGroupID=" + strID[1] + " and ";
                                        query_isnotempty =
                                            " (select distinct vrc.subscriptionID from vw_RecentConsensus vrc " +
                                            " where vrc.MasterGroupID=" + strID[1];
                                    }
                                    else
                                    {
                                        query_isempty =
                                            " (select distinct sfilter.subscriptionid FROM subscriptions sfilter WITH (nolock) " +
                                            " join pubsubscriptions ps1 WITH (nolock) ON sfilter.subscriptionID = ps1.subscriptionID " +
                                            " join BrandDetails bd with (nolock) on bd.PubID = ps1.PubID and bd.BrandID = " + filter.BrandID + " " +
                                            " left outer join ( " +
                                            " select DISTINCT sd.subscriptionid  " +
                                            " FROM   subscriptiondetails sd WITH (nolock)  " +
                                            " 	join vw_brandconsensus v WITH (nolock) ON v.subscriptionid = sd.subscriptionid  " +
                                            " 	join mastercodesheet ms WITH (nolock) ON v.masterid = ms.masterid  " +
                                            " join branddetails bd5 WITH ( nolock) ON bd5.brandid = v.brandid  " +
                                            " WHERE  bd5.brandid = " + filter.BrandID + " AND ms.mastergroupid = " + strID[1] + ") inn3 on sfilter.SubscriptionID = inn3.SubscriptionID	 " +
                                            " WHERE inn3.SubscriptionID is null ";
                                        query_notnull =
                                            " (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) " +
                                                " join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID " +
                                                " join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID " +
                                                " join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID " +
                                                " where  (bd.BrandID = " + filter.BrandID + " and ms.MasterGroupID=" + strID[1] + " and (";
                                        query_isnotempty =
                                            " (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) " +
                                                " join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID " +
                                                " join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID " +
                                                " join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID " +
                                                " where  (bd.BrandID = " + filter.BrandID + " and ms.MasterGroupID=" + strID[1] + ")";
                                    }
                                }
                                else
                                {
                                    if (filter.BrandID == 0)
                                    {
                                        query_isempty =
                                            " (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock) " +
                                            " left outer join (select distinct sd.subscriptionID from subscriptiondetails sd WITH (nolock) " +
                                                " join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID " +
                                        " where ms.MasterGroupID=" + strID[1] + ") inn1 on sfilter.SubscriptionID = inn1.SubscriptionID " +
                                        " where inn1.subscriptionID is null ";
                                        query_notnull =
                                                " (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock) " +
                                                " join Subscriptiondetails sd  with (nolock) on sd.SubscriptionID = sfilter.SubscriptionID " +
                                                " join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID and ms.MasterGroupID=" + strID[1] +
                                            " where ";
                                        query_isnotempty =
                                                " (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock) " +
                                                " join Subscriptiondetails sd  with (nolock) on sd.SubscriptionID = sfilter.SubscriptionID " +
                                                " join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID and ms.MasterGroupID=" + strID[1];
                                    }
                                    else
                                    {
                                        query_isempty =
                                            " (select distinct sfilter.subscriptionid FROM subscriptions sfilter WITH (nolock) " +
                                            " join pubsubscriptions ps1 WITH (nolock) ON sfilter.subscriptionID = ps1.subscriptionID " +
                                            " join BrandDetails bd with (nolock) on bd.PubID = ps1.PubID and bd.BrandID = " + filter.BrandID + " " +
                                        " left outer join ( " +
                                            " select DISTINCT sd.subscriptionid  " +
                                        " FROM   subscriptiondetails sd WITH (nolock)  " +
                                            " 	   join vw_brandconsensus v WITH (nolock) ON v.subscriptionid = sd.subscriptionid  " +
                                            " 	   join mastercodesheet ms WITH (nolock) ON v.masterid = ms.masterid  " +
                                            " 	   join branddetails bd5 WITH ( nolock) ON bd5.brandid = v.brandid  " +
                                        " WHERE  bd5.brandid = " + filter.BrandID + " AND ms.mastergroupid = " + strID[1] + ") inn3 on sfilter.SubscriptionID = inn3.SubscriptionID	 " +
                                        " WHERE inn3.SubscriptionID is null ";
                                        query_notnull =
                                                " (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) " +
                                                    " join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID " +
                                                    " join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID " +
                                                    " join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID " +
                                                " where  (bd.BrandID = " + filter.BrandID + " and ms.MasterGroupID=" + strID[1] + " and (";
                                        query_isnotempty =
                                                " (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) " +
                                                    " join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID " +
                                                    " join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID " +
                                                    " join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID " +
                                                    " where  (bd.BrandID = " + filter.BrandID + " and ms.MasterGroupID=" + strID[1] + ")";
                                    }
                                }
                                includeSubscriptionTbl = true;
                                switch (field.SearchCondition.ToUpper())
                                {

                                    case "EQUAL":
                                        wherecondition += " s.SubscriptionID in " + query_notnull;
                                        includeSubscriptionTbl = true;
                                        break;
                                    case "CONTAINS":
                                        wherecondition += " s.SubscriptionID in " + query_notnull;
                                        includeSubscriptionTbl = true;
                                        break;
                                    case "DOES NOT CONTAIN":
                                        wherecondition += " s.SubscriptionID not in " + query_notnull;
                                        includeSubscriptionTbl = true;
                                        break;
                                    case "START WITH":
                                        wherecondition += " s.SubscriptionID in " + query_notnull;
                                        includeSubscriptionTbl = true;
                                        break;
                                    case "END WITH":
                                        wherecondition += " s.SubscriptionID in " + query_notnull;
                                        includeSubscriptionTbl = true;
                                        break;
                                    case "IS EMPTY":
                                        wherecondition += " s.SubscriptionID in " + query_isempty;
                                        includeSubscriptionTbl = true;
                                        break;
                                    case "IS NOT EMPTY":
                                        wherecondition += " s.SubscriptionID in " + query_isnotempty;
                                        includeSubscriptionTbl = true;
                                        break;
                                }

                                string[] strAdhoc = field.Values.Split(',');

                                for (int i = 0; i < strAdhoc.Length; i++)
                                {
                                    switch (field.SearchCondition.ToUpper())
                                    {
                                        case "EQUAL":
                                            wherecondition += (i > 0 ? " OR " : "") + " (ms.MasterDesc = '" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()) + "' or ms.Mastervalue = '" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()) + "') ";
                                            break;
                                        case "CONTAINS":
                                        case "DOES NOT CONTAIN":
                                            wherecondition += (i > 0 ? " OR " : "") + " (ms.MasterDesc like '%" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "%'  or  ms.Mastervalue like '%" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "%') ";
                                            break;
                                        case "START WITH":
                                            wherecondition += (i > 0 ? " OR " : "") + "(ms.MasterDesc like '" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "%' or ms.Mastervalue like '" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "%') ";
                                            break;
                                        case "END WITH":
                                            wherecondition += (i > 0 ? " OR " : "") + " (ms.MasterDesc like '%" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "' or  ms.Mastervalue like '%" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "') ";
                                            break;
                                    }
                                }

                                if (filter.BrandID > 0 && !string.Equals("IS EMPTY", field.SearchCondition, StringComparison.OrdinalIgnoreCase) && !string.Equals("IS NOT EMPTY", field.SearchCondition, StringComparison.OrdinalIgnoreCase))
                                {
                                    wherecondition += "))";
                                }

                                wherecondition += ")";
                            }
                            else if (strID[0] == "d")
                            {
                                var i = 0;

                                string wherecondition_field = string.Empty;

                                if (strID[1].ToLower().Contains("qdate") || strID[1].ToLower().Contains("qualificationdate"))
                                {
                                    wherecondition_field = "ps.QualificationDate";
                                }
                                else if (strID[1].ToLower().Contains("[transactiondate]") || strID[1].ToLower().Contains("[pubtransactiondate]"))
                                {
                                    if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                        wherecondition_field = "convert(date,ps." + strID[1] + ")";
                                    else
                                        wherecondition_field = "convert(date,s." + strID[1] + ")";
                                }
                                else if (strID[1].ToLower().Contains("[statusupdateddate]"))
                                {
                                    wherecondition_field = "convert(date,ps." + strID[1] + ")";
                                }
                                else if (strID[1].ToLower().Contains("datecreated") || strID[1].ToLower().Contains("dateupdated"))
                                {
                                    if (filter.BrandID > 0 || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                        wherecondition_field = "ps." + strID[1];
                                    else
                                    {
                                        includeSubscriptionTbl = true;
                                        wherecondition_field = "s." + strID[1];
                                    }
                                }
                                else
                                {
                                    if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                        wherecondition_field = "ps." + strID[1];
                                    else
                                    {
                                        includeSubscriptionTbl = true;
                                        wherecondition_field = "s." + strID[1];
                                    }
                                }

                                var builder = new StringBuilder(wherecondition);
                                AppendDateCondition(builder, wherecondition_field, field.SearchCondition, field.Values);
                                wherecondition = builder.ToString();
                            }
                            else if (strID[0] == "i" || strID[0] == "f")
                            {
                                if (strID[1].Equals("[PRODUCT COUNT]", StringComparison.OrdinalIgnoreCase))
                                {

                                    wherecondition += "(";
                                    includeSubscriptionTbl = true;
                                    if (filter.BrandID > 0)
                                    {
                                        wherecondition += " s.SubscriptionID in (select ps2.subscriptionid FROM pubsubscriptions ps2 WITH (nolock) join branddetails bd2 WITH (nolock) ON bd2.pubid = ps2.pubid where bd2.BrandID = " + filter.BrandID + " group by ps2.subscriptionid ";
                                    }
                                    else
                                    {
                                        wherecondition += " s.SubscriptionID in (select ps2.subscriptionid FROM pubsubscriptions ps2 WITH (nolock) group by ps2.subscriptionid ";
                                    }

                                    switch (field.SearchCondition.ToUpper())
                                    {
                                        case "EQUAL":
                                            wherecondition += " having COUNT(ps2.PubID) = " + field.Values + ")";
                                            break;
                                        case "GREATER":
                                            wherecondition += " having COUNT(ps2.PubID) > " + field.Values + ")";
                                            break;
                                        case "LESSER":
                                            wherecondition += " having COUNT(ps2.PubID) < " + field.Values + ")";
                                            break;
                                    }

                                    wherecondition += ")";
                                }
                                else
                                {
                                    if (filter.BrandID != 0 && strID[1].ToUpper() == "[SCORE]")
                                    {
                                        includeSubscriptionTbl = true;
                                        query += " join BrandScore bs  with (nolock)  on s.SubscriptionId = bs.SubscriptionId and bs.BrandID = " + filter.BrandID;
                                    }

                                    wherecondition += "(";

                                    string[] strValue = field.Values.Split('|');

                                    int i = 0;

                                    switch (field.SearchCondition.ToUpper())
                                    {
                                        case "EQUAL":

                                            if (strValue[0] != "")
                                            {
                                                if (filter.BrandID != 0 && strID[1].ToUpper() == "[SCORE]")
                                                {
                                                    wherecondition += "bs." + strID[1] + " = " + strValue[0];
                                                }
                                                else
                                                {
                                                    if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                                        wherecondition += "ps." + strID[1] + " = " + strValue[0];
                                                    else
                                                    {
                                                        includeSubscriptionTbl = true;
                                                        wherecondition += "s." + strID[1] + " = " + strValue[0];
                                                    }

                                                }
                                                i = 1;
                                            }
                                            break;
                                        case "RANGE":

                                            if (strValue[0] != "")
                                            {
                                                if (filter.BrandID != 0 && strID[1].ToUpper() == "[SCORE]")
                                                {
                                                    wherecondition += "bs." + strID[1] + " >= " + strValue[0];
                                                }
                                                else
                                                {
                                                    if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                                        wherecondition += "ps." + strID[1] + " >= " + strValue[0];
                                                    else
                                                    {
                                                        includeSubscriptionTbl = true;
                                                        wherecondition += "s." + strID[1] + " >= " + strValue[0];
                                                    }

                                                }
                                                i = 1;
                                            }

                                            if (strValue[1] != "")
                                            {
                                                if (filter.BrandID != 0 && strID[1].ToUpper() == "[SCORE]")
                                                {
                                                    wherecondition += (i > 0 ? " and " : "") + "bs." + strID[1] + " <= " + strValue[1];
                                                }
                                                else
                                                {
                                                    if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                                        wherecondition += (i > 0 ? " and " : "") + "ps." + strID[1] + " <= " + strValue[1];
                                                    else
                                                    {
                                                        includeSubscriptionTbl = true;
                                                        wherecondition += (i > 0 ? " and " : "") + "s." + strID[1] + " <= " + strValue[1];
                                                    }

                                                }
                                            }
                                            break;
                                        case "GREATER":

                                            if (strValue[0] != "")
                                            {
                                                if (filter.BrandID != 0 && strID[1].ToUpper() == "[SCORE]")
                                                {
                                                    wherecondition += "bs." + strID[1] + " > " + strValue[0];
                                                }
                                                else
                                                {
                                                    if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                                        wherecondition += "ps." + strID[1] + " > " + strValue[0];
                                                    else
                                                    {
                                                        includeSubscriptionTbl = true;
                                                        wherecondition += "s." + strID[1] + " > " + strValue[0];
                                                    }

                                                }
                                                i = 1;
                                            }
                                            break;
                                        case "LESSER":

                                            if (strValue[0] != "")
                                            {
                                                if (filter.BrandID != 0 && strID[1].ToUpper() == "[SCORE]")
                                                {
                                                    wherecondition += "bs." + strID[1] + " < " + strValue[0];
                                                }
                                                else
                                                {
                                                    if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                                        wherecondition += "ps." + strID[1] + " < " + strValue[0];
                                                    else
                                                    {
                                                        includeSubscriptionTbl = true;
                                                        wherecondition += "s." + strID[1] + " < " + strValue[0];
                                                    }
                                                }
                                                i = 1;
                                            }
                                            break;
                                    }

                                    wherecondition += ")";
                                }
                            }
                            else if (strID[0] == "e")
                            {
                                //if (sub_query.Length >= 5)
                                //{
                                //    sub_query = sub_query.Remove(sub_query.Length - 5);
                                //}

                                if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                {
                                    if (field.SearchCondition.ToUpper() != "DOES NOT CONTAIN" && field.SearchCondition.ToUpper() != "IS EMPTY")
                                    {
                                        wherecondition += " ps.PubSubscriptionID in (select E.PubSubscriptionID" +
                                                          " FROM PubSubscriptionsExtension E with (nolock) join pubsubscriptions ps on ps.pubsubscriptionID = E.pubsubscriptionID" +
                                                          " WHERE ps.PubID = " + filter.PubID + " and ";
                                    }
                                    else
                                    {
                                        wherecondition += " ps.PubSubscriptionID not in (select E.PubSubscriptionID" +
                                                          " FROM PubSubscriptionsExtension E with (nolock) join pubsubscriptions ps on ps.pubsubscriptionID = E.pubsubscriptionID" +
                                                          " WHERE ps.PubID = " + filter.PubID + " and ";
                                    }
                                }
                                else
                                {
                                    includeSubscriptionTbl = true;
                                    if (field.SearchCondition.ToUpper() != "DOES NOT CONTAIN" && field.SearchCondition.ToUpper() != "IS EMPTY")
                                    {
                                        wherecondition += " s.SubscriptionID in (select E.SubscriptionID" +
                                                          " FROM SubscriptionsExtension E with (nolock) " +
                                                          " WHERE ";
                                    }
                                    else
                                    {
                                        wherecondition += " s.SubscriptionID not in (select E.SubscriptionID" +
                                                          " FROM SubscriptionsExtension E with (nolock) " +
                                                          " WHERE ";
                                    }
                                }

                                string columnName = string.Concat("E.", strID[1]);

                                switch (strID[2].ToLower())
                                {
                                    case "i":
                                    case "f":
                                        {
                                            columnName = string.Compare(strID[2], "i", true) == 0
                                                ? string.Format("CAST({0} AS INT)", columnName)
                                                : string.Format("CAST({0} AS FLOAT)", columnName);

                                            string[] strValue = field.Values.Split('|');
                                            wherecondition += "(";

                                            switch (field.SearchCondition.ToUpper())
                                            {
                                                case "EQUAL":

                                                    if (strValue[0] != "")
                                                    {
                                                        wherecondition += columnName + " = " + strValue[0];
                                                    }
                                                    break;
                                                case "RANGE":

                                                    if (strValue[0] != "")
                                                    {
                                                        wherecondition += columnName + " >= " + strValue[0];
                                                    }

                                                    if (strValue[1] != "")
                                                    {
                                                        wherecondition += (strValue[0] != "" ? " and " : " ") + columnName + " <= " + strValue[1];
                                                    }
                                                    break;
                                                case "GREATER":

                                                    if (strValue[0] != "")
                                                    {
                                                        wherecondition += columnName + " > " + strValue[0];
                                                    }
                                                    break;
                                                case "LESSER":

                                                    if (strValue[0] != "")
                                                    {
                                                        wherecondition += columnName + " < " + strValue[0];
                                                    }
                                                    break;
                                            }

                                            wherecondition += "))";
                                            break;
                                        }
                                    case "b":
                                        {
                                            wherecondition += string.Format("CAST({0} AS BIT) = {1})", columnName, field.Values);
                                            break;
                                        }
                                    case "d":
                                        {
                                            columnName = string.Format("case when IsDate({0}) = 1 then CAST({0} AS DATETIME) else null end", columnName);
                                            var builder = new StringBuilder(wherecondition);
                                            AppendDateCondition(builder, columnName, field.SearchCondition, field.Values);
                                            wherecondition = builder.ToString();
                                            break;
                                        }
                                    default:
                                        {
                                            string[] strAdhoc = field.Values.Split(',');

                                            for (int i = 0; i < strAdhoc.Length; i++)
                                            {
                                                switch (field.SearchCondition.ToUpper())
                                                {
                                                    case "EQUAL":
                                                        wherecondition += (i > 0 ? " OR " : "") + " " + columnName + " = '" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()) + "'";
                                                        break;
                                                    case "CONTAINS":
                                                    case "DOES NOT CONTAIN":
                                                        wherecondition += (i > 0 ? " OR " : "") + " " + "PATINDEX('%" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "%', " + columnName + ") > 0 ";
                                                        break;
                                                    case "START WITH":
                                                        wherecondition += (i > 0 ? " OR " : "") + " " + "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "%', " + columnName + ") > 0 ";
                                                        break;
                                                    case "END WITH":
                                                        wherecondition += (i > 0 ? " OR " : "") + " " + "PATINDEX('%" + Object.Helper.ReplaceSingleQuotes(strAdhoc[i].Trim()).Replace("_", "[_]") + "', " + columnName + ") > 0 ";
                                                        break;
                                                    case "IS EMPTY":
                                                    case "IS NOT EMPTY":
                                                        wherecondition += (i > 0 ? " OR " : "") + " (ISNULL(" + columnName + ", '') != '' " + " )";
                                                        break;
                                                }
                                            }

                                            wherecondition += ")";
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        {
                            string wherecondition_field = string.Empty;
                            
                            if (field.Group.Equals("[COUNTRY]", StringComparison.OrdinalIgnoreCase))
                            {
                                if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                {
                                    if (field.SearchCondition.Equals("IS EMPTY", StringComparison.OrdinalIgnoreCase))
                                    {
                                        query += " left outer join UAD_Lookup..Country c on c.CountryID = ps.countryID ";
                                    }
                                    else
                                    {
                                        query += " join UAD_Lookup..Country c on c.CountryID = ps.countryID ";
                                    }
                                }
                                else
                                {
                                    includeSubscriptionTbl = true;
                                    if (field.SearchCondition.Equals("IS EMPTY", StringComparison.OrdinalIgnoreCase))
                                    {
                                        query += " left outer join UAD_Lookup..Country c on c.CountryID = s.countryID ";
                                    }
                                    else
                                    {
                                        query += " join UAD_Lookup..Country c on c.CountryID = s.countryID ";
                                    }
                                }

                                wherecondition_field = "c.ShortName";
                            }
                            else if (field.Group.Equals("[EMAIL]", StringComparison.OrdinalIgnoreCase))
                            {
                                wherecondition_field = "ps." + field.Group;
                            }
                            else if (field.Group.Equals("[IGRP_NO]", StringComparison.OrdinalIgnoreCase))
                            {
                                includeSubscriptionTbl = true;
                                wherecondition_field = "cast(s." + field.Group + " as varchar(100))";
                            }
                            else
                            {
                                if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                                    wherecondition_field = "ps." + field.Group;
                                else
                                {
                                    includeSubscriptionTbl = true;
                                    wherecondition_field = "s." + field.Group;
                                }
                            }

                            wherecondition = CreateStringCondition(wherecondition, field.Group, field.SearchCondition, field.Values, wherecondition_field);
                        }
                        break;
                    #endregion
                    case "ZIPCODE-RADIUS":
                        includeSubscriptionTbl = true;
                        wherecondition += CreateZipCodeRadiusCondition(field.SearchCondition);
                        break;
                    case "LAST NAME":
                        if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', ps.LastName ) > 0 ";
                        else
                        {
                            includeSubscriptionTbl = true;
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', s.lname ) > 0 ";
                        }
                        break;
                    case "FIRST NAME":
                        if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', ps.FirstName ) > 0 ";
                        else
                        {
                            includeSubscriptionTbl = true;
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', s.fname ) > 0 ";
                        }
                        break;
                    case "COMPANY":
                        if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', ps.Company ) > 0 ";
                        else
                        {
                            includeSubscriptionTbl = true;
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', s.company ) > 0 ";
                        }
                        break;
                    case "PHONENO":
                        if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', ps.phone ) > 0 ";
                        else
                        {
                            includeSubscriptionTbl = true;
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', s.phone ) > 0 ";
                        }


                        break;
                    case "EMAILID":
                        if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', ps.emailID ) > 0 ";
                        else
                        {
                            includeSubscriptionTbl = true;
                            wherecondition += "PATINDEX('" + Object.Helper.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', s.email ) > 0 ";
                        }


                        break;
                    case "WAVE MAILING":
                        wherecondition += " ISNULL(ps.IsInActiveWaveMailing,0) = " + field.Values;
                        break;
                    case "QUALIFICATIONYEAR":
                        var pubID = Convert.ToInt32(profilefields.Find(x => x.Name.Equals("PRODUCT", StringComparison.OrdinalIgnoreCase)).Values);
                        wherecondition += CreateQualificationYearCondition(clientconnection, pubID, field.Values);
                        break;
                }

            }

            #endregion

            #region Dimension fields
            // Loop through all of the Dimensions's
            // the PubId's go into the sub_query list as it limits the number of rows we need to get
            // All the other business/function/industry/etc fields


            // Now loop through all of the added master_ids
            // by putting them into a list and looping over the list, creating 
            // something called the bubble_up_query. This is a recursive query that places each
            // master id group in it's own query to properly get the OR behavior between each of these master id groups.

            bool added_master_id = false;
            string dimquery = "";
            string dimquery_start = "";
            // Each one of the queries starts the same way either selecting the igrp_no or the cgrp_no depending on if we want individual or company consensus
            //string bubble_up_query_igrp_start = "select distinct igrp_no from subscriptions sfilter join subscriptiondetails sd on sd.subscriptionid = sfilter.subscriptionid where ";


            if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
            {
                //dimquery_start = "select distinct ps1.pubsubscriptionID  from pubsubscriptions ps1  with (nolock) join PubSubscriptionDetail psd  with (nolock) on ps1.pubsubscriptionID = psd.pubsubscriptionID  where ";
                dimquery_start = "select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) join pubsubscriptions ps1  with (nolock) on sfilter.subscriptionID = ps1.subscriptionID join PubSubscriptionDetail psd  with (nolock) on ps1.pubsubscriptionID = psd.pubsubscriptionID  where ";
            }
            else if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.RecencyView)
            {
                if (filter.BrandID > 0)
                {
                    dimquery_start = " select distinct vrbc.subscriptionid from vw_RecentBrandConsensus vrbc with (nolock) where ";
                }
                else
                {
                    dimquery_start = "select distinct vrc.SubscriptionID from vw_RecentConsensus vrc with (nolock) where ";
                }

            }
            else
            {
                if (filter.BrandID > 0)
                {
                    dimquery_start = " select distinct vbc.subscriptionid from vw_BrandConsensus vbc with (nolock) where ";
                }
                else
                {
                    dimquery_start = "select distinct sd.SubscriptionID from SubscriptionDetails sd  with (nolock) where ";
                }
            }

            var dimensionfields = (from f in filter.Fields
                                   where f.FilterType == FrameworkUAD.BusinessLogic.Enums.FiltersType.Dimension
                                   //where f.Column.ToUpper() == "MASTERID"
                                   select f
                                       ).ToList();

            foreach (Object.FilterDetails f in dimensionfields)
            {
                if (dimquery != "")
                    dimquery += " intersect ";

                dimquery += dimquery_start;

                if (added_master_id)
                {
                    if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                    {
                        dimquery += " ps1.pubID = " + filter.PubID + " and psd.codesheetID in ( " + f.Values + " ) ";
                    }
                    else if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.RecencyView)
                    {
                        if (filter.BrandID > 0)
                        {
                            dimquery += " vrbc.brandid = " + filter.BrandID + " and vrbc.masterid  in ( " + f.Values + " ) ";
                        }
                        else
                        {
                            dimquery += " vrc.masterid  in ( " + f.Values + " ) ";
                        }
                    }
                    else
                    {
                        if (filter.BrandID > 0)
                        {
                            dimquery += " vbc.brandid= " + filter.BrandID + " and vbc.masterid  in ( " + f.Values + " ) ";
                        }
                        else
                        {
                            dimquery += " sd.masterid  in ( " + f.Values + " ) ";
                        }
                    }
                }
                else
                {
                    // This is the first (or only) mid list selected so we need to pair it down via the subquery
                    //bubble_up_query = bubble_up_query_start + " ";

                    //if (sub_query.Trim().Length > 0)
                    //    bubble_up_query += sub_query + " and ";
                    if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                    {
                        dimquery += " psd.codesheetID in (" + f.Values + " ) ";
                    }
                    else if (filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.RecencyView)
                    {
                        if (filter.BrandID > 0)
                        {
                            dimquery += " vrbc.brandid= " + filter.BrandID + " and vrbc.masterid in (" + f.Values + " ) ";
                        }
                        else
                        {
                            dimquery += " vrc.masterid in (" + f.Values + " ) ";
                        }
                    }
                    else
                    {
                        if (filter.BrandID > 0)
                        {
                            dimquery += " vbc.brandid= " + filter.BrandID + " and vbc.masterid in (" + f.Values + " ) ";
                        }
                        else
                        {
                            dimquery += " sd.masterid in (" + f.Values + " ) ";
                        }
                    }
                }
                added_master_id = true;
            }

            #endregion

            #region Activity Fields

            var Activityfields = (from f in filter.Fields
                                  where f.FilterType == FrameworkUAD.BusinessLogic.Enums.FiltersType.Activity
                                  //where f.Column.ToUpper() == "ACTIVITY"
                                  select f
                            ).ToList();

            var OpenCondition = new StringBuilder();
            var openBlastCondition = new StringBuilder();

            var ClickCondition = new StringBuilder();
            var clickBlastCondition = new StringBuilder();
            
            var VisitCondition = new StringBuilder();
            string openquery = string.Empty;
            string clickquery = string.Empty;
            string visitquery = string.Empty;
            int opencount = -1;
            int clickcount = -1;
            int visitcount = -1;
            string link = string.Empty;
            string domainTrackingID = string.Empty;
            string url = string.Empty;
            string openSearchType = string.Empty;
            string clickSearchType = string.Empty;

            bool bjoinBlastforOpen = false;
            bool bjoinBlastforClick = false;

            foreach (Object.FilterDetails f in Activityfields)
            {
                #region Activity field for loop
                switch (f.Name.ToUpper())
                {
                    // DIRECT INFO CASES
                    case "OPEN CRITERIA":
                        openSearchType = f.SearchCondition;
                        opencount = Convert.ToInt32(f.Values);
                        break;

                    case "OPEN ACTIVITY":
                        AppendDateNumberCondition(OpenCondition, f.SearchCondition, f.Values);
                        break;

                    case "OPEN BLASTID":
                        bjoinBlastforOpen = true;
                        openBlastCondition.Append(openBlastCondition.Length > 0 ? " and bla.BlastID in (" + f.Values + ")" : " where bla.BlastID in (" + f.Values + ")");
                        break;

                    case "OPEN CAMPAIGNS":
                        bjoinBlastforOpen = true;
                        openBlastCondition.Append(openBlastCondition.Length > 0 ? " and bla.ECNCampaignID in (" + f.Values + ")" : " where bla.ECNCampaignID in (" + f.Values + ")");
                        break;

                    case "OPEN EMAIL SUBJECT":
                        bjoinBlastforOpen = true;
                        openBlastCondition.Append(openBlastCondition.Length > 0 ? " and bla.Emailsubject like '%" + Object.Helper.ReplaceSingleQuotes(f.Values.Trim()).Replace("_", "[_]") + "%'" : " Where bla.Emailsubject like '%" + Object.Helper.ReplaceSingleQuotes(f.Values.Trim()).Replace("_", "[_]") + "%'");
                        break;

                    case "OPEN EMAIL SENT DATE":
                        bjoinBlastforOpen = true;
                        AppendDateCondition(openBlastCondition, BlastSendTimeColumnName, f.SearchCondition, f.Values);
                        break;

                    case "CLICK CRITERIA":
                        clickSearchType = f.SearchCondition;
                        clickcount = Convert.ToInt32(f.Values);
                        break;

                    case "LINK":
                        link = f.Values;

                        break;

                    case "CLICK ACTIVITY":
                        AppendDateNumberCondition(ClickCondition, f.SearchCondition, f.Values);
                        break;

                    case "CLICK BLASTID":
                        bjoinBlastforClick = true;
                        clickBlastCondition.Append(clickBlastCondition.Length > 0 ? " and bla.BlastID in (" + f.Values + ")" : " where bla.BlastID in (" + f.Values + ")");
                        break;

                    case "CLICK CAMPAIGNS":
                        bjoinBlastforClick = true;
                        clickBlastCondition.Append(clickBlastCondition.Length > 0 ? " and bla.ECNCampaignID in (" + f.Values + ")" : " where bla.ECNCampaignID in (" + f.Values + ")");
                        break;

                    case "CLICK EMAIL SUBJECT":
                        bjoinBlastforClick = true;
                        clickBlastCondition.Append(clickBlastCondition.Length > 0 ? " and bla.Emailsubject like '%" + Object.Helper.ReplaceSingleQuotes(f.Values.Trim()).Replace("_", "[_]") + "%'" : " Where bla.Emailsubject like '%" + Object.Helper.ReplaceSingleQuotes(f.Values.Trim()).Replace("_", "[_]") + "%'");
                        break;

                    case "CLICK EMAIL SENT DATE":
                        bjoinBlastforClick = true;
                        AppendDateCondition(clickBlastCondition, BlastSendTimeColumnName, f.SearchCondition, f.Values);
                        break;

                    case "DOMAIN TRACKING":
                        domainTrackingID = f.Values;
                        break;

                    case "URL":
                        url = f.Values;
                        break;

                    case "VISIT CRITERIA":
                        visitcount = Convert.ToInt32(f.Values);
                        break;

                    case "VISIT ACTIVITY":
                        AppendDateNumberCondition(VisitCondition, f.SearchCondition, f.Values);
                        break;
                }
                #endregion
            }

            if (pubIDs.Length > 0)
            {
                if (string.Equals("Search Selected Products", openSearchType, StringComparison.OrdinalIgnoreCase) || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                {
                    OpenCondition.Append(OpenCondition.Length > 0 ? " and pso.pubID in (" + pubIDs + ")" : " where pso.pubID in (" + pubIDs + ")");
                }
                if (string.Equals("Search Selected Products", clickSearchType, StringComparison.OrdinalIgnoreCase) || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                {
                    ClickCondition.Append(ClickCondition.Length > 0 ? " and psc.pubID in (" + pubIDs + ")" : " where psc.pubID in (" + pubIDs + ")");
                }
            }

            string activityWhereCondition = string.Empty;

            #region Open

            if (opencount >= 0)
            {
                if (string.Equals("Search All", openSearchType, StringComparison.OrdinalIgnoreCase) && !(filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView || filter.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView))
                {
                    #region Search all Products
                    if (filter.BrandID > 0)
                    {
                        //brand

                        openquery += "select x.subscriptionID from (";
                        openquery += " select so.SubscriptionID, openactivityID from SubscriberOpenActivity so with (NOLOCK) " +
                                       " join PubSubscriptions pso  with (NOLOCK) on so.PubSubscriptionID = pso.PubSubscriptionID ";

                        if (bjoinBlastforOpen)
                        {
                            openquery += " join #tempOblast tob  with (NOLOCK) on so.blastID = tob.blastID ";
                        }

                        openquery += OpenCondition;
                        openquery += (OpenCondition.Length > 0 ? " and " : " where ");
                        openquery += " pubID in (select PubID from BrandDetails bd  with (nolock) join Brand b with (nolock) on bd.BrandID = b.BrandID where bd.BrandID in (" + filter.BrandID + ") and  b.Isdeleted = 0) ";

                        // ADD UNION

                        openquery += " union select so1.SubscriptionID, so1.openactivityID from SubscriberOpenActivity so1 with (NOLOCK) ";

                        if (bjoinBlastforOpen)
                        {
                            openquery += " join #tempOblast tob  with (NOLOCK) on so1.blastID = tob.blastID ";
                        }

                        openquery += OpenCondition;
                        openquery += (OpenCondition.Length > 0 ? " and " : " where ");
                        openquery += " so1.pubsubscriptionid IS NULL ";

                        openquery += " ) x GROUP  BY x.subscriptionid ";
                        openquery += (opencount > 0 ? " HAVING Count(x.openactivityid) >= " + opencount.ToString() : " ");
                    }
                    else
                    {
                        //consensus
                        openquery += " select so.SubscriptionID from SubscriberOpenActivity so  with (NOLOCK) ";

                        if (bjoinBlastforOpen)
                        {
                            openquery += " join #tempOblast tob  with (NOLOCK) on so.blastID = tob.blastID ";
                        }

                        openquery += OpenCondition;

                        openquery += " group by so.SubscriptionID";

                        openquery += (opencount > 0 ? " HAVING Count(so.openactivityid) >= " + opencount.ToString() : " ");
                    }
                    #endregion
                }
                else
                {
                    #region Search Specific Products
                    openquery = " select so.SubscriptionID  from PubSubscriptions pso   with (NOLOCK) " +
                                       " join SubscriberOpenActivity so  with (NOLOCK) on so.PubSubscriptionID = pso.PubSubscriptionID ";

                    if (filter.BrandID > 0)
                    {
                        OpenCondition.Append(OpenCondition.Length > 0 ? " and " : " where ");
                        OpenCondition.Append(" bd.BrandID in (" + filter.BrandID + ") and b.Isdeleted = 0");
                        openquery += " JOIN branddetails bd  with (NOLOCK) ON bd.pubID = pso.pubID join Brand b  with (NOLOCK) on b.BrandID = bd.BrandID ";
                    }

                    if (bjoinBlastforOpen)
                    {
                        openquery += " join #tempOblast tob  with (NOLOCK) on so.blastID = tob.blastID ";
                    }

                    openquery += OpenCondition;

                    openquery += " group by so.SubscriptionID";

                    openquery += (opencount > 0 ? " HAVING Count(so.openactivityid) >= " + opencount.ToString() : " ");

                    #endregion
                }
            }

            #endregion

            #region Click

            if (link.Length > 0)
            {
                if (clickcount < 0)
                    clickcount = 1;

                string linkquery = string.Empty;

                string[] strlink = link.Split(',');

                for (int i = 0; i < strlink.Length; i++)
                {
                    linkquery += (linkquery.Length > 0 ? " or (Link like '%" + Object.Helper.ReplaceSingleQuotes(strlink[i].Trim()).Replace("_", "[_]") + "%' or  LinkAlias like '%" + Object.Helper.ReplaceSingleQuotes(strlink[i].Trim()).Replace("_", "[_]") + "%')" : " (Link like'%" + Object.Helper.ReplaceSingleQuotes(strlink[i].Trim()).Replace("_", "[_]") + "%' or LinkAlias like '%" + Object.Helper.ReplaceSingleQuotes(strlink[i].Trim()).Replace("_", "[_]") + "%')");
                }

                ClickCondition.Append(ClickCondition.Length > 0 ? " and (" + linkquery + ")" : " Where (" + linkquery + ")");
            }

            clickquery = ClickQuery(filter, clickcount, clickSearchType, bjoinBlastforClick, ClickCondition);

            #endregion

            #region Visit Activity

            if (visitcount >= 0)
            {

                visitquery = " select sv.SubscriptionID from " +
                                " SubscriberVisitActivity sv  with (NOLOCK) ";

                if (domainTrackingID.Length > 0)
                {
                    VisitCondition.Append(VisitCondition.Length > 0 ? " and sv.DomainTrackingID = " + domainTrackingID : " where sv.DomainTrackingID = " + domainTrackingID);
                }

                if (url.Length > 0)
                {
                    string[] strUrl = url.Split(',');

                    string urlquery = string.Empty;

                    for (int i = 0; i < strUrl.Length; i++)
                    {
                        urlquery += (urlquery.Length > 0 ? " or sv.url like '%" + Object.Helper.ReplaceSingleQuotes(strUrl[i].Trim()).Replace("_", "[_]") + "%'" : " sv.url like '%" + Object.Helper.ReplaceSingleQuotes(strUrl[i].Trim()).Replace("_", "[_]") + "%'");
                    }

                    VisitCondition.Append(VisitCondition.Length > 0 ? " and (" + urlquery + ")" : " Where (" + urlquery + ")");
                }

                if (VisitCondition.Length > 0)
                {
                    visitquery += VisitCondition;
                }

                visitquery += " group by sv.SubscriptionID";

                visitquery += (visitcount > 0 ? " having COUNT(sv.VisitActivityID) >= " + visitcount.ToString() : " ");

            }
            #endregion


            #endregion
            // If we have added master_ids we need to select based on the cgrp or igrp entities that the bubble_up_query selects for us
            // Otherwise we can just add the sub query and it will select things ignoring their survey responses
            if (added_master_id)
            {
                createTempTablequery += " Create table #tempDimSub (SubscriptionID int);";
                createTempTablequery += " CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (SubscriptionID); ";

                createTempTablequery += " Insert into #tempDimSub " + dimquery + ";";

                dropTempTablequery += " drop table #tempDimSub; ";
            }

            if (openquery.Length > 0)
            {
                if (bjoinBlastforOpen)
                {
                    createTempTablequery += "CREATE TABLE #tempOblast (blastid INT); ";
                    createTempTablequery += "CREATE UNIQUE CLUSTERED INDEX #tempOblast_1 ON #tempOblast (blastid);  ";
                    createTempTablequery += " insert into #tempOblast SELECT distinct blastid FROM blast bla WITH(nolock)  ";
                    createTempTablequery += openBlastCondition;
                }

                createTempTablequery += "; Create table #tempSOA (subscriptionid int);";
                createTempTablequery += " CREATE UNIQUE CLUSTERED INDEX tempSOA_1 on #tempSOA  (subscriptionid); ";

                createTempTablequery += " Insert into #tempSOA " + openquery + ";";

                includeSubscriptionTbl = true;
                if (opencount == 0)
                {
                    query += " left outer join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ";
                    wherecondition += (wherecondition.Length == 0 ? " " : " and ") + " soa1.SubscriptionID is null";
                }
                else
                {
                    query += " join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ";
                }



                dropTempTablequery += " drop table #tempSOA; ";
            }

            if (clickquery.Length > 0)
            {
                if (bjoinBlastforClick)
                {
                    createTempTablequery += "CREATE TABLE #tempCblast (blastid INT); ";
                    createTempTablequery += "CREATE UNIQUE CLUSTERED INDEX #tempCblast_1 ON #tempCblast (blastid);  ";
                    createTempTablequery += "Insert into #tempCblast SELECT distinct blastid FROM blast bla WITH (nolock)  ";
                    createTempTablequery += clickBlastCondition;
                }

                createTempTablequery += "; Create table #tempSCA (subscriptionid int);";
                createTempTablequery += " CREATE UNIQUE CLUSTERED INDEX tempSCA_1 on #tempSCA  (subscriptionid); ";

                createTempTablequery += " Insert into #tempSCA " + clickquery + ";";

                includeSubscriptionTbl = true;
                if (clickcount == 0)
                {
                    query += " left outer join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ";
                    wherecondition += (wherecondition.Length == 0 ? " " : " and ") + " sca1.SubscriptionID is null";
                }
                else
                    query += " join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ";

                dropTempTablequery += " drop table #tempSCA; ";
            }

            if (visitquery.Length > 0)
            {
                createTempTablequery += "; Create table #tempSVA (subscriptionid int);";
                createTempTablequery += " CREATE UNIQUE CLUSTERED INDEX tempSVA_1 on #tempSVA  (subscriptionid); ";

                createTempTablequery += " Insert into #tempSVA " + visitquery + ";";

                includeSubscriptionTbl = true;
                if (visitcount == 0)
                {
                    query += " left outer join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ";
                    wherecondition += (wherecondition.Length == 0 ? " " : " and ") + " sva1.SubscriptionID is null";
                }
                else
                {
                    query += " join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ";
                }

                dropTempTablequery += " drop table #tempSVA; ";
            }

            if (addlFilters.Length > 0)
                query += addlFilters;

            //txtShowQuery.Text = txtShowQuery.Text + "\r\n" + "\r\n" + query + (wherecondition.Length == 0 ? "" : " where " + wherecondition);

            if (bjoinBlastforOpen)
            {
                dropTempTablequery += "drop table #tempOblast; ";
            }

            if (bjoinBlastforClick)
            {
                dropTempTablequery += "drop table #tempCblast; ";
            }

            if (includeSubscriptionTbl)
            {
                if (IsAddRemove)
                {
                    if (added_master_id)
                    {
                        //query = "select " + select_list + " from #tempDimSub x4  with (nolock) join pubsubscriptions ps  with (nolock)  on x4.pubsubscriptionID = ps.PubSubscriptionID " + query;
                        query = "select " + select_list + " from #tempDimSub x4  with (nolock) join subscriptions s with (nolock) on x4.SubscriptionID = s.SubscriptionID join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID " +
                            " left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0 " + query;
                    }
                    else
                    {
                        query = "select " + select_list + " from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID " +
                            " left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0 " + query;
                    }
                }
                else
                {
                    if (added_master_id)
                        query = "select " + select_list + " from #tempDimSub x4  with (nolock) join subscriptions s with (nolock) on x4.SubscriptionID = s.SubscriptionID join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID " + query;
                    else
                        query = "select " + select_list + " from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID " + query;
                }
            }
            else
            {
                if (IsAddRemove)
                {
                    if (added_master_id)
                    {
                        query = "select " + select_list + " from #tempDimSub x4  with (nolock) join pubsubscriptions ps  with (nolock)  on x4.subscriptionID = ps.subscriptionID " +
                            " left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0 " + query;
                    }
                    else
                    {
                        query = "select " + select_list + " from pubsubscriptions ps  with (nolock) " +
                            " left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0 " + query;
                    }
                }
                else
                {
                    if (added_master_id)
                        query = "select " + select_list + " from #tempDimSub x4  with (nolock) join pubsubscriptions ps  with (nolock)  on x4.subscriptionID = ps.subscriptionID " + query;
                    else
                        query = "select " + select_list + " from pubsubscriptions ps  with (nolock) " + query;
                }


            }


            return createTempTablequery + query + (wherecondition.Length == 0 ? "" : " where " + wherecondition) + ";" + dropTempTablequery;
        }

        private static string ClickQuery(
            Object.FilterMVC filter,
            int clickcount,
            string clickSearchType,
            bool bjoinBlastforClick,
            StringBuilder clickCondition)
        {
            var clickquery = new StringBuilder();

            if (clickcount < 0)
            {
                return string.Empty;
            }

            if (string.Equals(ClickSearchTypeSearchAll, clickSearchType, StringComparison.OrdinalIgnoreCase)
                && (filter.ViewType != Enums.ViewType.ProductView && filter.ViewType != Enums.ViewType.CrossProductView))
            {
                return ClickQueryForSearchAll(filter, clickcount, bjoinBlastforClick, clickCondition);
            }

            return ClickQueryForNonSearchAll(filter, clickcount, bjoinBlastforClick, clickCondition);
        }

        private static string ClickQueryForNonSearchAll(
            Object.FilterMVC filter,
            int clickcount,
            bool bjoinBlastforClick,
            StringBuilder clickCondition)
        {
            var clickQuery = new StringBuilder();
            clickQuery
                .Append(" select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK) ")
                .Append(" join SubscriberClickActivity sc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID ");

            if (filter.BrandID > 0)
            {
                clickCondition
                    .Append(clickCondition.Length > 0
                                ? " and "
                                : " where ")
                    .Append($" bd.BrandID in ({filter.BrandID}) and b.Isdeleted = 0");

                clickQuery.Append(" JOIN branddetails bd  with (NOLOCK) ON bd.pubID = psc.pubID join Brand b  with (NOLOCK) on b.BrandID = bd.BrandID ");
            }

            if (bjoinBlastforClick)
            {
                clickQuery.Append(" join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID ");
            }

            clickQuery
                .Append(clickCondition)
                .Append(" group by sc.SubscriptionID")
                .Append(clickcount > 0
                            ? $" having COUNT(sc.ClickActivityID) >= {clickcount}"
                            : " ");

            return clickQuery.ToString();
        }

        private static string ClickQueryForSearchAll(
            Object.FilterMVC filter,
            int clickcount,
            bool bjoinBlastforClick,
            StringBuilder clickCondition)
        {
            var clickQuery = new StringBuilder();
            if (filter.BrandID > 0)
            {
                clickQuery
                    .Append("select x.subscriptionID from (")
                    .Append(" select sc.SubscriptionID, sc.ClickActivityID  from  SubscriberClickActivity sc  with (NOLOCK) ")
                    .Append(" join PubSubscriptions psc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID ")
                    .Append(AddJoinToTable(bjoinBlastforClick, clickCondition, "sc"))
                    .Append(" pubID in (select PubID from BrandDetails bd  with (nolock) join Brand b with (nolock) on bd.BrandID = b.BrandID where bd.BrandID in (")
                    .Append(filter.BrandID)
                    .Append(") and  b.Isdeleted = 0)")
                    .Append("union select sc1.SubscriptionID, sc1.ClickActivityID  from  SubscriberClickActivity sc1  with (NOLOCK) ")
                    .Append(AddJoinToTable(bjoinBlastforClick, clickCondition, "sc1"))
                    .Append(" sc1.pubsubscriptionid IS NULL ")
                    .Append(" ) x GROUP BY x.subscriptionid ")
                    .Append(clickcount > 0 
                                ? $" HAVING Count(x.ClickActivityID) >= {clickcount}"
                                : " ");
            }
            else
            {
                clickQuery.Append(" select sc.SubscriptionID from SubscriberClickActivity sc  with (NOLOCK) ");

                if (bjoinBlastforClick)
                {
                    clickQuery.Append(" join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID ");
                }

                clickQuery
                    .Append(clickCondition)
                    .Append(" group by sc.SubscriptionID")
                    .Append(clickcount > 0
                                ? $" HAVING Count(sc.ClickActivityID) >= {clickcount}"
                                : " ");
            }

            return clickQuery.ToString();
        }

        private static string AddJoinToTable(bool bjoinBlastforClick, StringBuilder clickCondition, string tableName)
        {
            var clickQuery = new StringBuilder();
            if (bjoinBlastforClick)
            {
                clickQuery.Append($" join #tempCblast tcb with (NOLOCK) on {tableName}.blastID = tcb.blastID ");
            }

            clickQuery.Append(clickCondition)
                .Append(clickCondition.Length > 0
                            ? " and "
                            : " where ");

            return clickQuery.ToString();
        }

        /// <summary>
        /// Gets the string to use as the begining of a condition
        /// </summary>
        /// <param name="whereCondition">the original where condition</param>
        /// <param name="field">field to check</param>
        /// <returns>the resulting where condition</returns>
        private static string GetConditionBeginning(string whereCondition, Object.FilterDetails field)
        {
            if (whereCondition != string.Empty)
            {
                switch (field.Name.ToUpper())
                {
                    case "CATEGORY TYPE":
                    case "QSOURCE TYPE":
                    case "XACT":
                        whereCondition += "  ";
                        break;
                    default:
                        whereCondition += " and ";
                        break;
                }
            }

            return whereCondition;
        }

        /// <summary>
        /// Checks if a fieldName exists in a filter
        /// </summary>
        /// <param name="filter">the filter</param>
        /// <param name="fieldName">the fieldName to check</param>
        /// <returns>true if exists</returns>
        private static bool FieldNameExists(Object.FilterMVC filter, string fieldName)
        {
            return filter.Fields.Exists(x => x.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
        }

        public static StringBuilder generateCombinationQuery(
            FilterCollection filterCollection,
            string selectedFilterOperation,
            string suppressedFilterOperation,
            string selectedFilterNo,
            string suppressedFilterNo,
            string additionalFilters,
            int productId,
            int brandId,
            ClientConnections clientConnections)
        {
            return GenerateCombinationQuery(
                filterCollection,
                selectedFilterOperation,
                suppressedFilterOperation,
                selectedFilterNo,
                suppressedFilterNo,
                additionalFilters,
                clientConnections,
                null,
                FilterQuery);
        }
      
        public static StringBuilder generateCombinationQueryForIssueArchived(
            FilterCollection filterCollection, 
            string selectedFilterOperation,
            string suppressedFilterOperation,
            string selectedFilterNo, 
            string suppressedFilterNo, 
            string additionalFilters, 
            int productId, 
            int brandId, 
            int issueId,
            ClientConnections clientConnections)
        {
            return GenerateCombinationQuery(
                filterCollection,
                selectedFilterOperation,
                suppressedFilterOperation,
                selectedFilterNo,
                suppressedFilterNo,
                additionalFilters,
                clientConnections,
                issueId,
                FilterQueryArchived);
        }

        private static string FilterQuery(Object.FilterMVC filter, string additionalFilters, ClientConnections clientConnections, int? issuedId)
        {
            return getFilterQuery(filter, $" distinct {filter.FilterNo}, ps.SubscriptionID ", additionalFilters, clientConnections);
        } 
        
        private static string FilterQueryArchived(Object.FilterMVC filter, string additionalFilters, ClientConnections clientConnections, int? issuedId)
        {
            return getProductArchiveFilterQuery(filter, $" distinct {filter.FilterNo}, ps.SubscriptionID ", additionalFilters, issuedId.Value, clientConnections);
        }

        private static StringBuilder GenerateCombinationQuery(
            FilterCollection filterCollection,
            string selectedFilterOperation,
            string suppressedFilterOperation,
            string selectedFilterNo,
            string suppressedFilterNo,
            string additionalFilters,
            ClientConnections clientConnections,
            int? issuedId,
            Func<Object.FilterMVC, string, ClientConnections, int?, string> filterQuery)
        {
            var query = new StringBuilder();
            var result = new StringBuilder();

            query.Append("<xml><Queries>");

            var filters = new List<int>();

            if (filterCollection != null && filterCollection.Any())
            {
                foreach (var filterNo in selectedFilterNo.Split(CommaSeparator))
                {
                    AddFilterToQuery(filterCollection, additionalFilters, clientConnections, filterNo, filters, query, issuedId, filterQuery);
                }

                foreach (var filterNo in suppressedFilterNo.Split(CommaSeparator))
                {
                    AddFilterToQuery(filterCollection, additionalFilters, clientConnections, filterNo, filters, query, issuedId, filterQuery);
                }
            }

            result.Append(
                string.Format(
                    FilterResultFormatString,
                    1,
                    selectedFilterNo,
                    selectedFilterOperation,
                    suppressedFilterNo,
                    suppressedFilterOperation,
                    string.Empty));

            query
                .Append("</Queries>")
                .Append("<Results>")
                .Append(result)
                .Append("</Results>")
                .Append("</xml>");

            return query;
        }
        
        private static void AddFilterToQuery(
            FilterCollection filterCollection,
            string additionalFilters,
            ClientConnections clientConnections,
            string filterNo,
            ICollection<int> filters,
            StringBuilder query,
            int? issuedId,
            Func<Object.FilterMVC, string, ClientConnections, int?, string> filterQuery)
        {
            if (string.IsNullOrWhiteSpace(filterNo))
            {
                return;
            }

            var filter = filterCollection.SingleOrDefault(f => f.FilterNo.ToString() == filterNo);
            if (filter == null || filters.Contains(filter.FilterNo))
            {
                return;
            }

            var filterQueryResult = filterQuery(
                filter,
                additionalFilters,
                clientConnections,
                issuedId);

            filters.Add(filter.FilterNo);
            query.Append(string.Format(QueryFilterNoCdataFormatString, filter.FilterNo, filterQueryResult));
        }
    }
}
