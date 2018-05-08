using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActivePager
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PagerBuilderTest
    {
        private PagerBuilder _pagerBuilder;
        private PrivateObject _privateObject;
        private IDisposable _shims;
        private const string navigation = "$PAGESIZE$ $PREVIOUSPAGE$ $FIRSTPAGE$ $PAGESELECTOR$ $PAGEGROUP$ $LASTPAGE$ $NEXTPAGE$";
        private const string pageSizes = "1, 2, 3";

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void BuildNavigation_WhenCurrentPage0_AddControls()
        {
            // Arrange
            var tableCell = new Moq.Mock<TableCell>();
            tableCell.Setup(x => x.Controls).Returns(new ControlCollection(new Control()));
            SetUp("x");

            // Act
            _privateObject.Invoke("BuildNavigation", tableCell.Object);

            // Assert
            tableCell.ShouldSatisfyAllConditions(
                () => tableCell.Object.Controls.Count.ShouldBe(34),
                () => tableCell.Object.ClientID.ShouldBe(null),
                () => tableCell.Object.Page.ShouldBe(null));
        }

        [Test]
        public void BuildNavigation_WhenCurrentPage10_AddControls()
        {
            // Arrange
            var tableCell = new Moq.Mock<TableCell>();
            tableCell.Setup(x => x.Controls).Returns(new ControlCollection(new Control()));
            SetUp("y");

            // Act
            _privateObject.Invoke("BuildNavigation", tableCell.Object);

            // Assert
            tableCell.ShouldSatisfyAllConditions(
                () => tableCell.Object.Controls.Count.ShouldBe(34),
                () => tableCell.Object.ClientID.ShouldBe(null),
                () => tableCell.Object.Page.ShouldBe(null));
        }

        protected void SetUp(String param)
        {
            _pagerBuilder = new PagerBuilder();
            _privateObject = new PrivateObject(_pagerBuilder);
            var bag = new StateBag();
            bag.Add("_pageSizes", pageSizes);
            _shims = ShimsContext.Create();
            ShimPagerBuilder.AllInstances.RecordCountGet = (x) => 0;
            ShimPagedControl.AllInstances.NavigationTemplateGet = (x) => navigation;
            ShimControl.AllInstances.ViewStateGet = (x) => bag;
            ShimPagerBuilder.AllInstances.CurrentIndexGet = (x) => 1;
            if (param.Equals("x"))
            {
                ShimPagerBuilder.AllInstances.CurrentPageGet = (x) => 0;
            }
            else if (param.Equals("y"))
            {
                ShimPagerBuilder.AllInstances.CurrentPageGet = (x) => 5;
            }

            ShimPagerBuilder.AllInstances.PageCountGet = (x) => 11;
            ShimPagerBuilder.AllInstances.CurrentGroupGet = (x) => 2;
            ShimPagerBuilder.AllInstances.RecordCountGet = (x) => 1;
            ShimPagerBuilder.AllInstances.PageSizeGet = (x) => 0;
            ShimPagedControl.AllInstances.PageSelectorEnabledGet = (x) => true;
        }
    }
}
