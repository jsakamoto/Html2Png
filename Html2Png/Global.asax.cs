using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Html2Png
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "Html2Png",
                defaults: new { controller = "Default", action = "Html2Png", id = UrlParameter.Optional }
            );
        }
    }
}
