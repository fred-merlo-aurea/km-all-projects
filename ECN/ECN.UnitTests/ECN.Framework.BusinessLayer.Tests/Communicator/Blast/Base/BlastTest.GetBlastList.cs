using System.Collections.Generic;
using System.Data;
using System.Linq;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Communicator;
using NUnit.Framework;
using Shouldly;
using BlastAB = ECN_Framework_Entities.Communicator.BlastAB;
using BlastAbstract = ECN_Framework_Entities.Communicator.BlastAbstract;
using KmUser = KMPlatform.Entity.User;
using ShimBusinessBlast = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimBlast;
using ShimDataBlast = ECN_Framework_DataLayer.Communicator.Fakes.ShimBlast;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class BlastTest
    {
        private const string BlastIdKey = "BlastID";
        private const string GetBlastListMethod = "GetBlastList";
        private bool _getChildren;
        private KmUser _user;

        [Test]
        [TestCase(1, null, null)]
        [TestCase(null, 1, null)]
        [TestCase(null, null, 1)]
        public void GetBlastList_ShouldSetDataTableBlast_ReturnsBlastList(int? campaignItemId, int? customerId, int? sampleId)
        {
            // Arrange
            CreateShimsForGetBlastList();

            // Act
            var result = CallGetBlastListMethod(new object[] 
            {
                campaignItemId,
                customerId,
                sampleId,
                _user,
                _getChildren
            }) as IList<BlastAbstract>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result?.Count.ShouldBe(1));
        }

        [Test]
        [TestCase(1, null, null)]
        [TestCase(null, 1, null)]
        [TestCase(null, null, 1)]
        public void GetBlastList_WhenUserIsNotNull_ReturnsBlastList(int? campaignItemId, int? customerId, int? sampleId)
        {
            // Arrange
            CreateShimsForGetBlastList();
            _user = new KmUser();

            // Act
            var result = CallGetBlastListMethod(new object[] 
            {
                campaignItemId,
                customerId,
                sampleId,
                _user,
                _getChildren
            }) as IList<BlastAbstract>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result?.Count.ShouldBe(1));
        }

        [Test]
        [TestCase(1, null, null)]
        [TestCase(null, 1, null)]
        [TestCase(null, null, 1)]
        public void GetBlastList_WhenDataTableBlastIsNull_ReturnsBlastList(int? campaignItemId, int? customerId, int? sampleId)
        {
            // Arrange
            CreateShimsForGetBlastList();

            ShimDataBlast.GetByCampaignItemIDInt32 = _ => null;
            ShimDataBlast.GetByCustomerIDInt32 = _ => null;
            ShimDataBlast.GetBySampleIDInt32 = _ => null;

            // Act
            var result = CallGetBlastListMethod(new object[] 
            {
                campaignItemId,
                customerId,
                sampleId,
                _user,
                _getChildren
            }) as IList<BlastAbstract>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result?.Count.ShouldBe(0));
        }

        private static object CallGetBlastListMethod(object[] parametersValues)
        {
            var methodInfo = typeof(Blast).GetAllMethods()
                .FirstOrDefault(x => x.Name == GetBlastListMethod && x.IsPrivate);

            return methodInfo?.Invoke(null, parametersValues);
        }

        private static void CreateShimsForGetBlastList()
        {
            ShimDataBlast.GetByCampaignItemIDInt32 = _ => CreateDataTableForTests();
            ShimDataBlast.GetByCustomerIDInt32 = _ => CreateDataTableForTests();
            ShimDataBlast.GetBySampleIDInt32 = _ => CreateDataTableForTests();

            ShimBusinessBlast.GetByBlastIDInt32UserBoolean = (id, user, getChildren) => new BlastAB();
            ShimBusinessBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, getChildren) => new BlastAB();
        }

        private static DataTable CreateDataTableForTests()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(BlastIdKey, typeof(int));
            
            var dataRow1 = dataTable.NewRow();
            dataRow1[BlastIdKey] = 1;
            dataTable.Rows.Add(dataRow1);
            
            return dataTable;
        }
    }
}
