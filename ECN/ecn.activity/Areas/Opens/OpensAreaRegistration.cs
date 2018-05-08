using System.Web.Mvc;

namespace ecn.activity.Areas.Opens
{
    public class OpensAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Opens";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Opens_default",
                "Opens/{*query}",
                new { controller = "Index", action = "Index", query = UrlParameter.Optional }
            );
        }
    }
}
