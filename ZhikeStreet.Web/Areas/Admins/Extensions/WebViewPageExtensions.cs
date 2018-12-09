using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZhikeStreet.Models.DO;
using ZhikeStreet.Web.Areas.Admins.Helpers;

namespace System.Web.Mvc
{
    public static class WebViewPageExtensions
    {
        public static AdminUser CurrentAdminUser(this WebViewPage wvp)
        {
            return wvp.Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
        }
    }
}