using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhikeStreet.Common.Utility;
using ZhikeStreet.Models.DO;

namespace ZhikeStreet.DAL
{
    public class ArticleAccessDao
    {
        private static ArticleAccessDao _Instance = null;

        public static ArticleAccessDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ArticleAccessDao();
                return _Instance;
            }
        }

        private ArticleAccessDao()
        {

        }

        public bool IsExist(int action,int articleId,string ip)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"SELECT COUNT(1) AS Num FROM `article_access` WHERE `ArticleId`=@ArticleId AND `IP`=@IP AND `Action`=@Action";

                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@ArticleId",articleId),
                       new MySqlParameter("@IP",ip),
                       new MySqlParameter("@Action",action)
         };
                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    int num =int.Parse(dr["Num"].ToString());
                    return num > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleAccessDao.IsExist.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        public bool Create(ArticleAccess model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO article_access
            (articleid,
             userid,
             ACTION,
             ip,
             createtime)
VALUES (
        @articleid,
        @userid,
        @action,
        @ip,
        NOW());
  )";
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@articleid",model.ArticleId),
                       new MySqlParameter("@userid",model.UserId),
                       new MySqlParameter("@action",model.Action),
                       new MySqlParameter("@ip",model.IP),
         };
                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }
    }
}
