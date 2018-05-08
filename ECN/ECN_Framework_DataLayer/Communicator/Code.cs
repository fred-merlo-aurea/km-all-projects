using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Code
    {
        public static bool Exists()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Exists";
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int codeID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select count(CodeID) from Code where CustomerID = @CustomerID and CodeID = @CodeID and IsDeleted = 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CodeID", codeID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int codeID, string codeValue, string codeType, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Codes_Exists_ByValue";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CodeID", codeID);
            cmd.Parameters.AddWithValue("@CodeValue", codeValue);
            cmd.Parameters.AddWithValue("@CodeType", codeType);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.Code GetByCodeID(int codeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Code WHERE CodeID = @CodeID and IsDeleted = 0 ORDER BY CodeDisplay, SortOrder ASC";
            cmd.Parameters.AddWithValue("@CodeID", codeID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.Code Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Code retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Code();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Code>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        public static List<ECN_Framework_Entities.Communicator.Code> GetByCustomerAndCategory(ECN_Framework_Common.Objects.Communicator.Enums.CodeType ctype, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Code WHERE CodeType = @CodeType AND CustomerID = @CustomerID and IsDeleted = 0 ORDER BY CodeDisplay, SortOrder ASC";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CodeType", ctype.ToString());
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Code> GetByCustomerAndCategory(string type, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Code WHERE CodeType = @CodeType AND CustomerID = @CustomerID and IsDeleted = 0 ORDER BY CodeDisplay, SortOrder ASC";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CodeType", type);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Code> GetAllByCustomer(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Code WHERE CustomerID = @CustomerID and IsDeleted = 0 ORDER BY CodeDisplay, SortOrder ASC";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.Code> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Code> retList = new List<ECN_Framework_Entities.Communicator.Code>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Code retItem = new ECN_Framework_Entities.Communicator.Code();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Code>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static int Save(ECN_Framework_Entities.Communicator.Code code)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Codes_Save";
            cmd.Parameters.Add(new SqlParameter("@CodeID", (object)code.CodeID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)code.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CodeType", code.CodeType));
            cmd.Parameters.Add(new SqlParameter("@CodeValue", code.CodeValue));
            cmd.Parameters.Add(new SqlParameter("@CodeDisplay", code.CodeDisplay));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", (object)code.SortOrder ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DisplayFlag", code.DisplayFlag));
            if (code.CodeID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)code.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)code.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int codeID, int customerID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Delete";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CodeID", codeID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
