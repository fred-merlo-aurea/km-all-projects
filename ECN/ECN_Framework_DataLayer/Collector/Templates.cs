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
    public class Templates
    {
        public static List<ECN_Framework_Entities.Collector.Templates> GetByCustomerID(int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Templates WHERE CustomerID = @CustomerID and IsActive=1";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd, customerID);
        }

        private static ECN_Framework_Entities.Collector.Templates Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Collector.Templates retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Collector.Templates();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.Templates>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Collector.Templates> GetList(SqlCommand cmd, int customerID)
        {
            List<ECN_Framework_Entities.Collector.Templates> retList = new List<ECN_Framework_Entities.Collector.Templates>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Collector.Templates retItem = new ECN_Framework_Entities.Collector.Templates();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.Templates>.CreateBuilder(rdr);
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
        
        public static bool Exists(string TemplateName, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 TemplateID FROM Templates WITH (NOLOCK) " +
                              "WHERE CustomerID = @CustomerID and TemplateName=@TemplateName) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@TemplateName", TemplateName);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Collector.Templates GetByTemplateID(int TemplateID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Templates with (NOLOCK) WHERE TemplateID = @TemplateID and IsActive=1";
            cmd.Parameters.AddWithValue("@TemplateID", TemplateID);
            return Get(cmd);
        }

        public static int Save(ECN_Framework_Entities.Collector.Templates item, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Templates_Save";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)item.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TemplateName", (object)item.TemplateName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TemplateImage", (object)item.TemplateImage ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsDefault", (object)item.IsDefault ?? DBNull.Value));
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
            cmd.Parameters.Add(new SqlParameter("@IsActive", (object)item.IsActive ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString()).ToString());
        }
    }
}
