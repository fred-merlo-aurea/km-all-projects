using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for AdhocFilterCreation.xaml
    /// </summary>
    public partial class AdhocFilter : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriptionsExtensionMapper> semWorker = FrameworkServices.ServiceClient.UAD_SubscriptionsExtensionMapperClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IAdhoc> aWorker = FrameworkServices.ServiceClient.UAD_AdhocClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriptionsExtensionMapper>> svSEM = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriptionsExtensionMapper>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>> svAdhocs = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Adhoc>>();

        FrameworkUAD.Entity.SubscriptionsExtensionMapper currentItem = new FrameworkUAD.Entity.SubscriptionsExtensionMapper();
        List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> allItems = new List<FrameworkUAD.Entity.SubscriptionsExtensionMapper>();
        List<FrameworkUAD.Entity.Adhoc> adhocs = new List<FrameworkUAD.Entity.Adhoc>();
        #endregion
        public AdhocFilter()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadData();                
            LoadCombo();
            currentItem = null;            
        }

        #region Loads
        public void LoadData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svSEM = semWorker.Proxy.SelectAll(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svAdhocs = aWorker.Proxy.SelectAll(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load                
                if (svSEM.Result != null && svSEM.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allItems = svSEM.Result;
                    gridGroups.ItemsSource = allItems;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();                    
                }
                if (svAdhocs.Result != null && svAdhocs.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    adhocs = svAdhocs.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
                #endregion

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }        

        public void LoadCombo()
        {            
            Dictionary<string, string> dataTypes = new Dictionary<string,string>();
            dataTypes.Add("varchar","varchar");
            dataTypes.Add("int","int");
            dataTypes.Add("float","float");
            dataTypes.Add("smalldatetime","smalldatetime");
            dataTypes.Add("datetime","datetime");
            dataTypes.Add("bit","bit");

            cbDataType.ItemsSource = null;
            cbDataType.ItemsSource = dataTypes;
            cbDataType.SelectedValuePath = "Key";
            cbDataType.DisplayMemberPath = "Value";
        }
        #endregion
        #region Button Clicks
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.SubscriptionsExtensionMapper))
                {
                    FrameworkUAD.Entity.SubscriptionsExtensionMapper semItem = (FrameworkUAD.Entity.SubscriptionsExtensionMapper)b.DataContext;
                    if (semItem != null)
                    {
                        currentItem = semItem;
                        tbxName.Text = semItem.CustomField.ToString();
                        cbDataType.SelectedValue = semItem.CustomFieldDataType.ToString();
                        cbxActive.IsChecked = semItem.Active;
                        btnSave.Tag = semItem.SubscriptionsExtensionMapperID.ToString();
                        btnSave.Content = "Update";
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            busy.IsBusy = true;
            bool hasError = false;
            //Create Object for Save|Update
            #region Input Checks
            //Checks Name provided and if new that name not used already
            string customField = "";
            if (!string.IsNullOrEmpty(tbxName.Text))
            {
                if (currentItem == null && allItems.Select(x => x.CustomField).Contains(tbxName.Text))
                {
                    Core_AMS.Utilities.WPF.Message("Adhoc Name already exists. Please choose a different adhoc name before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                    busy.IsBusy = false;
                    return;
                }
                else
                    customField = tbxName.Text;
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Adhoc Name wasn't selected. Please fill out before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                busy.IsBusy = false;
                return;
            }

            //Check Data Type Selected
            string customFieldDataType = "";
            if (cbDataType.SelectedValue != null)
                customFieldDataType = cbDataType.SelectedValue.ToString();
            else
            {
                Core_AMS.Utilities.WPF.Message("Data Type wasn't selected. Please fill out before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion

            #region Remove/Update Adhoc if used
            if(currentItem != null && adhocs.Select(x=> x.AdhocName.ToLower()).Contains(currentItem.CustomField.ToLower()))
            {
                MessageBoxResult result = MessageBox.Show("This will update any current AdHoc Categories using this AdHoc field. Do you want to continue?", "Warning", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes)
                {
                    FrameworkUAD.Entity.Adhoc adhoc = adhocs.Where(x => x.AdhocName.ToLower() == currentItem.CustomField.ToLower()).SingleOrDefault();
                    if(adhoc != null)
                    {
                        if (currentItem.Active == false)
                            aWorker.Proxy.Delete_AdHoc(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, adhoc.AdhocID);
                        else
                        {
                            adhoc.AdhocName = customField.ToUpper();
                            aWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, adhoc);
                        }
                    }
                }
                else
                {
                    busy.IsBusy = false;
                    return;
                }
            }
            #endregion

            #region Prepare Item
            if (currentItem != null)
            {
                currentItem.CustomField = customField;
                currentItem.CustomFieldDataType = customFieldDataType;                
                currentItem.Active = cbxActive.IsChecked.Value;
            }
            else
            {
                currentItem = new FrameworkUAD.Entity.SubscriptionsExtensionMapper();
                currentItem.CustomField = customField;
                currentItem.CustomFieldDataType = customFieldDataType;
                currentItem.Active = cbxActive.IsChecked.Value;
                int fields = 1;
                if (allItems != null)
                {
                    List<string> stringFieldNumbers = allItems.Select(x => x.StandardField.Replace("Field", "")).ToList();
                    try
                    {
                        List<int> intFieldNumbers = stringFieldNumbers.Select(int.Parse).ToList();
                        int last = intFieldNumbers.Max();
                        fields = last + 1;                        
                    }
                    catch (Exception ex)
                    {
                        Core_AMS.Utilities.WPF.Message("Issue determining next available field.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");
                        hasError = true;
                    }
                }

                currentItem.StandardField = "Field" + fields.ToString();
                int id = 0;
                if (btnSave.Tag != null)
                    int.TryParse(btnSave.Tag.ToString(), out id);

                currentItem.SubscriptionsExtensionMapperID = id;
            }
            #endregion

            if (!hasError)
            {
                int saveID = 0;
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (o, ea) =>
                {
                    //Save|Update
                    saveID = semWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, currentItem).Result;
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    #region Refresh|Clear
                    if (saveID > 0)
                    {
                        //Set currentItem to null
                        currentItem = null;

                        //Change Button tag to zero content back to save
                        btnSave.Tag = "0";
                        btnSave.Content = "Save";

                        //Clear control
                        tbxName.Text = "";
                        cbDataType.SelectedIndex = -1;
                        cbxActive.IsChecked = false;

                        //Refresh Grid
                        LoadData();

                        Core_AMS.Utilities.WPF.MessageSaveComplete();
                    }
                    else
                        Core_AMS.Utilities.WPF.Message("Save failed.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");

                    #endregion

                    busy.IsBusy = false;
                };
                bw.RunWorkerAsync();
            }
            else
                busy.IsBusy = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Change Button tag to zero content back to save
            btnSave.Tag = "0";
            btnSave.Content = "Save";

            //Set currentItem to null
            currentItem = null;

            //Clear control
            tbxName.Text = "";
            cbDataType.SelectedIndex = -1;
            cbxActive.IsChecked = false;
        }
        #endregion
    }
}
