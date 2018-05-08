using System.Web.UI.WebControls;

namespace ECN.Communicator.Tests.Main.Lists
{
    public partial class emaileditorTest
    {
        private const string EmailAddress = "test@test.com";
        private const int EmailID = 125;
        private const string Title = "txtTitle";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string FullName = "FullName";
        private const string CompanyName = "CompanyName";
        private const string Occupation = "SE";
        private const string Address = "Address";
        private const string Address2 = "Address2";
        private const string City = "City";
        private const string State = "State";
        private const string Zip = "Zip";
        private const string Country = "Country";
        private const string Voice = "Voice";
        private const string Mobile = "Mobile";
        private const string Fax = "Fax";
        private const string Website = "test.com";
        private const string Age = "20";
        private const string Income = "2000";
        private const string Gender = "Male";
        private const string User1 = "User1";
        private const string User2 = "User2";
        private const string User3 = "User3";
        private const string User4 = "User4";
        private const string User5 = "User5";
        private const string User6 = "User6";
        private const string UserEvent1 = "UserEvent1";
        private const string UserEvent2 = "UserEvent2";
        private const string Password = "test-password";
        private const string BirthDate = "3/24/2018";
        private const string UserEvent1Date = "3/25/2018";
        private const string UserEvent2Date = "3/25/2018";
        private const string MergeProfile = "MergeProfile";
        private const string ErrorMessage ="\'This email address already exists for this Customer Account. Would you like to merge these two profiles?\'";
        private const string Location = "groupeditor.aspx?groupID=0";
        private const string Source = "Ecn.communicator.main.lists.emaileditor.updateEmail";

        private static readonly DropDownList FormatTypeCode = new DropDownList();
        private static readonly DropDownList SubscribeTypeCode = new DropDownList();
    }
}