using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace KicectClasse
{
    public abstract class Gesture : BaseGesture
    {
        public Boolean isRecognitionRunning = false;

        public static int MinNbOffFrames;

        public static int MaxNbOffFrames;

        int mCurrentFrameCount;
        public Gesture(EventHandler<GestureRecognizer> gestureReconized, string gestureName, int mCurrentFrameCount) : base(gestureReconized, gestureName)
        {
            this.mCurrentFrameCount = mCurrentFrameCount;
        }

        public override void TestGesture(Body body)
        {

        }

         protected bool TestInitialConditions(Body body)
        {
            return false;
        }

        protected bool TestPosture(Body body)
        {
            return false;
        }

        protected bool TestRunningGesture(Body body)
        {
            return false;
        }

        protected bool TestEndConditions(Body body)
        {
            return false;
        }

    }
}
