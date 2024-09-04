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
        private const double Scalar = 1;

        Point p1 = new Point(1, 1);
        Point p2 = new Point(2, 1);
        Point p3 = new Point(1.5, 2);
        private System.Windows.Media.Color CrosshairColor;
        #endregion // Fields

        #region Properties
        public double Height { get; set; }
        public double Width { get; set; }
        public Polygon Up { get; set; }
        public Polygon Down { get; set; }
        public Polygon Left { get; set; }
        public Polygon Right { get; set; }
        public bool Outline { get; set; }
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
        public void Modify(ICrosshair crosshair)
        {
            Up.Points[0] = new Point(p1.X - crosshair.Thickness * Scalar,p1.Y - crosshair.Size * Scalar);
            Up.Points[1] = new Point(p2.X + crosshair.Thickness * Scalar, p1.Y - crosshair.Size * Scalar);
            Down.Points[0] = new Point(p1.X - crosshair.Thickness * Scalar, p1.Y - crosshair.Size * Scalar);
            Down.Points[1] = new Point(p2.X + crosshair.Thickness * Scalar, p1.Y - crosshair.Size * Scalar);
            Left.Points[0] = new Point(p1.X - crosshair.Thickness * Scalar, p1.Y - crosshair.Size * Scalar);
            Left.Points[1] = new Point(p2.X + crosshair.Thickness * Scalar, p1.Y - crosshair.Size * Scalar);
            Right.Points[0] = new Point(p1.X - crosshair.Thickness * Scalar, p1.Y - crosshair.Size * Scalar);
            Right.Points[1] = new Point(p2.X + crosshair.Thickness * Scalar, p1.Y - crosshair.Size * Scalar);
            Outline = crosshair.Outline;
            CrosshairColor = crosshair.CrosshairColor;

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
            Down.Fill = new SolidColorBrush(CrosshairColor);
            Left.Fill = new SolidColorBrush(CrosshairColor);
            Right.Fill = new SolidColorBrush(CrosshairColor);
        }
        public void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas)
        {
            Canvas.SetLeft(Up, ActualWidth / 2 );
            Canvas.SetTop(Up, ActualHeight / 2 );
            canvas.Children.Add(Up);

            Down.RenderTransform = new RotateTransform(180);
            Canvas.SetLeft(Down, ActualWidth / 2 + Width *1.5 );
            Canvas.SetTop(Down, ActualHeight / 2 + Height * 2);
            canvas.Children.Add(Down);

            Left.RenderTransform = new RotateTransform(270);
            Canvas.SetLeft(Left, ActualWidth / 2 - Width / 3.5);
            Canvas.SetTop(Left, ActualHeight / 2 + Height * 1.75);
            canvas.Children.Add(Left);

            Right.RenderTransform = new RotateTransform(90);
            Canvas.SetLeft(Right, ActualWidth / 2 + Width * 1.8);
            Canvas.SetTop(Right, ActualHeight / 2 + Height / 4);
            canvas.Children.Add(Right);

        }
        #endregion // ICrosshairView interface implementation

        #region private methods
        private double Distance(Point point1, Point point2)
        {
            double first = (int)Math.Pow(point2.X - point1.X, 2);
            double second = (int)Math.Pow(point2.Y - point1.Y, 2);
            return Math.Sqrt(first + second);
        }
        #endregion // private methods
    }
}
