using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using CssConvert.CssParser;
using CssConvert.CssParser.Fakes;

namespace CssConvert20.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ParserTests
    {
        private IDisposable _shimObject;
        public Term _term;
        public object []_args;
        public List<int> _kind;
        public List<int> _getCall;
        public List<int> _startOf;
        public List<int> _expr;
        public List<string> _val;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();            
            _term = new Term();
            _args =  new object[] { null };
        }

        [Test]
        [TestCase(7, TermType.String)]
        [TestCase(8, TermType.String)]
        [TestCase(9, TermType.Url)]
        [TestCase(46, TermType.Unicode)]
        [TestCase(33, TermType.Hex)]
        public void TermTest_Nested_If_Else<T>(int token_kind, T expectedResult)
        {
            // Arrange
            _kind = new List<int>() { 1, token_kind, 0 };
            _getCall = new List<int>() { 1, token_kind, 0 };
            _startOf = new List<int>() { 1, 0 };

            InitializeFakeParserConstructors(token_kind);
            InitializeFakeWithNoInputParameters();

            var parser = new PrivateObject(new Parser(new Scanner("empty")));
            
            // Act
            parser.Invoke("term", _args);
            _term = _args[0] as Term;

            // Assert
            if (expectedResult is TermType)
            {
                NUnit.Framework.Assert.AreEqual(expectedResult, _term.Type);
            }
        }

        [Test]
        public void TermTest_Nested_Sub_Conditions_StartOf_3_And_StartOf_15()
        {
            // Arrange
            _kind = new List<int>() { 1, 43, 34, 36 };
            _getCall = new List<int>() {1, 43, 34, 36, 36, 36, 36, 0 };
            _startOf = new List<int>() { 1, 3, 15, 3, 16, 0 };
            const string expectedResult = "2222222";

            // Act
            InvokeParser_TermMethod();

            //Asset
            NUnit.Framework.Assert.AreEqual(expectedResult, _term.Value);
        }

        [Test]
        public void TermTest_Nested_Sub_Conditions_StartOf_3_And_Kind_10()
        {
            // Arrange
            _kind = new List<int>() { 1, 10, 0};
            _getCall = new List<int>() { 1, 4, 5, 0 };
            _startOf = new List<int>() { 1, 3, 0 };
            _expr = new List<int>() { 1, 4, 0 };
            TermType expectedResult = TermType.Function;

            // Act
            InvokeParser_TermMethod();

            // Assert
            NUnit.Framework.Assert.AreEqual(expectedResult, _term.Type);
        }

        [Test]
        public void TermTest_Nested_Sub_Conditions_StartOf_17_And_Kind_22_While_Kind_3_if_kind_34()
        {
            // Arrange
            _kind = new List<int>() { 1, 22 };
            _getCall = new List<int>() { 1, 3, 34, 3, 0 };
            _startOf = new List<int>() { 1, 17, 0 };
            Term expectedResult = new Term()
            {
                Value = "vvv",
                Sign = '-'
            };

            // Act
            InvokeParser_TermMethod();

            // Assert
            NUnit.Framework.Assert.AreEqual(expectedResult.Value, _term.Value);
            NUnit.Framework.Assert.AreEqual(expectedResult.Sign, _term.Sign);
        }

        [Test]
        public void TermTest_Nested_Sub_Conditions_StartOf_17_And_Kind_29_While_Kind_3_if_kind_34()
        {
            // Arrange
            _kind = new List<int>() { 1, 29 };
            _getCall = new List<int>() { 1, 3, 34, 3, 0 };
            _startOf = new List<int>() { 1, 17, 0 };
            Term expectedResult = new Term()
            {
                Value = "vvv",
                Sign = '+'
            };

            // Act
            InvokeParser_TermMethod();

            // Assert

            NUnit.Framework.Assert.AreEqual(expectedResult.Value, _term.Value);
            NUnit.Framework.Assert.AreEqual(expectedResult.Sign, _term.Sign);
        }

        [Test]
        public void TermTest_Nested_Sub_Conditions_StartOf_17_And_StartOf_18_Kind_47()
        {
            // Arrange
            _kind = new List<int>() { 1, 47 };
            _getCall = new List<int>() { 1, 0 };
            _startOf = new List<int>() { 1, 17, 18, 0 };
            Term expectedResult = new Term()
            {
                Unit = Unit.Percent
            };

            // Act
            InvokeParser_TermMethod();

            // Assert
            NUnit.Framework.Assert.AreEqual(expectedResult.Unit, _term.Unit);
        }

        [Test]
        public void TermTest_Nested_Sub_Conditions_StartOf_17_And_StartOf_18_End_Else_Throws_Exception()
        {
            // Arrange
            _kind = new List<int>() { 1, 0 };
            _getCall = new List<int>() { 1, 0 };
            _startOf = new List<int>() { 1, 17, 18, 0 };

            Term expectedResult = new Term()
            {
                Unit = Unit.Percent
            };

            InitializeFakeConstructors();
            InitializeFakeWithNoInputParameters();

            // Act
            var testDelegate = new TestDelegate(InvokeMethod);

            // Assert
            NUnit.Framework.Assert.Throws(typeof(Exception), testDelegate);
        }        

        [Test]
        public void TermTest_Nested_Sub_Conditions_StartOf_17_And_StartOf_18_Kind_22()
        {
            // Arrange
            _kind = new List<int>() { 1, -1, 22, 0 };
            _getCall = new List<int>() { 1, 3, 0 };
            _startOf = new List<int>() { 1, 17, 18, 0 };
            _val = new List<string>() { "1", "n" };

            Term expectedResult = new Term()
            {
                Value = "nnnn"
            };

            // Act
            InvokeParser_TermMethod();

            // Assert
            NUnit.Framework.Assert.AreEqual(expectedResult.Value, _term.Value);
        }

        [Test]
        public void TermTest_Nested_Sub_Conditions_StartOf_17_And_StartOf_18_Kind_29()
        {
            // Arrange
            _kind = new List<int>() { 1, -1, 29, 0 };
            _getCall = new List<int>() { 1, 3, 0 };
            _startOf = new List<int>() { 1, 17, 18, 0 };
            _val = new List<string>() { "1", "n" };

            Term expectedResult = new Term()
            {
                Value = "nnnn"
            };

            // Act
            InvokeParser_TermMethod();
            
            // Assert
            NUnit.Framework.Assert.AreEqual(expectedResult.Value, _term.Value);
        }

        [Test]
        public void TermTest_Nested_End_Else()
        {
            // Arrange
            _kind = new List<int>() { 1, 0 };
            _getCall = new List<int>() { 1, 0 };
            _startOf = new List<int>() { 1, 0 };

            Term expectedResult = new Term();

            // Act
            InvokeParser_TermMethod();

            // Assert
            NUnit.Framework.Assert.AreEqual(expectedResult.Value, _term.Value);
            NUnit.Framework.Assert.AreEqual(expectedResult.Unit, _term.Unit);
            NUnit.Framework.Assert.AreEqual(expectedResult.Sign, _term.Sign);
            NUnit.Framework.Assert.AreEqual(expectedResult.Type, _term.Type);
        }

        private void InvokeParser_TermMethod()
        {
            InitializeFakeConstructors();
            InitializeFakeWithNoInputParameters();

            var parser = new PrivateObject(new Parser(new Scanner("empty")));

            parser.Invoke("term", _args);
            _term = _args[0] as Term;
        }

        private void InitializeFakeConstructors()
        {
            InitializeFakeParserConstructorsDefault();

            ShimParser.AllInstances.StartOfInt32 = (Parser instance, int localValue) =>
            {
                if (localValue == 16)
                {
                    instance.la.kind = 3;
                }

                if (_startOf[_startOf[0]] == localValue)
                {
                    _startOf[0]++;
                    return true;
                }

                return false;
            };
        }

        private void InitializeFakeParserConstructorsDefault()
        {
            InitializeFakeParserConstructors(_kind[_kind[0]]);
            _kind[0]++;
        }        

        private void InitializeFakeParserConstructors(int input)
        {
            int? numElements = _val?.Count;
            var stringVal = "v";
            int index = 0;

            if (numElements.HasValue)
            {
                int valueToIntIndex = int.Parse(_val[index]);
                stringVal = _val[valueToIntIndex];
                index = valueToIntIndex + 1;
                _val[valueToIntIndex] = index.ToString();
            }

            ShimParser.ConstructorScanner = (instance, localValue) =>
            {
                var token = new Token()
                {
                    col = 0,
                    kind = input,
                    line = 1,
                    next = null,
                    pos = 0,
                    val = stringVal
                };

                instance.la = token;
                instance.t = token;
            };            
        }

        private void InitializeFakeWithNoInputParameters()
        {
            ShimParser.AllInstances.Get = (instance) =>
            {
                instance.la.kind = _getCall[_getCall[0]];
                InitializeFakeParserConstructors(_getCall[_getCall[0]]);
                _getCall[0]++;
            };
            
            ShimParser.AllInstances.exprExpressionOut = (Parser instance, out Expression localValue) =>
            {
                instance.la.kind = _expr[_expr[0]];
                _expr[0]++;
                localValue = null;
            };

            ShimScanner.ConstructorString = (instance, localValue) =>
            {

            };

            ShimTerm.Constructor = (instance) =>
            {
                instance.Type = _term.Type;
                instance.Seperator = _term.Seperator;
            };

            ShimParser.AllInstances.identityStringOut = (Parser instance, out string localValue) =>
            {
                localValue = string.Empty;
            };

            ShimParser.AllInstances.HexValueStringOut = (Parser instance, out string returnV) =>
            {
                returnV = string.Empty;
            };

            ShimParser.AllInstances.QuotedStringStringOut = (Parser instance, out string returnV) =>
            {
                returnV = string.Empty;
            };

            ShimParser.AllInstances.URIStringOut = (Parser instance, out string returnV) =>
            {
                returnV = string.Empty;
            };

            ShimParser.AllInstances.identityStringOut = (Parser instance, out string returnV) =>
            {
                returnV = string.Empty;
            };

            ShimParser.AllInstances.SynErrInt32 = (Parser instance, int localValue) =>
            {
                //Do nothing
            };

            ShimParser.AllInstances.ExpectInt32 = (Parser instance, int localValue) =>
            {
                if(localValue == 23)
                {
                    instance.la.kind = _kind[2];
                }
            };

            ShimParser.AllInstances.IsUnit = (instance) =>
            {
                return true;
            };            
        }

        private void InvokeMethod()
        {
            var parser = new PrivateObject(new Parser(new Scanner("empty")));
            try
            {
                parser.Invoke("term", _term);
            }
            catch
            {
                throw new Exception("Exception encountered in final else statement");
            }
        }
        
        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }        
    }
}
