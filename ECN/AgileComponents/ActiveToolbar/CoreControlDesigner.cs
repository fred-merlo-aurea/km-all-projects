using System.Web.UI.Design;

namespace ActiveUp.WebControls.ActiveToolbar
{
    public class CoreControlDesigner: ControlDesigner
    {
        private const string DefaultDesignTimeHtmlText = "this should be never displayed.";

        /// <summary>
        /// Gets the HTML that is used to represent an empty control at design time.
        /// </summary>
        /// <returns>
        /// The HTML that is used to represent an empty control at design time. 
        /// By default, this HTML contains the name of the component.
        /// </returns>
        protected override string GetEmptyDesignTimeHtml()
        {
            return CreatePlaceHolderDesignTimeHtml(DefaultDesignTimeHtmlText);
        }
    }
}
