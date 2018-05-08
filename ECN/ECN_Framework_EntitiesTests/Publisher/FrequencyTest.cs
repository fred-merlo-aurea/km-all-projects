using System;
using NUnit.Framework;
using ECN_Framework_Entities.Publisher;
using Shouldly;

namespace ECN_Framework_Entities.Publisher.Tests
{
    [TestFixture]
    public class FrequencyTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int frequencyID = -1;
            string frequencyName = string.Empty;
            bool? isDeleted = false;        

            // Act
            Frequency frequency = new Frequency();    

            // Assert
            frequency.FrequencyID.ShouldBe(frequencyID);
            frequency.FrequencyName.ShouldBe(frequencyName);
            frequency.IsDeleted.ShouldBe(isDeleted);
        }
    }
}