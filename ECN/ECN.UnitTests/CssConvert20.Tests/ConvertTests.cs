using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CssConvert.CssParser;
using HtmlAgilityPack;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace CssConvert20.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ConvertTests
    {
        private IDisposable _shimObject;
        public object[] _parameters = new object[] { };
        private PrivateObject _cssListBuilder;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _cssListBuilder = new PrivateObject(new CssConvert_2_0.Convert());
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("empty", "empty")]
        [TestCase("<HTML></html>", "<HTML></html>")]
        [TestCase("<HTML><stylewrongtag</style></html>", "<HTML><stylewrongtag</style></html>")]
        [TestCase("<html>" +
                "<style>" +
                ".widthStyle1 { width: 10px; }" +
                ".widthStyle2:a { width: 20px; }" +
                ".widthStyle3 { width: 30px; }" +
                "#divId:a { color: red; }" +
                "</style>" +
                "<body>" +
                "<SPAN id=\"1\" class=\"widthStyle1\">text1</SPAN>" +
                "<DIV id=\"divId\">" +
                "<a href=\"http://www.google.com/\" class=\"widthStyle2\" style=\"width: 2px;\">link1</a>" +
                "<img src=\"http://www.google.com/IMG1.jpg\" class=\"widthStyle3\" style=\"width: 3px;\" />" +
                "</div></body></html>",
                "<html><body>" +
                "<span id=\"1\" class=\"widthstyle1\">text1</span>" +
                "<div id=\"divid\" style=\":a {color: red;}\">" +
                "<a href=\"http://www.google.com/\" class=\"widthstyle2\" style=\"width: 2px;:a {width: 20px;}\">link1</a>" +
                "<img src=\"http://www.google.com/IMG1.jpg\" class=\"widthstyle3\" style=\"width: 3px;width: 30px;\">" +
                "</div></body></html>")]
        public void InlineCss_ValidHtmlElement_ParsesCss(string htmlString, string expectedResult)
        {
            // Arrange
            var converter = new CssConvert_2_0.Convert();

            // Act
            var html = converter.InlineCss(new StringBuilder(htmlString));
            var actualResult = html.ToString();

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void BuildChildList_Test()
        {
            // Arrange
            var type = HtmlNodeType.Document;
            var document = new HtmlDocument();
            const int index = 0;
            var parent = new HtmlNode(type, document, index);

            var child = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "html"
            };
            parent.AppendChild(child);

            child = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "body"
            };
            parent.AppendChild(child);

            child = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "div"
            };
            parent.AppendChild(child);

            child = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "ul"
            };
            var newChild = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "li"
            };
            child.AppendChild(newChild);
            parent.AppendChild(child);

            var childToFind = "ul";
            var returnList = new List<HtmlNode>();
            _parameters = new object[] { parent, childToFind, returnList };

            var parser = new PrivateObject(new CssConvert_2_0.Convert());

            // Act
            var output = parser.Invoke("BuildChildList", _parameters) as List<HtmlNode>;

            // Assert
            Assert.AreEqual("ul", output[0].Name);
        }

        [Test]
        public void AppendDeclarations_Test()
        {
            var declaration = new Declaration
            {
                Name = "h1",
                Expression = null,
                Important = true
            };

            var declarations = new List<Declaration>
            {
                declaration
            };

            StringBuilder builder = null;
            string separator = " ";

            _parameters = new object[] { declarations, builder, separator };

            // Act            
            var testDelegate = new TestDelegate(InvokeMethod);

            // Assert
            Assert.Throws(typeof(System.Exception), testDelegate);

            // Arrange
            declarations = null;
            _parameters = new object[] { declarations, builder, separator };

            // Act
            testDelegate = new TestDelegate(InvokeMethod);

            //Assert
            Assert.DoesNotThrow(testDelegate);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("li:hover", "li")]
        public void GetCleanedUpSelector_Test(string selectorName, string expectedOutput)
        {
            // Arrange
            var parser = new PrivateObject(new CssConvert_2_0.Convert());
            _parameters = new object[] { selectorName, null };

            // Act
            var output = parser.Invoke("GetCleanedUpSelector", _parameters) as string;

            // Assert
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        [TestCase("<h1><style color:red;></style></h1>", "<h1></h1>")]
        public void GetHtmlWithoutStyleSection_Test(string selectorName, string expectedOutput)
        {
            // Arrange
            var parser = new PrivateObject(new CssConvert_2_0.Convert());

            // Act
            var output = parser.Invoke("GetHtmlWithoutStyleSection", selectorName) as string;

            // Assert
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void IdentifyNodesMatchingSelector_Test()
        {
            // Arrange
            var htmlDocument = new HtmlDocument();
            htmlDocument.DocumentNode.Name = "document";

            var document = new HtmlDocument();

            var child = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "ul"
            };
            htmlDocument.DocumentNode.AppendChild(child);

            var selectorName = ".class ul";
            var selectSyntax = "ul";
            var stringSelectorStart = ".";
            var removeEmptySpace = true;
            System.Action<HtmlNode> processNode = null;

            _parameters = new object[] { selectorName, selectSyntax, stringSelectorStart, htmlDocument, processNode, removeEmptySpace };

            var parser = new PrivateObject(new CssConvert_2_0.Convert());

            // Act
            var output = parser.Invoke("IdentifyNodesMatchingSelector", _parameters) as bool?;

            // Assert
            Assert.IsTrue(output);

            // Arrange
            stringSelectorStart = "#";
            _parameters = new object[] { selectorName, selectSyntax, stringSelectorStart, htmlDocument, processNode, removeEmptySpace };

            // Act
            output = parser.Invoke("IdentifyNodesMatchingSelector", _parameters) as bool?;

            // Assert
            Assert.IsFalse(output);
        }

        [Test]
        public void NodeHasAncestors_Test()
        {
            // Arrange
            var document = new HtmlDocument();
            document.DocumentNode.Name = "document";
            var node = new HtmlNode(HtmlNodeType.Element, document, 1);
            node.Name = "ul";

            var cssDocument = new CSSDocument();
            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();

            var className = "h1Class";
            var elementName = "h1";
            var name = new StringBuilder();
            testInputItems.ClassName = name.Append(className)
                .Append(" ")
                .Append(elementName)
                .ToString();
            testInputItems.PseudoName = "hover";
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null; // Option of GetDirectives();

            var methodCaller = new PrivateObject(new CssConvert_2_0.Convert());

            _parameters = new object[] { node, cssDocument };

            // Act
            var result = methodCaller.Invoke("NodeHasAncestors", _parameters) as bool?;

            // Assert
            Assert.IsFalse(result);

            // Arrange

            node = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "ul"
            };
            var node_ancestor = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "div"
            };
            node_ancestor.AppendChild(node);

            _parameters = new object[] { node, cssDocument };

            // Act
            result = methodCaller.Invoke("NodeHasAncestors", _parameters) as bool?;

            // Assert
            Assert.IsFalse(result);

            // Arrange
            node = new HtmlNode(HtmlNodeType.Element, document, 1);
            node.Name = "ul";
            node_ancestor = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "div"
            };
            node_ancestor.AppendChild(node);

            var ancestor = new HtmlNode(HtmlNodeType.Element, document, 1)
            {
                Name = "body"
            };
            ancestor.AppendChild(node_ancestor);

            _parameters = new object[] { node, cssDocument };

            // Act
            result = methodCaller.Invoke("NodeHasAncestors", _parameters) as bool?;

            // Assert
            Assert.IsFalse(result);
        }

        private void InvokeMethod()
        {
            var parser = new PrivateObject(new CssConvert_2_0.Convert());

            try
            {
                parser.Invoke("AppendDeclarations", _parameters);
            }
            catch (Exception exception)
            {
                throw new Exception("Exception encountered", exception);
            }
        }

        private List<RuleSet> GetRuleSets(List<TestInputItems> testInputItemsList)
        {
            var rulesets = new List<RuleSet>();

            var ruleSet = new RuleSet();
            ruleSet.Selectors = GetSelectors(testInputItemsList);
            ruleSet.Declarations = GetDeclarations();
            rulesets.Add(ruleSet);

            return rulesets;
        }

        private List<Selector> GetSelectors(List<TestInputItems> testInputItemsList)
        {
            var selectors = new List<Selector>();

            foreach (var item in testInputItemsList)
            {
                var simpleselectors = new List<SimpleSelector>();
                var simpleSelector = new SimpleSelector();
                simpleSelector.Attribute = null;
                simpleSelector.Child = null;
                simpleSelector.Function = null;
                simpleSelector.Combinator = null;
                simpleSelector.Class = item.ClassName;
                simpleSelector.ElementName = item.ElementName;
                simpleSelector.ID = item.IdName;
                simpleSelector.Pseudo = item.PseudoName;
                simpleselectors.Add(simpleSelector);

                var selector = new Selector();
                selector.SimpleSelectors = simpleselectors;
                selectors.Add(selector);
            }

            return selectors;
        }

        private List<Declaration> GetDeclarations()
        {
            var declarations = new List<Declaration>();

            var declaration = new Declaration
            {
                Name = "Declaration",
                Expression = GetExpression(),
                Important = false
            };

            declarations.Add(declaration);

            return declarations;
        }

        private Expression GetExpression()
        {
            var expression = new Expression
            {
                Terms = GetTermsList()
            };

            return expression;
        }

        private List<Term> GetTermsList()
        {
            var terms = new List<Term>();

            var term = new Term
            {
                Function = null,
                Seperator = ':',
                Sign = null,
                Type = TermType.Url,
                Unit = null,
                Value = "http://localhost"
            };

            terms.Add(term);

            return terms;
        }
    }
}