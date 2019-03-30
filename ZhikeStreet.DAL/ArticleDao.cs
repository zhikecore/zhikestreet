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
    public class ArticleDao
    {
        private static ArticleDao _Instance = null;

        public static ArticleDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ArticleDao();
                return _Instance;
            }
        }

        private ArticleDao()
        {

        }

        public Article GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                Article item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `article` WHERE `Id`=@Id";
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@Id",id),
         };

                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr != null)
                    {
                        item = RowToObject(dr);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public Article GetByType(string articleTypeId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                Article item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `article` WHERE `ArticleTypeId`=@ArticleTypeId";
                    MySqlParameter[] paramters = new MySqlParameter[]
        {
                       new MySqlParameter("@ArticleTypeId",articleTypeId),
        };

                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr != null)
                    {
                        item = RowToObject(dr);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.GetByAccount.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public int Count(int articleTypeId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                int num = 0;
                try
                {
                    string strCmd = @"SELECT COUNT(1) AS NUM FROM `article` WHERE `ArticleTypeId`=@ArticleTypeId";
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@ArticleTypeId",articleTypeId),
         };

                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr == null)
                    {
                        return 0;
                    }

                    num = int.Parse(dr["NUM"].ToString());
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.Count.{0}", ex.Message), new Exception("error"));
                }

                return num;
            }
        }

        public List<Article> GetCarousels()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Article> list = new List<Article>();
                try
                {
                    string strCmd = @" SELECT * FROM `article`  WHERE `IsUp`=TRUE OR `IsRecommend`=TRUE OR `IsHot`=TRUE";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Article item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.GetCarousels.{0}", ex.Message), new Exception("error"));
                }

                return list;
            }
        }

        public List<Article> GetNews()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Article> list = new List<Article>();
                try
                {
                    string strCmd = @" SELECT * FROM `article`  WHERE `IsRecommend`=TRUE";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Article item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.GetNews.{0}", ex.Message), new Exception("error"));
                }

                return list;
            }
        }

        public List<Article> GetAll()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Article> list = new List<Article>();
                try
                {
                    string strCmd = @" SELECT * FROM `article`";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Article item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.GetAll.{0}", ex.Message), new Exception("error"));
                }

                return list;
            }
        }

        public List<Article> GetRelatives(int id,int articletypeId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Article> list = new List<Article>();
                try
                {
                    string strCmd = @" SELECT 
  * 
FROM
  `article` 
WHERE `ArticleTypeId` = @ArticleTypeId 
AND `Id` !=@Id
ORDER BY `CreateTime` DESC 
LIMIT 3 ";

                    MySqlParameter[] paramters = new MySqlParameter[]
        {
                       new MySqlParameter("@Id",id),
                       new MySqlParameter("@ArticleTypeId",articletypeId)
        };

                    DataTable dt = db.GetDataSet(strCmd,paramters);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Article item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.GetRelatives.{0}", ex.Message), new Exception("error"));
                }

                return list;
            }
        }

        public List<Article> GetBySomeWhere(string keyword, int categoryid, int limit, int pageSize, out int total)
        {
            List<Article> filtered = new List<Article>();
            try
            {
                total = 0;
                filtered = _Filter(keyword,categoryid, limit, pageSize);
                total = Count(categoryid,keyword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return filtered;
        }

        public bool Create(Article model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `article` (
  `ArticleTypeId`,
  `ArticleTypeName`,
  `UserId`,
  `TagIds`,
  `Tags`,
  `Title`,
  `LinkUrl`,
  `Cover`,
  `Summary`,
  `Content`,
  `Description`,
  `CreateTime`,
  `ModifyTime`
) 
VALUES
  (
    @ArticleTypeId,
    @ArticleTypeName,
    @UserId,
    @TagIds,
    @Tags,
    @Title,
    @LinkUrl,
    @Cover,
    @Summary,
    @Content,
    @Description,
    NOW(),
    NOW()
  )";
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@ArticleTypeId",model.ArticleTypeId),
                       new MySqlParameter("@ArticleTypeName",model.ArticleTypeName),
                       new MySqlParameter("@UserId",model.UserId),
                       new MySqlParameter("@TagIds",model.TagIds),
                       new MySqlParameter("@Tags",model.Tags),
                       new MySqlParameter("@Title",model.Title),
                       new MySqlParameter("@LinkUrl",model.LinkUrl),
                       new MySqlParameter("@Cover",model.Cover),
                       new MySqlParameter("@Content",model.Content),
                       new MySqlParameter("@Summary",model.Summary),
                       new MySqlParameter("@Description",model.Description),
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

        public bool Update(Article model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"UPDATE 
  `article` 
SET
  `Title`=@Title,
  `LinkUrl`=@LinkUrl,
  `Cover`=@Cover,
  `Summary`=@Summary,
  `Content`=@Content,
  `Description`=@Description
   WHERE `Id` = @Id ";

                    MySqlParameter[] paramters = new MySqlParameter[]
        {
                       new MySqlParameter("@Title",model.Title),
                       new MySqlParameter("@LinkUrl",model.LinkUrl),
                       new MySqlParameter("@Cover",model.Cover),
                       new MySqlParameter("@Summary",model.Summary),
                       new MySqlParameter("@Content",model.Content),
                       new MySqlParameter("@Description",model.Description),
        };

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.Update.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        public bool Update(int action,int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    int count = 0;
                    string strCmd = String.Empty;
                    switch (action)
                    {
                        case 1:
                            //浏览
                            strCmd = @"UPDATE `article` SET `ScanNum`=`ScanNum`+1 WHERE `Id`= @Id";
                            break;
                        case 2:
                            //点赞
                            strCmd = @"UPDATE `article` SET `LikeNum`=`LikeNum`+1  WHERE `Id`=@Id";
                            break;
                    }

                    MySqlParameter[] paramters = new MySqlParameter[]
       {
                       new MySqlParameter("@Id",id)
       };
                    count = db.ExecuteNonQuery(strCmd, paramters);


                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.Update.{0}", ex.Message), new Exception("error"));
                    return false;
                }

            }
        }

        public bool Top(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    int count = 0;
                    string strCmd = @"UPDATE `article` SET `IsUp`=IF(IsUp=0,1,IF(IsUp=1,0,IsUp)) WHERE id=@id";

                    MySqlParameter[] paramters = new MySqlParameter[]
       {
                       new MySqlParameter("@Id",id)
       };
                    count = db.ExecuteNonQuery(strCmd, paramters);


                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.Top.{0}", ex.Message), new Exception("error"));
                    return false;
                }

            }
        }

        public bool Recommend(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    int count = 0;
                    string strCmd = @"UPDATE `article` SET `IsRecommend` =IF(IsRecommend=0,1,IF(IsRecommend=1,0,IsRecommend)) WHERE id=@id";

                    MySqlParameter[] paramters = new MySqlParameter[]
       {
                       new MySqlParameter("@Id",id)
       };
                    count = db.ExecuteNonQuery(strCmd, paramters);


                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.Recommend.{0}", ex.Message), new Exception("error"));
                    return false;
                }

            }
        }

        public bool Hot(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    int count = 0;
                    string strCmd = @"UPDATE `article` SET `IsHot` =IF(IsHot=0,1,IF(IsHot=1,0,IsHot)) where id=@id";

                    MySqlParameter[] paramters = new MySqlParameter[]
       {
                       new MySqlParameter("@Id",id)
       };
                    count = db.ExecuteNonQuery(strCmd, paramters);


                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.Recommend.{0}", ex.Message), new Exception("error"));
                    return false;
                }

            }
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginAccountId"></param>
        /// <returns></returns>
        public bool SoftDelete(int id, string loginAccountId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"UPDATE `article` SET  `IsSoftDelete`=IF( `IsUse`=FALSE,TRUE,FALSE) WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@Id",id)
         };
                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.SoftDelete.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginAccountId"></param>
        /// <returns></returns>
        public bool PhysicDelete(int id, string loginAccount)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"DELETE FROM `article`` WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@Id",MySqlDbType.Int32)
             };

                    paramters[0].Value = id;

                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.PhysicDelete.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        #region Private

        private List<Article> _Filter(string keyword, int categoryid, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Article> list = new List<Article>();
                try
                {
                    //string strCmd = @"SELECT * FROM `article` WHERE `IsHot`=FALSE ";
                    string strCmd = @"SELECT * FROM `article` WHERE 1=1 ";

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        strCmd += @" AND `Title` like " + string.Format("'%{0}%'", keyword);
                    }

                    if (categoryid > 0)
                    {
                        strCmd += @" AND `ArticleTypeId`=@ArticleTypeId";
                    }

                    
                    strCmd += " ORDER BY CreateTime DESC";
                    strCmd += " LIMIT " + limit + "," + pageSize;

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@ArticleTypeId",categoryid)
             };

                    DataTable dt = db.GetDataSet(strCmd,paramters);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Article item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao._Filter.{0}", ex.Message), new Exception("error"));
                }
                return list;
            }

        }

        private int Count(int categoryid, string keyword)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                int total = 0;
                try
                {
                    //string strCmd = @" SELECT * FROM `article` WHERE `IsHot`=FALSE ";
                    string strCmd = @" SELECT COUNT(1) FROM `article` WHERE 1=1 ";

                    if (!String.IsNullOrEmpty(keyword))
                    {
                        strCmd += @" AND `Title` LIKE '%" + keyword + "%'";
                    }

                    if (categoryid > 0)
                    {
                        strCmd += @" AND `ArticleTypeId`=@ArticleTypeId";
                    }

                    MySqlParameter[] paramters = new MySqlParameter[]
             {
                       new MySqlParameter("@ArticleTypeId",categoryid)
             };

                    DataRow row = db.GetDataRow(strCmd, paramters);
                    if (row != null)
                    {
                        total = int.Parse(row["COUNT(1)"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleDao.Count.{0}", ex.Message), ex);
                }
                return total;
            }
        }

        private Article RowToObject(DataRow row)
        {
            return new Article
            {
                Id = int.Parse(row["Id"].ToString()),
                ArticleTypeId = int.Parse(row["ArticleTypeId"].ToString()),
                ArticleTypeName = row["ArticleTypeName"].ToString(),
                UserId = int.Parse(row["UserId"].ToString()),
                TagIds = row["TagIds"].ToString(),
                Tags = row["Tags"].ToString(),
                Title = row["Title"].ToString(),
                LinkUrl = row["LinkUrl"].ToString(),
                Cover = row["Cover"].ToString(),
                Summary = row.IsNull("Summary") ? String.Empty : row["Summary"].ToString(),
                Content=row.IsNull("Content")?String.Empty:row["Content"].ToString(),
                IsUp = bool.Parse(row["IsUp"].ToString()),
                IsRecommend = bool.Parse(row["IsRecommend"].ToString()),
                IsHot= bool.Parse(row["IsHot"].ToString()),
                OpenState = int.Parse(row["OpenState"].ToString()),
                ScanNum = int.Parse(row["ScanNum"].ToString()),
                LikeNum = int.Parse(row["LikeNum"].ToString()),
                CommentNum = int.Parse(row["CommentNum"].ToString()),
                ForwardNum = int.Parse(row["ForwardNum"].ToString()),
                IsSoftDelete = bool.Parse(row["IsSoftDelete"].ToString()),
                Description = row["Description"].ToString(),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString())
            };
        }

        #endregion
    }
}
