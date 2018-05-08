using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Entity;

namespace ecn.communicator.main.Salesforce.Controls
{
    public partial class SF_Search : System.Web.UI.UserControl
    {
        private const string DdlFieldName = "ddlField";
        private const string DdlBooleanName = "ddlBoolean";
        private const string TxtSearchName = "txtSearch";
        private const string DdlLogicName = "ddlLogic";
        private const string AccountName = "accountname";
        private const string PostalCode = "postalcode";
        private const string BithDate = "birthdate";
        private const string LastActivityDate = "lastactivitydate";
        private const string ShortFormat = "yyyy-MM-dd";
        private const string LongFormat = "yyyy-MM-ddThh:mm:ss.000Z";
        private const string Separator = ",";
        private const string Percent = "%";

        private List<GridItem> listGI
        {
            get
            {
                if (Session["listGI"] != null)
                {
                    return (List<GridItem>)Session["listGI"];
                }
                else
                    return null;
            }
            set
            {
                Session["listGI"] = value;
            }
        }
        private static List<string> listLogic;
        private List<System.Reflection.PropertyInfo> listFields
        {
            get
            {
                if (Session["listFields"] != null)
                    return (List<System.Reflection.PropertyInfo>)Session["listFields"];
                else
                    return null;
            }
            set
            {
                Session["listFields"] = value;
            }

        }
        private static bool isAccount;
        private static bool isLead;

        public string GetQuery()
        {
            const string whereExpression = "WHERE";
            var queryBuilder = new StringBuilder(whereExpression);

            foreach (GridViewRow row in gvSearch.Rows)
            {
                var ddlLogic = GetControlOrThrow<DropDownList>(row, DdlLogicName);
                var notLastRow = row.RowIndex < gvSearch.Rows.Count - 1;
                var expression = GetExpressionByRow(row);
                if (notLastRow)
                {
                    queryBuilder.Append($"{expression} {ddlLogic.SelectedItem.Text.ToUpper()}");
                }
                else
                {
                    queryBuilder.Append(expression);
                }
            }

            return queryBuilder.ToString();
        }

        public void ResetSearch()
        {
            listGI = new List<GridItem>();
            GridItem gi = new GridItem();
            listGI.Add(gi);

            gvSearch.DataSource = listGI;
            gvSearch.DataBind();

            upSearch.Update();
        }

        public void BindSearch(SF_Utilities.SFObject sfobject)
        {
            listGI = new List<GridItem>();
            listFields = new List<System.Reflection.PropertyInfo>();
            isAccount = false;
            isLead = false;
            if (sfobject.ToString().ToLower().Equals("contact"))
            {
                listFields = typeof(SF_Contact).GetProperties().Where(x => !x.Name.ToLower().Contains("id")).OrderBy(x => x.Name).ToList();
                

            }
            else if (sfobject.ToString().ToLower().Equals("lead"))
            {
                listFields = typeof(SF_Lead).GetProperties().Where(x => !x.Name.ToLower().Contains("id")).OrderBy(x => x.Name).ToList();
                isLead = true;
            }
            else if (sfobject.ToString().ToLower().Equals("account"))
            {
                listFields = typeof(SF_Account).GetProperties().Where(x => !x.Name.ToLower().Contains("id")).OrderBy(x => x.Name).ToList();
                isAccount = true;
            }

            List<string> listToRemove = new List<string>();
            listToRemove.Add("isdeleted");
            listToRemove.Add("lastactivity");
            listToRemove.Add("lastcurequestdate");
            listToRemove.Add("mailinglatitude");
            listToRemove.Add("mailinglongitude");
            listToRemove.Add("latitude");
            listToRemove.Add("longitude");
            listToRemove.Add("systemmodstamp");
            listToRemove.Add("otherlatitude");
            listToRemove.Add("otherlongitude");
            listToRemove.Add("salutation");
            listToRemove.Add("description");

            
            foreach (string s in listToRemove)
            {
                if (listFields.Exists(x => x.Name.ToLower() == s))
                    listFields.Remove(listFields.First(x => x.Name.ToLower() == s));
            }

            GridItem gi = new GridItem();
            listGI.Add(gi);

            gvSearch.DataSource = listGI;
            gvSearch.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                listLogic = GetLogic(false);

            }
        }

        protected void ddlLogic_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLogic = (DropDownList)sender;
            TableCell tc = (TableCell)ddlLogic.Parent;
            GridViewRow dgi = (GridViewRow)tc.Parent;
            string lastLogic = listGI[dgi.RowIndex].SelectedLogic;

            if (ddlLogic.SelectedIndex > 0)
            {
                listGI = BuildGridItems();
                if (dgi.RowIndex == gvSearch.Rows.Count - 1)
                {
                    GridItem gi = new GridItem();
                    listGI.Add(gi);

                    gvSearch.DataSource = listGI;
                    gvSearch.DataBind();
                }
            }
            else if (ddlLogic.SelectedIndex == 0 && listGI.Count > dgi.RowIndex + 1)
            {
                listGI = BuildGridItems();
                listGI.RemoveAt(dgi.RowIndex + 1);
                listGI[dgi.RowIndex].SelectedLogic = lastLogic;

                gvSearch.DataSource = listGI;
                gvSearch.DataBind();
            }
        }

        protected void gvSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlField = (DropDownList)e.Row.FindControl("ddlField");
                DropDownList ddlLogic = (DropDownList)e.Row.FindControl("ddlLogic");
                TextBox txtCriteria = (TextBox)e.Row.FindControl("txtSearch");
                DropDownList ddlBool = (DropDownList)e.Row.FindControl("ddlBoolean");

                GridItem gi = (GridItem)e.Row.DataItem;

                ddlField.DataSource = listFields;
                ddlField.DataTextField = "Name";
                ddlField.DataValueField = "Name";
                ddlField.DataBind();
                if (!isLead && !isAccount)
                {
                    ddlField.Items.Insert(0, new ListItem() { Text = "AccountName", Value = "AccountName" });
                }

                if (e.Row.RowIndex < listGI.Count - 1)
                {
                    ddlLogic.DataSource = GetLogic(true);
                }
                else
                    ddlLogic.DataSource = GetLogic(false);
                ddlLogic.DataBind();

                if (gi != null)
                {
                    try
                    {

                        ddlField.SelectedIndex = ddlField.Items.IndexOf(ddlField.Items.FindByText(gi.SelectedField));

                    }
                    catch { }

                    try
                    {
                        ddlLogic.SelectedIndex = listLogic.IndexOf(gi.SelectedLogic);
                    }
                    catch { }

                    try
                    {
                        if (!gi.SelectedField.ToLower().Equals("accountname"))
                        {
                            Type typ = listFields.First(x => x.Name == gi.SelectedField).PropertyType;
                            if (typ.Name.ToLower().Equals("boolean"))
                            {
                                txtCriteria.Text = string.Empty;
                                txtCriteria.Visible = false;
                                ddlBool.Visible = true;

                                ddlBool.SelectedValue = gi.Criteria;
                            }
                            else
                            {
                                ddlBool.Visible = false;
                                txtCriteria.Visible = true;
                                txtCriteria.Text = gi.Criteria;
                            }
                        }
                        else
                        {
                            ddlBool.Visible = false;
                            txtCriteria.Visible = true;
                            txtCriteria.Text = gi.Criteria;
                        }

                    }
                    catch { }
                }
            }
        }

        private List<string> GetLogic(bool hasRemove)
        {
            List<string> retList = new List<string>();
            if (hasRemove)
                retList.Add("REMOVE");
            else
                retList.Add("SELECT");

            retList.Add("AND");
            retList.Add("OR");

            return retList;
        }

        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlField = (DropDownList)sender;
            TableCell tc = (TableCell)ddlField.Parent;
            GridViewRow dgi = (GridViewRow)tc.Parent;
            TextBox txtCrit = (TextBox)dgi.FindControl("txtSearch");
            DropDownList ddlBool = (DropDownList)dgi.FindControl("ddlBoolean");

            if (!ddlField.SelectedItem.Text.ToLower().Equals("accountname"))
            {
                Type type = listFields.First(x => x.Name == ddlField.SelectedItem.Text).PropertyType;

                if (type.Name.ToLower().Equals("boolean"))
                {
                    txtCrit.Text = string.Empty;
                    txtCrit.Visible = false;

                    ddlBool.Visible = true;
                }
                else
                {
                    txtCrit.Text = string.Empty;
                    txtCrit.Visible = true;

                    ddlBool.Visible = false;
                }
            }
            else
            {
                txtCrit.Text = string.Empty;
                txtCrit.Visible = true;
                ddlBool.Visible = false;
            }
        }

        private string GetStringExpression(DropDownList ddlField, TextBox txtSearch)
        {
            var searchString = txtSearch.Text.Trim();
            const string emptyExpression = " = ''";
            var equalsExpression = $" = '{searchString}'";

            if (ddlField.SelectedItem.Text.Contains(PostalCode))
            {
                return equalsExpression;
            }
            else
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchString.Contains(Percent))
                    {
                        return $" LIKE '{searchString}'";
                    }
                    else
                    {
                        return equalsExpression;
                    }
                }
                else
                {
                    return emptyExpression;
                }
            }
        }

        private string GetDateTimeExpresion(DropDownList ddlField, TextBox txtSearch)
        {
            var date = new DateTime();
            DateTime.TryParse(txtSearch.Text, out date);
            if (ddlField.SelectedItem.Text.Equals(BithDate, StringComparison.OrdinalIgnoreCase) 
                || ddlField.SelectedItem.Text.Equals(LastActivityDate, StringComparison.OrdinalIgnoreCase))
            {
                return $" = {date.ToString(ShortFormat)}";
            }
            else
            {
                return $" >= {date.ToString(LongFormat)} and {ddlField.SelectedItem.Text} <= {date.AddDays(1).ToString(LongFormat)}";
            }
        }

        private string GetRightExpression(Type propertyType, DropDownList ddlField, TextBox txtSearch, DropDownList ddlBoolean)
        {
            if (propertyType == typeof(bool))
            {
                return $" = {ddlBoolean.SelectedValue}";
            }
            if (propertyType == typeof(double) || propertyType == typeof(int))
            {
                return $" = {txtSearch.Text.Trim()}";
            }
            if (propertyType == typeof(string))
            {
                return GetStringExpression(ddlField, txtSearch);

            }
            if (propertyType == typeof(DateTime))
            {
                return GetDateTimeExpresion(ddlField, txtSearch);
            }

            return null;
        }

        private string GetExpressionByRow(GridViewRow row)
        {
            var expression = string.Empty;
            var ddlField = GetControlOrThrow<DropDownList>(row, DdlFieldName);
            var ddlBoolean = GetControlOrThrow<DropDownList>(row, DdlBooleanName);
            var txtSearch = GetControlOrThrow<TextBox>(row, TxtSearchName);

            if (!ddlField.SelectedItem.Text.Equals(AccountName, StringComparison.OrdinalIgnoreCase))
            {
                var type = listFields.First(x => x.Name == ddlField.SelectedItem.Text).PropertyType;
                var leftExpression = ddlField.SelectedItem.Text;
                var rightExpression = GetRightExpression(type, ddlField, txtSearch, ddlBoolean);
                if (rightExpression != null)
                {
                    expression = $" {leftExpression}{rightExpression}";
                }
            }
            else
            {
                if (!isAccount)
                {
                    var accointIds = GetAccounts(txtSearch.Text).Select(x => $"'{x.Id}'").ToArray();
                    expression = $" AccountID IN ({string.Join(Separator, accointIds)})";
                }
                else
                {
                    expression = $" Name = '{txtSearch.Text.Trim()}'";
                }
            }

            return expression;
        }

        private List<SF_Account> GetAccounts(string search)
        {
            var whereExpression = search.Contains(Percent)
                ? $"WHERE Name LIKE '{search}'"
                : $"WHERE Name = '{search}'";

            return SF_Account.GetList(SF_Authentication.Token.access_token, whereExpression);
        }

        private List<GridItem> BuildGridItems()
        {
            var results = new List<GridItem>();
            foreach (GridViewRow row in gvSearch.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    var ddlField = GetControlOrThrow<DropDownList>(row, DdlFieldName);
                    var txtSearch = GetControlOrThrow<TextBox>(row, TxtSearchName);
                    var ddlBoolean = GetControlOrThrow<DropDownList>(row, DdlBooleanName);
                    var ddlBool = GetControlOrThrow<DropDownList>(row, DdlLogicName);

                    var item = new GridItem
                    {
                        SelectedField = ddlField.SelectedValue,
                        SelectedLogic = ddlBool.SelectedValue,
                        Criteria = txtSearch.Text
                    };

                    if (!ddlField.SelectedItem.Text.Equals(AccountName, StringComparison.OrdinalIgnoreCase))
                    {
                        var property = listFields.FirstOrDefault(x => x.Name == ddlField.SelectedItem.Text);
                        if (property?.PropertyType == typeof(bool))
                        {
                            item.Criteria = ddlBoolean.SelectedValue;
                        }
                    }

                    results.Add(item);
                }
            }

            return results;
        }

        private T GetControlOrThrow<T>(GridViewRow row, string name) where T : class
        {
            var element = row.FindControl(name) as T;
            if (element == null)
            {
                throw new ArgumentException($"There is no control with name: {name}");
            }

            return element;
        }
    }

    public partial class GridItem
    {
        public GridItem()
        {
            Criteria = string.Empty;
            SelectedLogic = string.Empty;
            SelectedField = string.Empty;
        }

        public string Criteria { get; set; }
        public string SelectedField { get; set; }
        public string SelectedLogic { get; set; }
    }

}