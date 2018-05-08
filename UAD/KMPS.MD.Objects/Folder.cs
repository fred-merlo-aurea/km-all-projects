using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class Folder
    {
        public Folder() { }
        #region Properties
        public int FolderID { get; set; }
        public string Foldername { get; set; }
        #endregion

        #region Data
        public static List<Folder> GetByCustomerID(int customerID)
        {
            List<Folder> retList = new List<Folder>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ecnCommunicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT foldername, folderID FROM [FOLDER]  WHERE customerID = @CustomerID and foldertype='grp' and isDeleted = 0  order by foldername asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Folder> builder = DynamicBuilder<Folder>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Folder x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }
        #endregion
    }
}
