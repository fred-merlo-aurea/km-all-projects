using System;
using System.Diagnostics.CodeAnalysis;
using ActiveUp.WebControls.Fakes;
using ActiveUp.WebControls.Tests.Helper;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ToolReplaceTest
    {
        private const string TestValue = "Test";
        private const string ToolReplaceId = "_toolReplace0";
        private const string TitleText = "Find and Replace";
        private const string ReplaceOffImage = "replace_off.gif";
        private const string ReplaceOverImage = "replace_over.gif";
        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanup()
        {
            _context.Dispose();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Init_InitPopupContents_SetsPopupContentsValues(bool isEmptyId)
        {
            // Arrange
            var usePopupOnClickSet = false;
            ShimToolButton.AllInstances.UsePopupOnClickSetBoolean = (obj, value) => usePopupOnClickSet = value;
            Editor.indexTools = 0;

            // Act
            using (var testObject = new ToolReplace(isEmptyId ? string.Empty : TestValue))
            {
                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => testObject.ID.ShouldBe(isEmptyId ? ToolReplaceId : TestValue),
                    () => testObject.PopupContents.ID.ShouldBe(isEmptyId ? ToolReplaceId + nameof(Popup) : TestValue + nameof(Popup)),
                    () => TestsHelper.AssertNotFX1(string.Empty, testObject.ImageURL),
                    () => TestsHelper.AssertNotFX1(string.Empty, testObject.OverImageURL),
                    () => TestsHelper.AssertFX1(ReplaceOffImage, testObject.ImageURL),
                    () => TestsHelper.AssertFX1(ReplaceOverImage, testObject.OverImageURL),
                    () => testObject.ToolTip.ShouldBe(TitleText),
                    () => usePopupOnClickSet.ShouldBeTrue(),
                    () => testObject.PopupContents.Height.ShouldBe(165),
                    () => testObject.PopupContents.Width.ShouldBe(325),
                    () => testObject.PopupContents.TitleText.ShouldBe(TitleText),
                    () => testObject.PopupContents.ShowShadow.ShouldBeFalse());
            }
        }
    }
}