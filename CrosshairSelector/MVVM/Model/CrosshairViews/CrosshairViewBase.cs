using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrosshairSelector
{
    public abstract class CrosshairViewBase : ICrosshairView
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public bool Outline { get; protected set; }
        public int Thickness { get; protected set; }
        public int Size { get; protected set; }
        public Color CrosshairColor { get; protected set; }
        public Color OutlineColor { get; protected set; }
        public int OutlineThickness { get; protected set; }
        public int Gap { get; protected set; }

        public abstract void Modify(ICrosshair crosshair);

        public abstract void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas);

        public abstract void RemoveCrosshairFromCanvas(ref Canvas canvas);
        public abstract void SetSize(int thickness, int size, int gap);
        public virtual void SetStyle(bool outline, Color crosshairColor, Color outlineColor, int outlineThickness)
        {
            Outline = outline;
            OutlineColor = outlineColor;
            OutlineThickness = outlineThickness;
            CrosshairColor = crosshairColor;
        }

    }
}
