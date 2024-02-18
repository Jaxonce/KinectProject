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
    public class GestureRightSlap : Gesture
    {
        public GestureRightSlap(string gestureName, int mCurrentFrameCount) : base(gestureName, mCurrentFrameCount)
        {
        }

        protected override bool TestEndConditions(Body body)
        {
            if(body.IsTracked)
            {
                if (body.Joints[JointType.HandRight].TrackingState == TrackingState.Tracked)
                {
                    if(body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.ShoulderLeft].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.AnkleLeft].Position.Y && body.HandRightState == HandState.Open && body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderLeft].Position.X) {  
                        return true;
                    }
                }
            }
            return false;
        }

        protected override bool TestInitialConditions(Body body)
        {
            if (body.IsTracked)
            {
                if (body.Joints[JointType.HandRight].TrackingState == TrackingState.Tracked)
                {
                    if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.ShoulderRight].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.AnkleRight].Position.Y && body.HandRightState == HandState.Open && body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override bool TestPosture(Body body)
        {
            if (body.IsTracked)
            {
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.ShoulderRight].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.AnkleRight].Position.Y && body.HandRightState == HandState.Open && body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X+0.2 && body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderLeft].Position.X-0.2)
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool TestRunningGesture(Body body)
        {
            return false;
        }
    }
}
