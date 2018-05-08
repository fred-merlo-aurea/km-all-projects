using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace FileMapperWizard.Windows
{
    /// <summary>
    /// Interaction logic for SelectApplyRules.xaml
    /// </summary>
    public partial class FpsSelectApplyRules : RadWindow
    {
        #region Services
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> srvClient = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFpsArchive> srvFpsArc = FrameworkServices.ServiceClient.UAS_FpsArchiveClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFpsCustomRule> srvFpsCr = FrameworkServices.ServiceClient.UAS_FpsCustomRuleClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFpsStandardRule> srvFpsSr = FrameworkServices.ServiceClient.UAS_FpsStandardRuleClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFpsMap> srvFpsMap = FrameworkServices.ServiceClient.UAS_FpsMapClient();
        #endregion

        private List<FrameworkUAS.Entity.FpsCustomRule> crList;
        private List<FrameworkUAS.Entity.FpsStandardRule> srList;

        public FpsSelectApplyRules(CompositeFilterDescriptorCollection filterDescriptors, KMPlatform.Entity.Client client)
        {
            InitializeComponent();
            crList = new List<FrameworkUAS.Entity.FpsCustomRule>();
            srList = new List<FrameworkUAS.Entity.FpsStandardRule>();
            //txtFilterDescriptions.Text = filterDescriptors.ToString();

            ObservableItemCollection<Rules> RulesIncluded = new ObservableItemCollection<Rules>();
            var crResult = srvFpsCr.Proxy.SelectClientId(client.AccessKey, client.ClientID);
            var srResult = srvFpsSr.Proxy.Select(client.AccessKey);
            if (crResult != null && crResult.Result != null)
                crList = crResult.Result;
            if (srResult != null && srResult.Result != null)
                srList = srResult.Result;

            List<Rules> rulesCollection = new List<Rules>();

            crList.ForEach(x => {
                rulesCollection.Add(new Rules
                {
                    RuleName = x.DisplayName,
                    RuleId = x.FpsCustomRuleId,
                    IsCustomRule = true
                });
            });
            srList.ForEach(x => {
                rulesCollection.Add(new Rules
                {
                    RuleName = x.DisplayName,
                    RuleId = x.FpsStandardRuleId,
                    IsCustomRule = false
                });
            });
            //lstRules.ItemsSource = rulesCollection;
            //lstRulesIncluded.ItemsSource = RulesIncluded;
        }

        private void rbNew_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (rbNew.IsChecked == true)
                rbExisting.IsChecked = false;        
        }

        private void rbExisting_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Height = 250;
            if (rbExisting.IsChecked == true)
                rbNew.IsChecked = false;
            else
                this.Height = 100;
        }
    }

    public class Rules : INotifyPropertyChanged
    {
        public string RuleName { get; set; }
        public int RuleId { get; set; }
        //public string RuleValue { get; set; }
        public bool IsCustomRule { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
