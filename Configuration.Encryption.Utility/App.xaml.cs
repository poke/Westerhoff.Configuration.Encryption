using System.Windows;

namespace Westerhoff.Configuration.Encryption.Utility
{
    /// <summary>
    /// Application.
    /// </summary>
    public partial class App : Application
    {
        private CertificateCollection _certificateCollection;

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            base.OnStartup(eventArgs);

            _certificateCollection = new CertificateCollection();

            var window = new MainWindow();
            window.DataContext = new MainViewModel(new ConfigurationProtector(), _certificateCollection);
            window.Show();
        }

        protected override void OnExit(ExitEventArgs eventArgs)
        {
            base.OnExit(eventArgs);

            _certificateCollection?.Dispose();
        }
    }
}
