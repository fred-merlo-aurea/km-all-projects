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
    public partial class ToolSpellCheckerTest
    {
        private const string ToolSpellCheckerIdText = "_toolSpellChecker0";
        private const string TestValue = "Test";
        private const string SpellOffImage = "spell_off.gif";
        private const string SpellOverImage = "spell_over.gif";
        private const string TitleText = "Spell Checker";
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
            var usePopupOnClickSet = true;
            ShimToolButton.AllInstances.UsePopupOnClickSetBoolean = (obj, value) => usePopupOnClickSet = value;
            Editor.indexTools = 0;

            // Act
            using (var testObject = new ToolSpellChecker(isEmptyId ? string.Empty : TestValue))
            {
                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => testObject.ID.ShouldBe(isEmptyId ? ToolSpellCheckerIdText : TestValue),
                    () => testObject.PopupContents.ID.ShouldBe(isEmptyId ? $"{ToolSpellCheckerIdText}{nameof(Popup)}" : $"{TestValue}{nameof(Popup)}"),
                    () => TestsHelper.AssertNotFX1(string.Empty, testObject.ImageURL),
                    () => TestsHelper.AssertNotFX1(string.Empty, testObject.OverImageURL),
                    () => TestsHelper.AssertFX1(SpellOffImage, testObject.ImageURL),
                    () => TestsHelper.AssertFX1(SpellOverImage, testObject.OverImageURL),
                    () => testObject.ToolTip.ShouldBe(TitleText),
                    () => usePopupOnClickSet.ShouldBeFalse(),
                    () => testObject.PopupContents.Height.ShouldBe(105),
                    () => testObject.PopupContents.Width.ShouldBe(305),
                    () => testObject.PopupContents.TitleText.ShouldBe(TitleText),
                    () => testObject.PopupContents.AutoContent.ShouldBeTrue(),
                    () => testObject.PopupContents.ShowShadow.ShouldBeFalse());
            }
        }
    }
}