using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using CssConvert.CssParser;
using CssConvert.CssParser.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace CssConvert20.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ParserTest
    {
        private IDisposable _shimObject;
        public List<int> _kind;
        public List<int> _getCall;
        public List<int> _startOf;
        public List<int> _expect;
        public object[] _args;
        public string _directiveType;
        public string _testName;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _args = new object[] { null };
            _directiveType = null;
            _testName = string.Empty;
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        [TestCase("#zzz", false)]
        [TestCase("#123afd", false)]
        [TestCase("#123afdf", false)]
        [TestCase("A", true)]
        public void PartOfHex_Test(string input, bool expected)
        {
            //Arrange
            CreateFakeScannerConstructors();
            var fakeScanner = new Scanner("TEST");

            CreateFakeParserConstructors(fakeScanner);

            var parser = new Parser(fakeScanner);
            parser.la.val = input[input.Length - 1].ToString();

            var privateObj = new PrivateObject(parser);

            //Act
            var result = (bool)privateObj.Invoke("PartOfHex", input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(5, false)]
        [TestCase(1, true)]
        public void IsUnit_Test(int input, bool expected)
        {
            //Arrange
            CreateFakeScannerConstructors();
            var fakeScanner = new Scanner("TEST");

            CreateFakeParserConstructors(fakeScanner);

            var parser = new Parser(fakeScanner);
            parser.la.val = "em";
            parser.la.kind = input;

            var privateObj = new PrivateObject(parser);

            //Act
            var result = (bool)privateObj.Invoke("IsUnit");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(7, 7)]
        [TestCase(50, 48)]
        public void Get_Test(int input, int expected)
        {
            //Arrange
            CreateFakeScannerConstructors();
            var fakeScanner = new Scanner("TEST");

            CreateFakeParserConstructors(fakeScanner);
            CreateFakeScannerScan(input);

            var parser = new Parser(fakeScanner);
            parser.la.val = "em";
            parser.la.kind = 1;
            var privateObj = new PrivateObject(parser);

            //Act
            var result = privateObj.Invoke("Get");

            //Assert
            Assert.AreEqual(expected, parser.la.kind);
        }

        [Test]
        public void CSS3_Test()
        {
            //Arrange
            _kind = new List<int>() { 1, 4, 5 };
            _getCall = new List<int>() { 1, 5, 6, 0, 6, 4, 0, -1 };
            _startOf = new List<int>() { 1, 1, 2, 1, 3, 0 };
            _testName = "CSS3_Test";

            CreateFakeScannerConstructors();
            var fakeScanner = new Scanner("TEST");

            CreateFakeParserConstructors(fakeScanner);

            var parser = new Parser(fakeScanner);
            parser.la.val = "em";
            parser.la.kind = _kind[_kind[0]];
            _kind[0]++;

            InitializeFakeGetMethod(parser);
            InitializeFakeStartofMethod(parser);
            InitializeFakeExpectMethod(parser);
            InitializeFakeselectorMethod(parser);
            InitializeFakedeclarationMethod(parser);
            InitializeFakerulesetMethod(parser);
            InitializeFakedirectiveMethod(parser);

            var privateObj = new PrivateObject(parser);

            //Act
            var result = privateObj.Invoke("CSS3");

            //Assert
            Assert.AreEqual(_kind.Last(), parser.la.kind);
        }

        [Test]
        public void Ruleset_Test()
        {
            //Arrange
            _kind = new List<int>() { 1, 4, 4, 4, 4, 4, 4, 4, 0 };
            _getCall = new List<int>() { 1, 25, 4, -1, -1, -1, 27, 4, -1, 4, -1, -1, -1 };
            _startOf = new List<int>() { 1, 3, 0 };
            _expect = new List<int>() { 1, 26, 28, 0 };
            _args = new object[] { null };

            CreateFakeScannerConstructors();
            var fakeScanner = new Scanner("TEST");

            CreateFakeParserConstructors(fakeScanner);

            var parser = new Parser(fakeScanner);
            parser.la.val = "em";
            parser.la.kind = _kind[_kind[0]];
            _kind[0]++;

            InitializeFakeGetMethod(parser);
            InitializeFakeStartofMethod(parser);
            InitializeFakeExpectMethod(parser);
            InitializeFakeselectorMethod(parser);
            InitializeFakedeclarationMethod(parser);
            InitializeFakedirectiveMethod(parser);

            var privateObj = new PrivateObject(parser);

            //Act
            privateObj.Invoke("ruleset", _args);

            //Assert
            Assert.AreEqual(_getCall.Last(), parser.la.kind);
        }

        [Test]
        [TestCase(new object[] { new int[] { 1, 7, 0 }, //_kind
            new int[] { 1, 7, 0 },                      //_getCall
            new int[] { 1, 7, -1, 0 },                  //_startOf
            new int[] { 1, 7, 0 } }, "em")]             //_expect
        [TestCase(new object[] { new int[] { 1, 8, 0 },
            new int[] { 1, 8, 0 },
            new int[] { 1, 8, -1, 0 },
            new int[] { 1, 8, 0 } }, "em")]
        [TestCase(new object[] { new int[] { 1, -1 },
            new int[] { 1,0 },
            new int[] { 1, 0 },
            new int[] { 1, 0 } }, "")]
        public void QuotedString_Test(object[] inputs, string dummyOutput)
        {
            //Arrange
            var x = inputs[0] as int[];
            _kind = x.ToList();

            x = inputs[1] as int[];
            _getCall = x.ToList();

            x = inputs[2] as int[];
            _startOf = x.ToList();

            x = inputs[3] as int[];
            _expect = x.ToList();

            _args = new object[] { null };

            CreateFakeScannerConstructors();
            var fakeScanner = new Scanner("TEST");

            CreateFakeParserConstructors(fakeScanner);

            var parser = new Parser(fakeScanner);
            parser.la.val = dummyOutput;
            parser.la.kind = _kind[_kind[0]];
            _kind[0]++;

            InitializeFakeGetMethod(parser);
            InitializeFakeStartofMethod(parser);
            InitializeFakeExpectMethod(parser);

            var privateObj = new PrivateObject(parser);

            //Act
            privateObj.Invoke("QuotedString", _args);

            //Assert
            Assert.AreEqual(dummyOutput, _args[0].ToString());
        }

        [Test]
        [TestCase(new object[] { new int[]{1, 4, 4, 4, 4, 4, 4, 0},
        new int[]{ 1, -1, 25, 4, -1, 26, 4, -1, 27, 4, -1, 4, -1, -1, 0},
        new int[]{ 1, 4, 5, 6, 1, 0},
        new int[]{ 1, 0 },
        "font-face" }, "@font-face")]
        [TestCase(new object[] { new int[]{1, 4, 4, 4, 4, 4, 4, 0},
        new int[]{ 1, -1, 25, 4, -1, 26, 4, -1, -1, 0 },
        new int[]{ 1, 4, 5, 6, 1, 2, 0},
        new int[]{ 1, 0 },
        "media" }, "@media print")]
        public void Directive_Test(object[] inputs, string expected)
        {
            //Arrange
            var x = inputs[0] as int[];
            _kind = x.ToList();

            x = inputs[1] as int[];
            _getCall = x.ToList();

            x = inputs[2] as int[];
            _startOf = x.ToList();

            x = inputs[3] as int[];
            _expect = x.ToList();

            _args = new object[] { null };
            _directiveType = inputs[4].ToString();
            _testName = "Directive_Test";
            CreateFakeScannerConstructors();
            var fakeScanner = new Scanner("TEST");

            CreateFakeParserConstructors(fakeScanner);

            var parser = new Parser(fakeScanner);
            parser.la.val = "TEST";
            parser.la.kind = _kind[_kind[0]];
            _kind[0]++;

            InitializeFakeGetMethod(parser);
            InitializeFakeStartofMethod(parser);
            InitializeFakeExpectMethod(parser);
            InitializeFakeIdentityMethod(parser);
            InitializeFakeMediumMethod(parser);
            InitializeFakedeclarationMethod(parser);
            InitializeFakerulesetMethod(parser);

            var privateObj = new PrivateObject(parser);

            //Act
            privateObj.Invoke("directive", _args);

            //Assert
            Assert.IsTrue(_args[0].ToString().StartsWith(expected));
        }

        private void InitializeFakeIdentityMethod(Parser parser)
        {
            ShimParser.AllInstances.identityStringOut = (Parser instance, out string shimValue) =>
            {
                shimValue = _directiveType;
            };
        }

        private void InitializeFakeSynErrMethod(Parser parser)
        {
            ShimParser.AllInstances.SynErrInt32 = (instance, shimValue) =>
            {
                //Do nothing
            };
        }

        private void CreateFakeParserConstructors(Scanner fakeScanner)
        {
            ShimParser.ConstructorScanner = (instance, shimValue) =>
            {
                var token = new Token()
                {
                    col = 0,
                    kind = 7,
                    line = 1,
                    next = null,
                    pos = 0,
                    val = ""
                };

                instance.la = token;
                instance.t = token;

                instance.scanner = fakeScanner;
            };
        }

        private void CreateFakeScannerScan(int kind_value)
        {
            ShimScanner.AllInstances.Scan = (instance) =>
            {
                var token = new Token()
                {
                    col = 0,
                    kind = kind_value--,
                    line = 1,
                    next = null,
                    pos = 0,
                    val = "TEST"
                };

                return token;
            };
        }

        private void CreateFakeScannerConstructors()
        {
            ShimScanner.ConstructorString = (instance, shimValue) =>
            {

            };
        }

        private void InitializeFakeGetMethod(Parser parser)
        {
            ShimParser.AllInstances.Get = (instance) =>
            {
                instance.la.kind = _getCall[_getCall[0]];
                InitializeFakeParserConstructors(_getCall[_getCall[0]]);
                _getCall[0]++;

                parser.la.kind = instance.la.kind;
            };
        }

        private void InitializeFakeMediumMethod(Parser parser)
        {
            ShimParser.AllInstances.mediumMediumOut = (Parser instance, out Medium medium) =>
            {
                medium = Medium.print;
                instance.la.kind = _kind[_kind[0]];
                _kind[0]++;
                parser.la.kind = instance.la.kind;
            };
        }

        private void InitializeFakeStartofMethod(Parser parser)
        {
            ShimParser.AllInstances.StartOfInt32 = (Parser instance, int shimValue) =>
            {
                if (instance.la.kind == 0)
                {
                    instance.la.kind = 5;
                    parser.la.kind = instance.la.kind;
                }

                if (_startOf[_startOf[0]] == shimValue)
                {
                    _startOf[0]++;

                    if (shimValue == 2 && _testName.Equals("Directive_Test"))
                    {
                        instance.la.kind = _kind[_kind[0]];
                        _kind[0]++;
                        parser.la.kind = instance.la.kind;
                    }

                    return true;
                }

                if (instance.la.kind == -1)
                {
                    instance.la.kind = _kind[_kind[0]];
                    _kind[0]++;
                    parser.la.kind = instance.la.kind;
                }

                return false;
            };
        }

        private void InitializeFakeExpectMethod(Parser parser)
        {
            ShimParser.AllInstances.ExpectInt32 = (Parser instance, int shimValue) =>
            {
                if (_expect[_expect[0]] == shimValue)
                {
                    _expect[0]++;
                    instance.la.kind = _kind[_kind[0]];
                    _kind[0]++;
                    parser.la.kind = instance.la.kind;
                }
            };
        }

        private void InitializeFakeselectorMethod(Parser parser)
        {
            ShimParser.AllInstances.selectorSelectorOut = (Parser instance, out Selector selector) =>
            {
                var simpleselectors = new List<SimpleSelector>();
                var simpleSelector = new SimpleSelector();
                simpleSelector.Attribute = null;
                simpleSelector.Child = null;
                simpleSelector.Function = null;
                simpleSelector.Combinator = null;
                simpleSelector.Class = "TEST CLASS";
                simpleSelector.ElementName = "TEST ELEMENT";
                simpleSelector.ID = "TEST ID";
                simpleSelector.Pseudo = "TEST PSEUDO";
                simpleselectors.Add(simpleSelector);

                selector = new Selector();
                selector.SimpleSelectors = simpleselectors;

                if (instance.la.kind == 4)
                {
                    instance.la.kind = _kind[_kind[0]];
                    _kind[0]++;
                    parser.la.kind = instance.la.kind;
                }

                if (instance.la.kind == -1)
                {
                    instance.la.kind = _kind[_kind[0]];
                    _kind[0]++;
                    parser.la.kind = instance.la.kind;
                }
            };
        }

        private void InitializeFakedeclarationMethod(Parser parser)
        {
            ShimParser.AllInstances.declarationDeclarationOut = (Parser instance, out Declaration declaration) =>
            {
                declaration = new Declaration
                {
                    Name = "Declaration",
                    Expression = GetExpression(),
                    Important = false
                };

                if (instance.la.kind == -1)
                {
                    instance.la.kind = _kind[_kind[0]];
                    _kind[0]++;
                    parser.la.kind = instance.la.kind;
                }
            };
        }

        private void InitializeFakerulesetMethod(Parser parser)
        {
            ShimParser.AllInstances.rulesetRuleSetOut = (Parser instance, out RuleSet ruleSet) =>
            {
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

                ruleSet = new RuleSet
                {
                    Selectors = GetSelectors(testInputItemsList),
                    Declarations = GetDeclarations()
                };
            };
        }

        private void InitializeFakedirectiveMethod(Parser parser)
        {
            ShimParser.AllInstances.directiveDirectiveOut = (Parser instance, out Directive directive) =>
            {
                var testInputItemsList = new List<TestInputItems>();
                var testInputItems = new TestInputItems();
                var className = "h1Class";
                var elementName = "h1";
                var name = new StringBuilder();
                testInputItems.ClassName = name.Append(className)
                    .Append(" ")
                    .Append(elementName).ToString();
                testInputItems.PseudoName = "hover";
                testInputItemsList.Add(testInputItems);

                directive = new Directive
                {
                    Type = DirectiveType.Charset,
                    Name = "DIRECTIVE-TEST"
                };
                var mediums = new List<Medium>
                {
                    Medium.print
                };
                directive.Mediums = mediums;
                directive.Directives = null;
                directive.RuleSets = GetRuleSets(testInputItemsList);
                directive.Declarations = GetDeclarations(); ;
                directive.Expression = GetExpression();
            };
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
            var expression = new Expression();
            var terms = new List<Term>();
            var term = new Term
            {
                Value = "20",
                Type = TermType.Hex
            };

            terms.Add(term);

            term = new Term
            {
                Value = "210",
                Type = TermType.Number
            };
            terms.Add(term);

            term = new Term
            {
                Value = "240",
                Type = TermType.Number
            };
            terms.Add(term);

            expression.Terms = terms;

            return expression;
        }

        private void InitializeFakeParserConstructors(int input)
        {
            ShimParser.ConstructorScanner = (instance, shimValue) =>
            {
                var token = new Token()
                {
                    col = 0,
                    kind = input,
                    line = 1,
                    next = null,
                    pos = 0,
                    val = "A"
                };

                instance.la = token;
                instance.t = token;
            };
        }
    }
}