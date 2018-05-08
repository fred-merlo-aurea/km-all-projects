using System;
using System.Collections.Generic;
using ECN_Framework.Common;
using ECN_Framework.Common.Interfaces;
using ECN_Framework_Common.Objects;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.Tests
{
    [TestFixture]
    public class SecurityAccessTest
    {
        private const string CustomerID = "0";
        private const string BaseChannelID = "0";
        private const string UnknownType = "UnknownType";
        private const string BaseChannelType = "BaseChannel";
        private const string EmailsType = "Emails";

        [Test, TestCaseSource(nameof(AccessTypes))]
        public void HasAccess_EmptyParameters_ReturnsTrueAndCallsExecuteScalar(string type, string sql, string connectionStringName)
        {
            // Arrange
            var dataFunctions = new Mock<IDataFunctions>();
            dataFunctions.Setup(x => x.ExecuteScalar(sql, connectionStringName)).Returns(CustomerID);
            SecurityAccess.Initialize(dataFunctions.Object);

            // Act
            var hasAccess = SecurityAccess.hasAccess(type, string.Empty);

            // Assert
            hasAccess.ShouldBeTrue();
            dataFunctions.VerifyAll();
        }

        [Test]
        public void HasAccess_BaseChannelType_ReturnsFalse()
        {
            // Arrange, Act
            var hasAccess = SecurityAccess.hasAccess(BaseChannelType, string.Empty);

            // Assert
            hasAccess.ShouldBeFalse();
        }

        [Test]
        public void HasAccess_UnknownType_ReturnsFalse()
        {
            // Arrange, Act
            var hasAccess = SecurityAccess.hasAccess(UnknownType, string.Empty);

            // Assert
            hasAccess.ShouldBeFalse();
        }

        [Test]
        public void HasAccess_ExceptionOnExecuteScalar_ThrowsSecurityException()
        {
            // Arrange
            var dataFunctions = new Mock<IDataFunctions>();
            dataFunctions.Setup(x => x.ExecuteScalar(It.IsAny<string>(), It.IsAny<string>())).Throws<SecurityException>();
            SecurityAccess.Initialize(dataFunctions.Object);

            // Act
            var exception = Should.Throw<Exception>(() => SecurityAccess.hasAccess(EmailsType, string.Empty));

            // Assert
            exception.ShouldBeOfType<SecurityException>();
        }

        [Test, TestCaseSource(nameof(AccessTypes))]
        public void HasAccess4_EmptyParameters_ReturnsTrueAndCallsExecuteScalar(string type, string sql, string connectionStringName)
        {
            // Arrange
            var dataFunctions = new Mock<IDataFunctions>();
            dataFunctions.Setup(x => x.ExecuteScalar(sql, connectionStringName)).Returns(CustomerID);
            SecurityAccess.Initialize(dataFunctions.Object);

            // Act
            var hasAccess = SecurityAccess.hasAccess(type, string.Empty, CustomerID, BaseChannelID);

            // Assert
            hasAccess.ShouldBeTrue();
            dataFunctions.VerifyAll();
        }

        [Test]
        public void HasAccess4_BaseChannelType_ReturnsFalse()
        {
            // Arrange, Act
            var hasAccess = SecurityAccess.hasAccess(BaseChannelType, string.Empty, CustomerID, BaseChannelID);

            // Assert
            hasAccess.ShouldBeFalse();
        }

        [Test]
        public void HasAccess4_UnknownType_ReturnsFalse()
        {
            // Arrange, Act
            var hasAccess = SecurityAccess.hasAccess(UnknownType, string.Empty, CustomerID, BaseChannelID);

            // Assert
            hasAccess.ShouldBeFalse();
        }

        [Test]
        public void HasAccess4_ExceptionOnExecuteScalar_ThrowsSecurityException()
        {
            // Arrange
            var dataFunctions = new Mock<IDataFunctions>();
            dataFunctions.Setup(x => x.ExecuteScalar(It.IsAny<string>(), It.IsAny<string>())).Throws<SecurityException>();
            SecurityAccess.Initialize(dataFunctions.Object);

            // Act
            var exception = Should.Throw<Exception>(() => SecurityAccess.hasAccess(EmailsType, string.Empty, string.Empty, string.Empty));

            // Assert
            exception.ShouldBeOfType<SecurityException>();
        }

        private static IReadOnlyCollection<TestCaseData> AccessTypes => new TestCaseData[]
        {
            new TestCaseData("Emails", "SELECT CustomerID from Email where EmailID = ", "communicator"),
            new TestCaseData("EmailDataValues", "SELECT e.CustomerID from Email e, EmailDataValues ed where e.EmailID = ed.EmailID AND ed.EmailDataValuesID = ", "communicator"),
            new TestCaseData("Groups", "SELECT CustomerID from Groups where GroupID = ", "communicator"),
            new TestCaseData("Content", "SELECT CustomerID from Content where ContentID = ", "communicator"),
            new TestCaseData("Blasts", "SELECT CustomerID from Blasts where BlastID = ", "communicator"),
            new TestCaseData("FiltersDetails", "SELECT f.CustomerID from Filters f, FiltersDetails fd where f.FilterID = fd.FilterID AND fd.FDID = ", "communicator"),
            new TestCaseData("Filters", "SELECT CustomerID from Filters where FilterID = ", "communicator"),
            new TestCaseData("ContentFiltersDetails", "SELECT l.CustomerID from ContentFilter f, ContentFilterDetail fd, Layout l where f.FilterID = fd.FilterID AND f.layoutID = l.layoutID AND fd.FDID = ", "communicator"),
            new TestCaseData("ContentFilters", "SELECT l.CustomerID from ContentFilter f, Layout l where l.LayoutID = f.LayoutID AND f.FilterID = ", "communicator"),
            new TestCaseData("Layouts", "SELECT CustomerID from Layout where LayoutID = ", "communicator"),
            new TestCaseData("Folders", "SELECT CustomerID from Folder where FolderID = ", "communicator"),
            new TestCaseData("Survey", "SELECT CustomerID from survey where surveyid = ", "collector"),
            new TestCaseData("Events", "SELECT CustomerID from Events where EventID = ", "creator"),
            new TestCaseData("Menus", "SELECT CustomerID from Menus where MenuID = ", "creator"),
            new TestCaseData("Pages", "SELECT CustomerID from Page where PageID = ", "creator"),
            new TestCaseData("Templates", "SELECT CustomerID from Templates where TemplateID = ", "creator"),
            new TestCaseData("HeaderFooters", "SELECT CustomerID from HeaderFooters where HeaderFooterID = ", "creator"),
            new TestCaseData("colHeaderFooters", "SELECT CustomerID from HeaderFooters where HeaderFooterID = ", "collector"),
            new TestCaseData("Users", "SELECT CustomerID from Users where UserID = ", "accounts"),
            new TestCaseData("Customers", "SELECT BaseChannelID from Customer where CustomerID = ", "accounts"),
            new TestCaseData("CustomerTemplates", "SELECT c.BaseChannelID from Customer c, CustomerTemplate ct where c.CustomerID = ct.CustomerID AND ct.CTID=", "accounts"),
            new TestCaseData("CustomerLicenses", "SELECT c.BaseChannelID from Customer c, CustomerLicense cl where c.CustomerID = cl.CustomerID AND cl.CLID=", "accounts"),
            new TestCaseData("Channels", "SELECT BaseChannelID from Channel where ChannelID=", "accounts"),
            new TestCaseData("Publications", "SELECT CustomerID from Publications where PublicationID = ", "publisher"),
            new TestCaseData("Editions", "SELECT CustomerID from Editions e join Publications m on e.PublicationID = m.PublicationID where e.EditionID = ", "publisher"),
            new TestCaseData("ChannelPartnerTemplates", $"SELECT ChannelID FROM Templates WHERE ChannelID = { CustomerID } AND TemplateID = ", "communicator")
        };
    }
}
