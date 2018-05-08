﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class ABSummaryReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.ABSummaryReport> Get(int CustomerID, DateTime startdate, DateTime enddate)
        {
            List<ECN_Framework_Entities.Activity.Report.ABSummaryReport> retList = new List<ECN_Framework_Entities.Activity.Report.ABSummaryReport>();
            string sqlQuery = "v_ABSummaryReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customerID", CustomerID);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.ABSummaryReport>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.ABSummaryReport x = builder.Build(rdr);
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

 