using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KicectClasse;

namespace KinectProject
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KinectViewModel KinectStream;

        public MainWindow()
        {
            KinectStream = new KinectViewModel(canva4body);
            KinectStream.Manager.StartSensor();
            DataContext = KinectStream;
            InitializeComponent();
        }

        private void Button_Click_ColorFrame(object sender, RoutedEventArgs e)
        {
            if (KinectStream.ColorStream.ColorFrameReader == null)
            {
                KinectStream.ColorStream.start();
                canva.Source = KinectStream.ColorStream.ColorBitmap;
            } else
            {
                KinectStream.ColorStream.stop();
                KinectStream.ColorStream.ColorFrameReader = null;
                canva.Source = null;
            }     
        }

        private void Button_Click_DepthFrame(object sender, RoutedEventArgs e)
        {
            canva.Source = KinectStream.DepthStream.DepthBitMap;
        }

        private void Button_Click_BodyFrame(object sender, RoutedEventArgs e)
        {
            if (KinectStream.ColorStream.ColorFrameReader != null)
            {
                KinectStream.ColorStream.stop();
                KinectStream.ColorStream.ColorFrameReader = null;
                canva.Source = null;
            }
                KinectStream.BodyStream.start(canva4body);
                KinectStream.BodyStream.stop();
                KinectStream.ColorStream.ColorFrameReader = null;
                canva.Source = null;

            if (canva4body.Children.Count != 0)
            {
                //canva4body.Children.Add(KinectStream.BodyStream.DrawingCanvas);
            }
            
        }

        
    }
}
