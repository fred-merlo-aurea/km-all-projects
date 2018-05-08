using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace AMS_Desktop.Windows
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Windows.Login login = new Windows.Login();
            login.Show();
            this.Close();
        }

        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            KMPlatform.Entity.User u = null;
            var kmKey = Guid.Parse("651A1297-59D1-4857-93CB-0B049635762E");
            //get the User by emailaddress
            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> uClient = FrameworkServices.ServiceClient.UAS_UserClient();
            u = uClient.Proxy.SearchUserName(kmKey, Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbUserName.Text)).Result;

            busyIcon.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
               {
                   if (u != null || !string.IsNullOrEmpty(u.UserName)) 
                   {
                       ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                       u.Password = Core_AMS.Utilities.StringFunctions.GenerateTempPassword();
                       u.RequirePasswordReset = true;
                      

                       string url;
                       //ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;  this wont work, need to grab appSetting
                       string appEnviron = System.Configuration.ConfigurationManager.AppSettings["AppEnvironment"].ToString();
                       if (!appEnviron.Equals("LIVE", StringComparison.CurrentCultureIgnoreCase))
                           url = "http://test.ecn5.com/EmailMarketing.Site/Reset?id=" + u.UserID;
                       else
                           url = "http://www.ecn5.com/EmailMarketing.Site/Reset?id=" + u.UserID;

                       //send email
                       //Core_AMS.Utilities.SendEmail email = new Core_AMS.Utilities.SendEmail();
                       ed.EmailSubject = "KM Platform Password Reset";
                       var sb = new System.Text.StringBuilder();
                       //sb.AppendLine("<form action='" + url + "'>");
                       //sb.AppendLine("<img src='http://ams.kmpsgroup.com/Images/KM_banner.png' alt='KMbanner' />");
                       //sb.AppendLine("<br/>");
                       //sb.AppendLine("<br/>");
                       sb.AppendLine("Dear " + u.FirstName);
                       sb.AppendLine("<br/>");
                       sb.AppendLine("<br/>");
                       sb.AppendLine("As you requested, below is your temorary password which will allow you to access your KM Platform account.");
                       sb.AppendLine("<br/>");
                       sb.AppendLine("<br/>");
                       sb.AppendLine("<b>Temporary password: " + u.Password + "</b>");
                       sb.AppendLine("<br/>");
                       sb.AppendLine("<br/>");
                       sb.AppendLine("Please click the link below to reset your password using the temporary password provided.");
                       sb.AppendLine("<br/>");
                       sb.AppendLine("<br/>");
                       sb.AppendLine("<a href='" + url + "'>Reset My Password</a>");
                       //sb.AppendLine("<input type='submit' value='Reset My Password'></input>");
                       sb.AppendLine("<br/>");
                       sb.AppendLine("<br/>");
                       sb.AppendLine("If you did not request a temporary password and you believe someone attempted to access your account information, please contact us immediately at 1.866.844.6275.");
                       //sb.AppendLine("</form>");

                       ed.Content = sb.ToString();
                       ed.ReplyEmailAddress = System.Configuration.ConfigurationManager.AppSettings["EmailFrom"];
                       ed.EmailAddress = u.EmailAddress;
                       ed.SendTime = DateTime.Now;
                       ed.Process = "AMS forgot password";
                       ed.Source = "AMS Desktop";
                       ed.FromName = "Knowledge Marketing";
                       ed.CustomerID = u.DefaultClientID;
                       //ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                       uClient.Proxy.Save(kmKey, u, ed);
                       //email.Send();
                   }
               };
            bw.RunWorkerCompleted += (o, ea) =>
                {
                    busyIcon.IsBusy = false;
                    if (u == null || string.IsNullOrEmpty(u.UserName))
                    {
                        //give error that email not found
                        Core_AMS.Utilities.WPF.Message("The username provided does not exist, please check the spelling and try again.",
                                            MessageBoxButton.OK, MessageBoxImage.Error, "Username Not Found");
                    }
                    else
                    {
                        //give confirmation message of email sent
                        Core_AMS.Utilities.WPF.Message("Your password reset link will be sent shortly to: " + u.EmailAddress + " for Username: " + u.UserName,
                                           MessageBoxButton.OK, MessageBoxImage.Information, "Password Sent");

                        Windows.Login login = new Windows.Login();
                        login.Show();
                        this.Close();
                    }
                };
            bw.RunWorkerAsync();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
