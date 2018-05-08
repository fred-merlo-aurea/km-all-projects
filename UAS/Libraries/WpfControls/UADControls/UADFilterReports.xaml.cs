using FrameworkUAS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace WpfControls.UADControls
{
    /// <summary>
    /// Interaction logic for UADFilterReports.xaml
    /// </summary>
    public partial class UADFilterReports : UserControl
    {
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();

        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();

        #region Variables/Lists
        private ObservableCollection<ExpressionBuilder> expressions = new ObservableCollection<ExpressionBuilder>();
        private ObservableCollection<string> builtExpressionsDisplay = new ObservableCollection<string>();
        private List<Expression> builtExpressions = new List<Expression>();
        private ObservableCollection<Helpers.Common.FilterCriteria> myFilters = new ObservableCollection<Helpers.Common.FilterCriteria>();
        private ObservableCollection<ReportCount> reportCounts = new ObservableCollection<ReportCount>();

        private List<FrameworkUAD_Lookup.Entity.Code> codes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private Guid accessKey;
        #endregion
        #region Classes
        public class ExpressionBuilder
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public ExpressionBuilder(string name, string type)
            {
                this.Name = name;
                this.Type = type;
            }
        }
        public class Expression
        {
            public List<ExpressionBuilder> Expressions { get; set; }

            public Expression(List<ExpressionBuilder> expressions)
            {
                this.Expressions = expressions;
            }
        }
        public class ReportCount
        {
            public string FilterName { get; set; }
            public int Count { get; set; }
            public List<int> SubscriberIDs { get; set; }

            public ReportCount(string name, int count, List<int> subIDs)
            {
                this.FilterName = name;
                this.Count = count;
                this.SubscriberIDs = subIDs;
            }
        }
        #endregion

        public UADFilterReports(ObservableCollection<Helpers.Common.FilterCriteria> filters)
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            CreateExpressions();
            rlbBuild.ItemsSource = expressions;
            rlbBuiltExpressions.ItemsSource = builtExpressionsDisplay;
            rlbFilters.ItemsSource = filters.Select(x=> x.FilterName);
            myFilters = filters;
            icReportCounts.ItemsSource = reportCounts;
            //rgvReports.ItemsSource = reportCounts;
        }

        private void CreateExpressions()
        {
            codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Report_Expression);
            if(Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                codes = codeResponse.Result;
            }

            rlbExpressions.ItemsSource = codes;
            rlbExpressions.DisplayMemberPath = "DisplayName";
            rlbExpressions.SelectedValuePath = "DisplayName";
        }

        #region UI Events
        private void rlbFilters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RadListBox rcb = sender as RadListBox;
            if (rcb.SelectedItem != null)
            {
                //if (expressions.Count == 3)
                //    MessageBox.Show("Max expression already created. Please complete existing combination or delete it.");
                if (expressions.Count > 0)
                {
                    if(expressions.Select(x=> x.Name).Contains(rcb.SelectedItem.ToString()))
                        MessageBox.Show("This filter is already used in the expression. Please select a different filter.");
                    else if (expressions.Last().Type != "Filter" && !expressions.Last().Name.Contains("ALL"))
                        expressions.Add(new ExpressionBuilder(rcb.SelectedItem.ToString(), "Filter"));
                    else if(expressions.Select(x=> x.Name).Contains(FrameworkUAD_Lookup.Enums.ReportExpressionTypes.NOT_IN.ToString().Replace("_", " ")))
                        expressions.Add(new ExpressionBuilder(rcb.SelectedItem.ToString(), "Filter"));
                    else
                        MessageBox.Show("Invalid combination. Connect filters with expressions unless using 'Not In'.");
                }
                else
                    expressions.Add(new ExpressionBuilder(rcb.SelectedItem.ToString(), "Filter"));
            }
        }

        private void rlbExpressions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RadListBox rcb = sender as RadListBox;
            if (rcb.SelectedItem != null)
            {
                #region ALL INTERSECT/UNION
                if (rcb.SelectedValue.ToString() == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.ALL_Intersect.ToString().Replace("_", " ") || rcb.SelectedValue.ToString() == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.ALL_Union.ToString().Replace("_", " "))
                {
                    if (expressions.Count == 0)
                    {
                        expressions.Add(new ExpressionBuilder(rcb.SelectedValue.ToString(), "Expression"));
                    }
                    else
                        MessageBox.Show("The ALL expressions are used without any other Filters or expressions. Please clear existing expression to add ALL.");
                }
                #endregion
                //else if (expressions.Count == 3)
                //    MessageBox.Show("Max expression already created. Please complete existing combination or delete it.");
                else if (expressions.Count > 0)
                {
                    if (rcb.SelectedValue.ToString() == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.NOT_IN.ToString().Replace("_", " ") && expressions.Select(x => x.Name).Contains(FrameworkUAD_Lookup.Enums.ReportExpressionTypes.NOT_IN.ToString().Replace("_", " ")))
                    {
                        MessageBox.Show("Only one 'Not In' expression is allowed per combination.");
                    }
                    else if (expressions.Last().Type == "Filter")
                        expressions.Add(new ExpressionBuilder(rcb.SelectedValue.ToString(), "Expression"));
                    else
                        MessageBox.Show("Invalid combination. Connect filters with expressions unless using 'Not In'.");
                }
                else
                    MessageBox.Show("Please add a Filter to begin a report expression or add an ALL expression.");
            }
        }

        private void btnAddExpression_Click(object sender, RoutedEventArgs e)
        {
            //if (expressions.Count == 3 || (expressions.Count == 1 && expressions.Last().Name.ToString() == Enums.ReportExpressionTypes.ALL_Intersect.ToString().Replace("_", " ") || 
            //    expressions.Last().Name.ToString() == Enums.ReportExpressionTypes.ALL_Union.ToString().Replace("_", " ")))
            //{
            //    string combo = "";
            //    List<ExpressionBuilder> ex = new List<ExpressionBuilder>();
            //    foreach (ExpressionBuilder eb in expressions)
            //    {
            //        combo = combo + eb.Name + " ";
            //        ex.Add(eb);
            //    }

            //    combo = combo.TrimEnd(' ');

            //    if (!builtExpressionsDisplay.Contains(combo))
            //    {
            //        builtExpressionsDisplay.Add(combo);
            //        builtExpressions.Add(new Expression(ex));
            //        expressions.Clear();
            //    }
            //    else
            //    {
            //        MessageBox.Show("You have already added this expression.", "Warning", MessageBoxButton.OK);
            //        expressions.Clear();
            //    }
            //}
            //else
            //    MessageBox.Show("Invalid expression.");
            if (IsExpressionValid())
            {
                string combo = "";
                List<ExpressionBuilder> ex = new List<ExpressionBuilder>();
                foreach (ExpressionBuilder eb in expressions)
                {
                    combo = combo + eb.Name + " ";
                    ex.Add(eb);
                }

                combo = combo.TrimEnd(' ');

                if (!builtExpressionsDisplay.Contains(combo))
                {
                    builtExpressionsDisplay.Add(combo);
                    builtExpressions.Add(new Expression(ex));
                    expressions.Clear();
                }
                else
                {
                    MessageBox.Show("You have already added this expression.", "Warning", MessageBoxButton.OK);
                    expressions.Clear();
                }
            }
            else
                MessageBox.Show("Invalid expression.");
        }

        private void btnRemoveCol_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            ExpressionBuilder ex = btn.DataContext as ExpressionBuilder;
            expressions.Remove(ex);
        }

        private void btnRemoveEx_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            string ex = btn.DataContext as string;
            builtExpressionsDisplay.Remove(ex);
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            grdBuildReports.Visibility = Visibility.Collapsed;
            foreach (Expression ex in builtExpressions)
            {
                GetReportCount(ex);
            }
            grdReportCounts.Visibility = Visibility.Visible;
        }
        #endregion

        private void GetReportCount(Expression ex)
        {
            #region ALL Expressions
            if (ex.Expressions[0].Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.ALL_Intersect.ToString().Replace("_", " ") ||
                ex.Expressions[0].Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.ALL_Union.ToString().Replace("_", " "))
            {
                if (ex.Expressions[0].Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.ALL_Intersect.ToString().Replace("_", " "))
                {
                    List<int> subs = new List<int>();
                    for (int i = 0; i < myFilters.Count; i++)
                    {
                        if (i == 0)
                            subs = myFilters[i].SubscriberIDs;
                        else
                            subs = subs.Intersect(myFilters[i].SubscriberIDs).ToList();

                    }
                    reportCounts.Add(new ReportCount(ex.Expressions[0].Name, subs.Count, subs));
                }
                else if (ex.Expressions[0].Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.ALL_Union.ToString().Replace("_", " "))
                {
                    List<int> subs = new List<int>();
                    for (int i = 0; i < myFilters.Count; i++)
                    {
                        if (i == 0)
                            subs = myFilters[i].SubscriberIDs;
                        else
                            subs = subs.Union(myFilters[i].SubscriberIDs).ToList();

                    }
                    reportCounts.Add(new ReportCount(ex.Expressions[0].Name, subs.Count, subs));
                }
            }
            #endregion
            #region Combination Expressions
            else
            {
                Helpers.Common.FilterCriteria f = myFilters.Where(x=> x.FilterName == ex.Expressions[0].Name).FirstOrDefault();
                List<int> subs = new List<int>();
                string expression = "";
                FrameworkUAD_Lookup.Enums.ReportExpressionTypes? current = null;

                foreach(ExpressionBuilder exb in ex.Expressions)
                {
                    if (exb.Type == "Filter")
                    {
                        f = myFilters.Where(x => x.FilterName == exb.Name).FirstOrDefault();
                        if (current.HasValue && current.Value == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.NOT_IN)
                        {
                            if (subs.Count > 0)
                                subs = subs.Except(f.SubscriberIDs).ToList();
                            else
                                subs = f.SubscriberIDs;

                            expression += f.FilterName + " ";
                        }
                        else if (current.HasValue && current.Value == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.INTERSECT)
                        {
                            if (subs.Count > 0)
                                subs = subs.Intersect(f.SubscriberIDs).ToList();
                            else
                                subs = f.SubscriberIDs;

                            expression += f.FilterName + " ";
                        }
                        else if (current.HasValue && current.Value == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.UNION)
                        {
                            if (subs.Count > 0)
                                subs = subs.Union(f.SubscriberIDs).ToList();
                            else
                                subs = f.SubscriberIDs;

                            expression += f.FilterName + " ";
                        }
                        else
                        {
                            expression += f.FilterName + " ";
                            if (subs.Count == 0)
                                subs = f.SubscriberIDs;
                        }
                    }
                    else if (exb.Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.INTERSECT.ToString().Replace("_", " "))
                    {
                        current = FrameworkUAD_Lookup.Enums.ReportExpressionTypes.INTERSECT;
                        expression += exb.Name + " ";
                    }
                    else if (exb.Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.NOT_IN.ToString().Replace("_", " "))
                    {
                        current = FrameworkUAD_Lookup.Enums.ReportExpressionTypes.NOT_IN;
                        expression += exb.Name + " ";
                    }
                    else if (exb.Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.UNION.ToString().Replace("_", " "))
                    {
                        current = FrameworkUAD_Lookup.Enums.ReportExpressionTypes.UNION;
                        expression += exb.Name + " ";
                    }
                }
                reportCounts.Add(new ReportCount(expression.Trim(), subs.Count, subs));
            }
            #endregion
        }

        private bool IsExpressionValid()
        {
            #region All Expressions
            if (expressions.Count == 1 && expressions.Last().Name.ToString() == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.ALL_Intersect.ToString().Replace("_", " ") ||
                expressions.Last().Name.ToString() == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.ALL_Union.ToString().Replace("_", " "))
            {
                return true;
            }
            #endregion
            else if (expressions.Count >= 3)
            {
                if (expressions[0].Type == "Filter")
                {
                    #region Intersect, Union Expressions
                    if (expressions[1].Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.INTERSECT.ToString().Replace("_", " ") || expressions[1].Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.UNION.ToString().Replace("_", " "))
                    {
                        string current = "";
                        for (int i = 2; i < expressions.Count; i++)
                        {
                            if (expressions[i].Type == current)
                                return false;
                            else if (expressions[i].Name != FrameworkUAD_Lookup.Enums.ReportExpressionTypes.INTERSECT.ToString().Replace("_", " ") && expressions[1].Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.UNION.ToString().Replace("_", " ") &&
                                     expressions[i].Type != "Filter")
                            {
                                return false;
                            }
                            else
                                current = expressions[i].Type;
                        }
                        if(current == "Filter")
                            return true;
                    }
                    #endregion
                    #region Not In Expressions
                    else if (expressions[1].Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.NOT_IN.ToString().Replace("_", " "))
                    {
                        string current = "";
                        for (int i = 2; i < expressions.Count; i++)
                        {
                            if (expressions[i].Type == current && expressions[i].Type != "Filter")
                                return false;
                            else if (expressions[i].Name != FrameworkUAD_Lookup.Enums.ReportExpressionTypes.INTERSECT.ToString().Replace("_", " ") && expressions[1].Name == FrameworkUAD_Lookup.Enums.ReportExpressionTypes.UNION.ToString().Replace("_", " ") &&
                                     expressions[i].Type != "Filter")
                            {
                                return false;
                            }
                            else
                                current = expressions[i].Type;
                        }
                        return true;
                    }
                    #endregion
                }
            }

            return false;
        }

        private void lbPopUp_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.Name == "lbStandardReports")
            {
                lb.ItemsSource = new List<string>() { "Hello World", "Hello" };
            }
            else if (lb.Name == "lbGeoReports")
            {
                lb.ItemsSource = new List<string>() { "Geo", "Geo" };
            }
        }
    }
}
