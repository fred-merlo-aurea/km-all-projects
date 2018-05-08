using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using ECN_Framework_Entities.Communicator.ContentReplacement;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.ContentReplacement
{
    [TestFixture]
    public class ContentReplacementEventArgsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();
            var blastId = Fixture.Create<int?>();
            var feedName = Fixture.Create<string>();
            var customerId = Fixture.Create<int>();
            var match = Fixture.Create<System.Text.RegularExpressions.Match>();
            var context = Fixture.Create<string>();
            var text = Fixture.Create<string>();
            var hTML = Fixture.Create<string>();

            // Act
            contentReplacementEventArgs.BlastID = blastId;
            contentReplacementEventArgs.FeedName = feedName;
            contentReplacementEventArgs.CustomerID = customerId;
            contentReplacementEventArgs.Match = match;
            contentReplacementEventArgs.Context = context;
            contentReplacementEventArgs.Text = text;
            contentReplacementEventArgs.HTML = hTML;

            // Assert
            contentReplacementEventArgs.BlastID.ShouldBe(blastId);
            contentReplacementEventArgs.FeedName.ShouldBe(feedName);
            contentReplacementEventArgs.CustomerID.ShouldBe(customerId);
            contentReplacementEventArgs.Match.ShouldBe(match);
            contentReplacementEventArgs.Context.ShouldBe(context);
            contentReplacementEventArgs.Text.ShouldBe(text);
            contentReplacementEventArgs.HTML.ShouldBe(hTML);
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (BlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentReplacementEventArgs.BlastID = random;

            // Assert
            contentReplacementEventArgs.BlastID.ShouldBe(random);
            contentReplacementEventArgs.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();

            // Act , Set
            contentReplacementEventArgs.BlastID = null;

            // Assert
            contentReplacementEventArgs.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();
            var propertyInfo = contentReplacementEventArgs.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(contentReplacementEventArgs, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentReplacementEventArgs.BlastID.ShouldBeNull();
            contentReplacementEventArgs.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();

            // Act , Assert
            Should.NotThrow(() => contentReplacementEventArgs.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();
            var propertyInfo  = contentReplacementEventArgs.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (Context) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Context_Property_Setting_String_No_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameContext = "Context";
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = contentReplacementEventArgs.GetType().GetProperty(propertyNameContext);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.NotThrow(() => propertyInfo.SetValue(contentReplacementEventArgs, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (Context) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Class_Invalid_Property_ContextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContext = "ContextNotPresent";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();

            // Act , Assert
            Should.NotThrow(() => contentReplacementEventArgs.GetType().GetProperty(propertyNameContext));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Context_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContext = "Context";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();
            var propertyInfo  = contentReplacementEventArgs.GetType().GetProperty(propertyNameContext);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();
            contentReplacementEventArgs.CustomerID = Fixture.Create<int>();
            var intType = contentReplacementEventArgs.CustomerID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();

            // Act , Assert
            Should.NotThrow(() => contentReplacementEventArgs.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();
            var propertyInfo  = contentReplacementEventArgs.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (FeedName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_FeedName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();
            contentReplacementEventArgs.FeedName = Fixture.Create<string>();
            var stringType = contentReplacementEventArgs.FeedName.GetType();

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

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (FeedName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Class_Invalid_Property_FeedNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFeedName = "FeedNameNotPresent";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();

            // Act , Assert
            Should.NotThrow(() => contentReplacementEventArgs.GetType().GetProperty(propertyNameFeedName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_FeedName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFeedName = "FeedName";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();
            var propertyInfo  = contentReplacementEventArgs.GetType().GetProperty(propertyNameFeedName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (HTML) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_HTML_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();
            contentReplacementEventArgs.HTML = Fixture.Create<string>();
            var stringType = contentReplacementEventArgs.HTML.GetType();

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

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (HTML) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Class_Invalid_Property_HTMLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHTML = "HTMLNotPresent";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();

            // Act , Assert
            Should.NotThrow(() => contentReplacementEventArgs.GetType().GetProperty(propertyNameHTML));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_HTML_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHTML = "HTML";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();
            var propertyInfo  = contentReplacementEventArgs.GetType().GetProperty(propertyNameHTML);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (Match) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Match_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameMatch = "Match";
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = contentReplacementEventArgs.GetType().GetProperty(propertyNameMatch);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(contentReplacementEventArgs, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (Match) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Class_Invalid_Property_MatchNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMatch = "MatchNotPresent";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();

            // Act , Assert
            Should.NotThrow(() => contentReplacementEventArgs.GetType().GetProperty(propertyNameMatch));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Match_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMatch = "Match";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();
            var propertyInfo  = contentReplacementEventArgs.GetType().GetProperty(propertyNameMatch);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (Text) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Text_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentReplacementEventArgs = Fixture.Create<ContentReplacementEventArgs<string>>();
            contentReplacementEventArgs.Text = Fixture.Create<string>();
            var stringType = contentReplacementEventArgs.Text.GetType();

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

        #region General Getters/Setters : Class (ContentReplacementEventArgs) => Property (Text) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Class_Invalid_Property_TextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameText = "TextNotPresent";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();

            // Act , Assert
            Should.NotThrow(() => contentReplacementEventArgs.GetType().GetProperty(propertyNameText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentReplacementEventArgs_Text_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameText = "Text";
            var contentReplacementEventArgs  = Fixture.Create<ContentReplacementEventArgs<string>>();
            var propertyInfo  = contentReplacementEventArgs.GetType().GetProperty(propertyNameText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion
    }
}