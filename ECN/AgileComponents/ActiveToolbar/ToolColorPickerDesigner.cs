using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.ActiveToolbar;

namespace ActiveUp.WebControls
{
    #region class ToolColorPickerDesigner

    /// <summary>
    /// Designer of the <see cref="ToolColorPickerDesigner"/> control.
    /// </summary>
    [Serializable]
    public class ToolColorPickerDesigner : CoreControlDesigner
    {
        #region Constrcutor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ToolColorPickerDesigner()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the control can be resized.
        /// </summary>
        public override bool AllowResize
        {
            get { return false; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the HTML that is used to represent the control at design time.
        /// </summary>
        /// <returns>The HTML that is used to represent the control at design time.</returns>
        public override string GetDesignTimeHtml()
        {
            try
            {
                ToolColorPicker colorPicker = (ToolColorPicker)base.Component;

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

                DesignColorPicker(ref writer, colorPicker);

                /*StreamWriter sw = new StreamWriter(@"c:\temp\render.txt", false);
                sw.Write(stringWriter.ToString());
                sw.Close();*/

                return stringWriter.ToString();

            }

            catch (Exception e)
            {
                return this.GetErrorDesignTimeHtml(e);
            }
        }

        /// <summary>
        /// Gets the HTML that provides information about the specified exception. This method is typically called after an error has been encountered at design time.
        /// </summary>
        /// <param name="e">The exception that occurred.</param>
        /// <returns>The HTML for the specified exception.</returns>
        protected override string GetErrorDesignTimeHtml(System.Exception e)
        {
            string text = string.Format("There was an error and the ColorPicker control can't be displayed<br>Exception : {0}", e.Message);
            return this.CreatePlaceHolderDesignTimeHtml(text);
        }

        /// <summary>
        /// Create a DropDownList object at design time.
        /// </summary>
        /// <param name="output">Output stream that contains the HTML used to represent the control.</param>
        /// <param name="colorPicker"><see cref="ToolColorPicker"/> object to design.</param>
        /// 
        public static void DesignColorPicker(ref HtmlTextWriter output, ToolColorPicker colorPicker)
        {
            IEnumerator enumerator = colorPicker.Style.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                output.AddStyleAttribute((string)enumerator.Current, colorPicker.Style[(string)enumerator.Current]);
            }
            colorPicker.ControlStyle.AddAttributesToRender(output);


            string backImage = string.Empty;
            if (colorPicker.BackImage != string.Empty)
            {
                if (colorPicker.Parent != null && colorPicker.Parent is ActiveUp.WebControls.Toolbar)
                    backImage = "url(" + Utils.ConvertToImageDir(((Toolbar)colorPicker.Parent).ImagesDirectory, colorPicker.BackImage) + ")";
                else
                    backImage = "url(" + colorPicker.BackImage + ")";
            }

            string style = "style=\"";
            if (colorPicker.BackColor != Color.Empty)
                style += string.Format("background-color:{0};", Utils.Color2Hex(colorPicker.BackColor));
            if (colorPicker.BorderColor != Color.Empty)
                style += string.Format("border-color:{0};", Utils.Color2Hex(colorPicker.BorderColor));
            style += string.Format("border-style:{0};", colorPicker.BorderStyle.ToString());
            if (colorPicker.BorderWidth != Unit.Empty)
                style += string.Format("border-width:{0};", colorPicker.BorderWidth);
            if (backImage != string.Empty)
                style += string.Format("background-image:{0};", backImage);
            style += "\"";

            string table = string.Empty;
            table += "<table";
            table += string.Format(" id={0}_ddl", colorPicker.ClientID);
            if (colorPicker.Width != Unit.Empty)
                table += string.Format(" width={0}", colorPicker.Width.ToString());
            if (colorPicker.Height != Unit.Empty)
                table += string.Format(" height={0}", colorPicker.Height.ToString());
            table += " cellpadding=0 cellspacing=0";
            table += string.Format(" {0}", style);
            table += ">";
            output.Write(table);
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.Write("\n");

            if (colorPicker.Width.IsEmpty == true)
                output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            else
                output.AddAttribute(HtmlTextWriterAttribute.Width, colorPicker.Width.Value.ToString());
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            output.RenderBeginTag(HtmlTextWriterTag.Table);
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Id, colorPicker.ClientID + "_text");
            if (colorPicker.UseSquareColor)
                output.AddStyleAttribute("padding", "2px 2px 2px 2px");
            else
                output.AddStyleAttribute("padding", "0px 0px 0px 4px");
            output.AddAttribute(HtmlTextWriterAttribute.Nowrap, null);
            if (colorPicker.ForeColor != Color.Empty)
                output.AddStyleAttribute(HtmlTextWriterStyle.Color, Utils.Color2Hex(colorPicker.ForeColor));
            Utils.AddStyleFontAttribute(output, colorPicker.Font);
            output.RenderBeginTag(HtmlTextWriterTag.Td);

            if (colorPicker.UseSquareColor)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Id, colorPicker.ClientID + "_squareColor");
                output.AddAttribute(HtmlTextWriterAttribute.Width, "14px");
                output.AddAttribute(HtmlTextWriterAttribute.Height, "14px");
                output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(colorPicker.SelectedColor));
                output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(Color.Silver));
                output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
                output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px");
                output.RenderBeginTag(HtmlTextWriterTag.Table);
                output.RenderBeginTag(HtmlTextWriterTag.Tr);
                output.RenderBeginTag(HtmlTextWriterTag.Td);
                output.RenderEndTag();
                output.RenderEndTag();
                output.RenderEndTag();
            }
            else
            {
                //output.Write(string.Format("<img src='{0}'>", Utils.ConvertToImageDir(((Toolbar)colorPicker.Parent).ImagesDirectory, colorPicker.Image,"color.gif",colorPicker.Page,colorPicker.GetType())));
                output.Write(string.Format("<img src='{0}'>", Utils.ConvertToImageDir(((Toolbar)colorPicker.Parent).ImagesDirectory, colorPicker.Image, "color.gif", colorPicker.Page, colorPicker.GetType())));
            }
            output.RenderEndTag();
            output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
            output.RenderBeginTag(HtmlTextWriterTag.Td);

            // drop down image

            output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
            // Added by PMENGAL

            output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.RenderBeginTag(HtmlTextWriterTag.Table);
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            if (colorPicker.DropDownImage != string.Empty)
            {
                if (colorPicker.Parent != null && colorPicker.Parent is ActiveUp.WebControls.Toolbar)
                    output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(((Toolbar)colorPicker.Parent).ImagesDirectory, colorPicker.DropDownImage, "down.gif", colorPicker.Page, colorPicker.GetType()));
                else
                    output.AddAttribute(HtmlTextWriterAttribute.Src, colorPicker.DropDownImage);
            }
            else
            {
                if (colorPicker.Parent != null && colorPicker.Parent is ActiveUp.WebControls.Toolbar)
#if (!FX1_1)
                output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(((Toolbar)colorPicker.Parent).ImagesDirectory, "", "down.gif", colorPicker.Page, colorPicker.GetType()));
#else
                output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(((Toolbar)colorPicker.Parent).ImagesDirectory, "down.gif"));
#endif
            else
                    output.AddAttribute(HtmlTextWriterAttribute.Src, "down.gif");
            }

            output.RenderBeginTag(HtmlTextWriterTag.Img);
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();

            output.Write("</table>");

            /*style = string.Empty;
            style += "style=\"";
            if (WindowBorderColor != Color.Empty)
                style += string.Format("border-color:{0};", Utils.Color2Hex(this.WindowBorderColor));
            style += string.Format("border-style:{0};", WindowBorderStyle.ToString());
            if (WindowBorderWidth != Unit.Empty)
                style += string.Format("border-width:{0};", WindowBorderWidth.ToString());
            if (BackColorItems != Color.Empty)
                style += string.Format("background-color:{0};", Utils.Color2Hex(BackColorItems));
            style += "display:none;cursor:pointer;cursor: hand;";
            style += "\"";

            table = string.Empty;
            table += "<table";
            table += string.Format(" id={0}_items", ClientID);
            table += " cellpadding=0 cellspacing=0";
            table += " onmouseenter=\"this.style.display='';\"";
            table += string.Format(" onmouseleave=ATB_closeDropDownList('{0}')", ClientID);
            table += string.Format(" {0}", style);
            table += ">";

            
            output.Write(table);
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.RenderBeginTag(HtmlTextWriterTag.Td);

            
            output.Write(string.Format("<div id=\"{0}\" style=\"overflow:auto;height:{1};\">", ClientID + "_divItems", (ItemsAreaHeight != Unit.Empty ? ItemsAreaHeight.ToString() : "120")));
            if (Cellpadding != Unit.Empty)
                output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, this.Cellpadding.ToString());
            else
                output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            if (Cellspacing != Unit.Empty)
                output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, this.Cellspacing.ToString());
            else
                output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_tableItems");
            output.RenderBeginTag(HtmlTextWriterTag.Table); // Open Table
            RenderItems(output);
            output.RenderEndTag(); // Close Table
            
            output.Write("</div>");
            output.RenderEndTag();
            output.RenderEndTag();
            
            output.Write("</table>");*/
        }


        #endregion

    }

    #endregion
}
