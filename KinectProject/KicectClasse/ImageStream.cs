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
    public partial class ImageStream : ObservableObject
    {

        public KinectManager KinectManager { get; set; }

        public ImageStream(KinectManager Manager)
        {
            KinectManager = Manager;
        }
    }
}
