using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using KM.Common.Utilities.Email;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Utilities.Email
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EmailServiceTests
    {
        private const string EmailFromAddress = "email_sender@email.com";
        private const string EmailToAddress = "email_receiver@email.com";
        private const string PreconfiguredEmailAddress = "preconfigured@email.com";

        [Test]
        public void SendEmail_NullMessageParam_ExceptionIsThrown()
        {
            // Arrange
            EmailMessage emailMessage = null;
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<ArgumentNullException>(
                () =>
                {
                    emailService.SendEmail(emailMessage);
                }
            );
        }

        [Test]
        public void SendEmail_NoToAddress_ExceptionIsThrown()
        {
            // Arrange
            var emailMessage = new EmailMessage();
            emailMessage.From = EmailFromAddress;
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<InvalidOperationException>(
                () =>
                {
                    emailService.SendEmail(emailMessage);
                }
            );
        }

        [Test]
        public void SendEmail_EmptyToAddress_ExceptionIsThrown()
        {
            // Arrange
            var emailMessage = new EmailMessage();
            emailMessage.From = EmailFromAddress;
            emailMessage.To.Add(string.Empty);
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<ArgumentException>(
                () =>
                {
                    emailService.SendEmail(emailMessage);
                }
            );
        }

        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public void SendEmail_NoFromAddress_ExceptionIsThrown(string fromAddress)
        {
            // Arrange
            var emailMessage = new EmailMessage();
            emailMessage.From = fromAddress;
            emailMessage.To.Add(EmailToAddress);
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<ArgumentException>(
                () =>
                {
                    emailService.SendEmail(emailMessage);
                }
            );
        }

        [Test]
        [TestCase("test")]
        [TestCase("123")]
        [TestCase("123@")]
        [TestCase(".123@email.com")]
        public void SendEmail_InvalidFromAddress_ExceptionIsThrown(string toAddress)
        {
            // Arrange
            var emailMessage = new EmailMessage();
            emailMessage.From = EmailFromAddress;
            emailMessage.To.Add(toAddress);
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<FormatException>(
                () =>
                {
                    emailService.SendEmail(emailMessage);
                }
            );
        }

        [Test]
        [TestCase("test")]
        [TestCase("123")]
        [TestCase("123@")]
        [TestCase(".123@email.com")]
        public void SendEmail_InvalidToAddress_ExceptionIsThrown(string fromAddress)
        {
            // Arrange
            var emailMessage = new EmailMessage();
            emailMessage.From = fromAddress;
            emailMessage.To.Add(EmailToAddress);
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<FormatException>(
                () =>
                {
                    emailService.SendEmail(emailMessage);
                }
            );
        }

        [Test]
        public void SendEmail_ValidMessageParam_EmailIsSent()
        {
            // Arrange
            var emailMessage = new EmailMessage();
            emailMessage.To.Add(EmailToAddress);
            emailMessage.From = EmailFromAddress;
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act
            emailService.SendEmail(emailMessage);

            // Assert
            emailClientMock.Verify(x => x.Send(It.IsAny<MailMessage>(), null), Times.Once());
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void SendEmail_InvalidErrorMessageParam_ExceptionIsThrown(string errorMessage)
        {
            // Arrange            
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<ArgumentException>(
                () =>
                {
                    emailService.SendEmail(errorMessage);
                }
            );
        }

        [Test]
        public void SendEmail_ValidErrorMessageParam_EmailIsSent()
        {
            // Arrange
            var errorMessage = "error";
            var configuredValue = PreconfiguredEmailAddress;
            var subject = $"IsDemo {configuredValue} : {configuredValue}";
            MailMessage mailMessage = null;
            var emailClientMock = GetEmailClientMock();
            emailClientMock
                .Setup(x => x.Send(It.IsAny<MailMessage>(), It.IsAny<string>()))
                .Callback(
                    (MailMessage message, string mailServer) =>
                    {
                        mailMessage = message;
                    });
            var configurationProviderMock = GetConfigurationProviderMock();
            configurationProviderMock
                .Setup(x => x.GetValue<string>(It.IsAny<string>()))
                .Returns(configuredValue);
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act
            emailService.SendEmail(errorMessage);

            // Assert
            emailService.ShouldSatisfyAllConditions(
                () => emailClientMock.Verify(x => x.Send(It.IsAny<MailMessage>(), null), Times.Once()),
                () => mailMessage.Body = errorMessage,
                () => mailMessage.To.First().Address.Equals(configuredValue, StringComparison.OrdinalIgnoreCase),
                () => mailMessage.From.Address.Equals(configuredValue, StringComparison.OrdinalIgnoreCase),
                () => mailMessage.IsBodyHtml = false,
                () => mailMessage.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void SendEmail_ValidErrorMessageParamInvalidConfiguration_ExceptionIsThrown()
        {
            // Arrange
            var errorMessage = "error";
            var configuredValue = string.Empty;
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            configurationProviderMock
                .Setup(x => x.GetValue<string>(It.IsAny<string>()))
                .Returns(configuredValue);
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<ArgumentException>(() => emailService.SendEmail(errorMessage));
        }

        [Test]
        public void SendEmail_NullExceptionParam_ExceptionIsThrown()
        {
            // Arrange            
            Exception exception = null;
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<ArgumentNullException>(
                () =>
                {
                    emailService.SendEmail(exception);
                }
            );
        }

        [Test]
        public void SendEmail_ValidExceptionParam_EmailIsSent()
        {
            // Arrange
            var exceptionMessage = "exception_message";
            var exceptionSource = "exception_source";
            var exception = new Exception(exceptionMessage)
            {
                Source = exceptionSource
            };

            var configuredValue = PreconfiguredEmailAddress;
            var subject = $"IsDemo {configuredValue} : {configuredValue}";
            MailMessage mailMessage = null;
            var emailClientMock = GetEmailClientMock();
            emailClientMock
                .Setup(x => x.Send(It.IsAny<MailMessage>(), It.IsAny<string>()))
                .Callback(
                    (MailMessage message, string mailServer) =>
                    {
                        mailMessage = message;
                    });
            var configurationProviderMock = GetConfigurationProviderMock();
            configurationProviderMock
                .Setup(x => x.GetValue<string>(It.IsAny<string>()))
                .Returns(configuredValue);
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act
            emailService.SendEmail(exception);

            // Assert
            emailService.ShouldSatisfyAllConditions(
                () => emailClientMock.Verify(x => x.Send(It.IsAny<MailMessage>(), null), Times.Once()),
                () => mailMessage.Body.Contains(exceptionMessage),
                () => mailMessage.Body.Contains(exceptionSource),
                () => mailMessage.Body.Contains(exception.GetType().ToString()),
                () => mailMessage.To.First().Address.Equals(configuredValue, StringComparison.OrdinalIgnoreCase),
                () => mailMessage.From.Address.Equals(configuredValue, StringComparison.OrdinalIgnoreCase),
                () => mailMessage.IsBodyHtml = true,
                () => mailMessage.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void SaveEmail_NullMessageParam_ExceptionIsThrown()
        {
            // Arrange
            var filename = string.Empty;
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<ArgumentNullException>(
                () =>
                {
                    emailService.SaveEmail(null, filename);
                }
            );
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void SaveEmail_InvalidFilenameParam_ExceptionIsThrown(string filename)
        {
            // Arrange
            var message = new EmailMessage();
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<ArgumentException>(
                () =>
                {
                    emailService.SaveEmail(message, filename);
                }
            );
        }

        [Test]
        public void SaveEmail_NoToAddress_ExceptionIsThrown()
        {
            // Arrange
            var filename = "file";
            var message = new EmailMessage();
            message.From = EmailFromAddress;
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<InvalidOperationException>(
                () =>
                {
                    emailService.SaveEmail(message, filename);
                }
            );
        }

        [Test]
        public void SaveEmail_EmptyToAddress_ExceptionIsThrown()
        {
            // Arrange
            var filename = "file";
            var message = new EmailMessage();
            message.From = EmailFromAddress;
            message.To.Add(string.Empty);
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<ArgumentException>(
                () =>
                {
                    emailService.SaveEmail(message, filename);
                }
            );
        }

        [Test]
        [TestCase("test")]
        [TestCase("123")]
        [TestCase("123@")]
        [TestCase(".123@email.com")]
        public void SaveEmail_InvalidFromAddress_ExceptionIsThrown(string toAddress)
        {
            // Arrange
            var filename = "file";
            var message = new EmailMessage();
            message.From = EmailFromAddress;
            message.To.Add(toAddress);
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<FormatException>(
                () =>
                {
                    emailService.SaveEmail(message, filename);
                }
            );
        }

        [Test]
        [TestCase("test")]
        [TestCase("123")]
        [TestCase("123@")]
        [TestCase(".123@email.com")]
        public void SaveEmail_InvalidToAddress_ExceptionIsThrown(string fromAddress)
        {
            // Arrange
            var filename = "file";
            var message = new EmailMessage();
            message.From = fromAddress;
            message.To.Add(EmailToAddress);
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act & Assert
            Should.Throw<FormatException>(
                () =>
                {
                    emailService.SaveEmail(message, filename);
                }
            );
        }

        [Test]
        public void SaveEmail_ValidMessageParam_EmailIsSent()
        {
            // Arrange
            var filename = "file";
            var message = new EmailMessage();
            message.To.Add(EmailToAddress);
            message.From = EmailFromAddress;
            var emailClientMock = GetEmailClientMock();
            var configurationProviderMock = GetConfigurationProviderMock();
            var emailService = new EmailService(emailClientMock.Object, configurationProviderMock.Object);

            // Act
            emailService.SaveEmail(message, filename);

            // Assert
            emailClientMock.Verify(x => x.Save(It.IsAny<MailMessage>(), filename, null), Times.Once());
        }

        private static Mock<IEmailClient> GetEmailClientMock()
        {
            return new Mock<IEmailClient>();
        }

        private Mock<IConfigurationProvider> GetConfigurationProviderMock()
        {
            return new Mock<IConfigurationProvider>();
        }
    }
}
