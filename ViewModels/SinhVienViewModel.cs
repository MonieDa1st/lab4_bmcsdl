using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using QLSVNhom.Models;
using QLSVNhom.Views;

namespace QLSVNhom.ViewModels
{
    public class SinhVienViewModel : INotifyPropertyChanged
    {
        public string LoggedInMaNV { get; set; } // Mã nhân viên đăng nhập
        public string SelectedLop { get; set; } // Lớp được chọn
        public bool CanEdit { get; set; } // Xác định quyền thêm/xóa sinh viên
        public ObservableCollection<SinhVien> DsSinhVien { get; set; } // Danh sách sinh viên

        public ICommand ShowAddPanel { get; }
        public ICommand ConfirmAddCommand { get; }

        public ICommand ShowDeletePanel { get; }
        public ICommand DeleteStudentCommand { get; }

        public ICommand ShowEnterScorePanelCommand { get; }
        public ICommand SaveDiemCommand { get; }


        //public SinhVien NewSinhVien { get; set; }
        public SinhVien SelectedSinhVien { get; set; }

        public SinhVienViewModel(string manv, string selectedLop = null)
        {
            LoggedInMaNV = manv;
            DsSinhVien = new ObservableCollection<SinhVien>();
            ShowDeletePanel = new RelayCommand(_ => { IsDeleting = true; OnPropertyChanged(nameof(IsDeleting)); }, _ => CanEdit);
            DeleteStudentCommand = new RelayCommand(_ => DeleteStudent());
            ShowAddPanel = new RelayCommand(_ => { IsAdding = true; OnPropertyChanged(nameof(IsAdding)); }, _ => CanEdit);
            ConfirmAddCommand = new RelayCommand(_ => ConfirmAdd());
            ShowEnterScorePanelCommand = new RelayCommand(_ => ShowEnterScorePanel(), _ => CanEdit);
            SaveDiemCommand = new RelayCommand(_ => SaveDiem());
            NewBangDiem = new BangDiem();




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

        private SinhVien _newSinhVien;
        public SinhVien NewSinhVien
        {
            get => _newSinhVien;
            set
            {
                _newSinhVien = value;
                OnPropertyChanged(nameof(NewSinhVien)); // Cập nhật UI
            }
        }

        private void ConfirmAdd()
        {
            if (string.IsNullOrWhiteSpace(NewSinhVien.MaSV))
            {
                MessageBox.Show("Vui lòng nhập Mã Sinh Viên!");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewSinhVien.HoTen))
            {
                MessageBox.Show("Vui lòng nhập Họ Tên!");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewSinhVien.TenDN))
            {
                MessageBox.Show("Vui lòng nhập Tên Đăng Nhập!");
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
            OnPropertyChanged(nameof(IsAdding));

        }

        private string _maSVXoa;
        public string MaSVXoa
        {
            get { return _maSVXoa; }
            set { _maSVXoa = value; OnPropertyChanged(nameof(MaSVXoa)); }
        }

        private bool _isDeleting;
        public bool IsDeleting
        {
            get { return _isDeleting; }
            set { _isDeleting = value; OnPropertyChanged(nameof(IsDeleting)); }
        }

        private void DeleteStudent()
        {
            if (string.IsNullOrWhiteSpace(MaSVXoa))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên cần xóa.");
                return;
            }

            string query = "EXEC SP_DELETE_STUDENT @MaSV";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@MaSV", MaSVXoa));

            MessageBox.Show("Xóa sinh viên thành công!");
            MaSVXoa = string.Empty; // Clear the input after deletion
            LoadStudents();
            IsDeleting = false;
            OnPropertyChanged(nameof(IsDeleting));
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
            NewBangDiem = new BangDiem(); // Reset dữ liệu nhập
        }

        private void SaveDiem()
        {
            if (string.IsNullOrWhiteSpace(NewBangDiem.MaSV) || string.IsNullOrWhiteSpace(NewBangDiem.MaHP) || NewBangDiem.DiemThi < 0 || NewBangDiem.DiemThi > 10)
            {
                MessageBox.Show("Vui lòng nhập đúng thông tin!");
                return;
            }

            string query = "EXEC SP_INS_PUBLIC_BANGDIEM @MaSV, @MaHP, @DiemThi, @MaNV";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@MaSV", NewBangDiem.MaSV),
                new SqlParameter("@MaHP", NewBangDiem.MaHP),
                new SqlParameter("@DiemThi", NewBangDiem.DiemThi),
                new SqlParameter("@MaNV", LoggedInUser.Manv)
                );

            MessageBox.Show("Lưu điểm thành công!");
            IsEnteringScore = false;  // Ẩn bảng nhập sau khi lưu
            OnPropertyChanged(nameof(IsEnteringScore));

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}