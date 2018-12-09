using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhikeStreet.Web.Filters;

namespace ZhikeStreet.Web.Controllers
{
    [LoginFilter]
    public class BaseController : Controller
    {

    }
}