using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Standards.Helpers
{
    public class GridFilter
    {
      
        public string FilterColumnName { get; private set; }
        public string FilterColumnValue { get; private set; }

        public GridFilter(string fcn, string fcv)
        {
            this.FilterColumnName = fcn;
            this.FilterColumnValue = fcv;

        }

        public GridFilter(string member, object value)
        {
            this.FilterColumnName = member;
            this.FilterColumnValue = (string)value;
        }
    }
    public class GridSort
    {
        public string SortColumnName { get; private set; }
        public string SortDirection { get; private set; }

        public GridSort(string scn, string sd)
        {
            this.SortColumnName = scn;
            this.SortDirection = sd;
        }
    }

    interface IGridHelper<T>
    {
        List<GridSort> GetGridSort(DataSourceRequest request, string FirstColumnName);
        List<GridFilter> GetGridFilter(DataSourceRequest request);
     
    }

    public class KendoGridHelper<T> : IGridHelper<T>
    {
        /// <summary>
        /// Get FilterColumnname and Filter Column object as value
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<GridFilter> GetGridFilter(DataSourceRequest request)
        {
            List<GridFilter> listFilter = new List<GridFilter>();
            if (request.Filters != null && request.Filters.Count > 0)
            {
                foreach (var filter in request.Filters)
                {

                    var descriptor = filter as FilterDescriptor;
                    if (descriptor != null)
                    {
                        listFilter.Add(new GridFilter(descriptor.Member, descriptor.Value));
                    }
                    else if (filter is CompositeFilterDescriptor)
                    {

                    }

                }
            }
            return listFilter;
        }
        /// <summary>
        /// Provides Sorts column and Direction if sort count is zero sort is set to ASC and sorted by first provided column
        /// Please make sure for null column names disable sort on Grid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="FirstColumnName"></param>
        /// <returns></returns>
        public List<GridSort> GetGridSort(DataSourceRequest request, string FirstColumnName)
        {
            List<GridSort> sortList = new List<GridSort>();

            if (request.Sorts!=null && request.Sorts.Count>0)
            {
                foreach (var sort in request.Sorts)
                {
                    string sordir = "ASC";
                    if (sort.SortDirection.ToString().Equals("Ascending", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sordir = "ASC";
                    }
                    else
                    {
                        sordir = "DESC";
                    }
                    GridSort gd = new GridSort(sort.Member.ToString(), sordir);
                    sortList.Add(gd);
                }
            }
            else
            {
                sortList.Add(new GridSort(FirstColumnName, "ASC"));

            }
            return sortList;
        }
    }
    //if (request.PageSize == 0)
    //    request.PageSize = 20;

    ////get the number of dummy records to be added, so that grouping will work with pagination.
    //int NoOfDummyRecords = (request.Page - 1) * request.PageSize;

    ////add the dummy records in the strating of the list.
    //List<ContentViewModel> list = new List<ContentViewModel>();
    //for(int i=0;i< NoOfDummyRecords; i++)
    //{
    //    list.Add(new ContentViewModel());
    //}
}