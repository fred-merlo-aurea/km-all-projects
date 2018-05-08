using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Core.ADMS;
using KM.Common;
using KM.Common.Utilities.Email;

namespace AMS_Desktop.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        System.Configuration.Configuration config;
        private bool loginFailureMessageSet { get; set; }

        public Login()
        {
            try
            {
                InitializeComponent();
                //spin up Helpers
                FrameworkUAS.Object.AppData.myAppData = new FrameworkUAS.Object.AppData();

                string appEnviron = System.Configuration.ConfigurationManager.AppSettings["AppEnvironment"].ToString();
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                    lbVersion.Content = appEnviron + " - Version: " + ad.CurrentVersion.ToString();
                }
                else
                    lbVersion.Content = appEnviron + " - Version: DEV";

                LoadCredentials();
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".Login", app, string.Empty, logClientId);
            }
        }
        private void ZipAndEmailLogs()
        {
            //delete any existing zip files
            foreach (string filePath in Directory.GetFiles(BaseDirs.getLogDir()))
            {
                FileInfo fi = new FileInfo(filePath);
                if (fi.Extension.Equals(".zip"))
                    fi.Delete();
            }

            if (Directory.GetFiles(BaseDirs.getLogDir()).Count() > 0)
            {
                Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
                FileInfo myZipLog = ff.CreateZipFile(BaseDirs.getLogDir(), "AMSLogFiles");
                string from = ConfigurationManager.AppSettings["EmailFrom"].ToString();
                string to = ConfigurationManager.AppSettings["ErrorNotification"].ToString();
                string[] toArray = to.Split(',');
                List<string> toList = new List<string>();
                foreach (string t in toArray)
                    toList.Add(t);
                string mailServer = ConfigurationManager.AppSettings["MailServer"].ToString();
                List<Attachment> listAtt = new List<Attachment>();
                listAtt.Add(new Attachment(myZipLog.FullName));

                try
                {
                    var message = "AMS Log Files";
                    var emailService = new EmailService(new EmailClient(), new ConfigurationProvider());
                    var emailMessage = new EmailMessage
                    {
                        From = from,
                        Subject = message,
                        Body = message,
                        IsHtml = false
                    };
                    emailMessage.AddRange(emailMessage.To, toList);
                    emailMessage.AddRange(emailMessage.Attachments, listAtt);
                    emailService.SendEmail(emailMessage, mailServer);
                }
                catch { }
                try
                {
                    //delete all files
                    foreach (string filePath in Directory.GetFiles(BaseDirs.getLogDir()))
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(filePath);
                            fi.Delete();
                        }
                        catch { }
                    }

                    myZipLog.Delete();
                }
                catch { }
            }
        }
        private void LoadCredentials()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            tbUserName.Text = config.AppSettings.Settings["UserName"].Value.ToString();
            tbPassword.Password = config.AppSettings.Settings["Password"].Value.ToString();

            config.AppSettings.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
            config.AppSettings.SectionInformation.ForceSave = true;

            try
            {
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch { }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private FrameworkUAS.Object.UserAuthorization UserAuthorization(string user, string pass)
        {
            FrameworkUAS.Object.UserAuthorization uAuth = new FrameworkUAS.Object.UserAuthorization();
            try
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IServerVariable> svClient = FrameworkServices.ServiceClient.UAS_ServerVariableClient();
                FrameworkServices.ServiceClient<UAS_WS.Interface.IUserAuthorization> uaClient = FrameworkServices.ServiceClient.UAS_UserAuthorizationClient();

                KMPlatform.Object.ServerVariable sv = new KMPlatform.Object.ServerVariable();
                FrameworkUAS.Service.Response<KMPlatform.Object.ServerVariable> svResponse = new FrameworkUAS.Service.Response<KMPlatform.Object.ServerVariable>();
                svResponse = svClient.Proxy.GetServerVariables();
                if (svResponse.Result != null && svResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    sv = svResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                svClient.Close();

                string ipAddress = string.Empty;
                if (!string.IsNullOrEmpty(sv.REMOTE_ADDR))
                    ipAddress = sv.REMOTE_ADDR;
                else
                    ipAddress = uaClient.Proxy.GetIpAddress().Result;

                string appVersion = string.Empty;
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                    if (ad != null)
                    {
                        if (ad.CurrentVersion != null)
                            appVersion = ad.CurrentVersion.ToString();
                    }
                }
                else
                    appVersion = "DEV";

                FrameworkUAS.Object.Encryption ec = new FrameworkUAS.Object.Encryption();
                ec.PlainText = pass;
                FrameworkUAS.BusinessLogic.Encryption e = new FrameworkUAS.BusinessLogic.Encryption();
                ec = e.Encrypt(ec);

                FrameworkUAS.Service.Response<KMPlatform.Object.UserAuthorization> uaResponse = new FrameworkUAS.Service.Response<KMPlatform.Object.UserAuthorization>();
                uaResponse = uaClient.Proxy.Login(user, ec.EncryptedText, ec.SaltValue, "AMS", ipAddress, sv, appVersion);
                if (uaResponse.Result != null)
                    uAuth = new FrameworkUAS.Object.UserAuthorization(uaResponse.Result);
                else
                {
                    loginFailureMessageSet = true;
                    if (uaResponse.Message == "Incorrect Login")
                        Core_AMS.Utilities.WPF.MessageError("Incorrect password. Click the Forgot Password link if you cannot remember your password.");
                    else if (uaResponse.Message == "Disabled")
                        Core_AMS.Utilities.WPF.MessageError("This User Account is currently disabled. Please contact your system administrator.");
                    else if (uaResponse.Message == "Locked")
                        Core_AMS.Utilities.WPF.MessageError("This User Account is currently locked. Please contact your system administrator.");
                    else if (uaResponse.Message == "No Roles")
                        Core_AMS.Utilities.WPF.MessageError("This User Account is not set up. Please contact your system administrator.");
                    else
                        Core_AMS.Utilities.WPF.MessageServiceError();
                }

                if (uAuth.IsAuthenticated == true)
                {
                    FrameworkServices.ServiceClient<UAS_WS.Interface.IApplication> appB = FrameworkServices.ServiceClient.UAS_ApplicationClient();
                    List<KMPlatform.Entity.Application> apps = appB.Proxy.Select(uAuth.AuthAccessKey).Result;
                    if (apps != null)
                    {
                        foreach (KMPlatform.Entity.Application app in apps)
                        {
                            if (!Directory.Exists(BaseDirectories["Applications"] + "\\" + app.ApplicationName))
                                Directory.CreateDirectory(BaseDirectories["Applications"] + "\\" + app.ApplicationName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            }
            return uAuth;
        }
        private void CheckSettings()
        {
            //unlock our app.config
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //add any code here to make sure temp folder, RegKeys, etc.. are installed
            config.AppSettings.Settings["UserName"].Value = tbUserName.Text.Trim();
            config.AppSettings.Settings["Password"].Value = tbPassword.Password.Trim();

            //could set the Theme here

            try
            {
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch { }
        }
        
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            loginFailureMessageSet = false;
            if (string.IsNullOrEmpty(tbUserName.Text) || string.IsNullOrEmpty(tbPassword.Password))
            {
                Core_AMS.Utilities.WPF.Message("You must supply an Username and Password, please try again.", MessageBoxButton.OK, MessageBoxImage.Error, "Login Aborted");
            }
            else
            {
                CheckSettings();
                BackgroundWorker bw = new BackgroundWorker();
                busyIcon.IsBusy = true;

                string user = tbUserName.Text.Trim();
                string pass = tbPassword.Password.Trim();

                string dc = BaseDirectories["Applications"] + "\\DataCompare";
                if (!Directory.Exists(dc))
                    Directory.CreateDirectory(dc);

                bw.DoWork += (o, ea) =>
                {
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser = UserAuthorization(user, pass);
                    if(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsPlatformAdministrator)
                    {
                        FrameworkServices.ServiceClient<UAS_WS.Interface.IClientGroup> clientGroupW = FrameworkServices.ServiceClient.UAS_ClientGroupClient();
                        FrameworkServices.ServiceClient<UAS_WS.Interface.IService> serviceW = FrameworkServices.ServiceClient.UAS_ServiceClient();
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups = clientGroupW.Proxy.SelectLite(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup == null)
                            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup();


                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services = serviceW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, true).Result;
                    }
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    busyIcon.IsBusy = false;
                    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.IsAuthenticated == true)
                    {
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.RequirePasswordReset == true)
                        {
                            RedirectChangePassword(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User);
                        }
                        else
                        {
                            //could change to only do this if user has access to Circulation Application
                            #region Circulation
                            if (FrameworkUAS.Object.AppData.myAppData.BatchList == null)
                                FrameworkUAS.Object.AppData.myAppData.BatchList = new List<FrameworkUAS.Object.Batch>();

                            FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> bClient = FrameworkServices.ServiceClient.UAD_BatchClient();
                            List<FrameworkUAD.Entity.Batch> existing = null;

                            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections != null)
                                existing = bClient.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.AccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, true, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                            if (existing != null)
                            {
                                if (existing.Count > 0)
                                {
                                    List<FrameworkUAS.Object.Batch> b = new List<FrameworkUAS.Object.Batch>();

                                    foreach (var x in existing)
                                    {
                                        b.Add(new FrameworkUAS.Object.Batch
                                        {
                                            BatchCount = x.BatchCount,
                                            BatchID = x.BatchID,
                                            BatchNumber = x.BatchNumber,
                                            DateCreated = x.DateCreated,
                                            DateFinalized = x.DateFinalized,
                                            IsActive = x.IsActive,
                                            PublicationID = x.PublicationID,
                                            UserID = x.UserID
                                        });
                                    }

                                    FrameworkUAS.Object.AppData.myAppData.BatchList = b;
                                }
                            }
                            #endregion

                            try
                            {
                                Windows.Home w = new Windows.Home(true);
                                w.Width = 380;
                                w.Height = 420;
                                Application.Current.MainWindow = w;

                                w.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                                w.Show();
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".btnLogin_Click", app, string.Empty, logClientId);
                            }
                        }
                    }
                    else
                    {
                        if (Core_AMS.Utilities.StringFunctions.isEmail(tbUserName.Text.Trim()))
                        {
                            //lets get the user object and just see if a password reset is required.
                            FrameworkUAS.Service.Response<KMPlatform.Entity.User> uResponse = new FrameworkUAS.Service.Response<KMPlatform.Entity.User>();
                            var kmKey = Guid.Parse("651A1297-59D1-4857-93CB-0B049635762E");
                            //get the User by emailaddress
                            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> uClient = FrameworkServices.ServiceClient.UAS_UserClient();
                            uResponse = uClient.Proxy.SearchUserName(kmKey, tbUserName.Text.Trim());
                            if (uResponse.Result != null)
                            {
                                if(uResponse.Result.RequirePasswordReset)
                                    RedirectChangePassword(uResponse.Result);
                            }
                        }
                        //show a failed message if one not set from UserAuthorization()
                        if (loginFailureMessageSet == false)
                            Core_AMS.Utilities.WPF.Message("Login Failed, please try again.", MessageBoxButton.OK, MessageBoxImage.Error, "Login Failed");
                    }
                };
                bw.RunWorkerAsync();
            }
        }
        private void RedirectChangePassword(KMPlatform.Entity.User user)
        {
            if (user.RequirePasswordReset)
            {
                string url;
                //ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;  this wont work, need to grab appSetting
                string appEnviron = System.Configuration.ConfigurationManager.AppSettings["AppEnvironment"].ToString();
                if (!appEnviron.Equals("LIVE", StringComparison.CurrentCultureIgnoreCase))
                    url = "http://test.ecn5.com/EmailMarketing.Site/Reset?id=" + user.UserID;
                else
                    url = "http://www.ecn5.com/EmailMarketing.Site/Reset?id=" + user.UserID;


                Helpers.ResetPassword rp = new Helpers.ResetPassword(url);

                this.Width = 650;
                this.Height = 650;
                this.ResizeMode = ResizeMode.CanResizeWithGrip;

                NavigationWindow _navigationWindow = new NavigationWindow();
                _navigationWindow.Height = this.Height;
                _navigationWindow.Width = this.Width;
                _navigationWindow.Show();
                _navigationWindow.Navigate(rp);
                this.Close();
            }
        }
        private void btnForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            Windows.ForgotPassword fp = new Windows.ForgotPassword();
            fp.Show();
            this.Close();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (typeof(TextBox) == sender.GetType())
            {
                if (e.Key == Key.Enter)
                {
                    btnLogin.Focus();
                    btnLogin_Click(sender, e);
                }
            }
        }
        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (typeof(PasswordBox) == sender.GetType())
            {
                if (e.Key == Key.Enter)
                {
                    btnLogin.Focus();
                    btnLogin_Click(sender, e);
                }
            }
        }

        public Dictionary<string, string> BaseDirectories
        {
            get { return BaseDirs.GetBaseDirectories(); }
        }

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbUserName.Focus();
        }

        private void tbPassword_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            PasswordBox pwb = sender as PasswordBox;
            pwb.SelectAll();
        }
    }
}
