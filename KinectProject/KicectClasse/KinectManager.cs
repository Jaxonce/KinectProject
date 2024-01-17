using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace KicectClasse
{
    public partial class KinectManager: ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusText))]  
        private bool status;
        public String StatusText => Status ? "Running" : "Kinect Sensor not available";

        public KinectSensor KinectSensor { get; set; }

        public ColorFrameReader ColorFrameReader { get; set; }

        public DepthFrameReader DepthFrameReader { get; set; }

        public InfraredFrameReader IrFrameReader { get; set; }

        private WriteableBitmap bitmap = null;

        private byte[] colorPixels = null;

        private readonly uint bytesPerPixel;



        public KinectManager()
        {
            KinectSensor = KinectSensor.GetDefault();
            KinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;

            List<Color> colors = new List<Color>();

            for (var i = 0; i < 256; i++)
                colors.Add(Color.FromRgb(i, i, i));

            FrameDescription colorFrameDescription = this.KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            this.bytesPerPixel = colorFrameDescription.BytesPerPixel;
            this.colorPixels = new byte[colorFrameDescription.Width * colorFrameDescription.Height * this.bytesPerPixel];
            this.bitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height,100, 100, colorFrameDescription.BytesPerPixel, new BitmapPalette());



            ColorFrameReader = KinectSensor.ColorFrameSource.OpenReader();
            DepthFrameReader = KinectSensor.DepthFrameSource.OpenReader();
            IrFrameReader = KinectSensor.InfraredFrameSource.OpenReader();
        }

        public void StartSensor()
        {
            KinectSensor?.Open();
        }

        public void StopSensor() {
            KinectSensor?.Close();
        }

        private void KinectSensor_IsAvailableChanged(Object sender, EventArgs args) {
            Status = KinectSensor.IsAvailable;
        }

        public ColorFrame GetColorFrame()
        {
            return ColorFrameReader.AcquireLatestFrame();
        }

        public DepthFrame GetDepthFrame()
        {
            return DepthFrameReader.AcquireLatestFrame();
        }

        public InfraredFrame GetIRFrame()
        {
            return IrFrameReader.AcquireLatestFrame();
        }

    }
}
