using System.Data;
using System.Data.SqlClient.Fakes;
using FrameworkUAS.DataAccess;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.FrameworkUAS.DataAccess
{
    public partial class ClientMethodsTest
    {
        private const string TableTempHaymarketCmsEcomm = "tempHaymarketCMS_Ecomm";
        private const string TableTempHaymarketCmsPublication = "tempHaymarketCMS_Publication";

        [Test]
        public void Haymarket_CMS_Insert_Ecomm_DataTableIsNotNull_WritesDataTableToServer()
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
            var result = ClientMethods.Haymarket_CMS_Insert_Ecomm(table);

            // Assert
            result.ShouldBeTrue();
            destinationTable.ShouldContain(TableTempHaymarketCmsEcomm);
        }

        [Test]
        public void Haymarket_CMS_Insert_PublicationPubCode_DataTableIsNotNull_WritesDataTableToServer()
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
            var result = ClientMethods.Haymarket_CMS_Insert_PublicationPubCode(table);

            // Assert
            result.ShouldBeTrue();
            destinationTable.ShouldContain(TableTempHaymarketCmsPublication);
        }
    }
}
