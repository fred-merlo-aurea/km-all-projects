using FrameworkUAD.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for MultiColumnMapper.xaml
    /// </summary>
    public partial class MultiColumnMapper : UserControl
    {
        private Dictionary<StackPanel, StackPanel> abc = new Dictionary<StackPanel, StackPanel>();
        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        public List<FileMappingColumn> myMappings;
        public string FieldMapType;
        public int numberOfTransformations;
        public bool TransformationOpened;
        public int mySourceFileID;
        public int myFieldMappingID;
        public int myFieldMultiMappingID;
        public string myMAFField;
        public KMPlatform.Entity.Client myClient;
        public List<FrameworkUAS.Entity.TransformationFieldMultiMap> allTranFieldMultiMap = new List<FrameworkUAS.Entity.TransformationFieldMultiMap>();

        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationFieldMultiMap> tfmData = FrameworkServices.ServiceClient.UAS_TransformationFieldMultiMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMultiMap> fmData = FrameworkServices.ServiceClient.UAS_FieldMultiMapClient();        

        public MultiColumnMapper(FrameworkUAS.Object.AppData appData, KMPlatform.Entity.Client client, List<FileMappingColumn> mappings, int sourceFileID = 0, int fieldMappingID = 0, int fieldMultiMapID = 0, string MAFField = "")
        {
            InitializeComponent();
            myAppData = appData;
            myMappings = mappings;
            mySourceFileID = sourceFileID;
            myFieldMappingID = fieldMappingID;
            myFieldMultiMappingID = fieldMultiMapID;
            myMAFField = MAFField;
            myClient = client;
            foreach (FileMappingColumn fmc in myMappings)
            {
                cbUADMultiMapper.Items.Add(fmc.ColumnName);
            }
            if (MAFField != "")
                cbUADMultiMapper.SelectedValue = myMAFField;
            else
                cbUADMultiMapper.SelectedValue = "Ignore";

            numberOfTransformations = 0;
            TransformationOpened = false;

            btnDel.Tag = myFieldMultiMappingID;            
        }

        public int AddSourceFileID
        {
            set
            {
                mySourceFileID = value;                
            }
        }
        public int AddFieldMappingID
        {
            set
            {
                myFieldMappingID = value;
            }
        }
        public int AddFieldMultiMappingID
        {
            set
            {
                myFieldMultiMappingID = value;
                btnDel.Tag = myFieldMultiMappingID;
            }
        }
        public string ComboBoxMultiMap
        {
            get
            {
                if (cbUADMultiMapper.SelectedValue != null)
                    return cbUADMultiMapper.SelectedValue.ToString();
                else
                    return "";
            }
            set { cbUADMultiMapper.SelectedValue = value; }
        }
        public string MultiMapButtonTag
        {
            get
            {
                if (btnDel.Tag != null)
                    return btnDel.Tag.ToString();
                else
                    return "";
                
            }
            set { btnDel.Tag = value; }
        }
        public string GetFieldMapType
        {
            get { return FieldMapType.ToString(); }
            set { FieldMapType = value; }
        }

        
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Are you sure this should be deleted?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No) return;

            Telerik.Windows.Controls.RadButton thisButton = (Telerik.Windows.Controls.RadButton)sender;
            int btnTag = 0;
            int.TryParse(thisButton.Tag.ToString(), out btnTag);
            if (btnTag > 0)
            {
                int t = tfmData.Proxy.DeleteByFieldMultiMapID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, btnTag).Result;
                int i = fmData.Proxy.DeleteByFieldMultiMapID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, btnTag).Result;
                Core_AMS.Utilities.WPF.MessageDeleteComplete();
            }

            StackPanel child = (StackPanel)thisButton.Parent;
            FileMapperWizard.Controls.MultiColumnMapper parent = (MultiColumnMapper)child.Parent;
            StackPanel sp = (StackPanel)parent.Parent;
            sp.Children.Remove(parent);
        }

        private void cbUADMultiMapper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbUADMultiMapper.SelectedValue != null)
                myMAFField = cbUADMultiMapper.SelectedValue.ToString();

            FileMappingColumn foundColumn = myMappings.FirstOrDefault(x => x.ColumnName.Equals(myMAFField, StringComparison.CurrentCultureIgnoreCase));

            if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
            {
                cbUADMultiMapper.SelectedItem = foundColumn.ColumnName;
                if (foundColumn.IsDemographic == false)
                {
                    FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString();
                }
                else
                {
                    FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString();
                }
            }
            else
            {
                FieldMapType = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString();
            }
        }
    }
}
