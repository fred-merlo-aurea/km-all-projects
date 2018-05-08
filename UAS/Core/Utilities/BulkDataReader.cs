using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Core_AMS.Utilities
{
    public class BulkDataReader<T> : IDataReader
    {
        private IEnumerator<T> list = null;
		private List<PropertyInfo> properties = new List<PropertyInfo>();

		public BulkDataReader(IEnumerable<T> list)
		{
			this.list = list.GetEnumerator();
			foreach (PropertyInfo property in typeof(T).GetProperties(
				BindingFlags.GetProperty |
				BindingFlags.Instance |
				BindingFlags.Public))
			{
				if (
					property.PropertyType.IsPrimitive ||
					property.PropertyType == typeof(string) ||
					property.PropertyType == typeof(DateTime)
					)
				{
					properties.Add(property);
				}
			}
            
            //--- convert decimal values
            //--- loop through all rows and convert the values to data types that match our database's data type for that field
            //foreach (DataRow dr in dtData.Rows)
            //{
            //    foreach (DataColumn DecCol in DecimalColumns)
            //    {
            //        if (string.IsNullOrEmpty(dr[DecCol].ToString()))
            //            dr[DecCol] = null; //--- this had to be set to null, not empty
            //        else
            //            dr[DecCol] = Helpers.CleanDecimal(dr[DecCol].ToString());
            //    }
            //}
		}
        public static DataTable ToDataTable(IList<T> myList)
        {
            System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                System.ComponentModel.PropertyDescriptor prop = props[i];
                //table.Columns.Add(prop.Name, prop.PropertyType);
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in myList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(values);
            }
            return table;
        }
		#region IDataReader Members

		public void Close()
		{
			list.Dispose();
		}

		public int Depth
		{
			get { throw new NotImplementedException(); }
		}

		public DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		public bool IsClosed
		{
			get { throw new NotImplementedException(); }
		}

		public bool NextResult()
		{
			throw new NotImplementedException();
		}

		public bool Read()
		{
			return list.MoveNext();
		}

		public int RecordsAffected
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Close();
		}

		#endregion

		#region IDataRecord Members

		public int FieldCount
		{
			get { return properties.Count; }
		}

		public bool GetBoolean(int i)
		{
			throw new NotImplementedException();
		}

		public byte GetByte(int i)
		{
			throw new NotImplementedException();
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public char GetChar(int i)
		{
			throw new NotImplementedException();
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public IDataReader GetData(int i)
		{
			throw new NotImplementedException();
		}

		public string GetDataTypeName(int i)
		{
			throw new NotImplementedException();
		}

		public DateTime GetDateTime(int i)
		{
			throw new NotImplementedException();
		}

		public decimal GetDecimal(int i)
		{
			throw new NotImplementedException();
		}

		public double GetDouble(int i)
		{
			throw new NotImplementedException();
		}

		public Type GetFieldType(int i)
		{
			return properties[i].PropertyType;
		}

		public float GetFloat(int i)
		{
			throw new NotImplementedException();
		}

		public Guid GetGuid(int i)
		{
			throw new NotImplementedException();
		}

		public short GetInt16(int i)
		{
			throw new NotImplementedException();
		}

		public int GetInt32(int i)
		{
			throw new NotImplementedException();
		}

		public long GetInt64(int i)
		{
			throw new NotImplementedException();
		}

		public string GetName(int i)
		{
			return properties[i].Name;
		}

		public int GetOrdinal(string name)
		{
			throw new NotImplementedException();
		}

		public string GetString(int i)
		{
			throw new NotImplementedException();
		}

		public object GetValue(int i)
		{
			return properties[i].GetValue(list.Current, null);
		}

		public int GetValues(object[] values)
		{
			throw new NotImplementedException();
		}

		public bool IsDBNull(int i)
		{
			throw new NotImplementedException();
		}

		public object this[string name]
		{
			get { throw new NotImplementedException(); }
		}

		public object this[int i]
		{
			get { throw new NotImplementedException(); }
		}

		#endregion
	}
}
