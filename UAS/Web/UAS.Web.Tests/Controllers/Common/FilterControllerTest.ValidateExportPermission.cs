using System.Collections.Generic;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.Web.Models.UAD.Filter;
using Enums = FrameworkUAD.BusinessLogic.Enums;
using EnumsAccess = KMPlatform.Enums.Access;
using EnumsService = KMPlatform.Enums.Services;
using EnumsServiceFeature = KMPlatform.Enums.ServiceFeatures;

namespace UAS.Web.Tests.Controllers.Common
{
    public partial class FilterControllerTest
    {
        private const string MethodValidateExportPermission = "ValidateExportPermission";
        private const string ErrorNoPermissionToDownloadData = "You do not have permission to download/export the data.";
        private const string ErrorTypeNotAuthorized = "NotAuthorized";
        private const string ErrorTypeUnAuthorized = "UnAuthorized";
        private const string MaskFieldEmail = "EMAIL";

        [Test]
        public void ValidateExportPermission_WithoutAccess_ReturnsError()
        {
            // Arrange
            var model = new DownLoadPopupViewModel();

            // Act
            var result = ControllerObject.Invoke(MethodValidateExportPermission, model) as DownLoadPopupViewModel;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsError.ShouldBeTrue(),
                () => result.ErrorMessage.ShouldBe(ErrorNoPermissionToDownloadData),
                () => result.ErrorType.ShouldBe(ErrorTypeNotAuthorized),
                () => result.PanelUADExportVisible.ShouldBeFalse());
        }

        [Test]
        public void ValidateExportPermission_WithoutDownloadAndExportToGroupAccess_ReturnsError()
        {
            // Arrange
            SetUpForValidateExportPermission();
            var model = new DownLoadPopupViewModel();
            UserAccesses.Remove(EnumsAccess.Download);
            UserAccesses.Remove(EnumsAccess.ExportToGroup);

            // Act
            var result = ControllerObject.Invoke(MethodValidateExportPermission, model) as DownLoadPopupViewModel;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsError.ShouldBeTrue(),
                () => result.ErrorMessage.ShouldBe(ErrorNoPermissionToDownloadData),
                () => result.ErrorType.ShouldBe(ErrorTypeUnAuthorized));
        }

        [Test]
        public void ValidateExportPermission_WithDownloadAndSaveToCampaignAccess_ViewDownloadVisibleTrue()
        {
            // Arrange
            SetUpForValidateExportPermission();
            UserAccesses.Remove(EnumsAccess.ExportToGroup);
            var model = new DownLoadPopupViewModel();

            // Act
            var result = ControllerObject.Invoke(MethodValidateExportPermission, model) as DownLoadPopupViewModel;

            // Assert
            VerifyCommonVisible(result);
            result.ViewExportToGroupVisible.ShouldBeFalse();
        }

        [Test]
        public void ValidateExportPermission_WithoutExternalExportAccess_ViewExportToGroupVisibleFalse()
        {
            // Arrange
            SetUpForValidateExportPermission();
            var model = new DownLoadPopupViewModel();

            // Act
            var result = ControllerObject.Invoke(MethodValidateExportPermission, model) as DownLoadPopupViewModel;

            // Assert
            VerifyCommonVisible(result);
            result.ViewExportToGroupVisible.ShouldBeFalse();
        }

        [Test]
        [TestCase(true, false, false)]
        [TestCase(true, true, false)]
        [TestCase(true, true, true)]
        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, true, true)]
        public void ValidateExportPermission_WithExternalExportAccessAndHasEmailMaskField_ViewExportToGroupVisible(
            bool hasEmailMaskField,
            bool hasEditGroupAccess,
            bool hasViewGroupAccess)
        {
            // Arrange
            SetUpForValidateExportPermission();
            SetUpEmailAccess(hasEmailMaskField);
            UserFeatures.Add(EnumsServiceFeature.Groups);

            if (hasEditGroupAccess)
            {
                UserAccesses.Add(EnumsAccess.Edit);
            }

            if (hasViewGroupAccess)
            {
                UserAccesses.Add(EnumsAccess.View);
            }

            var model = new DownLoadPopupViewModel();

            // Act
            var result = ControllerObject.Invoke(MethodValidateExportPermission, model) as DownLoadPopupViewModel;

            // Assert
            VerifyCommonVisible(result);
            result.ShouldSatisfyAllConditions(
                () => result.ViewExportToGroupVisible.ShouldBe(!hasEmailMaskField),
                () => result.NewGroupVisible.ShouldBe(!hasEmailMaskField && hasEditGroupAccess),
                () => result.ExistingGroupVisible.ShouldBe(!hasEmailMaskField && hasViewGroupAccess));
        }

        [Test]
        public void ValidateExportPermission_DcRunIdPositiveAndIsKmStaff_IsBillableFalse()
        {
            // Arrange
            SetUpForValidateExportPermission();
            UserFeatures.Add(EnumsServiceFeature.DataCompare);
            UserAccesses.Add(EnumsAccess.Yes);
            var model = new DownLoadPopupViewModel
            {
                DCRunID = SampleId,
                BrandID = SampleId,
                IsBillable = true,
                ViewType = Enums.ViewType.ConsensusView
            };
            EcnSession.CurrentUser.IsKMStaff = true;

            var compareView = new DataCompareView
            {
                DcTargetIdUad = SampleId,
                IsBillable = true
            };
            var viewList = new List<DataCompareView>{ compareView };
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (view, i) => viewList;

            // Act
            var result = ControllerObject.Invoke(MethodValidateExportPermission, model) as DownLoadPopupViewModel;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ViewDownloadVisible.ShouldBeTrue(),
                () => result.ViewExportToGroupVisible.ShouldBeFalse(),
                () => result.IsBillable.ShouldBeFalse());
        }

        private void SetUpForValidateExportPermission()
        {
            UserServices.Add(EnumsService.UAD);
            UserFeatures.Add(EnumsServiceFeature.UADExport);
            UserAccesses.Add(EnumsAccess.Download);
            UserAccesses.Add(EnumsAccess.ExportToGroup);
            UserAccesses.Add(EnumsAccess.SaveToCampaign);
        }

        private void SetUpEmailAccess(bool addEmailMaskField)
        {
            UserServices.Add(EnumsService.EMAILMARKETING);
            UserFeatures.Add(EnumsServiceFeature.Email);
            UserAccesses.Add(EnumsAccess.ExternalImport);

            var list = new List<UserDataMask>();
            if (addEmailMaskField)
            {
                list.Add(new UserDataMask{ MaskField = MaskFieldEmail });
            }
            ShimUserDataMask.GetByUserIDClientConnectionsInt32 = (a, b) => list;
        }

        private void VerifyCommonVisible(DownLoadPopupViewModel result)
        {
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsError.ShouldBeFalse(),
                () => result.ViewDownloadVisible.ShouldBeTrue(),
                () => result.ViewSaveToCampaignVisible.ShouldBeTrue(),
                () => result.ExportFieldsVisible.ShouldBeTrue(),
                () => result.DownloadCountVisible.ShouldBeTrue(),
                () => result.PromoCodeVisible.ShouldBeTrue());
        }
    }
}
