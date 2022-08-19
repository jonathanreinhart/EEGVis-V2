using EEGVis_V2.Viewmodels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EEGVis_V2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel mainViewModel;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow();
            mainViewModel = new MainViewModel();
            mainWindow.DataContext = mainViewModel;
            Exit += App_Exit;
            mainWindow.Show();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            mainViewModel.CurrentViewModel.closing = true;
        }
    }
}
