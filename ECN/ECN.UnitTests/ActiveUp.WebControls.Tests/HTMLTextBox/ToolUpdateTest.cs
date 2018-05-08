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
	public class ToolUpdateTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		private Mock<ToolUpdate> _toolUpdate;
		private IDisposable _shimObject;

		[SetUp]
		public void SetUp()
		{
			_toolUpdate = new Mock<ToolUpdate>();

			_toolUpdate.Setup(x => x.Page).Returns(new Page());
			_toolUpdate.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));
			_toolUpdate.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());
			_toolUpdate.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);

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
			_toolUpdate.CallBase = true;
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
			_toolUpdate.Object.RenderDesign(output);

			// Assert
			_toolUpdate.Object.ShouldSatisfyAllConditions(
				() => _toolUpdate.Object.ImageURL.ShouldBe(DummyImageUrl),
				() => _toolUpdate.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var privateObject = new PrivateObject(_toolUpdate.Object, new PrivateType(typeof(ToolUpdate)));

			// Act
			privateObject.Invoke(MethodOnPreRender, new EventArgs());

			// Assert
			_toolUpdate.Object.ShouldSatisfyAllConditions(
					() => _toolUpdate.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => _toolUpdate.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => _toolUpdate.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
		}
	}
}
