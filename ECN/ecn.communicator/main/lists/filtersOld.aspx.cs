using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace ecn.communicator.listsmanager
{
    public partial class filters : ECN_Framework.WebPageHelper
    {
        /*
        protected void Page_Load(object sender, System.EventArgs e)
        {
            
           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS; 
            Master.SubMenu = "";
            Master.Heading = "Create Filters";
            Master.HelpContent = "<B>Adding/Editing Filters</B><div id='par1'><ul><li>Find the Group you want to create a filter for.</li><li>Click on the <em>Funnel</em> icon for that group.</li><li>Enter a title for your filter (for example, pet owners)</li><li>Click <em>Create new filter</em>.</li><li>Under filter names, click on the <em>pencil (Add/Edit Filter Attributes)</em> icon to define the filter attributes.</li><li>In the Compare Field section, use the drop down menu and click on profile field to define attributes of your filter.</li><li>In the Comparator section you have the option of making the field equal to (=), contains, ends with, or starts with.</li><li>In the Compare Value field, enter the information you would want the system to filter (for example, dog).</li><li>The Join Filters allow you to select And, or, Or.</li><li>To add, click <em>Add this Filter</em>.</li><li>Repeat this process several times to fully develop the attributes you are looking for (for example, dog, dogs, cat, cats, dog owners, etc.)</li><li>After all fields and attributes have been selected and added, click <em>Preview filtered e-mails</em> button to view emails in your filtered list.</li><li>When filter is complete, Click on <em>Return to Filters List</em>.</li></ul></div>";
            Master.HelpTitle = "Filters Manager";	

            if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "grouppriv") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                PreviewFilterButton.Attributes.Add("onclick", "window.open('filterPreview.aspx?filterID=" + getFilterID() + "', 'Preview', 'width=550,height=500,resizable=yes,scrollbars=yes,status=yes');");

                if (Page.IsPostBack == false)
                {
                    string requestGroupID = getGroupID();
                    string action = getAction();
                    string filterID = getFilterID();
                    string filterDetailID = getFilterDetailID();
                    string filterDetaildisplay = getFilterDetailDisplay();

                    if (action != "deleteFilterDetail")
                    {
                        // Add the user defined fields
                        if (requestGroupID.Equals("0"))
                        {
                            ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(Convert.ToInt32(filterID), Master.UserSession.CurrentUser);
                            requestGroupID = filter.GroupID.Value.ToString();
                        }

                        List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(Convert.ToInt32(requestGroupID), Master.UserSession.CurrentUser);
                        int i = CompFieldName.Items.Count;
                        foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList)
                        {
                            CompFieldName.Items.Insert(i++, groupDataFields.ShortName);
                        }
                        BuildComparatorDR(0);
                    }
                    if (filterDetaildisplay.Equals("true"))
                    {
                        FilterAttribsPanel.Visible = true;
                        AddFilterButtonPanel.Visible = false;
                        FiltersPanel.Visible = false;
                        loadAddFiltersGrid(filterID);
                       
                    }

                    if (action == "deleteFilter")
                    {
                      DeleteFilter(filterID);
                    }
                    else if (action == "editFilter")
                    {
                       EditFilter(filterID);
                    }
                    else if (action == "deleteFilterDetail")
                    {
                      DeleteFilterDetail(filterDetailID);
                    }
                    else
                    {
                        string CustomerID = Master.UserSession.CurrentUser.CustomerID.ToString();
                        loadFiltersGrid(CustomerID, requestGroupID);
                    }
                }
                else
                {
                    loadFiltersGrid(Master.UserSession.CurrentUser.CustomerID.ToString(), getGroupID());
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        #region Request Parameters
        private int getGroupID()
        {
            if (Request.QueryString["GroupID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["GroupID"].ToString());
            }
            else
                return 0;
        }

        private string getAction()
        {
            String theAction = "";
            try
            {
                theAction = Request.QueryString["action"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theAction;
        }

        private int getFilterID()
        {
            if (Request.QueryString["FilterID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["FilterID"].ToString());
            }
            else
                return 0;
        }

        private int getFilterDetailID()
        {
            if (Request.QueryString["FilterDetailID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["FilterDetailID"].ToString());
            }
            else
                return 0;
        }

        private string getFilterDetailDisplay()
        {
            string display = "";
            try
            {
                display = Request.QueryString["fd"].ToString();
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return display;
        }
        #endregion

        #region Data Load
        private void loadFiltersGrid(int CustomerID, int GroupID)
        {
            List<ECN_Framework_Entities.Communicator.Filter> filterList=
            ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(GroupID, Master.UserSession.CurrentUser);

            ECN_Framework_Entities.Communicator.Group myGroup =
            ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, Master.UserSession.CurrentUser);

            var result = (from src in filterList
                         select new
                         {
                             FilterName= src.FilterName,
                             CreateDate= src.CreatedDate,
                             FilterID= src.FilterID,
                             GroupName = myGroup.GroupName
                         }).ToList();

            foreach (ECN_Framework_Entities.Communicator.Filter filter in filterList)
            {
                GroupNameDisplay.Text = myGroup.GroupName;
                break;
            }

            FiltersGrid.DataSource = result;
            FiltersGrid.DataBind();
            FiltersPager.RecordCount = result.Count;
        }

        private void loadAddFiltersGrid(int FilterID)
        {
            string sqlquery = "SELECT fd.FDID, fd.FieldName, fd.Comparator, fd.CompareValue, f.FilterName, fd.CompareType " +
                " FROM FiltersDetails fd, Filters f" +
                " WHERE fd.FilterID=" + FilterID + " and f.FilterID = fd.FilterID";

            DataTable dt = DataFunctions.GetDataTable(sqlquery);
            foreach (DataRow dr in dt.Rows)
            {
                FilterNameTxtLabel.Text = dr["Filtername"].ToString();
                FilterIDValue.Text = FilterID;
                break;
            }

            AddFiltersGridArray.DataSource = dt.DefaultView;
            AddFiltersGridArray.DataBind();

            FilterPreviewPanel.Visible = true;
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
        #endregion

        #region Data Handlers
        public void FiltersList(object sender, System.EventArgs e)
        {
            string sqlgetgroup = " SELECT GroupID FROM Filters WHERE FilterID=" + getFilterID();
            string theGroupID = DataFunctions.ExecuteScalar(sqlgetgroup).ToString();

            Response.Redirect("filters.aspx?GroupID=" + theGroupID);
        }

        public void DisplayAddFilterForm(object sender, System.EventArgs e)
        {
            string sqlquery = "";
            string lastInsertedFilterID = "";

            string now = System.DateTime.Now.ToString();
            string FilterName = DataFunctions.CleanString(FilterNameTxt.Text);
            sqlquery =
                " INSERT INTO Filters ( " +
                " CustomerID, UserID, GroupID, FilterName, WhereClause, CreateDate" +
                " ) VALUES ( " + Master.UserSession.CurrentUser.CustomerID + ", " + Master.UserSession.CurrentUser.UserID + ", " +
                getGroupID() + ", '" + FilterName + "','','" + now + "'); SELECT @@IDENTITY";

            lastInsertedFilterID = DataFunctions.ExecuteScalar(sqlquery).ToString();

            FilterAttribsPanel.Visible = false;
            AddFilterButtonPanel.Visible = true;
            FiltersPanel.Visible = true;
            FilterIDValue.Text = lastInsertedFilterID;
            FilterNameTxtValue.Text = "";
            CompValue.Text = "";

            String CustomerID = Master.UserSession.CurrentUser.CustomerID.ToString();
            loadFiltersGrid(CustomerID, getGroupID());
        }

        public void AddFilter(object sender, System.EventArgs e)
        {
            string sqlquery = "";
            string lastInsertedFilterID = FilterIDValue.Text.ToString();

            string cmp_fieldName = "[" + CompFieldName.SelectedValue.ToString() + "]";
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
                    toDt = "'" + toDt + "'";
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

            string connection_string = DataFunctions.GetConnectionString();
            SqlConnection db_connection = new SqlConnection(connection_string);
            db_connection.Open();
            SqlCommand command = new SqlCommand(null, db_connection);
            sqlquery =
                " INSERT INTO FiltersDetails (" +
                " FilterID, FieldType, CompareType, FieldName, Comparator, CompareValue" +
                " ) VALUES ( @lastInsertedFilterID, @cmp_fieldType, @cmp_type, @_compareFieldName, @cmp_comparator,@_compareValues)";

            command.Parameters.Add("@lastInsertedFilterID", SqlDbType.Int, 4).Value = Convert.ToInt32(lastInsertedFilterID);
            command.Parameters.Add("@cmp_fieldType", SqlDbType.VarChar).Value = cmp_fieldType;
            command.Parameters.Add("@cmp_type", SqlDbType.VarChar).Value = cmp_type;
            command.Parameters.Add("@_compareFieldName", SqlDbType.VarChar).Value = _compareFieldName;
            command.Parameters.Add("@cmp_comparator", SqlDbType.VarChar).Value = cmp_comparator;
            command.Parameters.Add("@_compareValues", SqlDbType.VarChar).Value = _compareValues;
            command.CommandText = sqlquery;
            command.ExecuteScalar();

            sqlquery = "SELECT FDID, CompareType, FieldName, Comparator, CompareValue " +
                " FROM FiltersDetails" +
                " WHERE FilterID=" + lastInsertedFilterID;

            DataTable dt = DataFunctions.GetDataTable(sqlquery);
            AddFiltersGridArray.DataSource = dt.DefaultView;
            AddFiltersGridArray.DataBind();

            UpdateFilterWhereclause(lastInsertedFilterID);

            FilterPreviewPanel.Visible = true;
            FilterIDValue.Text = lastInsertedFilterID;
            FilterNameTxt.ReadOnly = true;
            FilterNameTxt.Style.Add("background-color", "#FCF8E9");
            loadFiltersGrid(Master.UserSession.CurrentUser.CustomerID.ToString(), getGroupID());
        }

        public void UpdateFilterWhereclause(string FilterID)
        {

            string sqlcreateFilter = " SELECT *  FROM FiltersDetails fd, Filters f WHERE fd.FilterID= " + FilterID + " and fd.FilterID = f.FilterID";
            DataTable dt = DataFunctions.GetDataTable(sqlcreateFilter);
            string whereClause = "";
            string dynamicWhere = "";
            string fieldName = "";
            string comparator = "";
            string compareValue = "";
            string compareType = "";
            string groupID = "";
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
                groupID = dr["GroupID"].ToString();
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
            }

            whereClause = whereClause.Replace("user_", "");
            string sqlupdateFilter = " UPDATE Filters SET " +
                " WhereClause ='" + DataFunctions.CleanString(whereClause) + "' , " +
                " DynamicWhere ='" + DataFunctions.CleanString(dynamicWhere) + "' " +
                " WHERE FilterID=" + FilterID;
            //throw new System.Exception(sqlupdateFilter);
            DataFunctions.Execute(sqlupdateFilter);
        }

        private ArrayList GetEmailStructure()
        {
            ArrayList arrEmails = new ArrayList();
            string sqlgetfilter = "select lower(c.name) as 'colname' from sysobjects o join syscolumns c on o.id = c.id where o.name = 'Emails'";
            DataTable dt = DataFunctions.GetDataTable(sqlgetfilter);
            foreach (DataRow dr in dt.Rows)
            {
                arrEmails.Add(dr["colname"].ToString());
            }
            return arrEmails;

        }

        public void EditFilter(String theFilterID)
        {
            string sqlgetfilter = " SELECT * FROM Filters WHERE FilterID=" + theFilterID;
            string theGroupID = "0";
            string theFilterName = "";
            DataTable dt = DataFunctions.GetDataTable(sqlgetfilter);
            foreach (DataRow dr in dt.Rows)
            {
                theGroupID = dr["GroupID"].ToString();
                theFilterName = dr["FilterName"].ToString();
                break;
            }
            FilterNameTxtLabel.Text = theFilterName;
            FilterIDValue.Text = theFilterID;
            FilterAttribsPanel.Visible = true;
            AddFilterButtonPanel.Visible = false;
            FiltersPanel.Visible = false;

            loadAddFiltersGrid(theFilterID);
        }

        public void DeleteFilter(String theFilterID)
        {
            string sqlgetgroup = " SELECT GroupID FROM Filters WHERE FilterID=" + theFilterID;
            string theGroupID = DataFunctions.ExecuteScalar(sqlgetgroup).ToString();
            string sqldelFilter =
                " DELETE FROM Filters " +
                " WHERE FilterID='" + theFilterID + "' ";
            DataFunctions.Execute(sqldelFilter);

            string sqldelFilterDetails =
                " DELETE FROM FiltersDetails " +
                " WHERE FilterID='" + theFilterID + "' ";
            DataFunctions.Execute(sqldelFilterDetails);

            Response.Redirect("filters.aspx?GroupID=" + theGroupID);
        }

        public void DeleteFilterDetail(String theFilterDetailID)
        {
            string filterID = "";
            string groupID = "";
            string sqlgetgroup = " SELECT f.FilterID, f.GroupID  FROM Filters f, FiltersDetails fd WHERE FDID=" + theFilterDetailID +
                                       " and fd.FilterID = f.FilterID";
            DataTable dt = DataFunctions.GetDataTable(sqlgetgroup);
            foreach (DataRow dr in dt.Rows)
            {
                filterID = dr["FilterID"].ToString();
                groupID = dr["GroupID"].ToString();
            }

            string sqldelFilterDetail =
                " DELETE FROM FiltersDetails " +
                " WHERE FDID='" + theFilterDetailID + "' ";
            DataFunctions.Execute(sqldelFilterDetail);
            UpdateFilterWhereclause(filterID);

            Response.Redirect("filters.aspx?FilterID=" + filterID + "&action=editFilter");
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
             */
    }
}