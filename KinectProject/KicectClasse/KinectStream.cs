using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicectClasse
{
    public partial class KinectStream
    {
        public KinectManager Manager { get; private set; }  

        public KinectStream(KinectManager manager) {
            Manager = manager;
        }
    }
}
