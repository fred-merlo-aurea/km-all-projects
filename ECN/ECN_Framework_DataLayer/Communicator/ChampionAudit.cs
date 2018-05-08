using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    public class ChampionAudit
    {
        public static int Insert(ECN_Framework_Entities.Communicator.ChampionAudit champion)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChampionAudit_Insert";
            cmd.Parameters.AddWithValue("@SampleID", champion.SampleID);
            cmd.Parameters.AddWithValue("@BlastIDA", champion.BlastIDA);
            cmd.Parameters.AddWithValue("@BlastIDB", champion.BlastIDB);
            cmd.Parameters.AddWithValue("@BlastIDChampion", champion.BlastIDChampion);
            cmd.Parameters.AddWithValue("@ClicksA", champion.ClicksA);
            cmd.Parameters.AddWithValue("@ClicksB", champion.ClicksB);
            cmd.Parameters.AddWithValue("@OpensA", champion.OpensA);
            cmd.Parameters.AddWithValue("@OpensB", champion.OpensB);
            cmd.Parameters.AddWithValue("@BouncesA", champion.BouncesA);
            cmd.Parameters.AddWithValue("@BouncesB", champion.BouncesB);
            cmd.Parameters.AddWithValue("@BlastIDWinning", champion.BlastIDWinning);
            cmd.Parameters.AddWithValue("@SendToNonWinner", champion.SendToNonWinner);
            cmd.Parameters.AddWithValue("@Reason", champion.Reason);
            
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd,DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
        public static bool Exists(int ChampionAuditID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChampionAudit_Exists_ByChampionAuditID";
            cmd.Parameters.AddWithValue("@ChampionAuditID", ChampionAuditID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }
        public static bool ExistsByBlastIDChampion(int BlastIDChampion)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChampionAudit_Exists_ByBlastIDCHampion";
            cmd.Parameters.AddWithValue("@BlastIDChampion", BlastIDChampion);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }
        public static bool ExistsBySampleID(int SampleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChampionAudit_Exists_BySampleID";
            cmd.Parameters.AddWithValue("@SampleID", SampleID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }
        public static ECN_Framework_Entities.Communicator.ChampionAudit GetByChampionAuditID(int championAuditID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChampionAudit_Select";
            cmd.Parameters.AddWithValue("@ChampionAuditID", championAuditID);
            return Get(cmd);
        }
        private static ECN_Framework_Entities.Communicator.ChampionAudit Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.ChampionAudit retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.ChampionAudit();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ChampionAudit>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            return retItem;
        }

    }
}
