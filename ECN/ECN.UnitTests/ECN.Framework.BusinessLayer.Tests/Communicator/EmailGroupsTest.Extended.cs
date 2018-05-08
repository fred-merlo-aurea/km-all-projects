using System;
using System.Collections.Generic;
using System.Data;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Fakes;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using ECN_Framework_DataLayer.Communicator.Fakes;
using NUnit.Framework;
using Shouldly;
using Entities = ECN_Framework_Entities.Communicator;
using BussinessFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMBusinessFakes = KMPlatform.BusinessLogic.Fakes;
using static ECN_Framework_DataLayer.Communicator.EmailGroup;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class EmailGroupsTest
    {
        private string _executedSqlCommand;

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void ImportEmails_WhenUserHasAccess_ReturnsDataTable(int masterSuppressionValue)
        {
            // Arrange    
            var user = new User();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimGroup.GetByGroupIDInt32User = (_,__) => new Entities.Group
            {
                MasterSupression = masterSuppressionValue
            };
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.ImportEmails(user, 1, 1, string.Empty, string.Empty, string.Empty, string.Empty, false);

            // Assert
            dataTable.ShouldSatisfyAllConditions(
                () => dataTable.ShouldNotBeNull(),
                () => dataTable.Rows.Count.ShouldBe(1),
                () => dataTable.Rows[0][GroupIDColumn].ShouldBe(1),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => {
                    if (masterSuppressionValue == 1)
                        _executedSqlCommand.ShouldContain("e_EmailGroup_ImportMSEmails");
                    else
                        _executedSqlCommand.ShouldContain("e_EmailGroup_ImportEmails");
                });
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void ImportEmails_PreImportResults_WhenUserHasAccess_ReturnsDataTable(int masterSuppressionValue)
        {
            // Arrange    
            var user = new User();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimGroup.GetByGroupIDInt32User = (_, __) => new Entities.Group
            {
                MasterSupression = masterSuppressionValue
            };
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.ImportEmails_PreImportResults(user, 1, 1, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "e_EmailGroup_ImportEmails_PreImportResults");
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void ImportEmails_NoAccessCheck_WhenUserHasAccess_ReturnsDataTable(int masterSuppressionValue)
        {
            // Arrange    
            var user = new User();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimGroup.GetByGroupID_NoAccessCheckInt32 = (_) => new Entities.Group
            {
                MasterSupression = masterSuppressionValue
            };
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.ImportEmails_NoAccessCheck(user, 1, 1, string.Empty, string.Empty, string.Empty, string.Empty, false);

            // Assert
            dataTable.ShouldSatisfyAllConditions(
                () => dataTable.ShouldNotBeNull(),
                () => dataTable.Rows.Count.ShouldBe(1),
                () => dataTable.Rows[0][GroupIDColumn].ShouldBe(1),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => {
                    if (masterSuppressionValue == 1)
                        _executedSqlCommand.ShouldContain("e_EmailGroup_ImportMSEmails");
                    else
                        _executedSqlCommand.ShouldContain("e_EmailGroup_ImportEmails");
                });
        }

        [Test]
        public void ExportFromImportEmails_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.ExportFromImportEmails(user, string.Empty, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "e_EmailGroup_ExportFromImportEmails");
        }

        [Test]
        public void ImportEmailsOverrideCustomer_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.ImportEmailsOverrideCustomer(user, 1, 1, string.Empty, string.Empty, string.Empty, string.Empty, false);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, ProcedureEmailGroupImportEmails);
        }

        [Test]
        public void ImportEmailsWithDupes_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User
            {
                CurrentClient = new Client { ClientID = 1 }
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            KMBusinessFakes.ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (u, e, ei) => true;
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.
                ImportEmailsWithDupes(user, 1, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, ProcedureEmailGroupImportEmailsWithDupes);
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void ImportEmailsWithDupes_WhenUserHasNoAccess_ThrowsException(bool hasAccess)
        {
            // Arrange    
            var user = new User
            {
                CurrentClient = new Client { ClientID = 1 }
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => hasAccess;
            KMBusinessFakes.ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (u, e, ei) => false;
            SetDataFunctionGetDataTableFakes();

            // Act
            var exp = Should.Throw<SecurityException>(() => EmailGroup.
                ImportEmailsWithDupes(user, 1, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false, string.Empty));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.Message.ShouldContain(SecurityErrorMessage));
        }

        [Test]
        public void ImportEmailsWithDupes_NoAccessCheck_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User
            {
                CurrentClient = new Client { ClientID = 1 }
            };
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.
                ImportEmailsWithDupes_NoAccessCheck(user, 1, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, ProcedureEmailGroupImportEmailsWithDupes);
        }

        [Test]
        public void GetColumnNames_NoAccessCheck_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.GetColumnNames();

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "e_EmailGroup_GetColumnNames");
        }

        [Test]
        public void ImportEmailsToCS_NoAccessCheck_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.ImportEmailsToCS(user, 1, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "e_EmailGroup_ImportEmailsToCS");
        }

        [Test]
        public void ImportEmailsToGlobalMS_NoAccessCheck_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.ImportEmailsToGlobalMS(user, 1, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "e_EmailGroup_ImportEmailsToGlobalMS");
        }

        [Test]
        public void ImportEmailsToNoThreshold_NoAccessCheck_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.ImportEmailsToNoThreshold(user, 1, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "e_EmailGroup_ImportEmailsToNoThreshold");
        }

        [Test]
        public void GetBestProfileForEmailAddress_NoAccessCheck_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.GetBestProfileForEmailAddress(1, 1, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "e_EmailGroup_GetBestProfileForEmailAddress");
        }

        [Test]
        public void GetGroupEmailProfilesWithUDF_WhenUsersHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.GetGroupEmailProfilesWithUDF(1, 1, string.Empty, DateTime.UtcNow, DateTime.UtcNow, false, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "sp_GetGroupEmailProfilesWithUDF_By_Date");
        }

        [Test]
        public void GetGroupEmailProfilesWithUDF_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.GetGroupEmailProfilesWithUDF(1, 1, string.Empty, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "sp_GetGroupEmailProfilesWithUDF");
        }

        [Test]
        public void PreviewFilteredEmails_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.PreviewFilteredEmails(1, 1, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "e_EmailGroup_PreviewFilteredEmails");
        }

        [Test]
        public void PreviewFilteredEmails_Paging_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.PreviewFilteredEmails_Paging(1, 1, string.Empty, string.Empty, string.Empty, 1, 1);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "e_EmailGroup_PreviewFilteredEmails_Paging");
        }

        [Test]
        public void GetByUserID_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.GetByUserID(1, 1);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "v_EmailGroup_Get_UserID");
        }

        [Test]
        public void GetByBounceScore_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataTable = EmailGroup.GetByBounceScore(1, 1, 1, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "v_EmailGroup_Get_BounceScore");
        }

        [Test]
        public void GetBySearchStringPaging_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataSet = EmailGroup.GetBySearchStringPaging(1, 1, 1, 1, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataSet.Tables[0], "v_EmailGroup_Get_Paging");
        }

        [Test]
        public void GetBySearchStringPagingByDate_WhenUserHasAccess_ReturnsDataTable()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();

            // Act
            var dataSet = EmailGroup.GetBySearchStringPaging(1, 1, 1, 1, DateTime.UtcNow, DateTime.UtcNow, false, string.Empty);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataSet.Tables[0], "v_EmailGroup_Get_Paging_By_Date");
        }

        [Test]
        public void DeleteFromMasterSuppressionGroup_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            SetDataFunctionGetDataTableFakes();

            // Act
            EmailGroup.DeleteFromMasterSuppressionGroup(1, 1, user);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("e_EmailGroup_DeleteFromMasterSuppression"));
        }

        [Test]
        public void DeleteFromMasterSuppressionGroup_WhenUserHasNoAccess_ThrowsECNException()
        {
            // Arrange    
            var user = new User { IsActive = false, IsPlatformAdministrator = false };
            SetDataFunctionGetDataTableFakes();

            // Act
            var exp = Should.Throw<ECNException>(() => EmailGroup.DeleteFromMasterSuppressionGroup(1, 1, user));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.ErrorList.Count.ShouldBe(1),
                () => exp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Access Denied. Only System Administrators can delete")));
        }

        [Test]
        public void DeleteFromMasterSuppressionGroup_NoAccessCheck_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            SetDataFunctionGetDataTableFakes();

            // Act
            EmailGroup.DeleteFromMasterSuppressionGroup_NoAccessCheck(1, 1, 1, user);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("e_EmailGroup_DeleteFromMasterSuppression"));
        }

        [Test]
        public void DeleteFromMasterSuppressionGroup_NoAccessCheck_WhenUserHasNoAccess_ThrowsECNException()
        {
            // Arrange    
            var user = new User { IsActive = false, IsPlatformAdministrator = false };
            SetDataFunctionGetDataTableFakes();

            // Act
            var exp = Should.Throw<ECNException>(() => EmailGroup.DeleteFromMasterSuppressionGroup_NoAccessCheck(1, 1, 1,user));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.ErrorList.Count.ShouldBe(1),
                () => exp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Access Denied. Only System Administrators can delete")));
        }

        [Test]
        public void UnsubscribeSubscribers_NoAccessCheck_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => true);
            ShimEmailGroup.GetListSqlCommand = (cmd) =>
            {
                _executedSqlCommand = cmd.CommandText;
                return new List<Entities.EmailGroup>();
            };

            // Act
            EmailGroup.UnsubscribeSubscribers(1, string.Empty, user);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("update EmailGroups set SubscribeTypeCode = 'U', LastChanged = GetDate()"));
        }

        [Test]
        public void UnsubscribeSubscribers_NoAccessCheck_NoAccessCheck_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            SetDataFunctionGetDataTableFakes();
            
            // Act
            EmailGroup.UnsubscribeSubscribers_NoAccessCheck(1, string.Empty);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("update EmailGroups set SubscribeTypeCode = 'U', LastChanged = GetDate()"));
        }

        [Test]
        public void AddToMasterSuppression_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            EmailGroup.AddToMasterSuppression(1, 1, user);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("e_EmailGroup_AddToMasterSuppression"));
        }

        [Test]
        public void UnsubscribeSubscribersInFolder_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            EmailGroup.UnsubscribeSubscribersInFolder(1, string.Empty, user);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("UPDATE EmailGroups SET SubscribeTypeCode = 'U', LastChanged = GETDATE()"));
        }

        [Test]
        public void UnsubscribeBounces_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            EmailGroup.UnsubscribeBounces(1, string.Empty, user);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("e_EmailGroup_UnsubscribeBounces"));
        }

        [Test]
        public void GetEmailIDFromComposite_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
           var emailId = EmailGroup.GetEmailIDFromComposite(1, 1, string.Empty, string.Empty, string.Empty, user);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => emailId.ShouldBe(1),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("v_EmailGroup_Select_CompositeKey"));
        }

        [Test]
        public void GetEmailIDFromWhatEmail_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var emailId = EmailGroup.GetEmailIDFromWhatEmail(1, 1, string.Empty, user);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => emailId.ShouldBe(1),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("v_EmailGroup_Select_WhatEmail"));
        }

        [Test]
        public void GetEmailIDFromWhatEmail_NoAccessCheck_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var emailId = EmailGroup.GetEmailIDFromWhatEmail_NoAccessCheck(1, 1, string.Empty);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => emailId.ShouldBe(1),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("v_EmailGroup_Select_WhatEmail"));
        }

        [Test]
        public void GetSubscriberCount_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            var user = new User();
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var emailId = EmailGroup.GetSubscriberCount(1, 1, user);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => emailId.ShouldBe(1),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("v_EmailGroup_GetSubscriberCount"));
        }

        [Test]
        public void GetSubscriberCount_NoAccessCheck_WhenUserHasAccess_ExecutesQuery()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var emailId = EmailGroup.GetSubscriberCount_NoAccessCheck(1, 1);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => emailId.ShouldBe(1),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("v_EmailGroup_GetSubscriberCount"));
        }

        [Test]
        public void GetSubscriberStatus_WhenUserHasAccess_RetrunsDataTable()
        {
            // Arrange    
            var user = new User { CustomerID = 1 };
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var dataTable = EmailGroup.GetSubscriberStatus(string.Empty, user);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "v_EmailGroup_GetSubscriberStatus");
        }

        [Test]
        public void GetUnsubscribesForDay_WhenUserHasAccess_RetrunsDataTable()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var dataTable = EmailGroup.GetUnsubscribesForDay(1, 1, 1);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "v_EmailGroup_GetUnsubscribesForDay");
        }

        [Test]
        public void GetUnsubscribesForCurrentBackToDay_WhenUserHasAccess_RetrunsDataTable()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var dataTable = EmailGroup.GetUnsubscribesForCurrentBackToDay(1, 1);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "v_EmailGroup_GetUnsubscribesForCurrentBackToDay");
        }

        [Test]
        public void GetEmailsFromOtherGroupsToUnsubscribe_WhenUserHasAccess_RetrunsDataTable()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var dataTable = EmailGroup.GetEmailsFromOtherGroupsToUnsubscribe(string.Empty, 1, 1);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "v_EmailGroup_GetEmailsFromOtherGroupsToUnsubscribe");
        }

        [Test]
        public void GetGroupStats_WhenUserHasAccess_RetrunsDataTable()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var dataTable = EmailGroup.GetGroupStats( 1, 1);

            // Assert
            AssertDataTableAndExecutedSqlCommand(dataTable, "v_EmailGroup_GetGroupStats");
        }

        [Test]
        public void CopyProfileFromGroup_WhenUserHasAccess_RetrunsDataTable()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();

            // Act
            EmailGroup.CopyProfileFromGroup(1, 1, 1);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("e_EmailGroup_CopyProfileFromGroup"));
        }

        [Test]
        public void EmailExistsInCustomerSeedList_WhenUserHasAccess_RetrunsDataTable()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();

            // Act
            var isExist = EmailGroup.EmailExistsInCustomerSeedList(1, 1);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => isExist.ShouldBeTrue(),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("e_EmailGroup_Exists_SeedList"));
        }

        [Test]
        public void FDSubscriberLogin_WhenUserHasAccess_RetrunsDataTable()
        {
            // Arrange    
            SetDataFunctionGetDataTableFakes();

            // Act
            var count = EmailGroup.
                FDSubscriberLogin(
                1, 
                string.Empty,
                1,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty);

            // Assert
            _executedSqlCommand.ShouldSatisfyAllConditions(
                () => count.ShouldBe(1),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain("sp_FDSubscriberLogin"));
        }
            
        private void SetDataFunctionGetDataTableFakes()
        {
            _executedSqlCommand = string.Empty;
            ShimDataFunctions.GetDataTableSqlCommandString = (cmd, conn) =>
            {
                _executedSqlCommand += cmd.CommandText;
                return GetTestDataTable();
            };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                _executedSqlCommand += cmd.CommandText;
                return 1;
            };
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (cmd, conn) =>
            {
                _executedSqlCommand += cmd.CommandText;
                return true;
            };
            ShimDataFunctions.ExecuteSqlCommandString = (cmd, conn) =>
            {
                _executedSqlCommand += cmd.CommandText;
                return 1;
            };
            ShimDataFunctions.GetDataSetSqlCommandString = (cmd, conn) =>
            {
                _executedSqlCommand += cmd.CommandText;
                var dataSet = new DataSet();
                dataSet.Tables.Add(GetTestDataTable());
                return dataSet;
            };
        }

        private DataTable GetTestDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(GroupIDColumn,typeof(int));

            var row = dataTable.NewRow();
            row[GroupIDColumn] = 1;
            dataTable.Rows.Add(row);

            return dataTable;
        }

        private void AssertDataTableAndExecutedSqlCommand(DataTable dataTable, string commandText)
        {
            dataTable.ShouldSatisfyAllConditions(
                () => dataTable.ShouldNotBeNull(),
                () => dataTable.Rows.Count.ShouldBe(1),
                () => dataTable.Rows[0][GroupIDColumn].ShouldBe(1),
                () => _executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => _executedSqlCommand.ShouldContain(commandText));
        }
    }
}
