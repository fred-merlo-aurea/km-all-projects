using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using KM.Common.Functions;
using NUnit.Framework;
using Shouldly;
using static System.Reflection.BindingFlags;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class KeyLineComputationTest
    {
        private const string AlphaToNumericMethodName = "AlphaToNumeric";
        private const string ComputeAdditionMethodName = "ComputeAddition";
        private const string ComputeCheckDigitMethodName = "ComputeCheckDigit";
        private const int KeyLineHelperNumber = 1337;
        private const string EmptyKeyLinResult = "#////////4#";
        private const string ErrorResult = "error";
        private const char UnderScore = '_';
        private const string ErrorMessageFormatString = "A character or digit was not recognized in your input: {0}. This record will not be included in file.";

        private static IEnumerable<(char, int)> GetAlphaToNumericTestData()
        {
            yield return ('A', 1);
            yield return ('B', 2);
            yield return ('C', 3);
            yield return ('D', 4);
            yield return ('E', 5);
            yield return ('F', 6);
            yield return ('G', 7);
            yield return ('H', 8);
            yield return ('I', 9);
            yield return ('J', 10);
            yield return ('K', 11);
            yield return ('L', 12);
            yield return ('M', 13);
            yield return ('N', 14);
            yield return ('O', 15);
            yield return ('P', 0);
            yield return ('Q', 1);
            yield return ('R', 2);
            yield return ('S', 3);
            yield return ('T', 4);
            yield return ('U', 5);
            yield return ('V', 6);
            yield return ('W', 7);
            yield return ('X', 8);
            yield return ('Y', 9);
            yield return ('Z', 10);
            yield return ('/', 15);
        }

        private static IEnumerable<char> GetAlphaToNumericInvalidTestData()
        {
            yield return '#';
            yield return 'a';
            yield return UnderScore;
            yield return ' ';
        }

        public static object CallMethod(string methodName, object[] parametersValues = null, object instance = null)
        {
            parametersValues = parametersValues ?? new object[0];
            var methodInfo = typeof(KeyLineComputation)
                .GetMethods(Instance | NonPublic | Public | Static)
                .FirstOrDefault(info => info.Name == methodName);

            return methodInfo?.Invoke(methodInfo.IsStatic ? null : instance, parametersValues);
        }

        [Test]
        [TestCaseSource(nameof(GetAlphaToNumericTestData))]
        public void AlphaToNumeric_ValidInput_NumericReturned((char alphaChar, int intValue) testData)
        {
            // Arrange
            var keyLineComputation = new KeyLineComputation();

            // Act
            var result = CallAlphaToNumeric(keyLineComputation, testData.alphaChar);

            // Assert
            result.ShouldBe(testData.intValue);
        }       
        
        [Test]
        [TestCaseSource(nameof(GetAlphaToNumericInvalidTestData))]
        public void AlphaToNumeric_ValidInput_NumericReturned(char testData)
        {
            // Arrange
            var keyLineComputation = new KeyLineComputation();

            // Act
            var result = CallAlphaToNumeric(keyLineComputation, testData);

            // Assert
            result.ShouldBe(-1);
        }

        [Test]
        public void ComputeAddition_AlphaNumNPositionOdd_Returns1()
        {
            // Arrange
            var keyLineComputation = new KeyLineComputation();

            // Act
            var result = CallComputeAddition(keyLineComputation, 'N', KeyLineHelperNumber, 1);

            // Assert
            result.ShouldBe(1);
        } 
        
        [Test]
        [TestCase(0, 0)]
        [TestCase(1337, 14)]
        [TestCase(1010, 2)]
        public void ComputeAddition_ValidInput_ReturnsSumOfDigits(int input, int sumOfDigits)
        {
            // Arrange
            var keyLineComputation = new KeyLineComputation();

            // Act
            var result = CallComputeAddition(keyLineComputation, UnderScore, input, 1);

            // Assert
            result.ShouldBe(sumOfDigits);
        }
        
        [Test]
        public void ComputeCheckDigit_EmptyInput_ReturnsSumOfDigits()
        {
            // Arrange
            var keyLineComputation = new KeyLineComputation();

            // Act
            var result = CallComputeCheckDigit(keyLineComputation, Enumerable.Empty<KeyLineHelper>().ToList());

            // Assert
            result.ShouldBe(0);
        }  
        
        [Test]
        public void ComputeCheckDigit_OddInput_ReturnsSumOfDigits()
        {
            // Arrange
            var keyLineComputation = new KeyLineComputation();
            var keylineHelper = new KeyLineHelper(UnderScore, 42, 1);

            // Act
            var result = CallComputeCheckDigit(keyLineComputation, new List<KeyLineHelper>() { keylineHelper });

            // Assert
            result.ShouldBe(8);
        } 
        
        [Test]
        public void ComputeCheckDigit_OddAndEvenInput_ReturnsSumOfDigits()
        {
            // Arrange
            var keyLineComputation = new KeyLineComputation();
            var keylineHelperList = new List<KeyLineHelper>()
            {
                new KeyLineHelper(UnderScore, 1, 1),
                new KeyLineHelper(UnderScore, 4, 1),
                new KeyLineHelper(UnderScore, 8, 1),
            };

            // Act
            var result = CallComputeCheckDigit(keyLineComputation, keylineHelperList);

            // Assert
            result.ShouldBe(7);
        }
        
        [Test]
        [TestCase("")]
        [TestCase("                ")]
        public void Compute_EmptyInput_ReturnsSumOfDigits(string emptyString)
        {
            // Act
            var result = KeyLineComputation.Compute(emptyString);

            // Assert
            result.ShouldBe(EmptyKeyLinResult);
        } 
        
        [Test]
        [TestCase("12345678", "#123456786#")]
        [TestCase("AB49824", "#AB49824/6#")]
        [TestCase(" A B 4 9 8 24", "#AB49824/6#")]
        [TestCase(" 24A B24", "#24AB24//1#")]
        [TestCase(" 42", "#42//////3#")]
        public void Compute_ValidInput_ReturnsSumOfDigits(string inputString, string outputString)
        {
            // Act
            var result = KeyLineComputation.Compute(inputString);

            // Assert
            result.ShouldBe(outputString);
        }
        
        [Test]
        [TestCase(".")]
        [TestCase("a")]
        [TestCase("#")]
        public void Compute_InvalidInput_ReturnsError(string inputString)
        {
            // Act
            var result = KeyLineComputation.Compute(inputString);

            // Assert
            result.ShouldBe(ErrorResult);
        } 
        
        [Test]
        [TestCase(".", ".///////")]
        [TestCase("a", "a///////")]
        [TestCase("#", "#///////")]
        public void Compute_InvalidInput_ReturnsErrorAndWritesErrorMessage(string inputString, string paddedString)
        {
            // Arrange
            string errorMessage = null;

            // Act
            var result = KeyLineComputation.Compute(inputString, msg => errorMessage = msg);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(ErrorResult),
                () => errorMessage.ShouldBe(
                    string.Format(
                        ErrorMessageFormatString,
                        paddedString)));
        }

        private static int CallAlphaToNumeric(KeyLineComputation keyLineComputation, char alphaChar)
        {
            return (int)CallMethod(AlphaToNumericMethodName, new object[] { alphaChar }, keyLineComputation);
        }
        
        private static int CallComputeAddition(KeyLineComputation keyLineComputation, char alpha, int number, int position)
        {
            return (int)CallMethod(ComputeAdditionMethodName, new object[] { alpha, number, position }, keyLineComputation);
        }

        private static int CallComputeCheckDigit(KeyLineComputation keyLineComputation, IReadOnlyCollection<KeyLineHelper> keyLineHelpers)
        {
            return (int)CallMethod(ComputeCheckDigitMethodName, new object[] { keyLineHelpers }, keyLineComputation);
        }
    }
}
