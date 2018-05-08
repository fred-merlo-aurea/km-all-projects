using System;
using System.Collections;

namespace ECN_Framework_Common.Objects
{
    [Serializable]
    public class ImportMapper
    {
        Hashtable _mappings;
        public ImportMapper()
        {
            _mappings = new Hashtable();
        }

        public int MappingCount
        {
            get { return _mappings.Count; }
        }

        public bool IsIgnored(int index)
        {
            if (_mappings[index] == null)
            {
                return true;
            }
            return false;
        }

        public bool HasEmailAddress
        {
            get
            {
                foreach (string colName in _mappings.Values)
                {
                    if (colName == "EmailAddress")
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool HasMobileNumber
        {
            get
            {
                foreach (string colName in _mappings.Values)
                {
                    if (colName == "Mobile")
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool AddMapping(int index, string columnName)
        {
            foreach (string column in _mappings.Values)
            {
                if (column == columnName)
                {
                    return false;
                }
            }
            _mappings.Add(index, columnName);
            return true;
        }

        public string GetColumnName(int index)
        {
            return Convert.ToString(_mappings[index]);
        }
    }
}
