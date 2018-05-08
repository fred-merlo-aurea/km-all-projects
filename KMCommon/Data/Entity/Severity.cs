using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace KM.Common.Entity
{
    [Serializable]
    [DataContract]
    public class Severity
    {
        public Severity() { }
        #region Properties
        [DataMember]
        public int SeverityID { get; set; }
        [DataMember]
        public string SeverityName { get; set; }
        [DataMember]
        public string SeverityDescription { get; set; }
        #endregion

        #region Data
        public static List<Severity> Select()
        {
            List<Severity> retList = new List<Severity>();
            string sqlQuery = "e_Severity_Select";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader rdr = DataFunctions.ExecuteReader(cmd);

            var builder = DynamicBuilder<Severity>.CreateBuilder(rdr);
            while (rdr.Read())
            {
                Severity x = builder.Build(rdr);
                retList.Add(x);
            }

            cmd.Connection.Close();
            return retList;
        }
        public static Severity Get(SeverityLevel sl)
        {
            return Select().SingleOrDefault(x => x.SeverityName.Equals(sl.ToString()));
        }
        #endregion

        public enum SeverityLevel
        {
            Critical = 1,
            Non_Critical = 2
        }
    }
}
