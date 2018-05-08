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
    /// Interaction logic for DCProfileAttributes.xaml
    /// </summary>
    public partial class DCProfileAttributes : UserControl
    {
        private const string StepFourContainer = "StepFourContainer";
        FileMapperWizard.Modules.DataCompareSteps thisDCSteps { get; set; }
        List<FrameworkUAD_Lookup.Entity.Code> profileStandards { get; set; }
        List<FrameworkUAD_Lookup.Entity.Code> profilePremiums { get; set; }
        bool didSave { get; set; }
        public DCProfileAttributes(FileMapperWizard.Modules.DataCompareSteps dcSteps)
        {
            thisDCSteps = dcSteps;
            InitializeComponent();
            didSave = false;

            ObservableCollection<WpfControls.Helpers.CheckBoxItem> itemList = new ObservableCollection<WpfControls.Helpers.CheckBoxItem>
            {
                new WpfControls.Helpers.CheckBoxItem { IsChecked = false, Name="All" },
                new WpfControls.Helpers.CheckBoxItem { IsChecked = true, Name="Custom" },
            };
            if (!string.IsNullOrEmpty(thisDCSteps.profileSelection))
            {
                foreach(WpfControls.Helpers.CheckBoxItem cbi in itemList)
                {
                    if (cbi.Name.Equals(thisDCSteps.profileSelection))
                        cbi.IsChecked = true;
                }
            }
            ListBox.ItemsSource = new WpfControls.Helpers.CheckBoxListModel(itemList).ItemsSource;

            if(thisDCSteps.profileAttributes != null)
            {
                LoadAttributes();
                LoadCheckBoxLists();
                spStandard.Visibility = System.Windows.Visibility.Visible;
                spPremium.Visibility = System.Windows.Visibility.Visible;
                btnSave.Visibility = System.Windows.Visibility.Visible;
            }

            foreach (WpfControls.Helpers.CheckBoxItem cbi in itemList)
            {
                if (cbi.Name.Equals("Custom"))
                {
                    cbi.IsChecked = true;
                    thisDCSteps.profileSelection = "Custom";

                    //get the list of Codes - Profile Standard Attributes and Profile Premium Attributes
                    LoadAttributes();
                    LoadCheckBoxLists();

                    spStandard.Visibility = System.Windows.Visibility.Visible;
                    spPremium.Visibility = System.Windows.Visibility.Visible;
                    btnSave.Visibility = System.Windows.Visibility.Visible;
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
            thisDCSteps.Step3ToStep2();
            var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisDCSteps);
            if (borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase));
                thisBorder.Child = new FileMapperWizard.Controls.DCResultQue(thisDCSteps);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (!AttributesHelper.GoNextAllowed(didSave))
            {
                return;
            }

            thisDCSteps.Step3ToStep4();
            var border = AttributesHelper.FindBorder(thisDCSteps, StepFourContainer);
            if (border != null)
            {
                border.Child = new DCDemoAttributes(thisDCSteps);
            }
        }
        
        private void LoadAttributes()
        {
            if ((profileStandards == null && profilePremiums == null) || profileStandards.Count == 0 || profilePremiums.Count == 0)
            {
                FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> cWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
                FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> respStandard = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
                FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> respPremium = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
                respStandard = cWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Profile_Standard_Attributes);
                respPremium = cWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Profile_Premium_Attributes);

                profileStandards = new List<FrameworkUAD_Lookup.Entity.Code>();
                profilePremiums = new List<FrameworkUAD_Lookup.Entity.Code>();

                if (respStandard.Result != null && respStandard.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    profileStandards = respStandard.Result.Where(x => x.IsActive == true).ToList();

                if (respPremium.Result != null && respPremium.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    profilePremiums = respPremium.Result.Where(x => x.IsActive == true).ToList();

                //only do this if NOT a Consensus DataCompare
                if (thisDCSteps.isConsensus == false)
                    RemoveMissingProductProperties();
            }
        }
        private void RemoveMissingProductProperties()
        {
            if (thisDCSteps.isConsensus == false)
            {
                profileStandards.RemoveAll(x => x.CodeName.Equals("ForZip"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Home_Work_Address"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("TransactionDate"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("RegCode"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("SubSrc"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Par3C"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Source"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Priority"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Sic"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("SicCode"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("IGrp_Rank"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Score"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("IsMailable"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Demo31"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Demo32"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Demo33"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Demo34"));
                profileStandards.RemoveAll(x => x.CodeName.Equals("Demo35"));

                profilePremiums.RemoveAll(x => x.CodeName.Equals("ForZip"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Home_Work_Address"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("TransactionDate"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("RegCode"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("SubSrc"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Par3C"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Source"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Priority"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Sic"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("SicCode"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("IGrp_Rank"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Score"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("IsMailable"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Demo31"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Demo32"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Demo33"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Demo34"));
                profilePremiums.RemoveAll(x => x.CodeName.Equals("Demo35"));
            }

        }
        private void LoadCheckBoxLists()
        {
            //now lets bind up 2 pick lists - Premium and Standard
            ObservableCollection<WpfControls.Helpers.CheckBoxItem> standardList = new ObservableCollection<WpfControls.Helpers.CheckBoxItem>();
            foreach (FrameworkUAD_Lookup.Entity.Code c in profileStandards)
            {
                WpfControls.Helpers.CheckBoxItem cbi = new WpfControls.Helpers.CheckBoxItem { IsChecked = false, Name = c.DisplayName, Attribute = c.CodeTypeId.ToString(), Value = c.CodeId.ToString() };
                standardList.Add(cbi);
            }
            ObservableCollection<WpfControls.Helpers.CheckBoxItem> premiumList = new ObservableCollection<WpfControls.Helpers.CheckBoxItem>();
            foreach (FrameworkUAD_Lookup.Entity.Code c in profilePremiums)
            {
                WpfControls.Helpers.CheckBoxItem cbi = new WpfControls.Helpers.CheckBoxItem { IsChecked = false, Name = c.DisplayName, Attribute = c.CodeTypeId.ToString(), Value = c.CodeId.ToString() };
                premiumList.Add(cbi);
            }
            if (thisDCSteps.profileAttributes != null)
            {
                foreach (XElement xe in thisDCSteps.profileAttributes.Descendants("Item"))
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

            var checkBox = (CheckBox)sender;
            var thisText = AttributesHelper.GetFirstChildText(checkBox.Parent);

            AttributesHelper.SetCheckBoxForRadioList(checkBox, ListBox);

            if (checkBox.IsChecked == true)
            {
                if (thisText.Equals("All"))
                {
                    thisDCSteps.profileSelection = "All";
                    AttributesHelper.SetVisibility(Visibility.Collapsed, spStandard, spPremium, btnSave);
                    didSave = true;

                    LoadAttributes();
                    List<FrameworkUAD_Lookup.Entity.Code> all = new List<FrameworkUAD_Lookup.Entity.Code>();
                    all.AddRange(profilePremiums);
                    all.AddRange(profileStandards);

                    StringBuilder xml = new StringBuilder();
                    xml.AppendLine("<List>");
                    foreach (FrameworkUAD_Lookup.Entity.Code c in all)
                    {
                        xml.AppendLine("<Item>");
                        xml.AppendLine("<CodeTypeId>" + c.CodeTypeId.ToString() + "</CodeTypeId>");
                        xml.AppendLine("<CodeId>" + c.CodeId.ToString() + "</CodeId>");
                        xml.AppendLine("</Item>");

                        thisDCSteps.profileCodes.Add(c);
                    }
                    xml.AppendLine("</List>");
                    thisDCSteps.profileAttributes = XDocument.Parse(xml.ToString());


                }
                else if (thisText.Equals("Custom"))
                {
                    thisDCSteps.profileSelection = "Custom";

                    //get the list of Codes - Profile Standard Attributes and Profile Premium Attributes
                    LoadAttributes();
                    LoadCheckBoxLists();

                    spStandard.Visibility = System.Windows.Visibility.Visible;
                    spPremium.Visibility = System.Windows.Visibility.Visible;
                    btnSave.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            thisDCSteps.profileCodes = new List<FrameworkUAD_Lookup.Entity.Code>();

            //create an xml file of CodeTypeId / CodeId send to insert sproc with DataCompareResultQueId and UserId
            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<List>");
            //IEnumerable<CheckBox> standardBoxes = Core_AMS.Utilities.WPF.FindVisualChildren<CheckBox>(ListBoxStandard);
            //ObservableCollection<WpfControls.Helpers.CheckBoxItem> itemList = (ObservableCollection<WpfControls.Helpers.CheckBoxItem>)ListBoxStandard.ItemsSource;
            ObservableCollection<WpfControls.Helpers.CheckBoxItem> cbiStandardList = (ObservableCollection<WpfControls.Helpers.CheckBoxItem>)ListBoxStandard.ItemsSource;
            foreach (WpfControls.Helpers.CheckBoxItem cbItem in cbiStandardList)
            {
                if (cbItem.IsChecked == true)
                {
                    xml.AppendLine("<Item>");
                    xml.AppendLine("<CodeTypeId>" + cbItem.Attribute.ToString() + "</CodeTypeId>");
                    xml.AppendLine("<CodeId>" + cbItem.Value.ToString() + "</CodeId>");
                    xml.AppendLine("</Item>");

                    int codeId = Convert.ToInt32(cbItem.Value.ToString());
                    if (profileStandards.Exists(x => x.CodeId == codeId))
                        thisDCSteps.profileCodes.Add(profileStandards.Single(x => x.CodeId == codeId));
                }
            }
            //foreach (CheckBox cbItem in standardBoxes)
            //{
            //    if (cbItem.IsChecked == true)
            //    {
            //        xml.AppendLine("<Item>");
            //        xml.AppendLine("<CodeTypeId>" + itemList.Single(x => x.Value.Equals(cbItem.CommandParameter)).Attribute.ToString() + "</CodeTypeId>");
            //        xml.AppendLine("<CodeId>" + cbItem.CommandParameter.ToString() + "</CodeId>");
            //        xml.AppendLine("</Item>");

            //        int codeId = Convert.ToInt32(cbItem.CommandParameter.ToString());
            //        if (profileStandards.Exists(x => x.CodeId == codeId))
            //            thisDCSteps.profileCodes.Add(profileStandards.Single(x => x.CodeId == codeId));
            //    }
            //}

            //IEnumerable<CheckBox> premiumBoxes = Core_AMS.Utilities.WPF.FindVisualChildren<CheckBox>(ListBoxPremium);
            //itemList = (ObservableCollection<WpfControls.Helpers.CheckBoxItem>)ListBoxPremium.ItemsSource;
            ObservableCollection<WpfControls.Helpers.CheckBoxItem> cbiPremiumList = (ObservableCollection<WpfControls.Helpers.CheckBoxItem>)ListBoxPremium.ItemsSource;
            foreach (WpfControls.Helpers.CheckBoxItem cbItem in cbiPremiumList)
            {
                if (cbItem.IsChecked == true)
                {
                    xml.AppendLine("<Item>");
                    xml.AppendLine("<CodeTypeId>" + cbItem.Attribute.ToString() + "</CodeTypeId>");
                    xml.AppendLine("<CodeId>" + cbItem.Value.ToString() + "</CodeId>");
                    xml.AppendLine("</Item>");

                    int codeId = Convert.ToInt32(cbItem.Value.ToString());
                    if (profilePremiums.Exists(x => x.CodeId == codeId))
                        thisDCSteps.profileCodes.Add(profilePremiums.Single(x => x.CodeId == codeId));
                }
            }
            //foreach (CheckBox cbItem in premiumBoxes)
            //{
            //    if (cbItem.IsChecked == true)
            //    {
            //        xml.AppendLine("<Item>");
            //        xml.AppendLine("<CodeTypeId>" + itemList.Single(x => x.Value.Equals(cbItem.CommandParameter)).Attribute.ToString() + "</CodeTypeId>");
            //        xml.AppendLine("<CodeId>" + cbItem.CommandParameter.ToString() + "</CodeId>");
            //        xml.AppendLine("</Item>");

            //        int codeId = Convert.ToInt32(cbItem.CommandParameter.ToString());
            //        if (profilePremiums.Exists(x => x.CodeId == codeId))
            //            thisDCSteps.profileCodes.Add(profilePremiums.Single(x => x.CodeId == codeId));
            //    }
            //}

            xml.AppendLine("</List>");

            thisDCSteps.profileAttributes = XDocument.Parse(xml.ToString());

            //FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareUserMatrix> rqWorker = FrameworkServices.ServiceClient.UAS_DataCompareUserMatrixClient();
            //FrameworkUAS.Service.Response<bool> resp = rqWorker.Proxy.InsertSelectedProfileAttributes(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisDCSteps.dataCompareResultQue.DataCompareQueId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, xml.ToString());
            //if (resp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //{
            //    didSave = true;
            //    Core_AMS.Utilities.WPF.MessageTaskComplete("Selected profile attributes have been activated");
            //}
            //else
            //    Core_AMS.Utilities.WPF.MessageError(resp.Message);
        }
    }
}
