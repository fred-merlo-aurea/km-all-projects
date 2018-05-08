using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.QualityTools.Testing.Fakes;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HtmlParserTest
    {
        private const string MethodGetTokens = "GetTokens";
        private const string MethodRemoveComments = "RemoveComments";
        private const string MethodRemoveSGMLComments = "RemoveSGMLComments";
        private HtmlParser _htmlParser;
        private MSTest::PrivateObject _privateObject;

        [SetUp]
        public void SetUp()
        {
            _htmlParser = new HtmlParser();
            _privateObject = new MSTest::PrivateObject(_htmlParser);
        }

        [Test]
        public void GetTokens_EmptyInput_ReturnEmptyCollection()
        {
            // Arrange
            var input = string.Empty;

            // Act	
            var actualResult = _privateObject.Invoke(MethodGetTokens, new object[] { input }) as StringCollection;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(0)
                );
        }

        [Test]
        public void GetTokens_ValidInput_ReturnStringCollection()
        {
            // Arrange
            var input = new StringBuilder();
            input.Append("</ div > <p id=\'testId\' \r\n\t/> <div> < p />");
            input.Append("<label id=\"lablelId\" name = \'TestName\' > TestValue</label></div>");
            const string StartTag = "<";
            var expectedTokensCount = 36;

            // Act	
            var actualResult = _privateObject.Invoke(MethodGetTokens, new object[] { input.ToString() }) as StringCollection;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedTokensCount),
                () => actualResult.Contains(StartTag).ShouldBeTrue());
        }

        [Test]
        public void GetTokens_NoTags_ReturnStringCollection()
        {
            // Arrange
            const string input = "test text";

            // Act	
            var actualResult = _privateObject.Invoke(MethodGetTokens, new object[] { input.ToString() }) as StringCollection;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(1),
                () => actualResult.Contains(input).ShouldBeTrue()
                );
        }

        [Test]
        public void GetTokens_AttributeValue_ReturnStringCollection()
        {
            // Arrange
            const string input = "<div id=TesId /> <div id=TesId ></div>";
            const string DivName = "div";
            // Act	
            var actualResult = _privateObject.Invoke(MethodGetTokens, new object[] { input.ToString() }) as StringCollection;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(16),
                () => actualResult.Contains(DivName).ShouldBeTrue(),
                () => actualResult.
                        Cast<string>().ToList().
                        Count(x => x.Equals(DivName)).
                        ShouldBe(3)
                );
        }

        [Test]
        public void GetTokens_AttributeWithoutQuotes_ReturnStringCollection()
        {
            // Arrange
            const string input = "<div id=/TesId />";
            const string DivName = "div";
            // Act	
            var actualResult = _privateObject.Invoke(MethodGetTokens, new object[] { input.ToString() }) as StringCollection;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(8),
                () => actualResult.Contains(DivName).ShouldBeTrue()
                );
        }

        [TestCase("test")]
        [TestCase("<div = />")]
        [TestCase("<div in />")]
        [TestCase("<div >")]
        public void Parse_ForTags_ReturnsNode(String html)
        {
            //Act
            var node = _htmlParser.Parse(html);

            //Assert
            node.ShouldSatisfyAllConditions(
                    () => node.Count.ShouldBe(1),
                    () => node.Capacity.ShouldBe(4)
                );
        }

        [TestCase(">")]
        [TestCase("</div in >")]
        public void Parse_ForFullTags_ReturnsNode(String html)
        {
            //Act
            var node = _htmlParser.Parse(html);

            //Assert
            node.ShouldSatisfyAllConditions(
                    () => node.Count.ShouldBe(0),
                    () => node.Capacity.ShouldBe(0)
                );
        }
        
        [Test]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase("test", "test")]
        [TestCase("test <!-- comment -->", "test ")]
        [TestCase("test <a href=\"url\">", "test <a href=\"url\">")]
        [TestCase("test <a href='url'>", "test <a href='url'>")]
        public void RemoveComments_DifferentInputs_ReturnExpectedString(string inputString, string expectedValue)
        {
            // Arrange & Act	
            var actualResult = _privateObject.Invoke(MethodRemoveComments, inputString) as string;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedValue));
        }

        [Test]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase("test", "test")]
        [TestCase("test <! comment >", "test ")]
        [TestCase("test <a href=\"url\">", "test <a href=\"url\">")]
        [TestCase("test <a href='url'>", "test <a href='url'>")]
        public void RemoveSGMLComments_DifferentInputs_ReturnExpectedString(string inputString, string expectedValue)
        {
            // Arrange & Act	
            var actualResult = _privateObject.Invoke(MethodRemoveSGMLComments, inputString) as string;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedValue));
        }

        [Test]
        [TestCase("<div>   </div>", "<div></div>")]
        [TestCase("<div>test</div>", "<div>test</div>")]
        public void Parse_RemoveEmptyElementText_ReturnsValidNodeCollection(string input, string expectedHtml)
        {
            // Arrange 
            _htmlParser.RemoveEmptyElementText = true;
            var expectedNumberOfNodes = 1;

            // Act
            var actualResult = _htmlParser.Parse(input);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedNumberOfNodes),
                () => actualResult[0].HTML.ShouldBe(expectedHtml));
        }
    }
}
