using mshtml;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for SubscriptionGenius.xaml
    /// </summary>
    public partial class SubscriptionGenius : UserControl
    {
        private readonly string reportURL = "https://app.subscriptiongenius.com/account/reports/";
        private readonly string subscriberURL = "https://app.subscriptiongenius.com/account/subscriber/subscriber/";
        private readonly string searchURL = "http://app.subscriptiongenius.com/account/subscriber/search/";
        private readonly string newSubscriberURL = "https://app.subscriptiongenius.com/account/subscriber/new/";
        private readonly string homeURL = "https://app.subscriptiongenius.com/account/";
        public SubscriptionGenius(KMPlatform.BusinessLogic.Enums.SubGenControls sg, int subscriberID = 0)
        {
            InitializeComponent();
            if (sg == KMPlatform.BusinessLogic.Enums.SubGenControls.Reports)
                webLogin.Source = new Uri(reportURL + GetURLParameters());
            else if(sg == KMPlatform.BusinessLogic.Enums.SubGenControls.Subscriber)
                webLogin.Source = new Uri(subscriberURL + GetURLParameters(subscriberID));
            else if (sg == KMPlatform.BusinessLogic.Enums.SubGenControls.Home)
                webLogin.Source = new Uri(homeURL + GetURLParameters());
            else if (sg == KMPlatform.BusinessLogic.Enums.SubGenControls.New_Subscriber)
                webLogin.Source = new Uri(newSubscriberURL + GetURLParameters());
            else if (sg == KMPlatform.BusinessLogic.Enums.SubGenControls.Search)
                webLogin.Source = new Uri(searchURL + GetURLParameters());
        }

        private void btnNewWindow_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser wb = new WebBrowser();
            wb.Source = new Uri("http://app.subscriptiongenius.com/account/subscriber/subscriber/?subscriberID=2312275&iframe=true", UriKind.Absolute);
            wb.Loaded += new RoutedEventHandler(wb_Loaded);

            NavigationWindow window = new NavigationWindow();
            //Uri source = new Uri("http://app.subscriptiongenius.com/account/subscriber/subscriber/?subscriberID=2312275&iframe=true", UriKind.Absolute);
            //window.Source = source;
            window.Content = wb;
            window.Width = 1280;
            window.Height = 800;
            window.Show();
        }
        private void btnSetCredentials_Click(object sender, RoutedEventArgs e)
        {
            //user_email
            //user_password
            HTMLDocument document = (HTMLDocument)webLogin.Document;
            IHTMLElement username = document.getElementById("user_email");
            //username.setAttribute("value", "Linda.courtney@teamkm.com");
            //username.setAttribute("value", "dev-group@TeamKM.com");
            IHTMLElement password = document.getElementById("user_password");
            //password.setAttribute("value", "87965555");
            //password.setAttribute("value", "Ant-Man");
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://app.subscriptiongenius.com/account/subscriber/search/?iframe=true";
            string api = "&api_login_token=" + FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.SubGenLoginToken;
            Uri source = new Uri(url + api, UriKind.Absolute);
            webLogin.Source = source;
        }
        private void wb_Loaded(object sender, RoutedEventArgs e)
        {
            WebBrowser wb = (WebBrowser)sender;
            HideScriptErrors(wb, true);

            HTMLDocument document = (HTMLDocument)webLogin.Document;
            IHTMLElement username = document.getElementById("user_email");
            //username.setAttribute("value", "Linda.courtney@teamkm.com");
            //username.setAttribute("value", "dev-group@TeamKM.com");
            IHTMLElement password = document.getElementById("user_password");
            //password.setAttribute("value", "87965555");
            //password.setAttribute("value", "Ant-Man");
        }
        private void webLogin_Loaded(object sender, RoutedEventArgs e)
        {
            HideScriptErrors(webLogin, true);
        }
        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }
        private void webLogin_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            string main = "https://" + e.Uri.Host + e.Uri.AbsolutePath;
            string url = "";
            if(e.Uri.Query.Length == 0)
                url = main + "/?iframe=true&api_login_token=" + FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.SubGenLoginToken;
            else
            {
                url = main + e.Uri.Query;
                if (!e.Uri.Query.Contains("iframe"))
                    url += "&iframe=true";
                if (!e.Uri.Query.Contains("api_login_token"))
                    url += "&iframe=true&api_login_token=" + FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.SubGenLoginToken;
            }
            if (!url.Equals(e.Uri.OriginalString))
                webLogin.Source = new Uri(url);
        }
        private string GetURLParameters()
        {
            return "?iframe=true&api_login_token=" + FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.SubGenLoginToken;
        }
        private string GetURLParameters(int subscriberID)
        {
            return "?iframe=true&api_login_token=" + FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.SubGenLoginToken + "&subscriberID=" + subscriberID.ToString();
        }
    }
}
