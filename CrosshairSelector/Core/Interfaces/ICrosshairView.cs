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
    public interface ICrosshairView
    {
        public double Width{ get; set; }
        public double Height{ get; set; }
        void Modify(int thickness, int size, int gap, bool outline, Color crosshairColor, Color outlineColor, int outlineThickness);
        void Modify(ICrosshair crosshair);
        void PutCrosshairOnCanvas(double ActualWidth,  double ActualHeight, ref Canvas canvas);
        void RemoveCrosshairFromCanvas(ref Canvas canvas);
    }
}
