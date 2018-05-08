using System.Diagnostics.CodeAnalysis;
using KM.Platform.Fakes;
using NUnit.Framework;
using UAS.Web.Controllers.Common;

namespace UAS.Web.Tests.Controllers.Common
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FilterControllerTest : ControllerTestBase
    {
        private const int SampleId = 100;

        private FilterController _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _testEntity = new FilterController();
            Initialize(_testEntity);
            ShimUser.IsAdministratorUser = user => false;
        }
    }
}
