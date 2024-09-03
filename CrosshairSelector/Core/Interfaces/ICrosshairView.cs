using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CrosshairSelector
{
    public interface ICrosshairView
    {
        public int Width{ get; set; }
        public int Height{ get; set; }
        void Modify(ICrosshair crosshair);
        void PutCrosshairOnCanvas(double ActualWidth,  double ActualHeight, ref Canvas canvas);
    }
}
