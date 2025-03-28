using System.Windows;

namespace QLSVNhom.Views
{
    public partial class InforNVView : Window
    {
        public InforNVView(string maNV)
        {
            InitializeComponent();
            DataContext = new ViewModels.InforNVViewModel(maNV);
        }
    }
}
