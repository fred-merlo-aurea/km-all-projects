using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using ActiveUp.WebControls.Tests.Utils;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Common.Tests
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class StateManagerImplTest : StateManagerTestBase<StateManagerImpl>
	{
		[Test]
		public void AddParsedSubObject_InvokeMethod_ExpectActionInvokedWithParameter()
		{
			// Arrange
			var testObject = new StateManagerImpl();
			var testValue = string.Empty;

			// Act
			testObject.AddParsedSubObject(
				new LiteralControl { Text = TestValue1 }, 
				(input) => { testValue = input; });

			// Assert
			testValue.ShouldBe(TestValue1);
		}
	}
}

