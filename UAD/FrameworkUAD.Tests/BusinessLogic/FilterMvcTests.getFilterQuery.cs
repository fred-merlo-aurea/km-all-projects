using System;
using System.Linq;
using System.Text;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Object;
using FrameworkUAD.Object.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using static System.Reflection.BindingFlags;
using static FrameworkUAD.BusinessLogic.Enums;
using FilterMVC = FrameworkUAD.BusinessLogic.FilterMVC;

namespace FrameworkUAD.Tests.BusinessLogic
{
    [TestFixture]
    public partial class FilterMVCTests
    {
        private const string OpenActivityInsertStatement = "Insert into #tempSOA  select so.SubscriptionID  from PubSubscriptions pso";
        private const string SubscriptionExtensionSelectStatement = "(select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)";
        private const string ClickActivityInsertStatement = "Insert into #tempSCA  select sc.SubscriptionID from PubSubscriptions psc";
        private const string VisitActivityInsertStatement = "Insert into #tempSVA  select sv.SubscriptionID from  SubscriberVisitActivity";
        private const string ClickBlastInsertStatement = "Insert into #tempCblast SELECT distinct blastid FROM blast bla WITH (nolock)";
        private const string OpenBlastInsertStatement = "insert into #tempOblast SELECT distinct blastid FROM blast bla WITH(nolock)";
        private const string CountrySelectStatement = "select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join UAD_Lookup..Country c";
        private const string CountryLeftJoinStatement = "select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  left outer join UAD_Lookup..Country c";
        private const string ExpectedPermissionQueryWithJoin = "select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ({0} = 0 OR {0} = 1 OR {0} is null);";
        private const string ExpectedPermissionQuery = "select  from pubsubscriptions ps  with (nolock)  where ({0} = 0 OR {0} = 1 OR {0} is null);";
        private const string ExpectedProfileQuery = "select  from pubsubscriptions ps  with (nolock)  where (isnull({0}, '') != ''  OR isnull({0}, '') = '' );";
        private const string ExpectedProfileQueryWithJoin = "select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ({0} = 1 OR {0} = 0);";

        private const string ExpectedPubSubscriptionsQuery = "select  from pubsubscriptions ps  with (nolock)  where ";
        private const string ExpectedPubSubscriptionsQueryWithJoin = "select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ";
        private const string ClickQueryMethodName = "ClickQuery";
        private const string TestClickCondition = "MyClickCondition";
        private const int FilterBrandId = 42;
        private const string ClickSearchTypeSearchAll = "Search All";

        [Test]
        public void getFilterQuery_MailPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionMail, ViewType.ProductView, "0,1,-1");
  
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQuery, "ps.MailPermission"));
        }

        [Test]
        public void getFilterQuery_MailPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionMail, ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQueryWithJoin, "s.MailPermission"));
        }

        [Test]
        public void getFilterQuery_PhonePermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionPhone, ViewType.ProductView, "0,1,-1");
  
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQuery, "ps.PhonePermission"));
        }

        [Test]
        public void getFilterQuery_PhonePermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionPhone, ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQueryWithJoin, "s.PhonePermission"));
        }

        [Test]
        public void getFilterQuery_TextPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionText, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQuery, "ps.TextPermission"));
        }

        [Test]
        public void getFilterQuery_TextPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionText, ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQueryWithJoin, "s.TextPermission"));
        }

        [Test]
        public void getFilterQuery_ThirdPartyPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionThirdParty, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQuery, "ps.ThirdPartyPermission"));
        }

        [Test]
        public void getFilterQuery_ThirdPartyPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionThirdParty, ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQueryWithJoin, "s.ThirdPartyPermission"));
        }
        
        [Test]
        public void getFilterQuery_FaxPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionFax, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQuery, "ps.FaxPermission"));
        }

        [Test]
        public void getFilterQuery_FaxPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionFax, ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQueryWithJoin, "s.FaxPermission"));
        }

        [Test]
        public void getFilterQuery_EmailRenewPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionEmailRenew, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQuery, "ps.EmailRenewPermission"));
        }

        [Test]
        public void getFilterQuery_EmailRenewPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionEmailRenew, ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQueryWithJoin, "s.EmailRenewPermission"));
        }

        [Test]
        public void getFilterQuery_OtherProductsPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionOtherProducts, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQuery, "ps.OtherProductsPermission"));
        }

        [Test]
        public void getFilterQuery_OtherProductsPermissionRecordDetails_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionOtherProducts, ViewType.RecordDetails, "0,1,-1");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedPermissionQueryWithJoin, "s.OtherProductsPermission"));
        }
        
        [Test]
        public void getFilterQuery_AdhocStringCountryEqual_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, Equal, "Target1,Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( c.ShortName = 'Target1') OR ( c.ShortName = 'Target2'))");
        }
        
        [Test]
        public void getFilterQuery_AdhocStringCountryContains_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, Contains, "Target1,Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( PATINDEX('%Target1%', c.ShortName) > 0 ) OR ( PATINDEX('%Target2%', c.ShortName) > 0 ))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryStartWith_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, StartWith, "Target1,Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( PATINDEX('Target1%', c.ShortName) > 0 ) OR ( PATINDEX('Target2%', c.ShortName) > 0 ))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryEndWith_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, EndWith, "Target1,Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( PATINDEX('%Target1', c.ShortName) > 0 ) OR ( PATINDEX('%Target2', c.ShortName) > 0 ))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryDoesNotContain_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, DoesNotContain, "Target1,Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("(( isnull(c.ShortName,'') not like '%Target1%'  ) AND ( isnull(c.ShortName,'') not like '%Target2%'  ))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryRange_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, Range, "Target1|Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(CountrySelectStatement);
            result.ShouldContain("( (substring(c.ShortName, 1, len('Target1')) between 'Target1' and 'Target2'))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryIsEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, IsEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(CountryLeftJoinStatement);
            result.ShouldContain("( (c.CountryID is NULL ) )");
        }

        [Test]
        public void getFilterQuery_AdhocStringIGRP_NOIsEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(IgrpNo, IsEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain("( ( cast(s.[IGRP_NO] as varchar(100)) is NULL) )");
        }

        [Test]
        public void getFilterQuery_AdhocStringOtherIsEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Other, IsEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain("(( s.[OTHER] is NULL or s.[OTHER] = ''))");
        }

        [Test]
        public void getFilterQuery_AdhocStringCountryIsNotEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, IsNotEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain("( (c.CountryID is NOT NULL ) )");
        }

        [Test]
        public void getFilterQuery_AdhocStringIGRP_NOIsNotEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(IgrpNo, IsNotEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain("( ( cast(s.[IGRP_NO] as varchar(100)) is NOT NULL) )");
        }

        [Test]
        public void getFilterQuery_AdhocStringOtherIsNotEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Other, IsNotEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain("( (ISNULL(s.[OTHER], '') != ''  ))");
        }

        
        [Test]
        public void getFilterQuery_EColumnDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "EXP:Today|EXP:Today", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column} >='{DateTime.Now.ToShortDateString()}' and {Column} <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_EColumnDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column} >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and {Column} <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_EColumnDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "1/1/1900|2/2/1902", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column} >= '1/1/1900'  and {Column} <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getFilterQuery_EColumnXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "1YR", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column}>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_EColumnXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "6mon", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column}>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_EColumnXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "7", "e|column|d|qdate");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column}>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_EColumnYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionYear, "2016|2017", "e|column|d|qdate");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"(year({Column}) >= '2016'  and year({Column}) <= '2017' )");
        }

        [Test]
        public void getFilterQuery_EColumnMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionMonth, "1|6", "e|column|d|qdate");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain(SubscriptionExtensionSelectStatement);
            result.ShouldContain($"(month({Column}) >= '1'  and month({Column}) <= '6' )");
        }

        [Test]
        public void getFilterQuery_DateAdhocDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "EXP:Today|EXP:Today", "d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_DateAdhocDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]", "d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_DateAdhocDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "1/1/1900|2/2/1902", "d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >= '1/1/1900'  and ps.QualificationDate <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getFilterQuery_DateAdhocXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "1YR", "d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_DateAdhocXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "6mon", "d|qdate");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_DateAdhocXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "7", "d|qdate");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_DateAdhocYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionYear, "2016|2017", "d|qdate");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(year(ps.QualificationDate) >= '2016'  and year(ps.QualificationDate) <= '2017' )");
        }

        [Test]
        public void getFilterQuery_DateAdhocMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionMonth, "1|6", "d|qdate");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(month(ps.QualificationDate) >= '1'  and month(ps.QualificationDate) <= '6' )");
        }

        [Test]
        public void getFilterQuery_VisitActivityDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today} and  DateNumber <= {today}");
        }

        [Test]
        public void getFilterQuery_VisitActivityDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today - 7} and  DateNumber <= {today - 1}");
        }

        [Test]
        public void getFilterQuery_VisitActivityDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= 0 and  DateNumber <= {(new DateTime(1902,2,2) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddYears(-1) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddMonths(-6) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddDays(-7) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(2016,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(2017,12,31) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_VisitActivityMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year,6,30) - new DateTime(1900,1,1)).TotalDays}");
        }
        
        [Test]
        public void getFilterQuery_OpenActivityDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today} and  DateNumber <= {today}");
        }

        [Test]
        public void getFilterQuery_OpenActivityDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today - 7} and  DateNumber <= {today - 1}");
        }

        [Test]
        public void getFilterQuery_OpenActivityDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= 0 and  DateNumber <= {(new DateTime(1902,2,2) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddYears(-1) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddMonths(-6) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddDays(-7) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(2016,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(2017,12,31) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_OpenActivityMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year,6,30) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime >='{DateTime.Now.ToShortDateString()}' and bla.SendTime <='{DateTime.Now.ToShortDateString()} 23:59:59'");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and bla.SendTime <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59'");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain("bla.SendTime >= '1/1/1900'  and bla.SendTime <= '2/2/1902 23:59:59'");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddYears(-1).ToShortDateString()}'");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}'");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddDays(-7).ToShortDateString()}'");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain("year(bla.SendTime) >= '2016'  and year(bla.SendTime) <= '2017'");
        }

        [Test]
        public void getFilterQuery_ClickEmailSentDateMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain("month(bla.SendTime) >= '1'  and month(bla.SendTime) <= '6'");
        }
        
        [Test]
        public void getFilterQuery_OpenEmailSentDateDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime >='{DateTime.Now.ToShortDateString()}' and bla.SendTime <='{DateTime.Now.ToShortDateString()} 23:59:59'");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and bla.SendTime <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59'");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain("bla.SendTime >= '1/1/1900'  and bla.SendTime <= '2/2/1902 23:59:59'");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddYears(-1).ToShortDateString()}'");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}'");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddDays(-7).ToShortDateString()}'");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain("year(bla.SendTime) >= '2016'  and year(bla.SendTime) <= '2017'");
        }

        [Test]
        public void getFilterQuery_OpenEmailSentDateMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain("month(bla.SendTime) >= '1'  and month(bla.SendTime) <= '6'");
        }

        [Test]
        public void getFilterQuery_QualificationDateDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_QualificationDateDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getFilterQuery_QualificationDateDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain("(ps.QualificationDate >= '1/1/1900'  and ps.QualificationDate <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getFilterQuery_QualificationDateXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_QualificationDateXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_QualificationDateXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getFilterQuery_QualificationDateYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(year(ps.QualificationDate) >= '2016'  and year(ps.QualificationDate) <= '2017' )");
        }

        [Test]
        public void getFilterQuery_QualificationDateMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(OpenActivityInsertStatement);
            result.ShouldContain($"(month(ps.QualificationDate) >= '1'  and month(ps.QualificationDate) <= '6' )");
        }

        [Test]
        public void getFilterQuery_ClickActivityDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today} and  DateNumber <= {today}");
        }

        [Test]
        public void getFilterQuery_ClickActivityDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today - 7} and  DateNumber <= {today - 1}");
        }

        [Test]
        public void getFilterQuery_ClickActivityDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= 0 and  DateNumber <= {(new DateTime(1902,2,2) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddYears(-1) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddMonths(-6) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddDays(-7) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(2016,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(2017,12,31) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_ClickActivityMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain(ClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year,6,30) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getFilterQuery_QualificationYearLessThanNow_CorrectQuery()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                var filter = GetProfileFilter("QUALIFICATIONYEAR", ViewType.ProductView, "2017,2018");
                filter.Fields.Add(new Object.FilterDetails
                {
                    Name = "Product",
                    Values = "100"
                });

                ShimProduct.AllInstances.SelectInt32ClientConnectionsBooleanBoolean = (_, __, ___, ____, _____) => new Entity.Product
                {
                    YearStartDate = "1/1",
                    YearEndDate = "12/31"
                };

                // Act
                var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

                // Assert
                result.ShouldBe(ExpectedPubSubscriptionsQuery +
                                "( ps.QualificationDate between convert(varchar(20), DATEADD(year, -2016, '1/1/2018'),111) " +
                                " and  convert(varchar(20), DATEADD(year, -2016, '12/31/2018'),111) + ' 23:59:59'" +
                                " OR  ps.QualificationDate between convert(varchar(20), DATEADD(year, -2017, '1/1/2018'),111) " +
                                " and  convert(varchar(20), DATEADD(year, -2017, '12/31/2018'),111) + ' 23:59:59') and ps.pubid in (100 ) ;");
            }
        }

        [Test]
        public void getFilterQuery_QualificationYearGreaterThanNow_CorrectQuery()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                var filter = GetProfileFilter("QUALIFICATIONYEAR", ViewType.ProductView, "2017,2018");
                filter.Fields.Add(new Object.FilterDetails
                {
                    Name = "Product",
                    Values = "100"
                });

                ShimProduct.AllInstances.SelectInt32ClientConnectionsBooleanBoolean = (_, __, ___, ____, _____) => new Entity.Product
                {
                    YearStartDate = "12/31",
                    YearEndDate = "1/1"
                };

                // Act
                var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

                // Assert
                result.ShouldBe(ExpectedPubSubscriptionsQuery +
                                "( ps.QualificationDate between convert(varchar(20), DATEADD(year, -2016, '12/31/2017'),111) " +
                                " and  convert(varchar(20), DATEADD(year, -2016, '12/30/2018'),111) + ' 23:59:59'" +
                                " OR  ps.QualificationDate between convert(varchar(20), DATEADD(year, -2017, '12/31/2017'),111) " +
                                " and  convert(varchar(20), DATEADD(year, -2017, '12/30/2018'),111) + ' 23:59:59') and ps.pubid in (100 ) ;");
            }
        }

        [Test]
        public void getFilterQuery_QualificationYearProductIsEmpty_CorrectQuery()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                var filter = GetProfileFilter("QUALIFICATIONYEAR", ViewType.ProductView, "2017,2018");
                filter.Fields.Add(new Object.FilterDetails
                {
                    Name = "Product",
                    Values = "100"
                });

                ShimProduct.AllInstances.SelectInt32ClientConnectionsBooleanBoolean = (_, __, ___, ____, _____) => new Entity.Product
                {
                };

                var todayText = DateTime.Now.ToString("M/d/yyyy");
                var tomorrowOfLastYearText = DateTime.Now.AddYears(-1).AddDays(1).ToString("M/d/yyyy");
                var expectedQuery = string.Format(ExpectedPubSubscriptionsQuery +
                                    "( ps.QualificationDate between convert(varchar(20), DATEADD(year, -2016, '{0}'),111) " +
                                    " and  convert(varchar(20), DATEADD(year, -2016, '{1}'),111) + ' 23:59:59'" +
                                    " OR  ps.QualificationDate between convert(varchar(20), DATEADD(year, -2017, '{0}'),111) " +
                                    " and  convert(varchar(20), DATEADD(year, -2017, '{1}'),111) + ' 23:59:59') and ps.pubid in (100 ) ;", todayText, tomorrowOfLastYearText);
                // Act
                var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

                // Assert
                result.ShouldBe(expectedQuery);
            }
        }

        [Test]
        public void getFilterQuery_EmailProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfileEmail, ViewType.ProductView, "1,0");
  
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedProfileQuery, "ps.Email"));
        }

        [Test]
        public void getFilterQuery_EmailDetailsView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfileEmail, ViewType.RecordDetails, "1,0");
  
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedProfileQueryWithJoin, "s.emailexists"));
        }

        [Test]
        public void getFilterQuery_PhoneProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePhone, ViewType.ProductView, "1,0");
  
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedProfileQuery, "ps.Phone"));
        }

        [Test]
        public void getFilterQuery_PhoneDetailsView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePhone, ViewType.RecordDetails, "1,0");
  
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedProfileQueryWithJoin, "s.phoneexists"));
        }

        [Test]
        public void getFilterQuery_FaxProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfileFax, ViewType.ProductView, "1,0");
  
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedProfileQuery, "ps.fax"));
        }

        [Test]
        public void getFilterQuery_FaxDetailsView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfileFax, ViewType.RecordDetails, "1,0");
  
            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null);

            // Assert
            result.ShouldBe(string.Format(ExpectedProfileQueryWithJoin, "s.faxexists"));
        }

        [Test]
        public void getFilterQuery_ZipCodeRadiusMinus2_CorrectQuery()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                var filter = GetProfileFilter("ZIPCODE-RADIUS", "1|-2|-2", "");

                ShimLocation.ValidateBingAddressLocationString = (_, __) => new Location
                {
                    IsValid = true,
                    Latitude = 1,
                    Longitude = 2
                };

                // Act
                var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

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

                ShimLocation.ValidateBingAddressLocationString = (_, __) => new Location
                {
                    IsValid = true,
                    Latitude = 1,
                    Longitude = 2
                };

                // Act
                var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

                // Assert
                result.ShouldBe(ExpectedPubSubscriptionsQueryWithJoin + ExectedZipCodeRadius2);
            }
        }

        [Test] 
        public void getFilterQuery_FilterTypesOnly_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.ProductView, "Value");
            filter.Fields.Add(new Object.FilterDetails{ Name = "XACT"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "QSOURCE TYPE"});

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldContain("select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock)");
            result.ShouldContain("select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock)");
            result.ShouldContain("select CodeID from UAD_Lookup..Code with (nolock)");
        }

        [Test]
        public void getFilterQuery_FilterTypesAndCodes_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.ProductView, "Value");
            filter.Fields.Add(new Object.FilterDetails{ Name = "XACT"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "QSOURCE TYPE"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "CATEGORY CODE"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "TRANSACTION CODE"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "QSOURCE CODE"});

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, null, false);

            // Assert
            result.ShouldNotContain("select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock)");
            result.ShouldNotContain("select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock)");
            result.ShouldNotContain("select CodeID from UAD_Lookup..Code with (nolock)");
        }

        [Test]
        public void ClickQuery_SearchAllFilterBrandIdZero_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");

            // Act
            var result = CallClickQueryMethod(filter, 2, ClickSearchTypeSearchAll, true, new StringBuilder());

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from SubscriberClickActivity sc  with (NOLOCK)  join #tempCblast tcb with (NOLOCK) on "
                            + "sc.blastID = tcb.blastID  group by sc.SubscriptionID HAVING Count(sc.ClickActivityID) >= 2");
        }
        
        [Test]
        public void ClickQuery_SearchAllFilterBrandSet_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");
            filter.BrandID = FilterBrandId;

            // Act
            var result = CallClickQueryMethod(filter, 2, ClickSearchTypeSearchAll, true, new StringBuilder());

            // Assert
            result.ShouldBe("select x.subscriptionID from ( select sc.SubscriptionID, sc.ClickActivityID  from  SubscriberClickActivity sc  with (NOLOCK)"
                            + "  join PubSubscriptions psc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID  join #tempCblast tcb with (NOLOCK) "
                            + "on sc.blastID = tcb.blastID  where  pubID in (select PubID from BrandDetails bd  with (nolock) join Brand b with (nolock) on "
                            + "bd.BrandID = b.BrandID where bd.BrandID in (42) and  b.Isdeleted = 0)union select sc1.SubscriptionID, sc1.ClickActivityID  from "
                            + " SubscriberClickActivity sc1  with (NOLOCK)  join #tempCblast tcb with (NOLOCK) on sc1.blastID = tcb.blastID  where "
                            + " sc1.pubsubscriptionid IS NULL  ) x GROUP BY x.subscriptionid  HAVING Count(x.ClickActivityID) >= 2");
        }   
        
        [Test]
        public void ClickQuery_SearchAllFilterBrandZeroJointBlastFalse_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");

            // Act
            var result = CallClickQueryMethod(filter, 2, ClickSearchTypeSearchAll, false, new StringBuilder());

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from SubscriberClickActivity sc  with (NOLOCK)  group by sc.SubscriptionID "
                            + "HAVING Count(sc.ClickActivityID) >= 2");
        }

        [Test]
        public void ClickQuery_SearchAllFilterBrandSetJointBlastFalse_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");
            filter.BrandID = FilterBrandId;

            // Act
            var result = CallClickQueryMethod(filter, 2, ClickSearchTypeSearchAll, false, new StringBuilder());

            // Assert
            result.ShouldBe("select x.subscriptionID from ( select sc.SubscriptionID, sc.ClickActivityID  from  SubscriberClickActivity sc"
                            + "  with (NOLOCK)  join PubSubscriptions psc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID  "
                            + "where  pubID in (select PubID from BrandDetails bd  with (nolock) join Brand b with (nolock) on bd.BrandID = "
                            + "b.BrandID where bd.BrandID in (42) and  b.Isdeleted = 0)union select sc1.SubscriptionID, sc1.ClickActivityID "
                            + " from  SubscriberClickActivity sc1  with (NOLOCK)  where  sc1.pubsubscriptionid IS NULL  ) x GROUP BY "
                            + "x.subscriptionid  HAVING Count(x.ClickActivityID) >= 2");
        } 
        
        [Test]
        public void ClickQuery_SearchAllFilterBrandIdZeroClickCondition_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");

            // Act
            var result = CallClickQueryMethod(filter, 2, ClickSearchTypeSearchAll, true, new StringBuilder(TestClickCondition));

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from SubscriberClickActivity sc  with (NOLOCK)  join #tempCblast tcb with"
                            + " (NOLOCK) on sc.blastID = tcb.blastID MyClickCondition group by sc.SubscriptionID HAVING Count(sc.ClickActivityID) >= 2");
        }
        
        [Test]
        public void ClickQuery_SearchAllFilterBrandSetClickCondition_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");
            filter.BrandID = FilterBrandId;

            // Act
            var result = CallClickQueryMethod(filter, 2, ClickSearchTypeSearchAll, true, new StringBuilder(TestClickCondition));

            // Assert
            result.ShouldBe("select x.subscriptionID from ( select sc.SubscriptionID, sc.ClickActivityID  from  SubscriberClickActivity sc  with (NOLOCK)"
                            + "  join PubSubscriptions psc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID  join #tempCblast tcb with "
                            + "(NOLOCK) on sc.blastID = tcb.blastID MyClickCondition and  pubID in (select PubID from BrandDetails bd  with (nolock) join"
                            + " Brand b with (nolock) on bd.BrandID = b.BrandID where bd.BrandID in (42) and  b.Isdeleted = 0)union select sc1.SubscriptionID,"
                            + " sc1.ClickActivityID  from  SubscriberClickActivity sc1  with (NOLOCK)  join #tempCblast tcb with (NOLOCK) on sc1.blastID ="
                            + " tcb.blastID MyClickCondition and  sc1.pubsubscriptionid IS NULL  ) x GROUP BY x.subscriptionid  HAVING Count(x.ClickActivityID) >= 2");
        }   
        
        [Test]
        public void ClickQuery_SearchAllTypeFilterBrandZeroJointBlastFalseClickCondition_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");

            // Act
            var result = CallClickQueryMethod(filter, 2, ClickSearchTypeSearchAll, false, new StringBuilder(TestClickCondition));

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from SubscriberClickActivity sc  with (NOLOCK) MyClickCondition group by sc.SubscriptionID HAVING"
                            + " Count(sc.ClickActivityID) >= 2");
        }

        [Test]
        public void ClickQuery_SearchAllTypeFilterBrandSetJointBlastFalseClickCondition_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");
            filter.BrandID = FilterBrandId;

            // Act
            var result = CallClickQueryMethod(filter, 2, ClickSearchTypeSearchAll, false, new StringBuilder(TestClickCondition));

            // Assert
            result.ShouldBe("select x.subscriptionID from ( select sc.SubscriptionID, sc.ClickActivityID  from  SubscriberClickActivity sc  with (NOLOCK)"
                            + "  join PubSubscriptions psc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID MyClickCondition and  pubID "
                            + "in (select PubID from BrandDetails bd  with (nolock) join Brand b with (nolock) on bd.BrandID = b.BrandID where bd.BrandID "
                            + "in (42) and  b.Isdeleted = 0)union select sc1.SubscriptionID, sc1.ClickActivityID  from  SubscriberClickActivity sc1  with "
                            + "(NOLOCK) MyClickCondition and  sc1.pubsubscriptionid IS NULL  ) x GROUP BY x.subscriptionid  HAVING Count(x.ClickActivityID) >= 2");
        } 
        
        [Test]
        public void ClickQuery_EmptyTypeFilterBrandIdZeroClickCountZero_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");

            // Act
            var result = CallClickQueryMethod(filter, 2, string.Empty, true, new StringBuilder());

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on sc.PubSubscriptionID"
                            + " = psc.PubSubscriptionID  join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID  group by sc.SubscriptionID having COUNT(sc.ClickActivityID) >= 2");
        }
        
        [Test]
        public void ClickQuery_EmptyTypeFilterBrandSetClickCountZero_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");
            filter.BrandID = FilterBrandId;

            // Act
            var result = CallClickQueryMethod(filter, 2, string.Empty, true, new StringBuilder());

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on sc.PubSubscriptionID = "
                            + "psc.PubSubscriptionID  JOIN branddetails bd  with (NOLOCK) ON bd.pubID = psc.pubID join Brand b  with (NOLOCK) on b.BrandID = bd.BrandID  join"
                            + " #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID  where  bd.BrandID in (42) and b.Isdeleted = 0 group by sc.SubscriptionID having "
                            + "COUNT(sc.ClickActivityID) >= 2");
        }   
        
        [Test]
        public void ClickQuery_EmptyTypeFilterBrandZeroJointBlastFalseClick_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");

            // Act
            var result = CallClickQueryMethod(filter, 2, string.Empty, false, new StringBuilder());

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on sc.PubSubscriptionID = "
                            + "psc.PubSubscriptionID  group by sc.SubscriptionID having COUNT(sc.ClickActivityID) >= 2");
        }

        [Test]
        public void ClickQuery_EmptyTypeFilterBrandSetJointBlastFalseClick_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");
            filter.BrandID = FilterBrandId;

            // Act
            var result = CallClickQueryMethod(filter, 0, string.Empty, false, new StringBuilder());

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with "
                            + "(NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID  JOIN branddetails bd  with (NOLOCK) ON bd.pubID ="
                            + " psc.pubID join Brand b  with (NOLOCK) on b.BrandID = bd.BrandID  where  bd.BrandID in (42) and b.Isdeleted ="
                            + " 0 group by sc.SubscriptionID ");
        } 
        
        [Test]
        public void ClickQuery_EmptyTypeFilterBrandIdZeroClickConditionClick_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");

            // Act
            var result = CallClickQueryMethod(filter, 2, string.Empty, true, new StringBuilder(TestClickCondition));

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on"
                            + " sc.PubSubscriptionID = psc.PubSubscriptionID  join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID MyClickCondition "
                            + "group by sc.SubscriptionID having COUNT(sc.ClickActivityID) >= 2");
        }
        
        [Test]
        public void ClickQuery_EmptyTypeFilterBrandSetClickConditionClick_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");
            filter.BrandID = FilterBrandId;

            // Act
            var result = CallClickQueryMethod(filter, 2, string.Empty, true, new StringBuilder(TestClickCondition));

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on "
                            + "sc.PubSubscriptionID = psc.PubSubscriptionID  JOIN branddetails bd  with (NOLOCK) ON bd.pubID = psc.pubID join Brand b  with "
                            + "(NOLOCK) on b.BrandID = bd.BrandID  join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID MyClickCondition and  bd.BrandID"
                            + " in (42) and b.Isdeleted = 0 group by sc.SubscriptionID having COUNT(sc.ClickActivityID) >= 2");
        }   
        
        [Test]
        public void ClickQuery_EmptyTypeFilterBrandZeroJointBlastFalseClickCondition_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");

            // Act
            var result = CallClickQueryMethod(filter, 2, string.Empty, false, new StringBuilder(TestClickCondition));

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on "
                            + "sc.PubSubscriptionID = psc.PubSubscriptionID MyClickCondition group by sc.SubscriptionID having COUNT(sc.ClickActivityID) >= 2");
        }

        [Test]
        public void ClickQuery_EmptyTypeFilterBrandSetJointBlastFalseClickCondition_ClickQueryWritten()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.RecordDetails, "Value");
            filter.BrandID = FilterBrandId;

            // Act
            var result = CallClickQueryMethod(filter, 2, string.Empty, false, new StringBuilder(TestClickCondition));

            // Assert
            result.ShouldBe(" select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on "
                            + "sc.PubSubscriptionID = psc.PubSubscriptionID  JOIN branddetails bd  with (NOLOCK) ON bd.pubID = psc.pubID join Brand b "
                            + " with (NOLOCK) on b.BrandID = bd.BrandID MyClickCondition and  bd.BrandID in (42) and b.Isdeleted = 0 group by "
                            + "sc.SubscriptionID having COUNT(sc.ClickActivityID) >= 2");
        }   

        private static string CallClickQueryMethod(
            Object.FilterMVC filter,
            int clickcount,
            string clickSearchType,
            bool joinBlastforClick,
            StringBuilder clickCondition)
        {
            return (string)CallMethod(
                ClickQueryMethodName,
                new object[] 
                {
                    filter,
                    clickcount,
                    clickSearchType,
                    joinBlastforClick,
                    clickCondition
                });
        }

        private static object CallMethod(string methodName, object[] parametersValues = null, object instance = null)
        {
            parametersValues = parametersValues ?? new object[0];
            var methodInfo = typeof(FilterMVC)
                .GetMethods(Instance | NonPublic | Public | Static)
                .FirstOrDefault(info => info.Name == methodName);

            return methodInfo?.Invoke(methodInfo.IsStatic ? null : instance, parametersValues);

        }
    }
}
