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
                newGraphView.NumChannels = sgv.SelectedPageNumChannels;
                newGraphView.CurStartChannel = sgv.CurStartChannel;
                Binding graphDataBinding = new Binding("Points");
                newGraphView.SetBinding(GraphView.GraphDataProperty, graphDataBinding);
                sgv.GraphContentControl.Content = newGraphView;
            }
        }

        public int SelectedPageNumChannels
        {
            get { return (int)GetValue(SelectedPageNumChannelsProperty); }
            set { SetValue(SelectedPageNumChannelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPageNumChannels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPageNumChannelsProperty =
            DependencyProperty.Register("SelectedPageNumChannels", typeof(int), typeof(SerialGraphView), new PropertyMetadata(1));



        public int CurStartChannel
        {
            get { return (int)GetValue(CurStartChannelProperty); }
            set { SetValue(CurStartChannelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurStartChannel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurStartChannelProperty =
            DependencyProperty.Register("CurStartChannel", typeof(int), typeof(SerialGraphView), new PropertyMetadata(0));



        #endregion

        public SerialGraphView(SerialGraphViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
            Binding graphDataBinding = new Binding("Points");
            Binding selectedPageBinding = new Binding("SelectedPage");
            Binding SelectedPageNumChannelsBinding = new Binding("SelectedPageNumChannels");
            Binding CurStartChannelBinding = new Binding("CurStartChannel");
            SetBinding(SelectedPageProperty, selectedPageBinding);
            SetBinding(SelectedPageNumChannelsProperty, SelectedPageNumChannelsBinding);
            SetBinding(CurStartChannelProperty, CurStartChannelBinding);
            GraphView newGraphView = new GraphView();
            newGraphView.NumChannels = SelectedPageNumChannels;
            newGraphView.CurStartChannel = CurStartChannel;
            newGraphView.SetBinding(GraphView.GraphDataProperty, graphDataBinding);
            GraphContentControl.Content = newGraphView;
        }
    }
}
