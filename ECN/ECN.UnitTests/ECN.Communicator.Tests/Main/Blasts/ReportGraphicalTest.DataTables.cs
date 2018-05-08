using System.Data;

namespace ECN.Communicator.Tests.Main.Blasts
{
    public partial class ReportGraphicalTest
    {
        private DataTable GetGroupNameDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(GroupNameColumn);
            dataTable.Columns.Add(GrpNavigateUrlColumn);
            dataTable.Columns.Add(FilterNameColumn);
            dataTable.Columns.Add(FltrNavigateUrlColumn);
            dataTable.Columns.Add(LayoutNameColumn);
            dataTable.Columns.Add(LytNavigateUrlColumn);
            dataTable.Columns.Add(EmailSubjectColumn);
            dataTable.Columns.Add(EmailFromNameColumn);
            dataTable.Columns.Add(EmailFromColumn);
            dataTable.Columns.Add(SendTimeColumn);
            dataTable.Columns.Add(FinishTimeColumn);
            dataTable.Columns.Add(SuccessTotalColumn);
            dataTable.Columns.Add(SendTotalColumn);
            dataTable.Columns.Add(SetupCostColumn);
            dataTable.Columns.Add(OutboundCostColumn);
            dataTable.Columns.Add(InboundCostColumn);
            dataTable.Columns.Add(DesignCostColumn);
            dataTable.Columns.Add(OtherCostColumn);

            dataTable.Rows.Add(GetGroupNameTableRow(dataTable));

            return dataTable;
        }

        private DataRow GetGroupNameTableRow(DataTable dataTable)
        {
            var row = dataTable.NewRow();

            row[GroupNameColumn] = GroupNameValue;
            row[GrpNavigateUrlColumn] = GrpNavigateUrlValue;
            row[FilterNameColumn] = FilterNameValue;
            row[FltrNavigateUrlColumn] = FltrNavigateUrlValue;
            row[LayoutNameColumn] = LayoutNameValue;
            row[LytNavigateUrlColumn] = LytNavigateUrlValue;
            row[EmailSubjectColumn] = EmailSubjectValue;
            row[EmailFromNameColumn] = EmailFromNameValue;
            row[EmailFromColumn] = EmailFromValue;
            row[SendTimeColumn] = SendTimeValue;
            row[FinishTimeColumn] = FinishTimeValue;
            row[SuccessTotalColumn] = SuccessTotalValue;
            row[SendTotalColumn] = SendTotalValue;
            row[SetupCostColumn] = SetupCostValue;
            row[OutboundCostColumn] = OutboundCostValue;
            row[InboundCostColumn] = InboundCostValue;
            row[DesignCostColumn] = DesignCostValue;
            row[OtherCostColumn] = OtherCostValue;

            return row;
        }

        private DataTable GetGraphicalBlastReportDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(ActionTypeCodeColumn);
            dataTable.Columns.Add(ActionValueColumn);
            dataTable.Columns.Add(ActionDateMmddyyyyColumn);

            foreach (var code in ActionTypeCodes)
            {
                foreach (var action in ActionValues)
                {
                    var row = dataTable.NewRow();

                    row[ActionTypeCodeColumn] = code;
                    row[ActionValueColumn] = action;
                    row[ActionDateMmddyyyyColumn] = SendTimeValue;

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        private DataTable GetGraphicalBlastBounceReportDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(BounceCountColumn);
            dataTable.Columns.Add(BounceTypeColumn);

            foreach (var action in ActionValues)
            {
                var row = dataTable.NewRow();

                row[BounceCountColumn] = BounceCount;
                row[BounceTypeColumn] = action;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}