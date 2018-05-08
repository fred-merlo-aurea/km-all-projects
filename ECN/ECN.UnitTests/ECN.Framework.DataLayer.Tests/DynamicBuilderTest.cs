using System.Data;
using ECN_Framework_DataLayer;
using Microsoft.SqlServer.Server;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.DataLayer.Tests
{
    [TestFixture]
    public class DynamicBuilderTest
    {
        public class Record
        {
            public int Id { get; set; }

            public bool? Done { get; set; }
        }
        private const int SampleInt = 777;
        private const string FieldId = "Id";
        private const string FieldDone = "Done";
        private const string GetNullableTypeMethod = "GetNullableType";

        [Test]
        public void CreateBuilder()
        {
            // Arrange
            var dataRecord = new SqlDataRecord(
                new SqlMetaData(FieldId, SqlDbType.Int),
                new SqlMetaData(FieldDone, SqlDbType.Bit)
            );

            dataRecord.SetInt32(0, SampleInt);

            // Act
            var builder = DynamicBuilder<Record>.CreateBuilder(dataRecord);

            // Assert
            builder.ShouldNotBeNull();
        }
    }
}
