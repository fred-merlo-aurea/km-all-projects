using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using UAD.Web.Admin.Infrastructure;
using UAD.Web.Admin.Infrastructure.Fakes;

namespace UAD.Web.Admin.Tests.Infrastructure
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HtmlHelperMethodsTest
    {
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [TestCase("ActionMenu_Product", "Product", "#=PubID#", "Id=#=PubID#;$('\\#deleteModal').modal('show');")]
        [TestCase("ActionMenu_MasterGroup", "MasterGroup", "#=MasterGroupID#", "Id=#=MasterGroupID#;$('\\#deleteModal').modal('show');")]
        [TestCase("ActionMenu_ProductType", "ProductType", "#=PubTypeID#", "Id=#=PubTypeID#;$('\\#deleteModal').modal('show');")]
        [TestCase("ActionMenu_ResponseGroup", "ResponseGroup", "#=ResponseGroupID#", "Id=#=ResponseGroupID#,PubID=#=PubID#;$('\\#deleteModal').modal('show');")]
        [TestCase("ActionMenu_PubCustomField", "PubCustomField", "#=PubSubscriptionsExtensionMapperId#", "Id=#=PubSubscriptionsExtensionMapperId#,PubID=#=PubID#;$('\\#deleteModal').modal('show');")]
        [TestCase("ActionMenu_Adhoc", "Adhoc", "#=SubscriptionsExtensionMapperID#", "Id=#=SubscriptionsExtensionMapperID#;$('\\#deleteModal').modal('show');")]
        public void HtmlHelperMethods_ValidHtmlHelper_AddsTwoActions(
            string methodName, 
            string expectedControllerName,
            string expectedIdName, 
            string expectedOnClickValue)
        {
            // Arrange
            ShimActionMenu.ToKendoGridClientTemplateMenuBuilder = (_) => { return null; } ;
            IList<ActionType> actions = null;
            var idName = string.Empty;
            ShimActionMenu.ActionMenuFactoryHtmlHelperStringIEnumerableOfActionType =
                (_, idNameParameter, actionsParameter) => 
                {
                    idName = idNameParameter;
                    actions = actionsParameter.ToList();
                    return null;
                };
            var helper = new HtmlHelper(new ViewContext(), new StubIViewDataContainer());

            // Act
            var methodInfo = ReflectionHelper
                .GetAllMethods(typeof(HtmlHelperMethods))
                .Where(x => x.Name == methodName).FirstOrDefault();
            methodInfo?.Invoke(helper, new object[] { helper });

            // Assert
            actions.ShouldSatisfyAllConditions(
                () => idName.ShouldBe(expectedIdName),
                () => actions.ShouldNotBeNull(),
                () => actions.Count.ShouldBe(2),
                () => actions[0].Text.ShouldBe("Edit"),
                () => actions[0].Action.ShouldNotBeNull(),
                () => actions[0].Action.ControllerName.ShouldBe(expectedControllerName),
                () => actions[0].HtmlAttributes.ShouldBeNull(),
                () => actions[1].Text.ShouldBe("Delete"),
                () => actions[1].HtmlAttributes.ShouldNotBeNull(),
                () => actions[1].HtmlAttributes.ToString().ShouldBe($"{{ onclick = {expectedOnClickValue } }}"));
        }
    }
}
