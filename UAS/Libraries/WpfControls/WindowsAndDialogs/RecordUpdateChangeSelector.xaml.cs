using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KM.Common;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;

namespace WpfControls.WindowsAndDialogs
{
    /// <summary>
    /// Interaction logic for RecordUpdateChangeSelector.xaml
    /// </summary>
    public partial class RecordUpdateChangeSelector : Window
    {
        private const string DefaultDateFormat = "MM/dd/yyyy";

        private const string CbDateKey = "cbDate";
        private const string CbOptionKey = "cbOption";
        private const string RdpDateKey = "rdpDate";
        private const string RlbOptionKey = "rlbOption";
        private const string SpDateKey = "spDate";
        private const string SpOtherKey = "spOther";
        private const string TbOtherKey = "tbOther";
        private const string TbOptionKey = "tbOption";

        private const string DemographicIsRequiredMessage =
            "demographic is marked as required. Please select an response.";

        private const string ProvideResponseMessage = "Please provide a response for";
        private const string MissingResponseMessage = "missing a response. Please select an response.";

        RecordUpdate myParent;
        int myPubID;
        List<AppliedChanges> currentAppliedChanges;
        List<FrameworkUAD.Entity.CodeSheet> codeSheetList;
        List<FrameworkUAD.Entity.ResponseGroup> responseGroupList;
        List<FrameworkUAD_Lookup.Entity.Code> codeList;
        List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList;
        List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList;
        List<FrameworkUAD_Lookup.Entity.TransactionCode> tranCodeList;
        List<string> adhocList;
        List<FrameworkUAD.Entity.EmailStatus> emailStatusList;
        List<string> pubSubscriptionColumns;
        List<ColumnSelectionTemplate> selectedColumns = new List<ColumnSelectionTemplate>();
        WpfControls.RecordUpdate.BulkRecordUpdateDetail bulkRecordUpdateDetail;

        public RecordUpdateChangeSelector(UserControl parent, WpfControls.RecordUpdate.BulkRecordUpdateDetail brud, int pubID, List<AppliedChanges> appliedChanges, List<string> pubSubProps, List<FrameworkUAD.Entity.CodeSheet> csList, List<FrameworkUAD.Entity.ResponseGroup> rgList, List<FrameworkUAD_Lookup.Entity.Code> cList, List<FrameworkUAD_Lookup.Entity.CategoryCode> ccList, List<FrameworkUAD_Lookup.Entity.TransactionCode> tcList, List<string> ahcList, List<FrameworkUAD.Entity.EmailStatus> esList, List<FrameworkUAD_Lookup.Entity.CodeType> ctList)
        {
            InitializeComponent();
            myParent = (RecordUpdate)parent;
            bulkRecordUpdateDetail = brud;
            myPubID = pubID;
            currentAppliedChanges = appliedChanges;
            pubSubscriptionColumns = pubSubProps;
            codeSheetList = csList;
            responseGroupList = rgList;
            codeList = cList;
            catCodeList = ccList;
            tranCodeList = tcList;
            adhocList = ahcList;
            emailStatusList = esList;
            codeTypeList = ctList;

            LoadValues();
            LoadAppliedChanges(currentAppliedChanges);
        }

        public void LoadValues()
        {
            List<ColumnSelectionTemplate> columns = new List<ColumnSelectionTemplate>();
            foreach (ValidPubSubscriptionColumns s in Enum.GetValues(typeof(ValidPubSubscriptionColumns)))
                columns.Add(new ColumnSelectionTemplate() { DisplayName = s.ToString(), IsMultiple = false, IsRequired = false, ResponseGroupID = 0, Type = ColumnSelectionType.Standard.ToString() });
            foreach (FrameworkUAD.Entity.ResponseGroup rg in responseGroupList)
                columns.Add(new ColumnSelectionTemplate() { DisplayName = rg.DisplayName, IsMultiple = rg.IsMultipleValue == true ? true : false, IsRequired = rg.IsRequired == true ? true : false, ResponseGroupID = rg.ResponseGroupID, Type = ColumnSelectionType.Demographic.ToString() });
            foreach (string psem in adhocList)
                columns.Add(new ColumnSelectionTemplate() { DisplayName = psem, IsMultiple = false, IsRequired = false, ResponseGroupID = 0, Type = ColumnSelectionType.Adhoc.ToString() });

            cbProperty.ItemsSource = columns.OrderBy(x => x.DisplayName);
            cbProperty.DisplayMemberPath = "DisplayName";
            cbProperty.SelectedValuePath = "DisplayName";
            //cbProperty.
        }

        public void LoadAppliedChanges(List<AppliedChanges> changes)
        {
            if (changes != null)
            {
                List<string> provideAnswers = new List<string>();
                foreach (ProvideAnswers pa in Enum.GetValues(typeof(ProvideAnswers)))
                    provideAnswers.Add(pa.ToString());

                List<string> provideDateTime = new List<string>();
                foreach (ProvideDateTime pa in Enum.GetValues(typeof(ProvideDateTime)))
                    provideDateTime.Add(pa.ToString());

                foreach (AppliedChanges ac in changes)
                {
                    selectedColumns.Add(ac.AppliedChange);

                    bool isDemographic = false;
                    if (ac.AppliedChange.Type == ColumnSelectionType.Demographic.ToString())
                        isDemographic = true;

                    if (provideAnswers.FirstOrDefault(x => x.Equals(ac.AppliedChange.DisplayName, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Dictionary<string, string> ComboBoxValues = SupplyAnswers(ac.AppliedChange.DisplayName);
                        AddComboBoxControl(ComboBoxValues, ac.AppliedChange, ac.option);
                    }
                    else if (provideDateTime.FirstOrDefault(x => x.Equals(ac.AppliedChange.DisplayName, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        DateTime time = DateTime.Now;
                        DateTime.TryParse(ac.option, out time);
                        AddDateTimeControl(ac.AppliedChange, time);
                    }
                    else if (isDemographic)
                    {
                        Dictionary<string, string> ListBoxValues = SupplyAnswers(ac.AppliedChange.DisplayName, ac.AppliedChange.ResponseGroupID);
                        AddListBoxControl(ListBoxValues, ac.AppliedChange, ac.rlbOptions, ac.other, ac.useQDateForDate);
                    }
                    else
                    {
                        AddTextBoxControl(ac.AppliedChange, ac.option);
                    }
                }
            }
        }
        
        private void btnApply_Click(object sender, RoutedEventArgs eventArgs)
        {
            var canContinue = true;
            var appliedChangesList = new List<AppliedChanges>();
            var provideAnswers = new List<string>();

            foreach (var answer in Enum.GetValues(typeof(ProvideAnswers)))
            {
                provideAnswers.Add(answer.ToString());
            }

            var provideDateTime = new List<string>();
            foreach (var answer in Enum.GetValues(typeof(ProvideDateTime)))
            {
                provideDateTime.Add(answer.ToString());
            }

            foreach (StackPanel child in spChanges.Children)
            {
                canContinue = CheckPermissionToContinue(appliedChangesList, provideAnswers, provideDateTime, child, canContinue);
            }

            if (canContinue)
            {
                myParent.ReturnResults(appliedChangesList, bulkRecordUpdateDetail);
                Close();
            }
        }

        private static bool CheckPermissionToContinue(
            IList<AppliedChanges> appliedChangesList, 
            IList<string> provideAnswers, 
            IList<string> provideDateTime, 
            StackPanel child, 
            bool canContinue)
        {
            var newAppliedChange = new AppliedChanges();
            var delete = child.Children.OfType<Button>().FirstOrDefault();
            var columnSelection = delete?.Tag as ColumnSelectionTemplate;

            var option = string.Empty;
            var other = string.Empty;
            var display = string.Empty;
            var useQDate = false;
            var rlbOptions = new List<string>();
            var isDemographic = columnSelection?.Type == ColumnSelectionType.Demographic.ToString();

            if (provideAnswers.FirstOrDefault(x => x.Equals(columnSelection?.DisplayName, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                GetSelectedOptionToSetProperties(child, columnSelection, ref canContinue, ref option, ref display);
            }
            else if (provideDateTime.FirstOrDefault(x => x.Equals(columnSelection?.DisplayName, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                GetSelectedDateToSetProperties(child, columnSelection, ref canContinue, ref option, ref display);
            }
            else if (isDemographic)
            {
                var stackPanel = child.Children.OfType<StackPanel>().FirstOrDefault();
                if (stackPanel != null)
                {
                    var rlb = stackPanel.Children.OfType<RadListBox>()
                        .FirstOrDefault(x => x.Name.Equals(RlbOptionKey, StringComparison.CurrentCultureIgnoreCase));

                    if (rlb != null)
                    {
                        GetRadioListDataToSetProperty(rlbOptions, rlb, columnSelection, ref canContinue, ref display);
                    }

                    SetOtherProperties(stackPanel, ref other, ref useQDate);
                }
            }
            else
            {
                GetTextBoxOptionToSetDisplayName(child, columnSelection, ref canContinue, ref option, ref display);
            }

            if (canContinue)
            {
                newAppliedChange.AppliedChange = columnSelection;
                newAppliedChange.option = option;
                newAppliedChange.other = other;
                newAppliedChange.rlbOptions = rlbOptions;
                newAppliedChange.display = display;
                newAppliedChange.useQDateForDate = useQDate;
                appliedChangesList.Add(newAppliedChange);
            }

            return canContinue;
        }

        private static void GetSelectedOptionToSetProperties(
            Panel childPanel, 
            ColumnSelectionTemplate columnSelection, 
            ref bool canContinue, 
            ref string option, 
            ref string display)
        {
            Guard.NotNull(childPanel, nameof(childPanel));
            Guard.NotNull(columnSelection, nameof(columnSelection));

            var cbOption = childPanel.Children
                .OfType<ComboBox>()
                .FirstOrDefault(x => x.Name.Equals(CbOptionKey, StringComparison.CurrentCultureIgnoreCase));

            if (cbOption != null)
            {
                if (cbOption.SelectedValue != null)
                {
                    option = cbOption.SelectedValue.ToString();
                    var item = (KeyValuePair<string, string>)cbOption.SelectedItem;
                    display = item.Value;
                }
                else
                {
                    canContinue = false;
                    MessageBox.Show($"{columnSelection?.DisplayName} {MissingResponseMessage}");
                }
            }
        }

        private static void GetSelectedDateToSetProperties(
            Panel childPanel, 
            ColumnSelectionTemplate columnSelection, 
            ref bool canContinue, 
            ref string option, 
            ref string display)
        {
            Guard.NotNull(childPanel, nameof(childPanel));
            Guard.NotNull(columnSelection, nameof(columnSelection));

            var datePicker = childPanel.Children
                .OfType<RadDatePicker>()
                .FirstOrDefault(x => x.Name.Equals(RdpDateKey, StringComparison.CurrentCultureIgnoreCase));

            if (datePicker != null)
            {
                if (datePicker.SelectedDate != null)
                {
                    option = datePicker.SelectedDate.ToString();
                    display = datePicker.SelectedDate.Value.ToString(DefaultDateFormat);
                }
                else
                {
                    canContinue = false;
                    MessageBox.Show($"{columnSelection?.DisplayName} {MissingResponseMessage}");
                }
            }
        }

        private static void GetRadioListDataToSetProperty(
            ICollection<string> rlbOptions, 
            IMultiSelector multiSelector, 
            ColumnSelectionTemplate columnSelection, 
            ref bool canContinue, 
            ref string display)
        {
            Guard.NotNull(rlbOptions, nameof(rlbOptions));
            Guard.NotNull(multiSelector, nameof(multiSelector));
            Guard.NotNull(columnSelection, nameof(columnSelection));

            if (!(multiSelector.SelectedItems.Count > 0) && columnSelection.IsRequired)
            {
                canContinue = false;
                MessageBox.Show($"{columnSelection.DisplayName} {DemographicIsRequiredMessage}");
            }
            else if (multiSelector.SelectedItems.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in multiSelector.SelectedItems)
                {
                    rlbOptions.Add(item.Key);
                    display += $",{item.Value}";
                }
                display = display.TrimStart(',');
            }
        }

        private static void SetOtherProperties(StackPanel stackPanel, ref string other, ref bool useQDate)
        {
            Guard.NotNull(stackPanel, nameof(stackPanel));

            var spDate = stackPanel.Children
                .OfType<StackPanel>()
                .FirstOrDefault(x => x.Name == SpDateKey);

            var checkBox = spDate?.Children
                .OfType<CheckBox>()
                .FirstOrDefault(x => x.Name == CbDateKey);

            if (checkBox != null)
            {
                useQDate = checkBox.IsChecked.Value;
            }

            var spOther = stackPanel.Children.OfType<StackPanel>().FirstOrDefault(x => x.Name == SpOtherKey);
            var textBox = spOther?.Children.OfType<TextBox>().FirstOrDefault(x => x.Name == TbOtherKey);

            if (textBox != null)
            {
                other = textBox.Text;
            }
        }

        private static void GetTextBoxOptionToSetDisplayName(
            StackPanel childPanel, 
            ColumnSelectionTemplate columnSelection, 
            ref bool canContinue, 
            ref string option, 
            ref string display)
        {
            Guard.NotNull(childPanel, nameof(childPanel));
            Guard.NotNull(columnSelection, nameof(columnSelection));

            var tbOption = childPanel.Children
                .OfType<TextBox>()
                .FirstOrDefault(x => x.Name.Equals(TbOptionKey, StringComparison.CurrentCultureIgnoreCase));

            if (tbOption != null)
            {
                option = tbOption.Text;
                display = tbOption.Text;
            }

            if (option == string.Empty)
            {
                canContinue = false;
                MessageBox.Show($"{ProvideResponseMessage} {columnSelection.DisplayName}.");
            }
        }
        
        public Dictionary<string, string> SupplyAnswers(string column, int rgID = 0)
        {
            Dictionary<string, string> answers = new Dictionary<string, string>();

            #region ComboBox Answers
            #region Demo7
            if (column.Equals(ProvideAnswers.Demo7.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                int codeTypeID = codeTypeList.FirstOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Deliver.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeTypeId;
                foreach (FrameworkUAD_Lookup.Entity.Code c in codeList.Where(x => x.CodeTypeId == codeTypeID).OrderBy(x => x.DisplayName))
                {
                    answers.Add(c.CodeValue.ToString(), c.DisplayName);
                }
            }
            #endregion
            #region PubQSource
            else if (column.Equals(ProvideAnswers.PubQSourceId.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                int codeTypeID = codeTypeList.FirstOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase)).CodeTypeId;
                foreach (FrameworkUAD_Lookup.Entity.Code c in codeList.Where(x => x.CodeTypeId == codeTypeID).OrderBy(x => x.DisplayName))
                {
                    answers.Add(c.CodeId.ToString(), c.DisplayName);
                }
            }
            #endregion
            #region Cat
            else if (column.Equals(ProvideAnswers.PubCategoryID.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in catCodeList.OrderBy(x => x.CategoryCodeValue))
                {
                    answers.Add(cc.CategoryCodeID.ToString(), cc.CategoryCodeValue + " - " + cc.CategoryCodeName);
                }
            }
            #endregion
            #region Tran
            else if (column.Equals(ProvideAnswers.PubTransactionID.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in tranCodeList.OrderBy(x => x.TransactionCodeValue))
                {
                    answers.Add(tc.TransactionCodeID.ToString(), tc.TransactionCodeValue + " - " + tc.TransactionCodeName);
                }
            }
            #endregion
            #region Permissions
            else if (column.Equals(ProvideAnswers.MailPermission.ToString(), StringComparison.CurrentCultureIgnoreCase) 
                || column.Equals(ProvideAnswers.FaxPermission.ToString(), StringComparison.CurrentCultureIgnoreCase)
                || column.Equals(ProvideAnswers.PhonePermission.ToString(), StringComparison.CurrentCultureIgnoreCase)
                || column.Equals(ProvideAnswers.OtherProductsPermission.ToString(), StringComparison.CurrentCultureIgnoreCase)
                || column.Equals(ProvideAnswers.ThirdPartyPermission.ToString(), StringComparison.CurrentCultureIgnoreCase)
                || column.Equals(ProvideAnswers.EmailRenewPermission.ToString(), StringComparison.CurrentCultureIgnoreCase)
                || column.Equals(ProvideAnswers.TextPermission.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                answers.Add("true", "Yes");
                answers.Add("false", "No");
            }
            #endregion
            #region EmailStatus
            else if (column.Equals(ProvideAnswers.EmailStatusID.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (FrameworkUAD.Entity.EmailStatus es in emailStatusList.OrderBy(x => x.Status))
                {
                    answers.Add(es.EmailStatusID.ToString(), es.Status);
                }
            }
            #endregion
            #region Par3C
            else if (column.Equals(ProvideAnswers.Par3CID.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                int codeTypeID = codeTypeList.FirstOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Par3c.ToString(),StringComparison.CurrentCultureIgnoreCase)).CodeTypeId;
                foreach (FrameworkUAD_Lookup.Entity.Code c in codeList.Where(x => x.CodeTypeId == codeTypeID).OrderBy(x => x.DisplayName))
                {
                    answers.Add(c.CodeId.ToString(), c.DisplayName);
                }
            }
            #endregion
            #region ReqFlag
            else if (column.Equals(ProvideAnswers.ReqFlag.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                int codeTypeID = codeTypeList.FirstOrDefault(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Requester_Flag.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase)).CodeTypeId;
                foreach (FrameworkUAD_Lookup.Entity.Code c in codeList.Where(x => x.CodeTypeId == codeTypeID).OrderBy(x => x.DisplayName))
                {
                    answers.Add(c.CodeId.ToString(), c.DisplayName);
                }
            }
            #endregion
            #endregion
            #region ListBox Answers
            else if (rgID > 0 && responseGroupList.FirstOrDefault(x => x.ResponseGroupID == rgID) != null)
            {
                List<FrameworkUAD.Entity.CodeSheet> responseGroupCodeSheets = new List<FrameworkUAD.Entity.CodeSheet>();
                responseGroupCodeSheets.AddRange(codeSheetList.Where(x => x.ResponseGroupID == rgID && x.PubID == myPubID));
                
                foreach (FrameworkUAD.Entity.CodeSheet cs in responseGroupCodeSheets)
                {
                    answers.Add(cs.CodeSheetID.ToString(), cs.ResponseValue + ". " + cs.ResponseDesc);
                }
            }
            #endregion

            return answers;
        }        

        private void cbProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProperty.SelectedValue != null)
            {
                bool isDemographic = false;
                ColumnSelectionTemplate cst = (ColumnSelectionTemplate)cbProperty.SelectedItem;
                if (selectedColumns.FirstOrDefault(x => x.DisplayName.Equals(cst.DisplayName, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Option has already been added. Please edit the existing option.");
                }
                else
                {
                    selectedColumns.Add(cst);
                    if (cst.Type == ColumnSelectionType.Demographic.ToString())
                        isDemographic = true;

                    List<string> provideAnswers = new List<string>();
                    foreach (ProvideAnswers pa in Enum.GetValues(typeof(ProvideAnswers)))
                        provideAnswers.Add(pa.ToString());

                    List<string> provideDateTime = new List<string>();
                    foreach (ProvideDateTime pa in Enum.GetValues(typeof(ProvideDateTime)))
                        provideDateTime.Add(pa.ToString());

                    if (provideAnswers.FirstOrDefault(x => x.Equals(cbProperty.SelectedValue.ToString(), StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Dictionary<string, string> ComboBoxValues = SupplyAnswers(cbProperty.SelectedValue.ToString());

                        AddComboBoxControl(ComboBoxValues, cst);
                    }
                    else if (provideDateTime.FirstOrDefault(x => x.Equals(cbProperty.SelectedValue.ToString(), StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        AddDateTimeControl(cst);
                    }
                    else if (isDemographic)
                    {
                        Dictionary<string, string> ListBoxValues = SupplyAnswers(cbProperty.SelectedValue.ToString(), cst.ResponseGroupID);

                        //Demographic 
                        AddListBoxControl(ListBoxValues, cst);
                    }
                    else
                    {
                        AddTextBoxControl(cst);
                    }
                }
            }
            cbProperty.SelectedIndex = -1;
        }

        public void AddComboBoxControl(Dictionary<string, string> ComboBoxValues, ColumnSelectionTemplate cst, string selectedValue = "")
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new TextBlock { Text = cst.DisplayName, Width = 175, Margin = new Thickness(2, 2, 2, 2) });
            ComboBox combo = new ComboBox() { Name = "cbOption", DisplayMemberPath = "Value", SelectedValuePath = "Key", ItemsSource = ComboBoxValues, Margin = new Thickness(0, 2, 2, 2), Width = 200 };
            if (selectedValue != "")
                combo.SelectedValue = selectedValue;

            stackPanel.Children.Add(combo);
            Button button = new Button() { Content = "X", Tag = cst, Width = 20, Height = 20, Margin = new Thickness(0, 2, 2, 2) };
            button.Click += new RoutedEventHandler(RemoveContent);
            stackPanel.Children.Add(button);
            spChanges.Children.Add(stackPanel);
        }

        public void AddListBoxControl(Dictionary<string, string> ListBoxValues, ColumnSelectionTemplate cst, List<string> selectedOptions = null, string other = "", bool useQDateForDate = false)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new TextBlock { Text = cst.DisplayName, Width = 175, Margin = new Thickness(2, 2, 2, 2) });
            //ListBox check if multiple select
            var stackPanel2 = new StackPanel { Orientation = Orientation.Vertical };
            SelectionMode sm = SelectionMode.Single;
            if (cst.IsMultiple)
                sm = SelectionMode.Multiple;
            
            Visibility otherVisibility = Visibility.Collapsed;
            Telerik.Windows.Controls.RadListBox lb = new Telerik.Windows.Controls.RadListBox { Name = "rlbOption", SelectionMode = sm, Width = 200, Height = 100, Margin = new Thickness(0, 2, 2, 2), ItemsSource = ListBoxValues.OrderBy(x => x.Value), DisplayMemberPath = "Value", SelectedValuePath = "Key" };
            if (selectedOptions != null)
            {
                foreach (string s in selectedOptions)
                {
                    if (codeSheetList.FirstOrDefault(x => x.CodeSheetID.ToString() == s && x.IsOther == true) != null)
                        otherVisibility = Visibility.Visible;

                    KeyValuePair<string, string> select = ListBoxValues.SingleOrDefault(x => x.Key == s);
                    lb.SelectedItems.Add(select);
                }
            }
            lb.SelectionChanged += new SelectionChangedEventHandler(ListBox_SelectionChanged);
            
            stackPanel2.Children.Add(lb);

            var stackPanelDate = new StackPanel { Name = "spDate", Orientation = Orientation.Horizontal };
            stackPanelDate.Children.Add(new TextBlock { Name = "lbDate", Text = "Use QDate as Demo Date", Margin = new Thickness(0, 2, 2, 2) });
            stackPanelDate.Children.Add(new CheckBox { Name = "cbDate", IsChecked = useQDateForDate, Margin = new Thickness(0, 2, 2, 2) });
            stackPanel2.Children.Add(stackPanelDate);

            var stackPanelOther = new StackPanel { Name = "spOther", Orientation = Orientation.Horizontal };
            stackPanelOther.Children.Add(new TextBlock { Name = "lbOther", Text = "Other", Visibility = otherVisibility, Margin = new Thickness(0, 2, 2, 2) });
            stackPanelOther.Children.Add(new TextBox { Name = "tbOther", Text = other, Visibility = otherVisibility, Margin = new Thickness(0, 2, 2, 2), Width = 150 });
            stackPanel2.Children.Add(stackPanelOther);
            stackPanel.Children.Add(stackPanel2);
            Button button = new Button() { Content = "X", VerticalAlignment = VerticalAlignment.Top, Tag = cst, Width = 20, Height = 20, Margin = new Thickness(0, 2, 2, 2) };
            button.Click += new RoutedEventHandler(RemoveContent);
            stackPanel.Children.Add(button);
            spChanges.Children.Add(stackPanel);
        }

        public void AddTextBoxControl(ColumnSelectionTemplate cst, string selectedOptions = "")
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new TextBlock { Text = cst.DisplayName, Width = 175, Margin = new Thickness(2, 2, 2, 2) });
            stackPanel.Children.Add(new TextBox { Name = "tbOption", Text = selectedOptions, Margin = new Thickness(0, 2, 2, 2), Width = 200 });
            Button button = new Button() { Content = "X", Tag = cst, Width = 20, Height = 20, Margin = new Thickness(0, 2, 2, 2) };
            button.Click += new RoutedEventHandler(RemoveContent);
            stackPanel.Children.Add(button);
            spChanges.Children.Add(stackPanel);
        }

        public void AddDateTimeControl(ColumnSelectionTemplate cst, DateTime? selectedTime = null)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new TextBlock { Text = cst.DisplayName, Width = 175, Margin = new Thickness(2, 2, 2, 2) });
            stackPanel.Children.Add(new Telerik.Windows.Controls.RadDatePicker { Name = "rdpDate", SelectedDate = selectedTime, Margin = new Thickness(0, 2, 2, 2), Width = 200 });
            Button button = new Button() { Content = "X", Tag = cst, Width = 20, Height = 20, Margin = new Thickness(0, 2, 2, 2) };
            button.Click += new RoutedEventHandler(RemoveContent);
            stackPanel.Children.Add(button);
            spChanges.Children.Add(stackPanel);
        }

        public void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Telerik.Windows.Controls.RadListBox rlb = (Telerik.Windows.Controls.RadListBox) sender;
            StackPanel sp = (StackPanel)rlb.Parent;
            bool hasOther = false;

            foreach (var item in rlb.SelectedItems)
            {
                KeyValuePair<string, string> kvp = (KeyValuePair< string, string>)item;
                if (kvp.Value.ToLower().Contains("other (please specify)"))
                {
                    hasOther = true;
                    break;
                }                
            }

            var spOther = sp.Children.OfType<StackPanel>().FirstOrDefault(x => x.Name == "spOther");
            TextBlock textBlock = null; 
            TextBox textBox = null;
            if (spOther != null)
            {
                textBlock = spOther.Children.OfType<TextBlock>().FirstOrDefault(x => x.Name == "lbOther");
                textBox = spOther.Children.OfType<TextBox>().FirstOrDefault(x => x.Name == "tbOther");
            }
            if (hasOther)
            {
                if (textBlock != null)
                    textBlock.Visibility = Visibility.Visible;
                if (textBox != null)
                    textBox.Visibility = Visibility.Visible;
            }
            else
            {
                if (textBlock != null)
                    textBlock.Visibility = Visibility.Collapsed;
                if (textBox != null)
                {
                    textBox.Visibility = Visibility.Collapsed;
                    textBox.Text = "";
                }
            }                                           
        }

        private void RemoveContent(object sender, RoutedEventArgs e)
        {
            Button x = (Button) sender;
            StackPanel sp = (StackPanel) x.Parent;
            spChanges.Children.Remove(sp);
            selectedColumns.Remove((ColumnSelectionTemplate)x.Tag);
        }

        public class ColumnSelectionTemplate
        {
            public string DisplayName { get; set; }
            public string Type { get; set; }
            public int ResponseGroupID { get; set; }
            public bool IsMultiple { get; set; }
            public bool IsRequired { get; set; }
        }

        public enum ColumnSelectionType
        {
            [EnumMember]
            Standard,
            [EnumMember]
            Demographic,
            [EnumMember]
            Adhoc
        }

        public enum ValidPubSubscriptionColumns
        {
            //[EnumMember]
            //SubscriptionStatusID,
            [EnumMember]
            Demo7,
            [EnumMember]
            QualificationDate,
            [EnumMember]
            PubQSourceId,
            [EnumMember]
            PubCategoryID,
            [EnumMember]
            PubTransactionID,
            [EnumMember]
            Email,
            //[EnumMember]
            //Status,
            [EnumMember]
            Copies,
            [EnumMember]
            GraceIssues,
            [EnumMember]
            OnBehalfOf,
            [EnumMember]
            SubscriberSourceCode,
            [EnumMember]
            OrigsSrc,
            [EnumMember]
            Verify,
            [EnumMember]
            Occupation,
            [EnumMember]
            CarrierRoute,
            [EnumMember]
            County,
            [EnumMember]
            Phone,
            [EnumMember]
            Fax,
            [EnumMember]
            Mobile,
            [EnumMember]
            Website,
            [EnumMember]
            Birthdate,
            [EnumMember]
            Age,
            [EnumMember]
            Income,
            [EnumMember]
            Gender,
            [EnumMember]
            PhoneExt,
            [EnumMember]
            ReqFlag,
            [EnumMember]
            MailPermission,
            [EnumMember]
            FaxPermission,
            [EnumMember]
            PhonePermission,
            [EnumMember]
            OtherProductsPermission,
            [EnumMember]
            ThirdPartyPermission,
            [EnumMember]
            EmailRenewPermission,
            [EnumMember]
            TextPermission,
            [EnumMember]
            EmailStatusID,
            [EnumMember]
            MemberGroup,
            [EnumMember]
            Par3CID,
            [EnumMember]
            SubSrcID
        }
        
        //Combobox Answers that we provide answers for
        public enum ProvideAnswers
        {
            [EnumMember]
            Demo7,
            [EnumMember]
            PubQSourceId,
            [EnumMember]
            PubCategoryID,
            [EnumMember]
            PubTransactionID,
            [EnumMember]
            MailPermission,
            [EnumMember]
            FaxPermission,
            [EnumMember]
            PhonePermission,
            [EnumMember]
            OtherProductsPermission,
            [EnumMember]
            ThirdPartyPermission,
            [EnumMember]
            EmailRenewPermission,
            [EnumMember]
            TextPermission,
            [EnumMember]
            EmailStatusID,
            [EnumMember]
            Par3CID,
            [EnumMember]           
            ReqFlag
        }

        public enum ProvideDateTime
        {
            [EnumMember]
            QualificationDate,
            [EnumMember]
            BirthDate
        }

        public class AppliedChanges
        {            
            public ColumnSelectionTemplate AppliedChange { get; set; }
            public string option { get; set; }
            public List<string> rlbOptions { get; set; }    
            public string other { get; set; } 
            public string display { get; set; }    
            public bool useQDateForDate { get; set; }   
        }
    }
}
