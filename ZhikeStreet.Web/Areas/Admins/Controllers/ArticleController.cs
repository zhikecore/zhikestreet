using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZhikeStreet.BLL;
using ZhikeStreet.Common.Utility;
using ZhikeStreet.Models.DO;
using ZhikeStreet.Models.VO;
using ZhikeStreet.Web.Areas.Admins.Helpers;

namespace ZhikeStreet.Web.Areas.Admins.Controllers
{
    public class ArticleController : BaseController
    {
        // GET: Admins/Article
        public ActionResult Index()
        {
            return View();
        }

        #region Public

        //combox
        public JsonResult GetArticleTypesForCombox()
        {
            return Json(ArticleTypeService.Instance.GetArticleTypesForCombox(),JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTagsForCombox()
        {
            return Json(TagService.Instance.GetTagsForCombox(), JsonRequestBehavior.AllowGet);
        }

        //Data table
        public JsonResult GetDataTableRecords()
        {
            //Base Params
            DataTableParams dtParams = new DataTableParams(this);

            //Custom Parmas
            string keyword = Request["Keyword"];
            int categoryid = null == Request["categoryid"] ? 0 :int.Parse(Request["categoryid"].ToString());

            var dtRecords = ToDataTable(dtParams,categoryid, keyword);

            return Json(dtRecords, JsonRequestBehavior.AllowGet);
        }

        public Object ToDataTable(DataTableParams dtParams,int categoryid, string keyword)
        {
            int total = 0;
            var filters =ArticleService.Instance.GetBySomeWhere(keyword, dtParams.DisplayStart, dtParams.DisplayLength,categoryid, out total);

            List<ArticleViewModel> _NewFilters = ToNewFilters(filters);

            var aaData = new Object();
            if (_NewFilters.Count > dtParams.DisplayLength)
                aaData = _NewFilters.Skip(dtParams.DisplayStart).Take(dtParams.DisplayLength).Select(m => ToJson(m, dtParams));
            else
                aaData = _NewFilters.Select(m => ToJson(m, dtParams));

            return new { sEcho = dtParams.Echo, iTotalRecords = _NewFilters.Count, iTotalDisplayRecords = total, aaData = aaData };

            //var aaData = new Object();
            //if (filters.Count > dtParams.DisplayLength)
            //    aaData = filters.Skip(dtParams.DisplayStart).Take(dtParams.DisplayLength).Select(m => ToJson(m, dtParams));
            //else
            //    aaData = filters.Select(m => ToJson(m, dtParams));

            //return new { sEcho = dtParams.Echo, iTotalRecords = filters.Count, iTotalDisplayRecords = total, aaData = aaData };
        }

        [HttpPost]
        [System.Web.Services.WebMethod]
        [ValidateInput(false)]
        public JsonResult Create(
            int articleTypeId,
            string tagIds,
            string title,
            string linkUrl,
            string cover,
            string summary,
            string content,
            string description
            )
        {
            var oResult = new Object();

            try
            {
                //登录验证
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
                if (currentAdminUser == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "登录超时!",
                        Url = "/admins/account/login"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                //必填参数验证
                if (String.IsNullOrEmpty(title))
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "标题不能为空!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                if (String.IsNullOrEmpty(summary))
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "摘要不能为空!",
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                string articleTypeName = String.Empty;
                if(articleTypeId>0)
                {
                    ArticleType articleType=ArticleTypeService.Instance.GetById(articleTypeId);
                    if (articleType != null)
                    {
                        articleTypeName = articleType.Name;
                    }
                }

                string tagNames = String.Empty;
                StringBuilder sbTags = new StringBuilder();
                if (!String.IsNullOrEmpty(tagIds))
                {
                    List<int> lstTagIds = tagIds.TrimEnd(',').Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();

                    foreach (int id in lstTagIds)
                    {
                        if (id == 0)
                            continue;

                        Tag tag = TagService.Instance.GetById(id);

                        if (tag != null)
                        {
                            sbTags.Append(tag.Name);
                            sbTags.Append(',');
                        }
                    }
                    tagNames = sbTags.ToString().TrimEnd(',');
                }
                
                //Markdown mk = new Markdown();
                //string htmlContent = mk.Transform(content);

                Article article = new Article
                {
                    ArticleTypeId=articleTypeId,
                    ArticleTypeName=articleTypeName,
                    UserId= currentAdminUser.Id,
                    TagIds=tagIds,
                    Tags=tagNames,
                    Title=title,
                    LinkUrl=linkUrl,
                    Cover=cover,
                    Summary=summary,
                    Content= content,
                    Description=description
                };
                
                bool bRet = ArticleService.Instance.Create(article);
                oResult = new
                {
                    Bresult = bRet,
                    Notice = bRet ? "操作成功!" : "操作失败!"
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("操作失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [System.Web.Services.WebMethod]
        [ValidateInput(false)]
        public JsonResult Update(
            int id,
            string title,
            string linkUrl,
            string cover,
            string content,
            string description)
        {
            var oResult = new Object();
            try
            {
                //登录验证
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
                if (currentAdminUser == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "登录超时!",
                        Url = "/admins/account/login"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                //必填参数验证
                if (String.IsNullOrEmpty(title))
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "文章标题不为空!",
                    };

                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                Article originArticle = ArticleService.Instance.GetById(id);
                if (originArticle == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "不存在该文章!请检查!"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }
                
                string articleTypeName = originArticle.ArticleTypeName;
                string tags = String.Empty;
                originArticle.Title =title ;
                originArticle.LinkUrl = linkUrl;
                originArticle.Cover = cover;
                originArticle.Content = content;
                originArticle.Description = description;
                
                bool bRet = ArticleService.Instance.Update(originArticle);
                oResult = new
                {
                    Bresult = bRet,
                    Notice = bRet ? "操作成功!" : "操作失败!"
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("操作失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpDelete]
        //public JsonResult PhysicDelete(int id)
        //{
        //    var oResult = new Object();
        //    try
        //    {
        //        //登录验证
        //        var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
        //        if (currentAdminUser == null)
        //        {
        //            oResult = new
        //            {
        //                Bresult = false,
        //                Notice = "登录超时!",
        //                Url = "/admins/account/login"
        //            };
        //            return Json(oResult, JsonRequestBehavior.AllowGet);
        //        }

        //        App originApp = AppService.Instance.GetById(id);
        //        if (originApp == null)
        //        {
        //            oResult = new
        //            {
        //                Bresult = false,
        //                Notice = "不存在该应用!请检查!"
        //            };
        //            return Json(oResult, JsonRequestBehavior.AllowGet);
        //        }

        //        bool bRet = AppService.Instance.PhysicDelete(id, currentAdminUser.Account);

        //        oResult = new
        //        {
        //            Bresult = bRet,
        //            Notice = bRet ? "操作成功!" : "操作失败!"
        //        };
        //        return Json(oResult, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        oResult = new
        //        {
        //            Bresult = false,
        //            Notice = String.Format("操作失败!异常:{0}", ex.Message)
        //        };
        //        return Json(oResult, JsonRequestBehavior.AllowGet);
        //    }
        //}

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Top(int id)
        {
            var oResult = new Object();
            try
            {
                //登录验证
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
                if (currentAdminUser == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "登录超时!",
                        Url = "/admins/account/login"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                bool bRet = ArticleService.Instance.Top(id);
                if (bRet)
                {
                    oResult = new
                    {
                        Bresult = true,
                        Notice = "文章置顶成功!"
                    };
                }
                else
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "文章置顶失败!请检查!"
                    };
                }
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("操作失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 推荐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Recommend(int id)
        {
            var oResult = new Object();
            try
            {
                //登录验证
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
                if (currentAdminUser == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "登录超时!",
                        Url = "/admins/account/login"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                bool bRet = ArticleService.Instance.Recommend(id);
                if (bRet)
                {
                    oResult = new
                    {
                        Bresult = true,
                        Notice = "文章置顶成功!"
                    };
                }
                else
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "文章置顶失败!请检查!"
                    };
                }
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("操作失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 热门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Hot(int id)
        {
            var oResult = new Object();
            try
            {
                //登录验证
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as AdminUser;
                if (currentAdminUser == null)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "登录超时!",
                        Url = "/admins/account/login"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }

                bool bRet = ArticleService.Instance.Hot(id);
                if (bRet)
                {
                    oResult = new
                    {
                        Bresult = true,
                        Notice = "文章置顶成功!"
                    };
                }
                else
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "文章置顶失败!请检查!"
                    };
                }
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                oResult = new
                {
                    Bresult = false,
                    Notice = String.Format("操作失败!异常:{0}", ex.Message)
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region Private

        private List<ArticleViewModel> ToNewFilters(List<Article> filters)
        {
            List<ArticleViewModel> _NewFilters = new List<ArticleViewModel>();

            foreach (Article item in filters)
            {
                if (item == null)
                    continue;

                string author = String.Empty;
                AdminUser user = AdminUserService.Instance.GetById(item.UserId);
                if (user != null)
                {
                    author = user.Account;
                }

                _NewFilters.Add(new ArticleViewModel
                {
                    Id = item.Id,
                    ArticleTypeId = item.ArticleTypeId,
                    ArticleTypeName = item.ArticleTypeName,
                    UserId = item.UserId,
                    TagIds=item.TagIds,
                    Tags=item.Tags,
                    Title=item.Title,
                    LinkUrl=item.LinkUrl,
                    Cover=item.Cover,
                    Summary=item.Summary,
                    Content=item.Content,
                    IsUp=item.IsUp,
                    IsRecommend=item.IsRecommend,
                    OpenState=item.OpenState,
                    ScanNum=item.ScanNum,
                    LikeNum=item.LikeNum,
                    CommentNum=item.CommentNum,
                    ForwardNum=item.ForwardNum,
                    IsSoftDelete=item.IsSoftDelete,
                    Description = item.Description,
                    Author = author,
                    CreateTime = item.CreateTime,
                    ModifyTime = item.ModifyTime
                });
            }

            return _NewFilters;
        }

        private Object ToJson(ArticleViewModel model, DataTableParams dtParams)
        {
            var json = model.AsJson() as Hashtable;
            json.Add("Actions", RenderViewHelper.RenderToString("_Actions", model, dtParams.Controller));

            return json;
        }

        #endregion
    }
}