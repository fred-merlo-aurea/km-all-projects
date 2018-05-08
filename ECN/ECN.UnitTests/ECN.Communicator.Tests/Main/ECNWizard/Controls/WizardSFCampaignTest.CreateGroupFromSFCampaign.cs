using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Controls.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Fakes;
using NUnit.Framework;
using Shouldly;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class WizardSFCampaignTest
    {
        private const string SampleId = "1";
        private const string SampleState = "alabama";
        private const string SampleEmail = "test@test.com";
        private const string SampleFirstName = "SampleFirstName";
        private const string SampleLastName = "SampleLastName";
        private const string SampleName = "SampleName";
        private const string SampleMailingCity = "SampleCity";
        private const string SampleMailingPostalCode = "99999";
        private const string SamplePhone = "8888888888";
        private const string SampleMobilePhone = "7777777777";
        private const string SampleMailingCountry = "US";
        private const string SampleTitle = "SampleTitle";
        private const string SampleWebsite = "http://www.KM.com";
        private const string ColumnAction = "Action";
        private const string ColumnCounts = "Counts";
        private const string SampleAction = "Update";
        private const string SampleGroupName = "SampleGroupName";
        private const string ShortNameSFID = "SFID";
        private const string ShortNameSFType = "SFType";
        private const string UTException = "UT Exception";
        private const string UpdatedRecordsField = "hUpdatedRecords";
        private const string ErrorPlaceHolder = "phError";
        private const string LabelErrorMessage = "lblErrorMessage";
        private const string CreateGroupFromSFCampaignMethodName = "CreateGroupFromSfCampaign";
        private bool _isGroupSaved;
        private bool _isGroupDataFieldsSaved;
        private bool _isEmailImported;
        private string _xmlProfile;
        private string _xmlUDF;

        [Test]
        public void CreateGroupFromSFCampaign_WhenSFContactListHasValueAndSFLeadIsEmpty_UpdatesDBAndReturnsGroupId()
        {
            // Arrange
            const string SFCampaignID = "1";
            SetFakesForCreateGroupFromSFCampaignMethod();

            // Act
            var resultGroupId = _privateTestObject.Invoke(CreateGroupFromSFCampaignMethodName, SFCampaignID);
            var hUpdatedRecords = Get<Hashtable>(_privateTestObject, UpdatedRecordsField);

            // Arrange
            resultGroupId.ShouldSatisfyAllConditions(
                () => resultGroupId.ShouldBe(1),
                () => _isGroupSaved.ShouldBeTrue(),
                () => _isGroupDataFieldsSaved.ShouldBeTrue(),
                () =>_isEmailImported.ShouldBeTrue(),
                () => hUpdatedRecords.ShouldNotBeNull(),
                () => hUpdatedRecords.ContainsKey(SampleAction.ToUpper()),
                () => hUpdatedRecords[SampleAction.ToUpper()].ToString().ShouldBe("1"),
                () => _xmlProfile.ShouldNotBeNullOrWhiteSpace(),
                () => _xmlProfile.ShouldContain(SampleEmail),
                () => _xmlProfile.ShouldContain(SampleId),
                () => _xmlProfile.ShouldContain(SampleFirstName),
                () => _xmlProfile.ShouldContain(SampleLastName),
                () => _xmlProfile.ShouldContain(SampleName),
                () => _xmlProfile.ShouldContain(SamplePhone),
                () => _xmlProfile.ShouldContain(SampleMailingPostalCode),
                () => _xmlProfile.ShouldContain(SampleMailingCity),
                () => _xmlProfile.ShouldContain(SampleMailingCountry),
                () => _xmlUDF.ShouldContain("1"),
                () => _xmlUDF.ShouldContain(SampleEmail),
                () => _xmlUDF.ShouldNotBeNullOrWhiteSpace());
        }

        [Test]
        public void CreateGroupFromSFCampaign_WhenSFContactListHasValueAndUpdateDBThrowsException_LogsException()
        {
            // Arrange
            const string SFCampaignID = "1";
            SetFakesForCreateGroupFromSFCampaignMethod();
            ShimWizardSFCampaign.AllInstances.UpdateToDBInt32StringString = (wz, gid, profile, udf) => 
                    throw new InvalidOperationException(UTException);
            var logMessage = string.Empty;
            ShimSF_Utilities.WriteToLogString = (log) => logMessage = log;

            // Act
            var resultGroupId = _privateTestObject.Invoke(CreateGroupFromSFCampaignMethodName, SFCampaignID);

            resultGroupId.ShouldSatisfyAllConditions(
                () => resultGroupId.ShouldBe(1),
                () => _isEmailImported.ShouldBeFalse(),
                () => _isGroupSaved.ShouldBeTrue(),
                () => _isGroupDataFieldsSaved.ShouldBeTrue(),
                () => logMessage.ShouldNotBeNullOrWhiteSpace(),
                () => logMessage.ShouldContain(UTException),
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, LabelErrorMessage).Text.ShouldContain("Blast: ERROR:Import Unsuccessful -error"));
        }

        [Test]
        public void CreateGroupFromSFCampaign_WhenSFContactLeadHasValueAndSFContactIsEmpty_UpdatesDBAndReturnsGroupId()
        {
            // Arrange
            const string SFCampaignID = "1";
            SetFakesForCreateGroupFromSFCampaignMethod();
            ShimSF_Lead.GetCampaignMembersStringString = (toke, sfCampaignId) => new List<SF_Lead> { GetSF_Lead() };
            ShimSF_Contact.GetCampaignMembersStringString = (token, sfCampaignId) => new List<SF_Contact> { };

            // Act
            var resultGroupId = _privateTestObject.Invoke(CreateGroupFromSFCampaignMethodName, SFCampaignID);
            var hUpdatedRecords = Get<Hashtable>(_privateTestObject, UpdatedRecordsField);

            // Arrange
            resultGroupId.ShouldSatisfyAllConditions(
                () => resultGroupId.ShouldBe(1),
                () => _isGroupSaved.ShouldBeTrue(),
                () => _isEmailImported.ShouldBeTrue(),
                () => _isGroupDataFieldsSaved.ShouldBeTrue(),
                () => hUpdatedRecords.ShouldNotBeNull(),
                () => hUpdatedRecords.ContainsKey(SampleAction.ToUpper()),
                () => hUpdatedRecords[SampleAction.ToUpper()].ToString().ShouldBe("1"),
                () => _xmlProfile.ShouldNotBeNullOrWhiteSpace(),
                () => _xmlProfile.ShouldContain(SampleEmail),
                () => _xmlProfile.ShouldContain(SampleId),
                () => _xmlProfile.ShouldContain(SampleFirstName),
                () => _xmlProfile.ShouldContain(SampleLastName),
                () => _xmlProfile.ShouldContain(SampleName),
                () => _xmlProfile.ShouldContain(SamplePhone),
                () => _xmlProfile.ShouldContain(SampleWebsite),
                () => _xmlUDF.ShouldContain("1"),
                () => _xmlUDF.ShouldContain(SampleEmail),
                () => _xmlUDF.ShouldNotBeNullOrWhiteSpace());
        }

        [Test]
        public void CreateGroupFromSFCampaign_WhenSFLeadListHasValueAndUpdateDBThrowsException_LogsException()
        {
            // Arrange
            const string SFCampaignID = "1";
            SetFakesForCreateGroupFromSFCampaignMethod();
            ShimSF_Lead.GetCampaignMembersStringString = (toke, sfCampaignId) => new List<SF_Lead> { GetSF_Lead() };
            ShimSF_Contact.GetCampaignMembersStringString = (token, sfCampaignId) => new List<SF_Contact> { };
            ShimWizardSFCampaign.AllInstances.UpdateToDBInt32StringString = (wz, gid, profile, udf) =>
                    throw new InvalidOperationException(UTException);
            var logMessage = string.Empty;
            ShimSF_Utilities.WriteToLogString = (log) => logMessage = log;

            // Act
            var resultGroupId = _privateTestObject.Invoke(CreateGroupFromSFCampaignMethodName, SFCampaignID);

            // Assert
            resultGroupId.ShouldSatisfyAllConditions(
                () => resultGroupId.ShouldBe(1),
                () => _isEmailImported.ShouldBeFalse(),
                () => _isGroupSaved.ShouldBeTrue(),
                () => _isGroupDataFieldsSaved.ShouldBeTrue(),
                () => logMessage.ShouldNotBeNullOrWhiteSpace(),
                () => logMessage.ShouldContain(UTException),
                () => Get<PlaceHolder>(_privateTestObject, ErrorPlaceHolder).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateTestObject, LabelErrorMessage).Text.ShouldContain("Blast: ERROR:Import Unsuccessful -error"));
        }

        private void SetFakesForCreateGroupFromSFCampaignMethod()
        {
            _isGroupSaved = false;
            _isGroupDataFieldsSaved = false;
            _isEmailImported = false;
            _xmlProfile = string.Empty;
            _xmlUDF = string.Empty;
            ShimGroup.SaveGroupUser = (grp, user) =>
            {
                _isGroupSaved = true;
                return grp.GroupID = 1;
            };
            ShimGroupDataFields.SaveGroupDataFieldsUser = (grpDataFields, user) =>
            {
                _isGroupDataFieldsSaved = true;
                return grpDataFields.GroupID;
            };
            ShimGroup.GetByGroupIDInt32User = (grpId, user) =>
            {
                return new CommunicatorEntities.Group
                {
                    GroupID = 1,
                    GroupName = SampleGroupName
                };
            };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (grpDFIds, user, b) =>
            {
                return new List<CommunicatorEntities.GroupDataFields>
                {
                     new CommunicatorEntities.GroupDataFields { GroupID = 1, ShortName = ShortNameSFID, GroupDataFieldsID = 1 },
                     new CommunicatorEntities.GroupDataFields { GroupID = 1, ShortName = ShortNameSFType, GroupDataFieldsID = 2 }
                };
            };
            ShimSF_Lead.GetCampaignMembersStringString = (toke, sfCampaignId) => new List<SF_Lead> { };
            ShimSF_Contact.GetCampaignMembersStringString = (token, sfCampaignId) => new List<SF_Contact> { GetSFContact() };
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (q, e, r, xmlProfile, xmlUDF, u, p, i, k, a) =>
            {
                _xmlProfile = xmlProfile;
                _xmlUDF = xmlUDF;
                _isEmailImported = true;
                return GetDataTable();
            };
        }

        private SF_Contact GetSFContact()
        {
            return new SF_Contact
            {
                Id = SampleId,
                MailingState = SampleState,
                Email = SampleEmail,
                FirstName = SampleFirstName,
                LastName = SampleLastName,
                Name = SampleName,
                MailingCity = SampleMailingCity,
                MailingPostalCode = SampleMailingPostalCode,
                Phone = SamplePhone,
                MobilePhone = SampleMobilePhone,
                MailingCountry = SampleMailingCountry,
                Title = SampleMailingCountry,
                BirthDate = new DateTime(1980, 10, 10),
            };
        }

        private SF_Lead GetSF_Lead()
        {
            return new SF_Lead
            {
                Id = SampleId,
                State = SampleState,
                Email = SampleEmail,
                FirstName = SampleFirstName,
                LastName = SampleLastName,
                Name = SampleName,
                MailingCity = SampleMailingCity,
                MailingPostalCode = SampleMailingPostalCode,
                Phone = SamplePhone,
                MobilePhone = SampleMobilePhone,
                MailingCountry = SampleMailingCountry,
                Title = SampleTitle,
                Website = SampleWebsite,
            };
        }

        private DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(ColumnAction, typeof(string));
            dataTable.Columns.Add(ColumnCounts, typeof(int));

            var row = dataTable.NewRow();
            row[ColumnAction] = SampleAction;
            row[ColumnCounts] = 1;
            dataTable.Rows.Add(row);
            return dataTable;
        }
    }
}
