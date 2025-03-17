using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using QLSVNhom.Views;


namespace QLSVNhom.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _message;
        private AuthenticateUser _authenticator;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _authenticator = new AuthenticateUser();
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            try
            {
                var (success, userId, pubKey) = _authenticator.Login(Username, Password);

                if (success)
                {
                    LoggedInUser.Manv = userId;
                    LoggedInUser.PubKey = pubKey;
                    Message = $"Đăng nhập thành công!\nUser ID: {userId}\nPublic Key: {pubKey}";
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        //Application.Current.Windows.OfType<LoginView>().FirstOrDefault()?.Close();

                        LopView lopView = new LopView();
                        lopView.Show();
                       
                    });
                }
                else
                {
                    Message = "Sai tài khoản hoặc mật khẩu!";
                }
            }
            catch (Exception ex)
            {
                Message = $"Đã xảy ra lỗi: {ex.Message}";
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}