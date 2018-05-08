using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        #region Global Circ Objects
        public static List<FrameworkUAD_Lookup.Entity.CategoryCode> CategoryCodes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.TransactionCode> TransactionCodes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> SubscriptionStatuses { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix> SubscriptionStatusMatrices { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.Region> Regions { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.Country> Countries { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.Code> Codes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.Code> QSourceCodes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.Code> AddressCodes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.Code> Par3CCodes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.Code> MarketingCodes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.CodeType> CodeTypes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.Action> Actions { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.CategoryCodeType> CategoryCodeTypes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.TransactionCodeType> TransactionCodeTypes { get; set; }
        public static List<FrameworkUAD_Lookup.Entity.ZipCode> ZipCodes { get; set; }
        public static bool IsLoaded { get; set; }
        #endregion
        #region Workers
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> tCodeW = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> tCodeTypeW = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catCodeW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatus> subStatusW = FrameworkServices.ServiceClient.UAD_Lookup_SubscriptionStatusClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatusMatrix> subStatusMatrixW = FrameworkServices.ServiceClient.UAD_Lookup_SubscriptionStatusMatrixClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> actionW = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> regionW = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryW = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeW = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catCodeTypeW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeW = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IZipCode> zipCodeW = FrameworkServices.ServiceClient.UAD_Lookup_ZipCodeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productW = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> productSubscriptionW = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        #endregion
        #region Service Response
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Country>> countryResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Region>> regionResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> transResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> transTypeResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> catResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>> actionResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>> sstResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>> ssmResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> catTypeResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.ZipCode>> zipResponse;
        #endregion
        public Home()
        {
            InitializeComponent();
            IsLoaded = false;
            busy.IsBusy = true;
            busy.BusyContent = "Loading Circulation Data...";
            busy.IsIndeterminate = true;
            var ui = SynchronizationContext.Current;
            new Thread(() => LoadData(0, ui)).Start();
        }

        //We Load various Circulation data and then use it across the different Circ modules.
        private void LoadData(int att, SynchronizationContext ui)
        {
            int attempts = att;
            Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

            try
            {
                countryResponse = countryW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(countryResponse.Result, countryResponse.Status))
                    Countries = countryResponse.Result;

                regionResponse = regionW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(regionResponse.Result, regionResponse.Status))
                    Regions = regionResponse.Result;

                codeResponse = codeW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                    Codes = codeResponse.Result;

                transResponse = tCodeW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(transResponse.Result, transResponse.Status))
                    TransactionCodes = transResponse.Result;

                transTypeResponse = tCodeTypeW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(transTypeResponse.Result, transTypeResponse.Status))
                    TransactionCodeTypes = transTypeResponse.Result;

                catResponse = catCodeW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(catResponse.Result, catResponse.Status))
                    CategoryCodes = catResponse.Result;

                catTypeResponse = catCodeTypeW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(catTypeResponse.Result, catTypeResponse.Status))
                    CategoryCodeTypes = catTypeResponse.Result;

                actionResponse = actionW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(actionResponse.Result, actionResponse.Status))
                    Actions = actionResponse.Result;

                sstResponse = subStatusW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(sstResponse.Result, sstResponse.Status))
                    SubscriptionStatuses = sstResponse.Result;

                ssmResponse = subStatusMatrixW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(ssmResponse.Result, ssmResponse.Status))
                    SubscriptionStatusMatrices = ssmResponse.Result;

                codeTypeResponse = codeTypeW.Proxy.Select(accessKey);
                if (Helpers.Common.CheckResponse(codeTypeResponse.Result, codeTypeResponse.Status))
                    CodeTypes = codeTypeResponse.Result;

                //zipResponse = zipCodeW.Proxy.Select(accessKey);
                //if (Helpers.Common.CheckResponse(zipResponse.Result, zipResponse.Status))
                //    ZipCodes = zipResponse.Result;

                if(Codes.Count > 0 && CodeTypes.Count > 0)
                {
                    int addrType = CodeTypes.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Address.ToString())).CodeTypeId;
                    AddressCodes = Codes.Where(x => x.CodeTypeId == addrType).ToList();
                    if (AddressCodes.Where(x => x.CodeId == 0).Count() == 0)
                        AddressCodes.Add(new FrameworkUAD_Lookup.Entity.Code() { CodeId = 0, DisplayName = "", IsActive = true });

                    int qSourceType = CodeTypes.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source.ToString().Replace("_", " "))).CodeTypeId;
                    QSourceCodes = Codes.Where(x => x.CodeTypeId == qSourceType).ToList();

                    int par3cType = CodeTypes.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Par3c.ToString())).CodeTypeId;
                    Par3CCodes = Codes.Where(x => x.CodeTypeId == par3cType).ToList();

                    int marketingType = CodeTypes.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Marketing.ToString())).CodeTypeId;
                    MarketingCodes = Codes.Where(x => x.CodeTypeId == marketingType).ToList();
                }
                
                // Make sure we are not on the Default KM Client.
                if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientName != "KM")
                {
                    //Unlock all Products that may be locked by this User in Open Close.
                    productW.Proxy.UpdateLock(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID);

                    //Unlock all PubSubscriptions that may be locked by this user.
                    productSubscriptionW.Proxy.UpdateLock(accessKey, 0, false, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                }


                IsLoaded = true;
                ui.Send(x => { busy.IsBusy = false; }, null);
            }
            catch (Exception ex)
            {
                ui.Send(state => HandleException(ex, attempts, ui), null);
                Thread.CurrentThread.Abort();
            }
        }

        private void HandleException(Exception ex, int attempts, SynchronizationContext ui)
        {
            if (attempts > 2)
            {
                Core_AMS.Utilities.WPF.MessageError("There was a problem loading Circulation data. Please restart the Circulation application and try again.");
                busy.IsBusy = false;
                IsLoaded = true;
                throw ex;
            }
            else
                new Thread(() => LoadData(++attempts, ui)).Start();
        }
    }
}
