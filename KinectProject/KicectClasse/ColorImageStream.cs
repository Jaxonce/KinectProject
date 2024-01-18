using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicectClasse
{
    public partial class ColorImageStream: KinectStream
    {
        public ColorFrameReader ColorFrameReader { get; set; }
        public ColorImageStream(KinectManager manager) : base(manager)
        {
            ColorFrameReader = Manager.KinectSensor.ColorFrameSource.OpenReader();
        }
    }
}
