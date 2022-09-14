using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using EEGVis_V2.models;
using EEGVis_V2.Viewmodels;
using EEGVis_V2.Views;

namespace EEGVis_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialGraphViewModel _serialGraphViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _serialGraphViewModel = new SerialGraphViewModel();
            NavigationMenuListBox.SelectionChanged += NavigationMenuListBox_SelectionChanged;
        }

        private void NavigationMenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)NavigationMenuListBox.SelectedItem == "Graph")
            {
                ViewContentControl.Content = new SerialGraphView(_serialGraphViewModel);
            }
            else
            {
                ViewContentControl.Content = new HomeView();
            }
        }
    }
}
