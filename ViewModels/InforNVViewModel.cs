using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using QLSVNhom.Helpers;
using QLSVNhom.Views;
using System.Security.Cryptography;


namespace QLSVNhom.ViewModels
{
    internal class InforNVViewModel : INotifyPropertyChanged
    {
        private string _maNV;
        private string _hoTen;
        private string _email;
        private string _luong;

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

        public string Luong
        {
            get => _luong;
            set { _luong = value; OnPropertyChanged(nameof(Luong)); }
        }

        

        public ICommand OpenLopViewCommand { get; }
        public ICommand LoadDataCommand { get; }
        public ICommand UpdateNhanVienCommand { get; }


        public InforNVViewModel(string maNV)
        {
            OpenLopViewCommand = new RelayCommand(_ => OpenLopView());
            LoadDataCommand = new RelayCommand(_ => LoadNhanVien(maNV));
            UpdateNhanVienCommand = new RelayCommand(_ => UpdateNhanVien());


            if (!string.IsNullOrEmpty(maNV))
            {
                LoadNhanVien(maNV);
            }
        }

        private void LoadNhanVien(string maNV)
        {
            string query = $"EXEC SP_GET_NHANVIEN_INFO '{maNV}'";
            DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                MaNV = row["MANV"].ToString();
                HoTen = row["HOTEN"].ToString();
                Email = row["EMAIL"].ToString();

                byte[] encryptedLuongBytes = (byte[])row["LUONG"];

                // Giải mã lương bằng RSA với hashedPassword
                Luong = CryptoHelper.DecryptRSA(encryptedLuongBytes, LoggedInUser.hashedPasswordBase64);
            }
        }

        private void OpenLopView()
        {
            LopView lopView = new LopView();
            lopView.Show();
        }

        private void UpdateNhanVien()
        {
            if (string.IsNullOrWhiteSpace(MaNV))
                return;

            try
            {
                byte[] encryptedLuong = null;
                if (!string.IsNullOrWhiteSpace(Luong))
                {
                    // Kiểm tra lương có phải số hợp lệ không
                    if (!decimal.TryParse(Luong, out _))
                    {
                        MessageBox.Show("Lương phải là số hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (LoggedInUser.PubKey == null)
                    {
                        MessageBox.Show("Không có khóa công khai để mã hóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    encryptedLuong = CryptoHelper.EncryptRSA(Luong, LoggedInUser.hashedPasswordBase64);
                }

                string query = "EXEC SP_UPDATE_NHANVIEN @MANV, @HOTEN, @EMAIL, @LUONG";
                var parameters = new SqlParameter[]
                {
            new SqlParameter("@MANV", MaNV),
            new SqlParameter("@HOTEN", string.IsNullOrWhiteSpace(HoTen) ? (object)DBNull.Value : HoTen),
            new SqlParameter("@EMAIL", string.IsNullOrWhiteSpace(Email) ? (object)DBNull.Value : Email),
            new SqlParameter("@LUONG", encryptedLuong ?? (object)DBNull.Value)
                };

                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show($"Lỗi mã hóa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
