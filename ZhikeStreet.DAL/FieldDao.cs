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
    public class FieldDao
    {
        private static FieldDao _Instance = null;

        public static FieldDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FieldDao();
                return _Instance;
            }
        }

        private FieldDao()
        {

        }

        public Field GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                Field item = null;
                try
                {
                    string strCmd = @"SELECT * FROM  `field` WHERE `Id`=@Id";
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
                    Log4Helper.Error(this.GetType(), String.Format("FieldDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public List<Field> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            List<Field> filtered = new List<Field>();
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

        public bool Create(Field model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"INSERT INTO `field` (
  `Name`,
  `Description`,
  `CreateTime`,
  `ModifyTime`
) 
VALUES
  (
    @Name,
    @Description,
    NOW(),
    NOW()
  )";
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@Name",model.Name),
                       new MySqlParameter("@Description",model.Description),
         };
                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("FieldDao.Create.{0}", ex.Message), new Exception("error"));
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
                    string strCmd = @"UPDATE `field` SET  `IsSoftDelete`=IF( `IsSoftDelete`=FALSE,TRUE,FALSE) WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@Id",id)
         };
                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("FieldDao.SoftDelete.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        #region Private

        private List<Field> _Filter(string keyword, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<Field> list = new List<Field>();
                try
                {
                    string strCmd = @"SELECT * FROM `field` WHERE 1=1";

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
                            Field item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("FieldDao._Filter.{0}", ex.Message), new Exception("error"));
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
                    string strCmd = @" SELECT COUNT(1) FROM `field` WHERE 1=1 ";

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
                    Log4Helper.Error(this.GetType(), String.Format("FieldDao.Count.{0}", ex.Message), ex);
                }
                return total;
            }
        }

        private Field RowToObject(DataRow row)
        {
            return new Field
            {
                Id = int.Parse(row["Id"].ToString()),
                Name = row["Name"].ToString(),
                IsSoftDelete = bool.Parse(row["IsSoftDelete"].ToString()),
                CreateTime = row.IsNull("CreatTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString())
            };
        }

        #endregion
    }
}
