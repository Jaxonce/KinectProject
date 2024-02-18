using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KicectClasse
{
    public partial class BodyStream : ImageStream
    {
        private CoordinateMapper coordinateMapper = null;

        public BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Radius of drawn hand circles
        /// </summary>
        private const double HighConfidenceHandSize = 40;

        /// <summary>
        /// Radius of drawn hand circles
        /// </summary>
        private const double LowConfidenceHandSize = 20;

        /// <summary>
        /// Thickness of seen bone lines
        /// </summary>
        private const double TrackedBoneThickness = 4.0;

        /// <summary>
        /// Thickness of inferred joint lines
        /// </summary>
        private const double InferredBoneThickness = 1.0;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 8.0;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 5;

        /// <summary>
        /// Constant for clamping Z values of camera space points from being negative
        /// </summary>
        private const float InferredZPositionClamp = 0.1f;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        private Body[] bodies { get; }

        /// <summary>
        /// List of BodyInfo objects for each potential body
        /// </summary>
        private BodyInfo[] BodyInfos;

        /// <summary>
        /// List of colors for each body tracked
        /// </summary>
        private List<Color> BodyColors;

        private int BodyCount
        {
            set
            {
                if (value == 0)
                {
                    this.BodyInfos = null;
                    return;
                }

                // creates instances of BodyInfo objects for potential number of bodies
                if (this.BodyInfos == null || this.BodyInfos.Length != value)
                {
                    this.BodyInfos = new BodyInfo[value];

                    for (int bodyIndex = 0; bodyIndex < this.bodies.Length; bodyIndex++)
                    {
                        this.BodyInfos[bodyIndex] = new BodyInfo(this.BodyColors[bodyIndex]);
                    }
                }
            }

            get { return this.BodyInfos == null ? 0 : this.BodyInfos.Length; }
        }

        private float JointSpaceWidth { get; set; }

        private float JointSpaceHeight { get; set; }

        [ObservableProperty]
        private Canvas drawingCanvas;
        public BodyStream(KinectManager Manager, Canvas Canva) : base(Manager)
        {
            this.coordinateMapper = Manager.KinectSensor.CoordinateMapper;

            // one sensor is currently supported
            Manager.KinectSensor = KinectSensor.GetDefault();

            // get the coordinate mapper
            this.coordinateMapper = Manager.KinectSensor.CoordinateMapper;

            // get the depth (display) extents
            //FrameDescription frameDescription = Manager.KinectSensor.DepthFrameSource.FrameDescription;

            // get size of joint space
            //this.JointSpaceWidth = frameDescription.Width;
            //this.JointSpaceHeight = frameDescription.Height;

            // get total number of bodies from BodyFrameSource
            this.bodies = new Body[Manager.KinectSensor.BodyFrameSource.BodyCount];



            // populate body colors, one for each BodyIndex
            this.BodyColors = new List<Color>
            {
                Colors.Red,
                Colors.Orange,
                Colors.Green,
                Colors.Blue,
                Colors.Indigo,
                Colors.Violet
            };

            // sets total number of possible tracked bodies
            // create ellipses and lines for drawing bodies
            this.BodyCount = Manager.KinectSensor.BodyFrameSource.BodyCount;

            // Instantiate a new Canvas
           // this.DrawingCanvas = new Canvas();

            // set the clip rectangle to prevent rendering outside the canvas
           // this.DrawingCanvas.Clip = new RectangleGeometry();
        }

        public void start(Canvas canvas)
        {
            FrameDescription frameDescription = KinectManager.KinectSensor.DepthFrameSource.FrameDescription;
            canvas.Width = frameDescription.Width;
            canvas.Height = frameDescription.Height;

            // open the reader for the body frames
            this.bodyFrameReader = KinectManager.KinectSensor.BodyFrameSource.OpenReader();

            // wire handler for frame arrival
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;


            foreach (var bodyInfo in this.BodyInfos)
            {

                // add joint ellipses of all bodies to canvas
                foreach (var joint in bodyInfo.JointPoints)
                {
                    canvas.Children.Add(joint.Value);
                }

                // add bone lines of all bodies to canvas
                foreach (var bone in bodyInfo.Bones)
                {
                    canvas.Children.Add(bodyInfo.BoneLines[bone]);
                }
            }

        }

        public void stop(Canvas canvas)
        {
            this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
            this.bodyFrameReader.Dispose();
            canvas.Children.Clear();
        }

        /// <summary>
        /// Handles the body frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;
            bool hasTrackedBody = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                this.BeginBodiesUpdate();

                // iterate through each body
                for (int bodyIndex = 0; bodyIndex < this.bodies.Length; bodyIndex++)
                {
                    Body body = this.bodies[bodyIndex];

                    if (body.IsTracked)
                    {
                        this.UpdateBody(body, bodyIndex);

                        hasTrackedBody = true;
                    }
                    else
                    {
                        // collapse this body from canvas as it goes out of view
                        this.ClearBody(bodyIndex);
                    }
                }
            }
        }

        /// <summary>
        /// Clear update status of all bodies
        /// </summary>
        internal void BeginBodiesUpdate()
        {
            if (this.BodyInfos != null)
            {
                foreach (var bodyInfo in this.BodyInfos)
                {
                    bodyInfo.Updated = false;
                }
            }
        }

        /// <summary>
        /// Update body data for each body that is tracked.
        /// </summary>
        /// <param name="body">body for getting joint info</param>
        /// <param name="bodyIndex">index for body we are currently updating</param>
        internal void UpdateBody(Body body, int bodyIndex)
        {
            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
            var jointPointsInDepthSpace = new Dictionary<JointType, Point>();

            var bodyInfo = this.BodyInfos[bodyIndex];

            CoordinateMapper coordinateMapper = KinectManager.KinectSensor.CoordinateMapper;

            // update all joints
            foreach (var jointType in body.Joints.Keys)
            {
                // sometimes the depth(Z) of an inferred joint may show as negative
                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                CameraSpacePoint position = body.Joints[jointType].Position;
                if (position.Z < 0)
                {
                    position.Z = InferredZPositionClamp;
                }

                // map joint position to depth space
                DepthSpacePoint depthSpacePoint = coordinateMapper.MapCameraPointToDepthSpace(position);
                jointPointsInDepthSpace[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);

                // modify the joint's visibility and location
                this.UpdateJoint(bodyInfo.JointPoints[jointType], joints[jointType], jointPointsInDepthSpace[jointType]);
            }
            //Debug.WriteLine(jointPointsInDepthSpace[JointType.HandRight].X);

            // update all bones
            foreach (var bone in bodyInfo.Bones)
            {
                this.UpdateBone(bodyInfo.BoneLines[bone], joints[bone.Item1], joints[bone.Item2],
                                jointPointsInDepthSpace[bone.Item1],
                                jointPointsInDepthSpace[bone.Item2]);
            }
        }

        /// <summary>
        /// Collapse the body from the canvas.
        /// </summary>
        /// <param name="bodyIndex"></param>
        private void ClearBody(int bodyIndex)
        {
            var bodyInfo = this.BodyInfos[bodyIndex];

            // collapse all joint ellipses
            foreach (var joint in bodyInfo.JointPoints)
            {
                joint.Value.Visibility = System.Windows.Visibility.Collapsed;
            }

            // collapse all bone lines
            foreach (var bone in bodyInfo.Bones)
            {
                bodyInfo.BoneLines[bone].Visibility = Visibility.Collapsed;
            }

            // collapse handstate ellipses
            bodyInfo.HandLeftEllipse.Visibility = Visibility.Collapsed;

            bodyInfo.HandRightEllipse.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Update a joint.
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="joint"></param>
        /// <param name="point"></param>
        private void UpdateJoint(Ellipse ellipse, Joint joint, Point point)
        {
            TrackingState trackingState = joint.TrackingState;

            // only draw if joint is tracked or inferred
            if (trackingState != TrackingState.NotTracked)
            {
                if (trackingState == TrackingState.Tracked)
                {
                    ellipse.Fill = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    // inferred joints are yellow
                    ellipse.Fill = new SolidColorBrush(Colors.Yellow);
                }

                Canvas.SetLeft(ellipse, point.X - JointThickness / 2);
                Canvas.SetTop(ellipse, point.Y - JointThickness / 2);

                ellipse.Visibility = Visibility.Visible;
            }
            else
            {
                ellipse.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Update a bone line.
        /// </summary>
        /// <param name="line">line representing a bone line</param>
        /// <param name="startJoint">start joint of bone line</param>
        /// <param name="endJoint">end joint of bone line</param>
        /// <param name="startPoint">location of start joint</param>
        /// <param name="endPoint">location of end joint</param>
        private void UpdateBone(Line line, Joint startJoint, Joint endJoint, Point startPoint, Point endPoint)
        {
            // don't draw if neither joints are tracked
            if (startJoint.TrackingState == TrackingState.NotTracked || endJoint.TrackingState == TrackingState.NotTracked)
            {
                line.Visibility = Visibility.Collapsed;
                return;
            }

            // all lines are inferred thickness unless both joints are tracked
            line.StrokeThickness = InferredBoneThickness;

            if (startJoint.TrackingState == TrackingState.Tracked &&
                endJoint.TrackingState == TrackingState.Tracked)
            {
                line.StrokeThickness = TrackedBoneThickness;
            }

            line.Visibility = Visibility.Visible;

            line.X1 = startPoint.X;
            line.Y1 = startPoint.Y;
            line.X2 = endPoint.X;
            line.Y2 = endPoint.Y;
        }

        /// <summary>
        /// Instantiate new objects for joints, bone lines, and clipped edge rectangles
        /// </summary>
        private void PopulateVisualObjects()
        {

            foreach (var bodyInfo in this.BodyInfos)
            {

                // add joint ellipses of all bodies to canvas
                foreach (var joint in bodyInfo.JointPoints)
                {
                    this.DrawingCanvas.Children.Add(joint.Value);
                }

                // add bone lines of all bodies to canvas
                foreach (var bone in bodyInfo.Bones)
                {
                    this.DrawingCanvas.Children.Add(bodyInfo.BoneLines[bone]);
                }
            }

        }
    }
}
