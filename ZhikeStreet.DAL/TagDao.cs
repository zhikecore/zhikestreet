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
    public class TagDao
    {
        private static TagDao _Instance = null;

        public static TagDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new TagDao();
                return _Instance;
            }
        }

        private TagDao()
        {

        }

        public Tag GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                Tag item = null;
                try
                {
                    string strCmd = @"SELECT * FROM  `tag` WHERE `Id`=@Id";
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
                    Log4Helper.Error(this.GetType(), String.Format("TagsDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public List<Tag> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            List<Tag> filtered = new List<Tag>();
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

        public List<Tag> GetAll()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Tag> tags = new List<Tag>();
                try
                {
                    string strCmd = @" SELECT * FROM `tag` WHERE `FieldId`=1";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Tag item = RowToObject(row);
                            tags.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("TagDao.GetAll.{0}", ex.Message), new Exception("error"));
                }

                return tags;
            }
        }

        public bool Create(Tag model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `tag` (
  `FieldId`,
  `Name`,
  `IsSoftDelete`
  `Description`,
  `CreateTime`,
  `ModifyTime`
) 
VALUES
  (
    @FieldId,
    @Name,
    @IsSoftDelete,
    @Description,
    NOW(),
    NOW()
  )";
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@FieldId",model.FieldId),
                       new MySqlParameter("@Name",model.Name),
                       new MySqlParameter("@IsSoftDelete",model.IsSoftDelete),
                       new MySqlParameter("@Description",model.Description),
         };
                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("TagDao.Create.{0}", ex.Message), new Exception("error"));
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
                    string strCmd = @"UPDATE `tag` SET  `IsSoftDelete`=IF( `IsSoftDelete`=FALSE,TRUE,FALSE) WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@Id",id)
         };
                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("TagDao.SoftDelete.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        #region Private

        private List<Tag> _Filter(string keyword, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Tag> list = new List<Tag>();
                try
                {
                    string strCmd = @"SELECT * FROM `tag` WHERE 1=1";

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
                            Tag item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("TagDao._Filter.{0}", ex.Message), new Exception("error"));
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
                    string strCmd = @" SELECT COUNT(1) FROM `tag` WHERE 1=1 ";

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
                    Log4Helper.Error(this.GetType(), String.Format("TagDao.Count.{0}", ex.Message), ex);
                }
                return total;
            }
        }

        private Tag RowToObject(DataRow row)
        {
            return new Tag
            {
                Id = int.Parse(row["Id"].ToString()),
                FieldId= int.Parse(row["FieldId"].ToString()),
                Name = row["Name"].ToString(),
                IsSoftDelete = bool.Parse(row["IsSoftDelete"].ToString()),
                Description=row["Description"].ToString(),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString())
            };
        }

        #endregion
    }
}
