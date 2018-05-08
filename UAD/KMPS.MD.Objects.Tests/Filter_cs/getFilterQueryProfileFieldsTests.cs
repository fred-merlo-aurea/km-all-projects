using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.MD.Objects.Tests.Filter_cs
{
    [TestFixture]
    public class getFilterQueryProfileFieldsTests
    {
        const string Separator = ",";
        private const int NowDateMonth = 6;
        private const string profileFieldName = "QUALIFICATION YEAR";
        private const int CodeId = 10;
        private IDisposable _shimsContext;
        private string _pubsYearStardDate;
        private string _pubsYearEndDate;
        private DateTime _now;
        private string _codeName;

        [SetUp]
        public void Setup()
        {
            _pubsYearStardDate = null;
            _pubsYearEndDate = null;
            _now = new DateTime(DateTime.Now.Year, NowDateMonth, 1);
            _codeName = GetUniqueString();
            _shimsContext = ShimsContext.Create();
            ShimPubs.GetByIDClientConnectionsInt32 = ShimPubsGetById;
            ShimDateTime.NowGet = ShimDateTimeNowGet;
            ShimCode.GetDataCompareType = ShimCodeGetDataCompareType;
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void getFilterQuery_CirculationTypeProfileFiledSimpleQueries_CorrectQuery()
        {
            //Arrange
            var fieldNamesAndValuesAndExpected = new Dictionary<string, Func<string, string>>
            {
                { "CATEGORY TYPE" , value => $"ps.PubCategoryID in (select CategoryCodeID from "+
                    $"UAD_Lookup..CategoryCode with (nolock) where CategoryCodeTypeID in ( {value} ) )"},
                { "CATEGORY CODE", value => $"ps.PubCategoryID in ({value}) " },
                { "XACT", value=> $"ps.PubTransactionID in (select TransactionCodeID from "+
                    $"UAD_Lookup..TransactionCode with (nolock) where TransactionCodeTypeID in ( {value } ) )" },
                { "TRANSACTION CODE", value=> $"ps.PubTransactionID in (  {value}) " },
                { "QSOURCE TYPE", value=> $"ps.PubQSourceID in (select CodeID from "+
                    $"UAD_Lookup..Code with (nolock) where ParentCodeId in ( {value} ) )" },
                { "QSOURCE CODE", value=> $"ps.PubQSourceID in ({value}) " },
                { "MEDIA", value=> $"ps.Demo7 in ('{value}') " },
                { "QFROM", value=> $" ps.QualificationDate >= '{value}' "},
                { "QTO", value=> $" ps.QualificationDate <= '{value} 23:59:59' "},
                { "WAVE MAILING", value=> $" ISNULL(ps.IsInActiveWaveMailing,0) = {value}"}
            };
            foreach (var field in fieldNamesAndValuesAndExpected)
            {
                var value = GetUniqueString();
                var filter = GetCirculationProfileFilter(field.Key, value);
                var expected = field.Value(value);

                //Act
                var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

                //Assert
                result.ShouldContain(expected);
            }
        }

        [Test]
        public void getFilterQuery_CirculationTypeProfileFiledQualificationYearWithoutStartEndDate_CorrectQuery()
        {
            //Arrange
            var years = new[] { 2010, 2011, 2012 };
            var value = string.Join(Separator, years.Select(year => year.ToString()));
            var filter = GetCirculationProfileFilter(profileFieldName, value);
            var startDate = _now;
            var endDate = startDate.AddYears(-1).AddDays(+1);
            var expectedList = years
                .Select(year => QualificatinYearExpectedQuery(year, startDate, endDate))
                .ToList();

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var expected in expectedList)
            {
                result.ShouldContain(expected);
            }
        }

        [Test]
        public void getFilterQuery_CirculationTypeProfileFiledQualificationYearWithStartDateLowerNow_CorrectQuery()
        {
            //Arrange
            var years = new[] { 2010, 2011, 2012 };
            var value = string.Join(Separator, years.Select(year => year.ToString()));
            var filter = GetCirculationProfileFilter(profileFieldName, value);
            var currentYear = _now.Year;
            _pubsYearStardDate = "5/1";
            _pubsYearEndDate = "10/1";
            var startDate = Convert.ToDateTime(_pubsYearStardDate + "/" + currentYear, CultureInfo.InvariantCulture);
            var endDate = Convert.ToDateTime(_pubsYearEndDate + "/" + (currentYear + 1), CultureInfo.InvariantCulture);
            var expectedList = years
                .Select(year => QualificatinYearExpectedQuery(year, startDate, endDate))
                .ToList();

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var expected in expectedList)
            {
                result.ShouldContain(expected);
            }
        }

        [Test]
        public void getFilterQuery_CirculationTypeProfileFiledQualificationYearWithStartDateAboveNow_CorrectQuery()
        {
            //Arrange
            var years = new[] { 2010, 2011, 2012 };
            var value = string.Join(Separator, years.Select(year => year.ToString()));
            var filter = GetCirculationProfileFilter(profileFieldName, value);
            var currentYear = _now.Year - 1;
            _pubsYearStardDate = "7/1";
            _pubsYearEndDate = "10/1";
            var startDate = Convert.ToDateTime(_pubsYearStardDate + "/" + currentYear, CultureInfo.InvariantCulture);
            var endDate = Convert.ToDateTime(_pubsYearEndDate + "/" + (currentYear + 1), CultureInfo.InvariantCulture);
            var expectedList = years
                .Select(year => QualificatinYearExpectedQuery(year, startDate, endDate))
                .ToList();

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var expected in expectedList)
            {
                result.ShouldContain(expected);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledBrand_CorrectQuery()
        {
            //Arrange
            const string fieldName = "BRAND";
            var value = GetUniqueString();
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.None;
            const string expectedQuery = " join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  " +
                "join brand b with (nolock) on b.brandID = bd.brandID ";
            var expectedWhere = $"b.IsDeleted = 0 and bd.BrandID = {value}";

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            result.ShouldContain(expectedQuery);
            result.ShouldContain(expectedWhere);
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledProduct_CorrectQuery()
        {
            //Arrange
            const string fieldName = "PRODUCT";
            var value = GetUniqueString();
            var expectedWhere = $"ps.pubid in ({value} ) ";
            var expectedPIDOpenCondition = $"pso.pubID in ({value})";
            var expPIDClickCondition = $"psc.pubID in ({value})";
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            result.ShouldContain(expectedWhere);
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledDataCompare_CorrectQuery()
        {
            //Arrange
            const string fieldName = "DATACOMPARE";
            var dataCompare = GetUniqueString();
            var value = $"{dataCompare}|{CodeId}";
            var expectedQuery = " left outer join DataCompareProfile d with(nolock) on s.IGRP_NO = " +
                $"d.IGrp_No and  d.ProcessCode = '{dataCompare}'";
            const string expectedWhere = "d.SubscriberFinalId is null ";
            var filter = GetNoneCirculationProfileFilter(fieldName, value);

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            result.ShouldContain(expectedQuery);
            result.ShouldContain(expectedWhere);
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledDataCompareAndCodeNameMatch_CorrectQuery()
        {
            //Arrange
            const string codeName = "Match";
            _codeName = codeName;
            const string fieldName = "DATACOMPARE";
            var dataCompare = GetUniqueString();
            var value = $"{dataCompare}|{CodeId}";
            const string expectedQuery = " join DataCompareProfile d with(nolock) on s.IGRP_NO = d.IGrp_No ";
            var expectedWhere = $"d.ProcessCode = '{dataCompare}'";
            var filter = GetNoneCirculationProfileFilter(fieldName, value);

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            result.ShouldContain(expectedQuery);
            result.ShouldContain(expectedWhere);
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledStateWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "STATE";
            var value = GetUniqueString();
            var expectedWhereCondition = $"ps.RegionCode in ('{value}') ";
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            result.ShouldContain(expectedWhereCondition);
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledState_CorrectQuery()
        {
            //Arrange
            const string fieldName = "STATE";
            var value = GetUniqueString();
            var expectedWhereCondition = $"s.State in ('{value}') ";
            var filter = GetNoneCirculationProfileFilter(fieldName, value);

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            result.ShouldContain(expectedWhereCondition);
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledCountry_CorrectQuery()
        {
            //Arrange
            const string fieldName = "COUNTRY";
            var countriesAndCondition = new Dictionary<string, string>
            {
                { "1", "s.CountryID = 1" },
                { "3", "((s.CountryID = 1) or (s.CountryID = 2)) " },
                { "4", "not ((s.CountryID = 1) or (s.CountryID = 2) or ISNULL(s.CountryID,0) = 0)" },
                {"5", "s.CountryID in ( 5 ) " }
            };
            var value = string.Join(Separator, countriesAndCondition.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in countriesAndCondition.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledCountryWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "COUNTRY";
            var countriesAndCondition = new Dictionary<string, string>
            {
                { "1", "ps.CountryID = 1" },
                { "3", "((ps.CountryID = 1) or (ps.CountryID = 2)) " },
                { "4", "not ((ps.CountryID = 1) or (ps.CountryID = 2) or ISNULL(ps.CountryID,0) = 0)" },
                { "5", "ps.CountryID in ( 5 ) " }
            };
            var value = string.Join(Separator, countriesAndCondition.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var country in countriesAndCondition.Values)
            {
                result.ShouldContain(country);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledEmail_CorrectQuery()
        {
            //Arrange
            const string fieldName = "EMAIL";
            var emailAndCondition = new Dictionary<string, string>
            {
                { "1", "s.emailexists = 1" },
                { "2", "s.emailexists = 2" }
            };
            var value = string.Join(Separator, emailAndCondition.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in emailAndCondition.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledEmailWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "EMAIL";
            var emailAndCondition = new Dictionary<string, string>
            {
                { "1", "isnull(ps.Email, '') != '' " },
                { "2", "isnull(ps.Email, '') = '' " }
            };
            var value = string.Join(Separator, emailAndCondition.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in emailAndCondition.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledPhone_CorrectQuery()
        {
            //Arrange
            const string fieldName = "PHONE";
            var phonesAndCondition = new Dictionary<string, string>
            {
                { "1", "s.phoneexists = 1" },
                { "2", "s.phoneexists = 2" },
            };
            var value = string.Join(Separator, phonesAndCondition.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in phonesAndCondition.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledPhoneWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "PHONE";
            var phonesAndCondition = new Dictionary<string, string>
            {
                { "1", "isnull(ps.Phone, '') != '' " },
                { "2", "isnull(ps.Phone, '') = '' " },
            };
            var value = string.Join(Separator, phonesAndCondition.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in phonesAndCondition.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledFax_CorrectQuery()
        {
            //Arrange
            const string fieldName = "FAX";
            var phonesAndCondition = new Dictionary<string, string>
            {
                { "1", "s.faxexists = 1" },
                { "2", "s.faxexists = 2" },
            };
            var value = string.Join(Separator, phonesAndCondition.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in phonesAndCondition.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledFaxWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "FAX";
            var phonesAndCondition = new Dictionary<string, string>
            {
                { "1", "isnull(ps.fax, '') != '' " },
                { "2", "isnull(ps.fax, '') = '' " },
            };
            var value = string.Join(Separator, phonesAndCondition.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in phonesAndCondition.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledGeolocated_CorrectQuery()
        {
            //Arrange
            const string fieldName = "GEOLOCATED";
            const int itemsCount = 5;
            var itemsAndConditions = Enumerable.Range(1, itemsCount)
                .ToDictionary(i => i.ToString(), i => $"s.IsLatLonValid = {i}");
            var value = string.Join(Separator, itemsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in itemsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledMailPermisstion_CorrectQuery()
        {
            //Arrange
            const string fieldName = "MAILPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "s.MailPermission is null"},
                { "2", "s.MailPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledMailPermisstionWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "MAILPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "ps.MailPermission is null"},
                { "2", "ps.MailPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledFaxPermissionWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "FAXPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "ps.FaxPermission is null"},
                { "2", "ps.FaxPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledFaxPermission_CorrectQuery()
        {
            //Arrange
            const string fieldName = "FAXPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "s.FaxPermission is null"},
                { "2", "s.FaxPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledPhonePermissionWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "PHONEPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "ps.PhonePermission is null"},
                { "2", "ps.PhonePermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledPhonePermission_CorrectQuery()
        {
            //Arrange
            const string fieldName = "PHONEPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "s.PhonePermission is null"},
                { "2", "s.PhonePermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledOtherProductPermissionWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "OTHERPRODUCTSPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "ps.OtherProductsPermission is null"},
                { "2", "ps.OtherProductsPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledOtherProductPermission_CorrectQuery()
        {
            //Arrange
            const string fieldName = "OTHERPRODUCTSPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "s.OtherProductsPermission is null"},
                { "2", "s.OtherProductsPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledThirPartyPermissionWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "THIRDPARTYPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "ps.ThirdPartyPermission is null"},
                { "2", "ps.ThirdPartyPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledThirdPartyPermission_CorrectQuery()
        {
            //Arrange
            const string fieldName = "THIRDPARTYPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "s.ThirdPartyPermission is null"},
                { "2", "s.ThirdPartyPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledEmailRenewWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "EMAILRENEWPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "ps.EmailRenewPermission is null"},
                { "2", "ps.EmailRenewPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;

            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledEmailRenewPermission_CorrectQuery()
        {
            //Arrange
            const string fieldName = "EMAILRENEWPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "s.EmailRenewPermission is null"},
                { "2", "s.EmailRenewPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;
            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledTextWithProductView_CorrectQuery()
        {
            //Arrange
            const string fieldName = "TEXTPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "ps.TextPermission is null"},
                { "2", "ps.TextPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;
            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiledTextPermission_CorrectQuery()
        {
            //Arrange
            const string fieldName = "TEXTPERMISSION";
            var permissionsAndConditions = new Dictionary<string, string>
            {
                { "-1", "s.TextPermission is null"},
                { "2", "s.TextPermission = 2" }
            };
            var value = string.Join(Separator, permissionsAndConditions.Keys);
            var filter = GetNoneCirculationProfileFilter(fieldName, value);
            filter.ViewType = Enums.ViewType.ProductView;
            //Act
            var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

            //Assert
            foreach (var condition in permissionsAndConditions.Values)
            {
                result.ShouldContain(condition);
            }
        }

        [Test]
        public void getFilterQuery_NoneCirculationTypeProfileFiled_CorrectQuery()
        {
            //Arrange
            Func<string, string> formatValue =
                value => Utilities.ReplaceSingleQuotes(value).Replace("_", "[_]");
            var fieldsAndConditions = new Dictionary<string, Func<string, string>>
            {
                {
                    "MEDIA",
                    value => $"ps.Demo7 in (  '{value.Replace(",", "','")}') "
                },
                {
                    "QFROM",
                    value => $" ps.QualificationDate >= '{value}' "
                },
                {
                    "QTO",
                    value => $" ps.QualificationDate <= '{value} 23:59:59' "
                },
                {
                    "EMAIL STATUS",
                    value => $"ps.EmailStatusID in ({value}) "
                },
                {
                    "LAST NAME",
                    value => $"PATINDEX('{formatValue(value)}%', s.lname ) > 0 "
                } ,
                {
                    "FIRST NAME",
                    value => $"PATINDEX('{formatValue(value)}%', s.fname ) > 0 "
                },
                {
                    "COMPANY",
                    value => $"PATINDEX('{formatValue(value)}%', s.company ) > 0 "
                } ,
                {
                    "PHONENO",
                    value => $"PATINDEX('{formatValue(value)}%', s.phone ) > 0 "
                },
                {
                    "EMAILID",
                    value => $"PATINDEX('{formatValue(value)}%', ps.email ) > 0 "
                }
            };
            foreach (var fieldNameAndCondition in fieldsAndConditions)
            {
                var value = $"{GetUniqueString()},_ '{GetUniqueString()}'";
                var filter = GetNoneCirculationProfileFilter(fieldNameAndCondition.Key, value);
                var expectedCondition = fieldNameAndCondition.Value(value);

                //Act
                var result = Filter.getFilterQuery(filter, string.Empty, string.Empty);

                //Assert
                result.ShouldContain(expectedCondition);
            }
        }

        private Pubs ShimPubsGetById(ClientConnections clientConnections, int id)
        {
            return new Pubs
            {
                YearStartDate = _pubsYearStardDate,
                YearEndDate = _pubsYearEndDate
            };
        }

        private DateTime ShimDateTimeNowGet()
        {
            return _now;
        }

        private Filter GetCirculationProfileFilter(string fieldName, string value)
        {
            const string productFieldName = "PRODUCT";
            const string productFieldValue = "123";

            return new Filter
            {
                Fields = new List<Field>
                {
                    new Field
                    {
                        FilterType = Enums.FiltersType.Circulation,
                        Name = fieldName,
                        Values = value
                    },
                    new Field
                    {
                        FilterType = Enums.FiltersType.Circulation,
                        Name = productFieldName,
                        Values = productFieldValue
                    }
                }
            };
        }

        private Filter GetNoneCirculationProfileFilter(string fieldName, string value)
        {
            return new Filter
            {
                Fields = new List<Field>
                {
                    new Field
                    {
                        FilterType = Enums.FiltersType.Standard,
                        Name = fieldName,
                        Values = value
                    }
                }
            };
        }

        private string QualificatinYearExpectedQuery(int year, DateTime startDate, DateTime endDate)
        {
            {
                return $" ps.QualificationDate between convert(varchar(20), DATEADD(year, -{(year - 1)}" +
                $", '{startDate.ToShortDateString()}'),111)  and  convert(varchar(20), DATEADD(year, -" +
                $"{(year - 1)}, '{endDate.ToShortDateString()}'),111) + ' 23:59:59'";
            }
        }

        private string GetUniqueString()
        {
            return Guid.NewGuid().ToString();
        }

        private List<Code> ShimCodeGetDataCompareType()
        {
            return new List<Code>
            {
                new Code
                {
                    CodeID = CodeId,
                    CodeName = _codeName,
                }
            };
        }
    }
}