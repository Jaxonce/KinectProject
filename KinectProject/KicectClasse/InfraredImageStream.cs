using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicectClasse
{
    public partial class InfraredImageStream: KinectViewModel
    {
        public InfraredFrameReader IrFrameReader { get; set; }
        public InfraredImageStream(): base()
        {
            IrFrameReader = Manager.KinectSensor.InfraredFrameSource.OpenReader();
        }
    }
}
