using System;
using System.Windows.Input;

namespace UMOVEWPF
{
    /// <summary>
    /// Implementering af ICommand, så man kan binde knapper til metoder i ViewModel.
    /// Gør det muligt at bruge kommandoer i MVVM.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Opretter en ny RelayCommand.
        /// </summary>
        /// <param name="execute">Metode der skal køres når kommandoen aktiveres</param>
        /// <param name="canExecute">Valgfri metode der bestemmer om kommandoen kan aktiveres</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Returnerer true hvis kommandoen må aktiveres
        /// </summary>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        /// <summary>
        /// Udfører den tilknyttede metode
        /// </summary>
        public void Execute(object parameter) => _execute(parameter);

        /// <summary>
        /// Event der trigges når CanExecute skal genvurderes
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
} 