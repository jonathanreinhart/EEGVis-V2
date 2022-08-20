using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
    internal class SerialData
    {
        public bool closing = false;
        public bool newDataAvailable = false;

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
        

        private const string comPort = "COM3";
        private const int baudrate = 115200;
        private const int dataLen = 502;
        private const int buffersize = 800;
        private const string DataFile = @"EEGData.csv";
        private readonly FastSerial fs = new FastSerial();

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialData"/> class.
        /// </summary>
        public SerialData()
        {
            CurData = new List<double>();
            Trace.WriteLine("Constructor");
            for (int i = 0; i < (dataLen - 2) / 5; i++)
            {
                CurData.Add(0);
            }
            fs.init(comPort, baudrate, buffersize);
            Thread.Sleep(1000);
            fs.writeStringToSerial("p\n");
            Thread.Sleep(100);
            Thread dataThread = new Thread(GetData);
            dataThread.Start();
        }

        /// <summary>
        /// Gets Serial data and saves it in <see cref="curData"/>
        /// </summary>
        /// <param name="obj"></param>
        private void GetData(object? obj)
        {
            bool firstString = true;
            while (!closing)
            {
                if (fs.available())
                {
                    string data = fs.getString();
                    if (!firstString)
                    {
                        //divide string in groups of five, and save
                        //in curData as double
                        for (int i = 0; i < (dataLen - 2) / 5; i++)
                        {
                            string dataPoint = data.Substring(i * 5, 5);
                            CurData[i] = double.Parse(dataPoint);
                            //Trace.WriteLine(CurData[i]);
                            //write curData[i] to DataFile
                        }
                        newDataAvailable = true;
                    }
                    else 
                    {
                        firstString = false;
                    }
                }
                Thread.Sleep(10);
            }
            fs.close();
        }
    }
}
