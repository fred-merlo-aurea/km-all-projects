using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ecn.common.classes;
using ECN_Framework_BusinessLayer.Communicator;

namespace ecn.communicator.classes
{
    public class Filter : FilterBase
    {
        private const string ContainsComparator = "contains";
        private const string EqualsComparator = "equals";
        private const string ExpToday = "EXP:Today";

        #region Constructor
        public Filter()
        {
            _FilterID = null;
            _CustomerID = null;
            _UserID = null;
            _GroupID = null;
            _FilterName = "";
            _WhereClause = "";
            _DynamicWhere = "";
            _GroupCompareType = "";
            _FilterGroupList = null;
        }
        #endregion

        #region Private Properties
        private int? _FilterID;
        private int? _CustomerID;
        private int? _UserID;
        private int? _GroupID;
        private string _FilterName;
        private string _WhereClause;
        private string _DynamicWhere;
        private string _GroupCompareType;
        private DateTime? _CreatedDate;
        private List<FilterGroup> _FilterGroupList;
        #endregion

        #region Public Properties
        public int? FilterID
        {
            get
            {
                return _FilterID;
            }
        }
        public int? CustomerID
        {
            get
            {
                return _CustomerID;
            }
            set
            {
                _CustomerID = value;
            }
        }
        public int? UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }
        public int? GroupID
        {
            get
            {
                return _GroupID;
            }
            set
            {
                _GroupID = value;
            }
        }
        public string FilterName
        {
            get
            {
                return _FilterName;
            }
            set
            {
                _FilterName = value;
            }
        }
        public string WhereClause
        {
            get
            {
                return _WhereClause;
            }
            set
            {
                _WhereClause = value;
            }
        }
        public string DynamicWhere
        {
            get
            {
                return _DynamicWhere;
            }
            set
            {
                _DynamicWhere = value;
            }
        }
        public string GroupCompareType
        {
            get
            {
                return _GroupCompareType;
            }
            set
            {
                _GroupCompareType = value;
            }
        }
        public DateTime? CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                _CreatedDate = value;
            }
        }
        public List<FilterGroup> FilterGroupList
        {
            get
            {
                return _FilterGroupList;
            }
            set
            {
                _FilterGroupList = value;
            }
        }
        #endregion

        #region Public Static Methods
        public static List<Filter> GetFilterList(int groupID)
        {
            Filter filter = null;
            List<Filter> filterList = new List<Filter>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Filter WHERE GroupID = @GroupID ORDER BY FilterName ASC";
            cmd.Parameters.Add(new SqlParameter("@GroupID", groupID));

            SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    filter = new Filter();
                    if (reader["FilterID"] != DBNull.Value)
                    {
                        filter._FilterID = Convert.ToInt32(reader["FilterID"].ToString());
                    }
                    if (reader["CustomerID"] != DBNull.Value)
                    {
                        filter._CustomerID = Convert.ToInt32(reader["CustomerID"].ToString());
                    }
                    if (reader["UserID"] != DBNull.Value)
                    {
                        filter._UserID = Convert.ToInt32(reader["UserID"].ToString());
                    }
                    if (reader["GroupID"] != DBNull.Value)
                    {
                        filter._GroupID = Convert.ToInt32(reader["GroupID"].ToString());
                    }
                    if (reader["FilterName"] != DBNull.Value)
                    {
                        filter._FilterName = reader["FilterName"].ToString().Trim();
                    }
                    if (reader["WhereClause"] != DBNull.Value)
                    {
                        filter._WhereClause = reader["WhereClause"].ToString().Trim();
                    }
                    if (reader["DynamicWhere"] != DBNull.Value)
                    {
                        filter._DynamicWhere = reader["DynamicWhere"].ToString().Trim();
                    }
                    if (reader["GroupCompareType"] != DBNull.Value)
                    {
                        filter._GroupCompareType = reader["GroupCompareType"].ToString().Trim();
                    }
                    if (reader["CreatedDate"] != DBNull.Value)
                    {
                        filter._CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    }
                    filter._FilterGroupList = FilterGroup.GetFilterGroupList(Convert.ToInt32(filter.FilterID));
                    filterList.Add(filter);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return filterList;
        }

        public static Filter GetFilter(int filterID)
        {
            Filter filter = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Filter WHERE FilterID = @FilterID";
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));

            try
            {
                DataTable dt = DataFunctions.GetDataTable(cmd);
                if (dt != null && dt.Rows.Count == 1)
                {
                    filter = new Filter();
                    DataRow row = dt.Rows[0];
                    if (row["FilterID"] != DBNull.Value)
                    {
                        filter._FilterID = Convert.ToInt32(row["FilterID"].ToString());
                    }
                    if (row["CustomerID"] != DBNull.Value)
                    {
                        filter._CustomerID = Convert.ToInt32(row["CustomerID"].ToString());
                    }
                    if (row["UserID"] != DBNull.Value)
                    {
                        filter._UserID = Convert.ToInt32(row["UserID"].ToString());
                    }
                    if (row["GroupID"] != DBNull.Value)
                    {
                        filter._GroupID = Convert.ToInt32(row["GroupID"].ToString());
                    }
                    if (row["FilterName"] != DBNull.Value)
                    {
                        filter._FilterName = row["FilterName"].ToString().Trim();
                    }
                    if (row["WhereClause"] != DBNull.Value)
                    {
                        filter._WhereClause = row["WhereClause"].ToString().Trim();
                    }
                    if (row["DynamicWhere"] != DBNull.Value)
                    {
                        filter._DynamicWhere = row["DynamicWhere"].ToString().Trim();
                    }
                    if (row["GroupCompareType"] != DBNull.Value)
                    {
                        filter._GroupCompareType = row["GroupCompareType"].ToString().Trim();
                    }
                    if (row["CreatedDate"] != DBNull.Value)
                    {
                        filter._CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                    }
                    filter._FilterGroupList = FilterGroup.GetFilterGroupList(Convert.ToInt32(filter.FilterID));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return filter;
        }

        public static bool Save(ref Filter filter)
        {
            bool success = false;
            if (ValidateFilterObject(filter))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@FilterName", filter._FilterName.Trim());
                cmd.Parameters.AddWithValue("@CustomerID", filter._CustomerID);
                cmd.Parameters.AddWithValue("@UserID", filter._UserID);
                cmd.Parameters.AddWithValue("@GroupID", filter._GroupID);
                cmd.Parameters.AddWithValue("@WhereClause", CreateWhereClause(filter));
                cmd.Parameters.AddWithValue("@DynamicWhere", CreateDynamicWhere(filter));
                cmd.Parameters.AddWithValue("@GroupCompareType", filter.GroupCompareType);
                if (filter._FilterID == null)
                {
                    //add
                    cmd.CommandText = "insert into Filter (FilterName, CustomerID, UserID, GroupID, WhereClause, DynamicWhere, GroupCompareType, CreatedDate)" +
                                        " values (@FilterName, @CustomerID, @UserID, @GroupID, @WhereClause, @DynamicWhere, @GroupCompareType, GETDATE());SELECT @@IDENTITY ";
                    try
                    {
                        filter._FilterID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd).ToString());
                        success = true;
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    //update
                    cmd.Parameters.AddWithValue("@FilterID", filter._FilterID);
                    cmd.CommandText = "update Filter set FilterName = @FilterName, CustomerID = @CustomerID, UserID = @UserID," +
                        " GroupID = @GroupID, WhereClause = @WhereClause, DynamicWhere = @DynamicWhere, GroupCompareType = @GroupCompareType where FilterID = @FilterID";
                    try
                    {
                        if (DataFunctions.Execute(cmd) == 1)
                        {
                            success = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return success;
        }

        public static bool Delete(int filterID)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete FilterCondition where FilterGroupID in (select FilterGroupID from FilterGroup where FilterID = @FilterID);delete FilterGroup where FilterID = @FilterID;delete Filter where FilterID = @FilterID";
                cmd.Parameters.AddWithValue("@FilterID", filterID);
                DataFunctions.Execute(cmd);
                success = true;
            }
            catch (Exception)
            {
            }
            return success;
        }

        public static string CreateWhereClause(Filter filter)
        {
            StringBuilder where = new StringBuilder();
            string groupConnector = " " + filter._GroupCompareType + " ";

            int groupIterator = 1;
            if (filter.FilterGroupList != null)
            {
                foreach (FilterGroup group in filter.FilterGroupList)
                {
                    if (groupIterator > 1)
                    {
                        where.Append(groupConnector);
                    }
                    where.Append("(");

                    string conditionConnector = " " + group.ConditionCompareType + " ";
                    int conditionIterator = 1;
                    if (group.FilterConditionList != null)
                    {
                        foreach (FilterCondition condition in group.FilterConditionList)
                        {
                            if (conditionIterator > 1)
                            {
                                where.Append(conditionConnector);
                            }
                            where.Append("(");

                            where.Append(ConvertField(condition) + " ");// + condition.Comparator + " " + condition.CompareValue);

                            switch (condition.Comparator)
                            {
                                case "contains":
                                    if (condition.NotComparator == 1)
                                    {
                                        where.Append("NOT ");
                                    }
                                    where.Append("LIKE ");
                                    where.Append(ConvertValue(condition));
                                    break;
                                case "is in":
                                    if (condition.NotComparator == 1)
                                    {
                                        where.Append("NOT ");
                                    }
                                    where.Append("IN (");
                                    where.Append(ConvertValue(condition));
                                    where.Append(")");
                                    break;
                                case "equals":
                                    if (condition.NotComparator == 1)
                                    {
                                        where.Append("!");
                                    }
                                    where.Append("= ");
                                    where.Append(ConvertValue(condition));
                                    break;
                                case "greater than":
                                    if (condition.NotComparator == 1)
                                    {
                                        where.Append("<= ");
                                    }
                                    else
                                    {
                                        where.Append("> ");
                                    }
                                    where.Append(ConvertValue(condition));
                                    break;
                                case "less than":
                                    if (condition.NotComparator == 1)
                                    {
                                        where.Append(">= ");
                                    }
                                    else
                                    {
                                        where.Append("< ");
                                    }
                                    where.Append(ConvertValue(condition));
                                    break;
                                default:
                                    break;
                            }










                            //switch (condition.FieldType)
                            //{
                            //    case "String":
                            //        switch (condition.Comparator)
                            //        {
                            //            case "contains":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append("NOT ");
                            //                }
                            //                where.Append("LIKE '%");
                            //                where.Append(condition.CompareValue);
                            //                where.Append("%'");
                            //                break;
                            //            case "is in":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append("NOT ");
                            //                }
                            //                where.Append("IN (");
                            //                where.Append(ConvertValue(condition));
                            //                where.Append(")");
                            //                break;
                            //            case "equals":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append("!");
                            //                }
                            //                where.Append("=");
                            //                where.Append(ConvertValue(condition));
                            //                break;
                            //            default:
                            //                break;
                            //        }
                            //        break;
                            //    case "Date":
                            //        switch (condition.Comparator)
                            //        {
                            //            case "equals":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append("!");
                            //                }
                            //                where.Append("=");
                            //                where.Append(ConvertValue(condition));
                            //                break;
                            //            case "greater than":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append("<=");
                            //                }
                            //                else
                            //                {
                            //                    where.Append(">");
                            //                }
                            //                where.Append(ConvertValue(condition));
                            //                break;
                            //            case "less than":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append(">=");
                            //                }
                            //                else
                            //                {
                            //                    where.Append("<");
                            //                }
                            //                where.Append(ConvertValue(condition));
                            //                break;
                            //            case "is in":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append("NOT ");
                            //                }
                            //                where.Append("IN (");
                            //                where.Append(ConvertValue(condition));
                            //                where.Append(")");
                            //                break;
                            //            default:
                            //                break;
                            //        }
                            //        break;
                            //    case "Number":
                            //    case "Money":
                            //        switch (condition.Comparator)
                            //        {
                            //            case "equals":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append("!");
                            //                }
                            //                where.Append("=");
                            //                where.Append(condition.CompareValue);
                            //                break;
                            //            case "greater than":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append("<=");
                            //                }
                            //                else
                            //                {
                            //                    where.Append(">");
                            //                }
                            //                where.Append(condition.CompareValue);
                            //                break;
                            //            case "less than":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append(">=");
                            //                }
                            //                else
                            //                {
                            //                    where.Append("<");
                            //                }
                            //                where.Append(condition.CompareValue);
                            //                break;
                            //            case "is in":
                            //                if (condition.NotComparator == 1)
                            //                {
                            //                    where.Append("NOT ");
                            //                }
                            //                where.Append("IN (");
                            //                bool first = true;
                            //                foreach (string value in condition.CompareValue.Split(','))
                            //                {
                            //                    if (!first)
                            //                    {
                            //                        where.Append(",");                                                    
                            //                    }
                            //                    where.Append(value);
                            //                    first = false;
                            //                }
                            //                where.Append(")");
                            //                break;
                            //            default:
                            //                break;
                            //        }
                            //        break;
                            //    default:
                            //        break;
                            //}

                            where.Append(") ");
                            conditionIterator++;
                        }
                    }
                    where.Append(") ");
                    groupIterator++;
                }
            }
            return where.ToString();
        }

        public static string CreateDynamicWhere(Filter filter)
        {
            string where = "";
            //needs to be written

            return where;
        }

        public static int NameExists(Filter filter)
        {
            int count = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@GroupID", Convert.ToInt32(filter._GroupID));
            cmd.Parameters.AddWithValue("@FilterName", filter._FilterName);
            string sqlSelect = "";
            if (filter._FilterID == null || filter._FilterID == 0)
            {
                sqlSelect = "SELECT COUNT(*) FROM Filter WHERE GroupID = @GroupID and FilterName = @FilterName";
            }
            else
            {
                cmd.Parameters.AddWithValue("@FilterID", Convert.ToInt32(filter._FilterID));
                sqlSelect = "SELECT COUNT(*) FROM Filter WHERE GroupID = @GroupID AND FilterName = @FilterName AND FilterID != @FilterID";
            }
            cmd.CommandText = sqlSelect;
            try
            {
                count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
            }
            catch (Exception)
            {
            }

            return count;
        }
        #endregion

        #region Private Static Methods
        private static bool ValidateFilterObject(Filter filter)
        {
            bool isValid = false;
            try
            {
                if (filter._CustomerID >= 0 && filter._GroupID >= 0 && filter._UserID >= 0 && filter._FilterName.Trim().Length > 0 && filter._GroupCompareType.Trim().Length > 0)
                {
                    isValid = true;
                }
            }
            catch (Exception)
            {
            }
            return isValid;
        }

        private static string ConvertField(FilterCondition condition)
        {
            if (CommonFiltersFields.Any(x => string.CompareOrdinal(x, condition.Field) == 0))
            {
                return $"ISNULL({condition.Field}, '')";
            }
            else if (NonCommonFiltersFields.Any(x => string.CompareOrdinal(x, condition.Field) == 0))
            {
                var nonEqualAndFullDateField = string.Format("ISDATE({0}) = 1 AND {0}", condition.Field);
                var nonEqualAndNonFullDateField = string.Format("ISDATE({0}) = 1 AND {1}({0})", condition.Field, condition.DatePart);

                var equalAndFullDateField = string.Format("ISDATE({0}) = 1 AND CONVERT(VARCHAR(10), {0}, 101)", condition.Field);
                var equalAndNonFullDateField = string.Format("ISDATE({0}) = 1 AND CONVERT(VARCHAR, {1}({0}))", condition.Field, condition.DatePart);

                return GetDateField(condition.DatePart, condition.Comparator, nonEqualAndFullDateField, nonEqualAndNonFullDateField, equalAndFullDateField, equalAndNonFullDateField);
            }
            else
            {
                switch (condition.FieldType)
                {
                    case "String":
                        return string.Format("CONVERT(VARCHAR, ISNULL([{0}], ''))", condition.Field);
                    case "Date":
                        var nonEqualAndFullDateField = string.Format("CASE WHEN ISDATE([{0}]) = 1 THEN CONVERT(DATETIME, [{0}]) end", condition.Field);
                        var nonEqualAndNonFullDateField = string.Format("CASE WHEN ISDATE([{0}]) = 1 THEN {1}(CONVERT(DATETIME, [{0}])) end", condition.Field, condition.DatePart);

                        var equalAndFullDateField = string.Format("CASE WHEN ISDATE([{0}]) = 1 then CONVERT(VARCHAR(10),CONVERT(DATETIME, [{0}]), 101) end", condition.Field);
                        var equalAndNonFullDateField = string.Format("CASE WHEN ISDATE([{0}]) = 1 then CONVERT(VARCHAR,{1}(CONVERT(DATETIME, [{0}]))) end", condition.Field, condition.DatePart);

                        return GetDateField(condition.DatePart, condition.Comparator, nonEqualAndFullDateField, nonEqualAndNonFullDateField, equalAndFullDateField, equalAndNonFullDateField);
                    case "Number":
                        return "ISNUMERIC([" + condition.Field + "]) = 1 AND CONVERT(INT, [" + condition.Field + "])";
                    case "Money":
                        return "ISNUMERIC([" + condition.Field + "]) = 1 AND CONVERT(MONEY, [" + condition.Field + "])";
                    default:
                        return condition.Field;
                }
            }
        }

        private static string GetConvertValueBasedOnComparatorCondition(string comparator, string equalsCompareValue, string isInCompareValue, string containsCompareValue)
        {
            switch (comparator)
            {
                case EqualsComparator:
                    return equalsCompareValue;
                case IsInComparator:
                    return isInCompareValue;
                case ContainsComparator:
                    return containsCompareValue;
                default:
                    return string.Empty;
            }
        }

        private static string ConvertValue(FilterCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (CommonFiltersFields.Contains(condition.Field))
            {
                var equalsCompareValue = $"'{ condition.CompareValue }'";
                var isInCompareValue = GetIsInCompareValue(condition.CompareValue, "'{0}'");
                var containsCompareValue = $"'%{ condition.CompareValue }%'";
                return GetConvertValueBasedOnComparatorCondition(
                    condition.Comparator,
                    equalsCompareValue,
                    isInCompareValue,
                    containsCompareValue);
            }
            else
            {
                switch (condition.FieldType)
                {
                    case "String":
                        var equalsCompareValue = $"CONVERT(VARCHAR, '{ condition.CompareValue }')";
                        var isInCompareValue = GetIsInCompareValue(condition.CompareValue, "CONVERT(VARCHAR, '{0}')");
                        var containsCompareValue = $"CONVERT(VARCHAR, '%{ condition.CompareValue }%')";
                        return GetConvertValueBasedOnComparatorCondition(
                            condition.Comparator,
                            equalsCompareValue,
                            isInCompareValue,
                            containsCompareValue);
                    case "Date":
                        return GetConvertValueBasedOnDate(condition);
                    case "Number":
                        return GetNumberAndMoneyConvertValue(
                                            condition.Comparator,
                                            condition.CompareValue,
                                            "CONVERT(INT, {0})");
                    case "Money":
                        return GetNumberAndMoneyConvertValue(
                                            condition.Comparator,
                                            condition.CompareValue,
                                            "CONVERT(MONEY, {0})");
                }
            }

            return string.Empty;
        }

        private static string GetConvertValueBasedOnDate(FilterCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (string.CompareOrdinal(condition.Comparator, GreaterThanComparator) == 0
                 || string.CompareOrdinal(condition.Comparator, LessThanComparator) == 0)
            {
                var fullDateField = string.Empty;
                var nonFullDateField = string.Empty;

                if(condition.CompareValue.Contains($"{ExpToday}["))
                {
                    var days = GetDays(condition.CompareValue);

                    fullDateField = string.Format("DATEADD(dd, {0}, GETDATE())", days);
                    nonFullDateField = string.Format("{0}(DATEADD(dd, {1}, GETDATE()))", condition.DatePart, days);
                }
                else if (condition.CompareValue.Contains(ExpToday))
                {
                    fullDateField = "GETDATE()";
                    nonFullDateField = string.Format("{0}(GETDATE())", condition.DatePart);
                }
                else
                {
                    fullDateField = string.Format("CONVERT(DATETIME, '{0}')", condition.CompareValue);
                    nonFullDateField = string.Format("{0}(CONVERT(DATETIME, '{1}'))", condition.DatePart, condition.CompareValue);
                }

                return GetDateFieldBasedOnDatePart(condition.DatePart, fullDateField, nonFullDateField);
            }
            else if (string.CompareOrdinal(condition.Comparator, EqualsComparator) == 0)
            {
                var fullDateField = string.Empty;
                var nonFullDateField = string.Empty;

                if (condition.CompareValue.Contains($"{ExpToday}["))
                {
                    var days = GetDays(condition.CompareValue);

                    fullDateField = string.Format("CONVERT(VARCHAR(10),DATEADD(dd, {0}, GETDATE()),101)", days);
                    nonFullDateField = string.Format("CONVERT(VARCHAR,{0}(DATEADD(dd, {1}, GETDATE())))", condition.DatePart, days);
                }
                else if (condition.CompareValue.Contains(ExpToday))
                {
                    fullDateField = "CONVERT(VARCHAR(10),GETDATE(),101)";
                    nonFullDateField = string.Format("CONVERT(VARCHAR,{0}(GETDATE()))", condition.DatePart);
                }
                else
                {
                    fullDateField = string.Format("CONVERT(VARCHAR(10),CONVERT(DATETIME, '{0}'),101)", condition.CompareValue);
                    nonFullDateField = string.Format("CONVERT(VARCHAR,{0}(CONVERT(DATETIME, '{1}')))", condition.DatePart, condition.CompareValue);
                }

                return GetDateFieldBasedOnDatePart(condition.DatePart, fullDateField, nonFullDateField);
            }

            return string.Empty;
        }
        #endregion
    }

    public class FilterGroup
    {
        #region Constructor
        public FilterGroup()
        {
            _FilterGroupID = null;
            _FilterID = null;
            _SortOrder = null;
            _ConditionCompareType = "";
            _Name = "";
            _FilterConditionList = null;
        }
        #endregion

        #region Private Properties
        private int? _FilterGroupID;
        private int? _FilterID;
        private int? _SortOrder;
        private string _ConditionCompareType;
        private string _Name;
        private List<FilterCondition> _FilterConditionList;
        #endregion

        #region Public Properties
        public int? FilterGroupID
        {
            get
            {
                return _FilterGroupID;
            }
        }
        public int? FilterID
        {
            get
            {
                return _FilterID;
            }
            set
            {
                _FilterID = value;
            }
        }
        public int? SortOrder
        {
            get
            {
                return _SortOrder;
            }
            set
            {
                _SortOrder = value;
            }
        }
        public string ConditionCompareType
        {
            get
            {
                return _ConditionCompareType;
            }
            set
            {
                _ConditionCompareType = value;
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        public List<FilterCondition> FilterConditionList
        {
            get
            {
                return _FilterConditionList;
            }
            set
            {
                _FilterConditionList = value;
            }
        }
        #endregion

        #region Public Static Methods
        public static List<FilterGroup> GetFilterGroupList(int filterID)
        {
            FilterGroup filterGroup = null;
            List<FilterGroup> filterGroupList = new List<FilterGroup>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM FilterGroup WHERE FilterID = @FilterID ORDER BY SortOrder ASC";
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));

            SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    filterGroup = new FilterGroup();
                    if (reader["FilterGroupID"] != DBNull.Value)
                    {
                        filterGroup._FilterGroupID = Convert.ToInt32(reader["FilterGroupID"].ToString());
                    }
                    if (reader["FilterID"] != DBNull.Value)
                    {
                        filterGroup._FilterID = Convert.ToInt32(reader["FilterID"].ToString());
                    }
                    if (reader["SortOrder"] != DBNull.Value)
                    {
                        filterGroup._SortOrder = Convert.ToInt32(reader["SortOrder"].ToString());
                    }
                    if (reader["ConditionCompareType"] != DBNull.Value)
                    {
                        filterGroup._ConditionCompareType = reader["ConditionCompareType"].ToString().Trim();
                    }
                    if (reader["Name"] != DBNull.Value)
                    {
                        filterGroup._Name = reader["Name"].ToString().Trim();
                    }
                    filterGroup._FilterConditionList = FilterCondition.GetFilterConditionList(Convert.ToInt32(filterGroup.FilterGroupID));
                    filterGroupList.Add(filterGroup);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return filterGroupList;
        }

        public static FilterGroup GetFilterGroup(int filterGroupID)
        {
            FilterGroup filterGroup = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM FilterGroup WHERE FilterGroupID = @FilterGroupID";
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", filterGroupID));

            try
            {
                DataTable dt = DataFunctions.GetDataTable(cmd);
                if (dt != null && dt.Rows.Count == 1)
                {
                    filterGroup = new FilterGroup();
                    DataRow row = dt.Rows[0];

                    if (row["FilterGroupID"] != DBNull.Value)
                    {
                        filterGroup._FilterGroupID = Convert.ToInt32(row["FilterGroupID"].ToString());
                    }
                    if (row["FilterID"] != DBNull.Value)
                    {
                        filterGroup._FilterID = Convert.ToInt32(row["FilterID"].ToString());
                    }
                    if (row["SortOrder"] != DBNull.Value)
                    {
                        filterGroup._SortOrder = Convert.ToInt32(row["SortOrder"].ToString());
                    }
                    if (row["ConditionCompareType"] != DBNull.Value)
                    {
                        filterGroup._ConditionCompareType = row["ConditionCompareType"].ToString().Trim();
                    }
                    if (row["Name"] != DBNull.Value)
                    {
                        filterGroup._Name = row["Name"].ToString().Trim();
                    }
                    filterGroup._FilterConditionList = FilterCondition.GetFilterConditionList(Convert.ToInt32(filterGroup.FilterGroupID));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return filterGroup;
        }

        public static bool Save(ref FilterGroup filterGroup)
        {
            bool success = false;
            if (ValidateFilterGroupObject(filterGroup))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@FilterID", filterGroup._FilterID);
                cmd.Parameters.AddWithValue("@SortOrder", filterGroup._SortOrder);
                cmd.Parameters.AddWithValue("@ConditionCompareType", filterGroup._ConditionCompareType.Trim());
                cmd.Parameters.AddWithValue("@Name", filterGroup._Name.Trim());
                if (filterGroup._FilterGroupID == null)
                {
                    //add
                    cmd.CommandText = "insert into FilterGroup (FilterID, SortOrder, ConditionCompareType, Name)" +
                                        " values (@FilterID, @SortOrder, @ConditionCompareType, @Name);SELECT @@IDENTITY ";
                    try
                    {
                        filterGroup._FilterGroupID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd).ToString());
                        success = true;
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    //update
                    cmd.Parameters.AddWithValue("@FilterGroupID", filterGroup._FilterGroupID);
                    cmd.CommandText = "update FilterGroup set FilterID = @FilterID, SortOrder = @SortOrder," +
                        " ConditionCompareType = @ConditionCompareType, Name = @Name where FilterGroupID = @FilterGroupID";
                    try
                    {
                        if (DataFunctions.Execute(cmd) == 1)
                        {
                            success = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                if (success)
                {
                    Filter filter = Filter.GetFilter(Convert.ToInt32(filterGroup._FilterID));
                    filter.WhereClause = Filter.CreateWhereClause(filter);
                    Filter.Save(ref filter);
                }
            }
            return success;
        }

        public static bool Delete(int filterGroupID)
        {
            bool success = false;
            FilterGroup filterGroup = GetFilterGroup(filterGroupID);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete FilterCondition where FilterGroupID = @FilterGroupID;delete FilterGroup where FilterGroupID = @FilterGroupID";
                cmd.Parameters.AddWithValue("@FilterGroupID", filterGroupID);
                DataFunctions.Execute(cmd);
                if (ReSort(Convert.ToInt32(filterGroup._FilterID)))
                {
                    success = true;
                }
            }
            catch (Exception)
            {
            }
            return success;
        }

        public static int GetSortOrder(int filterID)
        {
            int sort = 0;
            StringBuilder selectSQL = new StringBuilder();

            selectSQL.Append("IF EXISTS (SELECT * FROM FilterGroup WHERE FilterID = @FilterID)");
            selectSQL.Append(" BEGIN");
            selectSQL.Append(" SELECT MAX(ISNULL(SortOrder, 0)) + 1 FROM FilterGroup WHERE FilterID = @FilterID");
            selectSQL.Append(" END");
            selectSQL.Append(" ELSE");
            selectSQL.Append(" BEGIN");
            selectSQL.Append(" SELECT 1");
            selectSQL.Append(" END");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.CommandText = selectSQL.ToString();
            try
            {
                sort = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd).ToString());
            }
            catch (Exception)
            {
            }

            return sort;
        }

        public static int NameExists(FilterGroup filterGroup)
        {
            int count = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterID", Convert.ToInt32(filterGroup._FilterID));
            cmd.Parameters.AddWithValue("@Name", filterGroup._Name);
            string sqlSelect = "";
            if (filterGroup._FilterGroupID == null || filterGroup._FilterGroupID == 0)
            {
                sqlSelect = "SELECT COUNT(*) FROM FilterGroup WHERE FilterID = @FilterID AND Name = @Name";
            }
            else
            {
                cmd.Parameters.AddWithValue("@FilterGroupID", Convert.ToInt32(filterGroup._FilterGroupID));
                sqlSelect = "SELECT COUNT(*) FROM FilterGroup WHERE FilterID = @FilterID AND Name = @Name AND FilterGroupID != @FilterGroupID";
            }
            cmd.CommandText = sqlSelect;
            try
            {
                count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
            }
            catch (Exception)
            {
            }

            return count;
        }
        #endregion

        #region Private Static Methods
        private static bool ValidateFilterGroupObject(FilterGroup filterGroup)
        {
            bool isValid = false;
            try
            {
                if (filterGroup._FilterID >= 0 && filterGroup._SortOrder >= 0 && filterGroup._Name.Trim().Length > 0 && filterGroup._ConditionCompareType.Trim().Length > 0)
                {
                    isValid = true;
                }
            }
            catch (Exception)
            {
            }
            return isValid;
        }

        private static bool ReSort(int filterID)
        {
            bool success = true;

            List<FilterGroup> filterGroupList = GetFilterGroupList(filterID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.Int));
            cmd.CommandText = "UPDATE FilterGroup SET SortOrder = @SortOrder WHERE FilterGroupID = @FilterGroupID";
            int i = 0;
            foreach (FilterGroup group in filterGroupList)
            {
                cmd.Parameters["@FilterGroupID"].Value = Convert.ToInt32(group._FilterGroupID);
                cmd.Parameters["@SortOrder"].Value = ++i;
                try
                {
                    DataFunctions.Execute(cmd);
                }
                catch (Exception)
                {
                    success = false;
                }
            }
            return success;
        }
        #endregion
    }

    public class FilterCondition
    {
        #region Constructor
        public FilterCondition()
        {
            _FilterConditionID = null;
            _FilterGroupID = null;
            _SortOrder = null;
            _Field = "";
            _FieldType = "";
            _Comparator = "";
            _CompareValue = "";
            _DatePart = "full";
            _NotComparator = 0;
        }
        #endregion

        #region Private Properties
        private int? _FilterConditionID;
        private int? _FilterGroupID;
        private int? _SortOrder;
        private int _NotComparator;
        private string _Field;
        private string _FieldType;
        private string _Comparator;
        private string _CompareValue;
        private string _DatePart;
        #endregion

        #region Public Properties
        public int? FilterConditionID
        {
            get
            {
                return _FilterConditionID;
            }
        }
        public int? FilterGroupID
        {
            get
            {
                return _FilterGroupID;
            }
            set
            {
                _FilterGroupID = value;
            }
        }
        public int? SortOrder
        {
            get
            {
                return _SortOrder;
            }
            set
            {
                _SortOrder = value;
            }
        }
        public int NotComparator
        {
            get
            {
                return _NotComparator;
            }
            set
            {
                _NotComparator = value;
            }
        }
        public string Field
        {
            get
            {
                return _Field;
            }
            set
            {
                _Field = value;
            }
        }
        public string FieldType
        {
            get
            {
                return _FieldType;
            }
            set
            {
                _FieldType = value;
            }
        }
        public string Comparator
        {
            get
            {
                return _Comparator;
            }
            set
            {
                _Comparator = value;
            }
        }
        public string CompareValue
        {
            get
            {
                return _CompareValue;
            }
            set
            {
                _CompareValue = value;
            }
        }
        public string DatePart
        {
            get
            {
                return _DatePart;
            }
            set
            {
                _DatePart = value;
            }
        }
        #endregion

        #region Public Static Methods
        public static List<FilterCondition> GetFilterConditionList(int filterGroupID)
        {
            FilterCondition filterCondition = null;
            List<FilterCondition> filterConditionList = new List<FilterCondition>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM FilterCondition WHERE FilterGroupID = @FilterGroupID ORDER BY SortOrder ASC";
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", filterGroupID));

            SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    filterCondition = new FilterCondition();
                    if (reader["FilterConditionID"] != DBNull.Value)
                    {
                        filterCondition._FilterConditionID = Convert.ToInt32(reader["FilterConditionID"].ToString());
                    }
                    if (reader["FilterGroupID"] != DBNull.Value)
                    {
                        filterCondition._FilterGroupID = Convert.ToInt32(reader["FilterGroupID"].ToString());
                    }
                    if (reader["SortOrder"] != DBNull.Value)
                    {
                        filterCondition._SortOrder = Convert.ToInt32(reader["SortOrder"].ToString());
                    }
                    if (reader["NotComparator"] != DBNull.Value)
                    {
                        filterCondition._NotComparator = Convert.ToInt32(reader["NotComparator"].ToString());
                    }
                    if (reader["Field"] != DBNull.Value)
                    {
                        filterCondition._Field = reader["Field"].ToString().Trim();
                    }
                    if (reader["FieldType"] != DBNull.Value)
                    {
                        filterCondition._FieldType = reader["FieldType"].ToString().Trim();
                    }
                    if (reader["Comparator"] != DBNull.Value)
                    {
                        filterCondition._Comparator = reader["Comparator"].ToString().Trim();
                    }
                    if (reader["CompareValue"] != DBNull.Value)
                    {
                        filterCondition._CompareValue = reader["CompareValue"].ToString().Trim();
                    }
                    if (reader["DatePart"] != DBNull.Value)
                    {
                        filterCondition._DatePart = reader["DatePart"].ToString().Trim();
                    }
                    filterConditionList.Add(filterCondition);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return filterConditionList;
        }

        public static FilterCondition GetFilterCondition(int filterConditionID)
        {
            FilterCondition filterCondition = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM FilterCondition WHERE FilterConditionID = @FilterConditionID";
            cmd.Parameters.Add(new SqlParameter("@FilterConditionID", filterConditionID));

            try
            {
                DataTable dt = DataFunctions.GetDataTable(cmd);
                if (dt != null && dt.Rows.Count == 1)
                {
                    filterCondition = new FilterCondition();
                    DataRow row = dt.Rows[0];
                    if (row["FilterConditionID"] != DBNull.Value)
                    {
                        filterCondition._FilterConditionID = Convert.ToInt32(row["FilterConditionID"].ToString());
                    }
                    if (row["FilterGroupID"] != DBNull.Value)
                    {
                        filterCondition._FilterGroupID = Convert.ToInt32(row["FilterGroupID"].ToString());
                    }
                    if (row["SortOrder"] != DBNull.Value)
                    {
                        filterCondition._SortOrder = Convert.ToInt32(row["SortOrder"].ToString());
                    }
                    if (row["NotComparator"] != DBNull.Value)
                    {
                        filterCondition._NotComparator = Convert.ToInt32(row["NotComparator"].ToString());
                    }
                    if (row["Field"] != DBNull.Value)
                    {
                        filterCondition._Field = row["Field"].ToString().Trim();
                    }
                    if (row["FieldType"] != DBNull.Value)
                    {
                        filterCondition._FieldType = row["FieldType"].ToString().Trim();
                    }
                    if (row["Comparator"] != DBNull.Value)
                    {
                        filterCondition._Comparator = row["Comparator"].ToString().Trim();
                    }
                    if (row["CompareValue"] != DBNull.Value)
                    {
                        filterCondition._CompareValue = row["CompareValue"].ToString().Trim();
                    }
                    if (row["DatePart"] != DBNull.Value)
                    {
                        filterCondition._DatePart = row["DatePart"].ToString().Trim();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return filterCondition;
        }

        public static bool Save(ref FilterCondition filterCondition)
        {
            bool success = false;
            if (ValidateFilterConditionObject(filterCondition))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@FilterGroupID", filterCondition._FilterGroupID);
                cmd.Parameters.AddWithValue("@SortOrder", filterCondition._SortOrder);
                cmd.Parameters.AddWithValue("@NotComparator", filterCondition._NotComparator);
                cmd.Parameters.AddWithValue("@Field", filterCondition._Field.Trim());
                cmd.Parameters.AddWithValue("@FieldType", filterCondition._FieldType.Trim());
                cmd.Parameters.AddWithValue("@Comparator", filterCondition._Comparator.Trim());
                cmd.Parameters.AddWithValue("@CompareValue", filterCondition._CompareValue.Trim());
                cmd.Parameters.AddWithValue("@DatePart", filterCondition._DatePart.Trim());
                if (filterCondition._FilterConditionID == null)
                {
                    //add
                    cmd.CommandText = "insert into FilterCondition (FilterGroupID, SortOrder, Field, FieldType, Comparator, CompareValue, NotComparator, DatePart)" +
                                        " values (@FilterGroupID, @SortOrder, @Field, @FieldType, @Comparator, @CompareValue, @NotComparator, @DatePart);SELECT @@IDENTITY ";
                    try
                    {
                        filterCondition._FilterConditionID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd).ToString());
                        success = true;
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    //update
                    cmd.Parameters.AddWithValue("@FilterConditionID", filterCondition._FilterConditionID);
                    cmd.CommandText = "update FilterCondition set FilterGroupID = @FilterGroupID, SortOrder = @SortOrder," +
                        " Field = @Field, FieldType = @FieldType, Comparator = @Comparator, CompareValue = @CompareValue, NotComparator = @NotComparator, DatePart = @DatePart" +
                        " where FilterConditionID = @FilterConditionID";
                    try
                    {
                        if (DataFunctions.Execute(cmd) == 1)
                        {
                            success = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                if (success)
                {
                    Filter filter = Filter.GetFilter(Convert.ToInt32(FilterGroup.GetFilterGroup(Convert.ToInt32(filterCondition._FilterGroupID)).FilterID));
                    //filter.WhereClause = Filter.CreateWhereClause(filter);
                    try
                    {
                        Filter.Save(ref filter);
                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                }
            }
            return success;
        }

        public static bool Delete(int filterConditionID)
        {
            FilterCondition condition = GetFilterCondition(filterConditionID);
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete FilterCondition where FilterConditionID = @FilterConditionID";
                cmd.Parameters.AddWithValue("@FilterConditionID", filterConditionID);
                DataFunctions.Execute(cmd);
                if (ReSort(Convert.ToInt32(condition._FilterGroupID)))
                {
                    success = true;
                }
            }
            catch (Exception)
            {
            }
            return success;
        }

        public static int GetSortOrder(int filterGroupID)
        {
            int sort = 0;
            StringBuilder selectSQL = new StringBuilder();

            selectSQL.Append("IF EXISTS (SELECT * FROM FilterCondition WHERE FilterGroupID = @FilterGroupID)");
            selectSQL.Append(" BEGIN");
            selectSQL.Append(" SELECT MAX(ISNULL(SortOrder, 0)) + 1 FROM FilterCondition WHERE FilterGroupID = @FilterGroupID");
            selectSQL.Append(" END");
            selectSQL.Append(" ELSE");
            selectSQL.Append(" BEGIN");
            selectSQL.Append(" SELECT 1");
            selectSQL.Append(" END");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterGroupID", filterGroupID);
            cmd.CommandText = selectSQL.ToString();
            try
            {
                sort = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd).ToString());
            }
            catch (Exception)
            {
            }

            return sort;
        }

        #endregion

        #region Private Static Methods
        private static bool ValidateFilterConditionObject(FilterCondition filterCondition)
        {
            bool isValid = false;
            try
            {
                if (filterCondition._FilterGroupID >= 0 && filterCondition._SortOrder >= 0 && filterCondition._Field.Trim().Length > 0 &&
                    filterCondition._FieldType.Trim().Length > 0 && filterCondition._Comparator.Trim().Length > 0)
                {
                    if (filterCondition._FieldType == "String" || filterCondition._CompareValue.Trim().Length > 0)
                    {
                        isValid = true;
                    }

                    //switch (filterCondition._FieldType)
                    //{
                    //    case "String":
                    //        isValid = true;
                    //        break;
                    //    case "Date":
                    //        Convert.ToDateTime(filterCondition._CompareValue);
                    //        isValid = true;                            
                    //        break;
                    //    case "Money":
                    //        Convert.ToDecimal(filterCondition._CompareValue);
                    //        isValid = true;
                    //        break;
                    //    case "Number":
                    //        Convert.ToInt32(filterCondition._CompareValue);
                    //        isValid = true;
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
            }
            catch (Exception)
            {
            }
            return isValid;
        }

        private static bool ReSort(int filterGroupID)
        {
            bool success = true;

            List<FilterCondition> filterConditionList = GetFilterConditionList(filterGroupID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@FilterConditionID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.Int));
            cmd.CommandText = "UPDATE FilterCondition SET SortOrder = @SortOrder WHERE FilterConditionID = @FilterConditionID";
            int i = 0;
            foreach (FilterCondition condition in filterConditionList)
            {
                cmd.Parameters["@FilterConditionID"].Value = Convert.ToInt32(condition._FilterConditionID);
                cmd.Parameters["@SortOrder"].Value = ++i;
                try
                {
                    DataFunctions.Execute(cmd);
                }
                catch (Exception)
                {
                    success = false;
                }
            }
            return success;
        }

        #endregion
    }
}
