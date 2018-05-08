using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class SmartFormActivityLogTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int sALID = -1;
            int sFID = -1;
            int customerID = -1;
            int groupID = -1;
            int emailID = -1;
            string emailType = string.Empty;
            string emailTo = string.Empty;
            string emailFrom = string.Empty;
            string emailSubject = string.Empty;
            DateTime sendTime = Convert.ToDateTime("1/1/1900");        

            // Act
            SmartFormActivityLog smartFormActivityLog = new SmartFormActivityLog();    

            // Assert
            smartFormActivityLog.SALID.ShouldBe(sALID);
            smartFormActivityLog.SFID.ShouldBe(sFID);
            smartFormActivityLog.CustomerID.ShouldBe(customerID);
            smartFormActivityLog.GroupID.ShouldBe(groupID);
            smartFormActivityLog.EmailID.ShouldBe(emailID);
            smartFormActivityLog.EmailType.ShouldBe(emailType);
            smartFormActivityLog.EmailTo.ShouldBe(emailTo);
            smartFormActivityLog.EmailFrom.ShouldBe(emailFrom);
            smartFormActivityLog.EmailSubject.ShouldBe(emailSubject);
            smartFormActivityLog.SendTime.ShouldBe(sendTime);
            smartFormActivityLog.CreatedUserID.ShouldBeNull();
            smartFormActivityLog.CreatedDate.ShouldBeNull();
            smartFormActivityLog.UpdatedUserID.ShouldBeNull();
            smartFormActivityLog.UpdatedDate.ShouldBeNull();
            smartFormActivityLog.IsDeleted.ShouldBeNull();
        }
    }
}