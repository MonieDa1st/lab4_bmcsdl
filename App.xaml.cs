using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using QLSVNhom.Views;
using QLSVNhom.ViewModels;


namespace QLSVNhom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LoginView loginWindow = new LoginView();
            loginWindow.Show();
        }
    }
}
