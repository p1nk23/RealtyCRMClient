using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace RealtyCRMClient
{
    public class RelayCommand : ICommand
    {

        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public async void Execute(object parameter)
        {
            await _execute();
        }

        public void Execute()
        {
            try
            {
                _execute();
            }
            catch (Exception ex)
            {
                // Логируем ошибки выполнения команды
                Serilog.Log.Information("Ошибка выполнения RelayCommand: {Message}", ex.Message);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool canExecute = !_isExecuting && (_canExecute?.Invoke() ?? true);
            Log.Information("AsyncRelayCommand CanExecute: {CanExecute}, IsExecuting: {IsExecuting}", canExecute, _isExecuting);
            return canExecute;
        }

        public async void Execute(object parameter)
        {
            if (_isExecuting)
            {
                Log.Information("AsyncRelayCommand уже выполняется, игнорируем повторный вызов");
                return;
            }

            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                Log.Information("AsyncRelayCommand выполняется");
                await _execute();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка выполнения AsyncRelayCommand");
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}

