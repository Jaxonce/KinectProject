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
    public class PostureRightHandUp : Posture
    {
        public PostureRightHandUp(string gestureName) : base(gestureName)
        {
        }

        protected override bool TestPosture(Body body)
        {
            if (body.IsTracked)
            {
                if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.Head].Position.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
