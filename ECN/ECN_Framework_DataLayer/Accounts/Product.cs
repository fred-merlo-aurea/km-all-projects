using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class Product
    {
        public static List<ECN_Framework_Entities.Accounts.Product> GetAll()
        {
            List<ECN_Framework_Entities.Accounts.Product> retItemList = new List<ECN_Framework_Entities.Accounts.Product>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Product_Select_All";
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.Product> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.Product> retList = new List<ECN_Framework_Entities.Accounts.Product>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            ECN_Framework_Entities.Accounts.Product retItem = new ECN_Framework_Entities.Accounts.Product();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Product>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                retItem = builder.Build(rdr);
                                retList.Add(retItem);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return retList;
        }
    }
}
