using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;

[assembly: TagPrefix("KM.Common.UserControls", "NestableGridView")]

namespace KM.Common.UserControls
{
    public class NestableGridView : SearchGridView
    {
        #region Controls and constants
        GridViewRow PagerRow;
        //To hold the ID and alignment of the child control
        private const string CHILD_TABLE_ID = "ChildTableID";
        private const string CHILD_TABLE_ALIGN = "ChildTableAlign";
        #endregion

        #region Constructor
        // Constructor
        public NestableGridView()
            : base()
        {
        }
        #endregion

        #region properties
        [Category("Behaviour")]
        [Bindable(BindableSupport.No)]
        public string ChildTableID
        {
            get
            {
                if (this.ViewState[CHILD_TABLE_ID] == null)
                {
                    this.ViewState[CHILD_TABLE_ID] = String.Empty;
                }

                return (string)this.ViewState[CHILD_TABLE_ID];
            }
            set
            {
                this.ViewState[CHILD_TABLE_ID] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(HorizontalAlign.Center)]
        public HorizontalAlign ChildTableAlign
        {
            get
            {
                if (this.ViewState[CHILD_TABLE_ALIGN] == null)
                {
                    this.ViewState[CHILD_TABLE_ALIGN] = HorizontalAlign.Center;
                }

                return (HorizontalAlign)this.ViewState[CHILD_TABLE_ALIGN];
            }
            set
            {
                this.ViewState[CHILD_TABLE_ALIGN] = value;
            }
        }
        #endregion

        #region overridden functions
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);
            if (DesignMode)
                return;
            //Get a handle for the pager row
            if (e.Row.RowType == DataControlRowType.Pager)
                PagerRow = e.Row;
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            //If there are no rows to be rendered
            if (DesignMode || Rows.Count == 0)
            {
                base.RenderContents(writer);
                return;
            }

            //Render the begin tag <table>
            base.RenderBeginTag(writer);
            //Render header row
            if (HeaderRow != null)
                HeaderRow.RenderControl(writer);
            foreach (GridViewRow row in Rows)
            {
                Control childCtl = row.FindControl(ChildTableID);
                //Render child row by custom method if the child control is visible
                if (childCtl != null && childCtl.Visible)
                {
                    row.RenderBeginTag(writer);
                    TableCell childCell = null;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (cell.Controls.IndexOf(childCtl) == -1)
                            cell.RenderControl(writer);
                        else
                        {
                            //If this is the child cell close the table cell
                            writer.Write("<td></td>");
                            childCell = cell;
                        }
                    }
                    //Child control needs to be rendered in the next row
                    if (childCell != null)
                    {
                        writer.Write("</tr>");
                        writer.Write("<tr>");
                        childCell.ColumnSpan = row.Cells.Count;
                        childCell.Attributes.Add("align", ChildTableAlign.ToString());
                        childCell.RenderControl(writer);
                    }

                    row.RenderEndTag(writer);
                }
                else
                {
                    //If the child control is not visible, render thr row
                    row.RenderControl(writer);
                }
            }
            //Render Footer Row
            if (FooterRow != null)
                FooterRow.RenderControl(writer);
            //Render Pager Row
            if (PagerRow != null)
                PagerRow.RenderControl(writer);
            base.RenderEndTag(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (DesignMode)
                return;
            //Assign the image url for search button
            _btnSearch.ImageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType().BaseType, "KM.WebControls.search.bmp");
            _btnAdd.ImageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType().BaseType, "KM.WebControls.new.gif");
        }

        protected override void OnSelectedIndexChanging(GridViewSelectEventArgs e)
        {
            base.OnSelectedIndexChanging(e);
            if (DesignMode)
                return;
            if (Rows[e.NewSelectedIndex] != null && Rows[e.NewSelectedIndex].FindControl(ChildTableID) != null)
            {
                Control childCtl = Rows[e.NewSelectedIndex].FindControl(ChildTableID);
                if (childCtl != null)
                    childCtl.Visible = !childCtl.Visible;
            }
        }
        #endregion
    }
}
