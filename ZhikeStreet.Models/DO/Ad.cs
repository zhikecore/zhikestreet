using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class Ad
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string LinkUrl { get; set; }
        public string AdCode { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public String Contactor { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TelePhone { get; set; }
        public int ClickNum { get; set; }
        public bool Enabled { get; set; }
        public DateTime Description { get; set; }
        public DateTime CreatTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
