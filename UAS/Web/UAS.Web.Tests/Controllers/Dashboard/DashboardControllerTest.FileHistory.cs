using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Shouldly;

namespace UAS.Web.Tests.Controllers.Dashboard
{
    [TestFixture]
    public partial class DashboardControllerTest
    {
        [Test]
        public void FileHistory_isCirc_WithSuccess()
        {
            //Arrange
            var model = CreateModel();

            //Act 
            Should.NotThrow(() => _controller.FileHistory(model));
            
            //Assert
            _errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void FileHistory_PubIdZero_WithSuccess()
        {
            //Arrange
            var model = CreateModel();
            model.PubID = 0;

            //Act 
            Should.NotThrow(() => _controller.FileHistory(model));

            //Assert
            _errorWasLogged.ShouldBeFalse();
        }

        [Test]
        public void FileHistory_ExceptionCaught_WithErrorLogged()
        {
            //Arrange
            var model = CreateModel(isCirc: false, recordSource: RecordSource_API);
            _admsLogs.First().StatusMessage = null;
            _products.First().IsCirc = null;

            //Act 
            Should.NotThrow(() => _controller.FileHistory(model));

            //Assert
            _errorWasLogged.ShouldBeTrue();
        }

        [Test]
        public void FileHistory_UserNoAccess_WithErrorReturn()
        {
            //Arrange
            var model = CreateModel(isCirc: false, recordSource: RecordSource_API);            
            _hasAccessReturn = false;

            //Act
            var response = _controller.FileHistory(model) as RedirectToRouteResult;

            //Assert            
            VerifyRedirectToError(response);
        }        
    }
}
