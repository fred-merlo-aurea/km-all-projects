using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.Object.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Objects.Tests
{
    /// <summary>
    /// Unit test for <see cref="Filter"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterTests
    {
        private const string column = "case when IsDate(E.column) = 1 then CAST(E.column AS DATETIME) else null end";
        private const string OpenActivityInsertStatement = "Insert into #tempSOA  select so.SubscriptionID from PubSubscriptions pso";
        private const string ClickActivityInsertStatement = "Insert into #tempSCA  select sc.SubscriptionID from PubSubscriptions psc";
        private const string VisitActivityInsertStatement = "Insert into #tempSVA  select sv.SubscriptionID from  SubscriberVisitActivity";
        private const string ClickBlastInsertStatement = "Insert into #tempCblast SELECT distinct blastid FROM blast bla WITH (nolock)";
        private const string OpenBlastInsertStatement = "insert into #tempOblast SELECT distinct blastid FROM blast bla WITH(nolock)";
        private const string SubscriptionExtensionSelectStatement = "(select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)";
        private const string ExpectedPermissionsQuery = "select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ({0} = 0 OR {0} = 1 OR {0} is null);";
        private const string CountrySelectStatement = "select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join UAD_Lookup..Country c on c.CountryID = s.countryID";
        private const string CountryLeftJoinStatement = "select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  left outer join UAD_Lookup..Country c on c.CountryID = s.countryID";
        private const string ExpectedPubSubscriptionsQueryWithJoin = "select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ";

        private const string ExectedZipCodeRadiusMinus2 = "(s.Latitude >= 0.971014492753623 and s.Latitude <= 1.02898550724638 and s.Longitude >= 1.97100981774379 and s.Longitude <= 2.02898967027162 and (s.Latitude<=0.971014492753623 OR 1.02898550724638<=s.Latitude OR s.Longitude<=1.97100981774379 OR 2.02898967027162<=s.Longitude) and isnull(s.IsLatLonValid,0) = 1 );";
        private const string ExectedZipCodeRadius2 = "(s.Latitude >= 0.971014492753623 and s.Latitude <= 1.02898550724638 and s.Longitude >= 1.97100981774379 and s.Longitude <= 2.02898967027162 and (s.Latitude<=0.971014492753623 OR 1.02898550724638<=s.Latitude OR s.Longitude<=1.97100981774379 OR 2.02898967027162<=s.Longitude) and isnull(s.IsLatLonValid,0) = 1  and ( master.dbo.fn_CalcDistanceBetweenLocations(1, 2, s.Latitude, s.Longitude, 0) between  2 and 2));";

        private const string ProfilePermissionFax = "FAXPERMISSION";
        private const string ProfilePermissionPhone = "PHONEPERMISSION";
        private const string ProfilePermissionOtherProducts = "OTHERPRODUCTSPERMISSION";
        private const string ProfilePermissionThirdParty = "THIRDPARTYPERMISSION";
        private const string ProfilePermissionText = "TEXTPERMISSION";
        private const string Values = "EQUAL,CONTAINS,END WITHDOES NOT CONTAIN,START WITH,END WITH,IS EMPTY,IS NOT EMPTY,DOES NOT CONTAIN";
        private const string FieldGroupStartWithm = "m|d";
        private const string FieldGroupStartWithd = "d|qdate";
        private const string Equal = "EQUAL";
        private const string Contains = "CONTAINS";
        private const string DoseNotContain = "DOES NOT CONTAIN";
        private const string StartWith = "START WITH";
        private const string EndWith = "END WITH";
        private const string IsEmpty = "IS EMPTY";
        private const string IsNotEmpty = "IS NOT EMPTY";
        private const string SearchAll = "Search All";
        private const string Range = "RANGE";
        private const string Greater = "GREATER";
        private const string Lesser = "LESSER";
        private const string DefaultValues = "a|b|c";
        private const string FiledGroupStartWithe = "e|e|i";
        private const string FieldGroupStartWitheEndsWithf = "e|e|f";
        private const string FieldGroupStartWitheEndsWithx = "e|e|x";
        private const string FieldGroupStartWitheEndsWithd = "e|e|d";
        private const string FieldGroupStartWitheEndsWithb = "e|e|b";
        private const string FiledGroupStartWithiEndsWithScore = "i|[SCORE]";
        private const string FiledGroupStartWithiEndsWithProductCount = "i|[PRODUCT COUNT]";
        private const string FiledGroupStartWithdEndsWithqdate = "d|qdate";
        private const string FiledGroupStartWithdEndsWithTransactiondate = "d|[transactiondate]";
        private const string FiledGroupStartWithdEndsWithStatusUpdatedDate = "d|[statusupdateddate]";
        private const string FiledGroupStartWithdEndsWithDateCreated = "d|datecreated";
        private const string FiledGroupStartWithdEndsWithDefaultCase = "d|defaultCase";
        private const string FiledGroupStartWithdEndsWithDefault = "d|zzz";
        private const int BrandIdDefault = 1;
        private const int BrandIdIsZero = 0;

        private int today;

        [SetUp]
        public void Setup()
        {
            today = (int)(DateTime.Today - new DateTime(1900, 1, 1)).TotalDays;
        }

        [Test]
        public void getFilterQuery_VisitActivityDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Visit Activity", "VISIT CRITERIA", "DateRange", "EXP:Today|EXP:Today");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today} and  DateNumber <= {today}");
        }

        [Test]
        public void getFilterQuery_VisitActivityDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Visit Activity", "VISIT CRITERIA", "DateRange", "EXP:Today[-7]|EXP:Today[-1]");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today - 7} and  DateNumber <= {today - 1}");
        }

        [Test]
        public void getFilterQuery_VisitActivityDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Visit Activity", "VISIT CRITERIA", "DateRange", "1/1/1900|2/2/1902");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= 0 and  DateNumber <= {(new DateTime(1902, 2, 2) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Visit Activity", "VISIT CRITERIA", "XDays", "1YR");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddYears(-1) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Visit Activity", "VISIT CRITERIA", "XDays", "6mon");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddMonths(-6) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Visit Activity", "VISIT CRITERIA", "XDays", "7");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddDays(-7) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Visit Activity", "VISIT CRITERIA", "Year", "2016|2017");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(2016, 1, 1) - new DateTime(1900, 1, 1)).TotalDays} and  DateNumber <= {(new DateTime(2017, 12, 31) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Visit Activity", "VISIT CRITERIA", "Month", "1|6");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year, 1, 1) - new DateTime(1900, 1, 1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year, 6, 30) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Open Activity", "Open CRITERIA", "DateRange", "EXP:Today|EXP:Today");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today} and  DateNumber <= {today}");
        }

        [Test]
        public void getFilterQuery_OpenActivityDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Open Activity", "Open CRITERIA", "DateRange", "EXP:Today[-7]|EXP:Today[-1]");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today - 7} and  DateNumber <= {today - 1}");
        }

        [Test]
        public void getFilterQuery_OpenActivityDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Open Activity", "Open CRITERIA", "DateRange", "1/1/1900|2/2/1902");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= 0 and  DateNumber <= {(new DateTime(1902, 2, 2) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Open Activity", "Open CRITERIA", "XDays", "1YR");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddYears(-1) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Open Activity", "Open CRITERIA", "XDays", "6mon");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddMonths(-6) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Open Activity", "Open CRITERIA", "XDays", "7");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddDays(-7) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Open Activity", "Open CRITERIA", "Year", "2016|2017");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(2016, 1, 1) - new DateTime(1900, 1, 1)).TotalDays} and  DateNumber <= {(new DateTime(2017, 12, 31) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Open Activity", "Open CRITERIA", "Month", "1|6");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year, 1, 1) - new DateTime(1900, 1, 1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year, 6, 30) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_QualificationDateDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter("DateRange", "EXP:Today|EXP:Today");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_QualificationDateDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter("DateRange", "EXP:Today[-7]|EXP:Today[-1]");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_QualificationDateDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter("DateRange", "1/1/1900|2/2/1902");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain("(ps.QualificationDate >= '1/1/1900'  and ps.QualificationDate <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getFilterQuery_QualificationDateXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter("XDays", "1YR");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_QualificationDateXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter("XDays", "6mon");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_QualificationDateXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter("XDays", "7");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_QualificationDateYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter("Year", "2016|2017");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(year(ps.QualificationDate) >= '2016'  and year(ps.QualificationDate) <= '2017' )");
        }

        [Test]
        public void getFilterQuery_QualificationDateMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter("Month", "1|6");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(month(ps.QualificationDate) >= '1'  and month(ps.QualificationDate) <= '6' )");
        }

        [Test]
        public void getFilterQuery_EColumnDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetEColumnFilter("DateRange", "EXP:Today|EXP:Today");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({column} >='{DateTime.Now.ToShortDateString()}' and {column} <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_EColumnDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetEColumnFilter("DateRange", "EXP:Today[-7]|EXP:Today[-1]");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({column} >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and {column} <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_EColumnDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetEColumnFilter("DateRange", "1/1/1900|2/2/1902");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({column} >= '1/1/1900'  and {column} <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getFilterQuery_EColumnXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetEColumnFilter("XDays", "1YR");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({column}>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_EColumnXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetEColumnFilter("XDays", "6mon");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({column}>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_EColumnXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetEColumnFilter("XDays", "7");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({column}>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_EColumnYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetEColumnFilter("Year", "2016|2017");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"(year({column}) >= '2016'  and year({column}) <= '2017' )");
        }

        [Test]
        public void getFilterQuery_EColumnMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetEColumnFilter("Month", "1|6");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"(month({column}) >= '1'  and month({column}) <= '6' )");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("CLICK EMAIL SENT DATE", "CLICK CRITERIA", "DateRange", "EXP:Today|EXP:Today");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime >='{DateTime.Now.ToShortDateString()}' and bla.SendTime <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("CLICK EMAIL SENT DATE", "CLICK CRITERIA", "DateRange", "EXP:Today[-7]|EXP:Today[-1]");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and bla.SendTime <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("CLICK EMAIL SENT DATE", "CLICK CRITERIA", "DateRange", "1/1/1900|2/2/1902");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain("(bla.SendTime >= '1/1/1900'  and bla.SendTime <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("CLICK EMAIL SENT DATE", "CLICK CRITERIA", "XDays", "1YR");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("CLICK EMAIL SENT DATE", "CLICK CRITERIA", "XDays", "6mon");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("CLICK EMAIL SENT DATE", "CLICK CRITERIA", "XDays", "7");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("CLICK EMAIL SENT DATE", "CLICK CRITERIA", "Year", "2016|2017");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain("(year(bla.SendTime) >= '2016'  and year(bla.SendTime) <= '2017' )");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("CLICK EMAIL SENT DATE", "CLICK CRITERIA", "Month", "1|6");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain("(month(bla.SendTime) >= '1'  and month(bla.SendTime) <= '6' )");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("OPEN EMAIL SENT DATE", "OPEN CRITERIA", "DateRange", "EXP:Today|EXP:Today");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime >='{DateTime.Now.ToShortDateString()}' and bla.SendTime <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("OPEN EMAIL SENT DATE", "OPEN CRITERIA", "DateRange", "EXP:Today[-7]|EXP:Today[-1]");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and bla.SendTime <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("OPEN EMAIL SENT DATE", "OPEN CRITERIA", "DateRange", "1/1/1900|2/2/1902");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain("(bla.SendTime >= '1/1/1900'  and bla.SendTime <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("OPEN EMAIL SENT DATE", "OPEN CRITERIA", "XDays", "1YR");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("OPEN EMAIL SENT DATE", "OPEN CRITERIA", "XDays", "6mon");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("OPEN EMAIL SENT DATE", "OPEN CRITERIA", "XDays", "7");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(bla.SendTime>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("OPEN EMAIL SENT DATE", "OPEN CRITERIA", "Year", "2016|2017");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain("(year(bla.SendTime) >= '2016'  and year(bla.SendTime) <= '2017' )");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("OPEN EMAIL SENT DATE", "OPEN CRITERIA", "Month", "1|6");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain("(month(bla.SendTime) >= '1'  and month(bla.SendTime) <= '6' )");
        }

        [Test]
        public void getFilterQuery_PhonePermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionPhone, Enums.ViewType.ProductView, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "ps.PhonePermission"));
        }

        [Test]
        public void getFilterQuery_PhonePermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionPhone, Enums.ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "s.PhonePermission"));
        }

        [Test]
        public void getFilterQuery_TextPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionText, Enums.ViewType.ProductView, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "ps.TextPermission"));
        }

        [Test]
        public void getFilterQuery_TextPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionText, Enums.ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "s.TextPermission"));
        }

        [Test]
        public void getFilterQuery_ThirdPartyPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionThirdParty, Enums.ViewType.ProductView, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "ps.ThirdPartyPermission"));
        }

        [Test]
        public void getFilterQuery_ThirdPartyPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionThirdParty, Enums.ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "s.ThirdPartyPermission"));
        }

        [Test]
        public void getFilterQuery_FaxPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionFax, Enums.ViewType.ProductView, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "ps.FaxPermission"));
        }

        [Test]
        public void getFilterQuery_FaxPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionFax, Enums.ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "s.FaxPermission"));
        }

        [Test]
        public void getFilterQuery_OtherProductsPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionOtherProducts, Enums.ViewType.ProductView, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "ps.OtherProductsPermission"));
        }

        [Test]
        public void getFilterQuery_OtherProductsPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetPermissionFilter(ProfilePermissionOtherProducts, Enums.ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionsQuery, "s.OtherProductsPermission"));
        }

        [Test]
        public void getFilterQuery_ClickActivityDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Click Activity", "CLICK CRITERIA", "DateRange", "EXP:Today|EXP:Today");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today} and  DateNumber <= {today}");
        }

        [Test]
        public void getFilterQuery_ClickActivityDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Click Activity", "CLICK CRITERIA", "DateRange", "EXP:Today[-7]|EXP:Today[-1]");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today - 7} and  DateNumber <= {today - 1}");
        }

        [Test]
        public void getFilterQuery_ClickActivityDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Click Activity", "CLICK CRITERIA", "DateRange", "1/1/1900|2/2/1902");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= 0 and  DateNumber <= {(new DateTime(1902, 2, 2) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Click Activity", "CLICK CRITERIA", "XDays", "1YR");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddYears(-1) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Click Activity", "CLICK CRITERIA", "XDays", "6mon");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddMonths(-6) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Click Activity", "CLICK CRITERIA", "XDays", "7");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddDays(-7) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Click Activity", "CLICK CRITERIA", "Year", "2016|2017");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(2016, 1, 1) - new DateTime(1900, 1, 1)).TotalDays} and  DateNumber <= {(new DateTime(2017, 12, 31) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Click Activity", "CLICK CRITERIA", "Month", "1|6");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year, 1, 1) - new DateTime(1900, 1, 1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year, 6, 30) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryEqual_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "EQUAL", "Target1,Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( c.ShortName = 'Target1') OR ( c.ShortName = 'Target2'))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryContains_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "CONTAINS", "Target1,Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( PATINDEX('%Target1%', c.ShortName) > 0 ) OR ( PATINDEX('%Target2%', c.ShortName) > 0 ))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryStartWith_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "START WITH", "Target1,Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( PATINDEX('Target1%', c.ShortName) > 0 ) OR ( PATINDEX('Target2%', c.ShortName) > 0 ))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryEndWith_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "END WITH", "Target1,Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( PATINDEX('%Target1', c.ShortName) > 0 ) OR ( PATINDEX('%Target2', c.ShortName) > 0 ))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryDoesNotContain_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "DOES NOT CONTAIN", "Target1,Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( isnull(c.ShortName,'') not like '%Target1%'  ) AND ( isnull(c.ShortName,'') not like '%Target2%'  ))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryRange_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "RANGE", "Target1|Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("( (substring(c.ShortName, 1, len('Target1')) between 'Target1' and 'Target2'))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryIsEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "IS EMPTY", "Target1|Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(CountryLeftJoinStatement);
            result.ShouldContain("( (c.CountryID is NULL ) )");
        }

        [Test]
        public void getFilterQuery_AdhocStringIGRP_NOIsEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[IGRP_NO]", "IS EMPTY", "Target1|Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain("( ( cast(s.[IGRP_NO] as varchar(100)) is NULL) )");
        }

        [Test]
        public void getFilterQuery_AdhocStringOtherIsEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[OTHER]", "IS EMPTY", "Target1|Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain("(( s.[OTHER] is NULL or s.[OTHER] = ''))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryIsNotEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "IS NOT EMPTY", "Target1|Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain("( (c.CountryID is NOT NULL ) )");
        }

        [Test]
        public void getFilterQuery_AdhocStringIGRP_NOIsNotEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[IGRP_NO]", "IS NOT EMPTY", "Target1|Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain("( ( cast(s.[IGRP_NO] as varchar(100)) is NOT NULL) )");
        }

        [Test]
        public void getFilterQuery_AdhocStringOtherIsNotEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[OTHER]", "IS NOT EMPTY", "Target1|Target2");

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain("( (ISNULL(s.[OTHER], '') != ''  ))");
        }

        [Test]
        public void getFilterQuery_AdhocStringOtherIsEmptyProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[OTHER]", "IS EMPTY", "Target1|Target2", Enums.ViewType.ProductView);

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain("(( ps.[OTHER] is NULL or ps.[OTHER] = ''))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryIsEmptyProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "IS EMPTY", "Target1|Target2", Enums.ViewType.ProductView);

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain("( (c.CountryID is NULL ) )");
        }

        [Test]
        public void getFilterQuery_OpenActivityWithBrandAndBlast_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Open Activity", "OPEN CRITERIA", "Month", "1|6");
            filter.BrandID = 1;
            filter.Fields.Add(new Field
            {
                FilterType = Enums.FiltersType.Activity,
                Name = "OPEN BLASTID",
                Values = "2"
            });

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(OpenBlastInsertStatement);
            result.ShouldContain("and  bd.BrandID in (1) and b.Isdeleted = 0");
            result.ShouldContain("where bla.BlastID in (2)");
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year, 1, 1) - new DateTime(1900, 1, 1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year, 6, 30) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityWithBrandAndBlast_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter("Click Activity", "CLICK CRITERIA", "Month", "1|6");
            filter.BrandID = 1;
            filter.Fields.Add(new Field
            {
                FilterType = Enums.FiltersType.Activity,
                Name = "CLICK BLASTID",
                Values = "2"
            });

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain(ClickBlastInsertStatement);
            result.ShouldContain("and  bd.BrandID in (1) and b.Isdeleted = 0");
            result.ShouldContain("where bla.BlastID in (2)");
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year, 1, 1) - new DateTime(1900, 1, 1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year, 6, 30) - new DateTime(1900, 1, 1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryIsNotEmptyProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter("[COUNTRY]", "IS NOT EMPTY", "Target1|Target2", Enums.ViewType.ProductView);

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert          
            result.ShouldContain("( (c.CountryID is NOT NULL ) )");
        }


        [Test]
        public void getFilterQuery_ZipCodeRadiusMinus2_CorrectQuery()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                var filter = GetProfileFilter("ZIPCODE-RADIUS", "1|-2|-2", "");

                ShimLocation.ValidateBingAddressLocationString = (_, __) => new FrameworkUAD.Object.Location
                {
                    IsValid = true,
                    Latitude = 1,
                    Longitude = 2
                };

                // Act
                var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

                // Assert
                result.ShouldBe(ExpectedPubSubscriptionsQueryWithJoin + ExectedZipCodeRadiusMinus2);
            }
        }

        [Test]
        public void getFilterQuery_ZipCodeRadius2_CorrectQuery()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                var filter = GetProfileFilter("ZIPCODE-RADIUS", "1|2|2", "");

                ShimLocation.ValidateBingAddressLocationString = (_, __) => new FrameworkUAD.Object.Location
                {
                    IsValid = true,
                    Latitude = 1,
                    Longitude = 2
                };

                // Act
                var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

                // Assert
                result.ShouldBe(ExpectedPubSubscriptionsQueryWithJoin + ExectedZipCodeRadius2);
            }
        }

        [TestCase(FieldGroupStartWithm, BrandIdIsZero, Enums.ViewType.ProductView, Equal)]
        [TestCase(FieldGroupStartWithm, BrandIdDefault, Enums.ViewType.ProductView, Contains)]
        [TestCase(FieldGroupStartWithm, BrandIdIsZero, Enums.ViewType.ProductView, DoseNotContain)]
        [TestCase(FieldGroupStartWithm, BrandIdDefault, Enums.ViewType.ProductView, StartWith)]
        [TestCase(FieldGroupStartWithm, BrandIdIsZero, Enums.ViewType.ProductView, EndWith)]
        [TestCase(FieldGroupStartWithm, BrandIdIsZero, Enums.ViewType.ProductView, IsEmpty)]
        [TestCase(FieldGroupStartWithm, BrandIdIsZero, Enums.ViewType.ProductView, IsNotEmpty)]
        [TestCase(FieldGroupStartWithm, BrandIdDefault, Enums.ViewType.RecencyView, IsNotEmpty)]
        [TestCase(FieldGroupStartWithm, BrandIdIsZero, Enums.ViewType.RecencyView, IsNotEmpty)]
        [TestCase(FiledGroupStartWithdEndsWithqdate, BrandIdIsZero, Enums.ViewType.RecencyView, IsNotEmpty)]
        [TestCase(FiledGroupStartWithdEndsWithTransactiondate, BrandIdIsZero, Enums.ViewType.RecencyView, IsNotEmpty)]
        [TestCase(FiledGroupStartWithdEndsWithTransactiondate, BrandIdIsZero, Enums.ViewType.ProductView, IsNotEmpty)]
        [TestCase(FiledGroupStartWithdEndsWithStatusUpdatedDate, BrandIdIsZero, Enums.ViewType.RecencyView, IsNotEmpty)]
        [TestCase(FiledGroupStartWithdEndsWithDateCreated, BrandIdIsZero, Enums.ViewType.RecencyView, IsNotEmpty)]
        [TestCase(FiledGroupStartWithdEndsWithDateCreated, BrandIdDefault, Enums.ViewType.ProductView, IsNotEmpty)]
        [TestCase(FiledGroupStartWithdEndsWithDefaultCase, BrandIdIsZero, Enums.ViewType.RecencyView, IsNotEmpty)]
        [TestCase(FiledGroupStartWithdEndsWithDefaultCase, BrandIdDefault, Enums.ViewType.ProductView, IsNotEmpty)]
        [TestCase(FiledGroupStartWithdEndsWithDefault, BrandIdIsZero, Enums.ViewType.RecencyView, IsNotEmpty, Enums.FiltersType.Adhoc, Values)]
        [TestCase(FiledGroupStartWithdEndsWithDefault, BrandIdDefault, Enums.ViewType.ProductView, IsNotEmpty, Enums.FiltersType.Adhoc, Values)]
        [TestCase(FiledGroupStartWithiEndsWithProductCount, BrandIdIsZero, Enums.ViewType.ProductView, Equal)]
        [TestCase(FiledGroupStartWithiEndsWithProductCount, BrandIdDefault, Enums.ViewType.ProductView, Greater)]
        [TestCase(FiledGroupStartWithiEndsWithProductCount, BrandIdDefault, Enums.ViewType.ProductView, Lesser)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdDefault, Enums.ViewType.ProductView, Equal)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdIsZero, Enums.ViewType.ProductView, Equal)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdIsZero, Enums.ViewType.RecencyView, Equal)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdDefault, Enums.ViewType.ProductView, Range, Enums.FiltersType.Adhoc, DefaultValues)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdIsZero, Enums.ViewType.ProductView, Range, Enums.FiltersType.Adhoc, DefaultValues)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdIsZero, Enums.ViewType.RecencyView, Range, Enums.FiltersType.Adhoc, DefaultValues)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdDefault, Enums.ViewType.ProductView, Greater)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdIsZero, Enums.ViewType.ProductView, Greater)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdIsZero, Enums.ViewType.RecencyView, Greater)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdDefault, Enums.ViewType.ProductView, Lesser)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdIsZero, Enums.ViewType.ProductView, Lesser)]
        [TestCase(FiledGroupStartWithiEndsWithScore, BrandIdIsZero, Enums.ViewType.RecencyView, Lesser)]
        [TestCase(FiledGroupStartWithe, BrandIdIsZero, Enums.ViewType.ProductView, DoseNotContain, Enums.FiltersType.Adhoc, DefaultValues)]
        [TestCase(FieldGroupStartWitheEndsWithf, BrandIdIsZero, Enums.ViewType.ProductView, Equal, Enums.FiltersType.Adhoc, DefaultValues)]
        [TestCase(FieldGroupStartWitheEndsWithf, BrandIdIsZero, Enums.ViewType.RecencyView, Range, Enums.FiltersType.Adhoc, DefaultValues)]
        [TestCase(FieldGroupStartWitheEndsWithf, BrandIdIsZero, Enums.ViewType.RecencyView, Greater, Enums.FiltersType.Adhoc, DefaultValues)]
        [TestCase(FieldGroupStartWitheEndsWithf, BrandIdIsZero, Enums.ViewType.RecencyView, Lesser, Enums.FiltersType.Adhoc, DefaultValues)]
        [TestCase(FieldGroupStartWitheEndsWithf, BrandIdIsZero, Enums.ViewType.RecencyView, DoseNotContain, Enums.FiltersType.Adhoc, DefaultValues)]
        [TestCase(FieldGroupStartWitheEndsWithb, BrandIdIsZero, Enums.ViewType.RecencyView, DoseNotContain)]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdIsZero, Enums.ViewType.RecencyView, DoseNotContain)]
        [TestCase(FieldGroupStartWitheEndsWithx, BrandIdIsZero, Enums.ViewType.RecencyView, Equal, Enums.FiltersType.Adhoc, Values)]
        [TestCase(FieldGroupStartWitheEndsWithx, BrandIdIsZero, Enums.ViewType.RecencyView, Contains, Enums.FiltersType.Adhoc, Values)]
        [TestCase(FieldGroupStartWitheEndsWithx, BrandIdIsZero, Enums.ViewType.RecencyView, StartWith, Enums.FiltersType.Adhoc, Values)]
        [TestCase(FieldGroupStartWitheEndsWithx, BrandIdIsZero, Enums.ViewType.RecencyView, EndWith, Enums.FiltersType.Adhoc, Values)]
        [TestCase(FieldGroupStartWitheEndsWithx, BrandIdIsZero, Enums.ViewType.RecencyView, IsEmpty, Enums.FiltersType.Adhoc, Values)]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdDefault, Enums.ViewType.RecencyView, DoseNotContain)]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdIsZero, Enums.ViewType.ProductView, DoseNotContain, Enums.FiltersType.Dimension)]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdDefault, Enums.ViewType.RecencyView, DoseNotContain, Enums.FiltersType.Dimension)]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdIsZero, Enums.ViewType.RecencyView, DoseNotContain, Enums.FiltersType.Dimension)]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdIsZero, Enums.ViewType.RecordDetails, DoseNotContain, Enums.FiltersType.Dimension)]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdDefault, Enums.ViewType.RecordDetails, DoseNotContain, Enums.FiltersType.Dimension)]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdDefault, Enums.ViewType.RecordDetails, SearchAll, Enums.FiltersType.Activity, "0")]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdIsZero, Enums.ViewType.RecordDetails, SearchAll, Enums.FiltersType.Activity, "0")]
        [TestCase(FieldGroupStartWitheEndsWithd, BrandIdIsZero, Enums.ViewType.RecordDetails, SearchAll, Enums.FiltersType.Activity, "1")]
        public void getFilterQuery_AdhocStringCountryIsNotEmptyProductView_ReturnsCorrectQuery(
            string group,
            int brandID,
            Enums.ViewType viewType,
            string searchCondition,
            Enums.FiltersType filtersType = Enums.FiltersType.Adhoc,
            string values = Values)
        {
            // Arrange
            var filter = new Filter
            {
                BrandID = brandID,
                PubID = brandID,
                ViewType = viewType,
                Fields = CreateFieldListForAdhocType(group, searchCondition, filtersType, values)
            };

            // Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            // Assert          
            result.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void generateCombinationQuery_BySearchParameter_RetursQueryObject()
        {
            // Arrange
            var filters = new Filters(new KMPlatform.Object.ClientConnections(), 1);
            var selectedFilterOperation = string.Empty;
            var suppressedFilterOperation = string.Empty;
            var selectedFilterNo = "1";
            var suppressedFilterNo = "2";
            var addlFilters = string.Empty;
            var pubID = 1;
            var brandID = 1;
            var clientconnection = new ClientConnections();
            var filter = CreateFilterForCombinationQuery(brandID, 1);
            filters.Add(filter);
            filter = CreateFilterForCombinationQuery(brandID, 2);
            filters.Add(filter);

            // Act
            var result = Filter.generateCombinationQuery(filters, selectedFilterOperation, suppressedFilterOperation, selectedFilterNo, suppressedFilterNo, addlFilters, pubID, brandID, clientconnection);

            // Assert
            result.ShouldNotBeNull();
        }

        private Filter CreateFilterForCombinationQuery(int brandID, int filterNumber)
        {
            return new Filter
            {
                BrandID = brandID,
                PubID = brandID,
                ViewType = Enums.ViewType.ProductView,
                Fields = CreateFieldListForAdhocType("e|e|d", "Search All", Enums.FiltersType.Adhoc, Values),
                FilterNo = filterNumber
            };
        }

        private List<Field> CreateFieldListForAdhocType(string group, string searchCondition, Enums.FiltersType filtersType, string values)
        {
            var names = new string[] { "ADHOC", "OPEN CAMPAIGNS", "OPEN EMAIL SUBJECT", "LINK", "DOMAIN TRACKING", "URL", "CLICK CAMPAIGNS", "CLICK EMAIL SUBJECT", "CLICK CRITERIA", "VISIT CRITERIA", "DOMAIN TRACKING" };
            var filedList = new List<Field>();
            foreach (var item in names)
            {
                filedList.Add(new Field
                {
                    FilterType = filtersType,
                    Name = item,
                    SearchCondition = searchCondition,
                    Values = values,
                    Group = group,
                });
            }
            return filedList;
        }

        private Filter GetProfileFilter(string name, string condition, string values)
        {
            return new Filter
            {
                Fields = new List<Field>
                {
                    new Field
                    {
                        FilterType = Enums.FiltersType.Adhoc,
                        Name = name,
                        SearchCondition = condition,
                        Values = values
                    }
                }
            };
        }

        private Filter GetAdhocFilter(string group, string condition, string values, Enums.ViewType view = Enums.ViewType.RecordDetails)
        {
            return new Filter
            {
                ViewType = view,
                Fields = new List<Field>
                {
                    new Field
                    {
                        FilterType = Enums.FiltersType.Adhoc,
                        Name = "ADHOC",
                        SearchCondition = condition,
                        Values = values,
                        Group = group,
                    }
                }
            };
        }

        private Filter GetActivityFilter(string name, string criteria, string condition, string values)
        {
            return new Filter
            {
                Fields = new List<Field>
                {
                    new Field
                    {
                        FilterType = Enums.FiltersType.Activity,
                        Name = criteria,
                        Values = "1"
                    },
                    new Field
                    {
                        FilterType = Enums.FiltersType.Activity,
                        Name = name,
                        SearchCondition = condition,
                        Values = values
                    }
                }
            };
        }

        private Filter GetQualificationDateFilter(string condition, string values)
        {
            return new Filter
            {
                Fields = new List<Field>
                {
                    new Field
                    {
                        FilterType = Enums.FiltersType.Activity,
                        Name = "Open CRITERIA",
                        Values = "0"
                    },
                    new Field
                    {
                        FilterType = Enums.FiltersType.Adhoc,
                        Name = "ADHOC",
                        Group = "d|qdate",
                        SearchCondition = condition,
                        Values = values
                    }
                }
            };
        }

        private Filter GetEColumnFilter(string condition, string values)
        {
            return new Filter
            {
                Fields = new List<Field>
                {
                    new Field
                    {
                        FilterType = Enums.FiltersType.Activity,
                        Name = "Open CRITERIA",
                        Values = "0"
                    },
                    new Field
                    {
                        FilterType = Enums.FiltersType.Adhoc,
                        Name = "ADHOC",
                        Group = "e|column|d|qdate",
                        SearchCondition = condition,
                        Values = values
                    }
                }
            };
        }

        private Filter GetPermissionFilter(string name, Enums.ViewType viewType, string values)
        {
            return new Filter
            {
                ViewType = viewType,
                Fields = new List<Field>
                {
                    new Field
                    {
                        FilterType = Enums.FiltersType.Adhoc,
                        Name = name,
                        Values = values
                    }
                }
            };
        }
    }
}
