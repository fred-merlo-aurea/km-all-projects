using System;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Windows.Media;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class StringFunctionsTests
    {
        private const string NonLettersRegexPattern = "[^a-zA-Z]";
        private const string DateTimeFormat = "MMddyyyy_HH:mm:ss";
        private const string NonNumbersRegexPattern = "[^0-9]";
        private const string WhiteColorString = "#ffffff";
        private const string UnderscoreChar = "_";

        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void FriendlyServiceError_ReturnsCorrectMessage()
        {
            // Arrange
            var expectedMessage =
                "An error has occurred with your service call.  " +
                "Technical support has been notified.  If the problem is still " +
                "present in 24 hours please contact Customer Support.";

            // Act
            var message = StringFunctions.FriendlyServiceError;

            // Assert
            message.ShouldBe(expectedMessage);
        }

        [Test]
        public void GenerateProcessCode_GeneratesValidCode()
        {
            // Arrange
            ShimDateTime.NowGet = () => DateTime.MinValue;
            var date = DateTime.MinValue.ToString(DateTimeFormat);
            var expectedCodeEndsWith = $"_{date}";
            var expectedCodeLength = expectedCodeEndsWith.Length + 12;

            // Act
            var code = StringFunctions.GenerateProcessCode();

            // Assert
            code.ShouldSatisfyAllConditions(
                () => code.Length.ShouldBe(expectedCodeLength),
                () => code.ShouldEndWith(expectedCodeEndsWith, Case.Insensitive));
        }

        [Test]
        [TestCase("", "")]
        [TestCase("  ", "")]
        [TestCase(null, "")]
        [TestCase("abc", "")]
        [TestCase("1234567890", "1234567890")]
        [TestCase("(1234)567890", "1234567890")]
        [TestCase("1234-567.890", "1234567890")]
        [TestCase("1234.567.890", "1234567890")]
        [TestCase("1234 567 890", "1234567890")]
        [TestCase("+1234 567 890", "1234567890")]
        [TestCase("1234567890", "123-456-7890", true)]
        [TestCase("(1234)567890", "123-456-7890", true)]
        [TestCase("1234-567.890", "123-456-7890", true)]
        [TestCase("1234.567.890", "123-456-7890", true)]
        [TestCase("1234 567 890", "123-456-7890", true)]
        [TestCase("+1234 567 890", "123-456-7890", true)]
        public void FormatPhoneNumber_PhoneNumber_ReturnsFormattedNumber(
            string phoneNumber,
            string expectedResult,
            bool useDashes = false)
        {
            // Act
            var actualResult = StringFunctions.FormatPhoneNumber(phoneNumber, useDashes);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase(null, null)]
        [TestCase("aBc", "abc")]
        public void ToLower_StringInput_ReturnsLowerCaseString(string input, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.ToLower(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase(null, null)]
        [TestCase("aBc", "ABC")]
        public void ToUpper_StringInput_ReturnsUpperCaseString(string input, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.ToUpper(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        public void GetRandomAlphaString_LengthInput_ReturnsRandomAlphaString()
        {
            // Arrange
            var length = 10;
            var expectedResultLength = 10;

            // Act
            var actualResult = StringFunctions.GetRandomAlphaString(length);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.Length.ShouldBe(expectedResultLength),
                () => actualResult.ShouldMatch(NonLettersRegexPattern));
        }

        [Test]
        public void RandomAlphaNumericString_LengthInput_ReturnsRandomAlphaNumericString()
        {
            // Arrange
            var length = 10;
            var expectedResultLength = 10;

            // Act
            var actualResult = StringFunctions.GetRandomAlphaNumericString(length);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.Length.ShouldBe(expectedResultLength),
                () => actualResult.ShouldMatch(NonNumbersRegexPattern));
        }

        [Test]
        public void GetRandomString_LengthInput_ReturnsRandomString()
        {
            // Arrange
            var length = 10;
            var specialsLength = 5;
            var expectedResultLength = 10;

            // Act
            var actualResult = StringFunctions.GetRandomString(length, specialsLength);

            // Assert
            actualResult.Length.ShouldBe(expectedResultLength);
        }

        [Test]
        public void GetColor_ColorString_ReturnsCorrespondingColor()
        {
            // Arrange
            var color = new Color
            {
                R = 255,
                G = 255,
                B = 255
            };
            var colorString = WhiteColorString;

            // Act
            var actualResult = StringFunctions.GetColor(colorString);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.R.ShouldBe(color.R),
                () => actualResult.G.ShouldBe(color.G),
                () => actualResult.B.ShouldBe(color.B));
        }

        [Test]
        [TestCase("href=\"http://url#anchor1\"", "href=\"http://url%23anchor1\"")]
        [TestCase(
            "href=\"http://www.google.com/search#anchor1\"",
            "href=\"http://www.google.com/search%23anchor1\"")]
        public void ReplaceAnchor_ValidText_ReturnsTextWithoutAnchor(string anchor, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.ReplaceAnchor(anchor);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase(@"<xml>""test&node'1</xml>", @"&lt;xml&gt;&quot;test&amp;node&apos;1&lt;/xml&gt;")]
        [TestCase(@"<xml></xml>", @"&lt;xml&gt;&lt;/xml&gt;")]
        public void EscapeXmlString_XmlString_ReturnsCleanedXmlString(string xmlString, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.EscapeXmlString(xmlString);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("new test", "New Test")]
        [TestCase("newtest", "Newtest")]
        [TestCase("new.test", "New.Test")]
        [TestCase("new\ttest", "New\tTest")]
        [TestCase("new\ntest", "New\nTest")]
        public void ToTitleCase_ValidString_ReturnsTitleCasedString(string input, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.ToTitleCase(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("abc", "cba")]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase(null, null)]
        public void Reverse_ValidString_ReturnsReversedString(string input, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.Reverse(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("abc", false)]
        [TestCase("abba", true)]
        [TestCase("aba", true)]
        [TestCase("abab", false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase(null, false)]
        public void IsPalindrome_ValidString_ReturnsIfStringIsPalindrome(string input, bool expectedResult)
        {
            // Act
            var actualResult = StringFunctions.IsPalindrome(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("abcdef", 2, "ab")]
        [TestCase("abcdef", -1, "a")]
        [TestCase("abcdef", 10, "abcdef")]
        public void Left_ValidInput_ReturnsCorrectString(string input, int length, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.Left(input, length);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("abcdef", 2, "ef")]
        [TestCase("abcdef", -1, "f")]
        [TestCase("abcdef", 10, "abcdef")]
        public void Right_ValidInput_ReturnsCorrectString(string input, int length, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.Right(input, length);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("\"test\"", "test")]
        [TestCase("'test'", "test")]
        public void TrimQuotes_ValidInput_RemovesQuotes(string input, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.TrimQuotes(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("'test'", "_test_")]
        [TestCase("*123", "_123")]
        [TestCase("*az,.:'\"!@#$%^&*()_+~`<>/?;|[]{}123", "_az_____________________________123")]
        public void ReplaceNonAlphaNumeric_ValidInput_CharsReplaced(
            string input,
            string expectedResult)
        {
            // Arrange
            var replacement = UnderscoreChar;

            // Act
            var actualResult = StringFunctions.ReplaceNonAlphaNumeric(input, replacement);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("'test'", "test")]
        [TestCase("*123", "123")]
        [TestCase("*az,.:'\"!@#$%^&*()_+~`<>/?;|[]{}123", "az123")]
        public void RemoveNonAlphaNumeric_ValidInput_CharsRemoved(
            string input,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.RemoveNonAlphaNumeric(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("’test’", "'test'")]
        [TestCase("–“123”…", "-\"123\"...")]
        public void CleanString_ValidInput_StringCleaned(
            string input,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.CleanString(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("\"test\"", "test")]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase(null, null)]
        public void RemoveDoubleQuotes_ValidInput_QuotesRemoved(
            string input,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.RemoveDoubleQuotes(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("'test'", "test")]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase(null, null)]
        public void RemoveSingleQuotes_ValidInput_QuotesRemoved(
            string input,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.RemoveSingleQuotes(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("'test'", "test")]
        [TestCase("*123", "123")]
        [TestCase("*az,.:'\"!@#$%^&*()_+~`<>/?;|[]{}123", "az123")]
        public void StripString_ValidInput_NonAlphaCharsRemoved(
            string input,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.StripString(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("test@test.com", true)]
        [TestCase("123test@test.com", true)]
        [TestCase("testattest.com", false)]
        [TestCase("test@test", false)]
        [TestCase("test@", false)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void IsEmail_DifferentInputs_ValidatesEmail(
            string input,
            bool expectedResult)
        {
            // Act
            var actualResult = StringFunctions.IsEmail(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("test", "es", true, 1)]
        [TestCase("tEst", "es", false, 0)]
        [TestCase("test test", "es", true, 2)]
        [TestCase("test teSt", "es", false, 1)]
        [TestCase("", "es", true, 0)]
        [TestCase(null, "es", true, 0)]
        public void CountChars_DifferentInputs_ReturnsNumberOfOccurences(
            string text,
            string textToFind,
            bool ignoreCase,
            int expectedResult)
        {
            // Act
            var actualResult = StringFunctions.CountChars(text, textToFind, ignoreCase);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("test  test", "test test")]
        [TestCase("test       test", "test test")]
        [TestCase("  test       test   ", " test test ")]
        public void ToSingleSpace_DifferentInputs_RemovesDoubleSpaces(
            string input,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.ToSingleSpace(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("test", "es", "*", "t*t")]
        [TestCase("tESt", "es", "*", "t*t")]
        [TestCase("test", "a", "*", "test")]
        public void Replace_DifferentInputs_ReturnsNumberOfOccurences(
            string text,
            string textToFind,
            string textToReplaceWith,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.Replace(text, textToFind, textToReplaceWith);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase(null, "null", "null")]
        [TestCase(null, "", " ")]
        [TestCase(null, null, " ")]
        public void DbTypeToString_DifferentInputs_ReturnsConvertedValue(
            object input,
            string equivalent,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.DbTypeToString(input, equivalent);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("this is a test", "i", "ths s a test")]
        [TestCase("this is a test", "ies", "th  a tt")]
        public void Remove_DifferentInputs_RemoveAllCharsInSet(
            string input,
            string setToRemove,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.Remove(input, setToRemove);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase("&nbsp;&copy;&reg;&trade;<br><BR><p><P>", " \r\n\r\n\r\n\r\n")]
        [TestCase("<html>text</html>", "text")]
        public void CleanHtmlString_DifferentInputs_HtmlFormattingRemoved(string input, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.CleanHtmlString(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase(123, "one hundred and twenty three")]
        [TestCase(11, "eleven")]
        [TestCase(5, "five")]
        [TestCase(1200, "one thousand two hundred")]
        [TestCase(110000, "one hundred and ten  thousand")]
        public void GetNumberInWordsFormat_DifferentInputs_ReturnsNumberInWords(int input, string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.GetNumberInWordsFormat(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase(",'%*#--&<>;xp_/**/_test", "//test")]
        public void CleanSqlString_DifferentInputs_ReturnsSqlCleanedString(
            string input,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.CleanSqlString(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        [TestCase(@"file><""|1:*?/.txt", "file_1_.txt")]
        public void CleanFileName_DifferentInputs_ReturnsValidFilename(
            string input,
            string expectedResult)
        {
            // Act
            var actualResult = StringFunctions.CleanFileName(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }


        [TestCase(@"41", @"A")]
        [TestCase(@"41-41", @"AA")]
        [TestCase(@"1F600-1F600", @"😀😀")]
        public void ToUnicode_DifferentInputs_ReturnsValidFilename(
            string input,
            string expectedResult)
        {
            // Arrange, Act
            var actualResult = StringFunctions.ToUnicode(input);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        public void ToUnicode_NullInput_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Should.Throw<ArgumentNullException>(() => StringFunctions.ToUnicode(null));
        }
    }
}
