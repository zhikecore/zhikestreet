using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class MyScanArticle
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public bool IsLike { get; set; }
        public bool IsForward { get; set; }
        public bool IsComment { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModdifyTime { get; set; }
    }
}
