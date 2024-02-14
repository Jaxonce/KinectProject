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
                KinectStream.BodyStream.stop(canva4body);
                if (KinectStream.BodyStream.bodyFrameReader != null)
                {
                    KinectStream.BodyStream.stop(canva4body);
                    KinectStream.BodyStream.bodyFrameReader = null;
                }
                canva.Source = null;
            }     
        }

        private void Button_Click_DepthFrame(object sender, RoutedEventArgs e)
        {
            if (KinectStream.DepthStream.DepthFrameReader == null)
            {
                KinectStream.ColorStream.start();
                canva.Source = KinectStream.DepthStream.DepthBitMap;
            }
            else
            {
                KinectStream.DepthStream.stop();
                KinectStream.DepthStream.DepthFrameReader = null;
                KinectStream.BodyStream.stop(canva4body);
                if (KinectStream.BodyStream.bodyFrameReader != null)
                {
                    KinectStream.BodyStream.stop(canva4body);
                    KinectStream.BodyStream.bodyFrameReader = null;
                }
                canva.Source = null;
            }
        }

        private void Button_Click_InfraredFrame(object sender, RoutedEventArgs e)
        {
            if (KinectStream.InfraredImageStream.InfraredFrameReader == null)
            {
                KinectStream.InfraredImageStream.start();
                canva.Source = KinectStream.InfraredImageStream.InfraredBitMap;
            }
            else
            {
                KinectStream.InfraredImageStream.stop();
                KinectStream.InfraredImageStream.InfraredFrameReader = null;
                if (KinectStream.BodyStream.bodyFrameReader != null)
                {
                    KinectStream.BodyStream.stop(canva4body);
                    KinectStream.BodyStream.bodyFrameReader = null;
                }
                canva.Source = null;
            }
        }

        private void Button_Click_BodyFrame(object sender, RoutedEventArgs e)
        {
            if (KinectStream.BodyStream.bodyFrameReader == null)
            {
                KinectStream.BodyStream.start(canva4body);
            }
            else
            {   
                KinectStream.BodyStream.stop(canva4body);
                KinectStream.BodyStream.bodyFrameReader = null;
            }
        }

        
    }
}
