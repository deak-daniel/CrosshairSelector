using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrosshairSelector
{
    public class CircleView : ICrosshairView
    {
        #region Fields
        private const int Scalar = 1;
        private Color _crosshairColor;
        Ellipse Ellipse;
        #endregion // Fields

        #region Properties
        public double Width { get; set; }
        public double Height { get; set; }
        public int Thickness { get; private set; }
        #endregion // Properties

        #region Constructor
        public CircleView()
        {
            Ellipse = new Ellipse();
        }
        #endregion // Constructor

        #region ICrosshairView interface implementation
        public void Modify(ICrosshair crosshair)
        {
            Height = crosshair.Size * Scalar;
            Width = crosshair.Size * Scalar;
            Thickness = crosshair.Thickness * Scalar;
            Ellipse.Width = Width;
            Ellipse.Height = Height;
            if (crosshair.Outline)
            {
                Ellipse.Stroke = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Ellipse.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
            }
            Ellipse.StrokeThickness = Thickness;
            Ellipse.Fill = new SolidColorBrush(crosshair.CrosshairColor);
        }
        public void PutCrosshairOnCanvas(double ActualWidth,  double ActualHeight, ref Canvas canvas)
        {
            Canvas.SetLeft(Ellipse, ActualWidth / 2 - Ellipse.Width);
            Canvas.SetTop(Ellipse, ActualHeight / 2 - Ellipse.Height);
            canvas.Children.Add(Ellipse);
        }
        #endregion // ICrosshairView interface implementation
    }
}
