using System.Diagnostics.CodeAnalysis;
using KM.Common.Functions;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HtmlFunctionsTests
    {
        [Test]
        [TestCase(@"   ", " ")]
        [TestCase(@"", "")]
        [TestCase(@"test", "test")]
        [TestCase("tes\n\n\n\nt", "test")]
        [TestCase("tes\n\t\n\tt", "test")]
        [TestCase("<dIV>test</Div>", "\r\rtest")]
        [TestCase("<td>test</td>", "\ttest")]
        [TestCase("<tr>test</ tr>", "\r\rtest")]
        [TestCase("<tr><td>test</td></tr>", "\r\r\ttest")]
        [TestCase("<li>test</li>", "\rtest")]
        [TestCase("<br><br>test", "\r\rtest")]
        [TestCase("<br><br/>test", "\r\rtest")]
        [TestCase("<br><br />test", "\r\rtest")]
        [TestCase("<p>test</p>", "\r\rtest")]
        [TestCase("&nbsp;&#39;", " '")]
        [TestCase("&bull;", " * ")]
        [TestCase("&lsaquo;&rsaquo;", "<>")]
        [TestCase("&trade;&frasl;&copy;&reg;&amp;", "(tm)/(c)(r)&")]
        [TestCase("\n", "")]
        [TestCase("\n\n\n\n", "")]
        [TestCase("\r\t\r", "\r\r")]
        [TestCase("\r\t\t\t\t\t", "\r")]
        [TestCase("\r\t\t\t\t\r", "\r\r")]
        [TestCase("\r        \r", "\r\r")]
        [TestCase("&1;", "&1;")]
        [TestCase("&12;", "")]
        [TestCase("&123;", "")]
        [TestCase("&124;", "")]
        [TestCase("&1245;", "")]
        [TestCase("&12456;", "")]
        [TestCase("&124567;", "")]
        [TestCase("&1245678;", "&1245678;")]
        [TestCase(
            @"<html><head>head_text</head><style>.div1{width:20px;}</style><script>script</script><body>" + 
            @"<div class=""div1"">test<!-- html comments--></div><a href=""www.google.com""></a></body></html>",
            "\r\rtest[URL: <www.google.com> ] ")]
        public void StripTextFromHtml_DifferentInputs_RemovesHtmlFormatting(string input, string expectedResult)
        {
            // Arrange & Act
            var actualResult = HtmlFunctions.StripTextFromHtml(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase(",'\r", ",\r")]
        [TestCase(",',", ",,")]
        [TestCase("\"'\"", "\"\"")]
        [TestCase(
            @"<html><head>head_text</head><style>.div1{width:20px;}</style><script>script</script><body>" +
            @"<div class=""div1"">test<!-- html comments--></div><a href=""www.google.com""></a></body></html>",
            "\r\rtest[URL: <www.google.com> ] ")]
        public void StripTextFromHtmlForExport_DifferentInputs_RemovesHtmlFormatting(string input, string expectedResult)
        {
            // Arrange & Act
            var actualResult = HtmlFunctions.StripTextFromHtmlForExport(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }
    }
}
