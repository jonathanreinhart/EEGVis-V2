using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Wpf;
using LiveCharts;
using System.Windows.Media;
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

        private int _current_x = 0;

        public SerialGraphViewModel()
        {
            Points = new double[5000];
            for (int i = 0; i < 5000; i++)
            {
                Points[i] = 0;
            }
            Task.Factory.StartNew(() =>
            {
                //SerialData serialData = new SerialData();
                while (!App.Current.Dispatcher.HasShutdownStarted)
                {
                    /*
                    if (serialData.newDataAvailable)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            _data.RemoveAt(0);
                            _data.Add(serialData.CurData[i]);
                        }
                        Trace.WriteLine(_data[0]);

                        var y = _data.ToArray();
                        App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            Plot(x, y);
                        }));
                    }*/
                    App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        double[] newData = new double[5000];
                        for (int i = 0; i < Points.Length - 100; i++)
                        {
                            newData[i] = Points[i + 100];
                        }
                        for (int i = Points.Length-100; i < Points.Length; i++)
                        {
                            newData[i] = (_current_x % 1000)*0.001;
                            _current_x++;
                        }
                        Points = newData;
                        OnPropertyChanged(nameof(Points));
                    }));
                    Thread.Sleep(100);
                }
                //serialData.closing = true;
            });
        }
    }
}
