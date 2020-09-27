using System;
using System.Windows.Input;

namespace Westerhoff.Configuration.Encryption.Utility
{
    /// <summary>
    /// Command that relays its execution to an <see cref="Action"/>.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _executeAction;

#pragma warning disable 67 // Event is not used
        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
#pragma warning restore 67

        /// <summary>
        /// Create a new command.
        /// </summary>
        /// <param name="executeAction">Execute action.</param>
        public RelayCommand(Action executeAction)
        {
            _executeAction = executeAction;
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
            => true;

        /// <inheritdoc/>
        public void Execute(object parameter)
            => _executeAction();
    }
}
