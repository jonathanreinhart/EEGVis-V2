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
            7.38733376e-20, 3.90881184e-05, 8.23246028e-05, 1.32494273e-04,
            1.92516151e-04, 2.65405505e-04, 3.54233510e-04, 4.62085016e-04,
            5.92014953e-04, 7.47003923e-04, 9.29913544e-04, 1.14344214e-03,
            1.39008136e-03, 1.67207429e-03, 1.99137564e-03, 2.34961460e-03,
            2.74806082e-03, 3.18759402e-03, 3.66867774e-03, 4.19133760e-03,
            4.75514432e-03, 5.35920202e-03, 6.00214181e-03, 6.68212092e-03,
            7.39682756e-03, 8.14349132e-03, 8.91889931e-03, 9.71941774e-03,
            1.05410188e-02, 1.13793128e-02, 1.22295847e-02, 1.30868353e-02,
            1.39458264e-02, 1.48011288e-02, 1.56471743e-02, 1.64783088e-02,
            1.72888480e-02, 1.80731335e-02, 1.88255899e-02, 1.95407812e-02,
            2.02134663e-02, 2.08386535e-02, 2.14116519e-02, 2.19281204e-02,
            2.23841134e-02, 2.27761226e-02, 2.31011135e-02, 2.33565582e-02,
            2.35404620e-02, 2.36513847e-02, 2.36884560e-02, 2.36513847e-02,
            2.35404620e-02, 2.33565582e-02, 2.31011135e-02, 2.27761226e-02,
            2.23841134e-02, 2.19281204e-02, 2.14116519e-02, 2.08386535e-02,
            2.02134663e-02, 1.95407812e-02, 1.88255899e-02, 1.80731335e-02,
            1.72888480e-02, 1.64783088e-02, 1.56471743e-02, 1.48011288e-02,
            1.39458264e-02, 1.30868353e-02, 1.22295847e-02, 1.13793128e-02,
            1.05410188e-02, 9.71941774e-03, 8.91889931e-03, 8.14349132e-03,
            7.39682756e-03, 6.68212092e-03, 6.00214181e-03, 5.35920202e-03,
            4.75514432e-03, 4.19133760e-03, 3.66867774e-03, 3.18759402e-03,
            2.74806082e-03, 2.34961460e-03, 1.99137564e-03, 1.67207429e-03,
            1.39008136e-03, 1.14344214e-03, 9.29913544e-04, 7.47003923e-04,
            5.92014953e-04, 4.62085016e-04, 3.54233510e-04, 2.65405505e-04,
            1.92516151e-04, 1.32494273e-04, 8.23246028e-05, 3.90881184e-05,
            7.38733376e-20
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
