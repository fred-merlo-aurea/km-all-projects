using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.communicator.contentmanager.ckeditor.dialog
{
    public partial class SocialShare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            DataTable socialDT = DataFunctions.GetDataTable("select * from socialmedia where canshare = 'true' and isactive = 'true' and SocialMediaID <> 4");
            foreach (DataRow row in socialDT.Rows)
            {
                //add rows with controls
                TableRow trow = new TableRow();
                tblMedia.Rows.Add(trow);
                TableCell tcellIcon = new TableCell();
                tcellIcon.VerticalAlign = VerticalAlign.Middle;
                tcellIcon.HorizontalAlign = HorizontalAlign.Center;
                tcellIcon.Width = System.Web.UI.WebControls.Unit.Pixel(50);
                trow.Cells.Add(tcellIcon);
                TableCell tcellCheckBox = new TableCell();
                tcellCheckBox.VerticalAlign = VerticalAlign.Middle;
                tcellCheckBox.HorizontalAlign = HorizontalAlign.Left;
                tcellCheckBox.Width = System.Web.UI.WebControls.Unit.Pixel(150);
                trow.Cells.Add(tcellCheckBox);
                TableCell tcellMatchString = new TableCell();
                trow.Cells.Add(tcellMatchString);
                Image imgMedia = new Image();
                imgMedia.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + row["ImagePath"].ToString();
                imgMedia.AlternateText = row["DisplayName"].ToString();
                tcellIcon.Controls.Add(imgMedia);
                CheckBox cbxMedia = new CheckBox();
                cbxMedia.Text = row["DisplayName"].ToString();
                cbxMedia.CssClass = "formfield";
                tcellCheckBox.Controls.Add(cbxMedia);
                Label lblMedia = new Label();
                lblMedia.Text = row["MatchString"].ToString();
                lblMedia.Visible = false;
                tcellMatchString.Controls.Add(lblMedia);
            }
            
           
        }

        protected void chkEnable_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //old working without fieldset and title
            //string matchStrings = string.Empty;
            //foreach (TableRow trow in tblMedia.Rows)
            //{
            //    CheckBox tempCheckbox = (CheckBox)trow.Cells[1].Controls[0];
            //    if (tempCheckbox.Checked)
            //    {
            //        Label tempLabel = (Label)trow.Cells[2].Controls[0];
            //        Image tempImage = (Image)trow.Cells[0].Controls[0];
            //        //matchStrings += " " + tempLabel.Text;
            //        matchStrings += " <a href=\"" + tempLabel.Text + "\"><img border=\"0\" src=\"" + tempImage.ImageUrl + "\" /></a>&nbsp";
            //        //<a href="$$ECN_TwitterLink$$"><img border="0" src="http://images.ecn5.com/images/SocialIcons/twitter.jpg" /></a>&nbsp
            //    }
            //}
            //hfMedia.Value = matchStrings;
            //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ok()", true);

            
            //int tablewidth = 40 * tblMedia.Rows.Count + 35;
            //string matchStrings = "<div id=\"sharelinks\" style=\"width: 100%; height: 50px;\"><fieldset><legend>Share Message</legend><table><tr>";
            string matchStrings = "<table><tr>";
            foreach (TableRow trow in tblMedia.Rows)
            {
                CheckBox tempCheckbox = (CheckBox)trow.Cells[1].Controls[0];
                if (tempCheckbox.Checked)
                {
                    Label tempLabel = (Label)trow.Cells[2].Controls[0];
                    Image tempImage = (Image)trow.Cells[0].Controls[0];
                    //matchStrings += " " + tempLabel.Text;
                    matchStrings += "<td><a href=\"" + tempLabel.Text + "\"><img border=\"0\" src=\"" + tempImage.ImageUrl + "\" /></a>&nbsp</td>";
                }
            }
            matchStrings += "</tr></table>";
            hfMedia.Value = matchStrings;
            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ok()", true);
        }

    }
}