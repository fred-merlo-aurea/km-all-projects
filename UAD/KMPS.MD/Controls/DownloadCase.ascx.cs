using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMPS.MD.Controls
{
    public partial class DownloadCase : System.Web.UI.UserControl
    {
        public Delegate hideDownloadCasePopup;
        public Delegate LoadEditCaseData;
        public event EventHandler CausePostBack;
        public Dictionary<string, string> DownloadFields
        {
            get
            {
                try
                {
                    return (Dictionary<string, string>)ViewState["DownloadFields"];
                }
                catch
                {
                    return new Dictionary<string, string>();
                }
            }
            set
            {
                ViewState["DownloadFields"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.mpeCase.Show();
        }

        public void loadControls()
        {
            drpFileFormat.SelectedIndex = -1;

            dlDownloadFields.DataSource = DownloadFields;
            dlDownloadFields.DataBind();
            string fileFormatCase = string.Empty;
            string val = string.Empty;
            string selectedVal = string.Empty;
            int i = 0;

            foreach (DataListItem di in dlDownloadFields.Items)
            {
                HiddenField hfDownloadFieldText = (HiddenField)di.FindControl("hfDownloadFieldText");
                DropDownList drpDownloadFieldCase = (DropDownList)di.FindControl("drpDownloadFieldCase");

                selectedVal = DownloadFields.FirstOrDefault(x => x.Key.ToUpper() == hfDownloadFieldText.Value.ToUpper()).Key.Split('|')[2];

                i++;

                if (i == 1)
                {
                    fileFormatCase = selectedVal;
                }

                if(fileFormatCase != selectedVal)
                {
                    val = "Custom";
                }

                drpDownloadFieldCase.SelectedIndex = -1;

                if (drpDownloadFieldCase.Items.FindByValue(selectedVal) != null)
                {
                    drpDownloadFieldCase.Items.FindByValue(selectedVal).Selected = true;
                }
            }

            if(val == string.Empty)
            {
                try
                {
                    drpFileFormat.Items.FindByValue(selectedVal).Selected = true;
                }
                catch
                {

                }
            }
            else
            {
                if (!drpFileFormat.Items.Cast<ListItem>().Any(item => item.Value == "Custom"))
                {
                    drpFileFormat.Items.Add(new ListItem("Custom", "Custom"));
                }

                drpFileFormat.Items.FindByValue("Custom").Selected = true;
            }
        }

        protected void btnSelectCase_Click(object sender, EventArgs e)
        {
            if (drpFileFormat.Items.Cast<ListItem>().Any(item => item.Value == "Custom"))
            {
                drpFileFormat.Items.Remove(new ListItem("Custom", "Custom"));
            }

            Dictionary<string, string> downloadfields = new Dictionary<string, string>();

            foreach (DataListItem di in dlDownloadFields.Items)
            {
                Label lblDownloadFieldName = (Label)di.FindControl("lblDownloadFieldName");
                HiddenField hfDownloadFieldText = (HiddenField)di.FindControl("hfDownloadFieldText");
                DropDownList drpDownloadFieldCase = (DropDownList)di.FindControl("drpDownloadFieldCase");
                downloadfields.Add(hfDownloadFieldText.Value, drpDownloadFieldCase.SelectedValue);
            }

            LoadEditCaseData.DynamicInvoke(downloadfields);
            CausePostBack(sender, e);
            hideDownloadCasePopup.DynamicInvoke();
            this.mpeCase.Hide();
        }

        protected void btnCancelCase_Click(object sender, EventArgs e)
        {
            Reset();
            hideDownloadCasePopup.DynamicInvoke();
            this.mpeCase.Hide();
        }

        protected void drpFileFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = drpFileFormat.SelectedValue;

            if (selectedValue == "Custom")
            {
                drpFileFormat.SelectedIndex = -1;
                drpFileFormat.Items.FindByValue(hfFileFormat.Value).Selected = true;
            }
            else
            {
                hfFileFormat.Value = drpFileFormat.SelectedValue;

                foreach (DataListItem di in dlDownloadFields.Items)
                {
                    DropDownList drpDownloadFieldCase = (DropDownList)di.FindControl("drpDownloadFieldCase");
                    drpDownloadFieldCase.SelectedIndex = -1;

                    if (drpDownloadFieldCase.Items.FindByValue(selectedValue) != null)
                    {
                        drpDownloadFieldCase.Items.FindByValue(selectedValue).Selected = true;
                    }
                }
            }
        }

        protected void drpDownloadFieldCase_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddlList = (DropDownList)sender;
            var row = (DataListItem)ddlList.NamingContainer;
            string selectedValue = ((DropDownList)row.FindControl("drpDownloadFieldCase")).SelectedValue;

            foreach (DataListItem di in dlDownloadFields.Items)
            {
                DropDownList drpDownloadFieldCase = (DropDownList)di.FindControl("drpDownloadFieldCase");
                if (drpDownloadFieldCase.SelectedValue != selectedValue)
                {
                    drpFileFormat.SelectedIndex = -1;

                    if (!drpFileFormat.Items.Cast<ListItem>().Any(item => item.Value == "Custom"))
                    {
                        drpFileFormat.Items.Add(new ListItem("Custom", "Custom"));
                    }

                    drpFileFormat.Items.FindByValue("Custom").Selected = true;
                    return;
                }
            }

            if (drpFileFormat.Items.Cast<ListItem>().Any(item => item.Value == "Custom"))
            {
                drpFileFormat.Items.Remove(new ListItem("Custom", "Custom"));
            }

            drpFileFormat.SelectedIndex = -1;
            drpFileFormat.Items.FindByValue(selectedValue).Selected = true;
        }

        protected void Reset()
        {
            if (drpFileFormat.Items.Cast<ListItem>().Any(item => item.Value == "Custom"))
            {
                drpFileFormat.Items.Remove(new ListItem("Custom", "Custom"));
            }

            foreach (DataListItem di in dlDownloadFields.Items)
            {
                DropDownList drpDownloadFieldCase = (DropDownList)di.FindControl("drpDownloadFieldCase");
                drpDownloadFieldCase.SelectedIndex = -1;
            }
        }
    }
}