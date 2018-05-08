using System;
using System.Text;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Collections.Specialized;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	#region ToolSpellChecker

	/// <summary>
	/// Represents a <see cref="ToolSpellChecker"/> object.
	/// </summary>
	[Serializable]
	[ToolboxItem(false)]
	public class ToolSpellChecker : ToolButton, IPostBackEventHandler,IPostBackDataHandler
	{
		#region Fields

		private ActiveUp.WebControls.SpellChecker _spellChecker = new ActiveUp.WebControls.SpellChecker();

		private LabelValueCollection _spellValues = new LabelValueCollection();
		private string _names = string.Empty;
		private bool _showPopup = false;
		private bool _showNothing = false;
		private bool _tmpShowNothing = false;

		#endregion

		#region Constants

		/// <summary>
		/// Indicates if the postback becomes from an ajax panel.
		/// </summary>
		private const string _PARAMETER_IS_CALLBACK_ = "HTB_IsCallBack";

		/// <summary>
		/// Client id of the callback.
		/// </summary>
		private const string _PARAMETER_CLIENT_ID = "HTB_ClientId";

		private const string _PARAMETER_CONTENTS_ = "HTB_Contents";
		private const string SpellCheckerText = "SpellChecker";
		private const string SpellOffImage = "spell_off.gif";
		private const string SpellOverImage = "spell_over.gif";
		private const string TitleText = "Spell Checker";

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSpellChecker"/> class.
		/// </summary>
		public ToolSpellChecker() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSpellChecker"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolSpellChecker(string id) : base(id)
		{ 
			_Init(id);
		}

		#endregion

		#region Init

		private void _Init(string id)
		{
			InitToolButton(
			    string.IsNullOrEmpty(id) ? $"{ToolText}{SpellCheckerText}{Editor.indexTools++}" : id,
				Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, SpellOffImage),
				Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, SpellOverImage),
				TitleText,
				false);

			SetPopupContents(105, 305, true, false);

			string content = string.Empty;
			
			content += "<div>";
			content += "<table cellpadding=0 cellspacing=0>";
			content += "  <tr>";
			content += "     <td colspan=2><b>$NOTINDIC$ :</b></td>";
			content += "  </tr>";
			content += "  <tr>";
			content += "     <td colspan=2><input id=\'$EDITOR_ID$_NotInDictionary\' name=\'$EDITOR_ID$_NotInDictionary\' type=textbox style=\'width:100%\'></td>";
			content += "  </tr>";
			content += "   <tr>";
			content += "      <td>";
			content += "	<table>";
			content += "	    <tr>";
			content += "  		<td><b>$CHANGETO$ :</b></td>";
			content += "	    </tr>";
			content += "	    <tr>";
			content += "		<td><input id=\'$EDITOR_ID$_ChangeTo\' type=textbox style=\'width:180px;\'></td>";
			content += "	    </tr>";
			content += "	    <tr>";
			content += "		<td><b>$SUGGESTION$:</b></td>";
			content += "	    </tr>";
			content += "	    <tr>";
			content += "		<td>";
			content += "		  <select id=\'$EDITOR_ID$_Select\' onchange=\\\"HTB_SC_OnChangeChoice('$EDITOR_ID$',this);\\\" style=\'width:180px;height:88px;visibility:hidden\' size=\'5\'>";
			content += "		  </select>";
			content += "		</td>";
			content += "	    </tr>";
			content += "	</table>";
			content += "      </td>";
			content += "      <td valign=top>";
			content += "	<table>";
			content += "		<tr><td>&nbsp;</td></tr>";
			content += "		<tr>";
			content += "			<td><input type=button value=\'$BUTTON_IGNORE$\' style=\'width:90px\' onclick=\\\"HTB_SC_Ignore('$EDITOR_ID$');\\\"></td>";
			content += "			<td><input type=button value=\'$BUTTON_IGNORE_ALL$\' style=\'width:90px\' onclick=\\\"HTB_SC_IgnoreAll('$EDITOR_ID$');\\\"></td>";
			content += "		</tr>";
			content += "		<tr>";
			content += "			<td><input type=button value=\'$BUTTON_REPLACE$\' style=\'width:90px\' onclick=\\\"HTB_SC_Replace('$EDITOR_ID$');\\\"></td>";
			content += "			<td><input type=button value=\'$BUTTON_REPLACE_ALL$\' style=\'width:90px\' onclick=\\\"HTB_SC_ReplaceAll('$EDITOR_ID$');\\\"></td>";
			content += "		</tr>";
			content += "		<tr>";
			content += "			<td><input type=button value=\'$BUTTON_CLOSE$\' style=\'width:90px\' onclick=\\\"HTB_SC_Close('$POPUP_ID$');\\\"></td>";
			content += "		</tr>";
			content += "	</table>";
			content += "      </td>";
			content += "   </tr>";
			content += "</table>";
			content += "</div>";

			this.PopupContents.ContentText = content;

			this.ClientSideClick = "$POST_BACK$;";
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether show popup when process ended.
		/// </summary>
		/// <value><c>true</c> if show popup when process ended; otherwise, <c>false</c>.</value>
		public bool ShowPopup 
		{
			get
			{
				/*object local = ViewState["_showPopup"];
				if (local != null)
					return (bool)local;
				return false;*/
				return _showPopup;
			}

			set
			{
				//ViewState["_showPopup"] = value;
				_showPopup = value;
			}
		}

		/// <summary>
		/// Gets or sets the icons directory.
		/// </summary>
		/// <value>The icons directory.</value>
		public string IconsDirectory
		{
			get
			{
				object local = ViewState["_iconsDirectory"];
				if (local != null)
					return (string)local;
				else
					return Define.IMAGES_DIRECTORY;
			}

			set
			{
				ViewState["_iconsDirectory"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the dictionary file.
		/// </summary>
		/// <value>The dictionary file.</value>
		public string DictionaryFile
		{
			get
			{
				object dictionaryFile = ViewState["_dictionaryFile"];
				if (dictionaryFile != null)
					return (string)dictionaryFile;
				else
					return "englishwords.txt";
			}

			set
			{
				ViewState["_dictionaryFile"] = value;
			}
		}



		/// <summary>
		/// Indicates if a callback occurs from the panel.
		/// </summary>
		internal bool IsCallback
		{
			get
			{
				bool isCallBack;
				try
				{
					isCallBack = bool.Parse(HttpRequest.Params[_PARAMETER_IS_CALLBACK_]);
					/*&& HttpRequest.Params[_PARAMETER_CLIENT_ID] == this.ClientID;*/
				}
				catch
				{
					isCallBack = false;
				}
				
				return isCallBack;
			}
		}

		/// <summary>
		/// HTTP response information from the callback.
		/// </summary>
		private HttpResponse HttpResponse 
		{
			get
			{
				return System.Web.HttpContext.Current.Response;
			}
		}

		/// <summary>
		/// HTTP request information sent by the client.
		/// </summary>
		private HttpRequest HttpRequest 
		{
			get
			{
				return System.Web.HttpContext.Current.Request;				
			}
		}



		#endregion

		#region Methods

		#region Render

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.spell_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.spell_over.gif");
#endif

            this.RenderControl(output);
        }

		/// <summary>
		/// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.spell_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.spell_over.gif");
#endif
			base.OnPreRender(e);

			if (base.Page != null)
			{
				Page.RegisterRequiresPostBack(this);
				Page.RegisterHiddenField(UniqueID,DateTime.Now.ToString());
			}
				
			// Get Parent Editor
			Editor editor = (Editor)this.Parent.Parent.Parent;
			if (editor != null)
			{
				
				this.ClientSideClick = this.ClientSideClick.Replace("$POST_BACK$", string.Format("HTB_DoCallBack('{0}')",editor.ClientID));
				this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$", editor.ClientID);
				this.ClientSideClick = this.ClientSideClick.Replace("$POPUP_ID$", PopupContents.ClientID);
			}

			this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$EDITOR_ID$", editor.ClientID);
			this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$POPUP_ID$",this.PopupContents.ClientID);

			if (IsCallback)
			{
				this.PopupContents.ShowedOnStart = true;			
			}

		}

		/// <summary>
		/// Render the tool to the specified HtmlTextWriter object. Usually a Page.
		/// </summary>
		/// <param name="output"></param>
		protected override void RenderImage(HtmlTextWriter output)
		{
	          base.RenderImage(output);
		}

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(System.Web.UI.HtmlTextWriter output)
		{
			Editor editor = (Editor)this.Parent.Parent.Parent;

			if (IsCallback) 
			{
				StringWriter stringWriter = new StringWriter();
				output = new HtmlTextWriter(stringWriter);

				if (_showNothing) 
				{
					
					output.Write("alert('Spell checker process terminated');");
					_showNothing = false;
				}
				else
				{
					output.Write(string.Format("if (!document.getElementById('{0}_words'))\n",editor.ClientID));
					output.Write("{\n");
					output.Write("var words = document.createElement('input');\n");
					output.Write("words.setAttribute('type','hidden');\n");
					output.Write(string.Format("words.setAttribute('id', '{0}_words');\n",editor.ClientID));
					output.Write(string.Format("words.setAttribute('name', '{0}_words');\n",editor.ClientID));
					output.Write(string.Format("words.setAttribute('value', {0});\n",_names));
					output.Write("document.body.appendChild(words);\n");
					output.Write("}\n");
					output.Write("else\n");
					output.Write("{\n");
					output.Write(string.Format("document.getElementById('{0}_words').value = {1};\n",editor.ClientID,_names));
					output.Write("}\n");

					ArrayList writtenInPage = new ArrayList();
					foreach(LabelValueCollectionItem label in _spellValues) 
					{
						bool alreadyWritten = false;

						foreach(string name in writtenInPage) 
						{
							if (name.ToUpper() == label.Label.ToUpper())
							{
								alreadyWritten = true;
								break;
							}
						}

						if (alreadyWritten == false) 
						{
							output.Write("var variant = document.createElement('input');\n");
							output.Write("variant.setAttribute('type','hidden');\n");
							output.Write(string.Format("variant.setAttribute('id', '{0}_Variants_{1}');\n",editor.ClientID,label.Label.ToUpper()));
							output.Write(string.Format("variant.setAttribute('name', '{0}_Variants_{1}');\n",editor.ClientID,label.Label.ToUpper()));
							output.Write(string.Format("variant.setAttribute('value', '{0}');\n",label.Value));
							output.Write("document.body.appendChild(variant);\n");

							writtenInPage.Add(label.Label);
						}
					}
					//output.Write(string.Format("alert(document.getElementById('{0}_words').value);",ClientID));
					output.Write(string.Format("HTB_InitSpellChecker('{0}','{1}');\n",editor.ClientID,PopupContents.ClientID));
				}

				System.Web.HttpContext.Current.Response.Clear();
				System.Web.HttpContext.Current.Response.StatusCode = 200;
				System.Web.HttpContext.Current.Response.StatusDescription = "OK";
				System.Web.HttpContext.Current.Response.Write(stringWriter);
				System.Web.HttpContext.Current.Response.Flush();
				System.Web.HttpContext.Current.Response.End();
			}

			else 
			{
				base.Render(output);
			}


		}

		#endregion

		#region IPostBackEventHandler

		/// <summary>
		/// Enables the control to process an event raised when a form is posted to the server.
		/// </summary>
		/// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
		void IPostBackEventHandler.RaisePostBackEvent(String eventArgument)
		{
			Page.Trace.Warn(this.ID, "RaisePostBackEvent...");
		}

		#endregion

		#region IPostBackDataHandler

		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			Page.Trace.Warn(this.ID, "LoadPostData...");
						
			_names = string.Empty;
			_spellValues.Clear();
			
			Editor editor = (Editor)this.Parent.Parent.Parent;
			string contents = System.Web.HttpContext.Current.Request.Params[_PARAMETER_CONTENTS_];
			if (editor != null && contents != null && contents != string.Empty)
			{
				_spellChecker.DictionaryFile = DictionaryFile;
				_spellChecker.GetTextFromString(contents);
				_spellChecker.CheckAllWords();

				_names += "'";
				foreach (Word wrd in _spellChecker.WordsCollection )
				{
					if ( !wrd.IsTrue )
					{
						_names += /*"'" +*/ wrd.CurrentWord + /*"',"*/",";
						LabelValueCollectionItem item = new LabelValueCollectionItem();
						item.Label = wrd.CurrentWord;
						
						foreach (string trueWord in wrd.TrueWords)
						{
							item.Value +=  trueWord + ";";
						}
                        item.Value = item.Value.TrimEnd(';');

						_spellValues.Add(item);
					}
				}

				_names = _names.TrimEnd(',');
				_names += "'";

				_names = _names.TrimEnd(',');
			}

			if (postCollection[editor.ClientID] == string.Empty || _names == string.Empty || _names == @"''")
				_tmpShowNothing = true;
			else
				_tmpShowNothing = false;

			if (ShowPopup == false && _tmpShowNothing == false)
				ShowPopup = true;

			if (_tmpShowNothing)
				_showNothing = true;

			_tmpShowNothing = false;

			return true;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			Page.Trace.Warn(this.ID, "RaisePostDataChangedEvent...");
		}

		#endregion	

		#endregion
	}

	#endregion
}
