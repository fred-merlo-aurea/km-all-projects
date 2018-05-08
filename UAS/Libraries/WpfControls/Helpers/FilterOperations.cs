using FrameworkUAS.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfControls.Helpers
{
    public class FilterOperations
    {
        //THIS CLASS IS DEPRECATED.
        //Use FilterControls for all Filter related operations now.
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilter> filterData = FrameworkServices.ServiceClient.UAS_FilterClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetail> filterDetailData = FrameworkServices.ServiceClient.UAS_FilterDetailClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetailSelectedValue> filterDetailValuesData = FrameworkServices.ServiceClient.UAS_FilterDetailSelectedValueClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterGroup> filterGroupData = FrameworkServices.ServiceClient.UAS_FilterGroupClient();

        #region ForDisplayingFilterDetails
        //private FrameworkServices.ServiceClient<Circulation_WS.Interface.IDeliverability> deliverabilityData = FrameworkServices.ServiceClient.Circ_DeliverabilityClient();
        //private FrameworkServices.ServiceClient<Circulation_WS.Interface.IDeliverabilityMap> deliverabilityMapData = FrameworkServices.ServiceClient.Circ_DeliverabilityMapClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catData = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> transTypeData = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> transData = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> regionData = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryData = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> responseGroupData = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> codeSheetData = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IMasterCodeSheet> masterCodeSheetData = FrameworkServices.ServiceClient.UAD_MasterCodeSheetClient();
        #endregion
        #endregion
        #region ServiceResponse
        private FrameworkUAS.Service.Response<int> filterSaveResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<int> filterDetailSaveResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<int> filterDetailValueSaveResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Filter>> filterResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Filter>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetail>> filterDetailResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetail>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetailSelectedValue>> filterDetailValuesResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetailSelectedValue>>();

        #region ForDisplayingFilterDetail
        //private FrameworkUAS.Service.Response<List<FrameworkCirculation.Entity.Deliverability>> deliverabilityResponse = new FrameworkUAS.Service.Response<List<FrameworkCirculation.Entity.Deliverability>>();
        //private FrameworkUAS.Service.Response<List<FrameworkCirculation.Entity.DeliverabilityMap>> deliverabilityMapResponse = new FrameworkUAS.Service.Response<List<FrameworkCirculation.Entity.DeliverabilityMap>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> ccTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> ccResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> transTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> transResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>> regionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>> countryResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> csResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> rGroupResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> qSourceResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>> mcsResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MasterCodeSheet>>(); 
        #endregion
        #endregion
        #region Variables/Lists
        private static bool loadedData = false;
        private List<string> filterNameList = new List<string>();
        private static List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private static List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private static List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypeList = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private static List<FrameworkUAD_Lookup.Entity.TransactionCodeType> tCodeTypeList = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private static List<FrameworkUAD_Lookup.Entity.Code> QSourceList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private static List<FrameworkUAD_Lookup.Entity.Region> regionList = new List<FrameworkUAD_Lookup.Entity.Region>();
        private static List<FrameworkUAD_Lookup.Entity.Country> countryList = new List<FrameworkUAD_Lookup.Entity.Country>();
        //private static List<FrameworkCirculation.Entity.Deliverability> mediaList = new List<FrameworkCirculation.Entity.Deliverability>();
        private static List<FrameworkUAD.Entity.ResponseGroup> responseGroupList = new List<FrameworkUAD.Entity.ResponseGroup>();
        private static List<FrameworkUAD.Entity.CodeSheet> codeSheetList = new List<FrameworkUAD.Entity.CodeSheet>();
        private static List<FrameworkUAD.Entity.MasterCodeSheet> mCodeSheetList = new List<FrameworkUAD.Entity.MasterCodeSheet>();
        private static List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private static List<FrameworkUAD_Lookup.Entity.Code> mediaList = new List<FrameworkUAD_Lookup.Entity.Code>();
        #endregion
        #region Filter Classes
        public class FilterContainer
        {
            public Filter Filter { get; set; }
            public List<FilterDetailContainer> FilterDetails {get; set;}

            public FilterContainer()
            {
                this.Filter = new Filter();
                this.FilterDetails = new List<FilterDetailContainer>();
            }
        }

        public class FilterDetailContainer
        {
            public FilterDetail FilterDetail { get; set; }
            public List<FilterDetailSelectedValue> Values { get; set; }

            public FilterDetailContainer()
            {
                this.FilterDetail = new FilterDetail();
                this.Values = new List<FilterDetailSelectedValue>();
            }
        }

        public class DisplayedFilterDetail
        {
            public string FilterObject { get; set; }
            public string FilterValues { get; set; }

            public DisplayedFilterDetail(string fo, string fv)
            {
                this.FilterObject = fo;
                this.FilterValues = fv;
            }
        }
        #endregion

        public int SaveFilterContainer(FilterContainer container, string filterName)
        {
            Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            filterResponse = filterData.Proxy.Select(accessKey, container.Filter.ProductId);
            if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
                filterNameList = filterResponse.Result.Where(x=> x.IsActive == true && x.FilterGroupID == container.Filter.FilterGroupID).Select(x => x.FilterName).ToList();

            if(filterTypes.Count == 0)
            {
                codeResponse = codeData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Type);
                if(Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                {
                    filterTypes = codeResponse.Result;
                }
            }

            if (filterNameList.Contains(filterName))
            {
                MessageBox.Show("A filter with this name already exists. Please enter another one and try again.", "Warning", MessageBoxButton.OK);
                return -1;
            }

            int filterID = -1;
            container.Filter.FilterName = filterName;
            container.Filter.DateCreated = DateTime.Now;
            container.Filter.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            container.Filter.ClientID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
            filterSaveResponse = filterData.Proxy.Save(accessKey, container.Filter);
            if (Common.CheckResponse(filterSaveResponse.Result, filterSaveResponse.Status))
                filterID = filterSaveResponse.Result;
            else
                return filterID;

            foreach(FilterDetailContainer fdc in container.FilterDetails)
            {
                int filterDetailID = -1;
                fdc.FilterDetail.FilterId = filterID;
                fdc.FilterDetail.FilterGroupID = container.Filter.FilterGroupID;
                fdc.FilterDetail.DateCreated = DateTime.Now;
                fdc.FilterDetail.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                filterDetailSaveResponse = filterDetailData.Proxy.Save(accessKey, fdc.FilterDetail);
                if (Common.CheckResponse(filterDetailSaveResponse.Result, filterDetailSaveResponse.Status))
                    filterDetailID = filterDetailSaveResponse.Result;
                else
                    return filterDetailID;

                if (fdc.FilterDetail.FilterTypeId == filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Dynamic.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
                {
                    foreach (FilterDetailSelectedValue value in fdc.Values)
                    {
                        if (value.SelectedValue.Contains(":"))
                        {
                            string[] temp = value.SelectedValue.Split(':');
                            value.SelectedValue = temp[1];
                        }
                        value.DateCreated = DateTime.Now;
                        value.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        value.FilterDetailId = filterDetailID;
                        filterDetailValuesData.Proxy.Save(accessKey, value);
                    }
                }
                else
                {
                    foreach (FilterDetailSelectedValue value in fdc.Values)
                    {
                        value.DateCreated = DateTime.Now;
                        value.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        value.FilterDetailId = filterDetailID;
                        filterDetailValuesData.Proxy.Save(accessKey, value);
                    }
                }
            }

            return filterID;
        }

        public AddRemoveDataFetcher.AddRemoveDetail GetFilterDetailForAddKill(FrameworkUAS.Object.AppData appData, int pub, 
            List<int> ids, FrameworkUAD.Entity.SubscriberAddKill sak = null)
        {
            AddRemoveDataFetcher.AddRemoveDetail arDetail = new AddRemoveDataFetcher.AddRemoveDetail();
            ObservableCollection<DisplayedFilterDetail> deets = new ObservableCollection<DisplayedFilterDetail>();
            FilterContainer fc = new FilterContainer();

            //filterResponse = filterData.Proxy.Select(appData.AuthorizedUser.AuthAccessKey, pub);
            //if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
            //    fc.Filter = filterResponse.Result.Where(x=> x.FilterId == sak.FilterID).FirstOrDefault();

            if (fc.Filter == null)
                return null;

            #region CreateFilterContainer

            //filterDetailResponse = filterDetailData.Proxy.Select(appData.AuthorizedUser.AuthAccessKey, sak.FilterID);
            //if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
            //{                
            //    foreach(FilterDetail fd in filterDetailResponse.Result)
            //    {
            //        FilterDetailContainer detailContainer = new FilterDetailContainer();
            //        detailContainer.FilterDetail = fd;
            //        if(fd.FilterTypeId != 3)
            //        {
            //            filterDetailValuesResponse = filterDetailValuesData.Proxy.Select(appData.AuthorizedUser.AuthAccessKey, fd.FilterDetailId);
            //            if (Common.CheckResponse(filterDetailValuesResponse.Result, filterDetailValuesResponse.Status))
            //            {
            //                string values = "";
            //                foreach (FilterDetailSelectedValue fdsv in filterDetailValuesResponse.Result)
            //                {
            //                    detailContainer.Values.Add(fdsv);
            //                    values = values + fdsv.SelectedValue + ",";
            //                }
            //                values = values.TrimEnd(',');
            //                deets.Add(new DisplayedFilterDetail(fd.FilterObjectType, values));
            //            }
            //        }
            //        else
            //        {
            //            if (fd.FilterObjectType == "Standard")
            //                deets.Add(new DisplayedFilterDetail("AdHoc Filter - " + fd.FilterField, fd.AdHocFieldValue));
            //            else
            //                deets.Add(new DisplayedFilterDetail("AdHoc Filter - " + fd.FilterField, "From: " + fd.AdHocFromField + ", To: " + fd.AdHocToField));
            //        }
            //        fc.FilterDetails.Add(detailContainer);
            //    }
            //}

            #endregion

            if (sak.Type == "Add")
            {
                //arDetail = new AddRemoveDataFetcher.AddRemoveDetail(sak.Count, 0, deets, ids, fc);
                arDetail.ActualCount = sak.AddKillCount;
            }
            else if (sak.Type == "Remove")
            {
                //arDetail = new AddRemoveDataFetcher.AddRemoveDetail(0, sak.Count, deets, ids, fc);
                arDetail.ActualCount = sak.AddKillCount;
            }
            arDetail.AddKillID = sak.AddKillID;

            return arDetail;
        }

        public FilterContainer GetFilterContainer(FrameworkUAS.Object.AppData appData, int filterID, int productID)
        {
            FilterContainer returnContainer = new FilterContainer();
            List<FilterDetailContainer> detailContainers = new List<FilterDetailContainer>(); 
            Filter filter = new Filter();
            List<FilterDetail> details = new List<FilterDetail>();
            List<FilterDetailSelectedValue> selectDetails = new List<FilterDetailSelectedValue>();

            filterResponse = filterData.Proxy.Select(appData.AuthorizedUser.AuthAccessKey, productID);
            if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
                filter = filterResponse.Result.Where(x=> x.FilterId == filterID).FirstOrDefault();
            filterDetailResponse = filterDetailData.Proxy.Select(appData.AuthorizedUser.AuthAccessKey, filterID);
            if (Common.CheckResponse(filterDetailResponse.Result, filterDetailResponse.Status))
                details = filterDetailResponse.Result;

            foreach(FilterDetail fd in details)
            {
                filterDetailValuesResponse = filterDetailValuesData.Proxy.Select(appData.AuthorizedUser.AuthAccessKey, fd.FilterDetailId);
                if (Common.CheckResponse(filterDetailValuesResponse.Result, filterDetailValuesResponse.Status))
                    selectDetails = filterDetailValuesResponse.Result;

                FilterDetailContainer fdc = new FilterDetailContainer();
                fdc.FilterDetail = fd;
                fdc.Values.AddRange(selectDetails);
                detailContainers.Add(fdc);
            }

            returnContainer.Filter = filter;
            returnContainer.FilterDetails = detailContainers;
            return returnContainer;
        }

        public FrameworkUAD.Object.Reporting GetReportingObjectFromContainer(FilterContainer fc, int myPubID)
        {
            if(filterTypes.Count == 0)
            {
                codeResponse = codeData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Type);
                if(Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                {
                    filterTypes = codeResponse.Result;
                }
            }

            Dictionary<string, string> myValues = new Dictionary<string,string>();
            string adHocXML = "";
            StringBuilder sbXML = new StringBuilder();
            FrameworkUAD.Object.Reporting obj = new FrameworkUAD.Object.Reporting();

            #region Set Up Dictionary
            myValues.Add("CategoryCodeTypeID", "");
            myValues.Add("CategoryCodeID", "");
            myValues.Add("TransactionCodeTypeID", "");
            myValues.Add("TransactionCodeID", "");
            myValues.Add("QSourceID", "");
            myValues.Add("RegionCode", "");
            myValues.Add("Region", "");
            myValues.Add("CountryID", "");
            myValues.Add("CodeId", "");
            myValues.Add("DeliverabilityCode", "");
            myValues.Add("Email", "");
            myValues.Add("Fax", "");
            myValues.Add("Phone", "");
            myValues.Add("Mobile", "");
            myValues.Add("ToDate", "");
            myValues.Add("FromDate", "");
            myValues.Add("Year", "");
            myValues.Add("Responses", "");
            myValues.Add("UADResponses", "");
            myValues.Add("Demo31", "");
            myValues.Add("Demo32", "");
            myValues.Add("Demo33", "");
            myValues.Add("Demo34", "");
            myValues.Add("Demo35", "");
            myValues.Add("Demo36", "");
            myValues.Add("IsMailable", "");
            myValues.Add("EmailStatusID", "");
            myValues.Add("ZipCode", "");
            myValues.Add("RangeMax", "");
            myValues.Add("RangeMin", "");
            myValues.Add("OpenSearchType", "");
            myValues.Add("OpenCount", "-1");
            myValues.Add("OpenDateFrom", "");
            myValues.Add("OpenDateTo", "");
            myValues.Add("OpenBlastID", "");
            myValues.Add("OpenEmailSubject", "");
            myValues.Add("OpenEmailFromDate", "");
            myValues.Add("OpenEmailToDate", "");
            myValues.Add("ClickSearchType", "");
            myValues.Add("ClickCount", "-1");
            myValues.Add("ClickURL", "");
            myValues.Add("ClickDateFrom", "");
            myValues.Add("ClickDateTo", "");
            myValues.Add("ClickBlastID", "");
            myValues.Add("ClickEmailSubject", "");
            myValues.Add("ClickEmailFromDate", "");
            myValues.Add("ClickEmailToDate", "");
            myValues.Add("Domain", "");
            myValues.Add("VisitsURL", "");
            myValues.Add("VisitsDateFrom", "");
            myValues.Add("VisitsDateTo", "");
            myValues.Add("BrandID", "");
            myValues.Add("SearchType", "");
            myValues.Add("PublicationIDs", "");
            myValues.Add("WaveMail", "");
            #endregion

            foreach (Helpers.FilterOperations.FilterDetailContainer fdc in fc.FilterDetails)
            {
                string values = "";

                if (fdc.FilterDetail.FilterTypeId == filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.AdHoc.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
                {
                    adHocXML = Core_AMS.Utilities.XmlFunctions.ToXML(fdc.FilterDetail);
                    sbXML.AppendLine(adHocXML);
                }
                else if (fdc.FilterDetail.FilterTypeId == filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Dynamic.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
                {                    
                    foreach(FilterDetailSelectedValue fdsv in fdc.Values)
                    {
                        values = values + fdsv.SelectedValue.Trim() + ",";    
                    }
                    values = values.TrimEnd(',');
                    if(values.Contains(":"))
                    {
                        string[] split = values.Split(',');
                        string finalValues = "";
                        string curr = "";
                        foreach(string s in split)
                        {
                            string[] temp = s.Split(':');
                            if (curr != temp[0])
                            {
                                curr = temp[0];
                                finalValues += ":" + temp[1];
                            }
                            else
                                finalValues += "," + temp[1];
                        }
                        finalValues = finalValues.Trim(':');
                        finalValues = finalValues.Trim(',');
                        myValues[fdc.FilterDetail.FilterObjectType] = finalValues;
                    }
                    else
                    {
                        values = values.TrimEnd(',');
                        myValues[fdc.FilterDetail.FilterObjectType] = values;
                    }
                }
                else
                {
                    foreach (FrameworkUAS.Entity.FilterDetailSelectedValue fdsv in fdc.Values)
                    {
                        values = values + fdsv.SelectedValue.Trim() + ",";
                    }
                    values = values.TrimEnd(',');
                    if (fdc.FilterDetail.FilterField == "lbQSource")
                        myValues["QSourceID"] = values;
                    else
                        myValues[fdc.FilterDetail.FilterObjectType] = values;
                }
            }

            #region Calculate LatLong
            if ((myValues["ZipCode"] != string.Empty) && (myValues["RangeMax"] != string.Empty) && ((Int32.Parse(myValues["RangeMin"])) < (Int32.Parse(myValues["RangeMax"]))))
            {
                Core_AMS.Utilities.AddressLocation myLocation = new Core_AMS.Utilities.AddressLocation();
                myLocation.PostalCode = myValues["ZipCode"];
                Core_AMS.Utilities.AddressValidator av = new Core_AMS.Utilities.AddressValidator();
                myLocation = av.ValidateGoogle(myLocation);
                if (myLocation.IsValid)
                {
                    Double PI_180 = Math.PI / 180D;
                    Double salonLat = Convert.ToDouble(myLocation.Latitude);
                    Double salonLon = Convert.ToDouble(myLocation.Longitude);

                    Double radiusLatTotal_RangeMax = Convert.ToDouble(myValues["RangeMax"]) / 69D;
                    Double minLat_RangeMax = salonLat - radiusLatTotal_RangeMax;
                    Double maxLat_RangeMax = salonLat + radiusLatTotal_RangeMax;
                    Double minLon_RangeMax = salonLon + (radiusLatTotal_RangeMax / Math.Cos(minLat_RangeMax * PI_180));
                    Double maxLon_RangeMax = salonLon - (radiusLatTotal_RangeMax / Math.Cos(maxLat_RangeMax * PI_180));

                    Double radiusLatTotal_RangeMin = Convert.ToDouble(myValues["RangeMin"]) / 69D;
                    Double minLat_RangeMin = salonLat - radiusLatTotal_RangeMin;
                    Double maxLat_RangeMin = salonLat + radiusLatTotal_RangeMin;
                    Double minLon_RangeMin = salonLon + (radiusLatTotal_RangeMin / Math.Cos(minLat_RangeMin * PI_180));
                    Double maxLon_RangeMin = salonLon - (radiusLatTotal_RangeMin / Math.Cos(maxLat_RangeMin * PI_180));

                    Double temp;

                    if ((minLat_RangeMin > maxLat_RangeMin) && (minLon_RangeMin < maxLon_RangeMin))
                    {
                        temp = maxLat_RangeMin;
                        maxLat_RangeMin = minLat_RangeMin;
                        minLat_RangeMin = temp;
                    }
                    else if ((minLat_RangeMin < maxLat_RangeMin) && (minLon_RangeMin > maxLon_RangeMin))
                    {
                        temp = maxLon_RangeMin;
                        maxLon_RangeMin = minLon_RangeMin;
                        minLon_RangeMin = temp;

                    }
                    else if ((minLat_RangeMin > maxLat_RangeMin) && (minLon_RangeMin > maxLon_RangeMin))
                    {
                        temp = maxLat_RangeMin;
                        maxLat_RangeMin = minLat_RangeMin;
                        minLat_RangeMin = temp;

                        temp = maxLon_RangeMin;
                        maxLon_RangeMin = minLon_RangeMin;
                        minLon_RangeMin = temp;
                    }

                    if ((minLat_RangeMax > maxLat_RangeMax) && (minLon_RangeMax < maxLon_RangeMax))
                    {
                        temp = maxLat_RangeMax;
                        maxLat_RangeMax = minLat_RangeMax;
                        minLat_RangeMax = temp;
                    }
                    else if ((minLat_RangeMax < maxLat_RangeMax) && (minLon_RangeMax > maxLon_RangeMax))
                    {
                        temp = maxLon_RangeMax;
                        maxLon_RangeMax = minLon_RangeMax;
                        minLon_RangeMax = temp;

                    }
                    else if ((minLat_RangeMax > maxLat_RangeMax) && (minLon_RangeMax > maxLon_RangeMax))
                    {
                        temp = maxLat_RangeMax;
                        maxLat_RangeMax = minLat_RangeMax;
                        minLat_RangeMax = temp;

                        temp = maxLon_RangeMax;
                        maxLon_RangeMax = minLon_RangeMax;
                        minLon_RangeMax = temp;
                    }

                    obj.RangeMaxLatMax = maxLat_RangeMax.ToString();
                    obj.RangeMaxLatMin = minLat_RangeMax.ToString();
                    obj.RangeMaxLonMax = maxLon_RangeMax.ToString();
                    obj.RangeMaxLonMin = minLon_RangeMax.ToString();
                    obj.RangeMinLatMax = maxLat_RangeMin.ToString();
                    obj.RangeMinLatMin = minLat_RangeMin.ToString();
                    obj.RangeMinLonMax = maxLon_RangeMin.ToString();
                    obj.RangeMinLonMin = minLat_RangeMin.ToString();

                }
            }
            #endregion

            #region Calculate Open, Click Counts
            if(myValues["OpenCount"] != "-1")
                myValues["OpenCount"] = Regex.Match(myValues["OpenCount"], @"\d+").Value.ToString();
            if (myValues["OpenCount"] == string.Empty || myValues["OpenCount"] == null)
                myValues["OpenCount"] = "0";

            if (myValues["ClickCount"] != "-1")
                myValues["ClickCount"] = Regex.Match(myValues["OpenCount"], @"\d+").Value.ToString();
            if (myValues["ClickCount"] == string.Empty || myValues["OpenCount"] == null)
                myValues["ClickCount"] = "0";
            #endregion

            if (myPubID > 0)
                obj.PublicationIDs = myPubID.ToString();
            else
                obj.PublicationIDs = myValues["PublicationIDs"];

            #region Set Up Object
            obj.CategoryIDs = myValues["CategoryCodeTypeID"];
            obj.TransactionIDs = myValues["TransactionCodeTypeID"];
            obj.QSourceIDs = myValues["QSourceID"];
            obj.StateIDs = myValues["RegionCode"];
            obj.Regions = myValues["Region"];
            obj.CountryIDs = myValues["CountryID"];
            obj.Email = myValues["Email"];
            obj.Fax = myValues["Fax"];
            obj.Mobile = myValues["Mobile"];
            obj.Phone = myValues["Phone"];
            obj.FromDate = myValues["FromDate"];
            obj.ToDate = myValues["ToDate"];
            obj.Year = myValues["Year"];
            obj.CategoryCodes = myValues["CategoryCodeID"];
            obj.TransactionCodes = myValues["TransactionCodeID"];
            obj.ResponseIDs = myValues["Responses"];
            obj.UADResponseIDs = myValues["UADResponses"];
            if (myValues["CodeId"] != string.Empty)
                obj.Media = myValues["CodeId"];
            obj.AdHocXML = ("<XML>" + sbXML.ToString() + "</XML>").Replace("\r\n", "");
            obj.Demo31 = myValues["Demo31"];
            obj.Demo32 = myValues["Demo32"];
            obj.Demo33 = myValues["Demo33"];
            obj.Demo34 = myValues["Demo34"];
            obj.Demo35 = myValues["Demo35"];
            obj.Demo36 = myValues["Demo36"];
            obj.IsMailable = myValues["IsMailable"];
            obj.EmailStatusIDs = myValues["EmailStatusID"];
            obj.OpenSearchType = myValues["OpenSearchType"];
            obj.OpenCount = myValues["OpenCount"];
            obj.OpenDateFrom = myValues["OpenDateFrom"];
            obj.OpenDateTo = myValues["OpenDateTo"];
            obj.OpenBlastID = myValues["OpenBlastID"];
            obj.OpenEmailSubject = myValues["OpenEmailSubject"];
            obj.OpenEmailFromDate = myValues["OpenEmailFromDate"];
            obj.OpenEmailToDate = myValues["OpenEmailToDate"];
            obj.ClickSearchType = myValues["ClickSearchType"];
            obj.ClickCount = myValues["ClickCount"];
            obj.ClickURL = myValues["ClickURL"];
            obj.ClickDateFrom = myValues["ClickDateFrom"];
            obj.ClickDateTo = myValues["ClickDateTo"];
            obj.ClickBlastID = myValues["ClickBlastID"];
            obj.ClickEmailSubject = myValues["ClickEmailSubject"];
            obj.ClickEmailFromDate = myValues["ClickEmailFromDate"];
            obj.ClickEmailToDate = myValues["ClickEmailToDate"];
            obj.Domain = myValues["Domain"];
            obj.VisitsURL = myValues["VisitsURL"];
            obj.VisitsDateFrom = myValues["VisitsDateFrom"];
            obj.VisitsDateTo = myValues["VisitsDateTo"];
            obj.BrandID = myValues["BrandID"];
            obj.SearchType = myValues["SearchType"];
            obj.WaveMail = myValues["WaveMail"];
            #endregion

            return obj;
        }

        public FrameworkUAD.Object.Reporting GetReportingObjectFromFilter(FrameworkUAS.Object.AppData appData, int filterID, int pubID)
        {
            Dictionary<string, string> myValues = new Dictionary<string, string>();
            StringBuilder sbXML = new StringBuilder();
            string adHocXML = "";
            FrameworkUAD.Object.Reporting obj = new FrameworkUAD.Object.Reporting();
            FrameworkUAS.Entity.Filter f = new Filter();

            filterResponse = filterData.Proxy.Select(appData.AuthorizedUser.AuthAccessKey, pubID);
            if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
                f = filterResponse.Result.Where(x => x.FilterId == filterID).FirstOrDefault();

            filterDetailResponse = filterDetailData.Proxy.Select(appData.AuthorizedUser.AuthAccessKey, f.FilterId);
            if (Common.CheckResponse(filterDetailResponse.Result, filterDetailResponse.Status))
            {
                foreach (FrameworkUAS.Entity.FilterDetail fd in filterDetailResponse.Result)
                {
                    if (fd.FilterTypeId != 3)
                    {
                        filterDetailValuesResponse = filterDetailValuesData.Proxy.Select(appData.AuthorizedUser.AuthAccessKey, fd.FilterDetailId);
                        if (Common.CheckResponse(filterDetailValuesResponse.Result, filterDetailValuesResponse.Status))
                        {
                            string value = "";
                            foreach (FrameworkUAS.Entity.FilterDetailSelectedValue fdsv in filterDetailValuesResponse.Result)
                            {
                                value += fdsv.SelectedValue + ",";
                            }
                            value = value.TrimEnd(',');
                            #region Find Reporting Property
                            switch (fd.FilterField)
                            {
                                case "lbCategory":
                                    obj.CategoryIDs = value;
                                    break;
                                case "lbCatCode":
                                    obj.CategoryCodes = value;
                                    break;
                                case "lbTransaction":
                                    obj.TransactionIDs = value;
                                    break;
                                case "lbTransCode":
                                    obj.TransactionCodes = value;
                                    break;
                                case "lbQSource":
                                    obj.QSourceIDs = value;
                                    break;
                                case "lbState":
                                    obj.StateIDs = value;
                                    break;
                                case "lbRegion":
                                    obj.Regions = value;
                                    break;
                                case "lbCountry":
                                    obj.CountryIDs = value;
                                    break;
                                case "lbMedia":
                                    obj.Media = value;
                                    break;
                                case "lbYear":
                                    obj.Year = value;
                                    break;
                                case "rcbEmail":
                                    obj.Email = value;
                                    break;
                                case "rcbPhone":
                                    obj.Phone = value;
                                    break;
                                case "rcbFax":
                                    obj.Fax = value;
                                    break;
                                case "rdpTo":
                                    obj.ToDate = value;
                                    break;
                                case "rdpFrom":
                                    obj.FromDate = value;
                                    break;
                                case "rpbReportFilters":
                                    obj.ResponseIDs = value;
                                    break;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        adHocXML = Core_AMS.Utilities.XmlFunctions.ToXML(fd);
                        sbXML.AppendLine(adHocXML);
                    }
                }
            }
            obj.AdHocXML = "<XML>" + sbXML.ToString() + "</XML>";
            obj.PublicationIDs = pubID.ToString();
            return obj;
        }

        public ObservableCollection<DisplayedFilterDetail> GetDisplayFilterDetail(FilterContainer fc)
        {
            #region LoadData
            Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            if(loadedData == false)
            {
                ccTypeResponse = catTypeData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(ccTypeResponse.Result, ccTypeResponse.Status))
                {
                    catCodeTypeList = ccTypeResponse.Result;
                }

                ccResponse = catData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(ccResponse.Result, ccResponse.Status))
                {
                    catCodeList = ccResponse.Result.Where(x => x.IsActive == true).OrderBy(x => x.CategoryCodeValue).ToList();
                    //catCodeList = catCodeList.GroupBy(x => x.CategoryCodeValue).Select(x => x.First()).ToList();

                    foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in catCodeList)
                    {
                        cc.CategoryCodeName = cc.CategoryCodeValue + "-" + cc.CategoryCodeName;
                    }
                }

                transTypeResponse = transTypeData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(transTypeResponse.Result, transTypeResponse.Status))
                {
                    tCodeTypeList = transTypeResponse.Result;
                }

                transResponse = transData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(transResponse.Result, transResponse.Status))
                {
                    transCodeList = transResponse.Result.Where(x => x.IsActive == true).OrderBy(x => x.TransactionCodeValue).ToList();
                    //transCodeList = transCodeList.GroupBy(x => x.TransactionCodeValue).Select(x => x.First()).ToList();
                    foreach (FrameworkUAD_Lookup.Entity.TransactionCode tCode in transCodeList)
                    {
                        tCode.TransactionCodeName = tCode.TransactionCodeValue + "." + tCode.TransactionCodeName;
                    }
                }

                qSourceResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source);//qSourceData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(qSourceResponse.Result, qSourceResponse.Status))
                {
                    QSourceList = qSourceResponse.Result;
                }

                regionResponse = regionData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(regionResponse.Result, regionResponse.Status))
                {
                    regionList = regionResponse.Result;
                }

                countryResponse = countryData.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(countryResponse.Result, countryResponse.Status))
                {
                    countryList = countryResponse.Result;
                }

                codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Deliver);
                if(Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                {
                    mediaList = codeResponse.Result;
                }

                rGroupResponse = responseGroupData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, fc.Filter.ProductId);
                if (Helpers.Common.CheckResponse(rGroupResponse.Result, rGroupResponse.Status))
                    responseGroupList = rGroupResponse.Result.Where(x => !x.ResponseGroupName.Equals("DEMO7") && !x.ResponseGroupName.Equals("EXPIRE")).OrderBy(x => x.DisplayOrder).ToList();

                csResponse = codeSheetData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (Helpers.Common.CheckResponse(csResponse.Result, csResponse.Status))
                    codeSheetList = csResponse.Result;

                codeResponse = codeData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Type);
                if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                {
                    filterTypes = codeResponse.Result;
                }

                mcsResponse = masterCodeSheetData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if(Helpers.Common.CheckResponse(mcsResponse.Result, mcsResponse.Status))
                {
                    mCodeSheetList = mcsResponse.Result;
                }

                loadedData = true;
            }
            #endregion

            int standard = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Standard.ToString()).Select(x => x.CodeId).FirstOrDefault();
            int dynamic = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Dynamic.ToString()).Select(x => x.CodeId).FirstOrDefault();
            int activity = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Activity.ToString()).Select(x => x.CodeId).FirstOrDefault();
            int adHoc = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.AdHoc.ToString()).Select(x => x.CodeId).FirstOrDefault();
            ObservableCollection<DisplayedFilterDetail> retCollection = new ObservableCollection<DisplayedFilterDetail>();
            foreach (Helpers.FilterOperations.FilterDetailContainer fdc in fc.FilterDetails)
            {
                string values = "";

                if (fdc.FilterDetail.FilterTypeId == standard)
                {
                    foreach (FrameworkUAS.Entity.FilterDetailSelectedValue fdsv in fdc.Values)
                    {
                        int val = -1;
                        bool isNumeric = int.TryParse(fdsv.SelectedValue, out val);
                        if(!isNumeric)
                            values = values + fdsv.SelectedValue + ",";
                        else
                        {
                            string newValue = GetDisplayValue(fdc.FilterDetail.FilterObjectType, val);
                            values = values + newValue + ",";
                        }
                    }
                    values = values.TrimEnd(',');
                    if (values.Length > 0)
                        retCollection.Add(new DisplayedFilterDetail(fdc.FilterDetail.FilterObjectType, values));
                }
                else if (fdc.FilterDetail.FilterTypeId == dynamic)
                {
                    foreach (FrameworkUAS.Entity.FilterDetailSelectedValue fdsv in fdc.Values)
                    {
                        int val = -1;
                        if(fdsv.SelectedValue.Contains("YY") || fdsv.SelectedValue.Contains("ZZ"))
                            values = values + fdsv.SelectedValue.Substring(0, 2) + ",";
                        else
                        {
                            string[] array = fdsv.SelectedValue.Split('_');
                            if (array[1] != null)
                            {
                                int.TryParse(array[1].ToString(), out val);
                                string newValue = GetDisplayValue(fdc.FilterDetail.FilterObjectType, val);
                                values = values + newValue + ",";
                            }
                        }
                    }
                    values = values.TrimEnd(',');
                    if (values.Length > 0)
                        retCollection.Add(new DisplayedFilterDetail(fdc.FilterDetail.FilterObjectType, values));
                }
                else if (fdc.FilterDetail.FilterTypeId == adHoc)
                {
                    if (fdc.FilterDetail.FilterObjectType == "Standard")
                        retCollection.Add(new DisplayedFilterDetail("AdHoc Filter - " + fdc.FilterDetail.FilterField, fdc.FilterDetail.AdHocFieldValue));
                    else
                        retCollection.Add(new DisplayedFilterDetail("AdHoc Filter - " + fdc.FilterDetail.FilterField, "From: " + fdc.FilterDetail.AdHocFromField + ", To: " + fdc.FilterDetail.AdHocToField));
                }
                else if (fdc.FilterDetail.FilterTypeId == activity)
                {
                    foreach (FrameworkUAS.Entity.FilterDetailSelectedValue fdsv in fdc.Values)
                    {
                        values = values + fdsv.SelectedValue + ",";
                    }
                    values = values.TrimEnd(',');
                    retCollection.Add(new DisplayedFilterDetail(fdc.FilterDetail.FilterObjectType, values));
                }
            }
            return retCollection;
        }

        private string GetDisplayValue(string filterObject, int value)
        {
            string returnMe = "";

            if (filterObject == "Email" || filterObject == "Phone" || filterObject == "Fax" || filterObject == "Mobile")
            {
                if (value == 0)
                    returnMe = "Yes";
                else
                    returnMe = "No";
            }
            else if (filterObject == "CategoryCodeID")
            {
                int cat = catCodeList.Where(x => x.CategoryCodeID == value).Select(x => x.CategoryCodeValue).FirstOrDefault();
                if (cat > 0)
                    returnMe = cat.ToString();
                return returnMe.ToString();
            }
            else if (filterObject == "CategoryCodeTypeID")
            {
                returnMe = catCodeTypeList.Where(x => x.CategoryCodeTypeID == value).Select(x => x.CategoryCodeTypeName).FirstOrDefault();
            }
            else if (filterObject == "TransactionCodeID")
            {
                int trans = transCodeList.Where(x => x.TransactionCodeID == value).Select(x => x.TransactionCodeValue).FirstOrDefault();
                if (trans > 0)
                    returnMe = trans.ToString();
                return returnMe.ToString();
            }
            else if (filterObject == "TransactionCodeTypeID")
            {
                returnMe = tCodeTypeList.Where(x => x.TransactionCodeTypeID == value).Select(x => x.TransactionCodeTypeName).FirstOrDefault();
            }
            else if (filterObject == "QSourceID")
            {
                returnMe = QSourceList.Where(x => x.CodeId == value).Select(x => x.DisplayName).FirstOrDefault();
            }
            else if (filterObject == "RegionID")
            {
                //returnMe = regionList.Where(x => x.RegionID == value).Select(x => x.RegionCode).FirstOrDefault();
                returnMe = value.ToString();
            }
            else if (filterObject == "CountryID")
            {
                returnMe = countryList.Where(x => x.CountryID == value).Select(x => x.ShortName).FirstOrDefault();
            }
            else if (filterObject == "CodeID")
            {
                returnMe = mediaList.Where(x => x.CodeId == value).Select(x => x.DisplayName).FirstOrDefault();
            }
            else if (filterObject == "Year")
                returnMe = value.ToString();
            else if(filterObject == "Responses")
            {
                FrameworkUAD.Entity.CodeSheet cs = codeSheetList.Where(x => x.CodeSheetID == value).FirstOrDefault();
                returnMe = cs.ResponseGroup + " - " + cs.ResponseDesc;
            }
            else if (filterObject == "UADResponses")
            {
                FrameworkUAD.Entity.MasterCodeSheet msc = mCodeSheetList.Where(x => x.MasterID == value).FirstOrDefault();
                returnMe = msc.MasterDesc;
            }
            else if (filterObject == "PublicationIDs")
            {
                if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.Products != null)
                    returnMe = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.Products.Where(x => x.ProductID == value).Select(x => x.ProductCode).FirstOrDefault();
            }

            if (returnMe != "")
                return returnMe;
            else
                return value.ToString();
        }

        public FrameworkUAD.Object.ReportingXML GetXMLFilter(FilterContainer fc, int pubID)
        {
            FrameworkUAD.Object.ReportingXML rtnItem = new FrameworkUAD.Object.ReportingXML();
            string xml = "<XML><Filters>";
            string adhoc = "<XML></XML>";
            FrameworkUAD.Object.Reporting r = GetReportingObjectFromContainer(fc, pubID);
            xml += "<ProductID>" + r.PublicationIDs + "</ProductID>";
            if(r.CategoryIDs.Length > 0)
                xml += "<CategoryType>" + r.CategoryIDs + "</CategoryType>";
            if(r.CategoryCodes.Length > 0)
                xml += "<CategoryCode>" + r.CategoryCodes + "</CategoryCode>";
            if(r.TransactionIDs.Length > 0)
                xml += "<TransactionType>" + r.TransactionIDs + "</TransactionType>";
            if(r.TransactionCodes.Length > 0)
                xml += "<TransactionCode>" + r.TransactionCodes + "</TransactionCode>";
            if(r.QSourceIDs.Length > 0)
                xml += "<QsourceIDs>" + r.QSourceIDs + "</QsourceIDs>";
            if(r.StateIDs.Length > 0)
                xml += "<StateIDs>" + r.StateIDs + "</StateIDs>";
            if(r.CountryIDs.Length > 0)
                xml += "<CountryIDs>" + r.CountryIDs + "</CountryIDs>";
            if(r.Email.Length > 0)
                xml += "<Email>" + r.Email + "</Email>";
            if(r.Phone.Length > 0)
                xml += "<Phone>" + r.Phone + "</Phone>";
            if(r.Fax.Length > 0)
                xml += "<Fax>" + r.Fax + "</Fax>";
            if(r.Mobile.Length > 0)
                xml += "<Mobile>" + r.Mobile + "</Mobile>";
            if(r.Media.Length > 0)
                xml += "<Demo7>" + r.Media + "</Demo7>";
            if(r.FromDate.Length > 0)
                xml += "<StartDate>" + r.FromDate + "</StartDate>";
            if(r.ToDate.Length > 0)
                xml += "<EndDate>" + r.ToDate + "</EndDate>";
            if(r.Year.Length > 0)
                xml += "<Year>" + r.Year + "</Year>";
            if(r.Demo31.Length > 0)
                xml += "<Demo31>" + r.Demo31 + "</Demo31>";
            if(r.Demo32.Length > 0)
                xml += "<Demo32>" + r.Demo32 + "</Demo32>";
            if(r.Demo33.Length > 0)
                xml += "<Demo33>" + r.Demo33 + "</Demo33>";
            if(r.Demo34.Length > 0)
                xml += "<Demo34>" + r.Demo34 + "</Demo34>";
            if(r.Demo35.Length > 0)
                xml += "<Demo35>" + r.Demo35 + "</Demo35>";
            if(r.Demo36.Length > 0)
                xml += "<Demo36>" + r.Demo36 + "</Demo36>";
            if(r.IsMailable.Length > 0)
                xml += "<IsMailable>" + r.IsMailable + "</IsMailable>";
            if(r.ResponseIDs.Length > 0)
                xml += "<Responses>" + r.ResponseIDs + "</Responses>";
            if (r.WaveMail.Length > 0)
                xml += "<WaveMail>" + r.WaveMail + "</WaveMail>";
            if (r.AdHocXML.Length > 0)
                adhoc = r.AdHocXML;

            xml += "</Filters></XML>";
            rtnItem.Filters = xml;
            rtnItem.AdHocFilters = adhoc;

            return rtnItem;
        }

        public FrameworkUAD.Object.ReportingXML GetDefaultXMLFilter(int pubID, string catIDs, string transIDs)
        {
            FrameworkUAD.Object.ReportingXML rtnItem = new FrameworkUAD.Object.ReportingXML();
            string xml = "<XML><Filters>";
            string adhoc = "<XML></XML>";
            FrameworkUAD.Object.Reporting r = new FrameworkUAD.Object.Reporting();
            r.CategoryCodes = catIDs;
            r.TransactionCodes = transIDs;
            r.PublicationIDs = pubID.ToString();
            xml += "<ProductID>" + r.PublicationIDs + "</ProductID>";
            if (r.CategoryIDs.Length > 0)
                xml += "<CategoryType>" + r.CategoryIDs + "</CategoryType>";
            if (r.CategoryCodes.Length > 0)
                xml += "<CategoryCode>" + r.CategoryCodes + "</CategoryCode>";
            if (r.TransactionIDs.Length > 0)
                xml += "<TransactionType>" + r.TransactionIDs + "</TransactionType>";
            if (r.TransactionCodes.Length > 0)
                xml += "<TransactionCode>" + r.TransactionCodes + "</TransactionCode>";
            if (r.QSourceIDs.Length > 0)
                xml += "<QsourceIDs>" + r.QSourceIDs + "</QsourceIDs>";
            if (r.StateIDs.Length > 0)
                xml += "<StateIDs>" + r.StateIDs + "</StateIDs>";
            if (r.CountryIDs.Length > 0)
                xml += "<CountryIDs>" + r.CountryIDs + "</CountryIDs>";
            if (r.Email.Length > 0)
                xml += "<Email>" + r.Email + "</Email>";
            if (r.Phone.Length > 0)
                xml += "<Phone>" + r.Phone + "</Phone>";
            if (r.Fax.Length > 0)
                xml += "<Fax>" + r.Fax + "</Fax>";
            if (r.Mobile.Length > 0)
                xml += "<Mobile>" + r.Mobile + "</Mobile>";
            if (r.Media.Length > 0)
                xml += "<Demo7>" + r.Media + "</Demo7>";
            if (r.FromDate.Length > 0)
                xml += "<StartDate>" + r.FromDate + "</StartDate>";
            if (r.ToDate.Length > 0)
                xml += "<EndDate>" + r.ToDate + "</EndDate>";
            if (r.Year.Length > 0)
                xml += "<Year>" + r.Year + "</Year>";
            if (r.Demo31.Length > 0)
                xml += "<Demo31>" + r.Demo31 + "</Demo31>";
            if (r.Demo32.Length > 0)
                xml += "<Demo32>" + r.Demo32 + "</Demo32>";
            if (r.Demo33.Length > 0)
                xml += "<Demo33>" + r.Demo33 + "</Demo33>";
            if (r.Demo34.Length > 0)
                xml += "<Demo34>" + r.Demo34 + "</Demo34>";
            if (r.Demo35.Length > 0)
                xml += "<Demo35>" + r.Demo35 + "</Demo35>";
            if (r.Demo36.Length > 0)
                xml += "<Demo36>" + r.Demo36 + "</Demo36>";
            if (r.IsMailable.Length > 0)
                xml += "<IsMailable>" + r.IsMailable + "</IsMailable>";
            if (r.ResponseIDs.Length > 0)
                xml += "<Responses>" + r.ResponseIDs + "</Responses>";
            if (r.WaveMail.Length > 0)
                xml += "<WaveMail>" + r.WaveMail + "</WaveMail>";
            if (r.AdHocXML.Length > 0)
                adhoc = r.AdHocXML;

            xml += "</Filters></XML>";
            rtnItem.Filters = xml;
            rtnItem.AdHocFilters = adhoc;

            return rtnItem;
        }
    }
}
