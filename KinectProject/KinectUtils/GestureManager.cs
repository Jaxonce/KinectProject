﻿using KicectClasse;
using Microsoft.Kinect;
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

        public static event EventHandler<GestureRecognizer> GestureReconized
        {
            add
            {
                foreach (var item in KnowGesture)
                {
                    item.GestureReconized += Item_GestureReconized;
                }
            }

            remove
            {
                foreach (var item in KnowGesture)
                {
                    item.GestureReconized -= Item_GestureReconized;
                }
            }
        }

        private static void Item_GestureReconized(object sender, GestureRecognizer e)
        {

        }

        private static KinectViewModel KinectViewModel { get; set; }

        public static BaseGesture[] KnowGesture {  get; set; }

        private static IGestureFactory Factory { get; set; }

        public static void AddGestures(params BaseGesture[] gestures)
        {
            KnowGesture = gestures;
        }

        public static void AddGestures(IGestureFactory factory)
        {
            Factory = factory;
        }

        public static void RemovesGesture(BaseGesture baseGesture) 
        {
            int indexToRemove = -1;
            for (int i = 0; KnowGesture.Length > i; i++)
            {
                if (KnowGesture[i] == baseGesture)
                {
                    indexToRemove = i;
                }
            }
            if (indexToRemove !=  -1)
            {
                KnowGesture = KnowGesture.Where((source, index) => index != indexToRemove).ToArray();
            }
        }

        public static void StartAcquiringFrames(KinectViewModel kinectViewModel) 
        {
            KinectViewModel = kinectViewModel;
            KinectViewModel.Manager.KinectSensor.BodyFrameSource.OpenReader();
        }

        public static void StopAcquiringFrame() { }
    }
}
