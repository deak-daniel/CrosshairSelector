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

namespace CrosshairSelector.Model
{
    public sealed class Cross2View : CrosshairViewBase
    {
        #region Fields
        private Point helperPoint;
        private const double Scalar = 0.5;

        Point p1 = new Point(1, 1);
        Point p2 = new Point(2, 1);
        Point p3 = new Point(1.5, 2);
        #endregion // Fields

        #region Properties
        public Polygon Up { get; private set; }
        public Polygon Down { get; private set; }
        public Polygon Left { get; private set; }
        public Polygon Right { get; private set; }

        #endregion // Properties

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="thickness">optional</param>
        /// <param name="size">optional</param>
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

        #region CrosshairViewBase implementation
        public override void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas)
        {
            Canvas.SetLeft(Up, ActualWidth / 2 - 0.55);
            Canvas.SetTop(Up, ActualHeight / 2);
            canvas.Children.Add(Up);

            Down.RenderTransform = new RotateTransform(180);
            Canvas.SetLeft(Down, ActualWidth / 2 + Width * 1.3);
            Canvas.SetTop(Down, ActualHeight / 2 + Height * 2);
            canvas.Children.Add(Down);

            Left.RenderTransform = new RotateTransform(270);
            Canvas.SetLeft(Left, ActualWidth / 2 - Width / 2);
            Canvas.SetTop(Left, ActualHeight / 2 + Height * 1.5);
            canvas.Children.Add(Left);

            Right.RenderTransform = new RotateTransform(90);
            Canvas.SetLeft(Right, ActualWidth / 2 + Width * 1.8);
            Canvas.SetTop(Right, ActualHeight / 2 + Height / 8);
            canvas.Children.Add(Right);
        }
        public override void RemoveCrosshairFromCanvas(ref Canvas canvas)
        {
            canvas.Children.Remove(Up); 
            canvas.Children.Remove(Down);
            canvas.Children.Remove(Left);
            canvas.Children.Remove(Right);
        }
        public override void SetSize(int thickness, int size, int gap)
        {
            Thickness = (int)(thickness * Scalar);
            Size = (int)(size * Scalar);
            Gap = (int)(gap * Scalar);
            Up.Points[0] = new Point(p1.X - Thickness, p1.Y - Size);
            Up.Points[1] = new Point(p2.X + Thickness, p1.Y - Size);
            Up.Points[2] = new Point(p3.X, p3.X - Gap / 2);

            Down.Points[0] = new Point(p1.X - Thickness, p1.Y - Size);
            Down.Points[1] = new Point(p2.X + Thickness, p1.Y - Size);
            Down.Points[2] = new Point(p3.X, p3.Y - Gap / 2);

            Left.Points[0] = new Point(p1.X - Thickness, p1.Y - Size);
            Left.Points[1] = new Point(p2.X + Thickness, p1.Y - Size);
            Left.Points[2] = new Point(p3.X, p3.Y - Gap / 2);

            Right.Points[0] = new Point(p1.X - Thickness, p1.Y - Size);
            Right.Points[1] = new Point(p2.X + Thickness, p1.Y - Size);
            Right.Points[2] = new Point(p3.X, p3.Y - Gap / 2);
        }
        public override void SetStyle(bool outline, Color crosshairColor, Color outlineColor, int outlineThickness)
        {
            base.SetStyle(outline, crosshairColor, outlineColor, outlineThickness);
            if (Outline)
            {
                Up.Stroke = new SolidColorBrush(OutlineColor);
                Down.Stroke = new SolidColorBrush(OutlineColor);
                Left.Stroke = new SolidColorBrush(OutlineColor);
                Right.Stroke = new SolidColorBrush(OutlineColor);
                Up.StrokeThickness = OutlineThickness;
                Down.StrokeThickness = OutlineThickness;
                Left.StrokeThickness = OutlineThickness;
                Right.StrokeThickness = OutlineThickness;
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
        #endregion // CrosshairViewBase implementation

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
