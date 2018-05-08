using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

namespace KMPS.MD.Objects
{
    public static class ExportReport
    {

        public static void ExportToExcel<T>(this List<T> list, string filename)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", filename + ".xls"));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Table table = new Table();
                    TableRow row = new TableRow();

                    PropertyInfo[] propInfos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Select(x => new
                    {
                        Property = x,
                    })
                    .Select(x => x.Property)
                    .ToArray();

                    for (int i = 0; i <= propInfos.Length - 1; i++)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = propInfos[i].Name;
                        row.Cells.Add(hcell);
                    }

                    table.Rows.Add(row);
                    Dictionary<string, object> dTotals = new Dictionary<string, object>();

                    for (int i = 0; i <= list.Count - 1; i++)
                    {
                        T item = list[i];
                        TableRow row1 = new TableRow();
                        string value;

                        for (int j = 0; j <= propInfos.Length - 1; j++)
                        {
                            object o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                            if (o != null)
                            {
                                value = o.ToString();

                                TableCell cell = new TableCell();

                                cell.Text = value;
                                row1.Cells.Add(cell);
                            }
                            else
                            {
                                TableCell cell = new TableCell();
                                cell.Text = "";
                                row1.Cells.Add(cell);
                            }
                        }

                        table.Rows.Add(row1);
                    }

                    TableRow row2 = new TableRow();

                    for (int i = 0; i <= propInfos.Length - 1; i++)
                    {
                        TableCell tcell = new TableCell();
                        tcell.Text = "";

                        if (dTotals.ContainsKey(propInfos[i].Name))
                        {
                            tcell.Text = dTotals[propInfos[i].Name].ToString();
                        }

                        row2.Cells.Add(tcell);
                    }

                    table.Rows.Add(row2);

                    table.RenderControl(htw);
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }

        }

    }
}