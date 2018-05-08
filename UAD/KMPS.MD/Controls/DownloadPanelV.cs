using System;
using System.Linq;
using System.Web.UI.WebControls;
using KM.Common;
using KMPS.MD.Objects;
using Enums = KMPS.MD.Objects.Enums;

namespace KMPS.MD.Controls
{
    public abstract class DownloadPanelV : DownloadPanelBase
    {
        private const char DelimPipeChar = '|';
        private const char DemilParenthesisChar = '(';
        private const string DelimPipeString = "|";
        private const int IndexValue = 1;
        private const string ItemDefaultNameFormat = "{0}(Default)";
        private const string ItemDefaultValueFormat = "{0}|Default";
        private const string ItemValueFormat = "{0}|None";
        private const string MessageDownload = "Download";
        private const string ErrorNoFields = "Please select fields.";
        private const string HtmlAttributesHeight = "height";
        private const string HtmlAttributeOverflow = "overflow";
        private const string HtmlIdZero = "0";
        private const int IndexNoneSelected = -1;

        public Delegate HideDownloadPopup;

        protected abstract TextBox TxtDownloadUniqueCount { get; }

        protected abstract void DetailsDownload();

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            ListBox sourceList;
            if (PhProfileFields.Visible)
            {
                sourceList = LstAvailableProfileFields;
            }
            else if (PhDemoFields.Visible)
            {
                sourceList = LstAvailableDemoFields;
            }
            else
            {
                sourceList = LstAvailableAdhocFields;
            }

            SelectedToAvailableProfile(sourceList);
        }

        private void SelectedToAvailableProfile(ListBox sourceList)
        {
            Guard.NotNull(sourceList, nameof(sourceList));

            for (var i = 0; i < sourceList.Items.Count; i++)
            {
                if (sourceList.Items[i].Selected)
                {
                    var isDefault = string.Equals(
                            sourceList.Items[i].Value.Split(DelimPipeChar)[IndexValue],
                            Enums.FieldType.Varchar.ToString(),
                            StringComparison.OrdinalIgnoreCase);
                    var upperText = sourceList.Items[i].Text.ToUpper();
                    var itemText = isDefault
                        ? string.Format(ItemDefaultNameFormat, upperText)
                        : upperText;
                    var itemValue = isDefault
                        ? string.Format(ItemDefaultValueFormat, sourceList.Items[i].Value)
                        : string.Format(ItemValueFormat, sourceList.Items[i].Value);
                    LstSelectedFields.Items.Add(new ListItem(itemText,itemValue));
                    sourceList.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void BtnRemove_Click(object sender, EventArgs e)
        {
            var exportDemoFields = Utilities.GetExportFields(
                clientconnections, 
                ViewType, 
                BrandID, 
                PubIDs, 
                Enums.ExportType.FTP, 
                UserSession.CurrentUser.UserID, 
                Enums.ExportFieldType.Demo);
            var exportAdhocFields = Utilities.GetExportFields(
                clientconnections, 
                ViewType, 
                BrandID, 
                PubIDs, 
                Enums.ExportType.FTP, 
                UserSession.CurrentUser.UserID, 
                Enums.ExportFieldType.Adhoc);

            for (var i = 0; i < LstSelectedFields.Items.Count; i++)
            {
                var currentItem = LstSelectedFields.Items[i];
                if (LstSelectedFields.Items[i].Selected)
                {
                    var currentText = currentItem.Text;
                    var currentValue = currentItem.Value;
                    var valueSplitted = currentValue.Split(DelimPipeChar);
                    var valueSplitted0 = valueSplitted[0];
                    var valueSplitted1 = valueSplitted[1];
                    var textSplitted0 = currentText.Split(DemilParenthesisChar)[0];

                    var newItem = new ListItem(
                        textSplitted0.ToUpper(),
                        string.Join(DelimPipeString, valueSplitted0, valueSplitted1));

                    if (exportDemoFields.Any(x => x.Key.Split(DelimPipeChar)[0] == valueSplitted0))
                    {
                        LstAvailableDemoFields.Items.Add(newItem);
                    }
                    else if (exportAdhocFields.Any(x => x.Key.Split(DelimPipeChar)[0] == valueSplitted0))
                    {
                        LstAvailableAdhocFields.Items.Add(newItem);
                    }
                    else
                    {
                        LstAvailableProfileFields.Items.Add(newItem);
                    }
                    LstSelectedFields.Items.RemoveAt(i);

                    i--;
                }
            }
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < LstSelectedFields.Items.Count; i++)
            {
                if (LstSelectedFields.Items[i].Selected)
                {
                    if (i > 0 && !LstSelectedFields.Items[i - 1].Selected)
                    {
                        var bottom = LstSelectedFields.Items[i];
                        LstSelectedFields.Items.Remove(bottom);
                        LstSelectedFields.Items.Insert(i - 1, bottom);
                        LstSelectedFields.Items[i - 1].Selected = true;
                    }
                }
            }
        }

        protected void btndown_Click(object sender, EventArgs e)
        {
            var startindex = LstSelectedFields.Items.Count - 1;

            for (var i = startindex; i > -1; i--)
            {
                if (LstSelectedFields.Items[i].Selected)
                {
                    if (i < startindex && !LstSelectedFields.Items[i + 1].Selected)
                    {
                        var bottom = LstSelectedFields.Items[i];
                        LstSelectedFields.Items.Remove(bottom);
                        LstSelectedFields.Items.Insert(i + 1, bottom);
                        LstSelectedFields.Items[i + 1].Selected = true;
                    }
                }
            }
        }

        protected void DrpIsBillable_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(DrpIsBillable.SelectedValue))
            {
                BtnExport.OnClientClick = "return confirmPopupPurchase();";
                PlNotes.Visible = false;
            }
            else
            {
                BtnExport.OnClientClick = null;
                PlNotes.Visible = true;
            }
        }

        protected void rbDownload_CheckedChanged(object sender, EventArgs e)
        {
            BtnExport.Text = MessageDownload;
            if (ShowHeaderCheckBox)
            {
                PhShowHeader.Visible = true;
            }

            CbShowHeader.Checked = false;

            MdlDownloads.Show();
        }

        protected void BtnExport_click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (LstSelectedFields.Items.Count == 0)
            {
                DisplayError(ErrorNoFields);
                return;
            }

            DetailsDownload();
        }

        protected void btnCloseExport_Click(object sender, EventArgs e)
        {
            ResetPopupControls();
            HideDownloadPopup.DynamicInvoke();
            MdlDownloads.Hide();
        }

        protected virtual void ResetPopupControls()
        {
            TxtDownloadCount.Text = string.Empty;
            TxtDownloadUniqueCount.Text = string.Empty;

            TxtPromocode.Text = string.Empty;
            RbDownload.Visible = true;
            RbDownload.Checked = true;
            DvDownloads.Style.Remove(HtmlAttributesHeight);
            DvDownloads.Style.Remove(HtmlAttributeOverflow);
            PhShowHeader.Visible = false;
            CbShowHeader.Checked = false;
            HfDownloadTemplateID.Value = HtmlIdZero;

            DgDataCompareResult.DataSource = null;
            DgDataCompareResult.DataBind();
            PlDataCompareResult.Visible = false;
            LblDataCompareMessage.Text = string.Empty;
            DrpIsBillable.SelectedIndex = IndexNoneSelected;
            BtnExport.OnClientClick = null;
            PlNotes.Visible = false;
            TxtNotes.Text = string.Empty;
        }

        public void DisplayError(string errorMessage)
        {
            LblErrorMessage.Text = errorMessage;
            DivError.Visible = true;
        }
    }
}
