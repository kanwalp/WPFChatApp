using ChatApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApplication.Command
{
    public class RelayCommand : ICommand {

        Action<object> _execteMethod;
        Func<object, bool> _canexecuteMethod;


        public RelayCommand(Action<object> execteMethod, Func<object, bool> canexecuteMethod)
        {
            _execteMethod = execteMethod;
            _canexecuteMethod = canexecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            if (_canexecuteMethod != null)
            {
                return _canexecuteMethod(parameter);
            }
            else
            {
                return false;
            }
        }

        public event EventHandler CanExecuteChanged {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execteMethod(parameter);
        }
    }
}  

