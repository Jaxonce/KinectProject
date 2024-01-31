using KicectClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace KinectUtils
{
    public class GestureManager
    {
        public BaseGesture KnowGestures { get; set; }
        KinectManager KinectManager { get; set; }

        public EventHandler<GestureRecognizer> GestureReconized;

        public static void AddGestures(params BaseGesture[] gestures)
        {

        }

        public static void RemovesGesture(BaseGesture baseGesture) { }

        public static void StartAcquiringFrames(KinectManager kinectManager) { }

        public static void StopAcquiringFrame() { }
    }
}
