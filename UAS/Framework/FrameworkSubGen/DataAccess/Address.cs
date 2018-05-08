using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class Address
    {
        public static bool SaveBulkXml(string xml)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Address_SaveBulkXml";
                cmd.Parameters.AddWithValue("@xml", xml);
                cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Address", "SaveBulkXml");
            }
            return success;
        }
        public static List<Entity.Address> Select(int subscriberId)
        {
            List<Entity.Address> retItem = new List<Entity.Address>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Address_Select_SubscriberId";
                cmd.Parameters.AddWithValue("@subscriberId", subscriberId);
                retItem = GetList(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Address", "Select");
            }
            return retItem;
        }
        public static List<Entity.Address> SelectOnMailingId(int subscriberId, int publicationId)
        {
            List<Entity.Address> retItem = new List<Entity.Address>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Address_SelectOnMailingId_SubscriberId_PublicationId";
                cmd.Parameters.AddWithValue("@SubscriberId", subscriberId);
                cmd.Parameters.AddWithValue("@PublicationId", publicationId);
                retItem = GetList(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Address", "SelectOnMailingId");
            }
            return retItem;
        }
        public static List<Entity.Address> SelectOnMailingId(int subscriptionId)
        {
            List<Entity.Address> retItem = new List<Entity.Address>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Address_SelectOnMailingId_SubscriptionId";
                cmd.Parameters.AddWithValue("@subscriptionId", subscriptionId);
                retItem = GetList(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Address", "SelectOnMailingId");
            }
            return retItem;
        }
        public static List<Entity.Address> SelectOnMailingId(int accountId, int publicationId, string address, string address2, string city, string state, string zipCode)
        {
            List<Entity.Address> retItem = new List<Entity.Address>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Address_Select_OldAddress_AccountId_PublicationId";
                cmd.Parameters.AddWithValue("@AccountId", accountId);
                cmd.Parameters.AddWithValue("@PublicationId", publicationId);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@address2", address2);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@zipCode", zipCode);
                retItem = GetList(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Address", "SelectOnMailingId");
            }
            return retItem;
        }
        private static Entity.Address Get(SqlCommand cmd)
        {
            Entity.Address retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Address();
                        DynamicBuilder<Entity.Address> builder = DynamicBuilder<Entity.Address>.CreateBuilder(rdr);
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
        private static List<Entity.Address> GetList(SqlCommand cmd)
        {
            List<Entity.Address> retList = new List<Entity.Address>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Address retItem = new Entity.Address();
                        DynamicBuilder<Entity.Address> builder = DynamicBuilder<Entity.Address>.CreateBuilder(rdr);
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
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Address", "GetList");
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        public static bool Save(Entity.Address address)
        {
            List<Entity.Address> list = new List<Entity.Address>();
            list.Add(address);
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.Address>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());
            SqlBulkCopy bc = default(SqlBulkCopy);
            try
            {
                conn.Open();
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "Address");
                bc.BatchSize = 0;
                bc.BulkCopyTimeout = 0;
                bc.ColumnMappings.Add("address_id", "address_id");
                bc.ColumnMappings.Add("account_id", "account_id");
                bc.ColumnMappings.Add("first_name", "first_name");
                bc.ColumnMappings.Add("last_name", "last_name");
                bc.ColumnMappings.Add("address", "address");
                bc.ColumnMappings.Add("address_line_2", "address_line_2");
                bc.ColumnMappings.Add("company", "company");
                bc.ColumnMappings.Add("city", "city");
                bc.ColumnMappings.Add("state", "state");
                bc.ColumnMappings.Add("subscriber_id", "subscriber_id");
                bc.ColumnMappings.Add("country", "country");
                bc.ColumnMappings.Add("country_name", "country_name");
                bc.ColumnMappings.Add("country_abbreviation", "country_abbreviation");
                bc.ColumnMappings.Add("latitude", "latitude");
                bc.ColumnMappings.Add("longitude", "longitude");
                bc.ColumnMappings.Add("verified", "verified");
                bc.ColumnMappings.Add("zip_code", "zip_code");
                bc.WriteToServer(dt);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Address", "GetList");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
        public static bool Save(List<Entity.Address> list)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.Address>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());
            SqlBulkCopy bc = default(SqlBulkCopy);
            try
            {
                conn.Open();
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "Address");
                bc.BatchSize = 0;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("address_id", "address_id");
                bc.ColumnMappings.Add("account_id", "account_id");
                bc.ColumnMappings.Add("first_name", "first_name");
                bc.ColumnMappings.Add("last_name", "last_name");
                bc.ColumnMappings.Add("address", "address");
                bc.ColumnMappings.Add("address_line_2", "address_line_2");
                bc.ColumnMappings.Add("company", "company");
                bc.ColumnMappings.Add("city", "city");
                bc.ColumnMappings.Add("state", "state");
                bc.ColumnMappings.Add("subscriber_id", "subscriber_id");
                bc.ColumnMappings.Add("country", "country");
                bc.ColumnMappings.Add("country_name", "country_name");
                bc.ColumnMappings.Add("country_abbreviation", "country_abbreviation");
                bc.ColumnMappings.Add("latitude", "latitude");
                bc.ColumnMappings.Add("longitude", "longitude");
                bc.ColumnMappings.Add("verified", "verified");
                bc.ColumnMappings.Add("zip_code", "zip_code");
                bc.WriteToServer(dt);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Address", "Save");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
        public static bool UpdateForACS(string xml)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Address_AddressUpdate";
                cmd.Parameters.AddWithValue("@xml", xml);
                cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Address", "UpdateForACS");
            }
            return success;
        }
    }
}
