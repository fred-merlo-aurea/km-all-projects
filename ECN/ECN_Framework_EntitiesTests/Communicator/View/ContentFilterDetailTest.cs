using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator.View;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.View
{
    [TestFixture]
    public class ContentFilterDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ContentFilterDetail) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var customerId = Fixture.Create<int?>();
            var filterName = Fixture.Create<string>();
            var contentTitle = Fixture.Create<string>();

            // Act
            contentFilterDetail.CustomerID = customerId;
            contentFilterDetail.FilterName = filterName;
            contentFilterDetail.ContentTitle = contentTitle;

            // Assert
            contentFilterDetail.CustomerID.ShouldBe(customerId);
            contentFilterDetail.FilterName.ShouldBe(filterName);
            contentFilterDetail.ContentTitle.ShouldBe(contentTitle);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (ContentTitle) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_ContentTitle_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            contentFilterDetail.ContentTitle = Fixture.Create<string>();
            var stringType = contentFilterDetail.ContentTitle.GetType();

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

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (ContentTitle) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_ContentTitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentTitle = "ContentTitleNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameContentTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_ContentTitle_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentTitle = "ContentTitle";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameContentTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilterDetail.CustomerID = random;

            // Assert
            contentFilterDetail.CustomerID.ShouldBe(random);
            contentFilterDetail.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();

            // Act , Set
            contentFilterDetail.CustomerID = null;

            // Assert
            contentFilterDetail.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var propertyInfo = contentFilterDetail.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(contentFilterDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilterDetail.CustomerID.ShouldBeNull();
            contentFilterDetail.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FilterName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FilterName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            contentFilterDetail.FilterName = Fixture.Create<string>();
            var stringType = contentFilterDetail.FilterName.GetType();

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

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FilterName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_FilterNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterNameNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameFilterName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FilterName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterName";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameFilterName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ContentFilterDetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilterDetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ContentFilterDetail());
        }

        #endregion

        #region General Constructor : Class (ContentFilterDetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilterDetail_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfContentFilterDetail = Fixture.CreateMany<ContentFilterDetail>(2).ToList();
            var firstContentFilterDetail = instancesOfContentFilterDetail.FirstOrDefault();
            var lastContentFilterDetail = instancesOfContentFilterDetail.Last();

            // Act, Assert
            firstContentFilterDetail.ShouldNotBeNull();
            lastContentFilterDetail.ShouldNotBeNull();
            firstContentFilterDetail.ShouldNotBeSameAs(lastContentFilterDetail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilterDetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstContentFilterDetail = new ContentFilterDetail();
            var secondContentFilterDetail = new ContentFilterDetail();
            var thirdContentFilterDetail = new ContentFilterDetail();
            var fourthContentFilterDetail = new ContentFilterDetail();
            var fifthContentFilterDetail = new ContentFilterDetail();
            var sixthContentFilterDetail = new ContentFilterDetail();

            // Act, Assert
            firstContentFilterDetail.ShouldNotBeNull();
            secondContentFilterDetail.ShouldNotBeNull();
            thirdContentFilterDetail.ShouldNotBeNull();
            fourthContentFilterDetail.ShouldNotBeNull();
            fifthContentFilterDetail.ShouldNotBeNull();
            sixthContentFilterDetail.ShouldNotBeNull();
            firstContentFilterDetail.ShouldNotBeSameAs(secondContentFilterDetail);
            thirdContentFilterDetail.ShouldNotBeSameAs(firstContentFilterDetail);
            fourthContentFilterDetail.ShouldNotBeSameAs(firstContentFilterDetail);
            fifthContentFilterDetail.ShouldNotBeSameAs(firstContentFilterDetail);
            sixthContentFilterDetail.ShouldNotBeSameAs(firstContentFilterDetail);
            sixthContentFilterDetail.ShouldNotBeSameAs(fourthContentFilterDetail);
        }

        #endregion

        #region General Constructor : Class (ContentFilterDetail) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilterDetail_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var filterName = string.Empty;
            var contentTitle = string.Empty;

            // Act
            var contentFilterDetail = new ContentFilterDetail();

            // Assert
            contentFilterDetail.CustomerID.ShouldBeNull();
            contentFilterDetail.FilterName.ShouldBe(filterName);
            contentFilterDetail.ContentTitle.ShouldBe(contentTitle);
        }

        #endregion

        #endregion

        #endregion
    }
}