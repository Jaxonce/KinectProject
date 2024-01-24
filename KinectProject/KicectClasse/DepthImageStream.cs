using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KicectClasse
{
    public partial class DepthImageStream: ObservableObject
    {
        public DepthFrameReader DepthFrameReader { get; set; }

        [ObservableProperty]
        private WriteableBitmap depthBitMap;

        private ushort[] depthFrameData;
        private byte[] depthPixels;


        public DepthImageStream(KinectManager Manager)
        {
            DepthFrameReader = Manager.KinectSensor.DepthFrameSource.OpenReader();
            DepthFrameReader.FrameArrived += DepthFrameReader_FrameArrived;
            FrameDescription depthFrameDescription = Manager.KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            this.DepthBitMap = new WriteableBitmap(depthFrameDescription.Width, depthFrameDescription.Height, 72, 72, PixelFormats.Bgra32, null);
            this.depthFrameData = new ushort[depthFrameDescription.Width * depthFrameDescription.Height];
            this.depthPixels = new byte[depthFrameDescription.Width * depthFrameDescription.Height * 4];
        }

        private void DepthFrameReader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            ushort minDepth = 0;
            ushort maxDepth = 0;

            bool depthFrameProcessed = false;

            // DepthFrame is IDisposable
            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
            {
                if (depthFrame != null)
                {
                    FrameDescription depthFrameDescription = depthFrame.FrameDescription;

                    // verify data and write the new depth frame data to the display bitmap
                    if (((depthFrameDescription.Width * depthFrameDescription.Height) == this.depthFrameData.Length) &&
                        (depthFrameDescription.Width == depthBitMap.PixelWidth) && (depthFrameDescription.Height == depthBitMap.PixelHeight))
                    {
                        // Copy the pixel data from the image to a temporary array
                        depthFrame.CopyFrameDataToArray(this.depthFrameData);

                        minDepth = depthFrame.DepthMinReliableDistance;

                        // Note: In order to see the full range of depth (including the less reliable far field depth)
                        // we are setting maxDepth to the extreme potential depth threshold
                        maxDepth = ushort.MaxValue;

                        // If you wish to filter by reliable depth distance, uncomment the following line:
                        //// maxDepth = depthFrame.DepthMaxReliableDistance

                        depthFrameProcessed = true;
                    }
                }
            }
            //// we got a frame, convert and render
            //if (depthFrameProcessed)
            //{
            //    ConvertDepthData(minDepth, maxDepth);
            //    pixels.CopyTo(this.bitmap.PixelBuffer);
            //    DepthBitMap.;
            //}
        }

        private void ConvertDepthData(ushort minDepth, ushort maxDepth)
        {
            int colorPixelIndex = 0;
            for (int i = 0; i < this.depthFrameData.Length; ++i)
            {
                // Get the depth for this pixel
                ushort depth = this.depthFrameData[i];

                // To convert to a byte, we're mapping the depth value to the byte range.
                // Values outside the reliable depth range are mapped to 0 (black).
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? (depth / (8000/256)) : 0);

                // Write out blue byte
                this.depthPixels[colorPixelIndex++] = intensity;

                // Write out green byte
                this.depthPixels[colorPixelIndex++] = intensity;

                // Write out red byte                        
                this.depthPixels[colorPixelIndex++] = intensity;

                // Write out alpha byte                        
                this.depthPixels[colorPixelIndex++] = 255;
            }
        }
    }
}
