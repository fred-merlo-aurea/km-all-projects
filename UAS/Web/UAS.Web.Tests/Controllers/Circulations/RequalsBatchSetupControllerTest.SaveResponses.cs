using System.Linq;
using System.Web.Mvc;
using Shouldly;
using NUnit.Framework;

namespace UAS.Web.Tests.Controllers.Circulations
{
    [TestFixture]
    public partial class RequalsBatchSetupControllerTest
    {
        [Test]
        public void SaveResponse_WithMultipleValues_DemoChecked_NonQualifiedFree_WithSuccess()
        {
            //Arrange
            var viewModel = CreateVM();
            _rgIsMultipleValue = true;

            //Act
            var response = _controller.SaveResponses(viewModel) as JsonResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Data.ShouldBe(SaveSuccessReturnValue));
        }

        [Test]
        public void SaveResponse_WithMultipleValues_ResponseOtherChanged_NonQualifiedFree_WithSuccess()
        {
            //Arrange
            var viewModel = CreateVM(isDemoChecked: false);
            _rgIsMultipleValue = true;
            _prodSubDetails.Add(new FrameworkUAD.Entity.ProductSubscriptionDetail()
            {
                CodeSheetID = AnotherDummyInt
            });
            _pubSubDetailVM.CodeSheetID = AnotherDummyInt;

            //Act
            var response = _controller.SaveResponses(viewModel) as JsonResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Data.ShouldBe(SaveSuccessReturnValue));
        }

        [Test]
        public void SaveResponse_WithMultipleValues_AndNonQualifiedPaid_WithSuccess()
        {
            //Arrange
            var viewModel = CreateVM(isDemoChecked: false);
            _rgIsMultipleValue = true;
            _catTypes.First().CategoryCodeTypeName = FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " ");

            //Act
            var response = _controller.SaveResponses(viewModel) as JsonResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Data.ShouldBe(SaveSuccessReturnValue));
        }

        [Test]
        public void SaveResponse_NoMultipleValues_DemoChecked_QualifiedFree_WithSuccess()
        {
            //Arrange
            var viewModel = CreateVM(isDemoChecked: true);
            _rgIsMultipleValue = false;
            _catTypes.First().CategoryCodeTypeName = FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ");

            //Act
            var response = _controller.SaveResponses(viewModel) as JsonResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Data.ShouldBe(SaveSuccessReturnValue));
        }

        [Test]
        public void SaveResponse_NoMultipleValues_ResponseOtherChanged_NonQualifiedFree_WithSuccess()
        {
            //Arrange
            var viewModel = CreateVM(isDemoChecked: false);
            _rgIsMultipleValue = false;
            _prodSubDetails.Add(new FrameworkUAD.Entity.ProductSubscriptionDetail()
            {
                CodeSheetID = AnotherDummyInt
            });
            _pubSubDetailVM.CodeSheetID = AnotherDummyInt;

            //Act
            var response = _controller.SaveResponses(viewModel) as JsonResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Data.ShouldBe(SaveSuccessReturnValue));
        }

        [Test]
        public void SaveResponse_NoMultipleValues_AndQualifiedPaid_WithSuccess()
        {
            //Arrange
            var viewModel = CreateVM(isDemoChecked: false);
            _rgIsMultipleValue = false;
            _catTypes.First().CategoryCodeTypeName = FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ");

            //Act
            var response = _controller.SaveResponses(viewModel) as JsonResult;

            //Assert
            response.ShouldSatisfyAllConditions(() => response.ShouldNotBeNull(),
                                                () => response.Data.ShouldBe(SaveSuccessReturnValue));
        }

        [Test]
        public void SaveResponse_UserNoAccess_WithErrorReturn()
        {
            //Arrange
            var viewModel = CreateVM(isDemoChecked: true);
            _hasAccessReturn = false;

            //Act
            var response = _controller.SaveResponses(viewModel) as RedirectToRouteResult;

            //Assert            
            VerifyRedirectToError(response);
        }
    }
}
