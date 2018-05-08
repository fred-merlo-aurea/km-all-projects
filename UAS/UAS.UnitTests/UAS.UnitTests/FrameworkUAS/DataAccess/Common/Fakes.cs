using System;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

using ShimKMCommonDataFunctions = KM.Common.Fakes.ShimDataFunctions;
using ShimKMPlatformDataFunctions = KMPlatform.DataAccess.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkUAS.DataAccess.Common
{
    [ExcludeFromCodeCoverage]
    public class Fakes
    {
        private IDisposable _context;

        public void SetupFakes()
        {
            _context = ShimsContext.Create();
            ShimForSqlConnection();
            ShimForSqlCommand(1);
            ShimKMCommonDataFunctions.GetSqlConnectionString = s => new SqlConnection();
            ShimKMPlatformDataFunctions.GetClientSqlConnectionClient = _ => new SqlConnection();
        }

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        protected virtual void ShimForSqlConnection()
        {
            ShimSqlConnection.AllInstances.Open = connection => { };
        }

        protected virtual void ShimForSqlCommand(int affectedRows)
        {
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command => affectedRows;
        }
    }
}