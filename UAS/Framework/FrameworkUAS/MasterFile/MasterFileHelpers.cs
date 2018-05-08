using System.Collections.Generic;
using System.Data.SqlClient;

using FrameworkUAS.DataAccess;
using KM.Common;

namespace FrameworkUAS.MasterFile
{
    public static class MasterFileHelpers
    {
        public static List<Publication> ReadPublicationsList(SqlCommand cmd, bool setResponseTypeList)
        {
            if (cmd == null)
            {
                throw new System.ArgumentNullException(nameof(cmd));
            }

            var retList = new List<Publication>();

            using (var rdr = ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<Publication>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        var retItem = builder.Build(rdr);
                        if (setResponseTypeList)
                        {
                            retItem.ResponseTypeList = new List<ResponseType>();
                        }
                        
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                }
            }
            return retList;
        }

        private static SqlDataReader ExecuteReader(SqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new System.ArgumentNullException(nameof(cmd));
            }

            cmd.CommandTimeout = 0;
            cmd.Connection.Open();

            var rdr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (rdr != null && !rdr.HasRows)
            {
                rdr = null;
            }

            return rdr;
        }
    }
}
