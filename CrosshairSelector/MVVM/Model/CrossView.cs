using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public class CrossView : ICrossView
    {
        private const int Scalar = 1;
        public System.Windows.Shapes.Rectangle Up { get; set; }
        public System.Windows.Shapes.Rectangle Down { get; set; }
        public System.Windows.Shapes.Rectangle Left { get; set; }
        public System.Windows.Shapes.Rectangle Right { get; set; }
        public Tuple<int, int> MiddlePoint { get; set; }
        public CrossView(int thickness = 10, int size = 30)
        {
            MiddlePoint = PcInformations.GetMiddlePoint();
            int ScreenWidth = MiddlePoint.Item1;
            int ScreenHeight = MiddlePoint.Item2;

            Up = new System.Windows.Shapes.Rectangle();
            Up.Width = thickness;
            Up.Height = size;
            Down = new System.Windows.Shapes.Rectangle();
            Down.Width = thickness;
            Down.Height = size;
            Left = new System.Windows.Shapes.Rectangle();
            Left.Width = size;
            Left.Height = thickness;
            Right = new System.Windows.Shapes.Rectangle();
            Right.Width = size;
            Right.Height = thickness;
        }
        public void Modify(ICrosshair crosshair)
        {
            Up.Width = crosshair.Thickness * Scalar;
            Up.Height = crosshair.Size * Scalar;
            Down.Width = crosshair.Thickness * Scalar;
            Down.Height = crosshair.Size * Scalar;
            Left.Width = crosshair.Size * Scalar;
            Left.Height = crosshair.Thickness * Scalar;
            Right.Width = crosshair.Size * Scalar;
            Right.Height = crosshair.Thickness * Scalar;
        }
    }
}
