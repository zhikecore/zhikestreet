using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class ArticleAccess
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public int Action { get; set; }
        public string IP { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
