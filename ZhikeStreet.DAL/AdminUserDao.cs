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
using ZhikeStreet.Models.VO;

namespace ZhikeStreet.DAL
{
    public class AdminUserDao
    {
        private static AdminUserDao _Instance = null;

        public static AdminUserDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AdminUserDao();
                return _Instance;
            }
        }

        private AdminUserDao()
        {

        }

        public AdminUser GetById(int id)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                AdminUser item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `adminuser` WHERE `Id`=@Id";
                    MySqlParameter[] paramters = new MySqlParameter[]
                {
                       new MySqlParameter("@Id",MySqlDbType.String)
                };

                    paramters[0].Value = id;

                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr != null)
                    {
                        item = RowToObject(dr);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.GetById.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        public AdminUser GetByAccount(string account)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                AdminUser item = null;
                try
                {
                    string strCmd = @"SELECT * FROM `adminuser` WHERE `Account`=@Account";
                    MySqlParameter[] paramters = new MySqlParameter[]
                {
                       new MySqlParameter("@Account",account)
                };
                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr != null)
                    {
                        item = RowToObject(dr);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.GetByAccount.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }
        
        public List<AdminUser> GetAll()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<AdminUser> list = new List<AdminUser>();
                try
                {
                    string strCmd = @" SELECT * FROM `adminuser`";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            AdminUser item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.GetAll.{0}", ex.Message), new Exception("error"));
                }

                return list;
            }
        }

        public List<AdminUser> GetBySomeWhere(string keyword, int limit, int pageSize, out int total)
        {
            List<AdminUser> filtered = new List<AdminUser>();
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

        public bool IsExist(int flag, RegisterViewModel model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    StringBuilder sbText = new StringBuilder();
                    sbText.Append(@"SELECT COUNT(1) AS num  FROM `adminuser` WHERE 1=1 ");

                    switch(flag)
                    {
                        case 0:
                            //账户
                            sbText.Append("AND  `Account`=@Account");
                            break;
                        case 1:
                            //邮箱
                            sbText.Append("AND  `Email`=@Email");
                            break;
                        case 2:
                            //手机号码
                            sbText.Append("AND  `Phone`=@Phone");
                            break;
                    }
                    
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@Account",model.Account),
                       new MySqlParameter("@Email",model.Email),
                       new MySqlParameter("@Phone",model.Phone),
         };
                    DataRow dr = db.GetDataRow(sbText.ToString(),paramters);
                    if (dr == null)
                    {
                        return false;
                    }

                    int num = int.Parse(dr["num"].ToString());
                    return num > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.IsExist.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        public bool Create(AdminUser model)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"
INSERT INTO `adminuser` (
  Account,
  PasswordSalt,
  Phone,
  QQ,
  Email,
  NickName,
  RealName,
  Avatar,
  RegIp,
  IsUse,
  Description,
  CreateTime,
  ModifyTime
) 
VALUES
  (
    @Account,
    @PasswordSalt,
    @Phone,
    @QQ,
    @Email,
    @NickName,
    @RealName,
    @Avatar,
    @RegIp,
    @IsUse,
    @Description,
    NOW(),
    NOW()
  )";
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@Account",model.Account),
                       new MySqlParameter("@PasswordSalt",model.PasswordSalt),
                       new MySqlParameter("@Phone",model.Phone),
                       new MySqlParameter("@QQ",model.QQ),
                       new MySqlParameter("@Email",model.Email),
                       new MySqlParameter("@NickName",model.NickName),
                       new MySqlParameter("@RealName",model.RealName),
                       new MySqlParameter("@Avatar",model.Avatar),
                       new MySqlParameter("@RegIp",model.RegIp),
                       new MySqlParameter("@IsUse",model.IsUse),
                       new MySqlParameter("@Description",model.Description),
         };
                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        /// <summary>
        /// 设置是否可用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginAccountId"></param>
        /// <returns></returns>
        public bool CanUse(int id, string loginAccountId)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"UPDATE `adminuser` SET `IsUse`=IF( `IsUse`=FALSE,TRUE,FALSE) WHERE `Id`=@Id";

                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@Id",id)
         };
                    int count = db.ExecuteNonQuery(strCmd, paramters);
                    return count > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.CanUse.{0}", ex.Message), new Exception("error"));
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
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr_pangu"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                try
                {
                    string strCmd = @"DELETE FROM `adminuser` WHERE `Id`=@Id";

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
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.Create.{0}", ex.Message), new Exception("error"));
                    return false;
                }
            }
        }

        #region Private

        private List<AdminUser> _Filter(string keyword, int limit, int pageSize)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<AdminUser> list = new List<AdminUser>();
                try
                {
                    string strCmd = @"SELECT * FROM `adminuser` WHERE 1=1";

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        strCmd += @" AND `Name` like " + string.Format("%{0}%", keyword);
                    }

                    strCmd += " ORDER BY CreateTime DESC";
                    strCmd += " LIMIT " + limit + "," + pageSize;

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            AdminUser item = RowToObject(row);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.GetAll.{0}", ex.Message), new Exception("error"));
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
                    string strCmd = @" SELECT COUNT(1) FROM `adminuser` WHERE 1=1 ";

                    if (!String.IsNullOrEmpty(account))
                    {
                        strCmd += @" AND `Account` LIKE '%" + account + "%'";
                    }

                    DataRow row = db.GetDataRow(strCmd);
                    if (row != null)
                    {
                        total = int.Parse(row["COUNT(1)"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("AdminUserDao.Count.{0}", ex.Message), ex);
                }
                return total;
            }
        }

        private AdminUser RowToObject(DataRow row)
        {
            return new AdminUser
            {
                Id = int.Parse(row["Id"].ToString()),
                Account = row["Account"].ToString(),
                PasswordSalt=row["PasswordSalt"].ToString(),
                PhoneValideCode=row.IsNull("PhoneValideCode")?String.Empty:row["PhoneValideCode"].ToString(),
                Phone = row["Phone"].ToString(),
                QQ = row["QQ"].ToString(),
                Email = row["Email"].ToString(),
                RealName = row["RealName"].ToString(),
                Avatar = row["Avatar"].ToString(),
                RegIp= row["RegIp"].ToString(),
                IsUse = bool.Parse(row["IsUse"].ToString()),
                Description = row["Description"].ToString(),
                CreateTime = row.IsNull("CreateTime") ? new DateTime() : DateTime.Parse(row["CreateTime"].ToString()),
                ModifyTime = row.IsNull("ModifyTime") ? new DateTime() : DateTime.Parse(row["ModifyTime"].ToString())
            };
        }

        #endregion
    }
}
