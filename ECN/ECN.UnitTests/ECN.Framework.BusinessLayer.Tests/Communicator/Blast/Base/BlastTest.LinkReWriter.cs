using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ECN_Framework_BusinessLayer.Communicator;
using Entities = ECN_Framework_Entities.Communicator;
using EntitiesFakes = ECN_Framework_Entities.Communicator.Fakes;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using KMPlatform.Entity;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.BusinessLogic.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class BlastTest
    {
        private const string DummyString = "DummyString";
        private Entities::BlastRegular _blast;
        private readonly NameValueCollection _appSettings = new NameValueCollection();

        [Test]
        public void LinkReWriter_OnEmptyString_ReturnEmptyString()
        {
            // Arrange
            SetupForLinkReWriter();
            SetShimsForLinkReWriter();
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => false;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => false;

            // Act	
            var actualResult = Blast.LinkReWriter(
                string.Empty,
                _blast,
                1,
                string.Empty,
                string.Empty,
                String.Empty,
                null);

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void LinkReWriter_OnNonEmptyString_ReturnString()
        {
            // Arrange
            SetupForLinkReWriter();
            SetShimsForLinkReWriter();
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => true;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => true;
            const string StrText = "%%groupname%% %%listprofilepreferences%%";
            const string ProductFeature = "/engines/managesubscriptions.aspx?";

            // Act	
            var actualResult = Blast.LinkReWriter(
                StrText,
                _blast,
                1,
                string.Empty,
                string.Empty,
                String.Empty,
                null);

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
            actualResult.ShouldContain(DummyString);
            actualResult.ShouldContain(ProductFeature);
        }

        [Test]
        public void LinkReWriter_OnECNSubscriptionMGMT_ReturnString()
        {
            // Arrange
            SetupForLinkReWriter();
            SetShimsForLinkReWriter();
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => false;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => false;
            const string TempSubMLink = "TempSubMLink";
            const string expectedString = "engines/subscriptionmanagement";
            var strText = $" ECN.SUBSCRIPTIONMGMT.{TempSubMLink}.ECN.SUBSCRIPTIONMGMT";
            ShimSubscriptionManagement.GetByBaseChannelIDInt32 = (id) => new List<SubscriptionManagement>
            {
                new SubscriptionManagement
                {
                    Name = TempSubMLink,
                    SubscriptionManagementID = 1
                }
            };

            // Act	
            var actualResult = Blast.LinkReWriter(
                strText,
                _blast,
                1,
                string.Empty,
                string.Empty,
                String.Empty,
                null);

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
            actualResult.ShouldContain(expectedString);
        }

        [Test]
        public void LinkReWriter_OnForwardFriendException_ReturnString()
        {
            // Arrange
            SetupForLinkReWriter();
            SetShimsForLinkReWriter();
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => false;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => false;
            const string ForwardFriend = "ForwardFriend";
            ShimCustomerTemplate.GetByTypeIDInt32StringUser = (id, type, usr) =>
            {
                if (id == 2)
                {
                    throw new Exception();
                }
                return new CustomerTemplate
                {
                    HeaderSource = "%%email_friend%%"
                };
            };

            // Act	
            var actualResult = Blast.LinkReWriter(
                String.Empty, 
                _blast,
                1,
                string.Empty,
                string.Empty,
                ForwardFriend,
                null);

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
            actualResult.ShouldContain(ForwardFriend);
        }

        [Test]
        public void LinkReWriter_OnForwardFriend_ReturnString()
        {
            // Arrange
            SetupForLinkReWriter();
            SetShimsForLinkReWriter();
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => false;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (id, svc, feature) => false;
            const string ForwardFriend = "ForwardFriend";
            ShimCustomerTemplate.GetByTypeIDInt32StringUser = (id, type, usr) => new CustomerTemplate
            {
                HeaderSource = "%%email_friend%%"
            };

            // Act	
            var actualResult = Blast.LinkReWriter(
                String.Empty,
                _blast,
                1,
                string.Empty,
                string.Empty,
                ForwardFriend,
                null);

            // Assert
            actualResult.ShouldNotBeNullOrWhiteSpace();
            actualResult.ShouldContain(ForwardFriend);
        }


        private void SetupForLinkReWriter()
        {
            _blast = new Entities::BlastRegular
            {
                BlastID = 1,
                GroupID = 1,
                LayoutID = 1,
                CustomerID = 2
            };
            _appSettings.Clear();
            _appSettings.Add("Activity_DomainPath", string.Empty);
            _appSettings.Add("OpenClick_UseOldSite", "false");
            _appSettings.Add("MVCActivity_DomainPath", string.Empty);
            _appSettings.Add("Image_DomainPath", string.Empty);
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
        }

        private void SetShimsForLinkReWriter()
        {
            ShimGroup.GetByGroupIDInt32User = (id, usr) => new Entities::Group
            {
                GroupName = DummyString
            };
            ShimLayout.GetByLayoutIDInt32UserBoolean = (id, usr, child) => new Entities::Layout
            {
                DisplayAddress = DummyString
            };
            ShimCustomer.GetByCustomerIDInt32Boolean = (id, child) => new Customer
            {
                CustomerID = 1,
                BaseChannelID = 1
            };
        }
    }
}
