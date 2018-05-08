using System;
using System.Collections;
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
using System.IO;
using System.Text;
using KMPS_JF_Objects.Objects;

namespace KMPS_JF_Setup.Publisher
{
    public partial class PublisherAdd : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();

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

        public int GroupID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["GroupID"].ToString());
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

        public int ActiveTab
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["ActiveTab"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ActiveTab"] = value;
            }
        }

        public int SFID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["SFID"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["SFID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            try
            {
                if (!IsPostBack)
                {
                    ActiveTab = 0;

                    //Add Temporarly to fix the javascript issue in color picker.. happens only on the first colorpicker control
                    pnlTemp.Style.Add("display", "none");
                    LoadCustomer();
                    if (PubID > 0)
                        LoadPublisher();

                    LoadTab();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadCustomer()
        {
            SqlCommand cmdCustomer = new SqlCommand("Select CustomerID, CustomerName from Customer where customerID in (" + jfsess.AllowedCustoemerIDs() + ")");
            cmdCustomer.CommandType = CommandType.Text;
            DataTable dtCustomer = DataFunctions.GetDataTable("accounts", cmdCustomer);
            ddlCustomer.DataSource = dtCustomer;
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", ""));
        }

        #region Navigation

        protected void MnuPunlisher_MenuItemClick(Object Sender, MenuEventArgs e)
        {
            ActiveTab = Convert.ToInt32(e.Item.Value);
            LoadTab();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ActiveTab = ActiveTab + 1;
            LoadTab();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("PublisherList.aspx");
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            ActiveTab = ActiveTab - 1;
            LoadTab();
        }
        private void LoadTab()
        {
            MultiViewPublisher.ActiveViewIndex = ActiveTab;
            MnuPunlisher.Items[ActiveTab].Selected = true;

            switch (ActiveTab)
            {
                case 0:
                    btnPrevious.Visible = false;
                    btnNext.Visible = true;
                    break;
                case 9:
                    btnPrevious.Visible = true;
                    btnNext.Visible = false;
                    LoadQSValues();
                    break;
                default:
                    btnPrevious.Visible = true;
                    btnNext.Visible = true;
                    if (ActiveTab == 7)
                        ReloadColors();
                    break;
            }
        }

        private DataTable ExtURLQS
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["ExtURLQS"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["ExtURLQS"] = value;
            }
        }

        private void loadHttpPostData(int pubID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " select  CONVERT(varchar(255), hpp.HttpPostParamsID) as HttpPostParamsID , hpp.ParamName,hpp.ParamValue, hpp.CustomValue, hp.URL from HttpPostParams hpp " +
                               " join HttpPost hp " +
                               " on hpp.HttpPostID=hp.HttpPostID " +
                               " join Publications pb " +
                               " on pb.pubID=hp.EntityID " +
                               " where pb.pubID=@pubID and hp.IsNewsLetter=0";
            cmd.Parameters.AddWithValue("@pubID", pubID);
            DataTable dt = DataFunctions.GetDataTable(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                ExtURLQS = dt;
                txtPostURL.Text = dt.Rows[0]["URL"].ToString();
                gvHttpPost.DataSource = dt;
                gvHttpPost.DataBind();
            }
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
            }
            DataRow dr = dt.NewRow();
            dr["HttpPostParamsID"] = Guid.NewGuid();
            dr["ParamName"] = txtQSName.Text;
            dr["ParamValue"] = drpQSValue.SelectedValue;
            dr["CustomValue"] = txtQSValue.Text;
            dt.Rows.Add(dr);
            txtQSName.Text = "";
            txtQSValue.Text = "";
            ExtURLQS = dt;
            gvHttpPost.DataSource = dt;
            gvHttpPost.DataBind();
        }


        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            try
            {
                if (ValidateMultiView(MultiViewPublisher))
                {
                    StringBuilder sbxmldata = new StringBuilder();
                    if (ExtURLQS != null)
                    {
                        foreach (DataRow dr in ExtURLQS.AsEnumerable())
                        {
                            if (!dr["ParamValue"].ToString().Equals("-Select-"))
                                sbxmldata.Append(string.Format("<HttpPostID ParamName=\"{0}\" ParamValue=\"{1}\" CustomValue=\"{2}\"/>", dr["ParamName"].ToString(), dr["ParamValue"].ToString(), dr["CustomValue"].ToString()));
                        }
                    }
                    if (!sbxmldata.ToString().Equals(string.Empty) && txtPostURL.Text.Equals(string.Empty))
                        throw new Exception("External Post URL cannot be empty if the Query string values are specified");

                    if (sbxmldata.ToString().Equals(string.Empty) && !txtPostURL.Text.Equals(string.Empty))
                        throw new Exception("Query string values cannot be empty if the External Post URL is specified");


                    string strPubFormCSS = "#" + txtPBGColor.Text.Trim() + "|" + "#" + txtFBGColor.Text.Trim() + "|" + ddlPageBorder.SelectedValue + "|" + ddlPageFont.SelectedValue + "|" + ddlPageFontSize.SelectedValue +
                                           "|" + "#" + txtCBGColor.Text.Trim() + "|" + ddlCatFontSize.SelectedValue + "|" + "#" + txtCFColor.Text.Trim() +
                                           "|" + ddlQFSize.SelectedValue + "|" + "#" + txtQFColor.Text.Trim() + "|" + ddlQFBold.SelectedValue +
                                           "|" + ddlAFSize.SelectedValue + "|" + "#" + txtAFColor.Text.Trim() + "|" + ddlAFBold.SelectedValue;

                    string logoPath = UploadPubLogo();
                    string MailingLabelPath = UploadMailingLabel();
                    logoPath = (logoPath.ToString() == "" ? (hfldPubLogo.Value == "" ? System.DBNull.Value.ToString() : hfldPubLogo.Value) : logoPath.ToString());
                    sqldatasourceFinish.SelectParameters["CSS"].DefaultValue = strPubFormCSS.ToString();
                    sqldatasourceFinish.SelectParameters["PubLogo"].DefaultValue = logoPath.ToString();

                    MailingLabelPath = (MailingLabelPath.ToString() == "" ? (hfldMailingLabel.Value == "" ? System.DBNull.Value.ToString() : hfldMailingLabel.Value) : MailingLabelPath.ToString());

                    sqldatasourceFinish.SelectParameters["MailingLabel"].DefaultValue = MailingLabelPath.ToString();
                    sqldatasourceFinish.SelectParameters["AddedBy"].DefaultValue = jfsess.UserName();
                    sqldatasourceFinish.SelectParameters["ModifiedBy"].DefaultValue = jfsess.UserName();
                    sqldatasourceFinish.SelectParameters["iMode"].DefaultValue = "1";
                    sqldatasourceFinish.SelectParameters["MagCoverImage"].DefaultValue = fileUploadCoverImage.FileName;

                    if (PubID == 0)
                    {
                        if (GroupID == 0)
                        {
                            GroupID = ECNUpdate.SaveGroup(0, TxtName.Text, TxtName.Text, Convert.ToInt32(ddlCustomer.SelectedItem.Value));
                            sqldatasourceFinish.SelectParameters["ECNDefaultGroupID"].DefaultValue = GroupID.ToString();
                        }

                        //create smartform for group
                        if (SFID == 0)
                        {
                            SFID = ECNUpdate.SaveSmartForm(GroupID, TxtName.Text);
                            sqldatasourceFinish.SelectParameters["ECNSFID"].DefaultValue = SFID.ToString();
                        }
                    }
                    sqldatasourceFinish.SelectParameters["URL"].DefaultValue = txtPostURL.Text == string.Empty ? "0" : txtPostURL.Text;
                    sqldatasourceFinish.SelectParameters["qsNameValue"].DefaultValue = sbxmldata.ToString() == string.Empty ? "<HttpPostID></HttpPostID>" : sbxmldata.ToString();
                    sqldatasourceFinish.Select(DataSourceSelectArguments.Empty);
                    Response.Redirect("PublisherList.aspx?iFlag=" + (PubID == 0 ? "1" : "2"), true);
                    //} 
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }

        #endregion

        private string UploadPubLogo()
        {
            string UploadImageName = "", PubLogo = "";

            if (oFilePubLogo.FileName != String.Empty)
            {
                UploadImageName = oFilePubLogo.FileName;
                PubLogo = Server.MapPath(String.Concat(ConfigurationManager.AppSettings["LogoPath"].ToString(), UploadImageName));
                oFilePubLogo.PostedFile.SaveAs(PubLogo);
            }

            return UploadImageName;
        }

        private string UploadMailingLabel()
        {
            string UploadImageName = "";
            string MailingLabel = "";

            if (FileUploadMailingLabel.FileName != String.Empty)
            {
                UploadImageName = TxtCode.Text + "_" + "MailingLabel" + Path.GetExtension(FileUploadMailingLabel.FileName);
                string directory = ConfigurationManager.AppSettings["MailingLabelPath"].ToString();
                MailingLabel = String.Concat(directory, UploadImageName);
                string[] files = Directory.GetFiles(directory, (TxtCode.Text + "_" + "MailingLabel" + ".*"), SearchOption.TopDirectoryOnly);

                foreach (string fileName in files)
                    File.Delete(fileName);

                FileUploadMailingLabel.PostedFile.SaveAs(MailingLabel);
            }

            return UploadImageName;
        }

        private void ReloadColors()
        {
            ColorPickerExtender1.SelectedColor = txtPBGColor.Text;
            ColorPickerExtender2.SelectedColor = txtFBGColor.Text;
            ColorPickerExtender3.SelectedColor = txtCBGColor.Text;
            ColorPickerExtender4.SelectedColor = txtCFColor.Text;
            ColorPickerExtender5.SelectedColor = txtQFColor.Text;
            ColorPickerExtender6.SelectedColor = txtAFColor.Text;
        }

        private bool ValidateMultiView(MultiView mv)
        {
            int savedIndex = mv.ActiveViewIndex;
            for (int i = 0; i < mv.Views.Count; i++)
            {
                mv.ActiveViewIndex = i;
                if (i == 1)
                {
                    ReloadColors();
                }

                Page.Validate();

                if (!Page.IsValid)
                {
                    mv.ActiveViewIndex = i;

                    if (i == 1)
                    {
                        ReloadColors();
                    }
                    MnuPunlisher.Items[i].Selected = true;
                    return false;
                }
            }
            //mv.ActiveViewIndex = savedIndex;
            return true;
        }



        protected void ClearData()
        {
            TxtCode.Text = string.Empty;
            TxtName.Text = string.Empty;
            ExtURLQS = null;
            txtPostURL.Text = "";
            gvHttpPost.DataSource = ExtURLQS;
            gvHttpPost.DataBind();
            RdbLstIsActive.SelectedValue = "false";
            RdbLstHasPaid.SelectedValue = "false";
            RdbLstShowNewsLetters.SelectedValue = "false";
            RdbLstShowNewSubscriptionLink.SelectedValue = "false";
            RdbLstShowRenewLink.SelectedValue = "false";
            RdbLstShowCustomerServiceLink.SelectedValue = "false";
            RdbLstShowRelatedTradeShows.SelectedValue = "false";

            TxtThankYouPageLink.Text = string.Empty;
            DdlLoginVerification.SelectedValue = "false";

            RadEditorHeaderHTML.Content = string.Empty;
            RadEditorHomePageDesc.Content = string.Empty;
            RadEditorStep2PageDesc.Content = string.Empty;
            RadEditorNewPageDesc.Content = string.Empty;
            RadEditorRenewPageDesc.Content = string.Empty;
            RadEditorLoginPageDesc.Content = string.Empty;
            RadEditorForgotPassword.Content = string.Empty;
            RadEditorThankYouPageHTML.Content = string.Empty;
            RadEditorFooterHTML.Content = string.Empty;
            RadEditorCustomerServiceHTML.Content = string.Empty;
            RadEditorFAQHTML.Content = string.Empty;
        }

        private void LoadQSValues()
        {
            int groupid = GroupID;
            string strQuery = " Select shortname as shortname from (" +
                       " Select ('CustomValue') as ShortName" +
                       " UNION Select ('EmailAddress') as ShortName" +
                       " UNION Select ('FirstName') as ShortName" +
                       " UNION Select ('LastName') as ShortName" +
                       " UNION Select ('FullName') as ShortName" +
                       " UNION Select ('Password') as ShortName" +
                       " UNION Select ('Title') as ShortName" +
                       " UNION Select ('Company') as ShortName" +
                       " UNION Select ('Occupation') as ShortName" +
                       " UNION Select ('Address') as ShortName" +
                       " UNION Select ('Address2') as ShortName" +
                       " UNION Select ('City') as ShortName" +
                       " UNION Select ('State') as ShortName" +
                       " UNION Select ('Zip') as ShortName" +
                       " UNION Select ('ZipPlus') as ShortName" +
                       " UNION Select ('Country') as ShortName" +
                       " UNION Select ('Voice') as ShortName" +
                       " UNION Select ('Mobile') as ShortName" +
                       " UNION Select ('Fax') as ShortName" +
                       " UNION Select ('Website') as ShortName" +
                       " UNION Select ('Age') as ShortName" +
                       " UNION Select ('Income') as ShortName" +
                       " UNION Select ('Gender') as ShortName" +
                       " UNION Select ('Birthdate') as ShortName";
            if (groupid > 0)
                strQuery = strQuery + " UNION select ShortName from groupdatafields where groupID = " + groupid + " and isnull(datafieldSetID,0) = 0)AS ECNDATAFIELD ";
            else
                strQuery = strQuery + ") AS ECNDATAFIELD ";

            DataTable dtFormECNTextField = DataFunctions.GetDataTable(strQuery, ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString);
            if (dtFormECNTextField.Rows.Count > 0)
            {
                drpQSValue.DataTextField = "ShortName";
                drpQSValue.DataValueField = "ShortName";
                drpQSValue.DataSource = dtFormECNTextField;
                drpQSValue.DataBind();
                drpQSValue.Items.Insert(0, new ListItem("-Select-", "-Select-"));
                drpQSValue.Items.FindByValue("-Select-").Selected = true;
            }

        }

        protected void drpQSValue_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (drpQSValue.SelectedValue.Equals("CustomValue"))
            {
                txtQSValue.Visible = true;
            }
            else
            {
                txtQSValue.Visible = false;
            }
        }

        protected void LoadPublisher()
        {
            try
            {
                loadHttpPostData(PubID);
                sqldatasourceFinish.SelectParameters["iMode"].DefaultValue = "4";
                System.Data.DataView mydataview = (System.Data.DataView)sqldatasourceFinish.Select(DataSourceSelectArguments.Empty);
                if (mydataview.Table.Rows.Count > 0)
                {
                    try
                    {
                        ddlCustomer.ClearSelection();
                        ddlCustomer.Items.FindByValue(mydataview.Table.Rows[0]["ECNCustomerID"].ToString()).Selected = true;
                        ddlCustomer.Enabled = false;
                    }
                    catch
                    { }

                    TxtCode.Text = mydataview.Table.Rows[0]["PubCode"].ToString();
                    TxtName.Text = mydataview.Table.Rows[0]["PubName"].ToString();

                    string paymentGateWay = mydataview.Table.Rows[0]["PaymentGateway"].ToString();
                    pnlPaymentGateway.Visible = true;

                    rblstPaymentGateway.ClearSelection();
                    try
                    {
                        rblstPaymentGateway.Items.FindByValue(paymentGateWay).Selected = true;
                    }
                    catch
                    { }

                    if (paymentGateWay == Enums.CCProcessors.Paypal.ToString() || paymentGateWay == Enums.CCProcessors.PaypalRedirect.ToString())
                    {
                        pnlPayPal.Visible = true;
                        pnlPayPalPro.Visible = true;
                        pnlPayFlow.Visible = false;
                        pnlAuthorizeNet.Visible = false;
                        TxtPayflowAccount.Text = mydataview.Table.Rows[0]["PayflowAccount"].ToString();
                        TxtPayflowPassword.Text = mydataview.Table.Rows[0]["PayflowPassword"].ToString();
                        TxtPayflowSignature.Text = mydataview.Table.Rows[0]["PayflowSignature"].ToString();
                        if (paymentGateWay == Enums.CCProcessors.PaypalRedirect.ToString())
                        {
                            trPayflowPageStyle.Visible = true;
                            txtPayflowPageStyle.Text = mydataview.Table.Rows[0]["PayflowPageStyle"].ToString();
                        }
                        else
                        {
                            trPayflowPageStyle.Visible = false;
                        }
                    }
                    if (paymentGateWay == Enums.CCProcessors.Paypalflow.ToString())
                    {
                        pnlPayPal.Visible = true;
                        pnlPayPalPro.Visible = false;
                        pnlPayFlow.Visible = true;
                        pnlAuthorizeNet.Visible = false;

                        TxtPayflowPartner.Text = mydataview.Table.Rows[0]["PayflowPartner"].ToString();
                        TxtPayflowVendor.Text = mydataview.Table.Rows[0]["PayflowVendor"].ToString();
                        TxtPayflowAccount.Text = mydataview.Table.Rows[0]["PayflowAccount"].ToString();
                        TxtPayflowPassword.Text = mydataview.Table.Rows[0]["PayflowPassword"].ToString();
                        trPayflowPageStyle.Visible = false;
                    }
                    else if (paymentGateWay == Enums.CCProcessors.AuthorizeDotNet.ToString())
                    {
                        pnlAuthorizeNet.Visible = true;
                        pnlPayPal.Visible = false;
                        txtAuthorizeDotNetAccount.Text = mydataview.Table.Rows[0]["AuthorizeDotNetAccount"].ToString();
                        txtAuthorizeDotNetSignature.Text = mydataview.Table.Rows[0]["AuthorizeDotNetSignature"].ToString();
                    }

                    txtProcessExternalURL.Text = mydataview.Table.Rows[0]["ProcessExternalURL"].ToString();
                    GroupID = Convert.ToInt32(mydataview.Table.Rows[0]["ECNDefaultGroupID"].ToString());
                    SFID = Convert.ToInt32(mydataview.Table.Rows[0]["ECNSFID"].ToString());

                    string isactive = (mydataview.Table.Rows[0]["IsActive"].ToString().ToLower() == "true" ? "true" : "false");
                    RdbLstIsActive.SelectedValue = isactive;

                    try
                    {
                        if (isactive.ToUpper() == "TRUE")
                            pnlRedirectorHTML.Visible = false;
                        else
                            pnlRedirectorHTML.Visible = true;
                    }
                    catch
                    {
                        pnlRedirectorHTML.Visible = false;
                    }

                    try
                    {
                        RdbLstHasPaid.SelectedValue = (mydataview.Table.Rows[0]["HasPaid"].ToString().ToLower() == "true" ? "true" : "false");
                        bool IsPaymentAccountVisible = false;

                        try
                        {
                            rbProcessExternal.SelectedValue = (mydataview.Table.Rows[0]["ProcessExternal"].ToString().ToLower() == "true" ? "true" : "false");
                            IsPaymentAccountVisible = Convert.ToBoolean(rbProcessExternal.SelectedValue.ToString());
                        }
                        catch
                        { }

                        pnlPaymentOptions.Visible = Convert.ToBoolean(RdbLstHasPaid.SelectedValue);
                        pnlPaymentGateway.Visible = Convert.ToBoolean(RdbLstHasPaid.SelectedValue) && !IsPaymentAccountVisible;
                        pnlProcessExternal.Visible = Convert.ToBoolean(RdbLstHasPaid.SelectedValue);
                        pnlPayPal.Visible = Convert.ToBoolean(RdbLstHasPaid.SelectedValue) && ((paymentGateWay == Enums.CCProcessors.Paypal.ToString() || paymentGateWay == Enums.CCProcessors.PaypalRedirect.ToString()) && !IsPaymentAccountVisible);
                        pnlAuthorizeNet.Visible = Convert.ToBoolean(RdbLstHasPaid.SelectedValue) && (paymentGateWay == Enums.CCProcessors.AuthorizeDotNet.ToString() && !IsPaymentAccountVisible);

                        if (pnlPaymentOptions.Visible)
                        {
                            if (Convert.ToBoolean(rbProcessExternal.SelectedValue))
                            {
                                pnlPayPal.Visible = false;
                                trPayflowPageStyle.Visible = false;
                                pnlPayPalPro.Visible = false;
                                pnlPayFlow.Visible = false;
                                pnlProcessExternal.Visible = true;
                            }
                            else
                            {
                                pnlPayPal.Visible = (paymentGateWay == Enums.CCProcessors.Paypal.ToString() || paymentGateWay == Enums.CCProcessors.PaypalRedirect.ToString() || paymentGateWay == Enums.CCProcessors.Paypalflow.ToString());
                                pnlPayPalPro.Visible = (paymentGateWay == Enums.CCProcessors.Paypal.ToString() || paymentGateWay == Enums.CCProcessors.PaypalRedirect.ToString());
                                pnlPayFlow.Visible = (paymentGateWay == Enums.CCProcessors.Paypalflow.ToString());
                                trPayflowPageStyle.Visible = (paymentGateWay == Enums.CCProcessors.PaypalRedirect.ToString());
                                pnlProcessExternal.Visible = false;
                            }
                        }

                    }
                    catch
                    { }

                    RdbLstShowNewsLetters.SelectedValue = (mydataview.Table.Rows[0]["ShowNewsletters"].ToString().ToLower() == "true" ? "true" : "false");
                    RdbLstShowNewSubscriptionLink.SelectedValue = (mydataview.Table.Rows[0]["ShowNewSubLink"].ToString().ToLower() == "true" ? "true" : "false");
                    RdbLstShowRenewLink.SelectedValue = (mydataview.Table.Rows[0]["ShowRenewSubLink"].ToString().ToLower() == "true" ? "true" : "false");
                    RdbLstShowCustomerServiceLink.SelectedValue = (mydataview.Table.Rows[0]["ShowCustomerServiceLink"].ToString().ToLower() == "true" ? "true" : "false");
                    RdbLstShowRelatedTradeShows.SelectedValue = (mydataview.Table.Rows[0]["ShowTradeShowLink"].ToString().ToLower() == "true" ? "true" : "false");
                    TxtThankYouPageLink.Text = mydataview.Table.Rows[0]["ThankYouPageLink"].ToString();
                    
                    try
                    {
                        DdlLoginVerification.ClearSelection();
                        DdlLoginVerification.Items.FindByValue(mydataview.Table.Rows[0]["LoginVerfication"].ToString()).Selected = true;
                    }
                    catch
                    { }


                    try
                    {
                        rbDisableSubLogin.ClearSelection();
                        rbDisableSubLogin.SelectedValue = (mydataview.Table.Rows[0]["DisableSubcriberLogin"].ToString().ToLower() == "true" ? "true" : "false");
                    }
                    catch
                    {
                        rbDisableSubLogin.Items.FindByValue("false").Selected = true;
                    }


                    try
                    {
                        rbDisablePassword.ClearSelection();
                        rbDisablePassword.SelectedValue = (mydataview.Table.Rows[0]["DisablePassword"].ToString().ToLower() == "true" ? "true" : "false");
                    }
                    catch
                    {
                        rbDisablePassword.Items.FindByValue("false").Selected = true;
                    }

                    try
                    {
                        rbDisableEmail.ClearSelection();
                        rbDisableEmail.SelectedValue = (mydataview.Table.Rows[0]["DisableEmail"].ToString().ToLower() == "true" ? "true" : "false");
                    }
                    catch
                    {
                        rbDisableEmail.Items.FindByValue("false").Selected = true;
                    }


                    hfldPubLogo.Value = (mydataview.Table.Rows[0]["PubLogo"].ToString() == "0" ? "" : mydataview.Table.Rows[0]["PubLogo"].ToString());

                    try
                    {
                        lblPubLogo.Text = (hfldPubLogo.Value == "0" || hfldPubLogo.Value.ToString().Length == 0 ? "" : "<img alt='" + mydataview.Table.Rows[0]["PubName"].ToString() + "' src='" + ConfigurationManager.AppSettings["LogoPath"].ToString() + hfldPubLogo.Value + "' />");
                    }
                    catch
                    { }

                    hfldMailingLabel.Value = (mydataview.Table.Rows[0]["MailingLabel"].ToString() == "0" ? "" : mydataview.Table.Rows[0]["MailingLabel"].ToString());

                    try
                    {
                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["MailingLabelPath"].ToString() + hfldMailingLabel.Value))
                        {
                            lblMailingLabel.Text = (hfldMailingLabel.Value == "0" || hfldMailingLabel.Value.ToString().Length == 0 ? "" : "<img height='75px' width='250px' alt='" + mydataview.Table.Rows[0]["MailingLabel"].ToString() + "' src='" + ConfigurationManager.AppSettings["MailingLabelImageURL"].ToString() + hfldMailingLabel.Value + "' />");
                        }
                    }
                    catch
                    { }

                    drpWidth.ClearSelection();

                    if (mydataview.Table.Rows[0].IsNull("Width"))
                    {
                        drpWidth.Items[0].Selected = true;
                    }
                    else
                    {
                        drpWidth.Items.FindByValue(mydataview.Table.Rows[0]["Width"].ToString()).Selected = true;
                    }

                    drpColumnFormat.ClearSelection();

                    if (mydataview.Table.Rows[0].IsNull("ColumnFormat"))
                    {
                        drpColumnFormat.Items[0].Selected = true;
                    }
                    else
                    {
                        drpColumnFormat.Items.FindByValue(mydataview.Table.Rows[0]["ColumnFormat"].ToString()).Selected = true;
                    }

                    try
                    {
                        rblstCheckSubscriberExists.ClearSelection();
                        rblstCheckSubscriberExists.Items.FindByValue(Convert.ToBoolean(mydataview.Table.Rows[0]["CheckSubscriber"].ToString()).ToString()).Selected = true;
                    }
                    catch
                    {

                    }

                    try
                    {
                        rblstShowShippingAddress.ClearSelection();
                        rblstShowShippingAddress.Items.FindByValue(Convert.ToBoolean(mydataview.Table.Rows[0]["ShowShippingAddress"].ToString()).ToString()).Selected = true;
                    }
                    catch { }

                    RadEditorHeaderHTML.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["HeaderHTML"].ToString());
                    RadEditorHomePageDesc.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["HomePageDesc"].ToString());
                    RadEditorStep2PageDesc.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["Step2PageDesc"].ToString());
                    RadEditorNewPageDesc.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["NewPageDesc"].ToString());
                    RadEditorRenewPageDesc.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["RenewPageDesc"].ToString());
                    RadEditorLoginPageDesc.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["LoginPageDesc"].ToString());
                    RadEditorForgotPassword.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["ForgotPasswordHTML"].ToString());
                    RadEditorThankYouPageHTML.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["ThankYouPageHTML"].ToString());
                    RadEditorFooterHTML.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["FooterHTML"].ToString());
                    RadEditorCustomerServiceHTML.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["CustomerServicePageHTML"].ToString());
                    RadEditorFAQHTML.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["FAQPageHTML"].ToString());
                    RadEditorRedirectorHTML.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["RedirectHTML"].ToString());
                    txtRedirectorLink.Text = Server.HtmlDecode(mydataview.Table.Rows[0]["RedirectLink"].ToString());
                    RadEditorPaidResponseEmail.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["PaidResponseEmail"].ToString());
                    RadEditorPaidThankyouPage.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["PaidThankYouPageHTML"].ToString());
                    txtPaidPageFromEmail.Text = Server.HtmlDecode(mydataview.Table.Rows[0]["PaidPageFromEmail"].ToString());
                    txtPaidFormFromName.Text = Server.HtmlDecode(mydataview.Table.Rows[0]["PaidPageFromName"].ToString());
                    txtPaidFormResponseEmailSubject.Text = Server.HtmlDecode(mydataview.Table.Rows[0]["PaidFormResponseEmailSubject"].ToString());
                    txtPaidThankyouPageLink.Text = Server.HtmlDecode(mydataview.Table.Rows[0]["PaidPageThankyouLink"].ToString());
                    txtPageTitle.Text = Server.HtmlDecode(mydataview.Table.Rows[0]["PageTitle"].ToString());
                    RadEditorCompResponseEmail.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["CompResponseEmailHTML"].ToString());

                    try
                    {
                        rblstRepeatEmails.ClearSelection();
                        rblstRepeatEmails.Items.FindByValue(Convert.ToBoolean(mydataview.Table.Rows[0]["RepeatEmails"].ToString()).ToString()).Selected = true;
                    }
                    catch { }
                }


                if ((mydataview.Table.Rows[0]["CSS"].ToString() != string.Empty) && (mydataview.Table.Rows[0]["CSS"].ToString() != "0"))
                {
                    string[] strCSS = mydataview.Table.Rows[0]["CSS"].ToString().Split('|');

                    txtPBGColor.Text = strCSS[0].Replace("#", "");
                    ColorPickerExtender1.SelectedColor = strCSS[0].Replace("#", "");
                    txtFBGColor.Text = strCSS[1].Replace("#", "");
                    ColorPickerExtender2.SelectedColor = strCSS[1].Replace("#", "");
                    ddlPageBorder.SelectedIndex = ddlPageBorder.Items.IndexOf(ddlPageBorder.Items.FindByValue(strCSS[2]));
                    ddlPageFont.SelectedIndex = ddlPageFont.Items.IndexOf(ddlPageFont.Items.FindByValue(strCSS[3]));
                    ddlPageFontSize.SelectedIndex = ddlPageFontSize.Items.IndexOf(ddlPageFontSize.Items.FindByValue(strCSS[4]));
                    txtCBGColor.Text = strCSS[5].Replace("#", "");
                    ColorPickerExtender3.SelectedColor = strCSS[5].Replace("#", "");
                    ddlCatFontSize.SelectedIndex = ddlCatFontSize.Items.IndexOf(ddlCatFontSize.Items.FindByValue(strCSS[6]));
                    txtCFColor.Text = strCSS[7].Replace("#", "");
                    ColorPickerExtender4.SelectedColor = strCSS[7].Replace("#", "");
                    ddlQFSize.SelectedIndex = ddlQFSize.Items.IndexOf(ddlQFSize.Items.FindByValue(strCSS[8]));
                    txtQFColor.Text = strCSS[9].Replace("#", "");
                    ColorPickerExtender5.SelectedColor = strCSS[9].Replace("#", "");
                    ddlQFBold.SelectedIndex = ddlQFBold.Items.IndexOf(ddlQFBold.Items.FindByValue(strCSS[10]));
                    ddlAFSize.SelectedIndex = ddlAFSize.Items.IndexOf(ddlAFSize.Items.FindByValue(strCSS[11]));
                    txtAFColor.Text = strCSS[12].Replace("#", "");
                    ColorPickerExtender6.SelectedColor = strCSS[12].Replace("#", "");
                    ddlAFBold.SelectedIndex = ddlAFBold.Items.IndexOf(ddlAFBold.Items.FindByValue(strCSS[13]));
                }

                RadEditorNewSubscriptionHeader.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["NewSubscriptionHeader"].ToString());
                txtNewSubscriptionLink.Text = mydataview.Table.Rows[0]["NewSubscriptionLink"].ToString();
                RadEditorManageSubscriptionHeader.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["ManageSubscriptionHeader"].ToString());
                txtManageSubscriptionLink.Text = mydataview.Table.Rows[0]["ManageSubscriptionLink"] == null ? string.Empty : mydataview.Table.Rows[0]["ManageSubscriptionLink"].ToString();
                RadEditorRequiredFieldEditor.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["RequiredFieldHTML"] == null ? string.Empty : mydataview.Table.Rows[0]["RequiredFieldHTML"].ToString());
                RadEditorNewsletterHeader.Content = Server.HtmlDecode(mydataview.Table.Rows[0]["NewsletterHeaderHTML"].ToString());


            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void RdbLstIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(RdbLstIsActive.SelectedItem.Value))
                pnlRedirectorHTML.Visible = false;
            else
                pnlRedirectorHTML.Visible = true;
        }

        protected void rblstPaymentGateway_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPAL" || rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALREDIRECT" || rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALFLOW")
            {
                pnlPayPal.Visible = true;
                trPayflowPageStyle.Visible = (rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALREDIRECT");

                pnlPayPalPro.Visible = (rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPAL" || rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALREDIRECT");
                pnlPayFlow.Visible = (rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALFLOW");

                pnlAuthorizeNet.Visible = false;
            }
            else
            {
                pnlAuthorizeNet.Visible = true;
                pnlPayPal.Visible = false;
            }
        }

        protected void RdbLstHasPaid_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlPaymentOptions.Visible = Convert.ToBoolean(RdbLstHasPaid.SelectedValue);
            pnlProcessExternal.Visible = Convert.ToBoolean(RdbLstHasPaid.SelectedValue);
            pnlPaymentGateway.Visible = Convert.ToBoolean(RdbLstHasPaid.SelectedValue);

            if (!pnlPaymentGateway.Visible)
            {
                pnlAuthorizeNet.Visible = false;
                pnlPayPal.Visible = false;
            }
            else if (rblstPaymentGateway.SelectedItem != null && rblstPaymentGateway.SelectedItem.Value == Enums.CCProcessors.AuthorizeDotNet.ToString())
            {
                pnlAuthorizeNet.Visible = true;
                pnlPayPal.Visible = false;
            }
            else
            {
                trPayflowPageStyle.Visible = (rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALREDIRECT");
                pnlPayPal.Visible = true;
                pnlAuthorizeNet.Visible = false;
                pnlPayPalPro.Visible = (rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPAL" || rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALREDIRECT");
                pnlPayFlow.Visible = (rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALFLOW");
            }

            if (pnlPaymentOptions.Visible)
            {
                rbProcessExternal_SelectedIndexChanged(sender, e);
            }
        }

        protected void rbProcessExternal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(rbProcessExternal.SelectedValue))
            {
                pnlPayPal.Visible = false;
                pnlAuthorizeNet.Visible = false;
                pnlPaymentGateway.Visible = false;
                pnlProcessExternal.Visible = true;
            }
            else
            {
                pnlPaymentGateway.Visible = true;

                if (rblstPaymentGateway.SelectedItem != null && rblstPaymentGateway.SelectedItem.Value == Enums.CCProcessors.AuthorizeDotNet.ToString())
                    pnlAuthorizeNet.Visible = true;
                else
                {
                    pnlPayPal.Visible = true;
                    pnlPayPalPro.Visible = (rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPAL" || rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALREDIRECT");
                    pnlPayFlow.Visible = (rblstPaymentGateway.SelectedItem.Value.ToUpper() == "PAYPALFLOW");
                    if(rblstPaymentGateway.SelectedItem.Value == Enums.CCProcessors.PaypalRedirect.ToString())
                    {
                        trPayflowPageStyle.Visible = true;
                    }
                    else
                    {
                        trPayflowPageStyle.Visible = false;
                    }
                }

                pnlProcessExternal.Visible = false;
            }
        }

        protected void ValidateRequiredField_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (RadEditorRequiredFieldEditor.Content.Trim() == string.Empty)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void ValidateNewsletterHeader_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (RadEditorNewsletterHeader.Content.Trim() == string.Empty)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

    }
}
