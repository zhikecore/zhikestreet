using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZhikeStreet.BLL;

namespace ZhikeStreet.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Initialize();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void Initialize()
        {
            //1.初始化系统账户
            AdminUserService.Instance.Init();
        }
    }
}
