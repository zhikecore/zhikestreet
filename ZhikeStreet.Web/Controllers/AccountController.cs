using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZhikeStreet.BLL;
using ZhikeStreet.Common.Utility;
using ZhikeStreet.Models.DO;
using ZhikeStreet.Models.VO;
using ZhikeStreet.Web.Helpers;

namespace ZhikeStreet.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login
        public ActionResult Login()
        {
            var browserCookie = Request.Cookies[AccountConst.AdminUserBrowserCookie];

            if (browserCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(browserCookie.Value);
                var email = ticket.Name;
                var password = ticket.UserData;
                var currentAdminUser = AdminUserService.Instance.GetByAccount(email);

                if (currentAdminUser == null)
                {
                    ClearClientCookie(AccountConst.AdminUserBrowserCookie);
                    return View();
                }
                Session.Add(AccountConst.CurrentAdminUser, AdminUserService.Instance.GetByAccount(email));
                return RedirectToAction("index", "home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[AccountConst.LoginModelStateErrorMessage] = true;
                return RedirectToAction("login", "account");
            }

            AdminUser currentAdminUser = AdminUserService.Instance.GetByAccount(model.Account);
            if (currentAdminUser == null || String.IsNullOrEmpty(currentAdminUser.Account))
            {
                return Content("<script>alert('" + AccountConst.AccountNonExist + "');history.go(-1);</script>");
            }
            Log4Helper.Info(this.GetType(), String.Format("Account.Login.Account:{0},Password:{1}", model.Account, model.Password.Trim()));
            string passwordsalt = DEncryptHelper.Encrypt(model.Password.Trim(),SettingManager.Settings["DesKey"]);
            Log4Helper.Info(this.GetType(),String.Format("Account.Login.passwordsalt:{0},currentAdminUser.PasswordSalt:{1},currentAdminUser.Account:{2}", passwordsalt, currentAdminUser.PasswordSalt,currentAdminUser.Account));
            if (!passwordsalt.Equals(currentAdminUser.PasswordSalt))
            {
                return Content("<script>alert('" + AccountConst.PasswordError + "');history.go(-1);</script>");
            }

            // Session current valid AdminUser.
            Session.Add(AccountConst.CurrentAdminUser, currentAdminUser);
            AdminUser currentUser = Session[AccountConst.CurrentAdminUser] as AdminUser;
            return RedirectToAction("index", "Home");
        }

        public ActionResult Logout()
        {
            Session[AccountConst.CurrentAdminUser] = null;
            Session.Remove(AccountConst.CurrentAdminUser);
            ClearClientCookie(AccountConst.AdminUserBrowserCookie);

            return RedirectToAction("login", "account");
        }

        // GET: Login
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            Log4Helper.Info(this.GetType(), "Account.Register");
            if (!ModelState.IsValid)
            {
                return Content("<script>alert('" + AccountConst.LoginModelStateErrorMessage + "');history.go(-1);</script>");
            }

            //1.检查新两次密码是否一致
            if (!model.Password.Trim().Equals(model.RePassword.Trim()))
            {
                return Content("<script>alert('"+ AccountConst.TwicePasswordNotAsameMessage + "');history.go(-1);</script>");
                //return View();
            }

            //1.检查账户是否存在
            bool isAccountExist = AdminUserService.Instance.IsExist(0,model);
            if (isAccountExist)
            {
                TempData[AccountConst.RegisterMsg] = AccountConst.AccountHasExistMessage;
                return Content("<script>alert('"+ AccountConst.AccountHasExistMessage + "');history.go(-1);</script>");
            }

            bool isEmailExist = AdminUserService.Instance.IsExist(1, model);
            if (isEmailExist)
            {
                TempData[AccountConst.RegisterMsg] = AccountConst.EmailHasExistMessage;
                return Content("<script>alert('" + AccountConst.EmailHasExistMessage + "');history.go(-1);</script>");
            }

            bool isPhoneExist = AdminUserService.Instance.IsExist(2, model);
            if (isPhoneExist)
            {
                TempData[AccountConst.RegisterMsg] = AccountConst.PhoneHasExistMessage;
                return Content("<script>alert('" + AccountConst.PhoneHasExistMessage + "');history.go(-1);</script>");
            }

            string regIp = String.Empty;
            regIp = IPHelper.GetWebClientIp();

            AdminUser user = new AdminUser
            {
                Account=model.Account,
                PasswordSalt= DEncryptHelper.Encrypt(model.Password,SettingManager.Settings["DesKey"]),
                Phone =model.Phone,
                QQ=String.Empty,
                Email=model.Email,
                RealName=String.Empty,
                NickName=model.NickName,
                Avatar = "http://pepper.img.zhikestreet.com/zhike_avatar.png",
                RegIp=regIp,
                IsUse=true,
                Description=String.Empty
            };

            bool bRet= AdminUserService.Instance.Create(user);
            if (!bRet)
            {
                TempData[AccountConst.RegisterMsg] = AccountConst.RegisterFailure;
                return Content("<script>alert('"+ AccountConst.RegisterFailure + "');history.go(-1);</script>");
            }
            else
            {
                TempData[AccountConst.RegisterMsg] = AccountConst.RegisterSuccess;
                return Content("<script>alert('"+ AccountConst.RegisterSuccess + "');window.location ='/Account/Login/';</script>");
            }
        }

        #region

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

        #endregion

    }
}