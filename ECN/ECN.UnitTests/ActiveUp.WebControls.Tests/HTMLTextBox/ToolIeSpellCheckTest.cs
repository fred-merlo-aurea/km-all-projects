using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
	[TestFixture]
	public class ToolIeSpellCheckTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		private Mock<ToolIeSpellCheck> _toolIeSpellCheck;
		private IDisposable _shimObject;

		[SetUp]
		public void SetUp()
		{
			_toolIeSpellCheck = new Mock<ToolIeSpellCheck>();

			_toolIeSpellCheck.Setup(x => x.Page).Returns(new Page());
			_toolIeSpellCheck.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));
			_toolIeSpellCheck.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());
			_toolIeSpellCheck.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);

			_shimObject = ShimsContext.Create();

			var count = 0;
			ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (type, obj1, obj2) =>
			{
				if (count == 0)
				{
					count++;
					return DummyImageUrl;
				}
				else
				{
					return DummyOverImageUrl;
				}
			};
			_toolIeSpellCheck.CallBase = true;
		}

		[TearDown]
		public void TearDown()
		{
			_shimObject?.Dispose();
		}

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var output = new HtmlTextWriter(new StringWriter());

			// Act
			_toolIeSpellCheck.Object.RenderDesign(output);

			// Assert
			_toolIeSpellCheck.Object.ShouldSatisfyAllConditions(
				() => _toolIeSpellCheck.Object.ImageURL.ShouldBe(DummyImageUrl),
				() => _toolIeSpellCheck.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var privateObject = new PrivateObject(_toolIeSpellCheck.Object, new PrivateType(typeof(ToolIeSpellCheck)));

			// Act
			privateObject.Invoke(MethodOnPreRender, new EventArgs());

			// Assert
			_toolIeSpellCheck.Object.ShouldSatisfyAllConditions(
					() => _toolIeSpellCheck.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => _toolIeSpellCheck.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => _toolIeSpellCheck.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
		}
	}
}
