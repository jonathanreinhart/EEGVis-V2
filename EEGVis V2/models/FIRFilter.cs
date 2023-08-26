using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEGVis_V2.models
{
    public class FIRFilter
    {
        public const int FilterSize = 101;
        
        private static double[] b = {
            -1.24827514e-18, -5.00206831e-04, -3.26193039e-04,  3.51141463e-04,
            6.22025193e-04, -1.59957101e-18, -7.73203459e-04, -5.38950204e-04,
            6.10265657e-04,  1.12020469e-03, -2.61907133e-18, -1.44140067e-03,
           -1.00843561e-03,  1.13908995e-03,  2.07669163e-03, -4.20698031e-18,
           -2.61525545e-03, -1.80641766e-03,  2.01351689e-03,  3.62220491e-03,
           -6.20786236e-18, -4.44592512e-03, -3.03444335e-03,  3.34475056e-03,
            5.95528091e-03, -8.42585719e-18, -7.18072043e-03, -4.86507627e-03,
            5.32911843e-03,  9.44013168e-03, -1.06438520e-17, -1.13098022e-02,
           -7.65402659e-03,  8.38783744e-03,  1.48909943e-02, -1.26447341e-17,
           -1.80295497e-02, -1.23122070e-02,  1.36570388e-02,  2.46327243e-02,
           -1.42326430e-17, -3.12495780e-02, -2.20717327e-02,  2.55757372e-02,
            4.88589403e-02, -1.52521434e-17, -7.46403588e-02, -6.19004568e-02,
            9.32738767e-02,  3.02664459e-01,  4.00275819e-01,  3.02664459e-01,
            9.32738767e-02, -6.19004568e-02, -7.46403588e-02, -1.52521434e-17,
            4.88589403e-02,  2.55757372e-02, -2.20717327e-02, -3.12495780e-02,
           -1.42326430e-17,  2.46327243e-02,  1.36570388e-02, -1.23122070e-02,
           -1.80295497e-02, -1.26447341e-17,  1.48909943e-02,  8.38783744e-03,
           -7.65402659e-03, -1.13098022e-02, -1.06438520e-17,  9.44013168e-03,
            5.32911843e-03, -4.86507627e-03, -7.18072043e-03, -8.42585719e-18,
            5.95528091e-03,  3.34475056e-03, -3.03444335e-03, -4.44592512e-03,
           -6.20786236e-18,  3.62220491e-03,  2.01351689e-03, -1.80641766e-03,
           -2.61525545e-03, -4.20698031e-18,  2.07669163e-03,  1.13908995e-03,
           -1.00843561e-03, -1.44140067e-03, -2.61907133e-18,  1.12020469e-03,
            6.10265657e-04, -5.38950204e-04, -7.73203459e-04, -1.59957101e-18,
            6.22025193e-04,  3.51141463e-04, -3.26193039e-04, -5.00206831e-04,
           -1.24827514e-18
        };

        /// <summary>
        /// <para> Filter a packet of data </para>
        /// <para> Info: FIR coeffs are generated with Phython </para>
        /// <para> FIR Filter definition: <see href="https://en.wikipedia.org/wiki/Finite_impulse_response">FIR Wikipedia</see></para>
        /// </summary>
        /// <param name="lastData"></param>
        /// <returns>filtered data packet as UInt32 List</returns>
        public static List<UInt32> Filter24(List<UInt32> lastData)
        {
            List<UInt32> filtered = new List<UInt32>();
            if (lastData != null)
            {
                if (lastData.Count() > 0)
                {
                    // filter only the last packet
                    for (int i = lastData.Count() - SerialData.LenDataPacket; i < lastData.Count(); i++)
                    {
                        double data_add = lastData[i] * b[0];
                        // go through all the datapoints necessary for the filter
                        for (int j = 1; j < FilterSize; j++)    
                        {
                            // skip all the datapoints not belonging to the channel
                            int last_index = i - j * SerialData.NumChannels;
                            if (last_index > - 1 && last_index < lastData.Count())
                            {
                                data_add += lastData[last_index] * b[j];
                            }
                        }
                        filtered.Add((UInt32)data_add);
                    }
                }
            }
            return filtered;
        }
    }
}
