using System.Collections.Generic;
using FrameworkSubGen.BusinessLogic;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using Entity = FrameworkSubGen.Entity;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    /// <summary>
    ///     Unit Tests for <see cref="ImportSubscriber"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ImportSubscriberTest : Fakes
    {
        private ImportSubscriber _importSubscriber;
        private List<Entity.ImportSubscriber> _subscribers;
        private Entity.ImportDimension _dimension;

        [SetUp]
        public void SetUp()
        {
            SetupFakes(new Mocks());
            _importSubscriber = new ImportSubscriber();
            _dimension = typeof(Entity.ImportDimension).CreateInstance();

            _subscribers = new List<Entity.ImportSubscriber>
            {
                new Entity.ImportSubscriber
                {
                    AuditCategoryCode = "\0AuditCategoryCode\0",
                    AuditCategoryName = " AuditCategoryName ",
                    AuditRequestTypeCode = "&AuditRequestTypeCode",
                    AuditRequestTypeName = "\"AuditRequestTypeName",
                    BillingAddressCity = "<BillingAddressCity",
                    BillingAddressCompany = ">BillingAddressCompany",
                    BillingAddressFirstName = "'BillingAddressFirstName",
                    BillingAddressLastName = "’BillingAddressLastName",
                    BillingAddressLine1 = "\0BillingAddressLine1",
                    BillingAddressState = " BillingAddressState",
                    BillingAddressZip = "&BillingAddressZip",
                    BillingAddressCountry = "\"BillingAddressCountry",
                    MailingAddressCity = "<MailingAddressCity",
                    MailingAddressCompany = ">MailingAddressCompany",
                    MailingAddressFirstName = "'MailingAddressFirstName",
                    MailingAddressLastName = "’MailingAddressLastName",
                    MailingAddressLine1 = ">MailingAddressLine1",
                    MailingAddressState = "<MailingAddressState",
                    MailingAddressTitle = "\"MailingAddressTitle",
                    MailingAddressZip = "&MailingAddressZip",
                    MailingAddressCountry = "&MailingAddressCountry",
                    PublicationName = " PublicationName",
                    RenewalCode_CustomerID = " RenewalCode_CustomerID",
                    SubscriberAccountFirstName = "<SubscriberAccountFirstName",
                    SubscriberAccountLastName = ">SubscriberAccountLastName",
                    SubscriberEmail = "<SubscriberEmail",
                    SubscriberPhone = "\"SubscriberPhone",
                    SubscriberSource = "\0SubscriberSource",
                    Dimensions = _dimension
                }
            };
        }

        [Test]
        public void CleanForXml_ImportSubscribersWithInvalidXml_ReturnFormattedList()
        {
            // Arrange
            var expected = new List<Entity.ImportSubscriber>
            {
                new Entity.ImportSubscriber
                {
                    AuditCategoryCode = "AuditCategoryCode",
                    AuditCategoryName = "AuditCategoryName",
                    AuditRequestTypeCode = "&amp;AuditRequestTypeCode",
                    AuditRequestTypeName = "&quot;AuditRequestTypeName",
                    BillingAddressCity = "&lt;BillingAddressCity",
                    BillingAddressCompany = "&gt;BillingAddressCompany",
                    BillingAddressFirstName = "&apos;BillingAddressFirstName",
                    BillingAddressLastName = "&apos;BillingAddressLastName",
                    BillingAddressLine1 = "BillingAddressLine1",
                    BillingAddressState = "BillingAddressState",
                    BillingAddressZip = "&amp;BillingAddressZip",
                    BillingAddressCountry = "&quot;BillingAddressCountry",
                    MailingAddressCity = "&lt;MailingAddressCity",
                    MailingAddressCompany = "&gt;MailingAddressCompany",
                    MailingAddressFirstName = "&apos;MailingAddressFirstName",
                    MailingAddressLastName = "&apos;MailingAddressLastName",
                    MailingAddressLine1 = "&gt;MailingAddressLine1",
                    MailingAddressState = "&lt;MailingAddressState",
                    MailingAddressTitle = "&quot;MailingAddressTitle",
                    MailingAddressZip = "&amp;MailingAddressZip",
                    MailingAddressCountry = "&amp;MailingAddressCountry",
                    PublicationName = "PublicationName",
                    RenewalCode_CustomerID = "RenewalCode_CustomerID",
                    SubscriberAccountFirstName = "&lt;SubscriberAccountFirstName",
                    SubscriberAccountLastName = "&gt;SubscriberAccountLastName",
                    SubscriberEmail = "&lt;SubscriberEmail",
                    SubscriberPhone = "&quot;SubscriberPhone",
                    SubscriberSource = "SubscriberSource",
                    Dimensions = _dimension
                }
            };

            // Act
            List<Entity.ImportSubscriber> result = _importSubscriber.CleanForXml(_subscribers);

            // Assert
            expected.First().IsContentMatched(result.First()).ShouldBeTrue();
        }
    }
}