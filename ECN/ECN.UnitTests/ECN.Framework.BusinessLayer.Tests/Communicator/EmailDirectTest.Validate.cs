using System.Reflection;
using ECN_Framework_Common.Objects;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class EmailDirectTest
    {
        [Test]
        public void Validate_WithEmptyEmailDirectProperties_ThrowsEcnExceptionWithErrorCount()
        {
            // Arrange
            var emailDirect = GetTestDataCase1();

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() => 
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailAddress is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Source is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyEmailAddress is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmailAddress is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Process is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Content is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromName is required")));
        }

        [Test]
        public void Validate_WithInvalidEmailAddressesEmailDirectProperties_ThrowsEcnExceptionWithErrorCount()
        {
            // Arrange
            var emailDirect = GetTestDataCase2();

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailAddress must be a valid email address")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailAddress must be a valid email address")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Source is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyEmailAddress must be a valid email address")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmailAddress must be a valid email address")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Process is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Content is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromName is required")));
        }

        [Test]
        public void Validate_WithInvalidContentClosingTagEmailDirectProperties_ThrowsEcnExceptionWithErrorCount()
        {
            // Arrange
            var emailDirect = GetTestDataCase3();

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);            
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailAddress is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Source is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyEmailAddress is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmailAddress is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Process is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incorrect closing HTML tag.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incorrect closing HEAD tag.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incorrect closing BODY tag.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromName is required")));
        }

        [Test]
        public void Validate_WithInvalidContentOpeningTagEmailDirectProperties_ThrowsEcnExceptionWithErrorCount()
        {
            // Arrange
            var emailDirect = GetTestDataCase4();

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business);
            ecnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailAddress is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Source is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("ReplyEmailAddress is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromEmailAddress is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Process is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("EmailSubject is required")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incorrect opening HTML tag.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incorrect opening HEAD tag.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Incorrect opening BODY tag.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FromName is required")));
        }

        [Test]
        public void Validate_WithStartProcessingContentWithNoHTMLTagsEmailDirectProperties_SetsEmailDirectContent()
        {
            // Arrange
            var emailDirect = GetTestDataCase5();

            // Act
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect);

            // Assert
            emailDirect.ShouldNotBeNull();
            emailDirect.Content.ShouldNotBeNullOrWhiteSpace();
            emailDirect.Content.
                ShouldContain($"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\"><html><body>{SampleContent}</body></html>");
        }

        [Test]
        public void Validate_WithStartProcessingContentWithBodyTagsEmailDirectProperties_SetsEmailDirectContent()
        {
            // Arrange
            var emailDirect = GetTestDataCase6();

            // Act
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect);

            // Assert
            emailDirect.ShouldNotBeNull();
            emailDirect.Content.ShouldNotBeNullOrWhiteSpace();
            emailDirect.Content.
                ShouldContain($"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\"><html><body>{SampleContent}</body></html>");
        }

        [Test]
        public void Validate_WithStartProcessingContentWithHtmlTagsEmailDirectProperties_SetsEmailDirectContent()
        {
            // Arrange
            var emailDirect = GetTestDataCase7();

            // Act
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect);
            
            // Assert
            emailDirect.ShouldNotBeNull();
            emailDirect.Content.ShouldNotBeNullOrWhiteSpace();
            emailDirect.Content.
                ShouldContain($"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\"><html><body>{SampleContent}</body></html>");
        }

        [Test]
        public void Validate_WithStartProcessingContentWithDocTypeTagsEmailDirectProperties_SetsEmailDirectContent()
        {
            // Arrange
            var emailDirect = GetTestDataCase8();

            // Act
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect);

            // Assert
            emailDirect.ShouldNotBeNull();
            emailDirect.Content.ShouldNotBeNullOrWhiteSpace();
            emailDirect.Content.
                ShouldContain($"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\"><html><body>{SampleContent}</body></html>");
        }

        [Test]
        public void Validate_WithStartProcessingContentWithDocTypeHtmlTagsEmailDirectProperties_SetsEmailDirectContent()
        {
            // Arrange
            var emailDirect = GetTestDataCase9();

            // Act
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect);

            // Assert
            emailDirect.ShouldNotBeNull();
            emailDirect.Content.ShouldNotBeNullOrWhiteSpace();
            emailDirect.Content.
                ShouldContain($"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\"><html><body>{SampleContent}</body></html>");
        }

        [Test]
        public void Validate_WithStartProcessingContentWithDocTypeBodyTagsEmailDirectProperties_SetsEmailDirectContent()
        {
            // Arrange
            var emailDirect = GetTestDataCase10();

            // Act
            _privateEmailDirectType.InvokeStatic(ValidateMethodName, emailDirect);

            // Assert
            emailDirect.ShouldNotBeNull();
            emailDirect.Content.ShouldNotBeNullOrWhiteSpace();
            emailDirect.Content.
                ShouldContain($"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\"><html><body>{SampleContent}</body></html>");
        }
    }
}
