using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Specialized;
using System.IO;

namespace ecn.controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:BoxPanel runat=server></{0}:BoxPanel>")]
    public class BoxPanel : Panel
    {

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Title
        {
            get
            {
                String s = (String)ViewState["Title"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Title"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Layout
        {
            get
            {
                String s = (String)ViewState["Layout"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Layout"] = value;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            // The internal panel must cover 100% of the parent area
            // irrespective of the physical width. We change this here
            // so that the original Panel code doesn't reflect the

            // Capture the default output of the Panel
            StringWriter writer = new StringWriter();
            HtmlTextWriter buffer = new HtmlTextWriter(writer);
            base.Render(buffer);
            string panelOutput = writer.ToString();

            // Restore the wanted width because this affects the outer table
            BuildControlTree(output, this.ClientID, panelOutput);
            return;
        }

        private void BuildControlTree(HtmlTextWriter output, string id, string panelOutput)
        {
            Table t = new Table();
            t.ID = "Box_" + System.Guid.NewGuid().ToString().Substring(0, 5);
            t.CellPadding = 0;
            t.CellSpacing = 0;
            t.Width = Width;
            t.HorizontalAlign = HorizontalAlign.Center;

            // Prepare the topmost row
            TableRow rowTop = new TableRow();

            if (!Layout.Equals("TABLE", StringComparison.OrdinalIgnoreCase))
            {
                // Leftmost cell
                TableCell topleftCell = new TableCell();
                topleftCell.HorizontalAlign = HorizontalAlign.Left;
                topleftCell.VerticalAlign = VerticalAlign.Top;
                topleftCell.Width = Unit.Pixel(1);
                topleftCell.Height = Unit.Pixel(25);
                topleftCell.Text = "<img src='" + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath) + "/Images/" + "boxupperleft.gif'>";
                rowTop.Cells.Add(topleftCell);
            }

            //Label Info
            TableCell topcenterCell = new TableCell();
            topcenterCell.CssClass = "boxtitle ";
            topcenterCell.Height = Unit.Pixel(25);

            if (Layout.Equals("TABLE", StringComparison.OrdinalIgnoreCase))
                topcenterCell.ColumnSpan = 3;
            
            Label lbl = new Label();
            //lbl.CssClass = "boxtitletext";
            //lbl.Attributes.Add("style", "padding-top:5px;background:red;height:33px");
            lbl.Text = String.Format("&nbsp;{0}", Title);

            topcenterCell.Controls.Add(lbl);
            rowTop.Cells.Add(topcenterCell);

            if (!Layout.Equals("TABLE", StringComparison.OrdinalIgnoreCase))
            {
                // Right most cell
                TableCell toprightCell = new TableCell();
                toprightCell.HorizontalAlign = HorizontalAlign.Right;
                toprightCell.VerticalAlign = VerticalAlign.Top;
                toprightCell.Width = Unit.Pixel(1);
                toprightCell.Height = Unit.Pixel(25);
                toprightCell.Text = "<img src='" + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath) + "/Images/" + "boxupperright.gif'>";
                rowTop.Cells.Add(toprightCell);
            }
            // Add the top row to the table
            t.Rows.Add(rowTop);

            // Prepare the topmost row
            TableRow rowmiddle = new TableRow();
            // Middle Row
            TableCell middleleftCell = new TableCell();
            //middleleftCell.Height = Unit.Pixel(100);
            middleleftCell.Width = Unit.Pixel(1);
            middleleftCell.CssClass = "boxleftside";
            rowmiddle.Cells.Add(middleleftCell);

            //Label Info
            TableCell middlecenterCell = new TableCell();
            middlecenterCell.BackColor = Color.White;
            middlecenterCell.Text = panelOutput;
            rowmiddle.Cells.Add(middlecenterCell);

            // Right most cell
            TableCell middlerightCell = new TableCell();
            middlerightCell.HorizontalAlign = HorizontalAlign.Right;

            //middlerightCell.Height = Unit.Pixel(100);
            middlerightCell.CssClass = "boxrightside";
            rowmiddle.Cells.Add(middlerightCell);

            // Add the middle row to the table
            t.Rows.Add(rowmiddle);

            // bottom Row
            TableRow rowbottom = new TableRow();

            TableCell bottomleftCell = new TableCell();
            bottomleftCell.Height = Unit.Pixel(6);
            bottomleftCell.Width = Unit.Pixel(1);
            if (!Layout.Equals("TABLE", StringComparison.OrdinalIgnoreCase))
            {
                bottomleftCell.Text = "<img src='" + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath) + "/Images/" + "boxbottomleft.gif'>";
            }
            else
            {
                bottomleftCell.CssClass = "boxbottom boxleftside";
            }
            rowbottom.Cells.Add(bottomleftCell);

            //Label Info
            TableCell bottomcenterCell = new TableCell();
            bottomcenterCell.CssClass = "boxbottom";
            bottomcenterCell.Text = "<img src='" + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath) + "/Images/" + "spacer.gif'>";
            rowbottom.Cells.Add(bottomcenterCell);

            // Right most cell
            TableCell bottomrightCell = new TableCell();
            bottomrightCell.HorizontalAlign = HorizontalAlign.Right;
            bottomrightCell.Height = Unit.Pixel(6);
            if (!Layout.Equals("TABLE", StringComparison.OrdinalIgnoreCase))
            {
                bottomrightCell.Text = "<img src='" + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath) + "/Images/" + "boxbottomright.gif'>";
            }
            else
            {
                bottomrightCell.CssClass = "boxbottom boxrightside";
            }
            rowbottom.Cells.Add(bottomrightCell);

            // Add the bottom row to the table
            t.Rows.Add(rowbottom);

            // Output
            t.RenderControl(output);
        }
    }
}
