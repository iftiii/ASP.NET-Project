using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Project
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var roles = authTicket.UserData.Split(',');
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                }
            }
            else
            {
               // authCookie.Response.Redirect("~/Views/Shared/Page_403.cshtml");
            }
        }

        /*
        protected void Application_Error(object sender_, CommandEventArgs e_)
        {
            Exception exception = Server.GetLastError();
            if (exception is CryptographicException)
            {
                FormsAuthentication.SignOut();
            }
        }

        */
        void Session_Start(object sender, EventArgs e)
        {
            if (Session.IsNewSession && Session["UserId"] == null)
            {
                Session["UserId"] = "";
                Response.Redirect("/Auth/Login");
            }
        }
    }
}
