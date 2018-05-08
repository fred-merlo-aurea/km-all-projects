using System.Web.UI.WebControls;

namespace ecn.communicator.main.blasts
{
    public class CheckValidaters
    {
        public readonly CheckBox CheckBox;

        public readonly RequiredFieldValidator RequiredFieldValidator;

        public readonly RangeValidator RangeValidator;

        public CheckValidaters(
            CheckBox checkBox, 
            RequiredFieldValidator requiredFieldValidator, 
            RangeValidator rangeValidator)
        {
            CheckBox = checkBox;
            RequiredFieldValidator = requiredFieldValidator;
            RangeValidator = rangeValidator;
        }
    }
}