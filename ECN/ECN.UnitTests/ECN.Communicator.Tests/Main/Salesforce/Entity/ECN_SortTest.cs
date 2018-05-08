using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Entity;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_Entities.Accounts;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    /// <summary>
    ///     Unit test for <see cref="ECN_Sort"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ECN_SortTest
    {
        private List<Customer> customers;

        /// <summary>
        ///     Setup up <see cref="customers"/> which can be utilized in test scope
        /// </summary>
        [SetUp]
        public void SetUp() => customers = new List<Customer>
        {
            new Customer(),
            new Customer(),
            new Customer()
        };

        [Test]
        public void SortCustomers_SortByPropertyInAscendingOrder_ReturnSortedList(
            [ValueSource(nameof(SortProperties))] string sortBy)
        {
            // Arrange
            customers.ForEach(x => x.SetProperty(sortBy, $"{sortBy}_{customers.IndexOf(x)}"));
            List<Customer> expected = customers.OrderBy(x => x.GetPropertyValue(sortBy)).ToList();

            // Act
            List<Customer> result = ECN_Sort.Sort_Customers(customers, sortBy, SortDirection.Ascending);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void SortCustomers_SortByPropertyInDescendingOrder_ReturnSortedList(
            [ValueSource(nameof(SortProperties))] string sortBy)
        {
            // Arrange
            customers.ForEach(x => x.SetProperty(sortBy, $"{sortBy}_{customers.IndexOf(x)}"));
            List<Customer> expected = customers.OrderByDescending(x => x.GetPropertyValue(sortBy)).ToList();

            // Act
            List<Customer> result = ECN_Sort.Sort_Customers(customers, sortBy, SortDirection.Descending);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        private static readonly string[] SortProperties =
        {
            "CustomerName",
            "Address",
            "City",
            "State",
            "Zip",
            "Country",
            "Phone",
            "Fax",
            "WebAddress"
        };

        private static string[] GetNonSortProperties()
        {
            return typeof(Customer).GetAllProperties()
                .Where(x => !SortProperties.Contains(x.Name))
                .Select(x => x.Name).ToArray();
        }
    }
}