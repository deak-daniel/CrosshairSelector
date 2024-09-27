using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace CrosshairSelector
{
    public class Cross2View : ICrosshairView
    {
        #region Fields
        private Point helperPoint;
        private const double Scalar = 0.5;

        Point p1 = new Point(1, 1);
        Point p2 = new Point(2, 1);
        Point p3 = new Point(1.5, 2);
        #endregion // Fields

        #region Properties
        public double Height { get; set; }
        public double Width { get; set; }
        public Polygon Up { get; set; }
        public Polygon Down { get; set; }
        public Polygon Left { get; set; }
        public Polygon Right { get; set; }
        public bool Outline { get; set; }
        public int Thickness { get; set; }
        public int Size { get; set; }
        public Color CrosshairColor { get; set; }
        public Color OutlineColor { get; set; }
        public int OutlineThickness { get; set; }

        #endregion // Properties

        #region Constructor
        public Cross2View(int thickness = 10, int size = 10)
        {
            helperPoint = new Point(1, 2);
            Up = new Polygon() { Points = new PointCollection(new List<Point> { p1, p2, p3 }) };
            Down = new Polygon() { Points = new PointCollection(new List<Point> { p1, p2, p3 }) };
            Left = new Polygon() { Points = new PointCollection(new List<Point> { p1, p2, p3 }) };
            Right = new Polygon() { Points = new PointCollection(new List<Point> { p1, p2, p3 }) };
            Width = Distance(Up.Points[0], helperPoint) * 2;
            Height = Width;
        }
        #endregion // Constructor

        #region ICrosshairView interface implementation
        public void Modify(int thickness, int size, bool outline, Color crosshairColor, Color outlineColor, int outlineThickness, int gap = 0)
        {
            Thickness = thickness;
            Size = size;
            CrosshairColor = crosshairColor;
            Outline = outline;
            Up.Points[0] = new Point(p1.X - thickness * Scalar, p1.Y - size * Scalar);
            Up.Points[1] = new Point(p2.X + thickness * Scalar, p1.Y - size * Scalar);
            Up.Points[2] = new Point(p3.X, p3.X + gap * Scalar / 2);
            Width = Distance(Up.Points[0], helperPoint);
            Height = Distance(Up.Points[2], helperPoint);

            Down.Points[0] = new Point(p1.X - thickness * Scalar, p1.Y - size * Scalar);
            Down.Points[1] = new Point(p2.X + thickness * Scalar, p1.Y - size * Scalar);
            Down.Points[2] = new Point(p3.X, p3.Y + gap * Scalar / 2);

            Left.Points[0] = new Point(p1.X - thickness * Scalar, p1.Y - size * Scalar);
            Left.Points[1] = new Point(p2.X + thickness * Scalar, p1.Y - size * Scalar);
            Left.Points[2] = new Point(p3.X, p3.Y + gap * Scalar / 2);

            Right.Points[0] = new Point(p1.X - thickness * Scalar, p1.Y - size * Scalar);
            Right.Points[1] = new Point(p2.X + thickness * Scalar, p1.Y - size * Scalar);
            Right.Points[2] = new Point(p3.X, p3.Y + gap * Scalar / 2);
            Outline = outline;
            CrosshairColor = crosshairColor;
            OutlineColor = outlineColor;
            OutlineThickness = outlineThickness;

            if (Outline)
            {
                Up.Stroke = new SolidColorBrush(OutlineColor);
                Down.Stroke = new SolidColorBrush(OutlineColor);
                Left.Stroke = new SolidColorBrush(OutlineColor);
                Right.Stroke = new SolidColorBrush(OutlineColor);
                Up.StrokeThickness = outlineThickness;
                Down.StrokeThickness = outlineThickness;
                Left.StrokeThickness = outlineThickness;
                Right.StrokeThickness = outlineThickness;
            }
            else
            {
                Up.Stroke = new SolidColorBrush(CrosshairColor);
                Down.Stroke = new SolidColorBrush(CrosshairColor);
                Left.Stroke = new SolidColorBrush(CrosshairColor);
                Right.Stroke = new SolidColorBrush(CrosshairColor);
            }

            Up.Fill = new SolidColorBrush(CrosshairColor);
            Down.Fill = new SolidColorBrush(CrosshairColor);
            Left.Fill = new SolidColorBrush(CrosshairColor);
            Right.Fill = new SolidColorBrush(CrosshairColor);
        }
        public void Modify(ICrosshair crosshair)
        {
            Modify(crosshair.Thickness, crosshair.Size, crosshair.Outline, crosshair.CrosshairColor, crosshair.OutlineColor, crosshair.OutlineThickness, crosshair.Gap);
        }
        public void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas)
        {
            Canvas.SetLeft(Up, ActualWidth / 2 );
            Canvas.SetTop(Up, ActualHeight / 2 );
            canvas.Children.Add(Up);

            Down.RenderTransform = new RotateTransform(180);
            Canvas.SetLeft(Down, ActualWidth / 2 + Width * 0.2);
            Canvas.SetTop(Down, ActualHeight / 2);
            canvas.Children.Add(Down);

            Left.RenderTransform = new RotateTransform(270);
            Canvas.SetLeft(Left, ActualWidth / 2);
            Canvas.SetTop(Left, ActualHeight / 2);
            canvas.Children.Add(Left);

            Right.RenderTransform = new RotateTransform(90);
            Canvas.SetLeft(Right, ActualWidth / 2);
            Canvas.SetTop(Right, ActualHeight / 2 - Height * 0.4);
            canvas.Children.Add(Right);
        }
        public void RemoveCrosshairFromCanvas(ref Canvas canvas)
        {
            canvas.Children.Remove(Up); 
            canvas.Children.Remove(Down);
            canvas.Children.Remove(Left);
            canvas.Children.Remove(Right);
        }
        #endregion // ICrosshairView interface implementation

        #region Private methods
        private double Distance(Point point1, Point point2)
        {
            double first = (int)Math.Pow(point2.X - point1.X, 2);
            double second = (int)Math.Pow(point2.Y - point1.Y, 2);
            return Math.Sqrt(first + second);
        }
        #endregion // Private methods
    }
}
