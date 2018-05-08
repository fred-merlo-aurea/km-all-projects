using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Communicator.Entity
{
    [Serializable]
    [DataContract]
    public class Blast
    {
        public Blast() { }
        #region Properties
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string EmailFrom { get; set; }
        [DataMember]
        public string EmailFromName { get; set; }
        [DataMember]
        public DateTime SendTime { get; set; }
        [DataMember]
        public int AttemptTotal { get; set; }
        [DataMember]
        public int SendTotal { get; set; }
        [DataMember]
        public int SendBytes { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string BlastType { get; set; }
        [DataMember]
        public int CodeID { get; set; }
        [DataMember]
        public int LayoutID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public DateTime FinishTime { get; set; }
        [DataMember]
        public int SuccessTotal { get; set; }
        [DataMember]
        public string BlastLog { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public string Spinlock { get; set; }
        [DataMember]
        public string ReplyTo { get; set; }
        [DataMember]
        public string TestBlast { get; set; }
        [DataMember]
        public string BlastFrequency { get; set; }
        [DataMember]
        public string RefBlastID { get; set; }
        [DataMember]
        public string BlastSuppression { get; set; }
        [DataMember]
        public bool AddOptOuts_to_MS { get; set; }
        [DataMember]
        public string DynamicFromName { get; set; }
        [DataMember]
        public string DynamicFromEmail { get; set; }
        [DataMember]
        public string DynamicReplyToEmail { get; set; }
        [DataMember]
        public int BlastEngineID { get; set; }
        [DataMember]
        public bool HasEmailPreview { get; set; }
        #endregion
        #region Data
        #region Select
        public static Blast GetByBlastID(int blastID)
        {
            Blast retItem = new Blast();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_Select_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
            var builder = DynamicBuilder<Blast>.CreateBuilder(rdr);

            while (rdr.Read())
            {
                retItem = builder.Build(rdr);
            }
            rdr.Close();
            rdr.Dispose();
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        public static Blast GetByNodeID(string NodeID)
        {
            Blast retItem = new Blast();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_Select_NodeID";
            cmd.Parameters.AddWithValue("@NodeID", NodeID);

            SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
            var builder = DynamicBuilder<Blast>.CreateBuilder(rdr);

            while (rdr.Read())
            {
                retItem = builder.Build(rdr);
            }
            rdr.Close();
            rdr.Dispose();
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        #endregion
        #region CRUD
        public static bool SetHasEmailPreview(int blastID, bool hasEmailPreview)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_Set_HasEmailPreview";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@hasEmailPreview", hasEmailPreview);

            return ECN_Framework_DataLayer.DataFunctions.ExecuteNonQuery(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
        }
        #endregion
        #endregion
    }
}
