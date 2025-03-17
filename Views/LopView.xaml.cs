using System.Windows;
using System.Windows.Controls;
using QLSVNhom.Models;
using QLSVNhom.ViewModels;

namespace QLSVNhom.Views
{
    public partial class LopView : Window
    {
        private LopViewModel viewModel;

        public LopView()
        {
            InitializeComponent();
            viewModel = new LopViewModel();
            this.DataContext = viewModel;
        }

        private void ShowDeletePanel(object sender, RoutedEventArgs e)
        {
            DeletePanel.Visibility = Visibility.Visible;
            var viewModel = (LopViewModel)DataContext;
            viewModel.MaLopToDelete = ""; // Reset giá trị trước khi xóa
        }

        private void ConfirmDelete(object sender, RoutedEventArgs e)
        {
            var viewModel = (LopViewModel)DataContext;
            viewModel.DeleteLop(); // Gọi hàm xóa từ ViewModel

            DeletePanel.Visibility = Visibility.Collapsed; // Ẩn panel sau khi xóa
        }

        private void ShowAddPanel(object sender, RoutedEventArgs e)
        {
            AddPanel.Visibility = Visibility.Visible;
            var viewModel = (LopViewModel)DataContext;
            viewModel.MaLopMoi = "";
            viewModel.TenLopMoi = "";
            viewModel.MaNVMoi = "";
        }

        private void ConfirmAdd(object sender, RoutedEventArgs e)
        {
            var viewModel = (LopViewModel)DataContext;
            viewModel.AddLop(); // Gọi hàm thêm lớp từ ViewModel

            AddPanel.Visibility = Visibility.Collapsed; // Ẩn panel sau khi thêm
        }
        
        private void ShowViewPanel(object sender, RoutedEventArgs e)
        {
            ViewPanel.Visibility = Visibility.Visible;
            var viewModel = (LopViewModel)DataContext;
            viewModel.MaLopNhap = ""; // Reset giá trị trước khi nhập
        }

        private void ConfirmView(object sender, RoutedEventArgs e)
        {
            var viewModel = (LopViewModel)DataContext;
            if (string.IsNullOrWhiteSpace(viewModel.MaLopNhap))
            {
                MessageBox.Show("Vui lòng nhập Mã Lớp trước khi tiếp tục!");
                return;
            }

            SinhVienView sinhVienView = new SinhVienView(LoggedInUser.Manv, viewModel.MaLopNhap);
            sinhVienView.Show();

            ViewPanel.Visibility = Visibility.Collapsed; // Ẩn panel sau khi mở danh sách sinh viên
        }

    }
}
