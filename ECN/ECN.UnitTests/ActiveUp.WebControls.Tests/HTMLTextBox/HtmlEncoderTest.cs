using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HtmlEncoderTest
    {
        private const string DummyString = "DummyString";

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void Encode_OnEmptyString_ReturnEmptyString(string input)
        {
            // Arrange
            var output = new StringBuilder();

            // Act	
            HtmlEncoder.Encode(input, output, new ArrayList());
            var actualResult = output.ToString();

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void Encode_OnNonEmptyString_ReturnString()
        {
            // Arrange
            var output = new StringBuilder();
            const string DivTag = "<div>";
            const string HtmlTag = "<html>";
            const string CodeTag = "<code>";
            const string SpanTag = "<span";
            const string Comment = "<!-- CommentedCode -->";
            const string MacroTag = "<% response.body %>";
            var input = new StringBuilder();
            input.Append(HtmlTag);
            input.Append("<head> ");
            input.Append("<title><% response.title %></title>");
            input.Append("</head>");
            input.Append("<body>&#33; &#x0002E;");
            input.Append("<button/>\n\n\n\n\n\n\n\n");
            input.Append($"{SpanTag} id=\'{DummyString}\'> <> &\\ \'Test </span>");
            input.Append("<noscript>NoScriptText<noscript>");
            input.Append(MacroTag);
            input.Append($"{DivTag}<label id=\"TestLabel\" Text = \"TestString\" >&nbsp;</label></div>&nbsp;");
            input.Append(Comment);
            input.Append($"{CodeTag}\\'CodeSnippet</code>");
            input.Append("\\<code>CodeSnippet</code>");
            input.Append("<pre>CodeSnippet</pre>\\n");
            input.Append("<br/>");
            input.Append("</body>");
            input.Append("</html>");

            // Act	
            HtmlEncoder.Encode(input.ToString(), output, null);
            var actualResult = output.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(HtmlTag),
                () => actualResult.ShouldContain(DummyString),
                () => actualResult.ShouldContain(SpanTag),
                () => actualResult.ShouldContain(DivTag),
                () => actualResult.ShouldContain(Comment),
                () => actualResult.ShouldContain(MacroTag)
            );
        }

        [Test]
        public void Encode_AnotherHtmlTags_ReturnString()
        {
            // Arrange
            var output = new StringBuilder();
            const string EndTagWithNoOpenTag = "</head>";
            const string DivTag = "<div";
            const string HtmlTag = "<html>";
            const string HtmlNotMatched = "htmlNotMatched";
            var input = new StringBuilder();
            input.Append($"{HtmlTag}<br/>& #");
            input.Append($"{EndTagWithNoOpenTag}");
            input.Append("<title><%  \\' response.title %></title>");
            input.Append("<%  \' \'0 \\n response.title %>");
            input.Append("<div id=\\' \\'");
            input.Append("/>");
            input.Append($"{(char)130}");
            input.Append($"{(char)330}");
            input.Append($"{DivTag} id=\\'{DummyString}\\'");
            input.Append("\\>\\n");
            input.Append("</div>\n\n\n\n\n\n\n\n ");
            input.Append($"</{HtmlNotMatched}>");

            // Act	
            HtmlEncoder.Encode(input.ToString(), output, null);
            var actualResult = output.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(HtmlTag),
                () => actualResult.ShouldContain(DummyString),
                () => actualResult.ShouldNotContain(EndTagWithNoOpenTag),
                () => actualResult.ShouldContain(DivTag),
                () => actualResult.ShouldContain($"&lt;/{HtmlNotMatched}&gt;")
            );
        }
    }
}
