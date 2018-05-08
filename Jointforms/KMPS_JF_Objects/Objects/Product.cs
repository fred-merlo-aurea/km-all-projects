using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int GroupID { get; set; }
        public int CustomerID { get; set; }
        public string ImageName { get; set; }
        public bool IsBundle { get; set; }
        public bool IsSubscription { get; set; }
        public string PubCode { get; set; }
        public string ProductDesc { get; set; }


        public static Product GetByProductID(int ProductID)
        {
            Product retItem = new Product();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Product  with (NOLOCK) where ProductID=@ProductID";
            cmd.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int)).Value = ProductID;

            SqlDataReader rdr = DataFunctions.ExecuteReader(cmd);
            DynamicBuilder<Product> builder = DynamicBuilder<Product>.CreateBuilder(rdr);

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
    }
}
