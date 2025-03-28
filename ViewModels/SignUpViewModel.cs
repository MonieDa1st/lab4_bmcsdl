using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using QLSVNhom.Helpers;
using QLSVNhom.Models;
using Microsoft.Data.SqlClient;

namespace QLSVNhom.ViewModels
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private string _maNV;
        private string _hoTen;
        private string _email;
        private string _tenDN;
        private string _matKhau;
        private string _luong;  // Lương nhập dưới dạng string
        private string _pubKey;
        private string _message;

        public event PropertyChangedEventHandler PropertyChanged;

        public string MaNV
        {
            get => _maNV;
            set { _maNV = value; OnPropertyChanged(nameof(MaNV)); }
        }

        public string HoTen
        {
            get => _hoTen;
            set { _hoTen = value; OnPropertyChanged(nameof(HoTen)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string TenDN
        {
            get => _tenDN;
            set { _tenDN = value; OnPropertyChanged(nameof(TenDN)); }
        }

        public string MatKhau
        {
            get => _matKhau;
            set { _matKhau = value; OnPropertyChanged(nameof(MatKhau)); }
        }

        public string Luong  // Nhập dưới dạng string
        {
            get => _luong;
            set { _luong = value; OnPropertyChanged(nameof(Luong)); }
        }

        public string PubKey
        {
            get => _pubKey;
            set { _pubKey = value; OnPropertyChanged(nameof(PubKey)); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        public ICommand SignUpCommand { get; }

        public SignUpViewModel()
        {
            SignUpCommand = new RelayCommand(ExecuteSignUp);
        }
        
        private void ExecuteSignUp(object parameter)
        {
            try
            {
                // Kiểm tra input
                if (string.IsNullOrWhiteSpace(MaNV) || string.IsNullOrWhiteSpace(HoTen) ||
                    string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(TenDN) ||
                    string.IsNullOrWhiteSpace(MatKhau) || string.IsNullOrWhiteSpace(Luong))
                {
                    Message = "Vui lòng điền đầy đủ thông tin!";
                    return;
                }



                // Chuyển đổi lương sang dạng số
                if (!decimal.TryParse(Luong, out decimal salaryAmount))
                {
                    Message = "Lương phải là số hợp lệ!";
                    return;
                }

                // Hash mật khẩu bằng SHA-1
                byte[] hashedPassword = CryptoHelper.HashSHA1(MatKhau);
                string hashedPasswordBase64 = Convert.ToBase64String(hashedPassword);

                // Tạo hoặc lấy cặp khóa
                //CryptoHelper.KeyPair kb = CryptoHelper.GetOrCreateKeyPairFromString(hashedMatKhauBase64);
                string pk = CryptoHelper.GetPublicKey(hashedPasswordBase64);
                // Mã hóa lương bằng RSA-2048
                byte[] encryptedSalary = CryptoHelper.EncryptRSA(Luong, hashedPasswordBase64);

                // Gửi dữ liệu đến database
                string query = "EXEC SP_INS_PUBLIC_ENCRYPT_NHANVIEN @MANV, @HOTEN, @EMAIL, @LUONG, @TENDN, @MK, @PUBKEY" ;

                DatabaseHelper.ExecuteNonQuery(query,
                    new SqlParameter("@MANV", MaNV),
                    new SqlParameter("@HOTEN", HoTen),
                    new SqlParameter("@EMAIL", Email),
                    new SqlParameter("@LUONG", encryptedSalary),
                    new SqlParameter("@TENDN", TenDN),
                    new SqlParameter("@MK", hashedPassword),
                    new SqlParameter("@PUBKEY", pk)
                );

                MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Message = $"Lỗi: {ex.Message}";
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
