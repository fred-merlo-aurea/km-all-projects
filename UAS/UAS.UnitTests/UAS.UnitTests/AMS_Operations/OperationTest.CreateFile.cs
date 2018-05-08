using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS_Operations;
using FrameworkSubGen.Entity;
using FrameworkUAD.BusinessLogic.Fakes;
using NUnit.Framework;
using Shouldly;
using System.IO;
using OperationsFakes = AMS_Operations.Fakes;
using UAS.UnitTests.Helpers;
using UADEntity = FrameworkUAD.Entity;
namespace UAS.UnitTests.AMS_Operations
{
    /// <summary>
    /// Unit Test for <see cref="Operations.CreateFile"/>
    /// </summary>
    public partial class OperationTest
    {        
        [Test]
        public void CreateFile_WithValidData_CreatesValidCSVFile()
        {
            // Arrange
            var importSubscriberList = GetTestImportSubscriber();
            importSubscriberList[0].SystemSubscriberID = 9999;
            importSubscriberList[0].MailingAddressCompany = SampleCompany;
            var account = new Account();
            var parametes = new object[]
            {
                account,
                FileName,
                importSubscriberList,
            };
            SetUpFakes();

            // Act
            _privateOperationsObj.Invoke(CreateFileMethodName, parametes);
            var filesPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "FinalFiles");
           
            // Assert
            _isImportSubscriberUpdated.ShouldBeTrue();
            _updatedimportSubscriberList.ShouldNotBeNull();
            _updatedimportSubscriberList.ShouldNotBeEmpty();
            _updatedimportSubscriberList.Count.ShouldBe(1);
            _updatedimportSubscriberList[0].SubscriberAccountFirstName.ShouldBe(SampleFirstName);
            _updatedimportSubscriberList[0].SubscriberAccountLastName.ShouldBe(SampleLastName);
            _updatedimportSubscriberList[0].SubscriberAccountLastName.ShouldBe(SampleLastName);
            _updatedimportSubscriberList[0].SystemSubscriberID.ShouldBe(9999);
            _updatedimportSubscriberList[0].MailingAddressCompany.ShouldBe(SampleCompany);
            _updatedimportSubscriberList[0].SubscriptionID.ShouldBe(0);
            Directory.Exists(filesPath).ShouldBeTrue();
            Directory.GetFiles(filesPath).ShouldNotBeEmpty();
            Directory.GetFiles(filesPath).ShouldContain(x => x.Contains($"{FileName}_Valid.csv"));
        }

        [Test]
        public void CreateFile_WithBadRow_CreatesBadCSVFile()
        {
            // Arrange
            var importSubscriberList = GetTestImportSubscriber();
            var account = new Account();
            var parameters = new object[]
            {
                account,
                FileName,
                importSubscriberList,
            };
            SetUpFakes();
            // Need to Shim it for coverage as actual method-Convert_ImportSubscriber_to_SubscriberTrans- never returns at this conditions
            var subscriberTransformedList = GetSubscriberTransformedList();
            OperationsFakes.ShimImportSubscriberConverter.AllInstances.Convert_ImportSubscriber_to_SubscriberTransIReadOnlyCollectionOfImportSubscriberAccount =
                (o, sub, acc) => subscriberTransformedList;

            // Act
            _privateOperationsObj.Invoke(CreateFileMethodName, parameters);
            var filesPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "FinalFiles");

            // Assert
            _isImportSubscriberUpdated.ShouldBeTrue();
            _updatedimportSubscriberList.ShouldNotBeNull();
            _updatedimportSubscriberList.ShouldNotBeEmpty();
            Directory.Exists(filesPath).ShouldBeTrue();
            Directory.GetFiles(filesPath).ShouldNotBeEmpty();
            Directory.GetFiles(filesPath).ShouldContain(x => x.Contains($"{FileName}_Bad.csv"));
        }

        /// <summary>
        /// For Test Coverage need to Shim it
        /// </summary>
        /// <returns></returns>
        private List<UADEntity.SubscriberTransformed> GetSubscriberTransformedList()
        {
            return new List<UADEntity.SubscriberTransformed>
            {
                new UADEntity.SubscriberTransformed
                {
                    PubCode = SampleKMPubCode,
                    DemographicTransformedList = new HashSet<UADEntity.SubscriberDemographicTransformed>
                    {
                        new UADEntity.SubscriberDemographicTransformed { MAFField = CustomMapperField}
                    }
                },
                new UADEntity.SubscriberTransformed
                {
                    DemographicTransformedList = new HashSet<UADEntity.SubscriberDemographicTransformed>
                    {
                        new UADEntity.SubscriberDemographicTransformed { MAFField = CustomMapperField}
                    }
                },
                 new UADEntity.SubscriberTransformed
                {
                    DemographicTransformedList = new HashSet<UADEntity.SubscriberDemographicTransformed>
                    {
                        new UADEntity.SubscriberDemographicTransformed { MAFField = CustomMapperField}
                    }
                },
            };
        }
    }
}
