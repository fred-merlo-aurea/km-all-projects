using System;
using System.Diagnostics.CodeAnalysis;
using ActiveUp.WebControls.Fakes;
using ActiveUp.WebControls.Tests.Helper;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    /// <summary>
    /// UT class for <see cref="ToolFind"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ToolFindTest
    {
        private const string ToolFindId = "_toolFind0";
        private const string TestValue = "Test";
        private const string FindOffImage = "find_off.gif";
        private const string FindOverImage = "find_over.gif";
        private const string FindText = "Find";
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
            using (var testObject = new ToolFind(isEmptyId ? string.Empty : TestValue))
            {
                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => testObject.ID.ShouldBe(isEmptyId ? ToolFindId : TestValue),
                    () => testObject.PopupContents.ID.ShouldBe(isEmptyId ? $"{ToolFindId}{nameof(Popup)}" : $"{TestValue}{nameof(Popup)}"),
                    () => TestsHelper.AssertNotFX1(string.Empty, testObject.ImageURL),
                    () => TestsHelper.AssertNotFX1(string.Empty, testObject.OverImageURL),
                    () => TestsHelper.AssertFX1(FindOffImage, testObject.ImageURL),
                    () => TestsHelper.AssertFX1(FindOverImage, testObject.OverImageURL),
                    () => testObject.ToolTip.ShouldBe(FindText),
                    () => usePopupOnClickSet.ShouldBeTrue(),
                    () => testObject.PopupContents.Height.ShouldBe(105),
                    () => testObject.PopupContents.Width.ShouldBe(305),
                    () => testObject.PopupContents.TitleText.ShouldBe(FindText),
                    () => testObject.PopupContents.AutoContent.ShouldBeTrue(),
                    () => testObject.PopupContents.ShowShadow.ShouldBeFalse());
            }
        }
    }
}
