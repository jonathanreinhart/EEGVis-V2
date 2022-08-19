﻿using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
using System.Threading;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using InteractiveDataDisplay.WPF;
using System.ComponentModel;
using EEGVis_V2.Viewmodels;

namespace EEGVis_V2.Views
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {

        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set 
            { 
                SetValue(PointsProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(GraphView), new PropertyMetadata(PointsChanged));

        private static void PointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is GraphView)
            {
                GraphView graphView = d as GraphView;
                graphView.linegraph.Points = (PointCollection)e.NewValue;
            }
        }

        public GraphView()
        {
            
            InitializeComponent();
        }
    }
}
