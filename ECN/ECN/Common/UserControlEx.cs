using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.common.classes
{
    public abstract class UserControlEx : UserControl
    {
        protected abstract DataList ImageRepeaterControl { get; }
        protected abstract Image ImagePreviewControl { get; }
        protected abstract LinkButton TabPreviewControl { get; }
        protected abstract LinkButton TabUploadControl { get; }
        protected abstract LinkButton TabBrowseControl { get; }
        protected abstract Panel PanelBrowseControl { get; }
        protected abstract Panel PanelPreviewControl { get; }
        protected abstract Panel PanelUploadControl { get; }

        public string borderWidth
        {
            get
            {
                return ImageRepeaterControl.BorderWidth.ToString();
            }
            set
            {
                int width;

                if (int.TryParse(value, out width))
                {
                    ImageRepeaterControl.BorderWidth = width;
                }
                else
                {
                    throw new InvalidCastException($"{value} cannot be parsed to integer");
                }
            }
        }

        protected void ResetImagePreview()
        {
            ImagePreviewControl.Visible = false;
            ImagePreviewControl.ImageUrl = string.Empty;
        }

        protected void SetControlsVisibility(bool previewVisible, bool panelUploadVisible, bool panelBrowseVisible)
        {
            TabPreviewControl.Visible = previewVisible;
            TabUploadControl.Visible = true;
            TabBrowseControl.Visible = true;

            PanelPreviewControl.Visible = previewVisible;
            PanelUploadControl.Visible = panelUploadVisible;
            PanelBrowseControl.Visible = panelBrowseVisible;
        }
    }
}
