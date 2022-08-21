using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ScottPlot_Test
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        public GraphView()
        {
            InitializeComponent();
            double[] dataY = new double[5000];
            Plot.Plot.AddSignal(dataY);
            Plot.Plot.Frameless();
            Task.Factory.StartNew(() =>
            {
                int x = 0;
                while (!this.Dispatcher.HasShutdownStarted)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        for (int i = 0; i < dataY.Length - 50; i++)
                        {
                            dataY[i] = dataY[i + 50];
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            dataY[dataY.Length - 50 + i] = (x % 1000) * 0.001;
                            x++;
                        }
                        Plot.Render();
                        if (x <= 5000)
                            Plot.Plot.AxisAuto();
                    });
                    Thread.Sleep(50);
                }
            });
        }
    }
}
