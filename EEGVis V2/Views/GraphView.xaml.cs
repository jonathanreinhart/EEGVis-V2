using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
using System.Threading;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using InteractiveDataDisplay.WPF;
using System.ComponentModel;
using EEGVis_V2.Viewmodels;
using System.Windows.Threading;

namespace EEGVis_V2.Views
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        /*
        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set 
            { 
                SetValue(PointsProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(GraphView), new PropertyMetadata(PointsChanged));

        private static void PointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is GraphView)
            {
                GraphView graphView = d as GraphView;
                graphView.linegraph.Points = (PointCollection)e.NewValue;
            }
        }
        */

        public double[] DataY;

        public double[] GraphData
        {
            get { return (double[])GetValue(GraphDataProperty); }
            set { SetValue(GraphDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GraphData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphDataProperty =
            DependencyProperty.Register("GraphData", typeof(double[]), typeof(GraphView), new PropertyMetadata(PointsChanged));

        private static void PointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphView)
            {
                GraphView graphView = d as GraphView;
                graphView.Dispatcher.Invoke(() =>
                {
                    double[] newData = (double[])e.NewValue;
                    for(int i = 0; i < 5000; i++)
                    {
                        graphView.DataY[i] = newData[i];
                    }
                    graphView.DataPlot.Render();
                    graphView.DataPlot.Plot.AxisAuto();
                });
            }
        }

        public GraphView()
        {
            InitializeComponent();
            DataY = new double[5000];
            DataPlot.Plot.AddSignal(DataY);
            DataPlot.Plot.Frameless();
        }
    }
}