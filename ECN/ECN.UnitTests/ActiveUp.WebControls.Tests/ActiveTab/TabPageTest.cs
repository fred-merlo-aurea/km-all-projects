using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using ActiveUp.WebControls.Tests.Utils;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveInput
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class TabPageTest : StateManagerTestBase<TabPage>
	{
		[Test]
		public void AddParsedSubObject_InvokeMethod_ExpectActionInvokedWithParameter()
		{
			// Arrange
			var testObject = new TabPage();

			// Act
			(testObject as IParserAccessor).AddParsedSubObject(new LiteralControl { Text = TestValue1 });

			// Assert
			testObject.Text.ShouldBe(TestValue1);
		}
	}
}