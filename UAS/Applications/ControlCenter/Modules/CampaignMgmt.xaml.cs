using FrameworkUAS.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;
using System.Windows.Media;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for CampaignMgmt.xaml
    /// </summary>
    public partial class CampaignMgmt : UserControl
    {
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ICampaign> campaignWorker { get; set; }
        private List<KMPlatform.Entity.User> kmUsers { get; set; }        
        public List<FrameworkUAD.Entity.Campaign> campaigns { get; set; }
        public List<FrameworkUAD.Entity.Brand> brands { get; set; }
        private List<CampaignContainer> campaignContainer = new List<CampaignContainer>();
        private FrameworkUAD.Entity.Campaign originalCampaign = new FrameworkUAD.Entity.Campaign();
        private CampaignContainer currRow = new CampaignContainer();
        public CampaignMgmt()
        {
            if (FrameworkUAS.Object.AppData.IsKmUser() == true)
            {
            Window parentWindow = Application.Current.MainWindow;
            if (AppData.CheckParentWindowUid(parentWindow.Uid))
            {
                //only want this available to users that belong to KM
                if (AppData.IsKmUser() == true)
                {
                    InitializeComponent();
                    LoadData();
                    this.DataContext = this;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageAccessDenied();
                }
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageAccessDenied();
            }
        }
        private void LoadData()
        {
            //startUp = false;
            SortDescriptor sort = new SortDescriptor();
            sort.Member = "CampaignName";
            sort.SortDirection = ListSortDirection.Ascending;
            grdCampaign.SortDescriptors.Add(sort);
            campaignWorker = FrameworkServices.ServiceClient.UAD_CampaignClient();
            campaigns = campaignWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

            FrameworkServices.ServiceClient<UAD_WS.Interface.IBrand> brandWorker = FrameworkServices.ServiceClient.UAD_BrandClient();
            brands = brandWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            
            if (brands != null)
            {
                cbBrand.ItemsSource = brands;
                cbBrand.SelectedValuePath = "BrandID";
                cbBrand.DisplayMemberPath = "BrandName";
            }

            KMPlatform.Entity.ClientGroup cgKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.SingleOrDefault(x => x.ClientGroupName.Equals("Knowledge Marketing"));

            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            kmUsers = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cgKM.ClientGroupID, false).Result; 

            foreach (FrameworkUAD.Entity.Campaign campaign in campaigns)
            {
                CampaignContainer cc = new CampaignContainer(campaign);

                KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == campaign.AddedBy);
                if (user != null)
                    cc.CreatedByName = user.FullName;

                user = kmUsers.FirstOrDefault(x => x.UserID == campaign.UpdatedBy);
                if (user != null)
                    cc.UpdatedByName = user.FullName;

                FrameworkUAD.Entity.Brand brand = brands.FirstOrDefault(x => x.BrandID == campaign.BrandID);
                if (brand != null)
                    cc.BrandName = brand.BrandName;

                campaignContainer.Add(cc);
            }

            grdCampaign.ItemsSource = campaignContainer;           
        }        
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            RadButton thisBtn = ((RadButton)sender);

            var rows = this.grdCampaign.ChildrenOfType<GridViewRow>();
            foreach (GridViewRow row in rows)
            {
                if (row.DetailsVisibility == Visibility.Visible)
                {
                    row.DetailsVisibility = Visibility.Collapsed;
                    break;
                }
            }
            this.grdCampaign.SelectedItem = null;
        }
        private void grdCampaign_RowDetailsVisibilityChanged(object sender, GridViewRowDetailsEventArgs e)
        {
            CampaignContainer tmpCampaign = (CampaignContainer)e.DetailsElement.DataContext;
            Grid grd = e.DetailsElement.FindName("ItemDetails") as Grid;
            grdCampaign.ItemsSource = campaignContainer;

            List<RadComboBox> rcbList = grd.ChildrenOfType<RadComboBox>().ToList();

            foreach (RadComboBox rcb in rcbList)
            {
                if (rcb.Name == "cbBBrand")
                {
                    rcb.ItemsSource = brands;
                    rcb.SelectedValuePath = "BrandID";
                    rcb.DisplayMemberPath = "BrandName";
                }
            }

            if (e.Visibility == Visibility.Visible)
            {
                #region Get Campagin Variables
                originalCampaign.CampaignID = tmpCampaign.CampaignID;
                originalCampaign.CampaignName = tmpCampaign.CampaignName;
                originalCampaign.AddedBy = tmpCampaign.AddedBy;
                originalCampaign.DateAdded = tmpCampaign.DateAdded;
                originalCampaign.UpdatedBy = tmpCampaign.UpdatedBy;
                originalCampaign.DateUpdated = tmpCampaign.DateUpdated;
                originalCampaign.BrandID = tmpCampaign.BrandID;
                #endregion

                currRow = tmpCampaign;
            }
        }

        private string GetControlValue(List<ControlObject> obj, string ctrlName)
        {
            var item = obj.SingleOrDefault(i => i.Name == ctrlName);

            if (item == null || item.Value == null)
                return "";
            else
                return item.Value;

        }
        public class ControlObject
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        private Boolean infoChanged(List<ControlObject> o)
        {
            Boolean infoChanged = false;
            if (!originalCampaign.CampaignName.ToString().Equals(GetControlValue(o, "tbCamName").Trim()))
            {
                infoChanged = true;
            }

            if (!originalCampaign.BrandID.ToString().Equals(GetControlValue(o, "cbBBrand").Trim()))
            {
                infoChanged = true;
            }

            return infoChanged;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            RadButton thisBtn = ((RadButton)sender);
            List<ControlObject> objects = new List<ControlObject>();
            objects.AddRange(RadGridInformation(thisBtn));

            if (!infoChanged(objects))
            {
                Core_AMS.Utilities.WPF.MessageError("No changes were made.");
                grdCampaign.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            }
            else
            {
                //EditAction == Commit
                //save changes
                CampaignContainer myCampaign = currRow;
                #region Check Values
                if (string.IsNullOrEmpty(myCampaign.CampaignName))
                {
                    Core_AMS.Utilities.WPF.MessageError("Current campaign name cannot be blank.");
                    return;
                }
                if (campaigns != null && campaigns.FirstOrDefault(x => x.CampaignID == myCampaign.CampaignID).CampaignName != myCampaign.CampaignName)
                {
                    if (campaigns.FirstOrDefault(x => x.CampaignName.Equals(myCampaign.CampaignName, StringComparison.CurrentCultureIgnoreCase) && x.CampaignID != myCampaign.CampaignID) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Current campaign name has been used. Please provide a unique campaign name.");
                        return;
                    }
                }
                #endregion
                FrameworkUAD.Entity.Campaign saveCampaign = new FrameworkUAD.Entity.Campaign();
                #region Get Campagin Variables
                saveCampaign.CampaignID = myCampaign.CampaignID;
                saveCampaign.CampaignName = myCampaign.CampaignName;
                saveCampaign.AddedBy = myCampaign.AddedBy;
                saveCampaign.DateAdded = myCampaign.DateAdded;
                saveCampaign.UpdatedBy = myCampaign.UpdatedBy;
                saveCampaign.DateUpdated = myCampaign.DateUpdated;
                saveCampaign.BrandID = myCampaign.BrandID;
                #endregion

                campaignWorker = FrameworkServices.ServiceClient.UAD_CampaignClient();
                myCampaign.UpdatedBy = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                myCampaign.DateUpdated = DateTime.Now;
                saveCampaign.UpdatedBy = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                saveCampaign.DateUpdated = DateTime.Now;
                campaignWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, saveCampaign);

                KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == myCampaign.UpdatedBy);
                if (user != null)
                    myCampaign.UpdatedByName = user.FullName;

                FrameworkUAD.Entity.Brand brand = brands.FirstOrDefault(x => x.BrandID == myCampaign.BrandID);
                if (brand != null)
                    myCampaign.BrandName = brand.BrandName;

                campaigns = campaignWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                
                var rows = this.grdCampaign.ChildrenOfType<GridViewRow>();
                foreach (GridViewRow row in rows)
                {
                    if (row.DetailsVisibility == Visibility.Visible)
                    {
                        row.DetailsVisibility = Visibility.Collapsed;
                        break;
                    }
                }
                grdCampaign.Rebind();
                grdCampaign.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            }
        }
        //private void grdCampaign_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        //{
        //    if (e.Row is GridViewRow && !(e.Row is GridViewNewRow) && kmUsers != null)
        //    {
        //        FrameworkUAD.Entity.Campaign campaign = e.DataElement as FrameworkUAD.Entity.Campaign;
        //        KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == campaign.AddedBy);

        //        TextBlock tbCreatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbCreatedByName");
        //        TextBlock tbUpdatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbUpdatedByName");

        //        if (tbCreatedBy != null)
        //        {
        //            if (user != null)
        //            {
        //                if (campaign.UpdatedBy != 0 && campaign.AddedBy == campaign.UpdatedBy)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = user.FullName;
        //                }
        //                else if (campaign.UpdatedBy != 0 && campaign.AddedBy != campaign.UpdatedBy)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = string.Empty;
        //                }
        //                else if (campaign.UpdatedBy != 0 || campaign.AddedBy != campaign.UpdatedBy)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    KMPlatform.Entity.User updatedUser = kmUsers.FirstOrDefault(x => x.UserID == campaign.UpdatedBy);
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = updatedUser.FullName;
        //                }
        //            }
        //        }
        //    }
        //}
        //private void rdForm_Loaded(object sender, RoutedEventArgs e)
        //{
        //    RadDataForm rdForm = sender as RadDataForm;

        //    FrameworkUAD.Entity.Campaign myCampaign = (FrameworkUAD.Entity.Campaign)rdForm.DataContext;            
        //}

        private void rbNewCampaign_Click(object sender, RoutedEventArgs e)
        {
            this.Background = System.Windows.Media.Brushes.DarkGray;            
            rbNewCampaign.IsEnabled = false;
            grdCampaign.IsEnabled = false;
            grdCampaign.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;            
        }
        //private void grdCampaign_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        //{
        //    FrameworkUAD.Entity.Campaign myCampaign = (FrameworkUAD.Entity.Campaign)e.NewObject;
        //}
        #region Add new service

        private void ResetNewWindow()
        {
            tbCampaignName.Clear();
            cbBrand.SelectedIndex = -1;
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            FrameworkUAD.Entity.Campaign newCampaign = new FrameworkUAD.Entity.Campaign();

            newCampaign.CampaignName = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbCampaignName.Text.Trim());
            int brandId = 0;
            int.TryParse(cbBrand.SelectedValue.ToString(), out brandId);

            #region Check Values
            if (string.IsNullOrEmpty(newCampaign.CampaignName))
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a campaign name.");
                return;
            }
            else
            {
                if (campaigns.FirstOrDefault(x => x.CampaignName.Equals(newCampaign.CampaignName, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.MessageError("Must provide a unique campaign name that hasn't been used.");
                    return;
                }
            }
           
            #endregion                                    
            newCampaign.BrandID = brandId;            
            newCampaign.AddedBy = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newCampaign.DateAdded = DateTime.Now;

            newCampaign.CampaignID = campaignWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, newCampaign).Result;
            if (newCampaign.CampaignID > 0)
            {
                ResetNewWindow();

                CampaignContainer cc = new CampaignContainer(newCampaign);

                KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == newCampaign.AddedBy);
                if (user != null)
                    cc.CreatedByName = user.FullName;

                FrameworkUAD.Entity.Brand brand = brands.FirstOrDefault(x => x.BrandID == newCampaign.BrandID);
                if (brand != null)
                    cc.BrandName = brand.BrandName;

                campaignContainer.Add(cc);
                grdCampaign.ItemsSource = null;
                grdCampaign.ItemsSource = campaignContainer;

                campaigns = campaignWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                CloseWindow();
                this.grdCampaign.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();
            
        }
        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;            
            rbNewCampaign.IsEnabled = true;
            grdCampaign.IsEnabled = true;
            grdCampaign.Background = System.Windows.Media.Brushes.Transparent;

            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            CloseWindow();
            this.grdCampaign.SelectedItem = null;
        }
        #endregion

        private DependencyObject FindParentControl<T>(DependencyObject control)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(control);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }

        private List<ControlObject> RadGridInformation(DependencyObject control)
        {
            List<ControlObject> objects = new List<ControlObject>();
            object a = FindParentControl<Grid>(control);
            if (a != null && a.GetType() == typeof(Grid))
            {
                Grid aa = (Grid)a;
                foreach (object dobj in aa.Children)
                {
                    //object o = FindChildControl<StackPanel>(aa);
                    if (dobj != null && dobj.GetType() == typeof(StackPanel))
                    {
                        StackPanel sp = (StackPanel)dobj;
                        //object z;

                        foreach (object spDObj in sp.Children)
                        {
                            //z = FindChildControl<Object>(spDObj);
                            if (spDObj != null && spDObj.GetType() == typeof(TextBox))
                            {
                                TextBox t = (TextBox)spDObj;
                                objects.Add(new ControlObject() { Name = t.Name, Value = t.Text });
                            }
                            else if (spDObj != null && spDObj.GetType() == typeof(RadComboBox))
                            {
                                string value = "";
                                RadComboBox cb = (RadComboBox)spDObj;
                                if (cb.SelectedValue == null)
                                    value = "";
                                else
                                    value = cb.SelectedValue.ToString();

                                objects.Add(new ControlObject() { Name = cb.Name, Value = value });
                            }
                        }
                    }
                }
            }

            return objects;
        }
    }

    [Serializable]
    [DataContract]
    public class CampaignContainer
    {
        public CampaignContainer() { }
        public CampaignContainer(FrameworkUAD.Entity.Campaign campaign)
        {
            CampaignID = campaign.CampaignID;
            CampaignName = campaign.CampaignName;
            AddedBy = campaign.AddedBy;
            DateAdded = campaign.DateAdded;
            UpdatedBy = campaign.UpdatedBy;
            DateUpdated = campaign.DateUpdated;
            BrandID = campaign.BrandID;
            BrandName = string.Empty;
            CreatedByName = string.Empty;
            UpdatedByName = string.Empty;
        }
        #region Properties
        [DataMember]
        public int CampaignID { get; set; }
        [DataMember]
        public string CampaignName { get; set; }
        [DataMember]
        public int AddedBy { get; set; }
        [DataMember]
        public DateTime DateAdded { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int? BrandID { get; set; }
        [DataMember]
        public string CreatedByName { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        #endregion
    }
}
