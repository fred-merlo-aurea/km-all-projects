using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActivePanel
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PanelTest
    {
        private Panel _panel;
        private PrivateObject _privateObject;
        private IDisposable _shims;

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void RenderPanel_ForPanelStateCollapsed_shouldRender()
        {
            // Arrange
            var htmlTextWriter = SetUp("collapsed");

            // Act
            _privateObject.Invoke("RenderPanel", htmlTextWriter);

            // Assert
            htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBe(null));
        }

        [Test]
        public void RenderPanel_ForPanelStateExpanded_shouldRender()
        {
            // Arrange
            var htmlTextWriter = SetUp("expanded");

            // Act
            _privateObject.Invoke("RenderPanel", htmlTextWriter);

            // Assert
            htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBe(null));
        }

        protected HtmlTextWriter SetUp(String param)
        {
            _panel = new Panel();
            _privateObject = new PrivateObject(_panel);
            var textWriter = new Moq.Mock<TextWriter>();
            var htmlTextWriter = new HtmlTextWriter(textWriter.Object);
            _shims = ShimsContext.Create();
            ShimWebControl.AllInstances.HeightGet = (x) => 100;
            ShimWebControl.AllInstances.BackColorGet = (x) => Color.AliceBlue;
            if (param.Equals("collapsed"))
            {
                Fakes.ShimPanel.AllInstances.StateGet = (x) => PanelState.Collapsed;
            }
            else
            {
                Fakes.ShimPanel.AllInstances.StateGet = (x) => PanelState.Expanded;
            }
            Fakes.ShimPanel.AllInstances.TitleBackColorCollapsedGet = (x) => Color.AntiqueWhite;
            Fakes.ShimPanel.AllInstances.OnTitleClickClientSideGet = (x) => "click";
            Fakes.ShimPanel.AllInstances.BackgroundImageGet = (x) => "image.com";
            Fakes.ShimPanel.AllInstances.ScrollEffectGet = (x) => true;
            return htmlTextWriter;
        }
    }
}
