using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VisualControlLib;

namespace UAVSensorControl
{
    public class CopterData
    {
        public class FixedSizedQueue<T> : ConcurrentQueue<T>
        {
            public int Size { get; private set; }

            public FixedSizedQueue(int size)
            {
                Size = size;
            }

            public new void Enqueue(T obj)
            {
                base.Enqueue(obj);
                lock (this)
                {
                    while (base.Count > Size)
                    {
                        T outObj;
                        base.TryDequeue(out outObj);
                    }
                }
            }
        }

        private int maxSize;

        public FixedSizedQueue<KeyValuePair<DateTime, OSDDataLib.OSDData>> osdBuffer;

        public FixedSizedQueue<KeyValuePair<DateTime, str_RadarData>> radarBuffer;

        public FixedSizedQueue<KeyValuePair<DateTime, str_VisualData>> visualBuffer;

        public FixedSizedQueue<KeyValuePair<DateTime, str_VisualData>> visualBuffer2;

        public CopterData(int maxSize)
       {
           this.maxSize = maxSize;
           osdBuffer = new FixedSizedQueue<KeyValuePair<DateTime, OSDDataLib.OSDData>>(maxSize);
           radarBuffer = new FixedSizedQueue<KeyValuePair<DateTime, str_RadarData>>(maxSize);
           visualBuffer = new FixedSizedQueue<KeyValuePair<DateTime, str_VisualData>>(maxSize);
           visualBuffer2 = new FixedSizedQueue<KeyValuePair<DateTime, str_VisualData>>(maxSize);
       }

        public void resetOSDBuffer()
        {
            osdBuffer = new FixedSizedQueue<KeyValuePair<DateTime, OSDDataLib.OSDData>>(maxSize);
        }

        public void resetRadarBuffer()
        {
            radarBuffer = new FixedSizedQueue<KeyValuePair<DateTime, str_RadarData>>(maxSize);
        }

        public void resetvisualBuffer()
        {
            visualBuffer = new FixedSizedQueue<KeyValuePair<DateTime, str_VisualData>>(maxSize);
        }

        public void resetvisualBuffer2()
        {
            visualBuffer2 = new FixedSizedQueue<KeyValuePair<DateTime, str_VisualData>>(maxSize);
        }


    }
}
