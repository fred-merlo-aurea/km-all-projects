using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAD.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="SubscriberTransformed"/>
    /// </summary>
    [TestFixture]
    public partial class SubscriberTransformedTest
    {
        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithProcessCode_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.Select(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectProcessCode),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectTopOne_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.SelectTopOne(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectTopOne),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithSourceFileId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.Select(Client, SourceFileID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSourceFileId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectDimensionCount_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var expectedList = new List<DimensionErrorCount>
            {
                new DimensionErrorCount()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return expectedList.GetSqlDataReader();
            };

            // Act
            var result = SubscriberTransformed.SelectDimensionCount(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectDimensionCount),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => result.ShouldBe(expectedList.First()));
        }

        [Test]
        public void SelectImportRowNumbers_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var expectedList = new List<ImportRowNumber>
            {
                new ImportRowNumber()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return expectedList.GetSqlDataReader();
            };

            // Act
            var result = SubscriberTransformed.SelectImportRowNumbers(Client, ProcessCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectImportRowNumbers),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(expectedList).ShouldBeTrue());
        }

        [Test]
        public void SelectByAddressValidation_WhenCalledWithSourceFileId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.SelectByAddressValidation(Client, SourceFileID, IsLatLonValid);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => _sqlCommand.Parameters[ParamIsLatLonValid].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByAddressValidationSourceFileId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByAddressValidation_WhenCalledWithProcessCodeAndSourceFileId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.SelectByAddressValidation(Client, ProcessCode, SourceFileID, IsLatLonValid);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => _sqlCommand.Parameters[ParamIsLatLonValid].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByAddressValidationProcessCodeSourceFileId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByAddressValidation_WhenCalledWithProcessCode_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.SelectByAddressValidation(Client, ProcessCode, IsLatLonValid);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamIsLatLonValid].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByAddressValidationProcessCode),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByAddressValidation_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.SelectByAddressValidation(Client, IsLatLonValid);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamIsLatLonValid].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByAddressValidation),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForFileAudit_WhenCalled_VerifySqlParameters()
        {
            // Arrange 
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            // Act
            var result = SubscriberTransformed.SelectForFileAudit(ProcessCode, SourceFileID, startDate, endDate, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProcessCode].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => _sqlCommand.Parameters[ParamStartDate].Value.ShouldBe(startDate),
                () => _sqlCommand.Parameters[ParamEndDate].Value.ShouldBe(endDate),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForFileAudit),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForGeoCoding_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.SelectForGeoCoding(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForGeoCoding),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForGeoCoding_WhenCalledWithSourceFileId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberTransformed.SelectForGeoCoding(Client, SourceFileID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamSourceFileId].Value.ShouldBe(SourceFileID),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForGeoCodingSourceFileId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}