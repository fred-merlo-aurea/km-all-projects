using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.Extension;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class UploadLogTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int uploadID = -1;
            int userID = -1;
            int customerID = -1;
            string path = string.Empty;
            string fileName = string.Empty;
            DateTime uploadDate = DateTime.Now;
            string pageSource = string.Empty;        

            // Act
            UploadLog uploadLog = new UploadLog();    

            // Assert
            uploadLog.UploadID.ShouldBe(uploadID);
            uploadLog.UserID.ShouldBe(userID);
            uploadLog.CustomerID.ShouldBe(customerID);
            uploadLog.Path.ShouldBe(path);
            uploadLog.FileName.ShouldBe(fileName);
            uploadLog.UploadDate.ShouldBeCloseTo(uploadDate, 500);
            uploadLog.PageSource.ShouldBe(pageSource);
        }
    }
}