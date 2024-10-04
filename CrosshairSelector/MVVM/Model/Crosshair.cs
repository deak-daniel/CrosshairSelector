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
    public enum CrosshairShape
    {
        Cross,
        Cross2,
        Triangle
    }

    [DataContract]
    public class Crosshair : ICrosshair
    {
        #region Properties
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
            View = new CrossView();
            Shape = CrosshairShape.Cross;
            Thickness = 1;
            Opacity = 1;
            Gap = 1;
            Size = 1;
        }
        #endregion // Default Constructor

        #region ICrosshair interface implementation
        public void ModifyCrossView(int size, int thickness, int gap, int opacity, int red, int green, int blue, bool outline, bool centerDot, int outlineRed, int outlineGreen, int outlineBlue, int outlineOpacity, int outlineThickness, Key assignedKey)
        {
            Size = size == 0 ? 1 : size;
            Opacity = opacity == 0 ? 255 : opacity;
            Thickness = thickness == 0 ? 1 : thickness;
            Gap = gap == 0 ? 1 : gap;
            CrosshairColor = Color.FromArgb((byte)opacity, (byte)red, (byte)green, (byte)blue);
            OutlineColor = Color.FromArgb((byte)outlineOpacity, (byte)outlineRed, (byte)outlineGreen, (byte)outlineBlue);
            OutlineThickness = outlineThickness;
            Outline = outline;
            CenterDot = centerDot;
            AssignedKey = assignedKey;
            View.Modify(this);
        }
        public void ModifyCrossView(ICrosshair crosshair)
        {
            Size = crosshair.Size == 0 ? 1 : crosshair.Size;
            Opacity = crosshair.Opacity == 0 ? 255 : crosshair.Opacity;
            Thickness = crosshair.Thickness == 0 ? 1 : crosshair.Thickness;
            CenterDot = crosshair.CenterDot;
            OutlineColor = crosshair.OutlineColor;
            //Gap = gap == 0 ? 1 : gap;
            CrosshairColor = crosshair.CrosshairColor;
            OutlineThickness = crosshair.OutlineThickness;
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
                    View = new TriangularCrossView();
                    break;
                default:
                    Shape = CrosshairShape.Cross;
                    View = new CrossView();
                    break;
            }
        }

        #endregion // ICrosshair interface implementation

    }

}
