using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using KM.Common;
using KM.Common.Export;
using KM.Common.Functions;
using BusinessEnums = FrameworkUAD.BusinessLogic.Enums;
using DataFunctions = FrameworkUAD.DataAccess.DataFunctions;
using ObjectClientConnection = KMPlatform.Object.ClientConnections;
using UserDataMask = FrameworkUAD.BusinessLogic.UserDataMask;

namespace UAS.Web.Helpers
{
    public static class IEnumerableExtention
    {
        public static string DataTableToCSV(this DataTable datatable, char seperator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < datatable.Columns.Count; i++)
            {
                sb.Append(datatable.Columns[i]);
                if (i < datatable.Columns.Count - 1)
                    sb.Append(seperator);
            }
            sb.AppendLine();
            foreach (DataRow dr in datatable.Rows)
            {
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    sb.Append(dr[i].ToString());

                    if (i < datatable.Columns.Count - 1)
                        sb.Append(seperator);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        public static IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> ToKendoFilterCategoryTree(this IEnumerable<FrameworkUAD.Entity.FilterCategory> fcList)
        {
            Queue<Kendo.Mvc.UI.TreeViewItemModel> queue = new Queue<Kendo.Mvc.UI.TreeViewItemModel>();
            var root = new Kendo.Mvc.UI.TreeViewItemModel();
            root.Id = "0";
            root.Text = "Root";
            root.Expanded = false;
            queue.Enqueue(root);
            // Assume tree structure -- then we don't need to worry about depleting folderList. 
            // If graph is disconnected, folders will obviously be missed.
            // It will terminate because there are no cycles.
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                var children = (from fc in fcList
                                where cur.Id == fc.ParentID.ToString()
                                select fc).ToList();
                foreach (var fc in children)
                {
                    var treeItem = fc.ToKendoFilterCategoryTreeItem();
                    queue.Enqueue(treeItem);
                    cur.Items.Add(treeItem);
                }
            }
            return new List<Kendo.Mvc.UI.TreeViewItemModel> { root };
        }
        public static Kendo.Mvc.UI.TreeViewItemModel ToKendoFilterCategoryTreeItem(this FrameworkUAD.Entity.FilterCategory fc)
        {
            var treeItem = new Kendo.Mvc.UI.TreeViewItemModel();
            treeItem.Text = fc.CategoryName;
            treeItem.Id = fc.FilterCategoryID.ToString();
            treeItem.Expanded = false;
            return treeItem;
        }
        public static IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> ToKendoQuestionCategoryTree(this IEnumerable<FrameworkUAD.Entity.QuestionCategory> fcList)
        {
            Queue<Kendo.Mvc.UI.TreeViewItemModel> queue = new Queue<Kendo.Mvc.UI.TreeViewItemModel>();
            var root = new Kendo.Mvc.UI.TreeViewItemModel();
            root.Id = "0";
            root.Text = "Root";
            root.Expanded = false;
            queue.Enqueue(root);
            // Assume tree structure -- then we don't need to worry about depleting folderList. 
            // If graph is disconnected, folders will obviously be missed.
            // It will terminate because there are no cycles.
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                var children = (from fc in fcList
                                where cur.Id == fc.ParentID.ToString()
                                select fc).ToList();
                foreach (var fc in children)
                {
                    var treeItem = fc.ToKendoQuestionCategoryTreeItem();
                    queue.Enqueue(treeItem);
                    cur.Items.Add(treeItem);
                }
            }
            return new List<Kendo.Mvc.UI.TreeViewItemModel> { root };
        }
        public static Kendo.Mvc.UI.TreeViewItemModel ToKendoQuestionCategoryTreeItem(this FrameworkUAD.Entity.QuestionCategory fc)
        {
            var treeItem = new Kendo.Mvc.UI.TreeViewItemModel();
            treeItem.Text = fc.CategoryName;
            treeItem.Id = fc.QuestionCategoryID.ToString();
            treeItem.Expanded = false;
            return treeItem;
        }
    }

    public static class Utilities
    {
        private const string IdProperty = "ID";
        private const string IdRegex = "Id$";
        private const string AddressProperty = "Address1";
        private const string AddressRegex = "Address$";
        private const string DescriptionKey = "_Description";
        private const string IssueSplitKey = "IssueSplit";
        private const string ImbseqKey = "Imbseq";
        private const string MailStopKey = "MailStop";
        private const string VarCharDataType = "varchar";
        private const string OtherDataType = "other";
        private const string RegexJoinSymbol = "|";
        private const string ConnectionStringCommunicator = "communicator";
        private static readonly string UDFPrefix = "MAF_";

        public static Dictionary<string, string> GetExportingFields(
            ObjectClientConnection clientconnection,
            int pubId,
            BusinessEnums.ExportType exportType,
            int userId,
            BusinessEnums.ExportFieldType exportFieldType = BusinessEnums.ExportFieldType.All,
            string downLoadType = "Report",
            bool isArchived = false)
        {
            var lstExport = new Dictionary<string, string>();
            if (exportFieldType == BusinessEnums.ExportFieldType.Profile || exportFieldType == BusinessEnums.ExportFieldType.All)
            {
                if (exportType == BusinessEnums.ExportType.ECN)
                {
                    var replacements = GetDefaultReplacements();
                    ExportProfileFieldsWithClientConnection(clientconnection, lstExport, replacements, userId, true);
                }
                else
                {
                    ExportDefaultProfileFields(lstExport);
                }

                ExportOtherProfilesFiles(lstExport);
                ExportProfilePermissionFiles(lstExport);
                ExportPubTransactionFiles(lstExport, isArchived);
                lstExport = lstExport.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }

            if (exportFieldType == BusinessEnums.ExportFieldType.Demo || exportFieldType == BusinessEnums.ExportFieldType.All)
            {
                ExtractDemoFields(clientconnection, lstExport, exportType, pubId);
            }

            if (exportFieldType == BusinessEnums.ExportFieldType.Adhoc || exportFieldType == BusinessEnums.ExportFieldType.All)
            {
                ExportAdHocFields(clientconnection, lstExport, pubId);
            }

            if (IssueSplitKey.Equals(downLoadType, StringComparison.InvariantCultureIgnoreCase))
            {
                ExportIssueSplitFields(lstExport);
            }

            return lstExport;
        }

        public static void ExportProfileFieldsWithClientConnection(
            ObjectClientConnection clientConnection,
            IDictionary<string, string> lstExport,
            IDictionary<string, string> replacements,
            int userId,
            bool isDefaultReplacement)
        {
            Guard.NotNull(clientConnection, nameof(clientConnection));
            Guard.NotNull(lstExport, nameof(lstExport));
            Guard.NotNull(replacements, nameof(replacements));

            var regex = new Regex(string.Join(RegexJoinSymbol, replacements.Keys));
            var userDataMask = UserDataMask.GetByUserID(clientConnection, userId);

            foreach (var prop in typeof(ProfileFields).GetProperties())
            {
                var userDataMaskItem = userDataMask.FirstOrDefault(
                    userData => string.Equals(userData.MaskField, prop.Name, StringComparison.InvariantCultureIgnoreCase));

                if (userDataMaskItem != null)
                {
                    var propertyName = regex.Replace(prop.Name, element => replacements[element.Value]);
                    var maskField = propertyName.Equals(MailStopKey) ? MailStopKey : userDataMaskItem.MaskField;

                    if (isDefaultReplacement)
                    {
                        propertyName = Regex.Replace(propertyName, AddressRegex, AddressProperty);
                    }

                    var dbDataType = prop.PropertyType == typeof(string) ? VarCharDataType : OtherDataType;
                    lstExport.Add($"{propertyName}|{dbDataType}", maskField);
                }
            }
        }

        public static void ExportDefaultProfileFields(IDictionary<string, string> lstExport)
        {
            Guard.NotNull(lstExport, nameof(lstExport));

            var replacements = GetDefaultReplacements();
            var regex = new Regex(string.Join(RegexJoinSymbol, replacements.Keys));

            foreach (var prop in typeof(ProfileFields).GetProperties())
            {
                var propertyName = regex.Replace(prop.Name, element => replacements[element.Value]);
                propertyName = Regex.Replace(propertyName, AddressRegex, AddressProperty);
                var dbDataType = prop.PropertyType == typeof(string) ? VarCharDataType : OtherDataType;

                lstExport.Add($"{propertyName}|{dbDataType}", prop.Name);
            }
        }
        
        public static void ExportOtherProfilesFiles(IDictionary<string, string> lstExport)
        {
            Guard.NotNull(lstExport, nameof(lstExport));

            var replacements = GetDefaultReplacements();
            var regex = new Regex(string.Join(RegexJoinSymbol, replacements.Keys));

            foreach (var prop in typeof(SecondaryProfileFields).GetProperties())
            {
                var propertyName = Regex.Replace(prop.Name, IdRegex, IdProperty);
                propertyName = Regex.Replace(propertyName, AddressRegex, AddressProperty);
                propertyName = regex.Replace(propertyName, element => replacements[element.Value]);

                var propertyValue = Regex.Replace(prop.Name, IdRegex, IdProperty);
                propertyValue = propertyValue.Equals(ImbseqKey) ? ImbseqKey.ToUpper() : propertyValue;
                var dbDataType = prop.PropertyType == typeof(string) ? VarCharDataType : OtherDataType;

                lstExport.Add($"{propertyName}|{dbDataType}", propertyValue);
            }
        }

        public static void ExportProfilePermissionFiles(IDictionary<string, string> lstExport)
        {
            Guard.NotNull(lstExport, nameof(lstExport));
            lstExport.Add("MailPermission|other", "MailPermission");
            lstExport.Add("FaxPermission|other", "FaxPermission");
            lstExport.Add("PhonePermission|other", "PhonePermission");
            lstExport.Add("OtherProductsPermission|other", "OtherProductsPermission");
            lstExport.Add("ThirdPartyPermission|other", "ThirdPartyPermission");
            lstExport.Add("EmailRenewPermission|other", "EmailRenewPermission");
            lstExport.Add("TextPermission|other", "TextPermission");
        }
        
        public static void ExportPubTransactionFiles(Dictionary<string, string> lstExport, bool isArchived)
        {
            Guard.NotNull(lstExport, nameof(lstExport));

            if (!isArchived)
            {
                lstExport.Add("PubTransactionDate|other", "TransactionDate");
                lstExport.Add("EmailID|other", "EmailID");
                lstExport.Add("IGRP_NO|other", "IGRP_NO");
                lstExport.Add("CGRP_NO|other", "CGRP_NO");
                lstExport.Add("Score|other", "Score");
                lstExport.Add("LastOpenedDate|other", "LastOpenedDate");
                lstExport.Add("LastOpenedPubCode|varchar", "LastOpenedPubCode");
            }
        }

        public static void ExtractDemoFields(
            ObjectClientConnection clientConnection,
            IDictionary<string, string> lstExport,
            BusinessEnums.ExportType exportType,
            int pubId)
        {
            Guard.NotNull(clientConnection, nameof(clientConnection));
            Guard.NotNull(lstExport, nameof(lstExport));

            var rgQuery = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(pubId, clientConnection);

            foreach (var response in rgQuery)
            {
                var responseGroup = response.DisplayName;
                var columnName = response.ResponseGroupID.ToString();
                lstExport.Add(columnName + RegexJoinSymbol + OtherDataType, responseGroup);

                if (exportType != BusinessEnums.ExportType.ECN)
                {
                    lstExport.Add(
                        columnName + DescriptionKey + RegexJoinSymbol + VarCharDataType, responseGroup + DescriptionKey);
                }
            }
        }

        public static void ExportAdHocFields(
            ObjectClientConnection clientConnection, 
            IDictionary<string, string> lstExport, 
            int pubId)
        {
            Guard.NotNull(clientConnection, nameof(clientConnection));
            Guard.NotNull(lstExport, nameof(lstExport));

            var extensionMappers = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper()
                .SelectAll(clientConnection)
                .Where(x => x.PubID == pubId)
                .ToList();

            foreach (var extensionMapper in extensionMappers)
            {
                if (string.Equals(extensionMapper.CustomFieldDataType, BusinessEnums.FieldType.Varchar.ToString(),
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    lstExport.Add(extensionMapper.StandardField + RegexJoinSymbol + VarCharDataType, extensionMapper.CustomField);
                }
                else
                {
                    lstExport.Add(extensionMapper.StandardField + RegexJoinSymbol + OtherDataType, extensionMapper.CustomField);
                }
            }
        }

        public static void ExportIssueSplitFields(IDictionary<string, string> lstExport)
        {
            Guard.NotNull(lstExport, nameof(lstExport));
            lstExport.Add("ACSCode|varchar", "ACSCode");
            lstExport.Add("ExpireIssueDAte|varchar", "ExpireIssueDAte");
            lstExport.Add("ExpQdate|varchar", "ExpQdate");
            lstExport.Add("KeyCode|varchar", "KeyCode");
            lstExport.Add("Keyline|varchar", "Keyline");
            lstExport.Add("MailerID|varchar", "MailerID");
            lstExport.Add("SplitDescription|varchar", "SplitDescription");
            lstExport.Add("SplitName|varchar", "SplitName");
        }

        public static Dictionary<string, string> GetExportFields(KMPlatform.Object.ClientConnections clientconnection, FrameworkUAD.BusinessLogic.Enums.ViewType ViewType, int BrandID, List<int> PubIDs, FrameworkUAD.BusinessLogic.Enums.ExportType exportType, int userID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType downloadFieldType = FrameworkUAD.BusinessLogic.Enums.ExportFieldType.All, bool IsFilterSchedule = false)
        {
            Dictionary<string, string> lstExport = new Dictionary<string, string>();

            #region Profile Fields
            if (downloadFieldType == FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Profile || downloadFieldType == FrameworkUAD.BusinessLogic.Enums.ExportFieldType.All)
            {
                if (IsFilterSchedule && (exportType == FrameworkUAD.BusinessLogic.Enums.ExportType.ECN || exportType == FrameworkUAD.BusinessLogic.Enums.ExportType.FTP))
                {
                    lstExport.Add("SubscriptionID|other", "SubscriptionID");
                }
                #region Profile Field for ProductView
                if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                {
                    if (exportType == BusinessEnums.ExportType.ECN || exportType == BusinessEnums.ExportType.Marketo)
                    {
                        var replacements = GetDefaultReplacements();
                        ExportProfileFieldsWithClientConnection(clientconnection, lstExport, replacements, userID, true);
                    }
                    else
                    {
                        ExportDefaultProfileFields(lstExport);
                    }

                    #region Other PRofile Fields
                    lstExport.Add("Batch|other", "Batch");
                    lstExport.Add("Verify|varchar", "Verify");
                    lstExport.Add("IMBSEQ|varchar", "IMBSEQ");
                    lstExport.Add("ReqFlag|varchar", "ReqFlag");
                    lstExport.Add("Website|varchar", "Website");
                    lstExport.Add("Copies|other", "Copies");
                    lstExport.Add("WaveMailingID|other", "WaveMailingID");
                    lstExport.Add("OrigsSrc|varchar", "OrigsSrc");
                    lstExport.Add("Plus4|other", "Plus4");
                    lstExport.Add("Country|varchar", "Country");
                    lstExport.Add("Phone|other", "Phone");
                    lstExport.Add("Mobile|other", "Mobile");
                    lstExport.Add("Fax|other", "Fax");
                    lstExport.Add("County|varchar", "County");
                    lstExport.Add("DateCreated|other", "DateCreated");
                    lstExport.Add("DateUpdated|other", "DateUpdated");
                    lstExport.Add("Gender|varchar", "Gender");
                    lstExport.Add("IsActive|other", "IsActive");
                    //lstExport.Add("PubTransactionDate|other", "TransactionDate");
                    lstExport.Add("QualificationDate|other", "QDate");
                    lstExport.Add("EmailStatus|varchar", "EmailStatus");
                    lstExport.Add("StatusUpdatedDate|other", "StatusUpdatedDate");
                    lstExport.Add("Demo7|other", "Demo7");
                    lstExport.Add("SequenceID|other", "SequenceID");
                    lstExport.Add("ExternalKeyID|other", "ExternalKeyID");
                    lstExport.Add("AccountNumber|other", "AccountNumber");
                    //lstExport.Add("EmailID|other", "EmailID");
                    lstExport.Add("SubscriberSourceCode|other", "SubscriberSourceCode");
                    #endregion
                }


                #endregion

                #region Profile Fields for Other Views
                else
                {
                    #region Marketo or ECN Profile Fields
                    if (exportType == BusinessEnums.ExportType.ECN || exportType == BusinessEnums.ExportType.Marketo)
                    {
                        var replacements = GetDownloadTypeReplacements();
                        ExportProfileFieldsWithClientConnection(clientconnection, lstExport, replacements, userID, false);
                    }
                    #endregion
                    #region Downloads Profile Fields
                    else
                    {
                        lstExport.Add("Email|varchar", "Email");
                        lstExport.Add("FNAME|varchar", "FirstName");
                        lstExport.Add("LNAME|varchar", "LastName");
                        lstExport.Add("Company|varchar", "Company");
                        lstExport.Add("Title|varchar", "Title");
                        lstExport.Add("Address|varchar", "Address");
                        lstExport.Add("MailStop|varchar", "MailStop");
                        lstExport.Add("Address3|varchar", "Address3");
                        lstExport.Add("City|varchar", "City");
                        lstExport.Add("State|varchar", "State");
                        lstExport.Add("Zip|other", "Zip");
                    }
                    #endregion

                    #region Other Profile Fields

                    lstExport.Add("Plus4|other", "Plus4");
                    lstExport.Add("Country|varchar", "Country");
                    lstExport.Add("ForZip|other", "ForZip");
                    lstExport.Add("Phone|other", "Phone");
                    lstExport.Add("Mobile|other", "Mobile");
                    lstExport.Add("Fax|other", "Fax");
                    lstExport.Add("County|varchar", "County");
                    lstExport.Add("DateCreated|other", "DateCreated");
                    lstExport.Add("DateUpdated|other", "DateUpdated");
                    lstExport.Add("Gender|varchar", "Gender");
                    lstExport.Add("Home_Work_Address|varchar", "Home_Work_Address");
                    lstExport.Add("IsActive|other", "IsActive");
                    lstExport.Add("IsLatLonValid|other", "GeoLocated");
                    lstExport.Add("Regcode|other", "RegCode");
                    lstExport.Add("TransactionDate|other", "TransactionDate");
                    lstExport.Add("QDate|other", "QDate");
                    #endregion
                }
                #endregion

                #region Profile Fields For All Views
                lstExport.Add("CategoryID|other", "CategoryID");
                lstExport.Add("TransactionStatus|varchar", "TransactionStatus");
                lstExport.Add("TransactionID|other", "TransactionID");

                if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                {
                    var pubs = new FrameworkUAD.BusinessLogic.Product().Select(clientconnection, false).Where(x => x.PubID == PubIDs.First()).FirstOrDefault();
                    bool IsCirc = pubs.IsCirc.HasValue? (bool)pubs.IsCirc:false;
                    if (IsCirc)
                    {
                        lstExport.Add("QSourceID|other", "QSourceID");
                        lstExport.Add("Par3C|varchar", "Par3C");
                    }
                }
                else
                {
                    lstExport.Add("QSourceID|other", "QSourceID");
                    lstExport.Add("Par3C|varchar", "Par3C");
                    lstExport.Add("Score|other", "Score");
                    lstExport.Add("IGRP_NO|other", "IGRP_NO");
                    lstExport.Add("CGRP_NO|other", "CGRP_NO");
                    lstExport.Add("LastOpenedDate|other", "LastOpenedDate");
                    lstExport.Add("LastOpenedPubCode|varchar", "LastOpenedPubCode");
                }

                #region Permission Fields
                lstExport.Add("MailPermission|other", "MailPermission");
                lstExport.Add("FaxPermission|other", "FaxPermission");
                lstExport.Add("PhonePermission|other", "PhonePermission");
                lstExport.Add("OtherProductsPermission|other", "OtherProductsPermission");
                lstExport.Add("ThirdPartyPermission|other", "ThirdPartyPermission");
                lstExport.Add("EmailRenewPermission|other", "EmailRenewPermission");
                lstExport.Add("TextPermission|other", "TextPermission");
                #endregion

               
                lstExport.Add("TransactionName|varchar", "TransactionName");
                lstExport.Add("QSourceName|varchar", "QSourceName");
            
                #endregion

                lstExport = lstExport.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            #endregion

            #region Demo Fields
            if (downloadFieldType == FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo || downloadFieldType == FrameworkUAD.BusinessLogic.Enums.ExportFieldType.All)
            {
                if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                {
                    string responseGroup = string.Empty;
                    string columnName = string.Empty;

                    var rgQuery = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(PubIDs.First(), clientconnection);
                      

                    foreach (FrameworkUAD.Entity.ResponseGroup r in rgQuery)
                    {
                        responseGroup = r.DisplayName;
                        columnName = r.ResponseGroupID.ToString();
                        lstExport.Add(columnName + "|other", responseGroup);
                        if (exportType != FrameworkUAD.BusinessLogic.Enums.ExportType.ECN)
                            lstExport.Add(columnName + "_Description|varchar", responseGroup + "_Description");
                    }
                }
                else
                {
                    List<FrameworkUAD.Entity.MasterGroup> masterGroupList = new List<FrameworkUAD.Entity.MasterGroup>();
                    if (BrandID > 0)
                        masterGroupList = new FrameworkUAD.BusinessLogic.MasterGroup().SelectByBrandID(BrandID,clientconnection);
                    else
                        masterGroupList = new FrameworkUAD.BusinessLogic.MasterGroup().Select(clientconnection);

                    foreach (FrameworkUAD.Entity.MasterGroup m in masterGroupList)
                    {
                        lstExport.Add(m.ColumnReference + "|other", m.DisplayName);
                        if (exportType != FrameworkUAD.BusinessLogic.Enums.ExportType.ECN)
                            lstExport.Add(m.ColumnReference + "_Description|varchar", m.DisplayName + "_Description");
                    }
                }
            }
            #endregion

            #region AdHoc Fields
            if (downloadFieldType == FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc || downloadFieldType == FrameworkUAD.BusinessLogic.Enums.ExportFieldType.All)
            {
                if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                {
                    List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> extensionMappers =new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper()
                        .SelectAll(clientconnection).Where(x=>x.PubID== PubIDs.First()).ToList();

                    foreach (FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper s in extensionMappers)
                    {
                        if (s.CustomFieldDataType.ToUpper() == FrameworkUAD.BusinessLogic.Enums.FieldType.Varchar.ToString().ToUpper())
                            lstExport.Add(s.StandardField + "|varchar", s.CustomField);
                        else
                            lstExport.Add(s.StandardField + "|other", s.CustomField);
                    }
                }
                else
                {
                    List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> extensionMappers = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().SelectAll(clientconnection);

                    foreach (FrameworkUAD.Entity.SubscriptionsExtensionMapper s in extensionMappers)
                    {
                        if (s.CustomFieldDataType.ToUpper() == FrameworkUAD.BusinessLogic.Enums.FieldType.Varchar.ToString().ToUpper())
                            lstExport.Add(s.StandardField + "|varchar", s.CustomField);
                        else
                            lstExport.Add(s.StandardField + "|other", s.CustomField);
                    }
                }
            }
            #endregion

            return lstExport;
        }

        private static IDictionary<string, string> GetDefaultReplacements()
        {
            return new Dictionary<string, string>
            {
                ["Imbseq"] = "IMBSEQ",
                ["QDate"] = "QualificationDate",
                ["State"] = "RegionCode",
                ["Zip"] = "ZipCode"
            };
        }

        private static IDictionary<string, string> GetDownloadTypeReplacements()
        {
            return new Dictionary<string, string>
            {
                ["FirstName"] = "FNAME",
                ["LastName"] = "LNAME",
                ["Address2"] = "MailStop"
            };
        }

        public static string GetHeaderText(FrameworkUAD.Object.FilterCollection fc, string SelectedFilterIDs, string SuppressedFilterIDs, string OperationIn, string OperationNotIn, bool IsFilterSegmentation)
        {
            string headerText = string.Empty;

            if (SelectedFilterIDs != string.Empty)
            {
                headerText = "Operations = " + OperationIn + "\n";
                string[] sIDs = SelectedFilterIDs.Split(',');
                int i = 0;
                headerText += "Filters In";

                foreach (string s in sIDs)
                {
                    List<FrameworkUAD.Object.FilterDetails> lfield = fc.SingleOrDefault(f => f.FilterNo.ToString() == s).Fields;

                    if (i > 0)
                        headerText += "\r\n";

                    foreach (FrameworkUAD.Object.FilterDetails f in lfield)
                    {
                        headerText += "\r\n";
                        headerText += f.Name + " = " + f.Text;
                        headerText += f.Name == "Adhoc" || f.Name == "Open Activity" || f.Name == "Click Activity" || f.Name == "Open Email Sent Date" || f.Name == "Click Email Sent Date" || f.Name == "Visit Activity" ? f.Name == "Open Activity" || f.Name == "Click Activity" || f.Name == "Open Email Sent Date" || f.Name == "Click Email Sent Date" || f.Name == "Visit Activity" ? f.SearchCondition + " - " + f.Values : " - " + f.SearchCondition + " - " + f.Values : "";
                    }
                    i++;
                }

                if (SuppressedFilterIDs != string.Empty)
                {
                    headerText += "\r\n";

                    if (IsFilterSegmentation)
                    {
                        headerText += "\r\n" + "Operations = " + OperationNotIn;
                    }
                    string[] spIDs = SuppressedFilterIDs.Split(',');
                    headerText += "\r\n" + "Filters NotIn";
                    int j = 0;

                    foreach (string s in spIDs)
                    {
                        List<FrameworkUAD.Object.FilterDetails> lfield = fc.SingleOrDefault(f => f.FilterNo.ToString() == s).Fields;

                        if (j > 0)
                            headerText += "\r\n";

                        foreach (FrameworkUAD.Object.FilterDetails f in lfield)
                        {
                            headerText += "\r\n";
                            headerText += f.Name + " = " + f.Text;
                            headerText += f.Name == "Adhoc" || f.Name == "Open Activity" || f.Name == "Click Activity" || f.Name == "Open Email Sent Date" || f.Name == "Click Email Sent Date" || f.Name == "Visit Activity" ? f.Name == "Open Activity" || f.Name == "Click Activity" || f.Name == "Open Email Sent Date" || f.Name == "Click Email Sent Date" || f.Name == "Visit Activity" ? f.SearchCondition + " - " + f.Values : " - " + f.SearchCondition + " - " + f.Values : "";
                        }
                        j++;
                    }
                }
            }

            return headerText;
        }

        public static List<string> GetSelectedSubExtMapperExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items)
        {
            List<string> selectedValues = new List<string>();
            List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> SubExtensionMapperList =new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().SelectAll(clientconnection).Where(x=> x.Active==true).ToList();

            foreach (string s in items)
            {
                if (SubExtensionMapperList.Exists(x => x.StandardField == s.Split('|')[0]))
                {
                    selectedValues.Add(SubExtensionMapperList.Where(x => x.StandardField == s.Split('|')[0]).FirstOrDefault().CustomField + "|" + s.Split('|')[2]);
                }
            }

            return selectedValues;
        }
        public static Tuple<List<string>, List<string>> GetSelectedMasterGroupExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items, int brandID)
        {
            List<string> selectedvalues = new List<string>();
            List<string> selectedDescvalues = new List<string>();
            List<FrameworkUAD.Entity.MasterGroup> masterGroupList = new List<FrameworkUAD.Entity.MasterGroup>();
            if (brandID > 0)
                masterGroupList = new FrameworkUAD.BusinessLogic.MasterGroup().SelectByBrandID(brandID,clientconnection);
            else
                masterGroupList = new FrameworkUAD.BusinessLogic.MasterGroup().Select(clientconnection);

            foreach (string s in items)
            {
                if (masterGroupList.Exists(x => x.ColumnReference == s.Split('|')[0]))
                {
                    selectedvalues.Add(s.Split('|')[0] + "|" + s.Split('|')[2]);
                }
                else if (masterGroupList.Exists(x => x.ColumnReference + "_Description" == s.Split('|')[0]))
                {
                    selectedDescvalues.Add(s.Split('|')[0].Split(new string[] { "_Description" }, StringSplitOptions.None)[0] + "|" + s.Split('|')[2]);
                }
            }

            return Tuple.Create(selectedvalues, selectedDescvalues);
        }


        public static List<string> GetSelectedStandardExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items, int brandID)
        {
            List<string> selectedvalues = new List<string>();
            List<FrameworkUAD.Entity.MasterGroup> masterGroupList = new List<FrameworkUAD.Entity.MasterGroup>();
            if (brandID > 0)
                masterGroupList = new FrameworkUAD.BusinessLogic.MasterGroup().SelectByBrandID(brandID, clientconnection);
            else
                masterGroupList = new FrameworkUAD.BusinessLogic.MasterGroup().Select(clientconnection);

            List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> semList =new  FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().SelectAll(clientconnection).Where(x=>x.Active==true).ToList();

            foreach (string s in items)
            {
                if (!masterGroupList.Exists(x => x.ColumnReference == s.Split('|')[0]) && !masterGroupList.Exists(x => x.ColumnReference + "_Description" == s.Split('|')[0])
                    && !semList.Exists(x => x.StandardField == s.Split('|')[0]) && s.Split('|')[0].ToUpper() != "LASTOPENEDDATE" && s.Split('|')[0].ToUpper() != "LASTOPENEDPUBCODE")
                {
                    selectedvalues.Add(s.Split('|')[0] + "|" + s.Split('|')[2]);
                }
            }

            //string columns = string.Join(",", selectedvalues);

            return selectedvalues;
        }

        public static List<string> GetSelectedCustomExportColumns(List<string> items)
        {
            List<string> customColumnsList = new List<string>();
            string columns = string.Empty;

            foreach (string s in items)
            {
                if (s.Split('|')[0].ToUpper() == "LASTOPENEDDATE" || s.Split('|')[0].ToUpper() == "LASTOPENEDPUBCODE")
                {
                    customColumnsList.Add(s.Split('|')[0] + "|" + s.Split('|')[2]);
                }
            }

            return customColumnsList;
        }
      
       

      
      

        public static List<string> GetSelectedPubSubExtMapperExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items, int PubID)
        {
            List<string> selectedValues = new List<string>();
            List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> PubSubExtensionMapperList = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper().SelectAll(clientconnection).Where(x=>x.PubID==PubID).ToList();

            foreach (string s in items)
            {
                if (PubSubExtensionMapperList.Exists(x => x.StandardField == s.Split('|')[0]))
                {
                    selectedValues.Add(PubSubExtensionMapperList.Where(x => x.StandardField == s.Split('|')[0]).FirstOrDefault().CustomField + "|" + s.Split('|')[2]);
                }
            }

            return selectedValues;
        }

      

        public static Tuple<List<string>, List<string>, List<string>> GetSelectedResponseGroupStandardExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items, int pubID, bool IsGroupExport, bool IsFilterScheduleExport = false)
        {
            List<string> selectedID = new List<string>();
            List<string> selectedDescID = new List<string>();
            List<string> StandardColumnsList = new List<string>();
            List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> PubSubscriptionsExtMapperValueList = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper().SelectAll(clientconnection).Where(x => x.PubID == pubID).ToList();

            foreach (string s in items)
            {
                if (!PubSubscriptionsExtMapperValueList.Exists(x => x.StandardField == s.Split('|')[0]))
                {
                    List<FrameworkUAD.Entity.ResponseGroup> responseGroupList = new List<FrameworkUAD.Entity.ResponseGroup>();

                    if (IsFilterScheduleExport)
                        responseGroupList = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(pubID,clientconnection);
                    else
                        responseGroupList = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(pubID, clientconnection);

                    if (responseGroupList.Exists(x => x.ResponseGroupID.ToString() == s.Split('|')[0]))
                    {
                        selectedID.Add(s.Split('|')[0] + "|" + s.Split('|')[2]);
                    }
                    else if (responseGroupList.Exists(x => x.ResponseGroupID.ToString() + "_Description" == s.Split('|')[0]))
                    {
                        selectedDescID.Add(s.Split('|')[0].Split(new string[] { "_Description" }, StringSplitOptions.None)[0] + "|" + s.Split('|')[2]);
                    }
                    else
                    {
                        if (s.Split('|')[0].ToUpper() != "LASTOPENEDDATE" && s.Split('|')[0].ToUpper() != "LASTOPENEDPUBCODE")
                        {
                            if (s.Split('|')[0].ToUpper() == "ES.STATUS AS EMAILSTATUS")
                            {
                                if (!IsGroupExport)
                                    StandardColumnsList.Add(s.Split('|')[0] + "|" + s.Split('|')[2]);
                            }
                            else
                                StandardColumnsList.Add(s.Split('|')[0] + "|" + s.Split('|')[2]);
                        }
                    }
                }
            }

            return Tuple.Create(selectedID, selectedDescID, StandardColumnsList);
        }

        public static DataTable getImportedResult(Hashtable hUpdatedRecords, DateTime startDateTime)
        {
            DataTable dtResults = new DataTable();

            dtResults.Columns.Add("Type");
            dtResults.Columns.Add("Action");
            dtResults.Columns.Add("Totals");
            dtResults.Columns.Add("sortOrder");

            DataRow row;

            foreach (DictionaryEntry de in hUpdatedRecords)
            {
                row = dtResults.NewRow();

                if (de.Key.ToString() == "T")
                {
                    row["Action"] = "Total Records in the File";
                    row["sortOrder"] = 1;
                }
                else if (de.Key.ToString() == "I")
                {
                    row["Action"] = "New";
                    row["sortOrder"] = 2;
                }
                else if (de.Key.ToString() == "U")
                {
                    row["Action"] = "Changed";
                    row["sortOrder"] = 3;
                }
                else if (de.Key.ToString() == "D")
                {
                    row["Action"] = "Duplicate(s)";
                    row["sortOrder"] = 4;
                }
                else if (de.Key.ToString() == "S")
                {
                    row["Action"] = "Skipped";
                    row["sortOrder"] = 5;
                }
                else if (de.Key.ToString() == "M")
                {
                    row["Action"] = "Skipped(MasterSuppressed)";
                    row["sortOrder"] = 6;
                }
                else if (de.Key.ToString() == "CT")
                {
                    row["Action"] = "Total Records in the Campaign";
                    row["sortOrder"] = 7;
                }
                row["Totals"] = de.Value;
                dtResults.Rows.Add(row);
            }

            row = dtResults.NewRow();
            row["Action"] = "&nbsp;";
            row["Totals"] = " ";
            row["sortOrder"] = 8;
            dtResults.Rows.Add(row);

            TimeSpan duration = DateTime.Now - startDateTime;

            row = dtResults.NewRow();
            row["Action"] = "Time to Import";
            row["Totals"] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
            row["sortOrder"] = 9;
            dtResults.Rows.Add(row);

            return dtResults;
        }

        public static IList<string> GetStandardExportColumnFieldName(
            List<string> columnList,
            BusinessEnums.ViewType viewtype,
            int brandId,
            bool isGroupExport,
            bool issueSplit = false)
        {
            return Helper.GetStandardExportColumnFieldName(
                columnList,
                viewtype == BusinessEnums.ViewType.ProductView,
                brandId,
                isGroupExport,
                issueSplit);
        }

        public static string cleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }

        public static string ReplaceSingleQuotes(string text)
        {
            text = text.Replace("'", "''");
            return text;
        }

       
       

       

        public static List<int> getNth(int TotalRecords, int RequestedRecords)
        {
            List<int> listNth = new List<int>();

            if (RequestedRecords == 0)
                RequestedRecords = TotalRecords;

            double inccounter = (double) TotalRecords / RequestedRecords;

            double y = inccounter;

            for (; Math.Round(y, 2) <= TotalRecords; y = (y + inccounter))
            {
                listNth.Add(Convert.ToInt32(y - 1));

                if (listNth.Count == RequestedRecords)
                {
                    break;
                }
            }
            return listNth;

        }

        public static void Download(DataTable dt, string outFileName, string HeaderText, int TotalCount, int DownloadCount)
        {
            Hashtable SubscriptionID_list = new Hashtable(30000, (float) 0.6);

            #region Download Logic

            TextWriter txtfile = File.AppendText(outFileName);

            ArrayList columnHeadings = new ArrayList();
            string newline = "";

            if (!string.IsNullOrEmpty(HeaderText))
            {
                txtfile.WriteLine("*****************************************************************************");
                txtfile.WriteLine("Total Count =" + TotalCount);
                txtfile.WriteLine("Downloaded Count =" + DownloadCount);
                txtfile.WriteLine(HeaderText);
                txtfile.WriteLine("*****************************************************************************");
            }

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                columnHeadings.Add(dt.Columns[i].ColumnName.ToString());
            }

            IEnumerator aListEnum = null;
            aListEnum = columnHeadings.GetEnumerator();
            newline = "";
            while (aListEnum.MoveNext())
            {
                newline += aListEnum.Current.ToString() + "\t";
            }
            txtfile.WriteLine(newline);

            foreach (DataRow dr in dt.Rows)
            {
                //if (!(SubscriptionID_list.ContainsKey(dr["SubscriptionID"].ToString())))
                //{
                //    SubscriptionID_list.Add(dr["SubscriptionID"].ToString(), 1);

                    newline = string.Empty;
                    aListEnum.Reset();

                    bool isFirst = true;

                    while (aListEnum.MoveNext())
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                            newline += dr[aListEnum.Current.ToString()].ToString();
                        }
                        else
                        {
                            newline += "\t" + dr[aListEnum.Current.ToString()].ToString();
                        }
                    }
                    txtfile.WriteLine(newline);
                //}
            }
            dt.Dispose();
            txtfile.Close();
            txtfile.Dispose();
            GC.Collect();
            #endregion
        }

        public static void DownloadDataCompare(int downloadCount, DataTable dt, string outFileName)
        {
            List<int> LNth = Utilities.getNth(dt.Rows.Count, downloadCount);
            Hashtable SubscriptionID_list = new Hashtable(30000, (float) 0.6);

            #region Download Logic

            TextWriter txtfile = File.AppendText(outFileName);

            ArrayList columnHeadings = new ArrayList();
            string newline = "";

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                columnHeadings.Add(dt.Columns[i].ColumnName.ToString());
            }

            IEnumerator aListEnum = null;
            aListEnum = columnHeadings.GetEnumerator();
            while (aListEnum.MoveNext())
            {
                newline += aListEnum.Current.ToString() + "\t";
            }
            txtfile.WriteLine(newline);

            foreach (int n in LNth)
            {
                DataRow dr = dt.Rows[n];

                if (!(SubscriptionID_list.ContainsKey(dr["SubscriberFinalId"].ToString())))
                {
                    SubscriptionID_list.Add(dr["SubscriberFinalId"].ToString(), 1);

                    newline = "";
                    aListEnum.Reset();

                    while (aListEnum.MoveNext())
                    {
                        newline += dr[aListEnum.Current.ToString()].ToString() + "\t"; ;
                    }
                    txtfile.WriteLine(newline);
                }
            }

            dt.Dispose();

            txtfile.Close();
            txtfile.Dispose();

            GC.Collect();

            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=filter_report.tsv");
            HttpContext.Current.Response.TransmitFile(outFileName);

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

            #endregion
        }

        public static int InsertGroup(string groupName, int customerId, int folderId)
        {
            return GroupFunctions.InsertGroup(
                groupName, 
                customerId,
                folderId, 
                GetCommunicatorConnectionString());
        }

        public static int UdfExists(int groupId, string name)
        {
            return GroupFunctions.UdfExists(
                groupId,
                name,
                GetCommunicatorConnectionString());
        }

        public static int InsertUdf(int groupId, string name)
        {
            return GroupFunctions.InsertUdf(
                groupId,
                name,
                GetCommunicatorConnectionString());
        }

        public static Hashtable ExportToECN(int GroupID, string GroupName, int CustomerID, int FolderID, string PromoCode, string JobCode, List<FrameworkUAD.Object.ExportFields> exportFields, DataTable dtSubscribers, int UserID, FrameworkUAD.BusinessLogic.Enums.GroupExportSource source)
        {
            Hashtable subscriptionIdList = new Hashtable(30000, (float) 0.6);
            Hashtable hUpdatedRecords = new Hashtable();
            JobCode = string.IsNullOrEmpty(JobCode) ? " " : JobCode;
            PromoCode = string.IsNullOrEmpty(PromoCode) ? " " : PromoCode;
            KMPlatform.BusinessLogic.User user = new KMPlatform.BusinessLogic.User();
            KMPlatform.Entity.User u = user.SelectUser(UserID, true);

            ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false);
            ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(c.BaseChannelID.Value);

            u = (new KMPlatform.BusinessLogic.User()).SetAuthorizedUserObjects(u, bc.PlatformClientGroupID, c.PlatformClientID);

            #region UDF Promocode
            int PromocodeUDFID = 0;

            if (PromoCode != "")
            {
                PromocodeUDFID = Utilities.UdfExists(GroupID, UDFPrefix + "PROMOCODE");

                if (PromocodeUDFID == 0)
                {
                    PromocodeUDFID = Utilities.InsertUdf(GroupID, UDFPrefix + "PROMOCODE");
                }
            }
            #endregion

            #region UDF Job
            int JobUDFID = 0;

            if (JobCode != "")
            {
                JobUDFID = Utilities.UdfExists(GroupID, UDFPrefix + "JOB");

                if (JobUDFID == 0)
                {
                    JobUDFID = Utilities.InsertUdf(GroupID, UDFPrefix + "JOB");
                }
            }
            #endregion

            try
            {
                foreach (FrameworkUAD.Object.ExportFields exp in exportFields) // Loop through List with foreach
                {
                    if (exp.isECNUDF)
                    {
                        int UDFID = 0;

                        string name = "";

                        if (exp.FieldName.ToUpper() == "MAILSTOP" || exp.FieldName.ToUpper() == "COUNTY" || exp.FieldName.ToUpper() == "PLUS4" || exp.FieldName.ToUpper() == "FORZIP" || exp.FieldName.ToUpper() == "SEQUENCEID" || exp.FieldName.ToUpper() == "SUBSCRIPTIONID")
                        {
                            if (exp.FieldName.ToUpper() == "SEQUENCEID")
                                name = "SUBSCRIBERID";
                            else
                                name = exp.FieldName;
                        }
                        else
                            name = UDFPrefix + exp.FieldName;

                        UDFID = Utilities.UdfExists(GroupID, name);

                        if (UDFID == 0)
                        {
                            UDFID = Utilities.InsertUdf(GroupID, name);
                        }

                        exp.GroupdatafieldsID = UDFID;
                    }
                }

                StringBuilder xmlProfile = new StringBuilder("");
                StringBuilder xmlUDF = new StringBuilder("");
                int cnt = 0;

                foreach (DataRow dr in dtSubscribers.Rows)
                {
                    if (subscriptionIdList.ContainsKey(dr["subscriptionID"].ToString()))
                    {
                        cnt++;
                        continue;
                    }

                    subscriptionIdList.Add(dr["subscriptionID"].ToString(), 1);

                    xmlProfile.Append("<Emails>");

                    if (dr["EMAIL"].ToString().Trim().Length > 0)
                    {
                        xmlProfile.Append("<emailaddress>" + Utilities.cleanXMLString(dr["EMAIL"].ToString()) + "</emailaddress>");
                    }
                    if (dr.Table.Columns.Contains("TITLE"))
                    {
                        if (dr["TITLE"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<title>" + Utilities.cleanXMLString(dr["TITLE"].ToString()) + "</title>");
                    }
                    if (dr.Table.Columns.Contains("FNAME"))
                    {
                        if (dr["FNAME"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<firstname>" + Utilities.cleanXMLString(dr["FNAME"].ToString()) + "</firstname>");
                    }
                    if (dr.Table.Columns.Contains("FIRSTNAME"))
                    {
                        if (dr["FIRSTNAME"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<firstname>" + Utilities.cleanXMLString(dr["FIRSTNAME"].ToString()) + "</firstname>");
                    }
                    if (dr.Table.Columns.Contains("LNAME"))
                    {
                        if (dr["LNAME"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<lastname>" + Utilities.cleanXMLString(dr["LNAME"].ToString()) + "</lastname>");
                    }
                    if (dr.Table.Columns.Contains("LASTNAME"))
                    {
                        if (dr["LASTNAME"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<lastname>" + Utilities.cleanXMLString(dr["LASTNAME"].ToString()) + "</lastname>");
                    }
                    if (dr.Table.Columns.Contains("COMPANY"))
                    {
                        if (dr["COMPANY"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<company>" + Utilities.cleanXMLString(dr["COMPANY"].ToString()) + "</company>");
                    }
                    if (dr.Table.Columns.Contains("ADDRESS"))
                    {
                        if (dr["ADDRESS"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<address>" + Utilities.cleanXMLString(dr["ADDRESS"].ToString()) + "</address>");
                    }
                    if (dr.Table.Columns.Contains("ADDRESS1"))
                    {
                        if (dr["ADDRESS1"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<address>" + Utilities.cleanXMLString(dr["ADDRESS1"].ToString()) + "</address>");
                    }
                    if (dr.Table.Columns.Contains("CITY"))
                    {
                        if (dr["CITY"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<city>" + Utilities.cleanXMLString(dr["CITY"].ToString()) + "</city>");
                    }
                    if (dr.Table.Columns.Contains("STATE"))
                    {
                        if (dr["STATE"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<state>" + Utilities.cleanXMLString(dr["STATE"].ToString()) + "</state>");
                    }
                    if (dr.Table.Columns.Contains("REGIONCODE"))
                    {
                        if (dr["REGIONCODE"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<state>" + Utilities.cleanXMLString(dr["REGIONCODE"].ToString()) + "</state>");
                    }
                    if (dr.Table.Columns.Contains("ZIP"))
                    {
                        if (dr["ZIP"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<zip>" + Utilities.cleanXMLString(dr["ZIP"].ToString()) + "</zip>");
                    }
                    if (dr.Table.Columns.Contains("ZIPCODE"))
                    {
                        if (dr["ZIPCODE"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<zip>" + Utilities.cleanXMLString(dr["ZIPCODE"].ToString()) + "</zip>");
                    }
                    if (dr.Table.Columns.Contains("COUNTRY"))
                    {
                        if (dr["COUNTRY"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<country>" + Utilities.cleanXMLString(dr["COUNTRY"].ToString()) + "</country>");
                    }
                    if (dr.Table.Columns.Contains("PHONE"))
                    {
                        if (dr["PHONE"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<voice>" + Utilities.cleanXMLString(dr["PHONE"].ToString()) + "</voice>");
                    }
                    if (dr.Table.Columns.Contains("FAX"))
                    {
                        if (dr["FAX"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<fax>" + Utilities.cleanXMLString(dr["FAX"].ToString()) + "</fax>");
                    }
                    if (dr.Table.Columns.Contains("MOBILE"))
                    {
                        if (dr["MOBILE"].ToString().Trim().Length > 0)
                            xmlProfile.Append("<mobile>" + Utilities.cleanXMLString(dr["MOBILE"].ToString()) + "</mobile>");
                    }

                    xmlProfile.Append("</Emails>");

                    if (PromocodeUDFID > 0 || JobUDFID > 0 || exportFields.Count > 0)
                    {
                        xmlUDF.Append("<row>");

                        if (dr["EMAIL"].ToString().Trim().Length > 0)
                            xmlUDF.Append("<ea>" + Utilities.cleanXMLString(dr["EMAIL"].ToString()) + "</ea>");

                        if (PromocodeUDFID > 0)
                        {
                            if (PromoCode.Trim().Length > 0)
                                xmlUDF.Append("<udf id=\"" + PromocodeUDFID + "\"><v>" + Utilities.cleanXMLString(PromoCode) + "</v></udf>");
                        }

                        if (JobUDFID > 0)
                        {
                            if (JobCode.Trim().Length > 0)
                                xmlUDF.Append("<udf id=\"" + JobUDFID + "\"><v>" + Utilities.cleanXMLString(JobCode) + "</v></udf>");
                        }

                        if (exportFields.Count > 0)
                        {
                            foreach (FrameworkUAD.Object.ExportFields expo in exportFields) // Loop through List with foreach
                            {
                                if (expo.isECNUDF)
                                {
                                    if (dr[expo.FieldName].ToString().Trim().Length > 0)
                                        xmlUDF.Append("<udf id=\"" + expo.GroupdatafieldsID + "\"><v>" + Utilities.cleanXMLString(dr[expo.FieldName].ToString()) + "</v></udf>");
                                }
                            }
                        }

                        xmlUDF.Append("</row>");
                    }

                    if ((cnt != 0) && (cnt % 1000 == 0) || (cnt == dtSubscribers.Rows.Count - 1))
                    {
                        DataTable dtImportedRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(u, CustomerID, GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", "html", "S", false, "", source.ToString());

                        if (dtImportedRecords.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtImportedRecords.Rows)
                            {
                                if (!hUpdatedRecords.Contains(row["Action"].ToString()))
                                    hUpdatedRecords.Add(row["Action"].ToString().ToUpper(), Convert.ToInt32(row["Counts"]));
                                else
                                {
                                    int eTotal = Convert.ToInt32(hUpdatedRecords[row["Action"].ToString().ToUpper()]);
                                    hUpdatedRecords[row["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(row["Counts"]);
                                }
                            }
                        }

                        xmlProfile = new StringBuilder("");
                        xmlUDF = new StringBuilder("");
                    }

                    cnt++;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return hUpdatedRecords;
        }
       
        public static void Log_Error(string page, string method, Exception err)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendCatchNotification"]))
            {
                StringBuilder sbEx = new StringBuilder();
                try
                {
                    sbEx.AppendLine("<BR><b>Page URL: </b>" + HttpContext.Current.Request.Url.AbsoluteUri + "</br>");
                    sbEx.AppendLine("<b>Method:</b>" + method + "</br>");

                    KM.Common.Entity.ApplicationLog.LogCriticalError(err, "UAD application error", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), sbEx.ToString());
                }
                catch (Exception)
                {
                }
            }
        }

        private static string GetCommunicatorConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[ConnectionStringCommunicator].ConnectionString;
        }
    }
}