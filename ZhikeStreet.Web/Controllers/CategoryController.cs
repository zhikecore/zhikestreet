using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhikeStreet.BLL;
using ZhikeStreet.Models.DO;
using ZhikeStreet.Models.VO;

namespace ZhikeStreet.Web.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            List<ArticleTypeViewModel> categories = ArticleTypeService.Instance.GetArticleTypeViews();
            return View(categories);
        }
        
        public ActionResult Detail(int id)
        {
            ViewData["id"] = id;
            return View();
        }
    }
}