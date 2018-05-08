using System.Text;
using ecn.webservice.CustomAPI;
using ECN.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Webservice.Tests.CustomAPI
{
    /// <summary>
    /// Unit Tests for <see cref="WattLogic"/>
    /// </summary>
    public class WattLogicTest
    {
        private const string EmailAddressToXml = "EmailAddressToXML";

        private WattLogic _wattLogic;
        private PrivateObject _privateObject;
        private CustomerData _customerData;

        [SetUp]
        public void SetUp()
        {
            _wattLogic = new WattLogic();
            _privateObject = new PrivateObject(_wattLogic);
        }

        [Test]
        public void EmailAddressToXML_WhenCalled_VerifyXmlDocument()
        {
            // Arrange
            _customerData = new CustomerData(typeof(CustomerData).CreateInstance())
            {
                Age = 10
            };

            // Act
            var xml = (string)_privateObject.Invoke(EmailAddressToXml, _customerData);

            // Assert
            xml.ShouldBe(ExpectedXml.ToString());
        }

        private StringBuilder ExpectedXml => new StringBuilder()
            .Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>")
            .Append("<XML>")
            .Append("<Emails>")
            .Append($"<emailaddress>{_customerData.EktronUserName}</emailaddress>")
            .Append($"<firstname>{_customerData.FirstName}</firstname>")
            .Append($"<lastname>{_customerData.LastName}</lastname>")
            .Append($"<fullname>{_customerData.FirstName} {_customerData.LastName}</fullname>")
            .Append($"<company>{_customerData.CompanyName}</company>")
            .Append($"<address>{_customerData.AddressLine1}</address>")
            .Append($"<address2>{_customerData.AddressLine2}</address2>")
            .Append($"<city>{_customerData.City}</city>")
            .Append($"<state>{_customerData.State}</state>")
            .Append($"<zip>{_customerData.PostalCode}</zip>")
            .Append($"<country>{_customerData.Country}</country>")
            .Append($"<birthdate>{_customerData.BirthDay}</birthdate>")
            .Append($"<title>{_customerData.Title}</title>")
            .Append($"<fullname>{_customerData.FullName}</fullname>")
            .Append($"<occupation>{_customerData.Occupation}</occupation>")
            .Append($"<voice>{_customerData.Phone}</voice>")
            .Append($"<mobile>{_customerData.Mobile}</mobile>")
            .Append($"<fax>{_customerData.Fax}</fax>")
            .Append($"<website>{_customerData.Website}</website>")
            .Append($"<age>{_customerData.Age}</age>")
            .Append($"<income>{_customerData.Income}</income>")
            .Append($"<gender>{_customerData.Gender}</gender>")
            .Append($"<password>{_customerData.Password}</password>")
            .Append("</Emails>")
            .Append("</XML>");
    }
}
