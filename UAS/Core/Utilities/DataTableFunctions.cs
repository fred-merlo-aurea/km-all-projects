using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Core_AMS.Utilities
{
    public class DataTableFunctions
    {
        public static string GetDisplayName<T>(string propertyName)
        {
            MemberInfo mi = typeof(T).GetProperty(propertyName);
            var displayName = mi.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            if (displayName != null)
                return displayName.Name;
            else
                return propertyName;
        }
        public static string GetDisplayName<TSource, TProperty>(Expression<Func<TSource, TProperty>> expression)
        {
            var attribute = Attribute.GetCustomAttribute(((MemberExpression) expression.Body).Member, typeof(DisplayAttribute)) as DisplayAttribute;
            if (attribute == null)
            {
                throw new ArgumentException(String.Format("Expression '{0}' doesn't have DisplayAttribute", expression));
            }
            return attribute.GetName();
        }
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public static DataTable ToDataTable<T>(IList<T> data, List<string> columns)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            columns.ForEach(s => { table.Columns.Add(s); });
            //why use reflection?  having them pass column list
            //foreach (PropertyDescriptor prop in properties)
            //{
            //    if (columns.Exists(x => x.Equals(prop.Name, StringComparison.CurrentCultureIgnoreCase)))
            //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            //}
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                //here I need reflection to get values
                foreach (PropertyDescriptor prop in properties)
                {
                    //if prop is of type LIST lets call ToDataTable again
                    //would need to know type of List, not worth time right now
                    if (columns.Exists(x => x.Equals(prop.Name, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        row[prop.Name] = prop.GetValue(item) ?? string.Empty;//dr[fm.IncomingField] = myRow[fm.IncomingField] == null ? "" : myRow[fm.IncomingField].ToString();
                        table.Rows.Add(row);
                    }
                }
            }
            return table;
        }
        public static DataTable ToDataTable<T1, T2>(IList<T1> data1, List<string> columns, List<string> tmpDemoColumns)
        {
            PropertyDescriptorCollection properties1 = TypeDescriptor.GetProperties(typeof(T1));
            PropertyDescriptorCollection properties2 = TypeDescriptor.GetProperties(typeof(T2));
            DataTable table = new DataTable();
            columns.ForEach(s => { table.Columns.Add(s); });

            foreach (T1 item1 in data1)
            {
                DataRow row = table.NewRow();
                //here I need reflection to get values
                foreach (PropertyDescriptor prop1 in properties1)
                {
                    //MemberInfo mi1 = typeof(T1).GetProperty(prop1.Name);
                    //var dd1 = mi1.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                    string name1 =GetDisplayName<T1>(prop1.Name);
                    //if (dd1 != null)
                    //    name1 = dd1.Name;
                    if (columns.Exists(x => x.Equals(name1, StringComparison.CurrentCultureIgnoreCase)))
                        row[name1] = prop1.GetValue(item1).ToString() ?? string.Empty;

                    if (prop1.GetType() == typeof(IList<T2>))
                    {
                        foreach (T2 item2 in (IList<T2>)prop1.GetValue(item1))
                        {

                            PropertyDescriptor propName = properties2["MAFField"];
                            if (propName != null)
                            {
                                PropertyDescriptor propValue = properties2["Value"];
                                string demoName = propName.GetValue(item2).ToString();
                                if (tmpDemoColumns.Exists(x => x.Equals(demoName, StringComparison.CurrentCultureIgnoreCase)))
                                    row[demoName] = propValue.GetValue(item2).ToString() ?? string.Empty;
                            }

                            //foreach (PropertyDescriptor prop2 in properties2)
                            //{
                            //    //MemberInfo mi2 = typeof(T2).GetProperty(prop2.Name);
                            //    //var dd2 = mi2.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                            //    string name2 = GetDisplayName<T2>(prop2.Name);//on Demo tables this will be MAFField
                            //    if(name2 == "MAFFField")
                            //    {
                            //        if (columns.Exists(x => x.Equals(name2, StringComparison.CurrentCultureIgnoreCase)))
                            //            row[name2] = prop2.GetValue(item2).ToString() ?? string.Empty;
                            //    }
                            //    //if (dd2 != null)
                            //    //    name2 = dd2.Name;

                            //    if (columns.Exists(x => x.Equals(name2, StringComparison.CurrentCultureIgnoreCase)))
                            //        row[name2] = prop2.GetValue(item2).ToString() ?? string.Empty;
                            //}
                        }
                    }
                }

                table.Rows.Add(row);
            }
            return table;
        }
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var properties = typeof(T).GetProperties();

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();

                foreach (var pro in properties)
                {
                    if (pro.Name != null && columnNames.Contains(pro.Name))
                        pro.SetValue(objT, row[pro.Name]);
                }

                return objT;
            }).ToList();

        }

    }
}
