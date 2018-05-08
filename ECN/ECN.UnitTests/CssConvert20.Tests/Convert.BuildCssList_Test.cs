using System.Collections.Generic;
using System.Text;
using CssConvert.CssParser;
using CssConvert_2_0;
using CssConvert_2_0.Fakes;
using NUnit.Framework;
using Shouldly;

namespace CssConvert20.Tests
{
    public partial class ConvertTests
    {
        [Test]
        public void BuildCssTest_Null_Input()
        {
            // Arrange
            var cssDocument = new CSSDocument();
            cssDocument.RuleSets = null;
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.ShouldNotBeNull();
            result.HasClass.ShouldBeFalse();
            result.HasElement.ShouldBeFalse();
            result.HasId.ShouldBeFalse();
            result.ListClass.Count.ShouldBe(0);
            result.ListElement.Count.ShouldBe(0);
            result.ListId.Count.ShouldBe(0);
        }

        [Test]
        public void BuildCssTest_Has_Class_Element_And_Pseudo_Element()
        {
            // Arrange
            var cssDocument = new CSSDocument();

            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();

            var listStyle = "Declaration: url('http://localhost')";
            var className = "h1Class";
            var elementName = "h1";
            var name = new StringBuilder();
            testInputItems.ClassName = name.Append(className)
                .Append(" ")
                .Append(elementName).ToString();
            testInputItems.PseudoName = "hover";
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.ShouldNotBeNull();
            result.HasClass.ShouldBeTrue();
            result.HasElement.ShouldBeFalse();
            result.HasId.ShouldBeFalse();
            result.ListClass[0].Name.ShouldBe(className);
            result.ListClass[0].ListClassStyle.ShouldBeNull();
            result.ListClass[0].ListElement[0].ListElementStyle.ShouldBeNull();
            result.ListClass[0].ListElement[0].Name.ShouldBe(elementName);
            result.ListClass[0].ListElement[0].Parent.ShouldBeNull();
            result.ListClass[0].ListElement[0].ListElementStyle.ShouldBeNull();
            result.ListClass[0].ListElement[0].ListPsuedo[0].Name.ShouldBe(testInputItemsList[0].PseudoName);
            result.ListClass[0].ListElement[0].ListPsuedo[0].ListStyle[0].Format.ShouldBe(listStyle);
        }

        [Test]
        public void BuildCssTest_Has_Id__Pseudo_Element_And_Element()
        {
            // Arrange
            var cssDocument = new CSSDocument();

            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();

            var listStyle = "Declaration: url('http://localhost')";
            var id = "Id";
            var pseudo = "hover";
            var elementName = "h1";
            var name = new StringBuilder();
            testInputItems.IdName = name.Append(id).Append(":")
                .Append(pseudo)
                .Append(" ")
                .Append(elementName).ToString();
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.ShouldNotBeNull();
            result.HasClass.ShouldBeFalse();
            result.HasElement.ShouldBeFalse();
            result.HasId.ShouldBeTrue();
            result.ListId[0].Name.ShouldBe(testInputItems.IdName);
            result.ListId[0].ListIdStyle.ShouldBeNull();
            result.ListId[0].ListElement[0].ListElementStyle.ShouldBeNull();
            result.ListId[0].ListElement[0].Name.ShouldBe(id);
            result.ListId[0].ListElement[0].Parent.ShouldBeNull();
            result.ListId[0].ListElement[0].ListPsuedo[0].Name.ShouldBe(pseudo);
            result.ListId[0].ListElement[0].ListPsuedo[0].ListStyle[0].Format.ShouldBe(listStyle);
        }

        [Test]
        public void BuildCssTest_Has_Id_Element()
        {
            // Arrange
            var cssDocument = new CSSDocument();

            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();

            var listStyle = "Declaration: url('http://localhost')";
            var idName = "Id";
            var elementName = "ul";
            var name = new StringBuilder();
            testInputItems.IdName = name.Append(idName)
                .Append(" ")
                .Append(elementName).ToString();
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.ShouldNotBeNull();
            result.HasClass.ShouldBeFalse();
            result.HasElement.ShouldBeFalse();
            result.HasId.ShouldBeTrue();
            result.ListId[0].Name.ShouldBe(testInputItems.IdName);
            result.ListId[0].ListIdStyle.ShouldBeNull();
            result.ListId[0].ListElement[0].Name.ShouldBe(idName);
            result.ListId[0].ListElement[0].Parent.ShouldBeNull();
            result.ListId[0].ListElement[0].ListPsuedo.Count.ShouldBe(0);
            result.ListId[0].ListElement[0].ListElementStyle[0].Format.ShouldBe(listStyle);
        }

        [Test]
        public void BuildCssTest_Has_Class_And_Nested_Element()
        {
            // Arrange
            var cssDocument = new CSSDocument();

            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();

            var listStyle = "Declaration: url('http://localhost')";
            var className = "h1Class";
            var elementName = "h1";
            var name = new StringBuilder();
            testInputItems.ClassName = name.Append(className)
                .Append(" ")
                .Append(elementName).ToString();
            testInputItemsList.Add(testInputItems);

            testInputItems = new TestInputItems();
            name = new StringBuilder();
            testInputItems.ClassName = name.Append(className).Append(" ").Append(elementName).ToString();
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.ShouldNotBeNull();
            result.HasClass.ShouldBeTrue();
            result.HasElement.ShouldBeFalse();
            result.HasId.ShouldBeFalse();
            result.ListClass[0].Name.ShouldBe(className);
            result.ListClass[0].ListClassStyle.ShouldBeNull();
            result.ListClass[0].ListElement[0].ListElementStyle[0].Format.ShouldBe(listStyle);
            result.ListClass[0].ListElement[0].Name.ShouldBe(elementName);
            result.ListClass[0].ListElement[0].Parent.ShouldBeNull();
        }

        [Test]
        public void BuildCssTest_Has_Element_Only()
        {
            // Arrange
            var cssDocument = new CSSDocument();

            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();

            var listStyle = "Declaration: url('http://localhost')";
            var elementName = "h1";

            testInputItems.ElementName = elementName;
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.HasClass.ShouldBeFalse();
            result.HasElement.ShouldBeTrue();
            result.HasId.ShouldBeFalse();
            result.ListElement[0].Name.ShouldBe(elementName);
            result.ListElement[0].Parent.ShouldBeNull();
            result.ListElement[0].ListPsuedo.Count.ShouldBe(0);
            result.ListElement[0].ListElementStyle[0].Format.ShouldBe(listStyle);
            result.ListClass.Count.ShouldBe(0);
            result.ListId.Count.ShouldBe(0);
        }

        [Test]
        public void BuildCssTest_Has_Id_Only()
        {
            // Arrange
            var cssDocument = new CSSDocument();

            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();

            var listStyle = "Declaration: url('http://localhost')";
            var idName = "h1";

            testInputItems.IdName = idName;
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.ShouldNotBeNull();
            result.HasClass.ShouldBeFalse();
            result.HasElement.ShouldBeFalse();
            result.HasId.ShouldBeTrue();
            result.ListId[0].Name.ShouldBe(idName);
            result.ListId[0].ListElement.Count.ShouldBe(0);
            result.ListId[0].ListIdStyle[0].Format.ShouldBe(listStyle);
            result.ListClass.Count.ShouldBe(0);
            result.ListElement.Count.ShouldBe(0);
        }

        [Test]
        public void BuildCssTest_Has_Element_With_Pseudo()
        {
            // Arrange
            var cssDocument = new CSSDocument();

            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();
            var listStyle = "Declaration: url('http://localhost')";

            var elementName = "li";
            var pseudo = "hover";

            var syntax = new StringBuilder();

            testInputItems.ElementName = syntax.Append(elementName).Append(":").Append(pseudo).ToString();
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.ShouldNotBeNull();
            result.HasClass.ShouldBeFalse();
            result.HasElement.ShouldBeTrue();
            result.HasId.ShouldBeFalse();
            result.ListElement[0].Name.ShouldBe(elementName);
            result.ListClass.Count.ShouldBe(0);
            result.ListId.Count.ShouldBe(0);
            result.ListElement[0].Parent.ShouldBeNull();
            result.ListElement[0].ListElementStyle.ShouldBeNull();
            result.ListElement[0].ListPsuedo[0].Name.ShouldBe(pseudo);
            result.ListElement[0].ListPsuedo[0].ListStyle[0].Format.ShouldBe(listStyle);
        }

        [Test]
        public void BuildCssTest_Has_Elements_With_Pseudo_Then_Class()
        {
            // Arrange
            var cssDocument = new CSSDocument();

            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();
            var listStyle = "Declaration: url('http://localhost')";

            var elementName = "li";
            var pseudo = "hover";
            var className = "links";

            var syntax = new StringBuilder();

            testInputItems.ElementName = syntax.Append(elementName).Append(":").Append(pseudo).Append(".").Append(className).ToString();
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.ShouldNotBeNull();
            result.HasClass.ShouldBeTrue();
            result.HasElement.ShouldBeFalse();
            result.HasId.ShouldBeFalse();
            result.ListElement.Count.ShouldBe(0);
            result.ListId.Count.ShouldBe(0);
            result.ListClass[0].Name.ShouldBe(className);
            result.ListClass[0].ListClassStyle.ShouldBeNull();
            result.ListClass[0].ListElement[0].Parent.ShouldBeNull();
            result.ListClass[0].ListElement[0].Name.ShouldBe(elementName);
            result.ListClass[0].ListElement[0].ListElementStyle.ShouldBeNull();
            result.ListClass[0].ListElement[0].ListPsuedo[0].Name.ShouldBe(pseudo);
            result.ListClass[0].ListElement[0].ListPsuedo[0].ListStyle[0].Format.ShouldBe(listStyle);
        }

        [Test]
        public void BuildCssTest_Has_Class_Only()
        {
            // Arrange
            var cssDocument = new CSSDocument();

            var testInputItemsList = new List<TestInputItems>();
            var testInputItems = new TestInputItems();
            var listStyle = "Declaration: url('http://localhost')";
            var className = "h1Class";

            testInputItems.ClassName = className;
            testInputItemsList.Add(testInputItems);

            testInputItems = new TestInputItems();
            testInputItems.ClassName = "h1Class";
            testInputItemsList.Add(testInputItems);

            cssDocument.RuleSets = GetRuleSets(testInputItemsList);
            cssDocument.Directives = null;

            InitializeFakeConstructors();

            // Act
            var result = _cssListBuilder.Invoke("BuildCssList", cssDocument) as CssItem;

            // Assert
            result.ShouldNotBeNull();
            result.HasClass.ShouldBeTrue();
            result.HasElement.ShouldBeFalse();
            result.HasId.ShouldBeFalse();
            result.ListClass[0].Name.ShouldBe(className);
            result.ListClass[0].ListElement.Count.ShouldBe(0);
            result.ListClass[0].ListClassStyle[0].Format.ShouldBe(listStyle);
            result.ListId.Count.ShouldBe(0);
            result.ListElement.Count.ShouldBe(0);
        }

        private void InitializeFakeConstructors()
        {
            ShimClass.Constructor = (instance) =>
            {
                instance.ListElement = new List<Element> { };
            };

            ShimElement.Constructor = (instance) =>
            {
                instance.ListPsuedo = new List<Psuedo>();
            };

            ShimId.Constructor = (instance) =>
            {
                instance.ListElement = new List<Element>();
            };
        }

        private List<Directive> GetDirectives(List<TestInputItems> testInputItemsList)
        {
            var directives = new List<Directive>();
            var directive = new Directive();
            directive.Type = DirectiveType.Charset;
            directive.Name = "A";
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
    }
}