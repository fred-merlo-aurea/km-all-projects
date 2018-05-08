using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveInput
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class MaskedTextBoxTest
	{
		private const string DelimiterSymbolDefault = ",";
		private const string PercentageSymbol = "%";
		private const string DollarSymbol = "$";
		private const string TestSymbol = "@";

		[Test]
		public void Delimiter_DefaultValue()
		{
			// Arrange
			using (var testObject = new MaskedTextBox())
			{
				// Act, Assert
				testObject.Delimiter.ShouldBe(DelimiterSymbolDefault);
			}
		}

		[Test]
		public void Delimiter_SetAndGetValue()
		{
			// Arrange
			using (var testObject = new MaskedTextBox())
			{
				// Act
				testObject.Delimiter = TestSymbol;

				// Assert
				testObject.Delimiter.ShouldBe(TestSymbol);
			}
		}

		[Test]
		public void Symbol_DefaultValue_InputModePercent()
		{
			// Arrange
			using (var testObject = new MaskedTextBox { Mode = InputMode.NotSet })
			{
				// Act, Assert
				testObject.Symbol.ShouldBe(DollarSymbol);
			}
		}

		[Test]
		public void Symbol_DefaultValue_InputModeOthers()
		{
			// Arrange
			using (var testObject = new MaskedTextBox { Mode = InputMode.Percent })
			{
				// Act, Assert
				testObject.Symbol.ShouldBe(PercentageSymbol);
			}
		}

		[Test]
		public void Symbol_SetAndGetValue()
		{
			// Arrange
			using (var testObject = new MaskedTextBox())
			{
				// Act
				testObject.Symbol = TestSymbol;

				// Assert
				testObject.Symbol.ShouldBe(TestSymbol);
			}
		}
	}
}
