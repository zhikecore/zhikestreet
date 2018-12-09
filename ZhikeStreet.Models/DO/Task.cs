using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class Task
    {
        public int Id { get; set; }
        public string TagIds { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public int MessageTypeId { get; set; }
        public string Content { get; set; }
        public bool IsSoftDelete { get; set; }
        public bool IsDelay { get; set; }
        public string Description { get; set; }
        public DateTime CreatTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
