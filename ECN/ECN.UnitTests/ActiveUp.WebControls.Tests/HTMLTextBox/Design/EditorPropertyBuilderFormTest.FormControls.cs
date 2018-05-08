using System.Web.UI.WebControls;
using System.Windows.Forms;
using ActiveUp.WebControls.Tests.Helper;
using RadioButton = System.Windows.Forms.RadioButton;
using TextBox = System.Windows.Forms.TextBox;

namespace ActiveUp.WebControls.Tests.HTMLTextBox.Design
{
    public partial class EditorPropertyBuilderFormTest
    {
        private TextBox _tbTabIndex;
        private TextBox _tbMaxLength;
        private TextBox _tbDesignIcon;
        private TextBox _tbDesignLabel;
        private TextBox _tbHtmlIcon;
        private TextBox _tbHtmlLabel;
        private TextBox _tbPreviewIcon;
        private TextBox _tbPreviewLabel;
        private TextBox _tbRows;
        private TextBox _tbCols;
        private TextBox _tbCssClass;

        private RadioButton _rbNone;
        private RadioButton _rbCheckbox;
        private RadioButton _rbTabs;
        private RadioButton _rbDesign;
        private RadioButton _rbHtml;
        private RadioButton _rbPreview;

        private CheckedListBox _clbBehavior;
        private Style _style;
        private PropertyGrid _pgStyle;

        private void ReteriveControls()
        {
            _tbTabIndex = (TextBox)_editorForm.GetFieldValue(nameof(_tbTabIndex));
            _tbMaxLength = (TextBox)_editorForm.GetFieldValue(nameof(_tbMaxLength));
            _tbDesignIcon = (TextBox)_editorForm.GetFieldValue(nameof(_tbDesignIcon));
            _tbDesignLabel = (TextBox)_editorForm.GetFieldValue(nameof(_tbDesignLabel));
            _tbHtmlIcon = (TextBox)_editorForm.GetFieldValue(nameof(_tbHtmlIcon));
            _tbHtmlLabel = (TextBox)_editorForm.GetFieldValue(nameof(_tbHtmlLabel));
            _tbPreviewIcon = (TextBox)_editorForm.GetFieldValue(nameof(_tbPreviewIcon));
            _tbPreviewLabel = (TextBox)_editorForm.GetFieldValue(nameof(_tbPreviewLabel));
            _tbRows = (TextBox)_editorForm.GetFieldValue(nameof(_tbRows));
            _tbCols = (TextBox)_editorForm.GetFieldValue(nameof(_tbCols));
            _tbCssClass = (TextBox)_editorForm.GetFieldValue(nameof(_tbCssClass));

            _rbNone = (RadioButton)_editorForm.GetFieldValue(nameof(_rbNone));
            _rbCheckbox = (RadioButton)_editorForm.GetFieldValue(nameof(_rbCheckbox));
            _rbTabs = (RadioButton)_editorForm.GetFieldValue(nameof(_rbTabs));
            _rbDesign = (RadioButton)_editorForm.GetFieldValue(nameof(_rbDesign));
            _rbHtml = (RadioButton)_editorForm.GetFieldValue(nameof(_rbHtml));
            _rbPreview = (RadioButton)_editorForm.GetFieldValue(nameof(_rbPreview));

            _clbBehavior = (CheckedListBox)_editorForm.GetFieldValue(nameof(_clbBehavior));
            _style = (Style)_editorForm.GetFieldValue(nameof(_style));
            _pgStyle = (PropertyGrid)_editorForm.GetFieldValue(nameof(_pgStyle));
        }
    }
}