using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAD.DataCompare.Web.Models;
using Shouldly;
using static KMPlatform.Enums;
using static FrameworkUAD_Lookup.Enums;
using UASEntities = FrameworkUAS.Entity;
using UADEntities = FrameworkUAD_Lookup.Entity;

namespace UAD.DataCompare.Web.Tests.Controllers.UAD
{
    public partial class DatacompareControllerTest
    {
        private const string SaveSourceFileMethodName = "SaveSourceFile";
        private const string TestUser = "TestUser";
        private const string YesString = "Yes";
        private const string NoString = "No";
        private const string SampleFileName = "SampleFileName";
        private const string SampleNotes = "SampleNotes";
        private const string SampleDelimiter = ",";
        private const string SampleEmail = "test@test.com";
        private const string SampleFileExtension = ".txt";
        private bool _isSourceFileSaved;
        private PrivateObject _controllerPrivate;
        private UASEntities.SourceFile _savedSourceFile;

        [Test]
        public void SaveSourceFile_WhenSourceFileIDGreaterThanZero_SavesSourceFile()
        {
            // Arrange
            SetFakesForSaveSourceFileMethod();
            _controllerPrivate = new PrivateObject(_controller);
            var fileDetails = new FileDetails
            {
                SourceFileID = 1,
                FileName = SampleFileName,
                Delimiter = SampleDelimiter,
                HasQuotation = YesString,
                Notes = SampleNotes,
                IsImportBillable = NoString,
            };

            // Act
            var resultSourceFileId = _controllerPrivate.Invoke(SaveSourceFileMethodName, fileDetails);

            // Assert
            resultSourceFileId.ShouldSatisfyAllConditions(
                () => resultSourceFileId.ShouldBe(1),
                () => _isSourceFileSaved.ShouldBeTrue(),
                () => _savedSourceFile.ShouldNotBeNull(),
                () => _savedSourceFile.SourceFileID.ShouldBe(fileDetails.SourceFileID),
                () => _savedSourceFile.FileName.ShouldBe(fileDetails.FileName),
                () => _savedSourceFile.Delimiter.ShouldBe(fileDetails.Delimiter),
                () => _savedSourceFile.IsTextQualifier.ShouldBeTrue(),
                () => _savedSourceFile.Notes.ShouldBe(fileDetails.Notes),
                () => _savedSourceFile.IsBillable.ShouldBeFalse());
        }

        [Test]
        public void SaveSourceFile_WhenSourceFileIDGreaterThanZeroAndImportsBillableTrue_SavesSourceFile()
        {
            // Arrange
            SetFakesForSaveSourceFileMethod();
            _controllerPrivate = new PrivateObject(_controller);
            var fileDetails = new FileDetails
            {
                SourceFileID = 1,
                FileName = SampleFileName,
                Delimiter = SampleDelimiter,
                HasQuotation = NoString,
                Notes = SampleNotes,
                IsImportBillable = YesString,
            };

            // Act
            var resultSourceFileId = _controllerPrivate.Invoke(SaveSourceFileMethodName, fileDetails);

            // Assert
            resultSourceFileId.ShouldSatisfyAllConditions(
                () => resultSourceFileId.ShouldBe(1),
                () => _isSourceFileSaved.ShouldBeTrue(),
                () => _savedSourceFile.ShouldNotBeNull(),
                () => _savedSourceFile.SourceFileID.ShouldBe(fileDetails.SourceFileID),
                () => _savedSourceFile.FileName.ShouldBe(fileDetails.FileName),
                () => _savedSourceFile.Delimiter.ShouldBe(fileDetails.Delimiter),
                () => _savedSourceFile.IsTextQualifier.ShouldBeFalse(),
                () => _savedSourceFile.Notes.ShouldBe(fileDetails.Notes),
                () => _savedSourceFile.IsBillable.ShouldBeTrue());
        }

        [Test]
        public void SaveSourceFile_WhenSourceFileIDZeroAndImportsBillableTrue_SavesSourceFile()
        {
            // Arrange
            SetFakesForSaveSourceFileMethod();
            _controllerPrivate = new PrivateObject(_controller);
            var fileDetails = new FileDetails
            {
                SourceFileID = 0,
                FileName = SampleFileName,
                Delimiter = SampleDelimiter,
                HasQuotation = NoString,
                Notes = SampleNotes,
                IsImportBillable = YesString,
                NotificationEmail = SampleEmail,
            };

            // Act
            var resultSourceFileId = _controllerPrivate.Invoke(SaveSourceFileMethodName, fileDetails);

            // Assert
            resultSourceFileId.ShouldSatisfyAllConditions(
                () => resultSourceFileId.ShouldBe(0),
                () => _isSourceFileSaved.ShouldBeTrue(),
                () => _savedSourceFile.ShouldNotBeNull(),
                () => _savedSourceFile.SourceFileID.ShouldBe(fileDetails.SourceFileID),
                () => _savedSourceFile.FileName.ShouldBe(fileDetails.FileName),
                () => _savedSourceFile.Delimiter.ShouldBe(fileDetails.Delimiter),
                () => _savedSourceFile.IsTextQualifier.ShouldBeFalse(),
                () => _savedSourceFile.Notes.ShouldBe(fileDetails.Notes),
                () => _savedSourceFile.IsBillable.ShouldBeTrue(),
                () => _savedSourceFile.IsDeleted.ShouldBeFalse(),
                () => _savedSourceFile.IsIgnored.ShouldBeFalse(),
                () => _savedSourceFile.DatabaseFileTypeId.ShouldBe(1),
                () => _savedSourceFile.FileRecurrenceTypeId.ShouldBe(1),
                () => _savedSourceFile.FileSnippetID.ShouldBe(1),
                () => _savedSourceFile.ServiceID.ShouldBe(1),
                () => _savedSourceFile.ServiceFeatureID.ShouldBe(1),
                () => _savedSourceFile.PublicationID.ShouldBe(0));
        }

        [Test]
        public void SaveSourceFile_WhenSourceFileIDZeroDataBaseFileTypeListIsNull_SavesSourceFile()
        {
            // Arrange
            SetFakesForSaveSourceFileMethod();
            _controllerPrivate = new PrivateObject(_controller);
            var fileDetails = new FileDetails
            {
                SourceFileID = 0,
                FileName = SampleFileName,
                Delimiter = SampleDelimiter,
                HasQuotation = YesString,
                Notes = SampleNotes,
                IsImportBillable = NoString,
                NotificationEmail = SampleEmail,
            };
            ShimCode.AllInstances.SelectEnumsCodeType = (c, ct) => GetCodesList(codeId: 0);

            // Act
            var resultSourceFileId = _controllerPrivate.Invoke(SaveSourceFileMethodName, fileDetails);

            // Assert
            resultSourceFileId.ShouldSatisfyAllConditions(
                () => resultSourceFileId.ShouldBe(0),
                () => _isSourceFileSaved.ShouldBeTrue(),
                () => _savedSourceFile.ShouldNotBeNull(),
                () => _savedSourceFile.SourceFileID.ShouldBe(fileDetails.SourceFileID),
                () => _savedSourceFile.FileName.ShouldBe(fileDetails.FileName),
                () => _savedSourceFile.Delimiter.ShouldBe(fileDetails.Delimiter),
                () => _savedSourceFile.IsTextQualifier.ShouldBeTrue(),
                () => _savedSourceFile.Notes.ShouldBe(fileDetails.Notes),
                () => _savedSourceFile.IsBillable.ShouldBeFalse(),
                () => _savedSourceFile.IsDeleted.ShouldBeFalse(),
                () => _savedSourceFile.IsIgnored.ShouldBeFalse(),
                () => _savedSourceFile.DatabaseFileTypeId.ShouldBe(1),
                () => _savedSourceFile.FileRecurrenceTypeId.ShouldBe(0),
                () => _savedSourceFile.FileSnippetID.ShouldBe(1),
                () => _savedSourceFile.ServiceID.ShouldBe(1),
                () => _savedSourceFile.ServiceFeatureID.ShouldBe(1),
                () => _savedSourceFile.PublicationID.ShouldBe(0));
        }

        private void SetFakesForSaveSourceFileMethod()
        {
            InitializeSessionFakes();
            _isSourceFileSaved = false;
            _savedSourceFile = null;
            ShimSourceFile.AllInstances.SelectSourceFileIDInt32Boolean = (s, fid, b) => new UASEntities.SourceFile
            {
                SourceFileID = 1
            };
            ShimSourceFile.AllInstances.SaveSourceFileBoolean = (s, sourceFile, b) => 
            {
                _savedSourceFile = sourceFile;
                _isSourceFileSaved = true;
                return _savedSourceFile.SourceFileID;
            };
            ShimCode.AllInstances.SelectCodeValueEnumsCodeTypeString = (c, e, ct) => GetCodesList()[0];
            ShimCode.AllInstances.SelectEnumsCodeType = (c, ct) => GetCodesList();
            ShimService.AllInstances.SelectForClientIDInt32Boolean = (s, cid, b) => new List<Service>
            {
                new Service
                {
                    ServiceID = 1,
                    ServiceCode =  Services.UADFILEMAPPER.ToString(),
                }
            };
            ShimServiceFeature.AllInstances.SelectOnlyEnabledClientIDInt32Int32 = (sf, c, u) => new List<ServiceFeature>
            {
                new ServiceFeature { SFCode = "DC" , ServiceFeatureID = 1 }
            };
        }
        private void InitializeSessionFakes()
        {
            ShimECNSession.AllInstances.ClientIDGet = (s) => 1;
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUser, IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private List<UADEntities.Code> GetCodesList(int codeId = 1)
        {
            return new List<UADEntities.Code>
            {
                new UADEntities.Code
                {
                    CodeId = codeId,
                    CodeName = FileTypes.Data_Compare.ToString().Replace("_", " "),
                    IsActive = true
                },
                new UADEntities.Code
                {
                    CodeId = codeId,
                    CodeName = FileRecurrenceTypes.One_Time.ToString().Replace("_", " "),
                    IsActive = true
                },
                new UADEntities.Code
                {
                    CodeId = 1,
                    CodeName = FileTypes.Audience_Data.ToString().Replace("_", " "),
                    IsActive = true
                },
            };
        }
    }
}
