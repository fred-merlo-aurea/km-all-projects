using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameworkUAD.Object;
using KM.Common;
using KMPlatform.Object;

namespace FrameworkUAD.DataAccess
{
    public class FilterMVC
    {
        #region Data
        public static Object.FilterMVC ExecuteFilter(KMPlatform.Object.ClientConnections clientconnection,string filterQuery, Object.FilterMVC filter)
        {
            filter.Subscribers = new List<Object.Subscriber>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(filterQuery, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Object.Subscriber c = new Object.Subscriber();
                    c.SubscriptionID = Convert.ToInt32(rdr["SubscriptionID"]);
                    filter.Subscribers.Add(c);
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

            filter.Count = filter.Subscribers.Count();

            var query = filter.Subscribers
                        .GroupBy(x => new { x.SubscriptionID })
                        .Select(g => g.First());
            filter.Subscribers = query.ToList();
            filter.Executed = true;
            return filter;
        }

        public static Object.FilterMVC GetFilterByID(ClientConnections clientconnection, int filterID)
        {
            var retItem = new Object.FilterMVC();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from Filters where FilterId="+ filterID, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                retItem = Get(cmd);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retItem;
        }
        private static Object.FilterMVC Get(SqlCommand cmd)
        {
            Object.FilterMVC retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.FilterMVC();
                        DynamicBuilder<Object.FilterMVC> builder = DynamicBuilder<Object.FilterMVC>.CreateBuilder(rdr);
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
        public static Object.FilterMVC GetCounts(KMPlatform.Object.ClientConnections clientconnection,string filterQuery, Object.FilterMVC filter)
        {
            filter.Subscribers = new List<Object.Subscriber>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(filterQuery, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                filter.Count = int.Parse(cmd.ExecuteScalar().ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            filter.Executed = true;
            return filter;
        }
        public static Object.FilterMVC Execute(KMPlatform.Object.ClientConnections clientconnection, Object.FilterMVC filter, string filterQuery)
        {
            filter.Subscribers = new List<Object.Subscriber>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(filterQuery);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            filter.Count = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, conn.ConnectionString));

            filter.Executed = true;

            return filter;
        }
        public static  List<int> getSubscriber(KMPlatform.Object.ClientConnections clientconnection,Object.FilterMVC f, string filterQuery)
        {
            List<int> Subscriber = new List<int>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(filterQuery, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Subscriber.Add(Convert.ToInt32(rdr["SubscriptionID"]));
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

            var query = Subscriber
                        .GroupBy(x => new { x })
                        .Select(g => g.First());

            Subscriber = query.ToList();

            return Subscriber;
        }
        #endregion
    }
}
