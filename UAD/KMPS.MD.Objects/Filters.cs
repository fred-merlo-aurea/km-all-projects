using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using System.Text;
using System.Data.SqlClient;
using KM.Common;
using MoreLinq;

namespace KMPS.MD.Objects
{
    [Serializable]
    public class Filters : ICollection<Filter>
    {
        private const string AllText = "ALL";
        private const string ColumnNameTotal = "Total";
        private const string ColumnNameFilterNo = "FilterNo";
        private const string FilterOperationSingle = "SINGLE";

        public IEnumerator<Filter> GetEnumerator()
        {
            return new FilterEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FilterEnumerator(this);
        }

        //private int UserID { get; set; }

        // The inner collection to store objects.
        private List<Filter> FilterCol;

        private KMPlatform.Object.ClientConnections clientconnection = null;

        // For IsReadOnly
        private bool isRO = false;

        public Filters(KMPlatform.Object.ClientConnections clientsqlconnection, int userID)
        {
            if (userID == 0)
                throw new ArgumentException("Invalid user");

            if (clientsqlconnection == null)
                throw new ArgumentException("Invalid client connection");

            //UserID = userID;
            FilterCol = new List<Filter>();
            this.clientconnection = clientsqlconnection;
        }



        // Adds an index to the collection.
        public Filter this[int index]
        {
            get { return (Filter)FilterCol[index]; }
            set { FilterCol[index] = value; }
        }

        // Determines if an item is in the collection
        // by using the FilterSameDimensions equality comparer.
        public bool Contains(Filter item)
        {
            bool found = false;

            foreach (Filter f in FilterCol)
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
        public bool Contains(Filter item, EqualityComparer<Filter> comp)
        {
            bool found = false;

            foreach (Filter f in FilterCol)
            {
                if (comp.Equals(f, item))
                {
                    found = true;
                }
            }

            return found;
        }

        public void Add(Filter item, bool ExecuteFilter)
        {
            if (!Contains(item))
            {
                if (ExecuteFilter && !item.Executed) //(item.Count == 0)
                {
                    item.GetCounts(clientconnection);
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
        public void Add(Filter item)
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

        public void CopyTo(Filter[] array, int arrayIndex)
        {
            for (int i = 0; i < FilterCol.Count; i++)
            {
                array[i] = (Filter)FilterCol[i];
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

        public void Execute()
        {
            if (FilterCol.Count > 0)
            {
                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("<xml>");

                foreach (Filter filter in FilterCol)
                {
                    sbQuery.Append(string.Format("<query filterno=\"{0}\" ><![CDATA[{1}]]></query>", filter.FilterNo, Filter.getFilterQuery(filter, " distinct " + filter.FilterNo.ToString() + ", s.SubscriptionID ", "", clientconnection)));
                }
                sbQuery.Append("</xml>");

                List<FiltersExecuteResult> filtersexecuteresults = new List<FiltersExecuteResult>();

                SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
                SqlCommand cmd = new SqlCommand("sp_ExecuteFilters_with_Details", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@queries", sbQuery.ToString()));

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
                        foreach (Filter filter in FilterCol)
                        {
                            filter.Count = filtersexecuteresults.Find(x => x.FilterDescription == filter.FilterNo.ToString()).Count;
                        }

                        this.FilterComboList = filtersexecuteresults.Where(f => f.operation.ToLower() == Enums.Operation.Combo.ToString().ToLower()).ToList().ConvertAll(c => new FilterCombo { SelectedFilterNo = c.SelectedFilterNo, SuppressedFilterNo = c.SuppressedFilterNo, SelectedFilterOperation = c.SelectedFilterOperation, SuppressedFilterOperation = c.SuppressedFilterOperation, FilterDescription = c.FilterDescription, Count = c.Count }).OrderBy(x => x.FilterDescription).ToList();
                        this.FilterVennList = filtersexecuteresults.Where(f => f.operation.ToLower() == Enums.Operation.Venn.ToString().ToLower()).ToList().ConvertAll(c => new FilterVenn { SelectedFilterNo = c.SelectedFilterNo, SuppressedFilterNo = c.SuppressedFilterNo, SelectedFilterOperation = c.SelectedFilterOperation, SuppressedFilterOperation = c.SuppressedFilterOperation, FilterDescription = c.FilterDescription, Count = c.Count }).ToList();
                        //this.AllInterSect = filtersexecuteresults.Find(f => f.operation.ToLower() == Enums.Operation.AllIntersect.ToString().ToLower()).Count;
                        //this.AllUnion = filtersexecuteresults.Find(f => f.operation.ToLower() == Enums.Operation.AllUnion.ToString().ToLower()).Count;
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

        public DataTable GetIntersectCrossTabData()
        {
            return GetIntersectOrUnionCrossTabData("intersect");
        }

        public DataTable GetUnionCrossTabData()
        {
            return GetIntersectOrUnionCrossTabData("union");
        }

        private DataTable GetIntersectOrUnionCrossTabData(string filterDescriptionSuffix)
        {
            var dataTable = new DataTable();
            var dataColumn = new DataColumn(ColumnNameFilterNo);
            dataTable.Columns.Add(dataColumn);

            for (var x = 1; x <= FilterCol.Count; x++)
            {
                dataColumn = new DataColumn(x.ToString());
                dataTable.Columns.Add(dataColumn);
            }

            dataColumn = new DataColumn(ColumnNameTotal);
            dataTable.Columns.Add(dataColumn);

            DataRow newRowCrossTab;
            for (var i = 0; i <= FilterCol.Count - 1; i++)
            {
                var outerfilter = FilterCol[i];
                newRowCrossTab = dataTable.NewRow();
                newRowCrossTab[ColumnNameFilterNo] = i + 1;
                for (var j = 0; j <= FilterCol.Count - 1; j++)
                {
                    var innerfilter = FilterCol[j];

                    if (i == j)
                    {
                        newRowCrossTab[(j + 1).ToString()] = FilterComboList
                            .FirstOrDefault(x => x.SelectedFilterNo == outerfilter.FilterNo.ToString() 
                                                && FilterOperationSingle.Equals(x.SelectedFilterOperation, StringComparison.OrdinalIgnoreCase))
                            ?.Count;
                    }
                    else if (j < i)
                    {
                        newRowCrossTab[(j + 1).ToString()] = FilterComboList
                            .FirstOrDefault(x => x.FilterDescription == string.Format(
                                "{0},{1}({2})",
                                innerfilter.FilterNo,
                                outerfilter.FilterNo,
                                filterDescriptionSuffix))
                            ?.Count;
                    }
                    else
                    {
                        newRowCrossTab[(j + 1).ToString()] = FilterComboList
                            .FirstOrDefault(x => x.FilterDescription == string.Format(
                                "{0},{1}({2})", 
                                outerfilter.FilterNo,
                                innerfilter.FilterNo,
                                filterDescriptionSuffix))
                            ?.Count;
                    }
                }

                dataTable.Rows.Add(newRowCrossTab);
            }

            newRowCrossTab = dataTable.NewRow();
            newRowCrossTab[ColumnNameFilterNo] = AllText;
            newRowCrossTab[ColumnNameTotal] = FilterComboList
                .Where(x => string.Format(
                        "{0} {1}", 
                        AllText, 
                        filterDescriptionSuffix)
                    .Equals(x.FilterDescription, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault()
                .Count;
            dataTable.Rows.Add(newRowCrossTab);

            return dataTable;
        }

        public DataTable GetNotInCrossTabData()
        {
            int i = 0;
            #region Build Cross Tab Table

            DataTable dtCT = new DataTable();

            DataColumn dcCT;
            DataRow newRowCT;

            dcCT = new DataColumn("FilterNo");
            dtCT.Columns.Add(dcCT);

            for (int x = 1; x <= FilterCol.Count; x++)
            {
                dcCT = new DataColumn(x.ToString());
                dtCT.Columns.Add(dcCT);
            }

            for (i = 0; i <= FilterCol.Count - 1; i++)
            {
                Filter outerfilter = FilterCol[i];
                newRowCT = dtCT.NewRow();
                newRowCT["Filterno"] = i + 1;
                for (int j = 0; j <= FilterCol.Count - 1; j++)
                {
                    Filter innerfilter = FilterCol[j];
                    if (i == j)
                        newRowCT[(j + 1).ToString()] = "0";
                    else
                        newRowCT[(j + 1).ToString()] = FilterComboList.Where(x => x.FilterDescription == outerfilter.FilterNo.ToString() + " NOT IN " + innerfilter.FilterNo.ToString()).FirstOrDefault().Count;
                }

                dtCT.Rows.Add(newRowCT);
            }

            return dtCT;

            #endregion
        }

        public bool IsReadOnly
        {
            get { return isRO; }
        }

        public bool Remove(Filter item)
        {
            bool result = false;

            // Iterate the inner collection to 
            // find the Filter to be removed.
            for (int i = 0; i < FilterCol.Count; i++)
            {
                Filter curFilter = (Filter)FilterCol[i];

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


        public void Update(Filter f)
        {

            for (int i = 0; i < FilterCol.Count; i++)
            {
                Filter curFilter = (Filter)FilterCol[i];

                if (curFilter.FilterNo == f.FilterNo)
                {
                    FilterCol[i] = f;
                    break;
                }
            }
        }

        public void Update(Filter f, bool ExecuteFilter)
        {
            for (int i = 0; i < FilterCol.Count; i++)
            {
                Filter curFilter = (Filter)FilterCol[i];

                if (curFilter.FilterNo == f.FilterNo)
                {
                    FilterCol[i] = f;

                    if (ExecuteFilter) //(item.Count == 0)
                    {
                        f.GetCounts(clientconnection);
                        f.Executed = true;
                    }

                    break;
                }
            }
        }


        public bool Remove(string FilterID)
        {
            Filter filter = FilterCol.SingleOrDefault(f => f.FilterNo.ToString() == FilterID);

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

        public DataTable GetCrossTabData(string label)
        {
            List<Filter> Filters = new List<Filter>();

            foreach (Filter filter in this)
            {
                List<int> l = new List<int>();

                if (!filter.Executed) // (filter.Subscriber.Count == 0)
                {
                    filter.ExecuteFilter(clientconnection);
                }

                var query = (from s in filter.Subscriber
                             select s.SubscriptionID);

                Filters.Add(filter);
            }

            List<List<Filter>> combinations = produceCombinations(Filters);

            return BuildDataTable(combinations, Filters, label);
        }

        public List<int> DownloadCrossTabData(string DownloadCombination)
        {
            List<int> Subcriberlist = null;

            foreach (Filter filter in FilterCol)
            {
                if (!filter.Executed) // (filter.Subscriber.Count == 0)
                {
                    filter.Execute(clientconnection, "");
                }
            }


            if (DownloadCombination != string.Empty)
            {

                List<List<Filter>> generatedCombinations = produceCombinations(FilterCol).OrderBy(p => p.Count).Reverse().ToList();

                List<int> alreadyCounted = new List<int>();

                foreach (var Combination in generatedCombinations)
                {
                    if (Combination.Count > 0)
                    {
                        List<List<int>> allList = new List<List<int>>();

                        foreach (Filter filter in Combination)
                        {
                            var query = (from s in filter.Subscriber
                                         select s.SubscriptionID);

                            allList.Add(query.ToList());
                        }

                        var intersection = allList.Aggregate((previousList, nextList) => previousList.Intersect(nextList).ToList());

                        Subcriberlist = intersection.Except(alreadyCounted).ToList();

                        int intersectionCount = Subcriberlist.Count();

                        if (intersectionCount > 0)
                        {
                            foreach (int a in intersection)
                            {
                                alreadyCounted.Add(a);
                            }
                            string combination = "";

                            foreach (Filter cf in Combination)
                            {
                                int x = 0;
                                foreach (Filter f in FilterCol)
                                {
                                    if (f.Equals(cf))
                                    {
                                        combination += combination == string.Empty ? x.ToString() : "," + x.ToString();
                                    }
                                    x++;
                                }
                            }

                            if (combination == DownloadCombination)
                            {
                                return Subcriberlist;
                            }
                        }
                    }

                }
            }
            else
            {
                List<int> allList = new List<int>();

                foreach (Filter filter in FilterCol)
                {
                    var query = (from s in filter.Subscriber
                                 select s.SubscriptionID).ToList<int>();

                    allList.AddRange(query);
                }
                return allList.Distinct().ToList();
            
            }

            return Subcriberlist;
        }

        private DataTable BuildDataTable(List<List<Filter>> combinations, List<Filter> collectionOfSeries, string label)
        {

            #region Build Cross Tab Table columns

            DataTable dtCT = new DataTable();

            DataColumn dcCT;
            DataRow newRowCT;

            dcCT = new DataColumn("Combination");
            dtCT.Columns.Add(dcCT);
            dcCT = new DataColumn("Counts");
            dtCT.Columns.Add(dcCT);
            dcCT = new DataColumn("SelectedFilterNo");
            dtCT.Columns.Add(dcCT);
            dcCT = new DataColumn("SelectedOperation");
            dtCT.Columns.Add(dcCT);
            dcCT = new DataColumn("SuppressedFilterNo");
            dtCT.Columns.Add(dcCT);
            dcCT = new DataColumn("SuppressedOperation");
            dtCT.Columns.Add(dcCT);

            int k = 0;
            foreach (Filter filter in collectionOfSeries)
            {
                dtCT.Columns.Add(new DataColumn(k.ToString()));
                k++;
            }

            dcCT = new DataColumn(label);
            dtCT.Columns.Add(dcCT);
            #endregion

            List<List<Filter>> generatedCombinations = combinations.OrderBy(p => p.Count).Reverse().ToList();

            List<int> alreadyCounted = new List<int>();

            foreach (var generatedCombination in generatedCombinations)
            {
                string allSelectedFilterNo = string.Empty;
                string allSuppressedFilterNo = string.Empty;

                if (generatedCombination.Count > 0)
                {
                    List<List<int>> allList = new List<List<int>>();

                    foreach (Filter filter in generatedCombination)
                    {
                        var query = (from s in filter.Subscriber
                                     select s.SubscriptionID);

                        allList.Add(query.ToList());

                    }

                    var intersection = allList.Aggregate((previousList, nextList) => previousList.Intersect(nextList).ToList());

                    int intersectionCount = intersection.Except(alreadyCounted).Count();

                    if (intersectionCount > 0)
                    {
                        newRowCT = dtCT.NewRow();
                        newRowCT["Counts"] = intersectionCount;

                        foreach (int a in intersection)
                        {
                            alreadyCounted.Add(a);
                        }
                        string combination = "";

                        foreach (Filter i in generatedCombination)
                        {
                            int x = 0;

                            foreach (Filter f in collectionOfSeries)
                            {
                                if (f.Equals(i))
                                {
                                    combination += combination == string.Empty ? x.ToString() : "," + x.ToString();
                                    newRowCT[x.ToString()] = "X";
                                    allSelectedFilterNo += allSelectedFilterNo == string.Empty ? f.FilterNo.ToString() : "," + f.FilterNo.ToString();
                                }
                                x++;
                            }
                        }

                        string[] s = allSelectedFilterNo.Split(',');

                        foreach (Filter f in collectionOfSeries)
                        {
                            if (!s.Contains(f.FilterNo.ToString()))
                            {
                                allSuppressedFilterNo += allSuppressedFilterNo == string.Empty ? f.FilterNo.ToString() : "," + f.FilterNo.ToString();
                            }
                        }

                        newRowCT["Combination"] = combination;
                        newRowCT["SelectedFilterNo"] = allSelectedFilterNo;

                        if (allSelectedFilterNo.Contains(","))
                            newRowCT["SelectedOperation"] = "intersect";
                        else
                            newRowCT["SelectedOperation"] = "";

                        newRowCT["SuppressedFilterNo"] = allSuppressedFilterNo;

                        if (allSuppressedFilterNo.Contains(","))
                            newRowCT["SuppressedOperation"] = "union";
                        else
                            newRowCT["SuppressedOperation"] = "";

                        newRowCT[label] = generatedCombination.Count();
                        dtCT.Rows.Add(newRowCT);
                    }
                }
            }

            int y = 6;
            foreach (Filter filter in collectionOfSeries)
            {
                dtCT.Columns[y].ColumnName = filter.FilterName;// field.DisplayName;
                y++;
                //List<Field> Fields = filter.Fields;


                //foreach (Field field in Fields)
                //{
                //    dtCT.Columns[y].ColumnName = System.Guid.NewGuid().ToString();// field.DisplayName;
                //    y++;
                //}
            }

            return dtCT;
        }

        private IEnumerable<int> constructSetFromBits(int i)
        {
            for (int n = 0; i != 0; i /= 2, n++)
            {
                if ((i & 1) != 0)
                    yield return n;
            }
        }

        private IEnumerable<List<Filter>> produceEnumeration(List<Filter> collectionOfSeries)
        {
            for (int i = 0; i < (1 << collectionOfSeries.Count); i++)
            {
                yield return constructSetFromBits(i).Select(n => collectionOfSeries[n]).ToList();
            }
        }

        private List<List<Filter>> produceCombinations(List<Filter> collectionOfSeries)
        {
            return produceEnumeration(collectionOfSeries).ToList();
        }
    }

    public class FilterEnumerator : IEnumerator<Filter>
    {
        private Filters _collection;
        private int curIndex;
        private Filter curFilter;

        public FilterEnumerator(Filters collection)
        {
            _collection = collection;
            curIndex = -1;
            curFilter = default(Filter);

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

        public Filter Current
        {
            get { return curFilter; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}