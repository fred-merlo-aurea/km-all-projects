using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using EntityEncryption = KM.Common.Entity.Encryption;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EncryptionTests
    {
        private const string AlphaNumericCharsetRegexPattern = "^[a-zA-Z0-9]+$";

        [Test]
        public void GeneratePassword_ReturnsRandomPassword()
        {
            // Arrange
            var length = 10;
            var numberOfNonAlphaNumericChars = 2;
            var encryption = new Encryption();

            // Act
            var actualResult = encryption.GeneratePassword(length, numberOfNonAlphaNumericChars);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBe(string.Empty),
                () => actualResult.Length.ShouldBe(length));
        }

        [Test]
        public void GeneratePassword_NoNonAlphaChars_ReturnsRandomPasswordWithoutNonAlphaChars()
        {
            // Arrange
            var length = 10;
            var numberOfNonAlphaNumericChars = 0;            
            var encryption = new Encryption();

            // Act
            var actualResult = encryption.GeneratePassword(length, numberOfNonAlphaNumericChars);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.Length.ShouldBe(length),
                () => actualResult.ShouldMatch(AlphaNumericCharsetRegexPattern));
        }

        [Test]
        public void GeneratePassword_AllNonAlphaChars_ReturnsRandomPasswordWithAllNonAlphaChars()
        {
            // Arrange
            var length = 10;
            var numberOfNonAlphaNumericChars = 10;
            var encryption = new Encryption();

            // Act
            var actualResult = encryption.GeneratePassword(length, numberOfNonAlphaNumericChars);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.Length.ShouldBe(length),
                () => actualResult.ShouldNotMatch(AlphaNumericCharsetRegexPattern),
                () => actualResult
                    .Count(x => !Char.IsLetterOrDigit(x))
                    .ShouldBe(numberOfNonAlphaNumericChars));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(0, -2)]
        [TestCase(-2, 0)]
        [TestCase(5, -2)]
        [TestCase(5, 6)]
        public void GeneratePassword_InvalidArguments_ThrowsException(int length, int numberOfNonAlphaNumericChars)
        {
            // Arrange
            var encryption = new Encryption();

            // Act & Assert
            Should.Throw<ArgumentOutOfRangeException>(
                () => encryption.GeneratePassword(length, numberOfNonAlphaNumericChars));
        }

        [Test]
        public void GetRandomSalt_ReturnsRandomSalt()
        {
            // Arrange
            var expectedLength = 64;
            var expectedNumberOfNonAlphaNumericChars = 12;
            var encryption = new Encryption();

            // Act
            var actualResult = encryption.GetRandomSalt();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.Length.ShouldBe(expectedLength),
                () => actualResult
                    .Count(x => !Char.IsLetterOrDigit(x))
                    .ShouldBe(expectedNumberOfNonAlphaNumericChars));
        }

        [Test]
        public void Encrypt_ValidText_ReturnsEncryptedText()
        {
            // Arrange
            var text = "text";
            var expectedResult = "1yg1idiIOgJOLv0Io9MAUg==";
            var encryptionData = GetEncryptionData();

            // Act
            var actualResult = Encryption.Encrypt(text, encryptionData);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldBe(expectedResult));
        }

        [Test]
        public void Decrypt_ValidEncryptedText_ReturnsDecryptedText()
        {
            // Arrange
            var text = "1yg1idiIOgJOLv0Io9MAUg==";
            var expectedResult = "text";
            var encryptionData = GetEncryptionData();

            // Act
            var actualResult = Encryption.Decrypt(text, encryptionData);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldBe(expectedResult));
        }

        private static EntityEncryption GetEncryptionData()
        {
            return new EntityEncryption
            {
                PassPhrase = "a}MTLboFrVa1Q)V~s|DpyiRccV'<5j*>0umZ4o0m(YbS{,i^w2K'[>WFj:e[-+R",
                SaltValue = "kbujysrefbiu34f82rfhwekfdhunbswiukfw3efsaef",
                HashAlgorithm = "SHA256",
                PasswordIterations = 2,
                InitVector = "U^.6V#Gy,94@pQU]",
                KeySize = 256
            };
        }
    }
}
