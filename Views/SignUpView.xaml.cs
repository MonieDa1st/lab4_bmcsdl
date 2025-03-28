using System.Windows;
using System.Windows.Controls;
using QLSVNhom.ViewModels;

namespace QLSVNhom.Views
{
    public partial class SignUpView : Window
    {
        private readonly SignUpViewModel _viewModel;

        public SignUpView()
        {
            InitializeComponent();
            _viewModel = new SignUpViewModel();
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.MatKhau = passwordBox.Password;
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SignUpCommand.CanExecute(null))
            {
                _viewModel.SignUpCommand.Execute(null);
            }
        }
    }
}
