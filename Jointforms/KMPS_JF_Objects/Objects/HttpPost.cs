using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace KMPS_JF_Objects.Objects
{
    public class HttpPost
    {
        public static DataTable getHttpPostParams(int entityID, bool isNewsLetter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from HttpPost hp with (NOLOCK) inner join HttpPostParams hpp  with (NOLOCK) on hp.HttpPostID=hpp.HttpPostID where hp.EntityID=@entityID and hp.IsNewsLetter=@isNewsLetter";
            cmd.Parameters.AddWithValue("@entityID", entityID);
            cmd.Parameters.AddWithValue("@isNewsLetter", isNewsLetter);
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return dt;
        }

        public static DataTable getPaidFormProductHttpPostParams(int ProductID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from ProductHttpPost php with (NOLOCK) inner join ProductHttpPostParams phpp  with (NOLOCK) on php.ProductHttpPostID=phpp.ProductHttpPostID where php.ProductID=@ProductID";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return dt;
        }
    }
}
