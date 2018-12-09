using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhikeStreet.Models.DO
{
    public class SysMessageReceiver
    {
        public int Id { get; set; }
        public int ReceiverId { get; set; }
        public string Receiver { get; set; }
        public int MessageId { get; set; }
        public int State { get; set; }
        public DateTime CreatTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
