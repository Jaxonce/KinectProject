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
            KinectStream = new KinectViewModel();
            KinectStream.Manager.StartSensor();
            DataContext = KinectStream;
            InitializeComponent();
        }

        private void Button_Click_ColorFrame(object sender, RoutedEventArgs e)
        {
            canva.Source = KinectStream.ColorStream.ColorBitmap;
        }

        private void Button_Click_DepthFrame(object sender, RoutedEventArgs e)
        {
            canva.Source = KinectStream.DepthStream.DepthBitMap;
        }

        private void Button_Click_BodyFrame(object sender, RoutedEventArgs e)
        {
            canva4body.Children.Add(KinectStream.BodyStream.DrawingCanvas);
        }

        
    }
}
