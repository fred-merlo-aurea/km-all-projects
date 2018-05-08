using Core_AMS.Utilities;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities.FileWorker_cs
{
    [TestFixture]
    public class CheckTextFileHeadersForUnicodeCharsTest: Fakes
    {
        [Test]
        public void CheckTextFileHeadersForUnicodeChars_NoUnicode_NoUnicodeHeaders()
        {
            // Arrange
            FillFileHeaderCol2NonUnicodeRowCol2();

            var worker = new FileWorker();

            // Act
            var headers = worker.CheckTextFileHeadersForUnicodeChars(FileInfo1, null);

            // Assert
            headers.ShouldNotBeNull();
            headers.Count.ShouldBe(2);
            headers[FieldFileCol1Name].ShouldBeFalse();
            headers[FieldFileCol2Name].ShouldBeFalse();
        }

        [Test]
        public void CheckTextFileHeadersForUnicodeChars_Unicode_UnicodeHeaders()
        {
            // Arrange
            FillFileHeaderCol2UnicodeRowCol2();

            var worker = new FileWorker();

            // Act
            var headers = worker.CheckTextFileHeadersForUnicodeChars(FileInfo1, null);

            // Assert
            headers.ShouldNotBeNull();
            headers.Count.ShouldBe(2);
            headers[FieldFileCol1UnicodeName].ShouldBeTrue();
            headers[FieldFileCol1UnicodeName].ShouldBeTrue();
        }
    }
}
