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
    public class Code
    {
        public Code() { }

        #region Properties
        [DataMember]
        public int CodeID { get; set; }
        [DataMember]
        public int CodeTypeID { get; set; }
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string CodeValue { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string CodeDescription { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public bool HasChildren { get; set; }
        [DataMember]
        public int ParentCodeID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int UpdatedByUserID { get; set; }
        [DataMember]
        public string CodeTypeName { get; set; }
        #endregion

        #region Data
        public static List<Code> GetAll()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Code> code = (List<Code>)CacheUtil.GetFromCache("UL_CODE", DatabaseName);

                if (code == null)
                {
                    code = GetData();

                    CacheUtil.AddToCache("UL_CODE", code, DatabaseName);
                }

                return code;
            }
            else
            {
                return GetData();
            }
        }

        private static List<Code> GetData()
        {
            List<Code> retList = new List<Code>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select c.*, ct.CodeTypeName from Code c with (nolock) join CodeType ct with (nolock) on c.CodeTypeId = ct.CodeTypeId where c.IsActive=1 and ct.Isactive=1", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Code> builder = DynamicBuilder<Code>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Code x = builder.Build(rdr);
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

        public static List<Code> GetQsource()
        {
            return Code.GetAll().FindAll(x => (x.CodeTypeName ?? null) == "Qualification Source Type");
        }

        public static List<Code> GetResponseGroup()
        {
            return Code.GetAll().FindAll(x => (x.CodeTypeName ?? null) == "Response Group");
        }

        public static List<Code> GetDataCompareTarget()
        {
            return Code.GetAll().FindAll(x => (x.CodeTypeName ?? null) == "Data Compare Target");
        }

        public static List<Code> GetDataCompareType()
        {
            return Code.GetAll().FindAll(x => (x.CodeTypeName ?? null) == "Data Compare Type");
        }

        public static List<Code> GetUADFieldType()
        {
            return Code.GetAll().FindAll(x => (x.CodeTypeName ?? null) == "UAD Field Type");
        }

        public static List<Code> GetDataComparePaymentStatus()
        {
            return Code.GetAll().FindAll(x => (x.CodeTypeName ?? null) == "Payment Status");
        }

        public static Code GetByQSourceID(int QSourceID)
        {
            Code retItem = new Code();

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);

            SqlCommand cmd = new SqlCommand("select c.*, ct.CodeTypeName from Code c with (nolock) join CodeType ct with (nolock) on c.CodeTypeId = ct.CodeTypeId where  c.CodeId = @QSourceID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@QSourceID", QSourceID);
            cmd.CommandTimeout = 0;
            try
            {
                var reader = KM.Common.DataFunctions.ExecuteReader(cmd, conn);
                DynamicBuilder<Code> builder = DynamicBuilder<Code>.CreateBuilder(reader);

                while (reader.Read())
                {
                    retItem = builder.Build(reader);
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
