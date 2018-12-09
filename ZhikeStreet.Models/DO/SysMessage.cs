using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class SysMessage
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public int MessageTypeId { get; set; }
        public string Content { get; set; }
        public bool IsSoftDelete { get; set; }
        public string Description { get; set; }
        public DateTime CreatTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
