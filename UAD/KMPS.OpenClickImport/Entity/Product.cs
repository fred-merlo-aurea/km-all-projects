using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.ActivityImport.Entity
{
    public class Product
    {
        public Product() { }
        #region Properties
        public int PubID { get; set; }
        public string PubName { get; set; }
        public string PubCode { get; set; }
        public int? PubTypeID { get; set; }
        public int? GroupID { get; set; }
        public bool? IsTradeShow { get; set; }
        public bool? EnableSearching { get; set; }
        public int? Score { get; set; }
        public string ProductType { get; set; }
        #endregion

        #region Data
        public static Product Get(string pubCode, string sql)
        {
            Product retItem = new Product();
            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Pubs_Select_PubCode";
            cmd.Parameters.AddWithValue("@PubCode", pubCode);

            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DynamicBuilder<Product> builder = DynamicBuilder<Product>.CreateBuilder(rdr);

            while (rdr.Read())
            {
                retItem = builder.Build(rdr);
            }
            rdr.Close();
            rdr.Dispose();
            cmd.Connection.Close();

            return retItem;
        }
        public static bool PubCodeExist(string pubCode, string sql)
        {
            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Pubs_PubCode_Exist";
            cmd.Parameters.AddWithValue("@PubCode", pubCode);
            cmd.Connection.Open();

            int rows = cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            if (rows >= 1)
                return true;
            else
                return false;
        }
        public static int Insert(Product x, string sql)
        {
            if (x.PubTypeID.HasValue == false)
                x.PubTypeID = -1;
            if (x.GroupID.HasValue == false)
                x.GroupID = -1;
            if (x.IsTradeShow.HasValue == false)
                x.IsTradeShow = false;
            if (x.EnableSearching.HasValue == false)
                x.EnableSearching = false;
            if (x.Score.HasValue == false)
                x.Score = 0;

            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Pubs_Insert";
            cmd.Parameters.AddWithValue("@PubName", x.PubName);
            cmd.Parameters.AddWithValue("@IsTradeShow", x.IsTradeShow.Value);
            cmd.Parameters.AddWithValue("@PubCode", x.PubCode);
            cmd.Parameters.AddWithValue("@PubTypeID", x.PubTypeID.Value);
            cmd.Parameters.AddWithValue("@GroupID", x.GroupID.Value);
            cmd.Parameters.AddWithValue("@EnableSearching", x.EnableSearching.Value);
            cmd.Parameters.AddWithValue("@Score", x.Score.Value);
            cmd.Parameters.AddWithValue("@ProductType", x.ProductType);
            cmd.Connection.Open();

            int ID = 0;
            int.TryParse(cmd.ExecuteScalar().ToString(), out ID);

            cmd.Connection.Close();
            return ID;
        }
        #endregion
    }
}
