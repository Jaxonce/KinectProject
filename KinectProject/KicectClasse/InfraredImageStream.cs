using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicectClasse
{
    public partial class InfraredImageStream: KinectStream
    {
        public InfraredFrameReader IrFrameReader { get; set; }
        public InfraredImageStream(KinectManager manager): base(manager)
        {
            IrFrameReader = Manager.KinectSensor.InfraredFrameSource.OpenReader();
        }
    }
}
