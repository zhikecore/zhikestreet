using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class Tag
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string Name { get; set; }
        public bool IsSoftDelete { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
