using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using FrameworkUAD.BusinessLogic;
using KM.Common;
using KMPlatform.Object;
using KMPS.MD.Objects.Helpers;

namespace KMPS.MD.Objects
{
    public class Filter
    {
        public Filter()
        {
            FilterNo = 0;
            Count = 0;
            Fields = new List<Field>();
            ViewType = Enums.ViewType.None;
            PubID = 0;
            BrandID = 0;
            FilterGroupID = 0;
            FilterGroupName = string.Empty;
            Executed = false;
        }

        #region Properties
        public int FilterNo { get; set; }
        public string FilterName { get; set; }
        public bool Executed { get; set; }
        public int Count { get; set; }
        public List<Field> Fields = null;

        public List<Subscriber> Subscriber = null;

        public Enums.ViewType ViewType { get; set; }
        public int PubID { get; set; }
        public int BrandID { get; set; }
        public int FilterGroupID { get; set; }
        public string FilterGroupName { get; set; }
        #endregion

        #region Data

        public void ExecuteFilter(KMPlatform.Object.ClientConnections clientconnection)
        {
            Subscriber = new List<Subscriber>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(getFilterQuery(this, "distinct s.SubscriptionID", "", clientconnection), conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Subscriber c = new Subscriber();
                    c.SubscriptionID = Convert.ToInt32(rdr["SubscriptionID"]);
                    Subscriber.Add(c);
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

            Count = Subscriber.Count();

            var query = Subscriber
                        .GroupBy(x => new { x.SubscriptionID })
                        .Select(g => g.First());
            Subscriber = query.ToList();
            this.Executed = true;
        }
        public void GetCounts(KMPlatform.Object.ClientConnections clientconnection)
        {
            Subscriber = new List<Subscriber>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(getFilterQuery(this, " count(distinct s.SubscriptionID) ", "", clientconnection), conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                Count = int.Parse(cmd.ExecuteScalar().ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            this.Executed = true;
        }

        public void Execute(KMPlatform.Object.ClientConnections clientconnection, string AddlFilters)
        {
            Subscriber = new List<Subscriber>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(getFilterQuery(this, "count(distinct s.SubscriptionID) ", AddlFilters, clientconnection));
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            Count = Convert.ToInt32(DataFunctions.executeScalar(cmd, conn));

            this.Executed = true;
        }

        public static List<int> getSubscriber(KMPlatform.Object.ClientConnections clientconnection, string AddlFilters, Filter f)
        {
            List<int>  Subscriber = new List<int>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(getFilterQuery(f, "distinct s.SubscriptionID", AddlFilters, clientconnection), conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Subscriber.Add(Convert.ToInt32(rdr["SubscriptionID"]));
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

            var query = Subscriber
                        .GroupBy(x => new { x })
                        .Select(g => g.First());

            Subscriber = query.ToList();

            return Subscriber;
        }

        public string getFilterQuery(KMPlatform.Object.ClientConnections clientconnection)
        {
            return (getFilterQuery(this, "distinct s.SubscriptionID", "", clientconnection));
        }

        /// <summary>
        /// Builds an obnoxious query string based on user selections
        /// </summary>
        /// <param name="filter">The <see cref="Filter"/> object.</param>
        /// <param name="selectList">The column's needed or count(distinct igrp_no) for summaries</param>
        /// <param name="additionalFilters">The additional filter query.</param>
        /// <param name="clientConnection">The <see cref="ClientConnections"/> object.</param>
        /// <returns></returns>
        public static string getFilterQuery(
            Filter filter,
            string selectList,
            string additionalFilters,
            ClientConnections clientConnection = null)
        {
            Guard.NotNull(filter, nameof(filter));

            var args = new FilterArgs
            {
                Filter = filter,
                SelectList = selectList,
                AdditionalFilters = additionalFilters
            };

            // Outer Query that will finally run... Get the data needed + any additional tables for the final XLS output

            FilterHelper.UpdateProfileFields(args, clientConnection);
            FilterHelper.UpdateDimensionFields(args);
            FilterHelper.UpdateActivityFields(args);
            FilterHelper.UpdateTempTables(args);
            FilterHelper.UpdateOuterQuery(args);

            var whereCondition = args.Where.Length == 0
                ? string.Empty
                : $" where {args.Where}";

            return $"{args.CreateTempTableQuery}{args.Query}{whereCondition};{args.DropTempTableQuery}";
        }

        public static StringBuilder generateCombinationQuery(Filters fc, string SelectedFilterOperation, string SuppressedFilterOperation, string SelectedFilterNo, string SuppressedFilterNo, string AddlFilters, int pubID, int brandID, KMPlatform.Object.ClientConnections clientconnection)
        {
            StringBuilder sbQuery = new StringBuilder();
            StringBuilder sbResult = new StringBuilder();

            sbQuery.Append("<xml><Queries>");

            List<int> lFilters = new List<int>();

            foreach (Filter f in fc)
            {
                string[] Selected_FilterNo = SelectedFilterNo.Split(',');
                string[] Suppressed_FilterNo = SuppressedFilterNo.Split(',');

                foreach (string s in Selected_FilterNo)
                {
                    if (s != string.Empty)
                    {
                        Filter filter = fc.SingleOrDefault(x => x.FilterNo.ToString() == s);
                        if (!lFilters.Contains(filter.FilterNo))
                        {
                            lFilters.Add(filter.FilterNo);
                            sbQuery.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", filter.FilterNo, Filter.getFilterQuery(filter, " distinct " + filter.FilterNo.ToString() + ", s.SubscriptionID ", AddlFilters, clientconnection)));
                        }
                    }
                }

                foreach (string s in Suppressed_FilterNo)
                {
                    if (s != string.Empty)
                    {
                        Filter filter = fc.SingleOrDefault(x => x.FilterNo.ToString() == s);
                        if (!lFilters.Contains(filter.FilterNo))
                        {
                            lFilters.Add(filter.FilterNo);
                            sbQuery.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", filter.FilterNo, Filter.getFilterQuery(filter, " distinct " + filter.FilterNo.ToString() + ", s.SubscriptionID ", AddlFilters, clientconnection)));
                        }
                    }
                }
            }

            sbResult.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", 1, SelectedFilterNo, SelectedFilterOperation, SuppressedFilterNo, SuppressedFilterOperation, ""));

            sbQuery.Append("</Queries>");
            sbQuery.Append("<Results>");
            sbQuery.Append(sbResult.ToString());
            sbQuery.Append("</Results>");
            sbQuery.Append("</xml>");

            return sbQuery;
        }

        //    public static List<int> getSubscriptionIDforFilterOperation(KMPlatform.Object.ClientConnections clientconnection, Filters fc, string operationIn, string operationNotIn, List<string> Selected_FilterID, List<string> Suppressed_FilterID, string AddlFilters, string AddFilters2, int pubID, int brandID, string masterIDs)
        //    {
        //        List<int> FilteredList = new List<int>();
        //        List<int> FilteredListSelected = new List<int>();
        //        List<int> FilteredListSuppressed = new List<int>();
        //        List<List<int>> allSelectedList = new List<List<int>>();
        //        //List<int> allSuppressedList = new List<int>();
        //        List<List<int>> allSuppressedList = new List<List<int>>();

        //        bool bfirsttime = true;

        //        SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);

        //        if (Selected_FilterID.Count > 0)
        //        {
        //            foreach (string filterID in Selected_FilterID)
        //            {
        //                Filter filter = fc.SingleOrDefault(f => f.FilterNo.ToString() == filterID);

        //                if (AddlFilters != string.Empty)
        //                {
        //                    List<int> sub = getSubscriber(clientconnection, AddlFilters, filter);
        //                    allSelectedList.Add(sub);
        //                }
        //                else
        //                {
        //                    if (!filter.Executed)
        //                    {
        //                        filter.Execute(clientconnection, AddlFilters);
        //                    }
        //                    allSelectedList.Add((from f in filter.Subscriber
        //                                         select f.SubscriptionID).ToList());
        //                }
        //            }

        //            if (allSelectedList.Count > 0)
        //            {
        //                if (operationIn.Equals("UNION", StringComparison.OrdinalIgnoreCase) || operationIn.Equals("SINGLE", StringComparison.OrdinalIgnoreCase))
        //               {
        //                    FilteredListSelected = (from e in allSelectedList
        //                                            from e2 in e
        //                                            select e2).Distinct().ToList();
        //                }
        //                else if (operationIn.Equals("INTERSECT", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    foreach (List<int> l in allSelectedList)
        //                    {
        //                        if (bfirsttime)
        //                        {
        //                            FilteredListSelected = l;
        //                            bfirsttime = false;
        //                        }
        //                        else
        //                            FilteredListSelected = FilteredListSelected.Intersect(l).ToList();
        //                    }
        //                }
        //            }

        //            foreach (string filterID in Suppressed_FilterID)
        //            {
        //                Filter filter = fc.SingleOrDefault(f => f.FilterNo.ToString() == filterID);

        //                if (AddlFilters != string.Empty)
        //                {
        //                    List<int> sub = getSubscriber(clientconnection, AddlFilters, filter);
        //                    allSuppressedList.Add(sub);
        //                }
        //                else
        //                {
        //                    if (!filter.Executed)
        //                    {
        //                        filter.Execute(clientconnection, AddlFilters);
        //                    }
        //                    allSuppressedList.Add((from f in filter.Subscriber
        //                                         select f.SubscriptionID).ToList());
        //                }
        //            }

        //            if (allSuppressedList.Count > 0)
        //            {
        //                if (operationNotIn.Equals("UNION", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    FilteredListSuppressed = (from e in allSuppressedList
        //                                              from e2 in e
        //                                              select e2).Distinct().ToList();
        //                }
        //                else if (operationNotIn.Equals("INTERSECT", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    bfirsttime = true;

        //                    foreach (List<int> l in allSuppressedList)
        //                    {
        //                        if (bfirsttime)
        //                        {
        //                            FilteredListSuppressed = l;
        //                            bfirsttime = false;
        //                        }
        //                        else
        //                            FilteredListSuppressed = FilteredListSuppressed.Intersect(l).ToList();
        //                    }
        //                }
        //            }

        //            #region NOTIN

        //            if (Suppressed_FilterID.Count > 0)
        //            {
        //                FilteredList = FilteredListSelected.Except(FilteredListSuppressed).ToList();
        //            }
        //            else
        //                FilteredList = FilteredListSelected;

        //            if (AddFilters2 != string.Empty)
        //            {
        //                string query = string.Empty;

        //                if (pubID > 0)
        //                {
        //                    query = "select distinct ss.SubscriptionID from Subscriptions ss with (nolock) join pubSubscriptionDetail psd2 with (nolock) on ss.SubscriptionID = psd2.SubscriptionID where psd2.codesheetID IN (" + AddFilters2 + ")";
        //                }
        //                else
        //                {
        //                    if (fc.First().ViewType == Enums.ViewType.RecencyView)
        //                    {
        //                        if (brandID > 0)
        //                            query = "select distinct vrbc2.subscriptionID from vw_RecentBrandConsensus vrbc2 with (nolock) WHERE vrbc2.BrandID = " + brandID + " AND vrbc2.masterid in (" + masterIDs + ")";
        //                        else
        //                            query = "select distinct vrc2.subscriptionID from vw_RecentConsensus vrc2 with (nolock) where vrc2.masterid in (" + AddFilters2 + ")";
        //                    }
        //                    else
        //                    {
        //                        if (brandID > 0)
        //                            query = "select distinct vbc2.subscriptionID from vw_BrandConsensus vbc2 with (nolock) WHERE vbc2.BrandID = " + brandID + " AND vbc2.masterid in (" + masterIDs + ")";
        //                        else
        //                            query = "select distinct sd2.subscriptionID from SubscriptionDetails sd2 with (nolock) where sd2.masterid in (" + AddFilters2 + ")";
        //                    }
        //                }

        //                SqlCommand cmd = new SqlCommand(query, conn);
        //                cmd.CommandTimeout = 20000;
        //                SqlDataReader sdr = null;
        //                conn.Open();
        //                sdr = cmd.ExecuteReader();

        //                while (sdr.Read())
        //                {
        //                    if (!FilteredList.Contains(Convert.ToInt32(sdr["SubscriptionID"].ToString())))
        //                        FilteredList.Add(Convert.ToInt32(sdr["SubscriptionID"].ToString()));
        //                }
        //                sdr.Close();
        //                conn.Close();
        //            }

        //            allSelectedList = null;
        //            allSuppressedList = null;

        //            #endregion
        //        }

        //        return FilteredList;
        //    }

        #endregion
    }

    public class Field
    {
        public Field()
        {
            Name = string.Empty;
            Values = string.Empty;
            Text = string.Empty;
            SearchCondition = string.Empty;
            FilterType = Enums.FiltersType.None;
            Group = string.Empty;
        }

        public Field(string name, string values, string text, string searchcondition, Enums.FiltersType filtertype, string group)
        {
            Name = name;
            Values = values;
            Text = text;
            SearchCondition = searchcondition;
            FilterType = filtertype;
            Group = group;
        }

        public string Name { get; set; }
        public string Values { get; set; }
        public string Text { get; set; }
        public string SearchCondition { get; set; }
        public Enums.FiltersType FilterType { get; set; }
        public string Group { get; set; }
    }
}