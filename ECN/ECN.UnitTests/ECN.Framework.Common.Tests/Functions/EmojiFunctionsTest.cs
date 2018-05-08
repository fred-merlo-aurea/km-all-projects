using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN.Tests.Helpers;
using ECN_Framework_Common.Functions;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Common.Tests.Functions
{
    [TestFixture]
    public class EmojiFunctionsTest
    {
        private const char U0800 = (char)2048;
        private const char U00AA = (char)170;
        private const char UD800 = (char)55296;
        private const char UDC00 = (char)56230;
        private const char U0900 = (char)2304;
        private const char U00BB = (char)187;
        private const char U2A00 = (char)10752;
        private string[] _expectedData = null;

        [SetUp]
        public void TestSetup()
        {
            _expectedData = new string[]
                {
                    "Test" + (char)U0800 + (char)U00AA + "End",
                    "PlainData",
                    "Test" + (char)U0800 + (char)U00AA + (char)U0900 + (char)U00BB + "End",
                    "Test" + (char)U2A00 + "End",
                    "Test" + (char)U2A00 + (char)U0800 + (char)U00AA + (char)U0900 + (char)U00BB + "End"
                };
        }

        [Test]
        [TestCase("Test\\u0800\\u00aaEnd", "Test?End")]
        [TestCase("PlainData", "PlainData")]
        [TestCase("Test\\u0800\\u00aa\\u0900\\u00bbEnd", "Test??End")]
        [TestCase("Test\\u2a00End", "Test?End")]
        [TestCase("Test\\u2a00\\u0800\\u00aa\\u0900\\u00bbEnd", "Test???End")]
        public void ReplaceEmojiWithQuestion_ValidData_ReturnsValid(string input, string output)
        {
            // Arrange, Act
            var result = EmojiFunctions.ReplaceEmojiWithQuestion(input);

            // Assert
            result.ShouldBe(output);
        }

        [Test]
        [TestCase("Test\\u0800\\u00aaEnd", 0)]
        [TestCase("PlainData", 1)]
        [TestCase("Test\\u0800\\u00aa\\u0900\\u00bbEnd", 2)]
        [TestCase("Test\\u2a00End", 3)]
        [TestCase("Test\\u2a00\\u0800\\u00aa\\u0900\\u00bbEnd", 4)]
        public void GetSubjectUTF_ValidData_ReturnsValid(string input, int expectedIndex)
        {
            // Arrange, Act
            var result = EmojiFunctions.GetSubjectUTF(input);

            // Assert
            result.ToCharArray().ShouldBe(_expectedData[expectedIndex].ToCharArray());
        }
    }
}
