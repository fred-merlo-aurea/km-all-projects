using FrameworkUAD.BusinessLogic;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    public partial class ValidationResultTest
    {
        private const string DqmPath = "C:\\ADMS\\Applications\\DQM\\";

        [Test]
        public void CleanBadDataLocation_WhenFileNameIsEmpty_ReturnsPath()
        {
            // Arrange
            var fileName = string.Empty;
            var filePathResult = ResolveFilePath(fileName);

            // Act
            var result = ValidationResult.CleanBadDataLocation(fileName);

            // Assert
            result.ShouldBe(filePathResult);
        }

        [Test]
        public void CleanBadDataLocation_WhenFileNameIsNotEmpty_ReturnsPath()
        {
            // Arrange
            var fileName = FileNameSample;
            var filePathResult = ResolveFilePath(fileName);

            // Act
            var result = ValidationResult.CleanBadDataLocation(fileName);

            // Assert
            result.ShouldBe(filePathResult);
        }

        private string ResolveFilePath(string fileName)
        {
            var location = $"BadData_{fileName}.csv";
            return $"{DqmPath}{location}";
        }
    }
}
