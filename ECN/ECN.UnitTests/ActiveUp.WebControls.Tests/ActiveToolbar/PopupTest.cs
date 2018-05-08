using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
	[TestFixture]
	public class PopupTest
	{
		private const string DummyCssClass = "dummycssclass";
		private const string DummyFontName = "calibri";

		public static object[] PropertyNameWithExpectedValue =
		{
			new object[] { "CssClass", DummyCssClass},
			new object[] { "ForeColor", Color.Black},
			new object[] { "Enabled", true}
		};

		[TestCaseSource("PropertyNameWithExpectedValue")]
		public void Property_SetAndGetValue_ReturnsTheSetValue(string propertyName, object expected)
		{
			// Arrange
			var popup = new Popup();
			var privateObject = new PrivateObject(popup);

			// Act
			privateObject.SetFieldOrProperty(propertyName, expected);

			// Assert
			privateObject.GetFieldOrProperty(propertyName).ShouldBe(expected);
		}

		[Test]
		public void Font_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange
			var popup = new Popup();

			// Act
			popup.Font.Name = DummyFontName;

			// Assert
			popup.Font.Name.ShouldBe(DummyFontName);
		}
	}
}