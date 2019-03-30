using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhikeStreet.BLL;
using ZhikeStreet.Common.Utility;
using ZhikeStreet.Models.DO;
using ZhikeStreet.Models.VO;

namespace ZhikeStreet.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var key = null == Request["key"] ? String.Empty : Request["key"].ToString();
            ViewData["Key"] = key;
            List<Article> carousels = ArticleService.Instance.GetCarousels();
            return View(carousels);
        }

        public ActionResult Detail(int id)
        {
            Article article = ArticleService.Instance.GetById(id);
            if (article != null)
            {
                //执行浏览统计
                string ip = IPHelper.GetWebClientIp();

                //1.检查是否重复
                bool isActioned = ArticleAccessService.Instance.IsExist(1, id, ip);
                if (!isActioned)
                {
                    ArticleAccess access = new ArticleAccess
                    {
                        ArticleId=id,
                        UserId=0,
                        Action=1,
                        IP=ip
                    };
                    ArticleAccessService.Instance.Create(access);
                    ArticleService.Instance.Update(1, id);
                }
            }

            //获取相关主题文章
            List<Article> relatives = ArticleService.Instance.GetRelatives(article.Id, article.ArticleTypeId);

            ArticleViewModel vm = ToViewModel(article);
            vm.Previous  = ArticleService.Instance.GetById(id - 1); ;
            vm.Next= ArticleService.Instance.GetById(id + 1);
            vm.Relatives = relatives;
            return View(vm);
        }

        public JsonResult GetArticles(int curPage,int pagesize,int categoryid, string key)
        {
            int total = 0;
            int limit = (curPage - 1) * pagesize;
            var filters = ArticleService.Instance.GetBySomeWhere(key,limit,pagesize,categoryid, out total);
            List<ArticleViewModel> articleViews = ToNewFilters(filters);

            Object o = new
            {
                totalCount = total,
                data =articleViews
            };


            return Json(o, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCarousels()
        {
            List<Article> carousels = ArticleService.Instance.GetCarousels();
            return Json(carousels,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNews()
        {
            List<Article> carousels = ArticleService.Instance.GetNews();
            return Json(carousels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Praise(int id)
        {
            bool bRet = true;
            lock (this)
            {
                string ip = IPHelper.GetWebClientIp();
                bool isActioned = ArticleAccessService.Instance.IsExist(2, id, ip);
                if (!isActioned)
                {
                    ArticleAccess access = new ArticleAccess
                    {
                        ArticleId = id,
                        UserId = 0,
                        Action = 2,
                        IP = ip
                    };
                    ArticleAccessService.Instance.Create(access);
                    bRet = ArticleService.Instance.Update(2, id);
                }
            }
            var obj = new
            {
                Result = bRet,
                Notice = bRet ? "" : "点赞失败!"
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #region Private

        private List<ArticleViewModel> ToNewFilters(List<Article> filters)
        {
            List<ArticleViewModel> _NewFilters = new List<ArticleViewModel>();

            foreach (Article item in filters)
            {
                if (item == null)
                    continue;

                ArticleViewModel vm = ToViewModel(item);
                _NewFilters.Add(vm);
            }

            return _NewFilters;
        }

        private ArticleViewModel ToViewModel(Article item)
        {
            ArticleViewModel vm = null;
            if (item != null)
            {
                string author = String.Empty;
                AdminUser user = AdminUserService.Instance.GetById(item.UserId);
                if (user != null)
                {
                    author = user.RealName;
                }

                vm = new ArticleViewModel
                {
                    Id = item.Id,
                    ArticleTypeId = item.ArticleTypeId,
                    ArticleTypeName = item.ArticleTypeName,
                    UserId = item.UserId,
                    TagIds = item.TagIds,
                    Tags = item.Tags,
                    Title = item.Title,
                    LinkUrl = item.LinkUrl,
                    Cover = item.Cover,
                    Summary = item.Summary,
                    Content = item.Content,
                    IsUp = item.IsUp,
                    IsRecommend = item.IsRecommend,
                    OpenState = item.OpenState,
                    ScanNum = item.ScanNum,
                    LikeNum = item.LikeNum,
                    CommentNum = item.CommentNum,
                    ForwardNum = item.ForwardNum,
                    IsSoftDelete = item.IsSoftDelete,
                    Description = item.Description,
                    Author = author,
                    CreateTime = item.CreateTime,
                    ModifyTime = item.ModifyTime
                };
            }

            return vm;
        }

        #endregion

    }

    public class user
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string timespan { get; set; }
    }
}