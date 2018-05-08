using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Fakes;
using KMPS.MD.Tools;
using KMPS.MD.Tools.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Tools
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RecordViewPDFSetupTests : BasePageTests
    {
        private const string SampleFileName = "file1.png";
        private const string SampleInvalidFileName = "file1.exe";
        private const string MethodButtonUploadClick = "btnUpload_Click";
        private const string ErrorFileFormat = "Only files with an extension of";

        private RecordViewPDFSetup _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new RecordViewPDFSetup();
            InitializePage(_testEntity);
        }

        [Test]
        public void btnUpload_Click_FileNameIsValid_SavesFile()
        {
            // Arrange
            var fileSaved = false;
            ShimPostedFileForHtmlInputFile(SampleFileName);
            ShimForFileSystem(true, false);
            ShimHttpPostedFile.AllInstances.SaveAsString = (_, __) => fileSaved = true;

            // Act
            PrivatePage.Invoke(MethodButtonUploadClick, this, EventArgs.Empty);

            // Assert
            fileSaved.ShouldBeTrue();
        }

        [Test]
        public void btnUpload_Click_FileNameIsInvalid_DisplaysError()
        {
            // Arrange
            var hasError = false;
            var errorMessage = string.Empty;
            ShimPostedFileForHtmlInputFile(SampleInvalidFileName);
            ShimRecordViewPDFSetup.AllInstances.DisplayErrorString = (_, error) =>
            {
                hasError = true;
                errorMessage = error;
            };

            // Act
            PrivatePage.Invoke(MethodButtonUploadClick, this, EventArgs.Empty);

            // Assert
            hasError.ShouldBeTrue();
            errorMessage.ShouldContain(ErrorFileFormat);
        }
    }
}
