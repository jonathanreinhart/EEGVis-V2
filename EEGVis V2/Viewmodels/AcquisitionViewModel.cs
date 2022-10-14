using LiveCharts.Wpf;
using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
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
        
        private Random _rnd = new Random();
		private String _previousLabel;

        public AcquisitionViewModel()
		{
            ChangeLabel("lamp");
            Thread controlThread = new Thread(ShowLabels);
			controlThread.Start();
        }

		private void ShowLabels(object? obj)
		{
            for(int i = 0; i < 100; i++)
			{
                Thread.Sleep(2000);
                ChangeLabel();
            }
        }

        public void ChangeLabel(string? label = null)
		{
            //get random label
            if (label == null)
                label = _authors.Keys.ElementAt(_rnd.Next(_authors.Count));

			//don't show the same label two times
			while (label == _previousLabel)
			{
                label = _authors.Keys.ElementAt(_rnd.Next(_authors.Count));
            }

                Trace.WriteLine(label);
            Label = label;
            ImageSource = "../rsc/Acquisition/" + label + ".png";
            AttributionLink = "https://www.flaticon.com/free-icons/" + label;
            AttributionText = "Lamp icon created by " + _authors[label] + " -Flaticon";
			_previousLabel = label;
        }

    }
}
