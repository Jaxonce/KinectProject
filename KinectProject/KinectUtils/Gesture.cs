﻿using Microsoft.Kinect;
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
        public Gesture(string gestureName, int mCurrentFrameCount) : base(gestureName)
        {
            this.mCurrentFrameCount = mCurrentFrameCount;
        }

        public override void TestGesture(Body body)
        {
            if (isRecognitionRunning)
            {
                if (TestEndConditions(body)) {
                    this.OnGestureReconized(body);
                    isRecognitionRunning = false;
                }
                else if ( !TestPosture(body) || !TestRunningGesture(body)){
                    isRecognitionRunning = false;
                }
                else if( TestPosture(body) && TestRunningGesture(body)) { 
                }
            }
            else
            {
                if ( TestInitialConditions(body))
                {
                    isRecognitionRunning = true;
                }
            }
            
        }

        protected abstract bool TestInitialConditions(Body body);

        protected abstract bool TestPosture(Body body);


        protected abstract bool TestRunningGesture(Body body);

        protected abstract bool TestEndConditions(Body body);

    }
}
