using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.Model
{
    [Serializable]
    [DataContract]
    public class Field
    {
        public Field() { }
        public Field(int _id, string _dataTable, string _tablePrefix, string _columnName, string _dataType, bool _isDemographic, bool _isDemographicOther,
            bool _isMultiSelect, int _clientId, string _uxControl)
        {
            Id = _id;
            DataTable = _dataTable;
            TablePrefix = _tablePrefix;
            ColumnName = _columnName;
            DataType = _dataType;
            IsDemographic = _isDemographic;
            IsDemographicOther = _isDemographicOther;
            IsMultiSelect = _isMultiSelect;
            ClientId = _clientId;
            UxControl = _uxControl;
        }

        #region Properties
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string DataTable { get; set; }
        [DataMember]
        public string TablePrefix { get; set; }
        [DataMember]
        public string ColumnName { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public bool IsDemographic { get; set; }
        [DataMember]
        public bool IsDemographicOther { get; set; }
        [DataMember]
        public bool IsMultiSelect { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public string UxControl { get; set; }

        #endregion

        public FrameworkUAD_Lookup.Enums.FieldDataType dataType
        {
            get
            {
                return FrameworkUAD_Lookup.Enums.GetFieldDataType(DataType);
            }
        }

        public FrameworkUAD_Lookup.Enums.FieldType FieldType
        {
            get
            {
                if (!this.IsDemographic &&
                    !this.IsDemographicOther &&
                    !this.IsMultiSelect)
                    return FrameworkUAD_Lookup.Enums.FieldType.Profile;
                else if ((this.IsDemographic || this.IsDemographicOther) &&
                          this.IsMultiSelect)
                    return FrameworkUAD_Lookup.Enums.FieldType.Demo;
                else if (!this.IsDemographic &&
                         !this.IsDemographicOther &&
                         this.DataTable.Equals("PubSubscriptions", StringComparison.CurrentCultureIgnoreCase) &&
                         this.ColumnName.Equals("RegionCode", StringComparison.CurrentCultureIgnoreCase))
                    return FrameworkUAD_Lookup.Enums.FieldType.Lookup_State;
                else if (!this.IsDemographic &&
                         !this.IsDemographicOther &&
                         this.DataTable.Equals("PubSubscriptions", StringComparison.CurrentCultureIgnoreCase) &&
                         (this.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase) || this.ColumnName.Equals("CountryId", StringComparison.CurrentCultureIgnoreCase)))
                    return FrameworkUAD_Lookup.Enums.FieldType.Lookup_Country;
                else if (!this.IsDemographic &&
                    !this.IsDemographicOther &&
                    this.IsMultiSelect)
                    return FrameworkUAD_Lookup.Enums.FieldType.Lookup_Code;
                else if (!this.IsDemographic &&
                    !this.IsDemographicOther &&
                    this.IsMultiSelect &&
                    this.ColumnName.Equals("PUBTRANSACTIONID", StringComparison.CurrentCultureIgnoreCase))
                    return FrameworkUAD_Lookup.Enums.FieldType.Lookup_Transaction;
                else if (!this.IsDemographic &&
                    !this.IsDemographicOther &&
                    this.IsMultiSelect &&
                    this.ColumnName.Equals("PUBCATEGORYID", StringComparison.CurrentCultureIgnoreCase))
                    return FrameworkUAD_Lookup.Enums.FieldType.Lookup_Category;
                else
                    return FrameworkUAD_Lookup.Enums.FieldType.Custom;
            }
        }


    }
}
