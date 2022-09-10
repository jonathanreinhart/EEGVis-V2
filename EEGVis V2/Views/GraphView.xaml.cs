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
using ScottPlot;

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

        public double[] DataX;
        public double[] DataY;
        public bool FirstCall = true;

        #region propdp
        public double[] GraphData
        {
            get { return (double[])GetValue(GraphDataProperty); }
            set { SetValue(GraphDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GraphData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphDataProperty =
            DependencyProperty.Register("GraphData", typeof(double[]), typeof(GraphView), new PropertyMetadata(PointsChanged));

        public int Channel
        {
            get { return (int)GetValue(ChannelProperty); }
            set { SetValue(ChannelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Channel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChannelProperty =
            DependencyProperty.Register("Channel", typeof(int), typeof(GraphView), new PropertyMetadata(null));
        #endregion

        #region propdp functions
        private static void PointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphView)
            {
                GraphView graphView = d as GraphView;
                graphView.Dispatcher.Invoke(() =>
                {
                    double[] newData = (double[])e.NewValue;
                    if (graphView.FirstCall)
                    {
                        graphView.DataX = new double[newData.Length];
                        graphView.DataY = new double[newData.Length];
                        for(int i = 0; i < newData.Length; i++)
                        {
                            graphView.DataX[i] = i;
                        }
                        graphView.DataPlot.Plot.AddSignal(graphView.DataY);
                        graphView.FirstCall = false;
                    }
                    for(int i = 0; i < newData.Length; i++)
                    {
                        graphView.DataY[i] = newData[i];
                    }
                    //graphView.DataPlot.Plot.AxisAuto();
                    graphView.DataPlot.Render();
                });
            }
        }
        #endregion

        public GraphView()
        {
            InitializeComponent();
            DataPlot.Plot.Title("channel " + Channel.ToString());
            DataPlot.Plot.Style(ScottPlot.Style.Blue1);
            // hide just the horizontal axis ticks
            DataPlot.Plot.XAxis.Ticks(false);
            // hide the lines on the bottom, right, and top of the plot
            DataPlot.Plot.XAxis.Line(false);
            DataPlot.Plot.YAxis2.Line(false);
            DataPlot.Plot.XAxis2.Line(false);
        }
    }
}