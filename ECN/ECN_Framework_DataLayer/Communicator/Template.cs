using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Template
    {
        public static bool Exists(int templateID, int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 TemplateID from Template where TemplateID = @TemplateID AND BaseChannelID = @BaseChannelID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@TemplateID", templateID);
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.Template GetByTemplateID(int templateID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Template WHERE TemplateID = @TemplateID AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@TemplateID", templateID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Template> GetByStyleCode(int baseChannelID, string styleCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Template WHERE BaseChannelID = @BaseChannelID AND TemplateStyleCode = @TemplateStyleCode AND IsDeleted = 0 and IsActive=1";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@TemplateStyleCode", styleCode);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Template> GetByBaseChannelID(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Template WHERE BaseChannelID = @BaseChannelID AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.Template Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Template retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Template();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Template>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.Template> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Template> retList = new List<ECN_Framework_Entities.Communicator.Template>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Template retItem = new ECN_Framework_Entities.Communicator.Template();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Template>.CreateBuilder(rdr);
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
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static int Save(ECN_Framework_Entities.Communicator.Template template)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Template_Save";
            cmd.Parameters.Add(new SqlParameter("@TemplateID", (object)template.TemplateID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", template.BaseChannelID));
            cmd.Parameters.Add(new SqlParameter("@TemplateStyleCode", template.TemplateStyleCode));
            cmd.Parameters.Add(new SqlParameter("@TemplateName", template.TemplateName));
            cmd.Parameters.Add(new SqlParameter("@TemplateImage", template.TemplateImage));
            cmd.Parameters.Add(new SqlParameter("@TemplateDescription", template.TemplateDescription));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", template.SortOrder));
            cmd.Parameters.Add(new SqlParameter("@SlotsTotal", template.SlotsTotal));
            cmd.Parameters.Add(new SqlParameter("@TemplateSource", template.TemplateSource));
            cmd.Parameters.Add(new SqlParameter("@TemplateText", template.TemplateText));
            cmd.Parameters.Add(new SqlParameter("@TemplateSubject", template.TemplateSubject));
            cmd.Parameters.Add(new SqlParameter("@IsActive", template.IsActive));
            cmd.Parameters.Add(new SqlParameter("@Category", template.Category));
            if (template.TemplateID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)template.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)template.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int templateID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Template_Delete";
            cmd.Parameters.AddWithValue("@TemplateID", templateID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static ECN_Framework_Entities.Communicator.Template GetByNumberOfSlots(int numberOfSlots, int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT Top 1 * FROM Template WHERE BaseChannelID = @BaseChannelID AND SlotsTotal = @NumberOfSlots AND IsDeleted = 0 ORDER BY CreatedDate DESC";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@NumberOfSlots", numberOfSlots);
            return Get(cmd);
        }
    }
}
