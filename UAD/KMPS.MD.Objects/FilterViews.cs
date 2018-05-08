using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    public class FilterViews : ICollection<filterView>
    {
        public IEnumerator<filterView> GetEnumerator()
        {
            return new FilterViewEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FilterViewEnumerator(this);
        }

        private KMPlatform.Object.ClientConnections clientconnection = null;

        // For IsReadOnly
        private bool isRO = false;

        // The inner collection to store objects.
        private List<filterView> FilterViewCol;

        public FilterViews(KMPlatform.Object.ClientConnections clientsqlconnection, int userID)
        {
            if (userID == 0)
                throw new ArgumentException("Invalid user");

            if (clientsqlconnection == null)
                throw new ArgumentException("Invalid client connection");

            //UserID = userID;
            FilterViewCol = new List<filterView>();
            this.clientconnection = clientsqlconnection;
        }

        // Adds an index to the collection.
        public filterView this[int index]
        {
            get { return (filterView)FilterViewCol[index]; }
            set { FilterViewCol[index] = value; }
        }

        // Determines if an item is in the collection
        // by using the FilterViewSameDimensions equality comparer.
        public bool Contains(filterView item)
        {
            bool found = false;

            foreach (filterView fv in FilterViewCol)
            {

            }

            return found;
        }

        // Determines if an item is in the 
        // collection by using a specified equality comparer.
        public bool Contains(filterView item, EqualityComparer<filterView> comp)
        {
            bool found = false;

            foreach (filterView f in FilterViewCol)
            {
                if (comp.Equals(f, item))
                {
                    found = true;
                }
            }

            return found;
        }

        // Adds an item if it is not already in the collection
        // as determined by calling the Contains method.
        public void Add(filterView item)
        {
            if (!Contains(item))
            {
                FilterViewCol.Add(item);
            }
            else
            {
                throw new DuplicateFilterException("Filter View Already Exists");
            }
        }

        public void Update(filterView item)
        {

            for (int i = 0; i < FilterViewCol.Count; i++)
            {
                filterView curFilter = (filterView)FilterViewCol[i];

                if (curFilter.FilterViewNo == item.FilterViewNo)
                {
                    FilterViewCol[i] = item;
                    break;
                }
            }
        }

        public void Clear()
        {
            FilterViewCol.Clear();
        }

        public void CopyTo(filterView[] array, int arrayIndex)
        {
            for (int i = 0; i < FilterViewCol.Count; i++)
            {
                array[i] = (filterView)FilterViewCol[i];
            }
        }

        public int Count
        {
            get
            {
                return FilterViewCol.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return isRO; }
        }

        public bool Remove(filterView item)
        {
            bool result = false;

            // Iterate the inner collection to 
            // find the FilterView to be removed.
            for (int i = 0; i < FilterViewCol.Count; i++)
            {
                filterView curFilter = (filterView)FilterViewCol[i];

                if (curFilter.SelectedFilterNo == item.SelectedFilterNo && curFilter.SelectedFilterOperation == item.SelectedFilterOperation && curFilter.SuppressedFilterNo == item.SuppressedFilterNo && curFilter.SuppressedFilterOperation == item.SuppressedFilterOperation)
                {
                    FilterViewCol.RemoveAt(i);

                    result = true;
                    break;
                }
            }

            return result;
        }

        public List<FilterVenn> FilterVennList
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

        public void Execute(Filters FilterCol, string AddlFilter = "")
        {
            StringBuilder sbQuery = new StringBuilder();
            StringBuilder sbResult = new StringBuilder();
            int i = 0;

            sbQuery.Append("<xml><Queries>");

            List<int> lFilters = new List<int>();

            foreach (filterView v in FilterViewCol)
            {
                i++;

                string[] Selected_FilterNo = v.SelectedFilterNo.Split(',');
                string[] Suppressed_FilterNo = v.SuppressedFilterNo.Split(',');

                foreach (string s in Selected_FilterNo)
                {
                    if (s != string.Empty)
                    {
                        Filter filter = FilterCol.SingleOrDefault(f => f.FilterNo.ToString() == s);
                        if (!lFilters.Contains(filter.FilterNo))
                        {
                            lFilters.Add(filter.FilterNo);
                            sbQuery.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", filter.FilterNo, Filter.getFilterQuery(filter, " distinct " + filter.FilterNo.ToString() + ", s.SubscriptionID ", AddlFilter, clientconnection)));
                        }
                    }
                }

                foreach (string s in Suppressed_FilterNo)
                {
                    if (s != string.Empty)
                    {
                        Filter filter = FilterCol.SingleOrDefault(f => f.FilterNo.ToString() == s);
                        if (!lFilters.Contains(filter.FilterNo))
                        {
                            lFilters.Add(filter.FilterNo);
                            sbQuery.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", filter.FilterNo, Filter.getFilterQuery(filter, " distinct " + filter.FilterNo.ToString() + ", s.SubscriptionID ", AddlFilter, clientconnection)));
                        }
                    }
                }

                sbResult.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", i, v.SelectedFilterNo, v.SelectedFilterOperation, v.SuppressedFilterNo, v.SuppressedFilterOperation, ""));
            }

            sbQuery.Append("</Queries>");
            sbQuery.Append("<Results>");
            sbQuery.Append(sbResult.ToString());
            sbQuery.Append("</Results>");
            sbQuery.Append("</xml>");

            List<FiltersExecuteResult> filtersexecuteresults = new List<FiltersExecuteResult>();

            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_ExecuteFiltersegmentation_with_Details", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Queries", sbQuery.ToString()));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FiltersExecuteResult> builder = DynamicBuilder<FiltersExecuteResult>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    FiltersExecuteResult x = builder.Build(rdr);
                    filtersexecuteresults.Add(x);
                }

                if (filtersexecuteresults.Count > 0)
                {
                    foreach (filterView filterview in FilterViewCol)
                    {
                        //var query = filtersexecuteresults.Where(x => x.FilterDescription == filterview.FilterDescription.ToString()).ToList();
                        var query = filtersexecuteresults.Where(x => x.SelectedFilterNo == filterview.SelectedFilterNo.ToString() &&  x.SelectedFilterOperation == filterview.SelectedFilterOperation && x.SuppressedFilterNo == filterview.SuppressedFilterNo && x.SuppressedFilterOperation == filterview.SuppressedFilterOperation).ToList();

                        if (query.Any())
                            filterview.Count = query.FirstOrDefault().Count;
                    }

                    this.FilterVennList = filtersexecuteresults.Where(f => f.operation.ToLower() == Enums.Operation.Venn.ToString().ToLower()).ToList().ConvertAll(c => new FilterVenn { SelectedFilterNo = c.SelectedFilterNo, SuppressedFilterNo = c.SuppressedFilterNo, SelectedFilterOperation = c.SelectedFilterOperation, SuppressedFilterOperation = c.SuppressedFilterOperation, FilterDescription = c.FilterDescription, Count = c.Count }).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }

    public class FilterViewEnumerator : IEnumerator<filterView>
    {
        private FilterViews _collection;
        private int curIndex;
        private filterView curFilter;

        public FilterViewEnumerator(FilterViews collection)
        {
            _collection = collection;
            curIndex = -1;
            curFilter = default(filterView);

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
                // Set current FilterView to next item in collection.
                curFilter = _collection[curIndex];
            }
            return true;
        }

        public void Reset() { curIndex = -1; }

        void IDisposable.Dispose() { }

        public filterView Current
        {
            get { return curFilter; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
