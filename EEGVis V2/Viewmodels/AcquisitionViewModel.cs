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
    public class AcquisitionViewModel : ViewModelBase, IDisposable
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

		private String _progress;
		public String Progress
		{
			get
			{
				return _progress;
			}
			set
			{
				_progress = value;
				OnPropertyChanged(nameof(Progress));
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
			//{"lamp", "Freepik"},
			//{"off", "scrip"},
			//{"on", "Freepik"},
   //         {"zero", "Hight Quality Icons"},
   //         {"one", "Hight Quality Icons"},
   //         {"two", "Hight Quality Icons"},
   //         {"three", "Hight Quality Icons"},
   //         {"four", "Hight Quality Icons"},
   //         {"five", "Hight Quality Icons"},
   //         {"six", "Hight Quality Icons"},
   //         {"seven", "Hight Quality Icons"},
   //         {"eight", "Hight Quality Icons"},
   //         {"nine", "Hight Quality Icons"},
			//{"heater", "Nikita Golubev"},
   //         {"speaker", "Freepik"},
   //         {"temperature", "Freepik"},
   //         {"humidity", "Freepik"},
   //         {"pressure", "Freepik"},
   //         {"weather forecast", "Freepik"},
   //         {"surveillance camera", "Freepik"},
   //         {"computer", "Freepik"},
   //         {"greenhouse", "Freepik"},
			//{"lFist", "Smashicons"},
   //         {"rFist", "Smashicons"},
   //         {"bFists", "Smashicons"},
   //         {"bFeet", "Freepik"},
			{"blink", "Freepik"},
			{"rest", "Freepik"}
        };
		#endregion

		public static double SecondsData = 1.2;
		public static double SecondsRest = 2;
		public static int SecondsTotal = 60;

        private double[] _data;
        private Random _rnd = new Random();
		private string _previousLabel;
		//counts seconds since last label change
		private int _cur_sec = 0;
		private int _last_sec = 0;
        private SerialGraphViewModel _serialGraphViewModel;
        private const string _data_file = "AcquisitionData13.csv";

        public AcquisitionViewModel(SerialGraphViewModel serialGraphViewModel)
		{
			double data_l = SecondsData > SecondsRest ? SecondsData : SecondsRest;
            _data = new double[(int)(data_l * SerialData.NumDatapoints)];

            //set up csv file
            System.IO.File.WriteAllText(_data_file, "label,start,end" + Environment.NewLine);
			Debug.WriteLine("init acquisition");

            ChangeLabel("rest");
			Progress = "0/" + SecondsTotal;
            _serialGraphViewModel = serialGraphViewModel;
            _serialGraphViewModel.PropertyChanged += SerialGraphViewModel_PropertyChanged;
        }

		private void SerialGraphViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "RawData")
			{
                // if one run is done, detach the event handler
                if (_cur_sec > SecondsTotal * 10)
                {
                    _serialGraphViewModel.PropertyChanged -= SerialGraphViewModel_PropertyChanged;
                    return;
                }

				// save labels in specific interval
				if (_cur_sec > 0)
				{
					Debug.Write(_cur_sec - _last_sec);
					Debug.Write(", ");
					Debug.WriteLine(SecondsData * 10);
					if (_cur_sec - _last_sec >= (SecondsRest * 10) && Label == "rest")
					{
                        // only save indecies of the last SecondsRest seconds
                        System.IO.File.AppendAllText(_data_file, _previousLabel + ",");
						String startIndex = ((int)(SerialData.NumData - SecondsRest * SerialData.NumDatapoints * SerialData.NumChannels)).ToString();
						String endIndex = (SerialData.NumData - 1).ToString();
						System.IO.File.AppendAllText(_data_file, startIndex + "," + endIndex + Environment.NewLine);
						Progress = (_cur_sec / 10.0).ToString() + "/" + SecondsTotal;
						_last_sec = _cur_sec;
						ChangeLabel();
					}
					else if (_cur_sec - _last_sec >= (SecondsData * 10) && Label != "rest")
					{
                        // only save indecies of the last SecondsData seconds
                        System.IO.File.AppendAllText(_data_file, _previousLabel + ",");
                        String startIndex = ((int)(SerialData.NumData - SecondsData * SerialData.NumDatapoints * SerialData.NumChannels)).ToString();
                        String endIndex = (SerialData.NumData - 1).ToString();
                        System.IO.File.AppendAllText(_data_file, startIndex + "," + endIndex + Environment.NewLine);
                        Progress = (_cur_sec / 10.0).ToString() + "/" + SecondsTotal;
                        _last_sec = _cur_sec;
                        ChangeLabel();
                    }
				}

				_cur_sec++;
			}
		}

        /// <summary>
        /// <para>Changes the label and the image to a random one.</para>
		/// <para>The rest label always comes between the other labels</para>
        /// </summary>
        /// <param name="label">Label to swtich to.</param>
        public void ChangeLabel(string? label = null)
		{
			//get random label
			if (label == null)
			{
				if (_previousLabel != "rest")
				{
					label = "rest";
				}
				else
				{
                    label = _authors.Keys.ElementAt(_rnd.Next(_authors.Count - 1));

                    //avoid showing the same label two times
                    while (label == _previousLabel)
                    {
                        label = _authors.Keys.ElementAt(_rnd.Next(_authors.Count - 1));
                    }
                }
			}

            Trace.WriteLine("change label: " + label);
            Label = label;
            ImageSource = "../rsc/Acquisition/" + label + ".png";
			if (label != "")
			{
                AttributionLink = "https://www.flaticon.com/free-icons/" + label;
                AttributionText = label + " icon created by " + _authors[label] + " -Flaticon";

            }
			else
			{
                AttributionLink = "";
                AttributionText = "";
            }
			_previousLabel = label;
        }

        public void Dispose()
        {
            _serialGraphViewModel.PropertyChanged -= SerialGraphViewModel_PropertyChanged;
        }
    }
}
