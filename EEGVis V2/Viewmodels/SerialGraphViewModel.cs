using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Wpf;
using LiveCharts;
using System.Threading;
using EEGVis_V2.models;
using LiveCharts.Configurations;
using System.ComponentModel;
using System.Windows;
using InteractiveDataDisplay.WPF;
using System.Windows.Shapes;
using System.Collections;
using System.Globalization;
using System.Windows.Threading;
using System.Diagnostics;
using System.Linq.Expressions;
using ScottPlot.Drawing.Colormaps;
using System.Windows.Media;
using System.Windows.Input;
using EEGVis_V2.Commands;

namespace EEGVis_V2.Viewmodels
{
    public class SerialGraphViewModel : ViewModelBase
    {
        #region properties
        private double[] points;
        public double[] Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
                OnPropertyChanged(nameof(Points));
            }
        }

        private Brush conColor;
        public Brush ConColor
        {
            get
            {
                return conColor;
            }
            set
            {
                conColor = value;
                OnPropertyChanged(nameof(ConColor));
            }
        }

        private int numChannels;
        public int NumChannels
        {
            get
            {
                return numChannels;
            }
            set
            {
                numChannels = value;
                OnPropertyChanged(nameof(NumChannels));
            }
        }

        private int selectedPage;
        public int SelectedPage
        {
            get
            {
                return selectedPage;
            }
            set
            {
                selectedPage = value;
                OnPropertyChanged(nameof(SelectedPage));
            }
        }

        //max channels per page
        private int maxChannels;
        public int MaxChannels
        {
            get
            {
                return maxChannels;
            }
            set
            {
                maxChannels = value;
                OnPropertyChanged(nameof(MaxChannels));
            }
        }

        private int _selectedPageNumChannels;
        public int SelectedPageNumChannels
        {
            get
            {
                return _selectedPageNumChannels;
            }
            set
            {
                _selectedPageNumChannels = value;
                OnPropertyChanged(nameof(SelectedPageNumChannels));
            }
        }
        #endregion

        public ICommand Restart { get; }
        public ICommand NextPage { get; }
        public int NumPages;
        public SerialData SerialData_;
        private int _num_datapoints;
        private int _cur_start_channel;

        public SerialGraphViewModel()
        {
            MaxChannels = 6;
            SelectedPage = 0;
            NumChannels = SerialData.NumChannels;
            SelectedPageNumChannels = getPageNumChannels(0);
            SerialData_ = new SerialData("COM5");
            Restart = new RestartSerialCommand(this);
            NextPage = new NextPageCommand(this);
            ConColor = new SolidColorBrush(Color.FromRgb(152,0,5));
            NumPages = (int)Math.Ceiling((double)NumChannels / MaxChannels);
            
            Thread plotThread = new Thread(PlotData);
            plotThread.Start();
        }

        /// <summary>
        /// Gets the number of channels on the selected page and sets required variables
        /// </summary>
        /// <param name="page"></param>
        /// <returns>number of channels on selected page</returns>
        public int getPageNumChannels(int page)
        {
            int reamainingChannels = NumChannels - page * MaxChannels;
            int pageNumChannels = reamainingChannels > 0 ? reamainingChannels : MaxChannels;
            _num_datapoints = SerialData.NumDatapoints * 5 * pageNumChannels;
            _cur_start_channel = page * MaxChannels;

            double[] newData = new double[_num_datapoints];
            for (int i = 0; i < _num_datapoints; i++)
            {
                newData[i] = 0;
            }
            Points = newData;
            return pageNumChannels;
        }

        /// <summary>
        /// save the data from last 5 seconds and make it available to the GraphView
        /// </summary>
        /// <param name="obj"></param>
        private void PlotData(object? obj)
        {
            while (!SerialData_.connected && !App.Current.Dispatcher.HasShutdownStarted) ;
            App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                ConColor = new SolidColorBrush(Color.FromRgb(0, 155, 31));
            }));
            while (App.Current != null && !SerialData_.closing)
            {
                if (!App.Current.Dispatcher.HasShutdownStarted)
                {
                    if (SerialData_.newDataAvailable)
                    {
                        App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            double[] newData = new double[_num_datapoints];
                            int newDataNum = (_num_datapoints/5/10);
                            for (int i = 0; i < Points.Length - newDataNum; i++)
                            {
                                newData[i] = Points[i + newDataNum];
                            }
                            for (int i = 0; i < newDataNum; i++)
                            {
                                //save new points if it's the channel we are searching for
                                if (i % NumChannels >= _cur_start_channel && i % NumChannels < _cur_start_channel + MaxChannels)
                                {
                                    int pos = i + Points.Length - newDataNum;
                                    newData[i + Points.Length - newDataNum] = SerialData_.CurData[i];
                                }
                            }
                            Points = newData;
                        }));
                    }
                }
                Thread.Sleep(10);
            }
            SerialData_.closing = true;
            if (SerialData_.reconnecting)
            {
                SerialData_ = new SerialData("COM5");
                Thread new_plotThread = new Thread(PlotData);
                new_plotThread.Start();
            }
        }
    }
}
