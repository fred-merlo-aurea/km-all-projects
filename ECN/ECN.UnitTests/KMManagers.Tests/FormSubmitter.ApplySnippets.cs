using System.Collections.Generic;
using KMEntities;
using NUnit.Framework;
using Shouldly;

namespace KMManagers.Tests
{
    [TestFixture]
    public partial class FormSubmitterTest
    {
        private const string TestedMethodName_ApplySnippets = "ApplySnippets";
        private const string HtmlThatDoesNotMatchSnippetRegex = "<span name=\"label\" class=\"value-label\"></span>";
        private const string HtmlThatMatchesSnippetRegex = "%%<span cid=\"{0}\"></span>%%";
        private const string HtmlThatMatchesSnippetRegexWithLabelValue = "%%<span cid=\"{0}\">{1}</span>%%";
        private const string FieldLabelValue = "fieldLabelValue";
        private const string FieldLabelValue_2 = "fieldLabelValue_2";

        [Test]
        public void ApplySnippets_WithouMatchingSnippet_ReturnsSameHtml()
        {
            //Arrange
            var htmlToChange = HtmlThatDoesNotMatchSnippetRegex;
            var controls = CreateControls(ControlID, FieldLabelValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldBeSameAs(htmlToChange);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_ReturnsDifferentHtml()
        {
            //Arrange
            var htmlToChange = GetHtmlToChange(ControlID_2);
            var controls = CreateControls(ControlID, FieldLabelValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldNotBeSameAs(htmlToChange);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_MatchingControlID_StateFieldValue_ReturnsDifferentHtml()
        {
            //Arrange
            var htmlToChange = GetHtmlToChange(ControlID);
            var controls = CreateControls(ControlID, FieldLabelValue, HtmlControltypeAsCheckbox, TypeSeqIDForStates);

            _values.Add(ControlID, DummyIntValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldNotBeSameAs(htmlToChange);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_MatchingControlID_CountryFieldValue_ReturnsDifferentHtml()
        {
            //Arrange
            var htmlToChange = GetHtmlToChange(ControlID);
            var controls = CreateControls(ControlID, FieldLabelValue, HtmlControltypeAsCheckbox, TypeSeqIDForCountries);

            _values.Add(ControlID, DummyIntValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldNotBeSameAs(htmlToChange);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_MatchingControlID_StateFieldValue_CollectionValueEmpty_ReturnsSameHtml()
        {
            //Arrange
            var htmlToChange = HtmlThatDoesNotMatchSnippetRegex;
            var controls = CreateControls(ControlID, FieldLabelValue, HtmlControltypeAsListbox, TypeSeqIDForStates);

            _values.Add(ControlID, EmptyValue);
            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldBeSameAs(htmlToChange);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_MatchingControlID_ControlTypeAsNewsletter_ReturnsDifferentHtml()
        {
            //Arrange
            var htmlToChange = GetHtmlToChange(ControlID);
            var controls = CreateControls(controlID: ControlID,
                                          fieldLabelValue: FieldLabelValue,
                                          mainTypeId: HtmlControltypeAsNewsLetter);

            ChangeFirstControlToAddNewsletterGroup(controls);

            _values.Add(ControlID, DummyIntValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldNotBeSameAs(htmlToChange);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_MatchingControlID_ControlTypeAsNewsletter_MathingNewsletterGroupID_ReturnsSubscribeHtml()
        {
            //Arrange
            var htmlToChange = GetHtmlToChange(ControlID);
            var controls = CreateControls(controlID: ControlID,
                                          fieldLabelValue: FieldLabelValue,
                                          mainTypeId: HtmlControltypeAsNewsLetter);

            ChangeFirstControlToAddNewsletterGroup(controls);

            _values.Add(ControlID, GroupID.ToString());

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(HTMLGenerator.Subscribe);
        }


        [Test]
        public void ApplySnippets_MatchingSnippetsAndControlID_ControlTypeAsNewsletter_ValueEmpty_ReturnsUnsubscribeHtml()
        {
            //Arrange
            var htmlToChange = GetHtmlToChange(ControlID);
            var controls = CreateControls(controlID: ControlID,
                                          fieldLabelValue: FieldLabelValue,
                                          mainTypeId: HtmlControltypeAsNewsLetter);

            ChangeFirstControlToAddNewsletterGroup(controls);

            _values.Add(ControlID, EmptyValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(HTMLGenerator.Unsubscribe);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_MailingControlType_ReturnsDifferentHtml()
        {
            //Arrange
            var htmlToChange = GetHtmlToChange(ControlID);
            var controls = CreateControls(controlID: ControlID,
                                          fieldLabelValue: FieldLabelValue,
                                          typeSeqID: TypeSeqIDForMailing);

            var mailingGroupControl = CreateControl(controlID: ControlID_3,
                                                    fieldLabelValue: FieldLabelValue_2,
                                                    typeSeqID: TypeSeqIDForMailingPassword);

            controls.Add(mailingGroupControl);
            _values.Add(ControlID_2, DummyStringValue);
            _values.Add(ControlID_3, DummyEmailAddress);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(MailingProfilePswdColumnValue);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_WithoutMatchingSnippetValue_ReturnsSnippetValue()
        {
            //Arrange
            var snippetValue = DummyStringValue;
            var htmlToChange = GetHtmlToChange(ControlID_2, HtmlThatMatchesSnippetRegexWithLabelValue, snippetValue);
            var controls = CreateControls(ControlID, FieldLabelValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(snippetValue);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_MatchingSnippetValue_ReturnsCollectionValue()
        {
            //Arrange   
            var collectionValue = DummyStringValue;
            var htmlToChange = GetHtmlToChange(ControlID, HtmlThatMatchesSnippetRegexWithLabelValue, FieldLabelValue);
            var controls = CreateControls(ControlID_2, FieldLabelValue);

            _values.Add(ControlID_2, collectionValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(collectionValue);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_MatchingSnippetValue_GridControl_ReturnsGridValue()
        {
            //Arrange               
            var htmlToChange = GetHtmlToChange(ControlID, HtmlThatMatchesSnippetRegexWithLabelValue, FieldLabelValue);
            var controls = CreateControls(controlID: ControlID_2,
                                          fieldLabelValue: FieldLabelValue,
                                          typeSeqID: TypeSeqIDForGrid);

            _values.Add(ControlID_2, DummyStringValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(SnippetForGrid);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_MatchingSnippetValue_LiteralControl_ReturnsLiteralValue()
        {
            //Arrange               
            var htmlToChange = GetHtmlToChange(ControlID, HtmlThatMatchesSnippetRegexWithLabelValue, FieldLabelValue);
            var controls = CreateControls(controlID: ControlID_2,
                                          fieldLabelValue: FieldLabelValue,
                                          typeSeqID: TypeSeqIDForLiteral);

            _values.Add(ControlID_2, DummyStringValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(SnippetForLiteral);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_MatchingSnippetValue_MailingControl_ReturnsPasswordValue()
        {
            //Arrange               
            var htmlToChange = GetHtmlToChange(ControlID, HtmlThatMatchesSnippetRegexWithLabelValue, MailingProfilePswdColumnName);
            var controls = CreateControls(controlID: ControlID_2,
                                          fieldLabelValue: MailingProfilePswdColumnName);

            var controlForMailingPswd = CreateControl(controlID: ControlID_3,
                                          fieldLabelValue: FieldLabelValue_2,
                                          typeSeqID: TypeSeqIDForMailingPassword);

            controls.Add(controlForMailingPswd);
            _values.Add(ControlID_3, DummyEmailAddress);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(MailingProfilePswdColumnValue);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_MatchingCountryControlValue_ReturnsCountryName()
        {
            //Arrange               
            var htmlToChange = GetHtmlToChange(ControlID, HtmlThatMatchesSnippetRegexWithLabelValue, FieldLabelValue);
            var controls = CreateControls(controlID: ControlID_2,
                                          fieldLabelValue: FieldLabelValue,
                                          typeSeqID: TypeSeqIDForCountries);

            _values.Add(ControlID_2, DummyIntValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(CountryName);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_MatchingStateControlValue_ReturnsRegionCode()
        {
            //Arrange               
            var htmlToChange = GetHtmlToChange(ControlID, HtmlThatMatchesSnippetRegexWithLabelValue, FieldLabelValue);
            var controls = CreateControls(controlID: ControlID_2,
                                          fieldLabelValue: FieldLabelValue,
                                          typeSeqID: TypeSeqIDForStates);

            ChangeFirstControlToAddNewsletterGroup(controls);

            _values.Add(ControlID_2, DummyIntValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldContain(RegionCode);
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_NewsletterControl_ReturnsFieldLabelValuePlusSubscribe()
        {
            //Arrange               
            var htmlToChange = GetHtmlToChange(ControlID, HtmlThatMatchesSnippetRegexWithLabelValue, FieldLabelValue);
            var controls = CreateControls(controlID: ControlID_2,
                                          fieldLabelValue: FieldLabelValue,
                                           mainTypeId: HtmlControltypeAsNewsLetter);

            ChangeFirstControlToAddNewsletterGroup(controls, FieldLabelValue);

            _values.Add(ControlID_2, GroupID.ToString());

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldSatisfyAllConditions
                (
                    () => returnResult.ShouldContain(FieldLabelValue),
                    () => returnResult.ShouldContain(HTMLGenerator.Subscribe)
                );
        }

        [Test]
        public void ApplySnippets_MatchingSnippets_WithoutMatchingControlID_NewsletterControl_ReturnsFieldLabelValuePlusUnsubscribe()
        {
            //Arrange               
            var htmlToChange = GetHtmlToChange(ControlID, HtmlThatMatchesSnippetRegexWithLabelValue, FieldLabelValue);
            var controls = CreateControls(controlID: ControlID_2,
                                          fieldLabelValue: FieldLabelValue,
                                           mainTypeId: HtmlControltypeAsNewsLetter);

            ChangeFirstControlToAddNewsletterGroup(controls, FieldLabelValue);

            _values.Add(ControlID_2, EmptyValue);

            var methodArguments = new object[] { controls, htmlToChange };

            //Act
            var returnResult = (string)_formSubmitter.Invoke(TestedMethodName_ApplySnippets, methodArguments);

            //Assert
            returnResult.ShouldSatisfyAllConditions
                (
                    () => returnResult.ShouldContain(FieldLabelValue),
                    () => returnResult.ShouldContain(HTMLGenerator.Unsubscribe)
                );
        }

        private static void ChangeFirstControlToAddNewsletterGroup(List<Control> controls, string labelHtml = "")
        {
            var controlItem = controls[0];
            controlItem.NewsletterGroups = new List<NewsletterGroup>();
            controlItem.NewsletterGroups.Add(new NewsletterGroup()
            {
                GroupID = GroupID,
                LabelHTML = labelHtml
            });
        }

        private static string GetHtmlToChange(int controlID, string HTMLToChange = HtmlThatMatchesSnippetRegex, string labelValue = "")
        {
            if (string.IsNullOrEmpty(labelValue))
            {
                return string.Format(HTMLToChange, controlID.ToString());
            }
            else
            {
                return string.Format(HTMLToChange, controlID.ToString(), labelValue);
            }

        }

    }
}
