using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace KicectClasse
{
    public partial class KinectViewModel : ObservableObject
    {
        public KinectManager Manager { get; private set; }
        
        public ColorImageStream ColorStream { get; private set; }

        public DepthImageStream DepthStream { get; private set; }

        public KinectViewModel() {
            Manager = new KinectManager();
            ColorStream = new ColorImageStream(Manager);
            //DepthStream = new DepthImageStream(Manager);
        }
    }
}
