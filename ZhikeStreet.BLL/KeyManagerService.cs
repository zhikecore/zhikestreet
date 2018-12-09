using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhikeStreet.DAL;
using ZhikeStreet.Models.DO;

namespace ZhikeStreet.BLL
{
    public class KeyManagerService
    {
        private static KeyManagerService _Instance = null;

        public static KeyManagerService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new KeyManagerService();
                return _Instance;
            }
        }

        private KeyManagerService()
        {

        }

        public List<KeyManager> GetAll()
        {
            return KeyManagerDao.Instance.GetAll();
        }
    }
}
