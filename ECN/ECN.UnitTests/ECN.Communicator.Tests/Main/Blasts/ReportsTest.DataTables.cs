using System.Collections.Generic;
using System.Data;

namespace ECN.Communicator.Tests.Main.Blasts
{
    public partial class ReportsTest
    {
        private DataTable GetReportingDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add();
            dataTable.Columns.Add();
            dataTable.Columns.Add(ActionTypeCodeColumn);

            for (var index = 0; index < ActionTypeCodes.Count; index++)
            {
                var code = ActionTypeCodes[index];
                var row = dataTable.NewRow();

                row[0] = index;
                row[1] = index + 1;
                row[ActionTypeCodeColumn] = code;

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private DataTable GetInvalidDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(ActionTypeCodeColumn);

            foreach (var code in ActionTypeCodes)
            {
                var row = dataTable.NewRow();
                row[ActionTypeCodeColumn] = code;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
