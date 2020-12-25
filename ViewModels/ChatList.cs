using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.ViewModel
{

    public class ChatList
    {
        public int ReceiverID { get; set; }
        public string ChatContent { get; set; }
        public string AlignmentType { get; set; }
        public DateTime ChatTime { get; set; }
    }

    public enum AlignmentType
    {
        Left,
        Right
    }
}
