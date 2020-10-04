using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private bool _includeCertThumbprint;
        private ProtectedSecret _protectedSecret;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// List of available certificates.
        /// </summary>
        public IList<CertificateInfo> Certificates
        { get; private set; }

        /// <summary>
        /// Selected certificate to protect the secret with.
        /// </summary>
        public CertificateInfo SelectedCertificate
        {
            get => _selectedCertificate;
            set
            {
                if (UpdateNotifyingProperty(ref _selectedCertificate, value))
                    UpdateProtectedSecret();
            }
        }

        /// <summary>
        /// Secret value that should be protected.
        /// </summary>
        public string SecretValue
        {
            get => _secretValue;
            set
            {
                if (UpdateNotifyingProperty(ref _secretValue, value))
                    UpdateProtectedSecret();
            }
        }

        /// <summary>
        /// Optional label for the secret that should be protected.
        /// </summary>
        public string SecretLabel
        {
            get => _secretLabel;
            set
            {
                if (UpdateNotifyingProperty(ref _secretLabel, value))
                    OnPropertyChanged(nameof(ProtectedValue));
            }
        }

        /// <summary>
        /// Whether to include the certificate thumbprint in the protected value.
        /// </summary>
        public bool IncludeCertThumbprint
        {
            get => _includeCertThumbprint;
            set
            {
                if (UpdateNotifyingProperty(ref _includeCertThumbprint, value))
                    OnPropertyChanged(nameof(ProtectedValue));
            }
        }

        /// <summary>
        /// Protected value, containing the encrypted secret.
        /// </summary>
        public string ProtectedValue
            => _protectedSecret?.GetConfigurationValue(SecretLabel, IncludeCertThumbprint);

        /// <summary>
        /// Command to copy the protected value to the clipboard.
        /// </summary>
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

        /// <summary>
        /// Update the protected secret.
        /// </summary>
        private void UpdateProtectedSecret()
        {
            if (string.IsNullOrWhiteSpace(SecretValue) || SelectedCertificate is null)
                _protectedSecret = null;
            else
                _protectedSecret = _configurationProtector.Protect(SecretValue, SelectedCertificate);
            OnPropertyChanged(nameof(ProtectedValue));
        }

        /// <summary>
        /// Copy the protected value to the clipboard.
        /// </summary>
        private void CopyProtectedValue()
            => Clipboard.SetText(ProtectedValue ?? string.Empty);

        /// <summary>
        /// Update the field for a property and raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="field">Backing field reference.</param>
        /// <param name="value">Updated value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Whether the value has changed.</returns>
        private bool UpdateNotifyingProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Propert name that has changed.</param>
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
