using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace KicectClasse
{
    public partial class KinectManager: ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusText))]  
        private bool status;
        public String StatusText => Status ? "Running" : "Kinect Sensor not available";

        public KinectSensor KinectSensor { get; set; }

        public KinectManager()
        {
            KinectSensor = KinectSensor.GetDefault();
            KinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
        }

        public void StartSensor()
        {
            KinectSensor?.Open();
        }

        public void StopSensor() {
            KinectSensor?.Close();
        }

        private void KinectSensor_IsAvailableChanged(Object sender, EventArgs args) {
            Status = KinectSensor.IsAvailable;
        }
    }
}
