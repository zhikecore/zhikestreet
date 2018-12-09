using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZhikeStreet.Web.Helpers
{
    public static class AccountConst
    {
        // Login view model key.
        public static readonly string CaptchaErrorMessage = "CAPTCHA_ERROR";

        public static readonly string EmailOrPasswordErrorMessage = "EMAIL_OR_PASSWORD_ERROR";
        public static readonly string LoginModelStateErrorMessage = "MODEL_STATE_ERROR";
        public static readonly string TwicePasswordNotAsameMessage = "两次输入密码不一致!";
        public static readonly string PhoneValideCodeErrorMessage = "PHONE_VALIDATE_CODE_ERROR";
        public static readonly string PhoneValideCodeExpiredMessage = "PHONE_VALIDATE_CODE_EXPIRED";
        public static readonly string AccountHasExistMessage = "账户或已经存在!";
        public static readonly string EmailHasExistMessage = "邮箱已经存在!";
        public static readonly string PhoneHasExistMessage = "手机号已经存在!";
        public static readonly string AccountNonExist = "该用户不存在!";
        public static readonly string PasswordError = "密码错误!";
        public static readonly string RegisterMsg = "REGISTER_MSG";
        public static readonly string LoginMsg = "LOGIN_MSG";


        // Cookie key.
        public static readonly string UserBrowserCookie = "USER_BROWSER_COOKIE";

        public static readonly string AdminUserBrowserCookie = "USER_BROWSER_COOKIE";

        public static readonly string RegisterSuccess = "注册成功!";
        public static readonly string RegisterFailure = "注册失败!";
        public static readonly string PasswordConflict = "PASSWORD_CONFLICT!";
        public static readonly string DuplicateRegister = "DUPLICATE_REGISTER!";

        // Current user key.
        public static readonly string CurrentAccountUser = "CURRENT_ACCOUNT_USER";

        public static readonly string CurrentMerchantUser = "CURRENT_MERCHANT_USER";
        public static readonly string CurrentAdminUser = "CURRENT_USER";
    }
}