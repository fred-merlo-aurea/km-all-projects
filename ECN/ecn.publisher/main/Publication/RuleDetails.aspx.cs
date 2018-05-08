using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;

namespace ecn.publisher.main.Publication
{

	public partial class RuleDetails : System.Web.UI.Page
	{
        /*
		private int getRuleID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["RuleID"].ToString());
			}
			catch
			{
				return 0;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
            //if (!IsPostBack)
            //{
            //    DataTable dt = DataFunctions.GetDataTable("SELECT r.RuleName, e.EditionName, p.GroupID FROM Rules r join Editions e on r.editionID = r.editionID join Publications p on p.publicationID = e.publicationID WHERE r.RuleID=" + getRuleID());

            //    if (dt.Rows.Count > 0)
            //    {
            //        lblRuleName.Text = dt.Rows[0]["RuleName"].ToString() ;
            //        lblEditionName.Text = dt.Rows[0]["EditionName"].ToString();

            //        SqlCommand cmd = new SqlCommand(" SELECT ShortName FROM GroupDatafields WHERE GroupID="  + dt.Rows[0]["GroupID"].ToString());
            //        cmd.CommandTimeout = 0;
            //        cmd.CommandType = CommandType.Text;

            //        DataTable emailstable = DataFunctions.GetDataTable("communicator",cmd);

            //        int i = CompFieldName.Items.Count;
            //        foreach(DataRow dr in emailstable.Rows) 
            //        {
            //            CompFieldName.Items.Insert(i++, dr["ShortName"].ToString());
            //        }
            //        loadRulesDetailsGrid();
            //        BuildComparatorDR(0);
            //    }
            //    else
            //    {
            //        Response.Write("Invalid Rule ID");
            //    }
				
            //}
		}

		private void loadRulesDetailsGrid() 
		{
			
			DataTable dt = DataFunctions.GetDataTable("SELECT r.RuleName, rd.ruleDetailID, rd.FieldName, rd.Comparator, rd.CompareValue, rd.CompareType FROM RuleDetails rd join Rules r on r.RuleID = rd.RuleID WHERE r.RuleID=" + getRuleID());
			foreach ( DataRow dr in dt.Rows ) 
			{
				lblRuleName.Text = dr["RuleName"].ToString();
				break;
			}
			
			dgRuleDetails.DataSource=dt.DefaultView;
			dgRuleDetails.DataBind();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    
			this.dgRuleDetails.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgRuleDetails_DeleteCommand);

		}
		#endregion


        protected void ConvertToDataType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int CompFieldName_selIndex = Convert.ToInt32(CompFieldName.SelectedIndex.ToString());
            int ConvertToDataType_selIndex = Convert.ToInt32(ConvertToDataType.SelectedIndex.ToString());
            ErrorLabel.Text = "";
            ErrorLabel.Visible = false;
            if (ConvertToDataType_selIndex == 3)
            {
                Default_CompareValuePanel.Visible = false;
                DtTime_CompareValuePanel.Visible = true;
                BuildComparatorDR(3);
            }
            else if (ConvertToDataType_selIndex == 2)
            {
                Default_CompareValuePanel.Visible = true;
                DtTime_CompareValuePanel.Visible = false;
                BuildComparatorDR(ConvertToDataType_selIndex);
            }
            else
            {
                Default_CompareValuePanel.Visible = true;
                DtTime_CompareValuePanel.Visible = false;
                BuildComparatorDR(ConvertToDataType_selIndex);
            }

            //If "Convert to" is selected as Date
            if (ConvertToDataType_selIndex == 3)
            {
                if ((CompFieldName_selIndex < 21) || (CompFieldName_selIndex == 33))
                {
                    ErrorLabel.Visible = true;
                    ConvertToDataType.SelectedIndex = 0;
                    Default_CompareValuePanel.Visible = true;
                    DtTime_CompareValuePanel.Visible = false;
                    BuildComparatorDR(0);
                    ErrorLabel.Text = "ERROR: Compare Field that you have selected cannot be converted to DateTime. Please Correct";
                }
            }
            else if (ConvertToDataType_selIndex == 2)
            {
                CompValueNumberValidator.Enabled = true;
            }
            else
            {
                CompValueNumberValidator.Enabled = false;
                ErrorLabel.Visible = false;
            }
        }

        private void BuildComparatorDR(int index)
        {
            switch (index.ToString())
            {
                case "0":
                    Comparator.Items.Clear();
                    Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    Comparator.Items.Add(new ListItem("contains", "contains"));
                    Comparator.Items.Add(new ListItem("ends with", "ending with"));
                    Comparator.Items.Add(new ListItem("starts with", "starting with"));
                    Default_CompareValuePanel.Visible = true;
                    DtTime_CompareValuePanel.Visible = false;
                    break;

                case "1":
                    Comparator.Items.Clear();
                    Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    Comparator.Items.Add(new ListItem("contains", "contains"));
                    Comparator.Items.Add(new ListItem("ends with", "ending with"));
                    Comparator.Items.Add(new ListItem("starts with", "starting with"));
                    Default_CompareValuePanel.Visible = true;
                    DtTime_CompareValuePanel.Visible = false;
                    break;

                case "2":
                    Comparator.Items.Clear();
                    Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    Comparator.Items.Add(new ListItem("greater than [ > ]", "greater than"));
                    Comparator.Items.Add(new ListItem("less than [ < ]", "less than"));
                    Default_CompareValuePanel.Visible = true;
                    DtTime_CompareValuePanel.Visible = false;
                    break;

                case "3":
                    Comparator.Items.Clear();
                    Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    Comparator.Items.Add(new ListItem("greater than [ > ]", "greater than"));
                    Comparator.Items.Add(new ListItem("less than [ < ]", "less than"));
                    Comparator.Items.Add(new ListItem("between dates", "between"));
                    Default_CompareValuePanel.Visible = false;
                    DtTime_CompareValuePanel.Visible = true;
                    break;
                case "4":
                    Comparator.Items.Clear();
                    Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    Comparator.Items.Add(new ListItem("greater than [ > ]", "greater than"));
                    Comparator.Items.Add(new ListItem("less than [ < ]", "less than"));
                    Default_CompareValuePanel.Visible = true;
                    DtTime_CompareValuePanel.Visible = false;
                    break;

                default:
                    Comparator.Items.Clear();
                    Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
                    Comparator.Items.Add(new ListItem("contains", "contains"));
                    Comparator.Items.Add(new ListItem("ends with", "ending with"));
                    Comparator.Items.Add(new ListItem("starts with", "starting with"));
                    break;
            }
        }

        protected void AddRules_Click(object sender, System.EventArgs e)
        {

            string cmp_fieldName = CompFieldName.SelectedValue.ToString();
            bool cmp_comparatorChkbox = ComparatorChkBox.Checked;
            string cmp_comparator = Comparator.SelectedValue.ToString();
            if (cmp_comparatorChkbox)
            {
                cmp_comparator = "NOT " + cmp_comparator;
            }
            string cmp_value = CompValue.Text.ToString().Replace("'", "");
            string cmp_fieldType = ConvertToDataType.SelectedValue.ToString();
            int cmp_fieldTypeIndx = ConvertToDataType.SelectedIndex;
            string cmp_type = CompType.SelectedValue;

            string _compareValues = "";
            string _compareFieldName = "";

            if (cmp_fieldTypeIndx == 0)
            {
                _compareValues = cmp_value;
                _compareFieldName = cmp_fieldName;
            }
            else if (cmp_fieldTypeIndx > 0 && cmp_fieldTypeIndx < 3)
            {
                _compareValues = "CONVERT (" + cmp_fieldType + ", '" + cmp_value + "')";
                _compareFieldName = "CONVERT (" + cmp_fieldType + ", " + cmp_fieldName + ")";
            }
            else if (cmp_fieldTypeIndx == 3)
            {
                string frmDt = DtTime_Value1.Text.ToString();
                string toDt = DtTime_Value2.Text.ToString();
                if (frmDt.IndexOf("Today", 0) == -1)
                {
                    frmDt = "'" + frmDt + "'";
                }
                if (toDt.IndexOf("Today", 0) == -1)
                {
                    toDt = "'" + frmDt + "'";
                }

                if (Comparator.SelectedValue.ToString().Equals("between"))
                {
                    _compareValues = "CONVERT (" + cmp_fieldType + ", " + frmDt + ") AND CONVERT (" + cmp_fieldType + ", " + toDt + ")";
                    _compareFieldName = "CONVERT (" + cmp_fieldType + ", " + cmp_fieldName + ")";
                }
                else
                {
                    _compareValues = "CONVERT (DATETIME, " + frmDt + ")";
                    _compareFieldName = "CONVERT (" + cmp_fieldType + ", " + cmp_fieldName + ")";
                }
            }
            else if (cmp_fieldTypeIndx == 4)
            {
                _compareValues = "CONVERT (" + cmp_fieldType + ", '" + cmp_value + "')";
                _compareFieldName = "CONVERT (" + cmp_fieldType + ", " + cmp_fieldName + ")";
            }

            SqlCommand command = new SqlCommand(" INSERT INTO RuleDetails (RuleID, FieldType, CompareType, FieldName, Comparator, CompareValue) VALUES ( @RuleID, @cmp_fieldType, @cmp_type, @_compareFieldName, @cmp_comparator,@_compareValues)");

            command.Parameters.Add("@RuleID", SqlDbType.Int, 4).Value = getRuleID();
            command.Parameters.Add("@cmp_fieldType", SqlDbType.VarChar).Value = cmp_fieldType;
            command.Parameters.Add("@cmp_type", SqlDbType.VarChar).Value = cmp_type;
            command.Parameters.Add("@_compareFieldName", SqlDbType.VarChar).Value = _compareFieldName;
            command.Parameters.Add("@cmp_comparator", SqlDbType.VarChar).Value = cmp_comparator;
            command.Parameters.Add("@_compareValues", SqlDbType.VarChar).Value = _compareValues;
            DataFunctions.ExecuteScalar(command);

            UpdateFilterWhereclause();

            loadRulesDetailsGrid();
        }

        public void UpdateFilterWhereclause()
        {

            string sqlcreateFilter = " SELECT *  FROM RuleDetails rd join Rules r on rd.ruleID = r.ruleID WHERE r.RuleID= " + getRuleID();
            DataTable dt = DataFunctions.GetDataTable(sqlcreateFilter);
            string whereClause = "";
            string fieldName = "";
            string comparator = "";
            string compareValue = "";
            string compareType = "";
            string fieldType = "";
            bool made_where = false;
            //            bool made_dynamic_where = false;

            ArrayList arrEmailTblColumns = GetEmailStructure();

            foreach (DataRow dr in dt.Rows)
            {
                compareType = dr["compareType"].ToString();
                fieldName = dr["FieldName"].ToString();
                comparator = dr["Comparator"].ToString();
                compareValue = dr["compareValue"].ToString();
                fieldType = dr["FieldType"].ToString();

                if (!arrEmailTblColumns.Contains(fieldName.ToLower().ToString()))
                {
                    fieldName = "ISNULL(" + fieldName + ",'')";
                }

                if (fieldType.Equals("DATETIME") && comparator.Equals("equals"))
                {
                    fieldName = " CONVERT(varchar(10), " + fieldName + ", 101) ";
                    compareValue = " CONVERT(varchar(10), " + compareValue + ", 101)";
                }
                string sub_where = "";

                if (made_where)
                {
                    sub_where += " " + compareType + " ";
                }
                made_where = true;
                sub_where += fieldName;
                //}

                compareValue = compareValue.Replace("Today", "getDate()");
                compareValue = compareValue.Replace("[", "+(");
                compareValue = compareValue.Replace("]", ")");
                //compareValue = compareValue.Replace("'", "");

                if (comparator.StartsWith("ending"))
                {								//ending with
                    sub_where += " LIKE '%" + compareValue + "'";
                }
                else if (comparator.StartsWith("starting"))
                {						//starting with
                    sub_where += " LIKE '" + compareValue + "%'";
                }
                else if (comparator.Equals("contains"))
                {							//contatins
                    sub_where += " LIKE '%" + compareValue + "%'";
                }
                else if (comparator.Equals("equals"))
                {								//equals
                    if (fieldType.Length > 0)
                    {
                        sub_where += " = " + compareValue + "";
                    }
                    else
                    {
                        sub_where += " = '" + compareValue + "' ";
                    }
                }
                else if (comparator.Equals("greater than"))
                {						//greater than
                    sub_where += " > " + compareValue + "";
                }
                else if (comparator.Equals("less than"))
                {							//less than
                    sub_where += " < " + compareValue + "";
                }
                else if (comparator.Equals("between"))
                {							//between dates
                    sub_where += " BETWEEN " + compareValue + "";
                }
                else if (comparator.StartsWith("NOT ending"))
                {					//NOT ending with
                    sub_where += " NOT LIKE '%" + compareValue + "'";
                }
                else if (comparator.StartsWith("NOT starting"))
                {				//NOT starting with
                    sub_where += " NOT LIKE '" + compareValue + "%'";
                }
                else if (comparator.Equals("NOT contains"))
                {					//NOT Contains with
                    sub_where += " NOT LIKE '%" + compareValue + "%'";
                }
                else if (comparator.Equals("NOT equals"))
                {						//NOT equals
                    if (fieldType.Length > 0)
                    {
                        sub_where += " <> " + compareValue + "";
                    }
                    else
                    {
                        sub_where += " <> '" + compareValue + "' ";
                    }
                }
                else if (comparator.Equals("NOT greater than"))
                {				//NOT greater than which means 'less than'
                    sub_where += " < " + compareValue + "";
                }
                else if (comparator.Equals("NOT less than"))
                {					//NOT greater than which means 'greater than'
                    sub_where += " > " + compareValue + "";
                }
                else if (comparator.Equals("NOT between"))
                {							//between dates
                    sub_where += "NOT BETWEEN " + compareValue + "";
                }
                whereClause += sub_where;
                //}
            }
            whereClause = whereClause.Replace("user_", "");
            string sqlupdateFilter = " UPDATE Rules SET " +
                " WhereClause ='" + DataFunctions.CleanString(whereClause) + "' " +
                " WHERE RuleID=" + getRuleID();
            //throw new System.Exception(sqlupdateFilter);
            DataFunctions.Execute(sqlupdateFilter);
        }

        private ArrayList GetEmailStructure()
        {
            ArrayList arrEmails = new ArrayList();
            string sqlgetfilter = "select lower(c.name) as 'colname' from sysobjects o join syscolumns c on o.id = c.id where o.name = 'Emails'";
            DataTable dt = DataFunctions.GetDataTable(sqlgetfilter, ConfigurationManager.AppSettings["com"].ToString());
            foreach (DataRow dr in dt.Rows)
            {
                arrEmails.Add(dr["colname"].ToString());
            }
            return arrEmails;

        }

        private void dgRuleDetails_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            DataFunctions.Execute(" Delete from RuleDetails where RuleDetailID = " + dgRuleDetails.DataKeys[e.Item.ItemIndex].ToString());
            UpdateFilterWhereclause();

            Response.Redirect("RuleDetails.aspx?RuleID=" + getRuleID());
        }
        */
	}
}
