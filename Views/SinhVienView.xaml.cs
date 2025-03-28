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
    }
}
    