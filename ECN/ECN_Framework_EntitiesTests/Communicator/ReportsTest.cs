using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class ReportsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Reports) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var reports = Fixture.Create<Reports>();
            var reportId = Fixture.Create<int>();
            var reportName = Fixture.Create<string>();
            var controlName = Fixture.Create<string>();
            var showInSetup = Fixture.Create<bool>();
            var isExport = Fixture.Create<bool>();

            // Act
            reports.ReportID = reportId;
            reports.ReportName = reportName;
            reports.ControlName = controlName;
            reports.ShowInSetup = showInSetup;
            reports.IsExport = isExport;

            // Assert
            reports.ReportID.ShouldBe(reportId);
            reports.ReportName.ShouldBe(reportName);
            reports.ControlName.ShouldBe(controlName);
            reports.ShowInSetup.ShouldBe(showInSetup);
            reports.IsExport.ShouldBe(isExport);
        }

        #endregion

        #region General Getters/Setters : Class (Reports) => Property (ControlName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_ControlName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reports = Fixture.Create<Reports>();
            reports.ControlName = Fixture.Create<string>();
            var stringType = reports.ControlName.GetType();

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

        #region General Getters/Setters : Class (Reports) => Property (ControlName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_Class_Invalid_Property_ControlNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameControlName = "ControlNameNotPresent";
            var reports  = Fixture.Create<Reports>();

            // Act , Assert
            Should.NotThrow(() => reports.GetType().GetProperty(propertyNameControlName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_ControlName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameControlName = "ControlName";
            var reports  = Fixture.Create<Reports>();
            var propertyInfo  = reports.GetType().GetProperty(propertyNameControlName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Reports) => Property (IsExport) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_IsExport_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var reports = Fixture.Create<Reports>();
            reports.IsExport = Fixture.Create<bool>();
            var boolType = reports.IsExport.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Reports) => Property (IsExport) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_Class_Invalid_Property_IsExportNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsExport = "IsExportNotPresent";
            var reports  = Fixture.Create<Reports>();

            // Act , Assert
            Should.NotThrow(() => reports.GetType().GetProperty(propertyNameIsExport));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_IsExport_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsExport = "IsExport";
            var reports  = Fixture.Create<Reports>();
            var propertyInfo  = reports.GetType().GetProperty(propertyNameIsExport);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Reports) => Property (ReportID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_ReportID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var reports = Fixture.Create<Reports>();
            reports.ReportID = Fixture.Create<int>();
            var intType = reports.ReportID.GetType();

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

        #region General Getters/Setters : Class (Reports) => Property (ReportID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_Class_Invalid_Property_ReportIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportID = "ReportIDNotPresent";
            var reports  = Fixture.Create<Reports>();

            // Act , Assert
            Should.NotThrow(() => reports.GetType().GetProperty(propertyNameReportID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_ReportID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportID = "ReportID";
            var reports  = Fixture.Create<Reports>();
            var propertyInfo  = reports.GetType().GetProperty(propertyNameReportID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Reports) => Property (ReportName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_ReportName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reports = Fixture.Create<Reports>();
            reports.ReportName = Fixture.Create<string>();
            var stringType = reports.ReportName.GetType();

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

        #region General Getters/Setters : Class (Reports) => Property (ReportName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_Class_Invalid_Property_ReportNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportName = "ReportNameNotPresent";
            var reports  = Fixture.Create<Reports>();

            // Act , Assert
            Should.NotThrow(() => reports.GetType().GetProperty(propertyNameReportName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_ReportName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportName = "ReportName";
            var reports  = Fixture.Create<Reports>();
            var propertyInfo  = reports.GetType().GetProperty(propertyNameReportName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Reports) => Property (ShowInSetup) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_ShowInSetup_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var reports = Fixture.Create<Reports>();
            reports.ShowInSetup = Fixture.Create<bool>();
            var boolType = reports.ShowInSetup.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Reports) => Property (ShowInSetup) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_Class_Invalid_Property_ShowInSetupNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameShowInSetup = "ShowInSetupNotPresent";
            var reports  = Fixture.Create<Reports>();

            // Act , Assert
            Should.NotThrow(() => reports.GetType().GetProperty(propertyNameShowInSetup));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Reports_ShowInSetup_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameShowInSetup = "ShowInSetup";
            var reports  = Fixture.Create<Reports>();
            var propertyInfo  = reports.GetType().GetProperty(propertyNameShowInSetup);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Reports) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Reports_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Reports());
        }

        #endregion

        #region General Constructor : Class (Reports) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Reports_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfReports = Fixture.CreateMany<Reports>(2).ToList();
            var firstReports = instancesOfReports.FirstOrDefault();
            var lastReports = instancesOfReports.Last();

            // Act, Assert
            firstReports.ShouldNotBeNull();
            lastReports.ShouldNotBeNull();
            firstReports.ShouldNotBeSameAs(lastReports);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Reports_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstReports = new Reports();
            var secondReports = new Reports();
            var thirdReports = new Reports();
            var fourthReports = new Reports();
            var fifthReports = new Reports();
            var sixthReports = new Reports();

            // Act, Assert
            firstReports.ShouldNotBeNull();
            secondReports.ShouldNotBeNull();
            thirdReports.ShouldNotBeNull();
            fourthReports.ShouldNotBeNull();
            fifthReports.ShouldNotBeNull();
            sixthReports.ShouldNotBeNull();
            firstReports.ShouldNotBeSameAs(secondReports);
            thirdReports.ShouldNotBeSameAs(firstReports);
            fourthReports.ShouldNotBeSameAs(firstReports);
            fifthReports.ShouldNotBeSameAs(firstReports);
            sixthReports.ShouldNotBeSameAs(firstReports);
            sixthReports.ShouldNotBeSameAs(fourthReports);
        }

        #endregion

        #region General Constructor : Class (Reports) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Reports_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var reportId = -1;
            var reportName = string.Empty;
            var controlName = string.Empty;
            var showInSetup = false;
            var isExport = false;

            // Act
            var reports = new Reports();

            // Assert
            reports.ReportID.ShouldBe(reportId);
            reports.ReportName.ShouldBe(reportName);
            reports.ControlName.ShouldBe(controlName);
            reports.ShowInSetup.ShouldBeFalse();
            reports.IsExport.ShouldBeFalse();
        }

        #endregion

        #endregion

        #endregion
    }
}