using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ScottPlot;
using ScottPlot.WPF;

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



        public int CurStartChannel
        {
            get { return (int)GetValue(CurStartChannelProperty); }
            set { SetValue(CurStartChannelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurStartChannel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurStartChannelProperty =
            DependencyProperty.Register("CurStartChannel", typeof(int), typeof(GraphView), new PropertyMetadata(0));




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
                    if (newData != null)
                    {
                        if (newData.Length > 0)
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
                                    graphView.DataPlots[i].Plot.Title("channel " + (graphView.CurStartChannel + i).ToString());
                                    graphView.DataPlots[i].Margin = new Thickness(0);
                                    //graphView.DataPlots[i].Plot.Style(ScottPlot.Style.Blue1);
                                    //graphView.DataPlots[i].Plot.Layout.Fixed(new ScottPlot.PixelPadding(40, 10, 40, 5));

                                    // styling
                                    graphView.DataPlots[i].Plot.FigureBackground.Color = ScottPlot.Color.FromHex("07263B");
                                    graphView.DataPlots[i].Plot.DataBackground.Color = ScottPlot.Color.FromHex("0A304A");
                                    graphView.DataPlots[i].Plot.Axes.Color(ScottPlot.Colors.White);

                                    graphView.DataPlots[i].Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("17374C");
                                    graphView.DataPlots[i].Plot.Grid.MajorLineWidth = 1;

                                    //// hide just the horizontal axis ticks
                                    //graphView.DataPlots[i].Plot.XAxis.Ticks(false);
                                    //// hide the lines on the bottom, right, and top of the plot
                                    //graphView.DataPlots[i].Plot.XAxis.Line(false);
                                    //graphView.DataPlots[i].Plot.YAxis2.Line(false);
                                    //graphView.DataPlots[i].Plot.XAxis2.Line(false);

                                    // SP5: Hiding Axes and Lines
                                    //graphView.DataPlots[i].Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();
                                    graphView.DataPlots[i].Plot.Axes.Bottom.TickLabelStyle.IsVisible = false;
                                    graphView.DataPlots[i].Plot.Axes.Bottom.FrameLineStyle.IsVisible = false;
                                    graphView.DataPlots[i].Plot.Axes.Right.FrameLineStyle.IsVisible = false;
                                    graphView.DataPlots[i].Plot.Axes.Top.FrameLineStyle.IsVisible = false;
                                    graphView.DataPlots[i].Plot.Axes.Bottom.MajorTickStyle.Length = 0;
                                    graphView.DataPlots[i].Plot.Axes.Bottom.MinorTickStyle.Length = 0;
                                    //graphView.DataPlots[i].Plot.Layout.Frameless();

                                    //PixelPadding padding = new(50, 0, 0, 0);
                                    //graphView.DataPlots[i].Plot.Layout.Fixed(padding);
                                    graphView.DataPlots[i].Plot.Axes.Title.Label.FontSize = 12;
                                    graphView.DataPlots[i].Plot.Axes.Title.Label.Padding = 0;

                                    graphView.DataPlots[i].SetValue(Grid.RowProperty, i);
                                    graphView.grid.Children.Add(graphView.DataPlots[i]);
                                    graphView.DataYs[i] = new double[newData.Length / graphView.NumChannels];
                                    graphView.DataPlots[i].Plot.Add.Signal(graphView.DataYs[i]);
                                }
                                graphView.FirstCall = false;
                            }
                            // Trace.WriteLine(newData.Length);
                            
                            
                            try
                            {
                                for (int i = 0; i < newData.Length; i++)
                                {
                                    graphView.DataYs[i % graphView.NumChannels][i / graphView.NumChannels] = newData[i];
                                }
                                for (int i = 0; i < graphView.NumChannels; i++)
                                {
                                    graphView.DataPlots[i].Plot.Axes.SetLimitsX(0, graphView.DataYs[i].Length);
                                    graphView.DataPlots[i].Plot.Axes.AutoScaleY();
                                    graphView.DataPlots[i].Refresh();
                                }
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                // do nothing
                            }
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