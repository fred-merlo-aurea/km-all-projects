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
using System.IO;
using System.Text;
using ECN_Framework_Entities.Accounts;
using SecurityAccess = ECN_Framework.Common.SecurityAccess;

namespace ecn.accounts.customersmanager
{
    public partial class templatedetail : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS;
            Master.SubMenu = "add customer template";
            lblErrorMessage.Text = "";
            phError.Visible = false;

            //if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                int requestCTID = getCTID();

                if (Page.IsPostBack == false)
                {
                    ECN_Framework_Entities.Accounts.CustomerTemplate ct = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByCTID(requestCTID, Master.UserSession.CurrentUser);
                    if (ct != null)
                    {
                        ECN_Framework_Entities.Accounts.Customer cust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(ct.CustomerID.Value, false);
                        LoadCustomersDD(cust.BaseChannelID.Value.ToString());
                    }
                    else
                    {
                        LoadCustomersDD(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString());
                    }
                    
                    LoadTemplateDD();
                    if (requestCTID > 0)
                    {
                        btnSave.Text = "Update";
                        LoadFormData();
                    }
                    LoadTemplateDescription();
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        public int getCTID()
        {
            int theCTID = 0;
            try
            {
                theCTID = Convert.ToInt32(Request.QueryString["CTID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCTID;
        }


        #region Form Prep
        private void LoadCustomersDD(string theChannel)
        {
            ddlCustomerID.DataSource = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Convert.ToInt32(theChannel));
            ddlCustomerID.DataBind();
        }

        private void LoadTemplateDD()
        {
            ddlTemplateTypeCode.DataSource = ECN_Framework_BusinessLayer.Accounts.Code.GetByCodeType(ECN_Framework_Common.Objects.Accounts.Enums.CodeType.TemplateType, Master.UserSession.CurrentUser);
            ddlTemplateTypeCode.DataBind();
        }

        #endregion

        #region Data Load
        private void LoadFormData()
        {
            ECN_Framework_Entities.Accounts.CustomerTemplate customerTemplate = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByCTID(getCTID(), Master.UserSession.CurrentUser);
            
            ddlCustomerID.Items.FindByValue(customerTemplate.CustomerID.ToString()).Selected = true;
            ddlCustomerID.Enabled = false;
            ddlTemplateTypeCode.Items.FindByValue(customerTemplate.TemplateTypeCode.ToString()).Selected = true;
            tbHeaderSource.Text = customerTemplate.HeaderSource;
            tbFooterSource.Text = customerTemplate.FooterSource;
            cbActiveFlag.Checked = customerTemplate.IsActive.Value ? true : false;
        }

        private void LoadTemplateDescription()
        {
            string selectedTemplate = ddlTemplateTypeCode.SelectedValue.ToString();

           tbTemplateDescription.Text = ECN_Framework_BusinessLayer.Accounts.Code.GetByCodeType(ECN_Framework_Common.Objects.Accounts.Enums.CodeType.TemplateType, Master.UserSession.CurrentUser).Find(x => x.CodeValue == selectedTemplate).CodeDescription;
        }

        #endregion

        #region Data Handlers

        public void btnSave_Click(object sender, System.EventArgs e)
        {
            ECN_Framework_Entities.Accounts.CustomerTemplate customerTemplate = new ECN_Framework_Entities.Accounts.CustomerTemplate();
            customerTemplate.CTID = getCTID();
            customerTemplate.HeaderSource =  ECN_Framework_Common.Functions.StringFunctions.CleanString(tbHeaderSource.Text);
            customerTemplate.FooterSource = ECN_Framework_Common.Functions.StringFunctions.CleanString(tbFooterSource.Text);
            customerTemplate.CustomerID  = Convert.ToInt32(ddlCustomerID.SelectedItem.Value);
            customerTemplate.TemplateTypeCode = ddlTemplateTypeCode.SelectedItem.Value;
            customerTemplate.IsActive = cbActiveFlag.Checked ? true : false;

            if (customerTemplate.CTID > 0)
                customerTemplate.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            else
                customerTemplate.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

            try
            {
                ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.Save(customerTemplate, Master.UserSession.CurrentUser);
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                phError.Visible = true;
                return;
            }

            Response.Redirect("customerdetail.aspx?CustomerID=" + ddlCustomerID.SelectedItem.Value);
        }

        #endregion

        protected void TemplateTypeCode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadTemplateDescription();
        }
    }
}
