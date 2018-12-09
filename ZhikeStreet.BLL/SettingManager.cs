using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhikeStreet.Models.DO;

namespace ZhikeStreet.BLL
{
    public class SettingManager : Dictionary<string, string>
    {
        private static SettingManager _Settings = null;
        public static SettingManager Settings
        {
            get
            {
                if (_Settings == null)
                    _Settings = new SettingManager();
                return _Settings;
            }
        }

        private SettingManager()
        {
            List<KeyManager> keyManagers = KeyManagerService.Instance.GetAll();

            foreach (KeyManager kv in keyManagers)
            {
                if (kv == null)
                    continue;

                if (!this.Keys.Contains(kv.Key))
                    this.Add(kv.Key,kv.Value);
            }
        }

        public string this[string key]
        {
            get
            {
                if (!this.ContainsKey(key))
                    return String.Empty;
                return base[key];
            }
            set
            {
                base[key] = value;
            }
        }

        public static bool GetBoolValue(string key)
        {
            bool value = false;
            bool.TryParse(Settings[key], out value);
            return value;
        }

        public static int GetIntValue(string key)
        {
            int value = 0;
            int.TryParse(Settings[key], out value);
            return value;
        }
    }
}
