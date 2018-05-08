using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveInput
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class NumericTextBoxTest
	{
		private const string TestSymbol = "@";
		private const string DelimiterSymbolDefault = ",";

		[Test]
		public void Delimiter_DefaultValue()
		{
			// Arrange
			using (var testObject = new NumericTextBox())
			{
				// Act, Assert
				testObject.Delimiter.ShouldBe(DelimiterSymbolDefault);
			}
		}

		[Test]
		public void Delimiter_SetAndGetValue()
		{
			// Arrange
			using (var testObject = new NumericTextBox())
			{
				// Act
				testObject.Delimiter = TestSymbol;

				// Assert
				testObject.Delimiter.ShouldBe(TestSymbol);
			}
		}

		[Test]
		public void CurrencySymbol_DefaultValue()
		{
			// Arrange
			using (var testObject = new NumericTextBox ())
			{
				// Act, Assert
				testObject.CurrencySymbol.ShouldBeEmpty();
			}
		}

		[Test]
		public void CurrencySymbol_SetAndGetValue()
		{
			// Arrange
			using (var testObject = new NumericTextBox())
			{
				// Act
				testObject.CurrencySymbol = TestSymbol;

				// Assert
				testObject.CurrencySymbol.ShouldBe(TestSymbol);
			}
		}
	}
}
