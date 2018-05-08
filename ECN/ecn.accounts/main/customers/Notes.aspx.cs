using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using CommonFunctions = ECN_Framework_Common.Functions;
using AccountEntity = ECN_Framework_Entities.Accounts;
using AccountBLL = ECN_Framework_BusinessLayer.Accounts;

namespace ecn.accounts.main.customers
{
    public partial class Notes : ECN_Framework.WebPageHelper
    {

        private int CustomerID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["CustomerID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int NotesID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["ID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int reload
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["reload"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private ECN_Framework_BusinessLayer.Application.ECNSession _usersession = null;
        private ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                if (_usersession == null)
                    _usersession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

                return _usersession;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            phError.Visible = false;

            btnClose.Attributes.Add("onclick", "javascript:closewindow(0);");

            if (UserSession.CurrentUser.IsKMStaff)
            {
                if (!IsPostBack)
                {
                    if (NotesID > 0)
                    {
                        AccountEntity.CustomerNote customerNote = AccountBLL.CustomerNote.GetByNoteID(NotesID, UserSession.CurrentUser);

                        if (customerNote != null)
                        {
                            txtNotes.Text = customerNote.Notes; 
                            lblUpdatedby.Text = customerNote.UpdatedBy;
                            lblUpdatedDate.Text = customerNote.UpdatedDate.ToString();
                        }

                        btnSave.Visible = false;
                    }
                    else
                    {
                        pnlForEdit.Visible = false;
                    }
                }
                else
                {
                    btnSave.Visible = false;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AccountEntity.CustomerNote customerNote = new AccountEntity.CustomerNote();
            customerNote.NoteID = NotesID;
            customerNote.CustomerID = CustomerID;
            customerNote.Notes = txtNotes.Text.Replace("'", "''");
            customerNote.IsBillingNotes = chkbillingnotes.Checked ? true : false;

            if (customerNote.NoteID > 0)
                customerNote.UpdatedUserID = Convert.ToInt32(UserSession.CurrentUser.UserID);
            else
                customerNote.CreatedUserID = Convert.ToInt32(UserSession.CurrentUser.UserID);

            try
            {
                AccountBLL.CustomerNote.Save(customerNote, UserSession.CurrentUser);
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
            
            if (NotesID > 0)
            {
                Response.Write("<script>window.opener.location.reload();self.close();</script>");
            }
            else
            {
                if (reload == 1)
                    Response.Write("<script>window.opener.location.reload();self.close();</script>");
                else
                    Response.Write("<script>self.close();</script>");
            }

        }
    }
}
