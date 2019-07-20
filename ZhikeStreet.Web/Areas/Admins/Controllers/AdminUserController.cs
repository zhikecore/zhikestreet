using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZhikeStreet.BLL;
using ZhikeStreet.Common.Utility;
using ZhikeStreet.Models.DO;
using ZhikeStreet.Models.VO;
using ZhikeStreet.Web.Areas.Admins.Helpers;

namespace ZhikeStreet.Web.Areas.Admins.Controllers
{
    public class AdminUserController : Controller
    {
        // GET: Admins/AdminUser
        public ActionResult Index()
        {
            return View();
        }

        //Data table
        public JsonResult GetDataTableRecords()
        {
            //Base Params
            DataTableParams dtParams = new DataTableParams(this);

            //Custom Parmas
            string keyword = Request["Keyword"];

            var dtRecords = ToDataTable(dtParams, keyword);

            return Json(dtRecords, JsonRequestBehavior.AllowGet);
        }

        public Object ToDataTable(DataTableParams dtParams, string keyword)
        {
            int total = 0;
            var filters = AdminUserService.Instance.GetBySomeWhere(keyword, dtParams.DisplayStart, dtParams.DisplayLength, out total);

            var aaData = new Object();
            if (filters.Count > dtParams.DisplayLength)
                aaData = filters.Skip(dtParams.DisplayStart).Take(dtParams.DisplayLength).Select(m => ToJson(m, dtParams));
            else
                aaData = filters.Select(m => ToJson(m, dtParams));

            return new { sEcho = dtParams.Echo, iTotalRecords = filters.Count, iTotalDisplayRecords = total, aaData = aaData };

        }

        // GET: Login
        public ActionResult Login()
        {
            var browserCookie = Request.Cookies[AccountHashKeys.AdminUserBrowserCookie];

            if (browserCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(browserCookie.Value);
                var email = ticket.Name;
                var password = ticket.UserData;
                var currentAdminUser = AdminUserService.Instance.GetByAccount(email);

                if (currentAdminUser == null)
                {
                    ClearClientCookie(AccountHashKeys.AdminUserBrowserCookie);
                    return View();
                }
                Session.Add(AccountHashKeys.CurrentAdminUser, AdminUserService.Instance.GetByAccount(email));
                return RedirectToAction("index", "default");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[AccountHashKeys.LoginModelStateErrorMessage] = true;
                return RedirectToAction("login", "user");
            }
            
            AdminUser currentAdminUser = AdminUserService.Instance.GetByAccount(model.Account);


            if (currentAdminUser == null || String.IsNullOrEmpty(currentAdminUser.Account))
            {
                return RedirectToAction("login", "user");
            }

            // Session current valid AdminUser.
            Session.Add(AccountHashKeys.CurrentAdminUser, currentAdminUser);
            AdminUser currentUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
            return RedirectToAction("index", "default");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session[AccountHashKeys.CurrentAdminUser] = null;
            Session.Remove(AccountHashKeys.CurrentAdminUser);
            ClearClientCookie(AccountHashKeys.AdminUserBrowserCookie);

            return RedirectToAction("login", "user");
        }

        #region Private

        // Private methods
        private void ClearClientCookie(string cookieKey)
        {
            if (Request.Cookies[cookieKey] != null)
            {
                Response.Cookies.Add(new HttpCookie(cookieKey)
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }
        }

        private Object ToJson(AdminUser model, DataTableParams dtParams)
        {
            var json = model.AsJson() as Hashtable;
            json.Add("Actions", RenderViewHelper.RenderToString("_Actions", model, dtParams.Controller));

            return json;
        }

        #endregion
    }
}