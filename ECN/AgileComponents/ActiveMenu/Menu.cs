using System;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Xml.Serialization;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
    #region class ToolMenu

	/// <summary>
	/// Represents a <see cref="Menu"/> object.
	/// </summary>
    [
        ToolboxData("<{0}:Menu runat=server></{0}:Menu>"),
        Designer(typeof(MenuDesigner)),
        //Editor(typeof(ActiveUp.WebControls.ToolMenuComponentEditor), typeof(ComponentEditor)),
        ParseChildren(true, "MenuItems"),
		ToolboxBitmap(typeof(Menu), "ToolBoxBitmap.Menu.bmp"),
        Serializable,
    ]
    public class Menu : CoreWebControl, IPostBackDataHandler, IPostBackEventHandler
    {
        #region Variables

        /// <summary>
        /// Client side script block.
        /// </summary>
        private string CLIENTSIDE_API = null;

        /// <summary>
        /// Unique client script key.
        /// </summary>
        private const string SCRIPTKEY = "ActiveMenu";

        private MenuItemCollection _menuItems;
        private MenuItemStyle _styleMenuItems = new MenuItemStyle();

		private DataSourceOrder _sortOrder = DataSourceOrder.Ascending;
		private DataTable _dataSource;
		private string _dataText = string.Empty ,  _dataValue = string.Empty , _dataImage = string.Empty , _dataImageOver = string.Empty ,  _dataNavigateUrl = string.Empty, _dataClientEvent = string.Empty , _sortColumn = string.Empty; 

#if (LICENSE)
		//private string _license = string.Empty;
		private int _useCounter = 0;
#endif

        #endregion

        #region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Menu"/> class.
		/// </summary>
        public Menu()
        {
            _Init(string.Empty);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="Menu"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
        public Menu(string id)
        {
            _Init(id);
        }

		/// <summary>
		/// Initalize the new <see cref="Menu"/>.
		/// </summary>
		/// <param name="id">The id.</param>
        private void _Init(string id)
        {
            this.ID = id;
            _menuItems = new MenuItemCollection(this.Controls);
        }

        #endregion

        #region Methods

        private string GetCompatibilityVariable(string index)
        {
            if (Page != null)
            {
                string compatibility;
                if (Page.Request.Browser.Browser.ToUpper() == "IE")
                    compatibility = "{" + index +"}";
                else
                    compatibility = "document.getElementById('{" + index + "}')";

                return compatibility;
            }

            return "{0}";
            
        }

        /// <summary>
        /// Register the client-side script block in the ASPX page.
        /// </summary>
        public void RegisterAPIScriptBlock()
        {
            // Register the script block is not allready done.
            if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY))
            {
                if ((this.ExternalScript == null || this.ExternalScript.TrimEnd() == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
                {
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveMenu.js"));
#else
                    if (CLIENTSIDE_API == null)
                        CLIENTSIDE_API = Utils.GetResource("ActiveUp.WebControls._resources.ActiveMenu.js");

                    if (!CLIENTSIDE_API.StartsWith("<script"))
                        CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

                    CLIENTSIDE_API += "\n</script>\n";

                    Page.RegisterClientScriptBlock(SCRIPTKEY, CLIENTSIDE_API);
#endif                    
                }
                else
                {
                    if (this.ScriptDirectory.StartsWith("~"))
                        this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
                    Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript.TrimEnd() == string.Empty ? "ActiveMenu.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
                }
            }


            string startupString = "\n<script language='javascript'>\n";

			startupString += "// Test if the client script is present.\n";
			startupString += "try\n{\n";
			startupString += "AME_testIfScriptPresent();\n";
			//startupString += "}\ncatch (e) \n{\nalert('Could not find external script file. Please Check the documentation.');\n}\n";
			startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";
			
			startupString += "// Variable declaration related to the control '" + ClientID + "'\n";
            //startupString += string.Format("document.getElementById('{0}').AllowRollOver = '{1}';\n", ClientID, StyleMenuItems.AllowRollOver);

            foreach (MenuItem menuItem in MenuItems)
            {
                startupString += string.Format("\n// {0}\n", menuItem.ClientID);
                startupString += string.Format(GetCompatibilityVariable("0") + ".Parent = null;\n", menuItem.ClientID);
                startupString += string.Format(GetCompatibilityVariable("0") + ".Type = 'master';\n", menuItem.ClientID);
                startupString += string.Format(GetCompatibilityVariable("0") + ".AllowRollOver = '{1}';\n", menuItem.ClientID, this.StyleMenuItems.AllowRollOver);
                if (this.StyleMenuItems.AllowRollOver == true)
                {
                    startupString += string.Format(GetCompatibilityVariable("0") + ".Class = 'AME_{1}_Item';\n", menuItem.ClientID, this.ClientID);
                    startupString += string.Format(GetCompatibilityVariable("0") + ".ClassOver = 'AME_{1}_ItemOver';\n", menuItem.ClientID, this.ClientID);

                    if (menuItem.Image != string.Empty && menuItem.ImageOver != string.Empty)
                    {
                        startupString += string.Format(GetCompatibilityVariable("0") + ".Image = '{1}';\n", menuItem.ClientID, Utils.ConvertToImageDir(this.ImagesDirectory, menuItem.Image));
                        startupString += string.Format(GetCompatibilityVariable("0") + ".ImageOver = '{1}';\n", menuItem.ClientID, Utils.ConvertToImageDir(this.ImagesDirectory, menuItem.ImageOver));
                    }
                }

                string onClick = string.Empty;

                if (menuItem.OnClickClient != string.Empty)
                {
                    onClick += menuItem.OnClickClient;
                    if (onClick[onClick.Length - 1] != ';')
                        onClick += ";";
                 }

                if (AutoPostBack == true || Click != null)
                    onClick += this.Page.GetPostBackEventReference(this,menuItem.ClientID);

                if (onClick != string.Empty)
                    startupString += string.Format(GetCompatibilityVariable("0") + ".OnClickClient = \"{1}\";\n", menuItem.ClientID, onClick);

                if (menuItem.SubMenu.Items.Count > 0)
                    startupString += RegisterSubMenuStartupScript(menuItem.SubMenu, menuItem.ClientID, string.Empty);

				/*for (int i = 0 ; i < menuItem.SubMenu.Items.Count ; i++)
					startupString += RegisterSubMenuStartupScript(menuItem.SubMenu.Items[i],menuItem.SubMenu.Items[i].ClientID,string.Empty);*/

				//startupString += RegisterSubMenuStartupScript(menuItem.SubMenu, menuItem.ClientID, string.Empty);

            }

            startupString += "</script>\n";
            // Render the startup script
            Page.RegisterStartupScript(ClientID + "_startup", startupString);
        }

        private string RegisterSubMenuStartupScript(ToolSubMenu subMenu, string parent, string result)
        {
            result += string.Format(GetCompatibilityVariable("0") + ".Parent = " + GetCompatibilityVariable("1") + ";\n", subMenu.ClientID, parent);
            result += string.Format(GetCompatibilityVariable("0") + ".Type = 'block';\n", subMenu.ClientID);
            result += string.Format(GetCompatibilityVariable("0") + ".AllowRollOver = '{1}';\n", subMenu.ClientID, subMenu.StyleItems.AllowRollOver);
            result += string.Format(GetCompatibilityVariable("0") + ".Class = 'AME_{0}_Block';\n", subMenu.ClientID);
            result += string.Format(GetCompatibilityVariable("0") + ".ClassItem = 'AME_{0}_Item';\n", subMenu.ClientID);
            if (subMenu.StyleItems.AllowRollOver)
            {
                result += string.Format(GetCompatibilityVariable("0") + ".ClassItemOver = 'AME_{0}_ItemOver';\n", subMenu.ClientID);

            }

            foreach (MenuItem subMenuItem in subMenu.Items)
            {
                result += string.Format(GetCompatibilityVariable("0") + ".Parent = " + GetCompatibilityVariable("1") + ";\n", subMenuItem.ClientID, subMenu.ClientID);
                result += string.Format(GetCompatibilityVariable("0") + ".Type = 'item';\n", subMenuItem.ClientID, subMenu.ClientID);
                if (subMenuItem.SubMenu.Items.Count > 0)
                {
                    result += string.Format(GetCompatibilityVariable("0") + ".SubMenu = " + GetCompatibilityVariable("1") + ";\n", subMenuItem.ClientID, subMenuItem.SubMenu.ClientID);

                    result += RegisterSubMenuStartupScript(subMenuItem.SubMenu, subMenuItem.ClientID, result);
                }
                else
                {
                    result += string.Format(GetCompatibilityVariable("0") + ".SubMenu = null;\n", subMenuItem.ClientID);
                }

                if (subMenu.StyleItems.AllowRollOver)
                {
                    if (subMenuItem.Image != string.Empty && subMenuItem.ImageOver != string.Empty)
                    {
                        result += string.Format(GetCompatibilityVariable("0") + ".Image = '{1}';\n", subMenuItem.ClientID, Utils.ConvertToImageDir(this.ImagesDirectory, subMenuItem.Image));
                        result += string.Format(GetCompatibilityVariable("0") + ".ImageOver = '{1}';\n", subMenuItem.ClientID, Utils.ConvertToImageDir(this.ImagesDirectory, subMenuItem.ImageOver));
                    }
                }

                string onClick = string.Empty;

                if (subMenuItem.OnClickClient != string.Empty)
                {
                    onClick += subMenuItem.OnClickClient;
                    if (onClick[onClick.Length - 1] != ';')
                        onClick += ";";
                }

                if (AutoPostBack == true || Click != null)
                    onClick += this.Page.GetPostBackEventReference(this,subMenuItem.ClientID);

                if (onClick != string.Empty)
                    result += string.Format(GetCompatibilityVariable("0") + ".OnClickClient = \"{1}\";\n", subMenuItem.ClientID, onClick);

            }

            return result;

        }

		/// <summary>
		/// Finds a menu item by his id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public MenuItem FindById(string id)
		{
			foreach(MenuItem menuItem in this.MenuItems)
			{
				if (menuItem.ClientID == id)
					return menuItem;

				if (menuItem.SubMenu.Items.Count > 0)
				{
					MenuItem toolMenuItem = FindRecurs(id,menuItem.SubMenu);
					if (toolMenuItem != null)
						return toolMenuItem;
				}
			}

			return null;
		}

		private MenuItem FindRecurs(string id, ToolSubMenu parent)
		{
			foreach(MenuItem menuItem in parent.Items)
			{
				if (menuItem.ClientID == id)
					return menuItem;

				if (menuItem.SubMenu.Items.Count > 0)
					return FindRecurs(id,menuItem.SubMenu);
			}

			return null;
		}

        /// <summary>
        /// Do some work before rendering the control.
        /// </summary>
        /// <param name="e">Event Args</param>
        protected override void OnPreRender(EventArgs e)
        {
            RegisterAPIScriptBlock();
        }

        /// <summary> 
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output">Output stream that contains the HTML used to represent the control.</param>
        protected override void Render(HtmlTextWriter output)
        {
#if (LICENSE)

			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.AMN, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else 
			{
				CreateStyle(output);
				RenderMenu(output);
			}
#else
			CreateStyle(output);
            RenderMenu(output);
#endif
            
        }

        private void EnablePostBackSubMenu(ToolSubMenu subMenu, HtmlTextWriter output)
        {
            if (subMenu.Items.Count > 0)
            {
                foreach (MenuItem subMenuItem in subMenu.Items)
                {
                    subMenuItem.RenderControl(output);

                    if (subMenuItem.SubMenu.Items.Count > 0)
                        EnablePostBackSubMenu(subMenuItem.SubMenu, output);
                }
            }
        }

        private void RenderMenu(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Type, "Hidden");
            output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            output.AddAttribute(HtmlTextWriterAttribute.Value, DateTime.Now.Ticks.ToString());
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();

            // For postback works
            foreach (MenuItem menuItem in MenuItems)
            {
                menuItem.RenderControl(output);
                EnablePostBackSubMenu(menuItem.SubMenu, output);
            }

            output.Write(string.Format("<div id=\"{0}_block\" onselectstart=\"return false\" ondragstart=\"return false\">", ClientID));

            output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_menu");

            if (CssClass == string.Empty)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Class, "AME_" + ClientID + "_Menu");
            }
            else
            {
                output.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
            }

            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, CellPadding.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, CellSpacing.ToString());
			Utils.AddStyleFontAttribute(output,Font);
            // open table (_menu)
            output.RenderBeginTag(HtmlTextWriterTag.Table);

            output.RenderBeginTag(HtmlTextWriterTag.Tr);

            RenderMenuItems(output);

            output.RenderEndTag();

            // close table (_menu)
            output.RenderEndTag();

            // close div (block)
            //output.RenderEndTag();
            output.Write("</div>");

            // Render all SubMenus
            foreach (MenuItem menuItem in MenuItems)
            {
                RenderSubMenu(menuItem.SubMenu, output);
            }
          
        }

        private void RenderMenuItems(HtmlTextWriter output)
		{
			foreach(MenuItem toolItem in MenuItems)
			{ 
				if (toolItem.Width != Unit.Empty)
					output.AddAttribute(HtmlTextWriterAttribute.Width,toolItem.Width.ToString());
				if (toolItem.Height != Unit.Empty)
					output.AddAttribute(HtmlTextWriterAttribute.Height,toolItem.Height.ToString());
				// open td 
				output.RenderBeginTag(HtmlTextWriterTag.Td);

				output.AddAttribute(HtmlTextWriterAttribute.Id,toolItem.ID);
				if (StyleMenuItems.CssClass == string.Empty)
					output.AddAttribute(HtmlTextWriterAttribute.Class,"AME_" + ClientID + "_Item");
				else
					output.AddAttribute(HtmlTextWriterAttribute.Class,StyleMenuItems.CssClass);
				/*if (toolItem.Align != HorizontalAlign.NotSet)
					output.AddAttribute(HtmlTextWriterAttribute.Align,toolItem.Align.ToString());*/
				string subMenuId = string.Empty;
				if (toolItem.SubMenu.Items.Count > 0)
					subMenuId = toolItem.SubMenu.ClientID;
				output.AddAttribute("onmouseover",string.Format("AME_OverMenu(this,'{0}');",subMenuId));
				output.AddAttribute("onmouseout",string.Format("AME_OutMenu(this,'{0}',event);",subMenuId));
				output.AddAttribute(HtmlTextWriterAttribute.Width,"100%");
				output.AddAttribute(HtmlTextWriterAttribute.Height,"100%");
				output.AddAttribute(HtmlTextWriterAttribute.Onclick,"AME_ClickItem(this);");
				output.RenderBeginTag(HtmlTextWriterTag.Table);
				output.RenderBeginTag(HtmlTextWriterTag.Tr);

				if (toolItem.EmbeddedObject == null)
				{
					if (toolItem.Image != string.Empty)
					{
						output.AddAttribute(HtmlTextWriterAttribute.Align,toolItem.Align.ToString());
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						output.AddAttribute(HtmlTextWriterAttribute.Id,toolItem.ClientID + "_IMG");
						output.AddAttribute(HtmlTextWriterAttribute.Src,Utils.ConvertToImageDir(this.ImagesDirectory,toolItem.Image));
						output.RenderBeginTag(HtmlTextWriterTag.Img);
						output.RenderEndTag();
						output.RenderEndTag();
					}

					output.AddAttribute(HtmlTextWriterAttribute.Align,toolItem.Align.ToString());
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					if (toolItem.Text != string.Empty)
					{
						if (toolItem.NavigateURL == string.Empty)
							output.Write(toolItem.Text);
						else
						{
							output.AddStyleAttribute("text-decoration","none");
							if (this.StyleMenuItems.ForeColor != Color.Empty)
								output.AddStyleAttribute("color",Utils.Color2Hex(this.StyleMenuItems.ForeColor));
							output.AddAttribute(HtmlTextWriterAttribute.Href,toolItem.NavigateURL);
							if (toolItem.Target != string.Empty)
								output.AddAttribute(HtmlTextWriterAttribute.Target,toolItem.Target);
							output.RenderBeginTag(HtmlTextWriterTag.A);
							output.Write(toolItem.Text);
							output.RenderEndTag();
						}
					}
				}
				
				output.RenderEndTag();
				output.RenderEndTag();
				output.RenderEndTag();

				//close td
				output.RenderEndTag();

			}
	   }

        private void RenderSubMenu(ToolSubMenu subMenu, HtmlTextWriter output)
        {

            if (subMenu.Items.Count > 0)
            {

               output.Write(string.Format("<div id=\"{0}\" onmouseout=\"AME_OutBlock(this,event);\" style=\"position:absolute;display:none;\">", subMenu.ClientID));

                // open table (_table)
                output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, subMenu.CellPadding.ToString());
                output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, subMenu.CellSpacing.ToString());
                output.AddAttribute(HtmlTextWriterAttribute.Id, subMenu.ClientID + "_table");
                if (subMenu.StyleMenu.CssClass == string.Empty)
                    output.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("AME_{0}_Block", subMenu.ClientID));
                else
                    output.AddAttribute(HtmlTextWriterAttribute.Class, subMenu.StyleMenu.CssClass);
                output.RenderBeginTag(HtmlTextWriterTag.Table);

                bool subMenuIsPresent = false;

                if (this.ImageArrowSubItemMenu != string.Empty)
                {
                    // check if a sub menu is present
                    foreach (MenuItem subMenuItem in subMenu.Items)
                    {
                        if (subMenuItem.SubMenu.Items.Count > 0)
                        {
                            subMenuIsPresent = true;
                            break;
                        }
                    }
                }

                Unit widthImageArrow = Unit.Parse("0px");
                if (subMenuIsPresent)
                {
					try
					{
						System.Drawing.Image arrow = System.Drawing.Image.FromFile(Page.Server.MapPath(Utils.ConvertToImageDir(this.ImagesDirectory, this.ImageArrowSubItemMenu)));
						widthImageArrow = arrow.Width;
					}
					catch {}
                    
                }

                foreach (MenuItem subMenuItem in subMenu.Items)
                {
                    // open tr
                    output.RenderBeginTag(HtmlTextWriterTag.Tr);
                    // open td
                    output.RenderBeginTag(HtmlTextWriterTag.Td);

					output.AddAttribute(HtmlTextWriterAttribute.Width,"100%");
                    output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                    output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                    output.AddAttribute(HtmlTextWriterAttribute.Id, subMenuItem.ClientID);
                    if (StyleMenuItems.CssClass == string.Empty)
                        output.AddAttribute(HtmlTextWriterAttribute.Class, "AME_" + subMenu.ClientID + "_Item");
                    else
                        output.AddAttribute(HtmlTextWriterAttribute.Class, StyleMenuItems.CssClass);
                    output.AddAttribute("onmouseover", "AME_OverItem(this,event);");
                    output.AddAttribute("onmouseout", "AME_OutItem(this,event);");

                    output.AddAttribute(HtmlTextWriterAttribute.Onclick, "AME_ClickItem(this);");
                    // open table (_subMenuItem)
                    output.RenderBeginTag(HtmlTextWriterTag.Table);
                    output.RenderBeginTag(HtmlTextWriterTag.Tr);

                    if (subMenuItem.EmbeddedObject != null)
                    {
                        output.RenderBeginTag(HtmlTextWriterTag.Td);
                        output.RenderEndTag();

                        output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                        output.RenderBeginTag(HtmlTextWriterTag.Td);
                        subMenuItem.EmbeddedObject.RenderControl(output);
                        output.RenderEndTag();

                        if (this.ImageArrowSubItemMenu != string.Empty)
                        {
                            output.AddAttribute(HtmlTextWriterAttribute.Width, string.Format("{0}px", widthImageArrow.Value + 10));
                            output.RenderBeginTag(HtmlTextWriterTag.Td);
                            output.RenderEndTag();
                        }
                    }

                    else
                    {

                        if (subMenuItem.Image != string.Empty)
                        {
                            output.AddAttribute(HtmlTextWriterAttribute.Width, "3px");
                            output.RenderBeginTag(HtmlTextWriterTag.Td);
                            output.RenderEndTag();

                            if (subMenuItem.Align != HorizontalAlign.NotSet)
                                output.AddAttribute(HtmlTextWriterAttribute.Align, subMenuItem.Align.ToString());
                            output.RenderBeginTag(HtmlTextWriterTag.Td);
                            output.AddAttribute(HtmlTextWriterAttribute.Id, subMenuItem.ClientID + "_IMG");
                            output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(this.ImagesDirectory, subMenuItem.Image));
                            output.RenderBeginTag(HtmlTextWriterTag.Img);
                            output.RenderEndTag();
                            output.RenderEndTag();
                        }

                        if (subMenuItem.Align != HorizontalAlign.NotSet)
                            output.AddAttribute(HtmlTextWriterAttribute.Align, subMenuItem.Align.ToString());
                        output.RenderBeginTag(HtmlTextWriterTag.Td);
                        if (subMenuItem.Image != string.Empty)
                            output.AddAttribute("style", "margin-left:3px;");
                        output.RenderBeginTag(HtmlTextWriterTag.Span);
                        if (subMenuItem.Text != string.Empty)
                        {
                            if (subMenuItem.NavigateURL == string.Empty)
                                output.Write(subMenuItem.Text);
                            else
                            {
                                output.AddStyleAttribute("text-decoration", "none");
                                if (subMenu.StyleItems.ForeColor != Color.Empty)
                                    output.AddStyleAttribute("color", Utils.Color2Hex(subMenu.StyleItems.ForeColor));
                                output.AddAttribute(HtmlTextWriterAttribute.Href, subMenuItem.NavigateURL);
                                if (subMenuItem.Target != string.Empty)
                                    output.AddAttribute(HtmlTextWriterAttribute.Target, subMenuItem.Target);
                                output.RenderBeginTag(HtmlTextWriterTag.A);
                                output.Write(subMenuItem.Text);
                                output.RenderEndTag();
                            }
                        }
                        output.RenderEndTag();
                        output.RenderEndTag();

                        if (this.ImageArrowSubItemMenu != string.Empty)
                        {
                            if (subMenuItem.SubMenu.Items.Count > 0)
                            {
                                output.RenderBeginTag(HtmlTextWriterTag.Td);
                                output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(this.ImagesDirectory, this.ImageArrowSubItemMenu));
                                output.AddAttribute("style", "margin-left:10px");
                                output.RenderBeginTag(HtmlTextWriterTag.Img);
                                output.RenderEndTag();
                                output.RenderEndTag();
                            }

                            else
                            {
                                output.AddAttribute(HtmlTextWriterAttribute.Width, string.Format("{0}px", widthImageArrow.Value + 10));
                                output.RenderBeginTag(HtmlTextWriterTag.Td);
                                output.RenderEndTag();
                            }
                        }
                    }

                    output.RenderEndTag();
                    // close table (_subMenuItem)
                    output.RenderEndTag();

                    // close td
                    output.RenderEndTag();
                    // close tr
                    output.RenderEndTag();
                }

                // close table (_table)
                output.RenderEndTag();

                // close div (_subMenu)
                //output.RenderEndTag();
                output.Write("</div>");

                // check if subMenu to render
                foreach (MenuItem subMenuItem in subMenu.Items)
                {
                    if (subMenuItem.SubMenu.Items.Count > 0)
                        RenderSubMenu(subMenuItem.SubMenu, output);
                }
            }
        }   

        private void CreateStyle(HtmlTextWriter output)
        {
            string CSSClass = string.Empty;
            CSSClass = string.Concat(CSSClass, "<style>\n");

            string val = CreateMenuHtmlCss();
            if (val != string.Empty)
                CSSClass = string.Concat(CSSClass, val);

            val = StyleMenuItems.CreateHtmlCss(ClientID, "_Item");
            if (val != string.Empty)
                CSSClass = string.Concat(CSSClass, val);

            foreach (MenuItem menuItem in MenuItems)
            {
                if (menuItem.SubMenu.Items.Count > 0)
                {
                    val = string.Empty;
                    val = CreateSubMenuStyle(menuItem.SubMenu, val);
                    if (val != string.Empty)
                        CSSClass = string.Concat(CSSClass, val);
                }
            }

            CSSClass = string.Concat(CSSClass, "</style>\n");

            output.Write(CSSClass);

        }

        private string CreateMenuHtmlCss()
        {
            string style = string.Empty;
            string valueCss = string.Empty;

            if (BackImage != string.Empty)
                valueCss = string.Format("background-image:url({0});\n", Utils.ConvertToImageDir(ImagesDirectory, BackImage));

            if (BackColor != Color.Empty)
            {
                valueCss = string.Format("background-color : {0};\n", Utils.Color2Hex(BackColor));
            }

            if (BorderStyle != BorderStyle.NotSet)
            {
                valueCss += string.Format("border-style : {0};\n", BorderStyle.ToString());
                valueCss += string.Format("border-width : {0};\n", BorderWidth);
                valueCss += string.Format("border-color : {0};\n", Utils.Color2Hex(BorderColor));
            }

            if (ForeColor != Color.Empty)
            {
                valueCss += string.Format("color : {0};\n", Utils.Color2Hex(ForeColor));
            }

            if (valueCss == string.Empty)
                style = string.Empty;
            else
            {
                style += string.Format(".AME_{0}_Menu\n", ClientID);
                style += "{\n";
                style += valueCss;
                style += "}\n";
            }

            return style;
        }

        private string CreateSubMenuStyle(ToolSubMenu subMenu, string style)
        {
            if (subMenu.Items.Count > 0)
            {
                style += subMenu.StyleMenu.CreateHtmlCss(subMenu.ClientID, "_Block");

                style += subMenu.StyleItems.CreateHtmlCss(subMenu.ClientID, "_Item");

                foreach (MenuItem item in subMenu.Items)
                {
                    if (item.SubMenu.Items.Count > 0)
                        return CreateSubMenuStyle(item.SubMenu, style);
                }
                return style;
            }
            return string.Empty;
        }

		/// <summary>
		/// Loads menu from XML.
		/// </summary>
		/// <param name="fileName">Name of the xml file.</param>
		public void LoadFromXml(string fileName)
		{
			TextReader reader = new StreamReader(fileName);
			XmlSerializer serializer = new XmlSerializer(typeof(ActiveUp.WebControls.XmlMenu)); 
			ActiveUp.WebControls.XmlMenu menu = (ActiveUp.WebControls.XmlMenu)serializer.Deserialize(reader);
			reader.Close();

			if (menu.ImageArrowSubItemMenu != null)
				this.ImageArrowSubItemMenu = menu.ImageArrowSubItemMenu;

			if (menu.StyleMenu.BackColor != null)
				this.BackColor = Utils.HexStringToColor(menu.StyleMenu.BackColor);

			if (menu.StyleMenu.BorderColor != null)
				this.BorderColor = Utils.HexStringToColor(menu.StyleMenu.BorderColor);

			this.BorderStyle = menu.StyleMenu.BorderStyle;
			
			if (menu.StyleMenu.CssClass != null)
				this.CssClass = menu.StyleMenu.CssClass;

			if (menu.StyleMenu.ForeColor != null)
				this.ForeColor = Utils.HexStringToColor(menu.StyleMenu.ForeColor);

			this.CellPadding = menu.StyleMenu.CellPadding;
			this.CellSpacing = menu.StyleMenu.CellSpacing;

			this.StyleMenuItems = Convert(menu.StyleMenuItems);

			this.MenuItems.Clear();
			foreach(XmlMenuItem menuItem in menu.MenuItems)
			{
				MenuItem menuItemConverted = Convert(menuItem);
				this.MenuItems.Add(menuItemConverted);
						
			}
		}
		
		private MenuItemStyle Convert(XmlMenuItemStyle xmlMenuItemStyle)
		{
			MenuItemStyle style = new MenuItemStyle();

			style.AllowRollOver = xmlMenuItemStyle.AllowRollOver;
			
			if (xmlMenuItemStyle.BackColor != null)
				style.BackColor = Utils.HexStringToColor(xmlMenuItemStyle.BackColor);

			if (xmlMenuItemStyle.BackColorOver != null)
				style.BackColorOver = Utils.HexStringToColor(xmlMenuItemStyle.BackColorOver);

			if (xmlMenuItemStyle.BorderColor != null)
				style.BorderColor = Utils.HexStringToColor(xmlMenuItemStyle.BorderColor);

			if (xmlMenuItemStyle.BorderColorOver != null)
				style.BorderColorOver = Utils.HexStringToColor(xmlMenuItemStyle.BorderColorOver);

			style.BorderStyle = xmlMenuItemStyle.BorderStyle;
			style.BorderStyleOver = xmlMenuItemStyle.BorderStyleOver;
			style.BorderWidth = Unit.Parse(xmlMenuItemStyle.BorderWidth.ToString());
			style.BorderWidthOver = xmlMenuItemStyle.BorderWidthOver;

			if (xmlMenuItemStyle.CssClass != null)
				style.CssClass = xmlMenuItemStyle.CssClass;

			if (xmlMenuItemStyle.CssClassOver != null)
				style.CssClassOver = xmlMenuItemStyle.CssClassOver;

			if (xmlMenuItemStyle.ForeColor != null)
				style.ForeColor = Utils.HexStringToColor(xmlMenuItemStyle.ForeColor);

			if (xmlMenuItemStyle.ForeColorOver != null)
				style.ForeColorOver = Utils.HexStringToColor(xmlMenuItemStyle.ForeColorOver);

			style.Margin = xmlMenuItemStyle.Margin;

			return style;
		}

		private MenuItem Convert(XmlMenuItem xmlMenuItem)
		{
			MenuItem menuItem = new MenuItem();
			menuItem.ID = xmlMenuItem.Id;
			menuItem.Align = xmlMenuItem.Align;
			menuItem.Height = xmlMenuItem.Height;
			menuItem.Image = xmlMenuItem.Image;
			menuItem.ImageOver = xmlMenuItem.ImageOver;
			menuItem.NavigateURL = xmlMenuItem.NavigateURL;
			menuItem.OnClickClient = xmlMenuItem.OnClickClient;
			menuItem.Target = xmlMenuItem.Target;
			menuItem.Text = xmlMenuItem.Text;
			menuItem.Width = xmlMenuItem.Width;

			menuItem.SubMenu.StyleItems = Convert(xmlMenuItem.SubMenu.StyleItems);
			menuItem.SubMenu.StyleMenu = Convert(xmlMenuItem.SubMenu.StyleMenu);
			
			menuItem.SubMenu.Items.Clear();
			foreach(XmlMenuItem xmlMenuItemSub in xmlMenuItem.SubMenu.MenuItems)
			{
				MenuItem menuItemSub = Convert(xmlMenuItemSub);
				menuItem.SubMenu.Items.Add(menuItemSub);
			}

			return menuItem;
		}

		private MenuStyle Convert(XmlMenuStyle xmlMenuStyle)
		{
			MenuStyle style = new MenuStyle();

			if (xmlMenuStyle.BackColor != null)
				style.BackColor = Utils.HexStringToColor(xmlMenuStyle.BackColor);

			if (xmlMenuStyle.BorderColor != null)
				style.BorderColor = Utils.HexStringToColor(xmlMenuStyle.BorderColor);

			style.BorderStyle = xmlMenuStyle.BorderStyle;
			style.BorderWidth = Unit.Parse(xmlMenuStyle.BorderWidth.ToString());

			if (xmlMenuStyle.CssClass != null)
				style.CssClass = xmlMenuStyle.CssClass;

			if (xmlMenuStyle.ForeColor != null)
				style.ForeColor = Utils.HexStringToColor(xmlMenuStyle.ForeColor);

			return style;
		}

		#endregion

        #region Properties

        #region Behavior

		/// <summary>
		/// Gets or sets a value indicating whether the post back occurs after each menu item selection.
		/// </summary>
		/// <value><c>true</c> if the post back occus after each menu item selection; otherwise, <c>false</c>.</value>
        [Bindable(false),
         Category("Behavior"),
         Description("The value indicating the post back occurs after each menu item selection."),
         DefaultValue(false)
         ]
        public bool AutoPostBack
        {
            get
            {
                object local = ViewState["AutoPostBack"];
                if (local != null)
                    return (bool)local;
                else
                    return false;
            }

            set
            {
                ViewState["AutoPostBack"] = value;
            }
        }

        #endregion

        #region Appearence

        /// <summary>
        /// Gets or sets the background color of the Web server control.
        /// </summary>
        [
            DefaultValue(typeof(Color),"#B4B1A3")
        ]
        public new Color BorderColor
        {
			get 
			{
				object borderColor = ViewState["BorderColor"];

				if (borderColor == null)
					return Color.FromArgb(0xB4,0xB1,0xA3);

				return (Color)borderColor;
			}

			set
			{
				ViewState["BorderColor"] = value;
			}
        }

        /// <summary>
        /// Gets or sets the border width of the Web server control.
        /// </summary>
        [
            DefaultValue(typeof(Unit),"1px")
        ]
        public new Unit BorderWidth
        {
			get 
			{
				object borderWidth = ViewState["BorderWidth"];

				if (borderWidth == null)
					return Unit.Parse("1px");

				return (Unit)borderWidth;
			}

			set
			{
				ViewState["BorderWidth"] = value;
			}
        }

        /// <summary>
        /// Gets or sets the cell spacing of the table representing the menu.
        /// </summary>
        [
        Bindable(false),
        Category("Appearence"),
        Description("Gets or sets the cell spacing of the table representing the menu."),
        DefaultValue("1")
        ]
        public int CellSpacing
        {
            get
            {
                object _cellSpacing;
                _cellSpacing = ViewState["_cellSpacing"];
                if (_cellSpacing != null)
                    return (int)_cellSpacing;
                return 0;
            }
            set
            {
                ViewState["_cellSpacing"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the cell padding of the table representing the toolbar.
        /// </summary>
        [
        Bindable(false),
        Category("Appearence"),
        Description("Gets or sets the cell padding of the table representing the toolbar."),
        DefaultValue("1")
        ]
        public int CellPadding
        {
            get
            {
                object _cellSpacing;
                _cellSpacing = ViewState["_cellPadding"];
                if (_cellSpacing != null)
                    return (int)_cellSpacing;
                return 0;
            }
            set
            {
                ViewState["_cellPadding"] = value;
            }
        }

        /// <summary>
        /// Image used as background of the tool.
        /// </summary>
        [
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Image used as background of the toolbar.")
		]
		public string BackImage
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(BackImage), string.Empty);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(BackImage), value);
			}
		}

		/// <summary>
		/// Gets or sets the menu items style.
		/// </summary>
		/// <value>The menu items style.</value>
        [
        Bindable(true),
        DefaultValue(""),
        Description("Style of the menu item."),
        //PersistenceModeAttribute(PersistenceMode.InnerDefaultProperty),
        DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
        Category("Appearance")
        ]
        public MenuItemStyle StyleMenuItems
        {
            get
            {
                return _styleMenuItems;
            }

			set
			{
				_styleMenuItems = value;
			}
        }

        /// <summary>
        /// Gets or sets the arrow image if a item contains a sub menu.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The arrow image if a item contains a sub menu."),
        Category("Appearance")
        ]
        public string ImageArrowSubItemMenu
        {
            get
            {
                object imageArrow = ViewState["_imageArrow"];
                if (imageArrow != null)
                    return (string)imageArrow;
                else
                    return string.Empty;
            }

            set
            {
                ViewState["_imageArrow"] = value;
            }
        }

        #endregion

        #region Data
/*
#if (LICENSE)

		/// <summary>
		/// Gets or sets the license key.
		/// </summary>
		[
		Bindable(false),
		Category("Data"),
		Description("Lets you specify the license key.")
		]
		public string License
		{
			get
			{
				return _license;
			}
		
			set
			{
				_license = value;
			}
		}

#endif*/

        /// <summary>
        /// Gets or sets the collection containing the menu items.
        /// </summary>
        [
        Category("Data"),
        Description("The collection of menu items"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerDefaultProperty),
		Browsable(false)
        ]
        public MenuItemCollection MenuItems
        {
            get
            {
                if (IsTrackingViewState)
                {
                    ((IStateManager)_menuItems).TrackViewState();
                }
                return _menuItems;
            }
        }

        #endregion

		#region DataBind

		/// <summary>
		/// Gets or sets the main data source of the body template.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Main data source of the body template."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		] 
		public DataTable DataSource
		{
			get
			{
				return _dataSource;
			}
			set
			{
				_dataSource = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the text content of the list items.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Field of the data source that provides the text content of the list items")
		] 
		public string DataText
		{
			get
			{	
				return _dataText;
			}

			set
			{
				_dataText = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each link item.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides the value of each link item.")
		]
		public string DataNavigateUrl
		{
			get
			{
				return _dataNavigateUrl;
			}

			set
			{
				_dataNavigateUrl = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each target item.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides the value of each target item.")
		]
		public string DataTarget { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each image item.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides the value of each image item.")
		]
		public string DataImage
		{
			get 
			{
				return _dataImage;
			}

			set 
			{
				_dataImage = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each image over item.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides the value of each image over item.")
		]
		public string DataImageOver
		{
			get 
			{
				return _dataImageOver;
			}

			set 
			{
				_dataImageOver = value;
			}
		}


		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each client side event item.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides the value of each client side event item.")
		]
		public string DataClientEvent
		{
			get 
			{
				return _dataClientEvent;
			}

			set 
			{
				_dataClientEvent = value;
			}
		}


		/// <summary>
		/// Gets or sets the field of data parent.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides the value of each target item.")
		]
		public string DataParent { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the field for sorting nodes.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Field of the data source for sorting nodes.")
		] 
		public string SortColumn
		{
			get
			{
				return _sortColumn;
			}

			set
			{
				_sortColumn = value;
			}
		}

		/// <summary>
		/// Gets or sets the field for sort order.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue("Ascending"),
		Description("Field of the data source for sort order.")
		] 
		public DataSourceOrder SortOrder
		{
			get
			{
				return _sortOrder;
			}

			set
			{
				_sortOrder = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each list item.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Field of the data source that provides the value of each list item.")
		] 
		public string DataValue
		{
			get
			{	
				return _dataValue;
			}

			set
			{
				_dataValue = value;
			}
		}

		#endregion

        #region JScript
        /// <summary>
        /// Gets or sets the filename of the external script file.
        /// </summary>
        [
        Bindable(false),
        Category("Behavior"),
        Description("Gets or sets the name of the API javascript file."),
        DefaultValue("")
        ]
        public string ExternalScript
        {
            get
            {
                string _externalScript;
                _externalScript = ((string)ViewState["_externalScript"]);
                if (_externalScript != null)
                    return _externalScript;
                return string.Empty;
            }
            set
            {
                ViewState["_externalScript"] = value;
            }
        }
        #endregion

        #region Events

		/// <summary>
		/// Click server event.
		/// </summary>
        [Category("Event")]
        public event StringEventHandler Click;

		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="e">The <see cref="ActiveUp.WebControls.StringEventArgs"/> instance containing the event data.</param>
        protected virtual void OnClick(StringEventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        #endregion

        #endregion

        #region Interface IPostBack
       
		/// <summary>
		/// Event handler contains a string as argument.
		/// </summary>
		public delegate void StringEventHandler(Object sender, StringEventArgs e);

        /// <summary>
        /// A RaisePostBackEvent.
        /// </summary>
        /// <param name="eventArgument">eventArgument</param>
        public void RaisePostBackEvent(String eventArgument)
        {
            Page.Trace.Warn(ID, "RaisePostBackEvent");
			OnClick(new StringEventArgs(eventArgument));
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            Page.Trace.Warn(ID, "LoadPostData");

            return true;
        }

		/// <summary>
		/// When implemented by a class, signals the server control object to notify the ASP.NET application that the state of the
		/// control has changed.
		/// </summary>
        public virtual void RaisePostDataChangedEvent()
        {
            Page.Trace.Warn(ID, "RaisePostDataChangedEvent");
        }

        #endregion

        #region Viewstate

		/// <summary>
		/// Loads the state of the view.
		/// </summary>
		/// <param name="savedState">State of the saved.</param>
		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState, (state) => ((IStateManager)_menuItems).LoadViewState(state));
		}

		/// <summary>
		/// Saves the view state of the control.
		/// </summary>
		/// <returns>The saved view state.</returns>
		protected override object SaveViewState()
		{
			return base.SaveViewState(_menuItems);
		}

		/// <summary>
		/// Tracks the view state.
		/// </summary>
		protected override void TrackViewState()
		{
			base.TrackViewState(_menuItems);
		}

        #endregion Custom State Management Implementation

		#region DataBind
	
		/// <summary>
		/// Binds a data source to the invoked server control and all its child
		/// controls.
		/// </summary>
		public override void DataBind()
		{
			if (this.DataSource != null)
			{
				AddToolMenuItem("0");
			}
		}

		private void AddToolMenuItem(string parent)
		{
			string sortOrder = string.Empty;
			switch (_sortOrder)
			{
				case DataSourceOrder.Descending:
				{
					sortOrder = " DESC";
				} break;

				default : 
				{
					sortOrder = " ASC";
				} break;
			}

			DataRow[] drTemp = this._dataSource.Select(this.DataParent + " = '" + parent + "'", (this.SortColumn != string.Empty ? this.SortColumn : this.DataText) + sortOrder);
			
			foreach(DataRow row in drTemp)
			{
				string datavalue = string.Empty;
				string datanavigateurl = string.Empty;
				string datatarget = string.Empty;
				string datatext = string.Empty;
				string dataimage = string.Empty;
				string dataimageover = string.Empty;
				string dataclientevent = string.Empty;

				// id
				if (this.DataValue != null && this.DataValue != string.Empty)
					datavalue = row[this.DataValue].ToString();

				// navigate url
				if (this.DataNavigateUrl != null && this.DataNavigateUrl != string.Empty)
					datanavigateurl = row[this.DataNavigateUrl].ToString();

				// target
				if (this.DataTarget != null && this.DataTarget != string.Empty)
					datatarget = row[this.DataTarget].ToString();

				// text
				if (this.DataText != null && this.DataText != string.Empty)
					datatext = row[this.DataText].ToString();

				// image
				if (this.DataImage != null && this.DataImage != string.Empty)
					dataimage = row[this.DataImage].ToString();

				// image over
				if (this.DataImageOver != null && this.DataImageOver != string.Empty)
					dataimageover = row[this.DataImageOver].ToString();

				// client event
				if (this.DataClientEvent != null && this.DataClientEvent != string.Empty)
					dataclientevent = row[this.DataClientEvent].ToString();

				MenuItem menuItemParent = this.FindById(parent);

				MenuItem newMenuItem = new MenuItem();
				newMenuItem.ID = datavalue;
				newMenuItem.Text = datatext;
				newMenuItem.Target = datatarget;
				newMenuItem.Image = dataimage;
				newMenuItem.ImageOver = dataimageover;
				newMenuItem.NavigateURL = datanavigateurl;
				newMenuItem.OnClickClient = dataclientevent;
				
				if (menuItemParent == null)
				{
					this.MenuItems.Add(newMenuItem);
				}

				else
				{
					menuItemParent.SubMenu.Items.Add(newMenuItem);
				}

				AddToolMenuItem(row[this.DataValue].ToString());
			}
		}

		#endregion

    }

    #endregion
}
