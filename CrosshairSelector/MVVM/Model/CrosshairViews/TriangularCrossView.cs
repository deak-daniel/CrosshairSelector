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

        public System.Windows.Shapes.Rectangle Up { get; private set; }
        public System.Windows.Shapes.Rectangle Left { get; private set; }
        public System.Windows.Shapes.Rectangle Right { get; private set; }
        public bool Outline { get; private set; }
        public int Thickness { get; private set; }
        public int Size { get; private set; }
        public Color CrosshairColor { get; private set; }
        public Color OutlineColor { get; private set; }
        public int OutlineThickness { get; private set; }
        public int Gap { get; set; }
        #endregion // Properties

        #region Constructor
        public TriangularCrossView(int thickness = 10, int size = 1)
        {
            Outline = false;
            Gap = 0;
            Up = new Rectangle();
            Up.Width = thickness;
            Up.Height = size;
            Left = new Rectangle();
            Left.Width = size;
            Left.Height = thickness;
            Right = new Rectangle();
            Right.Width = size;
            Right.Height = thickness;
            Width = thickness;
            Height = size;
            CrosshairColor = new Color();
            OutlineColor = new Color();
        }
        #endregion // Constructor

        #region ICrosshairView interface implementation
        public void Modify(int thickness, int size, int gap, bool outline, Color crosshairColor, Color outlineColor, int outlineThickness)
        {
            SetSize(thickness, size, gap);
            SetStyle(outline, crosshairColor, outlineColor, outlineThickness);
        }

        public void Modify(ICrosshair crosshair)
        {
            Modify(crosshair.Thickness, crosshair.Size, crosshair.Gap, crosshair.Outline, crosshair.CrosshairColor, crosshair.OutlineColor, crosshair.OutlineThickness);
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

        #region Private methods
        private bool SetSize(int thickness, int size, int gap)
        {
            bool res = false;
            Width = thickness * Scalar;
            Height = thickness * Scalar;
            Thickness = thickness * Scalar;
            Size = size * Scalar;
            Gap = gap * Scalar;
            try
            {
                Up.Width = Thickness;
                Up.Height = Size;

                Left.Width = Size;
                Left.Height = Thickness;

                Right.Width = Thickness;
                Right.Height = Size;
                res = true;
                return res;
            }
            catch (Exception)
            {
                return res;
            }
        }
        private bool SetStyle(bool outline, Color crosshairColor, Color outlineColor, int outlineThickness)
        {
            bool res = false;   
            Outline = outline;
            CrosshairColor = crosshairColor;
            OutlineColor = outlineColor;
            OutlineThickness = outlineThickness;
            try
            {

                if (Outline)
                {
                    Up.Stroke = new SolidColorBrush(OutlineColor);
                    Left.Stroke = new SolidColorBrush(OutlineColor);
                    Right.Stroke = new SolidColorBrush(OutlineColor);
                    Up.StrokeThickness = OutlineThickness;
                    Left.StrokeThickness = OutlineThickness;
                    Right.StrokeThickness = OutlineThickness;
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
                res = true; 
                return res;
            }
            catch (Exception)
            {
                return res;
            }
        }
        #endregion // Private methods
    }
}
