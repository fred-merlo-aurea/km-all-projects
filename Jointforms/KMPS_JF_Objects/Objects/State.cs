using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using System.Web;
using KM.Common;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public class State
    {
        public int CountryID { get; set; }
        public string StateName { get; set; }
        public string StateAbbr { get; set; }

        public State()
        {
            CountryID = 0;
            StateName = string.Empty;
            StateAbbr = string.Empty;
        }

        public static List<State> GetStates()
        {
            List<State> states = new List<State>();

            if (CacheUtil.IsCacheEnabled())
            {
                states = (List<State>)CacheUtil.GetFromCache("STATES", "JOINTFORMS");

                if (states == null)
                {
                    states = GetData();
                    CacheUtil.AddToCache("STATES", states, "JOINTFORMS");
                }

                return states;
            }
            else
            {
                return GetData();
            }
        }

        private static List<State> GetData()
        {
            List<State> states = new List<State>();

            SqlCommand cmd = new SqlCommand("select state, stateabbr, countryID from state  with (NOLOCK)  order by countryID, state asc");
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            SqlDataReader dr = DataFunctions.ExecuteReader(cmd);

            while (dr.Read())
            {
                State s = new State();
                s.CountryID = Convert.ToInt32(dr["countryID"]);
                s.StateName = dr["state"].ToString();
                s.StateAbbr = dr["stateabbr"].ToString();
                states.Add(s);
            }

            return states;
        }

        public static List<State> GetStates(int CountryID)
        {
            List<State> states = GetStates();

            return (from s in states
                    where s.CountryID == CountryID
                    orderby s.StateName
                    select s).ToList(); ;

        }
    }
}
