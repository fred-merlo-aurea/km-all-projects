using System.Collections.Generic;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.DataAccess;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.FrameworkUAS.DataAccess.Common;
using EntityAdhocDimensionGroupPubcodeMap = FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap;

namespace UAS.UnitTests.FrameworkUAS.DataAccess
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdHocDimensionGroupPubcodeMapTest : Fakes
    {
        private const string TableAdhocDimensionGroupPubcodeMap = "AdHocDimensionGroupPubcodeMap";
        private const string SamplePubCode = "code111";

        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void SaveBulkSqlInsert_ListIsNotNull_WritesDataTableToServer()
        {
            // Arrange
            var list = new List<EntityAdhocDimensionGroupPubcodeMap>
            {
                new EntityAdhocDimensionGroupPubcodeMap
                {
                    Pubcode = SamplePubCode
                }
            };
            string destinationTable = null;
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (copy, _) =>
            {
                destinationTable = copy.DestinationTableName;
                copy.Close();
            };

            // Act
            var result = AdHocDimensionGroupPubcodeMap.SaveBulkSqlInsert(list);

            // Assert
            result.ShouldBeTrue();
            destinationTable.ShouldContain(TableAdhocDimensionGroupPubcodeMap);
        }
    }
}
