using Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace List9.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UserManager<List9User> manager = new UserManager<List9User>(new UserStore<List9User>(new List9Context()));

            List9User user = new List9User();

            user.UserName = "dbeech";
            user.PhoneNumber = "070000000000";
            user.Email = "david@e9ine.com";
            user.Name = "David Beech";

            manager.Create(user, "Password");

        }
    }
}
