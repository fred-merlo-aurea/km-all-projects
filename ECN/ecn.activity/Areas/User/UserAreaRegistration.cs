using System.Web.Mvc;

namespace ecn.activity.Areas.User
{
    public class UserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "User_default",
                "User/{action}/{*query}",
                new {controller = "Index", action = "Index", query = UrlParameter.Optional }
            );
        }
    }
}
