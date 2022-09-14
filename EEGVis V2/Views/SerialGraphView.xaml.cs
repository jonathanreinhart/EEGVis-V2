using EEGVis_V2.Viewmodels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EEGVis_V2.Views
{
    /// <summary>
    /// Interaction logic for SerialDataGraph.xaml
    /// </summary>
    public partial class SerialGraphView : UserControl
    {
        public SerialGraphView(SerialGraphViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}
