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

namespace EEGVis_V2.Viewmodels
{
    public class SerialGraphViewModel : ViewModelBase
    {
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

        public SerialGraphViewModel()
        {
            conColor = new SolidColorBrush(Color.FromRgb(255,0,0));
            Points = new double[5000];
            for (int i = 0; i < 5000; i++)
            {
                Points[i] = 0;
            }
            Task.Factory.StartNew(() =>
            {
                SerialData serialData = new SerialData();
                while (!serialData.connected && !App.Current.Dispatcher.HasShutdownStarted) ;
                App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    ConColor = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                }));
                while (App.Current!=null)
                {
                    if (!App.Current.Dispatcher.HasShutdownStarted)
                    {
                        if (serialData.newDataAvailable)
                        {
                            App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                double[] newData = new double[5000];
                                for (int i = 0; i < Points.Length - 100; i++)
                                {
                                    newData[i] = Points[i + 100];
                                }
                                for (int i = Points.Length - 100; i < Points.Length; i++)
                                {
                                    newData[i] = serialData.CurData[i - Points.Length + 100];
                                }
                                Points = newData;
                                OnPropertyChanged(nameof(Points));
                            }));
                        }
                    }
                    Thread.Sleep(10);
                }
                serialData.closing = true;
            });
        }
    }
}
