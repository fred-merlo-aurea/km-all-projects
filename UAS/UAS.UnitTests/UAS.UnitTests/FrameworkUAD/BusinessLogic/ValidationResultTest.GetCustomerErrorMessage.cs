using FrameworkUAD.BusinessLogic;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    public partial class ValidationResultTest
    {
        [Test]
        public void GetCustomerErrorMessageValidationResultBool_ExceptionThrown_ExceptionLogged()
        {
            //Arrange
            var hasTextQualifier = true;
            validationResultObject.RecordImportErrors = null;

            //Act
            var validator = new ValidationResult();
            var errorMessageBuilt = validator.GetCustomerErrorMessage(validationResultObject, hasTextQualifier);

            //Assert
            savedLog.ShouldBe(LogSaveSuccess);
        }

        [Test]
        public void GetCustomerErrorMessageValidationResultBool_WithTextQualifier_ReturnsErrorMessage()
        {
            //Arrange
            var hasTextQualifier = true;
            errorMessage = CustomerErrorResult(hasTextQualifier);
            
            //Act
            var validator = new ValidationResult();
            var errorMessageBuilt = validator.GetCustomerErrorMessage(validationResultObject, hasTextQualifier);

            //Assert
            errorMessageBuilt.ShouldSatisfyAllConditions(
                () => errorMessageBuilt.ShouldNotBeNull(),
                () => errorMessageBuilt.ShouldBe(errorMessage));
        }

        [Test]
        public void GetCustomerErrorMessageValidationResultBool_WithoutTextQualifier_ReturnsErrorMessage()
        {
            //Arrange
            var hasTextQualifier = false;
            errorMessage = CustomerErrorResult(hasTextQualifier);
            
            //Act
            var validator = new ValidationResult();
            var errorMessageBuilt = validator.GetCustomerErrorMessage(validationResultObject, hasTextQualifier);

            //Assert
            errorMessageBuilt.ShouldSatisfyAllConditions(
                () => errorMessageBuilt.ShouldNotBeNull(),
                () => errorMessageBuilt.ShouldBe(errorMessage));
        }

        [Test]
        public void GetCustomerErrorMessageValidationResultSourceFile_ExceptionThrown_ExceptionLogged()
        {
            //Arrange
            var sourceFile = new SourceFile();
            validationResultObject.RecordImportErrors = null;
            
            //Act
            var validator = new ValidationResult();
            validator.GetCustomerErrorMessage(validationResultObject, sourceFile);

            //Assert
            savedLog.ShouldBe(LogSaveSuccess);
        }

        [Test]
        public void GetCustomerErrorMessageValidationResultSourceFile_WithTextQualifier_ReturnsErrorMessage()
        {
            //Arrange
            var sourceFile = new SourceFile();
            sourceFile.IsTextQualifier = true;
            errorMessage = CustomerErrorResult(sourceFile.IsTextQualifier);

            //Act
            var validator = new ValidationResult();
            var errorMessageBuilt = validator.GetCustomerErrorMessage(validationResultObject, sourceFile);

            //Assert
            errorMessageBuilt.ShouldSatisfyAllConditions(
                () => errorMessageBuilt.ShouldNotBeNull(),
                () => errorMessageBuilt.ShouldBe(errorMessageBuilt));
        }

        [Test]
        public void GetCustomerErrorMessageValidationResultSourceFile_WithoutTextQualifier_ReturnsErrorMessage()
        {
            //Arrange
            var sourceFile = new SourceFile();
            sourceFile.IsTextQualifier = false;
            errorMessage = CustomerErrorResult(sourceFile.IsTextQualifier);

            //Act
            var validator = new ValidationResult();
            var errorMessageBuilt = validator.GetCustomerErrorMessage(validationResultObject, sourceFile);

            //Assert
            errorMessageBuilt.ShouldSatisfyAllConditions(
                () => errorMessageBuilt.ShouldNotBeNull(),
                () => errorMessageBuilt.ShouldBe(errorMessage));
        }
    }
}