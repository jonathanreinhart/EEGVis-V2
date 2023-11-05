using System;
using System.Threading;
using EEGVis_V2.models;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Input;
using EEGVis_V2.Commands;

namespace EEGVis_V2.Viewmodels
{
    public class SerialGraphViewModel : ViewModelBase
    {
        #region properties

        private double[] points;
        /// <summary>
        /// The data to be displayed in the graph on the selected page
        /// </summary>
        public double[] Points
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

        private double[] _rawData;
        /// <summary>
        /// Data from all channels
        /// </summary>
        public double[] RawData
        {
            get
            {
                return _rawData;
            }
            set
            {
                _rawData = value;
                OnPropertyChanged(nameof(RawData));
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

        //max channels per page
        private int maxChannels;
        public int MaxChannels
        {
            get
            {
                return maxChannels;
            }
            set
            {
                maxChannels = value;
                OnPropertyChanged(nameof(MaxChannels));
            }
        }

        private int _selectedPageNumChannels;
        public int SelectedPageNumChannels
        {
            get
            {
                return _selectedPageNumChannels;
            }
            set
            {
                _selectedPageNumChannels = value;
                OnPropertyChanged(nameof(SelectedPageNumChannels));
            }
        }

        private int _curStartChannel;
        public int CurStartChannel
        {
            get
            {
                return _curStartChannel;
            }
            set
            {
                _curStartChannel = value;
                OnPropertyChanged(nameof(CurStartChannel));
            }
        }



        #endregion

        public ICommand Restart { get; }
        public ICommand NextPage { get; }
        public ICommand LastPage { get; }
        public int NumPages;
        public SerialData SerialData_;
        public bool UpdateData;
        public bool PlotSelected;
        public const int SecondsData = 5;
        private int _num_raw_datapoints;

        public SerialGraphViewModel()
        {
            SerialData_ = new SerialData("COM5");

            MaxChannels = 6;
            SelectedPage = 0;
            NumChannels = SerialData.NumChannels;
            getPageNumChannels(0);

            Restart = new RestartSerialCommand(this);
            NextPage = new NextPageCommand(this);
            LastPage = new LastPageCommand(this);
            ConColor = new SolidColorBrush(Color.FromRgb(152,0,5));

            NumPages = (int)Math.Ceiling((double)NumChannels / MaxChannels);
            //make Raw data size so that it can hold SecondsRawData seconds of data of all the channels
            _num_raw_datapoints = SerialData.NumChannels * SerialData.NumDatapoints * SecondsData;
            double[] newData = new double[_num_raw_datapoints];
            for (int i = 0; i < _num_raw_datapoints; i++)
            {
                newData[i] = 0;
            }
            RawData = newData;
            Thread plotThread = new Thread(PlotData);
            plotThread.Start();
        }

        /// <summary>
        /// Gets the number of channels on the selected page and sets required variables
        /// </summary>
        /// <param name="page"></param>
        /// <returns>number of channels on selected page</returns>
        public void getPageNumChannels(int page)
        {
            int reamainingChannels = NumChannels - page * MaxChannels;
            SelectedPageNumChannels = reamainingChannels > MaxChannels ? MaxChannels : reamainingChannels;
            CurStartChannel = page * MaxChannels;

            // set the data of selected channels to the data already saved in RawData
            if (PlotSelected)
            {
                int newPointsLength = SelectedPageNumChannels * SerialData.NumDatapoints * SecondsData;

                double[] newData = new double[newPointsLength];

                // index in the received data
                int l = CurStartChannel;
                for (int i = 0; i < newPointsLength; i++)
                {
                    newData[i] = RawData[l];
                    if (i % SelectedPageNumChannels == SelectedPageNumChannels - 1)
                    {
                        l += NumChannels - SelectedPageNumChannels;
                    }
                    l++;
                }
                Points = newData;
            }
            else
            {
                int newPointsLength = SelectedPageNumChannels * SerialData.NumDatapoints * SecondsData;

                double[] newData = new double[newPointsLength];
                
                for (int i = 0; i < newPointsLength; i++)
                {
                    newData[i] = 0;
                }
                Points = newData;
            }
        }

        /// <summary>
        /// save the data from last 5 seconds and make it available to the GraphView
        /// </summary>
        /// <param name="obj"></param>
        private void PlotData(object? obj)
        {
            while (!SerialData_.connected && !App.Current.Dispatcher.HasShutdownStarted);
            App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                ConColor = new SolidColorBrush(Color.FromRgb(0, 155, 31));
            }));
            //while Window is not closing
            while (App.Current != null && !SerialData_.closing)
            {
                //check if data has to be updated
                if (UpdateData && !App.Current.Dispatcher.HasShutdownStarted)
                {
                    if (SerialData_.newDataAvailable)
                    {
                        App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            //always save all the data to RawData
                            //updated data
                            double[] newData = new double[_num_raw_datapoints];
                            //number of datapoints that are new
                            int newDataNum = (_num_raw_datapoints / SecondsData / 10);
                            for (int i = 0; i < RawData.Length - newDataNum; i++)
                            {
                                newData[i] = RawData[i + newDataNum];
                            }
                            for (int i = 0; i < newDataNum; i++)
                            {
                                newData[i + RawData.Length - newDataNum] = SerialData_.CurData[i];
                            }
                            RawData = newData;

                            //check if data has to be plotted and then update the specified channels
                            if (PlotSelected)
                            {
                                // updated data
                                newData = new double[Points.Length];

                                // number of datapoints that are new
                                newDataNum = SerialData.NumDatapoints*SelectedPageNumChannels/10;

                                for (int i = 0; i < Points.Length - newDataNum; i++)
                                {
                                    newData[i] = Points[i + newDataNum];
                                }

                                // index in the received data
                                int l = CurStartChannel;
                                for (int i = 0; i < newDataNum; i++)
                                {
                                    newData[i + Points.Length - newDataNum] = SerialData_.CurData[l];
                                    if (i % SelectedPageNumChannels == SelectedPageNumChannels - 1)
                                    {
                                        l += NumChannels - SelectedPageNumChannels;
                                    }
                                    l++;
                                }
                                Points = newData;
                            }
                            
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
