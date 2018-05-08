using AUT.ConfigureTestProjects;
using ecn.collector.main.report;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Collector.Tests.Main.Report
{
    [TestFixture]
    public  class SurveyReportTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var surveyReport = new SurveyReport();

            // Assert
            surveyReport.Master.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SurveyReport_Class_Invalid_Property_MasterNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMaster = "MasterNotPresent";
            var surveyReport  = new SurveyReport();

            // Act , Assert
            Should.NotThrow(action: () => surveyReport.GetType().GetProperty(propertyNameMaster));
        }
    }
}