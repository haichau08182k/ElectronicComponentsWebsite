using electronicComponents.DAL;
using electronicComponents.Repository;
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
        GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["SumAccessTimes"] = _unitOfWork.GetRepositoryInstance<AccessTimeCount>().GetAllRecords().Sum(x => x.acessTime);
            Application["RealAccessTimes"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application["SumAccessTimes"] = (int)Application["SumAccessTimes"] + 1;
            var accessTimesCount = _unitOfWork.GetRepositoryInstance<AccessTimeCount>().GetAllRecords().Where(x => x.datee.Value.Date == DateTime.Now.Date);
            if (accessTimesCount.Count() != 0)
            {
                List<AccessTimeCount> list = accessTimesCount.ToList();
                list[0].acessTime += 1;
                _unitOfWork.GetRepositoryInstance<AccessTimeCount>().Update(list[0]);
            }
            else
            {
                AccessTimeCount accessTimesCountNew = new AccessTimeCount();
                accessTimesCountNew.datee = DateTime.Now;
                accessTimesCountNew.acessTime = 1;
                _unitOfWork.GetRepositoryInstance<AccessTimeCount>().Add(accessTimesCountNew);
            }
            Application["RealAccessTimes"] = (int)Application["RealAccessTimes"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock();
            Application["RealAccessTimes"] = (int)Application["RealAccessTimes"] - 1;
            Application.UnLock();
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
