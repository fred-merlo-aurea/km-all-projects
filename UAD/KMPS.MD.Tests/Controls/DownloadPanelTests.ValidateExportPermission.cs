using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web.UI.WebControls;
using Castle.Core.Logging;
using FrameworkUAS.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using KM.Platform.Fakes;
using KMPlatform.Object;
using Shouldly;
using Telerik.Web.UI;
using TestCommonHelpers;
using DownloadPanel = KMPS.MD.Controls.DownloadPanel;
using UASEntity = FrameworkUAS.Entity;
using static KMPlatform.Enums;

namespace KMPS.MD.Tests.Controls
{
    public partial class DownloadPanelTests
    {
        private const int DummyId = 1;
        private const int TestNegativeNumber = -1;
        private const string DummyString = "DummyString";
        private const string LblNoDownloadMessage = "lblNoDownloadMessage";
        private const string PnlUADExport = "pnlUADExport";
        private const string PhRBDownload = "phRBDownload";
        private const string PhShowHeader = "phShowHeader";
        private const string EmailString = "Email";
        private const string RcForDownload = "rcForDownload";
        private const string PhRbCampaign = "phRBCampaign";
        private const string RbCampaign = "rbCampaign";
        private const string RbGroupExport = "rbGroupExport";

        [Test]
        public void ValidateExportPermission_NoPermission_SetErrorMessage()
        {
            // Arrange
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => false;
            ShimEcnSession();
            const string ExpectedError = "You do not have permission to download/export the data.";

            // Act	
            _testEntity.ValidateExportPermission();

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () =>
                {
                    var lblNoDownloadMessage = GetField<Label>(LblNoDownloadMessage);
                    lblNoDownloadMessage.Text.ShouldContain(ExpectedError);
                    lblNoDownloadMessage.Visible.ShouldBeTrue();
                },
                () => GetField<Panel>(PnlUADExport).Visible.ShouldBeFalse());
        }

        [Test]
        [TestCase(DummyId, Enums.ViewType.ProductView)]
        [TestCase(DummyId, Enums.ViewType.ConsensusView)]
        [TestCase(TestNegativeNumber, Enums.ViewType.ProductView)]
        public void ValidateExportPermission_HasAccessIsTrue_SetControlsVisibility(
            int dcRunId,
            Enums.ViewType viewType)
        {
            // Arrange
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;
            ShimEcnSession();
            _testEntity.ViewType = viewType;
            _testEntity.dcRunID = dcRunId;
            _testEntity.dcTargetCodeID = DummyId;
            _testEntity.dcTypeCodeID = DummyId;
            _testEntity.PubIDs = new List<int> { DummyId };
            ShimUser.IsAdministratorUser = _ => false;
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (_, __) => new List<UASEntity::DataCompareView>
            {
                new UASEntity::DataCompareView
                {
                    DcTargetCodeId = DummyId,
                    DcTypeCodeId = DummyId,
                    DcTargetIdUad = DummyId,
                    IsBillable = true
                }
            };

            // Act	
            _testEntity.ValidateExportPermission();

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<RadCaptcha>(RcForDownload).Visible.ShouldBeTrue(),
                () => GetField<PlaceHolder>(PhRBDownload).Visible.ShouldBeTrue(),
                () => GetField<PlaceHolder>(PhShowHeader).Visible.ShouldBeTrue());
        }

        [Test]
        [TestCase(false, DummyString, false, true, true)]
        [TestCase(false, DummyString, false, true, false)]
        [TestCase(true, DummyString, true, true, true)]
        [TestCase(true, EmailString, true, true, true)]
        [TestCase(true, EmailString, true, false, true)]
        [TestCase(false, EmailString, true, true, true)]
        [TestCase(false, DummyString, true, false, true)]
        [TestCase(true, DummyString, true, true, true)]
        [TestCase(true, DummyString, true, true, false)]
        public void ValidateExportPermission_HasAccessForUADExport_SetControlsVisibility(
            bool showSave,
            string maskField,
            bool hasAccess,
            bool emailHasAccess,
            bool groupHasAccess)
        {
            // Arrange
            var invokeCount = 0;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, serviceFeatures, ____) =>
            {
                invokeCount++;
                if (invokeCount >= 2)
                {
                    switch (serviceFeatures)
                    {
                        case ServiceFeatures.UADExport:
                        case ServiceFeatures.DataCompare:
                        case ServiceFeatures.Marketo:
                            return hasAccess;
                        case ServiceFeatures.Email:
                            return emailHasAccess;
                        case ServiceFeatures.Groups:
                            return groupHasAccess;
                        default:
                            return true;
                    }
                }
                return true;
            };
            ShimEcnSession();
            ShimDownloadPanelBase.AllInstances.ShowHeaderCheckBoxGet = _ => true;
            _testEntity.Showsavetocampaign = showSave;
            _testEntity.Showexporttoemailmarketing = showSave;
            _testEntity.Showexporttomarketo = showSave;
            _testEntity.ViewType = Enums.ViewType.ProductView;
            _testEntity.PubIDs = new List<int> { DummyId };
            ShimUser.IsAdministratorUser = _ => false;
            ShimUserDataMask.GetByUserIDClientConnectionsInt32 = (_, __) => new List<UserDataMask>
            {
                new UserDataMask
                {
                    MaskField = maskField
                }
            };
            ShimMarketo.AllInstances.loadMarketoExportFields = _ => { };
            GetField<RadioButton>(RbCampaign).Checked = true;
            GetField<RadioButton>(RbGroupExport).Checked = groupHasAccess;

            // Act	
            _testEntity.ValidateExportPermission();

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<PlaceHolder>(PhRbCampaign).Visible.ShouldBe(showSave),
                () => GetField<PlaceHolder>(PhRBDownload).Visible.ShouldBe(hasAccess));
        }

        private static void ShimEcnSession()
        {
            var shimSession = new ShimECNSession { UserIDGet = () => DummyId };
            shimSession.Instance.CurrentUser = new User
            {
                IsKMStaff = true,
                UserID = DummyId
            };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimBaseControl.AllInstances.UserSessionGet = (sender) => shimSession.Instance;
            ShimBaseControl.AllInstances.clientconnectionsGet = _ => new ClientConnections();
        }
    }
} 
