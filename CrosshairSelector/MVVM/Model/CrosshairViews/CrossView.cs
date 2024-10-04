using CrosshairSelector.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CrosshairSelector
{
    public class CrossView : ICrosshairView
    {
        private const int Scalar = 1;
        private const int GapOffset = -4;

        #region Properties
        public double Height { get; set; }
        public double Width { get; set; }
        public System.Windows.Shapes.Rectangle Up { get; private set; }
        public System.Windows.Shapes.Rectangle Down { get; private set; }
        public System.Windows.Shapes.Rectangle Left { get; private set; }
        public System.Windows.Shapes.Rectangle Right { get; private set; }
        public bool Outline { get; private set; }
        public int Thickness { get; private set; }
        public int Size { get; private set; }
        public Color CrosshairColor { get; private set; }
        public Color OutlineColor { get; private set; }
        public int OutlineThickness { get; private set; }
        public int Gap { get; private set; }
        #endregion // Properties

        #region Constructor
        public CrossView(int thickness = 1, int size = 1)
        {
            Outline = false;
            Gap = 0;
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
            Width = thickness;
            Height = size;
            CrosshairColor = new System.Windows.Media.Color();
        }
        #endregion // constructor

        #region Public methods
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
            Canvas.SetLeft(Up, ActualWidth / 2);
            Canvas.SetTop(Up, ActualHeight / 2 - Up.Height - Gap / 4);
            canvas.Children.Add(Up);

            Canvas.SetLeft(Down, ActualWidth / 2);
            Canvas.SetTop(Down, ActualHeight / 2 + Left.Height + Gap / 4);
            canvas.Children.Add(Down);

            Canvas.SetLeft(Left, ActualWidth / 2 - Left.Width - Gap / 4);
            Canvas.SetTop(Left, ActualHeight / 2);
            canvas.Children.Add(Left);

            Canvas.SetLeft(Right, ActualWidth / 2 + Right.Height + Gap / 4);
            Canvas.SetTop(Right, ActualHeight / 2);
            canvas.Children.Add(Right);
        }
        public void RemoveCrosshairFromCanvas(ref Canvas canvas)
        {
            canvas.Children.Remove(Up);
            canvas.Children.Remove(Down);
            canvas.Children.Remove(Left);
            canvas.Children.Remove(Right);
        }
        #endregion // Public methods

        #region Private methods
        private bool SetSize(int thickness, int size, int gap)
        {
            bool res = false;
            Thickness = thickness * Scalar;
            Size = size * Scalar;
            Gap = (gap * Scalar) + GapOffset;
            try
            {
                Up.Width = Thickness;
                Up.Height = Size;
                Down.Width = Thickness;
                Down.Height = Size;
                Left.Width = Size;
                Left.Height = Thickness;
                Right.Width = Size;
                Right.Height = Thickness;
                Width = Thickness;
                Height = Thickness;
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
            try
            {
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
