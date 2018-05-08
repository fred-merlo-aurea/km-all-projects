using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Should;

namespace EmailMarketing.API.Specs.EmailGroup
{
    [TestClass]
    public class ManageSubscriberTestDataProviderTests
    {
        SubscriberTestDataProvider DataProvider = SubscriberTestDataProvider.Factory();

        public ManageSubscriberTestDataProviderTests()
        {
            DataProvider.ApiAccessKey = "8CAB09B9-BEC9-453F-A689-E85D5C9E4898";
        }

        [TestMethod]
        public void GetUniqueEmailAddress_SameTestNameReturnsSameValue()
        {
            string testName = "GetUniqueEmailAddress_SameTestNameReturnsSameValue";
            string v1 = DataProvider.GetUniqueEmailAddressInTestGroup(testName);
            string v2 = DataProvider.GetUniqueEmailAddressInTestGroup(testName);

            v1.ShouldEqual(v2);

        }

        [TestMethod]
        public void GetUniqueEmailAddress_DifferentTestnameReturnsDifferentValue()
        {
            string v1 = DataProvider.GetUniqueEmailAddressInTestGroup("1_GetUniqueEmailAddress_DifferentTestnameReturnsDifferentValue");
            string v2 = DataProvider.GetUniqueEmailAddressInTestGroup("2_GetUniqueEmailAddress_DifferentTestnameReturnsDifferentValue");
                        
            v1.ShouldNotEqual(v2);
        }

        [TestMethod]
        public void GetUniqueEmailAddress_NoOverlapFromInVsNotInTestGroup()
        {
            string v1 = DataProvider.GetUniqueEmailAddressInTestGroup("GetUniqueEmailAddress_InGroup");
            string v2 = DataProvider.GetUniqueEmailAddressNotInTestGroup("GetUniqueEmailAddress_NotInGroup");
            
            v1.ShouldNotEqual(v2);
        }

        [TestMethod]
        public void CustomerID_GreaterThanZero()
        {
            //Assert.AreEqual(1, DataProvider.CustomerID);
            DataProvider.CustomerID.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void GroupID_GreaterThanZero()
        {
            //Assert.AreEqual(DataProvider.GroupID, 147998);
            DataProvider.GroupID.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void GlobalMasterSupression_ContainsAtSign()
        {
            DataProvider.GetEmailAddressInGlobalMasterSupressionList("GlobalMasterSupression")
                .ShouldContain('@');
        }

        [TestMethod]
        public void ChannelMasterSupression_ContainsAtSign()
        {
            DataProvider.GetEmailAddressInChannelMasterSupressionList("ChannelMasterSupression")
                .ShouldContain('@');
        }

        [TestMethod]
        public void MasterSupressionGroup_ContainsAtSign()
        {
            DataProvider.GetEmailAddressInMasterSupressionGroup("MasterSupressionGroup")
                .ShouldContain('@');
        }
    }
}
