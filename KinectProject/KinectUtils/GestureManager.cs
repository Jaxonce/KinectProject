﻿using KicectClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace KinectUtils
{
    public static class GestureManager
    {

        public static EventHandler<GestureRecognizer> GestureReconized;

        public static void AddGestures(params BaseGesture[] gestures)
        {

        }

        public static void RemovesGesture(BaseGesture baseGesture) { }

        public static void StartAcquiringFrames(KinectManager kinectManager) { }

        public static void StopAcquiringFrame() { }
    }
}