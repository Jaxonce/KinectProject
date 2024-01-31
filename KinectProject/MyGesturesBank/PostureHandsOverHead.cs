using KicectClasse;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace MyGesturesBank
{
    public class PostureHandsOverHead : Posture
    {
        public PostureHandsOverHead(EventHandler<GestureRecognizer> gestureReconized, string gestureName) : base(gestureReconized, gestureName)
        {
        }

        public static bool HandsOverHead(Body body)
        {
            if (body.IsTracked)
            {
                if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.Head].Position.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
