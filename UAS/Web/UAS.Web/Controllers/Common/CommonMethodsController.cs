using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KM.Common;
using KM.Common.Functions;
using UAS.Web.Models.UAD.Filter;
using UAS.Web.Service.Filter;

namespace UAS.Web.Controllers.Common
{
    public class CommonMethodsController : BaseController
    {
        
        
        public CommonMethodsController()
        {
           
        }
        public JsonResult GetRegions(int? countryID = 1)
        {
            List<FrameworkUAD_Lookup.Entity.Region> regions = new FrameworkUAD_Lookup.BusinessLogic.Region().Select();
          
            List<SelectListItem> regionSelectList = new List<SelectListItem>();
            regionSelectList.Add(new SelectListItem() { Text = "", Value = "" });
            if (countryID != 1 && countryID != 2 && countryID != 429)
                regions = regions.Where(x => x.RegionCode == "FO").ToList();
            else
                regions = regions.Where(x => x.CountryID == countryID).ToList();

            regions.ForEach(c => regionSelectList.Add(new SelectListItem() { Text = c.RegionName, Value = c.RegionCode }));

            return Json(regionSelectList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCountries()
        {
            List<FrameworkUAD_Lookup.Entity.Country> countries =  new FrameworkUAD_Lookup.BusinessLogic.Country().Select();
            var countryList = countries.OrderBy(x => x.SortOrder).ToList();
            List<SelectListItem> countrySelectList = new List<SelectListItem>();
            countrySelectList.Add(new SelectListItem() { Text = "", Value = "" });
            countryList = countryList.Where(x => x.SortOrder != 0).OrderByDescending(o => o.CountryID == 1)
                .ThenByDescending(o => o.CountryID == 2)
                .ThenByDescending(o => o.CountryID == 429)
                .ThenBy(x => x.ShortName).ToList();
            countryList.ForEach(c => countrySelectList.Add(new SelectListItem() { Text = c.ShortName, Value = c.CountryID.ToString() }));
            return Json(countrySelectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAddressTypes()
        {
            List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new FrameworkUAD_Lookup.BusinessLogic.CodeType().Select();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();

            if (CodeList != null)
                codeList = CodeList;
            else
                codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select();
            int addrType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Address.ToString())).CodeTypeId;
            List<FrameworkUAD_Lookup.Entity.Code> addressTypeList = codeList.Where(x => x.CodeTypeId == addrType).OrderBy(x => x.DisplayOrder).ToList();

            List<SelectListItem> addTypeSelectList = new List<SelectListItem>();
            addTypeSelectList.Add(new SelectListItem() { Text = "", Value = "" });
            addressTypeList.ForEach(c => addTypeSelectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() }));
            return Json(addTypeSelectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCircProducts(int clientId=0)
        {
            List<SelectListItem> productSelectList = new List<SelectListItem>();
            productSelectList.Add(new SelectListItem() { Text = "Product", Value = "Product" });
            KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
            if (CurrentClient != null && CurrentClient.ClientID==clientId && clientId>0)
                _client = CurrentClient;
            else if(clientId>0)
                _client = new KMPlatform.BusinessLogic.Client().Select(clientId);
            else
                _client = new KMPlatform.BusinessLogic.Client().Select(CurrentClientID);
            List<FrameworkUAD.Entity.Product> productListUAD = new FrameworkUAD.BusinessLogic.Product().Select(_client.ClientConnections);
            var prodList = productListUAD.Where(x => x.IsCirc == true).OrderBy(x => x.PubCode).ToList();
            prodList.ForEach(c => productSelectList.Add(new SelectListItem() { Text = c.PubCode, Value = c.PubID.ToString() }));
            return Json(productSelectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCircProductsNoDefault(int clientId = 0)
        {
            List<SelectListItem> productSelectList = new List<SelectListItem>();            
            KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
            if (CurrentClient != null && CurrentClient.ClientID == clientId && clientId > 0)
                _client = CurrentClient;
            else if (clientId > 0)
                _client = new KMPlatform.BusinessLogic.Client().Select(clientId);
            else
                _client = new KMPlatform.BusinessLogic.Client().Select(CurrentClientID);
            List<FrameworkUAD.Entity.Product> productListUAD = new FrameworkUAD.BusinessLogic.Product().Select(_client.ClientConnections);
            var prodList = productListUAD.Where(x => x.IsCirc == true).OrderBy(x => x.PubCode).ToList();
            prodList.ForEach(c => productSelectList.Add(new SelectListItem() { Text = c.PubCode, Value = c.PubID.ToString() }));
            return Json(productSelectList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllProductsForTransformation()
        {
            List<SelectListItem> productSelectList = new List<SelectListItem>();
            productSelectList.Add(new SelectListItem() { Text = "-- Select --", Value = "-1" });
            productSelectList.Add(new SelectListItem() { Text = "ALL", Value = "0" });

            KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
            if (CurrentClient != null)
                _client = CurrentClient;
            else
                _client = new KMPlatform.BusinessLogic.Client().Select(CurrentClientID);

            List<FrameworkUAD.Entity.Product> productListUAD = new FrameworkUAD.BusinessLogic.Product().Select(_client.ClientConnections);
            var prodList = productListUAD.OrderBy(x => x.PubCode).Where(x => x.IsActive).ToList();
            prodList.ForEach(c => productSelectList.Add(new SelectListItem() { Text = c.PubCode, Value = c.PubID.ToString() }));            

            return Json(productSelectList, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult GetTransformationTypes()
        {
            List<FrameworkUAD_Lookup.Entity.Code> transformationTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            transformationTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            transformationTypes.RemoveAll(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Transform.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase));
            transformationTypes = transformationTypes.OrderBy(x => x.DisplayName).ToList();

            List<SelectListItem> transformationTypeSelectList = new List<SelectListItem>();
            transformationTypeSelectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            transformationTypes.ForEach(c => transformationTypeSelectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() }));
            return Json(transformationTypeSelectList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransformationDelimiters()
        {
            List<string> delimiters = new List<string>();
            foreach (Enums.ColumnDelimiter dl in (Enums.ColumnDelimiter[]) Enum.GetValues(typeof(Enums.ColumnDelimiter)))
            {
                delimiters.Add(dl.ToString().Replace("_", " "));
            }

            List<SelectListItem> delimiterList = new List<SelectListItem>();            
            delimiters.ForEach(c => delimiterList.Add(new SelectListItem() { Text = c, Value = c }));
            return Json(delimiterList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDatabaseFileTypes()
        {
            List<FrameworkUAD_Lookup.Entity.Code> databaseFileTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            databaseFileTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File);
            databaseFileTypes = databaseFileTypes.OrderBy(x => x.DisplayName).ToList();

            List<SelectListItem> fileTypeSelectList = new List<SelectListItem>();
            fileTypeSelectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            databaseFileTypes.ForEach(c => fileTypeSelectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() }));
            return Json(fileTypeSelectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDatabaseFileTypesNoDefault()
        {
            List<FrameworkUAD_Lookup.Entity.Code> databaseFileTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            databaseFileTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File);
            databaseFileTypes = databaseFileTypes.OrderBy(x => x.DisplayName).ToList();

            List<SelectListItem> fileTypeSelectList = new List<SelectListItem>();            
            databaseFileTypes.ForEach(c => fileTypeSelectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() }));
            return Json(fileTypeSelectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUadServiceFeatures()
        {
            List<KMPlatform.Entity.ServiceFeature> featuresList = new List<KMPlatform.Entity.ServiceFeature>();
            featuresList = GetFeatures(KMPlatform.Enums.Services.UADFILEMAPPER).OrderBy(x => x.SFName).ToList();

            List<SelectListItem> serviceFeatureSelectList = new List<SelectListItem>();
            serviceFeatureSelectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            featuresList.ForEach(c => serviceFeatureSelectList.Add(new SelectListItem() { Text = c.SFName, Value = c.ServiceFeatureID.ToString() }));
            return Json(serviceFeatureSelectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUadServiceFeaturesNoDefault()
        {
            List<KMPlatform.Entity.ServiceFeature> featuresList = new List<KMPlatform.Entity.ServiceFeature>();
            featuresList = GetFeatures(KMPlatform.Enums.Services.UADFILEMAPPER).OrderBy(x => x.SFName).ToList();

            List<SelectListItem> serviceFeatureSelectList = new List<SelectListItem>();            
            featuresList.ForEach(c => serviceFeatureSelectList.Add(new SelectListItem() { Text = c.SFName, Value = c.ServiceFeatureID.ToString() }));
            return Json(serviceFeatureSelectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCircServiceFeatures()
        {
            List<KMPlatform.Entity.ServiceFeature> featuresList = new List<KMPlatform.Entity.ServiceFeature>();
            featuresList = GetFeatures(KMPlatform.Enums.Services.CIRCFILEMAPPER).OrderBy(x => x.SFName).ToList();

            List<SelectListItem> serviceFeatureSelectList = new List<SelectListItem>();
            serviceFeatureSelectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            featuresList.ForEach(c => serviceFeatureSelectList.Add(new SelectListItem() { Text = c.SFName, Value = c.ServiceFeatureID.ToString() }));
            return Json(serviceFeatureSelectList, JsonRequestBehavior.AllowGet);
        }
        public List<KMPlatform.Entity.ServiceFeature> GetFeatures(KMPlatform.Enums.Services servEnum)
        {
            List<KMPlatform.Entity.ServiceFeature> featuresList = new List<KMPlatform.Entity.ServiceFeature>();
            KMPlatform.BusinessLogic.Service serviceWorker = new KMPlatform.BusinessLogic.Service();
            KMPlatform.Entity.Service myService = serviceWorker.Select(servEnum, true); 
            if (myService != null)
            {                
                KMPlatform.BusinessLogic.ServiceFeature serviceFeatureWorker = new KMPlatform.BusinessLogic.ServiceFeature();
                List<KMPlatform.Entity.ServiceFeature> AllFeatures = serviceFeatureWorker.Select();
                List<KMPlatform.Entity.ServiceFeature> currentClientFeatures = new List<KMPlatform.Entity.ServiceFeature>();
                KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
                if (CurrentClient != null)
                    _client = CurrentClient;
                else
                    _client = new KMPlatform.BusinessLogic.Client().Select(CurrentClientID);

                foreach (KMPlatform.Entity.ServiceFeature ss in myService.ServiceFeatures)
                {
                    if (myService.ServiceFeatures.Count > 0 && ss.IsEnabled)
                    {
                        //Outside User
                        if (_client.ClientID > 1 && ss.KMAdminOnly == false)
                            featuresList.Add(AllFeatures.SingleOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));

                        //KM Employee
                        if (_client.ClientID == 1)
                            featuresList.Add(AllFeatures.SingleOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));
                    }
                }
            }

            return featuresList;
        }
        public JsonResult GetQDateFormats()
        {
            Dictionary<string, string> dates = new Dictionary<string, string>();
            foreach (DateFormat df in (DateFormat[]) Enum.GetValues(typeof(DateFormat)))
            {
                dates.Add(df.ToString(), df.ToString());
            }

            List<SelectListItem> datesSelectList = new List<SelectListItem>();                        
            foreach (KeyValuePair<string, string> kvp in dates.OrderBy(x => x.Value))
            {
                datesSelectList.Add(new SelectListItem() { Text = kvp.Value, Value = kvp.Key });
            }
            return Json(datesSelectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSourceFileDelimiters()
        {
            Dictionary<string, string> delimiters = new Dictionary<string, string>();
            foreach (Enums.ColumnDelimiter dl in (Enums.ColumnDelimiter[]) Enum.GetValues(typeof(Enums.ColumnDelimiter)))
            {
                delimiters.Add(dl.ToString().Replace("_", " "), dl.ToString().Replace("_", " "));
            }

            List<SelectListItem> delimiterSelectList = new List<SelectListItem>();
            foreach (KeyValuePair<string, string> kvp in delimiters.OrderBy(x => x.Value))
            {
                delimiterSelectList.Add(new SelectListItem() { Text = kvp.Value, Value = kvp.Key });
            }
            return Json(delimiterSelectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetYesOrNo()
        {
            List<SelectListItem> yesnoSelectList = new List<SelectListItem>();
            yesnoSelectList.Add(new SelectListItem() { Text = "Yes", Value = "True" });
            yesnoSelectList.Add(new SelectListItem() { Text = "No", Value = "False" });
            return Json(yesnoSelectList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult HasOpenBatches()
        {
            FrameworkUAD.BusinessLogic.Batch batchWorker = new FrameworkUAD.BusinessLogic.Batch();
            List<FrameworkUAD.Entity.Batch> openBatches = new List<FrameworkUAD.Entity.Batch>();
            //KMPlatform.Entity.Client CurrentClient = new KMPlatform.BusinessLogic.Client().Select(24);
            openBatches = batchWorker.Select(CurrentUser.UserID, true, CurrentClient.ClientConnections);
            if (openBatches != null && openBatches.Count() > 0)
                return Json(true);
            else
                return Json(false);
        }
        public JsonResult CloseBatches()
        {
            FrameworkUAD.BusinessLogic.Batch batchWorker = new FrameworkUAD.BusinessLogic.Batch();
            List<FrameworkUAD.Entity.Batch> openBatches = new List<FrameworkUAD.Entity.Batch>();
            //KMPlatform.Entity.Client CurrentClient = new KMPlatform.BusinessLogic.Client().Select(24);
            int numberOfBatches = 0;
            if (CurrentClient != null)
            {
                openBatches = batchWorker.Select(CurrentUser.UserID, true, CurrentClient.ClientConnections);
                if (openBatches != null && openBatches.Count() > 0)
                {
                    numberOfBatches = openBatches.Count();
                    batchWorker.CloseBatches(CurrentUser.UserID, CurrentClient.ClientConnections);
                }
            }
            return Json(new { Result = numberOfBatches });
        }

        #region Getlists Ajax Calls
        //Get All Area Filter
       public JsonResult GetAllAreaFilters()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            var countries = FilterService.GetAllAreas();
            var distinctArea = countries.OrderBy(x => x.Area).Where(y => y.Area != "").Select(x => x.Area).Distinct().ToList();
            distinctArea.ForEach(x => selectList.Add(new SelectListItem() { Text = x, Value = x }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get All Countries Filter
       public JsonResult GetAllCountryFilters()
        {
            var countries = FilterService.GetAllCountries();
            var countryList = countries.OrderBy(x => x.SortOrder).ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            countryList = countryList.Where(x => x.SortOrder != 0).OrderByDescending(o => o.CountryID == 1)
                .ThenByDescending(o => o.CountryID == 2)
                .ThenByDescending(o => o.CountryID == 429)
                .ThenBy(x => x.ShortName).ToList();
            countryList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.ShortName, Value = c.CountryID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get Countries Filter By Area
       public JsonResult GetCountryFiltersByAreas(string[] areaNames = null)
        {
            var countries = FilterService.GetAllCountries();
            if (areaNames != null && areaNames.Count() > 0)
            {
                countries = countries.Select(x => x).Where(x => areaNames.Contains(x.Area)).ToList();
            }
            var countryList = countries.OrderBy(x => x.SortOrder).ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            countryList = countryList.Where(x => x.SortOrder != 0).OrderByDescending(o => o.CountryID == 1)
                .ThenByDescending(o => o.CountryID == 2)
                .ThenByDescending(o => o.CountryID == 429)
                .ThenBy(x => x.ShortName).ToList();
            countryList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.ShortName, Value = c.CountryID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get All RegionGroups Filter
       public JsonResult GetAllRegionGroupFilters()
        {
            var regionGroups = FilterService.GetAllRegionGroups();
            List<SelectListItem> selectList = new List<SelectListItem>();
            regionGroups = regionGroups.OrderBy(x => x.Sortorder).ToList();
            regionGroups.ForEach(c => selectList.Add(new SelectListItem() { Text = c.RegionGroupName, Value = c.RegionGroupID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get All RegionsFilters
       public JsonResult GetAllRegionFilters()
        {
            var regions = FilterService.GetAllRegions();
            List<SelectListItem> selectList = new List<SelectListItem>();
            regions.ForEach(c => selectList.Add(new SelectListItem() { Text = c.RegionName, Value = c.RegionID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get Regions By Selected RegionGroups
       public JsonResult GetAllRegionFiltersByRegionGroups(string  regionGroupIDs = "")
        {
            var regionGroupIds = regionGroupIDs.Split(',');
            var list = new List<int>(regionGroupIds.Length);
            regionGroupIds.ToList().ForEach(i => list.Add(Convert.ToInt32(i)));
            List<SelectListItem> selectList = new List<SelectListItem>();
            var regions = new List<FrameworkUAD_Lookup.Entity.Region>();
            if (regionGroupIDs == null || regionGroupIDs.Count() == 0)
            {
                regions = FilterService.GetAllRegions();
            }
            else
            {
                regions = FilterService.GetAllRegionByRegionGroups(list);
            }
            regions.ForEach(c => selectList.Add(new SelectListItem() { Text = c.RegionName, Value = c.RegionID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get Email Status Filter
       public JsonResult GetAllEmailStatuses()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get DC Filter
       public JsonResult GetDCFileFilter(int currentclientID)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<FrameworkUAS.Entity.SourceFile> lsf = new FrameworkUAS.BusinessLogic.SourceFile().Select(currentclientID, false);
            List<FrameworkUAS.Entity.DataCompareRun> ldcr = new FrameworkUAS.BusinessLogic.DataCompareRun().SelectForClient(currentclientID);
            var dcFileFilterList = (from s in lsf
                                    join d in ldcr on s.SourceFileID equals d.SourceFileId
                                    orderby d.DateCreated
                                    select new DCFileFilter { FileName = s.FileName + "_" + d.DateCreated, ProcessCode = d.ProcessCode }
                       ).ToList();
            dcFileFilterList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.FileName, Value = c.ProcessCode.ToString() }));

            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get All Brands Filter
       public JsonResult GetAllActiveBrands()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            var brands = FilterService.GetAllActiveBrands(CurrentClient.ClientConnections);
            brands.ForEach(c => selectList.Add(new SelectListItem() { Text = c.BrandName, Value = c.BrandID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get Brand Filter by User
       public JsonResult GetAllActiveBrandsByUser()
        {
            var brands = FilterService.GetAllActiveBrandsByUser(CurrentUser.UserID, CurrentClient.ClientConnections);
            List<SelectListItem> selectList = new List<SelectListItem>();
            brands.ForEach(c => selectList.Add(new SelectListItem() { Text = c.BrandName, Value = c.BrandID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get All Market Filter
       public JsonResult GetAllMarket()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            var markets = FilterService.GetAllMarket(CurrentClient.ClientConnections);
            markets.ForEach(c => selectList.Add(new SelectListItem() { Text = c.MarketName, Value = c.MarketID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        //Get All Market Filter By Brands
       public JsonResult GetAllMarketByBrands(int brandId)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            var markets = FilterService.GetMarketByBrand(CurrentClient.ClientConnections, brandId);
            markets.ForEach(c => selectList.Add(new SelectListItem() { Text = c.MarketName, Value = c.MarketID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);

        }
        //Get All Product Types
        public List<PubTypeFilter> GetAllProductTypes()
        {
            var productTypesFilterList = new List<PubTypeFilter>();
            var p = FilterService.GetAllProducts(CurrentClient.ClientConnections);
            var productTypes = FilterService.GetAllProductTypes(CurrentClient.ClientConnections);
            productTypes.ForEach(x => productTypesFilterList.Add(
                 new PubTypeFilter()
                 {
                     ColumnReference = x.ColumnReference,
                     IsActive = x.IsActive,
                     PubTypeDisplayName = x.PubTypeDisplayName,
                     PubTypeID = x.PubTypeID,
                     SortOrder = x.SortOrder,
                     PubList = p.Select(y => y).Where(y => y.PubTypeID == x.PubTypeID).ToList()

                 }));

            return productTypesFilterList;
        }

        //Get All Product Types by Brand
        public List<PubTypeFilter> GetAllProductTypesByBrand(int brandID)
        {
            var productTypesFilterList = new List<PubTypeFilter>();
            var p = FilterService.GetAllProducts(CurrentClient.ClientConnections);
            var productTypes = FilterService.GetAllProductTypesByBrand(CurrentClient.ClientConnections, brandID);
            productTypes.ForEach(x => productTypesFilterList.Add(
                 new PubTypeFilter()
                 {
                     ColumnReference = x.ColumnReference,
                     IsActive = x.IsActive,
                     PubTypeDisplayName = x.PubTypeDisplayName,
                     PubTypeID = x.PubTypeID,
                     SortOrder = x.SortOrder,
                     PubList = p.Select(y => y).Where(y => y.PubTypeID == x.PubTypeID).ToList()

                 }));

            return productTypesFilterList;
        }

        //Get All MasterGroups 
       public JsonResult GetAllMasterGroup()
        {
            var mastergroups = FilterService.GetAllMasterGroups(CurrentClient.ClientConnections);
            List<SelectListItem> selectList = new List<SelectListItem>();
            mastergroups.ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.MasterGroupID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);

        }
        //Get All MasterGroups By Brands
       public JsonResult GetAllMasterGroupByBrand(int brandId)
        {
            var mastergroups = FilterService.GetAllMasterGroupsByBrand(CurrentClient.ClientConnections, brandId);
            List<SelectListItem> selectList = new List<SelectListItem>();
            mastergroups.ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.MasterGroupID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQSourceList()
        {
            List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new FrameworkUAD_Lookup.BusinessLogic.CodeType().Select();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select();
            int qSourceType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source.ToString().Replace("_", " "))).CodeTypeId;
            var qSourceList = codeList.Where(x => x.CodeTypeId == qSourceType).OrderBy(x => x.DisplayOrder).ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            qSourceList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMediaTypeList()
        {
            List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new FrameworkUAD_Lookup.BusinessLogic.CodeType().Select();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select();
            int mediaType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Deliver.ToString())).CodeTypeId;
            var mediaTypeList = codeList.Where(x => x.CodeTypeId == mediaType).OrderBy(x => x.DisplayOrder).ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            mediaTypeList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
