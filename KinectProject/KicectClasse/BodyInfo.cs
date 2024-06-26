﻿using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace KicectClasse
{
    /// <summary>
    /// BodyInfo class that contains joint ellipses, handstate ellipses, lines for bones between two joints.
    /// </summary>
    internal class BodyInfo
    {
        public bool Updated { get; set; }

        public Color BodyColor { get; set; }

        // ellipse representing left handstate
        public Ellipse HandLeftEllipse { get; set; }

        // ellipse representing right handstate
        public Ellipse HandRightEllipse { get; set; }

        // dictionary of all joints in a body
        public Dictionary<JointType, Ellipse> JointPoints { get; private set; }

        // definition of bones
        public TupleList<JointType, JointType> Bones { get; private set; }

        // collection of bones associated with the line object
        public Dictionary<Tuple<JointType, JointType>, Line> BoneLines { get; private set; }

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 8.0;

        public BodyInfo(Color bodyColor)
        {
            this.BodyColor = bodyColor;

            // create hand state ellipses
            this.HandLeftEllipse = new Ellipse()
            {
                Visibility = Visibility.Collapsed
            };

            this.HandRightEllipse = new Ellipse()
            {
                Visibility = Visibility.Collapsed
            };

            // a joint defined as a jointType with a point location in XY space represented by an ellipse
            this.JointPoints = new Dictionary<JointType, Ellipse>();

            // pre-populate list of joints and set to non-visible initially
            foreach (JointType jointType in Enum.GetValues(typeof(JointType)))
            {
                this.JointPoints.Add(jointType, new Ellipse()
                {
                    Visibility = Visibility.Collapsed,
                    Fill = new SolidColorBrush(BodyColor),
                    Width = JointThickness,
                    Height = JointThickness
                });
            }

            // collection of bones
            this.BoneLines = new Dictionary<Tuple<JointType, JointType>, Line>();

            // a bone defined as a line between two joints
            this.Bones = new TupleList<JointType, JointType>
                {
                    // Torso
                    { JointType.Head, JointType.Neck },
                    { JointType.Neck, JointType.SpineShoulder },
                    { JointType.SpineShoulder, JointType.SpineMid },
                    { JointType.SpineMid, JointType.SpineBase },
                    { JointType.SpineShoulder, JointType.ShoulderRight },
                    { JointType.SpineShoulder, JointType.ShoulderLeft },
                    { JointType.SpineBase, JointType.HipRight },
                    { JointType.SpineBase, JointType.HipLeft },

                    // Right Arm
                    { JointType.ShoulderRight, JointType.ElbowRight },
                    { JointType.ElbowRight, JointType.WristRight },
                    { JointType.WristRight, JointType.HandRight },
                    { JointType.HandRight, JointType.HandTipRight },
                    { JointType.WristRight, JointType.ThumbRight },

                    // Left Arm
                    { JointType.ShoulderLeft, JointType.ElbowLeft },
                    { JointType.ElbowLeft, JointType.WristLeft },
                    { JointType.WristLeft, JointType.HandLeft },
                    { JointType.HandLeft, JointType.HandTipLeft },
                    { JointType.WristLeft, JointType.ThumbLeft },

                    // Right Leg
                    { JointType.HipRight, JointType.KneeRight },
                    { JointType.KneeRight, JointType.AnkleRight },
                    { JointType.AnkleRight, JointType.FootRight },
                
                    // Left Leg
                    { JointType.HipLeft, JointType.KneeLeft },
                    { JointType.KneeLeft, JointType.AnkleLeft },
                    { JointType.AnkleLeft, JointType.FootLeft },
                };

            // pre-populate list of bones that are non-visible initially
            foreach (var bone in this.Bones)
            {
                this.BoneLines.Add(bone, new Line()
                {
                    Stroke = new SolidColorBrush(BodyColor),
                    Visibility = Visibility.Collapsed
                });
            }
        }
    }
}
