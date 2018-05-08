using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace DataCompare.Modules
{
    /// <summary>
    /// Interaction logic for CompareViewer.xaml
    /// </summary>
    public partial class CompareViewer : UserControl
    {
        //FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareResult> rWorker = FrameworkServices.ServiceClient.UAS_DataCompareResultClient();
        //FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareResultQue> rqWorker = FrameworkServices.ServiceClient.UAS_DataCompareResultQueClient();
        //FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareResultDetail> dcrdWorker = FrameworkServices.ServiceClient.UAS_DataCompareResultDetailClient();
        //public List<FrameworkUAS.Entity.DataCompareDownload> dcResultList { get; set; }
        //public List<FrameworkUAS.Object.DataCompareGrid> dcResultQueList { get; set; }
        //public List<FrameworkUAS.Entity.DataCompareOption> dcOptions { get; set; }
        public List<KMPlatform.Entity.Client> AllClients { get; set; }
        private KMPlatform.Entity.Client selectedClient { get; set; } 
        public CompareViewer()
        {
            InitializeComponent();
            #region Load Clients
            int currentClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;

            if (currentClientId == 1)
            {
                AllClients = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.OrderBy(x => x.DisplayName).ToList(); //blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result.OrderBy(x => x.ClientName).ToList();
                rcbClients.ItemsSource = AllClients.Where(x => x.IsActive);
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
            }
            else
            {
                AllClients = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.OrderBy(x => x.DisplayName).ToList(); //blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result.OrderBy(x => x.ClientName).ToList();
                rcbClients.ItemsSource = AllClients.Where(x => x.IsActive && x.ClientID == currentClientId);
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
                KMPlatform.Entity.Client sc = AllClients.SingleOrDefault(x => x.ClientID == currentClientId);
                if (sc != null)
                    rcbClients.SelectedItem = sc;
            }
            #endregion

            //FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareOption> oClient = FrameworkServices.ServiceClient.UAS_DataCompareOptionClient();
            //FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.DataCompareOption>> response = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.DataCompareOption>>();
            //response = oClient.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            //if (response.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && response.Result != null)
            //    dcOptions = response.Result;
        }

        private void rcbClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClient = (KMPlatform.Entity.Client)rcbClients.SelectedItem;

            //FrameworkUAS.Service.Response<List<FrameworkUAS.Object.DataCompareGrid>> rqResp = new FrameworkUAS.Service.Response<List<FrameworkUAS.Object.DataCompareGrid>>();
            //rqResp = rqWorker.Proxy.SelectResultQueGrid(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, selectedClient.ClientID);
            //if (rqResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && rqResp.Result != null)
            //    dcResultQueList = rqResp.Result;

            //FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.DataCompareDownload>> rResp = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.DataCompareDownload>>();
            //rResp = rWorker.Proxy.SelectForClient(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, selectedClient.ClientID);
            //if (rResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && rResp.Result != null)
            //{
            //    dcResultList = rResp.Result;
            //}


            //foreach (FrameworkUAS.Object.DataCompareGrid q in dcResultQueList)
            //{
            //    FrameworkUAS.Entity.DataCompareDownload r = dcResultList.FirstOrDefault(x => x.DataCompareId == q.DataCompareResultQueId && x.ProcessCode == q.ProcessCode);
            //    q.DcResult = r;

            //    q.MatchClause = q.MatchClause.Replace("mg.Name = ", "").Replace("and mcs.MasterValue ", "").Replace("   ","");
            //    q.LikeClause = q.LikeClause.Replace("mg.Name = ", "").Replace("and mcs.MasterValue ", "").Replace("   ", "");
            //}

            //gridQues.ItemsSource = dcResultQueList;
        }

        private void HyperLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            FrameworkUAS.Entity.ClientFTP cFTP = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[selectedClient.ClientID].ClientFtpDirectoriesList.FirstOrDefault();
            if (cFTP != null)
            {
                Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(cFTP.Server, cFTP.UserName, cFTP.Password);

                string path = Core.ADMS.BaseDirs.getAppsDir() + e.Uri.AbsolutePath.Replace("/ADMS", "").Replace("/", "\\");
                ftp.Download(e.Uri.AbsolutePath.TrimStart('/'), path);
                //give message and download location
                Core_AMS.Utilities.WPF.MessageFileDownloadComplete(path);
            }
        }

        private void ProfilePurchased_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cb.Content = "True";
            Grid g = (Grid)cb.Parent;
            int dcResultDetailId = 0;
            if (cb.CommandParameter != null)
                int.TryParse(cb.CommandParameter.ToString(), out dcResultDetailId);
            if (dcResultDetailId > 0)
            {
                //FrameworkUAS.Entity.DataCompareResultDetail dcResultDetail = null;
                //foreach (FrameworkUAS.Entity.DataCompareDownload dcr in dcResultList)
                //{
                //    if (dcr.DcResultDetails.Exists(x => x.DataCompareResultDetailId == dcResultDetailId))
                //    {
                //        dcResultDetail = dcr.DcResultDetails.Single(x => x.DataCompareResultDetailId == dcResultDetailId);

                //        dcResultDetail.IsPurchased = true;
                //        dcResultDetail.PurchasedByUserId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                //        dcResultDetail.PurchasedDate = DateTime.Now;
                //        dcResultDetail.UpdatedByUserId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                //        dcResultDetail.DateUpdated = DateTime.Now;

                //        dcr.DcResultDetails.Single(x => x.DataCompareResultDetailId == dcResultDetailId).IsPurchased = dcResultDetail.IsPurchased;
                //        dcr.DcResultDetails.Single(x => x.DataCompareResultDetailId == dcResultDetailId).PurchasedByUserId = dcResultDetail.PurchasedByUserId;
                //        dcr.DcResultDetails.Single(x => x.DataCompareResultDetailId == dcResultDetailId).PurchasedDate = dcResultDetail.PurchasedDate;
                //        dcr.DcResultDetails.Single(x => x.DataCompareResultDetailId == dcResultDetailId).UpdatedByUserId = dcResultDetail.UpdatedByUserId;
                //        dcr.DcResultDetails.Single(x => x.DataCompareResultDetailId == dcResultDetailId).DateUpdated = dcResultDetail.DateUpdated;

                //        dcrdWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, dcResultDetail);
                //        Refresh(g, dcr.DcResultDetails);
                //        break;
                //    }
                //}
            }
        }
        //private void Refresh(Grid gridDCResults, List<FrameworkUAS.Entity.DataCompareResultDetail> dcResultDetails)
        //{
        //    Label lbNoDataCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbNoDataCount");
        //    Label lbFilteredCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbFilteredCount");

        //    Label lbMatchProfileRecordCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbMatchProfileRecordCount");
        //    Label lbMatchProfileAttributeCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbMatchProfileAttributeCount");
        //    Label lbMatchingProfileCost = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbMatchingProfileCost");
        //    CheckBox cbMatchingProfilePurchased = Core_AMS.Utilities.WPF.FindControl<CheckBox>(gridDCResults, "cbMatchingProfilePurchased");//CommandParameter="{Binding DcResult.DataCompareResultQueId}

        //    Label lbMatchDemoRecordCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbMatchDemoRecordCount");
        //    Label lbMatchDemoAttributeCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbMatchDemoAttributeCount");
        //    Label lbMatchingDemoCost = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbMatchingDemoCost");//Content="{Binding DcResult.MatchingDemoCost}"
        //    CheckBox cbMatchingDemoPurchased = Core_AMS.Utilities.WPF.FindControl<CheckBox>(gridDCResults, "cbMatchingDemoPurchased");//Content="{Binding DcResult.MatchingDemoPurchased}"

        //    Label lbLikeProfileRecordCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbLikeProfileRecordCount");
        //    Label lbLikeProfileAttributeCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbLikeProfileAttributeCount");
        //    Label lbLikeProfileCost = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbLikeProfileCost");
        //    CheckBox cbLikeProfilePurchased = Core_AMS.Utilities.WPF.FindControl<CheckBox>(gridDCResults, "cbLikeProfilePurchased");

        //    Label lbLikeDemoRecordCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbLikeDemoRecordCount");
        //    Label lbLikeDemoAttributeCount = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbLikeDemoAttributeCount");
        //    Label lbLikeDemoCost = Core_AMS.Utilities.WPF.FindControl<Label>(gridDCResults, "lbLikeDemoCost");
        //    CheckBox cbLikeDemoPurchased = Core_AMS.Utilities.WPF.FindControl<CheckBox>(gridDCResults, "cbLikeDemoPurchased");

        //    int matchProfileCount = 0;
        //    int noDataCount = 0;

        //    foreach (FrameworkUAS.Entity.DataCompareResultDetail d in dcResultDetails)
        //    {
        //        try
        //        {
        //            string nav = d.ReportFile.Replace(@"\\10.10.41.146\LocalUser\", "ftp://ftp.knowledgemarketing.com/").Replace(selectedClient.FtpFolder + @"\", "").Replace(@"\", "/");

        //            //1	Summary Report
        //            if (d.DataCompareOptionId == dcOptions.Single(x => x.OptionName.Equals(FrameworkUAS.BusinessLogic.Enums.DataCompareOptions.Summary_Report.ToString().Replace("_", " "))).DataCompareOptionId)
        //            {
        //                TextBlock tbSummary = Core_AMS.Utilities.WPF.FindControl<TextBlock>(gridDCResults, "tbSummary");
        //                tbSummary.Inlines.Clear();
        //                Hyperlink hyperLink = new Hyperlink()
        //                {
        //                    NavigateUri = new Uri(nav)
        //                };
        //                hyperLink.Inlines.Add("Summary Report");
        //                hyperLink.RequestNavigate += HyperLink_RequestNavigate;
        //                hyperLink.ToolTip = "Click link to download file.";
        //                tbSummary.Inlines.Add(hyperLink);
        //            }
        //            //1	No Data
        //            if (d.DataCompareOptionId == dcOptions.Single(x => x.OptionName.Equals(FrameworkUAS.BusinessLogic.Enums.DataCompareOptions.No_Data.ToString().Replace("_", " "))).DataCompareOptionId)
        //            {
        //                TextBlock tbNoData = Core_AMS.Utilities.WPF.FindControl<TextBlock>(gridDCResults, "tbNoData");
        //                tbNoData.Inlines.Clear();
        //                Hyperlink hyperLink = new Hyperlink()
        //                {
        //                    NavigateUri = new Uri(nav)
        //                };
        //                hyperLink.Inlines.Add("Non Matches");
        //                hyperLink.RequestNavigate += HyperLink_RequestNavigate;
        //                hyperLink.ToolTip = "Click link to download file.";
        //                tbNoData.Inlines.Add(hyperLink);

        //                lbNoDataCount.Content = d.TotalRecordCount.ToString("0,0.");

        //                noDataCount = d.TotalRecordCount;
        //            }
        //            //2	Matching Profiles
        //            if (d.DataCompareOptionId == dcOptions.Single(x => x.OptionName.Equals(FrameworkUAS.BusinessLogic.Enums.DataCompareOptions.Matching_Profiles.ToString().Replace("_", " "))).DataCompareOptionId)
        //            {
        //                TextBlock tbMatchProfile = Core_AMS.Utilities.WPF.FindControl<TextBlock>(gridDCResults, "tbMatchProfile");
        //                tbMatchProfile.Inlines.Clear();

        //                Hyperlink hyperLink = new Hyperlink()
        //                {
        //                    NavigateUri = new Uri(nav)
        //                };
        //                hyperLink.Inlines.Add("Matching Profiles");
        //                hyperLink.RequestNavigate += HyperLink_RequestNavigate;
        //                hyperLink.ToolTip = "Click link to download file.";
        //                tbMatchProfile.IsEnabled = d.IsPurchased;
        //                tbMatchProfile.Inlines.Add(hyperLink);

        //                lbMatchProfileRecordCount.Content = d.TotalRecordCount.ToString("0,0.");
        //                lbMatchProfileAttributeCount.Content = d.TotalItemCount.ToString("0,0.");
        //                lbMatchingProfileCost.Content = d.TotalCost.ToString("C2");

        //                cbMatchingProfilePurchased.IsChecked = d.IsPurchased;
        //                if (d.IsPurchased == true)
        //                    cbMatchingProfilePurchased.IsEnabled = false;
        //                cbMatchingProfilePurchased.CommandParameter = d.DataCompareResultDetailId.ToString();

        //                matchProfileCount = d.TotalRecordCount;
        //            }
        //            //3	Matching Demographics
        //            if (d.DataCompareOptionId == dcOptions.Single(x => x.OptionName.Equals(FrameworkUAS.BusinessLogic.Enums.DataCompareOptions.Matching_Demographics.ToString().Replace("_", " "))).DataCompareOptionId)
        //            {
        //                TextBlock tbMatchDemo = Core_AMS.Utilities.WPF.FindControl<TextBlock>(gridDCResults, "tbMatchDemo");
        //                tbMatchDemo.Inlines.Clear();
        //                Hyperlink hyperLink = new Hyperlink()
        //                {
        //                    NavigateUri = new Uri(nav)
        //                };
        //                hyperLink.Inlines.Add("Matching Demographics");
        //                hyperLink.RequestNavigate += HyperLink_RequestNavigate;
        //                hyperLink.ToolTip = "Click link to download file.";
        //                tbMatchDemo.IsEnabled = d.IsPurchased;
        //                tbMatchDemo.Inlines.Add(hyperLink);

        //                lbMatchDemoRecordCount.Content = d.TotalRecordCount.ToString("0,0.");
        //                lbMatchDemoAttributeCount.Content = d.TotalItemCount.ToString("0,0.");
        //                lbMatchingDemoCost.Content = d.TotalCost.ToString("C2");
        //                cbMatchingDemoPurchased.IsChecked = d.IsPurchased;
        //                if (d.IsPurchased == true)
        //                    cbMatchingDemoPurchased.IsEnabled = false;
        //                cbMatchingDemoPurchased.CommandParameter = d.DataCompareResultDetailId.ToString();
        //            }
        //            //4	Like Profiles
        //            if (d.DataCompareOptionId == dcOptions.Single(x => x.OptionName.Equals(FrameworkUAS.BusinessLogic.Enums.DataCompareOptions.Like_Profiles.ToString().Replace("_", " "))).DataCompareOptionId)
        //            {
        //                TextBlock tbLikeProfile = Core_AMS.Utilities.WPF.FindControl<TextBlock>(gridDCResults, "tbLikeProfile");
        //                tbLikeProfile.Inlines.Clear();
        //                Hyperlink hyperLink = new Hyperlink()
        //                {
        //                    NavigateUri = new Uri(nav)
        //                };
        //                hyperLink.Inlines.Add("Like Profiles");
        //                hyperLink.RequestNavigate += HyperLink_RequestNavigate;
        //                hyperLink.ToolTip = "Click link to download file.";
        //                tbLikeProfile.IsEnabled = d.IsPurchased;
        //                tbLikeProfile.Inlines.Add(hyperLink);

        //                lbLikeProfileRecordCount.Content = d.TotalRecordCount.ToString("0,0.");
        //                lbLikeProfileAttributeCount.Content = d.TotalItemCount.ToString("0,0.");
        //                lbLikeProfileCost.Content = d.TotalCost.ToString("C2");
        //                cbLikeProfilePurchased.IsChecked = d.IsPurchased;
        //                if (d.IsPurchased == true)
        //                    cbLikeProfilePurchased.IsEnabled = false;
        //                cbLikeProfilePurchased.CommandParameter = d.DataCompareResultDetailId.ToString();
        //            }
        //            //5	Like Demographics
        //            if (d.DataCompareOptionId == dcOptions.Single(x => x.OptionName.Equals(FrameworkUAS.BusinessLogic.Enums.DataCompareOptions.Like_Demographics.ToString().Replace("_", " "))).DataCompareOptionId)
        //            {
        //                TextBlock tbLikeDemo = Core_AMS.Utilities.WPF.FindControl<TextBlock>(gridDCResults, "tbLikeDemo");
        //                tbLikeDemo.Inlines.Clear();
        //                Hyperlink hyperLink = new Hyperlink()
        //                {
        //                    NavigateUri = new Uri(nav)
        //                };
        //                hyperLink.Inlines.Add("Like Demographics");
        //                hyperLink.RequestNavigate += HyperLink_RequestNavigate;
        //                hyperLink.ToolTip = "Click link to download file.";
        //                tbLikeDemo.IsEnabled = d.IsPurchased;
        //                tbLikeDemo.Inlines.Add(hyperLink);

        //                lbLikeDemoRecordCount.Content = d.TotalRecordCount.ToString("0,0.");
        //                lbLikeDemoAttributeCount.Content = d.TotalItemCount.ToString("0,0.");
        //                lbLikeDemoCost.Content = d.TotalCost.ToString("C2");
        //                cbLikeDemoPurchased.IsChecked = d.IsPurchased;
        //                if (d.IsPurchased == true)
        //                    cbLikeDemoPurchased.IsEnabled = false;
        //                cbLikeDemoPurchased.CommandParameter = d.DataCompareResultDetailId.ToString();
        //            }
        //            //6	Other Profiles
        //            if (d.DataCompareOptionId == dcOptions.Single(x => x.OptionName.Equals(FrameworkUAS.BusinessLogic.Enums.DataCompareOptions.Other_Profiles.ToString().Replace("_", " "))).DataCompareOptionId)
        //            {

        //            }
        //            //7	Other Demographics
        //            if (d.DataCompareOptionId == dcOptions.Single(x => x.OptionName.Equals(FrameworkUAS.BusinessLogic.Enums.DataCompareOptions.Other_Demographics.ToString().Replace("_", " "))).DataCompareOptionId)
        //            {

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
        //            Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        //            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
        //            int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
        //            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //            alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".Refresh", app, string.Empty, logClientId);
        //        }
        //    }

        //    int dcResultId = dcResultDetails.First().DataCompareResultId;
        //    FrameworkUAS.Entity.DataCompareDownload dcr = dcResultList.Single(x => x.DataCompareResultId == dcResultId);
        //    int filteredOut = (dcr.SourceFileRecordCount - noDataCount) - matchProfileCount;

        //    lbFilteredCount.ContentStringFormat = "0,0.";
        //    lbFilteredCount.Content = filteredOut.ToString("N0");
        //}
        private void ItemsControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //ItemsControl myIC = (ItemsControl)sender;
            //Grid gridDCResults = (Grid)myIC.Items[0];
            //FrameworkUAS.Object.DataCompareGrid g = (FrameworkUAS.Object.DataCompareGrid)e.NewValue;
            //Refresh(gridDCResults, g.DcResult.DcResultDetails);
        }
    }
}
