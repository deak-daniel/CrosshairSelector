using System;
using System.Windows.Media;
using System.Windows.Input;


namespace CrosshairSelector.Model
{
    public class Crosshair : ICrosshair, ICloneable
    {
        #region Properties
        public string Name { get; set; }
        public Key AssignedKey { get; set; }
        public int Thickness { get; set; }
        public int Opacity { get; set; }
        public int Gap { get; set; }
        public int Size { get; set; }
        public int OutlineOpacity { get; set; }
        public bool CenterDot { get; set; }
        public int OutlineThickness { get; set; }
        public bool Outline { get; set; }
        public CrosshairShape Shape { get; set; }
        public Color CrosshairColor { get; set; }
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
            return new Crosshair() 
            {
                View = (ICrosshairView)this.View.Clone(),
                Outline = this.Outline,
                AssignedKey = this.AssignedKey,
                OutlineColor = this.OutlineColor,
                OutlineThickness = this.OutlineThickness,
                Opacity = this.Opacity,
                Shape = this.Shape,
                Size = this.Size,
                Gap = this.Gap,
                OutlineOpacity = this.OutlineOpacity,
                CenterDot = this.CenterDot,
                Name = this.Name,
                Thickness = this.Thickness,
                CrosshairColor = this.CrosshairColor
            };
        }
        #endregion // ICloneable implementation

        #region Equals implementation
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj.GetType() == typeof(string))
            {
                return this.Name == (string)obj;
            }
            if (obj.GetType() != typeof(Crosshair)) return false;
            return (obj as Crosshair)!.Name == this.Name;
        }
        #endregion
    }

}
