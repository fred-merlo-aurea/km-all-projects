using System.Web.UI.WebControls;

namespace ecn.communicator.main.blasts
{
    public class CheckTextBoxValidationPair
    {
        public readonly CheckBox CheckBox;

        public readonly TextBox TextBox;

        public CheckTextBoxValidationPair(CheckBox checkBox, TextBox textBox)
        {
            CheckBox = checkBox;
            TextBox = textBox;
        }
    }
}