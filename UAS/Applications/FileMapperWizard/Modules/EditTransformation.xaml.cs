using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TileView;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for EditTransformation.xaml
    /// </summary>
    public partial class EditTransformation : UserControl
    {
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformation> blt = FrameworkServices.ServiceClient.UAS_TransformationClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> blc = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> ttData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();//ITransformationType
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sfData = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationFieldMap> tfmData = FrameworkServices.ServiceClient.UAS_TransformationFieldMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMapping> fmData = FrameworkServices.ServiceClient.UAS_FieldMappingClient();
        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        public List<KMPlatform.Entity.Client> AllClients = new List<KMPlatform.Entity.Client>();
        public KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        List<FrameworkUAD_Lookup.Entity.Code> transformationCodes { get; set; }
        public EditTransformation(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();

            myAppData = appData;
            KMPlatform.Entity.Client SelectedClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;

            if (SelectedClient.ClientID == 1)
            {
                AllClients = blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result.OrderBy(x => x.ClientName).ToList();
                rcbClients.ItemsSource = AllClients.Where(x => x.IsActive);
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
            }
            else
            {
                AllClients = blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result.OrderBy(x => x.ClientName).ToList();
                rcbClients.ItemsSource = AllClients.Where(x => x.IsActive && x.ClientID == SelectedClient.ClientID);
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
                KMPlatform.Entity.Client sc = AllClients.SingleOrDefault(x => x.ClientID == SelectedClient.ClientID);
                if (sc != null)
                    rcbClients.SelectedItem = sc;
            }

            transformationCodes = ttData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Transformation).Result;
        }

        #region Tile Methods
        private void tile_Loaded(object sender, RoutedEventArgs e)
        {
            Style s = this.FindResource("roundedComboBoxes") as Style;
            RadTileViewItem tvi = sender as RadTileViewItem;
            TileViewItemHeader tvh = (TileViewItemHeader)tvi.Template.FindName("HeaderPart", tvi);
            RadToggleButton btn = (RadToggleButton)tvh.Template.FindName("MaximizeToggleButton", tvh);
            btn.Visibility = System.Windows.Visibility.Hidden;
            btn.IsEnabled = false;

            if (tvi.Name.Equals("tileClient"))
            {
                var rcbs = FindVisualChildren<RadComboBox>(tvi);
                foreach (RadComboBox rcb in rcbs)
                {
                    rcb.Style = s;
                }
            }
        }
        private void TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Style s = this.FindResource("roundedComboBoxes") as Style;
            RadTileViewItem tvi = sender as RadTileViewItem;
            if (tvi.TileState.Equals(TileViewItemState.Maximized))
            {
                var rcbs = FindVisualChildren<RadComboBox>(tvi);
                foreach (RadComboBox rcb in rcbs)
                {
                    rcb.Style = s;
                }
            }
        }
        #endregion

        #region FindVisualChildren Methods
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
         where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        private childItem FindVisualChild<childItem>(DependencyObject obj)
         where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        #endregion

        private void rcbClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(rcbClients.SelectedValue != null) 
            {
                int cID = 0;
                int.TryParse(rcbClients.SelectedValue.ToString(), out cID);
                List<FrameworkUAS.Entity.Transformation> transformations = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cID, false).Result;
                rlbTransforms.ItemsSource = transformations.OrderBy(x => x.TransformationName);
                rlbTransforms.SelectedValuePath = "TransformationID";
                myClient = blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cID).Result;

                rlbTransforms.Visibility = Visibility.Visible;
                txtSelectFile.Visibility = Visibility.Visible;
            }
        }

        private void rlbTransforms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rlbTransforms.SelectedValue != null)
            {
                int tID = 0;
                int.TryParse(rlbTransforms.SelectedValue.ToString(), out tID);
                FrameworkUAS.Entity.Transformation t = new FrameworkUAS.Entity.Transformation();
                t = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tID).Result.FirstOrDefault();
                spTransInfo.Visibility = Visibility.Visible;
                txtDateCreated.Text = t.DateCreated.ToString();
                txtTransType.Text = transformationCodes.SingleOrDefault(x => x.CodeId == t.TransformationTypeID).CodeName;
                txtTransType.Text = txtTransType.Text.Replace('_', ' ');
            }
        }

        private void btnMe_Click(object sender, RoutedEventArgs e)
        {
            int tID = 0;
            RadButton me = sender as RadButton;
            int.TryParse(me.Tag.ToString(), out tID);

            if(tID > 0)
            {
                string files = "";
                List<int> sourceFiles = tfmData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tID).Result.Select(x=> x.SourceFileID).Distinct().ToList();
                foreach(int id in sourceFiles)
                {
                    files += sfData.Proxy.SelectForSourceFile(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, id).Result.FileName + "\n";
                }
                
                MessageBoxResult messBox;
                if(files.Length > 0)
                    messBox = MessageBox.Show("Are you sure you want to delete this Transformation? The following files will be affected: \n \n" + files, "Warning", MessageBoxButton.YesNo);
                else
                    messBox = MessageBox.Show("Are you sure you want to delete this Transformation?", "Warning", MessageBoxButton.YesNo);

                if (messBox == MessageBoxResult.Yes)
                {
                    int clientID = 0;
                    int.TryParse(rcbClients.SelectedValue.ToString(), out clientID);
                    blt.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tID);
                    Core_AMS.Utilities.WPF.Message("Transformation has been deleted.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Transformation Deleted.");
                    List<FrameworkUAS.Entity.Transformation> transformations = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, clientID, false).Result;
                    rlbTransforms.ItemsSource = null;
                    rlbTransforms.ItemsSource = transformations;
                    rlbTransforms.SelectedValuePath = "TransformationID";
                    return;
                }
                else
                    return;
            }
        }

        private void rlbTransforms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(rlbTransforms.SelectedValue != null)
            {
                List<FrameworkUAS.Entity.SourceFile> sFiles = new List<FrameworkUAS.Entity.SourceFile>();
                int tID = 0;
                int.TryParse(rlbTransforms.SelectedValue.ToString(), out tID);
                affectedFiles.Tag = tID.ToString();
                FrameworkUAS.Entity.Transformation t = new FrameworkUAS.Entity.Transformation();
                t = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tID).Result.FirstOrDefault();
                List<int> sourceFiles = tfmData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tID).Result.Select(x => x.SourceFileID).Distinct().ToList();
                foreach(int id in sourceFiles)
                {
                    sFiles.Add(sfData.Proxy.SelectForSourceFile(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, id).Result);
                }
                affectedFiles.ItemsSource = null;
                affectedFiles.ItemsSource = sFiles;
                affectedFiles.DisplayMemberPath = "FileName";
                affectedFiles.SelectedValuePath = "SourceFileID";
                string tType = transformationCodes.SingleOrDefault(x => x.CodeId == t.TransformationTypeID).CodeName;
                tType = tType.Replace('_', ' ');
                if(tType.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Assign_Value.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    FileMapperWizard.Controls.AssignTransformation child = new FileMapperWizard.Controls.AssignTransformation(myAppData, null, 0, myClient, transformationArea, tID, null, true);
                    child.grdAssign.IsEnabled = false;
                    child.btnAApply.Visibility = Visibility.Collapsed;
                    transformationArea.Children.Add(child);
                }
                else if (tType.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Data_Mapping.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    FileMapperWizard.Controls.DataMapTransformation child = new FileMapperWizard.Controls.DataMapTransformation(myAppData, null, 0, myClient, transformationArea, tID.ToString(), "0", true);
                    child.grdDataMap.IsEnabled = false;
                    child.btnDApply.Visibility = Visibility.Collapsed;
                    transformationArea.Children.Add(child);
                }
                else if (tType.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Join_Columns.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    FileMapperWizard.Controls.JoinTransformation child = new FileMapperWizard.Controls.JoinTransformation(myAppData, null, 0, myClient, transformationArea, tID.ToString(), "0", true);
                    child.grdJoin.IsEnabled = false;
                    child.btnJApply.Visibility = Visibility.Collapsed;
                    child.btnViewFormat.Visibility = Visibility.Collapsed;
                    transformationArea.Children.Add(child);
                }
                else if (tType.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    FileMapperWizard.Controls.SplitIntoRowTransformation child = new FileMapperWizard.Controls.SplitIntoRowTransformation(myAppData, null, 0, myClient, transformationArea, tID.ToString(), "0", true);
                    child.grdSplitRow.IsEnabled = false;
                    child.btnSApply.Visibility = Visibility.Collapsed;
                    transformationArea.Children.Add(child);
                }
                else if (tType.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Transform.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase))
                {
                    FileMapperWizard.Controls.SplitTransformation child = new FileMapperWizard.Controls.SplitTransformation(myAppData, null, 0, myClient, transformationArea, tID.ToString(), null, true);
                    child.grdSplitTransform.IsEnabled = false;
                    child.btnTApply.Visibility = Visibility.Collapsed;
                    transformationArea.Children.Add(child);
                }
                tileClient.ContentTemplate = (DataTemplate)this.FindResource("step1Min");
                tileClient.HeaderTemplate = null;
                tileTransformation.HeaderTemplate = (DataTemplate)this.FindResource("step2Header");
                tileTransformation.ContentTemplate = null;
                tileTransformation.UpdateLayout();
                tvMe.MaximizedItem = tileTransformation;
            }
        }

        private void affectedFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(affectedFiles.SelectedValue != null)
            {
                affectedColumns.Items.Clear();
                int tID = 0;
                int.TryParse(affectedFiles.Tag.ToString(), out tID);
                int sID = 0;
                int.TryParse(affectedFiles.SelectedValue.ToString(), out sID);

                List<int> fmIDs = tfmData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tID).Result.Where(x=> x.SourceFileID == sID).Select(x=> x.FieldMappingID).ToList();

                foreach(int id in fmIDs)
                {
                    affectedColumns.Items.Add(fmData.Proxy.SelectForFieldMapping(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, id).Result.MAFField);
                }
            }
        }

        private void btnSelectTransformation_Click(object sender, RoutedEventArgs e)
        {
            rcbClients.SelectedItem = null;
            KMPlatform.Entity.Client SelectedClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            KMPlatform.Entity.Client sc = AllClients.SingleOrDefault(x => x.ClientID == SelectedClient.ClientID);
            if (sc != null)
                rcbClients.SelectedValue = sc.ClientID;

            rlbTransforms.Visibility = Visibility.Visible;
            txtSelectFile.Visibility = Visibility.Collapsed;
            spTransInfo.Visibility = Visibility.Collapsed;
            transformationArea.Children.Clear();
            affectedColumns.Items.Clear();
            tileTransformation.ContentTemplate = (DataTemplate)this.FindResource("step2Min");
            tileTransformation.HeaderTemplate = null;
            tileClient.HeaderTemplate = (DataTemplate)this.FindResource("step1Header");
            tileClient.ContentTemplate = null;
            tileClient.UpdateLayout();
            tvMe.MaximizedItem = tileClient;
        }

        private void btnFinishStep2_Click(object sender, RoutedEventArgs e)
        {
            Windows.FMWindow win = this.ParentOfType<Windows.FMWindow>();
            win.Close();
        }


    }
}
