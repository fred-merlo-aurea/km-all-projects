using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Data;
using FrameworkUAS.Object;
using Telerik.Windows.Controls;
using System.Windows.Media.Animation;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for ApplicationManagement.xaml
    /// </summary>
    public partial class ApplicationManagement : UserControl, INotifyPropertyChanged
    {
        #region Entities/Lists
        List<string> activeTypes = new List<string>();
        List<KMPlatform.Entity.Application> apps = new List<KMPlatform.Entity.Application>();
        private ObservableItemCollection<ApplicationContainer> _applications = new ObservableItemCollection<ApplicationContainer>();
        public ObservableItemCollection<ApplicationContainer> Applications { get { return _applications; } }
        DoubleAnimation opacityAnimate = new DoubleAnimation()
        {
            To = 0.5,
            Duration = TimeSpan.FromSeconds(.4)
        };
        DoubleAnimation opacityReverseAnimate = new DoubleAnimation()
        {
            To = 1,
            Duration = TimeSpan.FromSeconds(.4)
        };
        #endregion
        #region Classes & Enums
        public class ApplicationContainer : INotifyPropertyChanged
        {
            private string _applicationName;
            private int _applicationID;
            private string _applicationCode;
            private string _description;
            private string _defaultView;
            private bool _isActive;
            private string _iconFullName;
            private string _fromEmailAddress;
            private string _toEmailAddress;
            private DateTime _dateCreated;
            private DateTime? _dateUpdated;
            private int _createdByUserID;
            private bool _infoChanged;
            private bool _isOpen;
            public string ApplicationName
            {
                get { return _applicationName; }
                set
                {
                    _applicationName = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ApplicationName"));
                    }
                }
            }
            public string ApplicationCode
            {
                get { return _applicationCode; }
                set
                {
                    _applicationCode = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ApplicationCode"));
                    }
                }
            }
            public string Description
            {
                get { return _description; }
                set
                {
                    _description = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                    }
                }
            }
            public string DefaultView
            {
                get { return _defaultView; }
                set
                {
                    _defaultView = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DefaultView"));
                    }
                }
            }
            public bool IsActive
            {
                get { return _isActive; }
                set
                {
                    _isActive = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsActive"));
                    }
                }
            }
            public string IconFullName
            {
                get { return _iconFullName; }
                set
                {
                    _iconFullName = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IconFullName"));
                    }
                }
            }
            public string DateCreated
            {
                get { return _dateCreated.ToShortDateString(); }
                set
                {
                    DateTime dt = DateTime.MinValue;
                    DateTime.TryParse(value, out dt);
                    _dateCreated = dt;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateCreated"));
                    }
                }
            }
            public string DateUpdated
            {
                get { return (_dateUpdated ?? DateTime.MinValue).ToShortDateString(); }
                set
                {
                    DateTime dt = DateTime.MinValue;
                    DateTime.TryParse(value, out dt);
                    _dateUpdated = dt;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateUpdated"));
                    }
                }
            }
            public bool InfoChanged
            {
                get { return _infoChanged; }
                set
                {
                    _infoChanged = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("InfoChanged"));
                    }
                }
            }
            public bool IsOpen
            {
                get { return _isOpen; }
                set
                {
                    _isOpen = value;
                    if (_isOpen == false)
                        RevertChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsOpen"));
                    }
                }
            }
            public KMPlatform.Entity.Application OriginalApplication { get; set; }
            public KMPlatform.Entity.Application GetApplicationEntity()
            {
                KMPlatform.Entity.Application rtnItem = new KMPlatform.Entity.Application();
                rtnItem.ApplicationID = _applicationID;
                rtnItem.ApplicationName = _applicationName;
                rtnItem.ApplicationCode = _applicationCode;
                rtnItem.Description = _description;
                rtnItem.DefaultView = _defaultView;
                rtnItem.IsActive = _isActive;
                rtnItem.IconFullName = _iconFullName;
                rtnItem.FromEmailAddress = _fromEmailAddress;
                rtnItem.ErrorEmailAddress = _toEmailAddress;

                if(_applicationID > 0)
                {
                    rtnItem.DateUpdated = DateTime.Now;
                    rtnItem.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                }
                else
                {
                    rtnItem.DateCreated = DateTime.Now;
                    rtnItem.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                }

                return rtnItem;
            }
            public void CheckChanges()
            {
                bool infoChanged = false;
                if (_applicationName != OriginalApplication.ApplicationName)
                    infoChanged = true;
                if (_applicationCode != OriginalApplication.ApplicationCode)
                    infoChanged = true;
                if (_description != OriginalApplication.Description)
                    infoChanged = true;
                if (_defaultView != OriginalApplication.DefaultView)
                    infoChanged = true;
                if (_isActive != OriginalApplication.IsActive)
                    infoChanged = true;
                if (_iconFullName != OriginalApplication.IconFullName)
                    infoChanged = true;

                this.InfoChanged = infoChanged;
            }
            public void RevertChanges()
            {
                this.ApplicationName = OriginalApplication.ApplicationName;
                this.ApplicationCode = OriginalApplication.ApplicationCode;
                this.Description = OriginalApplication.Description;
                this.DefaultView = OriginalApplication.DefaultView;
                this.IsActive = OriginalApplication.IsActive;
                this.IconFullName = OriginalApplication.IconFullName;
            }

            public ApplicationContainer(KMPlatform.Entity.Application app)
            {
                _applicationID = app.ApplicationID;
                _applicationName = app.ApplicationName;
                _applicationCode = app.ApplicationCode;
                _description = app.Description;
                _defaultView = app.DefaultView;
                _isActive = app.IsActive;
                _iconFullName = app.IconFullName;
                _fromEmailAddress = app.FromEmailAddress;
                _toEmailAddress = app.ErrorEmailAddress;
                _dateCreated = app.DateCreated;
                _createdByUserID = app.CreatedByUserID;
                _dateUpdated = app.DateUpdated;
                _isOpen = false;

                this.OriginalApplication = app;
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        public enum ActiveType
        {
            All,
            Active,
            Not_Active
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Services and Responses
        FrameworkServices.ServiceClient<UAS_WS.Interface.IApplication> appW = FrameworkServices.ServiceClient.UAS_ApplicationClient();
        FrameworkUAS.Service.Response<int> saveResponse = new FrameworkUAS.Service.Response<int>();
        #endregion
        public ApplicationManagement()
        {
            Window parentWindow = Application.Current.MainWindow;
            if (AppData.CheckParentWindowUid(parentWindow.Uid))
            {
                if (AppData.IsKmUser() == true)
                {
                    InitializeComponent();
                    LoadData();
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageAccessDenied();
                }
            }
        }
        private void LoadData()
        {
            activeTypes.Add(ActiveType.All.ToString());
            activeTypes.Add(ActiveType.Active.ToString());
            activeTypes.Add(ActiveType.Not_Active.ToString().Replace("_", " "));
            lstActive.ItemsSource = activeTypes;
            apps = appW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            foreach (KMPlatform.Entity.Application a in apps.OrderBy(x => x.ApplicationName))
            {
                this.Applications.Add(new ApplicationContainer(a));
            }
        }
        #region New Application
        private void rbNewApp_Click(object sender, RoutedEventArgs e)
        {
            grdNewApp.DataContext = null;
            ApplicationContainer app = new ApplicationContainer(new KMPlatform.Entity.Application());
            grdNewApp.DataContext = app;
            grdExistingApps.IsEnabled = false;
            grdExistingApps.BeginAnimation(Grid.OpacityProperty, opacityAnimate);
            grdNewApp.Opacity = 1; 
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            ApplicationContainer appC = btn.DataContext as ApplicationContainer;
            KMPlatform.Entity.Application app = appC.GetApplicationEntity();

            saveResponse = appW.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, app);
            if (saveResponse.Result > 0 && saveResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                Core_AMS.Utilities.WPF.Message("Save successful.");
                app.ApplicationID = saveResponse.Result;
                this.Applications.Add(new ApplicationContainer(app));
            }
            else
                Core_AMS.Utilities.WPF.MessageError("There was a problem saving the Application.");

            CloseWindow();
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
        private void CloseWindow()
        {
            grdExistingApps.IsEnabled = true;
            grdExistingApps.BeginAnimation(Grid.OpacityProperty, opacityReverseAnimate);
            grdNewApp.Opacity = 0;
            grdApplication.SelectedItem = null;
        }             
        #endregion
        #region Existing Apps
        private void lstActive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            this.Applications.Clear();
            string item = lb.SelectedItem as string;
            if (item == ActiveType.All.ToString())
            {
                foreach (KMPlatform.Entity.Application a in apps)
                    this.Applications.Add(new ApplicationContainer(a));
            }
            else if (item == ActiveType.Active.ToString())
            {
                foreach (KMPlatform.Entity.Application a in apps.Where(x => x.IsActive == true))
                    this.Applications.Add(new ApplicationContainer(a));
            }
            else
            {
                foreach (KMPlatform.Entity.Application a in apps.Where(x => x.IsActive == false))
                    this.Applications.Add(new ApplicationContainer(a));
            }
        }
        private void btnSaveExisting_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            ApplicationContainer appC = btn.DataContext as ApplicationContainer;
            KMPlatform.Entity.Application app = appC.GetApplicationEntity();

            saveResponse = appW.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, app);
            if (saveResponse.Result > 0 && saveResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                Core_AMS.Utilities.WPF.Message("Save successful.");
                appC.OriginalApplication = app;
            }
            else
                Core_AMS.Utilities.WPF.MessageError("There was a problem saving the Application.");

            appC.IsOpen = false;
            grdApplication.SelectedItem = null;
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            ApplicationContainer app = btn.DataContext as ApplicationContainer;
            app.RevertChanges();
        }
        public void grdApplication_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            foreach (ApplicationContainer ac in this.Applications.Where(x => x.IsOpen))
                ac.IsOpen = false;

            ApplicationContainer a = grdApplication.SelectedItem as ApplicationContainer;
            if (a != null)
            {
                grdApplication.ScrollIntoView(a);
                grdApplication.UpdateLayout();
                a.IsOpen = true;
            }
        }
        #endregion
    }
}
