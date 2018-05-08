using System.Collections.Generic;
using System.Data;
using NUnit.Framework;

namespace ecn.activityengines.Tests.Engines
{
    public partial class SO_multigroup_subscribeTest
    {
        private static readonly IEnumerable<TestCaseData> SqlCommandParametersTestData = new[]
        {
            new TestCaseData(EmailAddressParam, 250, SqlDbType.VarChar, EmailAddress),
            new TestCaseData(CustomerIDParam, 4, SqlDbType.Int, CustomerID.ToString()),
            new TestCaseData(TitleParam, 50, SqlDbType.VarChar, Title),
            new TestCaseData(FirstNameParam, 50, SqlDbType.VarChar, FirstName),
            new TestCaseData(LastNameParam, 50, SqlDbType.VarChar, LastName),
            new TestCaseData(FullNameParam, 50, SqlDbType.VarChar, FullName),
            new TestCaseData(CompanyParam, 50, SqlDbType.VarChar, CompanyName),
            new TestCaseData(OccupationParam, 50, SqlDbType.VarChar, Occupation),
            new TestCaseData(AddressParam, 255, SqlDbType.VarChar, StreetAddress),
            new TestCaseData(Address2Param, 255, SqlDbType.VarChar, StreetAddress2),
            new TestCaseData(CityParam, 50, SqlDbType.VarChar, City),
            new TestCaseData(StateParam, 50, SqlDbType.VarChar, State),
            new TestCaseData(ZipParam, 50, SqlDbType.VarChar, ZipCode),
            new TestCaseData(CountryParam, 50, SqlDbType.VarChar, Country),
            new TestCaseData(VoiceParam, 50, SqlDbType.VarChar, Phone),
            new TestCaseData(MobileParam, 50, SqlDbType.VarChar, MobilePhone),
            new TestCaseData(FaxParam, 50, SqlDbType.VarChar, Fax),
            new TestCaseData(WebsiteParam, 50, SqlDbType.VarChar, Website),
            new TestCaseData(AgeParam, 50, SqlDbType.VarChar, Age),
            new TestCaseData(IncomeParam, 50, SqlDbType.VarChar, Income),
            new TestCaseData(GenderParam, 50, SqlDbType.VarChar, Gender),
            new TestCaseData(User1Param, 255, SqlDbType.VarChar, User1),
            new TestCaseData(User2Param, 255, SqlDbType.VarChar, User2),
            new TestCaseData(User3Param, 255, SqlDbType.VarChar, User3),
            new TestCaseData(User4Param, 255, SqlDbType.VarChar, User4),
            new TestCaseData(User5Param, 255, SqlDbType.VarChar, User5),
            new TestCaseData(User6Param, 255, SqlDbType.VarChar, User6),
            new TestCaseData(BirthDateParam, 0, SqlDbType.DateTime, DateOfBirth),
            new TestCaseData(UserEvent1Param, 50, SqlDbType.VarChar, UserEvent1),
            new TestCaseData(UserEvent1DateParam, 0, SqlDbType.DateTime, UserEventDateTime),
            new TestCaseData(UserEvent2Param, 50, SqlDbType.VarChar, UserEvent2),
            new TestCaseData(UserEvent2DateParam, 0, SqlDbType.DateTime, UserEventDateTime)
        };

        private static readonly IEnumerable<TestCaseData> SqlCommandParametersWithInvalidQueryStringTestData = new[]
        {
            new TestCaseData(EmailAddressParam, 250, SqlDbType.VarChar, EmailAddress),
            new TestCaseData(CustomerIDParam, 4, SqlDbType.Int, CustomerID.ToString()),
            new TestCaseData(TitleParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(FirstNameParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(LastNameParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(FullNameParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(CompanyParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(OccupationParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(AddressParam, 255, SqlDbType.VarChar, string.Empty),
            new TestCaseData(Address2Param, 255, SqlDbType.VarChar, string.Empty),
            new TestCaseData(CityParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(StateParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(ZipParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(CountryParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(VoiceParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(MobileParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(FaxParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(WebsiteParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(AgeParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(IncomeParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(GenderParam, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(User1Param, 255, SqlDbType.VarChar, string.Empty),
            new TestCaseData(User2Param, 255, SqlDbType.VarChar, string.Empty),
            new TestCaseData(User3Param, 255, SqlDbType.VarChar, string.Empty),
            new TestCaseData(User4Param, 255, SqlDbType.VarChar, string.Empty),
            new TestCaseData(User5Param, 255, SqlDbType.VarChar, string.Empty),
            new TestCaseData(User6Param, 255, SqlDbType.VarChar, string.Empty),
            new TestCaseData(BirthDateParam, 0, SqlDbType.DateTime, Null),
            new TestCaseData(UserEvent1Param, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(UserEvent1DateParam, 0, SqlDbType.DateTime, Null),
            new TestCaseData(UserEvent2Param, 50, SqlDbType.VarChar, string.Empty),
            new TestCaseData(UserEvent2DateParam, 0, SqlDbType.DateTime, Null)
        };
    }
}