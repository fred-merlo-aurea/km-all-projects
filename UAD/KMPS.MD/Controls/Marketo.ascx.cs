using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;
using KM.Integration.OAuth2;
using KM.Integration.Marketo;
using KM.Integration.Marketo.Process;
using KMPS.MD.Objects;
using System.Web.UI.HtmlControls;

namespace KMPS.MD.Controls
{
    public partial class Marketo : BaseControl
    {
        DataTable ExtURLQS;
        
        public int GroupID
        {
            get
            {
                try
                {
                    return (int)ViewState["GroupID"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["GroupID"] = value;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblTestConnErrorMessage.Visible = false;
            lblTestConnErrorMessage.Text = string.Empty;
            lblTestErrMsg.Visible = false;
            lblTestErrMsg.Text = string.Empty;
            txtErrMsg.Visible = false;
            txtErrMsg.Text = "";

            //if (ddlQSValue.Items.Count == 0)
            //{
            //    List<int> PubIDs = new List<int>();
            //    PubIDs.Add(PubID);

            //    Dictionary<string, string> exportfields = Utilities.GetExportFields(ViewType, BrandID ?? 0, PubIDs, Enums.PageType.FilterSchedule);

            //    foreach (var item in exportfields)
            //    {
            //        ddlQSValue.Items.Add(new ListItem(item.Value, item.Key));
            //    }

            //    ddlQSValue.Items.Add(new ListItem("CustomValue", "CustomValue"));
            //}

            if (!IsPostBack)
            {
            }
            else
            {
                if (gvHttpPost.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    for (int i = 0; i < gvHttpPost.Rows.Count; i++)
                    {
                        if (gvHttpPost.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            Label lblHttpPostParamsID = (Label)gvHttpPost.Rows[i].FindControl("lblHttpPostParamsID");
                            Label lblParamName = (Label)gvHttpPost.Rows[i].FindControl("lblParamName");
                            Label lblParamValue = (Label)gvHttpPost.Rows[i].FindControl("lblParamValue");
                            Label lblCustomValue = (Label)gvHttpPost.Rows[i].FindControl("lblCustomValue");
                            Label lblParamDisplayName = (Label)gvHttpPost.Rows[i].FindControl("lblParamDisplayName");

                            if (dt.Columns.Count == 0)
                            {
                                DataColumn HttpPostParamsID = new DataColumn("HttpPostParamsID", typeof(string));
                                dt.Columns.Add(HttpPostParamsID);

                                DataColumn ParamName = new DataColumn("ParamName", typeof(string));
                                dt.Columns.Add(ParamName);

                                DataColumn ParamValue = new DataColumn("ParamValue", typeof(string));
                                dt.Columns.Add(ParamValue);

                                DataColumn CustomValue = new DataColumn("CustomValue", typeof(string));
                                dt.Columns.Add(CustomValue);

                                DataColumn ParamDisplayName = new DataColumn("ParamDisplayName", typeof(string));
                                dt.Columns.Add(ParamDisplayName);
                            }

                            DataRow dr = dt.NewRow();
                            dr["HttpPostParamsID"] = lblHttpPostParamsID.Text;
                            dr["ParamName"] = lblParamName.Text;
                            dr["ParamValue"] = lblParamValue.Text;
                            dr["CustomValue"] = lblCustomValue.Text;
                            dr["ParamDisplayName"] = lblParamDisplayName.Text;
                            dt.Rows.Add(dr);
                        }
                    }
                    ExtURLQS = dt;
                }
            }
        }

        public void loadMarketoExportFields()
        {
            ddlQSValue.Items.Clear();

            List<int> PubIDs = new List<int>();
            PubIDs.Add(PubID);

            Dictionary<string, string> exportfields = Utilities.GetExportFields(clientconnections, ViewType, BrandID, PubIDs, KMPS.MD.Objects.Enums.ExportType.Marketo, UserSession.CurrentUser.UserID);

            foreach (var item in exportfields)
            {
                ddlQSValue.Items.Add(new ListItem(item.Value, item.Key + "|" + (item.Key.Split('|')[1].ToUpper() == KMPS.MD.Objects.Enums.FieldType.Varchar.ToString().ToUpper() ? KMPS.MD.Objects.Enums.FieldCase.Default.ToString() : KMPS.MD.Objects.Enums.FieldCase.None.ToString())));
            }

            ddlQSValue.Items.Add(new ListItem("CustomValue", "CustomValue"));
        }

        protected void ddlQSValue_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlQSValue.SelectedValue.Equals("CustomValue"))
            {
                phCustomValue.Visible = true;
            }
            else
            {
                phCustomValue.Visible = false;
            }
        }

        protected void btnAddHttpPostURL_Click(object sender, EventArgs e)
        {
            DataTable dt = ExtURLQS;
            if (dt == null)
            {
                dt = new DataTable();
                DataColumn HttpPostParamsID = new DataColumn("HttpPostParamsID", typeof(string));
                dt.Columns.Add(HttpPostParamsID);

                DataColumn ParamName = new DataColumn("ParamName", typeof(string));
                dt.Columns.Add(ParamName);

                DataColumn ParamValue = new DataColumn("ParamValue", typeof(string));
                dt.Columns.Add(ParamValue);

                DataColumn CustomValue = new DataColumn("CustomValue", typeof(string));
                dt.Columns.Add(CustomValue);

                DataColumn ParamDisplayName = new DataColumn("ParamDisplayName", typeof(string));
                dt.Columns.Add(ParamDisplayName);
            }
            else
            {
                foreach (DataRow r in dt.Rows)
                {
                    if (string.Equals(r["ParamName"].ToString(), txtQSName.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        txtErrMsg.Visible = true;
                        txtErrMsg.Text = "Name entered has been already mapped. Please enter different Name.";
                        return;
                    }
                }
            }

            DataRow dr = dt.NewRow();
            dr["HttpPostParamsID"] = Guid.NewGuid();
            dr["ParamName"] = txtQSName.Text;
            dr["ParamValue"] = ddlQSValue.SelectedValue;
            dr["CustomValue"] = txtQSValue.Text;
            dr["ParamDisplayName"] = ddlQSValue.SelectedItem.Text;
            dt.Rows.Add(dr);
            txtQSName.Text = "";
            txtQSValue.Text = "";
            ExtURLQS = dt;
            gvHttpPost.DataSource = dt;
            gvHttpPost.DataBind();
        }

        protected void gvHttpPost_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string HttpPostParamsID = e.CommandArgument.ToString();
                if (e.CommandName == "ParamDelete")
                {
                    DataTable dt = ExtURLQS;
                    var result = (from src in dt.AsEnumerable()
                                  where src.Field<string>("HttpPostParamsID") == HttpPostParamsID
                                  select src).ToList();
                    dt.Rows.Remove(result[0]);
                    ExtURLQS = dt;
                    gvHttpPost.DataSource = ExtURLQS;
                    gvHttpPost.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnTestConnection_Click(Object sender, EventArgs e)
        {
            if (TestMarketoConnection(lblTestConnErrorMessage))
            {
                lblTestConnErrorMessage.Text = "Marketo Connection successful.";
            }
        }

        private bool TestMarketoConnection(Label lblErrorMsg)
        {
            bool ValidConnection = false;
            lblErrorMsg.Visible = true;
            if (txtMarketoBaseURL.Text == string.Empty || txtMarketoClientID.Text == string.Empty || txtMarketoClientSecret.Text == string.Empty)
                lblErrorMsg.Text = "Please enter Marketo information for testing connection.";
            else
            {
                try
                {
                    KM.Integration.OAuth2.Authentication auth = new KM.Integration.OAuth2.Authentication(txtMarketoBaseURL.Text, txtMarketoClientID.Text, txtMarketoClientSecret.Text);
                    Token token = auth.getToken();

                    if (token == null || token.expires_in == 0)
                        lblErrorMsg.Text = "Marketo Connection failed. Please verify the credentials.";
                    else
                    {

                        ValidConnection = true;
                    }

                }
                catch
                {
                    lblErrorMsg.Text = "Marketo Connection failed. Please verify the credentials.";
                }
            }

            return ValidConnection;

        }

        protected void btnSearchMarketoList_Click(object sender, EventArgs e)
        {
            try
            {
                if (TestMarketoConnection(lblTestErrMsg))
                {
                    MarketoRestAPIProcess ra = new MarketoRestAPIProcess(txtMarketoBaseURL.Text, txtMarketoClientID.Text, txtMarketoClientSecret.Text);

                    Authentication auth = new Authentication(txtMarketoBaseURL.Text, txtMarketoClientID.Text, txtMarketoClientSecret.Text);

                    var token = auth.getToken();

                    List<SearchResult> mlr = ra.GetMarketoLists(token, null, txtMarketoNames.Text.Split(','), 100, "")[0].result.OrderBy(x => x.name).ToList();

                    var query = from m in mlr
                                select new { id = m.id, name = m.name + " / " + m.programName };

                    ddlMarketoList.DataSource = query.ToList();
                    ddlMarketoList.DataBind();
                    ddlMarketoList.Items.Insert(0, new ListItem("", "0"));
                }
            }
            catch
            {
            }
        }
    }
}