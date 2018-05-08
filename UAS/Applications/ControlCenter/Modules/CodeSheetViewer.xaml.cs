using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for CodeSheetViewer.xaml
    /// </summary>
    public partial class CodeSheetViewer : UserControl
    {
        public CodeSheetViewer()
        {
            if (FrameworkUAS.Object.AppData.IsKmUser() == true)
            {
            InitializeComponent();

            CSMenu.ItemsSource = this.GetMenuItems();
        }
            else
            {
                Core_AMS.Utilities.WPF.MessageAccessDenied();
            }
        }

        public ObservableCollection<Core_AMS.Utilities.CustomMenuItem> GetMenuItems()
        {
            ObservableCollection<Core_AMS.Utilities.CustomMenuItem> items = new ObservableCollection<Core_AMS.Utilities.CustomMenuItem>();
            ObservableCollection<Core_AMS.Utilities.CustomMenuItem> productSubItems = new ObservableCollection<Core_AMS.Utilities.CustomMenuItem>();
            ObservableCollection<Core_AMS.Utilities.CustomMenuItem> masterSubItems = new ObservableCollection<Core_AMS.Utilities.CustomMenuItem>();
            ObservableCollection<Core_AMS.Utilities.CustomMenuItem> adhocSubItems = new ObservableCollection<Core_AMS.Utilities.CustomMenuItem>();
            ObservableCollection<Core_AMS.Utilities.CustomMenuItem> reportSubItems = new ObservableCollection<Core_AMS.Utilities.CustomMenuItem>();
            #region Product Sub Item
            Core_AMS.Utilities.CustomMenuItem prItem = new Core_AMS.Utilities.CustomMenuItem()
            {
                Text = "Product"
            };
            productSubItems.Add(prItem);
            Core_AMS.Utilities.CustomMenuItem pscItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Product Setup Copy"
            };
            productSubItems.Add(pscItem);
            Core_AMS.Utilities.CustomMenuItem psItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Product Sort"
            };
            productSubItems.Add(psItem);
            Core_AMS.Utilities.CustomMenuItem ptItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Product Types"
            };
            productSubItems.Add(ptItem);
            Core_AMS.Utilities.CustomMenuItem rgItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Response Groups"
            };
            productSubItems.Add(rgItem);
            Core_AMS.Utilities.CustomMenuItem rgcItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Response Group Copy"
            };
            productSubItems.Add(rgcItem);
            Core_AMS.Utilities.CustomMenuItem csItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Code Sheet"
            };
            productSubItems.Add(csItem);
            #endregion
            #region Master Group Sub Item
            Core_AMS.Utilities.CustomMenuItem mgrpItem = new Core_AMS.Utilities.CustomMenuItem()
            {
                Text = "Master Group"
            };
            masterSubItems.Add(mgrpItem);
            Core_AMS.Utilities.CustomMenuItem mgsItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Master Groups Sort"
            };
            masterSubItems.Add(mgsItem);
            Core_AMS.Utilities.CustomMenuItem mcsItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Master Code Sheet"
            };
            masterSubItems.Add(mcsItem);
            Core_AMS.Utilities.CustomMenuItem mcssItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Master Code Sheet Sort"
            };
            masterSubItems.Add(mcssItem);
            #endregion
            #region Adhoc Sub Item
            Core_AMS.Utilities.CustomMenuItem asItem = new Core_AMS.Utilities.CustomMenuItem()
            {                
                Text = "Adhoc Setup"
            };
            adhocSubItems.Add(asItem);
            Core_AMS.Utilities.CustomMenuItem aaItem = new Core_AMS.Utilities.CustomMenuItem()
            {
                Text = "Adhoc Admin"
            };
            adhocSubItems.Add(aaItem);
            #endregion            
            #region Report Sub Item
            Core_AMS.Utilities.CustomMenuItem reportItem = new Core_AMS.Utilities.CustomMenuItem()
            {
                Text = "Report"
            };
            reportSubItems.Add(reportItem);
            //Hiding until meghan tests MM
            //Core_AMS.Utilities.CustomMenuItem repgItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    Text = "Report Group"
            //};
            //reportSubItems.Add(repgItem);
            //Core_AMS.Utilities.CustomMenuItem repgoItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    Text = "Report Group Order"
            //};
            //reportSubItems.Add(repgoItem);
            #endregion
            #region Main Items
            Core_AMS.Utilities.CustomMenuItem pItem = new Core_AMS.Utilities.CustomMenuItem()
            {
                SubItems = productSubItems,
                Text = "Products"
            };
            items.Add(pItem);
            Core_AMS.Utilities.CustomMenuItem mgItem = new Core_AMS.Utilities.CustomMenuItem()
            {
                SubItems = masterSubItems,
                Text = "Master Groups"
            };
            items.Add(mgItem);
            Core_AMS.Utilities.CustomMenuItem aItem = new Core_AMS.Utilities.CustomMenuItem()
            {
                SubItems = adhocSubItems,
                Text = "Adhoc"
            };
            items.Add(aItem);
            Core_AMS.Utilities.CustomMenuItem repItem = new Core_AMS.Utilities.CustomMenuItem()
            {
                SubItems = reportSubItems,
                Text = "Reports"
            };
            items.Add(repItem);
            #endregion
            return items;
        }

        private void CSMenu_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var item = e.OriginalSource;
            if (item.GetType() == typeof(RadMenuItem))
            {
                RadMenuItem rmi = item as RadMenuItem;
                string header = rmi.Header.ToString();
                switch (header)
                {
                    case "Product":
                        RenewControl(new ControlCenter.Controls.ProductCreation());
                        break;
                    case "Product Setup Copy":
                        RenewControl(new ControlCenter.Controls.ProductSetupCopy());
                        break;
                    case "Product Sort":
                        RenewControl(new ControlCenter.Controls.ProductSortOrder());
                        break;
                    case "Product Types":
                        RenewControl(new ControlCenter.Controls.ProductType());
                        break;
                    case "Response Groups":
                        RenewControl(new ControlCenter.Controls.ResponseGroups());
                        break;
                    case "Response Group Copy":
                        RenewControl(new ControlCenter.Controls.ResponseGroupCopy());
                        break;
                    case "Code Sheet":
                        RenewControl(new ControlCenter.Controls.CodeSheet());
                        break;
                    case "Master Group":
                        RenewControl(new ControlCenter.Controls.MasterGroup());
                        break;
                    case "Master Groups Sort":
                        RenewControl(new ControlCenter.Controls.MasterGroupSort());
                        break;
                    case "Master Code Sheet":
                        RenewControl(new ControlCenter.Controls.MasterCodeSheet());
                        break;
                    case "Master Code Sheet Sort":
                        RenewControl(new ControlCenter.Controls.MasterCodeSheetSort());
                        break;
                    case "Adhoc Setup":
                        RenewControl(new ControlCenter.Controls.AdhocSetup());
                        break;
                    case "Adhoc Admin":
                        RenewControl(new ControlCenter.Controls.AdhocFilter());
                        break;
                    case "Report":
                        RenewControl(new ControlCenter.Controls.Reports());
                        break;
                    case "Report Group":
                        RenewControl(new ControlCenter.Controls.ReportGroups());
                        break;
                    case "Report Group Order":
                        RenewControl(new ControlCenter.Controls.ReportGroupOrder());
                        break;
                    default:
                        break;
                }
            }
        }
        public void RenewControl(UIElement uie)
        {
            spControls.Children.Clear();
            spControls.Children.Add(uie);
        }
    }
}
