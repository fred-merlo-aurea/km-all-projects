using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using UAS.Web.Tests.Helpers.Common;
using BusinessEnums = FrameworkUAD.BusinessLogic.Enums;
using HelpersFakes = UAS.Web.Tests.Helpers.Common.Fakes;
using ObjectClientConnection = KMPlatform.Object.ClientConnections;
using WebUtilities = UAS.Web.Helpers.Utilities;

namespace UAS.Web.Tests.Helpers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HelperClassesTest : HelpersFakes
    {
        private const int UserId = 10;
        private const int ProfileFieldsLength = 11;
        private const int OtherProfilesFilesLength = 34;
        private const int ProfilePermissionFilesLength = 7;
        private const int PubTransactionFilesLength = 7;
        private const int IssueSplitFieldsLength = 8;
        private const int TwoLength = 2;
        private const string IssueSplitKey = "IssueSplit";

        private ObjectClientConnection _clientConnection;
        private Dictionary<string, string> _lstExport;
        private Dictionary<string, string> _replacements;

        [SetUp]
        public void Initialize()
        {
            SetupFakes();

            _clientConnection = new ObjectClientConnection();
            _lstExport = new Dictionary<string, string>();
            _replacements = GetDefaultReplacements();
        }

        [Test]
        public void ExportProfileFieldsWithClientConnection_WhenClientConnectionIsNull_ThrowsException()
        {
            // Arrange
            _clientConnection = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                WebUtilities.ExportProfileFieldsWithClientConnection(_clientConnection, _lstExport, _replacements, UserId, true);
            });
        }

        [Test]
        public void ExportProfileFieldsWithClientConnection_WhenLstExportIsNull_ThrowsException()
        {
            // Arrange
            _lstExport = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                WebUtilities.ExportProfileFieldsWithClientConnection(_clientConnection, _lstExport, _replacements, UserId, true);
            });
        }

        [Test]
        public void ExportProfileFieldsWithClientConnection_WhenReplacementsIsNull_ThrowsException()
        {
            // Arrange
            _replacements = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                WebUtilities.ExportProfileFieldsWithClientConnection(_clientConnection, _lstExport, _replacements, UserId, true);
            });
        }

        [Test]
        public void ExportProfileFieldsWithClientConnection_WhenReplacementsIsDefault_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();

            // Act
            WebUtilities.ExportProfileFieldsWithClientConnection(_clientConnection, _lstExport, _replacements, UserId, true);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => _lstExport.Count.ShouldBe(ProfileFieldsLength),
                () => GetKey(_lstExport, Consts.FieldFirstName).ShouldBe(CreateDataTypeResult(Consts.FieldFirstName, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldLastName).ShouldBe(CreateDataTypeResult(Consts.FieldLastName, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldEmail).ShouldBe(CreateDataTypeResult(Consts.FieldEmail, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldCompany).ShouldBe(CreateDataTypeResult(Consts.FieldCompany, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldTitle).ShouldBe(CreateDataTypeResult(Consts.FieldTitle, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldAddress).ShouldBe(CreateDataTypeResult(Consts.FieldAddress1, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldAddress2).ShouldBe(CreateDataTypeResult(Consts.FieldAddress2, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldAddress3).ShouldBe(CreateDataTypeResult(Consts.FieldAddress3, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldCity).ShouldBe(CreateDataTypeResult(Consts.FieldCity, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldState).ShouldBe(CreateDataTypeResult(Consts.FieldRegionCode, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldZip).ShouldBe(CreateDataTypeResult(Consts.FieldZipCode, Consts.OtherDataType)));
        }

        [Test]
        public void ExportProfileFieldsWithClientConnection_WhenReplacementsIsDownloadType_ShouldExportAllProperties()
        {
            // Arrange
            _replacements = GetDownloadTypeReplacements();
            ShimForUserDataMaskProfileFields();

            // Act
            WebUtilities.ExportProfileFieldsWithClientConnection(_clientConnection, _lstExport, _replacements, UserId, false);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => _lstExport.Count.ShouldBe(ProfileFieldsLength),
                () => GetKey(_lstExport, Consts.FieldFirstName).ShouldBe(CreateDataTypeResult(Consts.FieldFName, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldLastName).ShouldBe(CreateDataTypeResult(Consts.FieldLName, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldEmail).ShouldBe(CreateDataTypeResult(Consts.FieldEmail, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldCompany).ShouldBe(CreateDataTypeResult(Consts.FieldCompany, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldTitle).ShouldBe(CreateDataTypeResult(Consts.FieldTitle, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldAddress).ShouldBe(CreateDataTypeResult(Consts.FieldAddress, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldMailStop).ShouldBe(CreateDataTypeResult(Consts.FieldMailStop, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldAddress3).ShouldBe(CreateDataTypeResult(Consts.FieldAddress3, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldCity).ShouldBe(CreateDataTypeResult(Consts.FieldCity, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldState).ShouldBe(CreateDataTypeResult(Consts.FieldState, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldZip).ShouldBe(CreateDataTypeResult(Consts.FieldZip, Consts.OtherDataType)));
        }

        [Test]
        public void ExportDefaultProfileFields_WhenLstExportIsNull_ThrowsException()
        {
            // Arrange
            _lstExport = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                WebUtilities.ExportDefaultProfileFields(_lstExport);
            });
        }

        [Test]
        public void ExportDefaultProfileFields_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();

            // Act
            WebUtilities.ExportDefaultProfileFields(_lstExport);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => _lstExport.Count.ShouldBe(ProfileFieldsLength),
                () => GetKey(_lstExport, Consts.FieldEmail).ShouldBe(CreateDataTypeResult(Consts.FieldEmail, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldFirstName).ShouldBe(CreateDataTypeResult(Consts.FieldFirstName, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldLastName).ShouldBe(CreateDataTypeResult(Consts.FieldLastName, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldCompany).ShouldBe(CreateDataTypeResult(Consts.FieldCompany, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldTitle).ShouldBe(CreateDataTypeResult(Consts.FieldTitle, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldAddress).ShouldBe(CreateDataTypeResult(Consts.FieldAddress1, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldAddress2).ShouldBe(CreateDataTypeResult(Consts.FieldAddress2, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldAddress3).ShouldBe(CreateDataTypeResult(Consts.FieldAddress3, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldCity).ShouldBe(CreateDataTypeResult(Consts.FieldCity, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldState).ShouldBe(CreateDataTypeResult(Consts.FieldRegionCode, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldZip).ShouldBe(CreateDataTypeResult(Consts.FieldZipCode, Consts.OtherDataType)));
        }

        [Test]
        public void ExportOtherProfilesFiles_WhenLstExportIsNull_ThrowsException()
        {
            // Arrange
            _lstExport = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                WebUtilities.ExportOtherProfilesFiles(_lstExport);
            });
        }

        [Test]
        public void ExportOtherProfilesFiles_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();

            // Act
            WebUtilities.ExportOtherProfilesFiles(_lstExport);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => _lstExport.Count.ShouldBe(OtherProfilesFilesLength),
                () => GetKey(_lstExport, Consts.FieldSubscriptionId).ShouldBe(CreateDataTypeResult(Consts.FieldSubscriptionId, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldBatch).ShouldBe(CreateDataTypeResult(Consts.FieldBatch, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldVerify).ShouldBe(CreateDataTypeResult(Consts.FieldVerify, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldImbseq).ShouldBe(CreateDataTypeResult(Consts.FieldImbseq, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldReqFlag).ShouldBe(CreateDataTypeResult(Consts.FieldReqFlag, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldWebsite).ShouldBe(CreateDataTypeResult(Consts.FieldWebsite, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldCopies).ShouldBe(CreateDataTypeResult(Consts.FieldCopies, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldWaveMailingId).ShouldBe(CreateDataTypeResult(Consts.FieldWaveMailingId, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldOrigsSrc).ShouldBe(CreateDataTypeResult(Consts.FieldOrigsSrc, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldPlus4).ShouldBe(CreateDataTypeResult(Consts.FieldPlus4, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldCountry).ShouldBe(CreateDataTypeResult(Consts.FieldCountry, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldPhone).ShouldBe(CreateDataTypeResult(Consts.FieldPhone, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldMobile).ShouldBe(CreateDataTypeResult(Consts.FieldMobile, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldFax).ShouldBe(CreateDataTypeResult(Consts.FieldFax, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldCounty).ShouldBe(CreateDataTypeResult(Consts.FieldCounty, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldDateCreated).ShouldBe(CreateDataTypeResult(Consts.FieldDateCreated, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldDateUpdated).ShouldBe(CreateDataTypeResult(Consts.FieldDateUpdated, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldGender).ShouldBe(CreateDataTypeResult(Consts.FieldGender, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldIsActive).ShouldBe(CreateDataTypeResult(Consts.FieldIsActive, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldQDate).ShouldBe(CreateDataTypeResult(Consts.FieldQualificationDate, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldEmailStatus).ShouldBe(CreateDataTypeResult(Consts.FieldEmailStatus, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldStatusUpdatedDate).ShouldBe(CreateDataTypeResult(Consts.FieldStatusUpdatedDate, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldDemo7).ShouldBe(CreateDataTypeResult(Consts.FieldDemo7, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldSequenceId).ShouldBe(CreateDataTypeResult(Consts.FieldSequenceId, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldExternalKeyId).ShouldBe(CreateDataTypeResult(Consts.FieldExternalKeyId, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldAccountNumber).ShouldBe(CreateDataTypeResult(Consts.FieldAccountNumber, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldSubscriberSourceCode).ShouldBe(CreateDataTypeResult(Consts.FieldSubscriberSourceCode, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldCategoryId).ShouldBe(CreateDataTypeResult(Consts.FieldCategoryId, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldQSourceName).ShouldBe(CreateDataTypeResult(Consts.FieldQSourceName, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldQSourceId).ShouldBe(CreateDataTypeResult(Consts.FieldQSourceId, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldPar3C).ShouldBe(CreateDataTypeResult(Consts.FieldPar3C, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldTransactionName).ShouldBe(CreateDataTypeResult(Consts.FieldTransactionName, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldTransactionStatus).ShouldBe(CreateDataTypeResult(Consts.FieldTransactionStatus, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldTransactionId).ShouldBe(CreateDataTypeResult(Consts.FieldTransactionId, Consts.OtherDataType)));
        }

        [Test]
        public void ExportProfilePermissionFiles_WhenLstExportIsNull_ThrowsException()
        {
            // Arrange
            _lstExport = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                WebUtilities.ExportProfilePermissionFiles(_lstExport);
            });
        }

        [Test]
        public void ExportProfilePermissionFiles_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();

            // Act
            WebUtilities.ExportProfilePermissionFiles(_lstExport);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => _lstExport.Count.ShouldBe(ProfilePermissionFilesLength),
                () => GetKey(_lstExport, Consts.FieldMailPermission).ShouldBe(CreateDataTypeResult(Consts.FieldMailPermission, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldFaxPermission).ShouldBe(CreateDataTypeResult(Consts.FieldFaxPermission, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldPhonePermission).ShouldBe(CreateDataTypeResult(Consts.FieldPhonePermission, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldOtherProductsPermission).ShouldBe(CreateDataTypeResult(Consts.FieldOtherProductsPermission, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldThirdPartyPermission).ShouldBe(CreateDataTypeResult(Consts.FieldThirdPartyPermission, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldEmailRenewPermission).ShouldBe(CreateDataTypeResult(Consts.FieldEmailRenewPermission, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldTextPermission).ShouldBe(CreateDataTypeResult(Consts.FieldTextPermission, Consts.OtherDataType)));
        }

        [Test]
        public void ExportPubTransactionFiles_WhenLstExportIsNull_ThrowsException()
        {
            // Arrange
            _lstExport = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                WebUtilities.ExportPubTransactionFiles(_lstExport, true);
            });
        }

        [Test]
        public void ExportPubTransactionFiles_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();

            // Act
            WebUtilities.ExportPubTransactionFiles(_lstExport, false);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => _lstExport.Count.ShouldBe(PubTransactionFilesLength),
                () => GetKey(_lstExport, Consts.FieldTransactionDate).ShouldBe(CreateDataTypeResult(Consts.FieldPubTransactionDate, Consts.OtherDataType)),
                () => GetKeyWithUppercase(_lstExport, Consts.FieldEmailId).ShouldBe(CreateDataTypeResult(Consts.FieldEmailId, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldIgrpNo).ShouldBe(CreateDataTypeResult(Consts.FieldIgrpNo, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldCgrpNo).ShouldBe(CreateDataTypeResult(Consts.FieldCgrpNo, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldScore).ShouldBe(CreateDataTypeResult(Consts.FieldScore, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldLastOpenedDate).ShouldBe(CreateDataTypeResult(Consts.FieldLastOpenedDate, Consts.OtherDataType)),
                () => GetKey(_lstExport, Consts.FieldLastOpenedPubCode).ShouldBe(CreateDataTypeResult(Consts.FieldLastOpenedPubCode, Consts.VarCharDataType)));
        }
        
        [Test]
        public void ExtractDemoFields_WhenClientConnectionIsNull_ThrowsException()
        {
            // Arrange
            _clientConnection = null;
            
            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                WebUtilities.ExtractDemoFields(_clientConnection, _lstExport, BusinessEnums.ExportType.Campaign, PubId);
            });
        }

        [Test]
        public void ExtractDemoFields_WhenLstExportIsNull_ThrowsException()
        {
            // Arrange
            _lstExport = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                WebUtilities.ExtractDemoFields(_clientConnection, _lstExport, BusinessEnums.ExportType.Campaign, PubId);
            });
        }

        [Test]
        public void ExtractDemoFields_WhenExportFieldTypeIsEcn_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();
            ShimResponseGroupSelect();

            // Act
            WebUtilities.ExtractDemoFields(_clientConnection, _lstExport, BusinessEnums.ExportType.ECN, PubId);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => GetKey(_lstExport, DisplayNameText).ShouldBe(CreateCustomResult(ResponseGroupId.ToString(), Consts.OtherDataType)));
        }

        [Test]
        public void ExtractDemoFields_WhenExportFieldTypeIsNotEcn_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();
            ShimResponseGroupSelect();

            // Act
            WebUtilities.ExtractDemoFields(_clientConnection, _lstExport, BusinessEnums.ExportType.Campaign, PubId);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => _lstExport.Count.ShouldBe(TwoLength),
                () => GetKey(_lstExport, DisplayNameText).ShouldBe(CreateCustomResult(ResponseGroupId.ToString(), Consts.OtherDataType)),
                () => GetKey(_lstExport,DisplayNameText + DescriptionKey)
                    .ShouldBe(CreateCustomResult(ResponseGroupId + DescriptionKey, Consts.VarCharDataType)));
        }

        [Test]
        public void ExportAdHocFields_WhenClientConnectionIsNull_ThrowsException()
        {
            // Arrange
            _clientConnection = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                WebUtilities.ExportAdHocFields(_clientConnection, _lstExport, PubId);
            });
        }

        [Test]
        public void ExportAdHocFields_WhenLstExportIsNull_ThrowsException()
        {
            // Arrange
            _lstExport = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                WebUtilities.ExportAdHocFields(_clientConnection, _lstExport, PubId);
            });
        }

        [Test]
        public void ExportAdHocFields_WhenCustomFieldDataTypeIsVarchar_ShouldExportCustomProperty()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();
            ShimProductsSubscriptions(BusinessEnums.FieldType.Varchar);

            // Act
            WebUtilities.ExportAdHocFields(_clientConnection, _lstExport, PubId);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => GetKey(_lstExport, CustomFieldText).ShouldBe(CreateCustomResult(StandardFieldText, Consts.VarCharDataType)));
        }

        [Test]
        public void ExportAdHocFields_WhenCustomFieldDataTypeIsOther_ShouldExportCustomProperty()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();
            ShimProductsSubscriptions(BusinessEnums.FieldType.Int);

            // Act
            WebUtilities.ExportAdHocFields(_clientConnection, _lstExport, PubId);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => GetKey(_lstExport, CustomFieldText).ShouldBe(CreateCustomResult(StandardFieldText, Consts.OtherDataType)));
        }

        [Test]
        public void ExportIssueSplitFields_WhenLstExportIsNull_ThrowsException()
        {
            // Arrange
            _lstExport = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
            { 
                WebUtilities.ExportIssueSplitFields(_lstExport);
            });
        }

        [Test]
        public void ExportIssueSplitFields_WhenReplacementsIsDefault_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();

            // Act
            WebUtilities.ExportIssueSplitFields(_lstExport);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(
                () => _lstExport.Count.ShouldBe(IssueSplitFieldsLength),
                () => GetKey(_lstExport, Consts.FieldAcsCode).ShouldBe(CreateDataTypeResult(Consts.FieldAcsCode, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldExpireIssueDate).ShouldBe(CreateDataTypeResult(Consts.FieldExpireIssueDate, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldExpQdate).ShouldBe(CreateDataTypeResult(Consts.FieldExpQdate, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldKeyCode).ShouldBe(CreateDataTypeResult(Consts.FieldKeyCode, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldKeyline).ShouldBe(CreateDataTypeResult(Consts.FieldKeyline, Consts.VarCharDataType)),
                () => GetKeyWithUppercase(_lstExport, Consts.FieldMailerId).ShouldBe(CreateDataTypeResult(Consts.FieldMailerId, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldSplitDescription).ShouldBe(CreateDataTypeResult(Consts.FieldSplitDescription, Consts.VarCharDataType)),
                () => GetKey(_lstExport, Consts.FieldSplitName).ShouldBe(CreateDataTypeResult(Consts.FieldSplitName, Consts.VarCharDataType)));
        }

        [Test]
        public void GetExportingFields_WhenExportTypeIsEcn_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();
            var result = ProfileFieldsLength + OtherProfilesFilesLength + ProfilePermissionFilesLength + 
                         PubTransactionFilesLength;

            // Act
            _lstExport = WebUtilities.GetExportingFields(_clientConnection, PubId, BusinessEnums.ExportType.ECN, UserId);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(() => _lstExport.Count.ShouldBe(result));
        }

        [Test]
        public void GetExportingFields_WhenExportTypeIsNotEcn_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();
            var result = ProfileFieldsLength + OtherProfilesFilesLength + ProfilePermissionFilesLength + 
                         PubTransactionFilesLength;

            // Act
            _lstExport = WebUtilities.GetExportingFields(_clientConnection, PubId, BusinessEnums.ExportType.Campaign, UserId);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(() => _lstExport.Count.ShouldBe(result));
        }

        [Test]
        public void GetExportingFields_WhenDownloadTypeEqualsIssueSplit_ShouldExportAllProperties()
        {
            // Arrange
            ShimForUserDataMaskProfileFields();
            var result = ProfileFieldsLength + OtherProfilesFilesLength + ProfilePermissionFilesLength + 
                         PubTransactionFilesLength + IssueSplitFieldsLength;

            // Act
            _lstExport = WebUtilities.GetExportingFields(
                _clientConnection, PubId, BusinessEnums.ExportType.ECN, UserId, downLoadType: IssueSplitKey);

            // Assert
            _lstExport.ShouldNotBeNull();
            _lstExport.ShouldSatisfyAllConditions(() => _lstExport.Count.ShouldBe(result));
        }

        private static string CreateCustomResult(string fieldText, string dbDataType)
        {
            return $"{fieldText}{Consts.RegexJoinSymbol}{dbDataType}";
        }

        private static Dictionary<string, string> GetDefaultReplacements()
        {
            return new Dictionary<string, string>
            {
                ["IMBSEQ"] = "Imbseq",
                ["QualificationDate"] = "QDate",
                ["State"] = "RegionCode",
                ["Zip"] = "ZipCode"
            };
        }

        private static Dictionary<string, string> GetDownloadTypeReplacements()
        {
            return new Dictionary<string, string>
            {
                ["FirstName"] = "FNAME",
                ["LastName"] = "LNAME",
                ["Address2"] = "MailStop"
            };
        }
    }
}
