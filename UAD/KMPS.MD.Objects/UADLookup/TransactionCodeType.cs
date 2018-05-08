using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;
using System.Configuration;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class TransactionCodeType
    {
        public TransactionCodeType() { }

        #region Properties
        [DataMember]
        public int TransactionCodeTypeID { get; set; }
        [DataMember]
        public string TransactionCodeTypeName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsFree { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int UpdatedByUserID { get; set; }
        #endregion

        #region Data
        public static List<TransactionCodeType> GetAll()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<TransactionCodeType> transactioncodetypes = (List<TransactionCodeType>)CacheUtil.GetFromCache("UL_TRANSACTIONCODETYPE", DatabaseName);

                if (transactioncodetypes == null)
                {
                    transactioncodetypes = GetData();

                    CacheUtil.AddToCache("UL_TRANSACTIONCODETYPE", transactioncodetypes, DatabaseName);
                }

                return transactioncodetypes;
            }
            else
            {
                return GetData();
            }
        }

        private static List<TransactionCodeType> GetData()
        {
            List<TransactionCodeType> retList = new List<TransactionCodeType>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select TransactionCodeTypeID, TransactionCodeTypeName from TransactionCodeType with (nolock) where IsActive = 1 order by TransactionCodeTypeName", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<TransactionCodeType> builder = DynamicBuilder<TransactionCodeType>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    TransactionCodeType x = builder.Build(rdr);

                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static TransactionCodeType GetByPubTransactionID(int TransactionID)
        {
            TransactionCodeType retItem = new TransactionCodeType();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select tct.TransactionCodeTypeID, TransactionCodeTypeName from TransactionCodeType tct with (nolock) join TransactionCode c  with (nolock) on tct.TransactionCodeTypeID = c.TransactionCodeTypeID where c.TransactionCodeID = @PubTransactionCodeID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@PubTransactionCodeID", TransactionID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<TransactionCodeType> builder = DynamicBuilder<TransactionCodeType>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retItem;
        }

        #endregion
    }
}
