using NUnit.Framework;
using Shouldly;

using FrameworkUAD.DataAccess;

namespace FrameworkUAD.Tests.DataAccess
{
    [TestFixture]
    public class FieldMapperTest
    {
        private const string DisplayName = "DisplayName";

        [TestCase("ADDRESS1", "Address")]
        [TestCase("REGIONCODE", "State")]
        [TestCase("ZIPCODE", "Zip")]
        [TestCase("PUBTRANSACTIONDATE", "TransactionDate")]
        [TestCase("QUALIFICATIONDATE", "QDate")]
        [TestCase(DisplayName, DisplayName)]
        public void GetColumnOrderByProductExportFieldDisplayName(string displayName, string expectedOrder)
        {
            // Arrange, Assert, Act
            FieldMapper.GetColumnOrderByProductExportFieldDisplayName(displayName).ShouldBe(expectedOrder);
        }

        [TestCase("FNAME", "FirstName")]
        [TestCase("LNAME", "LastName")]
        [TestCase("ISLATLONVALID", "GeoLocated")]
        [TestCase(DisplayName, DisplayName)]
        public void GetColumnOrderByDefaultExportFieldDisplayName(string displayName, string expectedOrder)
        {
            // Arrange, Assert, Act
            FieldMapper.GetColumnOrderByDefaultExportFieldDisplayName(displayName).ShouldBe(expectedOrder);
        }
    }
}
