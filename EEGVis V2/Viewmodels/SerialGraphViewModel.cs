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
        #endregion

        public ICommand Restart { get; }
        public ICommand NextPage { get; }
        public SerialData SerialData_;
        private int _num_datapoints;

        public SerialGraphViewModel()
        {
            SelectedPage = 0;
            SerialData_ = new SerialData("COM5");
            Restart = new RestartSerialCommand(this);
            NextPage = new NextPageCommand(this);
            ConColor = new SolidColorBrush(Color.FromRgb(152,0,5));
            NumChannels = SerialData.NumChannels;
            _num_datapoints = SerialData.NumDatapoints*5*NumChannels;

            Points = new double[_num_datapoints];
            for (int i = 0; i < _num_datapoints; i++)
            {
                Points[i] = 0;
            }
            Thread plotThread = new Thread(PlotData);
            plotThread.Start();
        }

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
                            int newDataNum = (SerialData.NumChannels * SerialData.NumDatapoints / 10);
                            for (int i = 0; i < Points.Length - newDataNum; i++)
                            {
                                newData[i] = Points[i + newDataNum];
                            }
                            for (int i = Points.Length - newDataNum; i < Points.Length; i++)
                            {
                                newData[i] = SerialData_.CurData[i - Points.Length + newDataNum];
                            }
                            Points = newData;
                            OnPropertyChanged(nameof(Points));
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
