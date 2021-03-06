﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KM.Common
{
    public sealed partial class GenericComparer<T> : IComparer<T>
    {
        #region Enums
        /// <summary>
        /// The sort order direction for sorting the collection
        /// </summary>
        public enum SortOrder
        {
            /// <summary>
            /// Ascending
            /// </summary>
            Ascending,
            /// <summary>
            /// Descending
            /// </summary>
            Descending
        };
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the GenericComparer class
        /// </summary>
        /// <param name="sortColumn">The property on which the collection should be sorted</param>
        /// <param name="sortingOrder">The direction of the sort</param>
        public GenericComparer(string sortColumn, SortOrder sortingOrder)
        {
            this.sortColumn = sortColumn;
            this.sortingOrder = sortingOrder;
        }
        #endregion

        #region Fields

        private string sortColumn;
        private SortOrder sortingOrder;

        #endregion

        #region Properties

        /// <summary>
        /// Column Name(public property of the class) to be sorted.
        /// </summary>
        public string SortColumn
        {
            get { return sortColumn; }
        }

        /// <summary>
        /// Sorting order.
        /// </summary>
        public SortOrder SortingOrder
        {
            get { return sortingOrder; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Compare interface implementation
        /// </summary>
        /// <param name="x">custom Object</param>
        /// <param name="y">custom Object</param>
        /// <returns>int</returns>
        public int Compare(T x, T y)
        {
            var propertyInfo = typeof(T).GetProperty(sortColumn);
            var obj1 = (IComparable)propertyInfo.GetValue(x, null);
            var obj2 = (IComparable)propertyInfo.GetValue(y, null);
            if (sortingOrder == SortOrder.Ascending)
            {
                return (obj1.CompareTo(obj2));
            }
            else
            {
                return (obj2.CompareTo(obj1));
            }
        }
        #endregion
    }
}
