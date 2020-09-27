using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Westerhoff.Configuration.Encryption.Utility
{
    /// <summary>
    /// Main view model.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ConfigurationProtector _configurationProtector;

        private CertificateInfo _selectedCertificate;
        private string _secretLabel;
        private string _secretValue;
        private ProtectedSecret _protectedSecret;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        public IList<CertificateInfo> Certificates
        { get; private set; }

        public CertificateInfo SelectedCertificate
        {
            get => _selectedCertificate;
            set
            {
                UpdateNotifyingProperty(ref _selectedCertificate, value, nameof(SelectedCertificate));
                UpdateEncryptedValue();
            }
        }

        public string SecretValue
        {
            get => _secretValue;
            set
            {
                UpdateNotifyingProperty(ref _secretValue, value, nameof(SecretValue));
                UpdateEncryptedValue();
            }
        }

        public string SecretLabel
        {
            get => _secretLabel;
            set
            {
                UpdateNotifyingProperty(ref _secretLabel, value, nameof(SecretLabel));
                UpdateSecretLabel();
            }
        }

        public string ProtectedValue
            => _protectedSecret?.SecretText;

        public ICommand CopyProtectedValueCommand { get; }

        public MainViewModel()
        {
            CopyProtectedValueCommand = new RelayCommand(CopyProtectedValue);
        }

        public MainViewModel(ConfigurationProtector configurationProtector, CertificateCollection certificateCollection)
            : this()
        {
            _configurationProtector = configurationProtector;
            Certificates = certificateCollection.GetCertificates();
            SelectedCertificate = Certificates.FirstOrDefault();
        }

        private void UpdateEncryptedValue()
        {
            if (string.IsNullOrWhiteSpace(SecretValue))
                _protectedSecret = null;
            else
            {
                _protectedSecret = _configurationProtector.Protect(SecretValue, SelectedCertificate);
                _protectedSecret.SetLabel(SecretLabel);
            }
            OnPropertyChanged(nameof(ProtectedValue));
        }

        private void UpdateSecretLabel()
        {
            _protectedSecret?.SetLabel(SecretLabel);
            OnPropertyChanged(nameof(ProtectedValue));
        }

        private void CopyProtectedValue()
            => Clipboard.SetText(ProtectedValue ?? string.Empty);

        private void UpdateNotifyingProperty<T>(ref T field, T value, string propertyName)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
