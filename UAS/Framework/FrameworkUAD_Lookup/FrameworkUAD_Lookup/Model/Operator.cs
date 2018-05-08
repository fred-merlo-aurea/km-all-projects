using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Model
{
    [Serializable]
    [DataContract]
    public class Operator
    {
        public Enums.FieldDataType fieldDataType
        { get
            {
                return Enums.GetFieldDataType(dataType);
            }
        }
        public Enums.OperatorTypes operatorType { get { return Enums.GetOperatorType(operatorText); } }

        public string dataType { get; set; }
        public string operatorValue { get; set; }
        public string operatorText { get; set; }
        public int sortOrder { get; set; }
        public int codeId { get; set; }
        
        public static List<Operator> GetOperators()
        {
            //this should be kept in a sproc that returns from db
            List<Operator> Operators = new FrameworkUAD_Lookup.BusinessLogic.Code().GetOperators();// List<Operator>();

            #region old hard coded - replaced with sproc UAD_Lookup.o_Operators_Select
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.String, OperatorValue = "Contains", OperatorText = "CONTAINS", SortOrder = 1 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.String, OperatorValue = "Is", OperatorText = "IS", SortOrder = 2 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.String, OperatorValue = "Starts With", OperatorText = "STARTS WITH", SortOrder = 3 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.String, OperatorValue = "Ends With", OperatorText = "ENDS WITH", SortOrder = 4 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.String, OperatorValue = "Not Contain", OperatorText = "DOES NOT CONTAIN", SortOrder = 5 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.String, OperatorValue = "Is Empty", OperatorText = "IS EMPTY", SortOrder = 6 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.String, OperatorValue = "Is Not Empty", OperatorText = "IS NOT EMPTY", SortOrder = 7 });

            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Date, OperatorValue = "DateRange", OperatorText = "DATERANGE", SortOrder = 1 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Date, OperatorValue = "XDays", OperatorText = "LAST X DAYS", SortOrder = 2 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Date, OperatorValue = "Year", OperatorText = "YEAR", SortOrder = 3 });

            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Int, OperatorValue = "Range", OperatorText = "RANGE", SortOrder = 1 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Int, OperatorValue = "Equal", OperatorText = "EQUAL", SortOrder = 2 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Int, OperatorValue = "Greater", OperatorText = "GREATER THAN", SortOrder = 3 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Int, OperatorValue = "Lesser", OperatorText = "LESSER THAN", SortOrder = 4 });

            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Float, OperatorValue = "Range", OperatorText = "RANGE", SortOrder = 1 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Float, OperatorValue = "Equal", OperatorText = "EQUAL", SortOrder = 2 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Float, OperatorValue = "Greater", OperatorText = "GREATER THAN", SortOrder = 3 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Float, OperatorValue = "Lesser", OperatorText = "LESSER THAN", SortOrder = 4 });


            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Decimal, OperatorValue = "is", OperatorText = "IS", SortOrder = 1 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Decimal, OperatorValue = "is not", OperatorText = "IS NOT", SortOrder = 2 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Decimal, OperatorValue = "is null", OperatorText = "IS NULL", SortOrder = 3 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Decimal, OperatorValue = "is not null", OperatorText = "IS NOT NULL", SortOrder = 4 });


            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Bit, OperatorValue = "Yes", OperatorText = "Yes", SortOrder = 1 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Bit, OperatorValue = "No", OperatorText = "No", SortOrder = 2 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Bit, OperatorValue = "Is Empty", OperatorText = "Is Empty", SortOrder = 3 });

            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Lookup, OperatorValue = "in", OperatorText = "IN", SortOrder = 1 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Lookup, OperatorValue = "not in", OperatorText = "NOT IN", SortOrder = 1 });

            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Demo, OperatorValue = "in", OperatorText = "IN", SortOrder = 1 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Demo, OperatorValue = "not in", OperatorText = "NOT IN", SortOrder = 2 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Demo, OperatorValue = "start with", OperatorText = "START WITH", SortOrder = 3 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Demo, OperatorValue = "end with", OperatorText = "END WITH", SortOrder = 4 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Demo, OperatorValue = "not Contain", OperatorText = "DOES NOT CONTAIN", SortOrder = 5 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Decimal, OperatorValue = "is", OperatorText = "IS", SortOrder = 6 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Decimal, OperatorValue = "is not", OperatorText = "IS NOT", SortOrder = 7 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Decimal, OperatorValue = "is null", OperatorText = "IS NULL", SortOrder = 8 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Decimal, OperatorValue = "is not null", OperatorText = "IS NOT NULL", SortOrder = 9 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Demo, OperatorValue = "is Empty", OperatorText = "IS EMPTY", SortOrder = 10 });
            //Operators.Add(new Operator() { DataType = Enums.FieldDataType.Demo, OperatorValue = "is Not Empty", OperatorText = "IS NOT EMPTY", SortOrder = 11 });
            #endregion

            return Operators;
        }
    }
}
