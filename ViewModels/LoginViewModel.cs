using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using QLSVNhom.Views;
using QLSVNhom.Helpers;


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
                byte[] hashedPassword = CryptoHelper.HashSHA1(Password);
                string hashedPasswordBase64 = Convert.ToBase64String(hashedPassword);


                var (success, userId, pubKey) = _authenticator.Login(Username, hashedPassword);

                if (success)
                {
                    LoggedInUser.Manv = userId;
                    LoggedInUser.PubKey = CryptoHelper.GetPublicKey(hashedPasswordBase64);
                    LoggedInUser.hashedPasswordBase64 = hashedPasswordBase64;
                    Message = $"Đăng nhập thành công!";
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        //Application.Current.Windows.OfType<LoginView>().FirstOrDefault()?.Close();

                        //LopView lopView = new LopView();
                        //lopView.Show();
                        InforNVView inforNVView = new InforNVView(LoggedInUser.Manv);
                        inforNVView.Show();

                       
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