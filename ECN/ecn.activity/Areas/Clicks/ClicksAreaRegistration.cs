using System.Web.Mvc;

namespace ecn.activity.Areas.Clicks
{
    public class ClicksAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Clicks";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Clicks_default",
                "Clicks/{*query}",
                new {controller = "Index", action = "Index", query = UrlParameter.Optional }
            );
        }
    }
}
