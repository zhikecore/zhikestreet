using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhikeStreet.DAL;
using ZhikeStreet.Models.DO;

namespace ZhikeStreet.BLL
{
    public class TagService
    {
        private static TagService _Instance = null;

        public static TagService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new TagService();
                return _Instance;
            }
        }

        private TagService()
        {

        }

        public Tag GetById(int id)
        {
            return TagDao.Instance.GetById(id);
        }

        public object GetTagsForCombox()
        {
            List<object> objs = new List<object>();
            List<Tag> tags = TagDao.Instance.GetAll();
            foreach (Tag item in tags)
            {
                objs.Add(new { Id = item.Id, Name = item.Name });
            }

            return objs;
        }
    }
}
