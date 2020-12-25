using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.ViewModel
{
    public class ReceiversChat
    {
        public string UsersName { get; set; }
        public string UserIconName { get; set; }
        public int ReceiverId { get; set; }
        public string UserPreviousMessage { get; set; }
        public DateTime UserMessageTime { get; set; }
    }

}
