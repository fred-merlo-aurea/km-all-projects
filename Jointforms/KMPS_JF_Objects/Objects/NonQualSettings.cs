using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;   

namespace KMPS_JF_Objects.Objects
{
    public class NonQualSettings
    {
        public int NonQualID { get; set; }  
        public int PFFieldID { get; set; }
        public string DataValue { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public NonQualSettings()
        {
            this.NonQualID = -1;
            this.PFFieldID = -1;
            this.DataValue = string.Empty;
            this.DateAdded = null;
            this.DateModified = null;
            this.AddedBy = string.Empty;
            this.ModifiedBy = string.Empty; 
        }

        public static List<NonQualSettings> GetNonQualSettings(int PSFieldID, int PFID) 
        {
            SqlCommand cmdFieldSettings = new SqlCommand("spGetDataValuesForNonQualSettings");    
            cmdFieldSettings.CommandType = CommandType.StoredProcedure;
            cmdFieldSettings.Parameters.AddWithValue("@PSFieldID", PSFieldID);      
            cmdFieldSettings.Parameters.AddWithValue("@PFID", PFID);
            DataTable dtFieldSettings = DataFunctions.GetDataTable(cmdFieldSettings);

            List<NonQualSettings> fsList = new List<NonQualSettings>();

            foreach (DataRow dr in dtFieldSettings.Rows)
            {
                NonQualSettings nqs = new NonQualSettings();               
                nqs.DataValue = dr["Datavalue"].ToString();
                fsList.Add(nqs);
            }

            return fsList;  
        }
    }
}
