using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Controls;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;
using TestCommonHelpers;

namespace KMPS.MD.Tests.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ActivityTests : BaseControlTests
    {
        private Activity _testEntity;
        const string DummyString = "DummyString";
        private const string RblOpenSearchType = "rblOpenSearchType";
        private const string DrpOpenActivity = "drpOpenActivity";
        private const string DrpOpenActivityDateRange = "drpOpenActivityDateRange";
        private const string DivOpenActivityDateRange = "divOpenActivityDateRange";
        private const string TxtOpenActivityFrom = "txtOpenActivityFrom";
        private const string TxtOpenActivityTo = "txtOpenActivityTo";
        private const string DrpOpenActivityDays = "drpOpenActivityDays";
        private const string TxtCustomOpenActivityDays = "txtCustomOpenActivityDays";
        private const string DivOpenActivityYear = "divOpenActivityYear";
        private const string DivOpenActivityMonth = "divOpenActivityMonth";
        private const string TxtOpenActivityFromYear = "txtOpenActivityFromYear";
        private const string TxtOpenActivityToYear = "txtOpenActivityToYear";
        private const string TxtOpenActivityFromMonth = "txtOpenActivityFromMonth";
        private const string TxtOpenActivityToMonth = "txtOpenActivityToMonth";
        private const string TxtOpenBlastID = "txtOpenBlastID";
        private const string TxtOpenEmailSubject = "txtOpenEmailSubject";
        private const string TxtClickBlastID = "txtClickBlastID";
        private const string TxtClickEmailSubject = "txtClickEmailSubject";
        private const string RadCBOpenCampaigns = "RadCBOpenCampaigns";
        private const string DrpOpenEmailDateRange = "drpOpenEmailDateRange";
        private const string DivOpenEmailDateRange = "divOpenEmailDateRange";
        private const string TxtOpenEmailFromDate = "txtOpenEmailFromDate";
        private const string TxtOpenEmailToDate = "txtOpenEmailToDate";
        private const string DrpOpenEmailDays = "drpOpenEmailDays";
        private const string TxtCustomOpenEmailDays = "txtCustomOpenEmailDays";
        private const string DivOpenEmailYear = "divOpenEmailYear";
        private const string TxtOpenEmailFromYear = "txtOpenEmailFromYear";
        private const string TxtOpenEmailToYear = "txtOpenEmailToYear";
        private const string TxtOpenEmailFromMonth = "txtOpenEmailFromMonth";
        private const string TxtOpenEmailToMonth = "txtOpenEmailToMonth";
        private const string DivOpenEmailMonth = "divOpenEmailMonth";
        private const string DrpClickActivityDateRange = "drpClickActivityDateRange";
        private const string DivClickActivityDateRange = "divClickActivityDateRange";
        private const string TxtClickActivityFrom = "txtClickActivityFrom";
        private const string TxtClickActivityTo = "txtClickActivityTo";
        private const string DrpClickActivityDays = "drpClickActivityDays";
        private const string TxtCustomClickActivityDays = "txtCustomClickActivityDays";
        private const string DivClickActivityYear = "divClickActivityYear";
        private const string DivClickActivityMonth = "divClickActivityMonth";
        private const string TxtClickActivityFromYear = "txtClickActivityFromYear";
        private const string TxtClickActivityToYear = "txtClickActivityToYear";
        private const string TxtClickActivityFromMonth = "txtClickActivityFromMonth";
        private const string TxtClickActivityToMonth = "txtClickActivityToMonth";
        private const string DrpClickEmailDateRange = "drpClickEmailDateRange";
        private const string DivClickEmailDateRange = "divClickEmailDateRange";
        private const string TxtClickEmailFromDate = "txtClickEmailFromDate";
        private const string TxtClickEmailToDate = "txtClickEmailToDate";
        private const string DrpClickEmailDays = "drpClickEmailDays";
        private const string TxtCustomClickEmailDays = "txtCustomClickEmailDays";
        private const string DivClickEmailYear = "divClickEmailYear";
        private const string TxtClickEmailFromYear = "txtClickEmailFromYear";
        private const string TxtClickEmailToYear = "txtClickEmailToYear";
        private const string TxtClickEmailFromMonth = "txtClickEmailFromMonth";
        private const string TxtClickEmailToMonth = "txtClickEmailToMonth";
        private const string DivClickEmailMonth = "divClickEmailMonth";
        private const string DrpVisitActivityDateRange = "drpVisitActivityDateRange";
        private const string DivVisitActivityDateRange = "divVisitActivityDateRange";
        private const string TxtVisitActivityFrom = "txtVisitActivityFrom";
        private const string TxtVisitActivityTo = "txtVisitActivityTo";
        private const string DrpVisitActivityDays = "drpVisitActivityDays";
        private const string TxtCustomVisitActivityDays = "txtCustomVisitActivityDays";
        private const string DivVisitActivityYear = "divVisitActivityYear";
        private const string DivVisitActivityMonth = "divVisitActivityMonth";
        private const string TxtVisitActivityFromYear = "txtVisitActivityFromYear";
        private const string TxtVisitActivityToYear = "txtVisitActivityToYear";
        private const string TxtVisitActivityFromMonth = "txtVisitActivityFromMonth";
        private const string TxtVisitActivityToMonth = "txtVisitActivityToMonth";
        private const string RblClickSearchType = "rblClickSearchType";
        private const string DrpClickActivity = "drpClickActivity";
        private const string TxtLink = "txtLink";
        private const string RadCBClickCampaigns = "RadCBClickCampaigns";
        private const string DrpDomain = "drpDomain";
        private const string TxtURL = "txtURL";
        private const string DrpVisitActivity = "drpVisitActivity";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new Activity();
            InitializeUserControl(_testEntity);
        }

        [Test]
        [TestCase("PubID", 1)]
        [TestCase("PubID", 2)]
        public void Property_Set_GetSameValue(string propertyName, object value)
        {
            // Arrange, Act
            PrivateControl.SetProperty(propertyName, value);
            var result = PrivateControl.GetProperty(propertyName);
            
            // Assert
            result.ShouldBe(value);
        }

        [Test]
        [TestCase("PubID", 0)]
        public void Property_Get_DefaultValue(string propertyName, object defaultValue)
        {
            // Arrange, Act
            var result = PrivateControl.GetProperty(propertyName);
            
            // Assert
            result.ShouldBe(defaultValue);
        }

        [Test]
        public void LoadActivityFilters_OPEN_CRITERIA_ReachEnd()
        {
            // Arrange
            var field = GetField("OPEN CRITERIA", string.Empty, string.Empty);
            InitializePageAndControls();

            // Act
            _testEntity.LoadActivityFilters(field);
            var rblOpenSearchTypeValue = PrivateControl.GetField(RblOpenSearchType) as RadioButtonList;
            var drpOpenActivity = PrivateControl.GetField(DrpOpenActivity) as DropDownList;

            // Assert 
            rblOpenSearchTypeValue.ShouldNotBeNull();
            rblOpenSearchTypeValue.SelectedValue.ShouldBeNullOrWhiteSpace();
            drpOpenActivity.ShouldNotBeNull();
            drpOpenActivity.SelectedValue.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LoadActivityFilters_OPEN_ACTIVITYAndDateRange_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "DateRange";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("OPEN ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpOpenActivity = PrivateControl.GetField(DrpOpenActivityDateRange) as DropDownList;
            var divOpenActivityDateRange = PrivateControl.GetField(DivOpenActivityDateRange) as HtmlGenericControl;
            var txtOpenActivityFrom = PrivateControl.GetField(TxtOpenActivityFrom) as TextBox;
            var txtOpenActivityTo = PrivateControl.GetField(TxtOpenActivityTo) as TextBox;

            // Assert 
            drpOpenActivity.ShouldNotBeNull();
            drpOpenActivity.SelectedValue.ShouldBeNullOrWhiteSpace();
            divOpenActivityDateRange.ShouldNotBeNull();
            divOpenActivityDateRange.Visible.ShouldBeTrue();
            txtOpenActivityFrom.ShouldNotBeNull();
            txtOpenActivityFrom.Text.ShouldContain(DummyString);
            txtOpenActivityTo.ShouldNotBeNull();
            txtOpenActivityTo.Text.ShouldContain(DummyString);
        }

        [Test]
        [TestCase("7")]
        [TestCase("Custom")]
        public void LoadActivityFilters_OPEN_ACTIVITYAndXDays_ReachEnd(string values)
        {
            // Arrange
            const string SearchCondition = "XDays";
            var field = GetField("OPEN ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpOpenActivity = PrivateControl.GetField(DrpOpenActivityDateRange) as DropDownList;

            // Assert 
            drpOpenActivity.ShouldNotBeNull();
            drpOpenActivity.SelectedValue.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LoadActivityFilters_OPEN_ACTIVITYAndYEAR_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "YEAR";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("OPEN ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpOpenActivity = PrivateControl.GetField(DrpOpenActivityDateRange) as DropDownList;
            var divOpenActivityYear = PrivateControl.GetField(DivOpenActivityYear) as HtmlGenericControl;
            var txtOpenActivityFromYear = PrivateControl.GetField(TxtOpenActivityFromYear) as TextBox;
            var txtOpenActivityToYear = PrivateControl.GetField(TxtOpenActivityToYear) as TextBox;

            // Assert 
            drpOpenActivity.ShouldNotBeNull();
            drpOpenActivity.SelectedValue.ShouldBeNullOrWhiteSpace();
            divOpenActivityYear.ShouldNotBeNull();
            divOpenActivityYear.Visible.ShouldBeTrue();
            txtOpenActivityFromYear.ShouldNotBeNull();
            txtOpenActivityFromYear.Text.ShouldContain(DummyString);
            txtOpenActivityToYear.ShouldNotBeNull();
            txtOpenActivityToYear.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_OPEN_ACTIVITYAndMONTH_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "MONTH";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("OPEN ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpOpenActivity = PrivateControl.GetField(DrpOpenActivityDateRange) as DropDownList;
            var divOpenActivityMonth = PrivateControl.GetField(DivOpenActivityMonth) as HtmlGenericControl;
            var txtOpenActivityFromMonth = PrivateControl.GetField(TxtOpenActivityFromMonth) as TextBox;
            var txtOpenActivityToMonth = PrivateControl.GetField(TxtOpenActivityToMonth) as TextBox;

            // Assert 
            drpOpenActivity.ShouldNotBeNull();
            drpOpenActivity.SelectedValue.ShouldBeNullOrWhiteSpace();
            divOpenActivityMonth.ShouldNotBeNull();
            divOpenActivityMonth.Visible.ShouldBeTrue();
            txtOpenActivityFromMonth.ShouldNotBeNull();
            txtOpenActivityFromMonth.Text.ShouldContain(DummyString);
            txtOpenActivityToMonth.ShouldNotBeNull();
            txtOpenActivityToMonth.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_OPEN_BLASTID_ReachEnd()
        {
            // Arrange
            var field = GetField("OPEN BLASTID", string.Empty, DummyString);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var txtOpenBlastID = PrivateControl.GetField(TxtOpenBlastID) as TextBox;

            // Assert 
            txtOpenBlastID.ShouldNotBeNull();
            txtOpenBlastID.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_OPEN_CAMPAIGNS_ReachEnd()
        {
            // Arrange
            var field = GetField("OPEN CAMPAIGNS", string.Empty, DummyString);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var radCBOpenCampaigns = PrivateControl.GetField(RadCBOpenCampaigns) as RadComboBox;

            // Assert 
            radCBOpenCampaigns.ShouldNotBeNull();
            radCBOpenCampaigns.SelectedValue.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_OPEN_EMAIL_SUBJECT_ReachEnd()
        {
            // Arrange
            var field = GetField("OPEN EMAIL SUBJECT", string.Empty, DummyString);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var txtOpenEmailSubject = PrivateControl.GetField(TxtOpenEmailSubject) as TextBox;

            // Assert 
            txtOpenEmailSubject.ShouldNotBeNull();
            txtOpenEmailSubject.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_OPEN_EMAIL_SENT_DATEAndDateRange_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "DateRange";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("OPEN EMAIL SENT DATE", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpOpenEmailDateRange = PrivateControl.GetField(DrpOpenEmailDateRange) as DropDownList;
            var divOpenEmailDateRange = PrivateControl.GetField(DivOpenEmailDateRange) as HtmlGenericControl;
            var txtOpenEmailFromDate = PrivateControl.GetField(TxtOpenEmailFromDate) as TextBox;
            var txtOpenEmailToDate = PrivateControl.GetField(TxtOpenEmailToDate) as TextBox;

            // Assert 
            drpOpenEmailDateRange.ShouldNotBeNull();
            drpOpenEmailDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
            divOpenEmailDateRange.ShouldNotBeNull();
            divOpenEmailDateRange.Visible.ShouldBeTrue();
            txtOpenEmailFromDate.ShouldNotBeNull();
            txtOpenEmailFromDate.Text.ShouldContain(DummyString);
            txtOpenEmailToDate.ShouldNotBeNull();
            txtOpenEmailToDate.Text.ShouldContain(DummyString);
        }

        [Test]
        [TestCase("7")]
        [TestCase("Custom")]
        public void LoadActivityFilters_OPEN_EMAIL_SENT_DATEAndXDays_ReachEnd(string values)
        {
            // Arrange
            const string SearchCondition = "XDays";
            var field = GetField("OPEN EMAIL SENT DATE", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpOpenEmailDateRange = PrivateControl.GetField(DrpOpenEmailDateRange) as DropDownList;

            // Assert 
            drpOpenEmailDateRange.ShouldNotBeNull();
            drpOpenEmailDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LoadActivityFilters_OPEN_EMAIL_SENT_DATEAndYEAR_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "YEAR";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("OPEN EMAIL SENT DATE", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpOpenEmailDateRange = PrivateControl.GetField(DrpOpenEmailDateRange) as DropDownList;
            var divOpenEmailYear = PrivateControl.GetField(DivOpenEmailYear) as HtmlGenericControl;
            var txtOpenEmailFromYear = PrivateControl.GetField(TxtOpenEmailFromYear) as TextBox;
            var txtOpenEmailToYear = PrivateControl.GetField(TxtOpenEmailToYear) as TextBox;

            // Assert 
            drpOpenEmailDateRange.ShouldNotBeNull();
            drpOpenEmailDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
            divOpenEmailYear.ShouldNotBeNull();
            divOpenEmailYear.Visible.ShouldBeTrue();
            txtOpenEmailFromYear.ShouldNotBeNull();
            txtOpenEmailFromYear.Text.ShouldContain(DummyString);
            txtOpenEmailToYear.ShouldNotBeNull();
            txtOpenEmailToYear.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_OPEN_EMAIL_SENT_DATEAndMONTH_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "MONTH";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("OPEN EMAIL SENT DATE", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpOpenEmail = PrivateControl.GetField(DrpOpenEmailDateRange) as DropDownList;
            var divOpenEmailMonth = PrivateControl.GetField(DivOpenEmailMonth) as HtmlGenericControl;
            var txtOpenEmailFromMonth = PrivateControl.GetField(TxtOpenEmailFromMonth) as TextBox;
            var txtOpenEmailToMonth = PrivateControl.GetField(TxtOpenEmailToMonth) as TextBox;

            // Assert 
            drpOpenEmail.ShouldNotBeNull();
            drpOpenEmail.SelectedValue.ShouldBeNullOrWhiteSpace();
            divOpenEmailMonth.ShouldNotBeNull();
            divOpenEmailMonth.Visible.ShouldBeTrue();
            txtOpenEmailFromMonth.ShouldNotBeNull();
            txtOpenEmailFromMonth.Text.ShouldContain(DummyString);
            txtOpenEmailToMonth.ShouldNotBeNull();
            txtOpenEmailToMonth.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_CLICK_ACTIVITYAndDateRange_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "DateRange";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("CLICK ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpClickActivityDateRange = PrivateControl.GetField(DrpClickActivityDateRange) as DropDownList;
            var divClickActivityDateRange = PrivateControl.GetField(DivClickActivityDateRange) as HtmlGenericControl;
            var txtClickActivityFromDate = PrivateControl.GetField(TxtClickActivityFrom) as TextBox;
            var txtClickActivityToDate = PrivateControl.GetField(TxtClickActivityTo) as TextBox;

            // Assert 
            drpClickActivityDateRange.ShouldNotBeNull();
            drpClickActivityDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
            divClickActivityDateRange.ShouldNotBeNull();
            divClickActivityDateRange.Visible.ShouldBeTrue();
            txtClickActivityFromDate.ShouldNotBeNull();
            txtClickActivityFromDate.Text.ShouldContain(DummyString);
            txtClickActivityToDate.ShouldNotBeNull();
            txtClickActivityToDate.Text.ShouldContain(DummyString);
        }

        [Test]
        [TestCase("7")]
        [TestCase("Custom")]
        public void LoadActivityFilters_CLICK_ACTIVITYAndXDays_ReachEnd(string values)
        {
            // Arrange
            const string SearchCondition = "XDays";
            var field = GetField("CLICK ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpClickActivityDateRange = PrivateControl.GetField(DrpClickActivityDateRange) as DropDownList;

            // Assert 
            drpClickActivityDateRange.ShouldNotBeNull();
            drpClickActivityDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LoadActivityFilters_CLICK_ACTIVITYAndYEAR_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "YEAR";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("CLICK ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpClickActivityDateRange = PrivateControl.GetField(DrpClickActivityDateRange) as DropDownList;
            var divClickActivityYear = PrivateControl.GetField(DivClickActivityYear) as HtmlGenericControl;
            var txtClickActivityFromYear = PrivateControl.GetField(TxtClickActivityFromYear) as TextBox;
            var txtClickActivityToYear = PrivateControl.GetField(TxtClickActivityToYear) as TextBox;

            // Assert 
            drpClickActivityDateRange.ShouldNotBeNull();
            drpClickActivityDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
            divClickActivityYear.ShouldNotBeNull();
            divClickActivityYear.Visible.ShouldBeTrue();
            txtClickActivityFromYear.ShouldNotBeNull();
            txtClickActivityFromYear.Text.ShouldContain(DummyString);
            txtClickActivityToYear.ShouldNotBeNull();
            txtClickActivityToYear.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_CLICK_ACTIVITYEAndMONTH_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "MONTH";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("CLICK ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpClickActivity = PrivateControl.GetField(DrpClickActivityDateRange) as DropDownList;
            var divClickActivityMonth = PrivateControl.GetField(DivClickActivityMonth) as HtmlGenericControl;
            var txtClickActivityFromMonth = PrivateControl.GetField(TxtClickActivityFromMonth) as TextBox;
            var txtClickActivityToMonth = PrivateControl.GetField(TxtClickActivityToMonth) as TextBox;

            // Assert 
            drpClickActivity.ShouldNotBeNull();
            drpClickActivity.SelectedValue.ShouldBeNullOrWhiteSpace();
            divClickActivityMonth.ShouldNotBeNull();
            divClickActivityMonth.Visible.ShouldBeTrue();
            txtClickActivityFromMonth.ShouldNotBeNull();
            txtClickActivityFromMonth.Text.ShouldContain(DummyString);
            txtClickActivityToMonth.ShouldNotBeNull();
            txtClickActivityToMonth.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_CLICK_BLASTID_ReachEnd()
        {
            // Arrange
            var field = GetField("CLICK BLASTID", string.Empty, DummyString);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var txtClickBlastID = PrivateControl.GetField(TxtClickBlastID) as TextBox;

            // Assert 
            txtClickBlastID.ShouldNotBeNull();
            txtClickBlastID.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_CLICK_EMAIL_SUBJECT_ReachEnd()
        {
            // Arrange
            var field = GetField("CLICK EMAIL SUBJECT", string.Empty, DummyString);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var txtClickEmailSubject = PrivateControl.GetField(TxtClickEmailSubject) as TextBox;

            // Assert 
            txtClickEmailSubject.ShouldNotBeNull();
            txtClickEmailSubject.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_CLICK_EMAIL_SENT_DATEAndDateRange_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "DateRange";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("CLICK EMAIL SENT DATE", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpClickEmailDateRange = PrivateControl.GetField(DrpClickEmailDateRange) as DropDownList;
            var divClickEmailDateRange = PrivateControl.GetField(DivClickEmailDateRange) as HtmlGenericControl;
            var txtClickEmailFromDate = PrivateControl.GetField(TxtClickEmailFromDate) as TextBox;
            var txtClickEmailToDate = PrivateControl.GetField(TxtClickEmailToDate) as TextBox;

            // Assert 
            drpClickEmailDateRange.ShouldNotBeNull();
            drpClickEmailDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
            divClickEmailDateRange.ShouldNotBeNull();
            divClickEmailDateRange.Visible.ShouldBeTrue();
            txtClickEmailFromDate.ShouldNotBeNull();
            txtClickEmailFromDate.Text.ShouldContain(DummyString);
            txtClickEmailToDate.ShouldNotBeNull();
            txtClickEmailToDate.Text.ShouldContain(DummyString);
        }

        [Test]
        [TestCase("7")]
        [TestCase("Custom")]
        public void LoadActivityFilters_CLICK_EMAIL_SENT_DATEAndXDays_ReachEnd(string values)
        {
            // Arrange
            const string SearchCondition = "XDays";
            var field = GetField("CLICK EMAIL SENT DATE", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpClickEmailDateRange = PrivateControl.GetField(DrpClickEmailDateRange) as DropDownList;

            // Assert 
            drpClickEmailDateRange.ShouldNotBeNull();
            drpClickEmailDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LoadActivityFilters_CLICK_EMAIL_SENT_DATEAndYEAR_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "YEAR";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("CLICK EMAIL SENT DATE", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpClickEmailDateRange = PrivateControl.GetField(DrpClickEmailDateRange) as DropDownList;
            var divClickEmailYear = PrivateControl.GetField(DivClickEmailYear) as HtmlGenericControl;
            var txtClickEmailFromYear = PrivateControl.GetField(TxtClickEmailFromYear) as TextBox;
            var txtClickEmailToYear = PrivateControl.GetField(TxtClickEmailToYear) as TextBox;

            // Assert 
            drpClickEmailDateRange.ShouldNotBeNull();
            drpClickEmailDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
            divClickEmailYear.ShouldNotBeNull();
            divClickEmailYear.Visible.ShouldBeTrue();
            txtClickEmailFromYear.ShouldNotBeNull();
            txtClickEmailFromYear.Text.ShouldContain(DummyString);
            txtClickEmailToYear.ShouldNotBeNull();
            txtClickEmailToYear.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_CLICK_EMAIL_SENT_DATEAndMONTH_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "MONTH";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("CLICK EMAIL SENT DATE", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpClickEmail = PrivateControl.GetField(DrpClickEmailDateRange) as DropDownList;
            var divClickEmailMonth = PrivateControl.GetField(DivClickEmailMonth) as HtmlGenericControl;
            var txtClickEmailFromMonth = PrivateControl.GetField(TxtClickEmailFromMonth) as TextBox;
            var txtClickEmailToMonth = PrivateControl.GetField(TxtClickEmailToMonth) as TextBox;

            // Assert 
            drpClickEmail.ShouldNotBeNull();
            drpClickEmail.SelectedValue.ShouldBeNullOrWhiteSpace();
            divClickEmailMonth.ShouldNotBeNull();
            divClickEmailMonth.Visible.ShouldBeTrue();
            txtClickEmailFromMonth.ShouldNotBeNull();
            txtClickEmailFromMonth.Text.ShouldContain(DummyString);
            txtClickEmailToMonth.ShouldNotBeNull();
            txtClickEmailToMonth.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_DOMAIN_TRACKING_ReachEnd()
        {
            // Arrange
            var field = GetField("DOMAIN TRACKING", string.Empty, DummyString);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpDomain = PrivateControl.GetField(DrpDomain) as DropDownList;

            // Assert 
            drpDomain.ShouldNotBeNull();
            drpDomain.SelectedValue.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LoadActivityFiltersURL_ReachEnd()
        {
            // Arrange
            var field = GetField("URL", DummyString, DummyString);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var txtURL = PrivateControl.GetField(TxtURL) as TextBox;

            // Assert 
            txtURL.ShouldNotBeNull();
            txtURL.Text.ShouldBe(DummyString);
        }

        [Test]
        public void LoadActivityFilters_VISIT_CRITERIA_ReachEnd()
        {
            // Arrange
            var field = GetField("VISIT CRITERIA", string.Empty, DummyString);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpVisitActivity = PrivateControl.GetField(DrpVisitActivity) as DropDownList;

            // Assert 
            drpVisitActivity.ShouldNotBeNull();
            drpVisitActivity.SelectedValue.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LoadActivityFilters_VISIT_ACTIVITYAndDateRange_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "DateRange";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("VISIT ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpVisitActivityDateRange = PrivateControl.GetField(DrpVisitActivityDateRange) as DropDownList;
            var divVisitActivityDateRange = PrivateControl.GetField(DivVisitActivityDateRange) as HtmlGenericControl;
            var txtVisitActivityFromDate = PrivateControl.GetField(TxtVisitActivityFrom) as TextBox;
            var txtVisitActivityToDate = PrivateControl.GetField(TxtVisitActivityTo) as TextBox;

            // Assert 
            drpVisitActivityDateRange.ShouldNotBeNull();
            drpVisitActivityDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
            divVisitActivityDateRange.ShouldNotBeNull();
            divVisitActivityDateRange.Visible.ShouldBeTrue();
            txtVisitActivityFromDate.ShouldNotBeNull();
            txtVisitActivityFromDate.Text.ShouldContain(DummyString);
            txtVisitActivityToDate.ShouldNotBeNull();
            txtVisitActivityToDate.Text.ShouldContain(DummyString);
        }

        [Test]
        [TestCase("7")]
        [TestCase("Custom")]
        public void LoadActivityFilters_VISIT_ACTIVITYAndXDays_ReachEnd(string values)
        {
            // Arrange
            const string SearchCondition = "XDays";
            var field = GetField("VISIT ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpVisitActivityDateRange = PrivateControl.GetField(DrpVisitActivityDateRange) as DropDownList;

            // Assert 
            drpVisitActivityDateRange.ShouldNotBeNull();
            drpVisitActivityDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LoadActivityFilters_VISIT_ACTIVITYAndYEAR_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "YEAR";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("VISIT ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpVisitActivityDateRange = PrivateControl.GetField(DrpVisitActivityDateRange) as DropDownList;
            var divVisitActivityYear = PrivateControl.GetField(DivVisitActivityYear) as HtmlGenericControl;
            var txtVisitActivityFromYear = PrivateControl.GetField(TxtVisitActivityFromYear) as TextBox;
            var txtVisitActivityToYear = PrivateControl.GetField(TxtVisitActivityToYear) as TextBox;

            // Assert 
            drpVisitActivityDateRange.ShouldNotBeNull();
            drpVisitActivityDateRange.SelectedValue.ShouldBeNullOrWhiteSpace();
            divVisitActivityYear.ShouldNotBeNull();
            divVisitActivityYear.Visible.ShouldBeTrue();
            txtVisitActivityFromYear.ShouldNotBeNull();
            txtVisitActivityFromYear.Text.ShouldContain(DummyString);
            txtVisitActivityToYear.ShouldNotBeNull();
            txtVisitActivityToYear.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_VISIT_ACTIVITYEAndMONTH_ReachEnd()
        {
            // Arrange
            const string SearchCondition = "MONTH";
            var values = $"{DummyString}|{DummyString}";
            var field = GetField("VISIT ACTIVITY", SearchCondition, values);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var drpVisitActivity = PrivateControl.GetField(DrpVisitActivityDateRange) as DropDownList;
            var divVisitActivityMonth = PrivateControl.GetField(DivVisitActivityMonth) as HtmlGenericControl;
            var txtVisitActivityFromMonth = PrivateControl.GetField(TxtVisitActivityFromMonth) as TextBox;
            var txtVisitActivityToMonth = PrivateControl.GetField(TxtVisitActivityToMonth) as TextBox;

            // Assert 
            drpVisitActivity.ShouldNotBeNull();
            drpVisitActivity.SelectedValue.ShouldBeNullOrWhiteSpace();
            divVisitActivityMonth.ShouldNotBeNull();
            divVisitActivityMonth.Visible.ShouldBeTrue();
            txtVisitActivityFromMonth.ShouldNotBeNull();
            txtVisitActivityFromMonth.Text.ShouldContain(DummyString);
            txtVisitActivityToMonth.ShouldNotBeNull();
            txtVisitActivityToMonth.Text.ShouldContain(DummyString);
        }

        [Test]
        public void LoadActivityFilters_CLICK_CRITERIA_ReachEnd()
        {
            // Arrange
            var field = GetField("CLICK CRITERIA", string.Empty, string.Empty);
            InitializePageAndControls();

            // Act
            _testEntity.LoadActivityFilters(field);
            var rblClickSearchType = PrivateControl.GetField(RblClickSearchType) as RadioButtonList;
            var drpClickActivity = PrivateControl.GetField(DrpClickActivity) as DropDownList;

            // Assert 
            rblClickSearchType.ShouldNotBeNull();
            rblClickSearchType.SelectedValue.ShouldBeNullOrWhiteSpace();
           drpClickActivity.ShouldNotBeNull();
            drpClickActivity.SelectedValue.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LoadActivityFilters_LINK_ReachEnd()
        {
            // Arrange
            var field = GetField("LINK", string.Empty, DummyString);
            InitializePageAndControls();

            // Act
            _testEntity.LoadActivityFilters(field);
            var txtLink = PrivateControl.GetField(TxtLink) as TextBox;

            // Assert 
            txtLink.ShouldNotBeNull();
            txtLink.Text.ShouldBe(DummyString);
        }

        [Test]
        public void LoadActivityFilters_CLICK_CAMPAIGNS_ReachEnd()
        {
            // Arrange
            var field = GetField("CLICK CAMPAIGNS", string.Empty, DummyString);
            InitializePageAndControls();

            // Act	
            _testEntity.LoadActivityFilters(field);
            var radCBClickCampaigns = PrivateControl.GetField(RadCBClickCampaigns) as RadComboBox;

            // Assert 
            radCBClickCampaigns.ShouldNotBeNull();
            radCBClickCampaigns.SelectedValue.ShouldContain(DummyString);
        }

        private void InitializePageAndControls()
        {
            PrivateControl.SetField(DrpOpenActivity, new DropDownList());
            PrivateControl.SetField(DrpOpenActivityDays, new DropDownList());
            PrivateControl.SetField(DrpOpenActivityDateRange, new DropDownList());
            PrivateControl.SetField(DrpOpenEmailDays, new DropDownList());
            PrivateControl.SetField(DrpOpenEmailDateRange, new DropDownList());
            PrivateControl.SetField(DrpClickActivityDays, new DropDownList());
            PrivateControl.SetField(DrpClickActivityDateRange, new DropDownList());
            PrivateControl.SetField(DrpClickEmailDays, new DropDownList());
            PrivateControl.SetField(DrpClickEmailDateRange, new DropDownList());
            PrivateControl.SetField(DrpVisitActivityDays, new DropDownList());
            PrivateControl.SetField(DrpVisitActivityDateRange, new DropDownList());
            PrivateControl.SetField(DrpClickActivity, new DropDownList());
            PrivateControl.SetField(DrpDomain, new DropDownList());
            PrivateControl.SetField(DrpVisitActivity, new DropDownList());

            PrivateControl.SetField(RblOpenSearchType, new RadioButtonList());
            PrivateControl.SetField(RblClickSearchType, new RadioButtonList());

            PrivateControl.SetField(RadCBOpenCampaigns, new RadComboBox
            {
                Items = {new RadComboBoxItem(DummyString, DummyString)}
            });
            PrivateControl.SetField(RadCBClickCampaigns, new RadComboBox
            {
                Items = { new RadComboBoxItem(DummyString, DummyString) }
            });

            PrivateControl.SetField(DivOpenActivityDateRange, new HtmlGenericControl());
            PrivateControl.SetField(DivOpenActivityYear, new HtmlGenericControl());
            PrivateControl.SetField(DivOpenActivityMonth, new HtmlGenericControl());
            PrivateControl.SetField(DivOpenEmailDateRange, new HtmlGenericControl());
            PrivateControl.SetField(DivOpenEmailYear, new HtmlGenericControl());
            PrivateControl.SetField(DivOpenEmailMonth, new HtmlGenericControl());
            PrivateControl.SetField(DivClickActivityDateRange, new HtmlGenericControl());
            PrivateControl.SetField(DivClickActivityYear, new HtmlGenericControl());
            PrivateControl.SetField(DivClickActivityMonth, new HtmlGenericControl());
            PrivateControl.SetField(DivClickEmailDateRange, new HtmlGenericControl());
            PrivateControl.SetField(DivClickEmailYear, new HtmlGenericControl());
            PrivateControl.SetField(DivClickEmailMonth, new HtmlGenericControl());
            PrivateControl.SetField(DivVisitActivityDateRange, new HtmlGenericControl());
            PrivateControl.SetField(DivVisitActivityYear, new HtmlGenericControl());
            PrivateControl.SetField(DivVisitActivityMonth, new HtmlGenericControl());

            PrivateControl.SetField(TxtCustomOpenActivityDays, new TextBox());
            PrivateControl.SetField(TxtOpenActivityFrom, new TextBox());
            PrivateControl.SetField(TxtOpenActivityTo, new TextBox());
            PrivateControl.SetField(TxtOpenActivityFromYear, new TextBox());
            PrivateControl.SetField(TxtOpenActivityToYear, new TextBox());
            PrivateControl.SetField(TxtOpenActivityFromMonth, new TextBox());
            PrivateControl.SetField(TxtOpenActivityToMonth, new TextBox());
            PrivateControl.SetField(TxtOpenBlastID, new TextBox());
            PrivateControl.SetField(TxtOpenEmailSubject, new TextBox());
            PrivateControl.SetField(TxtClickBlastID, new TextBox());
            PrivateControl.SetField(TxtClickEmailSubject, new TextBox());
            PrivateControl.SetField(TxtCustomOpenEmailDays, new TextBox());
            PrivateControl.SetField(TxtOpenEmailFromDate, new TextBox());
            PrivateControl.SetField(TxtOpenEmailToDate, new TextBox());
            PrivateControl.SetField(TxtOpenEmailFromYear, new TextBox());
            PrivateControl.SetField(TxtOpenEmailToYear, new TextBox());
            PrivateControl.SetField(TxtOpenEmailFromMonth, new TextBox());
            PrivateControl.SetField(TxtOpenEmailToMonth, new TextBox());
            PrivateControl.SetField(TxtCustomClickActivityDays, new TextBox());
            PrivateControl.SetField(TxtClickActivityFrom, new TextBox());
            PrivateControl.SetField(TxtClickActivityTo, new TextBox());
            PrivateControl.SetField(TxtClickActivityFromYear, new TextBox());
            PrivateControl.SetField(TxtClickActivityToYear, new TextBox());
            PrivateControl.SetField(TxtClickActivityFromMonth, new TextBox());
            PrivateControl.SetField(TxtClickActivityToMonth, new TextBox());
            PrivateControl.SetField(TxtCustomClickEmailDays, new TextBox());
            PrivateControl.SetField(TxtClickEmailFromDate, new TextBox());
            PrivateControl.SetField(TxtClickEmailToDate, new TextBox());
            PrivateControl.SetField(TxtClickEmailFromYear, new TextBox());
            PrivateControl.SetField(TxtClickEmailToYear, new TextBox());
            PrivateControl.SetField(TxtClickEmailFromMonth, new TextBox());
            PrivateControl.SetField(TxtClickEmailToMonth, new TextBox());
            PrivateControl.SetField(TxtCustomVisitActivityDays, new TextBox());
            PrivateControl.SetField(TxtVisitActivityFrom, new TextBox());
            PrivateControl.SetField(TxtVisitActivityTo, new TextBox());
            PrivateControl.SetField(TxtVisitActivityFromYear, new TextBox());
            PrivateControl.SetField(TxtVisitActivityToYear, new TextBox());
            PrivateControl.SetField(TxtVisitActivityFromMonth, new TextBox());
            PrivateControl.SetField(TxtVisitActivityToMonth, new TextBox());
            PrivateControl.SetField(TxtLink, new TextBox());
            PrivateControl.SetField(TxtURL, new TextBox());
        }

        private Field GetField(string fieldName, string searchCondition, string values)
        {
            return new Field
            {
                Name = fieldName,
                SearchCondition = searchCondition,
                Values = values
            };
        }
    }
}
