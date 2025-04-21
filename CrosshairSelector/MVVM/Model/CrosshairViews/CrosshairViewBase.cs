using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrosshairSelector.Model
{
    public abstract class CrosshairViewBase : ICrosshairView
    {
        #region Constants
        protected const int scalar = 1;
        protected const int gapOffset = -10;
        #endregion // Constants

        #region Properties
        public double Height { get; set; }
        public double Width { get; set; }
        public bool Outline { get; protected set; }
        public int Thickness { get; protected set; }
        public int Size { get; protected set; }
        public Color CrosshairColor { get; protected set; }
        public Color OutlineColor { get; protected set; }
        public int OutlineThickness { get; protected set; }
        public int Gap { get; protected set; }
        #endregion // Properties

        #region Abstract methods
        public abstract void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas);
        public abstract void RemoveCrosshairFromCanvas(ref Canvas canvas);
        public abstract void SetSize(int thickness, int size, int gap);
        #endregion // Abstract methods

        #region Virtual methods
        public virtual void SetStyle(bool outline, Color crosshairColor, Color outlineColor, int outlineThickness)
        {
            Outline = outline;
            OutlineColor = outlineColor;
            OutlineThickness = outlineThickness;
            CrosshairColor = crosshairColor;
        }
        public virtual void Modify(ICrosshair crosshair)
        {
            SetSize(crosshair.Thickness, crosshair.Size, crosshair.Gap);
            SetStyle(crosshair.Outline, crosshair.CrosshairColor, crosshair.OutlineColor, crosshair.OutlineThickness);
        }
        #endregion // Virtual methods
    }
}
