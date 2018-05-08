using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Core_AMS.Utilities;
using KM.Common.Data;
using KM.Common.Functions;
using KMCommon = KM.Common;

namespace FrameworkUAD_Lookup.DataAccess
{
    public class DataFunctions
    {
        public static List<T> GetList<T>(SqlCommand cmd)
        {
            var list = KMCommon.DataFunctions.GetList<T>(cmd, ConnectionString.UAD_Lookup.ToString());
            return list;
        }
    }
}
