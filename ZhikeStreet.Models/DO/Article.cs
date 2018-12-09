using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class Article
    {
        public int Id { get; set; }
        public int ArticleTypeId { get; set; }
        public string ArticleTypeName { get; set; }
        public int UserId { get; set; }
        public string TagIds { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public string Cover { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public bool IsUp { get; set; }
        public bool IsRecommend { get; set; }
        public bool IsHot { get; set; }
        public int OpenState { get; set; }//开放状态：0=公开 1=会员可看 2=付费可看
        public int ScanNum { get; set; }
        public int LikeNum { get; set; }
        public int CommentNum { get; set; }
        public int ForwardNum { get; set; }
        public bool IsSoftDelete { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
