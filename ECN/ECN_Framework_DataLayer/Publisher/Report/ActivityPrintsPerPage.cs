﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Publisher.Report
{
    [Serializable]
    public class ActivityPrintsPerPage
    {
        public static List<ECN_Framework_Entities.Publisher.Report.ActivityPrintsPerPage> GetList(int editionID, int blastID)
        {
            List<ECN_Framework_Entities.Publisher.Report.ActivityPrintsPerPage> retList = new List<ECN_Framework_Entities.Publisher.Report.ActivityPrintsPerPage>();

            string sqlQuery = "sp_GetActivity_PrintsPerPage";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EditionID", editionID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Report.ActivityPrintsPerPage>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Publisher.Report.ActivityPrintsPerPage x = builder.Build(rdr);
                        retList.Add(x);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
    }
}
