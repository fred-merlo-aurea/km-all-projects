using System;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Windows.Media;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class XmlFunctionsTest
    {
        [Test]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase(" \0  ")]
        public void EscapeXmlString_TrimmableStringIn_EmptyStringReturned(string inputString)
        {
            // Act
            var resultString = XMLFunctions.EscapeXmlString(inputString);

            // Assert
            resultString.ShouldBe(string.Empty);
        }

        [Test]
        public void EscapeXmlString_DirtyStringIn_EscapedStringReturned()
        {
            // Arrange
            var dirtyString = " &Why 0<2 is <true/>\"MySuperQuote\" 'test' ";

            // Act
            var escapedString = XMLFunctions.EscapeXmlString(dirtyString);

            // Assert
            escapedString.ShouldBe("&amp;Why 0&lt;2 is &lt;true/&gt;&quot;MySuperQuote&quot; test");
        }
    }
}
