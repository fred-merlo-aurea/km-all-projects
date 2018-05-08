using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class UserGroupTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int uGID = -1;
            int userID = -1;
            int groupID = -1;        

            // Act
            UserGroup userGroup = new UserGroup();    

            // Assert
            userGroup.UGID.ShouldBe(uGID);
            userGroup.UserID.ShouldBe(userID);
            userGroup.GroupID.ShouldBe(groupID);
            userGroup.CreatedUserID.ShouldBeNull();
            userGroup.CreatedDate.ShouldBeNull();
            userGroup.UpdatedUserID.ShouldBeNull();
            userGroup.UpdatedDate.ShouldBeNull();
            userGroup.IsDeleted.ShouldBeNull();
            userGroup.CustomerID.ShouldBeNull();
        }
    }
}