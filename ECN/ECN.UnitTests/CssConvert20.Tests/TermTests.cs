using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CssConvert.CssParser;
using CssConvert.CssParser.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace CssConvert20.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class TermTests
    {
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [Test]
        [TestCase(TermType.Function)]
        [TestCase(TermType.Url)]
        [TestCase(TermType.Unicode)]
        [TestCase(TermType.Hex)]
        [TestCase(TermType.Number)]
        [TestCase(TermType.String)]
        public void ToString_Test(TermType type)
        {
            var term = new Term();

            if (type == TermType.Function)
            {
                //Arrange
                InitializeFunctionFakes();

                var name = "test function";
                term = FunctionToString(name);
                term.Type = type;

                //Act
                var result = term.ToString();

                //Assert
                Assert.AreEqual(result, name);
            }
            else if (type == TermType.Url)
            {
                //Arrange
                string url = "www.xyz.com";
                term.Type = type;
                term.Value = url;

                //Act
                var result = term.ToString();

                //Assert
                Assert.AreEqual(result, "url('www.xyz.com')");
            }
            else if (type == TermType.Unicode)
            {
                string val = "abc";
                term.Type = type;
                term.Value = val;

                //Act
                var result = term.ToString();

                //Assert
                Assert.AreEqual(result, "U\\ABC");
            }
            else if (type == TermType.Hex)
            {
                string val = "#fff";
                term.Type = type;
                term.Value = val;

                //Act
                var result = term.ToString();

                //Assert
                Assert.AreEqual(result, "#FFF");
            }
            else if (type == TermType.Number || type == TermType.String)
            {
                //Arrange
                string val = "50";
                term.Type = type;
                term.Value = val;
                term.Sign = '+';

                //Act
                var result = term.ToString();

                //Assert
                Assert.AreEqual(result, "+50");

                //Arrange
                term.Unit = Unit.Percent;
                //Act
                result = term.ToString();

                //Assert
                Assert.AreEqual(result, "+50%");


                //Arrange
                term.Unit = Unit.PX;
                //Act
                result = term.ToString();

                //Assert
                Assert.AreEqual(result, "+50px");
            }
        }

        [Test]
        [TestCase(new object[] { Unit.Percent, "70" }, 178)]
        [TestCase(new object[] { Unit.PX, "70" }, 70)]
        [TestCase(new object[] { Unit.Percent, "abc" }, 0)]
        public void GetRGBValue_Test(object[] args, int expectedResult)
        {
            //Arrange
            var term = new Term();

            term.Unit = args[0] as Unit?;
            term.Value = args[1] as string;

            var parser = new PrivateObject(new Term());

            // Act
            var result = (int)parser.Invoke("GetRGBValue", term);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(new object[] { Unit.Percent, "70" }, 49)]
        [TestCase(new object[] { Unit.Percent, "abc" }, 0)]
        public void GetHueValue_Test(object[] args, int expectedResult)
        {
            //Arrange
            var term = new Term();

            term.Unit = args[0] as Unit?;
            term.Value = args[1] as string;

            var parser = new PrivateObject(new Term());

            // Act
            var result = (int)parser.Invoke("GetHueValue", term);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("abcdef", 225)]
        [TestCase("135dfa", 62)]
        public void DeHex_Test(string shimValue, int expectedResult)
        {
            //Arrange
            var parser = new PrivateObject(new Term());

            // Act
            var result = (int)parser.Invoke("DeHex", shimValue);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }


        [Test]
        [TestCase(new object[] { TermType.Hex, "#1a3b5f" }, true)]
        [TestCase(new object[] { TermType.Hex, "#zzz" }, false)]
        [TestCase(new object[] { TermType.String, "zzz" }, false)]
        [TestCase(new object[] { TermType.String, "123456" }, false)]
        [TestCase(new object[] { TermType.String, "Azure" }, true)]
        [TestCase(new object[] { TermType.Function, "rgb" }, true)]
        [TestCase(new object[] { TermType.Function, "rgb" }, false)]
        [TestCase(new object[] { TermType.Function, "hsl" }, true)]
        [TestCase(new object[] { TermType.Function, "hsl" }, false)]
        public void IsColor_Test(object[] args, bool expectedResult)
        {
            //Arrange
            var term = new Term();
            term.Type = (TermType)args[0];
            if (term.Type == TermType.Function)
            {
                var functionName = (string)args[1];
                term.Function = CreateFunction(functionName, expectedResult);
            }
            else
            {
                term.Value = (string)args[1];
            }

            // Act
            var result = term.IsColor;

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(new object[] { TermType.Hex, "#ffa" }, "ffffffaa")]
        [TestCase(new object[] { TermType.Hex, "ffa" }, "ffffffaa")]
        [TestCase(new object[] { TermType.Function, "rgb", true }, "ff14d2f0")]
        [TestCase(new object[] { TermType.Function, "rgb", false }, "Black")]
        [TestCase(new object[] { TermType.Function, "hsl", true }, "fff06b2a")]
        [TestCase(new object[] { TermType.Function, "hsl", false }, "Black")]
        [TestCase(new object[] { TermType.String, "Azure" }, "Azure")]
        public void ToColor_Test(object[] args, string expectedValue)
        {
            var term = new Term();
            term.Type = (TermType)args[0];

            if (term.Type == TermType.Function)
            {
                var functionName = (string)args[1];
                var res = (bool)args[2];
                term.Function = CreateFunction(functionName, res);
            }
            else
            {
                term.Value = (string)args[1];
            }

            // Act
            var result = term.ToColor();

            //Assert
            Assert.AreEqual(expectedValue, result.Name);
        }

        private Function CreateFunction(string functionName, bool expectedResult)
        {
            var colorFunc = new Function();
            var expression = new Expression();
            var terms = new List<Term>();
            var term = new Term
            {
                Value = "20",
                Type = TermType.Hex
            };

            if (expectedResult)
            {
                term.Type = TermType.Number;
            }
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
            colorFunc.Expression = expression;
            colorFunc.Name = functionName;

            return colorFunc;
        }

        private void InitializeFunctionFakes()
        {
            ShimFunction.AllInstances.ToString01 = (instance) =>
            {
                return instance.Name;
            };
        }

        private Term FunctionToString(string name)
        {
            var term = new Term();
            var function = new Function();
            function.Name = name;
            function.Expression = null;

            var expressionTerm = new Term();
            expressionTerm.Type = TermType.Function;
            expressionTerm.Unit = Unit.Percent;
            expressionTerm.Value = "function";
            expressionTerm.Sign = '+';

            var terms = new List<Term>();
            terms.Add(expressionTerm);

            Expression expression = new Expression();
            expression.Terms = terms;

            function.Expression = expression;

            term.Function = function;

            return term;
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}