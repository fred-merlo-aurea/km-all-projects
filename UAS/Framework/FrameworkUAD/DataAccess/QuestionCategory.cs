using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class QuestionCategory
    {
        public static List<Entity.QuestionCategory> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.QuestionCategory> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuestionCategory_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.QuestionCategory Get(SqlCommand cmd)
        {
            Entity.QuestionCategory retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.QuestionCategory();
                        DynamicBuilder<Entity.QuestionCategory> builder = DynamicBuilder<Entity.QuestionCategory>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        public static List<Entity.QuestionCategory> GetList(SqlCommand cmd)
        {
            List<Entity.QuestionCategory> retList = new List<Entity.QuestionCategory>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.QuestionCategory retItem = new Entity.QuestionCategory();
                        DynamicBuilder<Entity.QuestionCategory> builder = DynamicBuilder<Entity.QuestionCategory>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }        
    }
}
