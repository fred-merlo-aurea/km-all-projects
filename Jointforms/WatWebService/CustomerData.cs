using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WattWebService
{
    /// <summary>
    /// Class to represent CustomerData and Subscription Preferences
    /// </summary>
    public class CustomerData
    {
        public CustomerData()
        {
        }

        public string EktronUserName { get; set; }
        /// <summary>
        /// User's Email
        /// </summary>
        public string PubCode { get; set; }

        /// <summary>
        /// User's Birth Date
        /// </summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// User's First name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// User's Last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// User's Address Line 1
        /// </summary>
        public string AddressLine1 { get; set; }
        /// <summary>Customer
        /// User's Address Line 2
        /// </summary>
        public string AddressLine2 { get; set; }
        /// <summary>
        /// User's City
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// User's State Full Name in case of US address
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// User's Country code as per ISO Format
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// User's postal code
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// User's company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Title 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// FullName
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Occupation 
        /// </summary>
        public string Occupation { get; set; }

        /// <summary>
        /// Phone 
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Mobile
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Fax
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Website
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Age
        /// </summary>
        public Int16 Age { get; set; }

        /// <summary>
        /// Income
        /// </summary>
        public decimal Income { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        public object[][] hUDF { get; set; }

        public CustomerData(CustomerData profile)
        {
            EktronUserName = profile.EktronUserName;
            PubCode = profile.PubCode;
            BirthDay = profile.BirthDay;
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            AddressLine1 = profile.AddressLine1;
            AddressLine2 = profile.AddressLine2;
            City = profile.City;
            State = profile.State;
            Country = profile.Country;
            PostalCode = profile.PostalCode;
            CompanyName = profile.CompanyName;
            Title = profile.Title;
            FullName = profile.FullName;
            Occupation = profile.Occupation;
            Phone = profile.Phone;
            Mobile = profile.Mobile;
            Fax = profile.Fax;
            Website = profile.Website;
            Age = profile.Age;
            Income = profile.Income;
            Gender = profile.Gender;
            Password = profile.Password;
        }
        //public CustomerData(string ektronUserName, DateTime? birthDay, string firstName, string lastName, string addressLine1, string addressLine2, string city, string state,
        //        string nonUSProvince, string country, string postalCode, string companyName, Dictionary<string, string> initialDict)
        //{
        //    EktronUserName = ektronUserName;

        //    BirthDay = Convert.ToDateTime(birthDay);
        //    FirstName = firstName;
        //    LastName = lastName;
        //    AddressLine1 = addressLine1;
        //    AddressLine2 = addressLine2;
        //    City = city;
        //    State = state;
        //    NonUSProvince = nonUSProvince;
        //    Country = country;
        //    PostalCode = postalCode;
        //    CompanyName = companyName;
        //}
    }
}