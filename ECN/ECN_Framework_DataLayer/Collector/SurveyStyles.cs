using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Collector
{
    [Serializable]
    public class SurveyStyles
    {
        public static ECN_Framework_Entities.Collector.SurveyStyles GetBySurveyID(int SurveyID, int p)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM SurveyStyles WHERE SurveyID = @SurveyID";
            cmd.Parameters.AddWithValue("@SurveyID", SurveyID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Collector.SurveyStyles Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Collector.SurveyStyles retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Collector.SurveyStyles();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.SurveyStyles>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Collector.SurveyStyles> GetList(SqlCommand cmd, int customerID)
        {
            List<ECN_Framework_Entities.Collector.SurveyStyles> retList = new List<ECN_Framework_Entities.Collector.SurveyStyles>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Collector.SurveyStyles retItem = new ECN_Framework_Entities.Collector.SurveyStyles();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.SurveyStyles>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        retList.Add(retItem);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static void Save(ECN_Framework_Entities.Collector.SurveyStyles item)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SurveyStyles_Save";
            cmd.Parameters.Add(new SqlParameter("@SurveyID", (object)item.SurveyID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pWidth", (object)item.pWidth ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pbgcolor", (object)item.pbgcolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pAlign", (object)item.pAlign ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pBorder", (object)item.pBorder ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pBordercolor", (object)item.pBordercolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pfontfamily", (object)item.pfontfamily ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@hImage", (object)item.hImage ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@hAlign", (object)item.hAlign ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@hMargin", (object)item.hMargin ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@hbgcolor", (object)item.hbgcolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@phbgcolor", (object)item.phbgcolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@phfontsize", (object)item.phfontsize ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@phcolor", (object)item.phcolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@phBold", (object)item.phBold ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pdbgcolor", (object)item.pdbgcolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pdfontsize", (object)item.pdfontsize ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pdcolor", (object)item.pdcolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@pdbold", (object)item.pdbold ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@bbgcolor", (object)item.bbgcolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@qcolor", (object)item.qcolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@qfontsize", (object)item.qfontsize ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@qbold", (object)item.qbold ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@acolor", (object)item.acolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@abold", (object)item.abold ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@afontsize", (object)item.afontsize ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@fImage", (object)item.fImage ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@fAlign", (object)item.fAlign ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@fMargin", (object)item.fMargin ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@fbgcolor", (object)item.fbgcolor ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ShowQuestionNo", (object)item.ShowQuestionNo ?? DBNull.Value));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }
    }
}
