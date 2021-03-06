﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class User
    {
        public static bool SaveBulkXml(string xml)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_User_SaveBulkXml";
                cmd.Parameters.AddWithValue("@xml", xml);
                cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.User", "SaveBulkXml");
            }
            return success;
        }
    }
}
