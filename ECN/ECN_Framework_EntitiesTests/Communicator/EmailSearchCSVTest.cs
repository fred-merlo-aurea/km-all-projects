using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture()]
    public class EmailSearchCSVTest
    {
        [Test()]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {

            string baseChannelName = string.Empty;
            string customerName = string.Empty;
            string groupName = string.Empty;
            string emailAddress = string.Empty;
            string subscribe = string.Empty;
            string dateAdded = string.Empty;
            string dateModified = string.Empty;        

            EmailSearchCSV emailSearchCSV = new EmailSearchCSV();    

            emailSearchCSV.BaseChannelName.ShouldBe(baseChannelName);
            emailSearchCSV.CustomerName.ShouldBe(customerName);
            emailSearchCSV.GroupName.ShouldBe(groupName);
            emailSearchCSV.EmailAddress.ShouldBe(emailAddress);
            emailSearchCSV.Subscribe.ShouldBe(subscribe);
            emailSearchCSV.DateAdded.ShouldBe(dateAdded);
            emailSearchCSV.DateModified.ShouldBe(dateModified);
        }
    }
}