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
        private int gap;
        private const int Scalar = 1;

        #region Properties
        public double Height { get; set; }
        public double Width { get; set; }
        public System.Windows.Shapes.Rectangle Up { get; set; }
        public System.Windows.Shapes.Rectangle Down { get; set; }
        public System.Windows.Shapes.Rectangle Left { get; set; }
        public System.Windows.Shapes.Rectangle Right { get; set; }
        public bool Outline { get; set; }
        private System.Windows.Media.Color CrosshairColor;
        #endregion // Properties

        #region Constructor
        public CrossView(int thickness = 1, int size = 1)
        {
            Outline = false;
            gap = 0;
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
        public void Modify(int thickness, int size, int gap, bool outline, Color crosshairColor)
        {
            Up.Width = thickness * Scalar;
            Up.Height = size * Scalar;
            Down.Width = thickness * Scalar;
            Down.Height = size * Scalar;
            Left.Width = size * Scalar;
            Left.Height = thickness * Scalar;
            Right.Width = size * Scalar;
            Right.Height = thickness * Scalar;
            Outline = outline;
            CrosshairColor = crosshairColor;
            Width = thickness * Scalar;
            Height = thickness * Scalar;
            this.gap = gap * Scalar;
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
        public void Modify(ICrosshair crosshair)
        {
            Modify(crosshair.Thickness, crosshair.Size, crosshair.Gap, crosshair.Outline, crosshair.CrosshairColor);
        }
        public void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas)
        {
            Canvas.SetLeft(Up, ActualWidth / 2);
            Canvas.SetTop(Up, ActualHeight / 2 - Up.Height + gap / 4);
            canvas.Children.Add(Up);

            Canvas.SetLeft(Down, ActualWidth / 2);
            Canvas.SetTop(Down, ActualHeight / 2 + Left.Height - gap / 4);
            canvas.Children.Add(Down);

            Canvas.SetLeft(Left, ActualWidth / 2 - Left.Width + gap / 4);
            Canvas.SetTop(Left, ActualHeight / 2);
            canvas.Children.Add(Left);

            Canvas.SetLeft(Right, ActualWidth / 2 + Right.Height - gap / 4);
            Canvas.SetTop(Right, ActualHeight / 2);
            canvas.Children.Add(Right);
        }
        #endregion // Public methods
    }
}
