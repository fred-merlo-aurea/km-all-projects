using System;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class AcsFileHeader
    {
        public static int Save(Entity.AcsFileHeader x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AcsFileHeader_Save";
            cmd.Parameters.Add(new SqlParameter("@AcsFileHeaderId", x.AcsFileHeaderId));
            cmd.Parameters.Add(new SqlParameter("@RecordType", x.RecordType));
            cmd.Parameters.Add(new SqlParameter("@FileVersion", x.FileVersion));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", x.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@CreateDate", x.CreateDate));
            cmd.Parameters.Add(new SqlParameter("@ShipmentNumber", x.ShipmentNumber));
            cmd.Parameters.Add(new SqlParameter("@TotalAcsRecordCount", x.TotalAcsRecordCount));
            cmd.Parameters.Add(new SqlParameter("@TotalCoaCount", x.TotalCoaCount));
            cmd.Parameters.Add(new SqlParameter("@TotalNixieCount", x.TotalNixieCount));
            cmd.Parameters.Add(new SqlParameter("@TrdRecordCount", x.TrdRecordCount));
            cmd.Parameters.Add(new SqlParameter("@TrdAcsFeeAmount", x.TrdAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@TrdCoaCount", x.TrdCoaCount));
            cmd.Parameters.Add(new SqlParameter("@TrdCoaAcsFeeAmount", x.TrdCoaAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@TrdNixieCount", x.TrdNixieCount));
            cmd.Parameters.Add(new SqlParameter("@TrdNixieAcsFeeAmount", x.TrdNixieAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@OcdRecordCount", x.OcdRecordCount));
            cmd.Parameters.Add(new SqlParameter("@OcdAcsFeeAmount", x.OcdAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@OcdCoaCount", x.OcdCoaCount));
            cmd.Parameters.Add(new SqlParameter("@OcdCoaAcsFeeAmount", x.OcdCoaAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@OcdNixieCount", x.OcdNixieCount));
            cmd.Parameters.Add(new SqlParameter("@OcdNixieAcsFeeAmount", x.OcdNixieAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@FsRecordCount", x.FsRecordCount));
            cmd.Parameters.Add(new SqlParameter("@FsAcsFeeAmount", x.FsAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@FsCoaCount", x.FsCoaCount));
            cmd.Parameters.Add(new SqlParameter("@FsCoaAcsFeeAmount", x.FsCoaAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@FsNixieCount", x.FsNixieCount));
            cmd.Parameters.Add(new SqlParameter("@FsNixieAcsFeeAmount", x.FsNixieAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@ImpbRecordCount", x.ImpbRecordCount));
            cmd.Parameters.Add(new SqlParameter("@ImpbAcsFeeAmount", x.ImpbAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@ImpbCoaCount", x.ImpbCoaCount));
            cmd.Parameters.Add(new SqlParameter("@ImpbCoaAcsFeeAmount", x.ImpbCoaAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@ImpbNixieCount", x.ImpbNixieCount));
            cmd.Parameters.Add(new SqlParameter("@ImpbNixieAcsFeeAmount", x.ImpbNixieAcsFeeAmount));
            cmd.Parameters.Add(new SqlParameter("@Filler", x.Filler));
            cmd.Parameters.Add(new SqlParameter("@EndMarker", x.EndMarker));
            cmd.Parameters.Add(new SqlParameter("@ProcessCode", x.ProcessCode));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
