using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhikeStreet.Models.DO;
using ZhikeStreet.Web.Areas.Admins.Helpers;
using ZhikeStreet.Web.Helpers;

namespace ZhikeStreet.Web.Filters
{
    //类和方法都使用时，加上这个特性，此时都其作用，不加，只方法起作用  
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class LoginFilter : ActionFilterAttribute, IActionFilter
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
        public class AdminUserAuthentication : AuthorizeAttribute
        {
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(SkipAdminUserAuthentication), true)
                  || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipAdminUserAuthentication), true);
                if (skipAuthorization)
                {
                    base.OnAuthorization(filterContext);
                    return;
                }

                bool isStayInLoginPage = filterContext.RouteData.Values.ContainsValue("Login");
                if (!isStayInLoginPage)
                {
                    var currentAdminUser = filterContext.HttpContext.Session[AccountConst.CurrentAdminUser] as AdminUser;
                    bool isLoginAction = filterContext.RouteData.Values.ContainsValue("Login") || filterContext.RouteData.Values.ContainsValue("login");
                    bool isUnlogin = (currentAdminUser == null) && (!isLoginAction);

                    if (isUnlogin)
                    {
                        filterContext.HttpContext.Response.Redirect("/account/login");
                    }
                }
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
        public class SkipAdminUserAuthentication : AuthorizeAttribute
        {
        }
    }
}