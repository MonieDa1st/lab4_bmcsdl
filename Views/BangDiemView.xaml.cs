using QLSVNhom.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLSVNhom.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class BangDiemView : Window
    {
        private BangDiemViewModel viewModel;
        public BangDiemView(string masv, string manv)
        {
            InitializeComponent();
            viewModel = new BangDiemViewModel(masv, manv);
            this.DataContext = viewModel;
        }

        private void ShowEnterScorePanel(object sender, RoutedEventArgs e)
        {
            if (viewModel == null)
            {
                Debug.WriteLine("viewModel chưa được khởi tạo!");
                return;
            }

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
