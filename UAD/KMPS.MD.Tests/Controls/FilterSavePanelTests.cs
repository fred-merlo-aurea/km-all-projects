using System;
using System.Collections.Generic;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KMPlatform.Object;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI.Fakes;
using FilterPanel = KMPS.MD.Controls.FilterPanel;

namespace KMPS.MD.Tests.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterPanelTests : BaseControlTests
    {
        private FilterPanel _testEntity;
        private PrivateObject _privateObject;
        private IDisposable _shims;

        private const string MethodButtonSave = "btnSaveFilter_Click";
        private const string ParamEdit = "Edit";
        private const string ParamAddNew = "AddNew";
        private const string ParamEnd = "End";
        private const string ParamFilter = "filter";
        private const string ParamMdFilter = "MDFilter";
        private const string DivError = "divError";
        private const string TextFilterName = "txtFilterName";
        private const string Text = "text";
        private const string AddToSalesView = "cbAddtoSalesView";
        private const string Existing = "Existing";
        private const string StringTen = "10";
        private const string FilterType = "FilterType";
        private const string Exit = "Exit";
        private const int Ten = 10;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new FilterPanel();
            InitializeUserControl(_testEntity);
        }

        [TearDown]
        public override void TearDown()
        {
            _shims.Dispose();
        }

        [Test]
        [TestCase("PubID", 1)]
        [TestCase("PubID", 2)]
        [TestCase("BrandID", 1)]
        [TestCase("BrandID", 2)]
        [TestCase("UserID", 1)]
        [TestCase("UserID", 2)]
        [TestCase("ViewType", Enums.ViewType.ProductView)]
        [TestCase("ViewType", Enums.ViewType.ConsensusView)]
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
        [TestCase("UserID", 0)]
        [TestCase("BrandID", 0)]
        [TestCase("ViewType", Enums.ViewType.None)]
        public void Property_Get_DefaultValue(string propertyName, object defaultValue)
        {
            // Arrange, Act
            var result = PrivateControl.GetProperty(propertyName);
            
            // Assert
            result.ShouldBe(defaultValue);
        }

        [Test]
        [TestCase(ParamEdit)]
        [TestCase(ParamAddNew)]
        [TestCase(ParamEnd)]
        public void ButtonSaveFilterClick_ForMode_ShouldThrowException(string conditions)
        {
            // Arrange
            SetUpFilter(conditions);
            _privateObject = new PrivateObject(_testEntity);

            // Act & Assert
            try
            {
                _privateObject.Invoke(MethodButtonSave, this, new EventArgs());
            }
            catch(Exception ex)
            {
                NUnit.Framework.Assert.IsTrue(ex is Exception);
                GetField<HtmlGenericControl>(DivError).Visible.ShouldBeTrue();
            }
        }

        [Test]
        [TestCase(ParamFilter)]
        [TestCase(ParamMdFilter)]
        public void ButtonSaveFilterClick_ForNullFilterName_DisplayError(string condition)
        {
            // Arrange
            SetUpFilter(condition);
            _privateObject = new PrivateObject(_testEntity);

            // Act
            _privateObject.Invoke(MethodButtonSave, this, new EventArgs());

            // Assert
            GetField<HtmlGenericControl>(DivError).Visible.ShouldBeTrue();
        }

        private void SetUpFilter(string param)
        {
            base.SetUp();
            _testEntity = new FilterPanel();
            InitializeUserControl(_testEntity);
            InitializeAllControls(_testEntity);
            _shims = ShimsContext.Create();
            ShimDataFunctions.GetClientSqlConnectionClientConnections = (x) => new ShimSqlConnection();
            ShimDataFunctions.executeScalarSqlCommandSqlConnection = (x, y) => StringTen;
            if (param.Equals(ParamEdit))
            {
                ShimFilterPanel.AllInstances.ModeGet = (x) => ParamEdit;
                GetField<TextBox>(TextFilterName).Text = Text;
                GetField<CheckBox>(AddToSalesView).Checked = true;
                ShimMDFilter.ExistsByFilterNameClientConnectionsInt32String = (x, y, z) => false;
            }
            else if (param.Equals(ParamAddNew))
            {
                ShimFilterPanel.AllInstances.ModeGet = (x) => ParamAddNew;
                GetField<TextBox>(TextFilterName).Text = Text;
                GetField<CheckBox>(AddToSalesView).Checked = true;
                ShimListControl.AllInstances.SelectedValueGet = (x) => Existing;
                ShimHiddenField.AllInstances.ValueGet = (x) => StringTen;
                var filterGroupList = new List<FilterGroup>();
                var filterGroup = new FilterGroup()
                {
                    FilterID = Ten
                };
                filterGroupList.Add(filterGroup);
                ShimFilterGroup.getByFilterIDClientConnectionsInt32 = (x, y) => filterGroupList;
                var fieldList = new List<Field>();
                fieldList.Add(new Field());
                var filter = new Filter()
                {
                    FilterNo = Ten,
                    Executed = false,
                    Fields = fieldList
                };
                var mockFilters = new Moq.Mock<Filters>();
                var filters = new Filters(new ClientConnections(), Ten);
                filters.Add(filter);
                ShimFilterPanel.AllInstances.FilterCollectionsGet = (x) => filters;
                ShimMDFilter.ExistsByFilterNameClientConnectionsInt32String = (x, y, z) => false;
            }
            else if (param.Equals(ParamEnd))
            {
                ShimFilterPanel.AllInstances.ModeGet = (x) => ParamEnd;
                GetField<TextBox>(TextFilterName).Text = Text;
                ShimHiddenField.AllInstances.ValueGet = (x) => StringTen;
                GetField<CheckBox>(AddToSalesView).Checked = false;
                ShimListControl.AllInstances.SelectedValueGet = (x) => Exit;
                ShimFilterSchedule.ExistsByFilterIDClientConnectionsInt32 = (x, y) => false;
                var filterGroupList = new List<FilterGroup>();
                var filterGroup = new FilterGroup()
                {
                    FilterID = Ten
                };
                filterGroupList.Add(filterGroup);
                ShimFilterGroup.getByFilterIDClientConnectionsInt32 = (x, y) => filterGroupList;
                var fieldList = new List<Field>();
                fieldList.Add(new Field());
                var filter = new Filter()
                {
                    FilterNo = Ten,
                    Executed = false,
                    Fields = fieldList
                };
                var mockFilters = new Moq.Mock<Filters>();
                var filters = new Filters(new ClientConnections(), Ten);
                filters.Add(filter);
                ShimFilterPanel.AllInstances.FilterCollectionsGet = (x) => filters;
                ShimMDFilter.ExistsByFilterNameClientConnectionsInt32String = (x, y, z) => false;
            }
            else if (param.Equals(ParamFilter))
            {
                ShimFilterPanel.AllInstances.ModeGet = (x) => ParamAddNew;
                GetField<TextBox>(TextFilterName).Text = string.Empty;
                ShimMDFilter.ExistsByFilterNameClientConnectionsInt32String = (x, y, z) => false;
            }
            else if (param.Equals(ParamMdFilter))
            {
                ShimFilterPanel.AllInstances.ModeGet = (x) => ParamAddNew;
                GetField<TextBox>(TextFilterName).Text = Text;
                ShimMDFilter.ExistsByFilterNameClientConnectionsInt32String = (x, y, z) => true;
            }
            ShimFilterPanel.AllInstances.FilterIDsGet = (x) => StringTen;
            ShimBaseControl.AllInstances.clientconnectionsGet = (x) => new ClientConnections();
            ShimRadDropDownTree.AllInstances.SelectedValueGet = (x) => StringTen;
            ShimMDFilter.ExistsQuestionNameClientConnectionsInt32String = (x, y, z) => false;
            var MDFilter = new MDFilter()
            {
                FilterType = FilterType
            };
            ShimMDFilter.GetByIDClientConnectionsInt32 = (x, y) => MDFilter;
        }
    }
}
