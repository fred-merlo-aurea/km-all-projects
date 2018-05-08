using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class Brand
    {
        public Brand() { }

        #region Properties
        [DataMember]
        public int BrandID { get; set; }
        [DataMember]
        public string BrandName  { get; set; }
        [DataMember]
        public string Logo { get; set; }
        [DataMember]
        public bool IsBrandGroup { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        #endregion

        #region Data
        public static List<Brand> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Brand> brands = (List<Brand>)CacheUtil.GetFromCache("BRAND", DatabaseName);

                if (brands == null)
                {
                    brands = GetData(clientconnection);

                    CacheUtil.AddToCache("BRAND", brands, DatabaseName);
                }

                return brands;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<Brand> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Brand> retList = new List<Brand>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select * from brand with (nolock) where IsDeleted = 0", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Brand> builder = DynamicBuilder<Brand>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Brand x = builder.Build(rdr);
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

        public static Brand GetByID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            Brand b = GetAll(clientconnection).Find(x => x.BrandID == brandID);
            return b;
        }

        public static List<Brand> GetByUserID(KMPlatform.Object.ClientConnections clientconnection, int UserID)
        {
            List<Brand> retList = new List<Brand>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select b.* from Brand b with (nolock) join userbrand ub with (nolock) on b.BrandID = ub.brandID where b.IsDeleted = 0 and userID = @UserID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Brand> builder = DynamicBuilder<Brand>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Brand x = builder.Build(rdr);
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

        public static List<Brand> GetByPubID(KMPlatform.Object.ClientConnections clientconnection, int pubID)
        {
            List<Brand> retList = new List<Brand>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select b.* from Brand b with (nolock) join branddetails bd with (nolock) on b.BrandID = bd.brandID where b.IsDeleted = 0 and PubID = @PubID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PubID", pubID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Brand> builder = DynamicBuilder<Brand>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Brand x = builder.Build(rdr);
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

        public static bool ExistsByBrandName(KMPlatform.Object.ClientConnections clientconnection, int brandID, string brandName)
        {
            SqlCommand cmd = new SqlCommand("Select * from brand with (nolock) where IsDeleted = 0 and BrandName = @brandName and BrandID <> @brandID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@brandName", brandName));
            cmd.Parameters.Add(new SqlParameter("@brandID", brandID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static bool LogoAssociatedWithBrand(KMPlatform.Object.ClientConnections clientconnection, string logoName)
        {
            SqlCommand cmd = new SqlCommand("Select * from brand with (nolock) where logo = @logoname");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@logoname", logoName));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("BRAND", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("BRAND", DatabaseName);
                }
            }
        }
        #endregion

        #region CRUD

        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Brand b)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_Brand_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BrandID", b.BrandID));
            cmd.Parameters.Add(new SqlParameter("@BrandName", b.BrandName));
            cmd.Parameters.Add(new SqlParameter("@Logo", b.Logo));
            cmd.Parameters.Add(new SqlParameter("@IsBrandGroup", b.IsBrandGroup));
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", b.IsDeleted));
            cmd.Parameters.Add(new SqlParameter("@CreatedUserID", b.CreatedUserID));
            cmd.Parameters.Add(new SqlParameter("@CreatedDate", b.CreatedDate));
            cmd.Parameters.Add(new SqlParameter("@UpdatedUserID", b.UpdatedUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedDate", b.UpdatedDate));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int BrandID , int UserID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_Brand_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BrandID", BrandID));
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion

    }
}
