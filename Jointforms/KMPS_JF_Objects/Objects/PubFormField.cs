using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public enum FieldGroup
    {
        Profile,
        Demographic
    }

    [Serializable]
    public enum ControlType
    {
        Radio,
        Dropdown,
        TextBox,
        Checkbox,
        CatCheckbox,
        Hidden
    }

    [Serializable]
    public enum DataType
    {
        Text,
        Number,
        Currency,
        Date,
        Zip,
        Phone
    }

    [Serializable]
    public enum NotificatonFor
    {
        Print,
        Digital,
        Both,
        Other,
        NQ,
        Newsletter,
        Cancel
    }

    [Serializable]
    public class PubFormField
    {
        public int PFFieldID { get; set; }
        public int PFID { get; set; }
        public int PSFieldID { get; set; }
        public int SortOrder { get; set; }
        public bool AddSeparator { get; set; }
        public string SeparatorType { get; set; }

        public string ECNFieldName { get; set; }
        public string ECNCombinedFieldName { get; set; }
        public string DisplayName { get; set; }
        public FieldGroup Grouping { get; set; }
        public DataType DataType { get; set; }
        public ControlType ControlType { get; set; }
        public string QueryStringName { get; set; }
        public int MaxLength { get; set; }
        public bool Required { get; set; }
        public bool IsVisible { get; set; }
        public bool AutoPostBack { get; set; }
        public bool NonQualExists { get; set; }
        public string ValidationType { get; set; }
        public bool ShowTextField { get; set; }
        public string ECNTextFieldName { get; set; }
        public bool ShowNoneoftheAbove { get; set; }
        public bool IsActive { get; set; }
        public int ColumnFormat { get; set; }
        public string RepeatDirection { get; set; }
        public bool IsPrepopulate { get; set; }
        public string DefaultValue { get; set; }
        public int MaxSelections { get; set; }

        public List<FieldData> Data;

        public PubFormField()
        {
            PFFieldID = 0;
            PFID = 0;
            PSFieldID = 0;
            SortOrder = 0;
            AddSeparator = false;
            SeparatorType = string.Empty;
            ECNFieldName = string.Empty;
            DisplayName = string.Empty;
            Grouping = FieldGroup.Profile;
            DataType = DataType.Text;
            ControlType = ControlType.TextBox;
            QueryStringName = string.Empty;
            MaxLength = 0;
            Required = false;
            IsVisible = true;
            AutoPostBack = false;
            ValidationType = string.Empty;
            ShowTextField = false;
            ECNTextFieldName = string.Empty;
            ShowNoneoftheAbove = false;
            IsActive = false;
            ColumnFormat = 1;
            RepeatDirection = "VER";
            IsPrepopulate = false;
            DefaultValue = string.Empty;
            MaxSelections = 0;
        }

        public static List<PubFormField> GetPubFormFields(int PFID)
        {
            List<PubFormField> fields = new List<PubFormField>();

            try
            {
                SqlCommand cmd = new SqlCommand(string.Format("select pff.PFFieldID, pff.SortOrder, pff.AddSeparator, pff.SeparatorType, psf.*, case when exists (select top 1 fieldsettingiD from  Fieldsettings fs  with (NOLOCK) where fs.pffieldID = pff.PFFieldID) THEN 0 else 1 end as IsVisible, case when exists (select top 1 fieldsettingiD from fieldsettings where PFFReferenceID = pff.PFFieldID)  THEN 1 else 0 END as AutoPostBack, case when exists (select top 1 nq.PFFieldID from NonQualSettings nq where nq.PFFieldID = pff.PFFieldID)  THEN 1 else 0 END as NonQualExists from PubFormFields pff  with (NOLOCK) join PubSubscriptionFields psf  with (NOLOCK) on pff.PSFieldID = psf.PSFieldID where pff.PFID = {0}  order by grouping desc, sortorder", PFID));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 0;

                DataTable dt = DataFunctions.GetDataTable(cmd);

                foreach (DataRow dr in dt.Rows)
                {
                    PubFormField field = new PubFormField();

                    field.PFFieldID = Convert.ToInt32(dr["PFFieldID"]);
                    field.PFID = PFID;
                    field.PSFieldID = Convert.ToInt32(dr["PSFieldID"]);
                    field.SortOrder = Convert.ToInt32(dr["SortOrder"]);
                    field.AddSeparator = Convert.ToBoolean(dr["AddSeparator"]);
                    field.SeparatorType = dr["SeparatorType"].ToString();

                    field.ECNFieldName = dr["ECNFieldName"].ToString();
                    field.ECNCombinedFieldName = dr.IsNull("ECNCombinedFieldName") ? string.Empty : dr["ECNCombinedFieldName"].ToString();

                    field.DisplayName = dr["DisplayName"].ToString();
                    field.Grouping = dr["Grouping"].ToString().ToUpper().Equals("P") ? FieldGroup.Profile : FieldGroup.Demographic;
                    field.DataType = (DataType)Enum.Parse(typeof(DataType), dr["DataType"].ToString(), true);
                    field.ControlType = (ControlType)Enum.Parse(typeof(ControlType), dr["ControlType"].ToString(), true);
                    field.MaxLength = Convert.ToInt32(dr["MaxLength"]);
                    field.Required = dr.IsNull("Required") ? false : (dr["Required"].ToString().Equals("Y", StringComparison.OrdinalIgnoreCase) ? true : false);

                    field.IsVisible = dr.IsNull("IsVisible") ? true : Convert.ToBoolean(dr["IsVisible"]);
                    field.AutoPostBack = dr.IsNull("AutoPostBack") ? false : Convert.ToBoolean(dr["AutoPostBack"]);
                    field.NonQualExists = dr.IsNull("NonQualExists") ? false : Convert.ToBoolean(dr["NonQualExists"]);
                    field.ValidationType = dr["ValidationType"].ToString();
                    field.ShowTextField = Convert.ToBoolean(dr["ShowTextField"]);
                    field.ShowNoneoftheAbove = Convert.ToBoolean(dr["ShowNoneoftheAbove"]);
                    field.IsActive = Convert.ToBoolean(dr["IsActive"]);

                    field.ECNTextFieldName = dr.IsNull("ECNTextFieldName") ? string.Empty : dr["ECNTextFieldName"].ToString();
                    field.QueryStringName = dr.IsNull("QueryStringName") ? string.Empty : dr["QueryStringName"].ToString();

                    field.RepeatDirection = dr.IsNull("RepeatDirection") ? "VER" : dr["RepeatDirection"].ToString();
                    field.ColumnFormat = dr.IsNull("ColumnFormat") ? 1 : Convert.ToInt32(dr["ColumnFormat"].ToString());
                    field.IsPrepopulate = dr.IsNull("Prepopulate") ? true : Convert.ToBoolean(dr["Prepopulate"].ToString());

                    field.DefaultValue = dr.IsNull("DefaultValue") ? string.Empty : dr["DefaultValue"].ToString();
                    field.MaxSelections = dr.IsNull("MaxSelections") ? 0 : Convert.ToInt32(dr["MaxSelections"].ToString());
                    field.Data = FieldData.GetFieldData(field.PSFieldID);

                    if (field.NonQualExists)
                    {
                        DataTable dtNQ = DataFunctions.GetDataTable(string.Format("select DataValue from PubFormFields pff  with (NOLOCK) join NonQualSettings nq on pff.PFFieldID = nq.PFFieldID where pff.PFFieldID = {0}", field.PFFieldID));

                        foreach (DataRow drNQ in dtNQ.Rows)
                        {
                            try
                            {
                                FieldData fd = (FieldData)field.Data.SingleOrDefault(f => f.DataValue == drNQ["DataValue"].ToString());
                                fd.IsNonQual = true;
                            }
                            catch { }
                        }
                    }

                    fields.Add(field);

                }
                return fields;
            }
            catch 
            {
                return fields;
            }
        }
    }
}
