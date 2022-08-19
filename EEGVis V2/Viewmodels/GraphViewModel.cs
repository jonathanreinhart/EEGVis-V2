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

namespace EEGVis_V2.Viewmodels
{
    public class GraphViewModel : ViewModelBase
    {
        private double _axisMax;
        private double _axisMin;
        private double _trend;

        public GraphViewModel()
        {

            //To handle live data easily, in this case we built a specialized type
            //the MeasureModel class, it only contains 2 properties
            //DateTime and Value
            //We need to configure LiveCharts to handle MeasureModel class
            //The next code configures MeasureModel  globally, this means
            //that LiveCharts learns to plot MeasureModel and will use this config every time
            //a IChartValues instance uses this type.
            //this code ideally should only run once
            //you can configure series in many ways, learn more at 
            //http://lvcharts.net/App/examples/v1/wpf/Types%20and%20Configuration

            var mapper = Mappers.Xy<SerialDataPoint>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<SerialDataPoint>(mapper);

            //the values property will store our values array
            ChartValues = new ChartValues<SerialDataPoint>();

            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long)value).ToString("ss.ff");

            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);

            Task.Factory.StartNew(Read);
        }

        public ChartValues<SerialDataPoint> ChartValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }

        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

        private void Read()
        {
            var r = new Random();
            
            while (!closing)
            {
                Thread.Sleep(10);
                var now = DateTime.Now;

                _trend += r.Next(-8, 10);

                ChartValues.Add(new SerialDataPoint
                {
                    DateTime = now,
                    Value = _trend
                });

                SetAxisLimits(now);

                //lets only use the last 150 values
                if (ChartValues.Count > 150) ChartValues.RemoveAt(0);
            }
        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(5).Ticks; // and 8 seconds behind
        }
    }
}
