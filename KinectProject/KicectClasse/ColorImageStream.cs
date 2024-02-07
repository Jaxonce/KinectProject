using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KicectClasse
{
    public partial class ColorImageStream : ImageStream
    {
        public ColorFrameReader ColorFrameReader { get; set; }

        [ObservableProperty]
        private WriteableBitmap colorBitmap;
        public ColorImageStream(KinectManager Manager):base(Manager)
        {
            FrameDescription colorFrameDescription = Manager.KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            this.ColorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 72, 72, PixelFormats.Bgra32, null);
        }

        public bool start()
        {
            ColorFrameReader = KinectManager.KinectSensor.ColorFrameSource.OpenReader();
            ColorFrameReader.FrameArrived += ColorFrameReader_FrameArrived;
            return true;
        }

        public void stop()
        {
            ColorFrameReader.FrameArrived -= ColorFrameReader_FrameArrived;
            ColorFrameReader.Dispose(); 
        }

        private void ColorFrameReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            
            // Récupérer le frame couleur
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame()) { 
                if (colorFrame != null) 
                { // Convertir le frame en format Bgra
                  colorFrame.CopyConvertedFrameDataToIntPtr( ColorBitmap.BackBuffer, (uint)(ColorBitmap.PixelWidth * ColorBitmap.PixelHeight * 4), ColorImageFormat.Bgra); 
                  // Mettre à jour la bitmap
                  ColorBitmap.Lock(); 
                  ColorBitmap.AddDirtyRect(new Int32Rect(0, 0, ColorBitmap.PixelWidth, ColorBitmap.PixelHeight)); ColorBitmap.Unlock(); } } }

        }
    }
