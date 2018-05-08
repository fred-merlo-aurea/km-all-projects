using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using AMS_Operations;
using FrameworkUAD.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using SubscriberTransformed = FrameworkUAD.Entity.SubscriberTransformed;

namespace UAS.UnitTests.AMS_Operations.FileValidator_cs
{
    [TestFixture]
    public class HasQualifiedProfile
    {
        private const string ValidFirstName = "James";
        private const string ValidLastName = "Crocket";
        private const string ValidAddress = "200 SE 2nd Ave., Miami, FL 33131 United States (USA)";
        private const string ValidState = "Florida";
        private const string ValidCountry = "USA";
        private const string ValidZip = "33101";
        private const string ValidCity = "Miami";
        private const string ValidPhone = "555-1234567";
        private const string ValidCompany = "Miami-Dade Police";
        private const string ValidTitle = "Detective";
        private const string ProcessCode = "ProcessCode";
        private const int ClientId = 10;
        private const int SourceFileId = 40;
        private const string DataFieldName = "dataIV";
        private const string ErrorMessagesFieldName = "ErrorMessages";
        private const string NoAddressErrorMessage = "Address is not present ";
        private const string NoPhoneErrorMessage = "Phone is not present ";
        private const string NoCompanyErrorMessage = "Company is not present ";
        private const string NoNameOrTitleErrorMessage = "Name or Title is not present for the profile";
        private const string UnQualifiedErrorMessage = "Row missing Subscriber Profile and Email @ Row -1, you must have one or the other";
        private const string WillRejectFileErrorMessage = "Rejecting file due to error threshold - will delete file from repository then shut down current engine and start new instance";

        private FileValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new FileValidator();

            var importFile = new ImportFile
                         {
                             ClientId = ClientId,
                             ProcessCode = ProcessCode,
                             SourceFileId = SourceFileId
                         };

            var dataDictionary = new Dictionary<int, StringDictionary>
                                     {
                                         [-1] = new StringDictionary()
                                     };
            importFile.DataTransformed = dataDictionary;

            var errorMessages = new List<string>();

            validator.SetField(DataFieldName, importFile);
            validator.SetField(ErrorMessagesFieldName, errorMessages);
        }

        [Test]
        public void HasQualifiedProfile_EmptyCheckList_NegativReturnsEmptyList()
        {
            // Arrange
            var checklist = new List<SubscriberTransformed>();

            // Act
            var result = validator.CallHasQualifiedProfile(checklist);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<SubscriberTransformed>>(),
                () => result.Count.ShouldBe(0),
                () => validator.GetErrorMessages().Count.ShouldBe(0));
        }

        [Test]
        public void HasQualifiedProfile_NothingSet_NegativReturnsEmptyListAddErrorMessage()
        {
            // Arrange
            var checklist = CreateSubscriberTransformeds();

            // Act
            var result = validator.CallHasQualifiedProfile(checklist);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<SubscriberTransformed>>(),
                () => result.Count.ShouldBe(0),
                () => validator.GetErrorMessages().Count.ShouldBe(3),
                () => validator.ContainsErrorMessagesAtIndex(NoNameOrTitleErrorMessage, 0).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(WillRejectFileErrorMessage, 1).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(UnQualifiedErrorMessage, 2).ShouldBeTrue());
        }

        [Test]
        public void HasQualifiedProfile_HasName_NegativReturnsEmptyList()
        {
            // Arrange
            var checklist = CreateSubscriberTransformeds(setValidName: true);

            // Act
            var result = validator.CallHasQualifiedProfile(checklist);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<SubscriberTransformed>>(),
                () => result.Count.ShouldBe(0),
                () => validator.GetErrorMessages().Count.ShouldBe(3),
                () => validator.ContainsErrorMessagesAtIndex(NoAddressErrorMessage, 0).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(NoPhoneErrorMessage, 0).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(NoCompanyErrorMessage, 0).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(WillRejectFileErrorMessage, 1).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(UnQualifiedErrorMessage, 2).ShouldBeTrue());
        }

        [Test]
        public void HasQualifiedProfile_HasNameAddress_PositivIsQualified()
        {
            // Arrange
            var checklist = CreateSubscriberTransformeds(setValidName: true, setValidAddress: true);

            // Act
            var result = validator.CallHasQualifiedProfile(checklist);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<SubscriberTransformed>>(),
                () => result.Count.ShouldBe(1),
                () => result.Contains(checklist.First()).ShouldBeTrue());
        }

        [Test]
        public void HasQualifiedProfile_HasNamePhone_PositivIsQualified()
        {
            // Arrange
            var checklist = CreateSubscriberTransformeds(setValidName: true, setValidPhone: true);

            // Act
            var result = validator.CallHasQualifiedProfile(checklist);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<SubscriberTransformed>>(),
                () => result.Count.ShouldBe(1),
                () => result.Contains(checklist.First()).ShouldBeTrue());
        }

        [Test]
        public void HasQualifiedProfile_HasNamePhoneAddress_PositivIsQualified()
        {
            // Arrange
            var checklist = CreateSubscriberTransformeds(setValidName: true, setValidPhone: true, setValidAddress: true);

            // Act
            var result = validator.CallHasQualifiedProfile(checklist);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<SubscriberTransformed>>(),
                () => result.Count.ShouldBe(1),
                () => result.Contains(checklist.First()).ShouldBeTrue());
        }

        [Test]
        public void HasQualifiedProfile_HasTitle_NegativReturnsEmpty()
        {
            // Arrange
            var checklist = CreateSubscriberTransformeds(setValidTitle: true).ToList();

            // Act
            var result = validator.CallHasQualifiedProfile(checklist);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<SubscriberTransformed>>(),
                () => result.Count.ShouldBe(0),
                () => validator.ContainsErrorMessagesAtIndex(NoNameOrTitleErrorMessage, 0).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(WillRejectFileErrorMessage, 1).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(UnQualifiedErrorMessage, 2).ShouldBeTrue());
        }

        [Test]
        public void HasQualifiedProfile_HasTitleCompany_NegativReturnsEmpty()
        {
            // Arrange
            var checklist = CreateSubscriberTransformeds(setValidTitle: true, setValidCompany: true);

            // Act
            var result = validator.CallHasQualifiedProfile(checklist);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<SubscriberTransformed>>(),
                () => result.Count.ShouldBe(0),
                () => validator.GetErrorMessages()[0].Contains(NoAddressErrorMessage).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(WillRejectFileErrorMessage, 1).ShouldBeTrue(),
                () => validator.ContainsErrorMessagesAtIndex(UnQualifiedErrorMessage, 2).ShouldBeTrue());
        }

        [Test]
        public void HasQualifiedProfile_HasTitleCompanyAddress_PositivIsQualified()
        {
            // Arrange
            var checklist = CreateSubscriberTransformeds(setValidTitle: true, setValidCompany: true, setValidAddress: true);

            // Act
            var result = validator.CallHasQualifiedProfile(checklist);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<List<SubscriberTransformed>>(),
                () => result.Count.ShouldBe(1),
                () => result.Contains(checklist.First()).ShouldBeTrue());
        }

        [Test]
        public void HasQualifiedProperties_NothingSet_NotQualified()
        {
            // Arrange
            var subscriber = new SubscriberTransformed();

            // Act
            var hasQualifiedName = validator.CallHasQualifiedName(subscriber);
            var hasQualifiedAddress = validator.CallHasQualifiedAddress(subscriber);
            var hasQualifiedPhone = validator.CallHasQualifiedPhone(subscriber);
            var hasQualifiedCompany = validator.CallHasQualifiedCompany(subscriber);
            var hasQualifiedTitle = validator.CallHasQualifiedTitle(subscriber);

            // Assert
            hasQualifiedName.ShouldSatisfyAllConditions(
                () => hasQualifiedName.ShouldBeFalse(),
                () => hasQualifiedAddress.ShouldBeFalse(),
                () => hasQualifiedCompany.ShouldBeFalse(),
                () => hasQualifiedTitle.ShouldBeFalse(),
                () => hasQualifiedPhone.ShouldBeFalse());
        }

        [Test]
        [TestCase(ValidFirstName, "")]
        [TestCase("", ValidLastName)]
        [TestCase(ValidFirstName, ValidLastName)]
        public void HasQualifiedName_FNameOrLNameSet_IsQualified(string firstName, string lastName)
        {
            // Arrange
            var subscriber = new SubscriberTransformed
            {
                FName = firstName,
                LName = lastName
            };

            // Act
            var isQualified = validator.CallHasQualifiedName(subscriber);

            // Assert
            isQualified.ShouldBeTrue();
        }

        [Test]
        public void HasQualifiedAddress_AddressSet_NotQualified()
        {
            // Arrange
            var subscriber = new SubscriberTransformed { Address = ValidAddress };

            // Act
            var isQualified = validator.CallHasQualifiedAddress(subscriber);

            // Assert
            isQualified.ShouldBeFalse();
        }

        [Test]
        [TestCase("", ValidCity)]
        [TestCase(ValidZip, "")]
        [TestCase(ValidZip, ValidCity)]
        public void HasQualifiedAddress_AddressAndZipOrCitySet_NotQualified(string zipCode, string city)
        {
            // Arrange
            var subscriber = new SubscriberTransformed
            {
                Address = ValidAddress,
                City = city,
                Zip = zipCode
            };

            // Act
            var isQualified = validator.CallHasQualifiedAddress(subscriber);

            // Assert
            isQualified.ShouldBeFalse();
        }

        [Test]
        [TestCase(ValidState, "")]
        [TestCase("", ValidCountry)]
        [TestCase(ValidState, ValidCountry)]
        public void HasQualifiedAddress_AddressAndZipSetStateOrCountrySet_IsQualified(string state, string country)
        {
            // Arrange
            var subscriber = new SubscriberTransformed
            {
                Address = ValidAddress,
                Zip = ValidZip,
                State = state,
                Country = country
            };

            // Act
            var isQualified = validator.CallHasQualifiedAddress(subscriber);

            // Assert
            isQualified.ShouldBeTrue();
        }

        [Test]
        [TestCase(ValidState, "")]
        [TestCase("", ValidCountry)]
        [TestCase(ValidState, ValidCountry)]
        public void HasQualifiedAddress_AddressAndCitySetStateOrCountrySet_IsQualified(string state, string country)
        {
            // Arrange
            var subscriber = new SubscriberTransformed
            {
                Address = ValidAddress,
                City = ValidCity,
                State = state,
                Country = country
            };

            // Act
            var isQualified = validator.CallHasQualifiedAddress(subscriber);

            // Assert
            isQualified.ShouldBeTrue();
        }

        [Test]
        [TestCase("", "", ValidPhone)]
        [TestCase("", ValidPhone, "")]
        [TestCase("", ValidPhone, ValidPhone)]
        [TestCase(ValidPhone, "", "")]
        [TestCase(ValidPhone, "", ValidPhone)]
        [TestCase(ValidPhone, ValidPhone, "")]
        [TestCase(ValidPhone, ValidPhone, ValidPhone)]
        public void HasQualifiedPhone_PhoneOrFaxOrMobileSet_IsQualified(string phone, string fax, string mobile)
        {
            // Arrange
            var subscriber = new SubscriberTransformed
            {
                Phone = phone,
                Fax = fax,
                Mobile = mobile
            };

            // Act
            var isQualified = validator.CallHasQualifiedPhone(subscriber);

            // Assert
            isQualified.ShouldBeTrue();
        }

        [Test]
        public void HasQualifiedCompany_CompanySet_IsQualified()
        {
            // Arrange
            var subscriber = new SubscriberTransformed { Company = ValidCompany };

            // Act
            var isQualified = validator.CallHasQualifiedCompany(subscriber);

            // Assert
            isQualified.ShouldBeTrue();
        }

        [Test]
        public void HasQualifiedTitle_TitleSet_IsQualified()
        {
            // Arrange
            var subscriber = new SubscriberTransformed { Title = ValidTitle };

            // Act
            var isQualified = validator.CallHasQualifiedTitle(subscriber);

            // Assert
            isQualified.ShouldBeTrue();
        }

        private static IList<SubscriberTransformed> CreateSubscriberTransformeds(
            int count = 1, 
            bool setValidName = false, 
            bool setValidAddress = false,
            bool setValidPhone = false,
            bool setValidCompany = false,
            bool setValidTitle = false)
        {
            return Enumerable.Range(1, count)
                .Select(x => CreateSubscriberTransformed(
                    setValidName,
                    setValidAddress,
                    setValidPhone,
                    setValidCompany,
                    setValidTitle)).ToList();
        }

        private static SubscriberTransformed CreateSubscriberTransformed(
            bool setValidName, 
            bool setValidAddress,
            bool setValidPhone,
            bool setValidCompany,
            bool setValidTitle)
        {
            var subscriberTransformed = new SubscriberTransformed();

            if (setValidName)
            {
                subscriberTransformed.FName = ValidFirstName;
                subscriberTransformed.LName = ValidLastName;
            }

            if (setValidAddress)
            {
                subscriberTransformed.Address = ValidAddress;
                subscriberTransformed.City = ValidCity;
                subscriberTransformed.Zip = ValidZip;
                subscriberTransformed.State = ValidState;
            }

            if (setValidPhone)
            {
                subscriberTransformed.Phone = ValidPhone;
            }

            if (setValidCompany)
            {
                subscriberTransformed.Company = ValidCompany;
            }

            if (setValidTitle)
            {
                subscriberTransformed.Title = ValidTitle;
            }

            return subscriberTransformed;
        }
    }
}
