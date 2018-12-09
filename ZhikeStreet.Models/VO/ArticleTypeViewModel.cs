using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhikeStreet.Models.DO;

namespace ZhikeStreet.Models.VO
{
    public class ArticleTypeViewModel: ArticleType
    {
        public int ArticleCount { get; set; }
    }
}
