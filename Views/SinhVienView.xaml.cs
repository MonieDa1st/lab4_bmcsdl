using QLSVNhom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            DataContext = viewModel;
        }

        private void ShowAddPanel(object sender, RoutedEventArgs e)
        {
            AddPanel.Visibility = Visibility.Visible;
            var viewModel = (SinhVienViewModel)DataContext;
            viewModel.NewSinhVien = new QLSVNhom.Models.SinhVien(); // Reset giá trị trước khi nhập
        }

        private void ConfirmAdd(object sender, RoutedEventArgs e)
        {
            var viewModel = (SinhVienViewModel)DataContext;
            if (string.IsNullOrWhiteSpace(viewModel.NewSinhVien.MaSV) ||
                string.IsNullOrWhiteSpace(viewModel.NewSinhVien.HoTen) ||
                string.IsNullOrWhiteSpace(viewModel.NewSinhVien.TenDN))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên trước khi tiếp tục!");
                return;
            }

            viewModel.ConfirmAddCommand.Execute(null);
            AddPanel.Visibility = Visibility.Collapsed; // Ẩn panel sau khi thêm sinh viên
        }
    }

}