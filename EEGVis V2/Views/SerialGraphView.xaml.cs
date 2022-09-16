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
        #region propdp
        public int NumChannels
        {
            get { return (int)GetValue(NumChannelsProperty); }
            set { SetValue(NumChannelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumChannels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumChannelsProperty =
            DependencyProperty.Register("NumChannels", typeof(int), typeof(SerialGraphView), new PropertyMetadata(0));




        public int SelectedPage
        {
            get { return (int)GetValue(SelectedPageProperty); }
            set { SetValue(SelectedPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumChannels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPageProperty =
            DependencyProperty.Register("SelectedPage", typeof(int), typeof(SerialGraphView), new PropertyMetadata(newPage));

        private static void newPage(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SerialGraphView sgv)
            {
                GraphView newGraphView = new GraphView();
                newGraphView.NumChannels = sgv.NumChannels;
                Binding graphDataBinding = new Binding("Points");
                newGraphView.SetBinding(GraphView.GraphDataProperty, graphDataBinding);
                sgv.GraphContentControl.Content = newGraphView;
            }
        }
        #endregion

        public SerialGraphView(SerialGraphViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
            Binding numChannelsBinding = new Binding("NumChannels");
            Binding graphDataBinding = new Binding("Points");
            Binding selectedPageBinding = new Binding("SelectedPage");
            SetBinding(NumChannelsProperty, numChannelsBinding);
            SetBinding(SelectedPageProperty, selectedPageBinding);
            GraphView newGraphView = new GraphView();
            newGraphView.NumChannels = NumChannels;
            newGraphView.SetBinding(GraphView.GraphDataProperty, graphDataBinding);
            GraphContentControl.Content = newGraphView;
        }
    }
}
