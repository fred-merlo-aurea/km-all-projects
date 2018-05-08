using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_Entities.Accounts;
using Shouldly;

namespace ECN_Framework_EntitiesTests.Extension
{
    [ExcludeFromCodeCoverage]
    public static class ContactExtension
    {
        public static void ShouldBeEqualsComparingAllProperties(this Contact thisContact, Contact expected)
        {
            thisContact.BillingContactID.ShouldBe(expected.BillingContactID);
            thisContact.CustomerID.ShouldBe(expected.CustomerID);
            thisContact.Salutation.ShouldBe(expected.Salutation);
            thisContact.ContactName.ShouldBe(expected.ContactName);
            thisContact.FirstName.ShouldBe(expected.FirstName);
            thisContact.LastName.ShouldBe(expected.LastName);
            thisContact.ContactTitle.ShouldBe(expected.ContactTitle);
            thisContact.Phone.ShouldBe(expected.Phone);
            thisContact.Fax.ShouldBe(expected.Fax);
            thisContact.Email.ShouldBe(expected.Email);
            thisContact.StreetAddress.ShouldBe(expected.StreetAddress);
            thisContact.City.ShouldBe(expected.City);
            thisContact.State.ShouldBe(expected.State);
            thisContact.Country.ShouldBe(expected.Country);
            thisContact.Zip.ShouldBe(expected.Zip);
            thisContact.CreatedUserID.ShouldBe(expected.CreatedUserID);
            thisContact.CreatedDate.ShouldBe(expected.CreatedDate);
            thisContact.UpdatedUserID.ShouldBe(expected.UpdatedUserID);
            thisContact.UpdatedDate.ShouldBe(expected.UpdatedDate);
            thisContact.IsDeleted.ShouldBe(expected.IsDeleted);
        }
    }
}