using KicectClasse;
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

        public static event EventHandler<GestureRecognizer> GestureReconized;
        //{
        //    add
        //    {
        //        foreach (var item in KnowGesture)
        //        {
        //            item.GestureReconized += Item_GestureReconized;
        //        }
        //    }

        //    remove
        //    {
        //        foreach (var item in KnowGesture)
        //        {
        //            item.GestureReconized -= Item_GestureReconized;
        //        }
        //    }
        //}

        private static KinectManager KinectManager { get; set; }

        public static BaseGesture[] KnowGesture {  get; set; }

        private static IGestureFactory Factory { get; set; }

        private static BodyFrameReader BodyFrameReader { get; set; }

        public static void AddGestures(params BaseGesture[] gestures)
        {
            KnowGesture = gestures;

            // Souscrire à l'événement GestureReconized de chaque geste
            foreach (var gesture in KnowGesture)
            {
                gesture.GestureReconized += (sender, e) =>
                {
                    // Propager l'événement GestureReconized de BaseGesture
                    GestureReconized?.Invoke(sender, e);
                };
            }
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

        public static void StartAcquiringFrames(KinectManager kinectManager) 
        {
            KinectManager = kinectManager;
            BodyFrameReader = KinectManager.KinectSensor.BodyFrameSource.OpenReader();
            BodyFrameReader.FrameArrived += BodyFrameReader_FrameArrived;
        }

        public static void StopAcquiringFrame() 
        {
            if (BodyFrameReader != null)
            {
                BodyFrameReader.FrameArrived -= BodyFrameReader_FrameArrived;
                BodyFrameReader.Dispose();
                BodyFrameReader = null;
            }
        }

        private static void BodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {

            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame == null)
                    return;

                // Traitement du cadre du corps ici
                Body[] bodies = new Body[bodyFrame.BodyCount];
                bodyFrame.GetAndRefreshBodyData(bodies);


                foreach (var body in bodies)
                {
                    if (body.IsTracked)
                    {
                        foreach (var gesture in KnowGesture)
                        {
                            gesture.TestGesture(body);
                        }
                    }
                }
            }
        }
    }
}
