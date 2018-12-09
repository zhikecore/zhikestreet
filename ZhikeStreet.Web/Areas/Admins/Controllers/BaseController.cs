using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhikeStreet.Web.Areas.Admins.Filters;

namespace ZhikeStreet.Web.Areas.Admins.Controllers
{
    [LoginFilter]
    public class BaseController : Controller
    {

    }
}