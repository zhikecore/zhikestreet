using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhikeStreet.Common.Utility;

namespace ZhikeStreet.Web.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult TA()
        {
            return View();
        }

        #region Private

        private string Transform()
        {
            /*
            string mkStr = @"";
            Markdown mk = new Markdown();
            string html=mk.Transform(mkStr);
            return html;*/
            return String.Empty;
        }


        #endregion
    }
}