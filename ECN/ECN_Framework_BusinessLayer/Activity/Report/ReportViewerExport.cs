using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Entities.Activity.Report;
using Table = System.Web.UI.WebControls.Table;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    public static class ReportViewerExport
    {
        private const string Header = "Header";
        private const string Ignore = "Ignore";
        private const string Percent = " %";
        private const string TabChar = "\t";

        public static void ExportCSV<T>(this List<T> list, string filename, string StartDate = "", string EndDate = "")
        {
            var csv = string.Empty;

            Type typeParameterType = typeof(T);
            if (typeParameterType.Name == "DataRow")
            {
                DataTable dt = new DataTable();
                List<DataRow> dataRows = list as List<DataRow>;
                if (dataRows != null) { dt = dataRows[0].Table; }
                csv = GetCsvFromDataTable(dt, StartDate, EndDate);
            }
            else
            {
                csv = GetCSV(list, StartDate, EndDate);
            }
            ExportToCSV(csv, filename);
        }

        public static void ExportTabDelimited<T, U>(List<T> list, List<U> sub, string fileName)
        {
            string tab = GetTabDelimited(list, sub);
            ExportToCSV(tab, fileName);
        }

        public static string GetCsvFromDataTable(DataTable dt, string StartDate = "", string EndDate = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
                sb.AppendLine(string.Format("Data range from: {0} to: {1}", StartDate, EndDate));

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => string.Concat("\"", column.ColumnName.Replace("\"", "\"\""), "\""));
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }


            return sb.ToString();
        }

        public static string GetCSV<T>(this List<T> list, string StartDate = "", string EndDate = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
                sb.AppendLine(string.Format("Data range from: {0} to: {1}", StartDate, EndDate));

            //Get the properties for type T for the headers
            //PropertyInfo[] propInfos = typeof(T).GetProperties();

            PropertyInfo[] propInfos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(x => new
            {
                Property = x,
                Attribute = (ECN_Framework_Entities.Activity.Report.ExportAttribute)Attribute.GetCustomAttribute(x, typeof(ECN_Framework_Entities.Activity.Report.ExportAttribute), true)
            })
            .OrderBy(x => x.Attribute != null ? x.Attribute.FieldOrder : -1)
            .Select(x => x.Property)
            .ToArray();

            for (int i = 0; i <= propInfos.Length - 1; i++)
            {
                List<System.Reflection.CustomAttributeData> listCA = propInfos[i].CustomAttributes.ToList();
                string headerText = propInfos[i].Name;
                bool ignore = false;
                foreach (System.Reflection.CustomAttributeData cad in listCA)
                {
                    if (cad.NamedArguments.Count(x => x.MemberName == "Header") > 0)
                    {
                        headerText = cad.NamedArguments.First(x => x.MemberName == "Header").TypedValue.Value.ToString();

                    }
                    if (cad.NamedArguments.Count(x => x.MemberName == "Ignore") > 0)
                    {
                        ignore = Boolean.Parse(cad.NamedArguments.First(x => x.MemberName == "Ignore").TypedValue.Value.ToString());
                    }

                }
                if (!ignore)
                {
                    sb.Append(headerText);


                    if (i < propInfos.Length - 1)
                    {
                        sb.Append(",");
                    }
                }
            }
            sb.AppendLine();

            //Loop through the collection, then the properties and add the values
            for (int i = 0; i <= list.Count - 1; i++)
            {
                T item = list[i];
                for (int j = 0; j <= propInfos.Length - 1; j++)
                {
                    object o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                    if (o != null)
                    {
                        var att = propInfos[j].GetCustomAttributes(typeof(ECN_Framework_Entities.Activity.Report.ExportAttribute), true);
                        if (att != null && att.Length > 0)
                        {
                            if (((ECN_Framework_Entities.Activity.Report.ExportAttribute)att[0]).Ignore == true)
                                continue;
                        }
                        string value = o.ToString();

                        if(value.StartsWith("\""))
                        {
                            value = "“" + value.Substring(1);
                        }
                        //Check if the value contans a comma and place it in quotes if so
                        if (value.Contains(","))
                        {
                            //if the string contains quotes as well as commas, the export will bind the wrapping quotes with the first and last occurance of quotes in the string.
                            //This can expose the comma(s) and split the value between cells.
                            //To remedy this, this block adds an additional quote before the first quote and after the last quote.
                            //These allow our new quotes to have proper pairs regardless of what the string is.
                            //And all quotes will be displayed as desired.
                            if (value.Contains("\""))
                            {
                                int index = value.IndexOf("\"");
                                value = value.Substring(0, index) + "\"" + value.Substring(index);
                                index = value.LastIndexOf("\"");
                                value = value.Substring(0, index) + "\"" + value.Substring(index);
                            }

                            value = string.Concat("\"", value, "\"");
                        }

                        //replacing em dash with regular hyphen
                        if (value.Contains("—"))
                        {
                            value = value.Replace("—", "-");
                        }
                        //Replace any \r or \n special characters from a new line with a space
                        if (value.Contains("\r"))
                        {
                            value = value.Replace("\r", " ");
                        }
                        if (value.Contains("\n"))
                        {
                            value = value.Replace("\n", " ");
                        }


                        if (att != null && att.Length > 0)
                        {

                            if (((ECN_Framework_Entities.Activity.Report.ExportAttribute)att[0]).Format == ECN_Framework_Entities.Activity.Report.FormatType.Percent)
                            {
                                value = value + " %";
                            }
                        }

                        sb.Append(value);
                    }

                    if (j < propInfos.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string GetTabDelimited<T, U>(List<T> list, List<U> subReport)
        {
            var stringBuilder = new StringBuilder();

            //Get property info on first list
            var propertyInfosT = GetPropertyInfos<T>();

            //Get property info on second list
            var propertyInfosU = GetPropertyInfos<U>();

            stringBuilder.Append(GetHeaderInfo(propertyInfosT))
                         .AppendLine();

            //loop through first list
            for (var index = 0; index < list.Count; index++)
            {
                var item = list[index];
                stringBuilder.AppendLine(GetSubGroup<T>(propertyInfosT, item));

                //Build subreport from second list
                if (subReport != null && subReport.Count > 0)
                {
                    stringBuilder.Append(GetSubReport<U>(propertyInfosU, subReport, index, propertyInfosT.Length));
                }
            }

            return stringBuilder.ToString();
        }

        private static PropertyInfo[] GetPropertyInfos<T>()
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Select(x => new
                        {
                            Property = x,
                            Attribute = (ExportAttribute)Attribute.GetCustomAttribute(x, typeof(ExportAttribute), true)
                        })
                        .OrderBy(x => x.Attribute != null ? x.Attribute.FieldOrder : -1)
                        .Select(x => x.Property)
                        .ToArray();

            return propertyInfos;
        }

        private static string GetHeaderInfo(PropertyInfo[] propertyInfos)
        {
            var stringBuilder = new StringBuilder();

            for (var index = 0; index < propertyInfos.Length; index++)
            {
                var listCA = propertyInfos[index].CustomAttributes.ToList();
                var headerText = propertyInfos[index].Name;
                var ignore = false;
                foreach (var customAttribteData in listCA)
                {
                    if (customAttribteData.NamedArguments.Count(x => x.MemberName == Header) > 0)
                    {
                        headerText = customAttribteData.NamedArguments.First(x => x.MemberName == Header).TypedValue.Value.ToString();

                    }
                    if (customAttribteData.NamedArguments.Count(x => x.MemberName == Ignore) > 0)
                    {
                        ignore = Boolean.Parse(customAttribteData.NamedArguments.First(x => x.MemberName == Ignore).TypedValue.Value.ToString());
                    }
                }

                if (!ignore)
                {
                    stringBuilder.Append(headerText);

                    if (index < propertyInfos.Length - 1)
                    {
                        stringBuilder.Append("\t");
                    }
                }
            }

            return stringBuilder.ToString();
        }

        private static string GetSubGroup<T>(PropertyInfo[] propertyInfos, T item)
        {
            var stringBuilder = new StringBuilder();

            for (var index = 0; index < propertyInfos.Length; index++)
            {
                var customAttributeDatas = propertyInfos[index].CustomAttributes.ToList();
                var itemObject = item.GetType().GetProperty(propertyInfos[index].Name).GetValue(item, null);
                var ignore = false;

                foreach (var customAttribteData in customAttributeDatas)
                {
                    if (customAttribteData.NamedArguments.Count(x => x.MemberName == Ignore) > 0)
                    {
                        ignore = Boolean.Parse(customAttribteData
                            .NamedArguments
                            .First(x => x.MemberName == Ignore)
                            .TypedValue
                            .Value
                            .ToString());
                    }
                }

                if (itemObject != null && !ignore)
                {
                    var cellValue = itemObject.ToString();
                    var attribute = propertyInfos[index].GetCustomAttributes(typeof(ExportAttribute), true);

                    if (attribute != null && attribute.Length > 0)
                    {
                        if (((ExportAttribute)attribute[0]).Format == FormatType.Percent)
                        {
                            cellValue = cellValue + Percent;
                        }
                    }

                    stringBuilder.Append(cellValue);
                    if (index < propertyInfos.Length - 1)
                    {
                        stringBuilder.Append(TabChar);
                    }
                }
                else if (itemObject == null && !ignore)
                {
                    var cellValue = string.Empty;
                    var attribute = propertyInfos[index].GetCustomAttributes(typeof(ExportAttribute), true);

                    if (attribute != null && attribute.Length > 0)
                    {
                        if (((ExportAttribute)attribute[0]).Format == FormatType.Percent)
                        {
                            cellValue = cellValue + Percent;
                        }
                    }
                    stringBuilder.Append(cellValue);
                    if (index < propertyInfos.Length - 1)
                    {
                        stringBuilder.Append(TabChar);
                    }
                }
            }

            return stringBuilder.ToString();
        }

        private static string GetSubReport<U>(PropertyInfo[] propertyInfos, List<U> subReport, int index, int propertyInfoTLength)
        {
            var stringBuilder = new StringBuilder();
            var subItem = subReport[index];

            //Build subreport headers
            for (var i = 0; i <= propertyInfos.Length - 1; i++)
            {
                stringBuilder.Append(propertyInfos[i].Name);
                if (i < propertyInfos.Length - 1)
                {
                    stringBuilder.Append(TabChar);
                }

            }
            stringBuilder.AppendLine();

            //loop through second list
            for (var j = 0; j <= subReport.Count - 1; j++)
            {
                for (var p = 0; p <= propertyInfos.Length - 1; p++)
                {
                    object subItemData = subItem.GetType().GetProperty(propertyInfos[p].Name).GetValue(subItem, null);
                    if (subItemData != null)
                    {
                        var value = subItemData.ToString();
                        stringBuilder.Append(value);
                        if (j < propertyInfoTLength - 1)
                        {
                            stringBuilder.Append("\t");
                        }
                    }
                    else
                    {
                        if (p < propertyInfos.Length - 1)
                        {
                            stringBuilder.Append("\t");
                        }
                    }
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public static void ExportToCSV(string csv, string filename)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename=\"{0}.csv\"", filename));
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252"); // Standard for excel
            HttpContext.Current.Response.Write(csv);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static void ExportToTXT(string csv, string filename)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename=\"{0}.txt\"", filename));
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.Write(csv);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();


        }

        public static void ExportToTab(string tab, string filename)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename=\"{0}.xls\"", filename));
            HttpContext.Current.Response.ContentType = "text/xls";

            HttpContext.Current.Response.Write(tab);
            HttpContext.Current.Response.End();
        }

        public static void ExportToExcelFromDataTbl<T>(this List<T> list, string filename, string StartDate = "", string EndDate = "")
        {
            var csv = string.Empty;

            Type typeParameterType = typeof(T);
            if (typeParameterType.Name == "DataRow")
            {
                DataTable dt = new DataTable();
                List<DataRow> dataRows = list as List<DataRow>;
                if (dataRows != null) { dt = dataRows[0].Table; }
                csv = GetCsvFromDataTable(dt, StartDate, EndDate);
                csv = csv.Replace("\",\"", "\"\t\"");
            }

            ExportToTab(csv, filename);
        }

        public static void ExportToExcel<T>(this List<T> list, string filename)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename=\"{0}", filename + ".xls\""));
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
                        Attribute = (ECN_Framework_Entities.Activity.Report.ExportAttribute)Attribute.GetCustomAttribute(x, typeof(ECN_Framework_Entities.Activity.Report.ExportAttribute), true)
                    })
                    .OrderBy(x => x.Attribute != null ? x.Attribute.FieldOrder : -1)
                    .Select(x => x.Property)
                    .ToArray();

                    for (int i = 0; i <= propInfos.Length - 1; i++)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = propInfos[i].Name;
                        //row.Cells.Add(hcell);

                        List<System.Reflection.CustomAttributeData> listCA = propInfos[i].CustomAttributes.ToList();
                        string headerText = propInfos[i].Name;
                        bool ignore = false;
                        foreach (System.Reflection.CustomAttributeData cad in listCA)
                        {
                            if (cad.NamedArguments.Count(x => x.MemberName == "Header") > 0)
                            {
                                headerText = cad.NamedArguments.First(x => x.MemberName == "Header").TypedValue.Value.ToString();

                            }
                            if (cad.NamedArguments.Count(x => x.MemberName == "Ignore") > 0)
                            {
                                ignore = Boolean.Parse(cad.NamedArguments.First(x => x.MemberName == "Ignore").TypedValue.Value.ToString());
                            }

                        }
                        if (!ignore)
                        {
                            hcell.Text = headerText;


                            row.Cells.Add(hcell);

                        }
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

                                var att = propInfos[j].GetCustomAttributes(typeof(ECN_Framework_Entities.Activity.Report.ExportAttribute), true);

                                if (att != null && att.Length > 0)
                                {
                                    if (((ECN_Framework_Entities.Activity.Report.ExportAttribute)att[0]).Format == ECN_Framework_Entities.Activity.Report.FormatType.Percent)
                                    {
                                        value = value + " %";
                                    }
                                }

                                cell.Text = CleanString(value);
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

        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyInfo[] propInfos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(x => new
            {
                Property = x,
                Attribute = (ECN_Framework_Entities.Activity.Report.ExportAttribute)Attribute.GetCustomAttribute(x, typeof(ECN_Framework_Entities.Activity.Report.ExportAttribute), true)
            })
            .OrderBy(x => x.Attribute != null ? x.Attribute.FieldOrder : -1)
            .Select(x => x.Property)
            .ToArray();

            DataTable table = new DataTable();
            for (int i = 0; i < propInfos.Length; i++)
            {
                PropertyInfo prop = propInfos[i];
                List<System.Reflection.CustomAttributeData> listCA = propInfos[i].CustomAttributes.ToList();
                string headerText = propInfos[i].Name;
                foreach (System.Reflection.CustomAttributeData cad in listCA)
                {
                    if (cad.NamedArguments.Count(x => x.MemberName == "Header") > 0)
                    {
                        headerText = cad.NamedArguments.First(x => x.MemberName == "Header").TypedValue.Value.ToString();
                    }

                }

                table.Columns.Add(headerText, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo prop in propInfos)
                {
                    List<System.Reflection.CustomAttributeData> listCA = prop.CustomAttributes.ToList();
                    string headerText = prop.Name;
                    foreach (System.Reflection.CustomAttributeData cad in listCA)
                    {
                        if (cad.NamedArguments.Count(x => x.MemberName == "Header") > 0)
                        {
                            headerText = cad.NamedArguments.First(x => x.MemberName == "Header").TypedValue.Value.ToString();
                        }

                    }
                    row[headerText] = prop.GetValue(item) ?? DBNull.Value;

                }
                table.Rows.Add(row);
            }
            return table;
        }

        private static string CleanString(string text)
        {

            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            return text;
        }

    }
}
