using System.Data;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.DataAccess;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.FrameworkUAS.DataAccess.Common;

namespace UAS.UnitTests.FrameworkUAS.DataAccess
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ClientMethodsTest : Fakes
    {
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void Advanstar_Insert_RegCodeCompare_DataTableIsNotNull_WritesDataTableToServer()
        {
            // Arrange
            var table = new DataTable();
            string destinationTable = null;
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (copy, _) =>
            {
                destinationTable = copy.DestinationTableName;
                copy.Close();
            };

            // Act
            ClientMethods.Advanstar_Insert_RegCodeCompare(table);

            // Assert
            destinationTable.ShouldContain(Consts.TableTempAdvanstarRegCodeCompare);
        }

        [Test]
        public void Advanstar_Insert_RegCode_DataTableIsNotNull_WritesDataTableToServer()
        {
            // Arrange
            var table = new DataTable();
            string destinationTable = null;
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (copy, _) =>
            {
                destinationTable = copy.DestinationTableName;
                copy.Close();
            };

            // Act
            ClientMethods.Advanstar_Insert_RegCode(table);

            // Assert
            destinationTable.ShouldContain(Consts.TableTempAdvanstarRegCode);
        }

        [Test]
        public void Advanstar_Insert_SourceCode_DataTableIsNotNull_WritesDataTableToServer()
        {
            // Arrange
            var table = new DataTable();
            string destinationTable = null;
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (copy, _) =>
            {
                destinationTable = copy.DestinationTableName;
                copy.Close();
            };

            // Act
            ClientMethods.Advanstar_Insert_SourceCode(table);

            // Assert
            destinationTable.ShouldContain(Consts.TableTempAdvanstarSourceCode);
        }

        [Test]
        public void Advanstar_Insert_PriCode_DataTableIsNotNull_WritesDataTableToServer()
        {
            // Arrange
            var table = new DataTable();
            string destinationTable = null;
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (copy, _) =>
            {
                destinationTable = copy.DestinationTableName;
                copy.Close();
            };

            // Act
            ClientMethods.Advanstar_Insert_PriCode(table);

            // Assert
            destinationTable.ShouldContain(Consts.TableTempAdvanstarPriCode);
        }

        [Test]
        public void Advanstar_Insert_RefreshDupes_DataTableIsNotNull_WritesDataTableToServer()
        {
            // Arrange
            var table = new DataTable();
            string destinationTable = null;
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (copy, _) =>
            {
                destinationTable = copy.DestinationTableName;
                copy.Close();
            };

            // Act
            ClientMethods.Advanstar_Insert_RefreshDupes(table);

            // Assert
            destinationTable.ShouldContain(Consts.TableTempAdvanstarRefreshDupes);
        }
    }
}
