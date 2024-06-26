﻿using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace KicectClasse
{
    public abstract class Posture : BaseGesture
    {
        public Posture(string gestureName) : base(gestureName)
        {
        }

        public override void TestGesture(Body body)
        {
            if (this.TestPosture(body)) {
                this.OnGestureReconized(body);
            }
        }

        protected abstract bool TestPosture(Body body);

    }
}
