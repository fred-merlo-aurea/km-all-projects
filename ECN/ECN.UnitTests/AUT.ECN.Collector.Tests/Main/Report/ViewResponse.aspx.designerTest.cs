using AUT.ConfigureTestProjects;
using ecn.collector.main.report;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Collector.Tests.Main.Report
{
    [TestFixture]
    public partial class ViewResponseTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ViewResponse_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var viewResponse = new ViewResponse();

            // Assert
            viewResponse.Master.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ViewResponse_Class_Invalid_Property_MasterNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMaster = "MasterNotPresent";
            var viewResponse  = new ViewResponse();

            // Act , Assert
            Should.NotThrow(action: () => viewResponse.GetType().GetProperty(propertyNameMaster));
        }
    }
}