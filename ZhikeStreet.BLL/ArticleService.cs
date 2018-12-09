using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhikeStreet.DAL;
using ZhikeStreet.Models.DO;

namespace ZhikeStreet.BLL
{
    public class ArticleService
    {
        private static ArticleService _Instance = null;

        public static ArticleService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ArticleService();
                return _Instance;
            }
        }

        private ArticleService()
        {

        }

        public Article GetById(int id)
        {
            return ArticleDao.Instance.GetById(id);
        }

        public Article GetByType(string articleTypeId)
        {
            return ArticleDao.Instance.GetByType(articleTypeId);
        }

        public List<Article> GetRelatives(int id, int articletypeId)
        {
            return ArticleDao.Instance.GetRelatives(id,articletypeId);
        }

        public List<Article> GetCarousels()
        {
            return ArticleDao.Instance.GetCarousels();
        }

        public List<Article> GetNews()
        {
            return ArticleDao.Instance.GetNews();
        }

        public List<Article> GetAll()
        {
            return ArticleDao.Instance.GetAll();
        }

        public int Count(int articleTypeId)
        {
            return ArticleDao.Instance.Count(articleTypeId);
        }

        public List<Article> GetBySomeWhere(string keyword, int limit, int pageSize,int categoryid, out int total)
        {
            return ArticleDao.Instance.GetBySomeWhere(keyword,categoryid,limit,pageSize,out total);
        }

        public bool Create(Article model)
        {
            return ArticleDao.Instance.Create(model);
        }

        public bool Update(Article model)
        {
            return ArticleDao.Instance.Update(model);
        }

        public bool Update(int action,int id)
        {
            return ArticleDao.Instance.Update(action,id);
        }

        public bool Top(int id)
        {
            return ArticleDao.Instance.Top(id);
        }
        public bool Recommend(int id)
        {
            return ArticleDao.Instance.Recommend(id);
        }
        public bool Hot(int id)
        {
            return ArticleDao.Instance.Hot(id);
        }
    }
}
