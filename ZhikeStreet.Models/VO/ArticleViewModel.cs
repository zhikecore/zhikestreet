using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhikeStreet.Models.DO;

namespace ZhikeStreet.Models.VO
{
    public class ArticleViewModel:Article
    {
        public string Author { get; set; }
        public Article Previous { get; set; }
        public Article Next { get; set; }
        public List<Article> Relatives { get; set; }
    }
}
