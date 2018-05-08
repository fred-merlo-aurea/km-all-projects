using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.lists.reports
{
    public partial class GroupExportUdfSetting : System.Web.UI.UserControl
    {
        
        /// <summary>
        /// Profile only is only profile data
        /// ProfilePlusStandalone is profiles plus the non-transactional UDFs
        /// ProfilePlusAllUDFs is profiles plus both standalone and transactional UDFs
        /// These are also the @DataToInclude parameters passed to the stored proc
        /// </summary>
        ///
        public string getDDLClientID
        {
            get { return ddlUDFSelect.ClientID; }
        }

        public string selected
        {
            get 
            {
                if (Session["ProfileFilter"] != null)
                {
                    return Session["ProfileFilter"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (Session["ProfileFilter"] == null)
                {
                    Session.Add("ProfileFilter", value);
                    ddlUDFSelect.SelectedValue = value;
                }
                else
                {
                    Session["ProfileFilter"] = value;
                    ddlUDFSelect.SelectedValue = value;
                }
            }
        }

        public bool CanDownloadTrans
        {
            set
            {
                _CanDownloadTrans = value;
            }
        }
        private bool _CanDownloadTrans;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                selected = ddlUDFSelect.SelectedValue.ToString();

                if (!_CanDownloadTrans)
                {
                    ddlUDFSelect.Items.Remove(ddlUDFSelect.Items.FindByValue("ProfilePlusAllUDFs"));
                    //ddlUDFSelect.SelectedValue = "ProfilePlusStandalone";
                    //selected = ddlUDFSelect.SelectedValue.ToString();
                }
                else
                {
                    selected = ddlUDFSelect.SelectedValue.ToString();
                }
                
            }
        }

        protected void ddlUDFSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected = ddlUDFSelect.SelectedValue.ToString();
        }
    }
}