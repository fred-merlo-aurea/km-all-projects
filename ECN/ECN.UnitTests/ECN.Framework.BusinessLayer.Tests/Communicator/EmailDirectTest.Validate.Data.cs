using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class EmailDirectTest
    {
        private EmailDirect GetTestDataCase1()
        {
            return new EmailDirect { };
        }

        private EmailDirect GetTestDataCase2()
        {
            ShimEmail.IsValidEmailAddressString = (s) => false;

            return new EmailDirect
            {
                EmailAddress = "sample@test.com,test@test.com",
                ReplyEmailAddress = "sample@test.com",
                FromEmailAddress = "abc@abc.com"
            };
        }

        private EmailDirect GetTestDataCase3()
        {
            return new EmailDirect
            {
                Content = "<html><head><body><|body><|<head><|html>"
            };
        }

        private EmailDirect GetTestDataCase4()
        {
            return new EmailDirect
            {
                Content = "<!html><!head><!body></body></head></html>"
            };
        }

        private EmailDirect GetTestDataCase5()
        {
            ShimEmail.IsValidEmailAddressString = (s) => true;
            var emailDirect = GetDefaultEmailDirect();
            return emailDirect;
        }

        private EmailDirect GetTestDataCase6()
        {
            ShimEmail.IsValidEmailAddressString = (s) => true;
            var emailDirect = GetDefaultEmailDirect();
            emailDirect.Content = $"<body>{SampleContent}</body>";
            return emailDirect;
        }

        private EmailDirect GetTestDataCase7()
        {
            ShimEmail.IsValidEmailAddressString = (s) => true;
            var emailDirect = GetDefaultEmailDirect();
            emailDirect.Content = $"<html>{SampleContent}</html>";
            return emailDirect;
        }

        private EmailDirect GetTestDataCase8()
        {
            ShimEmail.IsValidEmailAddressString = (s) => true;
            var emailDirect = GetDefaultEmailDirect();
            emailDirect.Content = $"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\">{SampleContent}";
            return emailDirect;
        }

        private EmailDirect GetTestDataCase9()
        {
            ShimEmail.IsValidEmailAddressString = (s) => true;
            var emailDirect = GetDefaultEmailDirect();
            emailDirect.Content = $"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\"><html>{SampleContent}</html>";
            return emailDirect;
        }

        private EmailDirect GetTestDataCase10()
        {
            ShimEmail.IsValidEmailAddressString = (s) => true;
            var emailDirect = GetDefaultEmailDirect();
            emailDirect.Content = $"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transistional//EN\"><body>{SampleContent}</body>";
            return emailDirect;
        }
        private EmailDirect GetDefaultEmailDirect()
        {
            return new EmailDirect
            {
                EmailAddress = "sample@test.com,test@test.com",
                ReplyEmailAddress = "sample@test.com",
                FromEmailAddress = "abc@abc.com",
                Source = "SampleSource",
                Content = SampleContent,
                Process = "SampleProcess",
                EmailSubject = "SampleSubject",
                FromName = "SampleName",
            };
        }
    }
}
