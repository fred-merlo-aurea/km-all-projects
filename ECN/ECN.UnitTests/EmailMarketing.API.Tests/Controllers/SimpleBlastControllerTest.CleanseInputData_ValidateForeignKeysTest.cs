using System;
using System.Collections.Generic;
using ecn.common.classes.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Communicator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Controllers
{
    public partial class SimpleBlastControllerTest
    {
        [Test]
        public void CleanseInputData_ValidateForeignKeys_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.IsTestBlast = false;
            InitilizeCCFakes();
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            exceptionOccured.ShouldBeFalse();
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_NoModelExceptions()
        {
            // Arrange
            var testObject = CreateController();
            InitilizeCCFakes();
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", new object[] { null });
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldSatisfyAllConditions(
                () => _errorList.ShouldNotBeNull(),
                () => _errorList.Count.ShouldBe(1),
                () => _errorList[0].ErrorMessage.ShouldBe("bad request"));
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_NoSubject_NoLayout_NoGroup_Exceptions()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.EmailSubject = string.Empty;
            InitilizeCCFakes();
            ShimGroup.ExistsInt32Int32 = (groupId, customerId) => false;
            ShimLayout.ExistsInt32Int32 = (groupId, customerId) => false;
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", new object[] { model });
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldSatisfyAllConditions(
                () => _errorList.ShouldNotBeNull(),
                () => _errorList.Count.ShouldBe(3),
                () => _errorList[0].ErrorMessage.ShouldBe("missing EmailSubject"),
                () => _errorList[1].ErrorMessage.ShouldBe("GroupID unknown or inaccessible"),
                () => _errorList[2].ErrorMessage.ShouldBe("LayoutID unknown or inaccessible"));
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_InvalidSubject_NoFilter_NoSmartSegment_Exceptions()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.EmailSubject = "subject\n";
            model.ReferenceBlasts = new int[] { 1, 2};
            InitilizeCCFakes();
            ShimFilter.ExistsInt32Int32 = (groupId, customerId) => false;
            ShimSmartSegment.SmartSegmentOldExistsInt32 = (filterId) => true;
            ShimBlast.RefBlastsExistsStringInt32DateTime = (blastId, customerId, sendDate) => false;
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", new object[] { model });
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldSatisfyAllConditions(
                () => _errorList.ShouldNotBeNull(),
                () => _errorList.Count.ShouldBe(2),
                () => _errorList[0].ErrorMessage.ShouldBe("Email Subject contains newline characters"),
                () => _errorList[1].ErrorMessage.ShouldBe("Reference Blast unknown or inaccessible"));
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_NoFilter_Exceptions()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeCCFakes();
            ShimFilter.ExistsInt32Int32 = (groupId, customerId) => false;
            ShimSmartSegment.SmartSegmentOldExistsInt32 = (filterId) => false;
            ShimBlast.RefBlastsExistsStringInt32DateTime = (blastId, customerId, sendDate) => false;
            var exception = new ECNException(new List<ECNError> {
                new ECNError(Enums.Entity.Blast, Enums.Method.Validate, "test") }, Enums.ExceptionLayer.API);
            ShimLayout.ValidateLayoutContentInt32 = (layoutId) => throw exception;
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (layoutId, child) => throw exception;
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", new object[] { model });
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldSatisfyAllConditions(
                () => _errorList.ShouldNotBeNull(),
                () => _errorList.Count.ShouldBe(3),
                () => _errorList[0].ErrorMessage.ShouldBe("FilterID"),
                () => _errorList[1].ErrorMessage.ShouldBe("Content contains an invalid codesnippet"),
                () => _errorList[2].ErrorMessage.ShouldBe("Template contains an invalid codesnippet"));
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_LayoutExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeCCFakes();
            var exception = new Exception("Test");
            ShimLayout.ValidateLayoutContentInt32 = (layoutId) => throw exception;
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (layoutId, child) => throw exception;
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", new object[] { model });
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldSatisfyAllConditions(
                () => _errorList.ShouldNotBeNull(),
                () => _errorList.Count.ShouldBe(2),
                () => _errorList[0].ErrorMessage.ShouldBe("Content contains an invalid codesnippet"),
                () => _errorList[1].ErrorMessage.ShouldBe("Template contains an invalid codesnippet"));
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_LayoutSecurityExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeCCFakes();
            var exception = new SecurityException("Test");
            ShimLayout.ValidateLayoutContentInt32 = (layoutId) => throw exception;
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;
            Exception resultException = null;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", new object[] { model });
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                resultException = e;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            resultException.InnerException.ShouldBeOfType(typeof(SecurityException));
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_TemplateSecurityExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeCCFakes();
            var exception = new SecurityException("Test");
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (layoutId, child) => throw exception;
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;
            Exception resultException = null;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", new object[] { model });
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                resultException = e;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            resultException.InnerException.ShouldBeOfType(typeof(SecurityException));
        }

        private void InitilizeCCFakes()
        {
            ShimGroup.ExistsInt32Int32 = (groupId, customerId) => true;
            ShimLayout.ExistsInt32Int32 = (groupId, customerId) => true;
            ShimFilter.ExistsInt32Int32 = (groupId, customerId) => true;
            ShimECNException.ConstructorIListOfECNErrorEnumsExceptionLayer = (instance, errorList, layer) => _errorList = errorList;
            ShimLayout.ValidateLayoutContentInt32 = (layoutId) => new List<string>();
            ShimGroup.ValidateDynamicStringsListOfStringInt32User = (list, groupId, user) => { };
            ShimGroup.ValidateDynamicStringsForTemplateListOfStringInt32User = (list, groupId, user) => { };
            

            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Filter
            {
                GroupID = 1,
                FilterID = 1,
                CustomerID = 1
            };
            ShimSmartSegment.GetNewIDFromOldIDInt32 = (id) => 1;
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (layoutId, child) => new Layout
            {
                TemplateID = 1
            };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (templateId) => 
                new Template { TemplateSource = string.Empty, TemplateText = string.Empty};

            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) =>
                new ECN_Framework_Entities.Communicator.Content { IsValidated = true };
            ShimLicenseCheck.AllInstances.CurrentString = (instance, id) => "UNLIMITED";
            ShimCampaign.GetByCampaignNameStringUserBoolean = (id, name, child) => new Campaign { };
            ShimCampaign.SaveCampaignUser = (campaign, user) => 0;
            InitilizeHttpFakes();
            InitilizeCampaignFakes();
        }
    }
}
