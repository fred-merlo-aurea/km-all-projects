using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using KMPlatform.Object;

namespace KMPS.MD.Objects
{
    class FilterScheduleExport
    {
        private const string FilterSeparatorForQuery = ",";
        private const string SubscriptionExportColumnName = "SUBSCRIPTIONID";
        private const string AdHocFilterName = "Adhoc";
        private const string EndLineInHeader = "\r\n";
        private const string OpenActivityFilterName = "Open Activity";
        private const string ClickActivityForFilterName = "Click Activity";
        private const string OpenEmailSentDateFilterName = "Open Email Sent Date";
        private const string ClickEmailSentDateFilterName = "Click Email Sent Date";
        private const string VisitActivityFilterName = "Visit Activity";
        private const string SearchConditionDelimeter = " - ";
        private const string OperationsHeaderPrefix = "Operations = ";

        private readonly ClientConnections _clientconnection;

        public FilterScheduleExport(ClientConnections clientconnection)
        {
            _clientconnection = clientconnection;
        }

        public Tuple<DataTable, string, DataTable, bool> Export(int filterscheduleID)
        {
            var filterSchedule = FilterSchedule.GetByID(_clientconnection, filterscheduleID);
            var userID = filterSchedule.UpdatedBy > 0 
                ? filterSchedule.UpdatedBy 
                : filterSchedule.CreatedBy;

            var user = GetUser(userID);

            if (HasMailMask(userID) && 
                (filterSchedule.ExportTypeID == Enums.ExportType.ECN || filterSchedule.ExportTypeID == Enums.ExportType.Marketo))
            {
                return Tuple.Create(new DataTable(), string.Empty, new DataTable(), true);
            }

            var subscriptionDataTable = new DataTable();
            var filters = MDFilter.LoadFilters(_clientconnection, filterSchedule.FilterID, userID);
            var headerText = string.Empty;

            if (filters.Count > 0)
            {
                var selectedFilterNumbers = GetSeletcedFilterNumbers(filterSchedule, filters);
                var suppressedFilterNumbers = GetSupressedFilterNumbers(filterSchedule, filters);

                var queries = Filter.generateCombinationQuery(
                    filters, 
                    filterSchedule.SelectedOperation,
                    filterSchedule.SuppressedOperation,
                    string.Join(",", selectedFilterNumbers),
                    string.Join(FilterSeparatorForQuery, suppressedFilterNumbers),
                    string.Empty,
                    0,
                    0,
                    _clientconnection);

                var filter = filters.First();

                var selectedItem = GetSelectedItem(filterscheduleID, filterSchedule);

                subscriptionDataTable = filter.ViewType == Enums.ViewType.ProductView 
                    ? ExportProductView(filterSchedule, queries, filter, selectedItem) 
                    : ExportNonProductView(filterSchedule, filters, queries, filter, selectedItem);

                if (filterSchedule.ExportTypeID == Enums.ExportType.FTP)
                {
                    ExportFtp(filterscheduleID, user, filter, ref subscriptionDataTable);

                    if (filterSchedule.ShowHeader && selectedFilterNumbers.Any())
                    {
                        headerText = FormatHeaderText(filterSchedule, filters, selectedFilterNumbers, suppressedFilterNumbers);
                    }
                }
            }

            return Tuple.Create(subscriptionDataTable, headerText, new DataTable(), false);            
        }

        private DataTable ExportNonProductView(FilterSchedule filterSchedule, Filters filters, StringBuilder queries, Filter filter, List<string> selectedItem)
        {
            DataTable dtSubscription;
            IList<string> standardColumnsList = Utilities.GetSelectedStandardExportColumns(_clientconnection, selectedItem, filter.BrandID);
            var selectedMasterGroup = Utilities.GetSelectedMasterGroupExportColumns(_clientconnection, selectedItem, filter.BrandID);
            var masterGroupColumnList = selectedMasterGroup.Item1;
            var masterGroupColumnDescList = new List<string>();
            if (filterSchedule.ExportTypeID != Enums.ExportType.ECN)
            {
                masterGroupColumnDescList = selectedMasterGroup.Item2;
            }

            var subscriptionsExtMapperValueList = Utilities.GetSelectedSubExtMapperExportColumns(_clientconnection, selectedItem);

            standardColumnsList = Utilities.GetStandardExportColumnFieldName(
                                                    standardColumnsList,
                                                    filter.ViewType,
                                                    filter.BrandID,
                                                    filterSchedule.ExportTypeID == Enums.ExportType.ECN);

            var pubIDs = new List<int>();
            if (filters.Count == 1)
            {
                var field = filters[0].Fields.Find(x => x.Group.ToUpper() == "OPENCRITERIA" && x.SearchCondition.ToUpper() == "SEARCH SELECTED PRODUCTS");

                if (field != null)
                {
                    pubIDs = filters[0].Fields.Find(x => x.Name.ToUpper() == "PRODUCT").Values.Split(',').Select(int.Parse).ToList();
                }
            }

            dtSubscription = Subscriber.GetSubscriberData(
                _clientconnection,
                queries,
                standardColumnsList.ToList(),
                masterGroupColumnList,
                masterGroupColumnDescList,
                subscriptionsExtMapperValueList,
                Utilities.GetSelectedCustomExportColumns(selectedItem),
                filter.BrandID,
                pubIDs,
                filter.ViewType == Enums.ViewType.RecencyView ? true : false,
                0);
            return dtSubscription;
        }

        private DataTable ExportProductView(FilterSchedule filterSchedule, StringBuilder queries, Filter filter, List<string> selectedItem)
        {
            DataTable dtSubscription;

            var pubSubscriptionsExtMapperValueList = Utilities.GetSelectedPubSubExtMapperExportColumns(
                _clientconnection,
                selectedItem,
                filter.PubID);

            var responseGroup = Utilities.GetSelectedResponseGroupStandardExportColumns(
                _clientconnection,
                selectedItem,
                filter.PubID,
                filterSchedule.ExportTypeID == Enums.ExportType.ECN,
                true);

            var responseGroupIdList = responseGroup.Item1;

            var responseGroupDescIDList = new List<string>();
            if (filterSchedule.ExportTypeID != Enums.ExportType.ECN)
            {
                responseGroupDescIDList = responseGroup.Item2;
            }

            IList<string> standardColumnsList = responseGroup.Item3;
            standardColumnsList = Utilities.GetStandardExportColumnFieldName(
                                                standardColumnsList,
                                                filter.ViewType,
                                                filter.BrandID,
                                                filterSchedule.ExportTypeID == Enums.ExportType.ECN);

            var customColumnList = Utilities.GetSelectedCustomExportColumns(selectedItem);

            var pubIDs = filter.PubID.ToString().Split(',').Select(int.Parse).ToList();
            dtSubscription = Subscriber.GetProductDimensionSubscriberData(
                _clientconnection,
                queries,
                standardColumnsList.ToList(),
                pubIDs,
                responseGroupIdList,
                responseGroupDescIDList,
                pubSubscriptionsExtMapperValueList,
                customColumnList,
                filter.BrandID,
                0);

            return dtSubscription;
        }

        private bool HasMailMask(int userID)
        {
            var userDataMask = UserDataMask.GetByUserID(_clientconnection, userID);
            var hasEmailMask = userDataMask.Any(u => u.MaskField.Equals("EMAIL", StringComparison.OrdinalIgnoreCase));
            return hasEmailMask;
        }

        private List<string> GetSelectedItem(int filterscheduleID, FilterSchedule filterSchedule)
        {
            var filterExportField = filterSchedule.ExportTypeID == Enums.ExportType.ECN
                ? FilterExportField.getByFilterScheduleID(_clientconnection, filterscheduleID).FindAll(x => !x.IsCustomValue && !x.IsDescription)
                : FilterExportField.getByFilterScheduleID(_clientconnection, filterscheduleID).FindAll(x => !x.IsCustomValue);

            var selectedItem = GetSelectedItems(filterExportField);
            return selectedItem;
        }

        private static KMPlatform.Entity.User GetUser(int userId)
        {
            var user = KMPlatform.BusinessLogic.User.GetByUserID(userId, false);

            if (user == null)
            {
                throw new Exception("User does not exists");
            }

            return user;
        }

        private void ExportFtp(
            int filterscheduleId,
            KMPlatform.Entity.User user,
            Filter filter,
            ref DataTable subscriptionDataTableToFillIn)
        {
            var filterDisplayNameFields = FilterExportField.getDisplayName(_clientconnection, filterscheduleId);
            string[] columnsOrder;
            var counter = 0;

            if (!filterDisplayNameFields.Exists(x => x.ExportColumn.ToUpper() == SubscriptionExportColumnName))
            {
                columnsOrder = new string[filterDisplayNameFields.Count + 1];
                columnsOrder[0] = "SubscriptionID";
                counter = 1;
            }
            else
            {
                columnsOrder = new string[filterDisplayNameFields.Count];
            }

            foreach (var filterField in filterDisplayNameFields)
            {
                if (filter.ViewType == Enums.ViewType.ProductView)
                {
                    switch (filterField.DisplayName.ToUpper())
                    {
                        case "ADDRESS1":
                            columnsOrder[counter] = "Address";
                            break;
                        case "REGIONCODE":
                            columnsOrder[counter] = "State";
                            break;
                        case "ZIPCODE":
                            columnsOrder[counter] = "Zip";
                            break;
                        case "PUBTRANSACTIONDATE":
                            columnsOrder[counter] = "TransactionDate";
                            break;
                        case "QUALIFICATIONDATE":
                            columnsOrder[counter] = "QDate";
                            break;
                        default:
                            columnsOrder[counter] = filterField.DisplayName;
                            break;
                    }
                }
                else
                {
                    switch (filterField.DisplayName.ToUpper())
                    {
                        case "FNAME":
                            columnsOrder[counter] = "FirstName";
                            break;
                        case "LNAME":
                            columnsOrder[counter] = "LastName";
                            break;
                        case "ISLATLONVALID":
                            columnsOrder[counter] = "GeoLocated";
                            break;
                        default:
                            columnsOrder[counter] = filterField.DisplayName;
                            break;
                    }
                }
                counter++;
            }

            for (var j = 0; j < columnsOrder.Length; j++)
            {
                subscriptionDataTableToFillIn.Columns[columnsOrder[j]].SetOrdinal(j);
            }

            subscriptionDataTableToFillIn = (DataTable)ProfileFieldMask.MaskData(_clientconnection, subscriptionDataTableToFillIn, user);

            if (!filterDisplayNameFields.Exists(x => x.ExportColumn.ToUpper() == SubscriptionExportColumnName))
            {
                subscriptionDataTableToFillIn.Columns.Remove("subscriptionid");
            }
        }

        private static string FormatHeaderText(FilterSchedule filterSchedule, Filters filters, List<string> selectedFilterNumbers, List<string> suppressedFilterNumbers)
        {
            var headerText = new StringBuilder();            
            if (filterSchedule.SelectedOperation == string.Empty || filterSchedule.SelectedOperation == null)
            {
                headerText.Append(OperationsHeaderPrefix + "Single" + EndLineInHeader);
            }
            else
            {
                headerText.Append(OperationsHeaderPrefix + filterSchedule.SelectedOperation + EndLineInHeader);
            }

            AddSelectedFiltersToHeader(filters, selectedFilterNumbers, headerText);

            if (suppressedFilterNumbers.Any())
            {
                AddSuppressedFiltersToHeader(filterSchedule, filters, suppressedFilterNumbers, headerText);
            }

            return headerText.ToString();
        }

        private static void AddSelectedFiltersToHeader(Filters filters, List<string> selectedFilterNumbers, StringBuilder headerText)
        {
            var counter = 0;
            headerText.Append("Filters In");

            foreach (var filterNumber in selectedFilterNumbers)
            {
                if (counter > 0)
                {
                    headerText.Append(EndLineInHeader);
                }

                var filterFields = filters.SingleOrDefault(filter => filter.FilterNo.ToString() == filterNumber).Fields;
                foreach (var field in filterFields)
                {
                    headerText.Append(EndLineInHeader);
                    headerText.Append(field.Name + " = " + field.Text);

                    if (field.Name == AdHocFilterName || field.Name == OpenActivityFilterName || field.Name == ClickActivityForFilterName || field.Name == OpenEmailSentDateFilterName || field.Name == ClickEmailSentDateFilterName || field.Name == VisitActivityFilterName)
                    {
                        headerText.Append(field.Name == OpenActivityFilterName || field.Name == ClickActivityForFilterName || field.Name == OpenEmailSentDateFilterName || field.Name == ClickEmailSentDateFilterName || field.Name == VisitActivityFilterName ? field.SearchCondition + SearchConditionDelimeter + field.Values : SearchConditionDelimeter + field.SearchCondition + SearchConditionDelimeter + field.Values);
                    }                   
                }
                counter++;
            }
        }

        private static void AddSuppressedFiltersToHeader(FilterSchedule filterSchedule, Filters filters, List<string> suppressedFilterNumbers, StringBuilder headerText)
        {
            headerText.Append(EndLineInHeader);

            if (!string.IsNullOrEmpty(filterSchedule.SuppressedOperation))
            {
                headerText.Append(EndLineInHeader + "Operations Not In = " + filterSchedule.SuppressedOperation + EndLineInHeader);
            }

            headerText.Append("Filters NotIn");

            var counter = 0;
            foreach (var filterNumber in suppressedFilterNumbers)
            {
                if (counter > 0)
                {
                    headerText.Append(EndLineInHeader);
                }

                var filterFields = filters.SingleOrDefault(f => f.FilterNo.ToString() == filterNumber)?.Fields;

                if (filterFields != null)
                {
                    foreach (var field in filterFields)
                    {
                        headerText.Append(EndLineInHeader);
                        headerText.Append(field.Name + " = " + field.Text);

                        if (field.Name == AdHocFilterName || field.Name == OpenActivityFilterName || field.Name == ClickActivityForFilterName || field.Name == OpenEmailSentDateFilterName || field.Name == ClickEmailSentDateFilterName || field.Name == VisitActivityFilterName)
                        {
                            if (field.Name == OpenActivityFilterName || field.Name == ClickActivityForFilterName || field.Name == OpenEmailSentDateFilterName || field.Name == ClickEmailSentDateFilterName || field.Name == VisitActivityFilterName)
                            {
                                headerText.Append(field.SearchCondition + SearchConditionDelimeter + field.Values);
                            }
                            else
                            {
                                headerText.Append(SearchConditionDelimeter + field.SearchCondition + SearchConditionDelimeter + field.Values);
                            }
                        }
                    }
                }
                counter++;
            }
        }

        private static List<string> GetSelectedItems(List<FilterExportField> filterExportField)
        {
            var selectedItem = new List<string>();

            foreach (var feField in filterExportField)
            {
                if (string.IsNullOrEmpty( feField.FieldCase))
                {
                    selectedItem.Add(feField.ExportColumn + "|other|None");
                }
                else
                {
                    selectedItem.Add(feField.ExportColumn + "|other|" + feField.FieldCase);
                }
            }

            return selectedItem;
        }

        private static List<string> GetSupressedFilterNumbers(FilterSchedule filterSchedule, Filters filters)
        {
            var suppressedFilterNos = new List<string>();
            if (filterSchedule.FilterGroupID_Suppressed != null && filterSchedule.FilterGroupID_Suppressed.Any())
            {
                foreach (var filtergroupID in filterSchedule.FilterGroupID_Suppressed)
                {
                    var filterID = filters.SingleOrDefault(sf => sf.FilterGroupID == filtergroupID)?.FilterNo.ToString();
                    suppressedFilterNos.Add(filterID);
                }                
            }

            return suppressedFilterNos;
        }

        private static List<string> GetSeletcedFilterNumbers(FilterSchedule filterSchedule, Filters filters)
        {
            var selectedFilterNos = new List<string>();
            if (filterSchedule.FilterGroupID_Selected != null)
            {
                if (filterSchedule.FilterGroupID_Selected.Count > 0)
                {
                    foreach (var filtergroupID in filterSchedule.FilterGroupID_Selected)
                    {
                        var filterID = filters.SingleOrDefault(sf => sf.FilterGroupID == filtergroupID)?.FilterNo.ToString();
                        selectedFilterNos.Add(filterID);
                    }
                }
                else
                {
                    foreach (var filter in filters)
                    {
                        selectedFilterNos.Add(filter.FilterNo.ToString());
                    }
                }
            }
            else
            {
                foreach (var filter in filters)
                {
                    selectedFilterNos.Add(filter.FilterNo.ToString());
                }
            }

            return selectedFilterNos;
        }           
    }
}
