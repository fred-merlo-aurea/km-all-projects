using System.ComponentModel;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Common.Interface;

namespace ActiveUp.WebControls.Common
{
	public class CommonControlProperties : ViewStateManager
	{
		private const string EnableSslKey = "_enableSsl";

		/// <summary>
		/// Set to true if you need to use the control in a secure web page.
		/// </summary>
		[Bindable(false),
		Category("Behavior"),
		DefaultValue(false),
		Description("Set it to true if you need to use the control in a secure web page.")]
		public bool EnableSsl
		{
			get
			{
				var enableSsl = ViewState[EnableSslKey];
				if (enableSsl != null)
				{
					var isEnableSsl = false;
					bool.TryParse(enableSsl.ToString(), out isEnableSsl);
					return isEnableSsl;
				}
				else
				{
					return false;
				}
			}
			set
			{
				ViewState[EnableSslKey] = value;
			}
		}
	}
}