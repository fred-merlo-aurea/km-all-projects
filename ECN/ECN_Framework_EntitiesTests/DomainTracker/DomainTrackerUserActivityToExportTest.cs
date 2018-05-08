using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.DomainTracker;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.DomainTracker
{
    [TestFixture]
    public class DomainTrackerUserActivityToExportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var domainTrackerUserActivityToExport = Fixture.Create<DomainTrackerUserActivityToExport>();
            var emailAddress = Fixture.Create<string>();
            var pageURL = Fixture.Create<string>();
            var timeStamp = Fixture.Create<string>();
            var iPAddress = Fixture.Create<string>();
            var oS = Fixture.Create<string>();
            var browser = Fixture.Create<string>();

            // Act
            domainTrackerUserActivityToExport.EmailAddress = emailAddress;
            domainTrackerUserActivityToExport.PageURL = pageURL;
            domainTrackerUserActivityToExport.TimeStamp = timeStamp;
            domainTrackerUserActivityToExport.IPAddress = iPAddress;
            domainTrackerUserActivityToExport.OS = oS;
            domainTrackerUserActivityToExport.Browser = browser;

            // Assert
            domainTrackerUserActivityToExport.EmailAddress.ShouldBe(emailAddress);
            domainTrackerUserActivityToExport.PageURL.ShouldBe(pageURL);
            domainTrackerUserActivityToExport.TimeStamp.ShouldBe(timeStamp);
            domainTrackerUserActivityToExport.IPAddress.ShouldBe(iPAddress);
            domainTrackerUserActivityToExport.OS.ShouldBe(oS);
            domainTrackerUserActivityToExport.Browser.ShouldBe(browser);
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (Browser) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_Browser_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserActivityToExport = Fixture.Create<DomainTrackerUserActivityToExport>();
            domainTrackerUserActivityToExport.Browser = Fixture.Create<string>();
            var stringType = domainTrackerUserActivityToExport.Browser.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (Browser) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_Class_Invalid_Property_BrowserNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBrowser = "BrowserNotPresent";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameBrowser));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_Browser_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBrowser = "Browser";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();
            var propertyInfo  = domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameBrowser);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserActivityToExport = Fixture.Create<DomainTrackerUserActivityToExport>();
            domainTrackerUserActivityToExport.EmailAddress = Fixture.Create<string>();
            var stringType = domainTrackerUserActivityToExport.EmailAddress.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();
            var propertyInfo  = domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (IPAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_IPAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserActivityToExport = Fixture.Create<DomainTrackerUserActivityToExport>();
            domainTrackerUserActivityToExport.IPAddress = Fixture.Create<string>();
            var stringType = domainTrackerUserActivityToExport.IPAddress.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (IPAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_Class_Invalid_Property_IPAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIPAddress = "IPAddressNotPresent";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameIPAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_IPAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIPAddress = "IPAddress";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();
            var propertyInfo  = domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameIPAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (OS) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_OS_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserActivityToExport = Fixture.Create<DomainTrackerUserActivityToExport>();
            domainTrackerUserActivityToExport.OS = Fixture.Create<string>();
            var stringType = domainTrackerUserActivityToExport.OS.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (OS) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_Class_Invalid_Property_OSNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOS = "OSNotPresent";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameOS));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_OS_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOS = "OS";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();
            var propertyInfo  = domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameOS);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (PageURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_PageURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserActivityToExport = Fixture.Create<DomainTrackerUserActivityToExport>();
            domainTrackerUserActivityToExport.PageURL = Fixture.Create<string>();
            var stringType = domainTrackerUserActivityToExport.PageURL.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (PageURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_Class_Invalid_Property_PageURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageURL = "PageURLNotPresent";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserActivityToExport.GetType().GetProperty(propertyNamePageURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_PageURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageURL = "PageURL";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();
            var propertyInfo  = domainTrackerUserActivityToExport.GetType().GetProperty(propertyNamePageURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (TimeStamp) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_TimeStamp_Property_String_Type_Verify_Test()
        {
            // Arrange
            var domainTrackerUserActivityToExport = Fixture.Create<DomainTrackerUserActivityToExport>();
            domainTrackerUserActivityToExport.TimeStamp = Fixture.Create<string>();
            var stringType = domainTrackerUserActivityToExport.TimeStamp.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (DomainTrackerUserActivityToExport) => Property (TimeStamp) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_Class_Invalid_Property_TimeStampNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTimeStamp = "TimeStampNotPresent";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();

            // Act , Assert
            Should.NotThrow(() => domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameTimeStamp));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DomainTrackerUserActivityToExport_TimeStamp_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTimeStamp = "TimeStamp";
            var domainTrackerUserActivityToExport  = Fixture.Create<DomainTrackerUserActivityToExport>();
            var propertyInfo  = domainTrackerUserActivityToExport.GetType().GetProperty(propertyNameTimeStamp);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DomainTrackerUserActivityToExport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerUserActivityToExport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DomainTrackerUserActivityToExport());
        }

        #endregion

        #region General Constructor : Class (DomainTrackerUserActivityToExport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerUserActivityToExport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDomainTrackerUserActivityToExport = Fixture.CreateMany<DomainTrackerUserActivityToExport>(2).ToList();
            var firstDomainTrackerUserActivityToExport = instancesOfDomainTrackerUserActivityToExport.FirstOrDefault();
            var lastDomainTrackerUserActivityToExport = instancesOfDomainTrackerUserActivityToExport.Last();

            // Act, Assert
            firstDomainTrackerUserActivityToExport.ShouldNotBeNull();
            lastDomainTrackerUserActivityToExport.ShouldNotBeNull();
            firstDomainTrackerUserActivityToExport.ShouldNotBeSameAs(lastDomainTrackerUserActivityToExport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerUserActivityToExport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDomainTrackerUserActivityToExport = new DomainTrackerUserActivityToExport();
            var secondDomainTrackerUserActivityToExport = new DomainTrackerUserActivityToExport();
            var thirdDomainTrackerUserActivityToExport = new DomainTrackerUserActivityToExport();
            var fourthDomainTrackerUserActivityToExport = new DomainTrackerUserActivityToExport();
            var fifthDomainTrackerUserActivityToExport = new DomainTrackerUserActivityToExport();
            var sixthDomainTrackerUserActivityToExport = new DomainTrackerUserActivityToExport();

            // Act, Assert
            firstDomainTrackerUserActivityToExport.ShouldNotBeNull();
            secondDomainTrackerUserActivityToExport.ShouldNotBeNull();
            thirdDomainTrackerUserActivityToExport.ShouldNotBeNull();
            fourthDomainTrackerUserActivityToExport.ShouldNotBeNull();
            fifthDomainTrackerUserActivityToExport.ShouldNotBeNull();
            sixthDomainTrackerUserActivityToExport.ShouldNotBeNull();
            firstDomainTrackerUserActivityToExport.ShouldNotBeSameAs(secondDomainTrackerUserActivityToExport);
            thirdDomainTrackerUserActivityToExport.ShouldNotBeSameAs(firstDomainTrackerUserActivityToExport);
            fourthDomainTrackerUserActivityToExport.ShouldNotBeSameAs(firstDomainTrackerUserActivityToExport);
            fifthDomainTrackerUserActivityToExport.ShouldNotBeSameAs(firstDomainTrackerUserActivityToExport);
            sixthDomainTrackerUserActivityToExport.ShouldNotBeSameAs(firstDomainTrackerUserActivityToExport);
            sixthDomainTrackerUserActivityToExport.ShouldNotBeSameAs(fourthDomainTrackerUserActivityToExport);
        }

        #endregion

        #region General Constructor : Class (DomainTrackerUserActivityToExport) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DomainTrackerUserActivityToExport_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var emailAddress = string.Empty;
            var pageURL = string.Empty;

            // Act
            var domainTrackerUserActivityToExport = new DomainTrackerUserActivityToExport();

            // Assert
            domainTrackerUserActivityToExport.EmailAddress.ShouldBe(emailAddress);
            domainTrackerUserActivityToExport.PageURL.ShouldBe(pageURL);
            domainTrackerUserActivityToExport.TimeStamp.ShouldBeNull();
            domainTrackerUserActivityToExport.IPAddress.ShouldBeNull();
            domainTrackerUserActivityToExport.OS.ShouldBeNull();
            domainTrackerUserActivityToExport.Browser.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}