using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for CircCodeSheetViewer.xaml
    /// </summary>
    public partial class CircCodeSheetViewer : UserControl
    {
        public CircCodeSheetViewer()
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
            //ObservableCollection<Core_AMS.Utilities.CustomMenuItem> responseSubItems = new ObservableCollection<Core_AMS.Utilities.CustomMenuItem>();                                   
            //#region Responses Sub Item
            //Core_AMS.Utilities.CustomMenuItem rgItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    Text = "Response Group"
            //};
            //responseSubItems.Add(rgItem);
            //Core_AMS.Utilities.CustomMenuItem csvItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    Text = "Codesheet Values"
            //};
            //responseSubItems.Add(csvItem);
            //Core_AMS.Utilities.CustomMenuItem csdsItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    Text = "CS Display Settings"
            //};
            //responseSubItems.Add(csdsItem);
            //Core_AMS.Utilities.CustomMenuItem rpgItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    Text = "Report Group"
            //};
            //responseSubItems.Add(rpgItem);
            //Core_AMS.Utilities.CustomMenuItem rgdsItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    Text = "RG Display Settings"
            //};
            //responseSubItems.Add(rgdsItem);
            //#endregion
            //#region Main Items
            ////Core_AMS.Utilities.CustomMenuItem pItem = new Core_AMS.Utilities.CustomMenuItem()
            ////{
            ////    Text = "Publisher"
            ////};
            ////items.Add(pItem);
            //Core_AMS.Utilities.CustomMenuItem mItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    Text = "Magazines"
            //};
            //items.Add(mItem);
            //Core_AMS.Utilities.CustomMenuItem rItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    SubItems = responseSubItems,
            //    Text = "Responses"
            //};
            //items.Add(rItem);
            //Core_AMS.Utilities.CustomMenuItem repItem = new Core_AMS.Utilities.CustomMenuItem()
            //{
            //    Text = "Reports"
            //};
            //items.Add(repItem);
            //#endregion
            return items;
        }

        private void CSMenu_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //var item = e.OriginalSource;
            //if (item.GetType() == typeof(RadMenuItem))
            //{
            //    RadMenuItem rmi = item as RadMenuItem;
            //    string header = rmi.Header.ToString();
            //    switch (header)
            //    {
            //        //case "Publisher":
            //        //    RenewControl(new ControlCenter.Controls.CircCodesheet.Publisher());
            //        //    break;
            //        case "Magazines":
            //            RenewControl(new ControlCenter.Controls.CircCodesheet.Magazines());
            //            break;
            //        case "Response Group":
            //            RenewControl(new ControlCenter.Controls.CircCodesheet.Group());
            //            break;
            //        case "Codesheet Values":
            //            RenewControl(new ControlCenter.Controls.CircCodesheet.Response());
            //            break;
            //        case "CS Display Settings":
            //            RenewControl(new ControlCenter.Controls.CircCodesheet.CustomerService());
            //            break;
            //        case "Report Group":
            //            RenewControl(new ControlCenter.Controls.CircCodesheet.ReportGroups());
            //            break;
            //        case "RG Display Settings":
            //            RenewControl(new ControlCenter.Controls.CircCodesheet.ReportGroupOrder());
            //            break;
            //        case "Reports":
            //            RenewControl(new ControlCenter.Controls.CircCodesheet.Reports());
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }
        public void RenewControl(UIElement uie)
        {
            spControls.Children.Clear();
            spControls.Children.Add(uie);
        }
    }
}
