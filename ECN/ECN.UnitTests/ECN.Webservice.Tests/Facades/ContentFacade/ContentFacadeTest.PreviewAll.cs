using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;

namespace ECN.Webservice.Tests.Facades.ContentFacade
{
    [TestFixture]
    public partial class ContentFacadeTest
    {
        private const string XmlSearch = "<SearchFields><Title>Title</Title><User>user</User><Folder>10</Folder>" +
            "<ModifiedFrom>2018/04/05</ModifiedFrom><ModifiedTo>2018/04/05</ModifiedTo></SearchFields>";

        [Test]
        public void PreviewMessage_ForLayoutExist_ReturnSuccessResponse()
        {
            // Arrange
            InitializePreviewMessage();

            // Act
            var result = _facade.PreviewMessage(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void PreviewMessage_ForLayoutExistFalse_ReturnFailResponse()
        {
            // Arrange
            InitializePreviewMessage();
            ShimLayout.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.PreviewMessage(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void PreviewContent_ForContent_ReturnSuccessResponse()
        {
            // Arrange
            InitializePreviewContent();

            // Act
            var result = _facade.PreviewContent(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void PreviewContent_ForNullContent_ReturnFailResponse()
        {
            // Arrange
            InitializePreviewContent();
            ShimContent.GetByContentIDInt32UserBoolean = (x, y, z) => null;

            // Act
            var result = _facade.PreviewContent(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void SearchForContent_ForXmlString_ReturnSuccessResponse()
        {
            // Arrange
            Initialize();
            ShimContent.GetByContentSearchStringNullableOfInt32NullableOfInt32NullableOfDateTimeNullableOfDateTimeUserBooleanNullableOfBoolean = 
                (x, y, q, w, e, r, t, u) => new List<Content> { new Content() };
            BusinessLogicFakes.ShimUser.GetByUserNameStringInt32Boolean = (x, y, z) => new User() { UserID = Id };
            ShimFolder.ExistsInt32Int32 = (x, y) => true;

            // Act
            var result = _facade.SearchForContent(_context, XmlSearch);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void SearchForMessages_ForXmlString_ReturnSuccessResponse()
        {
            // Arrange
            Initialize();
            ShimLayout.GetByLayoutSearchStringNullableOfInt32NullableOfInt32NullableOfDateTimeNullableOfDateTimeUserBooleanNullableOfBoolean = 
                (x, y, q, w, e, r, t, u) => new List<Layout> { new Layout() };
            BusinessLogicFakes.ShimUser.GetByUserNameStringInt32Boolean = (x, y, z) => new User() { UserID = Id };
            ShimFolder.ExistsInt32Int32 = (x, y) => true;

            // Act
            var result = _facade.SearchForMessages(_context, XmlSearch);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        private void InitializePreviewContent()
        {
            Initialize();
            ShimContent.GetByContentIDInt32UserBoolean = (x, y, z) => new Content() { ContentSource = MethodName };
        }

        private void InitializePreviewMessage()
        {
            Initialize();
            ShimLayout.ExistsInt32Int32 = (x, y) => true;
            ShimLayout.GetPreviewInt32EnumsContentTypeCodeBooleanUserNullableOfInt32NullableOfInt32NullableOfInt32 = 
                (x, y, z, q, w, e, r) => MethodName;
        }
    }
}
