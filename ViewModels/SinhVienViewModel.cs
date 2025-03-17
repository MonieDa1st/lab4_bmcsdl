using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using QLSVNhom.Models;

namespace QLSVNhom.ViewModels
{
    public class SinhVienViewModel : INotifyPropertyChanged
    {
        public string LoggedInMaNV { get; set; } // Mã nhân viên đăng nhập
        public string SelectedLop { get; set; } // Lớp được chọn
        public bool CanEdit { get; set; } // Xác định quyền thêm/xóa sinh viên
        public ObservableCollection<SinhVien> DsSinhVien { get; set; } // Danh sách sinh viên

        public ICommand ShowAddPanelCommand { get; }
        public ICommand ConfirmAddCommand { get; }
        public ICommand DeleteStudentCommand { get; }

        public SinhVien NewSinhVien { get; set; }
        public SinhVien SelectedSinhVien { get; set; }

        public SinhVienViewModel(string manv, string selectedLop = null)
        {
            LoggedInMaNV = manv;
            DsSinhVien = new ObservableCollection<SinhVien>();
            DeleteStudentCommand = new RelayCommand(_ => DeleteStudent(), _ => CanEdit);
            ShowAddPanelCommand = new RelayCommand(_ => { IsAdding = true; OnPropertyChanged(nameof(IsAdding)); });
            ConfirmAddCommand = new RelayCommand(_ => ConfirmAdd(), _ => CanEdit);


            if (!string.IsNullOrEmpty(selectedLop))
            {
                SelectedLop = selectedLop;
                CheckEditPermission();
                LoadStudents();
            }
        }

        private void CheckEditPermission()
        {
            string query = "EXEC SP_CHECK_TEACHER_PERMISSION @Manv, @Malop";
            var result = DatabaseHelper.ExecuteScalar(query,
                new SqlParameter("@Manv", LoggedInMaNV),
                new SqlParameter("@Malop", SelectedLop));

            CanEdit = (result != null && (int)result == 1); // Nếu có quyền, CanEdit = true
        }

        private void LoadStudents()
        {
            string query = "EXEC SP_GET_STUDENTS_BY_CLASS @Malop";
            var data = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Malop", SelectedLop));
            DsSinhVien.Clear();
            foreach (DataRow row in data.Rows)
            {
                DsSinhVien.Add(new SinhVien
                {
                    MaSV = row["MASV"].ToString(),
                    HoTen = row["HOTEN"].ToString(),
                    NgaySinh = DateTime.Parse(row["NGAYSINH"].ToString()),
                    MaLop = row["MALOP"].ToString()
                });
            }
        }

        private bool _isAdding;
        public bool IsAdding
        {
            get { return _isAdding; }
            set { _isAdding = value; OnPropertyChanged(nameof(IsAdding)); }
        }

        private void ShowAddPanel()
        {
            IsAdding = true;
            NewSinhVien = new SinhVien();
        }

        private void ConfirmAdd()
        {
            if (NewSinhVien == null || string.IsNullOrEmpty(NewSinhVien.MaSV) ||
                string.IsNullOrEmpty(NewSinhVien.HoTen) || string.IsNullOrEmpty(NewSinhVien.TenDN) || NewSinhVien.MatKhau == null)
            {
                MessageBox.Show("Vui lòng nhập thông tin sinh viên hợp lệ!");
                return;
            }

            string query = "EXEC SP_INSERT_STUDENT @MaSV, @HoTen, @NgaySinh, @DiaChi, @MaLop, @TenDN, @MatKhau";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@MaSV", NewSinhVien.MaSV),
                new SqlParameter("@HoTen", NewSinhVien.HoTen),
                new SqlParameter("@NgaySinh", NewSinhVien.NgaySinh),
                new SqlParameter("@DiaChi", NewSinhVien.DiaChi ?? (object)DBNull.Value),
                new SqlParameter("@MaLop", SelectedLop),
                new SqlParameter("@TenDN", NewSinhVien.TenDN),
                new SqlParameter("@MatKhau", NewSinhVien.MatKhau));

            MessageBox.Show("Thêm sinh viên thành công!");
            LoadStudents();
            IsAdding = false;
        }

        private void DeleteStudent()
        {
            // Xóa sinh viên khỏi lớp
            MessageBox.Show("Xóa sinh viên khỏi lớp " + SelectedLop);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}