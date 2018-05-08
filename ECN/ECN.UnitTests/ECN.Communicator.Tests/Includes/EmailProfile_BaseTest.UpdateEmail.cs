using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Includes
{
    public partial class EmailProfile_BaseTest
    {
        private const string SubType = "S";

        [Test, TestCaseSource(nameof(UpdateCommandParameters))]
        public void UpdateEmail_WhenCalled_VerifyUpdateCommandParameters(
            string parameterName,
            SqlDbType dbType,
            int size,
            string parameterValue)
        {
            // Arrange
            SetupControls();

            // Act
            _page.UpdateEmail(null, new EventArgs());

            // Assert
            var parameter = _updateCommand.Parameters[parameterName];
            parameter.ShouldSatisfyAllConditions(
                () => parameter.ShouldNotBeNull(),
                () => parameter.Size.ShouldBe(size),
                () => parameter.SqlDbType.ShouldBe(dbType),
                () => parameter.Value.ToString().ShouldBe(parameterValue));
        }

        [Test, TestCaseSource(nameof(EmailGrpUpdateCommandParameters))]
        public void UpdateEmail_WhenCalled_VerifyEmailGroupUpdateCommandParameters(
            string parameterName,
            SqlDbType dbType,
            int size,
            string parameterValue)
        {
            // Arrange
            SetupControls();

            // Act
            _page.UpdateEmail(null, new EventArgs());

            // Assert
            var parameter = _emailGrpUpdateCommand.Parameters[parameterName];
            parameter.ShouldSatisfyAllConditions(
                () => parameter.ShouldNotBeNull(),
                () => parameter.Size.ShouldBe(size),
                () => parameter.SqlDbType.ShouldBe(dbType),
                () => parameter.Value.ToString().ShouldBe(parameterValue));
        }

        private void SetupControls()
        {
            FormatTypeCode.Items.Add(new ListItem(nameof(FormatTypeCode), nameof(FormatTypeCode)));
            var gender = new DropDownList();
            gender.Items.Add(new ListItem(nameof(Gender), Gender));

            _privateObject.SetField("_emailId", EmailID.ToString());
            _privateObject.SetField("_groupId", GroupID.ToString());
            _privateObject.SetField(nameof(EmailAddress), new TextBox { Text = EmailAddress });
            _privateObject.SetField(nameof(Title), new TextBox { Text = Title });
            _privateObject.SetField(nameof(FirstName), new TextBox { Text = FirstName });
            _privateObject.SetField(nameof(LastName), new TextBox { Text = LastName });
            _privateObject.SetField(nameof(FullName), new TextBox { Text = FullName });
            _privateObject.SetField(nameof(CompanyName), new TextBox { Text = CompanyName });
            _privateObject.SetField(nameof(Occupation), new TextBox { Text = Occupation });
            _privateObject.SetField(nameof(Address), new TextBox { Text = Address });
            _privateObject.SetField(nameof(Address2), new TextBox { Text = Address2 });
            _privateObject.SetField(nameof(City), new TextBox { Text = City });
            _privateObject.SetField(nameof(Zip), new TextBox { Text = Zip });
            _privateObject.SetField(nameof(Country), new TextBox { Text = Country });
            _privateObject.SetField(nameof(Voice), new TextBox { Text = Voice });
            _privateObject.SetField(nameof(Mobile), new TextBox { Text = Mobile });
            _privateObject.SetField(nameof(Fax), new TextBox { Text = Fax });
            _privateObject.SetField(nameof(Website), new TextBox { Text = Website });
            _privateObject.SetField(nameof(Age), new TextBox { Text = Age });
            _privateObject.SetField(nameof(Income), new TextBox { Text = Income });
            _privateObject.SetField(nameof(Gender), gender);
            _privateObject.SetField(nameof(User1), new TextBox { Text = User1 });
            _privateObject.SetField(nameof(User2), new TextBox { Text = User2 });
            _privateObject.SetField(nameof(User3), new TextBox { Text = User3 });
            _privateObject.SetField(nameof(User4), new TextBox { Text = User4 });
            _privateObject.SetField(nameof(User5), new TextBox { Text = User5 });
            _privateObject.SetField(nameof(User6), new TextBox { Text = User6 });
            _privateObject.SetField(nameof(UserEvent1), new TextBox { Text = UserEvent1 });
            _privateObject.SetField(nameof(UserEvent2), new TextBox { Text = UserEvent2 });
            _privateObject.SetField(nameof(BirthDate), new TextBox { Text = BirthDate });
            _privateObject.SetField(nameof(UserEvent1Date), new TextBox { Text = UserEvent1Date });
            _privateObject.SetField(nameof(UserEvent2Date), new TextBox { Text = UserEvent2Date });
            _privateObject.SetField(nameof(Notes), new TextBox { Text = Notes });
            _privateObject.SetField(nameof(FormatTypeCode), FormatTypeCode);
            _privateObject.SetField(nameof(State), State);
            _privateObject.SetField(nameof(MessageLabel), new Label());

            _privateObject.Invoke(LoadUsStatesDr);
        }

        private static readonly IEnumerable<TestCaseData> UpdateCommandParameters = new[]
        {
            new TestCaseData(EmailIDParameter, SqlDbType.Int, 4, EmailID.ToString()),
            new TestCaseData(EmailAddressParameter, SqlDbType.VarChar, 250, EmailAddress),
            new TestCaseData(TitleParameter, SqlDbType.VarChar, 50, Title),
            new TestCaseData(FirstNameParameter, SqlDbType.VarChar, 50, FirstName),
            new TestCaseData(LastNameParameter, SqlDbType.VarChar, 50, LastName),
            new TestCaseData(FullNameParameter, SqlDbType.VarChar, 50, FullName),
            new TestCaseData(CompanyNameParameter, SqlDbType.VarChar, 50, CompanyName),
            new TestCaseData(OccupationParameter, SqlDbType.VarChar, 50, Occupation),
            new TestCaseData(AddressParameter, SqlDbType.VarChar, 255, Address),
            new TestCaseData(Address2Parameter, SqlDbType.VarChar, 255, Address2),
            new TestCaseData(CityParameter, SqlDbType.VarChar, 50, City),
            new TestCaseData(ZipParameter, SqlDbType.VarChar, 50, Zip),
            new TestCaseData(CountryParameter, SqlDbType.VarChar, 50, Country),
            new TestCaseData(VoiceParameter, SqlDbType.VarChar, 50, Voice),
            new TestCaseData(MobileParameter, SqlDbType.VarChar, 50, Mobile),
            new TestCaseData(FaxParameter, SqlDbType.VarChar, 50, Fax),
            new TestCaseData(WebsiteParameter, SqlDbType.VarChar, 50, Website),
            new TestCaseData(AgeParameter, SqlDbType.VarChar, 50, Age),
            new TestCaseData(IncomeParameter, SqlDbType.VarChar, 50, Income),
            new TestCaseData(GenderParameter, SqlDbType.VarChar, 50, Gender),
            new TestCaseData(User1Parameter, SqlDbType.VarChar, 255, User1),
            new TestCaseData(User2Parameter, SqlDbType.VarChar, 255, User2),
            new TestCaseData(User3Parameter, SqlDbType.VarChar, 255, User3),
            new TestCaseData(User4Parameter, SqlDbType.VarChar, 255, User4),
            new TestCaseData(User5Parameter, SqlDbType.VarChar, 255, User5),
            new TestCaseData(User6Parameter, SqlDbType.VarChar, 255, User6),
            new TestCaseData(UserEvent1Parameter, SqlDbType.VarChar, 50, UserEvent1),
            new TestCaseData(UserEvent2Parameter, SqlDbType.VarChar, 50, UserEvent2),
            new TestCaseData(BirthDateParameter, SqlDbType.DateTime, 0, BirthDate),
            new TestCaseData(UserEvent1DateParameter, SqlDbType.DateTime, 0, UserEvent1Date),
            new TestCaseData(UserEvent2DateParameter, SqlDbType.DateTime, 0, UserEvent2Date),
            new TestCaseData(StateItemParameter, SqlDbType.VarChar, 50, string.Empty)
        };

        private static readonly IEnumerable<TestCaseData> EmailGrpUpdateCommandParameters = new[]
        {
            new TestCaseData(EmailIDParameter, SqlDbType.Int, 4, EmailID.ToString()),
            new TestCaseData(FormatTypeCodeParameter, SqlDbType.VarChar, 50, nameof(FormatTypeCode)),
            new TestCaseData(SubTypeCodeParameter, SqlDbType.VarChar, 50, SubType),
            new TestCaseData(GroupIdParameter, SqlDbType.Int, 4, GroupID.ToString())
        };
    }
}