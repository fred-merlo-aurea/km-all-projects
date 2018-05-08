﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    public class BlastClickSummary
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastClickSummary> Get(int BlastID)
        {
            List<ECN_Framework_Entities.Activity.Report.BlastClickSummary> retList = new List<ECN_Framework_Entities.Activity.Report.BlastClickSummary>();
            string sqlQuery = "spBlastClicksSummary";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BlastID", BlastID);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.BlastClickSummary>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.BlastClickSummary x = builder.Build(rdr);
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
