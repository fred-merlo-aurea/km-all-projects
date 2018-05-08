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
    public class ProductDetail
    {

        public static List<ECN_Framework_Entities.Accounts.ProductDetail> GetAll()
        {
            List<ECN_Framework_Entities.Accounts.ProductDetail> retItemList = new List<ECN_Framework_Entities.Accounts.ProductDetail>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductDetail_Select_All";
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.ProductDetail> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.ProductDetail> retList = new List<ECN_Framework_Entities.Accounts.ProductDetail>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            ECN_Framework_Entities.Accounts.ProductDetail retItem = new ECN_Framework_Entities.Accounts.ProductDetail();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.ProductDetail>.CreateBuilder(rdr);
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
