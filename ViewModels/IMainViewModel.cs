using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.ViewModels
{
    public interface IMainViewModel
    {
        void OnReceiverChanged();
        void SetSelectedReceiver(int id);
    }
}
