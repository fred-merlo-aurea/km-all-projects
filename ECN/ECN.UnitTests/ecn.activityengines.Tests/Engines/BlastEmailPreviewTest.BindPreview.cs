using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Text;
using ecn.activityengines.engines.Fakes;
using EmailPreview;
using KM.Common.Entity.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    public partial class BlastEmailPreviewTest
    {
        private const string TestedMethodName_BindPreview = "BindPreview";
        private const string AppSettingKmApplication = "KMCommon_Application";

        [Test]
        public void BindPreview_BlastIdZero_RedirectsUser()
        {
            //Arrange
            _privateTestObject.SetFieldOrProperty(BlastIDFieldName, 0);

            //Act and Assert
            Should.NotThrow(()=>_privateTestObject.Invoke(TestedMethodName_BindPreview));
        }

        [Test]
        public void BindPreview_AlternativeFlow_RedirectsUser()
        {
            //Arrange
            var settings = new NameValueCollection
            {
                { AppSettingKmApplication, "1" }
            };
            ShimConfigurationManager.AppSettingsGet = () => settings;
            ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (exception, s, arg3, arg4, arg5, arg6) => { };

            if (_emailPreviews.Count > 0)
            {
                _emailPreviews.RemoveAt(0);
            }

            //Act and Assert
            Should.NotThrow(() => _privateTestObject.Invoke(TestedMethodName_BindPreview));
        }

        [Test]
        public void BindPreview_WithCodeAnalysisValue_WithoutResultTypeValue()
        {
            //Arrange
            _codeAnalysis.CompatibilityRulesCount = 1;
            _codeAnalysis.HtmlProblems = new List<CodeAnalysisHtmlValidation>() { new CodeAnalysisHtmlValidation()};
            _codeAnalysis.CompatibilityProblems = new List<EmailPreview.CodeAnalysisPotentialProblems>()
            {
                new CodeAnalysisPotentialProblems()
                {
                    ApiIds = new List<string>()
                    {
                        ApiID.ToString(),
                        ApiID_2.ToString()
                    }
                }
            };

            var testingApplication = new TestingApplication()
            {
                ApplicationName = ApiID,
                ApplicationLongName = ApiID_Name
            };            
            _testingApplication.Add(testingApplication);            

            //Act and Assert
            Should.NotThrow(() => _privateTestObject.Invoke(TestedMethodName_BindPreview));
        }

        [Test]
        public void BindPreview_WithCodeAnalysisValue_ApiIDsDuplicated()
        {
            //Arrange
            _codeAnalysis.CompatibilityRulesCount = 1;
            _codeAnalysis.HtmlProblems = new List<CodeAnalysisHtmlValidation>() { new EmailPreview.CodeAnalysisHtmlValidation() };
            _codeAnalysis.CompatibilityProblems = new List<CodeAnalysisPotentialProblems>()
            {
                new CodeAnalysisPotentialProblems()
                {
                    ApiIds = new List<string>()
                    {
                        ApiID.ToString(),
                        ApiID.ToString()
                    }
                }
            };

            //Act and Assert
            Should.NotThrow(() => _privateTestObject.Invoke(TestedMethodName_BindPreview));
        }

        [Test]
        public void BindPreview_WithEmailAnalisysValue()
        {
            //Arrange            
            _emailResults.Add(new EmailResult()
            {
                ResultType = EmailResultEnum.ResultType.email.ToString(),
                ApplicationLongName = ApiID_Name
            });
            _emailResults.Add(new EmailResult()
            {
                ResultType = EmailResultEnum.ResultType.spam.ToString(),
                ApplicationName = ApiID_Name
            });
            _emailResults.Add(new EmailResult()
            {
                ResultType = EmailResultEnum.ResultType.spam.ToString(),
                ApplicationName = EmailResultEnum.EmailSpam.htmlvalidation.ToString(),
                Summary = new StringBuilder().Append(DummyStringValue)
            });

            //Act and Assert
            Should.NotThrow(() => _privateTestObject.Invoke(TestedMethodName_BindPreview));
        }

        [Test]
        public void BindPreview_WithoutValidEmailAnalisysValue()
        {
            //Arrange            
            _emailResults.Add(new EmailResult()
            {
                ResultType = EmailResultEnum.ResultType.spam.ToString(),
                ApplicationLongName = ApiID_Name
            });

            //Act and Assert
            Should.NotThrow(() => _privateTestObject.Invoke(TestedMethodName_BindPreview));
        }

        [Test]
        public void BindPreview_WithEmaiPreviewWrongDate()
        {
            //Arrange
            _defaultEmailPreview.DateCreated = DateTime.Now.AddDays(-2);

            //Act and Assert
            Should.NotThrow(() => _privateTestObject.Invoke(TestedMethodName_BindPreview));
        }

        [Test]       
        public void BindPreview_WithLinkTestValue()
        {
            //Arrange    
            var bindLinkCheckWasCalled = false;
            _linkTest.Links = new List<Link>()
            {
                new Link()
                {
                    IsValid = true,
                    Url = ImageUrl
                }
            };                       
            ShimBlastEmailPreview.AllInstances.BindLinkCheckLinkTest = (blast, link) => { bindLinkCheckWasCalled = true; };

            //Act and Assert
            Should.NotThrow(() => _privateTestObject.Invoke(TestedMethodName_BindPreview));
            bindLinkCheckWasCalled.ShouldBeTrue();
        }
    }
}
