using System;
using System.Collections.Generic;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using FrameworkUAD.Object;
using FrameworkUAD.Object.Fakes;
using NUnit.Framework;
using Shouldly;
using static FrameworkUAD.BusinessLogic.Enums;
using FilterMVC = FrameworkUAD.BusinessLogic.FilterMVC;

namespace FrameworkUAD.Tests.BusinessLogic
{
    [TestFixture]
    public partial class FilterMVCTests
    {
        private const string ArchiveCountrySelectStatement = "select  from IssueArchiveProductSubscription ps  with (nolock)  join UAD_Lookup..Country c";
        private const string ArchiveCountryLeftJoinStatement = "select  from IssueArchiveProductSubscription ps  with (nolock)  left outer join UAD_Lookup..Country c";
        private const string ArchiveOpenActivityInsertStatement = "Insert into #tempSOA  select so.SubscriptionID  from IssueArchiveProductSubscription pso";
        private const string ArchiveClickActivityInsertStatement = "Insert into #tempSCA  select sc.SubscriptionID from IssueArchiveProductSubscription psc";
        private const string ArchiveSubscriptionExtensionSelectStatement = "(select ips.PubSubscriptionID FROM IssueArchivePubSubscriptionsExtension E with (nolock)";
        private const string ArchiveExpectedPermissionsQuery = "select  from IssueArchiveProductSubscription ps  with (nolock)  where  IssueID = 0 and ({0} = 0 OR {0} = 1 OR {0} is null);";
        private const string ArchiveExpectedProfileQuery = "select  from IssueArchiveProductSubscription ps  with (nolock)  where  IssueID = 0 and (isnull({0}, '') != ''  OR isnull({0}, '') = '' );";
        private const string ArchiveExpectedPubSubscriptionsQuery = "select  from IssueArchiveProductSubscription ps  with (nolock)  where  IssueID = 0 and ";

        [Test]
        public void getProductArchiveFilterQuery_MailPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionMail, ViewType.ProductView, "0,1,-1");
  
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedPermissionsQuery, "ps.MailPermission"));
        }

        [Test]
        public void getProductArchiveFilterQuery_PhonePermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionPhone, ViewType.ProductView, "0,1,-1");
  
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedPermissionsQuery, "ps.PhonePermission"));
        }

        [Test]
        public void getProductArchiveFilterQuery_EmailRenewPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionEmailRenew, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedPermissionsQuery, "ps.EmailRenewPermission"));
        }

        [Test]
        public void getProductArchiveFilterQuery_TextPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionText, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedPermissionsQuery, "ps.TextPermission"));
        }
        
        [Test]
        public void getProductArchiveFilterQuery_ThirdPartyPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionThirdParty, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedPermissionsQuery, "ps.ThirdPartyPermission"));
        }
        
        [Test]
        public void getProductArchiveFilterQuery_FaxPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionFax, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedPermissionsQuery, "ps.FaxPermission"));
        }
        
        [Test]
        public void getProductArchiveFilterQuery_OtherProductsPermissionProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePermissionOtherProducts, ViewType.ProductView, "0,1,-1");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedPermissionsQuery, "ps.OtherProductsPermission"));
        }
        
        [Test]
        public void getProductArchiveFilterQuery_AdhocStringCountryEqual_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, Equal, "Target1,Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveCountrySelectStatement);
            result.ShouldContain("(( c.ShortName = 'Target1') OR ( c.ShortName = 'Target2'))");
        }
        
        [Test]
        public void getProductArchiveFilterQuery_AdhocStringCountryContains_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, Contains, "Target1,Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveCountrySelectStatement);
            result.ShouldContain("(( PATINDEX('%Target1%', c.ShortName) > 0 ) OR ( PATINDEX('%Target2%', c.ShortName) > 0 ))");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringCountryStartWith_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, StartWith, "Target1,Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveCountrySelectStatement);
            result.ShouldContain("(( PATINDEX('Target1%', c.ShortName) > 0 ) OR ( PATINDEX('Target2%', c.ShortName) > 0 ))");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringCountryEndWith_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, EndWith, "Target1,Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveCountrySelectStatement);
            result.ShouldContain("(( PATINDEX('%Target1', c.ShortName) > 0 ) OR ( PATINDEX('%Target2', c.ShortName) > 0 ))");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringCountryDoesNotContain_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, DoesNotContain, "Target1,Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveCountrySelectStatement);
            result.ShouldContain("(( isnull(c.ShortName,'') not like '%Target1%'  ) AND ( isnull(c.ShortName,'') not like '%Target2%'  ))");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringCountryRange_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, Range, "Target1|Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveCountrySelectStatement);
            result.ShouldContain("( (substring(c.ShortName, 1, len('Target1')) between 'Target1' and 'Target2'))");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringCountryIsEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, IsEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveCountryLeftJoinStatement);
            result.ShouldContain("( (c.CountryID is NULL ) )");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringIGRP_NOIsEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(IgrpNo, IsEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain("( ( cast(s.[IGRP_NO] as varchar(100)) is NULL) )");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringOtherIsEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Other, IsEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain("(( ps.[OTHER] is NULL or ps.[OTHER] = ''))");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringCountryIsNotEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Country, IsNotEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain("( (c.CountryID is NOT NULL ) )");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringIGRP_NOIsNotEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(IgrpNo, IsNotEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain("( ( cast(s.[IGRP_NO] as varchar(100)) is NOT NULL) )");
        }

        [Test]
        public void getProductArchiveFilterQuery_AdhocStringOtherIsNotEmpty_CorrectQuery()
        {
            // Arrange
            var filter = GetAdhocFilter(Other, IsNotEmpty, "Target1|Target2");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain("( (ISNULL(ps.[OTHER], '') != ''  ))");
        }

        [Test]
        public void getProductArchiveFilterQuery_EColumnDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "EXP:Today|EXP:Today", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain(ArchiveSubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column} >='{DateTime.Now.ToShortDateString()}' and {Column} <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getProductArchiveFilterQuery_EColumnDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain(ArchiveSubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column} >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and {Column} <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getProductArchiveFilterQuery_EColumnDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "1/1/1900|2/2/1902", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain(ArchiveSubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column} >= '1/1/1900'  and {Column} <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getProductArchiveFilterQuery_EColumnXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "1YR", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain(ArchiveSubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column}>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getProductArchiveFilterQuery_EColumnXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "6mon", "e|column|d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain(ArchiveSubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column}>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getProductArchiveFilterQuery_EColumnXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "7", "e|column|d|qdate");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain(ArchiveSubscriptionExtensionSelectStatement);
            result.ShouldContain($"({Column}>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getProductArchiveFilterQuery_EColumnYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionYear, "2016|2017", "e|column|d|qdate");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain(ArchiveSubscriptionExtensionSelectStatement);
            result.ShouldContain($"(year({Column}) >= '2016'  and year({Column}) <= '2017' )");
        }

        [Test]
        public void getProductArchiveFilterQuery_EColumnMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionMonth, "1|6", "e|column|d|qdate");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain(ArchiveSubscriptionExtensionSelectStatement);
            result.ShouldContain($"(month({Column}) >= '1'  and month({Column}) <= '6' )");
        }

        [Test]
        public void getProductArchiveFilterQuery_DateAdhocDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "EXP:Today|EXP:Today", "d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getProductArchiveFilterQuery_DateAdhocDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]", "d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getProductArchiveFilterQuery_DateAdhocDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionDateRange, "1/1/1900|2/2/1902", "d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >= '1/1/1900'  and ps.QualificationDate <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getProductArchiveFilterQuery_DateAdhocXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "1YR", "d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getProductArchiveFilterQuery_DateAdhocXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "6mon", "d|qdate");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getProductArchiveFilterQuery_DateAdhocXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionXDays, "7", "d|qdate");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getProductArchiveFilterQuery_DateAdhocYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionYear, "2016|2017", "d|qdate");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(year(ps.QualificationDate) >= '2016'  and year(ps.QualificationDate) <= '2017' )");
        }

        [Test]
        public void getProductArchiveFilterQuery_DateAdhocMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetDateFilter(DateConditionMonth, "1|6", "d|qdate");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(month(ps.QualificationDate) >= '1'  and month(ps.QualificationDate) <= '6' )");
        }

        [Test]
        public void getProductArchiveFilterQuery_VisitActivityDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today} and  DateNumber <= {today}");
        }

        [Test]
        public void getProductArchiveFilterQuery_VisitActivityDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today - 7} and  DateNumber <= {today - 1}");
        }

        [Test]
        public void getProductArchiveFilterQuery_VisitActivityDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= 0 and  DateNumber <= {(new DateTime(1902,2,2) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_VisitActivityXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddYears(-1) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_VisitActivityXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddMonths(-6) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_VisitActivityXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddDays(-7) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_VisitActivityYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(2016,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(2017,12,31) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_VisitActivityMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(VisitActivity, VisitCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(VisitActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year,6,30) - new DateTime(1900,1,1)).TotalDays}");
        }
        
        [Test]
        public void getProductArchiveFilterQuery_OpenActivityDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today} and  DateNumber <= {today}");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenActivityDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today - 7} and  DateNumber <= {today - 1}");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenActivityDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= 0 and  DateNumber <= {(new DateTime(1902,2,2) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenActivityXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddYears(-1) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenActivityXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddMonths(-6) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenActivityXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddDays(-7) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenActivityYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(2016,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(2017,12,31) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenActivityMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenActivity, OpenCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year,6,30) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickEmailSentDateDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime >='{DateTime.Now.ToShortDateString()}' and bla.SendTime <='{DateTime.Now.ToShortDateString()} 23:59:59'");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickEmailSentDateDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and bla.SendTime <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59'");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickEmailSentDateDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain("bla.SendTime >= '1/1/1900'  and bla.SendTime <= '2/2/1902 23:59:59'");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickEmailSentDateXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddYears(-1).ToShortDateString()}'");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickEmailSentDateXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}'");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickEmailSentDateXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddDays(-7).ToShortDateString()}'");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickEmailSentDateYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain("year(bla.SendTime) >= '2016'  and year(bla.SendTime) <= '2017'");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickEmailSentDateMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickEmailSentDate, ClickCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain("month(bla.SendTime) >= '1'  and month(bla.SendTime) <= '6'");
        }
        
        [Test]
        public void getProductArchiveFilterQuery_OpenEmailSentDateDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime >='{DateTime.Now.ToShortDateString()}' and bla.SendTime <='{DateTime.Now.ToShortDateString()} 23:59:59'");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenEmailSentDateDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and bla.SendTime <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59'");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenEmailSentDateDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain("bla.SendTime >= '1/1/1900'  and bla.SendTime <= '2/2/1902 23:59:59'");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenEmailSentDateXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddYears(-1).ToShortDateString()}'");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenEmailSentDateXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}'");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenEmailSentDateXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"bla.SendTime>= '{DateTime.Now.AddDays(-7).ToShortDateString()}'");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenEmailSentDateYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain("year(bla.SendTime) >= '2016'  and year(bla.SendTime) <= '2017'");
        }

        [Test]
        public void getProductArchiveFilterQuery_OpenEmailSentDateMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(OpenEmailSentDate, OpenCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain("month(bla.SendTime) >= '1'  and month(bla.SendTime) <= '6'");
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationDateDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationDateDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate >='{DateTime.Now.AddDays(-7).ToShortDateString()}' and ps.QualificationDate <='{DateTime.Now.AddDays(-1).ToShortDateString()} 23:59:59')");
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationDateDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain("(ps.QualificationDate >= '1/1/1900'  and ps.QualificationDate <= '2/2/1902 23:59:59')");
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationDateXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddYears(-1).ToShortDateString()}')");
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationDateXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddMonths(-6).ToShortDateString()}')");
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationDateXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(ps.QualificationDate>= '{DateTime.Now.AddDays(-7).ToShortDateString()}')");
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationDateYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(year(ps.QualificationDate) >= '2016'  and year(ps.QualificationDate) <= '2017' )");
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationDateMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetQualificationDateFilter(DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveOpenActivityInsertStatement);
            result.ShouldContain($"(month(ps.QualificationDate) >= '1'  and month(ps.QualificationDate) <= '6' )");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickActivityDateRangeTodayToday_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionDateRange, "EXP:Today|EXP:Today");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today} and  DateNumber <= {today}");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickActivityDateRangeToday7Today1_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionDateRange, "EXP:Today[-7]|EXP:Today[-1]");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {today - 7} and  DateNumber <= {today - 1}");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickActivityDateRangeFromTo_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionDateRange, "1/1/1900|2/2/1902");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= 0 and  DateNumber <= {(new DateTime(1902,2,2) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickActivityXDays1Yr_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionXDays, "1YR");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddYears(-1) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickActivityXDays6mon_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionXDays, "6mon");
            
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddMonths(-6) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickActivityXDaysNumber_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionXDays, "7");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(DateTime.Now.Date.AddDays(-7) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickActivityYearRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionYear, "2016|2017");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(2016,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(2017,12,31) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test]
        public void getProductArchiveFilterQuery_ClickActivityMonthRange_CorrectQuery()
        {
            // Arrange
            var filter = GetActivityFilter(ClickActivity, ClickCriteria, DateConditionMonth, "1|6");

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain(ArchiveClickActivityInsertStatement);
            result.ShouldContain($"where  DateNumber >= {(new DateTime(DateTime.Now.Year,1,1) - new DateTime(1900,1,1)).TotalDays} and  DateNumber <= {(new DateTime(DateTime.Now.Year,6,30) - new DateTime(1900,1,1)).TotalDays}");
        }

        [Test] 
        public void getProductArchiveFilterQuery_FilterTypesOnly_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.ProductView, "Value");
            filter.Fields.Add(new Object.FilterDetails{ Name = "XACT"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "QSOURCE TYPE"});

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldContain("select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock)");
            result.ShouldContain("select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock)");
            result.ShouldContain("select CodeID from UAD_Lookup..Code with (nolock)");
        }

        [Test]
        public void getProductArchiveFilterQuery_FilterTypesAndCodes_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter("CATEGORY TYPE", ViewType.ProductView, "Value");
            filter.Fields.Add(new Object.FilterDetails{ Name = "XACT"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "QSOURCE TYPE"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "CATEGORY CODE"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "TRANSACTION CODE"});
            filter.Fields.Add(new Object.FilterDetails{ Name = "QSOURCE CODE"});

            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldNotContain("select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock)");
            result.ShouldNotContain("select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock)");
            result.ShouldNotContain("select CodeID from UAD_Lookup..Code with (nolock)");
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationYearLessThanNow_CorrectQuery()
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
                var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

                // Assert
                result.ShouldBe(ArchiveExpectedPubSubscriptionsQuery +
                                "( ps.QualificationDate between convert(varchar(20), DATEADD(year, -2016, '1/1/2018'),111) " +
                                " and  convert(varchar(20), DATEADD(year, -2016, '12/31/2018'),111) + ' 23:59:59'" +
                                " OR  ps.QualificationDate between convert(varchar(20), DATEADD(year, -2017, '1/1/2018'),111) " +
                                " and  convert(varchar(20), DATEADD(year, -2017, '12/31/2018'),111) + ' 23:59:59') and ps.pubid in (100 ) ;");
            }
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationYearGreaterThanNow_CorrectQuery()
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
                var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

                // Assert
                result.ShouldBe(ArchiveExpectedPubSubscriptionsQuery +
                                "( ps.QualificationDate between convert(varchar(20), DATEADD(year, -2016, '12/31/2017'),111) " +
                                " and  convert(varchar(20), DATEADD(year, -2016, '12/30/2018'),111) + ' 23:59:59'" +
                                " OR  ps.QualificationDate between convert(varchar(20), DATEADD(year, -2017, '12/31/2017'),111) " +
                                " and  convert(varchar(20), DATEADD(year, -2017, '12/30/2018'),111) + ' 23:59:59') and ps.pubid in (100 ) ;");
            }
        }

        [Test]
        public void getProductArchiveFilterQuery_QualificationYearProductIsEmpty_CorrectQuery()
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
                var expectedQuery = string.Format(ArchiveExpectedPubSubscriptionsQuery +
                                    "( ps.QualificationDate between convert(varchar(20), DATEADD(year, -2016, '{0}'),111) " +
                                    " and  convert(varchar(20), DATEADD(year, -2016, '{1}'),111) + ' 23:59:59'" +
                                    " OR  ps.QualificationDate between convert(varchar(20), DATEADD(year, -2017, '{0}'),111) " +
                                    " and  convert(varchar(20), DATEADD(year, -2017, '{1}'),111) + ' 23:59:59') and ps.pubid in (100 ) ;", todayText, tomorrowOfLastYearText);
                // Act
                var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

                // Assert
                result.ShouldBe(expectedQuery);
            }
        }

        [Test]
        public void getProductArchiveFilterQuery_ZipCodeRadiusMinus2_CorrectQuery()
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
                var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

                // Assert
                result.ShouldBe(ArchiveExpectedPubSubscriptionsQuery + ExectedZipCodeRadiusMinus2);
            }
        }

        [Test]
        public void getProductArchiveFilterQuery_ZipCodeRadius2_CorrectQuery()
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
                var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

                // Assert
                result.ShouldBe(ArchiveExpectedPubSubscriptionsQuery + ExectedZipCodeRadius2);
            }
        }

        [Test]
        public void getProductArchiveFilterQuery_EmailProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfileEmail, ViewType.ProductView, "1,0");
  
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedProfileQuery, "ps.Email"));
        }

        [Test]
        public void getProductArchiveFilterQuery_PhoneProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfilePhone, ViewType.ProductView, "1,0");
  
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedProfileQuery, "ps.Phone"));
        }

        [Test]
        public void getProductArchiveFilterQuery_FaxProductView_CorrectQuery()
        {
            // Arrange
            var filter = GetProfileFilter(ProfileFax, ViewType.ProductView, "1,0");
  
            // Act
            var result = FilterMVC.getProductArchiveFilterQuery(filter, string.Empty, string.Empty, 0, null);

            // Assert
            result.ShouldBe(string.Format(ArchiveExpectedProfileQuery, "ps.fax"));
        }
    }
}
