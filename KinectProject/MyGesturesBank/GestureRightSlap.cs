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
        public GestureRightSlap(EventHandler<GestureRecognizer> gestureReconized, string gestureName, int mCurrentFrameCount) : base(gestureReconized, gestureName, mCurrentFrameCount)
        {
        }

        protected override bool TestEndConditions(Body body)
        {
            if(body.IsTracked)
            {
                if(body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.ShoulderLeft].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.AnkleLeft].Position.Y) {   // Verifier que la main est ouverte
                    return true;
                }
            }
            return false;
        }

        protected override bool TestInitialConditions(Body body)
        {
            if (body.IsTracked)
            {
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.ShoulderRight].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.AnkleRight].Position.Y)
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool TestPosture(Body body)
        {
            throw new NotImplementedException(); // Verifie s'il est bien entre l'epaule et la hanche
        }

        protected override bool TestRunningGesture(Body body)
        {
            throw new NotImplementedException(); // Verifie si la main avance bien par rapport au frame d'avant
        }
    }
}
