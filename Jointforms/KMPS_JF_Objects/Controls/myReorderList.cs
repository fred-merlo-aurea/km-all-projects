using System;
using System.Data;
using AjaxControlToolkit;

namespace KMPS_JF_Objects.Controls
{
    public class myReorderList : ReorderList
    {
        protected override bool DoReorder(int oldIndex, int newIndex)
        {
            if (DataSource is DataTable)
            {
                DataRowCollection rows = ((DataTable)DataSource).Rows;
                int NewListOrder = (int)rows[newIndex][SortOrderField];

                if (oldIndex < newIndex) //item moved down
                {
                    for (int i = oldIndex + 1; i <= newIndex; i++)
                    {
                        rows[i][SortOrderField] = (int)rows[i][SortOrderField] - 1;
                    }
                }
                else  //item moved up
                {
                    for (int i = oldIndex - 1; i >= newIndex; i--)
                    {
                        rows[i][SortOrderField] = (int)rows[i][SortOrderField] + 1;
                    }
                }
                rows[oldIndex][SortOrderField] = NewListOrder;
                return true;
            }
            else
            {
                throw new InvalidOperationException
            ("DataSource is not a System.Data.DataTable.");
            }
        }

    }
}
