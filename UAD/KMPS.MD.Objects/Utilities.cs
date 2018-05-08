using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using ECN_Framework_Entities.Accounts;
using KM.Common;
using KM.Common.Export;
using KM.Common.Extensions;
using KM.Common.Functions;
using KMPlatform.Object;
using Telerik.Web.UI;

namespace KMPS.MD.Objects
{
    public class Utilities
    {
        private const char CommaChar = ',';
        private const string ConnectionStringEcnCommunicator = "ecnCommunicator";
        private const string NameFirstName = "FirstName";
        private const string NameLastName = "LastName";
        private const string NameEmail = "Email";
        private const string NameCompany = "Company";
        private const string NameTitle = "Title";
        private const string NameAddress = "Address";
        private const string NameAddress2 = "Address2";
        private const string NameMailStop = "MailStop";
        private const string NameAddress3 = "Address3";
        private const string NameCity = "City";
        private const string NameState = "State";
        private const string NameZip = "Zip";
        private static readonly string UDFPrefix = "MAF_";
        private const string NamePromoCode = "PROMOCODE";
        private const string NameJob = "JOB";
        private const string NameCounty = "COUNTY";
        private const string NamePlus4 = "PLUS4";
        private const string NameForZip = "FORZIP";
        private const string NameSequenceId = "SEQUENCEID";
        private const string NameSubscriptionId = "subscriptionID";
        private const string NameSubscriberId = "SUBSCRIBERID";
        private const string NameHtml = "html";
        private const string NameS = "S";
        private const string ColumnEmail = "EMAIL";
        private const string ColumnAction = "Action";
        private const string ColumnCounts = "Counts";

        public static IList<T> CreateListFromBuilder<T>(SqlDataReader sqlDataReader)
        {
            var entities = new List<T>();
            var builder = DynamicBuilder<T>.CreateBuilder(sqlDataReader);
            while (sqlDataReader.Read())
            {
                entities.Add(builder.Build(sqlDataReader));
            }

            return entities;
        }

        public static string getListboxSelectedValues(ListBox lst)
        {
            string selectedvalues = string.Empty;
            foreach (ListItem item in lst.Items)
            {
                if (item.Selected)
                {
                    selectedvalues += selectedvalues == string.Empty ? item.Value : "," + item.Value;
                }
            }
            return selectedvalues;
        }

        public static string getListboxText(ListBox lst)
        {
            string text = string.Empty;

            string selectedvalues = string.Empty;
            foreach (ListItem item in lst.Items)
            {
                if (item.Selected)
                {
                    text = item.Text;

                    //if (text.IndexOf(".") > -1)
                    //    text = text.Substring(0, text.IndexOf("."));

                    text = text.Replace(item.Value + ".", "");

                    selectedvalues += selectedvalues == string.Empty ? text : "," + text;
                }
            }
            return selectedvalues;
        }

        public static List<dynamic> getListboxSelectedExportFields(ListBox listBox)
        {
            List<dynamic> selectedvalues = new List<dynamic>();

            foreach (ListItem item in listBox.Items)
            {
                selectedvalues.Add(new { text = item.Text, value = item.Value });
            }

            return selectedvalues;
        }

        public static Tuple<string,string> getRadComboBoxSelectedExportFields(RadComboBox comboBox)
        {
            List<dynamic> selectedvalues = new List<dynamic>();

            foreach (var item in comboBox.CheckedItems)
            {
                selectedvalues.Add(new { text = item.Text, value = item.Value });
            }

            var sbValue = new StringBuilder();
            var sbText = new StringBuilder();

            foreach (var item in comboBox.CheckedItems)
            {
                if (sbValue.ToString() == string.Empty)
                {
                    sbValue.Append(item.Value);
                    sbText.Append(item.Text);
                }
                else
                {
                    sbValue.Append("," + item.Value);
                    sbText.Append(", " + item.Text);
                }
            }

            return new Tuple<string, string>(sbValue.ToString(), sbText.ToString());
        }

        public static List<string> GetSelectedSubExtMapperExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items)
        {
            List<string> selectedValues = new List<string>();
            List<SubscriptionsExtensionMapper> SubExtensionMapperList = SubscriptionsExtensionMapper.GetActive(clientconnection);

            foreach (string s in items)
            {
                if (SubExtensionMapperList.Exists(x => x.StandardField == s.Split('|')[0]))
                {
                    selectedValues.Add(SubExtensionMapperList.Where(x => x.StandardField == s.Split('|')[0]).FirstOrDefault().CustomField + "|" + s.Split('|')[2]);
                }
            }

            return selectedValues;
        }

        public static List<string> GetSelectedPubSubExtMapperExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items, int PubID)
        {
            List<string> selectedValues = new List<string>();
            List<PubSubscriptionsExtensionMapper> PubSubExtensionMapperList = PubSubscriptionsExtensionMapper.GetActiveByPubID(clientconnection, PubID);

            foreach (string s in items)
            {
                if (PubSubExtensionMapperList.Exists(x => x.StandardField == s.Split('|')[0]))
                {
                    selectedValues.Add(PubSubExtensionMapperList.Where(x => x.StandardField == s.Split('|')[0]).FirstOrDefault().CustomField + "|" + s.Split('|')[2]);
                }
            }

            return selectedValues;
        }

        public static Tuple<List<string>, List<string>> GetSelectedMasterGroupExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items, int brandID)
        {
            List<string> selectedvalues = new List<string>();
            List<string> selectedDescvalues = new List<string>();
            List<MasterGroup> masterGroupList = new List<MasterGroup>();
            if (brandID > 0)
                masterGroupList = MasterGroup.GetActiveByBrandID(clientconnection, brandID);
            else
                masterGroupList = MasterGroup.GetActiveMasterGroupsSorted(clientconnection);

            foreach (string s in items)
            {
                if (masterGroupList.Exists(x => x.ColumnReference == s.Split('|')[0]))
                {
                    selectedvalues.Add(s.Split('|')[0] + "|" + s.Split('|')[2]);
                }
                else if(masterGroupList.Exists(x => x.ColumnReference + "_Description" == s.Split('|')[0]))
                {
                    selectedDescvalues.Add(s.Split('|')[0].Split(new string[] { "_Description" }, StringSplitOptions.None)[0] + "|" + s.Split('|')[2]);
                }
            }

            return Tuple.Create(selectedvalues,selectedDescvalues);
        }

        public static Tuple<List<string>, List<string>, List<string>> GetSelectedResponseGroupStandardExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items , int pubID, bool IsGroupExport, bool IsFilterScheduleExport = false)
        {
            List<string> selectedID = new List<string>();
            List<string> selectedDescID = new List<string>();
            List<string> StandardColumnsList = new List<string>();
            List<PubSubscriptionsExtensionMapper> PubSubscriptionsExtMapperValueList = PubSubscriptionsExtensionMapper.GetActiveByPubID(clientconnection, pubID);

            foreach (string s in items)
            {
                if (!PubSubscriptionsExtMapperValueList.Exists(x => x.StandardField == s.Split('|')[0]))
                {
                    List<ResponseGroup> responseGroupList = new List<ResponseGroup>();

                    if (IsFilterScheduleExport)
                        responseGroupList =  ResponseGroup.GetAcitiveByPubIDForFilterSchedule(clientconnection, pubID);
                    else
                        responseGroupList = ResponseGroup.GetActiveByPubID(clientconnection, pubID);

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

        public static List<string> GetSelectedStandardExportColumns(KMPlatform.Object.ClientConnections clientconnection, List<string> items, int brandID)
        {
            List<string> selectedvalues = new List<string>();
            List<MasterGroup> masterGroupList = new List<MasterGroup>();
            if (brandID > 0)
                masterGroupList = MasterGroup.GetActiveByBrandID(clientconnection, brandID);
            else
                masterGroupList = MasterGroup.GetActiveMasterGroupsSorted(clientconnection);

            List<SubscriptionsExtensionMapper> semList = SubscriptionsExtensionMapper.GetActive(clientconnection);

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

        public static IList<string> GetStandardExportColumnFieldName(IList<string> standardColumnsList, Enums.ViewType viewtype, int brandId, bool groupExport)
        {
            return Helper.GetStandardExportColumnFieldName(
                standardColumnsList,
                viewtype == Enums.ViewType.ProductView,
                brandId,
                groupExport);
        }

        public static Dictionary<string, string> GetExportFields(
            ClientConnections clientConnection,
            Enums.ViewType viewType,
            int brandId,
            IList<int> pubIds,
            Enums.ExportType exportType,
            int userId,
            Enums.ExportFieldType downloadFieldType = Enums.ExportFieldType.All,
            bool isFilterSchedule = false)
        {
            var exportFields = new Dictionary<string, string>();

            if (downloadFieldType == Enums.ExportFieldType.Profile || downloadFieldType == Enums.ExportFieldType.All)
            {
                LoadProfileExportFields(exportFields, clientConnection, viewType, pubIds, exportType, userId, isFilterSchedule);
                exportFields = exportFields.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }

            if (downloadFieldType == Enums.ExportFieldType.Demo || downloadFieldType == Enums.ExportFieldType.All)
            {
                LoadDemoExportFields(exportFields, clientConnection, viewType, pubIds, exportType, brandId);
            }

            if (downloadFieldType == Enums.ExportFieldType.Adhoc || downloadFieldType == Enums.ExportFieldType.All)
            {
                LoadAdhocExportFields(exportFields, clientConnection, viewType, pubIds);
            }

            return exportFields;
        }

        private static void LoadProfileExportFields(
            IDictionary<string, string> exportFields,
            ClientConnections clientConnection,
            Enums.ViewType viewType,
            IList<int> pubIds,
            Enums.ExportType exportType,
            int userId,
            bool isFilterSchedule = false)
        {
            Guard.NotNull(exportFields, nameof(exportFields));
            if (isFilterSchedule && (exportType == Enums.ExportType.ECN || exportType == Enums.ExportType.FTP))
            {
                exportFields.Add("SubscriptionID|other", "SubscriptionID");
            }

            if (viewType == Enums.ViewType.ProductView)
            {
                LoadProductFieldsByUserDataMasks(exportFields, clientConnection, exportType, userId);

                exportFields.Add("Plus4|other", "Plus4");
                exportFields.Add("Country|varchar", "Country");
                exportFields.Add("Phone|other", "Phone");
                exportFields.Add("Mobile|other", "Mobile");
                exportFields.Add("Fax|other", "Fax");
                exportFields.Add("County|varchar", "County");
                exportFields.Add("DateCreated|other", "DateCreated");
                exportFields.Add("DateUpdated|other", "DateUpdated");
                exportFields.Add("Gender|varchar", "Gender");
                exportFields.Add("IsActive|other", "IsActive");
                exportFields.Add("PubTransactionDate|other", "TransactionDate");
                exportFields.Add("QualificationDate|other", "QDate");
                exportFields.Add("EmailStatus|varchar", "EmailStatus");
                exportFields.Add("StatusUpdatedDate|other", "StatusUpdatedDate");
                exportFields.Add("Demo7|other", "Demo7");
                exportFields.Add("SequenceID|other", "SequenceID");
                exportFields.Add("ExternalKeyID|other", "ExternalKeyID");
                exportFields.Add("AccountNumber|other", "AccountNumber");
                exportFields.Add("EmailID|other", "EmailID");
                exportFields.Add("SubscriberSourceCode|other", "SubscriberSourceCode");
            }
            else
            {
                LoadNonProductFieldsByUserDataMasks(exportFields, clientConnection, exportType, userId);

                exportFields.Add("Plus4|other", "Plus4");
                exportFields.Add("Country|varchar", "Country");
                exportFields.Add("ForZip|other", "ForZip");
                exportFields.Add("Phone|other", "Phone");
                exportFields.Add("Mobile|other", "Mobile");
                exportFields.Add("Fax|other", "Fax");
                exportFields.Add("County|varchar", "County");
                exportFields.Add("DateCreated|other", "DateCreated");
                exportFields.Add("DateUpdated|other", "DateUpdated");
                exportFields.Add("Gender|varchar", "Gender");
                exportFields.Add("Home_Work_Address|varchar", "Home_Work_Address");
                exportFields.Add("IsLatLonValid|other", "GeoLocated");
                exportFields.Add("Regcode|other", "RegCode");
                exportFields.Add("TransactionDate|other", "TransactionDate");
                exportFields.Add("QDate|other", "QDate");
            }

            exportFields.Add("CategoryID|other", "CategoryID");
            exportFields.Add("TransactionStatus|varchar", "TransactionStatus");
            exportFields.Add("TransactionID|other", "TransactionID");

            if (viewType == Enums.ViewType.ProductView)
            {
                var isCirc = Pubs.GetActive(clientConnection)
                    .Find(x => x.PubID == pubIds.First())
                    .IsCirc;

                if (isCirc)
                {
                    exportFields.Add("QSourceID|other", "QSourceID");
                    exportFields.Add("Par3C|varchar", "Par3C");
                }
            }
            else
            {
                exportFields.Add("QSourceID|other", "QSourceID");
                exportFields.Add("Par3C|varchar", "Par3C");
            }

            exportFields.Add("MailPermission|other", "MailPermission");
            exportFields.Add("FaxPermission|other", "FaxPermission");
            exportFields.Add("PhonePermission|other", "PhonePermission");
            exportFields.Add("OtherProductsPermission|other", "OtherProductsPermission");
            exportFields.Add("ThirdPartyPermission|other", "ThirdPartyPermission");
            exportFields.Add("EmailRenewPermission|other", "EmailRenewPermission");
            exportFields.Add("TextPermission|other", "TextPermission");
            exportFields.Add("Score|other", "Score");
            exportFields.Add("TransactionName|varchar", "TransactionName");
            exportFields.Add("QSourceName|varchar", "QSourceName");
            exportFields.Add("IGRP_NO|other", "IGRP_NO");
            exportFields.Add("CGRP_NO|other", "CGRP_NO");
            exportFields.Add("LastOpenedDate|other", "LastOpenedDate");
            exportFields.Add("LastOpenedPubCode|varchar", "LastOpenedPubCode");
        }

        private static void LoadProductFieldsByUserDataMasks(
            IDictionary<string, string> exportFields,
            ClientConnections clientConnection,
            Enums.ExportType exportType,
            int userId)
        {
            Guard.NotNull(exportFields, nameof(exportFields));
            if (exportType == Enums.ExportType.ECN || exportType == Enums.ExportType.Marketo)
            {
                var userDataMasks = UserDataMask.GetByUserID(clientConnection, userId);

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameFirstName)))
                {
                    exportFields.Add("FirstName|varchar", NameFirstName);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameLastName)))
                {
                    exportFields.Add("LastName|varchar", NameLastName);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameEmail)))
                {
                    exportFields.Add("Email|varchar", NameEmail);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameCompany)))
                {
                    exportFields.Add("Company|varchar", NameCompany);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameTitle)))
                {
                    exportFields.Add("Title|varchar", NameTitle);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameAddress)))
                {
                    exportFields.Add("Address1|varchar", NameAddress);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameAddress2)))
                {
                    exportFields.Add("Address2|varchar", NameAddress2);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameAddress3)))
                {
                    exportFields.Add("Address3|varchar", NameAddress3);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameCity)))
                {
                    exportFields.Add("City|varchar", NameCity);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameState)))
                {
                    exportFields.Add("RegionCode|varchar", NameState);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameZip)))
                {
                    exportFields.Add("ZipCode|other", NameZip);
                }
            }
            else
            {
                exportFields.Add("Email|varchar", NameEmail);
                exportFields.Add("FirstName|varchar", NameFirstName);
                exportFields.Add("LastName|varchar", NameLastName);
                exportFields.Add("Company|varchar", NameCompany);
                exportFields.Add("Title|varchar", NameTitle);
                exportFields.Add("Address1|varchar", NameAddress);
                exportFields.Add("Address2|varchar", NameAddress2);
                exportFields.Add("Address3|varchar", NameAddress3);
                exportFields.Add("City|varchar", NameCity);
                exportFields.Add("RegionCode|varchar", NameState);
                exportFields.Add("ZipCode|other", NameZip);
            }
        }

        private static void LoadNonProductFieldsByUserDataMasks(
            IDictionary<string, string> exportFields,
            ClientConnections clientConnection,
            Enums.ExportType exportType,
            int userId)
        {
            Guard.NotNull(exportFields, nameof(exportFields));
            if (exportType == Enums.ExportType.ECN || exportType == Enums.ExportType.Marketo)
            {
                var userDataMasks = UserDataMask.GetByUserID(clientConnection, userId);

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameFirstName)))
                {
                    exportFields.Add("FNAME|varchar", NameFirstName);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameLastName)))
                {
                    exportFields.Add("LNAME|varchar", NameLastName);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameEmail)))
                {
                    exportFields.Add("Email|varchar", NameEmail);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameCompany)))
                {
                    exportFields.Add("Company|varchar", NameCompany);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameTitle)))
                {
                    exportFields.Add("Title|varchar", NameTitle);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameAddress)))
                {
                    exportFields.Add("Address|varchar", NameAddress);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameAddress2)))
                {
                    exportFields.Add("MailStop|varchar", NameMailStop);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameAddress3)))
                {
                    exportFields.Add("Address3|varchar", NameAddress3);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameCity)))
                {
                    exportFields.Add("City|varchar", NameCity);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameState)))
                {
                    exportFields.Add("State|varchar", NameState);
                }

                if (!userDataMasks.Exists(u => u.MaskField.EqualsIgnoreCase(NameZip)))
                {
                    exportFields.Add("Zip|other", NameZip);
                }
            }
            else
            {
                exportFields.Add("Email|varchar", NameEmail);
                exportFields.Add("FNAME|varchar", NameFirstName);
                exportFields.Add("LNAME|varchar", NameLastName);
                exportFields.Add("Company|varchar", NameCompany);
                exportFields.Add("Title|varchar", NameTitle);
                exportFields.Add("Address|varchar", NameAddress);
                exportFields.Add("MailStop|varchar", NameMailStop);
                exportFields.Add("Address3|varchar", NameAddress3);
                exportFields.Add("City|varchar", NameCity);
                exportFields.Add("State|varchar", NameState);
                exportFields.Add("Zip|other", NameZip);
            }
        }

        private static void LoadDemoExportFields(
            IDictionary<string, string> exportFields,
            ClientConnections clientConnection,
            Enums.ViewType viewType,
            IList<int> pubIds,
            Enums.ExportType exportType,
            int brandId)
        {
            Guard.NotNull(exportFields, nameof(exportFields));

            if (viewType == Enums.ViewType.ProductView)
            {
                var responseGroups = ResponseGroup.GetActiveByPubID(clientConnection, pubIds.First());

                foreach (var responseGroup in responseGroups)
                {
                    var groupName = responseGroup.DisplayName;
                    var columnName = responseGroup.ResponseGroupID.ToString();
                    exportFields.Add($"{columnName}|other", groupName);

                    if (exportType != Enums.ExportType.ECN)
                    {
                        exportFields.Add($"{columnName}_Description|varchar", $"{groupName}_Description");
                    }
                }
            }
            else
            {
                var masterGroups = brandId > 0
                    ? MasterGroup.GetActiveByBrandID(clientConnection, brandId)
                    : MasterGroup.GetActiveMasterGroupsSorted(clientConnection);

                foreach (var masterGroup in masterGroups)
                {
                    exportFields.Add($"{masterGroup.ColumnReference}|other", masterGroup.DisplayName);
                    if (exportType != Enums.ExportType.ECN)
                    {
                        exportFields.Add($"{masterGroup.ColumnReference}_Description|varchar", $"{masterGroup.DisplayName}_Description");
                    }
                }
            }
        }

        private static void LoadAdhocExportFields(
            IDictionary<string, string> exportFields,
            ClientConnections clientConnection,
            Enums.ViewType viewType,
            IList<int> pubIds)
        {
            Guard.NotNull(exportFields, nameof(exportFields));
            if (viewType == Enums.ViewType.ProductView)
            {
                var extensionMappers = PubSubscriptionsExtensionMapper.GetActiveByPubID(clientConnection, pubIds.First());

                foreach (var mapper in extensionMappers)
                {
                    exportFields.Add(mapper.CustomFieldDataType.EqualsIgnoreCase(Enums.FieldType.Varchar.ToString())
                            ? $"{mapper.StandardField}|varchar"
                            : $"{mapper.StandardField}|other",
                        mapper.CustomField);
                }
            }
            else
            {
                var extensionMappers = SubscriptionsExtensionMapper.GetActive(clientConnection);

                foreach (var mapper in extensionMappers)
                {
                    exportFields.Add(mapper.CustomFieldDataType.EqualsIgnoreCase(Enums.FieldType.Varchar.ToString())
                            ? $"{mapper.StandardField}|varchar"
                            : $"{mapper.StandardField}|other",
                        mapper.CustomField);
                }
            }
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

        public static void SelectFilterListBoxes(ListBox lbCurrent, string values)
        {

            if (!string.IsNullOrWhiteSpace(values))
            {
                string[] items = values.Split(',');
                foreach (ListItem item in lbCurrent.Items)
                {
                    if(!item.Selected)
                        item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                }

                for (int i = 0; i < lbCurrent.Items.Count; i++)
                {
                    if (lbCurrent.Items[i].Selected)
                    {
                        if (i > 0 && !lbCurrent.Items[i - 1].Selected)
                        {
                            ListItem top = lbCurrent.Items[i];
                            lbCurrent.Items.Remove(top);
                            lbCurrent.Items.Insert(0, top);
                            lbCurrent.Items[0].Selected = true;
                        }
                    }
                }
            }
        }

        public static void SelectFilterRadComboBox(RadComboBox comboBox, string values)
        {
            Guard.NotNull(comboBox, nameof(comboBox));
            if (!string.IsNullOrWhiteSpace(values))
            {
                var items = values.Split(CommaChar);
                foreach (RadComboBoxItem item in comboBox.Items)
                {
                    item.Selected = items.FirstOrDefault(c => c.Equals(item.Value)) != null;
                    item.Checked = item.Selected;
                }
            }
        }

        public static void SelectFilterDropDowns(DropDownList ddlCurrent, string values)
        {
            if (!string.IsNullOrWhiteSpace(values))
            {
                ddlCurrent.SelectedValue = values;
            }
        }

        public static void SelectFilterTextBox(TextBox txtCurrent, string values)
        {
            if (!string.IsNullOrWhiteSpace(values))
            {
                txtCurrent.Text = values;
            }
        }

        public static List<int> getNth(int TotalRecords, int RequestedRecords)
        {
            List<int> listNth = new List<int>();

            if (RequestedRecords == 0)
                RequestedRecords = TotalRecords;

            double inccounter = (double)TotalRecords / RequestedRecords;

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
            Hashtable SubscriptionID_list = new Hashtable(30000, (float)0.6);

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
                if (!(SubscriptionID_list.ContainsKey(dr["SubscriptionID"].ToString())))
                {
                    SubscriptionID_list.Add(dr["SubscriptionID"].ToString(), 1);

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

        public static void DownloadDataCompare(int downloadCount, DataTable dt, string outFileName)
        {
            List<int> LNth = Utilities.getNth(dt.Rows.Count, downloadCount);
            Hashtable SubscriptionID_list = new Hashtable(30000, (float)0.6);

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

        public static Hashtable ExportToECN(
            int groupId,
            string groupName,
            int customerId,
            int folderId,
            string promoCode,
            string jobCode,
            List<ExportFields> exportFields,
            DataTable dtSubscribers,
            int userId,
            Enums.GroupExportSource source)
        {
            const int hashTableCapacity = 30000;
            const float loadFactor = (float)0.6;
            var subscriptionIdList = new Hashtable(hashTableCapacity, loadFactor);
            var updatedRecords = new Hashtable();
            var userWorker = new KMPlatform.BusinessLogic.User();
            var user = userWorker.SelectUser(userId, true);
            var customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerId, false);
            var baseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(customer.BaseChannelID.Value);
            var promoCodeUdfId = GetUdfId(promoCode, groupId, NamePromoCode);
            var jobUdfId = GetUdfId(jobCode, groupId, NameJob);

            user = userWorker.SetAuthorizedUserObjects(user, baseChannel.PlatformClientGroupID, customer.PlatformClientID);
            UpdateFieldUdfId(exportFields, groupId);

            var xmlProfile = new StringBuilder();
            var xmlUdf = new StringBuilder();
            var counter = 0;
            const int batchSize = 1000;

            foreach (DataRow dataRow in dtSubscribers.Rows)
            {
                var subIdText = dataRow[NameSubscriptionId].ToString();
                if (subscriptionIdList.ContainsKey(subIdText))
                {
                    counter++;
                    continue;
                }

                subscriptionIdList.Add(subIdText, 1);
                AppendProfileXml(xmlProfile, dataRow);

                if (promoCodeUdfId > 0 || jobUdfId > 0 || exportFields.Any())
                {
                    AppendUdfXml(xmlUdf, dataRow, promoCodeUdfId, promoCode, jobUdfId, jobCode, exportFields);
                }

                if (counter != 0 && counter % batchSize == 0 || counter == dtSubscribers.Rows.Count - 1)
                {
                    var dtImportedRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(
                        user,
                        customerId, groupId,
                        $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlProfile}</XML>",
                        $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlUdf}</XML>",
                        NameHtml,
                        NameS,
                        false,
                        string.Empty,
                        source.ToString());

                    if (dtImportedRecords.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtImportedRecords.Rows)
                        {
                            var actionText = row[ColumnAction].ToString();
                            if (!updatedRecords.Contains(actionText))
                            {
                                updatedRecords.Add(actionText.ToUpper(), Convert.ToInt32(row[ColumnCounts]));
                            }
                            else
                            {
                                actionText = row[ColumnAction].ToString().ToUpper();
                                var updateTotal = Convert.ToInt32(updatedRecords[actionText]);
                                updatedRecords[actionText] = updateTotal + Convert.ToInt32(row[ColumnCounts]);
                            }
                        }
                    }

                    xmlProfile = new StringBuilder();
                    xmlUdf = new StringBuilder();
                }

                counter++;
            }

            return updatedRecords;
        }

        private static void AppendUdfXml(
            StringBuilder xmlUdf,
            DataRow dataRow,
            int promoCodeUdfId,
            string promoCode,
            int jobUdfId,
            string jobCode,
            IEnumerable<ExportFields> exportFields)
        {
            Guard.NotNull(xmlUdf, nameof(xmlUdf));
            Guard.NotNull(dataRow, nameof(dataRow));
            xmlUdf.Append("<row>");

            var emailText = dataRow[ColumnEmail].ToString();
            if (!string.IsNullOrWhiteSpace(emailText))
            {
                xmlUdf.Append($"<ea>{cleanXMLString(emailText)}</ea>");
            }

            if (promoCodeUdfId > 0 && !string.IsNullOrWhiteSpace(promoCode))
            {
                xmlUdf.Append($"<udf id=\"{promoCodeUdfId}\"><v>{cleanXMLString(promoCode)}</v></udf>");
            }

            if (jobUdfId > 0 && !string.IsNullOrWhiteSpace(jobCode))
            {
                xmlUdf.Append($"<udf id=\"{jobUdfId}\"><v>{cleanXMLString(jobCode)}</v></udf>");
            }

            foreach (var field in exportFields)
            {
                if (field.isECNUDF)
                {
                    var fieldText = dataRow[field.FieldName].ToString();
                    if (!string.IsNullOrWhiteSpace(fieldText))
                    {
                        xmlUdf.Append($"<udf id=\"{field.GroupdatafieldsID}\"><v>{cleanXMLString(fieldText)}</v></udf>");
                    }
                }
            }

            xmlUdf.Append("</row>");
        }

        private static void AppendProfileXml(StringBuilder xmlProfile, DataRow dataRow)
        {
            Guard.NotNull(xmlProfile, nameof(xmlProfile));
            xmlProfile.Append("<Emails>");

            AppendXml(xmlProfile, dataRow, "EMAIL", "emailaddress");
            AppendXml(xmlProfile, dataRow, "TITLE", "title");
            AppendXml(xmlProfile, dataRow, "FNAME", "firstname");
            AppendXml(xmlProfile, dataRow, "FIRSTNAME", "firstname");
            AppendXml(xmlProfile, dataRow, "LNAME", "lastname");
            AppendXml(xmlProfile, dataRow, "LASTNAME", "lastname");
            AppendXml(xmlProfile, dataRow, "COMPANY", "company");
            AppendXml(xmlProfile, dataRow, "ADDRESS", "address");
            AppendXml(xmlProfile, dataRow, "ADDRESS1", "address");
            AppendXml(xmlProfile, dataRow, "CITY", "city");
            AppendXml(xmlProfile, dataRow, "STATE", "state");
            AppendXml(xmlProfile, dataRow, "REGIONCODE", "state");
            AppendXml(xmlProfile, dataRow, "ZIP", "zip");
            AppendXml(xmlProfile, dataRow, "ZIPCODE", "zip");
            AppendXml(xmlProfile, dataRow, "COUNTRY", "country");
            AppendXml(xmlProfile, dataRow, "PHONE", "voice");
            AppendXml(xmlProfile, dataRow, "FAX", "fax");
            AppendXml(xmlProfile, dataRow, "MOBILE", "mobile");

            xmlProfile.Append("</Emails>");
        }

        private static void AppendXml(StringBuilder xmlProfile, DataRow dataRow, string columnName, string tagName)
        {
            Guard.NotNull(xmlProfile, nameof(xmlProfile));
            Guard.NotNull(dataRow, nameof(dataRow));

            if (dataRow.Table.Columns.Contains(columnName))
            {
                var rowText = dataRow[columnName].ToString();
                if (!string.IsNullOrWhiteSpace(rowText))
                {
                    xmlProfile.Append($"<{tagName}>{cleanXMLString(rowText)}</{tagName}>");
                }
            }
        }

        private static int GetUdfId(string code, int groupId, string name)
        {
            var udfId = 0;

            if (code != string.Empty)
            {
                udfId = UdfExists(groupId, $"{UDFPrefix}{name}");

                if (udfId == 0)
                {
                    udfId = InsertUdf(groupId, $"{UDFPrefix}{name}");
                }
            }

            return udfId;
        }

        private static void UpdateFieldUdfId(List<ExportFields> exportFields, int groupId)
        {
            Guard.NotNull(exportFields, nameof(exportFields));
            foreach (var exportField in exportFields)
            {
                if (exportField.isECNUDF)
                {
                    string name;

                    if (exportField.FieldName.EqualsAnyIgnoreCase(NameMailStop, NameCounty, NamePlus4, NameForZip, NameSequenceId, NameSubscriptionId))
                    {
                        name = exportField.FieldName.EqualsIgnoreCase(NameSequenceId)
                            ? NameSubscriberId
                            : exportField.FieldName;
                    }
                    else
                    {
                        name = UDFPrefix + exportField.FieldName;
                    }

                    var udfId = UdfExists(groupId, name);

                    if (udfId == 0)
                    {
                        udfId = InsertUdf(groupId, name);
                    }

                    exportField.GroupdatafieldsID = udfId;
                }
            }
        }

        public static string GetHeaderText(Filters fc, string SelectedFilterIDs, string SuppressedFilterIDs, string OperationIn, string OperationNotIn, bool IsFilterSegmentation)
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
                    List<Field> lfield = fc.SingleOrDefault(f => f.FilterNo.ToString() == s).Fields;

                    if (i > 0)
                        headerText += "\r\n";

                    foreach (Field f in lfield)
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
                        List<Field> lfield = fc.SingleOrDefault(f => f.FilterNo.ToString() == s).Fields;

                        if (j > 0)
                            headerText += "\r\n";

                        foreach (Field f in lfield)
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
            return ConfigurationManager.ConnectionStrings[ConnectionStringEcnCommunicator].ConnectionString;
        }
    }
}