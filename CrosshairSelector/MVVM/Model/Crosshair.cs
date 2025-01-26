using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Runtime.Serialization;
using System.ComponentModel.Design;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Input;


namespace CrosshairSelector
{
    [DataContract]
    public class Crosshair : ICrosshair, ICloneable
    {
        #region Properties
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Key AssignedKey { get; set; }

        [DataMember]
        public int Thickness { get; set; }

        [DataMember]
        public int Opacity { get; set; }

        [DataMember]
        public int Gap { get; set; }

        [DataMember]
        public int Size { get; set; }

        [DataMember]
        public int OutlineOpacity { get; set; }


        [DataMember]
        public bool CenterDot { get; set; }


        [DataMember]
        public int OutlineThickness { get; set; }

        [DataMember]
        public bool Outline { get; set; }

        [DataMember]
        public CrosshairShape Shape { get; set; }

        [DataMember]
        public Color CrosshairColor { get; set; }

        [DataMember]
        public Color OutlineColor { get; set; }
        public ICrosshairView View { get; set; }
        #endregion // Properties

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Crosshair()
        {
            Name = "";
            AssignedKey = Key.None;
            CenterDot = false;
            Outline = false;
            View = new CrossView();
            Shape = CrosshairShape.Cross;
            Thickness = 1;
            Opacity = 255;
            Gap = 1;
            Size = 1;
            OutlineOpacity = 255;
            OutlineThickness = 1;
        }
        #endregion // Default Constructor

        #region ICrosshair interface implementation
        public void ModifyCrossView(ICrosshair crosshair)
        {
            Size = crosshair.Size == 0 ? 1 : crosshair.Size;
            Opacity = crosshair.Opacity == 0 ? 255 : crosshair.Opacity;
            Thickness = crosshair.Thickness == 0 ? 1 : crosshair.Thickness;
            CenterDot = crosshair.CenterDot;
            OutlineColor = crosshair.OutlineColor;
            CrosshairColor = crosshair.CrosshairColor;
            OutlineThickness = crosshair.OutlineThickness == 0 ? 1 : crosshair.OutlineThickness;
            Outline = crosshair.Outline;
            AssignedKey = crosshair.AssignedKey;
            View.Modify(this);
        }
        public void ChangeCrosshairShape(CrosshairShape newShape)
        {
            switch (newShape)
            {
                case CrosshairShape.Cross:
                    Shape = CrosshairShape.Cross;
                    View = new CrossView();
                    break;
                case CrosshairShape.Cross2:
                    Shape = CrosshairShape.Cross2;
                    View = new Cross2View();
                    break;
                case CrosshairShape.Triangle:
                    Shape = CrosshairShape.Triangle;
                    View = new TriangularCrossView();
                    break;
                case CrosshairShape.Circle: 
                    Shape = CrosshairShape.Circle;
                    View = new CircleView();
                    break;
                case CrosshairShape.X:
                    Shape = CrosshairShape.X;
                    View = new XCrossView();
                    break;
                default:
                    break;
            }
        }
        #endregion // ICrosshair interface implementation

        #region ICloneable implementation
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion // ICloneable implementation

    }

}
