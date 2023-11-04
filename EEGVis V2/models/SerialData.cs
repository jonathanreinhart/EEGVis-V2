using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FastSerialLibWrapper;

namespace EEGVis_V2.models
{
    /// <summary>
    /// Class for handling the EEG data coming from Arduino
    /// </summary>
    public class SerialData
    {
        public bool closing = false;
        public bool newDataAvailable = false;
        public bool connected = false;
        public bool reconnecting = false;
        public const int NumDatapoints = 200;//datapoints in 1s
        public const int NumChannels = 6;
        public const int LenDatapoint = 7;
        public const int LenDataPacket = NumChannels * NumDatapoints / 10;

        #region properties
        private List<double> _curData;
        public List<double> CurData
        {
            get 
            {
                //when ViewModel read the data, don't read same data again
                newDataAvailable = false;
                return _curData; 
            }
            set { _curData = value; }
        }

        public static int NumData = 0;
        #endregion

        private List<UInt32> _lastData;


        private string comPort;
        private int baudrate;
        private const int buffersize = 1;
        private const int _start_delay = 1700;
        private const string _data_file = "EEGData13.csv";
        private readonly StreamWriter _writer;
        private readonly FastSerial fs = new FastSerial();

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialData"/> class.
        /// </summary>
        public SerialData(string _comPort, int _baudrate = 2000000)
        {
            comPort = _comPort;
            baudrate = _baudrate;
            CurData = new List<double>();
            for (int i = 0; i < NumChannels * NumDatapoints / 10; i++)
            {
                CurData.Add(0);
            }
            _lastData = new List<UInt32>();
            _writer = new StreamWriter(_data_file);
            _writer.WriteLine("data");
            fs.init(comPort, baudrate, buffersize);
            Thread dataThread = new Thread(GetData);
            dataThread.Start();
        }

        /// <summary>
        /// Gets Serial data and saves filtered signal in <see cref="CurData"/> and unfiltered signal in <see cref="CurDataUnfiltered"/>
        /// </summary>
        /// <param name="obj"></param>
        private void GetData(object? obj)
        {
            Thread.Sleep(_start_delay);
            while (!fs.writeStringToSerial("p\n"))
            {
                Thread.Sleep(100);
                fs.init(comPort, baudrate, buffersize);
                Thread.Sleep(_start_delay);
            }
            connected = true;
            Thread.Sleep(100);
            bool firstString = true;
            while (!closing)
            {
                if (fs.available())
                {
                    UInt32[] data = fs.get24Array();

                    //Debug.WriteLine("data: " + data.Length);
                    if (!firstString)
                    {
                        if (data.Length == LenDataPacket)
                        {
                            // save new raw data in _lastData
                            if (_lastData.Count() >= NumChannels * NumDatapoints)
                            {
                                _lastData.RemoveRange(0, LenDataPacket);
                            }

                            for (int i = 0; i < data.Count(); i++)
                            {
                                _lastData.Add(data[i]);
                            }
                            
                            List<UInt32> data_filtered = FIRFilter.Filter24(_lastData);

                            // save filtered data in CurData and data write to csv file
                            for (int i = 0; i < LenDataPacket; i++)
                            {
                                //CurDataUnfiltered[i] = data[i];
                                CurData[i] = data[i];
                                //write current UNFILTERED data to file
                                _writer.WriteLine(data[i].ToString());
                            }
                            NumData += data.Count();
                            newDataAvailable = true;
                        }
                    }
                    else
                    {
                        firstString = false;
                    }
                }
            }
            Trace.WriteLine("closing");
            _writer.Close();
            fs.close();
        }
    }
}