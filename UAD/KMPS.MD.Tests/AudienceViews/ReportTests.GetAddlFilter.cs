using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using KMPlatform.Object.Fakes;
using static KMPS.MD.Objects.Enums;
using ObjShimKmps = KMPS.MD.Objects.Fakes;
using ShimReport = KMPS.MD.Main.Fakes.ShimReport;
using ObjShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;

namespace KMPS.MD.Tests.AudienceViews
{
    /// <summary>
    /// Unit test for <see cref="Report"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReportTestsGetAddlFilter : BasePageTests
    {
        private const string GetAddlFilter = "GetAddlFilter";
        private const string LabelPermission = "lblPermission";
        private const string LabelMasterId = "lblMasterID";
        private const string LabelPremissionType = "lblPremissionType";
        private const string HiddenFiledRow = "hfRow";
        private const string HiddenFiledColumn = "hfColumn";
        private const string HiddenSelectedCrossTabLink = "hfSelectedCrossTabLink";
        private const string HiddenFiledRowSearchValue = "hfRowSearchValue";
        private const string DropDownCrossTabReport = "drpCrossTabReport";
        private const string HiddenFieldColumnSearchValue = "hfColumnSearchValue";
        private const string HiddenFiledBrandId = "hfBrandID";
        private const string FiltersObject = "fc";
        private const string FilterName = "test";
        private const string SelectedFilterNo = "1";
        private const string Permission = "Permission";
        private const string CrossTab = "CrossTab";
        private const string Mail = "MAIL";
        private const string Email = "EMAIL";
        private const string Phone = "PHONE";
        private const string MailPhone = "MAIL_PHONE";
        private const string EmailPhone = "EMAIL_PHONE";
        private const string MailEmail = "MAIL_EMAIL";
        private const string AllRecords = "ALL_RECORDS";
        private const string MailResultMessage = " and MailPermission=1";
        private const string EmailResultMessage = " AND OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1";
        private const string PhoneResultMessage = " and PhonePermission=1 and PhoneExists=1";
        private const string MailPhoneResultMessage = " and MailPermission=1 and PhonePermission=1 and PhoneExists=1";
        private const string EmailPhoneResultMessage = " and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1 and PhoneExists=1";
        private const string MailEmailResultPhone = " and MailPermission = 1 and OtherProductsPermission=1 and EmailRenewPermission=1";
        private const string AllRecordsResultMessage = " and MailPermission=1 and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1 and PhoneExists=1";
        private const string DefaultStringMessage = "mastercodesheet";
        private const int TestZero = 0;
        private const int TestOne = 1;
        private const string TestZeroString = "0";
        private const string TestMinusOneString = "-1";
        private const string TestMinusTwoString = "-2";
        private const string DefaultEmptyStringWithSpaceMessage = " ";
        private const string DefaultCase = "DefaultCase";
        private const string Zip = "Zip";
        private const string State = "State";
        private const string Country = "Country";
        private const string Title = "Title";
        private const string TestSearchValue = "Test|Test";
        private const string GrandTotal = "Grand Total|Grand Total";
        private const string NoResponse = "ZZZ. NO RESPONSE|ZZZ. NO RESPONSE";
        private const string CountrySearchValue = "Country|Country";
        private const string StateSearchValue = "State|State";
        private const string EmptyString = "";
        private const string SearchValue = "A_B,B_C,C_D";
        private const string RowSearchValue1 = "A|B";
        private const string ZipGrandTotalResultMessage = " union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock)";
        private const string StateGrandTotalResultMessage = "union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
        private const string CountryGrandTotalResultMessage = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID";
        private const string TitleGrandTotalResultMessageWithEmptyVaule = " CROSS APPLY (SELECT TOP 100 Title, SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID GROUP BY Title , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
        private const string TitleGrandTotalResultMessageWithValue = "like '%";
        private const string ZipNoResponseSearchValueMessageVale = " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock)";
        private const string StateNoResponseSearchValueMessageVale = " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
        private const string CountryNoResponseSearchValueMessageVale = " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID)";
        private const string TitleNoResponseSearchValueWithDefaultEmptyStringMessageVale = "CROSS APPLY (SELECT TOP 100 ";
        private const string TitleNoResponseSearchValueMessageVale = "CROSS APPLY (SELECT TOP 100 ";
        private const string CountrySearchValueMessageValue = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName";
        private const string StateSearchValueWithCommaAndUnderScoreMessageValue = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) where ss.";
        private const string DefaultCaseRecencyViewWithEmptyStringMessageValue = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentBrandConsensus vrbc2 with (nolock) on ss.subscriptionid = vrbc2.subscriptionid and vrbc2.BrandID =";
        private const string DefaultCaseRecencyViewWithSearchvalueMessageValue = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentBrandConsensus vrbc2 with (nolock) on ss.subscriptionid = vrbc2.subscriptionid and vrbc2.BrandID =";
        private const string DefaultCaseRecencyViewWithRowSearchValueMessage = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join ";
        private const string DefaultCaseGrandTotalMessageValue = "join (select distinct vrbc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentBrandConsensus";
        private const string DefaultCaseGrandTotalRowSearchMessage = " join (select distinct vrc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentConsensus vrc2 on ss.SubscriptionID  = vrc2.SubscriptionID join mastercodesheet m ON m.masterid = vrc2.masterid where m.MasterGroupID";
        private const string DefaultCaseGrandTotalRecordDetailsRowSearchMessage = "join (select distinct sd2.subscriptionID from SubscriptionDetails sd2 WITH (nolock) join mastercodesheet";
        private const string DefaultCaseTestSearchValueMessage = "join (select distinct sd2.subscriptionID from SubscriptionDetails sd2 WITH (nolock) join mastercode";
        private const string DefaultCaseTestSearchValueIsOneMessage = " join (select distinct v2.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 on ss.SubscriptionID  = v2.SubscriptionID join mastercodesheet m ON m.masterid = v2.masterid WHERE v2.BrandID = ";
        private const string DefaultMessageRecordView = "join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_Bran";
        private const string DefaultMessageRecordView1 = "join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in";
        private const string DefaultMessageRecordView2 = "join (select distinct v2.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus ";
        private const string DefaultMessageRecordView3 = "join (select distinct vrc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentConsensus vrc2 ";

        private Report _report;
        private Filters _filters;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _report = new Report();
            InitializePage(_report);
            ReflectionHelper.SetValue(
               _report,
               FiltersObject,
               new Filters(new ShimClientConnections(), 1)
               {
                   FilterComboList = new List<FilterCombo>
                   {
                        new FilterCombo {SelectedFilterNo = SelectedFilterNo}
                   }
               });
            _filters = new Filters(new ShimClientConnections(), 1);
            _filters.FilterComboList = new List<FilterCombo>
            {
                new FilterCombo {SelectedFilterNo =SelectedFilterNo}
            };

            _filters.Add(new Filter
            {
                FilterNo = 1,
                FilterName = FilterName
            });
            ObjShimKmps.ShimMDFilter.LoadFiltersClientConnectionsInt32Int32 = (_, __, ___) => _filters;
            ObjShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();
        }

        [TestCase(Permission, TestZeroString, Mail, ViewType.RecencyView, TestOne, MailResultMessage)]
        [TestCase(Permission, TestZeroString, Email, ViewType.RecencyView, TestZero, EmailResultMessage)]
        [TestCase(Permission, TestZeroString, Phone, ViewType.RecordDetails, TestOne, PhoneResultMessage)]
        [TestCase(Permission, TestZeroString, MailPhone, ViewType.RecordDetails, TestZero, MailPhoneResultMessage)]
        [TestCase(Permission, TestMinusOneString, EmailPhone, ViewType.RecencyView, TestOne, EmailPhoneResultMessage)]
        [TestCase(Permission, TestMinusOneString, MailEmail, ViewType.RecencyView, TestZero, MailEmailResultPhone)]
        [TestCase(Permission, TestMinusOneString, AllRecords, ViewType.RecordDetails, TestOne, AllRecordsResultMessage)]
        [TestCase(Permission, TestMinusOneString, DefaultEmptyStringWithSpaceMessage, ViewType.RecordDetails, TestZero, DefaultStringMessage)]
        [TestCase(Permission, TestMinusTwoString, Email, ViewType.RecencyView, TestOne, EmailResultMessage)]
        [TestCase(Permission, TestMinusTwoString, Email, ViewType.RecencyView, TestZero, EmailResultMessage)]
        [TestCase(Permission, TestMinusTwoString, Email, ViewType.RecordDetails, TestOne, EmailResultMessage)]
        [TestCase(Permission, TestMinusTwoString, Email, ViewType.RecordDetails, TestZero, EmailResultMessage)]
        public void GetAddlFilter_PermissionTabIsEnabled_ReturnsResultObject(string permission,
            string masterId,
            string premissionType,
            ViewType viewType,
            int brandId,
            string resultMessage)
        {
            // Arrange
            var lblPermission = GetField<Label>(LabelPermission);
            lblPermission.Text = permission;
            var lblMasterID = GetField<Label>(LabelMasterId);
            lblMasterID.Text = masterId;
            var lblPremissionType = GetField<Label>(LabelPremissionType);
            lblPremissionType.Text = premissionType;
            var hfBrandID = GetField<HiddenField>(HiddenFiledBrandId);
            hfBrandID.Value = brandId.ToString();
            CreatePageShimObject();
            ShimAudienceViewBase.AllInstances.ViewTypeGet = (x) => { return viewType; };

            // Act
            var result = PrivatePage.Invoke(GetAddlFilter) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(resultMessage)
            );
        }

        [TestCase(CrossTab, Zip, GrandTotal, RowSearchValue1, TestZero, ViewType.RecencyView, ZipGrandTotalResultMessage)]
        [TestCase(CrossTab, State, GrandTotal, EmptyString, TestZero, ViewType.RecencyView, StateGrandTotalResultMessage)]
        [TestCase(CrossTab, Country, GrandTotal, EmptyString, TestZero, ViewType.RecencyView, CountryGrandTotalResultMessage)]
        [TestCase(CrossTab, Title, GrandTotal, EmptyString, TestZero, ViewType.RecencyView, TitleGrandTotalResultMessageWithEmptyVaule)]
        [TestCase(CrossTab, Title, GrandTotal, SearchValue, TestZero, ViewType.RecencyView, TitleGrandTotalResultMessageWithValue)]
        [TestCase(CrossTab, Zip, NoResponse, RowSearchValue1, TestZero, ViewType.RecencyView, ZipNoResponseSearchValueMessageVale)]
        [TestCase(CrossTab, State, NoResponse, EmptyString, TestZero, ViewType.RecencyView, StateNoResponseSearchValueMessageVale)]
        [TestCase(CrossTab, Country, NoResponse, EmptyString, TestZero, ViewType.RecencyView, CountryNoResponseSearchValueMessageVale)]
        [TestCase(CrossTab, Title, NoResponse, EmptyString, TestZero, ViewType.RecencyView, TitleNoResponseSearchValueWithDefaultEmptyStringMessageVale)]
        [TestCase(CrossTab, Title, NoResponse, SearchValue, TestZero, ViewType.RecencyView, TitleNoResponseSearchValueMessageVale)]
        [TestCase(CrossTab, Country, CountrySearchValue, TestSearchValue, TestZero, ViewType.RecencyView, CountrySearchValueMessageValue)]
        [TestCase(CrossTab, State, StateSearchValue, TestSearchValue, TestZero, ViewType.RecencyView, StateSearchValueWithCommaAndUnderScoreMessageValue)]
        [TestCase(CrossTab, DefaultCase, NoResponse, EmptyString, TestOne, ViewType.RecencyView, DefaultCaseRecencyViewWithEmptyStringMessageValue)]
        [TestCase(CrossTab, DefaultCase, NoResponse, SearchValue, TestOne, ViewType.RecencyView, DefaultCaseRecencyViewWithSearchvalueMessageValue)]
        [TestCase(CrossTab, DefaultCase, NoResponse, EmptyString, TestZero, ViewType.RecencyView, DefaultCaseRecencyViewWithRowSearchValueMessage)]
        [TestCase(CrossTab, DefaultCase, NoResponse, SearchValue, TestZero, ViewType.RecencyView, DefaultCaseRecencyViewWithRowSearchValueMessage)]
        [TestCase(CrossTab, DefaultCase, NoResponse, SearchValue, TestZero, ViewType.RecencyView, DefaultCaseRecencyViewWithRowSearchValueMessage)]
        [TestCase(CrossTab, DefaultCase, NoResponse, EmptyString, TestOne, ViewType.RecordDetails, DefaultMessageRecordView)]
        [TestCase(CrossTab, DefaultCase, NoResponse, SearchValue, TestOne, ViewType.RecordDetails, DefaultMessageRecordView)]
        [TestCase(CrossTab, DefaultCase, NoResponse, EmptyString, TestZero, ViewType.RecordDetails, DefaultMessageRecordView1)]
        [TestCase(CrossTab, DefaultCase, NoResponse, SearchValue, TestZero, ViewType.RecordDetails, DefaultMessageRecordView1)]
        [TestCase(CrossTab, DefaultCase, GrandTotal, EmptyString, TestOne, ViewType.RecencyView, DefaultCaseGrandTotalMessageValue)]
        [TestCase(CrossTab, DefaultCase, GrandTotal, SearchValue, TestOne, ViewType.RecencyView, DefaultCaseGrandTotalMessageValue)]
        [TestCase(CrossTab, DefaultCase, GrandTotal, EmptyString, TestZero, ViewType.RecencyView, DefaultCaseGrandTotalRowSearchMessage)]
        [TestCase(CrossTab, DefaultCase, GrandTotal, SearchValue, TestZero, ViewType.RecencyView, DefaultCaseGrandTotalRowSearchMessage)]
        [TestCase(CrossTab, DefaultCase, GrandTotal, EmptyString, TestOne, ViewType.RecordDetails, DefaultMessageRecordView2)]
        [TestCase(CrossTab, DefaultCase, GrandTotal, SearchValue, TestOne, ViewType.RecordDetails, DefaultMessageRecordView2)]
        [TestCase(CrossTab, DefaultCase, GrandTotal, EmptyString, TestZero, ViewType.RecordDetails, DefaultCaseGrandTotalRecordDetailsRowSearchMessage)]
        [TestCase(CrossTab, DefaultCase, GrandTotal, SearchValue, TestZero, ViewType.RecordDetails, DefaultCaseGrandTotalRecordDetailsRowSearchMessage)]
        [TestCase(CrossTab, DefaultCase, TestSearchValue, TestSearchValue, TestZero, ViewType.RecencyView, DefaultMessageRecordView3)]
        [TestCase(CrossTab, DefaultCase, TestSearchValue, TestSearchValue, TestOne, ViewType.RecencyView, DefaultCaseGrandTotalMessageValue)]
        [TestCase(CrossTab, DefaultCase, TestSearchValue, TestSearchValue, TestZero, ViewType.RecordDetails, DefaultCaseTestSearchValueMessage)]
        [TestCase(CrossTab, DefaultCase, TestSearchValue, TestSearchValue, TestOne, ViewType.RecordDetails, DefaultCaseTestSearchValueIsOneMessage)]
        public void GetAddlFilter_CrossTabIsEnabled_ReturnsResultObject(
            string permission,
            string hfRow,
            string hfSelectedCrossTabLink,
            string hfRowSearchValue,
            int brandId,
            ViewType viewType,
            string resultMessage)
        {
            // Arrange 
            var lblPermission = GetField<Label>(LabelPermission);
            lblPermission.Text = permission;
            var hiddenFiledRow = GetField<HiddenField>(HiddenFiledRow);
            hiddenFiledRow.Value = hfRow;
            var hiddenFiledColumn = GetField<HiddenField>(HiddenFiledColumn);
            hiddenFiledColumn.Value = hfRow;
            var hiddenSelectedCrossTabLink = GetField<HiddenField>(HiddenSelectedCrossTabLink);
            hiddenSelectedCrossTabLink.Value = hfSelectedCrossTabLink;
            var hiddenFiledRowSearchValue = GetField<HiddenField>(HiddenFiledRowSearchValue);
            hiddenFiledRowSearchValue.Value = hfRowSearchValue;
            var hfColumnSearchValue = GetField<HiddenField>(HiddenFieldColumnSearchValue);
            hfColumnSearchValue.Value = hfRowSearchValue;

            var hfBrandID = GetField<HiddenField>(HiddenFiledBrandId);
            hfBrandID.Value = brandId.ToString();

            ShimAudienceViewBase.AllInstances.ViewTypeGet = (x) => { return viewType; };
            AddReportItems(4);
            CreatePageShimObject();

            // Act
            var result = PrivatePage.Invoke(GetAddlFilter) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(resultMessage)
            );
        }

        private void CreatePageShimObject()
        {
            ShimReport.AllInstances.MasterGet = (x) =>
            {
                MasterPages.Site site = new ShimSite
                {
                    clientconnectionsGet = () =>
                    {
                        return new KMPlatform.Object.ClientConnections
                        {
                            ClientLiveDBConnectionString = string.Empty,
                            ClientTestDBConnectionString = string.Empty
                        };
                    }
                };
                return site;
            };
            ObjShimKmps.ShimDataFunctions.GetClientSqlConnectionClientConnections = (x) => { return new SqlConnection(); };
        }

        private void AddReportItems(int count)
        {
            var drpCrossTabReport = GetField<DropDownList>(DropDownCrossTabReport);
            for (var i = 1; i <= count; i++)
            {
                var item = i.ToString();
                drpCrossTabReport.Items.Add(new ListItem(item, item));
            }
            drpCrossTabReport.DataBind();
            drpCrossTabReport.SelectedIndex = 1;
        }
    }
}
