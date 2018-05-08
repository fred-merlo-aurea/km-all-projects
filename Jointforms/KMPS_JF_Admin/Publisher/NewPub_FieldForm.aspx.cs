using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using KMPS_JF_Objects.Objects;

namespace KMPS_JF_Setup.Publisher
{
    public partial class NewPub_FieldForm : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();
        DataTable _dtFormField = new DataTable();
        private string strControlValue = string.Empty;
        List<FieldData> lstControlValues = new List<FieldData>();

        private int PubID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PubId"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int PSFieldID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PSFieldID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int PFID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PFID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int GroupID
        {
            get
            {
                return Convert.ToInt32(ViewState["GroupID"]);
            }
            set
            {
                ViewState["GroupID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["PSFieldID"] = "0";
                    ControlData = null;
                    OldControlData = null;
                    SqlCommand cmdGroupID = new SqlCommand("select ECNDefaultGroupID from publications where PubID = @PubID");
                    cmdGroupID.CommandType = CommandType.Text;
                    cmdGroupID.Parameters.Add(new SqlParameter("@PubID", PubID));
                    GroupID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmdGroupID).ToString());

                    LoadECNField();
                    LoadECNCombinedField();

                    if (PSFieldID > 0)
                    {
                        LoadFieldData();
                    }
                    else
                    {
                        ControlData = new List<FieldData>();
                    }

                    ddlControlType_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        public List<FieldData> ControlData
        {
            get
            {
                try
                {
                    return (List<FieldData>)Session["ControlData"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                Session["ControlData"] = value;
            }
        }

        public FieldData OldControlData
        {
            get
            {
                try
                {
                    return (FieldData)Session["ControlDataOld"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                Session["ControlDataOld"] = value;
            }
        }

        public string SortOrder
        {
            get
            {
                try
                {
                    return ViewState["SortOrder"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["SortOrder"] = value;
            }
        }

        private void BindControlGrid()
        {
            grdControlValue.DataSource = ControlData.OrderBy(x => x.SortOrder);
            grdControlValue.DataBind();
        }

        protected void LoadFieldData()
        {
            try
            {
                SqlCommand cmddtField = new SqlCommand("sp_GetPubSubscriptionFormsField");
                cmddtField.CommandType = CommandType.StoredProcedure;
                cmddtField.Parameters.AddWithValue("@PSFieldID", PSFieldID.ToString());
                DataTable dtField = DataFunctions.GetDataTable(cmddtField);

                if (dtField.Rows.Count > 0)
                {
                    List<FieldsSetting> fsList = FieldsSetting.GetFieldSettings(PSFieldID, PFID);
                    List<NonQualSettings> nqsList = NonQualSettings.GetNonQualSettings(PSFieldID, PFID);

                    RadEditorDisplayName.Content = dtField.Rows[0]["DisplayName"].ToString();

                    string[] FieldData = dtField.Rows[0]["ControlValue"].ToString().Split(new char[] { '\r', '\n' });
                    int order = 1;

                    foreach (string str in FieldData)
                    {
                        string[] dataTextvalues = str.Split('|');

                        if (dataTextvalues.Length == 2)
                        {
                            FieldData field = new FieldData();
                            field.DataText = dataTextvalues[1].ToString();
                            field.DataValue = dataTextvalues[0].ToString();
                            field.Category = string.Empty;
                            field.IsDefault = false;
                            field.SortOrder = order;
                            field.IsBranching = fsList.Find(x => x.DataValue.ToUpper() == field.DataValue.ToUpper()) != null ? true : false;
                            field.IsNonQual = nqsList.Find(x => x.DataValue.ToUpper() == field.DataValue.ToUpper()) != null ? true : false;

                            lstControlValues.Add(field);
                        }
                        else if (dataTextvalues.Length == 3)
                        {
                            FieldData field = new FieldData();
                            field.DataText = dataTextvalues[1].ToString();
                            field.DataValue = dataTextvalues[0].ToString();
                            field.Category = dataTextvalues[2].ToString() == "null" ? string.Empty : dataTextvalues[2].ToString();
                            field.IsDefault = false;
                            field.SortOrder = order;
                            field.IsBranching = fsList.Find(x => x.DataValue.ToUpper() == field.DataValue.ToUpper()) != null ? true : false;
                            field.IsNonQual = nqsList.Find(x => x.DataValue.ToUpper() == field.DataValue.ToUpper()) != null ? true : false;

                            lstControlValues.Add(field);
                        }
                        else if (dataTextvalues.Length == 4)
                        {
                            FieldData field = new FieldData();
                            field.DataText = dataTextvalues[1].ToString();
                            field.DataValue = dataTextvalues[0].ToString();
                            field.Category = dataTextvalues[2].ToString() == "null" ? string.Empty : dataTextvalues[2].ToString();
                            field.IsDefault = Convert.ToBoolean(dataTextvalues[3]);
                            field.SortOrder = order;
                            field.IsBranching = fsList.Find(x => x.DataValue.ToUpper() == field.DataValue.ToUpper()) != null ? true : false;
                            field.IsNonQual = nqsList.Find(x => x.DataValue.ToUpper() == field.DataValue.ToUpper()) != null ? true : false;
                            lstControlValues.Add(field);
                        }
                        else
                        {
                            lstControlValues.Clear();
                        }

                        order++;
                    }

                    ControlData = null;
                    OldControlData = null;
                    SaveToSession(lstControlValues);
                    BindControlGrid();
                    txtMaxSelections.Text = dtField.Rows[0]["MaxSelections"].ToString();
                    txtMaxLength.Text = dtField.Rows[0]["Maxlength"].ToString();
                    txtQueryString.Text = dtField.Rows[0]["QueryStringName"].ToString();

                    try
                    {
                        ddlECNField.Enabled = false;
                        ddlECNField.ClearSelection();
                        ddlECNField.Items.FindByText(dtField.Rows[0]["ECNFieldName"].ToString().ToUpper()).Selected = true;
                    }
                    catch
                    { }

                    try
                    {
                        ddlECNCombinedField.ClearSelection();
                        ddlECNCombinedField.Items.FindByText(dtField.Rows[0]["ECNCombinedFieldName"].ToString().ToUpper()).Selected = true;
                    }
                    catch
                    {
                        ddlECNCombinedField.Items[0].Selected = true;
                    }

                    try
                    {
                        rbtlstIsActive.ClearSelection();
                        rbtlstIsActive.SelectedValue = (Convert.ToBoolean(dtField.Rows[0]["IsActive"]) == true ? "1" : "0");
                    }
                    catch
                    { }

                    try
                    {
                        ddlGrouping.ClearSelection();
                        ddlGrouping.Items.FindByValue(dtField.Rows[0]["Grouping"].ToString()).Selected = true;
                    }
                    catch { }

                    try
                    {
                        ddlDataType.ClearSelection();
                        ddlDataType.Items.FindByText(dtField.Rows[0]["DataType"].ToString()).Selected = true;
                    }
                    catch { }

                    try
                    {
                        rblstprepopulate.ClearSelection();
                        rblstprepopulate.Items.FindByValue(Convert.ToBoolean(dtField.Rows[0]["Prepopulate"]) || dtField.Rows[0].IsNull("Prepopulate") ? "1" : "0").Selected = true;
                    }
                    catch
                    {
                        rblstprepopulate.Items.FindByValue("1").Selected = true;
                    }

                    try
                    {
                        ddlControlType.ClearSelection();
                        ddlControlType.Items.FindByValue(dtField.Rows[0]["ControlType"].ToString()).Selected = true;
                    }
                    catch { }

                    try
                    {
                        rbtlstRequired.ClearSelection();
                        rbtlstRequired.Items.FindByValue(dtField.Rows[0]["Required"].ToString()).Selected = true;
                    }
                    catch { }

                    try
                    {
                        rbtShowTextBox.ClearSelection();
                        rbtShowTextBox.Items.FindByValue(Convert.ToBoolean(dtField.Rows[0]["ShowTextField"]) ? "1" : "0").Selected = true;

                        pnlIsShowtextBox.Visible = (rbtShowTextBox.SelectedValue == "1" ? true : false);

                        ddlECNTextFieldName.ClearSelection();
                        ddlECNTextFieldName.Items.FindByText(dtField.Rows[0]["ECNTextFieldName"].ToString()).Selected = true;
                    }
                    catch { }

                    try
                    {
                        rbtNoneOfTheAbove.ClearSelection();
                        rbtNoneOfTheAbove.Items.FindByValue(Convert.ToBoolean(dtField.Rows[0]["ShowNoneoftheAbove"]) ? "true" : "false").Selected = true;
                    }
                    catch { }

                    try
                    {
                        rblstColumnFormat.ClearSelection();
                        rblstColumnFormat.Items.FindByValue(String.IsNullOrEmpty(dtField.Rows[0]["ColumnFormat"].ToString()) ? "1" : dtField.Rows[0]["ColumnFormat"].ToString()).Selected = true;
                    }
                    catch { }

                    try
                    {
                        rblstRepeatDirection.ClearSelection();
                        rblstRepeatDirection.Items.FindByValue(String.IsNullOrEmpty(dtField.Rows[0]["RepeatDirection"].ToString()) ? "VER" : dtField.Rows[0]["RepeatDirection"].ToString().ToUpper()).Selected = true;
                    }
                    catch { }

                    try
                    {
                        txtDefaultValue.Text = dtField.Rows[0].IsNull("DefaultValue") ? "" : dtField.Rows[0]["DefaultValue"].ToString();
                    }
                    catch
                    {
                        txtDefaultValue.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadECNCombinedField()
        {
            try
            {
                string strExistingFields = string.Empty;
                string strQuery = string.Empty;

                SqlCommand cmdExistingFields = new SqlCommand("select ECNFieldName from PubSubscriptionFields where PubId = @PubID");
                cmdExistingFields.CommandType = CommandType.Text;
                cmdExistingFields.Parameters.Add(new SqlParameter("@PubID", SqlDbType.Int)).Value = PubID;
                DataTable dtExistingFields = DataFunctions.GetDataTable(cmdExistingFields);

                foreach (DataRow drEF in dtExistingFields.Rows)
                    strExistingFields += strExistingFields == string.Empty ? "'" + drEF["ECNFieldName"].ToString().Trim() + "'" : ",'" + drEF["ECNFieldName"].ToString().Trim() + "'";

                strQuery = " Select upper(shortname) as shortname from (" +
                           " Select ('FirstName') as ShortName" +
                           " UNION Select ('LastName') as ShortName" +
                           " UNION Select ('Title') as ShortName" +
                           " UNION Select ('Company') as ShortName" +
                           " UNION Select ('Occupation') as ShortName" +
                           " UNION Select ('Address') as ShortName" +
                           " UNION Select ('Address2') as ShortName" +
                           " UNION Select ('City') as ShortName" +
                           " UNION Select ('State') as ShortName" +
                           " UNION Select ('Zip') as ShortName" +
                           " UNION Select ('ZipPlus') as ShortName" +
                           " UNION Select ('Voice') as ShortName" +
                           " UNION Select ('Mobile') as ShortName" +
                           " UNION Select ('Fax') as ShortName" +
                           " UNION Select ('Website') as ShortName" +
                           " UNION Select ('Age') as ShortName" +
                           " UNION Select ('Income') as ShortName" +
                           " UNION Select ('Gender') as ShortName" +
                           " UNION Select ('Birthdate') as ShortName" +
                           " UNION select ShortName from groupdatafields where groupID = " + GroupID.ToString() + " and isnull(datafieldSetID,0) = 0)AS ECNDATAFIELD ";


                if (PSFieldID == 0 && strExistingFields.Length > 0)
                    strQuery = strQuery + " where ECNDATAFIELD.ShortName not in (" + strExistingFields + ")";

                DataTable dtFormECNField = DataFunctions.GetDataTable(strQuery, ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString);

                if (dtFormECNField.Rows.Count > 0)
                {
                    ddlECNCombinedField.DataTextField = "ShortName";
                    ddlECNCombinedField.DataValueField = "ShortName";
                    ddlECNCombinedField.DataSource = dtFormECNField;
                    ddlECNCombinedField.DataBind();
                    ddlECNCombinedField.Items.Insert(0, new ListItem("-----", ""));
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadECNField()
        {
            try
            {
                string strExistingFields = string.Empty;
                string strQuery = string.Empty;

                SqlCommand cmdExistingFields = new SqlCommand("select ECNFieldName from PubSubscriptionFields where PubId = @PubID");
                cmdExistingFields.CommandType = CommandType.Text;
                cmdExistingFields.Parameters.Add(new SqlParameter("@PubID", SqlDbType.Int)).Value = PubID;
                DataTable dtExistingFields = DataFunctions.GetDataTable(cmdExistingFields);

                foreach (DataRow drEF in dtExistingFields.Rows)
                    strExistingFields += strExistingFields == string.Empty ? "'" + drEF["ECNFieldName"].ToString().Trim() + "'" : ",'" + drEF["ECNFieldName"].ToString().Trim() + "'";

                strQuery = " Select upper(shortname) as shortname from (" +
                           " Select ('FirstName') as ShortName" +
                           " UNION Select ('LastName') as ShortName" +
                           " UNION Select ('Title') as ShortName" +
                           " UNION Select ('Company') as ShortName" +
                           " UNION Select ('Occupation') as ShortName" +
                           " UNION Select ('Address') as ShortName" +
                           " UNION Select ('Address2') as ShortName" +
                           " UNION Select ('City') as ShortName" +
                           " UNION Select ('State') as ShortName" +
                           " UNION Select ('Zip') as ShortName" +
                           " UNION Select ('ZipPlus') as ShortName" +
                           " UNION Select ('Voice') as ShortName" +
                           " UNION Select ('Mobile') as ShortName" +
                           " UNION Select ('Fax') as ShortName" +
                           " UNION Select ('Website') as ShortName" +
                           " UNION Select ('Age') as ShortName" +
                           " UNION Select ('Income') as ShortName" +
                           " UNION Select ('Gender') as ShortName" +
                           " UNION Select ('Birthdate') as ShortName" +
                           " UNION select ShortName from groupdatafields where groupID = " + GroupID.ToString() + " and isnull(datafieldSetID,0) = 0)AS ECNDATAFIELD ";

                DataTable dtFormECNTextField = DataFunctions.GetDataTable(strQuery, ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString);

                if (dtFormECNTextField.Rows.Count > 0)
                {
                    ddlECNTextFieldName.DataTextField = "ShortName";
                    ddlECNTextFieldName.DataValueField = "ShortName";
                    ddlECNTextFieldName.DataSource = dtFormECNTextField;
                    ddlECNTextFieldName.DataBind();
                }

                if (PSFieldID == 0 && strExistingFields.Length > 0)
                    strQuery = strQuery + " where ECNDATAFIELD.ShortName not in (" + strExistingFields + ")";

                DataTable dtFormECNField = DataFunctions.GetDataTable(strQuery, ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString);

                if (dtFormECNField.Rows.Count > 0)
                {
                    ddlECNField.DataTextField = "ShortName";
                    ddlECNField.DataValueField = "ShortName";
                    ddlECNField.DataSource = dtFormECNField;
                    ddlECNField.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void grdControlValue_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = -1;
            try
            {
                rowIndex = Convert.ToInt32(e.CommandArgument);
            }
            catch { rowIndex = -1; }

            switch (e.CommandName)
            {
                case "Edit":
                    grdControlValue.EditIndex = rowIndex;
                    break;

                case "Update":
                    grdControlValue.EditIndex = -1;
                    break;

                case "Cancel":
                    grdControlValue.EditIndex = -1;
                    break;

                case "Delete":
                    grdControlValue.EditIndex = -1;
                    break;

                default:
                    grdControlValue.EditIndex = -1;
                    break;
            }
        }

        protected void grdControlValue_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdControlValue.EditIndex = e.NewEditIndex;
            BindControlGrid();

            TextBox dataText = (TextBox)grdControlValue.Rows[e.NewEditIndex].FindControl("txtControlFieldNameEdit");
            TextBox dataValue = (TextBox)grdControlValue.Rows[e.NewEditIndex].FindControl("txtControlFieldValueEdit");
            CheckBox rbDefault = (CheckBox)grdControlValue.Rows[e.NewEditIndex].FindControl("rbDefaultEdit");
            TextBox Category = (TextBox)grdControlValue.Rows[e.NewEditIndex].FindControl("txtCategoryName");
            Label lblOrder = (Label)grdControlValue.Rows[e.NewEditIndex].FindControl("lblSortOrder");

            FieldData field = new FieldData();
            field.DataText = dataText.Text;
            field.DataValue = dataValue.Text;
            field.Category = Category.Text;
            field.IsDefault = rbDefault.Checked;
            field.SortOrder = Convert.ToInt16(lblOrder.Text);

            this.OldControlData = field;
        }

        protected void grdControlValue_RowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
        {
            grdControlValue.EditIndex = -1;
            BindControlGrid();
        }

        protected void grdControlValue_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            List<FieldData> lstControlVals = (List<FieldData>)Session["ControlData"];
            TextBox dataText = (TextBox)grdControlValue.Rows[e.RowIndex].FindControl("txtControlFieldNameEdit");
            TextBox dataValue = (TextBox)grdControlValue.Rows[e.RowIndex].FindControl("txtControlFieldValueEdit");
            CheckBox rbDefault = (CheckBox)grdControlValue.Rows[e.RowIndex].FindControl("rbDefaultEdit");
            TextBox Category = (TextBox)grdControlValue.Rows[e.RowIndex].FindControl("txtCategoryName");
            Label lblOrder = (Label)grdControlValue.Rows[e.RowIndex].FindControl("lblSortOrder");

            //if (IsDataValueExists(dataValue.Text))
            //{
            //    lblDataValueErr.Visible = true;
            //    lblDataValueErr.Text = "DataValue " + dataValue.Text.Trim() + " already exists. Please enter different value!";
            //    grdControlValue.EditIndex = -1;
            //    BindControlGrid();
            //    return;
            //}

            lblDataValueErr.Visible = false;

            FieldData cv = OldControlData;
            FieldData field = lstControlVals[e.RowIndex];

            if (field != null)
            {
                field.DataText = dataText.Text;
                field.DataValue = dataValue.Text;
                field.Category = Category.Text;

                if (rbDefault.Checked && (ddlControlType.SelectedValue.ToUpper() == "TEXTBOX" || ddlControlType.SelectedValue.ToUpper() == "DROPDOWN" || ddlControlType.SelectedValue.ToUpper() == "RADIO" || ddlControlType.SelectedValue.ToUpper() == "HIDDEN"))
                {
                    List<FieldData> lstfData = lstControlVals.FindAll(x => x.IsDefault == true);

                    foreach (FieldData fd in lstfData)
                        fd.IsDefault = false;
                }

                field.IsDefault = rbDefault.Checked;
                field.SortOrder = Convert.ToInt16(lblOrder.Text);

                SaveToSession(lstControlVals);
                BindControlGrid();
            }
        }

        protected void grdControlValue_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int lenFields = ControlData.Count;
            DropDownList drpOrder = (DropDownList)e.Row.FindControl("drpItemOrder");

            if (drpOrder != null)
            {
                for (int i = 1; i <= lenFields; i++)
                    drpOrder.Items.Add(i.ToString());

                try
                {
                    Label lblDataValue = (Label)e.Row.FindControl("lblControlFieldValue");
                    drpOrder.ClearSelection();
                    drpOrder.Items.FindByValue((e.Row.RowIndex + 1).ToString()).Selected = true;
                }
                catch { }
            }
        }

        protected void drpItemOrder_selectedindexchanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdControlValue.Rows)
            {
                Label lblSortOrder = (Label)row.FindControl("lblSortOrder");
                DropDownList drpOrder = (DropDownList)row.FindControl("drpItemOrder");
                int OldSortOrder = Convert.ToInt16(lblSortOrder.Text);
                int newSortOrder = Convert.ToInt16(drpOrder.SelectedItem.Value);

                if (OldSortOrder != newSortOrder)
                {
                    FieldData fdOld = ControlData.Find(x => x.SortOrder == OldSortOrder);
                    fdOld.SortOrder = newSortOrder;

                    if (newSortOrder < row.RowIndex)
                    {
                        for (int i = newSortOrder - 1; i < row.RowIndex; i++)
                            ControlData[i].SortOrder++;
                    }
                    else
                    {
                        for (int i = row.RowIndex + 1; i < newSortOrder; i++)
                            ControlData[i].SortOrder--;
                    }

                    SaveToSession(ControlData);
                    BindControlGrid();
                }
            }
        }

        private Boolean GetBoolValuesFromString(string val)
        {
            bool retVal = false;

            switch (val.ToUpper())
            {
                case "NO":
                    retVal = false;
                    break;
                case "YES":
                    retVal = true;
                    break;
                case "TRUE":
                    retVal = true;
                    break;
                case "FALSE":
                    retVal = false;
                    break;
                default:
                    retVal = false;
                    break;
            }

            return retVal;
        }

        protected void grdControlValue_RowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
            List<FieldData> lstControlVals = (List<FieldData>)Session["ControlData"];

            Label dataText = (Label)grdControlValue.Rows[e.RowIndex].FindControl("lblControlFieldName");
            Label dataValue = (Label)grdControlValue.Rows[e.RowIndex].FindControl("lblControlFieldValue");
            Label lbDefault = (Label)grdControlValue.Rows[e.RowIndex].FindControl("lblDefault");
            Label lblSortOrder = (Label)grdControlValue.Rows[e.RowIndex].FindControl("lblSortOrder");

            int deletedRows = lstControlVals.RemoveAll(x => (x.DataText == dataText.Text && x.DataValue == dataValue.Text && x.IsDefault == GetBoolValuesFromString(lbDefault.Text) && x.SortOrder == Convert.ToInt32(lblSortOrder.Text)));

            if (deletedRows > 0)
            {
                for (int i = e.RowIndex; i < lstControlVals.Count; i++)
                    lstControlVals[i].SortOrder = lstControlVals[i].SortOrder - 1;
            }

            SaveToSession(lstControlVals);
            BindControlGrid();
        }

        private void SaveToSession(List<FieldData> listvals)
        {
            ControlData = listvals.OrderBy(x => x.SortOrder).ToList();
        }

        private bool IsDataValueExists(string dataValue)
        {
            return ControlData.Exists(x => x.DataValue.ToUpper() == dataValue.ToUpper());
        }

        protected void btnNew_Click(object sender, EventArgs args)
        {
            if (txtControlFieldValue.Text.Trim().Length > 0 && txtControlFieldName.Text.Trim().Length > 0)
            {
                //if (!IsDataValueExists(txtControlFieldValue.Text.Trim()))
                //{
                List<FieldData> lstcontrolvals = ControlData;
                FieldData field = new FieldData();
                field.DataText = txtControlFieldName.Text;
                field.DataValue = txtControlFieldValue.Text;
                field.Category = txtCategoryName.Text;
                field.IsDefault = rbDefaultNew.Checked;
                field.SortOrder = ControlData.Count + 1;

                if (rbDefaultNew.Checked)
                {
                    foreach (FieldData fddata in lstcontrolvals)
                        fddata.IsDefault = false;
                }

                lstcontrolvals.Add(field);
                SaveToSession(lstcontrolvals);
                BindControlGrid();
                lblDataValueErr.Visible = false;
                txtControlFieldValue.Text = "";
                txtControlFieldName.Text = "";
                txtCategoryName.Text = "";
                rbDefaultNew.Checked = false;
                //}
                //else
                //{
                //    lblDataValueErr.Visible = true;
                //    lblDataValueErr.Text = "DataValue " + txtControlFieldValue.Text.Trim() + " already exists. Please enter different value!";
                //}
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";

            try
            {
                string strControlValue = String.Empty;
                List<FieldData> lstControlVal = (List<FieldData>)ControlData;

                foreach (FieldData val in lstControlVal.OrderBy(x => x.SortOrder))
                {
                    strControlValue += val.DataValue + "|" + val.DataText + "|" + (val.Category.Trim().Length > 0 ? val.Category : "null") + "|" + val.IsDefault.ToString() + "\r\n";
                }

                sqlPubFormFields.InsertParameters["ControlValue"].DefaultValue = strControlValue.ToString().Replace("\r\n", "@@");
                sqlPubFormFields.InsertParameters["AddedBy"].DefaultValue = jfsess.UserName();
                sqlPubFormFields.InsertParameters["ModifiedBy"].DefaultValue = jfsess.UserName();
                if (pnlMaxSelections.Visible)
                    sqlPubFormFields.InsertParameters["MaxSelections"].DefaultValue = txtMaxSelections.Text;
                else
                    sqlPubFormFields.InsertParameters["MaxSelections"].DefaultValue = "-1";
                sqlPubFormFields.InsertParameters["ECNTextFieldName"].DefaultValue = (rbtShowTextBox.SelectedValue == "1" ? ddlECNTextFieldName.SelectedItem.Text : "");

                if ((!CheckECNFieldTextExists() && pnlIsShowtextBox.Visible) || (!pnlIsShowtextBox.Visible))
                {
                    sqlPubFormFields.InsertParameters["ECNTextFieldName"].DefaultValue = (rbtShowTextBox.SelectedValue == "1" ? ddlECNTextFieldName.SelectedItem.Text : "");
                    sqlPubFormFields.Insert();
                    ControlData = null;
                    OldControlData = null;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "self.parent.reloadpage();", true);
                }
                else
                {
                    lblErrorMsg.Text = "The Selected ECN text field is already assigned. Please select different one!! ";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblErrorMsg.Text = ex.Message;
            }
        }

        private bool CheckECNFieldTextExists()
        {
            SqlCommand cmdECNTextField = new SqlCommand("sp_CheckECNFieldTextExists");
            cmdECNTextField.CommandType = CommandType.StoredProcedure;
            cmdECNTextField.Parameters.Add(new SqlParameter("@PubID", SqlDbType.Int)).Value = PubID;
            cmdECNTextField.Parameters.Add(new SqlParameter("@PSFieldID", SqlDbType.Int)).Value = PSFieldID;
            cmdECNTextField.Parameters.Add(new SqlParameter("@EcnFieldText", ddlECNTextFieldName.Text));

            if (Convert.ToInt32(DataFunctions.ExecuteScalar(cmdECNTextField)) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void sqlPubFormFields_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            Session["PSFieldID"] = Convert.ToInt32(e.Command.Parameters["@ID"].Value);
        }

        protected void ddlControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TextBox,Checkbox,Dropdown,Radio,Hidden
            pnlText.Visible = false;
            pnlOther.Visible = false;
            pnlQS.Visible = false;
            pnlOT.Visible = true;
            pnlNon.Visible = false;

            switch (ddlControlType.SelectedItem.Value.ToUpper())
            {
                case "TEXTBOX":
                    pnlText.Visible = true;
                    pnlOT.Visible = false;
                    pnlQS.Visible = true;
                    pnlMultiColumnFormat.Visible = false;
                    pnlMaxSelections.Visible = false;
                    break;
                case "CHECKBOX":
                    pnlOther.Visible = true;
                    pnlNon.Visible = false;
                    pnlMultiColumnFormat.Visible = true;
                    pnlMaxSelections.Visible = true;
                    break;
                case "CATCHECKBOX":
                    pnlOther.Visible = true;
                    pnlNon.Visible = false;
                    pnlMultiColumnFormat.Visible = true;
                    pnlOT.Visible = false;
                    pnlMaxSelections.Visible = false;
                    break;
                case "RADIO":
                    pnlOther.Visible = true;
                    pnlMultiColumnFormat.Visible = true;
                    pnlMaxSelections.Visible = false;
                    break;
                case "CATRADIO":
                    pnlOther.Visible = true;
                    pnlMultiColumnFormat.Visible = false;
                    pnlMaxSelections.Visible = false;
                    break;
                case "DROPDOWN":
                    pnlOther.Visible = true;
                    pnlQS.Visible = true;
                    pnlMultiColumnFormat.Visible = false;
                    pnlMaxSelections.Visible = false;
                    break;
                case "HIDDEN":
                    pnlQS.Visible = true;
                    pnlMultiColumnFormat.Visible = false;
                    pnlDefaultValue.Visible = true;
                    pnlMaxSelections.Visible = false;
                    break;
            }

            if (ddlECNField.SelectedItem.Value.ToUpper() == "VERIFY")
            {
                pnlPrePopulate.Visible = false;
            }
        }

        protected void rbtShowTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtShowTextBox.SelectedValue == "1")
                {
                    pnlIsShowtextBox.Visible = true;
                }
                else
                {
                    pnlIsShowtextBox.Visible = false;
                    lblErrorMsg.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
    }
}

