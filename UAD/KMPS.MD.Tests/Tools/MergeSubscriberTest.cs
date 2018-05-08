using System;
using System.Collections;
using System.Data.SqlClient.Fakes;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KM.Platform.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using KMPS.MD.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;
using Telerik.Web.UI.Fakes;

namespace KMPS.MD.Tests.Tools
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MergeSubscriberTest : BasePageTests
    {
        private MergeSubscriber _testEntity;
        private PrivateObject _privateObject;
        private GridItemEventInfo info;

        private const string ParamAdmin = "admin";
        private const string ParamZero = "zero";
        private const string ParamTextBox = "textBox";
        private const string MethodButtonSelect = "btnSelect_Click";
        private const string MethodProductsDataBound = "rgProducts_ItemDataBound";
        private const string Error = "divError";
        private const string ErrorMessage = "lblErrorMessage";
        private const string ButtonMerge = "btnMerge";
        private const string GuidText1 = "dddddddd-dddd-dddd-dddd-dddddddddddd";
        private const string GuidText2 = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb";
        private const string GroupOne = "tbIGrp_No1";
        private const string GroupTwo = "tbIGrp_No2";
        private const string StringId = "10";
        private const string Name = "name";
        private const string Open = "open";
        private const string Details = "rgSubscriptionDetails";
        private const string Products = "rgProducts";
        private const string SubscriptionIdOne = "hfSubscriptionID1";
        private const string PubSubscriptionIdOne = "hfPubSubscriptionID1";
        private const string SubscriptionIdTwo = "hfSubscriptionID2";
        private const string PubSubscriptionIdTwo = "hfPubSubscriptionID2";
        private const string PubSubscriptionDetails = "rgPubSubscriptionDetails";
        private const string PubIdOne = "hfPubID1";
        private const string CheckBoxIdOne = "cbID1";
        private const string PubIdTwo = "hfPubID2";
        private const string CheckBoxIdTwo = "cbID2";
        private const string Order = "desc";
        private const int Id = 10;
        private const int Zero = 0;
        private const string Message = "divMessage";
        private const string LabelMessage = "lblMessage";
        private const string MethodButtonMerge = "btnMerge_Click";
        private const string HiddenFieldCircle1 = "hfIsCirc1";
        private const string HiddenFieldId1 = "hfID1";
        private const string HiddenFieldCircle2 = "hfIsCirc2";
        private const string HiddenFieldId2 = "hfID2";
        private const string LabelGroup1 = "lbltbIGrp_No1";
        private const string LabelGroup2 = "lbltbIGrp_No2";
        private const string LabelPubName = "lblPubName1";
        private const string SuppressedFilterOperation = "hfSuppressedFilterOperation";
        private const string Text = "text";
        private const string True = "true";
        private const string False = "false";

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ButtonMerge_ForGridItem_ShouldMergeSubscriber(bool value)
        {
            // Arrange
            SetUpButtonMerge(value);
            _privateObject = new PrivateObject(_testEntity);

            // Act
            _privateObject.Invoke(MethodButtonMerge, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<HtmlGenericControl>(Message).Visible.ShouldBeTrue(),
                () => GetField<Label>(LabelMessage).Text.ShouldNotBeEmpty());
        }

        [Test]
        public void ButtonMerge_ForNoControls_ShouldDisplayError()
        {
            // Arrange
            _testEntity = new MergeSubscriber();
            base.SetUp();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
            ShimUser.IsAdministratorUser = (_) => true;
            _privateObject = new PrivateObject(_testEntity);

            // Act
            _privateObject.Invoke(MethodButtonMerge, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue(),
                () => GetField<Label>(ErrorMessage).Text.ShouldNotBeEmpty());
        }

        private void SetUpButtonMerge(bool value)
        {
            _testEntity = new MergeSubscriber();
            base.SetUp();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
            ShimUser.IsAdministratorUser = (_) => true;
            var gridDataItem = new GridDataItem(new GridTableView(), Zero, Zero, GridItemType.ColItem);
            var items = new ArrayList();
            items.Add(gridDataItem);
            var gridDataItemCollection = new GridDataItemCollection(items);
            ShimRadGrid.AllInstances.ItemsGet = (x) => gridDataItemCollection;
            if (value)
            {
                ShimControl.AllInstances.FindControlString = (sender, controlId) => GetControlById(controlId, StringId, value);
            }
            else
            {
                ShimControl.AllInstances.FindControlString = (sender, controlId) => GetControlById(controlId, StringId, value);
            }
            var gridTableView = new GridTableView();
            ShimRadGrid.AllInstances.MasterTableViewGet = (x) => gridTableView;
            var gridFooterItem = new GridFooterItem(gridTableView, Zero, Zero);
            var gridItemArray = new GridItem[] { gridFooterItem };
            ShimGridTableView.AllInstances.GetItemsGridItemTypeArray = (x, y) => gridItemArray;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = (x) => new ShimSqlConnection();
            ShimDataFunctions.executeSqlCommandSqlConnection = (x, y) => Id;
            ShimRadGrid.AllInstances.DataBind = _ => { };
        }

        private static Control GetControlById(string controlId, string selectedHiddenValue, bool value)
        {
            if (controlId == HiddenFieldCircle1)
            {
                return new HiddenField { ID = controlId, Value = False};
            }
            if (controlId == HiddenFieldCircle2)
            {
                return new HiddenField { ID = controlId, Value = False};
            }
            if (controlId == CheckBoxIdOne)
            {
                return value ? new CheckBox { Checked = true } : new CheckBox { Checked = false };
            }
            if (controlId == CheckBoxIdTwo)
            {
                return value ? new CheckBox { Checked = false } : new CheckBox { Checked = true };
            }
            if (controlId == HiddenFieldId1)
            {
                return new HiddenField { Value = selectedHiddenValue };
            }
            if (controlId == HiddenFieldId2)
            {
                return new HiddenField { Value = selectedHiddenValue };
            }
            if (controlId == LabelGroup1)
            {
                return new Label { Text = Text };
            }
            if (controlId == LabelGroup2)
            {
                return new Label { Text = Text };
            }
            if (controlId == PubSubscriptionIdOne)
            {
                return new HiddenField { ID = controlId, Value = selectedHiddenValue };
            }
            if (controlId == PubSubscriptionIdTwo)
            {
                return new HiddenField { ID = controlId, Value = selectedHiddenValue };
            }
            if (controlId == SuppressedFilterOperation)
            {
                return new HiddenField { ID = controlId, Value = selectedHiddenValue };
            }
            if (controlId == LabelPubName)
            {
                return new Label { ID = controlId, Text = Text };
            }
            return new Control { ID = controlId };
        }
        
        [Test]
        public void ButtonSelectClick_ForAdministrator_ShouldBindProducts()
        {
            // Arrange
            SetUpButton(ParamAdmin);
            _privateObject = new PrivateObject(_testEntity);

            // Act
            _privateObject.Invoke(MethodButtonSelect, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue(),
                () => GetField<Label>(ErrorMessage).Text.ShouldBeEmpty(),
                () => GetField<Button>(ButtonMerge).Visible.ShouldBeTrue());
        }

        [Test]
        [TestCase(ParamZero)]
        [TestCase(ParamTextBox)]
        public void ButtonSelectClick_ForSubscriptionIdZero_DisplayError(string param)
        {
            // Arrange
            SetUpButton(param);
            _privateObject = new PrivateObject(_testEntity);

            // Act
            _privateObject.Invoke(MethodButtonSelect, this, new EventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<HtmlGenericControl>(Error).Visible.ShouldBeTrue(),
                () => GetField<Label>(ErrorMessage).Text.ShouldNotBeEmpty());
        }

        [Test]
        public void RgProductsItemDataBound_ForCheckBox_ShouldThrowException()
        {
            // Arrange
            SetUpProducts();
            _privateObject = new PrivateObject(_testEntity);
            var gridDataItem = new GridDataItem(new GridTableView(), 0, 0, GridItemType.ColItem);
            var gridItemEventArgs = new GridItemEventArgs(gridDataItem, info);

            // Act & Assert
            try
            {
                _privateObject.Invoke(MethodProductsDataBound, this, gridItemEventArgs);
            }
            catch (Exception ex)
            {
                NUnit.Framework.Assert.IsTrue(ex is MissingFieldException);
                GetField<HtmlGenericControl>(Error).Visible.ShouldBe(true);
            }
        }

        private void SetUpButton(string param)
        {
            _testEntity = new MergeSubscriber();
            base.SetUp();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
            ShimUser.IsAdministratorUser = (_) => true;
            if (param.Equals(ParamTextBox))
            {
                GetField<TextBox>(GroupOne).Text = GuidText1;
                GetField<TextBox>(GroupTwo).Text = GuidText1;
            }
            else if (param.Equals(ParamAdmin) || param.Equals(ParamZero))
            {
                GetField<TextBox>(GroupOne).Text = GuidText1;
                GetField<TextBox>(GroupTwo).Text = GuidText2;
            }
            if (param.Equals(ParamAdmin) || param.Equals(ParamTextBox))
            {
                var subscriber = new Subscriber()
                {
                    SubscriptionID = Id,
                    CategoryID = Id,
                    TransactionID = Id,
                    QSourceID = Id
                };
                ShimSubscriber.GetByIGrp_NoClientConnectionsGuid = (x, y) => subscriber;
            }
            else if (param.Equals(ParamZero))
            {
                var subscriber = new Subscriber()
                {
                    SubscriptionID = Zero,
                    CategoryID = Id,
                    TransactionID = Id,
                    QSourceID = Id
                };
                ShimSubscriber.GetByIGrp_NoClientConnectionsGuid = (x, y) => subscriber;
            }
            var category = new Category()
            {
                CategoryCodeID = Id,
                CategoryCodeName = StringId
            };
            var categories = new List<Category>();
            categories.Add(category);
            ShimCategory.GetAll = () => categories;
            var transactionCodeType = new TransactionCodeType()
            {
                TransactionCodeTypeName = Name
            };
            ShimTransactionCodeType.GetByPubTransactionIDInt32 = (x) => transactionCodeType;
            var code = new Code()
            {
                DisplayName = Name
            };
            ShimCode.GetByQSourceIDInt32 = (x) => code;
            var subscriberActivity = new SubscriberActivity()
            {
                Activity = Open
            };
            var subscriberActivityList = new List<SubscriberActivity>();
            subscriberActivityList.Add(subscriberActivity);
            ShimSubscriberActivity.GetClientConnectionsInt32Int32 = (x, y, z) => subscriberActivityList;
            var subscriberVisitActivity = new SubscriberVisitActivity();
            var subscriberVisitActivityList = new List<SubscriberVisitActivity>();
            ShimSubscriberVisitActivity.GetClientConnectionsInt32 = (x, y) => subscriberVisitActivityList;
            var rgSubscriptionDetails = GetField<RadGrid>(Details);
            ShimRadGrid.AllInstances.DataBind = _ => { };
            var subscriberPubs = new SubscriberPubs()
            {
                PubID = Id,
                pubname = Name,
                SubscriptionID = Id,
                IsCirc = true
            };
            var subscriberPubList = new List<SubscriberPubs>();
            subscriberPubList.Add(subscriberPubs);
            ShimSubscriberPubs.GetSubscriberPubsClientConnectionsInt32Int32 = (x, y, z) => subscriberPubList;
            var rgProducts = GetField<RadGrid>(Products);
            ShimRadGrid.AllInstances.DataSourceGet = (x) => 
            {
                var result = new Object();
                if (x.Equals(Details))
                {
                    result = rgSubscriptionDetails;
                }
                else if (x.Equals(Products))
                {
                    result = rgProducts;
                }
                return result;
            };
        }

        private void SetUpProducts()
        {
            _testEntity = new MergeSubscriber();
            base.SetUp();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
            ShimGridEditableItem.AllInstances.GetDataKeyValueString = (x, y) => StringId;
            GetField<HiddenField>(SubscriptionIdOne).Value = StringId;
            GetField<HiddenField>(SubscriptionIdTwo).Value = StringId;
            var pubSubscription = new PubSubscriptions()
            {
                PubCategoryID = Id,
                PubTransactionID = Id,
                PubQSourceID = Id,
                PubSubscriptionID = Id,
                PubID = Id
            };
            ShimPubSubscriptions.GetByPubIDSubscriptionIDClientConnectionsInt32Int32 = (x, y, z) => pubSubscription;
            var category = new Category()
            {
                CategoryCodeID = Id,
                CategoryCodeName = StringId
            };
            var categories = new List<Category>();
            categories.Add(category);
            ShimCategory.GetAll = () => categories;
            var transactionCodeType = new TransactionCodeType()
            {
                TransactionCodeTypeName = Name
            };
            ShimTransactionCodeType.GetByPubTransactionIDInt32 = (x) => transactionCodeType;
            var code = new Code()
            {
                DisplayName = Name
            };
            ShimCode.GetByQSourceIDInt32 = (x) => code;
            var subscriberOpenActivity = new SubscriberOpenActivity();
            var subscriberOpenActivityList = new List<SubscriberOpenActivity>();
            subscriberOpenActivityList.Add(subscriberOpenActivity);
            ShimSubscriberOpenActivity.GetByPubSubscriptionIDClientConnectionsInt32Int32 =
                (x, y, z) => subscriberOpenActivityList;
            var subscriberClickActivity = new SubscriberClickActivity();
            var subscriberClickActivityList = new List<SubscriberClickActivity>();
            subscriberClickActivityList.Add(subscriberClickActivity);
            ShimSubscriberClickActivity.GetByPubSubscriptionIDClientConnectionsInt32Int32 =
                (x, y, z) => subscriberClickActivityList;
            var pubSubscriptionsDimension = new PubSubscriptionsDimension()
            {
                ResponseGroupID = Id,
                ResponseGroupName = Name,
                ResponseDesc = Order
            };
            var pubSubscriptionsDimensionList = new List<PubSubscriptionsDimension>();
            pubSubscriptionsDimensionList.Add(pubSubscriptionsDimension);
            ShimPubSubscriptionsDimension.GetPubSubscriptionsDimensionClientConnectionsInt32Int32 =
                (x, y, z) => pubSubscriptionsDimensionList;
            ShimControl.AllInstances.FindControlString = (x, y) => {
                var result = new Control();
                if (y.Equals(PubSubscriptionDetails))
                {
                    result = GetField<RadGrid>(Details);
                }
                else if (y.Equals(PubSubscriptionIdOne))
                {
                    result = GetField<HiddenField>(SubscriptionIdOne);
                }
                else if (y.Equals(PubIdOne))
                {
                    result = GetField<HiddenField>(SubscriptionIdOne);
                }
                else if (y.Equals(PubSubscriptionIdTwo))
                {
                    result = GetField<HiddenField>(SubscriptionIdTwo);
                }
                else if (y.Equals(PubIdTwo))
                {
                    result = GetField<HiddenField>(SubscriptionIdTwo);
                }
                else if (y.Equals(CheckBoxIdOne))
                {
                    result = GetField<CheckBox>(CheckBoxIdOne);
                }
                else if (y.Equals(CheckBoxIdTwo))
                {
                    result = GetField<CheckBox>(CheckBoxIdTwo);
                }
                return result;
            };
            ShimRadGrid.AllInstances.DataBind = _ => { };
        }
    }
}
