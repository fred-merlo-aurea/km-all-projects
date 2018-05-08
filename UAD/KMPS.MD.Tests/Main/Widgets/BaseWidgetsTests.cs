using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KMPlatform.Object;
using KMPlatform.Object.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Dashboard;
using KMPS.MD.Objects.Dashboard.Fakes;
using KMPS.MD.Objects.Fakes;
using Moq;
using NUnit.Framework;
using Telerik.Web.UI;
using BusinessFakes = KMPlatform.BusinessLogic.Fakes;
using EntityFakes = KMPlatform.Entity.Fakes;

namespace KMPS.MD.Tests.Main.Widgets
{
    [ExcludeFromCodeCoverage]
    public class BaseWidgetsTests : BaseControlTests
    {
        protected internal const string UserSessionFieldName = "_userSession";
        protected internal const string UserSessionPropertyName = "UserSession";
        protected internal const string ClientConnectionsFieldName = "_clientConnections";
        protected internal const string ClientConnectionsPropertyName = "ClientConnections";

        private int _clientId = 1;

        protected string RadHtmlChartName { get; set; } = "RadHtmlChart1";
        protected ECNSession ECNSessionCurrentFake { get; private set; }
        protected ClientConnections ClientConnectionsFake { get; private set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            SetUpShimEcnSession();
            SetUpShimClient();
            SetUpShimClientConnections();
        }

        protected virtual Mock<RadHtmlChart> MockRadHtmlChart(string controlName)
        {
            var mock = new Mock<RadHtmlChart>();
            PrivateControl.SetField(controlName, mock.Object);
            return mock;
        }

        protected virtual void ShimForCumulativeGrowth()
        {
            var list = new List<CumulativeGrowth>();
            ShimCumulativeGrowth.GetClientConnectionsStringStringInt32Int32Int32Int32Int32 = (a, name, type, b, c, d, e, f) => list;
        }

        protected virtual void SetUpShimEcnSession()
        {
            ShimECNSession.AllInstances.ClientIDGet = (ecnSession) => { return _clientId; };
            ShimECNSession.AllInstances.ClientIDSetInt32 = (ecnSession, clientId) => { _clientId = clientId; };

            var shimEcnSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => { return shimEcnSession.Instance; };

            ECNSessionCurrentFake = shimEcnSession.Instance;
        }

        protected virtual void SetUpShimClient()
        {
            var shimClientEntity = new EntityFakes.ShimClient();
            BusinessFakes.ShimClient.AllInstances.SelectInt32Boolean =
                (instance, clientId, includeObjects) => { return shimClientEntity.Instance; };
        }

        protected virtual void SetUpShimClientConnections()
        {
            ShimClientConnections.Constructor =
                (instance) => { ClientConnectionsFake = instance; };
            ShimClientConnections.ConstructorClient =
                (instance, client) => { ClientConnectionsFake = instance; };
        }
    }
}
