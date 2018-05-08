using System.Text;
using System.Web.UI.WebControls;

namespace ECN.Communicator.Tests.Includes
{
    public partial class EmailProfile_BaseTest
    {
        private const int EmailID = 125;
        private const string EmailAddress = "test@test.com";
        private const string Title = "Title";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string FullName = "FullName";
        private const string CompanyName = "CompanyName";
        private const string Occupation = "SE";
        private const string Address = "Address";
        private const string Address2 = "Address2";
        private const string City = "City";
        private const string StateItem = "--";
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
        private const string Notes = "Notes";
        private const string BirthDate = "3/24/2018 12:00:00 AM";
        private const string UserEvent1Date = "3/25/2018 12:00:00 AM";
        private const string UserEvent2Date = "3/26/2018 12:00:00 AM";
        private const string LoadUsStatesDr = "LoadUSStatesDR";
        private const string ConnString = "connString";
        private const string TestConnectionString = "test-connection-string";
        private const string MessageLabel = "MessageLabel";
        private const int GroupID = 34;

        private static readonly DropDownList FormatTypeCode = new DropDownList();
        private static readonly DropDownList State = new DropDownList();

        private static readonly StringBuilder UpdateCommandText = new StringBuilder()
                .Append("UPDATE Emails SET ")
                .AppendFormat("EmailAddress={0}, Title={1}, FirstName={2}, LastName={3}, ", EmailAddressParameter, TitleParameter, FirstNameParameter, LastNameParameter)
                .AppendFormat("FullName={0}, Company={1}, Occupation={2}, ", FullNameParameter, CompanyNameParameter, OccupationParameter)
                .AppendFormat("Address={0}, Address2={1}, ", AddressParameter, Address2Parameter)
                .AppendFormat("City={0}, State={1}, Zip={2}, Country={3}, ", CityParameter, StateItemParameter, ZipParameter, CountryParameter)
                .AppendFormat("Voice={0}, Mobile={1}, Fax={2}, Website={3}, ", VoiceParameter, MobileParameter, FaxParameter, WebsiteParameter)
                .AppendFormat("Birthdate={0}, Age={1}, Income={2}, Gender={3}, ", BirthDateParameter, AgeParameter, IncomeParameter, GenderParameter)
                .AppendFormat("User1={0}, User2={1}, User3={2}, User4={3}, User5={4}, User6={5}, ", User1Parameter, User2Parameter, User3Parameter, User4Parameter, User5Parameter, User6Parameter)
                .AppendFormat("UserEvent1={0}, UserEvent1Date={1}, ", UserEvent1Parameter, UserEvent1DateParameter)
                .AppendFormat("UserEvent2={0}, UserEvent2Date={1}, Notes={2} ", UserEvent2Parameter, UserEvent2DateParameter, NotesParameter)
                .AppendFormat("WHERE EmailID={0};", EmailIDParameter);

        private const string EmailIDParameter = "@email_id";
        private const string EmailAddressParameter = "@emailAddress";
        private const string TitleParameter = "@title";
        private const string FirstNameParameter = "@first_name";
        private const string LastNameParameter = "@last_name";
        private const string FullNameParameter = "@full_name";
        private const string CompanyNameParameter = "@company";
        private const string OccupationParameter = "@occupation";
        private const string AddressParameter = "@address";
        private const string Address2Parameter = "@address2";
        private const string CityParameter = "@city";
        private const string ZipParameter = "@zip";
        private const string CountryParameter = "@country";
        private const string VoiceParameter = "@voice";
        private const string MobileParameter = "@mobile";
        private const string FaxParameter = "@fax";
        private const string WebsiteParameter = "@website";
        private const string AgeParameter = "@age";
        private const string IncomeParameter = "@income";
        private const string GenderParameter = "@gender";
        private const string User1Parameter = "@user1";
        private const string User2Parameter = "@user2";
        private const string User3Parameter = "@user3";
        private const string User4Parameter = "@user4";
        private const string User5Parameter = "@user5";
        private const string User6Parameter = "@user6";
        private const string UserEvent1Parameter = "@user_event1";
        private const string UserEvent2Parameter = "@user_event2";
        private const string BirthDateParameter = "@birthdate";
        private const string UserEvent1DateParameter = "@user_event1_date";
        private const string UserEvent2DateParameter = "@user_event2_date";
        private const string StateItemParameter = "@state";
        private const string FormatTypeCodeParameter = "@fmtTypeCde";
        private const string SubTypeCodeParameter = "@subTypeCde";
        private const string GroupIdParameter = "@groupID";
        private const string NotesParameter = "@notes";
    }
}
