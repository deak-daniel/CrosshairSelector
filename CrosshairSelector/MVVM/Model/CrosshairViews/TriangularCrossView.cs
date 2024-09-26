using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrosshairSelector
{
    public class TriangularCrossView : ICrosshairView
    {
        private const int Scalar = 1;

        #region Properties
        public double Height { get; set; }
        public double Width { get; set; }

        public System.Windows.Shapes.Rectangle Up { get; set; }
        public System.Windows.Shapes.Rectangle Left { get; set; }
        public System.Windows.Shapes.Rectangle Right { get; set; }
        public bool Outline { get; set; }
        public int Thickness { get; set; }
        public int Size { get; set; }
        public Color CrosshairColor { get; set; }
        public int Gap { get; set; }
        #endregion // Properties

        #region Constructor
        public TriangularCrossView(int thickness = 10, int size = 1)
        {
            Outline = false;
            Gap = 0;
            Up = new System.Windows.Shapes.Rectangle();
            Up.Width = thickness;
            Up.Height = size;
            Left = new System.Windows.Shapes.Rectangle();
            Left.Width = size;
            Left.Height = thickness;
            Right = new System.Windows.Shapes.Rectangle();
            Right.Width = size;
            Right.Height = thickness;
            Width = thickness;
            Height = size;
            CrosshairColor = new System.Windows.Media.Color();
        }
        #endregion // Constructor

        #region ICrosshairView interface implementation
        public void Modify(int thickness, int size, bool outline, Color crosshairColor, int gap = 0)
        {
            Up.Width = thickness * Scalar;
            Up.Height = size * Scalar;

            Left.Width = size * Scalar;
            Left.Height = thickness * Scalar;

            Right.Width = thickness * Scalar;
            Right.Height = size * Scalar;

            Outline = outline;
            CrosshairColor = crosshairColor;
            Width = thickness * Scalar;
            Height = thickness * Scalar;
            Gap = gap * Scalar;
            if (Outline)
            {
                Up.Stroke = new SolidColorBrush(Colors.Black);
                Left.Stroke = new SolidColorBrush(Colors.Black);
                Right.Stroke = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Up.Stroke = new SolidColorBrush(CrosshairColor);
                Left.Stroke = new SolidColorBrush(CrosshairColor);
                Right.Stroke = new SolidColorBrush(CrosshairColor);
            }

            Up.Fill = new SolidColorBrush(CrosshairColor);
            Left.Fill = new SolidColorBrush(CrosshairColor);
            Right.Fill = new SolidColorBrush(CrosshairColor);
        }

        public void Modify(ICrosshair crosshair)
        {
            Modify(crosshair.Thickness, crosshair.Size, crosshair.Outline, crosshair.CrosshairColor, crosshair.Gap);
        }

        public void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas)
        {
            Up.RenderTransform = new RotateTransform(0);
            Canvas.SetLeft(Up, ActualWidth / 2);
            Canvas.SetTop(Up, ActualHeight / 2 - Up.Height + Gap / 4);
            canvas.Children.Add(Up);

            Left.RenderTransform = new RotateTransform(140);
            Canvas.SetLeft(Left, ActualWidth / 2 + 3 + Gap / 4);
            Canvas.SetTop(Left, ActualHeight / 2 - Gap / 4);
            canvas.Children.Add(Left);

            Right.RenderTransform = new RotateTransform(310);
            Canvas.SetLeft(Right, ActualWidth / 2 - Gap / 4);
            Canvas.SetTop(Right, ActualHeight / 2 - Gap / 4);
            canvas.Children.Add(Right);
        }

        public void RemoveCrosshairFromCanvas(ref Canvas canvas)
        {
            canvas.Children.Remove(Up);
            canvas.Children.Remove(Left);
            canvas.Children.Remove(Right);
        }
        #endregion // ICrosshairView interface implementation
    }
}
