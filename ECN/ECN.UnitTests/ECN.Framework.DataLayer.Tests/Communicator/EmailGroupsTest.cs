using System.Data;
using System.Diagnostics.CodeAnalysis;
using ECN.Framework.DataLayer.Tests.Communicator.Common;
using ECN_Framework_DataLayer.Communicator;
using ECN_Framework_DataLayer.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EmailGroupsTest : Fakes
    {
        private const int GroupId = 10;
        private const int EmailId = 20;
        private const int CustomerId = 30;
        private const int UserId = 50;
        private const string CompositeKey = "Composite Key";
        private const string XmlProfile = "XmlProfile";
        private const string XmlUdf = "XmlUDF";
        private const string FormatTypeCode = "Format Type Code";
        private const string SubscribeTypeCode = "Subscribe Type Code";
        private const string Filename = "Filename";
        private const string Source = "Primary Source";
        private const string SecondarySource = "Secondary Source";
        private const bool OverwriteWithNull = true;
        private const bool EmailAddressOnly = true;
        
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void EmailGroups_Delete_ShouldFillAllParameters_UpdatesTable()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return true;
            };

            // Act
            EmailGroup.Delete(GroupId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureEmailGroupDeleteGroupId),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void EmailGroups_DeleteWithEmailId_ShouldFillAllParameters_UpdatesTable()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return true;
            };

            // Act
            EmailGroup.Delete(GroupId, EmailId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureEmailGroupDeleteGroupIdEmailId),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()),
                () => GetParameterValue(Consts.ParamEmailId).ShouldBe(EmailId.ToString()));
        }

        [Test]
        public void EmailGroups_ImportEmails_ShouldFillAllParameters_ReturnsDataTable()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.GetDataTableSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new DataTable();
            };

            // Act
            EmailGroup.ImportEmails(
                UserId, CustomerId, GroupId, XmlProfile, XmlUdf, FormatTypeCode, SubscribeTypeCode, EmailAddressOnly, Filename, SecondarySource);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureEmailGroupImportEmails),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamXmlProfile).ShouldBe(XmlProfile),
                () => GetParameterValue(Consts.ParamXmlUdf).ShouldBe(XmlUdf),
                () => GetParameterValue(Consts.ParamFormatTypeCode).ShouldBe(FormatTypeCode),
                () => GetParameterValue(Consts.ParamSubscribeTypeCode).ShouldBe(SubscribeTypeCode),
                () => GetParameterValue(Consts.ParamEmailAddressOnly).ShouldBe(EmailAddressOnly.ToString()),
                () => GetParameterValue(Consts.ParamFilename).ShouldBe(Filename),
                () => GetParameterValue(Consts.ParamSecondarySource).ShouldBe(SecondarySource));
        }

        [Test]
        public void EmailGroups_ImportEmailsPreImportResults_ShouldFillAllParameters_ReturnsDataTable()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.GetDataTableSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new DataTable();
            };

            // Act
            EmailGroup.ImportEmails_PreImportResults(CustomerId, GroupId, XmlProfile);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureEmailGroupImportEmailsPreImportResults),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamXmlProfile).ShouldBe(XmlProfile));
        }

        [Test]
        public void EmailGroups_ImportMSEmails_ShouldFillAllParameters_ReturnsDataTable()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.GetDataTableSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new DataTable();
            };

            // Act
            EmailGroup.ImportMSEmails(
                UserId, CustomerId, GroupId, XmlProfile, XmlUdf, FormatTypeCode, SubscribeTypeCode, EmailAddressOnly, Filename, SecondarySource);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureEmailGroupImportMsEmails),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamXmlProfile).ShouldBe(XmlProfile),
                () => GetParameterValue(Consts.ParamXmlUdf).ShouldBe(XmlUdf),
                () => GetParameterValue(Consts.ParamFormatTypeCode).ShouldBe(FormatTypeCode),
                () => GetParameterValue(Consts.ParamSubscribeTypeCode).ShouldBe(SubscribeTypeCode),
                () => GetParameterValue(Consts.ParamEmailAddressOnly).ShouldBe(EmailAddressOnly.ToString()),
                () => GetParameterValue(Consts.ParamFilename).ShouldBe(Filename),
                () => GetParameterValue(Consts.ParamSecondarySource).ShouldBe(SecondarySource));
        }

        [Test]
        public void EmailGroups_ImportEmailsWithDupes_ShouldFillAllParameters_ReturnsDataTable()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.GetDataTableSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new DataTable();
            };

            // Act
            EmailGroup.ImportEmailsWithDupes(
                UserId, CustomerId, GroupId, XmlProfile, XmlUdf, FormatTypeCode, SubscribeTypeCode, EmailAddressOnly, CompositeKey, OverwriteWithNull, Source);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureEmailGroupImportEmailsWithDupes),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamXmlProfile).ShouldBe(XmlProfile),
                () => GetParameterValue(Consts.ParamXmlUdf).ShouldBe(XmlUdf),
                () => GetParameterValue(Consts.ParamFormatTypeCode).ShouldBe(FormatTypeCode),
                () => GetParameterValue(Consts.ParamSubscribeTypeCode).ShouldBe(SubscribeTypeCode),
                () => GetParameterValue(Consts.ParamEmailAddressOnly).ShouldBe(EmailAddressOnly.ToString()),
                () => GetParameterValue(Consts.ParamOverwriteWithNull).ShouldBe(OverwriteWithNull.ToString()),
                () => GetParameterValue(Consts.ParamCompositeKey).ShouldBe(CompositeKey),
                () => GetParameterValue(Consts.ParamSource).ShouldBe(Source));
        }
    }
}
