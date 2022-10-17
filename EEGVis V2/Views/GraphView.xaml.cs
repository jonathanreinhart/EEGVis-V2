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
using System.Threading.Channels;

namespace EEGVis_V2.Views
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        public double[][] DataYs;//y-values for each channel so: 0-dim: channels, 1-dim: data
        public bool FirstCall = true;
        public int NumCall = 0;
        private WpfPlot[] DataPlots;

        #region propdp
        public double[] GraphData
        {
            get { return (double[])GetValue(GraphDataProperty); }
            set { SetValue(GraphDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GraphData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphDataProperty =
            DependencyProperty.Register("GraphData", typeof(double[]), typeof(GraphView), new PropertyMetadata(PointsChanged));
        
        public int NumChannels
        {
            get { return (int)GetValue(NumChannelsProperty); }
            set { SetValue(NumChannelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumChannels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumChannelsProperty =
            DependencyProperty.Register("NumChannels", typeof(int), typeof(GraphView), new PropertyMetadata(1));
        #endregion

        #region propdp functions
        private static void PointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphView)
            {
                GraphView graphView = d as GraphView;
                graphView.Dispatcher.Invoke(() =>
                {
                    double[] newData = graphView.GraphData;
                    //Trace.WriteLine(newData.Length);
                    if(newData.Length>0)
                    {
                        //Trace.WriteLine(newData.Length);
                        graphView.NumCall++;
                        //init every data-array
                        if (graphView.FirstCall)
                        {
                            //Trace.WriteLine("starting init");
                            graphView.DataPlots = new WpfPlot[graphView.NumChannels];
                            graphView.DataYs = new double[graphView.NumChannels][];
                            // add one graph for each channel
                            for (int i = 0; i < graphView.NumChannels; i++)
                            {
                                RowDefinition curRowDef = new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) };
                                graphView.grid.RowDefinitions.Add(curRowDef);
                                graphView.DataPlots[i] = new WpfPlot();
                                graphView.DataPlots[i].Plot.Title("channel " + i.ToString());
                                graphView.DataPlots[i].Plot.Style(ScottPlot.Style.Blue1);
                                // hide just the horizontal axis ticks
                                graphView.DataPlots[i].Plot.XAxis.Ticks(false);
                                // hide the lines on the bottom, right, and top of the plot
                                graphView.DataPlots[i].Plot.XAxis.Line(false);
                                graphView.DataPlots[i].Plot.YAxis2.Line(false);
                                graphView.DataPlots[i].Plot.XAxis2.Line(false);
                                graphView.DataPlots[i].SetValue(Grid.RowProperty, i);
                                graphView.grid.Children.Add(graphView.DataPlots[i]);
                                graphView.DataYs[i] = new double[newData.Length / graphView.NumChannels];
                                graphView.DataPlots[i].Plot.AddSignal(graphView.DataYs[i]);
                            }
                            graphView.FirstCall = false;
                        }
                        //Trace.WriteLine(newData.Length);
                        for (int i = 0; i < newData.Length; i++)
                        {
                            graphView.DataYs[i % graphView.NumChannels][i / graphView.NumChannels] = newData[i];
                        }
                        for (int i = 0; i < graphView.NumChannels; i++)
                        {
                            graphView.DataPlots[i].Plot.AxisAuto();
                            graphView.DataPlots[i].Render();
                        }
                    }
                });
            }
        }
        #endregion

        public GraphView()
        {
            InitializeComponent();
        }
    }
}