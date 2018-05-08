using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.communicator.CommonControls;
using ecn.communicator.CommonControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.CommonControls
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class DateValidatorTests
    {
        private IDisposable _shimObject;
        private object[] _methodParams;
        private DateValidator _dateValidator;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _methodParams = new object[] { };
            _dateValidator = new DateValidator();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        [TestCase("", "11/20/2020", true, false)]
        [TestCase("01/22/207", "11/20/2020", true, false)]
        [TestCase("01/22/2007", "11/20/220", true, false)]
        [TestCase("01/22/2007", "11/20/2200", true, false)]        
        [TestCase("01/22/2017", "11/20/2011", true, false)]
        [TestCase("01/22/2017", "11/20/2200", true, true)]
        public void ValidateDatesTest(string startDateText, string endDateText, bool datesRequired, bool expectedResult)
        {
            
                //Arrange
                var textBoxStartDate = new TextBox()
                {
                    Text = startDateText
                };

                var textBoxEndDate = new TextBox()
                {
                    Text = endDateText
                };

                const int archiveYears = 5;
                var phError = new PlaceHolder();
                var lblErrorMessage = new Label();
            try
            {
                InitializeFakes();

                //Act
                var result = _dateValidator.ValidateDates(textBoxStartDate, textBoxEndDate, archiveYears, phError, lblErrorMessage, datesRequired);

                //Assert
                result.ShouldBe(expectedResult);
            }
            finally
            {
                if (textBoxStartDate != null)
                {
                    textBoxStartDate.Dispose();
                }
                if (textBoxEndDate != null)
                {
                    textBoxEndDate.Dispose();
                }
                if (phError != null)
                {
                    phError.Dispose();
                }
                if (lblErrorMessage != null)
                {
                    lblErrorMessage.Dispose();
                }
            }
        }

        [TestCase("2007", true)]
        [TestCase("77", true)]
        [TestCase("1007", false)]
        public void IsRecentYear_Test(string year, bool expectedResult)
        {
            //Arrange
            var privateObject = new PrivateObject(_dateValidator);

            //Act
            var result = privateObject.Invoke("IsRecentYear", year) as bool?;

            //Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(), 
                () => result.ShouldBe(expectedResult));
        }

        private void InitializeFakes()
        {
            ShimDateValidator.AllInstances.clearECNErrorPlaceHolderLabel = (DateValidator instance, PlaceHolder phError, Label lblErrorMessage) =>
            {
                //Do nothing
            };

            ShimDateValidator.AllInstances.throwECNExceptionStringPlaceHolderLabel = (DateValidator instance, string message, PlaceHolder phError, Label lblErrorMessage) =>
            {
                //Do nothing
            };
        }
    }
}