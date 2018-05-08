using System.ComponentModel;
using System.Drawing;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Common.Interface;

namespace ActiveUp.WebControls.Common
{
    public class CoreWebControl : CommonControlProperties, IControl
    {
        private const string ScriptDirectoryKey = "ScriptDirectory";
        private const string ExternalScriptKey = "ExternalScript";

        #region IControl Members

        void IControl.LoadViewState(object savedState)
        {
            LoadViewState(savedState);
        }

        object IControl.SaveViewState()
        {
            return SaveViewState();
        }

        void IControl.TrackViewState()
        {
            TrackViewState();
        }

        #endregion

        #region Properties

		/// <summary>
		/// Gets or sets the relative or absolute path to the external Html TextBox API javascript file.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Html TextBox render.</remarks>
		[
			Bindable(false),
			Category("Behavior"),
			Description("Gets or sets the relative or absolute path to the external Html TextBox API javascript file."),
			Fx1ConditionalDefaultValue("", Define.IMAGES_DIRECTORY)
		]
		public virtual string ImagesDirectory
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, Define.IMAGES_DIRECTORY);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ImagesDirectory), defaultValue);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ImagesDirectory), value);
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the external API javascript file.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the control render.</remarks>
		[
			Bindable(false),
			Category("Behavior"),
			Description("Gets or sets the relative or absolute path to the external Html TextBox API javascript file."),
			Fx1ConditionalDefaultValue("", Define.IMAGES_DIRECTORY)
		]
		public virtual string IconsDirectory
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, Define.IMAGES_DIRECTORY);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(IconsDirectory), defaultValue);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(IconsDirectory), value);
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where input API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is <see cref="string.Empty" />, the external file script is not used and the API is rendered in the page together with the input render.</remarks>
		[
			Bindable(false),
			Category("Behavior"),
			Description("Gets or sets the relative or absolute path to the directory where input API javascript file is."),
			Fx1ConditionalDefaultValue("", Define.SCRIPT_DIRECTORY)
		]
		public string ScriptDirectory
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, Define.SCRIPT_DIRECTORY);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ScriptDirectory), defaultValue);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ScriptDirectory), value);
			}
		}

		protected virtual string ExternalScriptDefault
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the external script directory.
		/// </summary>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the external script directory.")]
		public string ExternalScript
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ExternalScript), ExternalScriptDefault);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ExternalScript), value);
			}
		}

		/// <summary>
		/// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
		/// </summary>
		[Browsable(false)]
		public override string CssClass
		{
			get
			{
				return base.CssClass;
			}
			set
			{
				base.CssClass = value;
			}
		}

		/// <summary>
		/// Gets the font properties associated with the Web server control.
		/// </summary>
		[Browsable(false)]
		public override FontInfo Font
		{
			get
			{
				return base.Font;
			}
		}

		/// <summary>
		/// Gets or sets the foreground color (typically the color of the text) of the Web server control.
		/// </summary>
		[Browsable(false)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the Web server control is enabled.
		/// </summary>
		[Browsable(false)]
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}
		#endregion

	}
}
