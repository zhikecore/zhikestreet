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
    public class ArticleTypeDao
    {
        private static ArticleTypeDao _Instance = null;

        public static ArticleTypeDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ArticleTypeDao();
                return _Instance;
            }
        }

        private ArticleTypeDao()
        {

        }

        public ArticleType GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                ArticleType item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `article_type` WHERE `Id`=@Id";
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
                    Log4Helper.Error(this.GetType(), String.Format("ArticleTypeDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public List<ArticleType> GetAll()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<ArticleType> _AppTypes = new List<ArticleType>();
                try
                {
                    string strCmd = @" SELECT * FROM `article_type` WHERE `ParenId` > 0";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ArticleType item = RowToObject(row);
                            _AppTypes.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("PlatformDao.GetAll.{0}", ex.Message), new Exception("error"));
                }

                return _AppTypes;
            }
        }
    
        public List<ArticleType> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            List<ArticleType> filtered = new List<ArticleType>();
            try
            {
                total = 0;
                filtered = _Filter(keyword, limit, pageSize);
                total = Count(keyword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return filtered;
        }

        public bool Create(ArticleType model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `article_type` (
  `ParenId`，  
  `Name`,
  `Description`,
  `CreateTime`,
  `ModifyTime`
) 
VALUES
  (
    @ParenId,
    @Name,
    @Description,
    NOW(),
    NOW()
  )";
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@ParenId",model.ParenId),
                       new MySqlParameter("@Name",model.Name),
                       new MySqlParameter("@Description",model.Description),
         };
                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleTypeDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }
        
        #region Private

        private List<ArticleType> _Filter(string keyword, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<ArticleType> list = new List<ArticleType>();
                try
                {
                    string strCmd = @"SELECT * FROM `article_type` WHERE 1=1";

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        strCmd += @" AND `Title` like " + string.Format("%{0}%", keyword);
                    }

                    strCmd += " ORDER BY CreateTime DESC";
                    strCmd += " LIMIT " + limit + "," + pageSize;

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ArticleType item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleTypeDao._Filter.{0}", ex.Message), new Exception("error"));
                }
                return list;
            }

        }

        private int Count(string account)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                int total = 0;
                try
                {
                    string strCmd = @" SELECT COUNT(1) FROM `article_type` WHERE 1=1 ";

                    if (!String.IsNullOrEmpty(account))
                    {
                        strCmd += @" AND `Name` LIKE '%" + account + "%'";
                    }

                    DataRow row = db.GetDataRow(strCmd);
                    if (row != null)
                    {
                        total = int.Parse(row["COUNT(1)"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("ArticleTypeDao.Count.{0}", ex.Message), ex);
                }
                return total;
            }
        }

        private ArticleType RowToObject(DataRow row)
        {
            return new ArticleType
            {
                Id = int.Parse(row["Id"].ToString()),
                ParenId = int.Parse(row["ParenId"].ToString()),
                Name = row["Name"].ToString(),
                Description=row["Descrption"].ToString(),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString())
            };
        }

        #endregion
    }
}
