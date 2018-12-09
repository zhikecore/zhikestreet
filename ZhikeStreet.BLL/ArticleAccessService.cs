using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhikeStreet.DAL;
using ZhikeStreet.Models.DO;

namespace ZhikeStreet.BLL
{
    public class ArticleAccessService
    {
        private static ArticleAccessService _Instance = null;

        public static ArticleAccessService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ArticleAccessService();
                return _Instance;
            }
        }

        private ArticleAccessService()
        {

        }

        public bool IsExist(int action,int articleId, string ip)
        {
            return ArticleAccessDao.Instance.IsExist(action,articleId,ip);
        }

        public bool Create(ArticleAccess model)
        {
            return ArticleAccessDao.Instance.Create(model);
        }

    }
}
