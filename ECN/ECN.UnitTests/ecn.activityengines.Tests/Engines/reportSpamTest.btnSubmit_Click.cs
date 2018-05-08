using System;
using System.Collections.Specialized;
using System.Net.Mail.Fakes;
using Shouldly;
using NUnit.Framework;
using KM.Common.Entity.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_BusinessLayer.Accounts.Fakes;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    public partial class reportSpamTest
    {
        private const string TestedMethodName_btnSubmit_Click = "btnSubmit_Click";

        [Test]
        public void ReportSpam_btnSubmit_Click_NoTextReasonValue_WithSuccess()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };
            var mailSent = false;
            var spamFeedBackInserted = false;

            QueryString = new NameValueCollection()
            {
                    {PreviewKey,  PreviewValue}
            };

            ShimSmtpClient.ConstructorString = (smtpClient, host) =>
            {
                var shimSmtp = new ShimSmtpClient(smtpClient)
                {
                    SendMailMessage = (message) => { }
                };

                mailSent = true;
            };

            ShimEmailActivityLog.InsertSpamFeedbackInt32Int32StringStringString = (blastID, emailID, reason, subscribeType, actionType) =>
            {
                spamFeedBackInserted = true;
            };

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_btnSubmit_Click, methodArguments));

            //Assert 
            mailSent.ShouldBeTrue();
            spamFeedBackInserted.ShouldBeTrue();
        }

        [Test]
        public void ReportSpam_btnSubmit_Click_NoReportAbuseInfo_WithLoggingError()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };
            var errorLogged = false;

            _privateTestedObject.SetFieldOrProperty("Preview", int.Parse(PreviewNegativeValue));

            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 = (error, source, application, severity, note, charityId) =>
            {
                errorLogged = true;
                return DummyInt;
            };
            ShimLandingPageAssign.GetOneToUseInt32Int32Boolean = (lpaid, customerId, getChildren) =>
            {
                return new LandingPageAssign()
                {
                    Header = DummyString,
                    Footer = DummyString
                };
            };

            _dtAbuseReport.Rows.RemoveAt(0);

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_btnSubmit_Click, methodArguments));

            //Assert 
            errorLogged.ShouldBeTrue();
        }
    }
}
