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
        private PointCollection points;
        public PointCollection Points
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

        //saves the data from the last 5 seconds
        private double[] x = new double[5000];
        private List<double> _data = new List<double>();
        private int _current_x = 0;

        public SerialGraphViewModel()
        {
            for (int i = 0; i < 5000; i++)
            {
                _data.Add(0);
                x[i] = i;
            }
            Task.Factory.StartNew(() =>
            {
            //SerialData serialData = new SerialData();
            while (!closing)
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
                for (int i = 0; i < 100; i++)
                {
                    _data.RemoveAt(0);
                    _data.Add(_current_x % 1000);
                    _current_x++;
                }
                var y = _data.ToArray();
                App.Current.Dispatcher.Invoke(() => Plot(x, y));
                    Thread.Sleep(100);
                }
            });
        }

        /// <summary>
        /// convert the data from an array to a pointcollection
        /// </summary>
        /// <param name="x">x values</param>
        /// <param name="y">y values</param>
        private void Plot(IEnumerable x, IEnumerable y)
        {
            var points = new PointCollection();
            var enx = x.GetEnumerator();
            var eny = y.GetEnumerator();
            while (enx.MoveNext() && eny.MoveNext())
            {
                points.Add(new Point((double)enx.Current, (double)eny.Current));
            }
            Points = points;
        }
    }
}
