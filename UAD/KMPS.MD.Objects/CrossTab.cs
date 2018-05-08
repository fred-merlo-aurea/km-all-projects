using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using KM.Common;
using KMPlatform.Object;

namespace KMPS.MD.Objects
{
    public class CrossTab
    {
        private const string QueriesParameterName = "@Queries";
        private const string RowParameterName = "@Row";
        private const string ColumnParameterName = "@Column";
        private const string ProductIdParameterName = "@PubID";
        private const string BrandIdParameterName = "@BrandID";
        private const string IsRecencyViewParameterName = "@IsRecencyView";
        private const string GetMasterCrossTabDataCommandText = "sp_GetCrossTabConsensusData";
        private const string GetProductCrossTabDataCommandText = "sp_GetCrossTabProductData";

        #region Properties
        public string RowDesc { get; set; }
        public string RowValue { get; set; }
        public string ColDesc { get; set; }
        public string ColValue { get; set; }
        public int Counts { get; set; }
        #endregion

        public static IList<CrossTab> GetMasterCrossTabData(
            ClientConnections clientConnections,
            StringBuilder queries,
            string column,
            string row,
            int brandId,
            bool isRecencyView)
        {
            Guard.NotNull(queries, nameof(queries));

            var sqlParameters = GetStandardSqlParameter(queries.ToString(), column, row).ToList();
            sqlParameters.Add(new SqlParameter(BrandIdParameterName, brandId));
            sqlParameters.Add(new SqlParameter(IsRecencyViewParameterName, isRecencyView));

            return GetCrossTabs(clientConnections, GetMasterCrossTabDataCommandText, sqlParameters);
        }

        public static IList<CrossTab> GetProductCrossTabData(
            ClientConnections clientConnections,
            StringBuilder queries,
            string column,
            string row,
            int productId)
        {
            Guard.NotNull(queries, nameof(queries));

            var sqlParameters = GetStandardSqlParameter(queries.ToString(), column, row).ToList();
            sqlParameters.Add(new SqlParameter(ProductIdParameterName, productId));

            return GetCrossTabs(clientConnections, GetProductCrossTabDataCommandText, sqlParameters);
        }

        private static IEnumerable<SqlParameter> GetStandardSqlParameter(string queries, string column, string row)
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter(QueriesParameterName, queries)
                {
                    SqlDbType = SqlDbType.Text
                },
                new SqlParameter(RowParameterName, row),
                new SqlParameter(ColumnParameterName, column)
            };
            
            return sqlParameters;
        }

        private static IList<CrossTab> GetCrossTabs(ClientConnections clientConnections, string commandText, IEnumerable<SqlParameter> sqlParameters)
        {
            IList<CrossTab> crossTabs;

            using (var sqlConnection = DataFunctions.GetClientSqlConnection(clientConnections))
            {
                using (var sqlCommand = DataFunctions.CreateStoredProcedureSqlCommand(
                    commandText,
                    sqlConnection,
                    sqlParameters))
                {
                    sqlConnection.Open();
                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        crossTabs = Utilities.CreateListFromBuilder<CrossTab>(dataReader);
                    }
                }
            }

            return crossTabs;
        }
    }
}
