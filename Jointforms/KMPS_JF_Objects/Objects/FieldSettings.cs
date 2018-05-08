using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;  

namespace KMPS_JF_Objects.Objects
{
    public class FieldsSetting
    {
        public int FieldSettingID { get; set; }
        public int PFFFieldID { get; set; }
        public int PFFReferenceID { get; set; }
        public string DataValue { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateModified { get; set; }    
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public FieldsSetting() 
        {
            this.FieldSettingID = -1;
            this.PFFFieldID = -1;
            this.PFFReferenceID = -1; 
            this.DataValue = string.Empty;
            this.DateAdded = null; 
            this.DateModified = null;
            this.AddedBy = string.Empty;
            this.ModifiedBy = string.Empty;   
        }              
     
        public static List<FieldsSetting> GetFieldSettings(int PSFieldID, int PFID) 
        {
            SqlCommand cmdFieldSettings = new SqlCommand("spGetDataValuesForFieldSettings");  
            cmdFieldSettings.CommandType = CommandType.StoredProcedure;
            cmdFieldSettings.Parameters.AddWithValue("@PSFieldID", PSFieldID);    
            cmdFieldSettings.Parameters.AddWithValue("@PFID", PFID);
            DataTable dtFieldSettings = DataFunctions.GetDataTable(cmdFieldSettings);

            List<FieldsSetting> fsList = new List<FieldsSetting>();

            foreach (DataRow dr in dtFieldSettings.Rows)
            {
                FieldsSetting fs = new FieldsSetting();                      
                fs.DataValue = dr["Datavalue"].ToString();
                fsList.Add(fs);  
            }

            return fsList;     
        }
    }
}
