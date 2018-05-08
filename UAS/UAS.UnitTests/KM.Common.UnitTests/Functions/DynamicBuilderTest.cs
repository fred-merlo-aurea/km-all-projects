using System;
using System.Data;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    public class DynamicBuilderTest
    {
        private const int SampleInt = 777;
        private const string FieldId = "Id";
        private const string FieldDone = "Done";
        private const string GetNullableTypeMethod = "GetNullableType";

        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void Teardown()
        {
            _shims?.Dispose();
        }

        public class Record
        {
            public int Id { get; set; }

            public bool? Done { get; set; }
        }

        [Test]
        public void CreateBuilder_FilledRecords_FilledBuilder()
        {
            // Arrange
            var dataRecord = CreateRecord();
            dataRecord.SetInt32(0, SampleInt);

            // Act
            var builder = DynamicBuilder<Record>.CreateBuilder(dataRecord);

            // Assert
            builder.ShouldNotBeNull();

            var builtRecord = builder.Build(dataRecord);
            builtRecord.ShouldNotBeNull();
            builtRecord.Id.ShouldBe(SampleInt);
            builtRecord.Done.ShouldBeNull();
        }
       
        [Test]
        public void CreateBuilder_FilledRecords_ThrowsException()
        {
            // Arrange, Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                DynamicBuilder<Record>.CreateBuilder(null));
        }

        [TestCase(typeof(bool), typeof(bool?))]
        [TestCase(typeof(byte), typeof(byte?))]
        [TestCase(typeof(DateTime), typeof(DateTime?))]
        [TestCase(typeof(decimal), typeof(decimal?))]
        [TestCase(typeof(double), typeof(double?))]
        [TestCase(typeof(float), typeof(float?))]
        [TestCase(typeof(Guid), typeof(Guid?))]
        [TestCase(typeof(Int16), typeof(Int16?))]
        [TestCase(typeof(Int32), typeof(Int32?))]
        [TestCase(typeof(Int64), typeof(Int64?))]
        public void GetNullableType(Type srcType, Type dstType)
        {
            // Arrange
            var dataRecord = CreateRecord();
            var privateObject = new PrivateObject(KM.Common.DynamicBuilder<Record>.CreateBuilder(dataRecord));

            // Act
            var result = privateObject.Invoke(GetNullableTypeMethod, srcType);

            // Assert
            result.ShouldBe(dstType);
        }

        [TestCase(typeof(TimeSpan), typeof(TimeSpan?))]
        public void GetNullableType_TypeDoesNotExist_ReturnsNull(Type srcType, Type dstType)
        {
            // Arrange
            var dataRecord = CreateRecord();
            var privateObject = new PrivateObject(KM.Common.DynamicBuilder<Record>.CreateBuilder(dataRecord));

            // Act
            var result = privateObject.Invoke(GetNullableTypeMethod, srcType);

            // Assert
            result.ShouldBeNull();
        }

        [TestCase(typeof(TimeSpan), typeof(TimeSpan?))]
        public void GetNullableType_TimeSpanExist_ReturnsNullableTimeSpan(Type srcType, Type dstType)
        {
            // Arrange
            var dataRecord = CreateRecord();

            var config = new DynamicBuilderConfiguration();
            config.TimeSpanFieldNullable = true;
            var privateObject = new PrivateObject(KM.Common.DynamicBuilder<Record>.CreateBuilder(dataRecord, config));

            // Act
            var result = privateObject.Invoke(GetNullableTypeMethod, srcType);

            // Assert
            result.ShouldBe(dstType);
        }

        private SqlDataRecord CreateRecord()
        {
            var dataRecord = new SqlDataRecord(
               new SqlMetaData(FieldId, SqlDbType.Int),
               new SqlMetaData(FieldDone, SqlDbType.Bit)
           );

            return dataRecord;
        }
    }
}
