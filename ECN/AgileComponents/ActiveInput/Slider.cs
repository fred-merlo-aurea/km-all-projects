using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	#region enum DirectionLift
	/// <summary>
	/// Direction of the slider.
	/// </summary>
	public enum DirectionLift
	{
		/// <summary>
		/// Horizontal direction.
		/// </summary>
		Horizontal = 0,
		/// <summary>
		/// Vertical direction
		/// </summary>
		Vertical
	};

	#endregion

	#region class Slider
	/// <summary>
	/// Represents a <see cref="Slider"/> object.
	/// </summary>
    [ToolboxBitmap(typeof(Slider), "ToolBoxBitmap.Slider.bmp")]
	public class Slider : CoreWebControl, IPostBackEventHandler, IPostBackDataHandler
	{
		#region Variables

		/// <summary>
		/// ValueChanged event handler.
		/// </summary>
		public delegate void ValueChangedEventHandler(object sender, EventArgs e);

		/// <summary>
		/// Occurs when the value is changed.
		/// </summary>
		public event ValueChangedEventHandler ValueChanged;

		private string _minusArrowOnImage;
		private string _plusArrowOnImage;
		private string _liftOff;
		private string _liftStartOff;
		private string _liftGrabberOff;
		private int _min, _max, _step;
		private const string SCRIPTKEY = "ActiveSlider";
		private string CLIENTSIDE_API; 
		private string _externalScript, _scriptDirectory;
		private bool _directStep;
		private int _speedArrow, _speedOutLift;
		private string _onValueChangedClient;
		private long _currentValue, _oldCurrentValue;
		private DirectionLift _direction;
		private Unit _size, _liftSize;

	    private const string ClientID_Postfix = "_plusArrow";
	    private const string ImageName_ArrowDownOff = "ArrowDownOff.gif";
	    private const string BorderAttributeValue_0 = "0";
	    private const string Event_OnMouseDown = "onmousedown";
	    private const string Event_OnMouseOut = "onmouseout";
	    private const string Event_OnMouseUp = "onmouseup";
        private const string ArrowUpOffImage = "ArrowUpOff.gif";
        private const string ArrowDownOffImage = "ArrowDownOff.gif";
        private const string BackgroundVertOnImage = "BackgroundVertOn.gif";
        private const string ButtonVertCenterOffImage = "ButtonVertCenterOff.gif";
        private const string ButtonVertEndOffImage = "ButtonVertEndOff.gif";
        private const string BackgroundVertOffImage = "BackgroundVertOff.gif";
        private const string ButtonVertStartOffImage = "ButtonVertStartOff.gif";
        private const string ButtonVertGrabberOffImage = "ButtonVertGrabberOff.gif";

#if (LICENSE)
        internal static int _useCounter;
#endif

		#endregion

		#region Constructors

		/// <summary>
		/// The default constructor.
		/// </summary>
		public Slider()
		{
#if (!FX1_1)
			_minusArrowOnImage = string.Empty;
			_plusArrowOnImage = string.Empty;
			_liftOff = string.Empty;
			_liftGrabberOff = string.Empty;
			_liftStartOff = string.Empty;
#else			
			_minusArrowOnImage = "ArrowUpOn.gif";
			_plusArrowOnImage = "ArrowDownOn.gif";
			_liftOff = "BackgroundVertOff.gif";
			_liftGrabberOff = "ButtonVertGrabberOff.gif";
			_liftStartOff = "ButtonVertStartOff.gif";
#endif			
			_min = 0;
			_max = 100;
			_step = 1;
			CLIENTSIDE_API = string.Empty;
			_externalScript = string.Empty;
			_onValueChangedClient = string.Empty;
			_directStep = false;
			_speedArrow = 150;
			_speedOutLift = 20;
			_currentValue = 0;
			_oldCurrentValue = 0;
			_size = Unit.Parse("300px");
			_liftSize = Unit.Parse("30px");
			_direction = DirectionLift.Vertical;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Register the client side script block in the ASP page.
		/// </summary>
		protected void RegisterScriptBlock() 
		{
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
					Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveSlider.js"));
#else					
					if (CLIENTSIDE_API == string.Empty)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveSlider.js");
					
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
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveSlider.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}

			string startupString = "<script language='javascript'>\n";
			startupString += "// Variable declaration related to the control '" + ClientID + "'\n";
			startupString += ClientID + "_min=" + _min + ";\n";
			startupString += ClientID + "_max=" + _max + ";\n";
			startupString += ClientID + "_step=" + _step + ";\n";
			startupString += ClientID + "_minusArrowOffImage=\"" + Utils.ConvertToImageDir(ImagesDirectory, MinusArrowOffImage, "ArrowUpOff.gif",Page,this.GetType()) + "\";\n";
            startupString += ClientID + "_minusArrowOnImage=\"" + Utils.ConvertToImageDir(ImagesDirectory, _minusArrowOnImage, "ArrowUpOn.gif", Page, this.GetType()) + "\";\n";
            startupString += ClientID + "_plusArrowOffImage=\"" + Utils.ConvertToImageDir(ImagesDirectory, PlusArrowOffImage, "ArrowDownOff.gif", Page, this.GetType()) + "\";\n";
            startupString += ClientID + "_plusArrowOnImage=\"" + Utils.ConvertToImageDir(ImagesDirectory, _plusArrowOnImage, "ArrowDownOn.gif", Page, this.GetType()) + "\";\n";
            startupString += ClientID + "_liftOn=\"" + Utils.ConvertToImageDir(ImagesDirectory, LiftOn, "BackgroundVertOn.gif", Page, this.GetType()) + "\";\n";
            startupString += ClientID + "_liftOff=\"" + Utils.ConvertToImageDir(ImagesDirectory, _liftOff, "BackgroundVertOff.gif", Page, this.GetType()) + "\";\n";
			startupString += ClientID + "_directStep=\"" + _directStep + "\";\n";
			startupString += ClientID + "_speedArrow=" + _speedArrow + ";\n";
			startupString += ClientID + "_speedOutLift=" + _speedOutLift + ";\n";
			startupString += ClientID + "_onValueChanged=\"" + _onValueChangedClient + "\";\n";
			startupString += ClientID + "_direction=\"" + _direction.ToString() + "\";\n";
			startupString += "try\n{\n";
			startupString += "ASD_SetValue('" + ClientID + "'," + _currentValue + ");\n";
			startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";
			startupString += "</script>\n";
			Page.RegisterStartupScript(ClientID + "_startup", startupString);
		}

		/// <summary>
		/// Raises the PreRender event.
		/// </summary>
		/// <remarks>This method notifies the server control to perform any necessary prerendering steps prior to saving view state and rendering content.</remarks>
		/// <param name="e">An <see cref="EventArgs"/> object that contains the event data. </param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);

			RegisterScriptBlock();
		}

		/// <summary>
		/// Sends server control content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
#if (LICENSE)
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.AIP, 4, Edition.S1));
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
				RenderSlider(output);
			}

#else
			RenderSlider(output);
#endif
		}

		/// <summary>
		/// Render the slider.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		private void RenderSlider(HtmlTextWriter output)
		{
			output.AddAttribute(HtmlTextWriterAttribute.Type, "Hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Value, DateTime.Now.Ticks.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.Write(
				$"<input type=\"hidden\" name=\"{ClientID}_oldCurrentValue\" id=\"{ClientID}_oldCurrentValue\" value=\"{_currentValue}\">");
			output.Write("<input type=\"hidden\" name=\"" + ClientID + "_currentValue\" id=\"" + ClientID +
			             "_currentValue\" value=\"" + _currentValue + "\">");

			output.RenderBeginTag(HtmlTextWriterTag.Div); // START DIV

			RenderSliderHeader(output);

			output.RenderBeginTag(HtmlTextWriterTag.Table); //START TABLE

			if(_direction == DirectionLift.Vertical)
			{
				RenderVerticalSlider(output);
			}
			else
			{
				RederHorizontalSlider(output);
			}

			output.RenderEndTag(); // END TABLE
			output.RenderEndTag(); // END DIV
		}

		private void RederHorizontalSlider(HtmlTextWriter output)
		{
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Width, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_minusArrow");
			output.AddAttribute(HtmlTextWriterAttribute.Name, ClientID + "_minusArrow");
			output.AddAttribute(HtmlTextWriterAttribute.Src,
				Utils.ConvertToImageDir(ImagesDirectory, MinusArrowOffImage, ArrowUpOffImage, Page, GetType()));
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute("onmousedown", $"ASD_MinusArrowDown('{ClientID}');");
			output.AddAttribute("onmouseout", $"ASD_StopStepMinus('{ClientID}');");
			output.AddAttribute("onmouseup", $"ASD_StopStepMinus('{ClientID}');");
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
			output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			output.AddAttribute("onmousedown", $"ASD_OutLiftDown('{ClientID}',this);");
			output.AddAttribute("onmousemove", "ASD_OutLiftMove();");
			output.AddAttribute("onmouseup", $"ASD_OutLiftStop('{ClientID}',this);");
			output.AddAttribute("onmouseout", $"ASD_OutLiftStop('{ClientID}',this);");
			output.AddAttribute(HtmlTextWriterAttribute.Background,
				Utils.ConvertToImageDir(ImagesDirectory, _liftOff, BackgroundVertOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_lift");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Width, _liftSize.ToString());
			output.AddStyleAttribute("position", "absolute");
			output.AddStyleAttribute("z-index", "101");
			output.AddAttribute("onmousedown", $"ASD_MouseDown(event,'{ClientID}');");
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);

			output.AddAttribute(HtmlTextWriterAttribute.Width, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_liftStartOff");
			output.AddAttribute(HtmlTextWriterAttribute.Src,
				Utils.ConvertToImageDir(ImagesDirectory, _liftStartOff, ButtonVertStartOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			output.AddAttribute(HtmlTextWriterAttribute.Background,
				Utils.ConvertToImageDir(ImagesDirectory, LiftCenterOff, ButtonVertCenterOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Width, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_liftGrabberOff");
			output.AddAttribute(HtmlTextWriterAttribute.Src,
				Utils.ConvertToImageDir(ImagesDirectory, _liftGrabberOff, ButtonVertGrabberOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			output.AddAttribute(HtmlTextWriterAttribute.Background,
				Utils.ConvertToImageDir(ImagesDirectory, LiftCenterOff, ButtonVertCenterOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Width, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_liftEndOff");
			output.AddAttribute(HtmlTextWriterAttribute.Src,
				Utils.ConvertToImageDir(ImagesDirectory, LiftEndOff, ButtonVertEndOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.RenderEndTag();

			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Width, "0");

			RenderTdWithPlusArrowImage(ref output);
			output.RenderEndTag();
		}

		private void RenderVerticalSlider(HtmlTextWriter output)
		{
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Height, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_minusArrow");
			output.AddAttribute(HtmlTextWriterAttribute.Name, ClientID + "_minusArrow");
			output.AddAttribute(HtmlTextWriterAttribute.Src,
				Utils.ConvertToImageDir(ImagesDirectory, MinusArrowOffImage, ArrowUpOffImage, Page, GetType()));
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute("onmousedown", $"ASD_MinusArrowDown('{ClientID}');");
			output.AddAttribute("onmouseout", $"ASD_StopStepMinus('{ClientID}');");
			output.AddAttribute("onmouseup", $"ASD_StopStepMinus('{ClientID}');");
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
			output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
			output.AddAttribute("onmousedown", $"ASD_OutLiftDown('{ClientID}',this);");
			output.AddAttribute("onmousemove", "ASD_OutLiftMove();");
			output.AddAttribute("onmouseup", $"ASD_OutLiftStop('{ClientID}',this);");
			output.AddAttribute("onmouseout", $"ASD_OutLiftStop('{ClientID}',this);");
			output.AddAttribute(HtmlTextWriterAttribute.Background,
				Utils.ConvertToImageDir(ImagesDirectory, _liftOff, BackgroundVertOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_lift");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Height, _liftSize.ToString());
			output.AddStyleAttribute("position", "absolute");
			output.AddStyleAttribute("z-index", "101");
			output.AddAttribute("onmousedown", $"ASD_MouseDown(event,'{ClientID}');");
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Height, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_liftStartOff");
			output.AddAttribute(HtmlTextWriterAttribute.Src,
				Utils.ConvertToImageDir(ImagesDirectory, _liftStartOff, ButtonVertStartOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
			output.AddAttribute(HtmlTextWriterAttribute.Background,
				Utils.ConvertToImageDir(ImagesDirectory, LiftCenterOff, ButtonVertCenterOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.RenderEndTag();
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Height, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_liftGrabberOff");
			output.AddAttribute(HtmlTextWriterAttribute.Src,
				Utils.ConvertToImageDir(ImagesDirectory, _liftGrabberOff, ButtonVertGrabberOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
			output.AddAttribute(HtmlTextWriterAttribute.Background,
				Utils.ConvertToImageDir(ImagesDirectory, LiftCenterOff, ButtonVertCenterOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Height, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_liftEndOff");
			output.AddAttribute(HtmlTextWriterAttribute.Src,
				Utils.ConvertToImageDir(ImagesDirectory, LiftEndOff, ButtonVertEndOffImage, Page, GetType()));
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Height, "0");

			RenderTdWithPlusArrowImage(ref output);
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
		}

		private void RenderSliderHeader(HtmlTextWriter output)
		{
			output.AddAttribute(
				_direction == DirectionLift.Vertical ? HtmlTextWriterAttribute.Height : HtmlTextWriterAttribute.Width,
				Size.ToString());

			output.AddAttribute("onselectstart", "return false;");
			output.AddAttribute("ondragstart", "return false;");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			if(CssClass == string.Empty)
			{
				output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(BackColor));
				if(BorderStyle != BorderStyle.None && BorderStyle != BorderStyle.NotSet)
				{
					output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, BorderStyle.ToString());
					output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(BorderColor));
					output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, BorderWidth.ToString());
				}

				output.AddStyleAttribute("cursor", "default");
			}
			else
			{
				output.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
			}
		}

	    private void RenderTdWithPlusArrowImage(ref HtmlTextWriter output)
	    {
	        output.RenderBeginTag(HtmlTextWriterTag.Td);
	        output.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat(ClientID, ClientID_Postfix));
	        output.AddAttribute(HtmlTextWriterAttribute.Name, string.Concat(ClientID, ClientID_Postfix));
	        output.AddAttribute(HtmlTextWriterAttribute.Src,
	                            Utils.ConvertToImageDir(ImagesDirectory, PlusArrowOffImage, 
	                            ImageName_ArrowDownOff, Page, GetType()));
	        output.AddAttribute(HtmlTextWriterAttribute.Border, BorderAttributeValue_0);
	        output.AddAttribute(Event_OnMouseDown, string.Format("ASD_PlusArrowDown('{0}');", ClientID));
	        output.AddAttribute(Event_OnMouseOut, string.Format("ASD_StopStepPlus('{0}');", ClientID));
	        output.AddAttribute(Event_OnMouseUp, string.Format("ASD_StopStepPlus('{0}');", ClientID));
	        output.RenderBeginTag(HtmlTextWriterTag.Img);
	        output.RenderEndTag();
	        output.RenderEndTag();
	    }

	    #endregion

        #region Properties

        #region Appearance

        /// <summary>
        /// The fore color of the object. Not used in this control.
        /// </summary>
        [
		Browsable(false)
		]
		public override Color ForeColor
		{
			get {return base.BackColor;}
			set {base.BackColor = value;}
		}

		/// <summary>
		/// The font of the object. Not used in this control.
		/// </summary>
		[Browsable(false)]
		public override FontInfo Font
		{
			get {return base.Font;}
		}

		/// <summary>
		/// Gets or sets the image displayed for default minus arrow.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Image displayed for the default minus arrow."),
#if (!FX1_1)
	DefaultValue("")
#else		
		DefaultValue("ArrowUpOn.gif")
#endif		
		]
		public string MinusArrowOnImage
		{
			get
			{
				return _minusArrowOnImage;
			}

			set
			{
				_minusArrowOnImage = value;
			}
		}

		/// <summary>
		/// Gets or sets the image displayed for the up arrow when clicked.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Image displayed for the minus arrow when clicked."),
#if (!FX1_1)
	DefaultValue("")
#else
		DefaultValue("ArrowUpOff.gif")
#endif
		]
		public string MinusArrowOffImage
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, ArrowUpOffImage);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(MinusArrowOffImage), defaultValue);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(MinusArrowOffImage), value);
			}
		}

		/// <summary>
		/// Gets or sets the image displayed for plus arrow when clicked.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Image displayed for the plus arrow when clicked."),
#if (!FX1_1)
	DefaultValue("")
#else		
		DefaultValue("ArrowDownOn.gif")
#endif		
		]
		public string PlusArrowOnImage
		{
			get
			{
				return _plusArrowOnImage;
			}

			set
			{
				_plusArrowOnImage = value;
			}
		}

		/// <summary>
		/// Gets or sets the image displayed for default down arrow.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Image displayed for the default plus arrow."),
#if (!FX1_1)
	DefaultValue("")
#else
		DefaultValue("ArrowDownOff.gif")
#endif
		]
		public string PlusArrowOffImage
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, ArrowDownOffImage);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(PlusArrowOffImage), defaultValue);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(PlusArrowOffImage), value);
			}
		}

		/// <summary>
		/// Gets or sets the back slider image when the state is up.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("The back slider image when the state is up."),
#if (!FX1_1)
	DefaultValue("")
#else				
		DefaultValue("BackgroundVertOff.gif")
#endif		
		]
		public string LiftOff
		{
			get
			{
				return _liftOff;
			}

			set
			{
				_liftOff = value;
			}
		}

		/// <summary>
		/// Gets or sets the back slider image when the state is down.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("The back slider image when the state is down."),
#if (!FX1_1)
	DefaultValue("")
#else
		DefaultValue("BackgroundVertOn.gif")
#endif
		]
		public string LiftOn
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, BackgroundVertOnImage);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(LiftOn), defaultValue);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(LiftOn), value);
			}
		}

		/// <summary>
		/// Gets or sets the start image of the lift.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("The start image of the lift."),
#if (!FX1_1)
	DefaultValue("")
#else				
		DefaultValue("ButtonVertStartOff.gif")
#endif		
		]
		public string LiftStartOff
		{
			get
			{
				return _liftStartOff;
			}

			set
			{
				_liftStartOff = value;
			}
		}

		
		/// <summary>
		/// Gets or sets the center image of the lift.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("The center image of the lift."),
#if (!FX1_1)
	DefaultValue("")
#else				
		DefaultValue("ButtonVertCenterOff.gif")
#endif		
		]
		public string LiftCenterOff
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, ButtonVertCenterOffImage);
				return  ViewStateHelper.GetFromViewState(ViewState, nameof(LiftCenterOff), defaultValue);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(LiftCenterOff), value);
			}
		}

		/// <summary>
		/// Gets or sets the grabber image of the lift.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("The grabber image of the lift."),
#if (!FX1_1)
	DefaultValue("")
#else				
		DefaultValue("ButtonVertGrabberOff.gif")
#endif		
		]
		public string LiftGrabberOff
		{
			get
			{
				return _liftGrabberOff;
			}

			set
			{
				_liftGrabberOff = value;
			}
		}

		/// <summary>
		/// Gets or sets the end image of the lift.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("The end image of the lift."),
#if (!FX1_1)
	DefaultValue("")
#else
		DefaultValue("ButtonVertEndOff.gif")
#endif
		]
		public string LiftEndOff
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, ButtonVertEndOffImage);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(LiftEndOff), defaultValue);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(LiftEndOff), value);
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
				object local = ViewState["License"];
				if (local != null)
					return (string)local;
				else
					return string.Empty;
			}
		
			set
			{
				_license = value;
			}
		}

#endif
*/

		/// <summary>
		/// Gets the current value of the slider.
		/// </summary>
		[
		Bindable(false),
		Browsable(true),
		Description("Gets the current value of the slider.")
		]
		public long Value
		{
			get 
			{
				return _currentValue;
			}

			set
			{
				_currentValue = value;
			}
		}

		#endregion

		#region Behavior

		/// <summary>
		/// Gets or sets the tool tip of the control. Not used in this control.
		/// </summary>
		[
		Browsable(false)
		]
		public override string ToolTip
		{
			get {return base.ToolTip;}
			set {base.ToolTip = value;}
		}

		/// <summary>
		/// Gets or sets the minimum value.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The minimum value."),
		DefaultValue(0)
		]
		public int Min
		{
			get
			{
				return _min;
			}
		
			set
			{
				_min = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum value.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The maximum value."),
		DefaultValue(100)
		]
		public int Max
		{
			get
			{
				return _max;
			}
		
			set
			{
				_max = value;
			}
		}

		/// <summary>
		/// Gets or sets the step of each operation.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The step of each operation."),
		DefaultValue(1)
		]
		public int Step
		{
			get
			{
				return _step;
			}
		
			set
			{
				_step = value;
			}
		}

		/// <summary>
		/// Gets or sets the step of each operation.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicates if the slider must be move directly or step by step."),
		DefaultValue(false)
		]
		public bool DirectStep
		{
			get
			{
				return _directStep;
			}
		
			set
			{
				_directStep = value;
			}
		}

		/// <summary>
		/// Gets or sets the speed between each step when you click down an arrow.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Specifies the speed between each step when you click down an arrow."),
		DefaultValue(150)
		]
		public int SpeedArrow
		{
			get
			{
				return _speedArrow;
			}
		
			set
			{
				_speedArrow = value;
			}
		}

		/// <summary>
		/// Gets or sets the speed between each step when you out the lift.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Specifies the speed between each step when you click out the lift."),
		DefaultValue(20)
		]
		public int SpeedOutLift
		{
			get
			{
				return _speedOutLift;
			}
		
			set
			{
				_speedOutLift = value;
			}
		}

		/// <summary>
		/// Gets or sets the direction of the slider.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("The direction of the slider."),
		DefaultValue(typeof(DirectionLift),"Vertical")
		]
		public DirectionLift Direction
		{
			get
			{
				return _direction;
			}

			set
			{	
				_direction = value;
			}
		}

		#endregion

		#region Disposition

		/// <summary>
		/// The height of the control.
		/// </summary>
		[
		Bindable(false),
		Browsable(false),
		]
		public override Unit Height
		{
			get {return base.Height;}
			set {base.Height = value;}
		}

		/// <summary>
		/// The width of the control.
		/// </summary>
		[
		Bindable(false),
		Browsable(false),
		]
		public override Unit Width
		{
			get {return base.Width;}
			set {base.Width = value;}
		}

		/// <summary>
		/// Gets or sets the size of the control.
		/// </summary>
		[
		Bindable(true),
		Browsable(true),
		DefaultValue(typeof(Unit),"300px"),
		Description("The size of the control.")
		]
		public Unit Size
		{
			get 
			{
				return _size;
			}

			set
			{
				_size = value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the lift.
		/// </summary>
		[
		Bindable(true),
		Browsable(true),
		DefaultValue(typeof(Unit),"30px"),
		Description("The size of the lift.")
		]
		public Unit LiftSize
		{
			get 
			{
				return _liftSize;
			}

			set
			{
				_liftSize = value;
			}
		}

		#endregion

		#region JScript

		/// <summary>
		/// Gets or sets the relative or absolute path to the external ActiveCalendar API javascript file.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the ActiveCalendar render.</remarks>
		[
		Bindable(false),
		Category("JScript"),
		Description("Relative or absolute path to the external ActiveCalendar API javascript file.")	
		]
		public string ExternalScript
		{
			get
			{
				return _externalScript;
			}
			set
			{
				_externalScript = value;
			}
		}	

		/// <summary>
		/// Gets or sets the client side code when the value changes.
		/// </summary>
		[
		Bindable(false),
		Category("JScript"),
		Description("Client side event occurs when the value changes"),
		DefaultValue("")
		]
		public string OnValueChangedClient
		{
			get
			{
				return _onValueChangedClient;
			}

			set
			{
				_onValueChangedClient = value;
			}
		}

		#endregion

		#endregion

		#region Events

		/// <summary>
		/// Raise the <see cref="ValueChanged"/> of the <see cref="Slider"/> control. This allows you to handle the event directly.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected virtual void OnValueChanged(EventArgs e) 
		{
			// Check if someone use our event.
			if (ValueChanged != null)
				ValueChanged(this,e);
		}

		#endregion

		#region IPostBackEventHandler

		/// <summary>
		/// Enables the control to process an event raised when a form is posted to the server.
		/// </summary>
		/// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
		void IPostBackEventHandler.RaisePostBackEvent(String eventArgument)
		{
			//Page.Trace.Write(this.ID, "RaisePostBackEvent...");
			OnValueChanged(EventArgs.Empty);
		}

		#endregion

		#region IPostBackDataHandler

		/// <summary>
		/// Processes post-back data from the control.
		/// </summary>
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>True if the state changes as a result of the post-back, otherwise it returns false.</returns>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			//Page.Trace.Write(this.ID, "LoadPostData...");

			_oldCurrentValue = long.Parse(postCollection[ClientID + "_oldCurrentValue"]);
			_currentValue = long.Parse(postCollection[ClientID + "_currentValue"]);

			if (_oldCurrentValue != _currentValue)
				return true;

			return false;

		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			//Page.Trace.Write(this.ID, "RaisePostDataChangedEvent...");
			OnValueChanged(EventArgs.Empty);
		}

		#endregion
	}

	#endregion
} 
 
