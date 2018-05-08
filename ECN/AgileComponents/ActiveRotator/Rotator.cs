using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="Rotator"/> object.
	/// </summary>
	[
	ToolboxData("<{0}:Rotator runat=server></{0}:Rotator>"),
	CLSCompliantAttribute(true),
	ComVisibleAttribute(true),
    ToolboxBitmap(typeof(Toolbar), "ToolBoxBitmap.Rotator.bmp"),
	ParseChildren(true,"Slides"),
	]
	public class Rotator : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private const string AssignMotion = "motion=";
		private const string AssignOrientation = "orientation=";
		private const string AssignDirection = "direction=";
		private const string AssignBands = "bands=";
		private const string AssignSquaresX = "squaresx=";
		private const string AssignSquaresY = "squaresy=";
		private const string AssignOverlap = "overlap=";
		private const string AssignGradientSize = "gradientsize=";
		private const string AssignWipeStyle = "wipestyle=";
		private const string AssignIrisStyle = "irisstyle=";
		private const string AssignMaxSquare = "maxsquare=";
		private const string AssignSlideStyle = "slidestyle=";
		private const string AssignSmoothStyle = "smoothstyle=";
		private const string AssignStretchStyle = "stretchstyle=";
		private const string LeftDown = "leftdown";
		private const string RightDown = "rightdown";
		private const string LeftUp = "leftup";
		private const string RightUp = "rightup";
		private const string Spokes = "spokes=";
		private const string AssignGridSizeX = "gridsizex=";
		private const string AssignGridSizeY = "gridsizey=";
		private const char Comma = ',';
		private const int IntegerNotSet = -1;
		private const string Zero = "0";
		private const string One = "1";

		private int _speed, _pause, _fadeChars, _frameRate;
		private string _externalScript, _scriptDirectory, _contentFile/*, _license*/;
		private Color _textColor, _fadeColor;
		private RotatorType _type;
		private Transition _transition;
		private TransitionProperties _parameters;

		private object _dataSource;
		private ITemplate _slideTemplate;

		private static string CLIENTSIDE_API;
		private static string SCRIPTKEY = "ACTIVEROTATOR";

#if (LICENSE)
		//private string _license = string.Empty;

		/// <summary>
		/// Used for the license counter.
		/// </summary>
		internal static int _useCounter;
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="Rotator"/> class.
		/// </summary>
		public Rotator()
		{
			_externalScript = string.Empty;
			//_scriptDirectory = "/aspnet_client/ActiveWebControls/" + StaticContainer.VersionString + "/";
#if (!FX1_1)
            _scriptDirectory = string.Empty;
#else
			_scriptDirectory = Define.SCRIPT_DIRECTORY;
#endif
			_speed = 3000;
			_pause = 1000;
			_frameRate = 50;
			_type = RotatorType.Html;
			_fadeChars = 5;
			_textColor = Color.Black;
			_fadeColor = Color.White;
			_contentFile = string.Empty;
			_transition = Transition.Slide;

			this.Width = Unit.Pixel(400);
			this.Height = Unit.Pixel(300);
		}

		/// <summary>
		/// Gets or sets the transition parameters
		/// </summary>
		/// <value>The parameters.</value>
		public TransitionProperties Params
		{
			get
			{
				if (_parameters == null)
					_parameters = new TransitionProperties();
				return _parameters;
			}
			set
			{
				_parameters = value;
			}
		}

		/// <summary>
		/// Gets or sets the contents file.
		/// </summary>
		/// <value>The contents file.</value>
		public string ContentFile
		{
			get
			{
				return _contentFile;
			}
			set
			{
				_contentFile = value;
			}
		}

		/// <summary>
		/// Gets or sets the frame rate.
		/// </summary>
		/// <value>The frame rate.</value>
		public int FrameRate
		{
			get
			{
				return _frameRate;
			}
			set
			{
				_frameRate = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the rotator start automatically.
		/// </summary>
		/// <value><c>true</c> if the rotator start automatically; otherwise, <c>false</c>.</value>
		public bool AutoStart
		{
			get
			{
				if (ViewState["_autoStart"] == null)
					return true;
				return (bool)ViewState["_autoStart"];
			}
			set
			{
				ViewState["_autoStart"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether pause on mouse over.
		/// </summary>
		/// <value><c>true</c> if pause on mouse over; otherwise, <c>false</c>.</value>
		public bool PauseOnMouseOver
		{
			get
			{
				if (ViewState["_pauseOnMouseOver"] == null)
					return true;
				return (bool)ViewState["_pauseOnMouseOver"];
			}
			set
			{
				ViewState["_pauseOnMouseOver"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether random on start.
		/// </summary>
		/// <value><c>true</c> if random on start; otherwise, <c>false</c>.</value>
		public bool RandomStart
		{
			get
			{
				if (ViewState["_randomStart"] == null)
					return false;
				return (bool)ViewState["_randomStart"];
			}
			set
			{
				ViewState["_randomStart"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public RotatorType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		/// <summary>
		/// Gets or sets the transition.
		/// </summary>
		/// <value>The transition.</value>
		public Transition Transition
		{
			get
			{
				return _transition;
			}
			set
			{
				_transition = value;
			}
		}

		/// <summary>
		/// Gets or sets the text color.
		/// </summary>
		/// <value>The text color.</value>
		public Color TextColor
		{
			get
			{
				return _textColor;
			}
			set
			{
				_textColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the fade color.
		/// </summary>
		/// <value>The fade color.</value>
		public Color FadeColor
		{
			get
			{
				return _fadeColor;
			}
			set
			{
				_fadeColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the fade chars.
		/// </summary>
		/// <value>The fade chars.</value>
		public int FadeChars
		{
			get
			{
				return _fadeChars;
			}
			set
			{
				_fadeChars = value;
			}
		}

		/// <summary>
		/// Gets or sets the speed.
		/// </summary>
		/// <value>The speed.</value>
		public int Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed = value;
			}
		}

		/// <summary>
		/// Gets or sets the pause.
		/// </summary>
		/// <value>The pause.</value>
		public int Pause
		{
			get
			{
				return _pause;
			}
			set
			{
				_pause = value;
			}
		}

		/// <summary>
		/// Gets the collection of slides.
		/// </summary>
		[
		Browsable( true ),
		DefaultValue( null ),
		//PersistenceModeAttribute(PersistenceMode.InnerDefaultProperty),
		//DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceModeAttribute(PersistenceMode.InnerDefaultProperty),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		]
		public ControlCollection Slides
		{
			get
			{
				return this.Controls;
			}
			
		}

		private void AddParamString(StringBuilder stringBuilder, string name, object val)
		{
			stringBuilder.Append(name);
			stringBuilder.Append(val.ToString().Replace(",", "."));
			stringBuilder.Append(",");
		}

		private void AddParamString<T>(StringBuilder stringBuilder, string name, T val, T valueShouldNotBe)
		{
			if (!valueShouldNotBe.Equals(val))
			{
				AddParamString(stringBuilder, name, val);
			}
		}

		private string BuildParams()
		{
			if (_parameters == null)
			{
				return string.Empty;
			}

			var prms = new StringBuilder();

			if (!BuildParamsAddParamString(prms))
			{
				switch (_transition)
				{
					case Transition.Strips:
						if (_parameters.Direction != Direction.NotSet)
						{
							var direction = string.Empty;
							switch (_parameters.Direction)
							{
								case Direction.DownLeft:
									direction = LeftDown;
									break;
								case Direction.DownRight:
									direction = RightDown;
									break;
								case Direction.UpLeft:
									direction = LeftUp;
									break;
								case Direction.UpRight:
									direction = RightUp;
									break;
							}

							if (!string.IsNullOrEmpty(direction))
							{
								AddParamString(prms, AssignMotion, direction);
							}
						}

						break;
					case Transition.Wheel:
						AddParamString(prms, Spokes, _parameters.Spokes, IntegerNotSet);
						break;
					case Transition.Zigzag:
					case Transition.Spiral:
						AddParamString(prms, AssignGridSizeX, _parameters.GridSizeX, IntegerNotSet);
						AddParamString(prms, AssignGridSizeY, _parameters.GridSizeY, IntegerNotSet);
						break;
				}
			}

			return prms.ToString().TrimEnd(Comma);
		}

		private bool BuildParamsAddParamString(StringBuilder prms)
		{
			switch (_transition)
			{
				case Transition.Barn:
					AddParamString(prms, AssignMotion, _parameters.Motion, Motion.NotSet);
					AddParamString(prms, AssignOrientation, _parameters.Orientation, Orientation.NotSet);
					break;
				case Transition.Blinds:
					AddParamString(prms, AssignDirection, _parameters.Direction, Direction.NotSet);
					AddParamString(prms, AssignBands, _parameters.Bands, IntegerNotSet);
					break;
				case Transition.CheckerBoard:
					AddParamString(prms, AssignDirection, _parameters.Direction, Direction.NotSet);
					AddParamString(prms, AssignSquaresX, _parameters.SquaresX, IntegerNotSet);
					AddParamString(prms, AssignSquaresY, _parameters.SquaresY, IntegerNotSet);
					break;
				case Transition.Fade:
					AddParamString(prms, AssignOverlap, _parameters.Overlap, IntegerNotSet);
					break;
				case Transition.GradientWipe:
					AddParamString(prms, AssignMotion, _parameters.Motion, Motion.NotSet);
					AddParamString(prms, AssignGradientSize, _parameters.GradientSize, IntegerNotSet);
					if (_parameters.Orientation != Orientation.NotSet)
					{
						AddParamString(prms, AssignWipeStyle, _parameters.Orientation == Orientation.Horizontal ? Zero : One);
					}

					break;
				case Transition.Iris:
					AddParamString(prms, AssignMotion, _parameters.Motion, Motion.NotSet);
					AddParamString(prms, AssignIrisStyle, _parameters.IrisStyle, IrisStyle.NotSet);
					break;
				case Transition.Pixelate:
					AddParamString(prms, AssignMaxSquare, _parameters.MaxSquare, IntegerNotSet);
					break;
				case Transition.RadialWipe:
					AddParamString(prms, AssignWipeStyle, _parameters.WipeStyle, WipeStyle.NotSet);
					break;
				case Transition.RandomBars:
					AddParamString(prms, AssignOrientation, _parameters.Orientation, Orientation.NotSet);
					break;
				case Transition.Slide:
					AddParamString(prms, AssignBands, _parameters.Bands, IntegerNotSet);
					AddParamString(prms, AssignSlideStyle, _parameters.SlideStyle, SlideStyle.NotSet);
					break;
				case Transition.SmoothScroll:
					AddParamString(prms, AssignDirection, _parameters.Direction, Direction.NotSet);
					AddParamString(prms, AssignSmoothStyle, _parameters.SmoothStyle, SmoothStyle.NotSet);
					break;
				case Transition.Stretch:
					AddParamString(prms, AssignStretchStyle, _parameters.StretchStyle, StretchStyle.NotSet);
					break;
				default:
					return false;
			}

			return true;
		}

		/// <summary>
		/// Loads the contents file.
		/// </summary>
		public void LoadContentFile()
		{
			this.LoadContentFile(this.ContentFile);
		}

		/// <summary>
		/// Loads the contents file.
		/// </summary>
		/// <param name="location">The file name location.</param>
		public void LoadContentFile(string location)
		{
			Stream stream = null;

			if (!System.IO.Path.IsPathRooted(location))
				location = Page.Server.MapPath(location);

			try
			{
				stream = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			catch
			{
				throw new Exception("Error while opening content file");
			}

			try
			{
				XmlReader reader = new XmlTextReader(stream);
				XmlDocument document = new XmlDocument();
				document.Load(reader);
				if ((document.DocumentElement != null) && (document.DocumentElement.LocalName == "Content"))
				{
					for (XmlNode node1 = document.DocumentElement.FirstChild; node1 != null; node1 = node1.NextSibling)
					{
						if (node1.LocalName.Equals("SlideData"))
						{
							for (XmlNode node2 = node1.FirstChild; node2 != null; node2 = node2.NextSibling)
							{
								if (node2.NodeType == XmlNodeType.Element
									|| node2.NodeType == XmlNodeType.CDATA
									|| node2.NodeType == XmlNodeType.Text)
								{
									this.Controls.Add(new Slide(node2.InnerText));
								}
							}
						}
					}
				}
			}
			catch
			{
				throw new Exception("Error while parsing content file");
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}
			if (this.Controls.Count == 0)
			{
				throw new Exception("No content found");
			}
		}

		#region Behavior Properties
		/// <summary>
		/// Gets or sets the relative or absolute path to the external Html TextBox API javascript file.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Html TextBox render.</remarks>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the external Html TextBox API javascript file.")	]
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
		/// Gets or sets the relative or absolute path to the directory where Rotator API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Rotator render.</remarks>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the directory where Rotator API javascript file is."),
#if (!FX1_1)
        DefaultValue("")
#else
		DefaultValue(Define.SCRIPT_DIRECTORY)
#endif
		]
		public string ScriptDirectory
		{
			get
			{
				/*if (_scriptDirectory == null)
					return "/aspnet_client/ActiveWebControls/" + ActiveUp.WebControls.Common.StaticContainer.VersionString + "/";
					return "/aspnet_client/ActiveWebControls/3_3_2011_0/";*/
				return _scriptDirectory;
			}
			set
			{
				_scriptDirectory = value;
			}
		}
		#endregion

		#region Databinding

		/// <summary>
		/// Binds a data source to the invoked server control and all its child
		/// controls.
		/// </summary>
		public override void DataBind()
		{
			this.OnDataBinding(EventArgs.Empty);

			Controls.Clear();
			this.ClearChildViewState();
			this.TrackViewState();
			this.CreateControlHierarchy(true);
			this.ChildControlsCreated = true;
		}

		/// <summary>
		/// Notifies server controls that use composition-based implementation to create any child
		/// controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			//Controls.Clear();
			if (ViewState["_dataSource"] != null)
				CreateControlHierarchy(false);	
		}

		/*protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);
			this.Controls.Clear();
			base.ClearChildViewState();
			this.CreateControlHierarchy(true);
			base.ChildControlsCreated = true;
		}*/
 
		/// <summary>
		/// Creates the control hierarchy.
		/// </summary>
		/// <param name="useDataSource">if set to <c>true</c> if you want to use the data source.</param>
		protected virtual void CreateControlHierarchy(bool useDataSource)
		{
			IEnumerable dataSource = null;

			if (useDataSource) 
				dataSource = GetDataSource();
			else
			{
				this.DataSource = ViewState["_dataSource"];
				dataSource = GetDataSource();
				//dataSource = (IEnumerable)ViewState["_dataSource"];
			}

			if (dataSource != null) 
			{
				foreach (object dataItem in dataSource) 
				{
					Slide slide = new Slide();
					
					this._slideTemplate.InstantiateIn(slide);
					slide.DataItem = dataItem;
					slide.DataBind();
					
					Controls.Add(slide);
					this.OnSlideDataBound(new RotatorSlideEventArgs(slide));
				}

				if (EnableViewState)
					ViewState["_dataSource"] = this.DataSource;
			}
		}

		/// <summary>
		/// Gets or sets the data source.
		/// </summary>
		/// <value>The data source.</value>
		[Bindable(true),
		DefaultValue((string) null),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual object DataSource
		{
			get
			{
				return this._dataSource;
			}
			set
			{
				if (((value != null) && !(value is IListSource)) && !(value is IEnumerable))
					throw new ArgumentException("Invalid Datasource Type.", this.ID);
				this._dataSource = value;
			}
		}
 
		/// <summary>
		/// Gets or sets the data member.
		/// </summary>
		/// <value>The data member.</value>
		public virtual string DataMember
		{
			get
			{
				object dateMember = this.ViewState["DataMember"];
				if (dateMember != null)
					return (string) dateMember;
				return string.Empty;
			}
			set
			{
				this.ViewState["DataMember"] = value;
			}
		}

		/// <summary>
		/// Gets the data source.
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerable GetDataSource() 
		{
			if (_dataSource == null) 
			{
				return null;
			}
            
			IEnumerable resolvedDataSource = _dataSource as IEnumerable;
			if (resolvedDataSource != null) 
			{
				return resolvedDataSource;
			}

			IListSource listSource = _dataSource as IListSource;
			if (listSource != null) 
			{
				IList memberList = listSource.GetList();

				if (listSource.ContainsListCollection == false) 
				{
					return (IEnumerable)memberList;
				}

				ITypedList typedMemberList = memberList as ITypedList;
				if (typedMemberList != null) 
				{
					PropertyDescriptorCollection propDescs = typedMemberList.GetItemProperties(new PropertyDescriptor[0]);
					PropertyDescriptor memberProperty = null;
                    
					if ((propDescs != null) && (propDescs.Count != 0)) 
					{
						string dataMember = DataMember;

						if (dataMember.Length == 0) 
						{
							memberProperty = propDescs[0];
						}
						else 
						{
							memberProperty = propDescs.Find(dataMember, true);
						}

						if (memberProperty != null) 
						{
							object listRow = memberList[0];
							object list = memberProperty.GetValue(listRow);

							if (list is IEnumerable) 
							{
								return (IEnumerable)list;
							}
						}
						throw new Exception("A list corresponding to the selected DataMember was not found. Please specify a valid DataMember or bind to an explicit datasource (a DataTable for exemple).");
					}

					throw new Exception("The selected data source did not contain any data members to bind to.");
				}
			}

			return null;
		}        

		/// <summary>
		/// Gets or sets the slide template.
		/// </summary>
		/// <value>The slide template.</value>
		[PersistenceMode(PersistenceMode.InnerProperty), DefaultValue((string) null), TemplateContainer(typeof(Slide)), Browsable(false)]
		public virtual ITemplate SlideTemplate
		{
			get
			{
				return this._slideTemplate;
			}
			set
			{
				this._slideTemplate = value;
			}
		}
 
		/// <summary>
		/// Raises the slide data bound event.
		/// </summary>
		/// <param name="e">The <see cref="ActiveUp.WebControls.RotatorSlideEventArgs"/> instance containing the event data.</param>
		protected virtual void OnSlideDataBound(RotatorSlideEventArgs e)
		{
			if (SlideDataBound != null)
			{
				SlideDataBound(this, e);
			}
		}

		/// <summary>
		/// The SlideDataBound event handler.
		/// </summary>
		public event RotatorSlideEventHandler SlideDataBound;

		/// <summary>
		/// Creates the slide.
		/// </summary>
		/// <returns></returns>
		protected virtual Slide CreateSlide()
		{
			return new Slide();
		}

		/// <summary>
		/// Object event for the slide data binding.
		/// </summary>
		public static readonly object EventSlideDataBound;

		/// <summary>
		/// Event handler conatains the slide.
		/// </summary>
		public delegate void RotatorSlideEventHandler(object sender, RotatorSlideEventArgs e);

 
		#endregion

		#region .NET API
		/// <summary>
		/// Registers the API script block.
		/// </summary>
		/// <param name="page">The page.</param>
		public virtual void RegisterAPIScriptBlock(System.Web.UI.Page page) 
		{
			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript.TrimEnd() == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveRotator.js"));
#else
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveRotator.js");
					
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
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveRotator.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}

			/*string startupString = "<script language='javascript'>\n";
			startupString += "var atv_" + this.ClientID + "_de = '" + this.Icons.Default + "'\n";
			startupString += "var atv_" + this.ClientID + "_ex = '" + this.Icons.Expanded + "'\n";
			startupString += "var atv_" + this.ClientID + "_co = '" + this.Icons.Collapsed + "'\n";
			startupString += "</script>\n";

			page.RegisterStartupScript("ActiveTree_Startup_" + this.ClientID, startupString);*/

            if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_startup"))
            {
                string startupString = string.Empty;
                startupString += "<script>\n";
                startupString += "// Test if the client script is present.\n";
                startupString += "try\n{\n";
                startupString += "ACR_testIfScriptPresent();\n";
                //startupString += "}\ncatch (e) \n{\nalert('Could not find external script file. Please Check the documentation.');\n}\n";
                startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";
                startupString += "</script>\n";

                page.RegisterClientScriptBlock(SCRIPTKEY + "_startup", startupString);
            }
		}

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary>
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);
			
			// Check if a content file is specified
			if (this.ContentFile != string.Empty)
				this.LoadContentFile(this.ContentFile);

			// Register the client side API
			RegisterAPIScriptBlock(this.Page);

			// Adjust default speed;
			if (this.Speed == 0)
			{
				if (this.Type == RotatorType.Html)
					this.Speed = 2000;
				else
					this.Speed = 50;
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
#if (LICENSE)
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.ACR, 4, Edition.S1));
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
				RenderRotator(output);
			}

#else
			RenderRotator(output);
#endif
		}

		/// <summary>
		/// Renders the rotator.
		/// </summary>
		/// <param name="output">The output.</param>
		protected void RenderRotator(HtmlTextWriter output) 
		{
			HtmlTextWriter writer = GetCorrectTagWriter(output);
			this.AddAttributesToRender(writer);
			writer.AddStyleAttribute("overflow", "hidden");
			writer.AddStyleAttribute("position", "relative");
				
			// prepare onmouse actions
			if (PauseOnMouseOver)
			{
				writer.AddAttribute("onmouseover", "ACR_stopRotator('" + this.ClientID + "');");//string.Format("ACR_StopRotator('{0}')", this.ClientID));
				writer.AddAttribute("onmouseout", "ACR_startRotator('" + this.ClientID + "');");// string.Format("ACR_StartRotator('{0}'}", this.ClientID));
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			
			//base.Render(output);
			if (this.Transition != Transition.SmoothScroll || this.Type == RotatorType.Ticker)
				writer.RenderEndTag();
			
			if (this.Controls.Count == 0 && System.Web.HttpContext.Current != null)
				throw new Exception("No slides defined.");

			int index=0,index2=0;
				
			if (this.RandomStart)
			{
				System.Random rnd = new System.Random();
				index2 = rnd.Next(-1,this.Controls.Count);
			}

			for(index=0;index<this.Controls.Count;index++)
			{
				this.Controls[index2].ID = "Slide" + index.ToString();
				this.Controls[index2].RenderControl(writer);
				index2++;

				if (index2>=this.Controls.Count)
					index2=0;
			}

			if (this.Transition == Transition.SmoothScroll && this.Type != RotatorType.Ticker)
				writer.RenderEndTag();

			 
			//this.RenderChildren(output);
			//output.Write(string.Format("<input type=\"hidden\" value=\"0\" id=\"{0}_slidePos\">", this.ClientID));
			//output.Write(string.Format("<input type=\"hidden\" value=\"0\" id=\"{0}_tickerPos\">", this.ClientID));

			string startup = @"<script language='javascript'>
{0}_slidesLen={1};
{0}_pause={2};
{0}_speed={3};
{0}_fps={11};
{0}_transition='{4}';
{0}_params='{9}';
{0}_aParams={0}_params.split(',');
{0}_textColor='{5}';
{0}_fadeColor='{6}';
{0}_fadeChars={7};
{0}_fadeColors=new Array({0}_fadeChars);
{0}_positions=new Array(4);
{0}_positions[0]={0}_positions[1]={0}_positions[2]=0;
{0}_scrollPosX=new Array();
{0}_scrollPosY=new Array();
{0}_useTicker={8};
{0}_positions[3]={12};//0=stopped,1=started,2=paused
var ACR_filtersAllowed = (document.getElementById && document.body.filters);
if ({0}_useTicker)
	ACR_initTicker('{0}', {10});
else
	ACR_initRotator('{0}', {10});
</script>";

			startup = string.Format(startup,
				this.ClientID, this.Controls.Count.ToString(), this.Pause.ToString(),
				this.Speed.ToString(), this.Transition.ToString().ToLower(),
				Utils.Color2Hex(this.TextColor), Utils.Color2Hex(this.FadeColor),
				this.FadeChars.ToString(), (this.Type == RotatorType.Ticker ? "true" : "false"),
				this.BuildParams(), (this.AutoStart && this.Controls.Count > 1 ? "true" : "false"),
				this.FrameRate.ToString(), (this.AutoStart ? One : Zero));

			Page.RegisterStartupScript(SCRIPTKEY + "_Startup" + this.ClientID, startup);
		}

		private HtmlTextWriter tagWriter;
		private HtmlTextWriter GetCorrectTagWriter( HtmlTextWriter writer ) 
		{
			if ( this.tagWriter != null ) return this.tagWriter;

			if ( writer is System.Web.UI.Html32TextWriter ) 
			{
				this.tagWriter =  new HtmlTextWriter( writer.InnerWriter );
			} 
			else 
			{
				this.tagWriter = writer;
			}
			return this.tagWriter;
		}
		#endregion

	}
}
