using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace WpfControls.UADControls
{
    /// <summary>
    /// Interaction logic for UADFilterMenu.xaml
    /// </summary>
    public partial class UADProductFilterMenu : Window
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilter> filterData = FrameworkServices.ServiceClient.UAS_FilterClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetail> filterDetailData = FrameworkServices.ServiceClient.UAS_FilterDetailClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetailSelectedValue> filterDetailValuesData = FrameworkServices.ServiceClient.UAS_FilterDetailSelectedValueClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterGroup> filterGroupData = FrameworkServices.ServiceClient.UAS_FilterGroupClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        #endregion
        #region Variables/List
        private int myProductID;
        private int groupID;
        private Guid accessKey;
        private List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        #endregion
        #region ServiceResponse
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<int> filterSaveResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<int> filterDetailSaveResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<int> filterDetailValueSaveResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Filter>> filterResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Filter>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetail>> filterDetailResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetail>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetailSelectedValue>> filterDetailSelectedValuedResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetailSelectedValue>>();
        #endregion

        public UADProductFilterMenu(FrameworkUAD_Lookup.Enums.FilterGroupTypes group)
        {
            InitializeComponent();
            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

            filterResponse = filterData.Proxy.SelectClient(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
            if (Helpers.Common.CheckResponse(filterResponse.Result, filterResponse.Status))
            {
                codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Group);
                if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                {
                    FrameworkUAD_Lookup.Entity.Code code = codeResponse.Result.Where(x => x.CodeName.Replace(" ", "").Trim() == group.ToString().Replace("_", "").Trim()).FirstOrDefault();
                    groupID = code.CodeId;

                    rcbFilters.ItemsSource = filterResponse.Result.Where(x => x.IsActive == true && x.FilterGroupID == code.CodeId).ToList();
                    rcbFilters.SelectedValuePath = "FilterId";
                    rcbFilters.DisplayMemberPath = "FilterName";
                }
            }

            codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Type);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                filterTypes = codeResponse.Result;
            }
        }

        #region UI Events

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid me = sender as Grid;
            Ellipse awYeah = me.Children.OfType<Ellipse>().First();
            Color color = (Color)ColorConverter.ConvertFromString("#045DA4");
            awYeah.Fill = new SolidColorBrush(color);
            awYeah.Stroke = new SolidColorBrush(color);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid me = sender as Grid;
            Ellipse awYeah = me.Children.OfType<Ellipse>().First();
            Color color = (Color)ColorConverter.ConvertFromString("#F47E1F");
            awYeah.Fill = new SolidColorBrush(color);
            awYeah.Stroke = new SolidColorBrush(color);
        }

        private void txtClose_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Options_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Reset_Filters(object sender, MouseButtonEventArgs e)
        {
            //Resets all selection items in the main window.

            Window w = this.Owner;
            StandardUADDemographics filters = w.FindChildByType<StandardUADDemographics>();
            DynamicUADProductDemographics reportFilters = w.FindChildByType<DynamicUADProductDemographics>();
            UADControls.ExtraFiltersTabControl extraFilters = w.FindChildByType<UADControls.ExtraFiltersTabControl>();
            UADControls.ActivityFilter activities = extraFilters.activities;
            AdHocUADFilters adHocFilters = extraFilters.adhocs;

            RadPanelBar rpb = reportFilters.FindChildByType<RadPanelBar>();
            foreach (RadPanelBarItem parent in rpb.Items)
            {
                foreach (RadPanelBarItem child in parent.Items)
                {
                    if (child.IsSelected)
                    {
                        Border visual = child.Template.FindName("SelectedVisual", child) as Border;
                        if (visual != null)
                            visual.Opacity = 0;
                        child.Tag = "0";
                        child.IsSelected = false;
                    }
                }
            }
            foreach (RadComboBox rcb in filters.ChildrenOfType<RadComboBox>())
            {
                rcb.SelectedItem = null;
            }
            foreach(TextBox tbx in filters.ChildrenOfType<TextBox>())
            {
                tbx.Text = string.Empty;
            }
            foreach (RadDatePicker rdp in filters.ChildrenOfType<RadDatePicker>())
            {
                rdp.SelectedDate = null;
            }
            foreach (ListBox box in filters.ChildrenOfType<ListBox>())
            {
                box.SelectedItems.Clear();
            }
            if (adHocFilters != null)
            {
                foreach (RadComboBox rcb in adHocFilters.ChildrenOfType<RadComboBox>())
                {
                    if (rcb.SelectedItem != null)
                        rcb.SelectedItem = null;
                }
                foreach (RadDatePicker rdp in adHocFilters.ChildrenOfType<RadDatePicker>())
                {
                    if (rdp.SelectedDate != null)
                        rdp.SelectedDate = null;
                }
                foreach (TextBox box in adHocFilters.ChildrenOfType<TextBox>())
                {
                    if (box.Text != String.Empty)
                        box.Text = String.Empty;
                }
            }
            if (activities != null)
                activities.ResetSelection();
        }

        private void Load_Filter(object sender, MouseButtonEventArgs e)
        {
            Window w = this.Owner;
            DynamicUADProductDemographics reportFilters = w.FindChildByType<DynamicUADProductDemographics>();

            Reset_Filters(this, null);
            if(rcbFilters.SelectedItem != null)
            {
                FrameworkUAS.Entity.Filter f = rcbFilters.SelectedItem as FrameworkUAS.Entity.Filter;
                reportFilters.SetProduct(f.ProductId);
                int filterID = -1;
                int.TryParse(rcbFilters.SelectedValue.ToString(), out filterID);

                filterDetailResponse = filterDetailData.Proxy.Select(accessKey, filterID);
                if (Helpers.Common.CheckResponse(filterDetailResponse.Result, filterDetailResponse.Status))
                {
                    foreach(FrameworkUAS.Entity.FilterDetail detail in filterDetailResponse.Result)
                    {
                        if (detail.FilterTypeId != filterTypes.Where(x=> x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.AdHoc.ToString().Replace("_", " ")).Select(x=> x.CodeId).FirstOrDefault())
                        {
                            filterDetailSelectedValuedResponse = filterDetailValuesData.Proxy.Select(accessKey, detail.FilterDetailId);
                            if (Helpers.Common.CheckResponse(filterDetailSelectedValuedResponse.Result, filterDetailSelectedValuedResponse.Status))
                            {
                                SetSelection(detail.FilterTypeId, detail.FilterField, filterDetailSelectedValuedResponse.Result);
                                this.Close();
                            }
                        }
                        else
                            SetAdHocSelection(detail.FilterField, detail.FilterObjectType, detail.SearchCondition, detail.AdHocFromField, detail.AdHocToField, detail.AdHocFieldValue);
                    }
                }
            }
        }

        private void Save_Filter(object sender, MouseButtonEventArgs e)
        {
            if (txtFilterName.Text != "")
            {
                Helpers.FilterOperations.FilterContainer fc = new Helpers.FilterOperations.FilterContainer();
                Helpers.FilterOperations fo = new Helpers.FilterOperations();
                fc.Filter.IsActive = true;
                fc.Filter.FilterGroupID = groupID;

                Window w = this.Owner;
                StandardUADDemographics filters = w.FindChildByType<StandardUADDemographics>();
                DynamicUADProductDemographics reportFilters = w.FindChildByType<DynamicUADProductDemographics>();
                UADControls.ExtraFiltersTabControl extraFilters = w.FindChildByType<UADControls.ExtraFiltersTabControl>();
                UADControls.ActivityFilter activities = extraFilters.activities;
                AdHocUADFilters adHocFilters = extraFilters.adhocs;

                fc.FilterDetails.AddRange(filters.GetSelection());
                Helpers.FilterOperations.FilterDetailContainer fdc = new Helpers.FilterOperations.FilterDetailContainer();
                fdc = reportFilters.GetSelection();
                if(fdc.Values.Count > 0)
                    fc.FilterDetails.Add(reportFilters.GetSelection());
                if(adHocFilters != null)
                    fc.FilterDetails.AddRange(adHocFilters.GetSelection());
                if (activities != null)
                    fc.FilterDetails.AddRange(activities.GetSelection());

                fc.Filter.ProductId = reportFilters.ProductID;
                fc.Filter.BrandId = reportFilters.BrandID;

                fo.SaveFilterContainer(fc, txtFilterName.Text);

                MessageBox.Show("Your filter has been saved.", "Save Complete", MessageBoxButton.OK);
                this.Close();
            }
            else
                MessageBox.Show("Please enter a name for your filter.", "Alert", MessageBoxButton.OK);
        }

        private void Delete_Filter(object sender, MouseButtonEventArgs e)
        {
            if(rcbFilters.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this filter?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    int filterID = -1;
                    int.TryParse(rcbFilters.SelectedValue.ToString(), out filterID);

                    if (filterID > 0)
                    {
                        filterResponse = filterData.Proxy.Select(accessKey, myProductID);
                        if (Helpers.Common.CheckResponse(filterResponse.Result, filterResponse.Status))
                        {
                            FrameworkUAS.Entity.Filter filter = filterResponse.Result.Where(x => x.FilterId == filterID).FirstOrDefault();
                            if (filter != null)
                            {
                                filter.IsActive = false;
                                filterData.Proxy.Save(accessKey, filter);
                            }
                            this.Close();
                            MessageBox.Show("Filter removed.", "Alert", MessageBoxButton.OK);
                        }
                    }
                }
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (this.IsMouseOver == false)
            {
                DoubleAnimation animateMenu = new DoubleAnimation();
                animateMenu = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromSeconds(.4)
                };

                Window main = this.Owner;
                main.MouseLeftButtonDown -= Window_Deactivated;
                animateMenu.Completed += (s, ev) =>
                {
                    this.Close();
                };
                this.BeginAnimation(Window.HeightProperty, animateMenu);
            }
        }

        #endregion

        #region Helper Methods

        private void SetSelection(int typeID, string control, List<FrameworkUAS.Entity.FilterDetailSelectedValue> values)
        {
            foreach(FrameworkUAS.Entity.FilterDetailSelectedValue value in values)
            {
                if(typeID == filterTypes.Where(x=> x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Standard.ToString().Replace("_", " ")).Select(x=> x.CodeId).FirstOrDefault())
                    SetStandardSelection(value, control);
                else if(typeID == filterTypes.Where(x=> x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Dynamic.ToString().Replace("_", " ")).Select(x=> x.CodeId).FirstOrDefault())
                    SetDynamicSelection(value);
                else if (typeID == filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Activity.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
                    SetActivitySelection(value.SelectedValue, control);
            }
        }

        private void SetStandardSelection(FrameworkUAS.Entity.FilterDetailSelectedValue value, string control)
        {
            Window w = this.Owner;
            StandardUADDemographics filters = w.FindChildByType<StandardUADDemographics>();

            var obj = filters.FindName(control);
            Type t = obj.GetType();

            if(t == typeof(ListBox))
            {
                ListBox lb = obj as ListBox;
                if (lb.Name == "lbRegion" || lb.Name == "lbYear") //Special case because these ListBoxes contain only strings.
                {
                    foreach(ListBoxItem lbi in lb.Items)
                    {
                        if (lbi.Content.ToString() == value.SelectedValue.ToString())
                            lbi.IsSelected = true;
                    }
                    return;
                }

                foreach(var i in lb.Items)
                {
                    Type t2 = i.GetType();
                    PropertyInfo p = t2.GetProperty(lb.SelectedValuePath); //The ListBoxes can have many different objects in them, so we dynamically find the type at runtime.
                    object v = p.GetValue(i, null);
                    if (v.ToString() == value.SelectedValue.ToString())
                        lb.SelectedItems.Add(i);
                }
            }
            else if(t == typeof(RadComboBox))
            {
                RadComboBox rcb = obj as RadComboBox;
                rcb.Text = value.SelectedValue.ToString();
            }
            else if(t == typeof(RadDatePicker))
            {
                RadDatePicker rdp = obj as RadDatePicker;
                rdp.SelectedDate = DateTime.Parse(value.SelectedValue);
            }
            else if(t == typeof(TextBox))
            {
                TextBox tbx = obj as TextBox;
                tbx.Text = value.SelectedValue;
            }
        }

        private void SetDynamicSelection(FrameworkUAS.Entity.FilterDetailSelectedValue value)
        {
            Window w = this.Owner;
            DynamicUADProductDemographics reportFilters = w.FindChildByType<DynamicUADProductDemographics>();
            RadPanelBar rpb = reportFilters.FindChildByType<RadPanelBar>() as RadPanelBar;

            foreach(RadPanelBarItem parent in rpb.Items) //RadPanelBar has two levels. We need to find the correct parent level first, and then find the matching child items.
            {
                foreach(RadPanelBarItem child in parent.Items)
                {
                    if (child.Name.ToString() == "_" + value.SelectedValue)
                        child.IsSelected = true;
                }
            }
        }

        private void SetAdHocSelection(string control, string adHocType, string searchCondition, string fromValue, string toValue, string value)
        {
            Window w = this.Owner;
            UADControls.ExtraFiltersTabControl extraFilters = w.FindChildByType<UADControls.ExtraFiltersTabControl>();
            AdHocUADFilters adHocFilters = extraFilters.adhocs;

            foreach(AdHocFilters.AdHocFilterField aff in adHocFilters.icAdHocFields.Items)
            {
                if(aff.FilterObject == control)
                {
                    if(adHocType == "Standard")
                    {
                        aff.SelectedCondition = searchCondition;
                        aff.Value = value;
                    }
                    else
                    {
                        aff.SelectedCondition = searchCondition;
                        aff.FromValue = fromValue;
                        aff.ToValue = toValue;
                    }
                }
            }
        }

        private void SetActivitySelection(string value, string control)
        {
            Window w = this.Owner;
            UADControls.ExtraFiltersTabControl extraFilters = w.FindChildByType<UADControls.ExtraFiltersTabControl>();
            UADControls.ActivityFilter activities = extraFilters.activities;

            var ctrl = activities.FindName(control);

            if (ctrl.GetType().ToString() == typeof(RadComboBox).ToString())
            {
                RadComboBox rcb = (RadComboBox)ctrl;
                rcb.Text = value;
            }
            else if (ctrl.GetType().ToString() == typeof(RadWatermarkTextBox).ToString())
            {
                RadWatermarkTextBox tbx = (RadWatermarkTextBox)ctrl;
                tbx.Text = value;
            }
            else if (ctrl.GetType().ToString() == typeof(RadDatePicker).ToString())
            {
                RadDatePicker rdp = (RadDatePicker)ctrl;
                rdp.SelectedDate = DateTime.Parse(value);
            }
            else if (ctrl.GetType().ToString() == typeof(TextBox).ToString())
            {
                TextBox tbx = (TextBox)ctrl;
                tbx.Text = value;
            }
            else if (ctrl.GetType().ToString() == typeof(StackPanel).ToString())
            {
                StackPanel sp = (StackPanel)ctrl;
                HorizontalRadioButtons hrb = (HorizontalRadioButtons)sp.FindChildByType<HorizontalRadioButtons>();
                hrb.SelectedText = value;
            }
        }

        #endregion
    }
}
