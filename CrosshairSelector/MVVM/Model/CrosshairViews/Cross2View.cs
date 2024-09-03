using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrosshairSelector
{
    public class Cross2View : ICrosshairView
    {
        private const int Scalar = 1;
        public int Height { get; set; }
        public int Width { get; set; }
        public Polyline Up { get; set; }
        public Polyline Down { get; set; }
        public Polyline Left { get; set; }
        public Polyline Right { get; set; }
        public bool Outline { get; set; }
        private System.Windows.Media.Color CrosshairColor;

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
            Outline = crosshair.Outline;
            CrosshairColor = crosshair.CrosshairColor;
        }
        public void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas)
        {
            if (Outline)
            {
                Up.Stroke = new SolidColorBrush(Colors.Black);
                Down.Stroke = new SolidColorBrush(Colors.Black);
                Left.Stroke = new SolidColorBrush(Colors.Black);
                Right.Stroke = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Up.Stroke = new SolidColorBrush(CrosshairColor);
                Down.Stroke = new SolidColorBrush(CrosshairColor);
                Left.Stroke = new SolidColorBrush(CrosshairColor);
                Right.Stroke = new SolidColorBrush(CrosshairColor);
            }

            Up.Fill = new SolidColorBrush(CrosshairColor);
            Canvas.SetLeft(Up, ActualWidth / 2);
            Canvas.SetTop(Up, ActualHeight / 2 - Up.Height);
            canvas.Children.Add(Up);
            Down.Fill = new SolidColorBrush(CrosshairColor);
            Canvas.SetLeft(Down, ActualWidth / 2);
            Canvas.SetTop(Down, ActualHeight / 2 + Left.Height);
            canvas.Children.Add(Down);

            Left.Fill = new SolidColorBrush(CrosshairColor);
            Canvas.SetLeft(Left, ActualWidth / 2 - Left.Width);
            Canvas.SetTop(Left, ActualHeight / 2);
            canvas.Children.Add(Left);
            Right.Fill = new SolidColorBrush(CrosshairColor);
            Canvas.SetLeft(Right, ActualWidth / 2 + Right.Height);
            Canvas.SetTop(Right, ActualHeight / 2);
            canvas.Children.Add(Right);

        }
    }
}
