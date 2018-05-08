using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.SqlTypes;
using KMPS.MD.Objects;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;
using KMPS.MD.Helpers;

namespace KMPS.MD.Tools
{
    public partial class RecordViewPDFSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Tools";
            Master.SubMenu = "Record View Setup";
            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                Config c = Config.getCustomerLogo(Master.clientconnections);
                if (c.ConfigID != 0)
                {
                    int customerID = Master.UserSession.CurrentCustomer.CustomerID;
                    imglogo.ImageUrl = "../Images/logo/" + customerID + "/" + c.Value;
                    imglogo.Visible = true;
                    hfConfigID.Value = c.ConfigID.ToString();
                    hfImage.Value = c.Value;
                    FileSelector.Visible = true;
                }

                loadMasterGroupFields();
                loadSubExtensionMapperFields();
            }
        }

        private void loadMasterGroupFields()
        {
            List<RecordViewField> rvf = new List<RecordViewField>();

            rvf = RecordViewField.getMasterGroup(Master.clientconnections);

            Dictionary<int, string> fields = new Dictionary<int, string>();

            try
            {
                    List<MasterGroup> masterGroupList = MasterGroup.GetActiveMasterGroupsSorted(Master.clientconnections);

                    foreach (MasterGroup m in masterGroupList)
                    {
                        if (!rvf.Exists(x => x.MasterGroupID == m.MasterGroupID))
                            lstSourceFields.Items.Add(new ListItem(m.DisplayName, m.MasterGroupID.ToString()));
                        else
                            fields.Add(m.MasterGroupID, m.DisplayName);
                    }

                    foreach (RecordViewField rv in rvf)
                    {
                        string displayname = fields.FirstOrDefault(x => x.Key == rv.MasterGroupID).Value;
                        lstDestFields.Items.Add(new ListItem(displayname, rv.MasterGroupID.ToString()));
                    }

            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }


        private void loadSubExtensionMapperFields()
        {
            List<RecordViewField> rvf = new List<RecordViewField>();

            rvf = RecordViewField.getSubscriptionsExtensionMapper(Master.clientconnections);

            Dictionary<int, string> fields = new Dictionary<int, string>();

            try
            {
                List<SubscriptionsExtensionMapper> extensionMappers = SubscriptionsExtensionMapper.GetActive(Master.clientconnections) ;

                foreach (SubscriptionsExtensionMapper s in extensionMappers)
                {
                    if (!rvf.Exists(x => x.SubscriptionsExtensionMapperID == s.SubscriptionsExtensionMapperId))
                        lstAdhocSourceFields.Items.Add(new ListItem(s.CustomField, s.SubscriptionsExtensionMapperId.ToString()));
                    else
                        fields.Add(s.SubscriptionsExtensionMapperId, s.CustomField);
                }

                foreach (RecordViewField rv in rvf)
                {
                    string CustomField = fields.FirstOrDefault(x => x.Key == rv.SubscriptionsExtensionMapperID).Value;
                    lstAdhocDestFields.Items.Add(new ListItem(CustomField, rv.SubscriptionsExtensionMapperID.ToString()));
                }

                if (lstAdhocDestFields.Items.Count == 0 && lstAdhocSourceFields.Items.Count == 0)
                {
                    pnlAdhoc.Visible = false;
                }

            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }


        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                try
                {
                    if (pnlAdhoc.Visible)
                    {
                        if (lstDestFields.Items.Count == 0 && lstAdhocDestFields.Items.Count == 0)
                        {
                            DisplayError("Please select Dimensions or Adhoc");
                            return;
                        }
                    }
                    else
                    {
                        if (lstDestFields.Items.Count == 0)
                        {
                            DisplayError("Please select Dimensions");
                            return;
                        }
                    }

                    if (hfImage.Value != string.Empty)
                    {
                        Config config = new Config();
                        config.Name = "CustomerLogo";
                        config.ConfigID = Convert.ToInt32(hfConfigID.Value);
                        config.Value = hfImage.Value;
                        Config.Save(Master.clientconnections, config);
                    }

                    RecordViewField.Delete(Master.clientconnections);

                    if (lstDestFields.Items.Count > 0)
                    {
                        foreach (ListItem item in lstDestFields.Items)
                        {
                            RecordViewField rvf = new RecordViewField();
                            rvf.MasterGroupID = Convert.ToInt32(item.Value);
                            rvf.SubscriptionsExtensionMapperID = 0;
                            RecordViewField.Save(Master.clientconnections, rvf);
                        }
                    }

                    if (lstAdhocDestFields.Items.Count > 0)
                    {
                        foreach (ListItem item in lstAdhocDestFields.Items)
                        {
                            RecordViewField rvf = new RecordViewField();
                            rvf.MasterGroupID = 0;
                            rvf.SubscriptionsExtensionMapperID = Convert.ToInt32(item.Value);
                            RecordViewField.Save(Master.clientconnections, rvf);
                        }
                    }

                    Response.Redirect("RecordViewPdfSetup.aspx");
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecordViewPdfSetup.aspx");
        }

        //image upload events

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;

            var result = FileHelper.SaveLogo(FileSelector.PostedFile, Master.UserSession.CustomerID, Server);

            if (result.Succeeded)
            {
                imglogo.Visible = true;
                imglogo.ImageUrl = result.Url;
                hfImage.Value = result.FileName;
            }
            else
            {
                DisplayError(result.ErrorMessage);
            }
        }

        //Dimemsion events

        protected void btnAdd_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstSourceFields.Items.Count; i++)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    lstDestFields.Items.Add(lstSourceFields.Items[i]);
                    lstSourceFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void btnRemove_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstDestFields.Items.Count; i++)
            {
                if (lstDestFields.Items[i].Selected)
                {
                    lstSourceFields.Items.Add(lstDestFields.Items[i]);
                    lstDestFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstDestFields.Items.Count; i++)
            {
                if (lstDestFields.Items[i].Selected)
                {
                    if (i > 0 && !lstDestFields.Items[i - 1].Selected)
                    {
                        ListItem bottom = lstDestFields.Items[i];
                        lstDestFields.Items.Remove(bottom);
                        lstDestFields.Items.Insert(i - 1, bottom);
                        lstDestFields.Items[i - 1].Selected = true;
                    }
                }
            }
        }

        protected void btndown_Click(object sender, EventArgs e)
        {
            int startindex = lstDestFields.Items.Count - 1;

            for (int i = startindex; i > -1; i--)
            {
                if (lstDestFields.Items[i].Selected)
                {
                    if (i < startindex && !lstDestFields.Items[i + 1].Selected)
                    {
                        ListItem bottom = lstDestFields.Items[i];
                        lstDestFields.Items.Remove(bottom);
                        lstDestFields.Items.Insert(i + 1, bottom);
                        lstDestFields.Items[i + 1].Selected = true;
                    }
                }
            }
        }

        //Adhoc events

        protected void btnAdhocAdd_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstAdhocSourceFields.Items.Count; i++)
            {
                if (lstAdhocSourceFields.Items[i].Selected)
                {
                    lstAdhocDestFields.Items.Add(lstAdhocSourceFields.Items[i]);
                    lstAdhocSourceFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void btnAdhocRemove_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstAdhocDestFields.Items.Count; i++)
            {
                if (lstAdhocDestFields.Items[i].Selected)
                {
                    lstAdhocSourceFields.Items.Add(lstAdhocDestFields.Items[i]);
                    lstAdhocDestFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void btnAdhocUp_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstAdhocDestFields.Items.Count; i++)
            {
                if (lstAdhocDestFields.Items[i].Selected)
                {
                    if (i > 0 && !lstAdhocDestFields.Items[i - 1].Selected)
                    {
                        ListItem bottom = lstAdhocDestFields.Items[i];
                        lstAdhocDestFields.Items.Remove(bottom);
                        lstAdhocDestFields.Items.Insert(i - 1, bottom);
                        lstAdhocDestFields.Items[i - 1].Selected = true;
                    }
                }
            }
        }

        protected void btnAdhocDown_Click(object sender, EventArgs e)
        {
            int startindex = lstAdhocDestFields.Items.Count - 1;

            for (int i = startindex; i > -1; i--)
            {
                if (lstAdhocDestFields.Items[i].Selected)
                {
                    if (i < startindex && !lstAdhocDestFields.Items[i + 1].Selected)
                    {
                        ListItem bottom = lstAdhocDestFields.Items[i];
                        lstAdhocDestFields.Items.Remove(bottom);
                        lstAdhocDestFields.Items.Insert(i + 1, bottom);
                        lstAdhocDestFields.Items[i + 1].Selected = true;
                    }
                }
            }
        }

    }
}