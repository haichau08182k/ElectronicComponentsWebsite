using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace electronicComponents
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var AccountCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (AccountCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(AccountCookie.Value);
                var role = authTicket.UserData.Split(new Char[] { ',' });
                var userPrincial = new GenericPrincipal(new GenericIdentity(authTicket.Name), role);
                Context.User = userPrincial;
            }
        }
    }
}
