using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KicectClasse
{
    public partial class DepthImageStream: ImageStream
    {
        public DepthFrameReader DepthFrameReader { get; set; }

        [ObservableProperty]
        private WriteableBitmap depthBitMap;

        private byte[] depthPixels;

        private const int MapDepthToByte = 8000 / 256;

        private FrameDescription depthFrameDescription = null;

        public DepthImageStream(KinectManager Manager) : base(Manager)
        {
            this.depthFrameDescription = Manager.KinectSensor.DepthFrameSource.FrameDescription;
            this.DepthBitMap = new WriteableBitmap(depthFrameDescription.Width, depthFrameDescription.Height, 72, 72, PixelFormats.Bgra32, null);
            this.depthPixels = new byte[depthFrameDescription.Width * depthFrameDescription.Height];
        }

        public bool start()
        {
            DepthFrameReader = KinectManager.KinectSensor.DepthFrameSource.OpenReader();
            DepthFrameReader.FrameArrived += DepthFrameReader_FrameArrived;
            return true;
        }

        public void stop()
        {
            DepthFrameReader.FrameArrived -= DepthFrameReader_FrameArrived;
            DepthFrameReader.Dispose();
        }

        private void DepthFrameReader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            bool depthFrameProcessed = false;

            // DepthFrame is IDisposable
            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
            {
                if (depthFrame != null)
                {
                    // the fastest way to process the body index data is to directly access 
                    // the underlying buffer
                    using (Microsoft.Kinect.KinectBuffer depthBuffer = depthFrame.LockImageBuffer())
                    {
                        // verify data and write the color data to the display bitmap
                        if (((this.depthFrameDescription.Width * this.depthFrameDescription.Height) == (depthBuffer.Size / this.depthFrameDescription.BytesPerPixel)) &&
                            (this.depthFrameDescription.Width == DepthBitMap.PixelWidth) && (this.depthFrameDescription.Height == DepthBitMap.PixelHeight))
                        {
                            // Note: In order to see the full range of depth (including the less reliable far field depth)
                            // we are setting maxDepth to the extreme potential depth threshold
                            ushort maxDepth = ushort.MaxValue;

                            // If you wish to filter by reliable depth distance, uncomment the following line:
                            //// maxDepth = depthFrame.DepthMaxReliableDistance

                            this.ProcessDepthFrameData(depthBuffer.UnderlyingBuffer, depthBuffer.Size, depthFrame.DepthMinReliableDistance, maxDepth);
                            depthFrameProcessed = true;
                        }
                    }
                }
            }
            if (depthFrameProcessed)
            {
                this.RenderDepthPixels();
            }
        }

        private unsafe void ProcessDepthFrameData(IntPtr depthFrameData, uint depthFrameDataSize, ushort minDepth, ushort maxDepth)
        {
            // depth frame data is a 16 bit value
            ushort* frameData = (ushort*)depthFrameData;

            // convert depth to a visual representation
            for (int i = 0; i < (int)(depthFrameDataSize / this.depthFrameDescription.BytesPerPixel); ++i)
            {
                // Get the depth for this pixel
                ushort depth = frameData[i];

                // To convert to a byte, we're mapping the depth value to the byte range.
                // Values outside the reliable depth range are mapped to 0 (black).
                this.depthPixels[i] = (byte)(depth >= minDepth && depth <= maxDepth ? (depth / MapDepthToByte) : 0);
            }
        }

        private void RenderDepthPixels()
        {
            DepthBitMap.WritePixels(
                new Int32Rect(0, 0, DepthBitMap.PixelWidth, DepthBitMap.PixelHeight),
                this.depthPixels,
                DepthBitMap.PixelWidth,
                0);
        }
    }
}
