using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for CircRulesMgmt.xaml
    /// </summary>
    public partial class CircRulesMgmt : UserControl
    {
        #region SERVICE CALLS
        //FrameworkServices.ServiceClient<UAS_WS.Interface.IFileRule> rulesWorker = FrameworkServices.ServiceClient.UAS_FileRuleClient();
        #endregion
        #region VARIABLES
        //FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>> svRules = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>>();

        //FrameworkUAS.Entity.FileRule currentItem = new FrameworkUAS.Entity.FileRule();
        //List<FrameworkUAS.Entity.FileRule> allItems = new List<FrameworkUAS.Entity.FileRule>();
        #endregion

        public CircRulesMgmt()
        {
            if (FrameworkUAS.Object.AppData.IsKmUser() == true)
            {
            InitializeComponent();
            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null)
            {
                LoadData();                
                //currentItem = null;
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
            }
        }
            else
            {
                Core_AMS.Utilities.WPF.MessageAccessDenied();
            }
        }

        public void LoadData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                //svRules = rulesWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load SubscriptionsExtensionMapper                
                //if (svRules.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                //    allItems = svRules.Result;
                //    gridRules.ItemsSource = allItems;
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();                    
                //}
                #endregion

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }   

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                //if (b.DataContext.GetType() == typeof(FrameworkUAS.Entity.FileRule))
                //{
                //    FrameworkUAS.Entity.FileRule ruleItem = (FrameworkUAS.Entity.FileRule)b.DataContext;
                //    if (ruleItem != null)
                //    {
                //        currentItem = ruleItem;
                //        tbxRuleName.Text = ruleItem.RuleName.ToString();
                //        tbxSproc.Text = ruleItem.RuleMethod.ToString();
                //        tbxDisplayName.Text = ruleItem.DisplayName.ToString();
                //        tbxDesc.Text = ruleItem.Description.ToString();
                //        cbxActive.IsChecked = ruleItem.IsActive;
                //        btnSave.Tag = ruleItem.FileRuleID.ToString();
                //        btnSave.Content = "Update";
                //    }
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                //}
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            busy.IsBusy = true;

            #region Check Values
            #region RuleName
            string ruleName = "";
            if (!string.IsNullOrEmpty(tbxRuleName.Text))
            {

                //if (currentItem == null && allItems.SingleOrDefault(x => x.RuleName.Equals(tbxRuleName.Text.ToString(), StringComparison.CurrentCultureIgnoreCase)) == null)                
                //{
                //    ruleName = tbxRuleName.Text.ToString();
                //}
                //else if (currentItem != null &&
                //    allItems.SingleOrDefault(x => x.RuleName.Equals(tbxRuleName.Text.ToString(), StringComparison.CurrentCultureIgnoreCase)) != null &&
                //    allItems.SingleOrDefault(x => x.RuleName.Equals(tbxRuleName.Text.ToString(), StringComparison.CurrentCultureIgnoreCase)).FileRuleID == currentItem.FileRuleID)
                //{
                //    ruleName = tbxRuleName.Text.ToString();
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.Message("Rule Name already exists. Please choose a different adhoc name before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                //    return;
                //}                
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Rule Name not detected. Please fill out before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion
            #region DisplayName
            string displayName = "";
            if (!string.IsNullOrEmpty(tbxDisplayName.Text))
            {

                //if (currentItem == null && allItems.SingleOrDefault(x => x.DisplayName.Equals(tbxDisplayName.Text.ToString(), StringComparison.CurrentCultureIgnoreCase)) == null)                
                //{
                //    displayName = tbxDisplayName.Text.ToString();
                //}
                //else if (currentItem != null &&
                //    allItems.SingleOrDefault(x => x.DisplayName.Equals(tbxDisplayName.Text.ToString(), StringComparison.CurrentCultureIgnoreCase)) != null &&
                //    allItems.SingleOrDefault(x => x.DisplayName.Equals(tbxDisplayName.Text.ToString(), StringComparison.CurrentCultureIgnoreCase)).FileRuleID == currentItem.FileRuleID)
                //{
                //    displayName = tbxDisplayName.Text.ToString();
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.Message("Display Name already exists. Please choose a different adhoc name before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                //    return;
                //}                
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Display Name not detected. Please fill out before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion
            #region Sproc
            string sproc = "";
            if (!string.IsNullOrEmpty(tbxSproc.Text))
            {                
                sproc = tbxSproc.Text.ToString();                                
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Stored Procedure not detected. Please fill out before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion
            #region Description
            string desc = "";
            if (!string.IsNullOrEmpty(tbxDesc.Text))
            {                
                desc = tbxDesc.Text.ToString();                                
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Description not detected. Please fill out before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion
            #endregion

            #region Setup Rule
            //if (currentItem != null)
            //{
            //    currentItem.RuleName = ruleName;
            //    currentItem.RuleMethod = sproc;
            //    currentItem.DisplayName = displayName;
            //    currentItem.Description = desc;
            //    currentItem.ExecutionPointId = 0;
            //    currentItem.ProcedureTypeId = 0;
            //    currentItem.IsActive = cbxActive.IsChecked.Value;
            //    currentItem.DateCreated = DateTime.Now;
            //    currentItem.DateUpdated = DateTime.Now;
            //    currentItem.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //    currentItem.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //}
            //else
            //{
            //    currentItem = new FrameworkUAS.Entity.FileRule();
            //    currentItem.RuleName = ruleName;
            //    currentItem.RuleMethod = sproc;
            //    currentItem.DisplayName = displayName;
            //    currentItem.Description = desc;
            //    currentItem.ExecutionPointId = 0;
            //    currentItem.ProcedureTypeId = 0;
            //    currentItem.IsActive = cbxActive.IsChecked.Value;
            //    currentItem.DateCreated = DateTime.Now;
            //    currentItem.DateUpdated = DateTime.Now;
            //    currentItem.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //    currentItem.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;

            //    int id = 0;
            //    if (btnSave.Tag != null)
            //        int.TryParse(btnSave.Tag.ToString(), out id);

            //    currentItem.FileRuleID = id;
            //}
            #endregion

            #region Save
            int saveID = 0;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                //Save|Update
                //saveID = semWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient, currentItem).Result;
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Refresh|Clear
                if (saveID > 0)
                {
                    //Set currentItem to null
                    //currentItem = null;

                    //Change Button tag to zero content back to save
                    btnSave.Tag = "0";
                    btnSave.Content = "Save";

                    //Clear control
                    //tbxName.Text = "";                    
                    cbxActive.IsChecked = false;

                    //Refresh Grid
                    //LoadData();

                    Core_AMS.Utilities.WPF.MessageSaveComplete();
                }
                else
                    Core_AMS.Utilities.WPF.Message("Save failed.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");

                #endregion

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
            #endregion
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Change Button tag to zero content back to save
            btnSave.Tag = "0";
            btnSave.Content = "Save";

            //Set currentItem to null            
            //currentItem = null;

            tbxRuleName.Text = "";
            tbxSproc.Text = "";
            tbxDisplayName.Text = "";
            tbxDesc.Text = "";
            cbxActive.IsChecked = false;
        }
    }
}
