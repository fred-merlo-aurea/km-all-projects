using System;
using System.Collections.Generic;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FrameworkUAD.Entity;
using KMPlatform.Object.Fakes;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BusinessLogicFakes = FrameworkUAD.BusinessLogic.Fakes;
using FilterSegmentationSave = KMPS.MD.Controls.FilterSegmentationSave;

namespace KMPS.MD.Tests.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterSegmentationSaveTests : BaseControlTests
    {
        private FilterSegmentationSave _testEntity;
        private PrivateObject _privateObject;

        private const string MethodButtonSave = "btnSaveFS_Click";
        private const string ErrorMessage = "lblErrorMessage";
        private const string Error = "divError";
        private const string Text = "text";
        private const string EditMode = "Edit";
        private const string AddNew = "AddNew";
        private const string TextBoxFilterName = "txtFilterName";
        private const string TextBoxFilterSegmentName = "txtFSName";
        private const string Test = "test";
        private const string Type = "type";
        private const string Name = "name";
        private const string StringTen = "10";
        private const string FilterSegmentationId = "hfFilterSegmentationID";
        private const string UnitTest = "UnitTest";
        private const int One = 1;
        private const int Ten = 10;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new FilterSegmentationSave();
            InitializeUserControl(_testEntity);
            InitializeAllControls(_testEntity);
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
        public void ButtonSaveFSClick_ForNullFilterName_DisplayError()
        {
            // Arrange
            InitializeAllControls(_testEntity);
            _privateObject = new PrivateObject(_testEntity);
            GetField<TextBox>(TextBoxFilterName).Text = string.Empty;

            // Act
            _privateObject.Invoke(MethodButtonSave, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue(),
                () => GetField<Label>(ErrorMessage).Text.ShouldNotBeEmpty());
        }

        [Test]
        public void ButtonSaveFSClick_ForDuplicateFilterName_DisplayError()
        {
            // Arrange
            InitializeAllControls(_testEntity);
            _privateObject = new PrivateObject(_testEntity);
            GetField<TextBox>(TextBoxFilterName).Text = Test;
            ShimFilterSegmentationSave.AllInstances.ModeGet = (x) => AddNew;
            ShimBaseControl.AllInstances.clientconnectionsGet = (x) => new ShimClientConnections();
            GetField<HiddenField>(FilterSegmentationId).Value = StringTen;
            ShimMDFilter.ExistsByFilterNameClientConnectionsInt32String = (x, y, z) => true;
            BusinessLogicFakes.ShimFilterSegmentation.AllInstances.ExistsByIDNameInt32StringClientConnections =
                (x, y, z, d) => true;

            // Act
            _privateObject.Invoke(MethodButtonSave, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue(),
                () => GetField<Label>(ErrorMessage).Text.ShouldNotBeEmpty());
        }

        [Test]
        [TestCase(EditMode)]
        [TestCase(AddNew)]
        public void ButtonSaveFSClick_ForModeEdit_ShouldSaveSegmentation(string mode)
        {
            // Arrange
            SetUpButton(mode);
            Func<string, string> resultValue = x => x + x;
            Action action = () => Console.WriteLine(UnitTest);
            _testEntity.LoadSavedFilterSegmentationName = resultValue;
            _testEntity.hideFilterSegmentationPopup = action;
            _privateObject = new PrivateObject(_testEntity);

            // Act
            _privateObject.Invoke(MethodButtonSave, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue(),
                () => GetField<Label>(ErrorMessage).Text.ShouldBeEmpty());
        }

        private void SetUpButton(string param)
        {
            base.SetUp();
            _testEntity = new FilterSegmentationSave();
            InitializeUserControl(_testEntity);
            InitializeAllControls(_testEntity);
            GetField<TextBox>(TextBoxFilterName).Text = Text;
            if (param.Equals(EditMode))
            {
                ShimFilterSegmentationSave.AllInstances.ModeGet = (x) => EditMode;
                var filterSegmentation = new FilterSegmentation()
                {
                    FilterID = Ten
                };
                BusinessLogicFakes.ShimFilterSegmentation.AllInstances.SelectByIDInt32ClientConnectionsBoolean =
                    (x, y, z, v) => filterSegmentation;
                ShimMDFilter.GetByIDClientConnectionsInt32 = (x, y) => new MDFilter()
                {
                    FilterType = Type
                };
            }
            else if (param.Equals(AddNew))
            {
                ShimFilterSegmentationSave.AllInstances.ModeGet = (x) => AddNew;
                var filters = new Filters(new ShimClientConnections(), One);
                var field = new Field()
                {
                    Name = Name,
                    Text = Test
                };
                var fields = new List<Field>();
                fields.Add(field);
                filters.Add(new KMPS.MD.Objects.Filter
                {
                    FilterNo = Ten,
                    FilterName = Test,
                    FilterGroupID = Ten,
                    FilterGroupName = Test,
                    Fields = fields
                });
                ShimFilterSegmentationSave.AllInstances.FilterCollectionGet = (x) => filters;
                ShimFilterGroup.SaveClientConnectionsInt32Int32 = (x, y, z) => Ten;
                ShimFilterDetails.SaveClientConnectionsFilterDetails = (x, y) => Ten;
                ShimFilterSegmentationSave.AllInstances.FilterViewNosGet = (x) => StringTen;
                var filterViews = new FilterViews(new ShimClientConnections(), One);
                var filterView = new filterView()
                {
                    FilterViewNo = Ten,
                    SelectedFilterNo = StringTen,
                    SuppressedFilterNo = StringTen
                };
                filterViews.Add(filterView);
                ShimFilterSegmentationSave.AllInstances.FilterViewCollectionGet = (x) => filterViews;
                ShimFilter.AllInstances.FilterNoGet = (x) => Ten;
                ShimFilter.AllInstances.FilterGroupIDGet = (x) => Ten;
                BusinessLogicFakes.ShimFilterSegmentationGroup.AllInstances.SaveFilterSegmentationGroupClientConnections =
                    (x, y, z) => Ten;
            }
            GetField<TextBox>(TextBoxFilterSegmentName).Text = Text;
            GetField<HiddenField>(FilterSegmentationId).Value = StringTen;
            ShimBaseControl.AllInstances.clientconnectionsGet = (x) => new ShimClientConnections();
            ShimMDFilter.ExistsByFilterNameClientConnectionsInt32String = (x, y, z) => false;
            BusinessLogicFakes.ShimFilterSegmentation.AllInstances.ExistsByIDNameInt32StringClientConnections =
                (x, y, z, d) => false;
            ShimBaseControl.AllInstances.UserIDGet = (x) => Ten;
            BusinessLogicFakes.ShimFilterSegmentation.AllInstances.SaveFilterSegmentationClientConnections =
                (x, y, z) => Ten;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = (x) => new ShimSqlConnection();
            ShimDataFunctions.executeScalarSqlCommandSqlConnection = (x, y) => StringTen;
        }
    }
}
