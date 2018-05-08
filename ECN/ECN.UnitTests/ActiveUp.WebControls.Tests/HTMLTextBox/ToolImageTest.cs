using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ToolImageTest
    {
        private ToolImage _toolImage;
        private PrivateObject _privateObject;
        private const string EmptyString = "";
        private const string ResultToolTip = "Image";
        private const string Id = "2";
        private const string MethodName = "_Init";

        [SetUp]
        public void SetUp()
        {
            _toolImage = new ToolImage();
            _privateObject = new PrivateObject(_toolImage);
        }

        [Test]
        public void Init_ForId_ShouldSetValuesInToolImage()
        {
            // Act
            _privateObject.Invoke(MethodName, Id);

            //Assert
            _toolImage.ShouldSatisfyAllConditions
                (
                    () => _toolImage.ImageURL.ShouldBe(EmptyString),
                    () => _toolImage.OverImageURL.ShouldBe(EmptyString),
                    () => _toolImage.ToolTip.ShouldBe(ResultToolTip),
                    () => _toolImage.ID.ShouldBe(Id)
                );
        }

        [Test]
        public void Init_ForNullId_ShouldSetValuesInToolImage()
        {
            // Act
            _privateObject.Invoke(MethodName, EmptyString);

            //Assert
            _toolImage.ShouldSatisfyAllConditions
                (
                    () => _toolImage.ImageURL.ShouldBe(EmptyString),
                    () => _toolImage.OverImageURL.ShouldBe(EmptyString),
                    () => _toolImage.ToolTip.ShouldBe(ResultToolTip)
                );
        }
    }
}
