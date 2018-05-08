using FrameworkUAD.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for DownloadDetails.xaml
    /// </summary>
    public partial class DownloadDetails : Window
    {
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rtData = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        
        private List<Subscription> subs = new List<Subscription>();
        private List<string> fields = new List<string>();
        private List<string> responses = new List<string>();
        private List<DownloadField> downLoadFields = new List<DownloadField>();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> responseW = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> prodSubW = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeW = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> response = new FrameworkUAS.Service.Response<List<ResponseGroup>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<string>> adHocResponses = new FrameworkUAS.Service.Response<List<string>>();
        public event Action<string> Check;
        public class DownloadField
        {
            public string DisplayName { get; set; }
            public string DownloadName { get; set; }
            public string Type { get; set; }
            public DownloadField(string display, string prefix, string type)
            {
                DisplayName = display;
                DownloadName = prefix + ".[" + display + "]";
                Type = type;
            }
        }

        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> rGroupResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();

        public DownloadDetails(int pubID, bool includeSplitsColumns = false)
        {
            InitializeComponent();
            int uadOnlyID = 0;
            codeResponse = codeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Response_Group);
            if(Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                FrameworkUAD_Lookup.Entity.Code c = codeResponse.Result.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.ResponseGroupTypes.UAD_Only.ToString().Replace("_", " ")).FirstOrDefault();
                if (c != null)
                    uadOnlyID = c.CodeId;
            }
            response = responseW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, 
                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, pubID);
            if (Helpers.Common.CheckResponse(response.Result, response.Status))
                responses = response.Result.Where(x => x.IsActive == true && x.ResponseGroupTypeId != uadOnlyID).OrderBy(x=> x.DisplayOrder).Select(x => x.DisplayName).ToList();
            //responses.Add("Permissions");
            List<string> adHocs = new List<string>();
            adHocResponses = prodSubW.Proxy.Get_AdHocs(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Helpers.Common.CheckResponse(adHocResponses.Result, adHocResponses.Status))
            {
                adHocs = adHocResponses.Result;
            }
            List<string> paidProperties = new List<string>()
            {
                "TotalIssues", "Term", "Frequency", "StartIssueDate", "ExpireIssueDate", "Amount", "AmountPaid", "PaidDate",
                "PaymentType", "CreditCardType", "CheckNumber", "CCNumber", "CCExpirationMonth", "CCExpirationYear", "Payor Name" 
            };
            List<string> properties = new List<string>()
            {
                "Pubcode","SequenceID","SubscriptionID", "Batch", "Email","FirstName","LastName","Company","Title","Address1","Address2","Address3","City","RegionCode","ZipCode","Plus4","Country","County","Phone","Mobile","Fax",
                "Website","CategoryCode", "TransactionCode", "QSource","Qualificationdate","Par3C", "Copies","Demo7","SubscriberSourceCode", "OrigsSrc", "WaveMailingID",
                "IMBSeq", "ReqFlag", "Verify", "MailPermission", "FaxPermission", "PhonePermission", "OtherProductsPermission", "EmailRenewPermission", "ThirdPartyPermission","TextPermission","DateCreated","DateUpdated"
            };
            if(includeSplitsColumns == true)
            {
                properties.Add("ACSCode");
                properties.Add("Keyline");
                properties.Add("MailerID");
                properties.Add("Split Name");
                properties.Add("Split Description");
                properties.Add("KeyCode");
                properties.Remove("SubscriptionID");
            }
            foreach(string s in properties)
            {
                if(s.Equals("Pubcode"))
                {
                    downLoadFields.Add(new DownloadField(s, "p", "Profile"));
                }
                else
                {
                    downLoadFields.Add(new DownloadField(s, "ps", "Profile"));
                }
            }
            foreach (string s in paidProperties)
                downLoadFields.Add(new DownloadField(s, "sp", "Paid"));
            foreach (string s in responses)
                downLoadFields.Add(new DownloadField(s, "demos", "Demo"));
            foreach (string s in adHocs)
                downLoadFields.Add(new DownloadField(s, "adhoc", "AdHoc"));

            rlbProfileFields.ItemsSource = downLoadFields.Where(x=> x.Type == "Profile");
            rlbDemoFields.ItemsSource = downLoadFields.Where(x=> x.Type == "Demo");
            rlbPaidFields.ItemsSource = downLoadFields.Where(x => x.Type == "Paid");
            rlbAdHocFields.ItemsSource = downLoadFields.Where(x => x.Type == "AdHoc");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            string final = "";
            foreach(string s in fields)
            {
                final = final + s + ",";
            }
            final = final.TrimEnd(',');
            if (Check != null)
                Check(final);
            CloseDialog();
        }

        private void CloseDialog()
        {
            DoubleAnimation animateOpacity = new DoubleAnimation();
            animateOpacity = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(.2)
            };
            animateOpacity.Completed += (s, e2) =>
            {
                this.Close();
            };
            this.Close();
        }

        private void Fields_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (DownloadField d in e.AddedItems)
                fields.Add(d.DownloadName);
            foreach (DownloadField d in e.RemovedItems)
                fields.Remove(d.DownloadName);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
