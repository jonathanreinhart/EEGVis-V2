﻿using EEGVis_V2.Viewmodels;
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

namespace EEGVis_V2.Views
{
    /// <summary>
    /// Interaction logic for AcquisitionView.xaml
    /// </summary>
    public partial class AcquisitionView : UserControl, IDisposable
    {

        public AcquisitionView(SerialGraphViewModel serialGraphViewModel)
        {
            InitializeComponent();
            //initialize the viewmodel and pass the current data ViewModel
            DataContext = new AcquisitionViewModel(serialGraphViewModel);
        }

        public void Dispose()
        {
            if (DataContext is IDisposable disposableViewModel)
                disposableViewModel.Dispose();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
