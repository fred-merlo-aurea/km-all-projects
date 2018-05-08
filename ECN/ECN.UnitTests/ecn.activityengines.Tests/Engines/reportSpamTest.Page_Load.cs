using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Specialized;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    public partial class reportSpamTest
    {
        private const string TestedMethodName_PageLoad = "Page_Load";

        [Test]
        public void ReportSpam_Page_Load_WithCacheValue_ValidParams_SetupPage_WithSuccess()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };
            _shimCache.ItemGetString = (value) => _user;
            var parametersValueFormatted = string.Format(ParametersValue, EmailAddressValue, EmailIdValue, GroupIdValue, CustomerIdValue, BlastIdValue);
                        
            QueryString = new NameValueCollection ()
            {
                    {ParametersKey,  parametersValueFormatted},
                    {PreviewKey,  PreviewValue}
            };            

            ShimPage.AllInstances.IsPostBackGet = (page) => false;

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));
            var lblResult = (Label)_privateTestedObject.GetFieldOrProperty(LabelEmailAddressId);

            //Assert 
            lblResult.ShouldSatisfyAllConditions(() => lblResult.ShouldNotBeNull(),
                                                 () => lblResult.Text.ShouldContain(EmailAddressValue));
        }

        [Test]
        public void ReportSpam_Page_Load_NoCacheValue_NoValidValue_SetError_WithSuccess()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };            

            QueryString = new NameValueCollection()
            {
                    {ParametersKey,  ParametersValue},
                    {PreviewKey,  PreviewNegativeValue}
            };            

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));
            var lblEmail = (Label)_privateTestedObject.GetFieldOrProperty(LabelEmailAddressId);
            var lblError = (Label)_privateTestedObject.GetFieldOrProperty(LabelErrorMessageId);

            //Assert 
            lblEmail.ShouldSatisfyAllConditions(() => lblEmail.ShouldNotBeNull(),
                                                () => lblEmail.Text.ShouldBeEmpty());
            lblError.ShouldSatisfyAllConditions(() => lblError.ShouldNotBeNull(),
                                                () => lblError.Text.ShouldNotBeNullOrEmpty());
        }
    }
}
