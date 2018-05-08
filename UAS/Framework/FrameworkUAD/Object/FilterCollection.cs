using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using KM.Common;
using DataFunctions = FrameworkUAD.DataAccess.DataFunctions;

namespace FrameworkUAD.Object
{
    public class FilterCollection : ICollection<Object.FilterMVC>
    {
        public IEnumerator<Object.FilterMVC> GetEnumerator()
        {
            return new FilterEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FilterEnumerator(this);
        }

        //private int UserID { get; set; }

        // The inner collection to store objects.
        private List<Object.FilterMVC> FilterCol;

        private KMPlatform.Object.ClientConnections clientconnection = null;

        // For IsReadOnly
        private bool isRO = false;

        public FilterCollection(KMPlatform.Object.ClientConnections clientsqlconnection, int userID)
        {
            if (userID == 0)
                throw new ArgumentException("Invalid user");

            if (clientsqlconnection == null)
                throw new ArgumentException("Invalid client connection");

            //UserID = userID;
            FilterCol = new List<Object.FilterMVC>();
            this.clientconnection = clientsqlconnection;
        }



        // Adds an index to the collection.
        public Object.FilterMVC this[int index]
        {
            get { return (Object.FilterMVC) FilterCol[index]; }
            set { FilterCol[index] = value; }
        }

        // Determines if an item is in the collection
        // by using the FilterSameDimensions equality comparer.
        public bool Contains(Object.FilterMVC item)
        {
            bool found = false;

            foreach (Object.FilterMVC f in FilterCol)
            {
                // Equality defined by the Filter
                // class's implmentation of IEquitable<T>.
                //if (f.FilterXML().Equals(item.FilterXML(), StringComparison.OrdinalIgnoreCase))
                //{
                //    found = true;
                //}
            }

            return found;
        }

        // Determines if an item is in the 
        // collection by using a specified equality comparer.
        public bool Contains(Object.FilterMVC item, EqualityComparer<Object.FilterMVC> comp)
        {
            bool found = false;

            foreach (Object.FilterMVC f in FilterCol)
            {
                if (comp.Equals(f, item))
                {
                    found = true;
                }
            }

            return found;
        }

        public void Add(Object.FilterMVC item, bool ExecuteFilter)
        {
            if (!Contains(item))
            {
                if (ExecuteFilter && !item.Executed) //(item.Count == 0)
                {
                    item= BusinessLogic.FilterMVC.GetCounts(clientconnection, item);
                    item.Executed = true;
                }
                FilterCol.Add(item);
            }
            else
            {
                throw new DuplicateFilterException("Filter Already Exists");
            }
        }


        // Adds an item if it is not already in the collection
        // as determined by calling the Contains method.
        public void Add(Object.FilterMVC item)
        {
            if (!Contains(item))
            {
                FilterCol.Add(item);
            }
            else
            {
                throw new DuplicateFilterException("Filter Already Exists");
            }
        }

        public void Clear()
        {
            FilterCol.Clear();
        }

        public void CopyTo(Object.FilterMVC[] array, int arrayIndex)
        {
            for (int i = 0; i < FilterCol.Count; i++)
            {
                array[i] = (Object.FilterMVC) FilterCol[i];
            }
        }

        public int Count
        {
            get
            {
                return FilterCol.Count;
            }
        }

        public List<FilterCombo> FilterComboList
        {
            get; set;
        }

        public List<FilterVenn> FilterVennList
        {
            get; set;
        }

        public int AllInterSect
        {
            get; set;
        }

        public int AllUnion
        {
            get; set;
        }

        public string VennCords
        {
            get
            {
                return string.Join(",", FilterVennList.Select(x => x.FilterDescription));
            }
        }

        public bool IsReadOnly
        {
            get { return isRO; }
        }

        public bool Remove(FilterMVC item)
        {
            bool result = false;

            // Iterate the inner collection to 
            // find the Filter to be removed.
            for (int i = 0; i < FilterCol.Count; i++)
            {
                FilterMVC curFilter = (FilterMVC) FilterCol[i];

                if (curFilter.FilterNo == item.FilterNo)
                {
                    FilterCol.RemoveAt(i);

                    result = true;
                    break;
                }
            }

            if (FilterCol.Count == 0)
                FilterCol.Clear();

            return result;
        }


        public void Update(FilterMVC f)
        {

            for (int i = 0; i < FilterCol.Count; i++)
            {
                FilterMVC curFilter = (FilterMVC) FilterCol[i];

                if (curFilter.FilterNo == f.FilterNo)
                {
                    FilterCol[i] = f;
                    break;
                }
            }
        }

        public void Update(FilterMVC f, bool ExecuteFilter)
        {
            for (int i = 0; i < FilterCol.Count; i++)
            {
                FilterMVC curFilter = (FilterMVC) FilterCol[i];

                if (curFilter.FilterNo == f.FilterNo)
                {
                    FilterCol[i] = f;

                    if (ExecuteFilter) //(item.Count == 0)
                    {
                        f = BusinessLogic.FilterMVC.GetCounts(clientconnection, f);
                        f.Executed = true;
                       
                    }

                    break;
                }
            }
        }


        public bool Remove(string FilterID)
        {
            FilterMVC filter = FilterCol.SingleOrDefault(f => f.FilterNo.ToString() == FilterID);

            return this.Remove(filter);

        }

        public IEnumerable<IEnumerable<T>> GetPowerSet<T>(List<T> list)
        {
            return from m in Enumerable.Range(0, 1 << list.Count)
                   select
                       from i in Enumerable.Range(0, list.Count)
                       where (m & (1 << i)) != 0
                       select list[i];
        }
    }
    public class FilterEnumerator : IEnumerator<Object.FilterMVC>
    {
        private FilterCollection _collection;
        private int curIndex;
        private Object.FilterMVC curFilter;

        public FilterEnumerator(FilterCollection collection)
        {
            _collection = collection;
            curIndex = -1;
            curFilter = default(Object.FilterMVC);

        }

        public bool MoveNext()
        {
            //Avoids going beyond the end of the collection.
            if (++curIndex >= _collection.Count)
            {
                return false;
            }
            else
            {
                // Set current Filter to next item in collection.
                curFilter = _collection[curIndex];
            }
            return true;
        }

        public void Reset() { curIndex = -1; }

        void IDisposable.Dispose() { }

        public Object.FilterMVC Current
        {
            get { return curFilter; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
    public class FilterCombo
    {
        #region Properties
        public string SelectedFilterNo { get; set; }
        public string SuppressedFilterNo { get; set; }
        public string SelectedFilterOperation { get; set; }
        public string SuppressedFilterOperation { get; set; }
        public string FilterDescription { get; set; }
        public int Count { get; set; }
        #endregion

        public FilterCombo()
        {
            SelectedFilterNo = string.Empty;
            SuppressedFilterNo = string.Empty;
            SelectedFilterOperation = string.Empty;
            SuppressedFilterOperation = string.Empty;
            FilterDescription = string.Empty;
            Count = 0;
        }

    }
    public class FilterVenn
    {
        #region Properties
        public string SelectedFilterNo { get; set; }
        public string SuppressedFilterNo { get; set; }
        public string SelectedFilterOperation { get; set; }
        public string SuppressedFilterOperation { get; set; }
        public string FilterDescription { get; set; }
        public int Count { get; set; }
        #endregion

        public FilterVenn()
        {
            SelectedFilterNo = string.Empty;
            SuppressedFilterNo = string.Empty;
            SelectedFilterOperation = string.Empty;
            SuppressedFilterOperation = string.Empty;
            FilterDescription = string.Empty;
            Count = 0;
        }
    }
    public class FiltersExecuteResult
    {
        public string SelectedFilterNo { get; set; }
        public string SuppressedFilterNo { get; set; }
        public string SelectedFilterOperation { get; set; }
        public string SuppressedFilterOperation { get; set; }
        public string FilterDescription { get; set; }
        public string operation { get; set; }
        public int Count { get; set; }
    }
    public class DuplicateFilterException : Exception
    {
        public DuplicateFilterException(string errorMessage) : base(errorMessage) { }
        public DuplicateFilterException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
    }
    public class FilterNoRecordsException : Exception
    {
        public FilterNoRecordsException(string errorMessage) : base(errorMessage) { }
        public FilterNoRecordsException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
    }
    public class InvalidZipCodeException : Exception
    {
        public InvalidZipCodeException(string errorMessage) : base(errorMessage) { }
        public InvalidZipCodeException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
    }
    public class Helper
    {
        public static string ReplaceSingleQuotes(string text)
        {
            text = text.Replace("'", "''");
            return text;
        }
    }
    
}
