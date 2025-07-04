using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CrosshairSelector
{
    public interface ICrosshairView : ICloneable
    {
        public double Width{ get; set; }
        public double Height{ get; set; }
        public int Thickness { get; }
        public int Size{ get; }
        public int Gap{ get; }
        public Color CrosshairColor { get; }
        public Color OutlineColor { get;  }
        public int OutlineThickness { get; }
        public bool Outline { get; }
        void Modify(ICrosshair crosshair);
        void PutCrosshairOnCanvas(double ActualWidth,  double ActualHeight, ref Canvas canvas);
        void RemoveCrosshairFromCanvas(ref Canvas canvas);
        void SetSize(int thickness, int size, int gap);
        void SetStyle(bool outline, Color crosshairColor, Color outlineColor, int outlineThickness);
    }
}
