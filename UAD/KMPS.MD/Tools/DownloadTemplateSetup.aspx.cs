using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Helpers;
using Telerik.Web.UI;
using KMPS.MD.Objects;

namespace KMPS.MD.Tools
{
    public partial class DownloadTemplateSetup : BrandsPageBase
    {
        delegate void HidePanel();
        delegate void LoadEditCaseData(Dictionary<string, string> downloadfields);

        private void DownloadCasePopupHide()
        {
            DownloadEditCase.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Tools";
            Master.SubMenu = "Download Template Setup";
            divError.Visible = false;
            lblErrorMessage.Text = string.Empty;

            HidePanel delDownloadCase = new HidePanel(DownloadCasePopupHide);
            this.DownloadEditCase.hideDownloadCasePopup = delDownloadCase;

            LoadEditCaseData delNoParamDownloadFields = new LoadEditCaseData(LoadEditCase);
            this.DownloadEditCase.LoadEditCaseData = delNoParamDownloadFields;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                LoadBrands();

                rgDownloadTemplate.DataSource = getTemplate();
                rgDownloadTemplate.DataBind();

                LoadExportFields();
            }
        }

        public void LoadEditCase(Dictionary<string, string> downloadfields)
        {
            ControlsValidators.LoadEditCase(downloadfields, lstDestFields);
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfBrandID.Value = drpBrand.SelectedValue;
            rgDownloadTemplate.DataSource = getTemplate();
            rgDownloadTemplate.DataBind();

            if (Convert.ToInt32(hfBrandID.Value) >= 0)
            {
                LoadProducts();
                LoadExportFields();
            }
        }

        protected void rblViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblViewType.SelectedValue.Equals("ConsensusView", StringComparison.OrdinalIgnoreCase) || rblViewType.SelectedValue.Equals("RecencyView", StringComparison.OrdinalIgnoreCase))
            {
                pnlProduct.Visible = false;
                LoadExportFields();
            }
            else
            {
                pnlProduct.Visible = true;
                lstSourceFields.Items.Clear();
                lstDestFields.Items.Clear();
                LoadProducts();
            }
        }

        protected void LoadProducts()
        {
            List<Pubs> lpubs = new List<Pubs>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                lpubs = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                lpubs = Pubs.GetSearchEnabled(Master.clientconnections);

            lpubs = (from p in lpubs
                     orderby p.PubName ascending
                     select p).ToList(); ;

            drpProduct.DataSource = lpubs;
            drpProduct.DataBind();
            drpProduct.Items.Insert(0, new ListItem("", "0"));
        }

        protected void drpProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfProductID.Value =  drpProduct.SelectedValue;
            LoadExportFields();
        }

        protected void LoadExportFields()
        {
            try
            {
                lstSourceFields.Items.Clear();
                lstDestFields.Items.Clear();

                List<int> PubIDs = new List<int>();

                if (pnlProduct.Visible)
                    PubIDs.Add(Convert.ToInt32(drpProduct.SelectedValue));

                Dictionary<string, string> exportfields = Utilities.GetExportFields(Master.clientconnections, (Enums.ViewType)Enum.Parse(typeof(Enums.ViewType), rblViewType.SelectedValue), Convert.ToInt32(hfBrandID.Value), PubIDs, Enums.ExportType.FTP, Master.UserSession.UserID);
                Dictionary<string, string> selectedfields = new Dictionary<string, string>();

                List<DownloadTemplateDetails> dtd = DownloadTemplateDetails.GetByDownloadTemplateID(Master.clientconnections, Convert.ToInt32(hfDownloadTemplateID.Value));

                foreach (var item in exportfields)
                {
                    if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                        lstSourceFields.Items.Add(new ListItem(item.Value.ToUpper(), item.Key));
                    else
                        selectedfields.Add(item.Key, item.Value);
                }

                foreach (DownloadTemplateDetails td in dtd)
                {
                    string text = string.Empty;

                    if (td.FieldCase != null && td.FieldCase != "")
                    {
                        switch (td.FieldCase.ToUpper())
                        {
                            case "PROPERCASE":
                                text = "Proper Case";
                                break;
                            case "UPPERCASE":
                                text = "UPPER CASE";
                                break;
                            case "LOWERCASE":
                                text = "lower case";
                                break;
                            default:
                                text = "Default";
                                break;
                        }
                    }

                    var field = selectedfields.FirstOrDefault(x => x.Key.Split('|')[0].ToUpper() == td.ExportColumn.ToUpper());
                    lstDestFields.Items.Add(new ListItem(field.Value.ToUpper() + (field.Key.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ?  "(" + (td.FieldCase == null | td.FieldCase == "" ? Enums.FieldCase.Default.ToString() : text) + ")" : ""), field.Key + "|" + (td.FieldCase == null | td.FieldCase == "" ? (field.Key.Split('|')[1].ToUpper() == KMPS.MD.Objects.Enums.FieldType.Varchar.ToString().ToUpper() ? KMPS.MD.Objects.Enums.FieldCase.Default.ToString() : KMPS.MD.Objects.Enums.FieldCase.None.ToString()) : td.FieldCase)));
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        protected void rgDownloadTemplate_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            rgDownloadTemplate.DataSource = getTemplate();
        }

        private List<dynamic> getTemplate()
        {
            List<KMPS.MD.Objects.DownloadTemplate> ldt = null;

            if (Convert.ToInt32(hfBrandID.Value) >= 0)
            {
                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    ldt = DownloadTemplate.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                else
                    ldt = DownloadTemplate.GetNotInBrand(Master.clientconnections);
            }

            List<Pubs> lpubs = Pubs.GetActive(Master.clientconnections);

            List<dynamic> lst = (from a in ldt
                           join b in lpubs on a.PubID equals b.PubID into temp
                           from b in temp.DefaultIfEmpty()
                                 select new { DownloadTemplateID = a.DownloadTemplateID, DownloadTemplateName = a.DownloadTemplateName, PubID = a.PubID, PubName = b != null ? b.PubName : default(string), }).ToList<dynamic>();
            return lst;
        }

        protected void lnk_Command(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditDownloadTemplate", StringComparison.OrdinalIgnoreCase))
                {
                    if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                    {
                        ResetControls();
                        DownloadTemplate t = DownloadTemplate.GetByID(Master.clientconnections, Convert.ToInt32(e.CommandArgument));
                        hfDownloadTemplateID.Value = t.DownloadTemplateID.ToString();
                        txtDownloadTemplateName.Text = t.DownloadTemplateName;
                        hfCreatedDate.Value = t.CreatedDate.ToString();
                        hfCreatedUserID.Value = t.CreatedUserID.ToString();

                        if(t.PubID > 0)
                        {
                            pnlProduct.Visible = true;
                            LoadProducts();
                            drpProduct.SelectedValue = t.PubID.ToString();
                            hfProductID.Value = t.PubID.ToString();
                            rblViewType.SelectedValue = "ProductView";
                            rblViewType.Enabled = false;
                        }
                        else
                        {
                            rblViewType.SelectedValue = "ConsensusView";
                        }

                        LoadExportFields();
                    }
                }
                else
                {
                    if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                    {
                        DownloadTemplate.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.UserID);
                    }
                }

            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.Message;
            }
        }

        protected void btnAdd_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstSourceFields.Items.Count; i++)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    lstDestFields.Items.Add(new ListItem(lstSourceFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ? lstSourceFields.Items[i].Text.ToUpper() + "(Default)" : lstSourceFields.Items[i].Text.ToUpper(), lstSourceFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper() ? lstSourceFields.Items[i].Value + "|Default" : lstSourceFields.Items[i].Value + "|None"));
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
                    lstSourceFields.Items.Add(new ListItem(lstDestFields.Items[i].Text.Split('(')[0].ToUpper(), lstDestFields.Items[i].Value.Split('|')[0] + "|" + lstDestFields.Items[i].Value.Split('|')[1]));
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
            ControlsValidators.ReorderSelectedList(lstDestFields);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                try
                {
                    if (DownloadTemplate.ExistsByDownloadTemplateName(Master.clientconnections, Convert.ToInt32(hfDownloadTemplateID.Value), txtDownloadTemplateName.Text))
                    {
                        DisplayError("Download Template Name already exists. Please enter different name.");
                        return;
                    }

                    if (lstDestFields.Items.Count == 0)
                    {
                        DisplayError("Please select a download field.");
                        return;
                    }

                    if (Convert.ToInt32(hfDownloadTemplateID.Value) > 0)
                    {
                        DownloadTemplateDetails.Delete(Master.clientconnections, Convert.ToInt32(hfDownloadTemplateID.Value));
                    }

                    DownloadTemplate t = new DownloadTemplate();

                    t.DownloadTemplateID = Convert.ToInt32(hfDownloadTemplateID.Value);
                    t.DownloadTemplateName = txtDownloadTemplateName.Text;
                    t.BrandID = Convert.ToInt32(hfBrandID.Value);
                    t.PubID = Convert.ToInt32(hfProductID.Value);
                    if (t.DownloadTemplateID > 0)
                    {
                        t.UpdatedUserID = Master.LoggedInUser;
                        t.UpdatedDate = DateTime.Now;
                        t.CreatedDate = Convert.ToDateTime(hfCreatedDate.Value);
                        t.CreatedUserID = Convert.ToInt32(hfCreatedUserID.Value);
                    }
                    else
                    {
                        t.CreatedUserID = Master.LoggedInUser;
                        t.CreatedDate = DateTime.Now;
                        t.UpdatedUserID = Master.LoggedInUser;
                        t.UpdatedDate = DateTime.Now;
                    }

                    int downloadtemplateID = DownloadTemplate.Save(Master.clientconnections, t);

                    if (lstDestFields.Items.Count > 0)
                    {
                        foreach (ListItem item in lstDestFields.Items)
                        {
                            DownloadTemplateDetails dtd = new DownloadTemplateDetails();
                            dtd.DownloadTemplateID = downloadtemplateID;

                            if (item.Value.Split('|')[0].Contains("_Description"))
                            {
                                dtd.ExportColumn = item.Value.Split('|')[0].Replace("_Description", "");
                                dtd.IsDescription = true;
                                dtd.FieldCase = item.Value.Split('|')[2].ToUpper() == KMPS.MD.Objects.Enums.FieldCase.None.ToString().ToUpper() ? null : item.Value.Split('|')[2];
                            }
                            else
                            {
                                dtd.ExportColumn = item.Value.Split('|')[0];
                                dtd.IsDescription = false;
                                dtd.FieldCase = item.Value.Split('|')[2].ToUpper() == KMPS.MD.Objects.Enums.FieldCase.None.ToString().ToUpper() ? null : item.Value.Split('|')[2];
                            }

                            DownloadTemplateDetails.Save(Master.clientconnections, dtd);
                        }
                    }

                    Response.Redirect("DownloadTemplateSetup.aspx");
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("DownloadTemplateSetup.aspx");
        }

        public void ResetControls()
        {
            hfDownloadTemplateID.Value = "0";
            txtDownloadTemplateName.Text = string.Empty;
            lstDestFields.Items.Clear();
            lstSourceFields.Items.Clear();
            rblViewType.Enabled = true;
        }

        protected void btnEditCase_Click(object sender, EventArgs e)
        {
            if (lstDestFields.Items.Count >= 1)
            {
                DownloadEditCase.Visible = true;
                Dictionary<string, string> downloadFields = new Dictionary<string, string>();

                for (int i = 0; i < lstDestFields.Items.Count; i++)
                {
                    if (lstDestFields.Items[i].Value.Split('|')[1].ToUpper() == Enums.FieldType.Varchar.ToString().ToUpper())
                        downloadFields.Add(lstDestFields.Items[i].Value, lstDestFields.Items[i].Text.Split('(')[0]);
                }

                DownloadEditCase.DownloadFields = downloadFields;
                DownloadEditCase.loadControls();
            }
            else
            {
                DisplayError("Please select field to edit case.");
            }
        }

        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;

        protected override void LoadPageFilters()
        {
            //Nothing to do here, but had to implement abstract method
        }
    }
}

