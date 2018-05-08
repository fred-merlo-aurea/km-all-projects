using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.Group
{
    public partial class addGroup : System.Web.UI.UserControl
    {

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            if (!IsPostBack)
            {
                LoadFolders(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);

                if (KMPlatform.BusinessLogic.Client.HasServiceFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SeedList))
                {
                    SeedListPanel.Visible = true;
                }
                else
                {
                    SeedListPanel.Visible = false;
                }
            }
        }

        public void Reset()
        {
            phError.Visible = false;
            GroupName.Text = "";
            GroupDescription.Text = "";
            drpFolder.SelectedValue = "0";

        }

        public bool Save()
        {
            int pub_folder = 0;
            if (PublicFolder.Checked)
            {
                pub_folder = 1;
            }
            string gname = ECN_Framework_Common.Functions.StringFunctions.CleanString(GroupName.Text.ToString().Trim());
            string gdesc = ECN_Framework_Common.Functions.StringFunctions.CleanString(GroupDescription.Text.ToString().Trim());

            try
            {
                ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
                group.GroupName = gname;
                group.GroupDescription = gdesc;
                group.FolderID = Convert.ToInt32(drpFolder.SelectedValue);
                group.PublicFolder = pub_folder;
                group.IsSeedList = Convert.ToBoolean(rbSeedList.SelectedItem.Value);
                group.AllowUDFHistory = "N";
                group.OwnerTypeCode = "customer";
                group.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                group.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                ECN_Framework_BusinessLayer.Communicator.Group.Save(group, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                return true;
            }
            catch (ECNException ex)
            {
                setECNError(ex);
                return false;
            }
        }


        public void LoadFolders(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            drpFolder.DataSource = folderList;
            drpFolder.DataBind();
            drpFolder.Items.Insert(0, "root");
            drpFolder.Items.FindByValue("root").Value = "0";
        }
    }
}