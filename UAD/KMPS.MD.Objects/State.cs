using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class State
    {
        public State() { }

        #region Properties
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string  state_desc { get; set; }
        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public string region { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public int sort_order { get; set; }
        [DataMember]
        public int country_sort_order { get; set; }
        #endregion

        #region Data
        public static List<State> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<State> states = (List<State>)CacheUtil.GetFromCache("STATE", DatabaseName);

                if (states == null)
                {
                    states = GetData(clientconnection);

                    CacheUtil.AddToCache("STATE", states, DatabaseName);
                }

                return states;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<State> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<State> retList = new List<State>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select state, State_desc, country from State order by State asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<State> builder = DynamicBuilder<State>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    State x = builder.Build(rdr);

                    retList.Add(x);
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
            return retList;
        }

        public static List<State> GetByCountry(KMPlatform.Object.ClientConnections clientconnection, string country)
        {
            return State.GetAll(clientconnection).FindAll(x => (x.country ?? null) == country);
        }

        public static List<State> GetRegion(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<State> retList = new List<State>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select distinct region from State where region is not null order by region asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<State> builder = DynamicBuilder<State>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    State x = builder.Build(rdr);

                    retList.Add(x);
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
            return retList;
        }

        public static State GetCountryByState(KMPlatform.Object.ClientConnections clientconnection, string state)
        {
            State retItem = new State();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from State where  state = @state", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@state", state));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<State> builder = DynamicBuilder<State>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
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
            return retItem;
        }
        #endregion
    }
}
