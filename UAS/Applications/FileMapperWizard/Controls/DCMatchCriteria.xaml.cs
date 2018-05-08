using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FileMapperWizard.Helpers;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for DCMatchCriteria.xaml
    /// </summary>
    public partial class DCMatchCriteria : UserControl
    {
        private const string ConfigureAdditionalMatchingCriteria = "Would you like to configure additional matching criteria?";
        private const string StepFourContainer = "StepFourContainer";

        FileMapperWizard.Modules.DataCompareSteps thisDCSteps { get; set; }
        ObservableCollection<Line> criteriaLines { get; set; }
        bool isSaved { get; set; }
        bool isYesNo { get; set; }
        List<FrameworkUAD_Lookup.Entity.Code> allSelectedCodes { get; set; }
        int addCounter { get; set; }
        List<FrameworkUAD_Lookup.Entity.Code> operators { get; set; }
        Dictionary<int, List<int>> groupDictionary { get; set; }
        //List<FrameworkUAS.Entity.CodeType> codeTypes { get; set; }
        public DCMatchCriteria(FileMapperWizard.Modules.DataCompareSteps dcSteps)
        {
            thisDCSteps = dcSteps;
            InitializeComponent();
            isYesNo = false;

            //get a collection of our selected Profile items and Demographics
            allSelectedCodes = new List<FrameworkUAD_Lookup.Entity.Code>();
            allSelectedCodes.AddRange(thisDCSteps.profileCodes);
            allSelectedCodes.AddRange(thisDCSteps.demoCodes);
            allSelectedCodes = allSelectedCodes.OrderBy(x => x.CodeName).ToList();

            criteriaLines = new ObservableCollection<Line>();
            addCounter = 0;

            //if (codeTypes == null || codeTypes.Count == 0)
            //{
            //    FrameworkServices.ServiceClient<UAS_WS.Interface.ICodeType> cWorker = FrameworkServices.ServiceClient.UAS_CodeTypeClient();
            //    FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.CodeType>> respCT = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.CodeType>>();
            //    respCT = cWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);

            //    if (respCT.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && respCT.Result != null)
            //    {
            //        codeTypes = respCT.Result;
            //    }
            //}

            //((INotifyCollectionChanged)icCriteria.Items).CollectionChanged += icCriteria_CollectionChanged;

            FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> oWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> respOperator = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
            respOperator = oWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Operators);
            if (respOperator.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && respOperator.Result != null)
                operators = respOperator.Result.Where(x => !x.CodeName.Equals("multiply") && !x.CodeName.Equals("divide") && !x.CodeName.Equals("modulus")
                    && !x.CodeName.Equals("add") && !x.CodeName.Equals("subtract") && !x.CodeName.Equals("or") && !x.CodeName.Equals("and")).OrderBy(x => x.CodeName).ToList();

            if (groupDictionary == null)
                groupDictionary = new Dictionary<int, List<int>>();

            //if (thisDCSteps.matchCriteriaList != null)
            //    LoadPreviousMatchData();
            //else
            //    thisDCSteps.matchCriteriaList = new List<FrameworkUAS.Entity.DataCompareUserMatchCriteria>();
        }
        private void LoadPreviousMatchData()
        {
            //foreach (FrameworkUAS.Entity.DataCompareUserMatchCriteria c in thisDCSteps.matchCriteriaList)
            //{
            //    addCounter++;
            //    criteriaLines.Add(new Line() { LineNumber = addCounter });
            //    icCriteria.ItemsSource = criteriaLines;
            //}
        }
        private void SetMatchData()
        {
            //IEnumerable<Grid> gridList = Core_AMS.Utilities.WPF.FindVisualChildren<Grid>(icCriteria);
            //List<FrameworkUAS.Entity.DataCompareUserMatchCriteria> clauseList = thisDCSteps.matchCriteriaList;
            //if(clauseList.Count > 0)
            //    rbYes.IsChecked = true;

            //int index = 0;
            //foreach (Grid g in gridList)
            //{
            //    try
            //    {
            //        if (g.Children.Count == 7)
            //        {
            //            index++;
            //            FrameworkUAS.Entity.DataCompareUserMatchCriteria umc = clauseList.Single(x => x.Line == index);
            //            if (umc != null)
            //            {
            //                foreach (UIElement c in g.Children)
            //                {
            //                    if (c.GetType() == typeof(Label))
            //                    {
            //                        Label l = (Label)c;
            //                        l.Content = umc.Line.ToString();
            //                    }
            //                    else if (c.GetType() == typeof(CheckBox))
            //                    {
            //                        CheckBox l = (CheckBox)c;
            //                        l.IsChecked = umc.IsGrouped;
            //                        if (l.IsChecked == true)
            //                        {
            //                            int line = umc.Line;
            //                            if (groupDictionary.ContainsKey(umc.GroupNumber))
            //                            {
            //                                if (!groupDictionary.Single(x => x.Key == umc.GroupNumber).Value.Contains(line))
            //                                    groupDictionary.Single(x => x.Key == umc.GroupNumber).Value.Add(line);
            //                            }
            //                            else
            //                            {
            //                                List<int> tagList = new List<int>();
            //                                tagList.Add(line);
            //                                groupDictionary.Add(groupDictionary.Count + 1, tagList);
            //                            }
            //                        }

            //                    }
            //                    else if (c.GetType() == typeof(TextBox))
            //                    {
            //                        TextBox l = (TextBox)c;
            //                        l.Text = umc.Value;
            //                    }
            //                    else if (c.GetType() == typeof(Telerik.Windows.Controls.RadComboBox))
            //                    {
            //                        Telerik.Windows.Controls.RadComboBox l = (Telerik.Windows.Controls.RadComboBox)c;
            //                        //rcbAndOr rcbAttribute rcbOperator
            //                        if (l.Name.Equals("rcbAndOr"))
            //                        {
            //                            if (!string.IsNullOrEmpty(umc.Link))
            //                            {
            //                                foreach(var v in l.Items)
            //                                {
            //                                    Telerik.Windows.Controls.RadComboBoxItem link = (Telerik.Windows.Controls.RadComboBoxItem)v;
            //                                    if(link.Content.Equals(umc.Link))
            //                                        l.SelectedItem = link;
            //                                }
            //                            }
            //                            if (index == 1)
            //                                l.Visibility = System.Windows.Visibility.Collapsed;
            //                        }
            //                        else if (l.Name.Equals("rcbAttribute"))
            //                        {
            //                            List<FrameworkUAD_Lookup.Entity.Code> list = (List<FrameworkUAD_Lookup.Entity.Code>)l.ItemsSource;
            //                            FrameworkUAD_Lookup.Entity.Code code = list.Single(x => x.CodeName.Equals(umc.MAFField));
            //                            l.SelectedItem = code;
            //                        }
            //                        else if (l.Name.Equals("rcbOperator"))
            //                        {
            //                            List<FrameworkUAD_Lookup.Entity.Code> list = (List<FrameworkUAD_Lookup.Entity.Code>)l.ItemsSource;
            //                            FrameworkUAD_Lookup.Entity.Code code = list.Single(x => x.CodeName.Equals(umc.Operator));
            //                            l.SelectedItem = code;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch { }
            //}
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            var previousAction = new CriteriaPreviousActionHelper(ConfigureAdditionalMatchingCriteria, rbYes, rbNo, Previous);
            var isSavedAfterAction = previousAction.CallPreviousAction(isSaved, isYesNo);
            if (isSavedAfterAction)
            {
                isSaved = true;
            }
            isYesNo = true;
        }

        private void Previous()
        {
            thisDCSteps.Step5ToStep4();
            var border = AttributesHelper.FindBorder(thisDCSteps, StepFourContainer);
            if (border != null)
            {
                border.Child = new DCDemoAttributes(thisDCSteps);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (isYesNo == false)
            {
                MessageBoxResult mbr = Core_AMS.Utilities.WPF.MessageResult("Would you like to configure additional matching criteria?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                    rbYes.IsChecked = true;
                else
                {
                    rbNo.IsChecked = true;
                    isSaved = true;
                }

                isYesNo = true;
            }
            else if (isSaved == false)
            {
                MessageBoxResult mbr = Core_AMS.Utilities.WPF.MessageResult("You have not saved.  Are you sure you want to leave?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                    Next();
            }
            else
                Next();
        }
        private void Next()
        {
            thisDCSteps.Step5ToStep6();
            var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisDCSteps);
            if (borderList.FirstOrDefault(x => x.Name.Equals("StepSixContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepSixContainer", StringComparison.CurrentCultureIgnoreCase));
                thisBorder.Child = new FileMapperWizard.Controls.DCLikeCriteria(thisDCSteps);
            }
        }
        private void rbYes_Checked(object sender, RoutedEventArgs e)
        {
            rbNo.IsChecked = false;
            isYesNo = true;
            //show gridView and btnSave
            spCritButtons.Visibility = System.Windows.Visibility.Visible;
            btnSave.Visibility = System.Windows.Visibility.Visible;
            spCriteria.Visibility = System.Windows.Visibility.Visible;
            isSaved = false;
        }

        private void rbNo_Checked(object sender, RoutedEventArgs e)
        {
            rbYes.IsChecked = false;
            isYesNo = true;
            spCritButtons.Visibility = System.Windows.Visibility.Collapsed;
            btnSave.Visibility = System.Windows.Visibility.Collapsed;
            spCriteria.Visibility = System.Windows.Visibility.Collapsed;
            isSaved = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ////create xml and send to a sproc
            ////do we have one grid or several
            //IEnumerable<Grid> gridList = Core_AMS.Utilities.WPF.FindVisualChildren<Grid>(icCriteria);
            //List<FrameworkUAS.Entity.DataCompareUserMatchCriteria> clauseList = new List<FrameworkUAS.Entity.DataCompareUserMatchCriteria>();

            //foreach (Grid g in gridList)
            //{
            //    try
            //    {
            //        if (g.Children.Count == 7)
            //        {
            //            FrameworkUAS.Entity.DataCompareUserMatchCriteria newClause = new FrameworkUAS.Entity.DataCompareUserMatchCriteria();
            //            newClause.CreatedByUserId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //            newClause.DataCompareResultQueId = thisDCSteps.dataCompareResultQue.DataCompareQueId;
            //            newClause.IsActive = true;

            //            foreach (UIElement c in g.Children)
            //            {
            //                if (c.GetType() == typeof(Label))
            //                {
            //                    Label l = (Label)c;
            //                    newClause.Line = Convert.ToInt32(l.Content.ToString());
            //                }
            //                else if (c.GetType() == typeof(CheckBox))
            //                {
            //                    CheckBox l = (CheckBox)c;
            //                    newClause.IsGrouped = l.IsChecked.Value;
            //                    int groupNumber = 0;
            //                    if (l.IsChecked == true)
            //                    {
            //                        int line = Convert.ToInt32(l.Tag.ToString());
            //                        foreach (KeyValuePair<int, List<int>> kvp in groupDictionary)
            //                        {
            //                            if (kvp.Value.Contains(line))
            //                            {
            //                                groupNumber = kvp.Key;
            //                                break;
            //                            }
            //                        }
            //                    }
            //                    newClause.GroupNumber = groupNumber;
            //                }
            //                else if (c.GetType() == typeof(TextBox))
            //                {
            //                    TextBox l = (TextBox)c;
            //                    newClause.Value = l.Text;
            //                }
            //                else if (c.GetType() == typeof(Telerik.Windows.Controls.RadComboBox))
            //                {
            //                    Telerik.Windows.Controls.RadComboBox l = (Telerik.Windows.Controls.RadComboBox)c;
            //                    //rcbAndOr rcbAttribute rcbOperator
            //                    if (l.Name.Equals("rcbAndOr"))
            //                    {
            //                        if (l.SelectedValue != null)
            //                        {
            //                            Telerik.Windows.Controls.RadComboBoxItem link = (Telerik.Windows.Controls.RadComboBoxItem)l.SelectedItem;
            //                            newClause.Link = link.Content.ToString();
            //                        }
            //                    }
            //                    else if (l.Name.Equals("rcbAttribute"))
            //                    {
            //                        if (l.SelectedValue != null)
            //                        {
            //                            FrameworkUAD_Lookup.Entity.Code mafCode = (FrameworkUAD_Lookup.Entity.Code)l.SelectedItem;
            //                            newClause.MAFField = mafCode.CodeName;
            //                            //here is where we will set IsProfile based on mafCode.CodeTypeId
            //                            FrameworkUAD_Lookup.Entity.CodeType ct = thisDCSteps.codeTypes.SingleOrDefault(x => x.CodeTypeId == mafCode.CodeTypeId);
            //                            string ctProfPrem = FrameworkUAS.BusinessLogic.Enums.CodeType.Profile_Premium_Attributes.ToString().Replace("_", " ");
            //                            string ctProfStd = FrameworkUAS.BusinessLogic.Enums.CodeType.Profile_Standard_Attributes.ToString().Replace("_", " ");

            //                            if (ct.CodeTypeName.Equals(ctProfPrem) || ct.CodeTypeName.Equals(ctProfStd))
            //                                newClause.IsProfile = true;
            //                            else
            //                                newClause.IsProfile = false;
            //                        }
            //                    }
            //                    else if (l.Name.Equals("rcbOperator"))
            //                    {
            //                        if (l.SelectedValue != null)
            //                        {
            //                            FrameworkUAD_Lookup.Entity.Code mafCode = (FrameworkUAD_Lookup.Entity.Code)l.SelectedItem;
            //                            newClause.Operator = mafCode.CodeName;
            //                        }
            //                    }
            //                }
            //            }
            //            newClause.Clause = CreateCriteriaStatement(newClause, thisDCSteps.dataCompareResultQue.IsConsensus);
            //            clauseList.Add(newClause);
            //        }
            //    }
            //    catch { }
            //}
            ////should now display built clause in text form - allow using to change then actually save
            //string clauseText = CreateCriteriaStatement(clauseList, thisDCSteps.dataCompareResultQue.IsConsensus);
            //string genXML = Core_AMS.Utilities.XmlFunctions.ToXML<List<FrameworkUAS.Entity.DataCompareUserMatchCriteria>>(clauseList);

            ////send xml and clauseText to save job
            //FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareUserMatchCriteria> mWorker = FrameworkServices.ServiceClient.UAS_DataCompareUserMatchCriteriaClient();
            //FrameworkUAS.Service.Response<bool> resp = new FrameworkUAS.Service.Response<bool>();
            //resp = mWorker.Proxy.SaveJob(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, genXML, clauseText);
            //if (resp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //{
            //    isSaved = true;
            //    thisDCSteps.matchCriteriaList = clauseList;
            //    thisDCSteps.matchClause = clauseText;
            //    Core_AMS.Utilities.WPF.MessageSaveComplete();
            //}
            //else
            //    Core_AMS.Utilities.WPF.MessageError(resp.Message);
        }
        //private string CreateCriteriaStatement(List<FrameworkUAS.Entity.DataCompareUserMatchCriteria> clauseList, bool isConsensus)
        //{
        //    string statement = string.Empty;
        //    List<FrameworkUAS.Entity.DataCompareUserMatchCriteria> singles = clauseList.Where(x => x.IsGrouped == false).ToList();
        //    List<FrameworkUAS.Entity.DataCompareUserMatchCriteria> groups = clauseList.Where(x => x.IsGrouped == true).ToList();
        //    bool hasGroups = groups.Count > 0 ? true : false;
        //    StringBuilder sb = new StringBuilder();

        //    if (hasGroups == false)
        //    {
        //        foreach (FrameworkUAS.Entity.DataCompareUserMatchCriteria c in singles)
        //        {
        //            //sb.AppendLine(c.Link + " " + c.MAFField + GetOperand(c));
        //            if (c.IsProfile == true)
        //                sb.AppendLine(c.Link + " s." + c.MAFField + GetOperand(c));
        //            else
        //            {
        //                if (isConsensus == true)
        //                {
        //                    sb.AppendLine(c.Link + " (mg.Name = '" + c.MAFField + "' and mcs.MasterValue " + GetOperand(c) + ")");
        //                }
        //                else
        //                {
        //                    sb.AppendLine(c.Link + " (cs.ResponseGroup = '" + c.MAFField + "' and cs.Responsevalue " + GetOperand(c) + ")");
        //                }
        //            }
        //        }
        //        statement = sb.ToString();
        //    }
        //    else
        //    {
        //        int totalItems = clauseList.Count;
        //        int counter = 0;
        //        while (counter < totalItems)
        //        {
        //            counter++;
        //            if (singles.Exists(x => x.Line == counter))
        //            {
        //                FrameworkUAS.Entity.DataCompareUserMatchCriteria c = singles.Single(x => x.Line == counter);
        //                //sb.AppendLine(c.Link + " " + c.MAFField + GetOperand(c));
        //                if (c.IsProfile == true)
        //                    sb.AppendLine(c.Link + " s." + c.MAFField + GetOperand(c));
        //                else
        //                {
        //                    if (isConsensus == true)
        //                    {
        //                        sb.AppendLine(c.Link + " (mg.Name = '" + c.MAFField + "' and mcs.MasterValue " + GetOperand(c) + ")");
        //                    }
        //                    else
        //                    {
        //                        sb.AppendLine(c.Link + " (cs.ResponseGroup = '" + c.MAFField + "' and cs.Responsevalue " + GetOperand(c) + ")");
        //                    }
        //                }
        //            }
        //            else if (groups.Exists(x => x.Line == counter))
        //            {
        //                FrameworkUAS.Entity.DataCompareUserMatchCriteria g = groups.Single(x => x.Line == counter);
        //                List<FrameworkUAS.Entity.DataCompareUserMatchCriteria> gClause = groups.Where(x => x.GroupNumber == g.GroupNumber && x.Line != counter).ToList();
        //                if (g.IsProfile == true)
        //                    sb.AppendLine(g.Link + " (s." + g.MAFField + GetOperand(g));
        //                else
        //                {
        //                    if (isConsensus == true)
        //                    {
        //                        sb.AppendLine(g.Link + " ((mg.Name = '" + g.MAFField + "' and mcs.MasterValue " + GetOperand(g) + ")");
        //                    }
        //                    else
        //                    {
        //                        sb.AppendLine(g.Link + " ((cs.ResponseGroup = '" + g.MAFField + "' and cs.Responsevalue " + GetOperand(g) + ")");
        //                    }
        //                }


        //                foreach (FrameworkUAS.Entity.DataCompareUserMatchCriteria gc in gClause)
        //                {
        //                    if (g.IsProfile == true)
        //                        sb.AppendLine(gc.Link + " s." + gc.MAFField + GetOperand(gc));
        //                    else
        //                    {
        //                        if (isConsensus == true)
        //                        {
        //                            sb.AppendLine(gc.Link + " (mg.Name = '" + gc.MAFField + "' and mcs.MasterValue " + GetOperand(gc) + ")");
        //                        }
        //                        else
        //                        {
        //                            sb.AppendLine(gc.Link + " (cs.ResponseGroup = '" + gc.MAFField + "' and cs.Responsevalue " + GetOperand(gc) + ")");
        //                        }
        //                    }

        //                    counter++;
        //                }
        //                sb.AppendLine(")");
        //            }
        //        }
        //        statement = sb.ToString();
        //    }

        //    return statement;
        //}
        //private string CreateCriteriaStatement(FrameworkUAS.Entity.DataCompareUserMatchCriteria c, bool isConsensus)
        //{
        //    string statement = string.Empty;
        //    StringBuilder sb = new StringBuilder();
        //    if(c.IsProfile == true)
        //        sb.AppendLine(c.Link + " s." + c.MAFField + GetOperand(c));
        //    else
        //    {
        //        if(isConsensus == true)
        //        {
        //            sb.AppendLine(c.Link + " (mg.Name = '" + c.MAFField + "' and mcs.MasterValue " + GetOperand(c) + ")");
        //        }
        //        else
        //        {
        //            sb.AppendLine(c.Link + " (cs.ResponseGroup = '" + c.MAFField + "' and cs.Responsevalue " + GetOperand(c) + ")");
        //        }
        //    }
        //    statement = sb.ToString();

        //    return statement;
        //}
        //private string GetOperand(FrameworkUAS.Entity.DataCompareUserMatchCriteria c)
        //{
        //    string operand = string.Empty;
        //    switch (c.Operator)
        //    {
        //        case "greater than":
        //            operand = " > '" + c.Value + "'";
        //            break;
        //        case "less than":
        //            operand = " < '" + c.Value + "'";
        //            break;
        //        case "greater than or equal to":
        //            operand = " >= '" + c.Value + "'";
        //            break;
        //        case "less than or equal to":
        //            operand = " <= '" + c.Value + "'";
        //            break;
        //        case "is not less than":
        //            operand = " !< '" + c.Value + "'";
        //            break;
        //        case "is not greater than":
        //            operand = " !> '" + c.Value + "'";
        //            break;
        //        case "equal":
        //            operand = " = '" + c.Value + "'";
        //            break;
        //        case "not equal":
        //            operand = " != '" + c.Value + "'";
        //            break;
        //        case "contains":
        //            operand = " like '%" + c.Value + "%'";
        //            break;
        //        case "starts with":
        //            operand = " like '" + c.Value + "%'";
        //            break;
        //        case "ends with":
        //            operand = " like '%" + c.Value + "'";
        //            break;
        //        case "in":
        //            if (c.Value.Contains(','))
        //            {
        //                operand = " in (";
        //                string[] values = c.Value.Split(',');
        //                foreach(string s in values)
        //                {
        //                    operand += "'" + s.Trim() + "',";
        //                }
        //                operand = operand.TrimEnd(',');
        //                operand += ")";
        //            }
        //            else
        //                operand = " in ('" + c.Value.Trim() + "')";
        //            break;
        //        case "not in":
        //            if (c.Value.Contains(','))
        //            {
        //                operand = " not in (";
        //                string[] values = c.Value.Split(',');
        //                foreach (string s in values)
        //                {
        //                    operand += "'" + s.Trim() + "',";
        //                }
        //                operand = operand.TrimEnd(',');

        //                operand += ")";
        //            }
        //            else
        //                operand = " not in ('" + c.Value.Trim() + "')";
        //            break;
        //    }
        //    return operand;
        //}
        private void imgAdd_MouseUp(object sender, MouseButtonEventArgs e)
        {
            addCounter++;
            criteriaLines.Add(new Line() { LineNumber = addCounter });
            icCriteria.ItemsSource = criteriaLines;
        }

        private void imgDelete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image del = (Image)sender;//Parent is Grid

            int lineNumber = Convert.ToInt32(del.Tag.ToString());
            Grid grdCriteria = (Grid)del.Parent;//Core_AMS.Utilities.WPF.FindChild<Grid>(icCriteria, "grdCriteria");

            //delete all controls with tag = lineNumber
            //Image CheckBox telerik:RadComboBox TextBox
            List<UIElement> deletes = new List<UIElement>();
            if (grdCriteria != null)
            {
                foreach (UIElement c in grdCriteria.Children)
                {
                    if (c.GetType() == typeof(Label))
                    {
                        Label l = (Label)c;
                        int ln = Convert.ToInt32(l.Tag.ToString());
                        if (ln == lineNumber)
                            deletes.Add(c);
                    }
                    else if (c.GetType() == typeof(Image))
                    {
                        Image l = (Image)c;
                        int ln = Convert.ToInt32(l.Tag.ToString());
                        if (ln == lineNumber)
                            deletes.Add(c);
                    }
                    else if (c.GetType() == typeof(CheckBox))
                    {
                        CheckBox l = (CheckBox)c;
                        int ln = Convert.ToInt32(l.Tag.ToString());
                        if (ln == lineNumber)
                            deletes.Add(c);
                    }
                    else if (c.GetType() == typeof(TextBox))
                    {
                        TextBox l = (TextBox)c;
                        int ln = Convert.ToInt32(l.Tag.ToString());
                        if (ln == lineNumber)
                            deletes.Add(c);
                    }
                    else if (c.GetType() == typeof(Telerik.Windows.Controls.RadComboBox))
                    {
                        Telerik.Windows.Controls.RadComboBox l = (Telerik.Windows.Controls.RadComboBox)c;
                        int ln = Convert.ToInt32(l.Tag.ToString());
                        if (ln == lineNumber)
                            deletes.Add(c);
                    }
                }

                foreach(UIElement c in deletes)
                    grdCriteria.Children.Remove(c);

                addCounter--;
                Line delLine = criteriaLines.SingleOrDefault(x => x.LineNumber == lineNumber);
                if (delLine != null)
                    criteriaLines.Remove(delLine);
                icCriteria.ItemsSource = criteriaLines;
            }
        }
        private void icCriteria_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {

                ItemCollection ic = (ItemCollection)sender;
                if (ic != null)
                {
                    IEnumerable<Telerik.Windows.Controls.RadComboBox> newrcbs = Core_AMS.Utilities.WPF.FindVisualChildren<Telerik.Windows.Controls.RadComboBox>(icCriteria);
                    foreach (Telerik.Windows.Controls.RadComboBox rcb in newrcbs)
                    {
                        if (addCounter == 1 && rcb.Name.Equals("rcbAndOr"))
                            rcb.Visibility = System.Windows.Visibility.Collapsed;

                        if (rcb.Name.Equals("rcbAttribute"))
                        {
                            if (rcb.Items.Count == 0)
                            {
                                rcb.ItemsSource = allSelectedCodes;
                                rcb.DisplayMemberPath = "DisplayName";
                                rcb.SelectedValuePath = "CodeId";
                            }
                        }
                        else if (rcb.Name.Equals("rcbOperator"))
                        {
                            if (rcb.Items.Count == 0)
                            {
                                rcb.ItemsSource = operators;
                                rcb.DisplayMemberPath = "CodeName";
                                rcb.SelectedValuePath = "CodeId";
                            }
                        }
                    }
                }
                //sender is ItemCollection
                IEnumerable<Telerik.Windows.Controls.RadComboBox> rcbs = Core_AMS.Utilities.WPF.FindVisualChildren<Telerik.Windows.Controls.RadComboBox>(icCriteria);
                foreach (Telerik.Windows.Controls.RadComboBox rcb in rcbs)
                {
                    if (addCounter == 1 && rcb.Name.Equals("rcbAndOr"))
                        rcb.Visibility = System.Windows.Visibility.Collapsed;

                    if (rcb.Name.Equals("rcbAttribute"))
                    {
                        if (rcb.Items.Count == 0)
                        {
                            rcb.ItemsSource = allSelectedCodes;
                            rcb.DisplayMemberPath = "DisplayName";
                            rcb.SelectedValuePath = "CodeId";
                        }
                    }
                    else if (rcb.Name.Equals("rcbOperator"))
                    {
                        if (rcb.Items.Count == 0)
                        {
                            rcb.ItemsSource = operators;
                            rcb.DisplayMemberPath = "CodeName";
                            rcb.SelectedValuePath = "CodeId";
                        }
                    }
                }
            }
        }

        private void imgGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //go through ItemCollection
            //find all check boxes
            //get Tag number and put in list
            //all list to groupDictionary
            //remove the check boxes 
            List<int> tagList = new List<int>();
            IEnumerable<CheckBox> cbList = Core_AMS.Utilities.WPF.FindVisualChildren<CheckBox>(icCriteria);
            foreach (CheckBox cb in cbList)
            {
                if (cb.IsChecked == true && cb.IsEnabled == true && cb.IsVisible == true)
                {
                    tagList.Add(Convert.ToInt32(cb.Tag.ToString()));
                    cb.IsEnabled = false;
                    cb.Visibility = System.Windows.Visibility.Hidden;
                }
            }

            if (tagList.Count > 0)
                groupDictionary.Add(groupDictionary.Count + 1, tagList);
        }

        private void icCriteria_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           
            IEnumerable<Telerik.Windows.Controls.RadComboBox> rcbs = Core_AMS.Utilities.WPF.FindVisualChildren<Telerik.Windows.Controls.RadComboBox>(icCriteria);
            foreach (Telerik.Windows.Controls.RadComboBox rcb in rcbs)
            {
                if (addCounter == 1 && rcb.Name.Equals("rcbAndOr"))
                    rcb.Visibility = System.Windows.Visibility.Collapsed;

                if (rcb.Name.Equals("rcbAttribute"))
                {
                    if (rcb.Items.Count == 0)
                    {
                        rcb.ItemsSource = allSelectedCodes;
                        rcb.DisplayMemberPath = "DisplayName";
                        rcb.SelectedValuePath = "CodeId";
                    }
                }
                else if (rcb.Name.Equals("rcbOperator"))
                {
                    if (rcb.Items.Count == 0)
                    {
                        rcb.ItemsSource = operators;
                        rcb.DisplayMemberPath = "CodeName";
                        rcb.SelectedValuePath = "CodeId";
                    }
                }
            }

            //if (thisDCSteps.matchCriteriaList != null)
            //    SetMatchData();
        }
    }
    public class Line
    {
        public int LineNumber { get; set; }
    }
}
