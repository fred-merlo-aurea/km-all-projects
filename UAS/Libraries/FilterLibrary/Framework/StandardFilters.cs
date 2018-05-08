using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FilterControls.Framework
{
    public class StandardFilters : Filters
    {
        #region Workers & Responses
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catTypeW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> transTypeW = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> transW = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeW = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeW = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> regionW = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryW = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();

        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> ccTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> ccResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> qualResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> transTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> transResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>> regionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>> countryResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>>();
        #endregion

        public override Framework.Enums.Filters FilterType { get; set; }
        public override string Title { get; set; }
        public override ObservableCollection<FilterObject> Objects { get; set; }

        public StandardFilters()
        {
            this.Objects = new ObservableCollection<FilterObject>();
            this.FilterType = Enums.Filters.Standard;
            this.Title = this.FilterType.ToString();
            LoadCatCodes();
            LoadTransactionCodes();
            LoadQSource();
            LoadCountries();
            LoadStates();
            LoadQualificationDate();
            LoadWaveMail();
            LoadMedia();
        }

        private void CategoryCodeTypeChanged(object sender, PropertyChangedEventArgs e)
        {
            ListObject parent = sender as ListObject;
            foreach (ListObject child in Objects.Where(x => x.Name == Enums.FilterObjects.CategoryCode).Cast<ListFilterObject>().FirstOrDefault().Options.Where(x => x.ParentValue == parent.Value))
            {
                if (child.Selected != parent.Selected)
                    child.Selected = parent.Selected;
            }
        }
        private void CategoryCodeChanged(object sender, PropertyChangedEventArgs e)
        {
            ListObject cat = sender as ListObject;
            if (Objects.Where(x => x.Name == Enums.FilterObjects.CategoryCode).Cast<ListFilterObject>().FirstOrDefault().Options.Where(x => x.ParentValue == cat.ParentValue && x.Selected == true).Count() == 0)
            {
                foreach (ListObject parent in Objects.Where(x => x.Name == Enums.FilterObjects.CategoryCodeType).Cast<ListFilterObject>().FirstOrDefault().Options.Where(x => x.Value == cat.ParentValue))
                {
                    if (parent.Selected != false)
                        parent.Selected = false;
                }
            }
        }
        private void TransactionCodeTypeChanged(object sender, PropertyChangedEventArgs e)
        {
            ListObject parent = sender as ListObject;
            foreach (ListObject child in Objects.Where(x => x.Name == Enums.FilterObjects.TransactionCode).Cast<ListFilterObject>().FirstOrDefault().Options.Where(x => x.ParentValue == parent.Value))
            {
                child.Selected = parent.Selected;
            }
        }
        private void TransactionCodeChanged(object sender, PropertyChangedEventArgs e)
        {
            ListObject cat = sender as ListObject;
            if (Objects.Where(x => x.Name == Enums.FilterObjects.TransactionCode).Cast<ListFilterObject>().FirstOrDefault().Options.Where(x => x.ParentValue == cat.ParentValue && x.Selected == true).Count() == 0)
            {
                foreach (ListObject parent in Objects.Where(x => x.Name == Enums.FilterObjects.TransactionCodeType).Cast<ListFilterObject>().FirstOrDefault().Options.Where(x => x.Value == cat.ParentValue))
                {
                    if (parent.Selected != false)
                        parent.Selected = false;
                }
            }
        }
        private void CountryRegionChanged(object sender, PropertyChangedEventArgs e)
        {
            ListObject countryRegion = sender as ListObject;
            foreach (ListObject country in Objects.Where(x => x.Name == Enums.FilterObjects.Country).Cast<ListFilterObject>().FirstOrDefault().Options.Where(x => x.ParentValue == countryRegion.Value))
            {
                country.Selected = countryRegion.Selected;
            }
        }
        private void CountryChanged(object sender, PropertyChangedEventArgs e)
        {
            ListObject country = sender as ListObject;
            if (Objects.Where(x => x.Name == Framework.Enums.FilterObjects.Country).Cast<ListFilterObject>().FirstOrDefault().Options.Where(x => x.ParentValue == country.ParentValue && x.Selected == true).Count() == 0)
            {
                foreach (ListObject parent in Objects.Where(x => x.Name == Framework.Enums.FilterObjects.CountryRegions).Cast<ListFilterObject>().FirstOrDefault().Options.Where(x => x.Value == country.ParentValue))
                {
                    if (parent.Selected != false)
                        parent.Selected = false;
                }
            }
        }

        private void LoadCatCodes()
        {
            ccTypeResponse = catTypeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            if (Common.CheckResponse(ccTypeResponse.Result, ccTypeResponse.Status))
            {
                List<ListObject> options = new List<ListObject>();
                foreach (FrameworkUAD_Lookup.Entity.CategoryCodeType cct in ccTypeResponse.Result.OrderBy(x => x.CategoryCodeTypeName))
                {
                    ListObject lo = new ListObject(cct.CategoryCodeTypeName, cct.CategoryCodeTypeID.ToString());
                    lo.PropertyChanged += CategoryCodeTypeChanged;
                    options.Add(lo);
                }
                Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.CategoryCodeType, "Category", options));
            }
            ccResponse = catW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            if (Common.CheckResponse(ccResponse.Result, ccResponse.Status))
            {
                List<ListObject> options = new List<ListObject>();
                foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in ccResponse.Result.OrderBy(x => x.CategoryCodeValue))
                {
                    ListObject lo = new ListObject(cc.CategoryCodeValue + ". " + cc.CategoryCodeName, cc.CategoryCodeID.ToString(), cc.CategoryCodeTypeID.ToString());
                    lo.PropertyChanged += CategoryCodeChanged;
                    options.Add(lo);
                }
                Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.CategoryCode, "Category Codes", options));
            }
        }
        private void LoadTransactionCodes()
        {
            transTypeResponse = transTypeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            if (Common.CheckResponse(transTypeResponse.Result, transTypeResponse.Status))
            {
                List<ListObject> options = new List<ListObject>();
                foreach (FrameworkUAD_Lookup.Entity.TransactionCodeType tct in transTypeResponse.Result.OrderBy(x => x.TransactionCodeTypeName))
                {
                    ListObject lo = new ListObject(tct.TransactionCodeTypeName, tct.TransactionCodeTypeID.ToString());
                    lo.PropertyChanged += TransactionCodeTypeChanged;
                    options.Add(lo);
                }

                Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.TransactionCodeType, "Transaction", options));
            }
            transResponse = transW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            if (Common.CheckResponse(transResponse.Result, transResponse.Status))
            {
                List<ListObject> options = new List<ListObject>();
                foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in transResponse.Result.OrderBy(x => x.TransactionCodeValue))
                {
                    ListObject lo = new ListObject(tc.TransactionCodeValue + ". " + tc.TransactionCodeName, tc.TransactionCodeID.ToString(), tc.TransactionCodeTypeID.ToString());
                    lo.PropertyChanged += TransactionCodeChanged;
                    options.Add(lo);
                }

                Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.TransactionCode, "Transaction Codes", options));
            }
        }
        private void LoadQSource()
        {
            codeResponse = codeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source);
            if (Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                List<ListObject> options = new List<ListObject>();
                codeResponse.Result.OrderBy(x => x.DisplayOrder).ToList().ForEach(x => options.Add(new ListObject(x.DisplayName, x.CodeId.ToString())));
                Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.QualificationSource, "Qualification Source", options));
            }
        }
        private void LoadStates()
        {
            regionResponse = regionW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            if (Common.CheckResponse(regionResponse.Result, regionResponse.Status))
            {
                List<ListObject> options = new List<ListObject>();
                regionResponse.Result.OrderBy(x => x.RegionCode).ToList().ForEach(x => options.Add(new ListObject(x.RegionCode, x.RegionID.ToString())));
                Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.RegionCode, "State", options));
            }
        }
        private void LoadCountries()
        {
            countryResponse = countryW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            if (Common.CheckResponse(countryResponse.Result, countryResponse.Status))
            {
                List<ListObject> options = new List<ListObject>();
                List<string> countries = new List<string>();
                countries = countryResponse.Result.Where(x => x.Area != null && x.Area != String.Empty).OrderBy(x => x.Area).Select(x => x.Area.ToUpper().Trim()).Distinct().ToList();
                foreach (string c in countries)
                {
                    ListObject lo = new ListObject(c, c);
                    lo.PropertyChanged += CountryRegionChanged;
                    options.Add(lo);
                }

                Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.CountryRegions, "Country Regions", options));

                options = new List<ListObject>();
                foreach (FrameworkUAD_Lookup.Entity.Country c in countryResponse.Result.Where(x => x.CountryID != 3 && x.CountryID != 4).OrderBy(x => x.SortOrder))
                {
                    string area = string.Empty;
                    if (c.Area != null)
                        area = c.Area.Trim();
                    ListObject lo = new ListObject(c.ShortName, c.CountryID.ToString(), area);
                    lo.PropertyChanged += CountryChanged;
                    options.Add(lo);
                }

                Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.Country, "Country", options));
            }
        }
        private void LoadQualificationDate()
        {
            List<ListObject> years = new List<ListObject>();
            for (int i = 0; i < 6; i++) years.Add(new ListObject(i.ToString(), i.ToString()));
            Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.Year, "Year", years));
            Objects.Add(new RangeFilterObject(this.FilterType, Enums.FilterObjects.QualificationDate, "Qualification Date"));
        }
        private void LoadWaveMail()
        {
            List<ListObject> options = new List<ListObject>();
            options.Add(new ListObject("Is Wave Mailed", "Is Wave Mailed"));
            options.Add(new ListObject("Is Not Wave Mailed", "Is Not Wave Mailed"));
            Objects.Add(new ComboFilterObject(this.FilterType, Enums.FilterObjects.WaveMail, "Wave Mailing", options));
        }

        private void LoadMedia()
        {
            List<ListObject> options = new List<ListObject>();
            options.Add(new ListObject("Print", "A"));
            options.Add(new ListObject("Digital", "B"));
            options.Add(new ListObject("Both", "C"));
            Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.Media, "Media", options));
        }
    }
}
