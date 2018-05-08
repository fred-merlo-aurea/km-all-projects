using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;
using System.Configuration;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class Category
    {
        public Category() { }

        #region Properties
        [DataMember]
        public int CategoryCodeID { get; set; }
        [DataMember]
        public int CategoryCodeTypeID { get; set; }
        [DataMember]
        public string CategoryCodeName { get; set; }
        [DataMember]
        public string CategoryCodeValue { get; set; }
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
        #endregion

        #region Data
        public static List<Category> GetAll()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string CacheRegion = "UAD_Lookup";

                List<Category> lcategory = (List<Category>)CacheUtil.GetFromCache("UL_CATEGORY", CacheRegion);

                if (lcategory == null)
                {
                    lcategory = GetData();

                    CacheUtil.AddToCache("UL_CATEGORY", lcategory, CacheRegion);
                }

                return lcategory;
            }
            else
            {
                return GetData();
            }
        }

        private static List<Category> GetData()
        {
            List<Category> retList = new List<Category>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select CategoryCodeID, convert(varchar,CategoryCodeValue) + '-' + CategoryCodeName as CategoryCodeName from CategoryCode with (nolock) where IsActive = 1 order by CategoryCodeValue", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Category> builder = DynamicBuilder<Category>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Category x = builder.Build(rdr);
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
        #endregion
    }
}
