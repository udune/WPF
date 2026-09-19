using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ch34_MVVM.Commands
{
    public class PersonCommand : ICommand
    {
        // CommandManager에 위임해 UI 상태가 바뀔 때마다 CanExecute가 다시 평가되도록 한다
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        Action<string?> execute;
        Predicate<string?> canExecute;
        public PersonCommand(Action<string?> msg, Predicate<string?> check)
        {
            execute = msg;
            canExecute = check;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute.Invoke(parameter as string);
        }

        public void Execute(object? parameter)
        {
            execute.Invoke(parameter as string);
        }
    }
}
