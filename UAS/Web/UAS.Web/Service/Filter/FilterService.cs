using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UAS.Web.Models.UAD.Filter;
using System.Data.SqlClient;
using System.Data;
using FrameworkUAD.Entity;
using KMPlatform.Object;

namespace UAS.Web.Service.Filter
{
    public class FilterService
    {
        
        public FilterService()
        {
            
        }
        //Get All FilterCategories
        public static List<FrameworkUAD.Entity.FilterCategory> GetAllFilterCategories(KMPlatform.Object.ClientConnections client)
        {
            return new FrameworkUAD.BusinessLogic.FilterCategory().Select(client);
        }

        //Get All Active Brands
        public static List<FrameworkUAD.Entity.Brand> GetAllActiveBrands(KMPlatform.Object.ClientConnections client)
        {
            return new FrameworkUAD.BusinessLogic.Brand().Select(client);
        }
        //Get Active Brands by User
        public static List<FrameworkUAD.Entity.Brand> GetAllActiveBrandsByUser(int userid,KMPlatform.Object.ClientConnections client)
        {
            return new FrameworkUAD.BusinessLogic.Brand().SelectByUserID(userid,client);
        }
        //Get All Markets
        public static List<FrameworkUAD.Entity.Market> GetAllMarket(KMPlatform.Object.ClientConnections client)
        {

            List<FrameworkUAD.Entity.Market> m = new FrameworkUAD.BusinessLogic.Market().Select(client);
            return m;
        }

        //Get Markets By Brand
        public static List<FrameworkUAD.Entity.Market> GetMarketByBrand(KMPlatform.Object.ClientConnections client,int brandID=0)
        {
            var markets = GetAllMarket(client).FindAll(x => x.BrandID == brandID);
            return markets;
        }

        //Get All Products
        public static List<FrameworkUAD.Entity.Product> GetAllProducts(KMPlatform.Object.ClientConnections client)
        {

            List<FrameworkUAD.Entity.Product> m = new FrameworkUAD.BusinessLogic.Product().Select(client);
            return m;
        }
        public static List<FrameworkUAD.Entity.Product> GetAllSearchEnabledProducts(KMPlatform.Object.ClientConnections client)
        {

            List<FrameworkUAD.Entity.Product> m = new FrameworkUAD.BusinessLogic.Product().Select(client).Where(x => x.EnableSearching == true).OrderBy(x => x.PubName).ToList();
            return m;
        }
        public static List<FrameworkUAD.Entity.Product> GetAllCircProducts(KMPlatform.Object.ClientConnections client)
        {

            List<FrameworkUAD.Entity.Product> m = new FrameworkUAD.BusinessLogic.Product().Select(client).Where(x => x.IsCirc == true && x.IsActive==true).OrderBy(x => x.PubName).ToList();
            return m;
        }
        public static List<FrameworkUAD.Entity.ResponseGroup> GetAllResponseGroupsByPubID(KMPlatform.Object.ClientConnections client, int pubid)
        {
            var responseGroup = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(client);
            if (responseGroup != null && responseGroup.Count > 0)
            {
                responseGroup = responseGroup.Where(x => x.PubID == pubid).ToList();
            }
            return responseGroup;
        }

        internal static List<FrameworkUAD.Entity.CodeSheet> GetAllResponsesByResonseGroupByPubID(ClientConnections clientConnections, int pubID)
        {
            var responses = new FrameworkUAD.BusinessLogic.CodeSheet().Select(pubID,clientConnections);

            return responses;
        }
        //Get Products by Brand
        public static List<FrameworkUAD.Entity.Product> GetAllSearchEnabledProductsByBrand(KMPlatform.Object.ClientConnections client,int brandID)
        {
            List<FrameworkUAD.Entity.Product> m = new FrameworkUAD.BusinessLogic.Product().SelectByBrandID(client, brandID);
            return m;
        }

        //Get All ProductTypes
        public static List<FrameworkUAD.Entity.ProductTypes> GetAllProductTypes(KMPlatform.Object.ClientConnections client)
        {

           var productTypes = new FrameworkUAD.BusinessLogic.ProductTypes().Select(client);
            return productTypes;
        }
        //Get ProductTypes by Brand
        public static List<FrameworkUAD.Entity.ProductTypes> GetAllProductTypesByBrand(KMPlatform.Object.ClientConnections client, int BrandID=0)
        {
            var productTypes = new FrameworkUAD.BusinessLogic.ProductTypes().SelectByBrand(BrandID, client);
            return productTypes;
        }
        


        //Get All MasterGroups
        public static List<FrameworkUAD.Entity.MasterGroup> GetAllMasterGroups(KMPlatform.Object.ClientConnections client)
        {
            var mastergroups = new FrameworkUAD.BusinessLogic.MasterGroup().Select(client);
            return mastergroups;
        }

        //Get MasterGroups by Brand
        public static List<FrameworkUAD.Entity.MasterGroup> GetAllMasterGroupsByBrand(KMPlatform.Object.ClientConnections client, int BrandID = 0)
        {
            var mastergroups = new FrameworkUAD.BusinessLogic.MasterGroup().SelectByBrandID(BrandID, client);
            return mastergroups;
        }
        //Get All Areas
        public static List<FrameworkUAD_Lookup.Entity.Country> GetAllAreas()
        {
            var countries = new FrameworkUAD_Lookup.BusinessLogic.Country().Select();
            return countries;
        }
        //Get All Countries
        public static List<FrameworkUAD_Lookup.Entity.Country> GetAllCountries()
        {
            var countries = new List<FrameworkUAD_Lookup.Entity.Country>();
            countries = new FrameworkUAD_Lookup.BusinessLogic.Country().Select().ToList();
            return countries;
        }
        //Get All Regions
        public static List<FrameworkUAD_Lookup.Entity.Region> GetAllRegions()
        {
            List<FrameworkUAD_Lookup.Entity.Region> regions = new List<FrameworkUAD_Lookup.Entity.Region>();
            regions = new FrameworkUAD_Lookup.BusinessLogic.Region().Select();
            return regions;
        }
        //Get Regions By Selected RegionGroups
        public static List<FrameworkUAD_Lookup.Entity.Region> GetAllRegionByRegionGroups(List<int> regionGroupIDs = null)
        {
            var regions = new List<FrameworkUAD_Lookup.Entity.Region>();
            if (regionGroupIDs == null || regionGroupIDs.Count() == 0)
            {
                regions = new FrameworkUAD_Lookup.BusinessLogic.Region().Select();
            }
            else
            {
                regions = new FrameworkUAD_Lookup.BusinessLogic.Region().Select().Select(x => x).Where(x => regionGroupIDs.Contains(x.RegionGroupID)).ToList();
            }
            return regions;
        }
        //Get All RegionGroups
        public static List<FrameworkUAD_Lookup.Entity.RegionGroup> GetAllRegionGroups()
        {
            var regionGroups = new FrameworkUAD_Lookup.BusinessLogic.RegionGroup().Select();
            return regionGroups;
        }

        //Get all responseGroup for Product
        public static List<FrameworkUAD.Entity.ResponseGroup> GetAllResponseGroup(int pubID, KMPlatform.Object.ClientConnections client)
        {
            var responseGroup = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(pubID, client);
            return responseGroup;
        }

    }

}