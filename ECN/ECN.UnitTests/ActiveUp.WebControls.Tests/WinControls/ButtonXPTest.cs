using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Drawing.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ActiveUp.WebControls.WinControls;
using ECN.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ActiveUp.WebControls.WinControls.ButtonXP;

namespace ActiveUp.WebControls.Tests.WinControls
{
    [TestFixture]
    public class ButtonXPTest
    {
        private IDisposable _shimObject;
        private PaintEventArgs _paintEventArgs;
        private ButtonXP _buttonXP;
        private PrivateObject _privateObj;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();

            _buttonXP = new ButtonXP();
            _buttonXP.Width = 20;
            _buttonXP.Height = 20;
            var rects0 = new Rectangle[] { new Rectangle(0, 0, 10, 10) };
            var rects1 = new Rectangle[] { new Rectangle(0, 0, 10, 10) };
            _privateObj = new PrivateObject(_buttonXP);
            _privateObj.SetField("rects0", rects0);
            _privateObj.SetField("rects1", rects1);
            var rect = new Rectangle(0, 0, 10, 10);
            _paintEventArgs = new PaintEventArgs(_buttonXP.CreateGraphics(), rect);
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void OnPaint_OnValidCall_FillRectangles()
        {
            // Arrange
            var rectFilled = false;
            ShimGraphics.AllInstances.FillRectangleBrushRectangle = (obj, brsh, rect) =>
            {
                rectFilled = true;
            };

            // Act
            _privateObj.Invoke("OnPaint", new object[] { _paintEventArgs });

            // Assert
            rectFilled.ShouldBeTrue();
        }

        [Test]
        public void OnPaint_ImageNotNull_FillRectangles()
        {
            // Arrange
            var rectFilled = false;
            _privateObj.SetField("image", new Bitmap(10, 10));
            ShimGraphics.AllInstances.FillRectangleBrushRectangle = (obj, brsh, rect) =>
            {
                rectFilled = true;
            };

            // Act
            _privateObj.Invoke("OnPaint", new object[] { _paintEventArgs });

            // Assert
            rectFilled.ShouldBeTrue();
        }

        [Test]
        public void OnPaint_ImageNotNullAndTextNotEmpty_FillRectangles()
        {
            // Arrange
            var rectFilled = false;
            _privateObj.SetField("image", new Bitmap(10, 10));
            _buttonXP.Text = "TestString";
            ShimGraphics.AllInstances.FillRectangleBrushRectangle = (obj, brsh, rect) =>
            {
                rectFilled = true;
            };

            // Act
            _privateObj.Invoke("OnPaint", new object[] { _paintEventArgs });

            // Assert
            rectFilled.ShouldBeTrue();
        }

        [Test]
        [TestCase(Schemes.Blue)]
        [TestCase(Schemes.Silver)]
        [TestCase(Schemes.OliveGreen)]
        public void OnPaint_EnabledIsFalse_DrawString(Schemes Scheme)
        {
            // Arrange
            var rectFilled = false;
            _buttonXP.Enabled = false;
            _buttonXP.Scheme = Scheme;
            _privateObj.SetField("image", new Bitmap(10, 10));
            ShimGraphics.AllInstances.DrawStringStringFontBrushPointF = (obj, txt, fnt, brsh, pnt) =>
            {
                rectFilled = true;
            };

            // Act
            _privateObj.Invoke("OnPaint", new object[] { _paintEventArgs });

            // Assert
            rectFilled.ShouldBeTrue();
        }

        [Test]
        [TestCase(Schemes.Blue)]
        [TestCase(Schemes.Silver)]
        [TestCase(Schemes.OliveGreen)]
        public void OnPaint_SchemeSwitch_DrawString(Schemes Scheme)
        {
            // Arrange
            var rectFilled = false;
            _buttonXP.Enabled = true;
            _buttonXP.Scheme = Scheme;
            _privateObj.SetField("image", new Bitmap(10, 10));
            ShimGraphics.AllInstances.FillRectangleBrushRectangle = (obj, brsh, rect) =>
            {
                rectFilled = true;
            };

            // Act
            _privateObj.Invoke("OnPaint", new object[] { _paintEventArgs });

            // Assert
            rectFilled.ShouldBeTrue();
        }

        [Test]
        [TestCase(States.MouseOver, Schemes.Blue)]
        [TestCase(States.MouseOver, Schemes.Silver)]
        [TestCase(States.MouseOver, Schemes.OliveGreen)]
        [TestCase(States.Normal, Schemes.Blue)]
        [TestCase(States.Normal, Schemes.Silver)]
        [TestCase(States.Normal, Schemes.OliveGreen)]
        [TestCase(States.Pushed, Schemes.Blue)]
        [TestCase(States.Pushed, Schemes.Silver)]
        [TestCase(States.Pushed, Schemes.OliveGreen)]
        public void OnPaint_StateSwitch_DrawString(States state, Schemes Scheme)
        {
            // Arrange
            var rectFilled = false;
            _buttonXP.Enabled = true;
            _buttonXP.Scheme = Scheme;
            _privateObj.SetField("selected", true);
            _privateObj.SetField("state", state);
            _privateObj.SetField("image", new Bitmap(10, 10));
            ShimGraphics.AllInstances.FillRectangleBrushRectangle = (obj, brsh, rect) =>
            {
                rectFilled = true;
            };

            // Act
            _privateObj.Invoke("OnPaint", new object[] { _paintEventArgs });

            // Assert
            rectFilled.ShouldBeTrue();
        }
    }
}
