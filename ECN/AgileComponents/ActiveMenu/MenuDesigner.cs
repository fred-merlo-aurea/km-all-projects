using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Collections;
using System.Drawing;

namespace ActiveUp.WebControls
{
    #region MenuDesigner

    /// <summary>
    /// Designer of the <see cref="MenuDesigner"/> control.
    /// </summary>
    [Serializable]
    public class MenuDesigner : ControlDesigner
    {
        #region Constrcutor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MenuDesigner()
        {
        }

        #endregion

        #region Methods

        private DesignerVerbCollection designerVerbs;

		/// <summary>
		/// Gets the design-time verbs supported by the component that is associated with the designer.
		/// </summary>
		/// <value></value>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (designerVerbs == null)
                {
                    designerVerbs = new DesignerVerbCollection();
                    designerVerbs.Add(new DesignerVerb("Property Builder...", new EventHandler(this.OnPropertyBuilder)));
                }

                return designerVerbs;
            }
        }

        private void OnPropertyBuilder(object sender, EventArgs e)
        {
            MenuComponentEditor compEditor = new MenuComponentEditor();
            compEditor.EditComponent(Component);
        }

		/// <summary>
		/// Initializes the designer and
		/// loads the specified component.
		/// </summary>
		/// <param name="component">The control element being designed.</param>
        public override void Initialize(IComponent component)
        {
            if (!(component is ActiveUp.WebControls.Menu))
            {
                throw new ArgumentException("Component must be a ActiveUp.WebControls.Menu control.", "component");
            }
            base.Initialize(component);
        }

        /// <summary>
        /// Gets the HTML that is used to represent the control at design time.
        /// </summary>
        /// <returns>The HTML that is used to represent the control at design time.</returns>
        public override string GetDesignTimeHtml()
        {
            try
            {
                Menu menu = (Menu)base.Component;

                if (menu.MenuItems.Count == 0)
                    return CreatePlaceHolderDesignTimeHtml("Please add items through the MenuItems property in the property pane.");

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter output = new HtmlTextWriter(stringWriter);

                output.RenderBeginTag(HtmlTextWriterTag.Div);
                if (menu.CssClass == string.Empty)
                {
                    string style = string.Empty;
                    if (menu.BackImage != string.Empty)
                        //style = string.Format("background-image:url({0});",Utils.ConvertToImageDir(toolMenu.ImagesDirectory,toolMenu.BackImage));
                        output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, Utils.ConvertToImageDir(menu.ImagesDirectory, menu.BackImage));

                    if (menu.BackColor != Color.Empty)
                    {
                        //style = string.Format("background-color : {0};",Utils.Color2Hex(toolMenu.BackColor));
                        output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(menu.BackColor));
                    }

                    if (menu.BorderStyle != BorderStyle.NotSet)
                    {
                        //style += string.Format("border-style : {0};",toolMenu.BorderStyle.ToString());
                        //style += string.Format("border-width : {0};",toolMenu.BorderWidth);
                        //style += string.Format("border-color : {0};",Utils.Color2Hex(toolMenu.BorderColor));

                        output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, menu.BorderStyle.ToString());
                        output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, menu.BorderWidth.ToString());
                        output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(menu.BorderColor));
                    }

                    if (menu.ForeColor != Color.Empty)
                    {
                        //style += string.Format("color : {0};\n",Utils.Color2Hex(toolMenu.ForeColor));
                        output.AddStyleAttribute(HtmlTextWriterStyle.Color, Utils.Color2Hex(menu.ForeColor));
                    }
                    //output.WriteAttribute("style",style);
                }
                else
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Class, menu.CssClass);
                }

                output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, menu.CellPadding.ToString());
                output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, menu.CellSpacing.ToString());
                // open table (_menu)
                output.RenderBeginTag(HtmlTextWriterTag.Table);

                output.RenderBeginTag(HtmlTextWriterTag.Tr);

                RenderMenuItems(output, menu);

                output.RenderEndTag();

                // close table (_menu)
                output.RenderEndTag();

                // close div (block)
                output.RenderEndTag();

                return stringWriter.ToString();

            }

            catch (Exception e)
            {
                return this.GetErrorDesignTimeHtml(e);
            }
        }

        private void RenderMenuItems(HtmlTextWriter output, Menu toolMenu)
        {
            foreach (MenuItem toolItem in toolMenu.MenuItems)
            {
                if (toolItem.Width != Unit.Empty)
                    output.AddAttribute(HtmlTextWriterAttribute.Width, toolItem.Width.ToString());
                if (toolItem.Height != Unit.Empty)
                    output.AddAttribute(HtmlTextWriterAttribute.Height, toolItem.Height.ToString());
                // open td
                output.RenderBeginTag(HtmlTextWriterTag.Td);

                if (toolMenu.StyleMenuItems.CssClass == string.Empty)
                {
                    string style = string.Empty;

                    if (toolMenu.StyleMenuItems.BackColor != Color.Empty)
                        output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(toolMenu.StyleMenuItems.BackColor));
                    //style += string.Format("background-color: {0};",Utils.Color2Hex(toolMenu.StyleMenuItems.BackColor));
                    if (toolMenu.StyleMenuItems.BorderStyle != BorderStyle.NotSet)
                    {
                        //style += string.Format("border-style : {0};",toolMenu.StyleMenuItems.BorderStyle.ToString());
                        //style += string.Format("border-width : {0};",toolMenu.StyleMenuItems.BorderWidth);
                        //style += string.Format("border-color : {0};",Utils.Color2Hex(toolMenu.StyleMenuItems.BorderColor));
                        output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, toolMenu.StyleMenuItems.BorderStyle.ToString());
                        output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, toolMenu.StyleMenuItems.BorderWidth.ToString());
                        output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(toolMenu.StyleMenuItems.BorderColor));
                    }

                    if (toolMenu.StyleMenuItems.ForeColor != Color.Empty)
                        //style += string.Format("color : {0};",Utils.Color2Hex(toolMenu.StyleMenuItems.ForeColor));
                        output.AddStyleAttribute(HtmlTextWriterStyle.Color, Utils.Color2Hex(toolMenu.StyleMenuItems.ForeColor));

                    if (toolMenu.StyleMenuItems.AllowRollOver && toolMenu.StyleMenuItems.BorderStyleOver != BorderStyle.NotSet && toolMenu.StyleMenuItems.BorderWidthOver.Value > 0)
                    {
                        int margin = 0;
                        if (toolMenu.StyleMenuItems.BorderWidth.Value > toolMenu.StyleMenuItems.BorderWidthOver.Value)
                            margin = 0;
                        else if (toolMenu.StyleMenuItems.BorderWidth.Value < toolMenu.StyleMenuItems.BorderWidthOver.Value)
                            margin = (int)(toolMenu.StyleMenuItems.BorderWidthOver.Value - toolMenu.StyleMenuItems.BorderWidth.Value);
                        else
                            margin = 0;

                        margin += (int)toolMenu.StyleMenuItems.Margin.Value;

                        //style += string.Format("margin : {0}px;",margin);
                        output.AddStyleAttribute("margin", margin.ToString());
                    }
                    else
                        //style += string.Format("margin : {0}px;",toolMenu.StyleMenuItems.Margin);
                        output.AddStyleAttribute("margin", toolMenu.StyleMenuItems.Margin.ToString());

                    //output.AddAttribute("style",style);
                }
                else
                    output.AddAttribute(HtmlTextWriterAttribute.Class, toolMenu.StyleMenuItems.CssClass);

                if (toolItem.Align != HorizontalAlign.NotSet)
                    output.AddAttribute(HtmlTextWriterAttribute.Align, toolItem.Align.ToString());

                output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
                output.RenderBeginTag(HtmlTextWriterTag.Table);
                output.RenderBeginTag(HtmlTextWriterTag.Tr);

                if (toolItem.Image != string.Empty)
                {
                    output.RenderBeginTag(HtmlTextWriterTag.Td);
                    output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(toolMenu.ImagesDirectory, toolItem.Image));
                    output.RenderBeginTag(HtmlTextWriterTag.Img);
                    output.RenderEndTag();
                }

                output.AddAttribute(HtmlTextWriterAttribute.Align, toolItem.Align.ToString());
                output.RenderBeginTag(HtmlTextWriterTag.Td);
                if (toolItem.Text != string.Empty)
                {
                    output.Write(toolItem.Text);
                }
                output.RenderEndTag();
                output.RenderEndTag();
                output.RenderEndTag();
                //close td
                output.RenderEndTag();
            }
        }

        /// <summary>
        /// Gets the HTML that provides information about the specified exception. This method is typically called after an error has been encountered at design time.
        /// </summary>
        /// <param name="e">The exception that occurred.</param>
        /// <returns>The HTML for the specified exception.</returns>
        protected override string GetErrorDesignTimeHtml(System.Exception e)
        {
            string text = string.Format("There was an error and the ToolMenu control can't be displayed<br>Exception : {0}", e.Message);
            return this.CreatePlaceHolderDesignTimeHtml(text);
        }

		/// <summary>
		/// Gets a value
		/// indicating whether the control can be resized.
		/// </summary>
		/// <value></value>
        public override bool AllowResize
        {
            get { return false; }
        }

        #endregion

    }

    #endregion
}
