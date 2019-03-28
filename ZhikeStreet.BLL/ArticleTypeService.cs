using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhikeStreet.DAL;
using ZhikeStreet.Models.DO;
using ZhikeStreet.Models.VO;

namespace ZhikeStreet.BLL
{
    public class ArticleTypeService
    {
        private static ArticleTypeService _Instance = null;

        public static ArticleTypeService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ArticleTypeService();
                return _Instance;
            }
        }

        private ArticleTypeService()
        {

        }

        public ArticleType GetById(int id)
        {
            return ArticleTypeDao.Instance.GetById(id);
        }

        public object GetArticleTypesForCombox()
        {
            List<object> objs = new List<object>();
            List<ArticleType> articletypes =ArticleTypeDao.Instance.GetAll();
            foreach (ArticleType item in articletypes)
            {
                objs.Add(new { Id = item.Id, Name = item.Name });
            }

            return objs;
        }

        public List<ArticleType> GetAll()
        {
            return ArticleTypeDao.Instance.GetAll();
        }

        public List<ArticleTypeViewModel> GetArticleTypeViews()
        {
            List<ArticleTypeViewModel> views = new List<ArticleTypeViewModel>();
            List<ArticleType> articleTypes = ArticleTypeDao.Instance.GetAll();
            foreach (ArticleType item in articleTypes)
            {
                if (item == null)
                    continue;

                int count = ArticleDao.Instance.Count(item.Id);

                ArticleTypeViewModel view = new ArticleTypeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ArticleCount = count,
                    Description=item.Description
                };
                views.Add(view);
            }
            return views;
        }

    }
}
