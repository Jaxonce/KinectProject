using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace KicectClasse
{
    public abstract class BaseGesture
    {
        public event EventHandler<GestureRecognizer> GestureReconized;
        public String GestureName;

        public BaseGesture(EventHandler<GestureRecognizer> gestureReconized, string gestureName)
        {
            GestureReconized = gestureReconized;
            GestureName = gestureName;
        }

        public abstract void TestGesture(Body body);

        protected void OnGestureReconized(Body body) {
            GestureReconized.Invoke(this, new GestureRecognizer());
        }
    }
}
