using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicectClasse
{
    public partial class DepthImageStream: KinectStream
    {
        public DepthFrameReader DepthFrameReader { get; set; }
        public DepthImageStream(KinectManager manager) :base(manager)
        {
            DepthFrameReader = Manager.KinectSensor.DepthFrameSource.OpenReader();
        }
    }
}
