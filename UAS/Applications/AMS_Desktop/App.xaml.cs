using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using Core.ADMS;
using KM.Common;
using KM.Common.Utilities.Email;

namespace AMS_Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        bool m_RequiredUpdateDetected = false;
        public void saveDialog(object sender, RoutedEventArgs e)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public Dictionary<string, string> BaseDirectories
        {
            get { return BaseDirs.GetBaseDirectories(); }
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                InitialSetup();
                //spin up Helpers
                FrameworkUAS.Object.AppData.myAppData = new FrameworkUAS.Object.AppData();

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.Save(ConfigurationSaveMode.Modified);

                //CheckForShortcut();
            }
            catch (Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);

                Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
                string errorFile = "C:\\ADMS\\Logs\\UnhandledExceptionError_" + DateTime.Now.ToString("MMddyyyy") + ".txt";
                try
                {
                    System.IO.StreamWriter sw = new StreamWriter(errorFile, true);
                    ff.WriteToFile(msg, sw);
                }
                catch { }
            }
        }
        void CheckForShortcut()
        {
            //string name = Path.GetFileName(Assembly.GetEntryAssembly().GetName().Name).Replace("_", " ");
            //try
            //{
            //    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //    if (!System.IO.File.Exists(desktopPath + "\\" + name + ".lnk"))
            //    {
            //        //object shDesktop = (object)"Desktop";
            //        //WshShell shell = new WshShell();
            //        //string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\" + name + ".lnk";
            //        //IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            //        //shortcut.Description = "AMS Desktop";
            //        //shortcut.Hotkey = "Ctrl+Shift+K";
            //        //shortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //        //shortcut.Save();
            //    }
            //}
            //catch { }

            //if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            //{
            //    ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            //    if (ad.IsFirstRun)  //first time user has run the app since installation or update
            //    {

            //    }
            //}

            Assembly code = Assembly.GetExecutingAssembly();
            string company = string.Empty;
            //string description = string.Empty;
            if (Attribute.IsDefined(code, typeof(AssemblyCompanyAttribute)))
            {
                AssemblyCompanyAttribute ascompany = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(code,
                    typeof(AssemblyCompanyAttribute));
                company = ascompany.Company;
            }
            //if (Attribute.IsDefined(code, typeof(AssemblyDescriptionAttribute)))
            //{
            //    AssemblyDescriptionAttribute asdescription = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(code,
            //        typeof(AssemblyDescriptionAttribute));
            //    description = asdescription.Description;
            //}
            if (company != string.Empty)
            {
                string appName = Path.GetFileNameWithoutExtension(Application.ResourceAssembly.Location).Replace("_", " ");
                string desktopPath = string.Empty;
                desktopPath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "\\", appName, ".appref-ms");
                string shortcutName = string.Empty;
                shortcutName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "\\", company, "\\", appName, ".appref-ms");
                System.IO.File.Copy(shortcutName, desktopPath, true);

                //string desktopPath = string.Empty;
                //desktopPath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "\\", description, ".appref-ms");
                //string shortcutName = string.Empty;
                //shortcutName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "\\", company, "\\", description, ".appref-ms");
                //System.IO.File.Copy(shortcutName, desktopPath, true);
            }

        }
        private void InitialSetup()
        {
            foreach (KeyValuePair<string, string> kvp in Core.ADMS.BaseDirs.GetBaseDirectories())
            {
                if (!Directory.Exists(kvp.Value))
                    Directory.CreateDirectory(kvp.Value);
            }
        }
        public static void OnCheckForUpdate()
        {
            // Check to ensure the application is running through ClickOnce.
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                // Check for updates asynchronization.
                ApplicationDeployment.CurrentDeployment.CheckForUpdateAsync();
            }
        }
        void OnCheckForUpdatesCompleted(object sender, CheckForUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                if (e.IsUpdateRequired)
                    m_RequiredUpdateDetected = true;
                ApplicationDeployment.CurrentDeployment.UpdateAsync();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There currently is not an update available.", MessageBoxButton.OK, MessageBoxImage.Information, "No Update");
            }
        }
        void OnUpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (m_RequiredUpdateDetected)
            {
                MessageBox.Show("Required update downloaded, application will restart");
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            else
            {
                MessageBoxResult result = Core_AMS.Utilities.WPF.MessageResult("Application update downloaded. Would you like to restart?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            // hook on error before app really starts
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment.CurrentDeployment.CheckForUpdateCompleted += OnCheckForUpdatesCompleted;
                ApplicationDeployment.CurrentDeployment.UpdateCompleted += new AsyncCompletedEventHandler(OnUpdateCompleted);
            }

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                //run on seperate thread
                CheckForShortcut();
                InitialSetup();
                ZipAndEmailLogs();
            };
            bw.RunWorkerAsync();
            base.OnStartup(e);
        }

        #region Restore To Previous Version code
        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }
        #endregion
        #region Interop Declarations
        [DllImport("user32.Dll")]
        private static extern int EnumWindows(EnumWindowsCallbackDelegate callback, IntPtr lParam);
        [DllImport("User32.Dll")]
        private static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);
        [DllImport("User32.Dll")]
        private static extern void GetClassName(int h, StringBuilder s, int nMaxCount);
        [DllImport("User32.Dll")]
        private static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsCallbackDelegate lpEnumFunc, IntPtr lParam);
        [DllImport("User32.Dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern short FlashWindowEx(ref FLASHWINFO pwfi);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        #endregion
        #region Constants
        private const int BM_CLICK = 0x00F5;
        private const uint FLASHW_ALL = 3;
        private const uint FLASHW_CAPTION = 1;
        private const uint FLASHW_STOP = 0;
        private const uint FLASHW_TIMER = 4;
        private const uint FLASHW_TIMERNOFG = 12;
        private const uint FLASHW_TRAY = 2;
        private const int FIND_DLG_SLEEP = 200; //Milliseconds to sleep between checks for installation dialogs.
        private const int FIND_DLG_LOOP_CNT = 50; //Total loops to look for an install dialog. Defaulting 200ms sleap time, 50 = 10 seconds.
        #endregion
        #region Delegates
        private delegate bool EnumWindowsCallbackDelegate(IntPtr hwnd, IntPtr lParam);
        #endregion
        /// <summary>
        /// Restore to previous version of the application.
        /// </summary>
        public static void RestoreToPreviousVersion()
        {
            string publicKeyToken = GetPublicKeyToken();
            if (publicKeyToken == "0000000000000000")
                publicKeyToken = "aebacd906c9bd4f4";
            // Find Uninstall string in registry
            string DisplayName = null;
            string uninstallString = GetUninstallString(publicKeyToken, out DisplayName);
            if (uninstallString.Length <= 0)
            {
                return;
            }

            string runDLL32 = uninstallString.Substring(0, 12);
            string args = uninstallString.Substring(13);

            //start the uninstall; this will bring up the uninstall dialog asking if it's ok
            //Process uninstallProcess = Process.Start(runDLL32, args);

            //stopping here cause we just want to prompt user to have ablility to Rollback to previous version
            Process.Start(runDLL32, args);

            //push the OK button
            PushUninstallOKButton(DisplayName);
            //InstallNewVersion();
        }
        /// <summary>
        /// Gets the public key token
        /// for the current running (ClickOnce) application.
        /// </summary>
        /// <returns></returns>
        public static string GetPublicKeyToken()
        {
            ApplicationSecurityInfo asi = new ApplicationSecurityInfo(AppDomain.CurrentDomain.ActivationContext);
            byte[] pk = asi.ApplicationId.PublicKeyToken;
            StringBuilder pkt = new StringBuilder();
            for (int i = 0; i < pk.GetLength(0); i++)
                pkt.Append(String.Format("{0:x2}", pk[i]));
            return pkt.ToString();
        }
        /// <summary>
        /// Gets the uninstall string for the current ClickOnce app
        /// from the Windows Registry.
        /// </summary>
        /// <param name="PublicKeyToken">The public key token of the app.</param>
        /// <returns>The command line to execute that will
        /// uninstall the app.</returns>
        public static string GetUninstallString(string PublicKeyToken, out string DisplayName)
        {
            string uninstallString = null;
            //set up the string to search for
            //string searchString = "PublicKeyToken=" + PublicKeyToken;
            //open the registry key and get the subkey names
            RegistryKey uninstallKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            string[] appKeyNames = uninstallKey.GetSubKeyNames();

            DisplayName = null;
            bool found = false;

            string appName = Path.GetFileNameWithoutExtension(Application.ResourceAssembly.Location);
            //search through the list for one with a match
            foreach (string appKeyName in appKeyNames)
            {
                RegistryKey appKey = uninstallKey.OpenSubKey(appKeyName);
                uninstallString = (string)appKey.GetValue("UninstallString");
                DisplayName = (string)appKey.GetValue("DisplayName");
                appKey.Close();
                if (uninstallString.Contains(PublicKeyToken) && DisplayName == appName.Replace("_", " "))
                {
                    found = true;
                    break;
                }
            }

            uninstallKey.Close();

            if (found)
                return uninstallString;
            else
                return string.Empty;
        }
        /// <summary>
        /// Find and Push the OK button on the uninstall dialog.
        /// </summary>
        /// <param name="DisplayName">Display Name value from the registry</param>
        private static void PushUninstallOKButton(string DisplayName)
        {
            bool success = false;

            //Find the uninstall dialog.
            IntPtr uninstallerWin = FindUninstallerWindow(DisplayName, out success);
            IntPtr OKButton = IntPtr.Zero;

            //If it found the window, look for the button.
            if (success)
                OKButton = FindUninstallerOKButton(uninstallerWin, out success);

            //If it found the button, press it.
            if (success)
                DoButtonClick(OKButton);
        }
        private static void DoButtonClick(IntPtr ButtonHandle)
        {
            SendMessage(ButtonHandle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        }
        /// <summary>
        /// Find the uninstall dialog.
        /// </summary>
        /// <param name="DisplayName">Display Name retrieved
        /// from the registry.</param>
        /// <param name="success">Whether the window was found or not.</param>
        /// <returns>Pointer to the uninstall dialog.</returns>
        private static IntPtr FindUninstallerWindow(string DisplayName, out bool success)
        {
            //Max number of times to look for the window,
            //used to let you out if there's a problem.
            int i = 25;
            IntPtr uninstallerWindow = IntPtr.Zero;
            while (uninstallerWindow == IntPtr.Zero && i > 0)
            {
                uninstallerWindow = SearchForTopLevelWindow(DisplayName + " Maintenance");
                System.Threading.Thread.Sleep(500);
                i--;
            }

            if (uninstallerWindow == IntPtr.Zero)
                success = false;
            else
                success = true;

            return uninstallerWindow;
        }
        private static IntPtr SearchForTopLevelWindow(string WindowTitle)
        {
            ArrayList windowHandles = new ArrayList();
            /* Create a GCHandle for the ArrayList */
            GCHandle gch = GCHandle.Alloc(windowHandles);
            try
            {
                EnumWindows(new EnumWindowsCallbackDelegate(EnumProc), (IntPtr)gch);
                /* the windowHandles array list contains all of the
                    window handles that were passed to EnumProc.  */
            }
            finally
            {
                /* Free the handle */
                gch.Free();
            }

            /* Iterate through the list and get the handle thats the best match */
            foreach (IntPtr handle in windowHandles)
            {
                StringBuilder sb = new StringBuilder(1024);
                GetWindowText((int)handle, sb, sb.Capacity);
                if (sb.Length > 0)
                {
                    if (sb.ToString().StartsWith(WindowTitle))
                    {
                        return handle;
                    }
                }
            }

            return IntPtr.Zero;
        }
        private static bool EnumProc(IntPtr hWnd, IntPtr lParam)
        {
            /* get a reference to the ArrayList */
            GCHandle gch = (GCHandle)lParam;
            ArrayList list = (ArrayList)(gch.Target);
            /* and add this window handle */
            list.Add(hWnd);
            return true;
        }
        /// <summary>
        /// Find the OK button on the uninstall dialog.
        /// </summary>
        /// <param name="UninstallerWindow">The pointer to
        /// the Uninstall Dialog</param>
        /// <param name="success">Whether it succeeded or not.</param>
        /// <returns>A pointer to the OK button</returns>
        private static IntPtr FindUninstallerOKButton(IntPtr UninstallerWindow, out bool success)
        {
            //max number of times to look for the button,
            //lets you out if there's a problem
            int i = 25;
            IntPtr OKButton = IntPtr.Zero;

            while (OKButton == IntPtr.Zero && i > 0)
            {
                OKButton = SearchForChildWindow(UninstallerWindow, "&OK");
                System.Threading.Thread.Sleep(500);
                i--;
            }

            if (OKButton == IntPtr.Zero)
                success = false;
            else
                success = true;

            return OKButton;
        }
        private static IntPtr SearchForChildWindow(IntPtr ParentHandle, string Caption)
        {
            ArrayList windowHandles = new ArrayList();
            /* Create a GCHandle for the ArrayList */
            GCHandle gch = GCHandle.Alloc(windowHandles);
            try
            {
                EnumChildWindows(ParentHandle, new EnumWindowsCallbackDelegate(EnumProc), (IntPtr)gch);
                /* the windowHandles array list contains all of the
                    window handles that were passed to EnumProc.  */
            }
            finally
            {
                /* Free the handle */
                gch.Free();
            }

            /* Iterate through the list and get the handle thats the best match */
            foreach (IntPtr handle in windowHandles)
            {
                StringBuilder sb = new StringBuilder(1024);
                GetWindowText((int)handle, sb, sb.Capacity);
                if (sb.Length > 0)
                {
                    if (sb.ToString().StartsWith(Caption))
                    {
                        return handle;
                    }
                }
            }

            return IntPtr.Zero;
        }
        public static void InstallNewVersion()
        {
            string url = ApplicationDeployment.CurrentDeployment.ActivationUri.ToString();//@"http://localhost/NewVersion/TestCertExp_CSharp.application";
            System.Diagnostics.Process.Start("iexplore.exe", url);
            System.Windows.Forms.Application.Exit();
            return;
        }
        #endregion

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
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception except = (Exception)e.ExceptionObject;
            FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
            Guid myKey = Guid.Empty;
            Exception myExp = new Exception();
            KMPlatform.BusinessLogic.Enums.Applications myApp = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
            int myClientId = 0;
            try
            {
                if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null)
                    myKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

                if (except != null)
                    myExp = except;

                if (FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                    myApp = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);

                if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null)
                    myClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
            }
            catch { }
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(myExp);
            alClient.Proxy.LogCriticalError(myKey, formatException, "AMS_Desktop.App.xaml.CurrentDomain_UnhandledException", myApp, string.Empty, myClientId);
        }
        public void ShutDownApplication_WithUserConfirmation()
        {
            UpdateLocks();

            MessageBoxResult r = Core_AMS.Utilities.WPF.MessageResult("User cannot log out until open batches are finalized.  Clicking Yes will automatically close your open batches.  Clicking No will require you to manually close open batches before logging out.", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, "Finalize Open Batches");
            if (r == MessageBoxResult.Yes)
            {
                CloseBatches();
                LogUserOut();
                CloseOpenWindows();
            }
            else
            {
                //Open new window for History?
                Circulation.Modules.History circHistory = new Circulation.Modules.History(true);
                Circulation.Helpers.Common.OpenCircPopoutWindow(circHistory);
                return;
            }
        }
        public void ShutDownApplication()
        {
            try
            {
                UpdateLocks();
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".UpdateLocks", app, string.Empty, logClientId);
            }
            try
            {
                CloseBatches();
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".CloseBatches", app, string.Empty, logClientId);
            }
            try
            {
                LogUserOut();
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".LogUserOut", app, string.Empty, logClientId);
            }
            try
            {
                CloseOpenWindows();
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".CloseOpenWindows", app, string.Empty, logClientId);
            }
        }
        public void CloseOpenWindows()
        {
            try
            {
                if (FrameworkUAS.Object.AppData.myAppData.OpenWindowCount > 0)
                {
                    foreach (Window win in Application.Current.Windows)
                    {
                        if (win.GetType() == typeof(Circulation.Windows.Popout))
                            win.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".CloseOpenWindows", app, string.Empty, logClientId);
            }
        }
        public void UpdateLocks()
        {
            try
            {
                FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
                FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> subscriberWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();

                if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections.ClientLiveDBConnectionString.Length > 0 ||
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections.ClientTestDBConnectionString.Length > 0)
                {
                    subscriberWorker.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, 0, false, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                }

                FrameworkUAS.Service.Response<bool> saveResp = new FrameworkUAS.Service.Response<bool>();
                for (int i = 0; i < FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.Count; i++)
                {
                    KMPlatform.Entity.Client c = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients[i];
                    saveResp = productWorker.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, c.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID);
                    if (saveResp != null && saveResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.RemoveAt(i);
                }
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null ? FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID : 0;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".UpdateLocks", app, string.Empty, logClientId);
            }
        }
        public void LogUserOut()
        {
            try
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IUserAuthorizationLog> ualWorker = FrameworkServices.ServiceClient.UAS_UserAuthorizationLogClient();
                ualWorker.Proxy.LogOut(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.UserAuthLogId);
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".LogUserOut", app, string.Empty, logClientId);
            }
        }
        public void CloseBatches()
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> worker = FrameworkServices.ServiceClient.UAD_BatchClient();
            //List<FrameworkUAD.Entity.Batch> openBatches = worker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, true, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

            try
            {
                //if (openBatches.Count > 0)
                worker.Proxy.CloseBatches(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".OnCloseActivity", app, string.Empty, logClientId);
            }

            FrameworkUAS.Object.AppData.myAppData.BatchList.Clear();

            //foreach (FrameworkUAD.Entity.Batch b in openBatches)
            //{
            //FrameworkUAS.Object.Batch batchInList = FrameworkUAS.Object.AppData.myAppData.BatchList.Where(x => x.BatchID == b.BatchID).FirstOrDefault();
            //if (batchInList != null)
            //    FrameworkUAS.Object.AppData.myAppData.BatchList.Remove(batchInList);

            //MessageBox.Show("Your batches have been finalized.");
            //}
        }
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
            Guid myKey = Guid.Empty;
            Exception myExp = new Exception();
            KMPlatform.BusinessLogic.Enums.Applications myApp = KMPlatform.BusinessLogic.Enums.Applications.AMS_Desktop;
            int myClientId = 0;
            try
            {
                if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null)
                    myKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

                if (e.Exception != null)
                    myExp = e.Exception;

                if (FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                    myApp = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);

                if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null)
                    myClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
            }
            catch { }
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(myExp);
            alClient.Proxy.LogCriticalError(myKey, formatException, "AMS_Desktop.App.xaml.Application_DispatcherUnhandledException", myApp, string.Empty, myClientId);

            ShutDownApplication();
        }
    }
}
