using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.OtherControls
{
    public partial class ruleEditor : System.Web.UI.UserControl
    {
        private DataTable Rule_DT
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["Rule_DT"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["Rule_DT"] = value;
            }
        }

        public int selectedRuleID
        {
            get
            {
                if (Request.QueryString["RuleID"] != null)
                    return Convert.ToInt32(Request.QueryString["RuleID"]);
                else if (ViewState["selectedRuleID"] != null)
                    return (int)ViewState["selectedRuleID"];
                else
                    return 0;
            }
            set
            {
                ViewState["selectedRuleID"] = value;
            }
        }

        private int getRuleID()
        {
            if (Request.QueryString["RuleID"] != null)
                return Convert.ToInt32(Request.QueryString["RuleID"].ToString());
            else if (ViewState["selectedRuleID"] != null)
                return (int)ViewState["selectedRuleID"];
            else
                return 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            if (!IsPostBack)
            {
                loadProfileFields();
                if (getRuleID() > 0)
                {
                    loadData(getRuleID());
                    lblRule.Text = "Edit Rule";
                }
                else
                {
                    loadGrid();
                }
            }
            else
            {
                var value = drpComparator.SelectedValue;
                if (value.Contains("Empty"))
                {
                    txtValue.Text = string.Empty;
                    txtValue.Enabled = false;
                }
                else
                {
                    txtValue.Enabled = true;
                }
            }
        }

        private DataTable createRuleConditionTable()
        {
            DataTable dt = new DataTable();
            DataColumn RuleConditionID = new DataColumn("RuleConditionID", typeof(string));
            dt.Columns.Add(RuleConditionID);

            DataColumn Field = new DataColumn("Field", typeof(string));
            dt.Columns.Add(Field);

            DataColumn DataType = new DataColumn("DataType", typeof(string));
            dt.Columns.Add(DataType);

            DataColumn Comparator = new DataColumn("Comparator", typeof(string));
            dt.Columns.Add(Comparator);

            DataColumn Value = new DataColumn("Value", typeof(string));
            dt.Columns.Add(Value);

            DataColumn IsDeleted = new DataColumn("IsDeleted", typeof(bool));
            dt.Columns.Add(IsDeleted);

            dt.AcceptChanges();
            return dt;
        }

        public void LoadExisting(int ruleID)
        {
            ViewState["RuleID"] = ruleID.ToString();

            lblRule.Text = "Edit Rule";
            ECN_Framework_Entities.Communicator.Rule rule = ECN_Framework_BusinessLayer.Communicator.Rule.GetByRuleID(ruleID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
            if (rule.RuleConditionsList != null)
            {
                DataTable dt = createRuleConditionTable();
                foreach (ECN_Framework_Entities.Communicator.RuleCondition RuleCondition in rule.RuleConditionsList)
                {
                    DataRow dr = dt.NewRow();
                    dr["RuleConditionID"] = RuleCondition.RuleConditionID.ToString();
                    dr["Field"] = RuleCondition.Field;
                    dr["DataType"] = RuleCondition.DataType;
                    dr["Comparator"] = RuleCondition.Comparator;
                    dr["Value"] = RuleCondition.Value;
                    dr["IsDeleted"] = RuleCondition.IsDeleted;
                    dt.Rows.Add(dr);
                }
                Rule_DT = dt;
            }
            txtRuleName.Text = rule.RuleName;
            drpConditionConnector.SelectedValue = rule.ConditionConnector;
            loadGrid();
        }

        private void loadData(int RuleID)
        {
            ECN_Framework_Entities.Communicator.Rule rule = ECN_Framework_BusinessLayer.Communicator.Rule.GetByRuleID(RuleID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
            if (rule.RuleConditionsList != null)
            {
                DataTable dt = createRuleConditionTable();
                foreach (ECN_Framework_Entities.Communicator.RuleCondition RuleCondition in rule.RuleConditionsList)
                {
                    DataRow dr = dt.NewRow();
                    dr["RuleConditionID"] = RuleCondition.RuleConditionID.ToString();
                    dr["Field"] = RuleCondition.Field;
                    dr["DataType"] = RuleCondition.DataType;
                    dr["Comparator"] = RuleCondition.Comparator;
                    dr["Value"] = RuleCondition.Value;
                    dr["IsDeleted"] = RuleCondition.IsDeleted;
                    dt.Rows.Add(dr);
                }
                Rule_DT = dt;
            }
            txtRuleName.Text = rule.RuleName;
            drpConditionConnector.SelectedValue = rule.ConditionConnector;
            loadGrid();
        }

        private void loadGrid()
        {
            if (Rule_DT != null)
            {
                var result = (from src in Rule_DT.AsEnumerable()
                              where src.Field<bool>("IsDeleted") == false
                              select new
                              {
                                  RuleConditionID = src.Field<string>("RuleConditionID"),
                                  Field = src.Field<string>("Field"),
                                  Comparator = src.Field<string>("Comparator"),
                                  Value = src.Field<string>("Value"),
                                  IsDeleted = src.Field<bool>("IsDeleted")
                              }).ToList();
                gvRuleCondition.DataSource = result;
            }
            else
            {
                gvRuleCondition.DataSource = new DataTable();
            }
            gvRuleCondition.DataBind();
        }

        public void Reset()
        {
            txtRuleName.Text = "";
            drpConditionConnector.SelectedValue = "OR";
            Rule_DT = null;
            selectedRuleID = 0;
            loadGrid();
 
        }

        public int SaveRule()
        {
            KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            ECN_Framework_Entities.Communicator.Rule Rule = new ECN_Framework_Entities.Communicator.Rule();           
            try
            {
                if (getRuleID() > 0)
                {
                    Rule = ECN_Framework_BusinessLayer.Communicator.Rule.GetByRuleID(getRuleID(), currentUser, true);
                    Rule.UpdatedUserID = currentUser.UserID;
                }
                else
                {
                    Rule.CreatedUserID = currentUser.UserID;
                    Rule.RuleConditionsList = new List<ECN_Framework_Entities.Communicator.RuleCondition>();
                }
                Rule.CustomerID = currentUser.CustomerID;
                
                Rule.RuleName = txtRuleName.Text;
                Rule.ConditionConnector = drpConditionConnector.SelectedValue;
                if (Rule_DT != null)
                {
                    foreach (DataRow dr in Rule_DT.AsEnumerable())
                    {
                        string isDeleted = dr["IsDeleted"].ToString();
                        if (dr["RuleConditionID"].ToString().Contains("-") && isDeleted.Equals("False"))
                        {
                            ECN_Framework_Entities.Communicator.RuleCondition RuleCondition;
                            if (dr["RuleConditionID"].ToString().Contains("-"))
                            {
                                RuleCondition = new ECN_Framework_Entities.Communicator.RuleCondition();
                                RuleCondition.Field = dr["Field"].ToString();
                                RuleCondition.DataType = dr["DataType"].ToString();
                                RuleCondition.Comparator = dr["Comparator"].ToString();
                                RuleCondition.Value = dr["Value"].ToString();
                                RuleCondition.CreatedUserID = currentUser.UserID;
                                RuleCondition.IsDeleted = false;
                                Rule.RuleConditionsList.Add(RuleCondition);
                            }
                        }
                        if (isDeleted.Equals("True") && !dr["RuleConditionID"].ToString().Contains("-") && getRuleID() > 0)
                        {
                            ECN_Framework_Entities.Communicator.RuleCondition r = Rule.RuleConditionsList.FirstOrDefault(x => x.RuleConditionID == Convert.ToInt32(dr["RuleConditionID"].ToString()));
                            if (r != null)
                                r.IsDeleted = true;
                        }
                    }
                }
                else
                {
                    ECN_Framework_Common.Objects.ECNError ecnError = new ECNError(Enums.Entity.Rule, Enums.Method.Save, "A Rule must have at least one condition");
                    List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECNError>();
                    errorList.Add(ecnError);
                    ECNException ecnException = new ECNException(errorList);
                    setECNError(ecnException);
                    return -1;
                }

                ECN_Framework_Entities.Communicator.RuleCondition rNotDeleted = Rule.RuleConditionsList.FirstOrDefault(x => x.IsDeleted == false);
                if (rNotDeleted == null)
                {
                    ECN_Framework_Common.Objects.ECNError ecnError = new ECNError(Enums.Entity.Rule, Enums.Method.Save, "A Rule must have at least one condition");
                    List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECNError>();
                    errorList.Add(ecnError);
                    ECNException ecnException = new ECNException(errorList);
                    setECNError(ecnException);
                    return -1;
                }

                ECN_Framework_BusinessLayer.Communicator.Rule.Save(Rule, currentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
                return -1;
            }
            return Rule.RuleID;
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void loadProfileFields()
        {
            drpField.Items.Clear();
            drpField.Items.Add(new ListItem("EmailAddress", "EmailAddress"));
            drpField.Items.Add(new ListItem("Title", "Title"));
            drpField.Items.Add(new ListItem("FirstName", "FirstName"));
            drpField.Items.Add(new ListItem("LastName", "LastName"));
            drpField.Items.Add(new ListItem("FullName", "FullName"));
            drpField.Items.Add(new ListItem("Company", "Company"));
            drpField.Items.Add(new ListItem("Occupation", "Occupation"));
            drpField.Items.Add(new ListItem("Address", "Address"));
            drpField.Items.Add(new ListItem("Address2", "Address2"));
            drpField.Items.Add(new ListItem("City", "City"));
            drpField.Items.Add(new ListItem("State", "State"));
            drpField.Items.Add(new ListItem("Zip", "Zip"));
            drpField.Items.Add(new ListItem("Country", "Country"));
            drpField.Items.Add(new ListItem("Voice", "Voice"));
            drpField.Items.Add(new ListItem("Mobile", "Mobile"));
            drpField.Items.Add(new ListItem("Fax", "Fax"));
            drpField.Items.Add(new ListItem("Website", "Website"));
            drpField.Items.Add(new ListItem("Age", "Age"));
            drpField.Items.Add(new ListItem("Income", "Income"));
            drpField.Items.Add(new ListItem("Gender", "Gender"));
            drpField.Items.Add(new ListItem("User1", "User1"));
            drpField.Items.Add(new ListItem("User2", "User2"));
            drpField.Items.Add(new ListItem("User3", "User3"));
            drpField.Items.Add(new ListItem("User4", "User4"));
            drpField.Items.Add(new ListItem("User5", "User5"));
            drpField.Items.Add(new ListItem("User6", "User6"));
            drpField.Items.Add(new ListItem("Birthdate", "Birthdate"));
            drpField.Items.Add(new ListItem("UserEvent1", "UserEvent1"));
            drpField.Items.Add(new ListItem("UserEvent1Date", "UserEvent1Date"));
            drpField.Items.Add(new ListItem("UserEvent2", "UserEvent2"));
            drpField.Items.Add(new ListItem("UserEvent2Date", "UserEvent2Date"));
            drpField.Items.Add(new ListItem("Notes", "Notes"));
            drpField.Items.Add(new ListItem("Profile Added", "CreatedOn"));
            drpField.Items.Add(new ListItem("Profile Updated", "LastChanged"));
        }

        private void loadUDFs()
        {
            KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            drpField.Items.Clear();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> udfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByCustomerID(currentUser.CustomerID, currentUser);
            var result = (from src in udfList
                          orderby src.ShortName
                        select src.ShortName).ToList().Distinct(StringComparer.CurrentCultureIgnoreCase);
            drpField.DataSource = result;
            drpField.DataBind();
        }

        protected void gvRuleCondition_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string RuleConditionID = e.CommandArgument.ToString();
            if (e.CommandName.Equals("RuleConditionDelete"))
            {
                foreach (DataRow dr in Rule_DT.AsEnumerable())
                {
                    if (dr["RuleConditionID"].Equals(RuleConditionID))
                    {
                        dr["IsDeleted"] = true;
                    }
                }
                loadGrid();
            }
        }

        protected void btnAddCondition_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string field = (btn.Parent.FindControl("drpField") as DropDownList).SelectedValue;
            string comparator = (btn.Parent.FindControl("drpComparator") as DropDownList).SelectedValue;
            string DataType = (btn.Parent.FindControl("drpDataType") as DropDownList).SelectedValue;
            string value = (btn.Parent.FindControl("txtValue") as TextBox).Text;

            DataTable dt = Rule_DT;
            if (dt == null)
            {
                dt = createRuleConditionTable();
            }
            DataRow dr = dt.NewRow();
            dr["RuleConditionID"] = Guid.NewGuid();
            dr["Field"] = field;
            dr["DataType"] = DataType;
            dr["Comparator"] = comparator;
            dr["Value"] = value;
            dr["IsDeleted"] = false;
            dt.Rows.Add(dr);

            (btn.Parent.FindControl("txtValue") as TextBox).Text = "";
            Rule_DT = dt;
            loadGrid();
        }

        protected void drpDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpDataType.SelectedValue.Equals("String"))
            {
                loadComparatorsString();
            }
            else if (drpDataType.SelectedValue.Equals("Date"))
            {
                loadComparatorsDate();
            }
            else if (drpDataType.SelectedValue.Equals("Number"))
            {
                loadComparatorsNumber();
            }
        }

        private void loadComparatorsString()
        {
            drpComparator.Items.Clear();
            drpComparator.Items.Add("Equals");
            drpComparator.Items.Add("Not Equals");
            drpComparator.Items.Add("Contains");
            drpComparator.Items.Add("Starts With");
            drpComparator.Items.Add("Ends With");
            drpComparator.Items.Add("Is Empty");
            drpComparator.Items.Add("Is Not Empty");

            txtValueCalendarExtender.Enabled = false;
        }

        private void loadComparatorsDate()
        {
            drpComparator.Items.Clear();
            drpComparator.Items.Add("Equals");
            drpComparator.Items.Add("Not Equals");
            drpComparator.Items.Add("Greater than");
            drpComparator.Items.Add("Less than");
            drpComparator.Items.Add("Is Empty");
            drpComparator.Items.Add("Is Not Empty");
            txtValueCalendarExtender.Enabled = true;
        }

        private void loadComparatorsNumber()
        {
            drpComparator.Items.Clear();
            drpComparator.Items.Add("Equals");
            drpComparator.Items.Add("Not Equals");
            drpComparator.Items.Add("Greater than");
            drpComparator.Items.Add("Less than");
            drpComparator.Items.Add("Is Empty");
            drpComparator.Items.Add("Is Not Empty");
            txtValueCalendarExtender.Enabled = false;
        }

        protected void drpField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblFieldType.SelectedValue.Equals("Profile"))
            {
                if (drpField.SelectedValue.Equals("Birthdate") || drpField.SelectedValue.Equals("CreatedOn") || drpField.SelectedValue.Equals("LastChanged")
                 || drpField.SelectedValue.Equals("UserEvent1Date") || drpField.SelectedValue.Equals("UserEvent2Date"))
                {
                    loadComparatorsDate();
                    drpDataType.SelectedValue = "Date";
                }
                else
                {
                    loadComparatorsString();
                    drpDataType.SelectedValue = "String";
                }
            }
        }

        protected void rblFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblFieldType.SelectedValue.Equals("Profile"))
            {
                loadProfileFields();
                pnlDataType.Visible = false;
            }
            else
            {
                loadUDFs();
                pnlDataType.Visible = true;
            }
        }
    }
}