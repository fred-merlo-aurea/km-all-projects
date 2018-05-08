using System.Linq;
using System.Web.Mvc;
using Shouldly;
using NUnit.Framework;

namespace UAS.Web.Tests.Controllers.Dashboard
{
    [TestFixture]
    public partial class DashboardControllerTest
    {
        private const int OriginalRecordCount2 = 20000;
        private const int OriginalRecordCount3 = 60000;
        private const int OriginalRecordCount4 = 120000;

        [Test]
        public void Index_isCIRC_OriginalRecordCount1_WithSuccess()
        {
            //Arrange                    
            _admsLogs.First().RecordSource = RecordSource_CiRC;
            SessionMock.Setup(s => s[SessionKeyCodeList]).Returns(_codeList);

            //Act 
            var response = _controller.Index() as ViewResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Model.ShouldNotBeNull());
        }

        [Test]
        public void Index_isAPI_OriginalRecordCount2_WithSuccess()
        {
            //Arrange                    
            _admsLogs.First().RecordSource = RecordSource_API;
            _admsLogs.First().OriginalRecordCount = OriginalRecordCount2;
            SessionMock.Setup(s => s[SessionKeyCodeList]).Returns(_codeList);

            //Act 
            var response = _controller.Index() as ViewResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Model.ShouldNotBeNull());
        }

        [Test]
        public void Index_isDataCompare_OriginalRecordCount3_WithSuccess()
        {
            //Arrange                    
            _admsLogs.First().RecordSource = RecordSource_DataCompare;
            _admsLogs.First().OriginalRecordCount = OriginalRecordCount3;
            SessionMock.Setup(s => s[SessionKeyCodeList]).Returns(_codeList);

            //Act 
            var response = _controller.Index() as ViewResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Model.ShouldNotBeNull());
        }

        [Test]
        public void Index_isCirc_OriginalRecordCount4_WithSuccess()
        {
            //Arrange                    
            _admsLogs.First().RecordSource = RecordSource_CiRC;
            _admsLogs.First().OriginalRecordCount = OriginalRecordCount4;
            SessionMock.Setup(s => s[SessionKeyCodeList]).Returns(_codeList);

            //Act 
            var response = _controller.Index() as ViewResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Model.ShouldNotBeNull());
        }

        [Test]
        public void Index_UserNoAccess_WithErrorReturn()
        {
            //Arrange
            SessionMock.Setup(s => s[SessionKeyCodeList]).Returns(_codeList);
            _hasAccessReturn = false;

            //Act
            var response = _controller.Index() as RedirectToRouteResult;

            //Assert            
            VerifyRedirectToError(response);
        }
    }
}
