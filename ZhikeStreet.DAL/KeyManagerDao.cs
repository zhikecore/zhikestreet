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
    public class KeyManagerDao
    {
        private static KeyManagerDao _Instance = null;

        public static KeyManagerDao Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new KeyManagerDao();
                return _Instance;
            }
        }

        private KeyManagerDao()
        {

        }

        public List<KeyManager> GetAll()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                List<KeyManager> keyValues = new List<KeyManager>();
                try
                {
                    string strCmd = @" SELECT * FROM `key_manager` ";

                    DataTable dt = db.GetDataSet(strCmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            KeyManager item = RowToObject(row);
                            keyValues.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("SettingMangerDao.GetAll.{0}", ex.Message), new Exception("error"));
                }

                return keyValues;
            }
        }

        public KeyManager GetByKey(string key)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connectStr"];
            string conStr = settings.ConnectionString;
            using (ZhikeStreet.Common.DB.DBHelper db = new Common.DB.DBHelper(conStr))
            {
                KeyManager item = null;
                try
                {
                    string strCmd = @"SELECT  * FROM `key_manager` WHERE `Key`=@Key";
                    MySqlParameter[] paramters = new MySqlParameter[]
         {
                       new MySqlParameter("@Key",key),
         };

                    DataRow dr = db.GetDataRow(strCmd, paramters);
                    if (dr != null)
                    {
                        item = RowToObject(dr);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), String.Format("SettingMangerDao.GetByKey.{0}", ex.Message), new Exception("error"));
                }

                return item;
            }
        }

        #region Private

        private KeyManager RowToObject(DataRow row)
        {
            return new KeyManager
            {
                Key=row["Key"].ToString(),
                Value=row["Value"].ToString()
            };
        }

        #endregion
    }
}
