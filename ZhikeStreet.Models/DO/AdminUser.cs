using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class AdminUser
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string PasswordSalt { get; set; }
        public string PhoneValideCode { get; set; }
        public string Phone { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string RealName { get; set; }
        public string Avatar { get; set; }
        public string RegIp { get; set; }
        public bool IsUse { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
