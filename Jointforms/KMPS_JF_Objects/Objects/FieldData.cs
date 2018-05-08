using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public class FieldData
    {
        public int PSFDID { get; set; }
        public int PSFieldID { get; set; }
        public string DataText { get; set; }
        public string DataValue { get; set; }
        public int SortOrder { get; set; }
        public bool ShowTextField { get; set; }            
        public bool ShowNoneoftheAbove { get; set; }    
        public bool IsNonQual { get; set; }
        public bool IsBranching { get; set; }  
        public string Category { get; set; }
        public bool IsDefault { get; set; }            

        public FieldData()
        {
            PSFDID = 0;
            PSFieldID = 0;
            DataText = string.Empty;
            DataValue = string.Empty;
            SortOrder = 0;
            ShowTextField = false;
            ShowNoneoftheAbove = false;
            Category = string.Empty;
            IsNonQual = false;
            IsDefault = false;
            IsBranching = false; 
        }

        public static List<FieldData> GetFieldData(int PSFieldID)
        {
            List<FieldData> fd = new List<FieldData>();

            SqlCommand cmd = new SqlCommand(string.Format("select * from PubSubscriptionFieldData  with (NOLOCK) where PSFieldID = {0} order by SortOrder", PSFieldID));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            DataTable dt = DataFunctions.GetDataTable(cmd);

            foreach (DataRow dr in dt.Rows)
            {
                FieldData data = new FieldData();
                data.PSFDID = Convert.ToInt32(dr["PSFDID"]);
                data.PSFieldID = Convert.ToInt32(dr["PSFieldID"]);
                data.DataText = dr["DataText"].ToString();
                data.DataValue = dr["DataValue"].ToString();
                data.SortOrder = Convert.ToInt32(dr["SortOrder"]);
                data.ShowTextField = Convert.ToBoolean(dr["ShowTextField"]);
                data.ShowNoneoftheAbove = Convert.ToBoolean(dr["ShowNoneoftheAbove"]);
                data.Category = dr.IsNull("Category") || (!dr.IsNull("Category") && dr["Category"].ToString().ToUpper() == "NULL") ? string.Empty : dr["Category"].ToString();
                data.IsDefault = dr.IsNull("IsDefault") ? false : Convert.ToBoolean(dr["IsDefault"]);  
                
                fd.Add(data);
            }
            
            return fd;
        }
    }
}
