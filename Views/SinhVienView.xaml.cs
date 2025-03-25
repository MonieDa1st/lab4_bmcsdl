using QLSVNhom.Models;
using QLSVNhom.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;


namespace QLSVNhom.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SinhVienView : Window
    {
        private SinhVienViewModel viewModel;
        public SinhVienView(string manv, string selectedLop)
        {
            InitializeComponent();
            viewModel = new SinhVienViewModel(manv, selectedLop);
            this.DataContext = viewModel;
        }

        private void ShowAddPanel(object sender, RoutedEventArgs e)
        {
            AddPanel.Visibility = Visibility.Visible;
            viewModel.NewSinhVien = new QLSVNhom.Models.SinhVien(); // Reset giá trị trước khi nhập
            TxtPassword.Password = string.Empty; // Đảm bảo mật khẩu không giữ giá trị cũ
        }


        private void ConfirmAdd(object sender, RoutedEventArgs e)
        {
            var viewModel = (SinhVienViewModel)DataContext;

            viewModel.NewSinhVien.MatKhau = TxtPassword.Password;


            viewModel.ConfirmAddCommand.Execute(null);

            // Ẩn panel sau khi thêm sinh viên
            AddPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowDeletePanel(object sender, RoutedEventArgs e)
        {
            DeletePanel.Visibility = Visibility.Visible;
            viewModel.MaSVXoa = "";
        }


        private void ConfirmDelete(object sender, RoutedEventArgs e)
        {
            var viewModel = (SinhVienViewModel)DataContext;

            viewModel.DeleteStudentCommand.Execute(viewModel.MaSVXoa);
            DeletePanel.Visibility = Visibility.Collapsed;
        }

        private void OpenBangDiem(object sender, RoutedEventArgs e)
        {
            var viewModel = (SinhVienViewModel)DataContext;

            if (viewModel.SelectedSinhVien == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên từ danh sách.");
                return;
            }

            BangDiemView bangDiemView = new BangDiemView(LoggedInUser.Manv, viewModel.SelectedSinhVien.MaSV);
            bangDiemView.Show();
        }

        private void ShowEnterScorePanel(object sender, RoutedEventArgs e)
        {
            ScorePanel.Visibility = Visibility.Visible;
            viewModel.NewBangDiem = new QLSVNhom.Models.BangDiem(); // Reset dữ liệu nhập
        }

        private void ConfirmSaveDiem(object sender, RoutedEventArgs e)
        {
            viewModel.SaveDiemCommand.Execute(null);
            ScorePanel.Visibility = Visibility.Collapsed;
        }


    }
}
    