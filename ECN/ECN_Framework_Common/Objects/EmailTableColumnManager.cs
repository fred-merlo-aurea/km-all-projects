using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace ECN_Framework_Common.Objects
{
    public class EmailTableColumnManager
    {
        protected SortedList _columns;

        public EmailTableColumnManager()
        {
            _columns = new SortedList(60);
            _columns.Add(0, "Ignore");
            _columns.Add(1, "EmailAddress");
            _columns.Add(2, "FormatTypeCode");
            _columns.Add(3, "SubscribeTypeCode");
            _columns.Add(4, "Title");
            _columns.Add(5, "FirstName");
            _columns.Add(6, "LastName");
            _columns.Add(7, "FullName");
            _columns.Add(8, "Company");
            _columns.Add(9, "Occupation");
            _columns.Add(10, "Address");
            _columns.Add(11, "Address2");
            _columns.Add(12, "City");
            _columns.Add(13, "State");
            _columns.Add(14, "Zip");
            _columns.Add(15, "Country");
            _columns.Add(16, "Voice");
            _columns.Add(17, "Mobile");
            _columns.Add(18, "Fax");
            _columns.Add(19, "Website");
            _columns.Add(20, "Age");
            _columns.Add(21, "Income");
            _columns.Add(22, "Gender");
            _columns.Add(23, "User1");
            _columns.Add(24, "User2");
            _columns.Add(25, "User3");
            _columns.Add(26, "User4");
            _columns.Add(27, "User5");
            _columns.Add(28, "User6");
            _columns.Add(29, "Birthdate");
            _columns.Add(30, "UserEvent1");
            _columns.Add(31, "UserEvent1Date");
            _columns.Add(32, "UserEvent2");
            _columns.Add(33, "UserEvent2Date");
            _columns.Add(34, "Notes");
            _columns.Add(35, "Password");
            _columns.Add(36, "SMSEnabled");
        }

        public int ColumnCount
        {
            get
            {
                return _columns.Count;
            }
        }

        public string GetColumnNameByIndex(int index)
        {
            return Convert.ToString(_columns[index]);
        }

        public SqlDbType GetSqlDbTypeByIndex(int index)
        {
            return GetSqlDbTypeByName(GetColumnNameByIndex(index));
        }

        public SqlDbType GetSqlDbTypeByName(string columnName)
        {
            if (columnName == "Notes")
            {
                return SqlDbType.Text;
            }

            if (columnName == "Birthdate" || columnName == "UserEvent1Date" || columnName == "UserEvent2Date")
            {
                return SqlDbType.DateTime;
            }
            return SqlDbType.VarChar;
        }


        public int GetColumnLengthByIndex(int index)
        {
            return GetColumnLengthByName(GetColumnNameByIndex(index));
        }

        public int GetColumnLengthByName(string columnName)
        {
            if (columnName == "EmailAddress" || columnName == "Address" || columnName == "Address2")
            {
                return 255;
            }
            if ((columnName.Length == 5 && columnName.StartsWith("User")) || (columnName.ToLower().StartsWith("user_")))
            {
                return 255;
            }
            if (columnName == "Company")
            {
                return 100;
            }
            if (columnName == "Birthdate" || columnName == "UserEvent1Date" || columnName == "UserEvent2Date")
            {
                return 8;
            }
            if (columnName == "Notes")
            {
                return 16;
            }
            return 50;
        }

        public static bool BelongToEmailGroupTable(string columnName)
        {
            if (columnName == "FormatTypeCode" || columnName == "SubscribeTypeCode" || columnName == "SMSEnabled")
            {
                return true;
            }
            return false;
        }

        public static bool IsUserDefinedField(string columnName)
        {
            return columnName.StartsWith("user_");
        }

        public void AddGroupDataFields(string ShortName)
        {
            int i = ColumnCount;
            _columns.Add(i++, string.Format("user_{0}", ShortName));
        }
    }

}