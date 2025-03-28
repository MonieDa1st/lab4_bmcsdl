using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using QLSVNhom.Models;
using QLSVNhom.Helpers;
using System.Diagnostics;

namespace QLSVNhom.ViewModels
{
    public class BangDiemViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<BangDiem> _bangDiemList = new();
        public ICommand LoadBangDiemCommand { get; }

        public ICommand ShowEnterScorePanelCommand { get; }
        public ICommand SaveDiemCommand { get; }

        public BangDiemViewModel()
        {
            LoadBangDiemCommand = new RelayCommand(_ => LoadBangDiem());
        }

        public ObservableCollection<BangDiem> BangDiemList
        {
            get => _bangDiemList;
            set
            {
                _bangDiemList = value;
                OnPropertyChanged(nameof(BangDiemList));
            }
        }

        public string MASV { get; private set; }
        private string _manv;

        public BangDiemViewModel(string masv, string manv)
        {
            MASV = masv;
            _manv = manv;
            LoadBangDiemCommand = new RelayCommand(_ => LoadBangDiem());
            ShowEnterScorePanelCommand = new RelayCommand(_ => ShowEnterScorePanel());
            SaveDiemCommand = new RelayCommand(_ => SaveDiem());
            NewBangDiem = new BangDiem{ MaSV = MASV };
            LoadBangDiem();
        }

        private void LoadBangDiem()
        {
            string query = "EXEC SP_SEL_BANGDIEM_ENCRYPTED @MASV";
            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new SqlParameter("@MASV", MASV));

            BangDiemList.Clear();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    byte[] encryptedDiemThi = row["DIEMTHI"] as byte[];

                    if (encryptedDiemThi == null || encryptedDiemThi.Length == 0)
                    {
                        Debug.WriteLine("DIEMTHI không có dữ liệu.");
                        continue;  // Bỏ qua nếu không có dữ liệu
                    }

                    Debug.WriteLine($"Dữ liệu mã hóa: {BitConverter.ToString(encryptedDiemThi)}");

                    string decryptedDiem = CryptoHelper.DecryptRSA(encryptedDiemThi, LoggedInUser.hashedPasswordBase64);

                    Debug.WriteLine($"Dữ lieu giai ma: {decryptedDiem}");

                    BangDiemList.Add(new BangDiem
                    {
                        MaHP = row["MAHP"].ToString(),
                        DiemThi = double.TryParse(decryptedDiem, out double diem) ? diem : 0.0
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi giải mã điểm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            OnPropertyChanged(nameof(BangDiemList)); // Cập nhật UI khi dữ liệu thay đổi
        }

        private bool _isEnteringScore;
        public bool IsEnteringScore
        {
            get { return _isEnteringScore; }
            set { _isEnteringScore = value; OnPropertyChanged(nameof(IsEnteringScore)); }
        }

        private BangDiem _newBangDiem;
        public BangDiem NewBangDiem
        {
            get { return _newBangDiem; }
            set { _newBangDiem = value; OnPropertyChanged(nameof(NewBangDiem)); }
        }

        private void ShowEnterScorePanel()
        {
            IsEnteringScore = true;
            NewBangDiem = new BangDiem { MaSV = MASV };
        }

        private void SaveDiem()
        {
            if (string.IsNullOrWhiteSpace(NewBangDiem.MaSV) || string.IsNullOrWhiteSpace(NewBangDiem.MaHP) || NewBangDiem.DiemThi < 0 || NewBangDiem.DiemThi > 10)
            {
                MessageBox.Show("Vui lòng nhập đúng thông tin!");
                return;
            }

            byte[] encryptedDiemThi = CryptoHelper.EncryptRSA(NewBangDiem.DiemThi.ToString(), LoggedInUser.hashedPasswordBase64);

            // Gọi stored procedure để lưu điểm đã mã hóa
            string query = "EXEC SP_INS_BANGDIEM_ENCRYPTED @MaSV, @MaHP, @DiemThi";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@MaSV", MASV),
                new SqlParameter("@MaHP", NewBangDiem.MaHP),
                new SqlParameter("@DiemThi", encryptedDiemThi)
            );

            MessageBox.Show("Lưu điểm thành công!");
            LoadBangDiem();
            IsEnteringScore = false;  // Ẩn bảng nhập sau khi lưu
            OnPropertyChanged(nameof(IsEnteringScore));

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

