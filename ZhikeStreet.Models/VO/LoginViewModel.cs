using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.VO
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("账号")]
        public string Account { get; set; }

        [Required]
        [DisplayName("密码")]
        public string Password { get; set; }

 
        [DisplayName("手机验证码")]
        public string PhoneValideCode { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [DisplayName("账号")]
        public string Account { get; set; }

        [Required]
        [DisplayName("密码")]
        public string Password { get; set; }

        [Required]
        [DisplayName("重复密码")]
        public string RePassword { get; set; }

        [Required]
        [DisplayName("邮箱")]
        public string Email { get; set; }

        [DisplayName("昵称")]
        public string NickName { get; set; }

        [Required]
        [DisplayName("手机")]
        public string Phone { get; set; }
    }

}
