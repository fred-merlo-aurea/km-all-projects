using System;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    public partial class emaileditorTest
    {
        private string _emailAddress;
        private bool _isEmailExist;
        private Email _emailObject;

        [Test]
        public void UpdateEmail_IfEmptyEmailAddress_RegisterStartupScript()
        {
            // Arrange
            _emailAddress = string.Empty;
            var expectedScript = "alert('You cannot create or update an email profile with an empty email address')";
            SetupControls();

            Type pageType = null;
            var pageKey = string.Empty;
            var pageScript = string.Empty;
            var pageAddScriptsTag = false;

            ShimClientScriptManager.AllInstances.RegisterStartupScriptTypeStringStringBoolean = (
                _,
                type,
                key,
                script,
                addScriptTags) =>
            {
                pageType = type;
                pageKey = key;
                pageScript = script;
                pageAddScriptsTag = addScriptTags;
            };

            // Act
            _page.UpdateEmail(null, new EventArgs());

            // Assert
            _page.ShouldSatisfyAllConditions(
                () => pageType.ShouldBe(_page.GetType()),
                () => pageKey.ShouldBeEmpty(),
                () => pageAddScriptsTag.ShouldBeTrue(),
                () => pageScript.ShouldBe(expectedScript));
        }

        [Test]
        public void UpdateEmail_IfEmailExists_RegisterStartupScript()
        {
            // Arrange
            _emailAddress = EmailAddress;
            _isEmailExist = true;
            var expectedScript =
                $"if(confirm({ErrorMessage})){{ window.location=\'mergeProfiles.aspx?oldemailid={EmailID}&newemailid={_email.EmailID}\';}}";
            SetupControls();

            Type pageType = null;
            var pageKey = string.Empty;
            var pageScript = string.Empty;
            var pageAddScriptsTag = false;

            ShimClientScriptManager.AllInstances.RegisterStartupScriptTypeStringStringBoolean = (
                _,
                type,
                key,
                script,
                addScriptTags) =>
            {
                pageType = type;
                pageKey = key;
                pageScript = script;
                pageAddScriptsTag = addScriptTags;
            };

            // Act
            _page.UpdateEmail(null, new EventArgs());

            // Assert
            _page.ShouldSatisfyAllConditions(
                () => pageType.ShouldBe(_page.GetType()),
                () => pageKey.ShouldBe(MergeProfile),
                () => pageAddScriptsTag.ShouldBeTrue(),
                () => pageScript.ShouldBe(expectedScript));
        }

        [Test]
        public void UpdateEmail_IfEmailNotExists_UpdateEmail()
        {
            // Arrange
            _emailAddress = EmailAddress;
            _isEmailExist = false;
            SetupControls();

            Email actualEmail = null;
            var actualSource = string.Empty;
            string actualLocation = string.Empty;
            ShimEmail.SaveUserEmailInt32String = (_, email, groupId, source) =>
            {
                actualEmail = email;
                actualSource = source;
            };

            ShimHttpResponse.AllInstances.RedirectString = (_, location) => { actualLocation = location; };

            // Act
            _page.UpdateEmail(null, new EventArgs());

            // Assert
            VerifyEmail(actualEmail);
            _page.ShouldSatisfyAllConditions(
                () => actualLocation.ShouldBe(Location),
                () => actualSource.ShouldBe(Source)
            );
        }

        private static void VerifyEmail(Email actualEmail)
        {
            actualEmail.ShouldSatisfyAllConditions(
                () => actualEmail.EmailAddress.ShouldBe(EmailAddress),
                () => actualEmail.Address.ShouldBe(Address),
                () => actualEmail.Title.ShouldBe(Title),
                () => actualEmail.FirstName.ShouldBe(FirstName),
                () => actualEmail.LastName.ShouldBe(LastName),
                () => actualEmail.FullName.ShouldBe(FullName),
                () => actualEmail.Company.ShouldBe(CompanyName),
                () => actualEmail.Occupation.ShouldBe(Occupation),
                () => actualEmail.Address.ShouldBe(Address),
                () => actualEmail.Address2.ShouldBe(Address2),
                () => actualEmail.City.ShouldBe(City),
                () => actualEmail.State.ShouldBe(State),
                () => actualEmail.Zip.ShouldBe(Zip),
                () => actualEmail.Country.ShouldBe(Country),
                () => actualEmail.Voice.ShouldBe(Voice),
                () => actualEmail.Mobile.ShouldBe(Mobile),
                () => actualEmail.Fax.ShouldBe(Fax),
                () => actualEmail.Website.ShouldBe(Website),
                () => actualEmail.Age.ShouldBe(Age),
                () => actualEmail.Income.ShouldBe(Income),
                () => actualEmail.Gender.ShouldBe(Gender),
                () => actualEmail.User1.ShouldBe(User1),
                () => actualEmail.User2.ShouldBe(User2),
                () => actualEmail.User3.ShouldBe(User3),
                () => actualEmail.User4.ShouldBe(User4),
                () => actualEmail.User5.ShouldBe(User5),
                () => actualEmail.User6.ShouldBe(User6),
                () => actualEmail.UserEvent1.ShouldBe(UserEvent1),
                () => actualEmail.UserEvent2.ShouldBe(UserEvent2),
                () => actualEmail.Password.ShouldBe(Password),
                () => actualEmail.Birthdate.ShouldBe(DateTime.Parse(BirthDate)),
                () => actualEmail.UserEvent1Date.ShouldBe(DateTime.Parse(UserEvent1Date)),
                () => actualEmail.UserEvent2Date.ShouldBe(DateTime.Parse(UserEvent2Date)));
        }

        private void SetupControls()
        {
            FormatTypeCode.Items.Add(new ListItem(nameof(FormatTypeCode), nameof(FormatTypeCode)));
            SubscribeTypeCode.Items.Add(new ListItem(nameof(SubscribeTypeCode), nameof(SubscribeTypeCode)));

            _privateObject.SetField(nameof(EmailID), new TextBox { Text = EmailID.ToString() });
            _privateObject.SetField(nameof(EmailAddress), new TextBox { Text = _emailAddress });
            _privateObject.SetField(Title, new TextBox { Text = Title });
            _privateObject.SetField(nameof(FirstName), new TextBox { Text = FirstName });
            _privateObject.SetField(nameof(LastName), new TextBox { Text = LastName });
            _privateObject.SetField(nameof(FullName), new TextBox { Text = FullName });
            _privateObject.SetField(nameof(CompanyName), new TextBox { Text = CompanyName });
            _privateObject.SetField(nameof(Occupation), new TextBox { Text = Occupation });
            _privateObject.SetField(nameof(Address), new TextBox { Text = Address });
            _privateObject.SetField(nameof(Address2), new TextBox { Text = Address2 });
            _privateObject.SetField(nameof(City), new TextBox { Text = City });
            _privateObject.SetField(nameof(State), new TextBox { Text = State });
            _privateObject.SetField(nameof(Zip), new TextBox { Text = Zip });
            _privateObject.SetField(nameof(Country), new TextBox { Text = Country });
            _privateObject.SetField(nameof(Voice), new TextBox { Text = Voice });
            _privateObject.SetField(nameof(Mobile), new TextBox { Text = Mobile });
            _privateObject.SetField(nameof(Fax), new TextBox { Text = Fax });
            _privateObject.SetField(nameof(Website), new TextBox { Text = Website });
            _privateObject.SetField(nameof(Age), new TextBox { Text = Age });
            _privateObject.SetField(nameof(Income), new TextBox { Text = Income });
            _privateObject.SetField(nameof(Gender), new TextBox { Text = Gender });
            _privateObject.SetField(nameof(User1), new TextBox { Text = User1 });
            _privateObject.SetField(nameof(User2), new TextBox { Text = User2 });
            _privateObject.SetField(nameof(User3), new TextBox { Text = User3 });
            _privateObject.SetField(nameof(User4), new TextBox { Text = User4 });
            _privateObject.SetField(nameof(User5), new TextBox { Text = User5 });
            _privateObject.SetField(nameof(User6), new TextBox { Text = User6 });
            _privateObject.SetField(nameof(UserEvent1), new TextBox { Text = UserEvent1 });
            _privateObject.SetField(nameof(UserEvent2), new TextBox { Text = UserEvent2 });
            _privateObject.SetField(nameof(Password), new TextBox { Text = Password });
            _privateObject.SetField(nameof(BirthDate), new TextBox { Text = BirthDate });
            _privateObject.SetField(nameof(UserEvent1Date), new TextBox { Text = UserEvent1Date });
            _privateObject.SetField(nameof(UserEvent2Date), new TextBox { Text = UserEvent2Date });
            _privateObject.SetField(nameof(FormatTypeCode), FormatTypeCode);
            _privateObject.SetField(nameof(FormatTypeCode), FormatTypeCode);
            _privateObject.SetField(nameof(SubscribeTypeCode), SubscribeTypeCode);
        }

        private void SetupEmailFakes()
        {
            ShimEmail.ExistsStringInt32Int32 = (email, customerId, emailId) =>
            {
                email.ShouldSatisfyAllConditions(
                    () => email.ShouldBe(EmailAddress),
                    () => customerId.ShouldBe(CustomerId),
                    () => emailId.ShouldBe(EmailID));

                return _isEmailExist;
            };

            _email = new Email();
            ShimEmail.GetByEmailAddressStringInt32Int32User = (email, customerId, emailId, user) =>
            {
                email.ShouldSatisfyAllConditions(
                    () => email.ShouldBe(EmailAddress),
                    () => customerId.ShouldBe(CustomerId),
                    () => emailId.ShouldBe(EmailID));

                return _email;
            };

            ShimEmail.GetByEmailIDInt32User = (emailId, _) =>
            {
                emailId.ShouldBe(EmailID);
                return new Email();
            };
        }
    }
}