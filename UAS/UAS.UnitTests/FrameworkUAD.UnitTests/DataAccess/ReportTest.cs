using System;
using System.Data.SqlClient;

using FrameworkUAD.Object;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

using UADReport = FrameworkUAD.DataAccess.Report;

namespace FrameworkUAD.UnitTests.DataAccess
{
    [TestFixture]
    public partial class ReportTest
    {
        private const string PrintColumnsSample = "Column01";
        private const int SampleIssueId = 19;
        private const int ReportIdSample = 29;
        private const string RowIdSample = "A119";
        private const string SampleFilterQuery = "filter-query";
        private const int CountryIdSample = 41;
        private const int SampleProductId = 771;
        private const string SampleRow = "Row 11";
        private const string SampleCol = "Col 73";
        private const string SampleDemo = "DemoABC";

        private PrivateType _reportType;

        private static readonly Reporting Reporting = new Reporting
        {
            PublicationIDs = "r.PublicationIDs",
            CategoryIDs = "r.CategoryIDs",
            TransactionIDs = "r.TransactionIDs",
            QSourceIDs = "r.QSourceIDs",
            StateIDs = "r.StateIDs",
            Regions = "r.Regions",
            CountryIDs = "r.CountryIDs",
            Email = "r.Email",
            Fax = "r.Fax",
            Phone = "r.Phone",
            Mobile = "r.Mobile",
            FromDate = "r.FromDate",
            ToDate = "r.ToDate",
            Year = "r.Year",
            Media = "r.Media",
            CategoryCodes = "r.CategoryCodes",
            TransactionCodes = "r.TransactionCodes",
            ResponseIDs = "r.ResponseIDs",
            UADResponseIDs = "r.UADResponseIDs",
            AdHocXML = "r.AdHocXML",
            Demo31 = "r.Demo31",
            Demo32 = "r.Demo32",
            Demo33 = "r.Demo33",
            Demo34 = "r.Demo34",
            Demo35 = "r.Demo35",
            Demo36 = "r.Demo36",
            IsMailable = "r.IsMailable",
            EmailStatusIDs = "r.EmailStatusIDs",
            OpenSearchType = "r.OpenSearchType",
            OpenCount = "r.OpenCount",
            OpenDateFrom = "r.OpenDateFrom",
            OpenDateTo = "r.OpenDateTo",
            OpenBlastID = "r.OpenBlastID",
            OpenEmailSubject = "r.OpenEmailSubject",
            OpenEmailFromDate = "r.OpenEmailFromDate",
            OpenEmailToDate = "r.OpenEmailToDate",
            ClickSearchType = "r.ClickSearchType",
            ClickCount = "r.ClickCount",
            ClickURL = "r.ClickURL",
            ClickDateFrom = "r.ClickDateFrom",
            ClickDateTo = "r.ClickDateTo",
            ClickBlastID = "r.ClickBlastID",
            ClickEmailSubject = "r.ClickEmailSubject",
            ClickEmailFromDate = "r.ClickEmailFromDate",
            ClickEmailToDate = "r.ClickEmailToDate",
            Domain = "r.Domain",
            VisitsURL = "r.VisitsURL",
            VisitsDateFrom = "r.VisitsDateFrom",
            VisitsDateTo = "r.VisitsDateTo",
            BrandID = "r.BrandID",
            SearchType = "r.SearchType",
            RangeMaxLatMin = "r.RangeMaxLatMin",
            RangeMaxLatMax = "r.RangeMaxLatMax",
            RangeMaxLonMin = "r.RangeMaxLonMin",
            RangeMaxLonMax = "r.RangeMaxLonMax",
            RangeMinLatMin = "r.RangeMinLatMin",
            RangeMinLatMax = "r.RangeMinLatMax",
            RangeMinLonMin = "r.RangeMinLonMin",
            RangeMinLonMax = "r.RangeMinLonMax",
            WaveMail = "r.WaveMail"
        };

        private IDisposable _shims;
        private SqlConnection _sqlConnection;
        private SqlCommand _calledSqlCommand;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            _reportType = new PrivateType(typeof(UADReport));
            _sqlConnection = new SqlConnection();
        }

        [TearDown]
        public void Teardown()
        {
            _calledSqlCommand?.Dispose();
            _sqlConnection?.Dispose();
            _shims?.Dispose();
        }

        private static void AssertCommonFields(SqlParameterCollection parameters)
        {
            parameters.ShouldSatisfyAllConditions(
                () => parameters[UADReport.ParamCategoryIDs].Value.ShouldBe(Reporting.CategoryIDs),
                () => parameters[UADReport.ParamTransactionIDs].Value.ShouldBe(Reporting.TransactionIDs),
                () => parameters[UADReport.ParamQsourceIDs].Value.ShouldBe(Reporting.QSourceIDs),
                () => parameters[UADReport.ParamStateIds].Value.ShouldBe(Reporting.StateIDs),
                () => parameters[UADReport.ParamCountryIDs].Value.ShouldBe(Reporting.CountryIDs),
                () => parameters[UADReport.ParamEmail].Value.ShouldBe(Reporting.Email),
                () => parameters[UADReport.ParamPhone].Value.ShouldBe(Reporting.Phone),
                () => parameters[UADReport.ParamFax].Value.ShouldBe(Reporting.Fax),
                () => parameters[UADReport.ParamResponseIds].Value.ShouldBe(Reporting.ResponseIDs),
                () => parameters[UADReport.ParamDemo7].Value.ShouldBe(Reporting.Media),
                () => parameters[UADReport.ParamAdHocXml].Value.ShouldBe(Reporting.AdHocXML));
        }
    }
}
