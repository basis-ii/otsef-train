using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Festo.Config.FullClient
{
    public class DelegateCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;
        private INotifyPropertyChanged vm;

        public DelegateCommand(Action execute, Func<bool> canExecute, INotifyPropertyChanged vm)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.vm = vm;
            vm.PropertyChanged += (_, __) => this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object _) => this.canExecute?.Invoke() ?? true;

        public void Execute(object _) => this.execute?.Invoke();
    }
}
