using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using FileMapperWizard.Helpers;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for DCDemoAttributes.xaml
    /// </summary>
    public partial class DCDemoAttributes : UserControl
    {
        private const string StepFiveContainer = "StepFiveContainer";
        FileMapperWizard.Modules.DataCompareSteps thisDCSteps { get; set; }
        List<FrameworkUAD_Lookup.Entity.Code> demoStandards { get; set; }
        List<FrameworkUAD_Lookup.Entity.Code> demoPremiums { get; set; }
        //List<FrameworkUAS.Entity.Code> demoCustom { get; set; }
        bool didSave { get; set; }

        public DCDemoAttributes(FileMapperWizard.Modules.DataCompareSteps dcSteps)
        {
            thisDCSteps = dcSteps;
            InitializeComponent();
            didSave = false;
            ObservableCollection<WpfControls.Helpers.CheckBoxItem> itemList = new ObservableCollection<WpfControls.Helpers.CheckBoxItem>
            {
                new WpfControls.Helpers.CheckBoxItem { IsChecked = false, Name="None" },
                new WpfControls.Helpers.CheckBoxItem { IsChecked = false, Name="All" },
                new WpfControls.Helpers.CheckBoxItem { IsChecked = false, Name="Custom" },
            };
            if (!string.IsNullOrEmpty(thisDCSteps.demoSelection))
            {
                foreach (WpfControls.Helpers.CheckBoxItem cbi in itemList)
                {
                    if (cbi.Name.Equals(thisDCSteps.demoSelection))
                        cbi.IsChecked = true;
                }
            }
            ListBox.ItemsSource = new WpfControls.Helpers.CheckBoxListModel(itemList).ItemsSource;

            try
            {
                //call db job to make sure all Demographics are in the Code table
                //exec job_DataCompareOptionCodeMap_UpdateWithCodes
                //FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareOptionCodeMap> worker = FrameworkServices.ServiceClient.UAS_DataCompareOptionCodeMapClient();
                //FrameworkUAS.Service.Response<bool> resp = new FrameworkUAS.Service.Response<bool>();
                //resp = worker.Proxy.UpdateWithCodes(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);

            }
            catch (Exception ex)
            {
                //Core.ADMS.Logging log = new Core.ADMS.Logging();
                //log.LogIssue(ex);

                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".DCDemoAttributes", app, string.Empty, logClientId);
            }

            if (thisDCSteps.demoAttributes != null)
            {
                LoadAttributes();
                LoadCheckBoxLists();
                if (demoStandards.Count > 0)
                    spStandard.Visibility = System.Windows.Visibility.Visible;
                if (demoPremiums.Count > 0)
                    spPremium.Visibility = System.Windows.Visibility.Visible;
                if (demoStandards.Count > 0 || demoPremiums.Count > 0)
                    btnSave.Visibility = System.Windows.Visibility.Visible;
            }

            foreach (WpfControls.Helpers.CheckBoxItem cbi in itemList)
            {
                if (cbi.Name.Equals("Custom"))
                {
                    cbi.IsChecked = true;
                    thisDCSteps.demoSelection = "Custom";

                    LoadAttributes();
                    if (demoStandards?.Count > 0 || demoPremiums?.Count > 0)
                    {
                        LoadCheckBoxLists();

                        if (demoStandards.Count > 0)
                            spStandard.Visibility = System.Windows.Visibility.Visible;
                        if (demoPremiums.Count > 0)
                            spPremium.Visibility = System.Windows.Visibility.Visible;
                        if (demoStandards.Count > 0 || demoPremiums.Count > 0)
                            btnSave.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (didSave == false)
            {
                MessageBoxResult mbr = Core_AMS.Utilities.WPF.MessageResult("You have not saved.  Are you sure you want to leave?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                    Previous();
            }
            else
                Previous();
        }
        private void Previous()
        {
            thisDCSteps.Step4ToStep3();
            var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisDCSteps);
            if (borderList.FirstOrDefault(x => x.Name.Equals("StepThreeContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepThreeContainer", StringComparison.CurrentCultureIgnoreCase));
                thisBorder.Child = new FileMapperWizard.Controls.DCProfileAttributes(thisDCSteps);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (!AttributesHelper.GoNextAllowed(didSave))
            {
                return;
            }

            thisDCSteps.Step4ToStep5();
            var border = AttributesHelper.FindBorder(thisDCSteps, StepFiveContainer);
            if (border != null)
            {
                border.Child = new DCMatchCriteria(thisDCSteps);
            }
        }

        private void LoadAttributes()
        {
            if ((demoStandards == null && demoPremiums == null) || demoPremiums.Count == 0 || demoStandards.Count == 0)
            {
                //get the list of Codes - Profile Standard Attributes and Profile Premium Attributes
                //need to limit this by Scope
                //FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> cWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
                //FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> respStandard = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
                //FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> respPremium = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
                //respStandard = cWorker.Proxy.SelectForDemographicAttribute(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Demographic_Standard_Attributes, thisDCSteps.dataCompareResultQue.DataCompareQueId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.FtpFolder);
                //respPremium = cWorker.Proxy.SelectForDemographicAttribute(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Demographic_Premium_Attributes, thisDCSteps.dataCompareResultQue.DataCompareQueId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.FtpFolder);

                //demoStandards = new List<FrameworkUAD_Lookup.Entity.Code>();
                //demoPremiums = new List<FrameworkUAD_Lookup.Entity.Code>();

                //if (respStandard.Result != null && respStandard.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //    demoStandards = respStandard.Result.Where(x => x.IsActive == true).ToList();

                //if (respPremium.Result != null && respPremium.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //    demoPremiums = respPremium.Result.Where(x => x.IsActive == true).ToList();
            }

            if (demoStandards.Count == 0 && demoPremiums.Count == 0)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("There are no DEMOGRAPHICS defined for your selected Target - " + thisDCSteps.targetName);
                if (thisDCSteps.brandId.HasValue)
                    msg.Append(" - " + thisDCSteps.brandName);
                if (thisDCSteps.marketId.HasValue)
                    msg.Append(" - " + thisDCSteps.marketName);
                if (thisDCSteps.productId.HasValue)
                    msg.Append(" - " + thisDCSteps.productName);
                Core_AMS.Utilities.WPF.Message(msg.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void LoadCheckBoxLists()
        {
            //now lets bind up 2 pick lists - Premium and Standard
            ObservableCollection<WpfControls.Helpers.CheckBoxItem> standardList = new ObservableCollection<WpfControls.Helpers.CheckBoxItem>();
            foreach (FrameworkUAD_Lookup.Entity.Code c in demoStandards)
            {
                WpfControls.Helpers.CheckBoxItem cbi = new WpfControls.Helpers.CheckBoxItem { IsChecked = false, Name = c.DisplayName, Attribute = c.CodeTypeId.ToString(), Value = c.CodeId.ToString() };
                standardList.Add(cbi);
            }
            ObservableCollection<WpfControls.Helpers.CheckBoxItem> premiumList = new ObservableCollection<WpfControls.Helpers.CheckBoxItem>();
            foreach (FrameworkUAD_Lookup.Entity.Code c in demoPremiums)
            {
                WpfControls.Helpers.CheckBoxItem cbi = new WpfControls.Helpers.CheckBoxItem { IsChecked = false, Name = c.DisplayName, Attribute = c.CodeTypeId.ToString(), Value = c.CodeId.ToString() };
                premiumList.Add(cbi);
            }
            if (thisDCSteps.demoAttributes != null)
            {
                foreach (XElement xe in thisDCSteps.demoAttributes.Descendants("Item"))
                {
                    bool found = false;
                    //string codeTypeId = xe.Element("CodeTypeId").Value;
                    string codeId = xe.Element("CodeId").Value;

                    foreach (WpfControls.Helpers.CheckBoxItem cbItem in standardList)
                    {
                        if (cbItem.Value.Equals(codeId))
                        {
                            cbItem.IsChecked = true;
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        foreach (WpfControls.Helpers.CheckBoxItem cbItem in premiumList)
                        {
                            if (cbItem.Value.Equals(codeId))
                            {
                                cbItem.IsChecked = true;
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }
            ListBoxStandard.ItemsSource = new WpfControls.Helpers.CheckBoxListModel(standardList).ItemsSource;
            ListBoxPremium.ItemsSource = new WpfControls.Helpers.CheckBoxListModel(premiumList).ItemsSource;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            didSave = false;
            var cb = (CheckBox)sender;

            var thisText = AttributesHelper.GetFirstChildText(cb.Parent);
            AttributesHelper.SetCheckBoxForRadioList(cb, ListBox);
 
            if (cb.IsChecked == true)
            {
                if (thisText.Equals("All"))
                {
                    thisDCSteps.demoSelection = "All";
                    AttributesHelper.SetVisibility(Visibility.Collapsed, spStandard, spPremium, btnSave);

                    LoadAttributes();
                    ObservableCollection<WpfControls.Helpers.CheckBoxItem> cbiStandardList = (ObservableCollection<WpfControls.Helpers.CheckBoxItem>)ListBoxStandard.ItemsSource;
                    foreach (WpfControls.Helpers.CheckBoxItem cbi in cbiStandardList)
                    {
                        cbi.IsChecked = true;
                    }

                    ObservableCollection<WpfControls.Helpers.CheckBoxItem> cbiPremiumList = (ObservableCollection<WpfControls.Helpers.CheckBoxItem>)ListBoxPremium.ItemsSource;
                    foreach (WpfControls.Helpers.CheckBoxItem cbi in cbiPremiumList)
                    {
                        cbi.IsChecked = true;
                    }

                    Save();
                }
                else if (thisText.Equals("None"))
                {
                    thisDCSteps.demoSelection = "None";

                    spStandard.Visibility = System.Windows.Visibility.Collapsed;
                    spPremium.Visibility = System.Windows.Visibility.Collapsed;
                    btnSave.Visibility = System.Windows.Visibility.Collapsed;
                    didSave = true;

                    thisDCSteps.demoCodes = new List<FrameworkUAD_Lookup.Entity.Code>();
                    thisDCSteps.demoAttributes = null;

                    //call a sproc to disable all Demographics - pass DataCompareResultQueId
                    //thisDCSteps.dataCompareResultQue.DataCompareResultQueId;
                    //FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareUserMatrix> rqWorker = FrameworkServices.ServiceClient.UAS_DataCompareUserMatrixClient();
                    //FrameworkUAS.Service.Response<bool> resp = rqWorker.Proxy.DisableAllDemographics(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisDCSteps.dataCompareResultQue.DataCompareQueId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID);
                    //if (resp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    //    Core_AMS.Utilities.WPF.MessageTaskComplete("All demographics have been disabled");
                    //else
                    //    Core_AMS.Utilities.WPF.MessageError(resp.Message);
                }
                else if (thisText.Equals("Custom"))
                {
                    thisDCSteps.demoSelection = "Custom";

                    LoadAttributes();
                    if (demoStandards.Count > 0 || demoPremiums.Count > 0)
                    {
                        LoadCheckBoxLists();

                        if (demoStandards.Count > 0)
                            spStandard.Visibility = System.Windows.Visibility.Visible;
                        if (demoPremiums.Count > 0)
                            spPremium.Visibility = System.Windows.Visibility.Visible;
                        if (demoStandards.Count > 0 || demoPremiums.Count > 0)
                            btnSave.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".btnSave_Click", app, string.Empty, logClientId);
            }
        }
        private void Save()
        {
            thisDCSteps.demoCodes = new List<FrameworkUAD_Lookup.Entity.Code>();
            //create an xml file of CodeTypeId / CodeId send to insert sproc with DataCompareResultQueId and UserId
            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<List>");
            ObservableCollection<WpfControls.Helpers.CheckBoxItem> cbiStandardList = (ObservableCollection<WpfControls.Helpers.CheckBoxItem>)ListBoxStandard.ItemsSource;
            foreach (WpfControls.Helpers.CheckBoxItem cbItem in cbiStandardList)
            {
                try
                {
                    if (cbItem.IsChecked == true)
                    {
                        xml.AppendLine("<Item>");
                        xml.AppendLine("<CodeTypeId>" + cbItem.Attribute.ToString() + "</CodeTypeId>");
                        xml.AppendLine("<CodeId>" + cbItem.Value.ToString() + "</CodeId>");
                        xml.AppendLine("</Item>");

                        int codeId = Convert.ToInt32(cbItem.Value.ToString());
                        if (demoStandards.Exists(x => x.CodeId == codeId))
                            thisDCSteps.demoCodes.Add(demoStandards.Single(x => x.CodeId == codeId));
                    }
                }
                catch (Exception ex)
                {
                    FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                    Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                    int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".Save_Standard", app, string.Empty, logClientId);
                }
            }
            //foreach (CheckBox cbItem in standardBoxes)
            //{
            //    try
            //    {
            //        if (cbItem.IsChecked == true)
            //        {
            //            xml.AppendLine("<Item>");
            //            xml.AppendLine("<CodeTypeId>" + itemList.Single(x => x.Value.Equals(cbItem.CommandParameter)).Attribute.ToString() + "</CodeTypeId>");
            //            xml.AppendLine("<CodeId>" + cbItem.CommandParameter.ToString() + "</CodeId>");
            //            xml.AppendLine("</Item>");

            //            int codeId = Convert.ToInt32(cbItem.CommandParameter.ToString());
            //            if (demoStandards.Exists(x => x.CodeId == codeId))
            //                thisDCSteps.demoCodes.Add(demoStandards.Single(x => x.CodeId == codeId));
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
            //        Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            //        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            //        int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
            //        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //        alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".Save_Standard", app, string.Empty, logClientId);
            //    }
            //}

            //IEnumerable<CheckBox> premiumBoxes = Core_AMS.Utilities.WPF.FindVisualChildren<CheckBox>(ListBoxPremium);
            //itemList = (ObservableCollection<WpfControls.Helpers.CheckBoxItem>)ListBoxPremium.ItemsSource;
            ObservableCollection<WpfControls.Helpers.CheckBoxItem> cbiPremiumList = (ObservableCollection<WpfControls.Helpers.CheckBoxItem>)ListBoxPremium.ItemsSource;
            foreach (WpfControls.Helpers.CheckBoxItem cbItem in cbiPremiumList)
            {
                try
                {
                    if (cbItem.IsChecked == true)
                    {
                        xml.AppendLine("<Item>");
                        xml.AppendLine("<CodeTypeId>" + cbItem.Attribute.ToString() + "</CodeTypeId>");
                        xml.AppendLine("<CodeId>" + cbItem.Value.ToString() + "</CodeId>");
                        xml.AppendLine("</Item>");

                        int codeId = Convert.ToInt32(cbItem.Value.ToString());
                        if (demoPremiums.Exists(x => x.CodeId == codeId))
                            thisDCSteps.demoCodes.Add(demoPremiums.Single(x => x.CodeId == codeId));
                    }
                }
                catch (Exception ex)
                {
                    FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                    Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                    int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".Save_Premium", app, string.Empty, logClientId);
                }
            }
            //foreach (CheckBox cbItem in premiumBoxes)
            //{
            //    try
            //    {
            //        if (cbItem.IsChecked == true)
            //        {
            //            xml.AppendLine("<Item>");
            //            xml.AppendLine("<CodeTypeId>" + itemList.Single(x => x.Value.Equals(cbItem.CommandParameter)).Attribute.ToString() + "</CodeTypeId>");
            //            xml.AppendLine("<CodeId>" + cbItem.CommandParameter.ToString() + "</CodeId>");
            //            xml.AppendLine("</Item>");

            //            int codeId = Convert.ToInt32(cbItem.CommandParameter.ToString());
            //            if (demoPremiums.Exists(x => x.CodeId == codeId))
            //                thisDCSteps.demoCodes.Add(demoPremiums.Single(x => x.CodeId == codeId));
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
            //        Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            //        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            //        int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
            //        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //        alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".Save_Premium", app, string.Empty, logClientId);
            //    }
            //}

            xml.AppendLine("</List>");

            thisDCSteps.demoAttributes = XDocument.Parse(xml.ToString());

            //FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareUserMatrix> rqWorker = FrameworkServices.ServiceClient.UAS_DataCompareUserMatrixClient();
            //FrameworkUAS.Service.Response<bool> resp = rqWorker.Proxy.InsertSelectedDemographics(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisDCSteps.dataCompareResultQue.DataCompareQueId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, xml.ToString());
            //if (resp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //{
            //    didSave = true;
            //    Core_AMS.Utilities.WPF.MessageTaskComplete("Selected demographics have been activated");
            //}
            //else
            //    Core_AMS.Utilities.WPF.MessageError(resp.Message);
        }
    }
}
