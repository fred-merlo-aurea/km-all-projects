using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_DataLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using Entities = ECN_Framework_Entities.Communicator;
using BussinessFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class EmailGroupsTest
    {
        private const string SecurityErrorMessage = "SECURITY VIOLATION!";
        private const string GroupIDColumn = "GroupID";
        private IDisposable _shimsObject;

        [SetUp]
        public void SetUp()
        {
            _shimsObject = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp()
        {
            _shimsObject.Dispose();
        }

        [Test]
        public void Validate_WithInValidProperties_ThrowsECNException()
        {
            // Arrange
            var emailGroup = new Entities.EmailGroup
            {
                CustomerID = -1,
                GroupID = -1,
                FormatTypeCode = null,
                SubscribeTypeCode = null
            };

            // Act
            var ecnExp = Should.Throw<ECNException>(() => EmailGroup.Validate(emailGroup));

            // Assert
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ShouldNotBeNull(),
                () => ecnExp.ErrorList.Count.ShouldBe(4),
                () => ecnExp.ErrorList.ShouldAllBe(x => x.Method == Enums.Method.Validate),
                () => ecnExp.ErrorList.ShouldAllBe(x => x.Entity == Enums.Entity.EmailGroup),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("CustomerID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("GroupID is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("FormatTypeCode is invalid")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("SubscribeTypeCode is invalid")));
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void Exists_WithEmailIDArgument_ReturnsIfEmailGroupExists(int returnedEmailID)
        {
            // Arrange
            const int EmailID = 1;
            const int GroupID = 1;
            const int CustomerID = 1;
            var executedSqlCommand = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand = cmd.CommandText;
                return returnedEmailID;
            };

            // Act
            var exists = EmailGroup.Exists(emailID: EmailID, groupID: GroupID, customerID: CustomerID);

            // Assert
            exists.ShouldSatisfyAllConditions(
                () => 
                {
                    if (returnedEmailID == EmailID)
                    {
                        exists.ShouldBeTrue();
                    }
                    else
                    {
                        exists.ShouldBeFalse();
                    }
                },
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Exists_EmailID_GroupID"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void Exists_WithValidEmailAddressArgument_ReturnsIfEmailGroupExists(int returnedEmailID)
        {
            // Arrange
            const string EmailAddress = "test@test.com";
            const int ValidEmailID = 1;
            const int GroupID = 1;
            const int CustomerID = 1;
            var executedSqlCommand = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand = cmd.CommandText;
                return returnedEmailID;
            };

            // Act
            var exists = EmailGroup.Exists(emailAddress: EmailAddress, groupID: GroupID, customerID: CustomerID);

            // Assert
            exists.ShouldSatisfyAllConditions(
                () =>
                {
                    if (returnedEmailID == ValidEmailID)
                    {
                        exists.ShouldBeTrue();
                    }
                    else
                    {
                        exists.ShouldBeFalse();
                    }
                },
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Exists_EmailAddress_GroupID"));
        }
        
        [Test]
        public void GetByEmailIDGroupID_WhenUserHasAccess_ReturnsEmailGroup()
        {
            // Arrange
            const int EmailID = 1;
            const int GroupID = 1;
            var user = new User();
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => true);

            // Act
            var resultEmailGroup = EmailGroup.GetByEmailIDGroupID(emailID: EmailID, groupID: GroupID, user: user);

            // Assert
            resultEmailGroup.ShouldSatisfyAllConditions(
                () => resultEmailGroup.ShouldNotBeNull(),
                () => resultEmailGroup.EmailID.ShouldBe(emailGroup.EmailID),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Select"));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void GetByEmailIDGroupID_WhenUserDoesNotHaveAccess_ThrowsException(bool hasAccess)
        {
            // Arrange
            const int EmailID = 1;
            const int GroupID = 1;
            var user = new User();
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => hasAccess;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => false);

            // Act
           var exp =  Should.Throw<SecurityException>(() => 
                            EmailGroup.GetByEmailIDGroupID(emailID: EmailID, groupID: GroupID, user: user));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull());
        }

        [Test]
        public void GetByEmailIDGroupID_NoAccessCheck_WhenValidEmailIDAndGroupID_ReturnsEmailGroup()
        {
            // Arrange
            const int EmailID = 1;
            const int GroupID = 1;
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroup;
            };
            
            // Act
            var resultEmailGroup = EmailGroup.GetByEmailIDGroupID_NoAccessCheck(emailID: EmailID, groupID: GroupID);

            // Assert
            resultEmailGroup.ShouldSatisfyAllConditions(
                () => resultEmailGroup.ShouldNotBeNull(),
                () => resultEmailGroup.EmailID.ShouldBe(emailGroup.EmailID),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Select"));
        }

        [Test]
        public void GetByEmailAddressGroupID_WhenUserHasAccess_ReturnsEmailGroup()
        {
            // Arrange
            const string EmailAddress = "test@test.com";
            const int EmailID = 1;
            const int GroupID = 1;
            var user = new User();
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID, };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => true);

            // Act
            var resultEmailGroup = EmailGroup.GetByEmailAddressGroupID(emailAddress: EmailAddress, groupID: GroupID, user: user);

            // Assert
            resultEmailGroup.ShouldSatisfyAllConditions(
                () => resultEmailGroup.ShouldNotBeNull(),
                () => resultEmailGroup.EmailID.ShouldBe(emailGroup.EmailID),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Select_EmailAddress_GroupID"));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void GetByEmailAddressGroupID_WhenUserDoesNotHaveAccess_ThrowsException(bool hasAccess)
        {
            // Arrange
            const string EmailAddress = "test@test.com";
            const int EmailID = 1;
            const int GroupID = 1;
            var user = new User();
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => hasAccess;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => false);

            // Act
            var exp = Should.Throw<SecurityException>(() =>
                            EmailGroup.GetByEmailAddressGroupID(emailAddress: EmailAddress, groupID: GroupID, user: user));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull());
        }

        [Test]
        public void GetByEmailAddressGroupID_NoAccessCheck_WhenValidEmailAddressAndGroupID_ReturnsEmailGroup()
        {
            // Arrange
            const string EmailAddress = "test@test.com";
            const int EmailID = 1;
            const int GroupID = 1;
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroup;
            };

            // Act
            var resultEmailGroup = EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(emailAddress: EmailAddress, groupID: GroupID);

            // Assert
            resultEmailGroup.ShouldSatisfyAllConditions(
                () => resultEmailGroup.ShouldNotBeNull(),
                () => resultEmailGroup.EmailID.ShouldBe(emailGroup.EmailID),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Select_EmailAddress_GroupID"));
        }


        [Test]
        public void GetByEmailID_WhenUserHasAccess_ReturnsEmailGroup()
        {
            // Arrange
            const int EmailID = 1;
            var user = new User();
            var emailGroupList = new List<Entities.EmailGroup>
            {
                new Entities.EmailGroup { EmailID = EmailID, }
            };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetListSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroupList;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<List<Entities.EmailGroup>>((em, u) => true);

            // Act
            var resultEmailGroup = EmailGroup.GetByEmailID(emailID: EmailID, user: user);

            // Assert
            resultEmailGroup.ShouldSatisfyAllConditions(
                () => resultEmailGroup.ShouldNotBeNull(),
                () => resultEmailGroup.Count.ShouldBe(1),
                () => resultEmailGroup[0].EmailID.ShouldBe(emailGroupList[0].EmailID),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Select_EmailID"));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void GetByEmailID_WhenUserDoesNotHaveAccess_ThrowsException(bool hasAccess)
        {
            // Arrange
            const int EmailID = 1;
            var user = new User();
            var emailGroup = new List<Entities.EmailGroup>
            {
                new Entities.EmailGroup { EmailID = EmailID }
            };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetListSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => hasAccess;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<List<Entities.EmailGroup>>((em, u) => false);

            // Act
            var exp = Should.Throw<SecurityException>(() =>
                            EmailGroup.GetByEmailID(emailID: EmailID, user: user));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull());
        }

        [Test]
        public void GetByGroupID_WhenUserHasAccess_ReturnsEmailGroup()
        {
            // Arrange
            const int GroupID = 1;
            var user = new User();
            var emailGroupList = new List<Entities.EmailGroup>
            {
                new Entities.EmailGroup { GroupID = GroupID, }
            };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetListSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroupList;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<List<Entities.EmailGroup>>((em, u) => true);

            // Act
            var resultEmailGroup = EmailGroup.GetByGroupID(groupID: GroupID, user: user);

            // Assert
            resultEmailGroup.ShouldSatisfyAllConditions(
                () => resultEmailGroup.ShouldNotBeNull(),
                () => resultEmailGroup.Count.ShouldBe(1),
                () => resultEmailGroup[0].GroupID.ShouldBe(emailGroupList[0].GroupID),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Select_GroupID"));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void GetByGroupID_WhenUserDoesNotHaveAccess_ThrowsException(bool hasAccess)
        {
            // Arrange
            const int GroupID = 1;
            var user = new User();
            var emailGroup = new List<Entities.EmailGroup>
            {
                new Entities.EmailGroup { GroupID = GroupID }
            };
            var executedSqlCommand = string.Empty;
            ShimEmailGroup.GetListSqlCommand = (cmd) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => hasAccess;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<List<Entities.EmailGroup>>((em, u) => false);

            // Act
            var exp = Should.Throw<SecurityException>(() =>
                            EmailGroup.GetByGroupID(groupID: GroupID, user: user));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull());
        }

        [Test]
        public void ValidateEmails_WithGroupIdAndUserId_ReturnsEmailGroup()
        {
            // Arrange
            const int GroupID = 1;
            const int UserID = 1;
            var executedSqlCommand = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd,conn) =>
            {
                executedSqlCommand = cmd.CommandText;
                return GroupID;
            };
            
            // Act
            var isValid = EmailGroup.ValidateEmails(groupID: GroupID, userID: UserID);

            // Assert
            isValid.ShouldSatisfyAllConditions(
                () => isValid.ShouldBe(GroupID),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_MarkBadEmails"));
        }

        [Test]
        public void DeleteBadEmails_WithGroupIdAndUserId_ReturnsEmailGroup()
        {
            // Arrange
            const int GroupID = 1;
            const int UserID = 1;
            var user = new User();
            var executedSqlCommand = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand = cmd.CommandText;
                return GroupID;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;

            // Act
            var deletedID = EmailGroup.DeleteBadEmails(groupID: GroupID, userID: UserID, user: user);

            // Assert
            deletedID.ShouldSatisfyAllConditions(
                () => deletedID.ShouldBe(GroupID),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_DeleteBadEmails"));
        }

        [Test]
        public void DeleteBadEmails_WhenUserDoesNotHaveAccess_ThrowsSecurityException()
        {
            // Arrange
            const int GroupID = 1;
            const int UserID = 1;
            var user = new User();
            var executedSqlCommand = string.Empty;
           
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => false;

            // Act
           var exp = Should.Throw<SecurityException>(() => EmailGroup.DeleteBadEmails(groupID: GroupID, userID: UserID, user: user));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.Message.ShouldContain(SecurityErrorMessage),
                () => executedSqlCommand.ShouldBeNullOrWhiteSpace());
        }

        [Test]
        public void Delete_WhenEmailIDNotExists_ThrowsECNException()
        {
            // Arrange
            const int GroupID = 1;
            const int EmailID = 1;
            var user = new User();
            var executedSqlCommand = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand = cmd.CommandText;
                return 0;
            };

            // Act
            var exp = Should.Throw<ECNException>(() => EmailGroup.Delete(GroupID, EmailID, user));

            // Assert
            executedSqlCommand.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.ErrorList.Count.ShouldBe(1),
                () => exp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Address does not exist in the Group")),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Exists_EmailID_GroupID"));
        }

        [Test]
        public void Delete_WhenEmailHasActivityForGroup_ThrowsECNException()
        {
            // Arrange
            const int GroupID = 1;
            const int EmailID = 1;
            var user = new User();
            var executedSqlCommand = string.Empty;
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand += cmd.CommandText;
                return EmailID ;
            };
            
            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand += cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => true);
            BussinessFakes.ShimBlast.GetSentByGroupIDInt32 = _ =>  "1";
            ShimBlastActivitySends.ActivityByBlastIDsEmailIDStringInt32 = (_, __) => true;

            // Act
            var exp = Should.Throw<ECNException>(() => EmailGroup.Delete(GroupID, EmailID, user));

            // Assert
            executedSqlCommand.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.ErrorList.Count.ShouldBe(1),
                () => exp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email has activity for this Group")),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldContain("e_EmailGroup_Exists_EmailID_GroupID"),
                () => executedSqlCommand.ShouldContain("e_EmailGroup_Select"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void Delete_WhenGroupHasMasterSuppressionValue_DeletesFromMasterSuppressionGroup(int masterSuppressionValue)
        {
            // Arrange
            const int GroupID = 1;
            const int EmailID = 1;
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            var executedSqlCommand = string.Empty;
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand += cmd.CommandText;
                return EmailID;
            };
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand += cmd.CommandText;
                return true;
            };
            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand += cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => true);
            BussinessFakes.ShimBlast.GetSentByGroupIDInt32 = _ => "1";
            ShimBlastActivitySends.ActivityByBlastIDsEmailIDStringInt32 = (_, __) => false;
            BussinessFakes.ShimGroup.GetByGroupIDInt32User = (_, __) => new Entities.Group
            {
                MasterSupression = masterSuppressionValue
            };

            // Act
            EmailGroup.Delete(GroupID, EmailID, user);

            // Assert
            executedSqlCommand.ShouldSatisfyAllConditions(
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldContain("e_EmailGroup_Exists_EmailID_GroupID"),
                () => executedSqlCommand.ShouldContain("e_EmailGroup_Select"),
                () => 
                {
                    if (masterSuppressionValue == 1)
                    {
                        executedSqlCommand.ShouldContain("e_EmailGroup_DeleteFromMasterSuppression");
                    }
                    else
                    {
                        executedSqlCommand.ShouldContain("e_EmailGroup_Delete_GroupID_EmailID");
                    }
                });
        }

        [Test]
        public void Delete_NoAccessCheck_WhenEmailIDNotExists_ThrowsECNException()
        {
            // Arrange
            const int GroupID = 1;
            const int EmailID = 1;
            const int CustomerID = 1;
            var user = new User();
            var executedSqlCommand = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand = cmd.CommandText;
                return 0;
            };

            // Act
            var exp = Should.Throw<ECNException>(() => EmailGroup.Delete_NoAccessCheck(GroupID, EmailID, CustomerID, user));

            // Assert
            executedSqlCommand.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.ErrorList.Count.ShouldBe(1),
                () => exp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Address does not exist in the Group")),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Exists_EmailID_GroupID"));
        }
        
        [Test]
        public void Delete_NoAccessCheck_WhenEmailHasActivityForGroup_ThrowsECNException()
        {
            // Arrange
            const int GroupID = 1;
            const int EmailID = 1;
            var user = new User();
            var executedSqlCommand = string.Empty;
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID, CustomerID = 1 };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand += cmd.CommandText;
                return EmailID;
            };

            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand += cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => true);
            BussinessFakes.ShimBlast.GetSentByGroupIDInt32 = _ => "1";
            ShimBlastActivitySends.ActivityByBlastIDsEmailIDStringInt32 = (_, __) => true;

            // Act
            var exp = Should.Throw<ECNException>(() => 
                EmailGroup.Delete_NoAccessCheck(GroupID, EmailID, emailGroup.CustomerID.Value, user));

            // Assert
            executedSqlCommand.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.ErrorList.Count.ShouldBe(1),
                () => exp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email has activity for this Group")),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldContain("e_EmailGroup_Exists_EmailID_GroupID"),
                () => executedSqlCommand.ShouldContain("e_EmailGroup_Select"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void Delete_NoAccessCheck_WhenGroupHasMasterSuppressionValue_DeletesFromMasterSuppressionGroup(int masterSuppressionValue)
        {
            // Arrange
            const int GroupID = 1;
            const int EmailID = 1;
            var user = new User { IsActive = true, IsPlatformAdministrator = true };
            var executedSqlCommand = string.Empty;
            var emailGroup = new Entities.EmailGroup { EmailID = EmailID, CustomerID = 1 };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand += cmd.CommandText;
                return EmailID;
            };
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand += cmd.CommandText;
                return true;
            };
            ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                executedSqlCommand += cmd.CommandText;
                return emailGroup;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => true);
            BussinessFakes.ShimBlast.GetSentByGroupIDInt32 = _ => "1";
            ShimBlastActivitySends.ActivityByBlastIDsEmailIDStringInt32 = (_, __) => false;
            BussinessFakes.ShimGroup.GetByGroupID_NoAccessCheckInt32 = (_) => new Entities.Group
            {
                MasterSupression = masterSuppressionValue
            };

            // Act
            EmailGroup.Delete_NoAccessCheck(GroupID, EmailID, emailGroup.CustomerID.Value, user);

            // Assert
            executedSqlCommand.ShouldSatisfyAllConditions(
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldContain("e_EmailGroup_Exists_EmailID_GroupID"),
                () => executedSqlCommand.ShouldContain("e_EmailGroup_Select"),
                () =>
                {
                    if (masterSuppressionValue == 1)
                    {
                        executedSqlCommand.ShouldContain("e_EmailGroup_DeleteFromMasterSuppression");
                    }
                    else
                    {
                        executedSqlCommand.ShouldContain("e_EmailGroup_Delete_GroupID_EmailID");
                    }
                });
        }

        [Test]
        public void Delete_WhenUserHasNoAccess_ThrowsECNException()
        {
            // Arrange
            const int GroupID = 1;
            const int EmailID = 1;
            var user = new User();
            var executedSqlCommand = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand = cmd.CommandText;
                return 0;
            };

            // Act
            var exp = Should.Throw<ECNException>(() => EmailGroup.Delete(GroupID, EmailID, user));

            // Assert
            executedSqlCommand.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.ErrorList.Count.ShouldBe(1),
                () => exp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Address does not exist in the Group")),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_Exists_EmailID_GroupID"));
        }

        [Test]
        public void Delete_WithGroupIDUserArgumentsAndHasNoAccess_ThrowsECNException()
        {
            // Arrange
            const int GroupID = 1;
            var user = new User();
            
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => false;
            BussinessFakes.ShimGroup.GetByGroupIDInt32User = (_,__) => new Entities.Group
            {
                MasterSupression = 1
            };

            // Act
            var exp = Should.Throw<SecurityException>(() => EmailGroup.Delete(GroupID, user));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.Message.ShouldContain(SecurityErrorMessage));
        }

        [Test]
        public void Delete_WithGroupIDUserArgumentsAndHasAccess_DeletesEmailGroupID()
        {
            // Arrange
            const int GroupID = 1;
            var user = new User();
            var executedSqlCommand = string.Empty;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand += cmd.CommandText;
                return true;
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimGroup.GetByGroupIDInt32User = (_, __) => new Entities.Group
            {
                MasterSupression = 1
            };

            // Act
            EmailGroup.Delete(GroupID, user);

            // Assert
            executedSqlCommand.ShouldSatisfyAllConditions(
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldContain("e_EmailGroup_Delete_GroupID"));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void Save_WithNoAccess_ThrowsSecurityException(bool hasAccess)
        {
            // Arrange
            var emailGroup = new Entities.EmailGroup
            {
                CustomerID = 1,
                GroupID = 1,
                FormatTypeCode = string.Empty,
                SubscribeTypeCode = string.Empty
            };
            var user = new User();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => hasAccess;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => false);

            // Act
            var exp = Should.Throw<SecurityException>(() =>
                            EmailGroup.Save(emailGroup, true, string.Empty, string.Empty, user));

            // Assert
            exp.ShouldSatisfyAllConditions(
                () => exp.ShouldNotBeNull(),
                () => exp.Message.ShouldContain(SecurityErrorMessage));
        }

        [Test]
        public void Save_WithAccess_SavesEmailGroupImport()
        {
            // Arrange
            var emailGroup = new Entities.EmailGroup
            {
                CustomerID = 1,
                GroupID = 1,
                EmailGroupID = 1,
                FormatTypeCode = string.Empty,
                SubscribeTypeCode = string.Empty
            };
            var user = new User();
            var executedSqlCommand = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                executedSqlCommand = cmd.CommandText;
                return emailGroup.GroupID;
            };

            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, a, g, h) => true;
            BussinessFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<Entities.EmailGroup>((em, u) => true);

            // Act
            var savedID = EmailGroup.Save(emailGroup, true, string.Empty, string.Empty, user);

            // Assert
            savedID.ShouldSatisfyAllConditions(
                () => savedID.ShouldBe(emailGroup.EmailGroupID),
                () => executedSqlCommand.ShouldNotBeNullOrWhiteSpace(),
                () => executedSqlCommand.ShouldBe("e_EmailGroup_ImportEmails"));
        }
    }
}
