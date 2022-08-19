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

        public SerialGraphViewModel()
        {
            var x = Enumerable.Range(0, 1001).Select(i => i / 10.0).ToArray();
            var y = x.Select(v => Math.Abs(v) < 1e-10 ? 1 : Math.Sin(v) / v).ToArray();
            Plot(x, y);
        }

        private void Plot(IEnumerable x, IEnumerable y)
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");

            var points = new PointCollection();
            var enx = x.GetEnumerator();
            var eny = y.GetEnumerator();
            while (true)
            {
                var nx = enx.MoveNext();
                var ny = eny.MoveNext();
                if (nx && ny)
                    points.Add(new Point(Convert.ToDouble(enx.Current, CultureInfo.InvariantCulture),
                        Convert.ToDouble(eny.Current, CultureInfo.InvariantCulture)));
                else if (!nx && !ny)
                    break;
                else
                    throw new ArgumentException("x and y have different lengthes");
            }

            Points = points;
        }
    }
}
