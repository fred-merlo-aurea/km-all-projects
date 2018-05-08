using FrameworkUAD.BusinessLogic;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    public partial class ValidationResultTest
    {
        [Test]
        public void GetCustomerErrorMessageFileValidatorValidationResultStringBool_ExceptionThrown_ExceptionLogged()
        {
            //Arrange
            var hasTextQualifier = true;
            validationResultObject.RecordImportErrors = null;

            //Act
            var validator = new ValidationResult();
            validator.GetCustomerErrorMessageFileValidator(validationResultObject, FileNameSample, hasTextQualifier);

            //Assert
            savedLog.ShouldBe(LogSaveSuccess);
        }

        [Test]
        public void GetCustomerErrorMessageFileValidatorValidationResultStringBool_WithTextQualifier_CreatesErrorFile()
        {
            //Arrange
            var hasTextQualifier = true;
            errorMessage = CustomerErrorFileValidatorResult(hasTextQualifier);

            //Act
            var validator = new ValidationResult();
            validator.GetCustomerErrorMessageFileValidator(validationResultObject, FileNameSample, hasTextQualifier);
            var fileContentBuilt = fileContentBuilder.ToString();

            //Assert
            fileContentBuilt.ShouldSatisfyAllConditions(
                () => fileContentBuilt.ShouldBe(errorMessage),
                () => previousFileDelete.ShouldBeTrue(),
                () => filePathUsed.ShouldBe(FileFullPath));
        }

        [Test]
        public void GetCustomerErrorMessageFileValidatorValidationResultStringBool_WithoutTextQualifier_CreatesErrorFile()
        {
            //Arrange
            var hasTextQualifier = false;
            errorMessage = CustomerErrorFileValidatorResult(hasTextQualifier);

            //Act
            var validator = new ValidationResult();
            validator.GetCustomerErrorMessageFileValidator(validationResultObject, FileNameSample, hasTextQualifier);
            var fileContentBuilt = fileContentBuilder.ToString();

            //Assert
            fileContentBuilt.ShouldSatisfyAllConditions(
                () => fileContentBuilt.ShouldBe(errorMessage),
                () => previousFileDelete.ShouldBeTrue(),
                () => filePathUsed.ShouldBe(FileFullPath));
        }

        [Test]
        public void GetCustomerErrorMessageFileValidatorValidationResultSourceFile_ExceptionThrown_ExceptionLogged()
        {
            //Arrange
            var sourceFile = new SourceFile();
            sourceFile.FileName = FileNameSample;
            validationResultObject.RecordImportErrors = null;

            //Act
            var validator = new ValidationResult();
            validator.GetCustomerErrorMessageFileValidator(validationResultObject, sourceFile);

            //Assert
            savedLog.ShouldBe(LogSaveSuccess);
        }

        [Test]
        public void GetCustomerErrorMessageFileValidatorValidationResultSourceFile_WithTextQualifier_CreatesErrorFile()
        {
            //Arrange
            var sourceFile = new SourceFile();
            sourceFile.FileName = FileNameSample;
            sourceFile.IsTextQualifier = true;
            errorMessage = CustomerErrorFileValidatorResult(sourceFile.IsTextQualifier);

            //Act
            var validator = new ValidationResult();
            validator.GetCustomerErrorMessageFileValidator(validationResultObject, sourceFile);
            var fileContentBuilt = fileContentBuilder.ToString();

            //Assert
            fileContentBuilt.ShouldSatisfyAllConditions(
                () => fileContentBuilt.ShouldBe(errorMessage),
                () => previousFileDelete.ShouldBeTrue(),
                () => filePathUsed.ShouldBe(FileFullPath));
        }

        [Test]
        public void GetCustomerErrorMessageFileValidatorValidationResultSourceFile_WithoutTextQualifier_CreatesErrorFile()
        {
            //Arrange
            var sourceFile = new SourceFile();
            sourceFile.FileName = FileNameSample;
            sourceFile.IsTextQualifier = false;
            errorMessage = CustomerErrorFileValidatorResult(sourceFile.IsTextQualifier);

            //Act
            var validator = new ValidationResult();
            validator.GetCustomerErrorMessageFileValidator(validationResultObject, sourceFile);
            var fileContentBuilt = fileContentBuilder.ToString();

            //Assert
            fileContentBuilt.ShouldSatisfyAllConditions(
                () => fileContentBuilt.ShouldBe(errorMessage),
                () => previousFileDelete.ShouldBeTrue(),
                () => filePathUsed.ShouldBe(FileFullPath));
        }
    }
}
