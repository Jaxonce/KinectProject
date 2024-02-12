using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KicectClasse
{
    public partial class InfraredImageStream: ImageStream
    {
        private const float InfraredSourceValueMaximum = (float)ushort.MaxValue;

        private const float InfraredSourceScale = 0.75f;

        private const float InfraredOutputValueMinimum = 0.01f;

        private const float InfraredOutputValueMaximum = 1.0f;

        private FrameDescription infraredFrameDescription;

        public InfraredFrameReader InfraredFrameReader { get; set; }

        [ObservableProperty]
        private WriteableBitmap infraredBitMap;
        public InfraredImageStream(KinectManager Manager) : base(Manager)
        {
            this.infraredFrameDescription = Manager.KinectSensor.InfraredFrameSource.FrameDescription;
            this.infraredBitMap = new WriteableBitmap(this.infraredFrameDescription.Width, this.infraredFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray32Float, null);
        }
        public bool start()
        {
            InfraredFrameReader = KinectManager.KinectSensor.InfraredFrameSource.OpenReader();
            InfraredFrameReader.FrameArrived += Reader_InfraredFrameArrived;
            return true;
        }

        public void stop()
        {
            InfraredFrameReader.FrameArrived -= Reader_InfraredFrameArrived;
            InfraredFrameReader.Dispose();
        }

        private void Reader_InfraredFrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            // InfraredFrame is IDisposable
            using (InfraredFrame infraredFrame = e.FrameReference.AcquireFrame())
            {
                if (infraredFrame != null)
                {
                    // the fastest way to process the infrared frame data is to directly access 
                    // the underlying buffer
                    using (Microsoft.Kinect.KinectBuffer infraredBuffer = infraredFrame.LockImageBuffer())
                    {
                        // verify data and write the new infrared frame data to the display bitmap
                        if (((this.infraredFrameDescription.Width * infraredFrameDescription.Height) == (infraredBuffer.Size / this.infraredFrameDescription.BytesPerPixel)) &&
                            (this.infraredFrameDescription.Width == InfraredBitMap.PixelWidth) && (this.infraredFrameDescription.Height == InfraredBitMap.PixelHeight))
                        {
                            this.ProcessInfraredFrameData(infraredBuffer.UnderlyingBuffer, infraredBuffer.Size);
                        }
                    }
                }
            }
        }

        private unsafe void ProcessInfraredFrameData(IntPtr infraredFrameData, uint infraredFrameDataSize)
        {
            // infrared frame data is a 16 bit value
            ushort* frameData = (ushort*)infraredFrameData;

            // lock the target bitmap
            InfraredBitMap.Lock();

            // get the pointer to the bitmap's back buffer
            float* backBuffer = (float*)this.InfraredBitMap.BackBuffer;

            // process the infrared data
            for (int i = 0; i < (int)(infraredFrameDataSize / this.infraredFrameDescription.BytesPerPixel); ++i)
            {
                // since we are displaying the image as a normalized grey scale image, we need to convert from
                // the ushort data (as provided by the InfraredFrame) to a value from [InfraredOutputValueMinimum, InfraredOutputValueMaximum]
                backBuffer[i] = Math.Min(InfraredOutputValueMaximum, (((float)frameData[i] / InfraredSourceValueMaximum * InfraredSourceScale) * (1.0f - InfraredOutputValueMinimum)) + InfraredOutputValueMinimum);
            }

            // mark the entire bitmap as needing to be drawn
            InfraredBitMap.AddDirtyRect(new Int32Rect(0, 0, InfraredBitMap.PixelWidth, InfraredBitMap.PixelHeight));

            // unlock the bitmap
            InfraredBitMap.Unlock();
        }
    }
}
