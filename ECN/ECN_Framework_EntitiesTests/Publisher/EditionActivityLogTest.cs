using System;
using NUnit.Framework;
using ECN_Framework_Entities.Publisher;
using Shouldly;

namespace ECN_Framework_Entities.Publisher.Tests
{
    [TestFixture]
    public class EditionActivityLogTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int eAID = -1;
            string actionValue = string.Empty;
            string iPAddress = string.Empty;
            string sessionID = string.Empty;        

            // Act
            EditionActivityLog editionActivityLog = new EditionActivityLog();    

            // Assert
            editionActivityLog.EAID.ShouldBe(eAID);
            editionActivityLog.EditionID.ShouldBeNull();
            editionActivityLog.EmailID.ShouldBeNull();
            editionActivityLog.BlastID.ShouldBeNull();
            editionActivityLog.ActionDate.ShouldBeNull();
            editionActivityLog.ActionTypeCode.ShouldBeNull();
            editionActivityLog.ActionValue.ShouldBe(actionValue);
            editionActivityLog.IPAddress.ShouldBe(iPAddress);
            editionActivityLog.IsAnonymous.ShouldBeNull();
            editionActivityLog.LinkID.ShouldBeNull();
            editionActivityLog.PageID.ShouldBeNull();
            editionActivityLog.PageNo.ShouldBeNull();
            editionActivityLog.SessionID.ShouldBe(sessionID);
            editionActivityLog.PageEnd.ShouldBeNull();
            editionActivityLog.PageStart.ShouldBeNull();
            editionActivityLog.CustomerID.ShouldBeNull();
            editionActivityLog.CreatedUserID.ShouldBeNull();
            editionActivityLog.CreatedDate.ShouldBeNull();
            editionActivityLog.UpdatedUserID.ShouldBeNull();
            editionActivityLog.UpdatedDate.ShouldBeNull();
            editionActivityLog.IsDeleted.ShouldBeNull();
        }
    }
}