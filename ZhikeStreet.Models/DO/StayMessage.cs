using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class StayMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string IP { get; set; }
        public bool IsSoftDeleted { get; set; }
        public DateTime CreatTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
