using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using QLSVNhom.Views;
using Microsoft.Data.SqlClient;

using QLSVNhom.Models;

namespace QLSVNhom.ViewModels
{
    public class LopViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Lop> _dsLop;
        public ObservableCollection<Lop> DsLop
        {
            get { return _dsLop; }
            set { _dsLop = value; OnPropertyChanged(nameof(DsLop)); }
        }

        public Lop SelectedLop { get; set; }
        public bool IsDeleting { get; set; } // Kiểm soát hiển thị ô nhập
        public bool IsAdding { get; set; } // Kiểm soát hiển thị ô thêm lớp
        public ICommand LoadDataCommand { get; }
        public ICommand AddLopCommand { get; }
        public ICommand DeleteLopCommand { get; }
        public ICommand ShowDeleteCommand { get; }
        public ICommand ShowAddCommand { get; }
        public ICommand ConfirmAddCommand { get; }

        public ICommand ShowViewPanelCommand { get; }
        public ICommand ConfirmViewCommand { get; }
        public LopViewModel()
        {
            LoadDataCommand = new RelayCommand(_ => LoadData());
            AddLopCommand = new RelayCommand(_ => AddLop());
            DeleteLopCommand = new RelayCommand(_ => DeleteLop());
            ShowDeleteCommand = new RelayCommand(_ => { IsDeleting = true; OnPropertyChanged(nameof(IsDeleting)); });
            ShowAddCommand = new RelayCommand(_ => { IsAdding = true; OnPropertyChanged(nameof(IsAdding)); });
            ShowViewPanelCommand = new RelayCommand(_ => { IsViewing = true; OnPropertyChanged(nameof(IsViewing)); });
            ConfirmViewCommand = new RelayCommand(_ => OpenStudentView());
            DsLop = new ObservableCollection<Lop>();

            SelectedLop = new Lop();
        }

        

        private bool _isViewing;
        public bool IsViewing
        {
            get { return _isViewing; }
            set { _isViewing = value; OnPropertyChanged(nameof(IsViewing)); }
        }

        private string _maLopNhap;
        public string MaLopNhap
        {
            get { return _maLopNhap; }
            set { _maLopNhap = value; OnPropertyChanged(nameof(MaLopNhap)); }
        }

        private void OpenStudentView()
        {
            if (string.IsNullOrWhiteSpace(MaLopNhap))
            {
                MessageBox.Show("Vui lòng nhập Mã Lớp trước khi tiếp tục!");
                return;
            }

            SinhVienView sinhVienView = new SinhVienView(LoggedInUser.Manv, MaLopNhap);
            sinhVienView.Show();

            IsViewing = false; // Ẩn panel sau khi mở danh sách sinh viên
            OnPropertyChanged(nameof(IsViewing));
        }

        private void LoadData()
        {
            string query = "EXEC SP_SEL_ALL_LOP"; // Truy vấn toàn bộ danh sách lớp
            var data = DatabaseHelper.ExecuteQuery(query);
            DsLop.Clear();
            foreach (DataRow row in data.Rows)
            {
                DsLop.Add(new Lop
                {
                    MaLop = row["MALOP"].ToString(),
                    TenLop = row["TENLOP"].ToString(),
                    MaNV = row["MANV"].ToString()
                });
            }
        }

        private string _maLopMoi;
        public string MaLopMoi
        {
            get { return _maLopMoi; }
            set { _maLopMoi = value; OnPropertyChanged(nameof(MaLopMoi)); }
        }

        private string _tenLopMoi;
        public string TenLopMoi
        {
            get { return _tenLopMoi; }
            set { _tenLopMoi = value; OnPropertyChanged(nameof(TenLopMoi)); }
        }

        private string _maNVMoi;
        public string MaNVMoi
        {
            get { return _maNVMoi; }
            set { _maNVMoi = value; OnPropertyChanged(nameof(MaNVMoi)); }
        }

        public void AddLop()
        {
            if (string.IsNullOrWhiteSpace(MaLopMoi) || string.IsNullOrWhiteSpace(TenLopMoi) || string.IsNullOrWhiteSpace(MaNVMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin lớp!");
                return;
            }

            string query = "EXEC SP_ADD_LOP @MaLop, @TenLop, @MaNV";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@MaLop", MaLopMoi),
                new SqlParameter("@TenLop", TenLopMoi),
                new SqlParameter("@MaNV", MaNVMoi));

            LoadData();

            MaLopMoi = "";
            TenLopMoi = "";
            MaNVMoi = "";
            IsAdding = false; // Ẩn ô nhập sau khi thêm thành công
            OnPropertyChanged(nameof(IsAdding));
        }




        private string _maLopToDelete;
        public string MaLopToDelete
        {
            get { return _maLopToDelete; }
            set { _maLopToDelete = value; OnPropertyChanged(nameof(MaLopToDelete)); }
        }

        public void DeleteLop()
        {
            if (string.IsNullOrWhiteSpace(MaLopToDelete))
            {
                MessageBox.Show("Vui lòng nhập mã lớp cần xóa.");
                return;
            }

            string query = "EXEC SP_DELETE_LOP @MaLop";
            DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@MaLop", MaLopToDelete));
            LoadData();
            MaLopToDelete = ""; // Xóa nội dung nhập sau khi xóa thành công
            IsDeleting = false; // Ẩn ô nhập sau khi xóa
            OnPropertyChanged(nameof(MaLopToDelete));
            OnPropertyChanged(nameof(IsDeleting));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}