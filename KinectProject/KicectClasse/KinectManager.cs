using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KicectClasse
{
    internal partial class KinectManager: ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusText))]  
        private bool status;
        public String StatusText => status ? "Running" : "Kinect Sensor not available";

        public KinectSensor kinectSensor { get; set; }

        public KinectManager()
        {
            kinectSensor = KinectSensor.GetDefault();
            kinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
        }

        public void StartSensor()
        {
            kinectSensor?.Open();
        }

        public void StopSensor() {
            kinectSensor?.Close();
        }

        private void KinectSensor_IsAvailableChanged(Object sender, EventArgs args) {
            status = kinectSensor.IsAvailable;
        }

    }
}
