using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.classes.ImportData
{
    public class EmailTableColumnManagerCommunicator : EmailTableColumnManager
    {
        public void AddGroupDataFields(IList groupDataFields)
        {
            var i = ColumnCount;
            foreach (GroupDataField groupDataField in groupDataFields)
            {
                _columns.Add(i++, $"user_{groupDataField.ShortName}");
            }
        }
    }
}
