using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using CrosshairSelector.Model;

namespace CrosshairSelector.Core
{
    public static class CrosshairFactory
    {
        public static Crosshair CreateCrosshair(CrosshairShape shape, int thickness, int size, bool outline, int opacity, int gap, int outlineOpacity, int outlineThickness, Color crosshairColor, Color outlineColor)
        {
            Crosshair crosshair = new Crosshair();
            switch (shape)
            {
                case CrosshairShape.Cross:
                   crosshair.View = new CrossView();
                    break;
                case CrosshairShape.Cross2:
                    crosshair.View = new Cross2View();
                    break;
                case CrosshairShape.X:
                    crosshair.View = new XCrossView();
                    break;
                case CrosshairShape.Circle:
                    crosshair.View = new CircleView();
                    break;
                case CrosshairShape.Triangle:
                    crosshair.View = new TriangularCrossView();
                    break;
                default:
                    crosshair.View = new CrossView();
                    break;
            }
            crosshair.Thickness = thickness;
            crosshair.Shape = shape;
            crosshair.Size = size;
            crosshair.Outline = outline;
            crosshair.Opacity = opacity;
            crosshair.Gap = gap;
            crosshair.OutlineColor = outlineColor;
            crosshair.CrosshairColor = crosshairColor;
            crosshair.ModifyCrossView(crosshair);

            return crosshair;
        }
    }
}
