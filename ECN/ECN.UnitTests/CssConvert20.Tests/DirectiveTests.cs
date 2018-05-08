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
    public partial class DirectiveTests
    {
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        [TestCase(new object[] { DirectiveType.Charset }, "FakeValue")]
        [TestCase(new object[] { DirectiveType.Page }, "FakeValue")]
        [TestCase(new object[] { DirectiveType.Media }, "FakeValue")]
        [TestCase(new object[] { DirectiveType.Import }, "FakeValue")]
        [TestCase(new object[] { DirectiveType.FontFace }, "FakeValue")]
        [TestCase(new object[] { DirectiveType.Other }, "Test 20 210 240  print, screen, handheld;")]
        [TestCase(new object[] { DirectiveType.Other, true }, "Test 20 210 240  print, screen, handheld {\r\n\t.h1Class h1:hover {\r\n\t\tDeclaration: 20 210 240;\r\n\t}\r\n\r\n\tDeclaration: 20 210 240\r\n}")]
        [TestCase(new object[] { DirectiveType.Other, false }, "Test 20 210 240  print, screen, handheld;")]
        public void ToString_Test(object[] input, string expectedValue)
        {
            //Arrange
            var directive = new Directive();
            directive.Name = "Test";
            directive.Type = (DirectiveType)input[0];
            directive.Mediums = GetDirectiveMediums();

            if (directive.Type == DirectiveType.Other)
            {
                directive.Expression = GetExpression();
            }

            if (input.Length > 1)
            {
                if ((bool)input[1])
                {
                    SetDirectiveDeclarations_RuleSets_Directives(directive);
                }
            }

            InitializeFakeMethods();

            string result = "";

            //Act
            if (input.Count() > 1)
            {
                if ((bool)input[1])
                {
                    result = directive.ToString(0);
                }
                else
                {
                    result = directive.ToString();
                }
            }
            else
            {
                result = directive.ToString(0);
            }

            //Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void ToFontFaceString_Test()
        {
            //Arrange
            var expected = "@font-face {\r\n\tTESTDeclaration: 20 210 240\r\n}";
            var directive = new Directive();
            directive.Name = "Test";
            directive.Mediums = GetDirectiveMediums();
            SetDirectiveDeclarations_RuleSets_Directives(directive);

            var directiveObj = new PrivateObject(directive);

            // Act
            var result = (string)directiveObj.Invoke("ToFontFaceString", "TEST");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToImportString_Test()
        {
            //Arrange
            var expected = "@import  print, screen, handheld;";
            var directive = new Directive();
            directive.Name = "Test";
            directive.Mediums = GetDirectiveMediums();
            SetDirectiveDeclarations_RuleSets_Directives(directive);

            var directiveObj = new PrivateObject(directive);

            // Act
            var result = (string)directiveObj.Invoke("ToImportString");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToMediaString_Test()
        {
            //Arrange
            var expected = "@media print, screen, handheld {\r\n\t\t\t.h1Class h1:hover {\r\n\t\t\t\tDeclaration: 20 210 240;\r\n\t\t\t}\r\n}";
            var directive = new Directive();
            directive.Name = "Test";
            directive.Mediums = GetDirectiveMediums();
            SetDirectiveDeclarations_RuleSets_Directives(directive);

            var directiveObj = new PrivateObject(directive);

            // Act
            var result = (string)directiveObj.Invoke("ToMediaString", 2);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToPageString_Test()
        {
            //Arrange
            var expected = "@page {\r\n\r\n\tTESTDeclaration: 20 210 240}";
            var directive = new Directive();
            directive.Name = "Test";
            directive.Mediums = GetDirectiveMediums();
            SetDirectiveDeclarations_RuleSets_Directives(directive);

            var directiveObj = new PrivateObject(directive);

            // Act
            var result = (string)directiveObj.Invoke("ToPageString", "TEST");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToCharSetString_Test()
        {
            //Arrange
            var expected = "-TEST-Test 20 210 240";
            var directive = new Directive();
            directive.Name = "Test";
            directive.Expression = GetExpression();
            //directive.Mediums = GetDirectiveMediums();
            //SetDirectiveDeclarations_RuleSets_Directives(directive);

            var directiveObj = new PrivateObject(directive);

            // Act
            var result = (string)directiveObj.Invoke("ToCharSetString", "-TEST-");

            //Assert
            Assert.AreEqual(expected, result);
        }

        private void InitializeFakeConstructors()
        {
            ShimDirective.Constructor = (instance) =>
            {
                instance.Name = "Test";
                instance.Mediums = GetDirectiveMediums();
                SetDirectiveDeclarations_RuleSets_Directives(instance);
            };
        }

        private void SetDirectiveDeclarations_RuleSets_Directives(Directive directive)
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

            directive.Declarations = GetDeclarations();
            directive.RuleSets = GetRuleSets(testInputItemsList);
            directive.Directives = GetDirectives(testInputItemsList);
        }

        private List<Directive> GetDirectives(List<TestInputItems> testInputItemsList)
        {
            var directives = new List<Directive>();
            var directive = new Directive();
            directive.Type = DirectiveType.Charset;
            directive.Name = "DIRECTIVE-TEST";
            var medium = new List<Medium>
            {
                Medium.print
            };
            directive.Mediums = medium;
            directive.Directives = null;
            directive.RuleSets = GetRuleSets(testInputItemsList);
            directive.Declarations = GetDeclarations(); ;
            directive.Expression = GetExpression();

            return directives;
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

        private void InitializeFakeMethods()
        {
            string fakeValue = "FakeValue";
            ShimDirective.AllInstances.ToCharSetStringString = (Directive instance, string start) =>
            {
                return fakeValue;
            };
            ShimDirective.AllInstances.ToPageStringString = (Directive instance, string start) =>
            {
                return fakeValue;
            };
            ShimDirective.AllInstances.ToMediaStringInt32 = (Directive instance, int start) =>
            {
                return fakeValue;
            };
            ShimDirective.AllInstances.ToImportString = (Directive instance) =>
            {
                return fakeValue;
            };
            ShimDirective.AllInstances.ToFontFaceStringString = (Directive instance, string start) =>
            {
                return fakeValue;
            };
        }

        private static List<Medium> GetDirectiveMediums()
        {
            var mediums = new List<Medium>();
            var medium = Medium.print;
            mediums.Add(medium);
            medium = Medium.screen;
            mediums.Add(medium);
            medium = Medium.handheld;
            mediums.Add(medium);

            return mediums;
        }
    }
}