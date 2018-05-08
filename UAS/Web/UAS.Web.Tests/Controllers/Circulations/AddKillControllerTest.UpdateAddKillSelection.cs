using System.Collections.Generic;
using System.Web.Mvc;
using Core_AMS.Utilities;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using UAS.Web.Models.Circulations;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAD.Entity;
using FrameworkUAD.BusinessLogic.Fakes;
using KM.Platform.Fakes;
using NUnit.Framework;
using Shouldly;

namespace UAS.Web.Tests.Controllers.Circulations
{
    public partial class AddKillControllerTest
    {
        private const string RemoveType = "Remove";
        private const string AddType = "Add";
        private const string AddKillGridViewName = "_addKillGrid";
        private const string RoutevalueUnAuthorized = "UnAuthorized";
        private bool _isBulkActionIDUpdateSaved;
        private bool _isSubscriberAddKillSaved;
        private bool _isInsertDetailSaved;

        [Test]
        [TestCase(RemoveType)]
        [TestCase(AddType)]
        public void UpdateAddKillSelection_WhenUserHasAccess_SavesAndReturnsPartialView(string type)
        {
            // Arrange
            var container = GetAddKillContainer();
            container.Type = type;
            var jsonString = GetAddKillContainerJson(container);
            SetFakesForUpdateAddKillSelection();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, en, ef, ea) => true; 

            // Act
            var result = _controller.UpdateAddKillSelection(jsonString);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => 
                    {
                       var partialView =  result.ShouldBeOfType<PartialViewResult>();
                        partialView.ShouldSatisfyAllConditions(
                            () => partialView.ShouldNotBeNull(),
                            () => partialView.ViewName.ShouldContain(AddKillGridViewName),
                            () => partialView.Model.ShouldNotBeNull(),
                            () => 
                                {
                                    var model = partialView.Model.ShouldBeOfType<List<AddKillContainer>>();
                                    model.ShouldSatisfyAllConditions(
                                        () => model.ShouldNotBeNull(),
                                        () => model.Count.ShouldBe(1),
                                        () => model[0].ProductID.ShouldBe(1),
                                        () => model[0].Type.ShouldBe(type));
                                });
                    },
               () => _isBulkActionIDUpdateSaved.ShouldBeTrue(),
               () => _isInsertDetailSaved.ShouldBeTrue(),
               () => _isSubscriberAddKillSaved.ShouldBeTrue());
        }

        [Test]
        public void UpdateAddKillSelection_WhenUserHasNoAccess_ReturnsErrorView()
        {
            // Arrange
            var container = GetAddKillContainer();
            var jsonString = GetAddKillContainerJson(container);
            SetFakesForUpdateAddKillSelection();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, en, ef, ea) => false;

            // Act
            var result = _controller.UpdateAddKillSelection(jsonString);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => 
                {
                    var redirectResult = result.ShouldBeOfType<RedirectToRouteResult>();
                    redirectResult.ShouldSatisfyAllConditions(
                        () => redirectResult.ShouldNotBeNull(),
                        () => redirectResult.RouteValues.ShouldContain(x => x.Value.Equals(RouteValueError)),
                        () => redirectResult.RouteValues.ShouldContain(x => x.Value.Equals(RoutevalueUnAuthorized)));
                });
        }

        private void SetFakesForUpdateAddKillSelection()
        {
            _isBulkActionIDUpdateSaved = false;
            _isSubscriberAddKillSaved = false;
            _isInsertDetailSaved = false;
            ShimCategoryCode.AllInstances.SelectInt32Int32 = (c, cc, cid) => new CategoryCode
            {
                CategoryCodeID = 1,
                CategoryCodeTypeID = 1,
                IsActive = true,
            };

            ShimTransactionCode.AllInstances.SelectTransactionCodeValueInt32 = (t, tid) => new TransactionCode
            {
                TransactionCodeID = 1,
                IsActive = true,
                IsKill = true,
                TransactionCodeTypeID = 1,
                TransactionCodeValue = 1,
            };
            ShimProductSubscription.AllInstances.SelectProductIDInt32ClientConnections = (p, pid, conn) => new List<ActionProductSubscription>
            {
                new ActionProductSubscription
                {
                    PubCategoryID = 1,
                    PubSubscriptionID = 1,
                    PubTransactionID = 1,
                    SubscriptionID = 1
                },
                new ActionProductSubscription
                {
                    PubCategoryID = 2,
                    PubSubscriptionID = 2,
                    PubTransactionID = 2,
                    SubscriptionID = 2
                }
            };
            ShimProductSubscription.AllInstances.SaveBulkActionIDUpdateStringClientConnections = (p, xml, conn) =>
            {
                _isBulkActionIDUpdateSaved = true;
                return _isBulkActionIDUpdateSaved;
            };
            ShimSubscriberAddKill.AllInstances.SaveSubscriberAddKillClientConnections = (s, addkill, conn) =>
            {
                _isSubscriberAddKillSaved = true;
                addkill.AddKillID = 1;
                return addkill.AddKillID;
            };
            ShimSubscriberAddKill.AllInstances.BulkInsertDetailListOfSubscriberAddKillDetailInt32ClientConnections = (s, details, id, conn) =>
            {
                _isInsertDetailSaved = true;
                return true;
            };
        }

        private static string GetAddKillContainerJson(AddKillContainer container)
        {
            var jsonFunctions = new JsonFunctions();
            var addKillContainer = container;
            var jsonString = jsonFunctions.ToJson(addKillContainer);
            return jsonString;
        }

        private static AddKillContainer GetAddKillContainer()
        {
            return new AddKillContainer(
                "1",
                5,
                new FrameworkUAD.Object.FilterMVC
                {
                    BrandID = 1,
                    Count = 5,
                    FilterID = 1,
                    FilterGroupID = 1,
                    FilterNo = 1,
                    SubscriberIDs = new List<int> { 1, 2 },
                },
                new List<int> { 1, 2 },
                1,
                RemoveType)
            {
                Update = true,
                ProductID = 1,
            };
        }
    }
}
