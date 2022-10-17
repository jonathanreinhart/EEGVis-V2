using EEGVis_V2.models;
using LiveCharts.Wpf;
using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace EEGVis_V2.Viewmodels
{
    public class AcquisitionViewModel : ViewModelBase
    {

        #region Properties
        private String _label;
		public String Label
		{
			get
			{
				return _label;
			}
			set
			{
                _label = value;
				OnPropertyChanged(nameof(Label));
			}
		}

		private String _attributionText;
		public String AttributionText
		{
			get
			{
				return _attributionText;
			}
			set
			{
				_attributionText = value;
				OnPropertyChanged(nameof(AttributionText));
			}
		}

		private String _attributionLink;
		public String AttributionLink
		{
			get
			{
				return _attributionLink;
			}
			set
			{
				_attributionLink = value;
				OnPropertyChanged(nameof(AttributionLink));
			}
		}

		private String _imageSource;
		public String ImageSource
		{
			get
			{
				return _imageSource;
			}
			set
			{
				_imageSource = value;
				OnPropertyChanged(nameof(ImageSource));
			}
		}
		#endregion

		#region Authors
		private IDictionary<string, string> _authors = new Dictionary<string, string>(){
			{"lamp", "Freepik"},
			{"off", "scrip"},
			{"on", "Freepik"},
            {"zero", "Hight Quality Icons"},
            {"one", "Hight Quality Icons"},
            {"two", "Hight Quality Icons"},
            {"three", "Hight Quality Icons"},
            {"four", "Hight Quality Icons"},
            {"five", "Hight Quality Icons"},
            {"six", "Hight Quality Icons"},
            {"seven", "Hight Quality Icons"},
            {"eight", "Hight Quality Icons"},
            {"nine", "Hight Quality Icons"},
			{"heater", "Nikita Golubev"},
            {"speaker", "Freepik"},
            {"temperature", "Freepik"},
            {"humidity", "Freepik"},
            {"pressure", "Freepik"},
            {"weather forecast", "Freepik"},
            {"surveillance camera", "Freepik"},
            {"computer", "Freepik"},
            {"greenhouse", "Freepik"},
            
        };
		#endregion

		public static int SecondsData = 5;

        private double[] _data;
        private Random _rnd = new Random();
		private string _previousLabel;
		//counts seconds since last label change
		private int _cur_sec = 0;
        private SerialGraphViewModel _serialGraphViewModel;
        private const string _data_file = "../../../models/AcquisitionData.csv";

        public AcquisitionViewModel(SerialGraphViewModel serialGraphViewModel)
		{
            _data = new double[SecondsData * SerialData.NumDatapoints];

			//set up csv file
			System.IO.File.WriteAllText(_data_file, "label,");
            for (int i = 0; i < SerialData.NumDatapoints * SecondsData; i++)
			{
                System.IO.File.AppendAllText(_data_file, "P" + i + ",");
            }
			System.IO.File.AppendAllText(_data_file, Environment.NewLine);

                ChangeLabel("lamp");
			_serialGraphViewModel = serialGraphViewModel;
            _serialGraphViewModel.PropertyChanged += SerialGraphViewModel_PropertyChanged;
        }

		private void SerialGraphViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "RawData")
			{
				if (_cur_sec >= SecondsData * 10)
				{
					//because we just set SecondsData to 5, we can just take data as it is for now
					_data = _serialGraphViewModel.RawData;
					System.IO.File.AppendAllText(_data_file, _previousLabel + ",");
                    System.IO.File.AppendAllText(_data_file, string.Join(",", _data) + Environment.NewLine);
                    
                    ChangeLabel();
					_cur_sec = 0;
                }
				_cur_sec++;
			}
		}

		public void ChangeLabel(string? label = null)
		{
            //get random label
            if (label == null)
                label = _authors.Keys.ElementAt(_rnd.Next(_authors.Count));

			//avoid showing the same label two times
			while (label == _previousLabel)
			{
                label = _authors.Keys.ElementAt(_rnd.Next(_authors.Count));
            }

            Trace.WriteLine("change label: " + label);
            Label = label;
            ImageSource = "../rsc/Acquisition/" + label + ".png";
            AttributionLink = "https://www.flaticon.com/free-icons/" + label;
            AttributionText = label + " icon created by " + _authors[label] + " -Flaticon";
			_previousLabel = label;
        }

    }
}
