using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace KM.Framework.Web.WebForms.EmailProfile
{
    partial class EmailProfileBaseBaseControl
    {
        protected Panel FieldsValidationPanel;
        protected CustomValidator EmailAddress_Validator;
        protected CustomValidator Voice_Validator;
        protected CustomValidator State_Validator;
        protected CustomValidator Zip_Validator;
        protected CustomValidator DT_BirthDate;
        protected CustomValidator DT_UserEvent1Date;
        protected CustomValidator DT_UserEvent2Date;
        protected Label MessageLabel;
        protected TextBox EmailAddress;
        protected TextBox Title;
        protected TextBox FirstName;
        protected TextBox LastName;
        protected TextBox FullName;
        protected TextBox CompanyName;
        protected TextBox Occupation;
        protected TextBox Address;
        protected TextBox Address2;
        protected TextBox City;
        protected DropDownList State;
        protected TextBox Zip;
        protected TextBox Country;
        protected TextBox Voice;
        protected TextBox Mobile;
        protected TextBox Fax;
        protected TextBox Income;
        protected DropDownList Gender;
        protected TextBox Age;
        protected TextBox Website;
        protected TextBox BirthDate;
        protected TextBox User1;
        protected TextBox User2;
        protected TextBox User3;
        protected TextBox User4;
        protected TextBox User5;
        protected TextBox User6;
        protected TextBox UserEvent1;
        protected TextBox UserEvent1Date;
        protected TextBox UserEvent2;
        protected TextBox UserEvent2Date;
        protected Button EditProfileButton;
    }
}
