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
    public class TriangularCrossView : CrosshairViewBase
    {
        private const int scalar = 1;
        private const int gapOffset = -10;

        #region Properties
        public Rectangle Up { get; private set; }
        public Rectangle Left { get; private set; }
        public Rectangle Right { get; private set; }
        #endregion // Properties

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="thickness">optional</param>
        /// <param name="size">optional</param>
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

        #region CrosshairViewBase implementation
        public override void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas)
        {
            Up.RenderTransform = new RotateTransform(0);
            Canvas.SetLeft(Up, ActualWidth / 2);
            Canvas.SetTop(Up, ActualHeight / 2 - Size - Gap / 4);
            canvas.Children.Add(Up);

            Left.RenderTransform = new RotateTransform(140);
            Canvas.SetLeft(Left, ActualWidth / 2 - 0.5 - Gap / 4 + Thickness/2);
            Canvas.SetTop(Left, ActualHeight / 2 + Gap / 4 + Thickness);
            canvas.Children.Add(Left);

            Right.RenderTransform = new RotateTransform(310);
            Canvas.SetLeft(Right, ActualWidth / 2 + Gap / 4 + Thickness/2);
            Canvas.SetTop(Right, ActualHeight / 2 + Gap / 4 + Thickness);
            canvas.Children.Add(Right);
        }
        public override void RemoveCrosshairFromCanvas(ref Canvas canvas)
        {
            canvas.Children.Remove(Up);
            canvas.Children.Remove(Left);
            canvas.Children.Remove(Right);
        }
        public override void SetSize(int thickness, int size, int gap)
        {
            Width = thickness * scalar;
            Height = thickness * scalar;
            Thickness = thickness * scalar;
            Size = size * scalar;
            Gap = gap * scalar + gapOffset;
            Up.Width = Thickness;
            Up.Height = Size;

            Left.Width = Size;
            Left.Height = Thickness;

            Right.Width = Thickness;
            Right.Height = Size;
        }
        public override void SetStyle(bool outline, Color crosshairColor, Color outlineColor, int outlineThickness)
        {
            base.SetStyle(outline, crosshairColor, outlineColor, outlineThickness);
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
        }
        #endregion // CrosshairViewBase implementation
    }
}
